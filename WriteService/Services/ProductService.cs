using AutoMapper;
using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using Microsoft.EntityFrameworkCore;
using WriteService.DTOs.Product;
using WriteService.DTOs.Review;
using WriteService.Entities;
using WriteService.Exceptions;

namespace WriteService.Services;

public class ProductService
{
    private readonly ShopDbContext _context;

    private readonly ILogger<ProductService> _logger;
    private readonly IMapper _mapper;
    private readonly RabbitMQProducer _producer;

    public ProductService(ShopDbContext context, ILogger<ProductService> logger, IMapper mapper, RabbitMQProducer producer)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
        _producer = producer;
    }

    public async Task<ProductEntity> CreateAsync(CreateProductDto dto)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var vendor = await _context
                .Vendors
                .FindAsync(dto.VendorId);

            if (vendor is null)
            {
                throw new EntityNotFoundException(dto.VendorId);
            }

            var product = new ProductEntity()
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                PiecesInStock = dto.PricesInStock,
                VendorId = dto.VendorId
            };

            _context.Add(product);

            await _context.SaveChangesAsync();

            var message = new CreateProductMessage()
            {
                Title = product.Title,
                Description = product.Description,
                PiecesInStock = product.PiecesInStock,
                Price = product.Price,
                Rating = 5,
                VendorId = product.Vendor.Id,
                VendorName = product.Vendor.Name,
                Categories = product.Categories.Select(x => x.Name),
                SubCategories = product.SubCategories.Select(x => x.Name)
            };

            _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.Order, message);

            await transaction.CommitAsync();

            return product;
        }
    }

    public async Task<ReviewEntity> AddReviewAsync(long productId, CreateReviewDto dto)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var product = await FindProductAsync(productId);

            var review = new ReviewEntity()
            {
                Rating = dto.Rating,
                Text = dto.Text,
                Product = product
            };

            _context.Add(review);

            await _context.SaveChangesAsync();

            // TODO: send message

            await transaction.CommitAsync();

            return review;
        }
    }

    public async Task DeleteAsync(long productId)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var product = await FindProductAsync(productId);

            product.IsDeleted = true;

            _context.Update(product);

            await _context.SaveChangesAsync();

            // TODO: send message

            await transaction.CommitAsync();
        }
    }

    private async Task<ProductEntity> FindProductAsync(long id)
    {
        var product = await _context
            .Products
            .FirstOrDefaultAsync(v => v.Id == id);

        if (product is null)
        {
            throw new EntityNotFoundException(id);
        }

        return product;
    }
}
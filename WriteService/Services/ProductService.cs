using AutoMapper;
using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using Common.RabbitMQ.Messages.Products;
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

            var categories = LoadEntities<ProductCategoryEntity>(dto.Categories);
            var subCategories = LoadEntities<ProductSubCategoryEntity>(dto.SubCategories);

            product.Categories.AddRange(categories);
            product.SubCategories.AddRange(subCategories);

            _context.Add(product);

            await _context.SaveChangesAsync();

            var message = new CreateProductMessage()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                PiecesInStock = product.PiecesInStock,
                Price = product.Price,
                VendorId = product.Vendor.Id,
                VendorName = product.Vendor.Name,
                Categories = product
                    .Categories
                    .Select(x => x.Name)
                    .ToList(),
                SubCategories = product
                    .SubCategories
                    .Select(x => x.Name)
                    .ToList()
            };

            _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.Product, message);

            await transaction.CommitAsync();

            return product;
        }
    }

    private IEnumerable<T> LoadEntities<T>(IEnumerable<long> ids) where T : EntityBase
    {
        var result = new List<T>();
        foreach (var id in ids)
        {
            var entity = _context
                .Set<T>()
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new EntityNotFoundException(id);
            }

            result.Add(entity);
        }

        return result;
    }

    public async Task<ReviewEntity> AddReviewAsync(long productId, long customerId, CreateReviewDto dto)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var product = await FindProductAsync(productId);

            var review = new ReviewEntity()
            {
                Rating = dto.Rating,
                Text = dto.Text,
                ProductId = productId,
                CustomerId = customerId
            };

            _context.Add(review);

            await _context.SaveChangesAsync();

            var message = new CreateReviewMessage()
            {
                Id = review.Id,
                Rating = review.Rating,
                Text = review.Text,
                ProductId = product.Id
            };

            _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.Review, message);

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

            // TODO: remove reviews of the product?

            await _context.SaveChangesAsync();

            var message = new DeleteProductMessage() { ProductId = product.Id };

            _producer.SendMessageAsync(RabbitMQOperation.Delete, RabbitMQEntities.Product, message);

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
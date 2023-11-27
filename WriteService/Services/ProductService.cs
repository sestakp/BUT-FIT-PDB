using System.Transactions;
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
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
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
                var saveChangesTask = _context.SaveChangesAsync();
                var productMessageDto = _mapper.Map<ProductMessage>(product);
                var sendMessageTask = _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.Product, productMessageDto);

                await Task.WhenAll(saveChangesTask, sendMessageTask);

                scope.Complete();

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating product: {ex.Message}", ex);
                throw;
            }
        }
    }

    public async Task<ReviewEntity> AddReviewAsync(long productId, CreateReviewDto dto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var product = await FindProductAsync(productId);

                var review = new ReviewEntity()
                {
                    Rating = dto.Rating,
                    Text = dto.Text,
                    Product = product
                };

                _context.Add(review);


                var saveChangesTask = _context.SaveChangesAsync();
                var reviewMessageDto = _mapper.Map<ReviewMessage>(review);
                var sendMessageTask = _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.Review, reviewMessageDto);

                await Task.WhenAll(saveChangesTask, sendMessageTask);

                scope.Complete();
                return review;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating review: {ex.Message}", ex);
                throw;
            }
        }
    }

    public async Task DeleteAsync(long productId)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var product = await FindProductAsync(productId);

                product.IsDeleted = true;

                _context.Update(product);


                var saveChangesTask = _context.SaveChangesAsync();
                var productMessageDto = _mapper.Map<ProductMessage>(product);
                var sendMessageTask = _producer.SendMessageAsync(RabbitMQOperation.Delete, RabbitMQEntities.Product, productMessageDto);

                await Task.WhenAll(saveChangesTask, sendMessageTask);

                scope.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product: {ex.Message}", ex);
                throw;
            }
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
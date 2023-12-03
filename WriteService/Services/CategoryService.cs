using Common.RabbitMQ;
using Common.RabbitMQ.Messages.Category;
using Common.RabbitMQ.Messages.Customer;
using Common.RabbitMQ.Messages.Products;
using Microsoft.EntityFrameworkCore;
using SharpCompress.Common;
using WriteService.DTOs.Category;
using WriteService.DTOs.Customer;
using WriteService.Entities;
using WriteService.Exceptions;

namespace WriteService.Services;

public class CategoryService
{

    private readonly ShopDbContext _context;
    private readonly RabbitMQProducer _producer;
    private readonly ILogger<CustomerService> _logger;

    public CategoryService(ShopDbContext context, RabbitMQProducer producer, ILogger<CustomerService> logger)
    {
        _context = context;
        _producer = producer;
        _logger = logger;
    }

    public async Task<ProductCategoryEntity> CreateAsync(CreateCategoryDto dto)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var entity = new ProductCategoryEntity()
            {
                Description = dto.Description,
                Name = dto.Name,
                NormalizedName = dto.Name
            };

            _context.Add(entity);

            await _context.SaveChangesAsync();

            var message = new CreateCategoryMessage()
            {
                Id = entity.Id,
                Description = dto.Description,
                Name = dto.Name
                
            };

            _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.ProductCategory, message);

            await transaction.CommitAsync();

            return entity;
        }
    }

    public async Task<ProductCategoryEntity> UpdateAsync(long categoryId, UpdateCategoryDto dto)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var entity = await _context.ProductCategories.FindAsync(categoryId);
            if (entity is null)
            {
                throw new EntityNotFoundException(categoryId);
            }

            // We do not allow updating email and phone number
            entity.Description = dto.Description;
            entity.Name = dto.Name;
            

            _context.Update(entity);

            await _context.SaveChangesAsync();

            var message = new UpdateCategoryMessage
            {
                Id = entity.Id,
                Description = dto.Description,
                Name = dto.Name
            };

            _producer.SendMessageAsync(RabbitMQOperation.Update, RabbitMQEntities.ProductCategory, message);

            await transaction.CommitAsync();

            return entity;
        }
    }

    public async Task DeleteAsync(long categoryId)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var category = await _context.ProductCategories.FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category is null)
            {
                throw new EntityNotFoundException(categoryId);
            }

            var products = _context.Products.Where(p => p.Categories.Contains(category));

            foreach(var product in products)
            {
                product.Categories.Remove(category);
                _context.Update(product);
            }

            _context.Remove(category);
            

            await _context.SaveChangesAsync();

            var message = new DeleteCategoryMessage() { Id = category.Id };

            _producer.SendMessageAsync(RabbitMQOperation.Delete, RabbitMQEntities.ProductCategory, message);

            await transaction.CommitAsync();
        }
    }
}

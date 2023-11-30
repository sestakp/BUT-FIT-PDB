using Common.RabbitMQ;
using Common.RabbitMQ.Messages.Category;
using Common.RabbitMQ.Messages.Customer;
using WriteService.DTOs.Category;
using WriteService.DTOs.Customer;
using WriteService.Entities;

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
                Name = dto.Name
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
}

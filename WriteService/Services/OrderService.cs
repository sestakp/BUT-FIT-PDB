using AutoMapper;
using Common.Enums;
using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using Microsoft.EntityFrameworkCore;
using WriteService.DTOs.Order;
using WriteService.Entities;
using WriteService.Exceptions;

namespace WriteService.Services;

public class OrderService
{
    private readonly ShopDbContext _context;
    private readonly ILogger<OrderService> _logger;
    private readonly IMapper _mapper;
    private readonly RabbitMQProducer _producer;

    public OrderService(ShopDbContext context, RabbitMQProducer producer, IMapper mapper, ILogger<OrderService> logger)
    {
        _context = context;
        _producer = producer;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<OrderEntity> CreateAsync(long customerId)
    {
        var orderInProgress = await _context.Orders.Where(o => o.CustomerId == customerId && o.Status == OrderStatusEnum.InProgress)
            .FirstOrDefaultAsync();

        if (orderInProgress != null)
        {
            return orderInProgress;
        }


        var order = new OrderEntity()
        {
            Created = DateTime.UtcNow,
            Status = OrderStatusEnum.InProgress,
            CustomerId = customerId
        };

        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        return order;
    }


    public async Task<OrderEntity> AddToCartAsync(long orderId, long productId)
    {
        var order = await FindOrderAsync(orderId);

        if (order.Status != OrderStatusEnum.InProgress)
        {
            throw new Exception("Unable to update order which is not in status 'InProgress'.");
        }

        var product = await _context
            .Products
            .FindAsync(productId);

        if (product == null)
        {
            throw new EntityNotFoundException(orderId);
        }

        if (product.IsDeleted)
        {
            throw new Exception("Cannot add deleted product to cart.");
        }

        if (product.PiecesInStock < 1)
        {
            throw new Exception("Specified product is out of stock.");
        }
        
        order.Products.Add(product);
        order.LastUpdated = DateTime.UtcNow;
        
        _context.Update(order);

        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<OrderEntity> CompleteOrderAsync(long orderId, CompleteOrderDto dto)
    {
        await using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var order = await FindOrderAsync(orderId);


            if (order.Status != OrderStatusEnum.InProgress)
            {
                throw new Exception("Unable to update order which is not in status 'InProgress'.");
            }

            if (order.Products.Count < 1)
            {
                throw new Exception("Unable to complete order for empty order.");
            }

            order.Country = dto.Country;
            order.ZipCode = dto.ZipCode;
            order.City = dto.City;
            order.Street = dto.Street;
            order.HouseNumber = dto.HouseNumber;
            order.Status = OrderStatusEnum.Completed;
            order.LastUpdated = DateTime.UtcNow;

            _context.Update(order);

            decimal price = 0;
            foreach (var product in order.Products)
            {
                if (product.PiecesInStock < 1)
                {
                    throw new Exception($"Product {product.Id} out of stock");
                }

                if (product.IsDeleted)
                {
                    throw new Exception($"Product {product.Id} is deleted.");
                }

                product.PiecesInStock -= 1;
                price += product.Price;

                _context.Update(product);
            }

            await _context.SaveChangesAsync();

            var products = _mapper.Map<IEnumerable<OrderProductRabbit>>(order.Products);

            var message = new OrderCompletedMessage()
            {
                Id = order.Id,
                CustomerId = order.Customer.Id,
                Price = price,
                AddressCountry = order.Country,
                AddressZipCode = order.ZipCode,
                AddressCity = order.City,
                AddressStreet = order.Street,
                AddressHouseNumber = order.HouseNumber,
                DateTimeCreated = order.Created,
                Products = products
            };

            _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.Order, message);

            await transaction.CommitAsync();
            return order;
        }
    }

    private async Task<OrderEntity> FindOrderAsync(long orderId)
    {
        var order = await _context
            .Orders
            .Include(x => x.Products)
            .ThenInclude(x => x.Vendor)
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(v => v.Id == orderId);

        if (order is null)
        {
            throw new EntityNotFoundException(orderId);
        }

        return order;
    }
}
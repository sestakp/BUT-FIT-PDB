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
    private readonly RabbitMQProducer _producer;

    public OrderService(ShopDbContext context, RabbitMQProducer producer, ILogger<OrderService> logger)
    {
        _context = context;
        _producer = producer;
        _logger = logger;
    }

    public async Task<OrderEntity> CreateAsync(long customerId)
    {
        var orderInProgress = await _context
            .Orders
            .Where(o => o.CustomerId == customerId && o.Status == OrderStatusEnum.InProgress)
            .FirstOrDefaultAsync();

        if (orderInProgress is not null)
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

        var existingOrderProduct = order
            .OrderProducts
            .FirstOrDefault(x => x.ProductId == productId);

        if (existingOrderProduct is null)
        {
            order.OrderProducts.Add(new OrderProductEntity()
            {
                OrderId = orderId,
                ProductId = productId,
                Count = 1
            });
        }
        else
        {
            existingOrderProduct.Count++;
        }

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

            if (order.OrderProducts.Count < 1)
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

            foreach (var orderProduct in order.OrderProducts)
            {
                var product = orderProduct.Product;

                if (product.IsDeleted)
                {
                    throw new Exception($"Product {product.Id} is deleted.");
                }

                var newPiecesInStock = product.PiecesInStock - orderProduct.Count;
                if (newPiecesInStock < 0)
                {
                    throw new Exception($"There is only {product.PiecesInStock} pieces in stock of the product '{product.Title}'.");
                }

                product.PiecesInStock = newPiecesInStock;
                price += product.Price * orderProduct.Count;

                _context.Update(product);
            }

            await _context.SaveChangesAsync();

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
                Products = order
                    .OrderProducts
                    .Select(x => new OrderCompletedMessage.Product(
                        x.Product.Id,
                        x.Product.Title,
                        x.Product.Description,
                        x.Product.Price,
                        x.Count,
                        x.Product.Vendor.Name))
                    .ToList()
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
            .Include(x => x.OrderProducts)
            .ThenInclude(x => x.Product)
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
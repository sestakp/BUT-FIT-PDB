using System.Transactions;
using AutoMapper;
using Common.Enums;
using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using Microsoft.EntityFrameworkCore;
using SharpCompress.Common;
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

    public async Task<OrderEntity> CreateAsync()
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var order = new OrderEntity()
                {
                    Created = DateTime.UtcNow,
                    Status = OrderStatusEnum.InProgress
                };

                _context.Orders.Add(order);

                var saveChangesTask = _context.SaveChangesAsync();
                // var orderMessageDto = _mapper.Map<OrderMessage>(order);
                // var sendMessageTask = _producer.SendMessageAsync(RabbitMQOperation.Create, RabbitMQEntities.Order, orderMessageDto);
                //
                // await Task.WhenAll(saveChangesTask, sendMessageTask);
                //
                // scope.Complete();
                return order;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                _logger.LogError($"Error creating order: {ex.Message}", ex);
                throw;
            }
        }
    }


    public async Task<OrderEntity> AddToCartAsync(long orderId, long productId)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
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

                product.PiecesInStock--;
                order.Products.Add(product);
                order.LastUpdated = DateTime.UtcNow;

                _context.Update(product);
                _context.Update(order);

                var saveChangesTask = _context.SaveChangesAsync();

                var productMessageDto = _mapper.Map<ProductMessage>(product);
                // var orderMessageDto = _mapper.Map<OrderMessage>(order);
                //
                // var sendMessageTask = _producer.SendMessageAsync(RabbitMQOperation.Update, RabbitMQEntities.Order, orderMessageDto);
                // var sendMessageTask2 = _producer.SendMessageAsync(RabbitMQOperation.Update, RabbitMQEntities.Product, productMessageDto);
                //
                // await Task.WhenAll(saveChangesTask, sendMessageTask, sendMessageTask2);
                //
                // scope.Complete();
                return order;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                _logger.LogError($"Error add to cart: {ex.Message}", ex);
                throw;
            }
        }
    }

    public async Task<OrderEntity> CompleteOrderAsync(long orderId, CompleteOrderDto dto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var order = await FindOrderAsync(orderId);

                if (order.Status != OrderStatusEnum.InProgress)
                {
                    throw new Exception("Unable to update order which is not in status 'InProgress'.");
                }

                order.Country = dto.Country;
                order.ZipCode = dto.ZipCode;
                order.City = dto.City;
                order.Street = dto.Street;
                order.HouseNumber = dto.HouseNumber;
                order.Status = OrderStatusEnum.Completed;
                order.LastUpdated = DateTime.UtcNow;

                _context.Update(order);
                var saveChangesTask = _context.SaveChangesAsync();
                // var orderMessageDto = _mapper.Map<OrderMessage>(order);
                // var sendMessageTask =
                //     _producer.SendMessageAsync(RabbitMQOperation.Update, RabbitMQEntities.Order, orderMessageDto);
                //
                // await Task.WhenAll(saveChangesTask, sendMessageTask);
                // scope.Complete();
                return order;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                _logger.LogError($"Error complete order: {ex.Message}", ex);
                throw;
            }
        }
    }

    private async Task<OrderEntity> FindOrderAsync(long orderId)
    {
        var order = await _context
            .Orders
            .Include(v => v.Products) // Include the related products
            .FirstOrDefaultAsync(v => v.Id == orderId);

        if (order is null)
        {
            throw new EntityNotFoundException(orderId);
        }

        return order;
    }
}
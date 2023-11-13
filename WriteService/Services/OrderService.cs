using Common.Enums;
using WriteService.Entities;
using Microsoft.EntityFrameworkCore;
using WriteService.DTOs.Order;

namespace WriteService.Services;

public class OrderService
{
    private readonly ShopDbContext _context;

    public OrderService(ShopDbContext context)
    {
        _context = context;
    }

    public OrderEntity Create()
    {
        var order = new OrderEntity()
        {
            Created = DateTime.UtcNow,
            Status = OrderStatusEnum.InProgress
        };

        _context.Orders.Add(order);
        _context.SaveChanges();

        return order;
    }


    public OrderEntity AddToCart(long orderId, long productId)
    {
        var order = FindOrder(orderId);

        if (order.Status != OrderStatusEnum.InProgress)
        {
            throw new Exception("Unable to update order which is not in status 'InProgress'.");
        }

        var product = _context
            .Products
            .Find(productId);

        if (product == null)
        {
            throw new Exception("Product with specified id does not exist.");
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
        _context.SaveChanges();

        return order;
    }

    public OrderEntity CompleteOrder(long orderId, CompleteOrderDto dto)
    {
        var order = FindOrder(orderId);

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
        _context.SaveChanges();

        return order;
    }

    private OrderEntity FindOrder(long orderId)
    {
        var order = _context
            .Orders
            .Include(v => v.Products) // Include the related products
            .FirstOrDefault(v => v.Id == orderId);

        if (order is null)
        {
            throw new Exception($"Order with id '{orderId}' does not exist.");
        }

        return order;
    }
}
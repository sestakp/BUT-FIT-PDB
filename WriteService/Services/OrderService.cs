using AutoMapper;
using System.Transactions;
using Common.Enums;
using WriteService.DTO;
using WriteService.Entities;
using Microsoft.EntityFrameworkCore;

namespace WriteService.Services
{
    public class OrderService
    {
        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;
        public OrderService(ShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public OrderEntity Create(OrderDto orderDto)
        {

            using var scope = new TransactionScope();

            var customer = _context.Customers
                .Include(v => v.Orders) // Include the related products
                .FirstOrDefault(v => v.Id == orderDto.CustomerId);

            if (customer == null)
            {
                scope.Dispose();
                return new OrderEntity();
            }

            var orderInProgress = customer.Orders.FirstOrDefault(o => o.Status == OrderStatusEnum.InProgress);

            if (orderInProgress != default)
            {
                scope.Dispose();
                return orderInProgress;
            }
            

            var orderEntity = _mapper.Map<OrderEntity>(orderDto);

            orderEntity.Created = DateTime.Now;
            orderEntity.LastUpdated = orderEntity.Created;
            orderEntity.Status = OrderStatusEnum.InProgress;
            

            _context.Orders.Add(orderEntity);

            _context.SaveChanges();

            scope.Complete();
            return orderEntity;
        }


        public OrderEntity AddToCart(long orderId, long productId)
        {
            using var scope = new TransactionScope();

            var order = _context.Orders
                .Include(v => v.Products) // Include the related products
                .FirstOrDefault(v => v.Id == orderId);

            if (order == null || order.Status != OrderStatusEnum.InProgress)
            {
                scope.Dispose();
                return new OrderEntity();
            }

            var product = _context.Products.Find(productId);

            if (product == null || product.PiecesInStock < 1)
            {
                scope.Dispose();
                return new OrderEntity();
            }

            product.PiecesInStock -= 1;

            product.Orders.Add(order);
            order.Products.Add(product);

            _context.SaveChanges();

            scope.Complete();
            return order;
        }


        public OrderEntity Pay(long orderId)
        {

            using var scope = new TransactionScope();
            var order = _context.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == orderId);

            if (order == default || order.Status != OrderStatusEnum.InProgress)
            {
                scope.Dispose();
                return new OrderEntity();
            }

            order.Status = OrderStatusEnum.Paid;

            _context.SaveChanges();

            scope.Complete();

            return order;
        }
    }
}

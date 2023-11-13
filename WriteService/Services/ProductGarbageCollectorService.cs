using Common.Enums;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace WriteService.Services
{
    public class ProductGarbageCollectorService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public ProductGarbageCollectorService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
                    using var db_scope = new TransactionScope();
                    // Now you can use dbContext for database operations
                    DateTime thresholdTime = DateTime.Now.AddMinutes(-10);
                    var orders = dbContext
                        .Orders
                        .Where(o => o.Status == OrderStatusEnum.InProgress)
                        .Where(o => o.LastUpdated < thresholdTime)
                        .Include(o => o.Products);

                    foreach (var order in orders)
                    {
                        foreach (var product in order.Products)
                        {
                            product.Orders.Remove(order);
                            order.Products.Remove(product);
                            product.PiecesInStock += 1;
                        }


                        order.LastUpdated = DateTime.Now;

                    }

                    dbContext.SaveChanges();

                    db_scope.Complete();
                }

                // Perform your garbage collection logic here
                // For example, remove allocated products from incomplete orders


                // Sleep for 10 minutes
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}

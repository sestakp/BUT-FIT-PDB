using Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace WriteService;

public class ProductGarbageCollector : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly PeriodicTimer _timer;

    public ProductGarbageCollector(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _timer = new PeriodicTimer(TimeSpan.FromMinutes(10));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (await _timer.WaitForNextTickAsync(cancellationToken) && !cancellationToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var sp = scope.ServiceProvider;
                var dbContext = sp.GetRequiredService<ShopDbContext>();
                await DoWorkAsync(dbContext);
            }
        }
    }

    private Task DoWorkAsync(ShopDbContext dbContext)
    {
        // DateTime thresholdTime = DateTime.UtcNow.AddMinutes(-10);
        //
        // var orders = dbContext
        //     .Orders
        //     .Where(o => o.Status == OrderStatusEnum.InProgress)
        //     .Where(o => o.LastUpdated < thresholdTime)
        //     .Include(o => o.Products);
        //
        // foreach (var order in orders)
        // {
        //     foreach (var product in order.Products)
        //     {
        //         order.Products.Remove(product);
        //         product.PiecesInStock += 1;
        //     
        //         dbContext.Update(product);
        //     }
        //     
        //     dbContext.Update(order);
        // }
        //
        // await dbContext.SaveChangesAsync();

        throw new NotImplementedException();
    }
}
using Common.RabbitMQ;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace ReadService.Subscribers;

public class ProductSubscriber : RabbitMQReceiver<ProductSubscriber>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ProductSubscriber(IModel channel, ILogger<ProductSubscriber> logger, IServiceScopeFactory serviceScopeFactory)
        : base(channel, logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override void HandleCreate(RabbitMQMessage message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            // TODO
        }
    }
}
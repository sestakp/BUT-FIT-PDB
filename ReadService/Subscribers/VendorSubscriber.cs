using Common.RabbitMQ;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace ReadService.Subscribers;

public class VendorSubscriber : RabbitMQReceiver<VendorSubscriber>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public VendorSubscriber(IModel channel, ILogger<VendorSubscriber> logger, IServiceScopeFactory serviceScopeFactory)
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
using Common.RabbitMQ;
using RabbitMQ.Client;

namespace ReadService.Subscribers;

public class CustomerSubscriber : RabbitMQReceiver<CustomerSubscriber>
{
    public CustomerSubscriber(IModel channel, ILogger<CustomerSubscriber> logger) : base(channel, logger)
    {
    }

    protected override void HandleCreate(RabbitMQMessage message)
    {
        Logger.LogInformation("Sevaaa");
    }
}
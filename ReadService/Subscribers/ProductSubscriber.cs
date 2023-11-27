using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using RabbitMQ.Client;

namespace ReadService.Subscribers;

public class ProductSubscriber : RabbitMQReceiver<object>
{
    public ProductSubscriber(IModel channel, ILogger<ProductSubscriber> logger) : base(channel, logger)
    {
    }
}
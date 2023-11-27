using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using RabbitMQ.Client;

namespace ReadService.Subscribers;

public class VendorSubscriber : RabbitMQReceiver<object>
{
    public VendorSubscriber(IModel channel, ILogger<ProductSubscriber> logger) : base(channel, logger)
    {
    }
}
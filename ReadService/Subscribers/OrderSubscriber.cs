using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using RabbitMQ.Client;

namespace ReadService.Subscribers;

public class OrderSubscriber : RabbitMQReceiver<object>
{
    public OrderSubscriber(IModel channel, ILogger<OrderSubscriber> logger) : base(channel, logger)
    {
    }
}
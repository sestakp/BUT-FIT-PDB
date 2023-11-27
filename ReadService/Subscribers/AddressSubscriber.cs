using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using RabbitMQ.Client;

namespace ReadService.Subscribers;

public class AddressSubscriber : RabbitMQReceiver<object>
{
    public AddressSubscriber(IModel channel, ILogger<AddressSubscriber> logger) : base(channel, logger)
    {
    }
}
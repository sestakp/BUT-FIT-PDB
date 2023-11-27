using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using RabbitMQ.Client;

namespace ReadService.Subscribers;

public class AddressSubscriber : RabbitMQReceiver<object>
{
    public AddressSubscriber(IModel channel, ILogger<AddressSubscriber> logger) : base(channel, logger)
    {
    }

    protected override void HandleMessage(RabbitMQMessage message)
    {

        Logger.LogInformation("Address subscriber receive message");

        var address = (AddressMessage)message.Data!;

        switch (message.Operation)
        {
            case RabbitMQOperation.Create:

                break;
            case RabbitMQOperation.Update:

                break;
            case RabbitMQOperation.Delete:

                break;
        }


    }
}
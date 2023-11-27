using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using RabbitMQ.Client;

namespace ReadService.Subscribers;

public class ProductSubscriber : RabbitMQReceiver<object>
{
    public ProductSubscriber(IModel channel, ILogger<ProductSubscriber> logger) : base(channel, logger)
    {
    }

    protected override void HandleMessage(RabbitMQMessage message)
    {
        Logger.LogInformation("Product subscriber receive message");

        var product = (ProductMessage)message.Data!;

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
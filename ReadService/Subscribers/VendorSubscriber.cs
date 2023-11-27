using Common.RabbitMQ;
using Common.RabbitMQ.MessageDTOs;
using RabbitMQ.Client;

namespace ReadService.Subscribers;

public class VendorSubscriber : RabbitMQReciever<object>
{
    public VendorSubscriber(IModel channel, ILogger<ProductSubscriber> logger) : base(channel, logger)
    {
    }

    public override void HandleMessage(RabbitMQMessage message)
    {
        Logger.LogInformation("Vendor subscriber receive message");

        var vendor = (VendorMessageDTO)message.Data!;

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
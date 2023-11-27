using Common.RabbitMQ;
using Common.RabbitMQ.MessageDTOs;
using RabbitMQ.Client;

namespace ReadService.Subscribers;

public class OrderSubscriber : RabbitMQReciever<object>
{
    public OrderSubscriber(IModel channel, ILogger<OrderSubscriber> logger) : base(channel, logger)
    {
    }

    public override void HandleMessage(RabbitMQMessage message)
    {
        Logger.LogInformation("Order subscriber receive message");

        var order = (OrderMessageDTO)message.Data!;

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
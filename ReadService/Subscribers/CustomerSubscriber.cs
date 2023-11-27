using Common.RabbitMQ;
using RabbitMQ.Client;

namespace ReadService.Subscribers;

public class CustomerSubscriber : RabbitMQReciever<CustomerSubscriber>
{
    public CustomerSubscriber(IModel channel, ILogger<CustomerSubscriber> logger) : base(channel, logger)
    {
    }

    protected override void HandleMessage(RabbitMQMessage message)
    {
        switch (message.Operation)
        {
            case RabbitMQOperation.Create:
                Logger.LogInformation("Customer subscriber receive create");

                break;
            case RabbitMQOperation.Update:
                Logger.LogInformation("Customer subscriber receive update");

                break;
            case RabbitMQOperation.Delete:
                Logger.LogInformation("Customer subscriber receive delete");

                break;
        }
    }
}
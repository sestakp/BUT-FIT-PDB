using Common.Configuration;
using Common.RabbitMQ;
using Common.RabbitMQ.MessageDTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace ReadService.Subscribers;

public class CustomerSubscriber : RabbitMQReciever<object>
{
    public CustomerSubscriber(IModel channel, ILogger<CustomerSubscriber> logger) : base(channel, logger)
    {
    }

    public override void HandleMessage(RabbitMQMessage message)
    {

        Logger.LogInformation($"Customer subscriber receive message");
        if (message.Entity != RabbitMQEntities.Customer) return;
        var customer = (CustomerMessageDTO)message.Data!;



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
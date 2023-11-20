using Common.Configuration;
using Common.RabbitMQ;
using Common.RabbitMQ.MessageDTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace ReadService.Subscribers
{
    public class CustomerSubscriber : RabbitMQReciever<object>
    {
        public CustomerSubscriber(IModel channel, ILogger<RabbitMQReciever<object>> logger) : base(channel, logger)
        {
        }

        public override void HandleMessage(RabbitMQMessage message)
        {
            Logger.LogInformation("Customer subscriber receive message");

            if (message.Data is not CustomerMessageDTO customer) return;
            
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
}

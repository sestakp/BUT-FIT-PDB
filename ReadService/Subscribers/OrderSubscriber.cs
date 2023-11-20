using Common.RabbitMQ.MessageDTOs;
using Common.RabbitMQ;
using RabbitMQ.Client;

namespace ReadService.Subscribers
{
    public class OrderSubscriber : RabbitMQReciever<object>
    {
        public OrderSubscriber(IModel channel, ILogger<RabbitMQReciever<object>> logger) : base(channel, logger)
        {
        }

        public override void HandleMessage(RabbitMQMessage message)
        {
            Logger.LogInformation("Order subscriber receive message");

            if (message.Data is not OrderMessageDTO order) return;

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

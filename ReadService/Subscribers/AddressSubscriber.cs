using Common.RabbitMQ.MessageDTOs;
using Common.RabbitMQ;
using RabbitMQ.Client;

namespace ReadService.Subscribers
{
    public class AddressSubscriber : RabbitMQReciever<object>
    {
        public AddressSubscriber(IModel channel, ILogger<AddressSubscriber> logger) : base(channel, logger)
        {
        }

        public override void HandleMessage(RabbitMQMessage message)
        {

            Logger.LogInformation("Address subscriber receive message");

            var address = (AddressMessageDTO)message.Data!;

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

using Common.RabbitMQ.MessageDTOs;
using Common.RabbitMQ;
using RabbitMQ.Client;

namespace ReadService.Subscribers
{
    public class ProductSubscriber : RabbitMQReciever<object>
    {
        public ProductSubscriber(IModel channel, ILogger<ProductSubscriber> logger) : base(channel, logger)
        {
        }

        public override void HandleMessage(RabbitMQMessage message)
        {
            Logger.LogInformation("Product subscriber receive message");
            
            var product = (ProductMessageDto)message.Data!;

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

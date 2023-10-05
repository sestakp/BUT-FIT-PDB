using Common.Configuration;
using Common.Models.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Common.RabbitMQ
{
    public class RabbitMQReciever<T> where T : IMongoItem
    {
        private readonly string _queueName;
        private readonly IModel _channel; 
        protected readonly IMongoCollection<T> Collection;
        protected readonly ILogger<RabbitMQReciever<T>> Logger;

        public RabbitMQReciever(IOptions<RabbitMQConfiguration> rabbitMqOptions, IModel channel, IMongoCollection<T> collection, ILogger<RabbitMQReciever<T>> logger)
        {
            Collection = collection;
            Logger = logger;

            _queueName = rabbitMqOptions.Value.QueueName;
            
            _channel = channel;
            Logger.LogDebug("Instantiating RabbitMQReciever");
        }

        public void ReceiveFromExchange(string exchangeName, string queueSuffix = "")
        {
            _channel.ExchangeDeclare(exchange: exchangeName, type: "fanout", durable: false, autoDelete: false, arguments: null);

            _channel.QueueDeclare(queue: _queueName+queueSuffix, durable: false, exclusive: true, autoDelete: false, arguments: null);
            _channel.QueueBind(exchange: exchangeName, queue: _queueName + queueSuffix, routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var id = Encoding.UTF8.GetString(ea.Body.ToArray());
                Logger.Log(LogLevel.Information, $"Received message from channel {_queueName + queueSuffix} with id {id}");
                HandleMessage(id);
                
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_queueName + queueSuffix, false, consumer);
        }

        public virtual void HandleMessage(string id)
        {
        }

        ~RabbitMQReciever()
        {
            Logger.LogDebug("Destructing RabbitMQReciever");
        }
    }
}

using Common.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace Common.RabbitMQ
{
    public class RabbitMQProducer
    {
        private readonly IConnection _connection;
        protected readonly ILogger<RabbitMQProducer> Logger;

        public RabbitMQProducer(IOptions<RabbitMQConfiguration> rabbitMqOptions, IConnection connection, ILogger<RabbitMQProducer> logger)
        {
            _connection = connection;
            Logger = logger;
            Logger.LogDebug("Instantiating RabbitMQProducer");
        }
        public void SendMessage<T>(RabbitMQOperation operation, RabbitMQEntities entity, T data)
        {
            using var channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchange: RabbitMQNames.SyncExchange, type: "direct", durable: true, autoDelete: false, arguments: null);
            
            var message = new RabbitMQMessage<T>
            {
                Operation = operation,
                Entity = entity,
                Data = data
            };

            var jsonMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            channel.BasicPublish(exchange: RabbitMQNames.SyncExchange, routingKey: entity.ToString(), basicProperties: null, body: body);
            Logger.Log(LogLevel.Information, $"Message produced with id {message}");
        }

        public async Task SendMessageAsync<T>(RabbitMQOperation operation, RabbitMQEntities entity, T data)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: RabbitMQNames.SyncExchange, type: "direct", durable: true, autoDelete: false, arguments: null);

                var message = new RabbitMQMessage<T>
                {
                    Operation = operation,
                    Entity = entity,
                    Data = data
                };

                var jsonMessage = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(jsonMessage);

                // Use the asynchronous version of BasicPublish
                await Task.Run(() => channel.BasicPublish(exchange: RabbitMQNames.SyncExchange, routingKey: entity.ToString(), basicProperties: null, body: body));

                Logger.Log(LogLevel.Information, $"Message produced with id {message}");
            }
        }


        ~RabbitMQProducer()
        {
            Logger.LogDebug("Destructing RabbitMQProducer");
        }
    }
}

using Common.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace Common.RabbitMQ
{
    public class RabbitMQProducer
    {
        private readonly string _exchangeName;
        private readonly IConnection _connection;
        protected readonly ILogger<RabbitMQProducer> Logger;

        public RabbitMQProducer(IOptions<RabbitMQConfiguration> rabbitMqOptions, IConnection connection, ILogger<RabbitMQProducer> logger) 
        {
            _exchangeName = string.Format(RabbitMQNames.Exchange, rabbitMqOptions.Value.QueueName);
            _connection = connection;
            Logger = logger;
            Logger.LogDebug("Instantiating RabbitMQProducer");
        }
        public void SendMessage(string id)
        {
            using var channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchange: _exchangeName, type: "fanout", durable: false, autoDelete: false, arguments: null); 

            var body = Encoding.UTF8.GetBytes(id);

            channel.BasicPublish(exchange: _exchangeName, routingKey: "", basicProperties: null, body: body);
            Logger.Log(LogLevel.Information, $"Message produced with id {id}");
        }

        ~RabbitMQProducer()
        {
            Logger.LogDebug("Destructing RabbitMQProducer");
        }
    }
}

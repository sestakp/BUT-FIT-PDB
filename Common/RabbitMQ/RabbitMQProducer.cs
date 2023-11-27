using System.Text;
using Common.RabbitMQ.Messages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Common.RabbitMQ;

public class RabbitMQProducer
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMQProducer> _logger;

    public RabbitMQProducer(IConnection connection, ILogger<RabbitMQProducer> logger)
    {
        _connection = connection;
        _logger = logger;

        _logger.LogDebug($"Instantiating {nameof(RabbitMQProducer)}.");
    }

    public void SendMessageAsync(RabbitMQOperation operation, RabbitMQEntities entity, MessageBase data)
    {
        using (var channel = _connection.CreateModel())
        {
            channel.ExchangeDeclare(
                exchange: RabbitMQNames.SyncExchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null);

            var message = new RabbitMQMessage
            {
                Operation = operation,
                Entity = entity,
                Data = data
            };

            var jsonMessage = JsonConvert.SerializeObject(message, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            channel.BasicPublish(
                exchange: RabbitMQNames.SyncExchange,
                routingKey: entity.ToString(),
                basicProperties: null,
                body: body);

            _logger.LogInformation("Produced message: {message}.", message);
        }
    }

    ~RabbitMQProducer()
    {
        _logger.LogDebug($"Destructing {nameof(RabbitMQProducer)}");
    }
}
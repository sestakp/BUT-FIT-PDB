using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.RabbitMQ;

public class RabbitMQReceiver<T>
{
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQReceiver<T>> _logger;

    protected RabbitMQReceiver(IModel channel, ILogger<RabbitMQReceiver<T>> logger)
    {
        _channel = channel;
        _logger = logger;

        _logger.LogDebug("Instantiating consumer {ConsumerName}.", typeof(T).Name);
    }

    public void ReceiveFromExchange(RabbitMQEntities entity)
    {
        var exchangeName = RabbitMQNames.SyncExchange;
        
        _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

        // Use the entity name to create a unique queue
        var queueName = $"{entity.ToString()}Queue";
        var routingKey = entity.ToString();

        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: true, autoDelete: false, arguments: null);
        _channel.QueueBind(exchange: exchangeName, queue: queueName, routingKey: routingKey);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var serializerOptions = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            var message = JsonConvert.DeserializeObject<RabbitMQMessage>(Encoding.UTF8.GetString(ea.Body.ToArray()), serializerOptions);
            if (message is null)
            {
                throw new Exception("Received message can not be null.");
            }

            _logger.LogInformation("Received message from channel {QueueName} with data: {Message}", queueName, message);

            Action<RabbitMQMessage> handler = message.Operation switch
            {
                RabbitMQOperation.Create => HandleCreate,
                RabbitMQOperation.Update => HandleUpdate,
                RabbitMQOperation.Delete => HandleDelete,
                _ => throw new UnreachableException()
            };

            try
            {
                handler(message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Message handler failed.");
                throw;
            }

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: queueName, false, consumer);
    }

    protected virtual void HandleCreate(RabbitMQMessage message)
    {
        throw new NotImplementedException();
    }

    protected virtual void HandleUpdate(RabbitMQMessage message)
    {
        throw new NotImplementedException();
    }

    protected virtual void HandleDelete(RabbitMQMessage message)
    {
        throw new NotImplementedException();
    }

    ~RabbitMQReceiver()
    {
        _logger.LogDebug("Destructing {ConsumerName}.", typeof(T).Name);
    }
}
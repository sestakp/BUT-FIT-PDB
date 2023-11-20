﻿using Common.Configuration;
using Common.Models.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Common.RabbitMQ
{
    public class RabbitMQReciever<T>
    {
        private readonly IModel _channel;
        //protected readonly IMongoCollection<T> Collection;
        protected readonly ILogger<RabbitMQReciever<T>> Logger;

        public RabbitMQReciever(IModel channel, ILogger<RabbitMQReciever<T>> logger)
        {
            //Collection = collection;
            Logger = logger;
            
            _channel = channel;
            Logger.LogDebug("Instantiating RabbitMQReciever");
        }

        public void ReceiveFromExchange<V>(string exchangeName, RabbitMQEntities entity)
        {
            _channel.ExchangeDeclare(exchange: exchangeName, type: "direct", durable: false, autoDelete: false, arguments: null);

            // Use the entity name to create a unique queue

            var queueName = $"{entity.ToString()}Queue";
            var routingKey = entity.ToString();

            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: true, autoDelete: false, arguments: null);
            _channel.QueueBind(exchange: exchangeName, queue: queueName, routingKey: routingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                RabbitMQMessage? message = JsonConvert.DeserializeObject<RabbitMQMessage>(Encoding.UTF8.GetString(ea.Body.ToArray()));
                Logger.Log(LogLevel.Information, $"Received message from channel {queueName} with data {message}");
                if (message != null)
                {
                    HandleMessage(message);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: queueName, false, consumer);
        }

        public virtual void HandleMessage(RabbitMQMessage message)
        {
            throw new NotImplementedException();
        }

        ~RabbitMQReciever()
        {
            Logger.LogDebug("Destructing RabbitMQReciever");
        }
    }
}

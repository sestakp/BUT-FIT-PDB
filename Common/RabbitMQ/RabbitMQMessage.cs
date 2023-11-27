using Common.RabbitMQ.Messages;

namespace Common.RabbitMQ;

public record RabbitMQMessage
{
    public RabbitMQOperation Operation { get; init; }
    public RabbitMQEntities Entity { get; init; }
    public MessageBase? Data { get; init; }
}
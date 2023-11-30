namespace Common.RabbitMQ.Messages.Products;

public sealed record DeleteProductMessage : MessageBase
{
    public required long ProductId { get; init; }
}
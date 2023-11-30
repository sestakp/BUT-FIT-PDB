namespace Common.RabbitMQ.Messages;

public sealed record CreateReviewMessage : MessageBase
{
    public required long Id { get; init; }
    public required long ProductId { get; init; }
    public required double Rating { get; init; }
    public required string Text { get; init; }
}
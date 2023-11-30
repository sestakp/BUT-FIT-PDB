namespace Common.RabbitMQ.Messages.Customer;

public sealed record DeleteCustomerMessage : MessageBase
{
    public required long Id { get; init; }
}
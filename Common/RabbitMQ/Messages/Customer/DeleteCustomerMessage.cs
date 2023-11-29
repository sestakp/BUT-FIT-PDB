namespace Common.RabbitMQ.Messages.Customer;

public sealed record DeleteCustomerMessage : MessageBase
{
    public required string CustomerEmail { get; init; }
}
namespace Common.RabbitMQ.Messages.Customer;

public sealed record UpdateCustomerMessage : MessageBase
{
    public required string CustomerEmail { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}
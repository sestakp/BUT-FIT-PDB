namespace Common.RabbitMQ.Messages.Customer;

public record CreateCustomerMessage : MessageBase
{
    public required long Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
}
namespace Common.RabbitMQ.Messages.Customer;

public sealed record DeleteCustomerAddressMessage : MessageBase
{
    public required string CustomerEmail { get; init; }
    public required long AddressId { get; init; }
}
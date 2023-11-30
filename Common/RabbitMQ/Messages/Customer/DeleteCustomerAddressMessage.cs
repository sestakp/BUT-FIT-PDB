namespace Common.RabbitMQ.Messages.Customer;

public sealed record DeleteCustomerAddressMessage : MessageBase
{
    public required long Id { get; init; }
    public required long AddressId { get; init; }
}
namespace Common.RabbitMQ.Messages.Customer;

public sealed record UpdateCustomerAddressMessage : MessageBase
{
    public required string CustomerEmail { get; init; }
    public required long AddressId { get; init; }
    public required string Country { get; init; }
    public required string ZipCode { get; init; }
    public required string City { get; init; }
    public required string Street { get; init; }
    public required string HouseNumber { get; init; }
}
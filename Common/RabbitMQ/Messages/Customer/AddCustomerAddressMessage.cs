namespace Common.RabbitMQ.Messages.Customer;

public sealed record AddCustomerAddressMessage : MessageBase
{
    public required long Id { get; init; }
    public required long CustomerId { get; init; }
    public required string Country { get; init; }
    public required string ZipCode { get; init; }
    public required string City { get; init; }
    public required string Street { get; init; }
    public required string HouseNumber { get; init; }
}
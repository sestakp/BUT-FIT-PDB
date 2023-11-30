namespace Common.RabbitMQ.Messages.Vendor;

public sealed record UpdateVendorMessage : MessageBase
{
    public required long VendorId { get; init; }
    public required string Name { get; init; }
    public required string Country { get; init; }
    public required string ZipCode { get; init; }
    public required string City { get; init; }
    public required string Street { get; init; }
    public required string HouseNumber { get; init; }
}
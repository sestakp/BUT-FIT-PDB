namespace Common.RabbitMQ.Messages;

public sealed record CreateVendorMessage : MessageBase
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public required string AddressCountry { get; init; }
    public required string AddressZipCode { get; init; }
    public required string AddressCity { get; init; }
    public required string AddressStreet { get; init; }
    public required string AddressHouseNumber { get; init; }
}
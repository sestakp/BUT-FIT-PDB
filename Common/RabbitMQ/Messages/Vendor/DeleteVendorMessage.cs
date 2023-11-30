namespace Common.RabbitMQ.Messages.Vendor;

public sealed record DeleteVendorMessage : MessageBase
{
    public required long VendorId { get; init; }
}
namespace Common.RabbitMQ.Messages;

public sealed record OrderCompletedMessage : MessageBase
{
    public required long Id { get; init; }
    public required long CustomerId { get; init; }
    public required decimal Price { get; init; }
    public required string AddressCountry { get; init; }
    public required string AddressZipCode { get; init; }
    public required string AddressCity { get; init; }
    public required string AddressStreet { get; init; }
    public required string AddressHouseNumber { get; init; }
    public required IEnumerable<OrderProductRabbit> Products { get; init; }
    public required DateTime DateTimeCreated { get; init; }

}
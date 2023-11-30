namespace Common.RabbitMQ.Messages;

public sealed record CreateProductMessage : MessageBase
{
    public required long Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required decimal Price { get; init; }
    public required int PiecesInStock { get; init; }
    public required long VendorId { get; init; }
    public required string VendorName { get; init; }
    public required IEnumerable<string> Categories { get; init; }
    public required IEnumerable<string> SubCategories { get; init; }
}
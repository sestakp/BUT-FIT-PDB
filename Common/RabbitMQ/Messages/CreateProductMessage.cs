namespace Common.RabbitMQ.Messages;

public sealed record CreateProductMessage : MessageBase
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required decimal Price { get; init; }
    public required int PiecesInStock { get; init; }
    public required double Rating { get; init; }
    public required ProductVendor Vendor { get; init; }
    public required IEnumerable<string> Categories { get; init; }
    public required IEnumerable<string> SubCategories { get; init; }

    public record ProductVendor(int Id, string Name);
}
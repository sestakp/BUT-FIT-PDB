namespace WriteService.Entities;

public class ReviewEntity : EntityBase
{
    public required int Rating { get; init; }
    public required string Text { get; init; }

    // Foreign keys
    public ProductEntity Product { get; init; } = null!;
    public long ProductId { get; init; }

    public CustomerEntity Customer { get; init; } = null!;
    public long CustomerId { get; init; }
}
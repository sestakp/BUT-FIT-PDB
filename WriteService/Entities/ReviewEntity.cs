namespace WriteService.Entities;

public class ReviewEntity : EntityBase
{
    public required int Rating { get; init; }
    public required string Text { get; init; }

    public long ProductId { get; init; }
    public required ProductEntity Product { get; init; }
}
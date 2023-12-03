namespace WriteService.Entities;

public sealed class OrderProductEntity : EntityBase
{
    public OrderEntity Order { get; init; } = null!;
    public long OrderId { get; init; }

    public ProductEntity Product { get; init; } = null!;
    public long ProductId { get; init; }

    public int Count { get; set; }
}
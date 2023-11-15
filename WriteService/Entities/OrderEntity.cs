using Common.Enums;

namespace WriteService.Entities;

public class OrderEntity : EntityBase
{
    public string? Country { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? HouseNumber { get; set; }
    public OrderStatusEnum Status { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsDeleted { get; set; }

    public IList<ProductEntity> Products { get; set; } = null!;

    public long CustomerId { get; set; }
    public CustomerEntity Customer { get; set; } = null!;
}
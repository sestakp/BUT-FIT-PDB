namespace WriteService.Entities;

public class ProductEntity : EntityBase
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required int PiecesInStock { get; set; }

    public bool IsDeleted { get; set; }

    public long VendorId { get; set; }
    public VendorEntity Vendor { get; set; } = null!;

    public IList<ProductCategoryEntity> Categories { get; set; } = null!;
    public IList<OrderEntity> Orders { get; set; } = null!;
    public IList<ReviewEntity> Reviews { get; set; } = null!;
}
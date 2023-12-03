namespace WriteService.Entities;

public class ProductEntity : EntityBase
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
    public required int PiecesInStock { get; set; }

    public bool IsDeleted { get; set; } = false;

    public long VendorId { get; set; }
    public VendorEntity Vendor { get; set; } = null!;

    public List<ProductCategoryEntity> Categories { get; set; } = new();
    public List<ProductSubCategoryEntity> SubCategories { get; set; } = new();
    public List<ReviewEntity> Reviews { get; set; } = new();
}
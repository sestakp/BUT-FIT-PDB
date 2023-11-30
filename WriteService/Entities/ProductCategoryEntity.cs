namespace WriteService.Entities;

public class ProductCategoryEntity : EntityBase
{
    public required string Name { get; set; }
    public required string Description { get; set; }

    public List<ProductSubCategoryEntity> SubCategories { get; set; } = new();
}
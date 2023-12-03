using WriteService.Entities.Interfaces;

namespace WriteService.Entities;

public class ProductCategoryEntity : EntityBase, INormalizedName
{
    public required string Name { get; set; }
    public required string NormalizedName { get; set; }
    public required string Description { get; set; }
    public List<ProductSubCategoryEntity> SubCategories { get; set; } = new();
}
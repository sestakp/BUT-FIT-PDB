using WriteService.Entities.Interfaces;

namespace WriteService.Entities;

public class ProductSubCategoryEntity : EntityBase, INormalizedName
{
    public required string Name { get; set; }
    public required string NormalizedName { get; set; }
    public required string Description { get; set; }
    public ProductCategoryEntity Category { get; set; } = null!;
}
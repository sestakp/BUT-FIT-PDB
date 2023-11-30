namespace WriteService.Entities;

public class ProductSubCategoryEntity : EntityBase
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required long CategoryId { get; set; }
    public required ProductCategoryEntity Category { get; set; }
}
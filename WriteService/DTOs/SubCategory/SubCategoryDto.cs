namespace WriteService.DTOs.SubCategory;

public class SubCategoryDto
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required string NormalizedName { get; set; }
    public required string Description { get; set; }
    public required long CategoryId { get; set; }
}

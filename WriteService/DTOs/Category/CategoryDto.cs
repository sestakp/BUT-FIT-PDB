namespace WriteService.DTOs.Category;

public class CategoryDto
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required string NormalizedName { get; set; }
    public required string Description { get; set; }
}

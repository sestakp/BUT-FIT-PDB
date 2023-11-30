namespace WriteService.DTOs.SubCategory;

public record UpdateSubCategoryDto(
    string Name,
    string Description,
    long CategoryId
);

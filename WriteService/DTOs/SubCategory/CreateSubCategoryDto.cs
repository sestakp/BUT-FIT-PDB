namespace WriteService.DTOs.SubCategory;

public record CreateSubCategoryDto(
    string Name,
    string Description,
    long CategoryId
);

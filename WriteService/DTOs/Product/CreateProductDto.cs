namespace WriteService.DTOs.Product;

public sealed record CreateProductDto(
    string Title,
    string Description,
    int PricesInStock,
    decimal Price,
    long VendorId,
    IEnumerable<string> Categories,
    IEnumerable<string> SubCategories);
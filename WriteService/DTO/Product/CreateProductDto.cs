namespace WriteService.DTO.Product;

public sealed record CreateProductDto(
    string Title,
    string Description,
    int PricesInStock,
    decimal Price,
    int VendorId);
namespace WriteService.DTOs.Product;

public record ProductDto(
    long Id,
    string Title,
    string Description,
    decimal Price,
    int PricesInStock);
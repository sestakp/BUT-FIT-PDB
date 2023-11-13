namespace WriteService.DTO.Product;

public record ProductDto(
    string Title,
    string Description,
    decimal Price,
    int PricesInStock);
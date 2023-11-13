namespace WriteService.DTO.Order;

public sealed record CompleteOrderDto(
    string Country,
    string ZipCode,
    string City,
    string Street,
    string HouseNumber);
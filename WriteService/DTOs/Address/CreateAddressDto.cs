namespace WriteService.DTOs.Address;

public sealed record CreateAddressDto(
    string Country,
    string ZipCode,
    string City,
    string Street,
    string HouseNumber);
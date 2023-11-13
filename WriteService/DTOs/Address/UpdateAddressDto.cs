namespace WriteService.DTOs.Address;

public record UpdateAddressDto(
    string Country,
    string ZipCode,
    string City,
    string Street,
    string HouseNumber);
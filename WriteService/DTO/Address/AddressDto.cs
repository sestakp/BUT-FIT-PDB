namespace WriteService.DTO.Address;

public record AddressDto(
    string Country,
    string ZipCode,
    string City,
    string Street,
    string HouseNumber);
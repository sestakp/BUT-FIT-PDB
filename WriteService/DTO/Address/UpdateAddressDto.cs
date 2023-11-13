namespace WriteService.DTO.Address;

public record UpdateAddressDto(
    string Country,
    string ZipCode,
    string City,
    string Street,
    string HouseNumber);
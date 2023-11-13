namespace WriteService.DTO.Vendor;

public record VendorDto(
    long Id,
    string Name,
    string Country,
    string ZipCode,
    string City,
    string Street,
    string HouseNumber);
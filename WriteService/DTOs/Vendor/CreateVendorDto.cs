namespace WriteService.DTOs.Vendor;

public record CreateVendorDto(
    string Name,
    string Country,
    string ZipCode,
    string City,
    string Street,
    string HouseNumber);
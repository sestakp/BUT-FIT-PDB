namespace WriteService.DTOs.Vendor;

public record UpdateVendorDto(
    string Name,
    string Country,
    string ZipCode,
    string City,
    string Street,
    string HouseNumber);
namespace WriteService.DTOs.Vendor;

public record UpdateVendorDto(
    string Country,
    string ZipCode,
    string City,
    string Street,
    string HouseNumber);
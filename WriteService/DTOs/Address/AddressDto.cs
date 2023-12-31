﻿namespace WriteService.DTOs.Address;

public record AddressDto(
    long Id,
    string Country,
    string ZipCode,
    string City,
    string Street,
    string HouseNumber);
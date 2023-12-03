﻿namespace WriteService.DTOs.Customer;

public record CustomerDto
{
    public long Id { get; set; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
}
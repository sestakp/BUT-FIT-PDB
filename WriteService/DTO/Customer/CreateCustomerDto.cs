namespace WriteService.DTO.Customer;

public record CreateCustomerDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string PasswordHash);
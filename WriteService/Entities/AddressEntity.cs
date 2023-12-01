namespace WriteService.Entities;

public class AddressEntity : EntityBase
{
    public required string Country { get; set; }
    public required string ZipCode { get; set; }
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string HouseNumber { get; set; }


    public long CustomerId { get; set; }
    public CustomerEntity? Customer { get; set; }
}
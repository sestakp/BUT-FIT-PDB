namespace WriteService.Entities;

public class VendorEntity : EntityBase
{
    public required string Name { get; set; }
    public required string Country { get; set; }
    public required string ZipCode { get; set; }
    public required string City { get; set; }
    public required string Street { get; set; }
    public required string HouseNumber { get; set; }

    public bool IsDeleted { get; set; } = false;

    public IList<ProductEntity> Products = new List<ProductEntity>();
}
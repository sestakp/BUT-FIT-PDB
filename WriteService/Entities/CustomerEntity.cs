using System.ComponentModel.DataAnnotations;

namespace WriteService.Entities;

public class CustomerEntity : EntityBase
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string PasswordHash { get; set; }
    public bool IsDeleted { get; set; } = false;

    public IList<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    public IList<AddressEntity> Addresses { get; set; } = new List<AddressEntity>();
}
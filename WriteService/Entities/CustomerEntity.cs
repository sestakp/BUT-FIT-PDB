using System.ComponentModel.DataAnnotations;

namespace WriteService.Entities
{
    public class CustomerEntity
    {
        [Key]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public bool isDeleted { get; set; }
        public IList<OrderEntity> Orders { get; set; }
    }
}

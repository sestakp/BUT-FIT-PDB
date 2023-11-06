using System.ComponentModel.DataAnnotations;

namespace WriteService.Entities
{
    public class VendorEntity
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}

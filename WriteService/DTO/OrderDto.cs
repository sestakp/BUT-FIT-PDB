using Common.Enums;

namespace WriteService.DTO
{
    public class OrderDto
    {
        public long Id { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public long CustomerId { get; set; }
    }
}

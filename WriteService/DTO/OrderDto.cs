using Common.Enums;

namespace WriteService.DTO
{
    public class OrderDto
    {
        public long Id { get; set; }
        public OrderStatusEnum Status { get; set; }
        public decimal Price { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool isDeleted { get; set; }
    }
}

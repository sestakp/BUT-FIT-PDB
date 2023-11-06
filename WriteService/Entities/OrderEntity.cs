﻿using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace WriteService.Entities
{
    public class OrderEntity
    {
        [Key]
        public long Id { get; set; }
        public OrderStatusEnum Status { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool isDeleted { get; set; }
        public IList<ProductEntity> Products { get; set; }
        public long CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.RabbitMQ.MessageDTOs.Interfaces;

namespace Common.RabbitMQ.MessageDTOs
{
    public class CustomerMessageDTO : IMessageDTO
    {
        public long Id { get; init; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
    }
}

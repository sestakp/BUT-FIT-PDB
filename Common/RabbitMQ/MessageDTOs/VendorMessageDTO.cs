using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.RabbitMQ.MessageDTOs.Interfaces;

namespace Common.RabbitMQ.MessageDTOs
{
    public class VendorMessageDTO : IMessageDTO
    {
        public long Id { get; init; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.RabbitMQ.MessageDTOs;
using Common.RabbitMQ.MessageDTOs.Interfaces;

namespace Common.RabbitMQ
{
    public class RabbitMQMessage
    {
        public RabbitMQOperation Operation { get; set; }
        public RabbitMQEntities Entity { get; set; }
        public IMessageDTO? Data { get; set; }

    }
}

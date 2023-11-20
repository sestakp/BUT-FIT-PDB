using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.RabbitMQ
{
    public class RabbitMQMessage<T>
    {
        public RabbitMQOperation Operation { get; set; }
        public RabbitMQEntities Entity { get; set; }
        public T? Data { get; set; }

    }
}

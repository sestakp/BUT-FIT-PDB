﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.RabbitMQ.MessageDTOs
{
    public class AddressMessageDTO : IMessageDTO
    {
        public long Id { get; init; }
    }
}
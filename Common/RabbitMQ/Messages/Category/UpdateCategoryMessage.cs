using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.RabbitMQ.Messages.Category;
public sealed record UpdateCategoryMessage : MessageBase
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}
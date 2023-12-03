using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.RabbitMQ.Messages.SubCategory;
public sealed record DeleteSubCategoryMessage : MessageBase
{
    public required long Id { get; init; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Common.Cqrs
{
    public interface IMemento
    {
        Guid Id { get; set; }
        int Version { get; set; }
    }
}

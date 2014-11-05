using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Common.Cqrs;

namespace Agile.Planning.Domain.Commands
{
    public class DeleteStoryCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agile.Common.Cqrs;

namespace Agile.Planning.Domain.Commands
{
    public class ChangeStoryTitleCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}

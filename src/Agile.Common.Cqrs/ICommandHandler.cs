using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agile.Common.Cqrs
{
    public interface ICommandHandler<in T> where T : ICommand
    {
        Task Handle(T command);
    }
}

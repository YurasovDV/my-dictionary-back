using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBack.Infrastructure
{
    interface ICommandHandler<T> where T : ICommand
    {
    }
}

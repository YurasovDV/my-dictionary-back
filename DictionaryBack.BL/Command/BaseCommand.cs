using DictionaryBack.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryBack.BL.Command
{
    public class BaseCommand 
    {
        protected readonly DictionaryContext DictionaryContext;

        public BaseCommand(DictionaryContext dictionaryContext)
        {
            DictionaryContext = dictionaryContext;
        }
    }
}

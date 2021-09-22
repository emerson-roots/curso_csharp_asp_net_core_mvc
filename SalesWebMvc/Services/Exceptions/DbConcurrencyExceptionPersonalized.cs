using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services.Exceptions
{   
    // aula 260
    public class DbConcurrencyExceptionPersonalized : ApplicationException
    {

        public DbConcurrencyExceptionPersonalized(string message) : base(message)
        {
        }

    }
}

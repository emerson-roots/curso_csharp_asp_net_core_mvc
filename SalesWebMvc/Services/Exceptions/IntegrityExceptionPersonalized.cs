using System;

namespace SalesWebMvc.Services.Exceptions
{

    // aula 265
    public class IntegrityExceptionPersonalized : ApplicationException
    {
        public IntegrityExceptionPersonalized(string message) : base(message)
        {

        }

    }
}

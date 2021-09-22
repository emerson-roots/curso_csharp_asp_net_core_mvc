using System;

namespace SalesWebMvc.Services.Exceptions
{
    // aula 260 - herda de ApplicationException
    public class NotFoundExceptionPersonalized : ApplicationException
    {
        // construtor simples que repassa a chamada para a classe base
        public NotFoundExceptionPersonalized(string message) : base(message)
        {
        }

    }
}

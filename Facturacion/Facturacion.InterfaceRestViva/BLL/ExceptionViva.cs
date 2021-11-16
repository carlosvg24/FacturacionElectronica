using System;

namespace Facturacion.InterfaceRestViva
{
    public class ExceptionViva : Exception
    {
        public bool RegistradaLog { get; set; }
        public ExceptionViva()
        {
        }

        public ExceptionViva(string message)
        : base(message)
        {
        }

        public ExceptionViva(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}

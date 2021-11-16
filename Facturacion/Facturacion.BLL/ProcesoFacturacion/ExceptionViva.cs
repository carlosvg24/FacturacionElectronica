using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.BLL.ProcesoFacturacion
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

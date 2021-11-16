using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.RestFul
{
    public class ENTDatosGeneraBodyTicketFactura
    {
        public string xmlCFDI { get; set; }
        public string cadenaOriginal { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
    }
}

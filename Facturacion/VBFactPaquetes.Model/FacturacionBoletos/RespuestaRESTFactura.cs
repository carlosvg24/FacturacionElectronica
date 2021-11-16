using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model.FacturacionBoletos
{
    public class RespuestaRESTFactura
    {
        public long Codigo { get; set; }
        public string Mensaje { get; set; }
        public string PagosFacturados { get; set; }

        public string XML { get; set; }

        public byte[] PDF { get; set; }

        public RespuestaRESTFactura()
        {
            this.Codigo = 0;
            this.Mensaje = string.Empty;
            this.PagosFacturados = string.Empty;

        }
    }
}

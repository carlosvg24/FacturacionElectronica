using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.FacturaGlobal
{
    public class ReservaDetalleTraslado
    {
        public Int64 FolioPrefactura { get; set; }

        public decimal Total { get; set; }

        public decimal PorcIva { get; set; }

        public string Tipo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model.PantallaFacturacion
{
    public class ItemPagoPorFacturar
    {
        public Int64 IdFactReserva { get; set; }

        public Int64 IdFactPagos { get; set; }

        public string Concepto { get; set; }

        public string Referencia { get; set; }
        
        public decimal Monto { get; set; }

        public decimal Iva { get; set; }
    }
}

using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzasTools
{
    public class ENTPagosCambiaFormaPago:ENTPagosCab
    {
        public string CveFormaPagoSAT { get; set; }

        public string DescripcionFormaPagoSAT { get; set; }
        public Decimal MontoPagoFacturacion { get; set; }

        public string DescripcionFormaPagoNavitaire { get; set; }

    }
}

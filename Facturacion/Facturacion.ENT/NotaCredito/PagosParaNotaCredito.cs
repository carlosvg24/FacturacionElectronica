using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.NotaCredito
{
    public class PagosParaNotaCredito
    {
        public Int64 IdPagosCab { get; set; }

        public Int64 PaymentId { get; set; }

        public string GlobalUUID { get; set; }

        public Int64 GlobalIdFacturaCab { get; set; }

        public Int64 FacturaIdFacturaCab { get; set; }

        public Int64 BookingId { get; set; }

        public string FacturaUUID { get; set; }

        public string XMLRequest { get; set; }

    }
}

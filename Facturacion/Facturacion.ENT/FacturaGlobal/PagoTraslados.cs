using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.FacturaGlobal
{
    public class PagoTraslados
    {
        public Int64 IdPagosCab { get; set; }

        public Int64 FolioPrefactura { get; set; }

        public Int64 PaymentId { get; set; }

        public Int64 BookingId { get; set; }

        public DateTime FechaPago { get; set; }

        public string IdFormaPago { get; set; }

        public string CurrencyCode { get; set; }

        public Decimal MontoTotal { get; set; }

        public Decimal MontoIVA { get; set; }
            
        public Decimal Base { get; set; }

        public Decimal Base0 { get; set; }

        public DateTime FechaHoraLocal { get; set; }

        public bool EsDividido { get; set; }

        public bool EsPadre { get; set; }

        public bool EsHijo { get; set; }

        public Int64 PaymentIdPadre { get; set; }

        public bool YaFacturado { get; set; }
    }
}

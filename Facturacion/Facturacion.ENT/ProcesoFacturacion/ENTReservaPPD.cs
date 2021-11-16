using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT
{
    [Serializable]
    public class ENTReservaPPD
    {
        public long BookingID { get; set; }
        public string PNR { get; set; }
        public string BookingStatus { get; set; }
        public string CreatedOrganizationCode { get; set; }
        public string Organizacion { get; set; }
        public int NumPasajeros { get; set; }
        public string Moneda { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal MontoPagado { get; set; }
        public int PaidStatus { get; set; }
        public string EstatusPago { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public decimal MontoFacturado { get; set; }
        public bool EstaMarcadoParaFacturacion { get; set; }
        public long IdFacturaCab { get; set; }
        public string EstatusFacturado { get; set; }
        public string DescEstatusFacturado { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturacion33.PortalAdmin.Models.ENT
{
    [Serializable]
    public class ENTFiltrosConsulta
    {
        public string PNR { get; set; }
        public string CodigoGrupoPPD { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public string EstatusPago { get; set; }
        public string EstatusFacturacion { get; set; }

    }
}
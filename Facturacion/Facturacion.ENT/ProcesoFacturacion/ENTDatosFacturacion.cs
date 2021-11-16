using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.ProcesoFacturacion
{
    public class ENTDatosFacturacion
    {

        public string ClaveReservacion { get; set; }

        public string NombrePasajero { get; set; }

        public string ApellidosPasajero { get; set; }

        public string UsoCFDI { get; set; }

        public bool EsExtranjero { get; set; }
        public string RFCReceptor { get; set; }

        public string PaisResidenciaFiscal { get; set; }

        public string TAXID { get; set; }

        public string EmailReceptor { get; set; }
    }
}

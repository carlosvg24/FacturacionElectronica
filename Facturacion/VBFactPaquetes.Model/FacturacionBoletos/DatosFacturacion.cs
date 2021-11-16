using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model.FacturacionBoletos
{
    public class DatosFacturacion
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

        

        public string Usuario { get; set; }
        public string Password { get; set; }

        public int Proceso { get; set; }

        public List<PagosSinFacturar> Pagos { get; set; }
    }
}

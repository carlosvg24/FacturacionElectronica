using Facturacion.ENT.ProcesoFacturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT
{
    /*
        {
            "ListaPagosSeleccionados": null,
            "Password": null,
            "PNR": null,
            "NombrePasajero": null,
            "ApellidosPasajero": null,
            "UsoCFDI": null,
            "EsExtranjero": false,
            "RFCReceptor": null,
            "ResidenciaFiscal": null,
            "NumRegIdTrib_TAXID": null,
            "emailReceptor": null
        }
    */

    public class ENTDatosFacturacionPorRest : ENTDatosFacturacion
    {
        public List<ENTPagosSinFacturar> Pagos { get; set; }

        public string Usuario { get; set; }
        public string Password { get; set; }
    }
}

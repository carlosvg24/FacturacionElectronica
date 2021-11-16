using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.ProcesoFacturacion
{
    public class ENTPagosFacturados:ENTPagosCab
    {
        public ENTFormapagoCat ENTFormaPago { get; set; }
        public List<ENTPagosDet> ListaDesglosePago { get; set; }

        public bool EstaMarcadoParaFacturacion { get; set; }
        public ENTFacturaCliente FacturaGenerada { get; set; }

        public decimal MontoAplicado { get; set; }

        public bool RequiereConfirmacion { get; set; }

        public bool EsVinculacionCorrecta { get; set; }


        public DateTime FechaIniEvento { get; set; }
        public DateTime FechaFinEvento { get; set; }


    }
}

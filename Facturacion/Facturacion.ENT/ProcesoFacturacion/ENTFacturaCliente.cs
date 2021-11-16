using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.ProcesoFacturacion
{
    public class ENTFacturaCliente : ENTFacturasCab
    {
        public List<ENTFacturasDet> ListaFacturasDet { get; set; }
        
        public List<ENTFacturasivaDet> ListaIVAPorDetalle { get; set; }
        public List<ENTFacturascargosDet> ListaCargosAero { get; set; }
        public List<ENTPagosFacturados> ListaPagosFacturados { get; set; }
        public ENTFacturascfdiDet FacturasCFDIDet { get; set; }
        public ENTXmlPegaso XmlPagaso { get; set; }
        public bool EsFacturado { get; set; }
        public bool SolicitaConfirmacion { get; set; }

    }
}

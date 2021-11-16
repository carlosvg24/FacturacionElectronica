using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.ProcesoFacturacion
{
    public class ENTNotaCredito:ENTNotascreditoCab 
    {

        List<ENTNotascreditoDet> ListaNotaCreditoDetalle { get; set; }

        List<ENTNotascreditoivaDet> ListaIVAPorDetalle { get; set; }
        List<ENTNotascreditocargosDet> ListaCargosAero { get; set; }
        List<ENTCfdirelacionadosDet> ListaCFDIRelacionados { get; set; }
        ENTNotascreditocfdiDet NotaCreditoCFDI { get; set; }
        ENTXmlPegaso XmlPagaso { get; set; }
    }
}

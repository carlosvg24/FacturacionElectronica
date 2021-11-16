using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.ProcesoFacturacion
{
    public class ENTEnvioFacturaGlobal
    {

        public long FolioFG { get; set; }
        public string LugarExp { get; set; }
        public int NumPagosEnviados { get; set; }
        public long IdPeticionPAC { get; set; }
        public DateTime FechaEmisionFG { get; set; }
        public string UUID { get; set; }
        public decimal MontoTotalFactura { get; set; }
        public bool TimbradoExitoso { get; set; }
        public string Mensaje { get; set; }
    }
}

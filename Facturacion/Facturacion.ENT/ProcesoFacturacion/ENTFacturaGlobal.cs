using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.ProcesoFacturacion
{
    public class ENTFacturaGlobal: ENTFacturaCliente 
    {
        /// <summary>
        /// Detalle de los pagos registrados por cada partida de la global
        /// </summary>
        public List<ENTGlobalticketsDet> ListaGlobalTicketsDet { get; set; }
        public List<ENTGlobalcargosaeroDet> ListaGlobalcargosaeroDet { get; set; }
    }
}

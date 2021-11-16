using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.ProcesoFacturacion
{
    public class ENTReservacion:ENTReservaCab
    {
        public List<ENTVuelosCab> ListaVuelos { get; set; }
        public List<ENTReservaDet> ListaReservaDet { get; set; }

    }
}

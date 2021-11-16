using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.Portal.Facturacion
{
    public class ENTPagosPorFacturar : ENTPagosCab
    {
        public List<ENTPagosDet> ListaDesglosePago { get; set; }

        public bool EstaMarcadoParaFacturacion { get; set; }

        public string RutaCFDI { get; set; }
        public string RutaPDF { get; set; }
        public string RutaZip { get; set; }
        public string Mensaje { get; set; }
        public bool EnVigenciaParaFacturacion { get; set; }
        
        public bool ConBloqueoFacturacion { get; set; }

        public string PNR { get; set; }

        public string pathVariosPNR { get; set; }
    }

}

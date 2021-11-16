using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.ProcesoFacturacion
{
    public class ENTImpresoraTermica
    {
        public int IdImpTermica { get; set; }
        public string NombreImpresora { get; set; }
        public string RutaImpresora { get; set; }
        public bool EsDefault { get; set; }
        public bool DisponibleEnRed { get; set; }
        public bool EsLocal { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT
{
    public class ENTMenusPorRol
    {
        public int IdRol { get; set; }
        public string DescripcionRol { get; set; }
        public List<ENTMenusViva> MenuPrincipalPorRol { get; set; }
    }
}

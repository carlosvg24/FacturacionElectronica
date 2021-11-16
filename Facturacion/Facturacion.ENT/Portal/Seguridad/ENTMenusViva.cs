using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT
{
    public class ENTMenusViva:ENTElementoMenu 
    {
        public List<ENTElementoMenu> ListaSubMenus { get; set; }
    }
}

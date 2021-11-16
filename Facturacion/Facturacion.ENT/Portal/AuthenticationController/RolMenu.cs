using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.Portal.AuthenticationController
{
    public class RolMenu
    {
        public int IdRol { get; set; }

        public string Rol { get; set; }

        public int IdMenuOpcion { get; set; }

        public int IdMenuPadre { get; set; }

        public string Menu { get; set; }

        public string Descripcion { get; set; }

        public string UrlMenu { get; set; }

        public string UrlImagen { get; set; }

        public int Orden { get; set; }
    }
}

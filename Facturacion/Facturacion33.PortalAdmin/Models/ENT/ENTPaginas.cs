using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturacion33.PortalAdmin.Models.ENT
{
    [Serializable]
    public class ENTPaginas
    {
        public int PaginaActual { get; set; }
        public string NumPaginaDeTotal { get; set; }
    }
}
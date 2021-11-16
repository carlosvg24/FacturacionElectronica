using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT
{
    public class ENTMenu
    {
        //Menu
        public int IdMenuOpcion { get; set; }
        public string Nombre_Menu { get; set; }
        public string UrlImagen { get; set; }
        public string UrlMenu { get; set; }
        public int Orden { get; set; }
        public string Descripcion { get; set; }
        public int IdMenuPadre { get; set; }
        public bool PermisoAgregar { get; set; }
        public bool PermisoConsultar { get; set; }
        public bool PermisoEditar { get; set; }
        public bool PermisoEliminar { get; set; }
        public decimal OrdenMostrar { get; set; }
        public bool EsParent { get; set; }
    }
}

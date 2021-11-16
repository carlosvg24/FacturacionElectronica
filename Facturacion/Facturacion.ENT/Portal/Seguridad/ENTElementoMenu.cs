using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT
{
    public class ENTElementoMenu
    {
        public int IdMenuOpcion { get; set; }
        public string UrlImagen { get; set; }
        public string UrlMenu { get; set; }
        public string Nombre_Menu { get; set; }
        public string Descripcion { get; set; }
        public bool PermisoAgregar { get; set; }

        /// <summary>
        /// Bandera que indica si el rol tiene permisos para Editar
        /// </summary>
        public bool PermisoEditar { get; set; }

        /// <summary>
        /// Bandera que indica si el rol tiene permisos para Eliminar
        /// </summary>
        public bool PermisoEliminar { get; set; }

        /// <summary>
        /// Bandera que indica si el rol tiene permisos para Consultar
        /// </summary>
        public bool PermisoConsultar { get; set; }

    }
}

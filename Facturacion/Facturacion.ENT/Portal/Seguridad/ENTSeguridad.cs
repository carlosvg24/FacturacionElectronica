using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Facturacion.ENT
{
    public class ENTSeguridad
    {
        #region Propiedades Públicas

        //validación de usuario
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int IdAgente { get; set; }
        public string CodigoSAP { get; set; }
        public string CodigoAgente { get; set; }

        public int IdRolActual { get; set; }
        public string DescripcionRol { get; set; }
        public List<ENTMenusViva> MenuPrincipalPorRol { get; set; }

        public bool EsValido { get; set; }
        public string Error { get; set; }
        public Dictionary<int, string> ListaComboRoles { get; set; }
        public List<ENTMenusPorRol> ListaRoles { get; set; }

        #endregion Propiedades Públicas
    }

}

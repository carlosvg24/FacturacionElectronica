using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.Portal.Seguridad
{
    public class ENTUsuario
    {

        #region Propiedades Públicas
        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Usuario { get; set; }

        /// <summary>
        /// Identificador de agente utilizado en Navitaire
        /// </summary>
        public int IdAgente { get; set; }

        /// <summary>
        /// Nombre del usuario
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Apellidos del usuario
        /// </summary>
        public string Apellidos { get; set; }

        /// <summary>
        /// Contraseña encriptada en BD
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Bandera que indica si el usuario esta activo o no
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Fecha en que inicia la vigencia del usuario
        /// </summary>
        public DateTime FechaIniVigencia { get; set; }

        /// <summary>
        /// Fecha en que finaliza la vigencia del usuario
        /// </summary>
        public DateTime FechaFinVigencia { get; set; }

        /// <summary>
        /// Fecha hora del registro del usuario
        /// </summary>
        public DateTime FechaHoraLocal { get; set; }


        #endregion Propiedades Públicas

    }
}

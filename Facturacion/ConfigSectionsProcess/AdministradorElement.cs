using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSectionsProcess
{
    public class AdministradorElement: ConfigurationElement
    {
        /// <summary>
        ///Nombre(s) del Administrador
        /// </summary>
        [ConfigurationProperty("Nombre", IsRequired = true)]
        public string Nombre
        {
            get { return (string)this["Nombre"]; }
            set { this["Nombre"] = value; }
        }

        /// <summary>
        ///Apellido(s) del Administrador
        /// </summary>
        [ConfigurationProperty("Apellido", IsRequired = true)]
        public string Apellido
        {
            get { return (string)this["Apellido"]; }
            set { this["Apellido"] = value; }
        }

        /// <summary>
        ///Correo del Admin
        /// </summary>
        [ConfigurationProperty("Correo", IsRequired = true, IsKey =true)]
        public string Correo
        {
            get { return (string)this["Correo"]; }
            set { this["Correo"] = value; }
        }
    }
}

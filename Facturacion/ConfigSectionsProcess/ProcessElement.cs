using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSectionsProcess
{
    public class ProcessElement : ConfigurationElement
    {
        /// <summary>
        ///Nombre del Proceso y clase del proceso
        /// </summary>
        [ConfigurationProperty("Nombre", IsRequired = true, IsKey = true)]
        public string Nombre
        {
            get { return (string)this["Nombre"]; }
            set { this["Nombre"] = value; }
        }

        /// <summary>
        /// Se activara el modo debugel cual es cuando la tarea crea la mayor parte de logs de bitacora posible.
        /// </summary>
        [ConfigurationProperty("TypeName", IsRequired = true)]
        public string TypeName
        {
            get { return (string)this["TypeName"]; }
            set { this["TypeName"] = value; }
        }
    }
}

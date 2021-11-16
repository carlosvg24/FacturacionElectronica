using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSectionsProcess
{
    public class ParameterElement : ConfigurationElement
    {
        /// <summary>
        /// Nombre o Id con el que se encuentra en la BD especificamente en la tabla \"dbo.VBFac_Parametros_Cnf\"
        /// </summary>
        [ConfigurationProperty("DataBaseKey", IsRequired = true, IsKey = true)]
        public string DataBaseKey
        {
            get { return (string)this["DataBaseKey"]; }
            set { this["DataBaseKey"] = value; }
        }

        /// <summary>
        /// Renombrado Corto del ID para encontarlo en la primera key delObjeto parametros de la interface ITask
        /// </summary>
        [ConfigurationProperty("Name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        /// <summary>
        /// Renombrado Corto del ID para encontarlo en la primera key delObjeto parametros de la interface ITask
        /// </summary>
        [ConfigurationProperty("IsString", IsRequired = true)]
        public bool IsString
        {
            get { return (bool)this["IsString"]; }
            set { this["IsString"] = value; }
        }
    }
}

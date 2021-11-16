using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSectionsProcess
{
    public class ProcessSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("ModoDebug", IsRequired = true)]
        public bool ModoDebug
        {
            get { return (bool)this["ModoDebug"]; }
            set { this["ModoDebug"] = value; }
        }


        //[ConfigurationProperty("NotificationError", IsRequired = true)]
        //public bool NotificationError
        //{
        //    get { return (bool)this["NotificationError"]; }
        //    set { this["NotificationError"] = value; }
        //}

        //[ConfigurationProperty("OnDemand", IsRequired = false, DefaultValue = false)]
        //public bool OnDemand
        //{
        //    get { return (bool)this["OnDemand"]; }
        //    set { this["OnDemand"] = value; }
        //}

        [ConfigurationProperty("SuccesAddressBook", IsRequired = false, DefaultValue = "")]
        public string SuccesAddressBook
        {
            get { return (string)this["SuccesAddressBook"]; }
            set { this["SuccesAddressBook"] = value; }
        }


        [ConfigurationProperty("FailAddressBook", IsRequired = false, DefaultValue = "")]
        public string FailAddressBook
        {
            get { return (string)this["FailAddressBook"]; }
            set { this["FailAddressBook"] = value; }
        }

        /// <summary>
        /// Nombre del Proceso(en "ListProcesses") REGISTRADO que se ejecutara alterminar el proceso actual (opcional)
        /// </summary>
        [ConfigurationProperty("ProcessNameNext", IsRequired = false, DefaultValue = "")]
        public string ProcessNameNext
        {
            get { return (string)this["ProcessNameNext"]; }
            set { this["ProcessNameNext"] = value; }
        }
    }
}

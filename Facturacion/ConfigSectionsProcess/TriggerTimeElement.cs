using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSectionsProcess
{
    public class TriggerTimeElement : ConfigurationElement
    {
        [ConfigurationProperty("Hora", IsRequired = true)]
        public int Hora
        {
            get { return (int)this["Hora"]; }
            set { this["Hora"] = value; }
        }


        [ConfigurationProperty("Minuto", IsRequired = true)]
        public int Minuto
        {
            get { return (int)this["Minuto"]; }
            set { this["Minuto"] = value; }
        }


        [ConfigurationProperty("Dia", IsRequired = true, IsKey = true)]
        public string Dia
        {
            get { return (string)this["Dia"]; }
            set { this["Dia"] = value; }
        }
    }
}

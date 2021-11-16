using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSectionsProcess
{
    public class ProcessSettingsSection : ConfigurationSection
    {
        public ProcessSettingsSection()
        {
        }

        [ConfigurationProperty("Process", IsRequired =true)]
        public ProcessSettingsElement Process
        {
            get { return (ProcessSettingsElement)this["Process"]; }
        }

        [ConfigurationProperty("Parameters", IsRequired =false)]
        public ParameterCollection Parameters
        {
            get { return (ParameterCollection)this["Parameters"]; }
        }


        [ConfigurationProperty("TriggerTime", IsRequired = false)]
        public TriggerTimeCollection TriggerTime
        {
            get { return (TriggerTimeCollection)this["TriggerTime"]; }
        }


        public List<ParameterElement> GetListParameterElement()
        {
            var objparameters =
                   (
                       from ParameterElement method in this.Parameters
                       select method
                   );

            return objparameters.ToList();
        }


        public List<TriggerTimeElement> GetListTriggerTimeElement()
        {
            var objTriggers =
                    (
                        from TriggerTimeElement method in this.TriggerTime
                        select method
                    );

            return objTriggers.ToList();
        }
     
    }
}

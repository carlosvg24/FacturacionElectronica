using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSectionsProcess
{
    [ConfigurationCollection(typeof(TriggerTimeCollection), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class TriggerTimeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TriggerTimeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(" escribir error") { Source = "TaskCollection.cs" };
            }

            return ((TriggerTimeElement)element).Minuto;
        }

        public void Add(TriggerTimeElement svc)
        {
            base.BaseAdd(svc);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}

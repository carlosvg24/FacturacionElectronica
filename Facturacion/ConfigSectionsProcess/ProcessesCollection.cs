using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSectionsProcess
{

    [ConfigurationCollection(typeof(ProcessesCollection), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ProcessesCollection : ConfigurationElementCollection
    {        
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProcessElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(" escribir error") { Source = "TaskCollection.cs" };
            }

            return ((ProcessElement)element).Nombre;
        }

        public void Add(ProcessElement svc)
        {
            base.BaseAdd(svc);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}

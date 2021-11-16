using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSectionsProcess
{
    [ConfigurationCollection(typeof(ParameterCollection), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ParameterCollection : ConfigurationElementCollection
    {        
        protected override ConfigurationElement CreateNewElement()
        {
            return new ParameterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(" escribir error") { Source = "ParameterCollection.cs" };
            }

            return ((ParameterElement)element).DataBaseKey;
        }

        public void Add(ParameterElement svc)
        {
            base.BaseAdd(svc);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}

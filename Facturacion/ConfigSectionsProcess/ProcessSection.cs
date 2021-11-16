using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigSectionsProcess
{
    public class ProcessSection : ConfigurationSection
    {
        /// <summary>
        /// Obtiene Coleccion de GroupStoredProcedure
        /// </summary>
        [ConfigurationCollection(typeof(ProcessesCollection), AddItemName = "add")]
        [ConfigurationProperty("Processes")]
        public ProcessesCollection Tasks
        {
            get { return (ProcessesCollection)this["Processes"]; }
        }

        [ConfigurationProperty("Admin")]
        public AdministradorElement Administrador
        {
            get { return (AdministradorElement)this["Admin"]; }
        }


        public  List<ProcessElement> GetListProcessElement()
        {
            var Objgrupos =
                (
                    from ProcessElement method in this.Tasks
                    select method
                );

            return Objgrupos.ToList();
        }
    }
}

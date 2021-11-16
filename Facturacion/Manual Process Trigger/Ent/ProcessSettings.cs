using ConfigSectionsProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manual_Process_Trigger.Ent
{
    public class ProcessSettings
    {

        public ProcessSettings()
        {
            this.FailAddressBook = string.Empty;
            this.ModeDebug = false;
            this.NameCtrlsCreated = new List<string>();
            this.NameProcess = string.Empty;
            this.OnDemand = false;
            this.Parameters = new Dictionary<string, Dictionary<string, string>>();
            this.SuccesAddressBook = string.Empty;
            this.TypeName = string.Empty;
        }
        /// <summary>
        /// Nombre corto de la tarea
        /// </summary>
        public string NameProcess { get; set; }

        /// <summary>
        /// Parametros neesarios que se le pasaran al proceso
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Parameters { get; set; }

        /// <summary>
        /// Bandera que indica si corre bajo demanda
        /// </summary>
        public bool OnDemand { get; set; }

        /// <summary>
        /// Nombre completo para poder hacer instancia al proceso
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Bandera que indica si se activa el Modo Debug del proceso
        /// </summary>
        public bool ModeDebug { get; set; }

        /// <summary>
        /// Grupo de direcciones de correo que sera enviada cuandoel processo sea exitoso
        /// </summary>
        public string SuccesAddressBook { get; set; }

        /// <summary>
        /// Grupo de direcciones de correo que sera enviada cuando el processo falle
        /// </summary>
        public string FailAddressBook { get; set; }

        /// <summary>
        /// Nombre de los controles creados anterioirmente para introducir los parametros ondemand
        /// </summary>
        public List<string> NameCtrlsCreated { get; set; }
    }
}

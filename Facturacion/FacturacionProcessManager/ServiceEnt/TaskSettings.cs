
using ConfigSectionsProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacturacionProcessManager.ServiceEnt
{
    /// <summary>
    /// Clase que contiene todos los settings necesarios para correr la tarea
    /// </summary>
    public class ProcessSettings
    {
        /// <summary>
        /// Nombre corto de la tarea
        /// </summary>
        public string NameProcess { get; set; }

        /// <summary>
        /// Tiemposde ejecucion
        /// </summary>
        public List<DateTime> TimeExecute { get; set; }

        /// <summary>
        /// Parametros neesarios que se le pasaran al proceso
        /// </summary>
        public List<ParameterElement> Parameters { get; set; }

        ///// <summary>
        ///// Informacion del Admin que se encarga del Task Manager
        ///// </summary>
        //public AdministradorElement Administrador { get; set; }

        /// <summary>
        /// Bandera que indica si corre bajo demanda
        /// </summary>
        public bool OnDemand { get; set; }

        /// <summary>
        /// Nombre completo para poder hacer instancia al proceso
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Nombre del proceso a ejecutar una vez termine el principal
        /// </summary>
        public string ProcessNameNextExecute { get; set; }

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

    }
}


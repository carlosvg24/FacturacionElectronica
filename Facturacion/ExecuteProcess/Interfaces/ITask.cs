using ExecuteProcess.ServiceEnt;
using Process.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteProcess.Interfaces
{
    public delegate void ShowPorcentProgress(object source, ShowPorcentProgressEventArgs e);
    interface IProcess
    {
        event ShowPorcentProgress OnShowPorcentProgress;

        /// <summary>
        /// Parametros declarados en VBFac_Parametros_Cnf que utilara el proceso
        /// </summary>
        Dictionary<string, Dictionary<string, string>> Parametros { get; set; }

        /// <summary>
        /// Propiedad (regularmente utilizada en desarrollo) que dispara la tarea cada intervalo de tiempo programado
        /// </summary>
        bool OnDemand { get; set; }

        /// <summary>
        /// Propiedad que indica que el proceso correra en moddo debug
        /// </summary>
        bool ModeDebug { get; set; }        

        /// <summary>
        /// Validaciones  a consideracion del desarrollador para correr el proceso
        /// </summary>
        /// <returns></returns>
        ResponseTask ValidationsBeforeExecution();

        /// <summary>
        /// Proceso principal
        /// </summary>
        /// <returns></returns>
        ResponseTask MainProcess();
        
    }
}

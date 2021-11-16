using ExecuteProcess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExecuteProcess.ServiceEnt;
using MansLog.ConfigXML;
using MansLog.Error;
using System.Configuration;
using Facturacion.BLL.ProcesoFacturacion;
using Process.Entidades;

namespace ExecuteProcess.Process
{
    public class RecuperarEmailReservacion : IProcess
    {
        #region Propiedades Privadas

        private LogError log { get; set; }


        #endregion


        public bool ModeDebug
        {
            get;

            set;
        }

        public bool OnDemand
        {
            get;

            set;
        }

        public Dictionary<string, Dictionary<string, string>> Parametros { get; set; }

        public event ShowPorcentProgress OnShowPorcentProgress;

        public RecuperarEmailReservacion()
        {
            LogMansSection sectionLog = (LogMansSection)ConfigurationManager.GetSection("FacturacionServiceSettings/LogMans");

            this.log = new LogError(sectionLog, this.GetType().Name);
            this.log.StackTraceCompleto = true;
            this.log.TipoStackTrace = MansLog.EL.TypeStackTrace.Default;
        }

        public ResponseTask MainProcess()
        {
            ResponseTask response = new ResponseTask();

            try
            {
                log.escribir("Proceso Inicializado");

                
                DateTime fecha = (this.OnDemand) ? (DateTime.Parse(Parametros["ONDEMAND"]["DiaPagos"])) : (DateTime.Now.AddDays(-1));

                if (OnShowPorcentProgress != null)
                    OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs(10, 15, "Proceso Inicializado"));

                response = Recuperar(fecha);

                log.escribir("El proceso termino");
            }
            catch (Exception ex)
            {
                response.Succes = false;
                response.Message = string.Format("Ocurrio un error en el MainProces de RecuperarEmailReservacion: {0}", ex.Message);
                response.StackTrace = ex.StackTrace;

                log.escribir(ex);
            }


            return response;
        }

        private ResponseTask Recuperar(DateTime fecha)
        {
            ResponseTask result = new ResponseTask();


            BLLEmailsReservacion emails = new BLLEmailsReservacion();


            result.Succes = true;
            result.Message = string.Format("Se concluyo la recuperación de emails del dia {0:dd-MMM-yyyy}\n\r\n\rReservaciones procesadas: {1},\n\r\n\rReservaciones con error: {2}\n\r\n\rReservaciones OK: {3}", fecha);

            return result;
        }

        public ResponseTask ValidationsBeforeExecution()
        {
            ResponseTask t = new ResponseTask();

            try
            {
                t.Succes = true;
                log.escribir("Termina validacion");
            }
            catch (Exception ex)
            {
                t.Succes = false;
                t.Message = ex.Message;
                t.StackTrace = ex.StackTrace;
            }

            return t;
        }
    }
}

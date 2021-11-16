using ExecuteProcess.ServiceEnt;

using ExecuteProcess.Interfaces;
using MansLog.ConfigXML;
using MansLog.Error;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.BLL.ProcesoFacturacion;
using Process.Entidades;
using Facturacion.ENT;

namespace ExecuteProcess.Process
{
    public class DistribucionPagos : IProcess
    {
        #region Propiedades Privadas

        private LogError log { get; set; }


        #endregion        

        public event ShowPorcentProgress OnShowPorcentProgress;

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

        public DistribucionPagos()
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
                
                BLLProcesarPagos bllProcesar = new BLLProcesarPagos();

                if (OnShowPorcentProgress != null)
                    OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs(10,15, "Proceso Inicializado"));

                DateTime fecha = (this.OnDemand) ? (DateTime.Parse(Parametros["ONDEMAND"]["DiaPagos"])) : (DateTime.Now);

                List<ENTDistribucionpagos> pagosToDistribuirAll = bllProcesar.RecuperarPNRConPagosPorFecha(fecha, "R9HQNM");
                List<ENTDistribucionpagos> pagosToDistribuirAll_x2 = bllProcesar.RecuperarPNRConPagosPorFecha(fecha, "H4TETY");
                List<ENTDistribucionpagos> pagosToDistribuirAll_x3 = bllProcesar.RecuperarPNRConPagosPorFecha(fecha, "G686QF");
                List<ENTDistribucionpagos> pagosToDistribuirAll_x4 = bllProcesar.RecuperarPNRConPagosPorFecha(fecha, string.Empty);

                //List<ENTDistribucionpagos> pagosToDistribuirAll = bllProcesar.RecuperarPNRConPagosPorFecha(fecha);
                foreach (ENTDistribucionpagos x in pagosToDistribuirAll_x2)
                {
                    pagosToDistribuirAll.Add(x);
                }
                foreach (ENTDistribucionpagos x in pagosToDistribuirAll_x3)
                {
                    pagosToDistribuirAll.Add(x);
                }
                foreach (ENTDistribucionpagos x in pagosToDistribuirAll_x4)
                {
                    pagosToDistribuirAll.Add(x);
                }

                if (OnShowPorcentProgress != null)
                    OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs(15,25,string.Format( "Se obtivieron {0} PNR's/Pagos para distribuir",pagosToDistribuirAll.Count)));

                response=Distribuir(pagosToDistribuirAll, fecha, bllProcesar);

                log.escribir("El proceso termino");
            }
            catch(Exception ex)
            {
                response.Succes = false;
                response.Message =string.Format("Ocurrio un error en el MainProces de DistribucionPagos: {0}" ,ex.Message);
                response.StackTrace = ex.StackTrace;

                log.escribir(ex);
            }
            

            return response;
        }

        public ResponseTask ValidationsBeforeExecution()
        {
            ResponseTask t = new ResponseTask();

            try
            {               
                t.Succes = true;
                log.escribir("Termina validacion");
            }
            catch(Exception ex)
            {
                t.Succes = false;
                t.Message = ex.Message;
                t.StackTrace = ex.StackTrace;
            }

            return t;
        }

        private ResponseTask Distribuir(List<ENTDistribucionpagos> pagosToDistribuirAll, DateTime fecha,BLLProcesarPagos bllProcesar)
        {//result = string.Format("true|{0}|{1}|{2}", procesadas, conError, procesadasOK,fecha);
            List<string> listResponse = new List<string>();
            int multiplo = pagosToDistribuirAll.Count / 10, inicio = 0, erroneas=0,correctas=0, procesadas=0, contador=0,porcentAnterior=25;

            ResponseTask result = new ResponseTask();

            List<ENTDistribucionpagos> pagosToDistribuir = new List<ENTDistribucionpagos>();

            for (int i = 1; i <= 10; i++)
            {
                contador = (i != 10) ? (multiplo) : (pagosToDistribuirAll.Count - 1 - inicio);
                pagosToDistribuir = pagosToDistribuirAll.GetRange(inicio, contador);

                listResponse = bllProcesar.EjecutarDistribucionPagos(
                    pagosToDistribuir,fecha                    
                ).Split('|').ToList();

                inicio = (inicio + multiplo);

                if (bool.Parse(listResponse[0]) == true)
                {
                    procesadas = procesadas + int.Parse(listResponse[1]);
                    erroneas = erroneas + int.Parse(listResponse[2]);
                    correctas = correctas + int.Parse(listResponse[3]);

                    if (OnShowPorcentProgress != null)
                    {
                        OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs(porcentAnterior, porcentAnterior + 7 , string.Format("Se han distribuido {0} pagos", procesadas)));
                        porcentAnterior = porcentAnterior + 7;
                    }
                }
                else
                {
                    result.Succes = false;
                    result.Message= string.Format("No se pudo distribuir del index {0} al {1} por lo siguiente:\n\r - {2}\n\r", inicio, contador, listResponse[1]);
                    result.StackTrace = listResponse[2];

                    return result;
                }
            }

            result.Succes = true;
            result.Message = string.Format("Se concluyo la distribución de pagos del dia {0:dd-MMM-yyyy}\n\r\n\rReservaciones procesadas: {1},\n\r\n\rReservaciones con error: {2}\n\r\n\rReservaciones OK: {3}", fecha, procesadas ,erroneas, correctas);

            return result;
        }
    }
}

using MansLog.Error;
using ExecuteProcess.Interfaces;
using ExecuteProcess.ServiceEnt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteProcess
{
    /// <summary>
    /// Evento que se dispara al finalizar el proceso
    /// </summary>
    public delegate void FinishExecutionHandler(object source,FinishEventArgs e);

    /// <summary>
    /// Evento que se disparacada vez que la clase proceso logea
    /// </summary>    
    public delegate void WritedLogHandler(object source, WritedLogEventArgs e);

    /// <summary>
    /// Evento que se dispara cuando la clase del proceso es instanceada
    /// </summary>
    public delegate void ValidationProcess(object souurce,ValidationProcessEventArgs e);

    /// <summary>
    /// Evento que se disara antes decorrer el MainProcess del proceso
    /// </summary>
    /// <param name="e"></param>
    public delegate void BeforeRun(object souurce, EventArgs e);

    public delegate void ShowProgressProcess(object source,ShowProgressProcessEventArgs e);

    public class FinishEventArgs : EventArgs
    {
        public ResponseTask Response { get; private set; }

        public string NombreProceso { get; private set; }

        public string Instancia { get; private set; }

        public FinishEventArgs(ResponseTask response,string nombreProceso,string instancia)
        {
            this.Response = response;
            this.NombreProceso = nombreProceso;
            this.Instancia = instancia;
        }
    }
    
    public class WritedLogEventArgs:EventArgs
    {
        public string Log { get; set; }
    }

    public class ValidationProcessEventArgs: EventArgs
    {
        ResponseTask Response { get; set; }        

        public ValidationProcessEventArgs(ResponseTask response)
        {
            this.Response = response;
        }        
    }

    public class ShowProgressProcessEventArgs:EventArgs
    {
        public int NewPorcentaje { get; set; }

        public int OldPorcentaje { get; set; }

        public string Mensaje { get; set; }

        public ShowProgressProcessEventArgs(int oldPorcentaje,int newPorcentaje,string mensaje)
        {
            this.OldPorcentaje = oldPorcentaje;
            this.NewPorcentaje = newPorcentaje;
            this.Mensaje = mensaje;
        }
    }

    public class Proceso
    {
        public event FinishExecutionHandler OnFinishExecution;
        public event WritedLogHandler OnWritedLog;
        public event ValidationProcess OnValidationProcess;
        public event BeforeRun OnBeforeRun;
        public event ShowProgressProcess OnShowProgressProcess;   

        #region Variables Privadas

        private string _ReerenciaCompleta;

        private string _NombreProceso;

        #endregion

        #region Propiedades Publicas

        /// <summary>
        /// Des/Activar elondemand en el proceso  a ejecutar.
        /// </summary>
        public bool ProcessOnDemand { get; set; }

        /// <summary>
        /// Des/Activar el Modo debug en elprocesoa ejecutar
        /// </summary>
        public bool ProcessModoDebug { get; set; }

        /// <summary>
        /// Nombre instancia completa dela clase 
        /// </summary>
        public string ReferenciaCompleta
        {
            get
            {
                return this._ReerenciaCompleta;
            }
        }

        /// <summary>
        /// Nombre corto del proceso
        /// </summary>
        public string NombreProceso
        {
            get
            {
                return this._NombreProceso;
            }
        }

        /// <summary>
        /// Objeto Log para poder logear con la mismainstancia desde donde se llama la clase (opcional)
        /// </summary>
        public LogError Log { get; set; }

        /// <summary>
        /// Activa/Des escritura a logdelas actividades que realiza esta clase
        /// </summary>
        public bool WriteLog { get; set; }

        #endregion


        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="referenciaCompleta">Nombre completo pcon el que se hace referencia a la tarea para poder hacer instancia</param>
        /// <param name="nombreProceso">Nombre del proceso</param>
        public Proceso(string referenciaCompleta, string nombreProceso)
        {
            this.ProcessOnDemand = false;
            this.ProcessModoDebug = false;
            this._NombreProceso = nombreProceso;
            this._ReerenciaCompleta = referenciaCompleta;
            this.WriteLog = false;
        }

        #endregion

        /// <summary>
        /// Se encarga de instanciar, validar y correr el proceso principal de la tarea
        /// </summary>        
        public ResponseTask Run(Dictionary<string, Dictionary<string, string>> parametrosProceso)
        {
            string txtError = string.Empty;
            ResponseTask response = new ResponseTask();

            try
            {
                Type type = Type.GetType(this.ReferenciaCompleta);

                if (type.GetInterfaces().Contains(typeof(IProcess)))
                {
                    MethodInfo methodInfoValidation = type.GetMethod("ValidationsBeforeExecution");
                    MethodInfo methodInfoMainProcess = type.GetMethod("MainProcess");

                    if (methodInfoMainProcess != null && methodInfoValidation != null)
                    {
                        object classInstance = Activator.CreateInstance(type, null);
                        WriteBitacora(string.Format("Se crea instacia de {0}", this.ReferenciaCompleta));

                        ((IProcess)classInstance).OnShowPorcentProgress += Proceso_OnShowPorcentProgress;

                        try
                        {
                            response = ((ResponseTask)methodInfoValidation.Invoke(classInstance, new object[] { }));
                        }
                        catch (Exception ex)
                        {
                            WriteBitacora(string.Format("Error no controlado en las validaciones de la tarea {0} favor de validar control de errores: {1}", this.NombreProceso, ex.Message));
                            response.Message = string.Format("Error no controlado en las validaciones de la tarea {0} favor de validar control de errores: {1}", this.NombreProceso, ex.Message);
                            response.StackTrace = ex.StackTrace;
                            response.Succes = false;
                        }

                        ValidationProcessEventArgs instanceArgs = new ValidationProcessEventArgs( response);

                        if(OnValidationProcess != null)
                            OnValidationProcess(this, instanceArgs);

                        if (response.Succes)
                        {
                            WriteBitacora(string.Format("El proceso {0} responde satisfactoriamente que realizo las validaciones pertinentes para correr", this.ReferenciaCompleta));

                            PropertyInfo propertyOndDemand = classInstance.GetType().GetProperty("OnDemand");
                            PropertyInfo propertyParametros = classInstance.GetType().GetProperty("Parametros");
                            PropertyInfo propertyModeDebug = classInstance.GetType().GetProperty("ModeDebug");

                            if (propertyOndDemand != null && propertyParametros != null)
                            {
                                propertyOndDemand.SetValue(classInstance, this.ProcessOnDemand);
                                propertyParametros.SetValue(classInstance, parametrosProceso);
                                propertyModeDebug.SetValue(classInstance, this.ProcessModoDebug);

                                ResponseTask result = null;
                                //ParameterInfo[] parameters = methodInfo.GetParameters();
                                //object[] parametersArray = new object[] { true };                               

                                if (OnBeforeRun != null)
                                    OnBeforeRun(this, new EventArgs());

                                WriteBitacora(string.Format("El proceso {0} esta corriendo", this.ReferenciaCompleta));

                                try
                                {                                    
                                    result = (ResponseTask)methodInfoMainProcess.Invoke(classInstance, new object[] { });
                                }
                                catch (Exception ex)
                                {
                                    result = new ResponseTask();
                                    WriteBitacora(string.Format("Error no controlado en el proceso {0} favor de validar control de errores: {1}", this.NombreProceso, ex.Message));
                                    result.Message = string.Format("Error no controlado en el proceso {0} favor de validar control de errores: {1}", this.NombreProceso, ex.Message);
                                    result.Succes = false;
                                    result.StackTrace = ex.StackTrace;
                                }
                                finally
                                {
                                    WriteBitacora(string.Format("El proceso {0} ha finalizado", this.ReferenciaCompleta));
                                }

                                //SendCorreoFinishTask(result, this.NombreProceso, this.ReferenciaCompleta, succesGrupoCorreos, failGrupoCorreos);
                                
                                FinishEventArgs args2 = new FinishEventArgs(result, this.NombreProceso, this.ReferenciaCompleta);

                                OnFinishExecution(this, args2);

                                WriteBitacora(string.Format("El proceso {0} ha enviado correo", this.ReferenciaCompleta));

                                return result;
                            }
                        }
                        else
                            txtError = string.Format("Se cancela el proceso {0}  por que este marco un error en sus validaciones:\n\r - {1}", this.ReferenciaCompleta, response.Message);
                    }
                    else
                        txtError = string.Format("Se cancela el proceso {0}  por que no contiene alguno de los metodos \"ValidationsBeforeExecution\" y/o \"MainProcess\"", this.ReferenciaCompleta);
                }
                else
                    txtError = string.Format("Se cancela el proceso {0}  por que la tarea no contiene la Interface \"ITask\"", this.ReferenciaCompleta);

                WriteBitacora(txtError);
                //SendCorreo(txtError, string.Format("Facturacion Process Manager no corrio a {0}", this.NombreProceso), failGrupoCorreos.Split(',').ToList(), "FacturacionNoReply", "Facturacion Task Manager", true);
                response = new ResponseTask() { Message = txtError, Result = string.Empty, StackTrace = string.Empty, Succes = false };
                FinishEventArgs args = new FinishEventArgs(
                    response,
                    this.NombreProceso,
                    this.ReferenciaCompleta);

                OnFinishExecution(this, args);

                return response;
            }
            catch (Exception ex)
            {
                var txt = string.Format("Facturacion Process Manager tuvo un error al tratar de correr {0}", this.NombreProceso);
                WriteBitacora(txt);
                Log.escribir(ex, txt);
                throw ex;
                //try
                //{
                //    SendCorreo(
                //        string.Format("{0}\n\r  - Mensaje: {1}\n\r  - StackTrace: {2}", txt, ex.Message, ex.StackTrace),
                //        "Facturacion Process Manager Error",
                //        new List<string>(),
                //        "FacturacionNoReply",
                //        "Facturacion Process Manager",
                //        true);
                //}
                //catch (Exception) { };
            }
        }

        private void Proceso_OnShowPorcentProgress(object source, global::Process.Entidades.ShowPorcentProgressEventArgs e)
        {
            if (OnShowProgressProcess != null)
                OnShowProgressProcess(this, new ShowProgressProcessEventArgs(e.OldPorcent,e.NewPorcent, e.Message));
        }


        /// <summary>
        /// Escribe en bitacora
        /// </summary>
        /// <param name="txt">texto a escribir</param>        
        private void WriteBitacora(string txt)
        {
            WritedLogEventArgs args = new WritedLogEventArgs() { Log = txt };

            if(this.OnWritedLog!=null)
                OnWritedLog(this, args);

            if (this.WriteLog && txt != string.Empty)
            {
                if (this.Log != null)
                {
                    try
                    {
                        Log.escribir(txt);
                    }   
                    catch (Exception)
                    {
                        this.ProcessModoDebug = false;
                    }
                }
            }

            Console.WriteLine("{0:dd-MM-yyyy}   {0:HH:mm:ss.ffff} : {1}", DateTime.Now, txt);
        }

       
    }
}

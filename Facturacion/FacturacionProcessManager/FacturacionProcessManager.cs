using Facturacion.BLL.FacturaGlobal;
using Facturacion.ENT;
using FacturacionProcessManager.ServiceEnt;
using MansLog.ConfigXML;
using MansLog.Error;
using MetodosComunes;
using MetodosComunes.ToolsFnWebConfig;
using ExecuteProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using ExecuteProcess.ServiceEnt;
using System.Net;
using ConfigSectionsProcess;
using MetodosComunes.Entity;

namespace FacturacionProcessManager
{
    public partial class FacturacionProcessManager : ServiceBase
    {
        #region VAriables Privadas

        /// <summary>
        /// Tiempo el cual se va ir disparando elevento que va ir checando que tarea disparar
        /// </summary>
        System.Timers.Timer Temporizador;

        /// <summary>
        /// Indica si na tarea esta en ejecucion
        /// </summary>
        bool isExecuting = false;

        /// <summary>
        /// Modo donde se logea mas datos para ayudar a la deteccion de errores
        /// </summary>
        bool isModoDebug = false;

        /// <summary>
        /// Nombre de la tarea que se esta ejecutando
        /// </summary>
        string nameProcessExecuting = string.Empty;

        /// <summary>
        /// Objeto donde se almaceneran los settings de cada proceso que estan en el config 
        /// </summary>
        List<ProcessSettings> procesosSettings = new List<ProcessSettings>();

        #endregion


        #region Propiedades Privadas

        private LogError Log { get; set; }


        private string IP { get; set; }


        private string HostName { get; set; }


        #endregion

        #region Constructor

        public FacturacionProcessManager()
        {
            LogMansSection sectionLog = (LogMansSection)ConfigurationManager.GetSection("FacturacionServiceSettings/LogMans");

            SetHostNameIP();

            this.Log = new LogError(sectionLog, this.GetType().Name);
            this.Log.StackTraceCompleto = true;
            this.Log.TipoStackTrace = MansLog.EL.TypeStackTrace.Default;

            InitializeComponent();
            Temporizador = new System.Timers.Timer(60000);
            Temporizador.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            Temporizador.AutoReset = true;

            components = new System.ComponentModel.Container();
            this.ServiceName = "VivaFacturacionProcessManager";

            bool.TryParse(ConfigurationManager.AppSettings["ModoDebug"], out this.isModoDebug);
            WriteBitacora(string.Format("El Servico Task Manager de Facturacion se ha instanciado"), false);
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento en donde se evalua si corre o no alguna tarea.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            WriteBitacora((this.isExecuting) ? (string.Empty) : ("Se dispara evento OnTimedEvent"), true);
            
            try
            {
                //var t = typeof(ConfigSections.TaskSettingsSection).FullName;
                List<ProcessElement> procesos = GetProcesos();
                WriteBitacora(string.Format((this.isExecuting) ? (string.Empty) : ("Se obtuvieron {0} tarea(s)"), procesos.Count()), true);

                procesosSettings = GetTaskSettings(procesos);
                WriteBitacora(string.Format((this.isExecuting) ? (string.Empty) : ("Se obtuvieron {0} objeto(s) de settings"), procesos.Count()), true);

                DateTime now = DateTime.Now;

                foreach (ProcessSettings setting in procesosSettings)
                {
                    foreach (DateTime tiempoEjecucion in setting.TimeExecute)
                    {
                        if (setting.OnDemand)
                        {
                            if (isExecuting == false)
                            {
                                this.isExecuting = true;

                                WriteBitacora(string.Format("El proceso {0} es autorizado por \"OnDemand\" para intentar correr en {1} ({2})", setting.TypeName,this.HostName,this.IP), false);

                                bool runAgain = true;
                                var settingTemp = setting;

                                while (runAgain)
                                {
                                    runAgain = RunProcess(settingTemp);

                                    settingTemp = (runAgain) ? (procesosSettings.Find(f => f.NameProcess.ToLower() == setting.ProcessNameNextExecute.ToLower())) : (new ProcessSettings());
                                }
                            }
                            else
                                WriteBitacora(string.Format("La tarea {0} no pudo ser procesada en {2} ya que el proceso {1} esta actualmente corriendo", setting.NameProcess, this.nameProcessExecuting,this.HostName), false);

                            break;
                        }
                        else if (tiempoEjecucion.Day == now.Day && tiempoEjecucion.Month == now.Month && tiempoEjecucion.Year == now.Year && tiempoEjecucion.Hour == now.Hour && tiempoEjecucion.Minute == now.Minute)
                        {
                            if (isExecuting == false)
                            {
                                this.isExecuting = true;

                                WriteBitacora(string.Format("El proceso {0} por horario es autorizado  para intentar correr en {1} ({2})", setting.TypeName,this.HostName,this.IP), false);

                                bool runAgain = true;
                                var settingTemp = setting;

                                while(runAgain)
                                {
                                    runAgain=RunProcess(settingTemp);

                                    settingTemp = (runAgain) ? (procesosSettings.Find(f => f.NameProcess.ToLower() == setting.ProcessNameNextExecute.ToLower())) : (new ProcessSettings());
                                }
                            }
                            else
                            {
                                WriteBitacora(string.Format("La tarea {0} no pudo ser procesada ya que el proceso {1} esta actualmente corriendo", setting.NameProcess, this.nameProcessExecuting), false);
                                SendCorreo(
                                    string.Format("La tarea {0} no pudo ser procesada ya que el proceso {1} esta actualmente corriendo, favor de checar los horarios en {2}", setting.NameProcess, this.nameProcessExecuting,this.HostName),
                                    "Facturacion Process Manager con tareas encimadas",
                                    new List<string>(),
                                    "FacturacionNoReply",
                                    "Facturacion Process Manager",
                                    true
                                    );
                            }

                            break;
                        }
                        else ///if (isExecuting)
                        {
                            WriteBitacora(string.Format("Ningun proceso para correr a las {0:dd-MM-yyyy} a {0:HH:mm} en {1}",DateTime.Now,this.HostName), true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var txt = string.Format("Facturacion Process Manager em {0} ({1}) tuvo un error en \"OnTimedEvent\"",this.HostName,this.IP);

                Log.escribir(ex, txt);

                try
                {
                    SendCorreo(string.Format("{0}\n\r  - Mensaje: {1}\n\r  - StackTrace: {2}", txt, ex.Message, ex.StackTrace), "Facturacion Process Manager Error", new List<string>(), "FacturacionNoReply", "Facturacion Process Manager", true);
                }
                catch (Exception) { };
            }
        }

#if (!DEBUG)
        protected override void OnStart(string[] args)
        {
#else
        public new void OnStart(string[] args)
        {
#endif

#if DEBUG
            System.Threading.Thread.Sleep(3000);
#endif
            while (DateTime.Now.Second != 0)
            {
            }            

            WriteBitacora(string.Format("El Servico Facturacion Process Manager ah inicializado en el server {0}({1})",this.HostName,this.IP), false);

            Temporizador.Start();

            try
            {
                SendCorreo(string.Format("El servicio Facturacion Process Manager ah iniciado en el server {1}({2}) a las {0:HH:mm:ss} del dia {0:dd-MMM-yyyy}",DateTime.Now,this.HostName,this.IP), "Facturacion Process Manager", new List<string>(), "FacturacionNoReply", "Facturacion Process Manager", true);
            }
            catch (Exception ex) { };
        }

        protected override void OnStop()
        {
            WriteBitacora(string.Format("El Servico Task Manager de Facturacion se ah detenido en {0} ({1})",this.HostName,this.IP), false);

            try
            {
                SendCorreo(string.Format("El servicio Facturacion Process Manager fue detenido en el server {1}({2}) a las {0:HH:mm:ss} del dia {0:dd-MMM-yyyy}", DateTime.Now,this.HostName,this.IP), "Facturacion Process Manager", new List<string>(), "FacturacionNoReply", "Facturacion Process Manager", true);
            }
            catch (Exception) { };
        }

        private void Process_OnFinishExecution(object source, FinishEventArgs e)
        {
            var setting = procesosSettings.Find(f => f.NameProcess.ToLower() == e.NombreProceso.ToLower());

            if (e.Response.Succes)
            {
                SendCorreoFinishTask(e.Response, e.NombreProceso, e.Instancia, setting.SuccesAddressBook, setting.FailAddressBook);
            }
            else
            {
                SendCorreo(e.Response.Message, string.Format("Facturacion Process Manager no corrio a {0}", e.NombreProceso), setting.FailAddressBook.Split(',').ToList(), "FacturacionNoReply", "Facturacion Process Manager", true);
            }
        }


        #endregion

        #region Metodos Privados

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="settingNextProcess"></param>
        /// <returns></returns>
        private bool RunProcess(ProcessSettings setting/*, ProcessSettings settingNextProcess*/)
        {
            this.nameProcessExecuting = setting.NameProcess;

            try
            {
                ExecuteProcess.Proceso process = new ExecuteProcess.Proceso(setting.TypeName, setting.NameProcess);
                process.Log = this.Log;
                process.ProcessModoDebug = setting.ModeDebug;
                process.ProcessOnDemand = false;//setting.OnDemand;
                process.OnFinishExecution += Process_OnFinishExecution;
                //process.OnWritedLog += Process_OnWritedLog;
                process.WriteLog = this.isModoDebug;

                WriteBitacora(string.Format("Se crea instancia de la clase Proceso"), true);

                var response=process.Run(GetProcessParameters(setting.Parameters));//,setting.SuccesAddressBook,setting.FailAddressBook

                if (setting.ProcessNameNextExecute != string.Empty && response.Succes)
                {                   
                    return true;
                }

                this.isExecuting = false;
                this.nameProcessExecuting = string.Empty;
            }
            catch(Exception ex)
            {
                this.isExecuting = false;
                this.nameProcessExecuting =string.Empty;
                WriteBitacora(string.Format("Sucedio un error al tratar de correr el proceso {0}: \r\n  - Error: {1}\r\n  - ST: {2}", setting.NameProcess, ex.Message, ex.StackTrace),false);
                SendCorreo(string.Format("Sucedio un error al tratar de correr el proceso {0}: \r\n  - Error: {1}\r\n  - ST: {2}", setting.NameProcess, ex.Message, ex.StackTrace), "Error Facturacion Process Manager", new List<string>(), "FacturacionNoReply", "Facturacion Process Manager", true);
            }

            return false;
        }

        


        /// <summary>
        /// Obtiene las Tareas que estan supuestamente configuradas para ser procesadas
        /// </summary>
        /// <returns></returns>
        private List<ProcessElement> GetProcesos()
        {
            object seccion = ConfigurationManager.GetSection("FacturacionServiceSettings/ListProcesses");
            ProcessSection _xml = (ProcessSection)seccion;

            return _xml.GetListProcessElement();
        }

        /// <summary>
        /// Obtiene los datos del administrador del Administradodr de tareas
        /// </summary>
        /// <returns></returns>
        private AdministradorElement GetAdministrador()
        {
            object seccion = ConfigurationManager.GetSection("FacturacionServiceSettings/ListProcesses");
            ProcessSection _xml = (ProcessSection)seccion;

            return _xml.Administrador;
        }

        /// <summary>
        /// Obtiene todas las configuraciones necesarias para cada tarea
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        private List<ProcessSettings> GetTaskSettings(List<ProcessElement> tasks)
        {
            List<ProcessSettings> tareasOpciones = new List<ProcessSettings>();

            foreach (ProcessElement task in tasks)
            {
                List<DateTime> timesExecute = new List<DateTime>();
                System.Configuration.ConfigurationManager.RefreshSection(string.Format("{0}/Settings", task.Nombre));
                object seccion = System.Configuration.ConfigurationManager.GetSection(string.Format("{0}/Settings", task.Nombre));
                ProcessSettingsSection xml = (ProcessSettingsSection)seccion;
                ProcessSettings taskSetting = new ProcessSettings();
 

                foreach (TriggerTimeElement trigger in xml.GetListTriggerTimeElement())
                {
                    Regex soloNumeros = new Regex(@"^[+-]?\d+(\,\d+)?$"), soloLetras = new Regex("^[A-Z a-z ,]*$");

                    if (soloNumeros.IsMatch(trigger.Dia))
                        timesExecute.Add(new DateTime(DateTime.Today.Year, DateTime.Today.Month, int.Parse(trigger.Dia), trigger.Hora, trigger.Minuto, 0));
                    else if (DateTime.Today.DayOfWeek == (DayOfWeek)Enum.Parse(typeof(DayOfWeek), trigger.Dia))
                        timesExecute.Add(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, trigger.Hora, trigger.Minuto, 0));
                }

                taskSetting.TimeExecute = timesExecute;
                taskSetting.NameProcess = task.Nombre;
                taskSetting.TypeName = task.TypeName;
                taskSetting.Parameters = xml.GetListParameterElement();
                taskSetting.OnDemand = false;//xml.Process.OnDemand;
                taskSetting.ProcessNameNextExecute = xml.Process.ProcessNameNext;
                taskSetting.ModeDebug = xml.Process.ModoDebug;
                taskSetting.SuccesAddressBook = xml.Process.SuccesAddressBook;
                taskSetting.FailAddressBook = xml.Process.FailAddressBook;

                tareasOpciones.Add(taskSetting);
            }

            return tareasOpciones;
        }

        /// <summary>
        /// Obtiene parametros configurados en web.congif para obtenerlos en BD
        /// </summary>
        /// <param name="parametersInfo"></param>
        /// <returns>Parametros</returns>
        private Dictionary<string, Dictionary<string, string>> GetProcessParameters(List<ParameterElement> parametersInfo)
        {
            Dictionary<string, Dictionary<string, string>> parametros = new Dictionary<string, Dictionary<string, string>>();

            try
            {
                List<ENTParametrosCnf> allParametrosBD = BLLFacturaGlobal.GETStringsConfigurationAndParams();               

                foreach (ParameterElement param in parametersInfo.FindAll(f => f.IsString == true).ToList())
                {//add validacion q exista databasekey por q si no truena
                    parametros.Add(param.Name.ToUpper(), FnComunes.CadenaConexionToDictionary(allParametrosBD.Find(f => f.Nombre.ToLower() == param.DataBaseKey.ToLower()).Valor, null));
                }

                var distinct = parametersInfo.FindAll(f => f.IsString == false).ToList().GroupBy(car => car.Name).ToList();

                foreach (var d in distinct)
                {
                    Dictionary<string, string> key2 = new Dictionary<string, string>();

                    foreach (ParameterElement param in parametersInfo.FindAll(f => f.IsString == false).FindAll(f => f.Name == d.Key).ToList())
                    {
                        key2.Add(param.DataBaseKey, allParametrosBD.Find(f => f.Nombre.ToLower() == param.DataBaseKey.ToLower()).Valor);
                    }

                    parametros.Add(d.Key.ToUpper(), key2);
                }
            }
            catch(SqlException ex)
            {
                throw new Exception(string.Format("Ocurrio un error al tratar de obtener de la BD los Parametros del proceso {2}: {0}\r\n Stack Trace: {1}", ex.Message, ex.StackTrace));
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("Ocurrio un error al tratar de formatear los Parametro del Proceso {2}: {0}\r\n Stack Trace: {1}", ex.Message, ex.StackTrace));
            }

            return parametros;
        }

        /// <summary>
        /// Escribe en bitacora
        /// </summary>
        /// <param name="txt">texto a escribir</param>
        /// <param name="onlyDebug">Solo escrbe si el modo debug esta activado</param>
        private void WriteBitacora(string txt, bool onlyDebug)
        {
            try
            {
                if (onlyDebug && txt != string.Empty)
                {
                    if (this.isModoDebug)
                    {
                        Log.escribir(txt);
                        Console.WriteLine("{0:dd-MM-yyyy}   {0:HH:mm:ss.ffff} : {1}", DateTime.Now, txt);
                    }
                }
                else if (txt != string.Empty)
                {
                    Log.escribir(txt);
                    Console.WriteLine("{0:dd-MM-yyyy}   {0:HH:mm:ss.ffff} : {1}", DateTime.Now, txt);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("{0:dd-MM-yyyy}   {0:HH:mm:ss.ffff} : {1}", DateTime.Now, string.Format("No se pudo escribir en el log por lo siguiente: {0}",ex.Message));
            }
        }

        /// <summary>
        /// Envia correo
        /// </summary>
        /// <param name="response">response de la tarea ejecutada</param>
        /// <param name="nameTask">nombre de la tarea</param>
        /// <param name="typeName">Nombre/Type de tarea que se utiliza para poder hacer intancia</param>
        private void SendCorreoFinishTask(ResponseTask response, string nameTask, string typeName,string succesGrupoCorreo, string failGrupoCorreo)
        {
            AdministradorElement admin = GetAdministrador();
            string txt = string.Empty;

            if (response.Succes)
            {
                txt = string.Format("Hola {0}\n\r\n\rLa tarea \"{1}\" fue terminada correctamente el {2:dd-MMM-yyyy} a las {2:HH:mm} hrs en el server {3}({4})",
                    (DateTime.Now.Hour > 5 && DateTime.Now.Hour < 12) ? ("Buenos DIas") : ((DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 19) ? ("Buenas Tardes") : ("Buenas Noches")),
                    typeName,
                    DateTime.Now,
                    this.HostName,
                    this.IP
                );

                if (response.Message != string.Empty)
                {
                    txt = string.Format("{0}\n\r\n\rMensaje del Proceso\n\r\n\r{1}", txt, response.Message);
                }

                txt = string.Format("{0}\n\rSaludos!!\n\r\n\rNota:Mensaje enviado desde el \"Adminstrador de Tareas de Facturacion\" favor de no contestar cualquier duda favor de mandar correo a {1}", txt, admin.Correo);

                SendCorreo(txt, string.Format("{0} corrio exitosamente", nameTask), succesGrupoCorreo.Split(',').ToList(), "FacturacionNoReply", "Facturacion Task Manager",true);
            }
            else
            {
                txt = string.Format("Hola {0}\n\r\n\rLa tarea \"{1}\" fue ejecutada con errores en el server {2}({3}), favor de validar dicho proceso",
                    (DateTime.Now.Hour > 5 && DateTime.Now.Hour < 12) ? ("Buenos DIas") : ((DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 19) ? ("Buenas Tardes") : ("Buenas Noches")),
                    typeName,
                    this.HostName,
                    this.IP);

                if (response.Message != string.Empty)
                {
                    txt = string.Format("{0}\n\r\n\rMENSAJE  ERROR\n\r\n\r{1}", txt, response.Message);
                }

                if (response.StackTrace != string.Empty)
                {
                    txt = string.Format("{0}\n\r\n\rSTACKTRACE\n\r\n\r{1}", txt, response.StackTrace);
                }

                txt = string.Format("{0}\n\rSaludos!!\n\r\n\rNota:Mensaje enviado desde el \"Adminstrador de Tareas de Facturacion\" favor de no contestar cualquier duda favor de mandar correo a {1}", txt, admin.Correo);

                SendCorreo(txt, string.Format("{0} corrio con Errores", nameTask), failGrupoCorreo.Split(',').ToList(), "FacturacionNoReply", "Facturacion Task Manager",true);
            }
        }

        /// <summary>
        /// Envia correo
        /// </summary>
        /// <param name="message">mensaje correo</param>
        /// <param name="title">titulo correo</param>
        /// <param name="groupNameAddressee">grupos de destinatarios</param>
        /// <param name="idSettings">Id de settings del emisor configurados en web.config</param>
        /// <param name="nameEmisor">Nombre emisor</param>
        private void SendCorreo(string message, string title, List<string> groupNameAddressee, string idSettings, string nameEmisor,bool sendAdmin)
        {
            //Se obtiene xml de la seccion customizada
            object seccion = ConfigurationManager.GetSection("FacturacionServiceSettings/MetodosComunes");

            //se manda para q se formatee y se administre con a clase MailSection
            MailSection mailSectionManager = new MailSection(seccion);
            AddressBookSection bookAdressManager = new AddressBookSection(seccion);

            FnSendMail mail = new FnSendMail(mailSectionManager.GetMailPropertiesById(idSettings));

            List<EmailUser> correos = new List<EmailUser>();

            foreach (string grupo in groupNameAddressee)
            {
                correos.AddRange(bookAdressManager.GetMailsAdressByGroup(grupo));
            }

            if(sendAdmin)
            {
                var adminData = GetAdministrador();

                EmailUser admin = new EmailUser();
                admin.EmailAdress = adminData.Correo;
                admin.Nombre = string.Format("{0} {1}", adminData.Nombre, adminData.Apellido);
                admin.TipoDestinatario = (correos.FindAll(f=>f.TipoDestinatario==TypeAddressee.To).ToList().Count == 0) ? (TypeAddressee.To) : (TypeAddressee.BCC);

                correos.Add(admin);
            }

            mail.SendMail(correos, nameEmisor, message, title);
        }

        private void SetHostNameIP()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
            Regex ipRegex = new Regex(@"^\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}");

            try
            {
                for(int i=0;i<= ipHostInfo.AddressList.Count()-1;i++)
                {
                    if(ipRegex.IsMatch(ipHostInfo.AddressList[i].ToString()))
                    {
                        this.IP = ipHostInfo.AddressList[i].ToString();
                    }
                }
               
                this.HostName = ipHostInfo.HostName;
            }
            catch
            {
                this.IP = "IP NO DISPONIBLE";
            }
        }

        #endregion
    }
}

using ConfigSectionsProcess;
using ExecuteProcess;
using ExecuteProcess.ServiceEnt;
using Facturacion.BLL.FacturaGlobal;
using Facturacion.ENT;
using MansLog.ConfigXML;
using MansLog.Error;
using Manual_Process_Trigger.Ent;
using MetodosComunes;
using MetodosComunes.Entity;
using MetodosComunes.ToolsFnWebConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manual_Process_Trigger
{
   
    public partial class Main : Form
    {
        #region Variables Privadas

        private BackgroundWorker bgWorker;

        #endregion

        #region Propiedades Privadas

        private LogError Log { get; set; }

        private ComboBox comboProcesos { get; set; }

        private Button buttonProcess { get; set; }

        private ProcessSettings settings { get; set; }

        private string IP { get; set; }

        private string HostName { get; set; }

        #endregion

        delegate void DelegaDoTxtBox(string text);
        delegate void DelegadoCtrls(bool habilitar);


        public Main()
        {
            InitializeComponent();

            SetHostNameIP();

            this.KeyDown += Main_KeyDown;

            this.settings = new ProcessSettings();

            LogMansSection sectionLog = (LogMansSection)ConfigurationManager.GetSection("FacturacionServiceSettings/LogMans");

            this.Log = new LogError(sectionLog, "ManualProcessTrigger");
            this.Log.StackTraceCompleto = true;
            this.Log.TipoStackTrace = MansLog.EL.TypeStackTrace.Default;

            bgWorker = new BackgroundWorker();
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.ProgressChanged += BgWorker_ProgressChanged;
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;

            progressBar1.Visible = false;
            stripStatusLabel.Text = "Inicio";
        }


        #region Metodos Eventos
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode==Keys.F)
            {
                panelInicio.Controls.Clear();

                CargaComboProcesos();

                stripStatusLabel.Text = "Seleccion de Proceso";
            }
        }

        private void ComboProcesos_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (string name in settings.NameCtrlsCreated)
            {
                var ctrls = panelInicio.Controls.Find(name, false);

                if(ctrls.Count()==1)
                    panelInicio.Controls.Remove(ctrls[0]);
            }

            this.settings = new ProcessSettings();

            SetProcessSetting(comboProcesos.Text,comboProcesos.SelectedValue.ToString());

            CargaParametersInPut();

            CargaButtonRunProcess();

            stripStatusLabel.Text = "Asignacion de Parametros";
            progressBar1.Visible = false;
            progressBar1.Value = 0;
        }

        private void ButtonProcess_Click(object sender, EventArgs e)
        {
            TextBox cajaLog = new TextBox();
            stripStatusLabel.Text = "Proceso Corriendo";

            try
            {
                cajaLog = CargaTextBoxLog();

                List<string> keys = this.settings.Parameters["ONDEMAND"].Keys.ToList();

                for (int i = 0; i <= this.settings.Parameters["ONDEMAND"].Count - 1; i++)
                {
                    var ctrls = panelInicio.Controls.Find(string.Format("CajaParam{0}", i), false);

                    if (ctrls.Count() == 1)
                    {
                        TextBox temp = (TextBox)ctrls[0];
                        this.settings.Parameters["ONDEMAND"][keys[i]] = temp.Text;
                    }
                }

                HabilitarCtrls(false);
                progressBar1.Visible = true;
                cajaLog.Enabled = true;

                //RunProcess();
                bgWorker.RunWorkerAsync();
                
                cajaLog.Text += "Proceso Finalizado sin errores";                
            }
            catch (Exception ex)
            {
                cajaLog.Text = string.Format(" Error: {0}\r\n SP: {1}", ex.Message, ex.StackTrace);
            }
        }
        
        private void Process_OnWritedLog(object source, WritedLogEventArgs e)
        {
            DelegaDoTxtBox MD = new DelegaDoTxtBox(EscribirLogTxtBox);
            this.Invoke(MD, new object[] { e.Log });
        }

        private void Process_OnValidationProcess(object souurce, ValidationProcessEventArgs e)
        {
            GradualSetProgressBar(3, 6);
        }

        private void Process_OnBeforeRun(object souurce, EventArgs e)
        {
            GradualSetProgressBar(6, 10);
        }

        private void Process_OnFinishExecution(object source, FinishEventArgs e)
        {
            if (e.Response.Succes)
            {
                if(e.Response.Message != "FacturaGlobal33")
                    SendCorreoFinishTask(e.Response, e.NombreProceso, e.Instancia, settings.SuccesAddressBook, settings.FailAddressBook);
            }
            else
            {
                SendCorreo(string.Format("El proceso {0} notifico que no fue ejecutado correctamente en el server {2}({3}) por lo siguiente: \n\r{1}", e.NombreProceso, e.Response.Message, this.HostName, this.IP), string.Format("Facturacion Process Manager - {0} error", e.NombreProceso), settings.FailAddressBook.Split(',').ToList(), "FacturacionNoReply", "Facturacion Process Manager", true);
            }

            GradualSetProgressBar(95, 100);
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            RunProcess();
        }

        private void Process_OnShowProgressProcess(object source, ShowProgressProcessEventArgs e)
        {
            GradualSetProgressBar(e.OldPorcentaje, e.NewPorcentaje);
            DelegaDoTxtBox MD = new DelegaDoTxtBox(EscribirLogTxtBox);
            this.Invoke(MD, new object[] { e.Mensaje});
        }

        private void BgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = (e.ProgressPercentage > 100) ? (100) : (e.ProgressPercentage);             
        }


        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            HabilitarCtrls(true);
        }

        #endregion

        #region Metodos Privados

        private void PopulateComboProcess()
        {
            List<ProcessElement> procesos = new List<ProcessElement>();
            procesos.Add(new ProcessElement() { Nombre = " ..::  S e l e c c i o n e  ::.. ", TypeName = string.Empty });
            procesos.AddRange(GetProcesos());

            comboProcesos.DataSource = procesos;
            comboProcesos.DisplayMember = "Nombre";
            comboProcesos.ValueMember = "TypeName";            
        }

        private List<ProcessElement> GetProcesos()
        {
            object seccion = ConfigurationManager.GetSection("FacturacionServiceSettings/ListProcesses");
            ProcessSection _xml = (ProcessSection)seccion;

            return _xml.GetListProcessElement();
        }

        private void CargaComboProcesos()
        {
            Label labelCombo = new Label();
            labelCombo.Name = "EtiquetaCombo";
            labelCombo.Text = "Procesos";
            labelCombo.Font = new Font(new FontFamily("Consolas"), 12.25f, FontStyle.Bold);
            labelCombo.Location = new Point(40, 30);

            this.comboProcesos = new ComboBox();
            comboProcesos.Name = "ComboProcesos";
            comboProcesos.Font = new Font(new FontFamily("Consolas"), 11.25f, FontStyle.Regular);
            comboProcesos.Location = new Point(140, 30);
            comboProcesos.Width = 425;
            comboProcesos.SelectedIndexChanged += ComboProcesos_SelectedIndexChanged;

            PopulateComboProcess();

            panelInicio.Controls.Add(labelCombo);
            panelInicio.Controls.Add(comboProcesos);
        }

        /// <summary>
        /// Carga los controles de losparametros OnDemand
        /// </summary>
        /// <returns>regrsa el nombre de los objetos creados</returns>
        private void CargaParametersInPut()
        {
            if (this.settings.Parameters.Keys.ToList().Exists(k => k == "ONDEMAND"))
            {
                List<Point> pocisionesLabels = new List<Point>()
                {
                    new Point(40,130),
                    new Point(320,130),
                    new Point(40,180),
                    new Point(320,180)
                };

                List<Point> pocisionesCajas = new List<Point>()
                {
                    new Point(140,130),
                    new Point(420,130),
                    new Point(140,180),
                    new Point(420,180)
                };

                Label tituloParamIN = new Label();
                tituloParamIN.Name = "EtiquetaTituloParamsIn";
                tituloParamIN.Text = "------------------------  PARAMETROS  DE  ENTRADA  ------------------------";
                tituloParamIN.Font = new Font(new FontFamily("Consolas"), 9.50f, FontStyle.Bold);
                tituloParamIN.Location = new Point(35, 90);
                tituloParamIN.AutoSize = true;
                settings.NameCtrlsCreated.Add(tituloParamIN.Name);

                panelInicio.Controls.Add(tituloParamIN);

                List<Label> labelParams = new List<Label>();
                List<TextBox> cajasParams = new List<TextBox>();
                List<string> llaves = this.settings.Parameters["ONDEMAND"].Keys.ToList();
                List<string> values = this.settings.Parameters["ONDEMAND"].Values.ToList();

                ApplyOperatorValues(ref values);

                for (int i = 0; i <= this.settings.Parameters["ONDEMAND"].Count - 1; i++)
                {

                    Label etiqueta = new Label();
                    etiqueta.Name = string.Format("EtiquetaParam{0}", i);
                    etiqueta.Text = llaves[i];
                    etiqueta.Font = new Font(new FontFamily("Consolas"), 9.50f, FontStyle.Regular);
                    etiqueta.Location = pocisionesLabels[i];
                    settings.NameCtrlsCreated.Add(etiqueta.Name);

                    TextBox caja = new TextBox();
                    caja.Name = string.Format("CajaParam{0}", i);
                    caja.Text = values[i];
                    caja.Font = new Font(new FontFamily("Consolas"), 9.50f, FontStyle.Regular);
                    caja.Location = pocisionesCajas[i];
                    caja.Width = 140;
                    settings.NameCtrlsCreated.Add(caja.Name);

                    labelParams.Add(etiqueta);
                    cajasParams.Add(caja);
                }

                panelInicio.Controls.AddRange(labelParams.ToArray());
                panelInicio.Controls.AddRange(cajasParams.ToArray());
            }
        }


        private TextBox CargaTextBoxLog()
        {
            TextBox cajaLog = new TextBox();
            cajaLog.Name = "TexBoxLog";
            cajaLog.Text = "";
            cajaLog.Font = new Font(new FontFamily("Consolas"), 9.50f, FontStyle.Regular);
            cajaLog.Location = new Point(40, 250);
            cajaLog.Width = 520;
            cajaLog.Multiline = true;
            cajaLog.Height = 85;
            cajaLog.ScrollBars = ScrollBars.Vertical;
            cajaLog.ReadOnly = true;
            settings.NameCtrlsCreated.Add(cajaLog.Name);

            panelInicio.Controls.Add(cajaLog);

            Label tituloLog = new Label();
            tituloLog.Name = "EtiquetaTituloLog";
            tituloLog.Text = "----------------------------------  Log  ----------------------------------";
            tituloLog.Font = new Font(new FontFamily("Consolas"), 9.50f, FontStyle.Bold);
            tituloLog.Location = new Point(35, 230);
            tituloLog.AutoSize = true;
            settings.NameCtrlsCreated.Add(tituloLog.Name);

            panelInicio.Controls.Add(tituloLog);

            return cajaLog;
        }

        private void CargaButtonRunProcess()
        {
            buttonProcess = new Button();
            buttonProcess.Name = "BotonRunProcess";
            buttonProcess.Text = "Run Process";
            buttonProcess.Font = new Font(new FontFamily("Consolas"), 10.50f, FontStyle.Italic);
            buttonProcess.Location = new Point(250, 375);
            buttonProcess.AutoSize = true;

            buttonProcess.Click += ButtonProcess_Click;

            panelInicio.Controls.Add(buttonProcess);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombreProceso"></param>
        /// <param name="referencia"></param>
        /// <returns></returns>
        private void SetProcessSetting(string nombreProceso, string referencia)
        {            
            System.Configuration.ConfigurationManager.RefreshSection(string.Format("{0}/Settings", nombreProceso));
            object seccion = System.Configuration.ConfigurationManager.GetSection(string.Format("{0}/Settings", nombreProceso));
            ProcessSettingsSection xml = (ProcessSettingsSection)seccion;

            settings = new ProcessSettings();            
            settings.NameProcess =nombreProceso;
            settings.TypeName = referencia;
            settings.Parameters = GetProcessParameters( xml.GetListParameterElement());
            settings.OnDemand = true;
            settings.ModeDebug = xml.Process.ModoDebug;
            settings.SuccesAddressBook = xml.Process.SuccesAddressBook;
            settings.FailAddressBook = xml.Process.FailAddressBook;
        }

        private Dictionary<string, Dictionary<string, string>> GetProcessParameters(List<ParameterElement> parametersInfo)
        {
            List<ENTParametrosCnf> allParametrosBD = BLLFacturaGlobal.GETStringsConfigurationAndParams();            

            Dictionary<string, Dictionary<string, string>> parametros = new Dictionary<string, Dictionary<string, string>>();

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

            return parametros;
        }

        private void RunProcess()
        {
            ExecuteProcess.Proceso process = new ExecuteProcess.Proceso(settings.TypeName, settings.NameProcess);
            process.Log = this.Log;
            process.ProcessModoDebug = settings.ModeDebug;
            process.ProcessOnDemand = true;
            process.OnFinishExecution += Process_OnFinishExecution;
            process.OnValidationProcess += Process_OnValidationProcess;
            process.OnBeforeRun += Process_OnBeforeRun;
            process.OnShowProgressProcess += Process_OnShowProgressProcess;
            process.WriteLog = true;

            process.OnWritedLog += Process_OnWritedLog;

            GradualSetProgressBar(0, 3);

            process.Run(settings.Parameters);
        }
 
        private void SendCorreoFinishTask(ResponseTask response, string nameTask, string typeName, string succesGrupoCorreo, string failGrupoCorreo)
        {
            AdministradorElement admin = GetAdministrador();
            string txt = string.Empty;

            if (response.Succes)
            {
                txt = string.Format("Hola {0}\n\r\n\rLa tarea \"{1}\" por \"OnDemand\" fue terminada correctamente el {2:dd-MMM-yyyy} a las {2:HH:mm} hrs en el server {3}({4})",
                    (DateTime.Now.Hour > 5 && DateTime.Now.Hour < 12) ? ("Buenos Dias") : ((DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 19) ? ("Buenas Tardes") : ("Buenas Noches")),
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

                SendCorreo(txt, string.Format("{0} corrio exitosamente", nameTask), succesGrupoCorreo.Split(',').ToList(), "FacturacionNoReply", "Facturacion Task Manager", true);
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

                SendCorreo(txt, string.Format("{0} corrio con Errores", nameTask), failGrupoCorreo.Split(',').ToList(), "FacturacionNoReply", "Facturacion Task Manager", true);
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
        private void SendCorreo(string message, string title, List<string> groupNameAddressee, string idSettings, string nameEmisor, bool sendAdmin)
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

            //Quita Repetidos
            foreach (string correo in correos.Select(f => f.EmailAdress).ToList()
                                                 .GroupBy(x => x)
                                                 .Where(w => w.Count() > 1)
                                                 .Select(y => y.Key).ToList()
                                                 )
            {
                foreach (TypeAddressee tipo in Enum.GetValues(typeof(TypeAddressee)))
                {                    
                    List<EmailUser> noBorrar = correos.Where(w => w.EmailAdress == correo && w.TipoDestinatario == tipo).ToList();

                    if (noBorrar.Count()> 0)
                    {
                        correos = correos.FindAll(w => w.EmailAdress != correo).ToList();
                        correos.Add(noBorrar[0]);
                        break;
                    }
                }
            }

            if (sendAdmin)
            {
                var adminData = GetAdministrador();

                EmailUser admin = new EmailUser();
                admin.EmailAdress = adminData.Correo;
                admin.Nombre = string.Format("{0} {1}", adminData.Nombre, adminData.Apellido);
                admin.TipoDestinatario = (correos.FindAll(f => f.TipoDestinatario == TypeAddressee.To).ToList().Count == 0) ? (TypeAddressee.To) : (TypeAddressee.BCC);

                correos.Add(admin);
            }

            mail.SendMail(correos, nameEmisor, message, title);
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


        private void SetHostNameIP()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // `Dns.Resolve()` method is deprecated.
            Regex ipRegex = new Regex(@"^\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}");

            try
            {
                for (int i = 0; i <= ipHostInfo.AddressList.Count() - 1; i++)
                {
                    if (ipRegex.IsMatch(ipHostInfo.AddressList[i].ToString()))
                    {
                        this.IP = ipHostInfo.AddressList[i].ToString();
                        break;
                    }
                }

                this.HostName = ipHostInfo.HostName;
            }
            catch
            {
                this.IP = "IP NO DISPONIBLE";
            }
        }

        private void EscribirLogTxtBox(string txt)
        {
            var ctrls = panelInicio.Controls.Find("TexBoxLog", false);

            if (ctrls.Count() == 1)
            {
                var textBox = (TextBox)ctrls[0];
                textBox.Text = string.Format("{0}\r\n{1}\r\n", textBox.Text, txt);
                textBox.ReadOnly = true;
                textBox.SelectionStart = textBox.TextLength;
                textBox.ScrollToCaret();
            }
        }

        private void HabilitarCtrls(bool habilitar)
        {
            foreach (Control ctrl in this.panelInicio.Controls)
            {
                ctrl.Enabled = habilitar;
            }

            this.buttonProcess.Enabled = habilitar;
        }

        private void GradualSetProgressBar(int oldValue, int newValue)
        {
            for (int i = oldValue; i <= newValue; i++)
            {
                Thread.Sleep(75);
                bgWorker.ReportProgress(i);
                stripStatusLabel.Text = string.Format("Proceso Corriendo {0}%",i);
            }
        }

        private void ApplyOperatorValues(ref List<string> values)
        {
            string[] operadores = new string[] { "$Today" };

            for (int contador = 0;contador<values.Count;contador++)
            {
                int openParentesis = values[contador].LastIndexOf('('), cierraParentesis= values[contador].LastIndexOf(')'), tamañoInstruccion= values[contador].Length;

                if (values[contador][0] == '$' && openParentesis > -1 && cierraParentesis > -1 && values[contador][tamañoInstruccion - 1] == ')')
                {
                    string nombreOperador = values[contador].Substring(1, openParentesis - 1).Trim();

                    switch (nombreOperador)
                    {
                        case "Today":
                            {
                                string paramcantidadDias = values[contador].Substring(openParentesis + 1, values[contador].LastIndexOf(')') - openParentesis - 1).Replace(" ", string.Empty);
                                values[contador] = string.Format("{0:yyyy-MM-dd}", DateTime.Today.AddDays(int.Parse(paramcantidadDias)));

                                break;
                            }
                    }

                }
            }
        }

        #endregion
    }
}

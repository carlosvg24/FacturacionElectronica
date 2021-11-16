using ExecuteProcess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExecuteProcess.ServiceEnt;
using System.Configuration;
using MetodosComunes;
using MetodosComunes.Entity;
using MetodosComunes.ToolsFnWebConfig;
using Process.Entidades;
using System.Threading;

namespace ExecuteProcess.Process
{
    public class Ejemplo : IProcess
    {
        #region Propiedades Publicas

        public bool ModeDebug { get; set; }

        public bool OnDemand { get; set; }

        public Dictionary<string, Dictionary<string, string>> Parametros { get; set; }

        #endregion

        /// <summary>
        /// Evento que sirve para actualizar la barra deprogress en el disparador manual
        /// </summary>
        public event ShowPorcentProgress OnShowPorcentProgress;

        #region Metodos Publicos

        /// <summary>
        /// Metodo principal que tendra el core del proceso, este se ejecutara por el ejecutor manual o automatico
        /// </summary>
        /// <returns></returns>
        public ResponseTask MainProcess()
        {
            ResponseTask response = new ResponseTask();
            int newPorcent = 25;

            try
            {                
                Thread.Sleep(2000);
                var configuraciones = GetConfiguracionMail("FacturacionNoReply");

                ///El porcentaje de progreso siempre sera apartir del 10% hasta el 95%                
                PrintProgressbar(10, newPorcent, "Se obtuvieron los settings");

                Thread.Sleep(2000);
                newPorcent = 45;
                PrintProgressbar(25, newPorcent, "Iniciando envio de correo...");

                SendCorreo(
                    string.Format("Hola\n\rMensaje de prueba a las {0:HH:mm:ss} el dia {0:ddd} de {0:MMM} del {0:yyyy}", DateTime.Now),
                    "Proceso de Ejemplo",
                    "Disparador Manual de Procesos",
                    configuraciones,
                    this.Parametros["ONDEMAND"]["eMail"]);

                newPorcent = 95;
                PrintProgressbar(45, newPorcent, "Correo enviado");

                response.Succes = true;
            }
            catch (Exception ex)
            {
                response.Succes = false;
                response.StackTrace = ex.StackTrace;
                response.Message = ex.Message;                
            }            

            return response;            
        }

        /// <summary>
        /// Validaciones o precauciones que considere el desarrollador a revisar previamente a correr el MainProcess para decidir o no correr el proceso
        /// </summary>
        /// <returns></returns>
        public ResponseTask ValidationsBeforeExecution()
        {
            return new ResponseTask()
                        {
                            Succes = true,
                        };
        }

        #endregion

        #region Metodos Privados

        private void SendCorreo(string message, string title, string nameEmisor, MailWebConfigElement settings,string correo)
        {
            FnSendMail mail = new FnSendMail(settings);

            List<EmailUser> correos = new List<EmailUser>();
            correos.Add( new EmailUser()
            {
                EmailAdress = correo,
                TipoDestinatario = TypeAddressee.To
            });

            mail.SendMail(correos, nameEmisor, message, title);
        }

        private MailWebConfigElement GetConfiguracionMail(string idSettings)
        {
            //Se obtiene xml de la seccion customizada
            object seccion = ConfigurationManager.GetSection("FacturacionServiceSettings/MetodosComunes");

            //se manda para q se formatee y se administre con a clase MailSection
            MailSection mailSectionManager = new MailSection(seccion);

            return mailSectionManager.GetMailPropertiesById(idSettings);
        }

        private void PrintProgressbar(int oldValor,int newValor,string msj)
        {
            if (OnShowPorcentProgress != null)
                OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs(oldValor, newValor, msj));
        }

        #endregion
    }
}

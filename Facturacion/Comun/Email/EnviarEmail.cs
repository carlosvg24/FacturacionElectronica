using Comun.Log;
using Facturacion.ENT;
using Facturacion.ENT.Portal.Facturacion;
using Facturacion.ENT.ProcesoFacturacion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Comun.Email
{
    public class EnviarEmail
    {
        private string cnfEmailUser;
        private string cnfEmailSourceName;
        private string cnfSendGridHost;
        private string cnfSendGridPort;
        private string cnfSendGridUser;
        private string cnfSendGridPass;
        private string cnfCategoria;
        private string cnfEmailContactoTestFac;
        private string cnfEmailCopiaOcultaFac;
        private string cnfEmailErrorFacturacion;
        private string cnfRegistrarLog;
        private string cnfRutaLog;
        private string cnfAsuntoCorreoFacturacion;
        private string cnfAsuntoCorreoFacturacionGlobal;
        private string cnfMensajeEncFac;
        private string cnfMensajePieFac;
        private string cnfEmailCopiaOcultaFacGlobal;
        private string cnfEmailFinanzasFacGlobal;
        private string apikey;

        private RegistrarLog regLog;

        public EnviarEmail(List<ENTParametrosCnf> listaParametros, ENTEmpresaCat empresaEmisora)
        {

            //Asignar variables globales de la aplicacion
            cnfEmailUser = listaParametros.Where(x => x.Nombre == "emailUser").FirstOrDefault().Valor;
            apikey = listaParametros.Where(x => x.Nombre == "SendGridApiKey").FirstOrDefault().Valor;
            cnfEmailSourceName = listaParametros.Where(x => x.Nombre == "emailSourceName").FirstOrDefault().Valor;
            cnfSendGridHost = listaParametros.Where(x => x.Nombre == "SendGridHost").FirstOrDefault().Valor;
            cnfSendGridPort = listaParametros.Where(x => x.Nombre == "SendGridPort").FirstOrDefault().Valor;
            cnfSendGridUser = "apikey";
            cnfSendGridPass = listaParametros.Where(x => x.Nombre == "SendGridPass").FirstOrDefault().Valor;
            cnfCategoria = listaParametros.Where(x => x.Nombre == "cnfCategoria").FirstOrDefault().Valor;
            cnfEmailContactoTestFac = listaParametros.Where(x => x.Nombre == "emailContactoTestFac").FirstOrDefault().Valor;
            cnfEmailCopiaOcultaFac = listaParametros.Where(x => x.Nombre == "emailCopiaOcultaFac").FirstOrDefault().Valor;
            cnfEmailErrorFacturacion = listaParametros.Where(x => x.Nombre == "emailErrorFacturacion").FirstOrDefault().Valor;
            cnfRegistrarLog = listaParametros.Where(x => x.Nombre == "registrarLog").FirstOrDefault().Valor;
            cnfRutaLog = listaParametros.Where(x => x.Nombre == "rutaLog").FirstOrDefault().Valor;
            cnfAsuntoCorreoFacturacion = listaParametros.Where(x => x.Nombre == "asuntoCorreoFacturacion").FirstOrDefault().Valor;
            cnfMensajeEncFac = listaParametros.Where(x => x.Nombre == "MensajeEncFac").FirstOrDefault().Valor;
            cnfMensajePieFac = listaParametros.Where(x => x.Nombre == "MensajePieFac").FirstOrDefault().Valor;
            regLog = new RegistrarLog(cnfRutaLog);
        }

        public string sendConfirmarAlta(string url, string email, Guid codigoVerificacion)
        {
            string result = "";

            try
            {
                
                string cuerpoCorreo = String.Empty;
                cuerpoCorreo = GeneraBodyConfirmarAlta(url, codigoVerificacion);
        //        cuerpoCorreo = "<br/><br/>Tu usuario a sido creado éxitosamente, por cuestiones de seguridad te pedimos por favor " +
        //" confirmar tu cuenta de correo. Por favor, da clic en el link de abajo para activar tu usuario" +
        //" <br/><br/><a href='" + URLverificacion + codigoVerificacion.ToString() + "'>" + URLverificacion + "</a> ";

                //Configuracion de la cuenta de correo
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient();

                SmtpServer.Host = cnfSendGridHost;
                SmtpServer.Port = int.Parse(cnfSendGridPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(cnfSendGridUser, apikey);
                mail.IsBodyHtml = true;

                //Agregando categoria
                string headers = "{\"category\":[\"Comercial\",\"" + cnfCategoria + "\"]}";
                mail.Headers.Add("X-SMTPAPI", headers);

                //Configurando la cuenta del remitente
                mail.From = new MailAddress(cnfEmailUser, cnfEmailSourceName);
                mail.To.Add(email);

                //Asignando el cuerpo del correo y Asunto
                mail.Subject = "Usuario creado correctamente";
                mail.Body = cuerpoCorreo;
                mail.Priority = MailPriority.High;

                //Enviando Correo
                SmtpServer.Send(mail);
                SmtpServer.Dispose();
                mail.Dispose();
                result = "OK";

            }
            catch (Exception ex)
            {
                result = "FALLA";
                string mensajeUsu = "El servicio de envio por correo de los archivos no se encuentra disponible por el momento, intente descargar los archivos desde el portal...";
                string mensajeLog = String.Format("EnviarEmail.sendEmailFactura. Error -> {0}", ex.Message + "|| InnerException --> " + ex.InnerException);
                regLog.RegistrarMensajeLog(mensajeUsu, mensajeLog, false);
                throw new Exception(mensajeUsu);
            }

            return result;
        }

        public string sendContraseniaUsuario(string email, string pass)
        {
            string result = "";

            try
            {

                string cuerpoCorreo = String.Empty;
                cuerpoCorreo = GeneraBodysendContrasenia(email, pass);

                //Configuracion de la cuenta de correo
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient();

                SmtpServer.Host = cnfSendGridHost;
                SmtpServer.Port = int.Parse(cnfSendGridPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(cnfSendGridUser, apikey);
                mail.IsBodyHtml = true;

                //Agregando categoria
                string headers = "{\"category\":[\"Comercial\",\"" + cnfCategoria + "\"]}";
                mail.Headers.Add("X-SMTPAPI", headers);

                //Configurando la cuenta del remitente
                mail.From = new MailAddress(cnfEmailUser, cnfEmailSourceName);
                mail.To.Add(email);

                //Asignando el cuerpo del correo y Asunto
                mail.Subject = "Recuperación de contraseña";
                mail.Body = cuerpoCorreo;
                mail.Priority = MailPriority.High;

                //Enviando Correo
                SmtpServer.Send(mail);
                SmtpServer.Dispose();
                mail.Dispose();
                result = "OK";
            }
            catch (Exception ex)
            {
                result = "FALLA";
                string mensajeUsu = "El servicio de envio por correo de los archivos no se encuentra disponible por el momento, intente descargar los archivos desde el portal...";
                string mensajeLog = String.Format("EnviarEmail.sendEmailFactura. Error -> {0}", ex.Message + "|| InnerException --> " + ex.InnerException);
                regLog.RegistrarMensajeLog(mensajeUsu, mensajeLog, false);
                throw new Exception(mensajeUsu);
            }

            return result;
        }

        public string sendEmailFactura(ENTDatosFacturacion datosCliente, ENTEmpresaCat empresaEmisora, List<ENTPagosPorFacturar> listaPagosFacturados, List<string> listaArchivosEnvio)
        {
            string result = "";


            RegexUtilities util = new RegexUtilities();
            List<string> listaTodosContactos = new List<string>();


            //Se realiza el envio del correo
            try
            {

                string cuerpoCorreo = "";
                cuerpoCorreo = GeneraBodyFacturacion(listaPagosFacturados, datosCliente, empresaEmisora);

                //Configuracion de la cuenta de correo
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient();

                SmtpServer.Host = cnfSendGridHost;
                SmtpServer.Port = int.Parse(cnfSendGridPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(cnfSendGridUser, apikey);
                mail.IsBodyHtml = true;

                //Agregando categoria
                string headers = "{\"category\":[\"Comercial\",\"" + cnfCategoria + "\"]}";
                mail.Headers.Add("X-SMTPAPI", headers);

                //Configurando la cuenta del remitente
                mail.From = new MailAddress(cnfEmailUser, cnfEmailSourceName);


                //Agregando correos destinatarios
                if (datosCliente.EmailReceptor.Length > 0)
                {
                    foreach (var dest in datosCliente.EmailReceptor.Split(';'))
                    {
                        if (util.IsValidEmail(dest) && listaTodosContactos.Contains(dest) == false)
                        {
                            mail.To.Add(dest);
                            listaTodosContactos.Add(dest);
                        }
                    }
                }


                if (mail.To.Count <= 0)
                {
                    throw new Exception("No existe correo válido para el envío...");
                }

                //Agregando contactos Con Copia Oculta (CCO)
                if (cnfEmailCopiaOcultaFac.Length > 0)
                {
                    foreach (var cco in cnfEmailCopiaOcultaFac.Split(';'))
                    {
                        if (util.IsValidEmail(cco) && listaTodosContactos.Contains(cco) == false)
                        {
                            mail.Bcc.Add(cco);
                            listaTodosContactos.Add(cco);
                        }
                    }
                }

                //Asignando el cuerpo del correo y Asunto
                mail.Subject = cnfAsuntoCorreoFacturacion;
                mail.Body = cuerpoCorreo;
                mail.Priority = MailPriority.High;

                foreach (string rutaArchivos in listaArchivosEnvio)
                {
                    mail.Attachments.Add(new Attachment(rutaArchivos));
                }


                //Enviando Correo
                SmtpServer.Send(mail);
                SmtpServer.Dispose();
                mail.Dispose();
                result = "OK";

            }
            catch (Exception ex)
            {
                result = "FALLA";
                string mensajeUsu = "El servicio de envio por correo de los archivos no se encuentra disponible por el momento, intente descargar los archivos desde el portal...";
                string mensajeLog = String.Format("EnviarEmail.sendEmailFactura. Error -> {0}", ex.Message + "|| InnerException --> " + ex.InnerException);
                regLog.RegistrarMensajeLog(mensajeUsu, mensajeLog, false);
                //throw new Exception(mensajeUsu);
            }

            return result;
        }


        public string sendEmailErrores(List<string> listaErrores, string procesoEnviaError, string pnr, string listaCorreosPorNivel)
        {
            string result = "";


            RegexUtilities util = new RegexUtilities();


            List<string> listaTodosContactos = new List<string>();


            //Se realiza el envio del correo
            try
            {
                string cuerpoCorreo = "";
                cuerpoCorreo = GenerarBodyErrores(listaErrores, procesoEnviaError, pnr);

                //Configuracion de la cuenta de correo
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Host = cnfSendGridHost;
                SmtpServer.Port = int.Parse(cnfSendGridPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(cnfSendGridUser, apikey);
                mail.IsBodyHtml = true;

                //Agregando categoria
                string headers = "{\"category\":[\"Comercial\",\"LogFacturacion\"]}";
                mail.Headers.Add("X-SMTPAPI", headers);

                //Configurando la cuenta del remitente
                mail.From = new MailAddress(cnfEmailUser, cnfEmailSourceName);

                //Agregando correos destinatarios
                if (cnfEmailErrorFacturacion.Length > 0)
                {
                    foreach (var dest in cnfEmailErrorFacturacion.Split(';'))
                    {
                        if (util.IsValidEmail(dest) && listaTodosContactos.Contains(dest) == false)
                        {
                            mail.To.Add(dest);
                            listaTodosContactos.Add(dest);
                        }
                    }
                }

                if (listaCorreosPorNivel.Length > 0)
                {
                    foreach (var dest in listaCorreosPorNivel.Split(';'))
                    {
                        if (util.IsValidEmail(dest) && listaTodosContactos.Contains(dest) == false)
                        {
                            mail.To.Add(dest);
                            listaTodosContactos.Add(dest);
                        }
                    }
                }


                if (mail.To.Count <= 0)
                {
                    throw new Exception("No existe correo válido para el envío...");
                }

                //Asignando el cuerpo del correo y Asunto
                mail.Subject = string.Format("Reporte de Errores en Facturacion {0}", DateTime.Now.ToString());
                mail.Body = cuerpoCorreo;
                mail.Priority = MailPriority.High;


                //Enviando Correo
                SmtpServer.Send(mail);
                SmtpServer.Dispose();
                mail.Dispose();
                result = "OK";

            }
            catch (Exception ex)
            {
                string mensajeUsu = "";
                string mensajeLog = String.Format("EnviarEmail.sendEmailErrores. Error -> {0}", ex.Message + "|| InnerException --> " + ex.InnerException);
                regLog.RegistrarMensajeLog(mensajeUsu, mensajeLog, false);
                throw new Exception(mensajeUsu);
            }

            return result;
        }

        private string GeneraBodyConfirmarAlta(String url, Guid codigoVerificacion)
        {
            string result = String.Empty;

            try
            {
                var URLverificacion = url + "/UsuariosPortal/get/" + codigoVerificacion.ToString();
                System.Text.StringBuilder body = new System.Text.StringBuilder();

                body.AppendLine("<!DOCTYPE html>");
                body.AppendLine("<html>");
                body.AppendLine("<head>");
                body.AppendLine("    <meta http-equiv='X-UA-Compatible' content='IE=edge'/>");
                body.AppendLine("     ");
                body.AppendLine("         <meta charset = 'UTF-8'/>");
                body.AppendLine("      ");
                body.AppendLine("</head>");
                body.AppendLine("<body style='font-family: Verdana, Geneva, Tahoma, sans-serif; background-color: #FFFFFF;'>");
                body.AppendLine("");
                body.AppendLine("");
                body.AppendLine("<table style ='font-family: Verdana, Geneva, Tahoma, sans-serif; margin: 5px; border-style: solid; width: 775px; border-width:2px;'>");
                body.AppendLine("<tr>");
                body.AppendLine("<td>");
                body.AppendLine("            <table style ='padding:10;");
                body.AppendLine("                            margin:10px;");
                body.AppendLine("            font-family: Verdana, Geneva, Tahoma, sans-serif;");
                body.AppendLine("            background-color: #fff;");
                body.AppendLine("                            width:750px;'> ");
                body.AppendLine("                 <tr>");
                body.AppendLine("                   <td>");
                body.AppendLine("                     <table style = 'width:100%;'>");
                body.AppendLine("                        <tr>");
                body.AppendLine("                          <td style='width:45%;'>");
                //body.AppendLine("                             <img src='https://www.vivaaerobus.com/upload/Images/logo/vivaaerobus-logo-responsive.png' width='300px'/>");
                body.AppendLine("                             <img src='http://facturacion.vivaaerobus.com/Contents/Images/vivaaerobus-logo.png' width='300px'/>");
                body.AppendLine("                              </td>");
                body.AppendLine("                              <td></td>");
                body.AppendLine("                            </tr>");
                body.AppendLine("                          </table>");
                body.AppendLine("                        </td>");
                body.AppendLine("                      </tr>");
                body.AppendLine("                      <tr>");
                body.AppendLine("                        <td style='background-color:#048c04;");
                body.AppendLine("                            border-top-right-radius:4px;");
                body.AppendLine("                            border-top-left-radius:4px;");
                body.AppendLine("                            border-color:#1e6f1e;");
                body.AppendLine("                            border-width:1px;");
                body.AppendLine("                            border-style:none;");
                body.AppendLine("                            color:white;");
                body.AppendLine("                            padding-top:8px;");
                body.AppendLine("                            padding-bottom:8px;");
                body.AppendLine("                            padding-left:15px;");
                body.AppendLine("                            font-weight:bold;");
                body.AppendLine("                            font-size:13pt;");
                body.AppendLine("                            vertical-align:middle;");
                body.AppendLine("                            text-align:center;'>");
                body.AppendLine("                            Nuevo Usuario Facturación ");
                body.AppendLine("                         </td>");
                body.AppendLine("                       </tr>");


                body.AppendLine("                       <tr>");
                body.AppendLine("                       <td>");
                body.AppendLine("                           <table style='width:100%;");
                body.AppendLine("                                       font-family: sans-serif;font-size: 11pt;");
                body.AppendLine("                                       border-collapse:collapse;");
                body.AppendLine("                                       margin-bottom:15px;margin-top:15px;'>");

               
                    body.AppendLine("                               <tr style='margin-bottom:2px;margin-top:2px;'>");
                    body.AppendLine("                                   <td class='valorDato'><p> Tu usuario ha sido creado exitosamente, Da clic en el link de abajo para activar tu usuario" +
        " <br/><br/><a href='" + URLverificacion + "'>" + URLverificacion + "</a></p></td>");
                    body.AppendLine("                               </tr>");
                    body.AppendLine("                           </table>");
                    body.AppendLine("                         </td>");
                    body.AppendLine("                       </tr>");


               
                body.AppendLine("                       <tr>");
                body.AppendLine("                       <td>");
                body.AppendLine("                           <table style='width:100%;");
                body.AppendLine("                                       font-family: sans-serif;font-size: 8pt;");
                body.AppendLine("                                       border-collapse:collapse; text-align:center;");
                body.AppendLine("                                       margin-bottom:15px;margin-top:15px;'>");
                body.AppendLine("                               <tr style='margin-bottom:2px;margin-top:2px;'>");
                body.AppendLine("                               <td class='valorDato'>");
               
                body.AppendLine("                                   <p>  Favor de no responder este correo ya que fue creado y enviado automaticamente por el Portal de Facturación de VivaAerobus  </p>");

                body.AppendLine("                                 </td>");
                body.AppendLine("                               </tr>");
                body.AppendLine("                           </table>");
                body.AppendLine("                         </td>");
                body.AppendLine("                       </tr>");
                body.AppendLine("               </table>");
                body.AppendLine("       </body>");
                body.AppendLine("       </html>");
                result = body.ToString();
            }
            catch (Exception ex)
            {
                string mensajeUsu = "";
                string mensajeLog = String.Format("EnviarEmail.GenerarBodyFacturacion. Error -> {0}", ex.Message + "|| InnerException --> " + ex.InnerException);
                regLog.RegistrarMensajeLog(mensajeUsu, mensajeLog, false);
                throw new Exception(mensajeUsu);
            }
            return result;
         
        }


        private string GeneraBodysendContrasenia(string email, string pass)
        {
            string result = String.Empty;

            try
            {
                
                System.Text.StringBuilder body = new System.Text.StringBuilder();

                body.AppendLine("<!DOCTYPE html>");
                body.AppendLine("<html>");
                body.AppendLine("<head>");
                body.AppendLine("    <meta http-equiv='X-UA-Compatible' content='IE=edge'/>");
                body.AppendLine("     ");
                body.AppendLine("         <meta charset = 'UTF-8'/>");
                body.AppendLine("      ");
                body.AppendLine("</head>");
                body.AppendLine("<body style='font-family: Verdana, Geneva, Tahoma, sans-serif; background-color: #FFFFFF;'>");
                body.AppendLine("");
                body.AppendLine("");
                body.AppendLine("<table style ='font-family: Verdana, Geneva, Tahoma, sans-serif; margin: 5px; border-style: solid; width: 775px; border-width:2px;'>");
                body.AppendLine("<tr>");
                body.AppendLine("<td>");
                body.AppendLine("            <table style ='padding:10;");
                body.AppendLine("                            margin:10px;");
                body.AppendLine("            font-family: Verdana, Geneva, Tahoma, sans-serif;");
                body.AppendLine("            background-color: #fff;");
                body.AppendLine("                            width:750px;'> ");
                body.AppendLine("                 <tr>");
                body.AppendLine("                   <td>");
                body.AppendLine("                     <table style = 'width:100%;'>");
                body.AppendLine("                        <tr>");
                body.AppendLine("                          <td style='width:45%;'>");
                //body.AppendLine("                             <img src='https://www.vivaaerobus.com/upload/Images/logo/vivaaerobus-logo-responsive.png' width='300px'/>");
                body.AppendLine("                             <img src='http://facturacion.vivaaerobus.com/Contents/Images/vivaaerobus-logo.png' width='300px'/>");
                body.AppendLine("                              </td>");
                body.AppendLine("                              <td></td>");
                body.AppendLine("                            </tr>");
                body.AppendLine("                          </table>");
                body.AppendLine("                        </td>");
                body.AppendLine("                      </tr>");
                body.AppendLine("                      <tr>");
                body.AppendLine("                        <td style='background-color:#048c04;");
                body.AppendLine("                            border-top-right-radius:4px;");
                body.AppendLine("                            border-top-left-radius:4px;");
                body.AppendLine("                            border-color:#1e6f1e;");
                body.AppendLine("                            border-width:1px;");
                body.AppendLine("                            border-style:none;");
                body.AppendLine("                            color:white;");
                body.AppendLine("                            padding-top:8px;");
                body.AppendLine("                            padding-bottom:8px;");
                body.AppendLine("                            padding-left:15px;");
                body.AppendLine("                            font-weight:bold;");
                body.AppendLine("                            font-size:13pt;");
                body.AppendLine("                            vertical-align:middle;");
                body.AppendLine("                            text-align:center;'>");
                body.AppendLine("                            Recuperación de contraseña ");
                body.AppendLine("                         </td>");
                body.AppendLine("                       </tr>");


                body.AppendLine("                       <tr>");
                body.AppendLine("                       <td>");
                body.AppendLine("                           <table style='width:100%;");
                body.AppendLine("                                       font-family: sans-serif;font-size: 11pt;");
                body.AppendLine("                                       border-collapse:collapse;");
                body.AppendLine("                                       margin-bottom:15px;margin-top:15px;'>");


                body.AppendLine("                               <tr style='margin-bottom:2px;margin-top:2px;'>");
                body.AppendLine("                                   <td class='valorDato'><p> <b>Contraseña<b>: "+pass+"</p></td>");
                body.AppendLine("                               </tr>");
                body.AppendLine("                           </table>");
                body.AppendLine("                         </td>");
                body.AppendLine("                       </tr>");



                body.AppendLine("                       <tr>");
                body.AppendLine("                       <td>");
                body.AppendLine("                           <table style='width:100%;");
                body.AppendLine("                                       font-family: sans-serif;font-size: 8pt;");
                body.AppendLine("                                       border-collapse:collapse; text-align:center;");
                body.AppendLine("                                       margin-bottom:15px;margin-top:15px;'>");
                body.AppendLine("                               <tr style='margin-bottom:2px;margin-top:2px;'>");
                body.AppendLine("                               <td class='valorDato'>");

                body.AppendLine("                                   <p>  Favor de no responder este correo ya que fue creado y enviado automaticamente por el Portal de Facturación de VivaAerobus  </p>");

                body.AppendLine("                                 </td>");
                body.AppendLine("                               </tr>");
                body.AppendLine("                           </table>");
                body.AppendLine("                         </td>");
                body.AppendLine("                       </tr>");
                body.AppendLine("               </table>");
                body.AppendLine("       </body>");
                body.AppendLine("       </html>");
                result = body.ToString();
            }
            catch (Exception ex)
            {
                string mensajeUsu = "";
                string mensajeLog = String.Format("EnviarEmail.GenerarBodyFacturacion. Error -> {0}", ex.Message + "|| InnerException --> " + ex.InnerException);
                regLog.RegistrarMensajeLog(mensajeUsu, mensajeLog, false);
                throw new Exception(mensajeUsu);
            }
            return result;
        }

        public string GeneraBodyFacturacion(List<ENTPagosPorFacturar> listaPagosFacturados, ENTDatosFacturacion datosCliente, ENTEmpresaCat empresaEmisora)
        {
            string result = "";
            string razonSocial = empresaEmisora.RazonSocial;

            string emailReceptor = datosCliente.EmailReceptor;
            List<string> listaMensajeEncabezado = new List<string>();
            List<string> listaMensajePie = new List<string>();

            try
            {
                string[] mensajeEnc = cnfMensajeEncFac.Split('|');
                foreach (string menEnc in mensajeEnc)
                {
                    listaMensajeEncabezado.Add(menEnc);
                }

                string[] mensajePie = cnfMensajePieFac.Split('|');
                foreach (string menPie in mensajePie)
                {
                    string textoPie = menPie;
                    if (textoPie.Contains("@razonSocial"))
                    {
                        textoPie = textoPie.Replace("@razonSocial", empresaEmisora.RazonSocial);
                    }

                    if (textoPie.Contains("@emailReceptor"))
                    {
                        textoPie = textoPie.Replace("@emailReceptor", datosCliente.EmailReceptor);
                    }

                    listaMensajePie.Add(textoPie);
                }

                System.Text.StringBuilder body = new System.Text.StringBuilder();

                body.AppendLine("<!DOCTYPE html>");
                body.AppendLine("<html>");
                body.AppendLine("<head>");
                body.AppendLine("    <meta http-equiv='X-UA-Compatible' content='IE=edge'/>");
                body.AppendLine("     ");
                body.AppendLine("         <meta charset = 'UTF-8'/>");
                body.AppendLine("      ");
                body.AppendLine("</head>");
                body.AppendLine("<body style='font-family: Verdana, Geneva, Tahoma, sans-serif; background-color: #FFFFFF;'>");
                body.AppendLine("");
                body.AppendLine("");
                body.AppendLine("<table style ='font-family: Verdana, Geneva, Tahoma, sans-serif; margin: 5px; border-style: solid; width: 775px; border-width:2px;'>");
                body.AppendLine("<tr>");
                body.AppendLine("<td>");
                body.AppendLine("            <table style ='padding:10;");
                body.AppendLine("                            margin:10px;");
                body.AppendLine("            font-family: Verdana, Geneva, Tahoma, sans-serif;");
                body.AppendLine("            background-color: #fff;");
                body.AppendLine("                            width:750px;'> ");
                body.AppendLine("                 <tr>");
                body.AppendLine("                   <td>");
                body.AppendLine("                     <table style = 'width:100%;'>");
                body.AppendLine("                        <tr>");
                body.AppendLine("                          <td style='width:45%;'>");
                //body.AppendLine("                             <img src='https://www.vivaaerobus.com/upload/Images/logo/vivaaerobus-logo-responsive.png' width='300px'/>");
                body.AppendLine("                             <img src='http://facturacion.vivaaerobus.com/Contents/Images/vivaaerobus-logo.png' width='300px'/>");
                body.AppendLine("                              </td>");
                body.AppendLine("                              <td></td>");
                body.AppendLine("                            </tr>");
                body.AppendLine("                          </table>");
                body.AppendLine("                        </td>");
                body.AppendLine("                      </tr>");
                body.AppendLine("                      <tr>");
                body.AppendLine("                        <td style='background-color:#048c04;");
                body.AppendLine("                            border-top-right-radius:4px;");
                body.AppendLine("                            border-top-left-radius:4px;");
                body.AppendLine("                            border-color:#1e6f1e;");
                body.AppendLine("                            border-width:1px;");
                body.AppendLine("                            border-style:none;");
                body.AppendLine("                            color:white;");
                body.AppendLine("                            padding-top:8px;");
                body.AppendLine("                            padding-bottom:8px;");
                body.AppendLine("                            padding-left:15px;");
                body.AppendLine("                            font-weight:bold;");
                body.AppendLine("                            font-size:13pt;");
                body.AppendLine("                            vertical-align:middle;");
                body.AppendLine("                            text-align:center;'>");
                body.AppendLine("                           " + razonSocial + " ");
                body.AppendLine("                         </td>");
                body.AppendLine("                       </tr>");


                body.AppendLine("                       <tr>");
                body.AppendLine("                       <td>");
                body.AppendLine("                           <table style='width:100%;");
                body.AppendLine("                                       font-family: sans-serif;font-size: 11pt;");
                body.AppendLine("                                       border-collapse:collapse;");
                body.AppendLine("                                       margin-bottom:15px;margin-top:15px;'>");

                foreach (string mensajeEncFin in listaMensajeEncabezado)
                {
                    body.AppendLine("                               <tr style='margin-bottom:2px;margin-top:2px;'>");
                    body.AppendLine("                                   <td class='valorDato'><p>" + mensajeEncFin + "</p></td>");
                    body.AppendLine("                               </tr>");
                    body.AppendLine("                           </table>");
                    body.AppendLine("                         </td>");
                    body.AppendLine("                       </tr>");
                }


                body.AppendLine("                       <tr style='padding: 1px; ");
                body.AppendLine("                                   margin:1px;");
                body.AppendLine("                                   font-size:11pt;");
                body.AppendLine("                                   background-color:rgb(0, 125, 0);");
                body.AppendLine("                                   vertical-align:middle;");
                body.AppendLine("                                   text-align:center;");
                body.AppendLine("                               color:white;");
                body.AppendLine("                               height:30px;'>");
                body.AppendLine("                           <td>");
                body.AppendLine("                               Detalle de Archivos Enviados");
                body.AppendLine("                           </td>");
                body.AppendLine("                       </tr>");
                body.AppendLine("                       <tr>");
                body.AppendLine("                           <td>");
                body.AppendLine("                               <table style='width:100%;");
                body.AppendLine("                                             border-style:solid;");
                body.AppendLine("                                             border-width:1px;text-align: center;");
                body.AppendLine("                                             border-collapse:collapse;");
                body.AppendLine("                                             font-family:sans-serif;'>");
                body.AppendLine("                                   <tr style='background-color:#3FAB37; padding:8px;color:#fff;border-spacing:0px;align:center;margin-bottom:0px;border-style:solid;border-width:1px;'>");
                body.AppendLine("                                       <th style='background-color:#3FAB37; padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;'>Folio Factura</th>");
                body.AppendLine("                                       <th style='background-color:#3FAB37; padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;'>Archivo CFDI</th>");
                body.AppendLine("                                       <th style='background-color:#3FAB37; padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;'>Archivo PDF</th>");
                body.AppendLine("                                       <th style='background-color:#3FAB37; padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;'>Mensaje</th>");
                body.AppendLine("                                   </tr>");
                bool esPar = true;
                foreach (ENTPagosPorFacturar pagosFact in listaPagosFacturados.Where(x => x.EstaMarcadoParaFacturacion))
                {
                    body.AppendLine("                               <tr style='" + (esPar ? "background-color: #FFFFFF;" : "background-color:#E2EFDA;") + "'>");
                    body.AppendLine("                                   <td style='padding:8px; font-size:11pt; margin-bottom:0px;'>" + (pagosFact.FolioFactura != 0 ? pagosFact.FolioFactura.ToString() : pagosFact.FolioPrefactura.ToString()) + "</td>");
                    string nombreArcCFDI = "";
                    string nombreArcPDF = "";
                    string mensaje = "";
                    if (pagosFact.RutaCFDI != null && pagosFact.RutaCFDI.Length > 0)
                    {
                        string[] rutaCFDI = pagosFact.RutaCFDI.Split('\\');
                        nombreArcCFDI = rutaCFDI[rutaCFDI.Length - 1];
                    }

                    if (pagosFact.RutaPDF != null && pagosFact.RutaPDF.Length > 0)
                    {
                        string[] rutaPDF = pagosFact.RutaPDF.Split('\\');
                        nombreArcPDF = rutaPDF[rutaPDF.Length - 1];
                    }

                    if (pagosFact.Mensaje != null)
                    {
                        if (pagosFact.Mensaje.Equals("OK"))
                        {
                            mensaje = "Factura Generada";
                        }
                        else
                        {
                            mensaje = pagosFact.Mensaje;
                        }
                    }
                    body.AppendLine("                                   <td style='padding:8px; font-size:11pt; margin-bottom:0px;'>" + nombreArcCFDI + "</td>");
                    body.AppendLine("                                   <td style='padding:8px; font-size:11pt; margin-bottom:0px;'>" + nombreArcPDF + "</td>");
                    body.AppendLine("                                   <td style='padding:8px; font-size:11pt; margin-bottom:0px;'>" + mensaje + "</td>");
                    body.AppendLine("                               </tr>");
                    esPar = !esPar;
                }
                body.AppendLine("                               </table>");
                body.AppendLine("                           </td>");
                body.AppendLine("                       </tr>");
                body.AppendLine("                       <tr>");
                body.AppendLine("                       <td>");
                body.AppendLine("                           <table style='width:100%;");
                body.AppendLine("                                       font-family: sans-serif;font-size: 10pt;");
                body.AppendLine("                                       border-collapse:collapse;");
                body.AppendLine("                                       margin-bottom:15px;margin-top:15px;'>");
                body.AppendLine("                               <tr style='margin-bottom:2px;margin-top:2px;'>");
                body.AppendLine("                               <td class='valorDato'>");

                foreach (string mensajePieFin in listaMensajePie)
                {
                    body.AppendLine("                                   <p>" + mensajePieFin + "</p>");
                }
                body.AppendLine("                                 </td>");
                body.AppendLine("                               </tr>");
                body.AppendLine("                           </table>");
                body.AppendLine("                         </td>");
                body.AppendLine("                       </tr>");
                body.AppendLine("               </table>");
                body.AppendLine("       </body>");
                body.AppendLine("       </html>");
                result = body.ToString();
            }
            catch (Exception ex)
            {
                string mensajeUsu = "";
                string mensajeLog = String.Format("EnviarEmail.GenerarBodyFacturacion. Error -> {0}", ex.Message + "|| InnerException --> " + ex.InnerException);
                regLog.RegistrarMensajeLog(mensajeUsu, mensajeLog, false);
                throw new Exception(mensajeUsu);
            }
            return result;

        }

        private string GenerarBodyErrores(List<string> listaErrores, string procesoEnviaError, string pnr)
        {
            string result = "";

            try
            {
                StringBuilder body = new StringBuilder();

                body.AppendLine("<!DOCTYPE html>");
                body.AppendLine("<html>");
                body.AppendLine("<head>");
                body.AppendLine("    <meta http-equiv='X-UA-Compatible' content='IE=edge'/>");
                body.AppendLine("     ");
                body.AppendLine("         <meta charset = 'UTF-8'/>");
                body.AppendLine("      ");
                body.AppendLine("</head>");
                body.AppendLine("<body style='font-family:sans-serif; background-color: #FFFFFF;'>");
                body.AppendLine("");
                body.AppendLine("");
                body.AppendLine("<table style ='font-family: sans-serif; margin: 5px; border-width:2px;width:100%;'>");
                body.AppendLine("<tr>");
                body.AppendLine("<td>");
                body.AppendLine("            <table style ='padding:10;");
                body.AppendLine("                            margin:10px;");
                body.AppendLine("            font-family: sans-serif;");
                body.AppendLine("            background-color: #fff;");
                body.AppendLine("            width: 100%;'> ");
                body.AppendLine("                 <tr>");
                body.AppendLine("                   <td>");
                body.AppendLine("                     <table style = 'width:100%;'>");
                body.AppendLine("                        <tr>");
                body.AppendLine("                          <td style='width:45%;'>");
                body.AppendLine("                             <img src='https://www.vivaaerobus.com/upload/Images/logo/vivaaerobus-logo-responsive.png' width='300px'/>");
                body.AppendLine("                              </td>");
                body.AppendLine("                              <td></td>");
                body.AppendLine("                            </tr>");
                body.AppendLine("                          </table>");
                body.AppendLine("                        </td>");
                body.AppendLine("                      </tr>");
                body.AppendLine("                      <tr>");
                body.AppendLine("                        <td style='background-color:#048c04;");
                body.AppendLine("                            border-top-right-radius:4px;");
                body.AppendLine("                            border-top-left-radius:4px;");
                body.AppendLine("                            border-color:#1e6f1e;");
                body.AppendLine("                            border-width:1px;");
                body.AppendLine("                            border-style:none;");
                body.AppendLine("                            color:white;");
                body.AppendLine("                            padding-top:2px;");
                body.AppendLine("                            padding-bottom:2px;");
                body.AppendLine("                            padding-left:15px;");
                body.AppendLine("                            font-weight:bold;");
                body.AppendLine("                            font-size:13pt;");
                body.AppendLine("                            vertical-align:middle;");
                body.AppendLine("                            text-align:center;'>");
                if (pnr.Length > 0) body.AppendLine(string.Format(" Reservación : {0}  -->  ", pnr));
                body.AppendLine("    Proceso : " + procesoEnviaError);

                body.AppendLine("                         </td>");
                body.AppendLine("                       </tr>");
                body.AppendLine("                       <tr>");
                body.AppendLine("                           <td>");
                body.AppendLine("                               <table style='width:100%;");
                body.AppendLine("                                             border-style:solid;");
                body.AppendLine("                                             border-width:1px;text-align: center;");
                body.AppendLine("                                             border-collapse:collapse;");
                body.AppendLine("                                             font-family:sans-serif;'>");
                body.AppendLine("                                   <tr style='background-color:#3FAB37; padding:2px;color:#fff;border-spacing:0px;align:center;margin-bottom:0px;border-style:solid;border-width:1px;'>");
                body.AppendLine("                                       <th style='background-color:#3FAB37; padding:4px; color:#fff;border-spacing:0px;text-align:left;margin-bottom: 0px;'>Listado de Errores</th>");
                body.AppendLine("                                   </tr>");
                bool esPar = true;
                foreach (var error in listaErrores)
                {
                    StringBuilder separarErrores = new StringBuilder();
                    string sepErrores = error.Replace("\n", "|");
                    foreach (string errorFila in sepErrores.Split('|'))
                    {
                        separarErrores.Append("<p>" + errorFila + "</p>");
                    }

                    body.AppendLine("                               <tr style='" + (esPar ? "background-color: #FFFFFF;" : "background-color:#E2EFDA;") + "'>");
                    body.AppendLine("                                   <td style='padding:8px; font-size:10pt; margin-bottom:0px;text-align: left;'>" + separarErrores.ToString() + "</td>");
                    body.AppendLine("                               </tr>");
                    esPar = !esPar;
                }
                body.AppendLine("                               </table>");
                body.AppendLine("                           </td>");
                body.AppendLine("                       </tr>");
                body.AppendLine("               </table>");
                body.AppendLine("       </body>");
                body.AppendLine("       </html>");
                result = body.ToString();
            }
            catch (Exception ex)
            {
                string mensajeUsu = "Error al generar correo de Errores Facturación...";
                string mensajeLog = String.Format("Email.GenerarBodyErrores. Error -> {0}", ex.Message + "|| InnerException --> " + ex.InnerException);
                regLog.RegistrarMensajeLog(mensajeUsu, mensajeLog, false);
                throw new Exception(mensajeLog);
            }
            return result;
        }




        public string sendEmailFacturaGlobal(ENTEmpresaCat empresaEmisora, List<ENTResultadoFacturaGlobal33> listaResultadoFG, DateTime fechaIni, DateTime fechaFin, String correos)
        {
            string result = "";


            RegexUtilities util = new RegexUtilities();
            List<string> listaTodosContactos = new List<string>();


            //Se realiza el envio del correo
            try
            {

                string cuerpoCorreo = "";
                cuerpoCorreo = GenerarBodyFacturaGlobal(listaResultadoFG, fechaIni, fechaFin);

                //Configuracion de la cuenta de correo
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient();

                SmtpServer.Host = cnfSendGridHost;
                SmtpServer.Port = int.Parse(cnfSendGridPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(cnfSendGridUser, apikey);
                mail.IsBodyHtml = true;

                //Agregando categoria
                string headers = "{\"category\":[\"Comercial\",\"" + cnfCategoria + "\"]}";
                mail.Headers.Add("X-SMTPAPI", headers);

                //Configurando la cuenta del remitente
                mail.From = new MailAddress(cnfEmailUser, cnfEmailSourceName);

                // EmailFinanzasFacGlobal

                cnfEmailFinanzasFacGlobal = correos;
                //Agregando correos destinatarios
                if (cnfEmailFinanzasFacGlobal.Length > 0)
                {
                    foreach (var dest in cnfEmailFinanzasFacGlobal.Split(';'))
                    {
                        if (util.IsValidEmail(dest) && listaTodosContactos.Contains(dest) == false)
                        {
                            mail.To.Add(dest);
                            listaTodosContactos.Add(dest);
                        }
                    }
                }


                if (mail.To.Count <= 0)
                {
                    throw new Exception("No existe correo válido para el envío...");
                }

                ////Agregando contactos Con Copia Oculta (CCO)
                //if (cnfEmailCopiaOcultaFacGlobal.Length > 0)
                //{
                //    foreach (var cco in cnfEmailCopiaOcultaFacGlobal.Split(';'))
                //    {
                //        if (util.IsValidEmail(cco) && listaTodosContactos.Contains(cco) == false)
                //        {
                //            mail.Bcc.Add(cco);
                //            listaTodosContactos.Add(cco);
                //        }
                //    }
                //}

                cnfAsuntoCorreoFacturacionGlobal = "La Factura Global se generó exitosamente";
                //Asignando el cuerpo del correo y Asunto
                mail.Subject = cnfAsuntoCorreoFacturacionGlobal;
                mail.Body = cuerpoCorreo;
                mail.Priority = MailPriority.High;


                //Enviando Correo
                SmtpServer.Send(mail);
                SmtpServer.Dispose();
                mail.Dispose();
                result = "OK";

            }
            catch (Exception ex)
            {
                result = "FALLA";
                string mensajeUsu = "El servicio de envio por correo de los archivos no se encuentra disponible por el momento, intente descargar los archivos desde el portal...";
                string mensajeLog = String.Format("EnviarEmail.sendEmailFactura. Error -> {0}", ex.Message + "|| InnerException --> " + ex.InnerException);
                regLog.RegistrarMensajeLog(mensajeUsu, mensajeLog, false);
                //throw new Exception(mensajeUsu);
            }

            return result;
        }

        private string GenerarBodyFacturaGlobal(List<ENTResultadoFacturaGlobal33> listaResultadoFG, DateTime fechaIni, DateTime fechaFin)
        {
            string result = "";
            StringBuilder body = new StringBuilder();

            body.AppendLine("<!DOCTYPE html>");
            body.AppendLine("<html>");
            body.AppendLine("<head>");
            body.AppendLine("    <meta http-equiv='X-UA-Compatible' content='IE=edge'/>");
            body.AppendLine("     ");
            body.AppendLine("         <meta charset = 'UTF-8'/>");
            body.AppendLine("      ");
            body.AppendLine("</head>");
            body.AppendLine("<body style='font-family: Verdana, Geneva, Tahoma, sans-serif; background-color: #FFFFFF;'>");
            body.AppendLine("");
            body.AppendLine("");
            body.AppendLine("<table style ='font-family: Verdana, Geneva, Tahoma, sans-serif; margin: 5px; border-style: solid; width: 95%; border-width:2px;'>");
            body.AppendLine("<tr>");
            body.AppendLine("<td>");
            body.AppendLine("            <table style ='padding:10;");
            body.AppendLine("                            margin:10px;");
            body.AppendLine("            font-family: Verdana, Geneva, Tahoma, sans-serif;");
            body.AppendLine("            background-color: #fff;");
            body.AppendLine("                            width:95%'> ");
            body.AppendLine("                 <tr>");
            body.AppendLine("                   <td>");
            body.AppendLine("                     <table style = 'width:100%;'>");
            body.AppendLine("                        <tr>");
            body.AppendLine("                          <td style='width:45%;'>");
            body.AppendLine("                             <img src='http://facturacion.vivaaerobus.com/Contents/Images/vivaaerobus-logo.png' width='300px'/>");
            body.AppendLine("                              </td>");
            body.AppendLine("                              <td></td>");
            body.AppendLine("                            </tr>");
            body.AppendLine("                          </table>");
            body.AppendLine("                        </td>");
            body.AppendLine("                      </tr>");
            body.AppendLine("                      <tr>");
            body.AppendLine("                        <td style='background-color:#048c04;");
            body.AppendLine("                            border-top-right-radius:4px;");
            body.AppendLine("                            border-top-left-radius:4px;");
            body.AppendLine("                            border-color:#1e6f1e;");
            body.AppendLine("                            border-width:1px;");
            body.AppendLine("                            border-style:none;");
            body.AppendLine("                            color:white;");
            body.AppendLine("                            padding-top:8px;");
            body.AppendLine("                            padding-bottom:8px;");
            body.AppendLine("                            padding-left:15px;");
            body.AppendLine("                            font-weight:bold;");
            body.AppendLine("                            font-size:13pt;");
            body.AppendLine("                            vertical-align:middle;");
            body.AppendLine("                            text-align:center;'>");
            body.AppendLine("                           FACTURACIÓN PÚBLICO GENERAL (FACTURA GLOBAL) ");
            body.AppendLine("                         </td>");
            body.AppendLine("                       </tr>");


            body.AppendLine("                       <tr>");
            body.AppendLine("                       <td>");
            body.AppendLine("                           <table style='width:100%;");
            body.AppendLine("                                       font-family: sans-serif;font-size: 11pt;");
            body.AppendLine("                                       border-collapse:collapse;");
            body.AppendLine("                                       margin-bottom:15px;margin-top:15px;'>");
            body.AppendLine("                               <tr style='margin-bottom:2px;margin-top:2px;'>");
            body.AppendLine("                                   <td class='valorDato'><p>Resultado del Proceso de Factura Global del periodo : " + fechaIni.ToString("dd/MM/yyyy") + " al " + fechaFin.ToString("dd/MM/yyyy") + "</p></td>");
            body.AppendLine("                               </tr>");
            body.AppendLine("                           </table>");
            body.AppendLine("                         </td>");
            body.AppendLine("                       </tr>");


            body.AppendLine("                       <tr style='padding: 1px; ");
            body.AppendLine("                                   margin:1px;");
            body.AppendLine("                                   font-size:14pt;");
            body.AppendLine("                                   background-color:rgb(0, 125, 0);");
            body.AppendLine("                                   vertical-align:middle;");
            body.AppendLine("                                   text-align:center;");
            body.AppendLine("                               color:white;");
            body.AppendLine("                               height:30px;'>");
            body.AppendLine("                           <td>");
            body.AppendLine("                               Detalle de Fechas Procesadas");
            body.AppendLine("                           </td>");
            body.AppendLine("                       </tr>");

            bool esPar = true;

            if (listaResultadoFG.Count > 0)
            {

                body.AppendLine("                       <tr>");
                body.AppendLine("                           <td>");
                body.AppendLine("                               <table style='width:100%;");
                body.AppendLine("                                             border-style:solid;");
                body.AppendLine("                                             border-width:1px;text-align: center;");
                body.AppendLine("                                             border-collapse:collapse;");
                body.AppendLine("                                             font-family:sans-serif;'>");
                body.AppendLine("                                   <tr style='background-color:#3FAB37; padding:8px;color:#fff;border-spacing:0px;align:center;margin-bottom:0px;border-style:solid;border-width:1px;'>");
                body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 10pt;'>Fecha Pagos</th>");
                body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 10pt;'>Moneda</th>");
                body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 10pt;'>Codigo Postal</th>");
                body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 10pt;'>Es Frontera</th>");
                body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 10pt;'>Num Fact Generadas</th>");
                body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 10pt;'>Pagos Procesados</th>");
                body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 10pt;'>Pagos Facturados</th>");
                body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 10pt;'>Pagos No Enviados</th>");
                body.AppendLine("                                   </tr>");

                foreach (ENTResultadoFacturaGlobal33 resultFactura in listaResultadoFG)
                {



                    body.AppendLine("                               <tr style='" + (esPar ? "background-color: #FFFFFF;" : "background-color:#E2EFDA;") + "'>");

                    body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + resultFactura.FechaPagos.ToString("dd/MM/yyyy") + "</td>");
                    body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + resultFactura.Moneda.ToString() + "</td>");
                    body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + resultFactura.CodigoPostal.ToString() + "</td>");
                    body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + (resultFactura.EsFrontera ? "SI" : "NO") + "</td>");
                    body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + resultFactura.NumFacturasGeneradas.ToString() + "</td>");
                    body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + resultFactura.NumPagosInicial.ToString() + "</td>");
                    body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + resultFactura.NumPagosEnviadosFG.ToString() + "</td>");
                    body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + resultFactura.NumPagosOmitidos.ToString() + "</td>");
                    body.AppendLine("                               </tr>");
                    esPar = !esPar;
                }

                body.AppendLine("                               </table>");
                body.AppendLine("                           </td>");
                body.AppendLine("                       </tr>");


                //DETALLE DE FACTURAS GENERADAS
                body.AppendLine("                       <tr style='padding: 1px; ");
                body.AppendLine("                                   margin:1px;");
                body.AppendLine("                                   font-size:14pt;");
                body.AppendLine("                                   background-color:rgb(0, 125, 0);");
                body.AppendLine("                                   vertical-align:middle;");
                body.AppendLine("                                   text-align:center;");
                body.AppendLine("                               color:white;");
                body.AppendLine("                               height:30px;'>");
                body.AppendLine("                           <td>");
                body.AppendLine("                               Listado de Facturas generadas");
                body.AppendLine("                           </td>");
                body.AppendLine("                       </tr>");


                if (listaResultadoFG.Sum(x => x.NumFacturasGeneradas) > 0)
                {
                    body.AppendLine("                       <tr>");
                    body.AppendLine("                           <td>");
                    body.AppendLine("                               <table style='width:100%;");
                    body.AppendLine("                                             border-style:solid;");
                    body.AppendLine("                                             border-width:1px;text-align: center;");
                    body.AppendLine("                                             border-collapse:collapse;");
                    body.AppendLine("                                             font-family:sans-serif;'>");

                    body.AppendLine("                                   <tr style='background-color:#3FAB37; padding:8px;color:#fff;border-spacing:0px;align:center;margin-bottom:0px;border-style:solid;border-width:1px;'>");
                    body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>Fecha Pagos</th>");
                    body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>Moneda</th>");
                    body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>Lugar Exped</th>");
                    body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>Núm. Pagos</th>");
                    body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>Monto Total</th>");
                    body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>Núm. Envio</th>");
                    body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>Folio CFDI</th>");
                    body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>Fecha Emision</th>");
                    body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>UUID</th>");
                    body.AppendLine("                                       <th style='padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>Mensaje Timbrado</th>");
                    body.AppendLine("                                   </tr>");


                    bool esParFactura = false;
                    foreach (ENTResultadoFacturaGlobal33 resultFactura in listaResultadoFG)
                    {
                        if (resultFactura.ListaEnviosFG.Count > 0)
                        {
                            foreach (ENTEnvioFacturaGlobal facturaEnviada in resultFactura.ListaEnviosFG)
                            {
                                body.AppendLine("                               <tr style='" + (esParFactura ? "background-color: #FFFFFF;" : "background-color:#E2EFDA;") + "'>");
                                body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + resultFactura.FechaPagos.ToString("dd/MM/yyyy") + "</td>");
                                body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + resultFactura.Moneda.ToString() + "</td>");
                                body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + facturaEnviada.LugarExp.ToString() + "</td>");
                                body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + facturaEnviada.NumPagosEnviados.ToString() + "</td>");
                                body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + facturaEnviada.MontoTotalFactura.ToString("$ ###,###,##0.00") + "</td>");
                                body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + facturaEnviada.IdPeticionPAC.ToString() + "</td>");
                                body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + facturaEnviada.FolioFG.ToString() + "</td>");
                                body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + facturaEnviada.FechaEmisionFG.ToString() + "</td>");
                                body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + facturaEnviada.UUID.ToString() + "</td>");
                                body.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + (facturaEnviada.Mensaje.ToString() == "OK" ? "Facturación exitosa" : facturaEnviada.Mensaje.ToString()) + "</td>");
                                body.AppendLine("                               </tr>");

                                esParFactura = !esParFactura;
                            }

                        }


                    }
                    body.AppendLine("                               </table>");
                    body.AppendLine("                           </td>");
                    body.AppendLine("                       </tr>");
                }
                else
                {
                    body.AppendLine("                       <tr>");
                    body.AppendLine("                       <td>");
                    body.AppendLine("                           <table style='width:100%;");
                    body.AppendLine("                                       font-family: sans-serif;font-size: 11pt;");
                    //body.AppendLine("                                       border-collapse:collapse;");
                    body.AppendLine("                                       margin-bottom:5px;margin-top:5px;'>");
                    body.AppendLine("                               <tr style='margin-bottom:5px;margin-top:5px;'>");
                    body.AppendLine("                                   <td class='valorDato'><p>No se generaron Facturas Globales con el periodo solicitado</p></td>");
                    body.AppendLine("                               </tr>");
                    body.AppendLine("                           </table>");
                    body.AppendLine("                         </td>");
                    body.AppendLine("                       </tr>");
                }
            }
            else
            {
                body.AppendLine("                       <tr>");
                body.AppendLine("                       <td>");
                body.AppendLine("                           <table style='width:100%;");
                body.AppendLine("                                       font-family: sans-serif;font-size: 11pt;");
                //body.AppendLine("                                       border-collapse:collapse;");
                body.AppendLine("                                       margin-bottom:5px;margin-top:5px;'>");
                body.AppendLine("                               <tr style='margin-bottom:5px;margin-top:5px;'>");
                body.AppendLine("                                   <td class='valorDato'><p>No se detectaron pagos pendientes por enviar en el periodo solicitado</p></td>");
                body.AppendLine("                               </tr>");
                body.AppendLine("                           </table>");
                body.AppendLine("                         </td>");
                body.AppendLine("                       </tr>");
            }







            //DETALLE DE PAGOS NO ENVIADOS A FACTURA GLOBAL
            StringBuilder resumenDetalleOmitidos = new StringBuilder();
            bool esParDet = false;
            foreach (ENTResultadoFacturaGlobal33 resultOmitidos in listaResultadoFG)
            {
                foreach (ENTGlobalpagosnoenviadosReg pagosNoEnviados in resultOmitidos.ListaPagosNoEnviados)
                {
                    esParDet = !esParDet;
                    resumenDetalleOmitidos.AppendLine("                               <tr style='" + (esParDet ? "background-color: #FFFFFF;" : "background-color:#E2EFDA;") + "'>");
                    resumenDetalleOmitidos.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + pagosNoEnviados.RecordLocator + "</td>");
                    resumenDetalleOmitidos.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + pagosNoEnviados.PaymentID + "</td>");
                    resumenDetalleOmitidos.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;'>" + pagosNoEnviados.FechaPago + "</td>");
                    resumenDetalleOmitidos.AppendLine("                                   <td style='padding:8px; font-size:9pt; margin-bottom:0px;text-align:left;'>" + pagosNoEnviados.MotivoOmision + "</td>");
                    resumenDetalleOmitidos.AppendLine("                               </tr>");
                }
            }

            if (resumenDetalleOmitidos.Length > 0)
            {
                body.AppendLine("                       <tr style='padding: 1px; ");
                body.AppendLine("                                   margin:1px;");
                body.AppendLine("                                   font-size:14pt;");
                body.AppendLine("                                   background-color:rgb(0, 125, 0);");
                body.AppendLine("                                   vertical-align:middle;");
                body.AppendLine("                                   text-align:center;");
                body.AppendLine("                               color:white;");
                body.AppendLine("                               height:30px;'>");
                body.AppendLine("                           <td>");
                body.AppendLine("                               Detalle de Pagos No Enviados");
                body.AppendLine("                           </td>");
                body.AppendLine("                       </tr>");

                body.AppendLine("                       <tr>");
                body.AppendLine("                           <td>");
                body.AppendLine("                               <table style='width:100%;");
                body.AppendLine("                                             border-style:solid;");
                body.AppendLine("                                             border-width:1px;text-align: center;");
                body.AppendLine("                                             border-collapse:collapse;");
                body.AppendLine("                                             font-family:sans-serif;'>");
                body.AppendLine("                                   <tr style='background-color:#3FAB37; padding:8px;color:#fff;border-spacing:0px;align:center;margin-bottom:0px;border-style:solid;border-width:1px;'>");
                body.AppendLine("                                       <th style='background-color:#3FAB37; padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>PNR</th>");
                body.AppendLine("                                       <th style='background-color:#3FAB37; padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>PaymentID</th>");
                body.AppendLine("                                       <th style='background-color:#3FAB37; padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>Fecha Pago</th>");
                body.AppendLine("                                       <th style='background-color:#3FAB37; padding:4px; color:#fff;border-spacing:0px;align:center;margin-bottom: 0px;font-size: 9pt;'>Motivo</th>");
                body.AppendLine("                                   </tr>");

                //Se inserta el detalle de los pagos no enviados
                body.Append(resumenDetalleOmitidos);

                body.AppendLine("                               </table>");
                body.AppendLine("                           </td>");
                body.AppendLine("                       </tr>");


            }

            body.AppendLine("               </table>");
            body.AppendLine("       </body>");
            body.AppendLine("       </html>");
            result = body.ToString();


            return result;
        }


    }
}

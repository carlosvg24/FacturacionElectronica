using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using System.Net.Mail;
using System.Net;
using Comun.Email;
using Facturacion.ENT;

namespace Comun.Log
{
    public class RegistrarLog
    {
        private string nombreArchivo;

        private List<string> ListaErroresAcumulados = new List<string>();

        public RegistrarLog(string rutaLog)
        {
            //Verifica la existencia del archivo Log
            nombreArchivo = VerificaExisteArchivoLog(rutaLog);
        }


        public void RegistrarMensajeLog(string mensajeUsuario, string mensajeDev)
        {
            RegistrarMensajeLog(mensajeUsuario, mensajeDev, true);
        }

        public void RegistrarMensajeLog(string mensajeUsuario, string mensajeDev, bool enviarErrorPorCorreo)
        {
            try
            {

                string mensajeError = String.Format("{0}\tMensaje Usuario:{1}\r\n\t\t\tMensaje Dev:{2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), mensajeUsuario, mensajeDev.Replace("Error ->", "\r\n\t\t\tError ->"));
                

                using (var fileStream = File.Open(nombreArchivo, FileMode.Append))
                {
                    byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                    fileStream.Write(newline, 0, newline.Length);
                    if (enviarErrorPorCorreo)
                    {
                        ListaErroresAcumulados.Add(mensajeError);
                    }
                    var texto = new UTF8Encoding(true).GetBytes(mensajeError);
                    fileStream.Write(texto, 0, texto.Length);
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(String.Format("Comun.VerificaExisteArchivoLog. Error -> {0}, Parametros({1},{2})", ex.Message, mensajeUsuario, mensajeDev));
            }
        }

        public bool EnviarCorreoErrores(List<ENTParametrosCnf> listaParametros, ENTEmpresaCat empresaEmisora, string procesoActual)
        {
            bool result = false;
            result = EnviarCorreoErrores(listaParametros,  empresaEmisora,  procesoActual, "");

            return result;
        }

        public bool EnviarCorreoErrores(List<ENTParametrosCnf> listaParametros, ENTEmpresaCat empresaEmisora, string procesoActual,string pnr)
        {
            bool result = false;
            try
            {
                EnviarEmail email = new EnviarEmail(listaParametros, empresaEmisora);
                email.sendEmailErrores(ListaErroresAcumulados, procesoActual,pnr,"");
                result = true;
            }
            catch (Exception ex)
            {
                string mensajeLog = String.Format("Email.GenerarBodyErrores. Error -> {0}", ex.Message + "|| InnerException --> " + ex.InnerException);
                RegistrarMensajeLog("", mensajeLog, false);
            }
            return result;
        }


        private string VerificaExisteArchivoLog(string ruta)
        {
            string result = "";
            try
            {
                string nombreArchivo = "LogError" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".txt";

                //Verifica si existe el subdirectorio
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }

                //Verifica si existe el archivo
                string rutaCompletaArchivo = ruta + "\\" + nombreArchivo;
                if (!File.Exists(rutaCompletaArchivo))
                {
                    // crear el fichero
                    using (var fileStream = File.Create(rutaCompletaArchivo))
                    {
                        var texto = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " Creacion de archivo Log.");
                        fileStream.Write(texto, 0, texto.Length);
                        fileStream.Flush();
                        fileStream.Close();

                    }
                }
                result = rutaCompletaArchivo;
            }
            catch (Exception ex)
            {
                //throw new Exception(String.Format("Comun.VerificaExisteArchivoLog. Error -> {0}, Parametros({1})", ex.Message, ruta));
            }

            return result;
        }



    }
}

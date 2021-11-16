using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using VBFactPaquetes.Comun.Log;
using VBFactPaquetes.Comun.Utilerias;
using VBFactPaquetes.Model.FacturacionBoletos;

namespace VBFactPaquetes.BLL
{
    public class BLLFacturacionBoletos
    {

        public static RespuestaRESTFactura FacturarBoletoAvion(DatosFacturacion datosFacturacion)
        {
            string json = string.Empty;
            string url = string.Empty;
            HttpWebRequest request;
            WebResponse response;
            bool EsJson = false;
            RespuestaRESTFactura respuestaRESTFactura = new RespuestaRESTFactura();
            string responseBody = String.Empty;

            try
            {
                url = System.Configuration.ConfigurationManager.AppSettings["urlTimbrarBoleto"].ToString();
                request = (HttpWebRequest)WebRequest.Create(url);

                json = JsonConvert.SerializeObject(datosFacturacion);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings["timeOutTimbrarBoleto"].ToString()); // 300000; // 5 min

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (response = request.GetResponse() as HttpWebResponse)
                {
                    //response = request.GetResponse() as HttpWebResponse;
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return respuestaRESTFactura;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            responseBody = objReader.ReadToEnd();
                            //Do something with responseBody
                            //Console.WriteLine(responseBody);
                            try
                            {
                                respuestaRESTFactura = JsonConvert.DeserializeObject<RespuestaRESTFactura>(responseBody);
                            }
                            catch (Exception ex)
                            {
                                respuestaRESTFactura.Codigo = 900;
                                respuestaRESTFactura.Mensaje = ex.Message;
                                respuestaRESTFactura.PagosFacturados = null;
                            }
                        }
                    }
                }
            }
            catch (WebException ex)
            {

                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                respuestaRESTFactura.Codigo = 901;
                EsJson = Validador.IsValidJson(resp);
                if (EsJson)
                {
                    respuestaRESTFactura = JsonConvert.DeserializeObject<RespuestaRESTFactura>(resp);
                }
                else
                {
                    respuestaRESTFactura.Mensaje = "Error no controlado";
                }
            }
            catch(Exception ex)
            {
                respuestaRESTFactura.Codigo = 902;
                respuestaRESTFactura.Mensaje = "Error controlado";
                throw new Excepciones (MethodBase.GetCurrentMethod().DeclaringType.Name,
                                                      MethodBase.GetCurrentMethod().Name,
                                                      null,
                                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }

            return respuestaRESTFactura;
        }

    }
}

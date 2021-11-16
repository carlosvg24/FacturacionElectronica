using Facturacion.ENT;
using Facturacion.ENT.ProcesoFacturacion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Facturacion.InterfaceRestViva
{
    public class MWFacturacionViva
    {

        private string UriRESTFacturacion { get; set; }
        private BLLBitacoraErrores BllLogErrores { get; set; }
        public string PNR { get; set; }
        public string MensajeErrorUsuario { get; set; }

        public BLLTicketFactura bllTicket;
        public List<ENTImpresoraTermica> ListaImpresorasDisponibles = new List<ENTImpresoraTermica>();
        public string UsuarioRest { get; set; }
        public string PwdRest { get; set; }

        public MWFacturacionViva()
        {
            RecuperarParametros();
            BllLogErrores = new BLLBitacoraErrores();
            bllTicket = new BLLTicketFactura();
            ListaImpresorasDisponibles = bllTicket.ListaImpresorasTermicas;
        }

        private void RecuperarParametros()
        {
            BLLParametrosCnf bllParam = new BLLParametrosCnf();
            List<ENTParametrosCnf> listaParametros = new List<ENTParametrosCnf>();
            listaParametros = bllParam.RecuperarTodo();

            if (listaParametros.Where(x => x.Nombre == "UriRestFacturacion" && x.Activo == true).Count() > 0)
            {
                ENTParametrosCnf paramUri = listaParametros.Where(x => x.Nombre == "UriRestFacturacion" && x.Activo == true).FirstOrDefault();
                UriRESTFacturacion = paramUri.Valor;
            }
            else
            {
                UriRESTFacturacion = "";
            }

            if (listaParametros.Where(x => x.Nombre == "MsjErrorPortal").Count() > 0)
            {
                MensajeErrorUsuario = listaParametros.Where(x => x.Nombre == "MsjErrorPortal").FirstOrDefault().Valor;
            }
            else
            {
                MensajeErrorUsuario = "Por el momento no es posible procesar su solicitud por favor intente más tarde...";

            }

            if (listaParametros.Where(x => x.Nombre == "USERREST" && x.Activo == true).Count() > 0)
            {
                ENTParametrosCnf paramUri = listaParametros.Where(x => x.Nombre == "USERREST" && x.Activo == true).FirstOrDefault();
                UsuarioRest = paramUri.Valor;
            }
            else
            {
                UsuarioRest = "VBREST";
            }

            BLLUsuariosCat bllUsuario = new BLLUsuariosCat();
            bllUsuario.RecuperarUsuariosCatUsuario("VBREST");

            if (bllUsuario.IdUsuario > 0)
            {
                PwdRest = bllUsuario.Password;
            }


        }


        public List<ENTPagosSinFacturar> BuscarPagosPorPNRDeTrafico(string pnr)
        {
            List<ENTPagosSinFacturar> listaResult = new List<ENTPagosSinFacturar>();

            ENTDatosFacturacionPorRest datosParaFacturar = new ENTDatosFacturacionPorRest();
            datosParaFacturar.ClaveReservacion = pnr;
            datosParaFacturar.Usuario = UsuarioRest;
            datosParaFacturar.Password = PwdRest;

            string uri = string.Format("{0}/facturacion/BuscarPagosPorPNRDeTrafico", UriRESTFacturacion);
            string resultPagos = InvocarRestPost(uri, datosParaFacturar);

            listaResult = new JavaScriptSerializer().Deserialize<List<ENTPagosSinFacturar>>(resultPagos);

            return listaResult;
        }


        public List<ENTPagosFacturadosREST> GenerarFacturasPorListaPagos(ENTDatosFacturacionPorRest datosParaFacturar)
        {
            //BLLFacturacion bllFacturacion = new BLLFacturacion(true);
            List<ENTPagosFacturadosREST> listaPagosFacturados = new List<ENTPagosFacturadosREST>();
            datosParaFacturar.Usuario = UsuarioRest;
            datosParaFacturar.Password = PwdRest;

            string uri = string.Format("{0}/facturacion/GenerarFacturasDeTrafico", UriRESTFacturacion);

            string resultPagos = InvocarRestPost(uri, datosParaFacturar);


            listaPagosFacturados = new JavaScriptSerializer().Deserialize<List<ENTPagosFacturadosREST>>(resultPagos);

            //Inicia el proceso de impresion de Ticket Factura
            if (ListaImpresorasDisponibles.Count > 0)
            {
                StringBuilder resultadoImpresion = new StringBuilder();
                foreach (ENTPagosFacturadosREST factura in listaPagosFacturados)
                {
                    string resultImp = "";
                    resultImp = GenerarTicketFactura(factura.CFDI, factura.CadenaOriginal);
                    resultadoImpresion.Append(resultImp);
                }
            }


            return listaPagosFacturados;
        }





        private string InvocarRestPost(string url, Dictionary<string, string> listaParam)
        {
            string result = "";
            StringBuilder urlFinal = new StringBuilder();
            urlFinal.Append(url);
            urlFinal.Append("?");
            string sep = "";
            foreach (var param in listaParam)
            {
                urlFinal.Append(sep);
                urlFinal.Append(string.Format("{0}={1}", param.Key, param.Value));
                sep = "&";
            }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(urlFinal.ToString());
            httpWebRequest.Timeout = 100 * 1000;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            //string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(UsuarioBoxEver + ":" + PasswordBoxEver));
            httpWebRequest.Accept = "application/json";
            //httpWebRequest.Headers.Add("authorization", "Basic " + credentials);
            ASCIIEncoding encoder = new ASCIIEncoding();

            string json = new JavaScriptSerializer().Serialize("");
            byte[] data = encoder.GetBytes(json); // a json object, or xml, whatever...
            httpWebRequest.GetRequestStream().Write(data, 0, data.Length);

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resultado = streamReader.ReadToEnd();
                result = resultado;
            }

            return result;
        }

        private string InvocarRestPost(string url, ENTDatosFacturacion datosFacturacion)
        {
            string result = "";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Timeout = 100 * 1000;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            //string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(UsuarioBoxEver + ":" + PasswordBoxEver));
            httpWebRequest.Accept = "application/json";
            //httpWebRequest.Headers.Add("authorization", "Basic " + credentials);


            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = new JavaScriptSerializer().Serialize(datosFacturacion);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var resultado = streamReader.ReadToEnd();
                result = resultado;
            }

            return result;
        }

        private string GenerarTicketFactura(string cfdi, string cadenaOriginal)
        {
            string result = "";

            try
            {
                //Recuperar informacion del CFDI del pago a imprimir
                string xmlCFDI = cfdi;
                //Se genera el string que se va a imprimir en la impresora
                string infoTicketFactura = "";

                //Se genera el cuerpo de la factura
                string codigoQR = "";
                infoTicketFactura = bllTicket.GeneraBodyTicketFactura(xmlCFDI, cadenaOriginal, ref codigoQR);

                //Se envia a imprimir el ticket factura
                bllTicket.GenerarTicketFactura(infoTicketFactura, codigoQR);

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "EnviarCorreoArchivos");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result;
        }
    }
}

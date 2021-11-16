using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Facturacion.BLL.Pegaso;
using System.Data.SqlClient;

namespace Facturacion.BLL.ProcesoFacturacion
{
    public class BLLPegaso
    {

        #region Propiedades privadas
        private BLLBitacoraErrores BllLogErrores { get; set; }
        private List<ENTParametrosCnf> ListaParametros { get; set; }
        public string PNR { get; set; }
        public string MensajeErrorUsuario { get; set; }

        public string ApiKeyFacturalo { get; set; }

        public string PAC { get; set; }

        public string RutaFTPSAT { get; set; }

        public int TimeOutWSFacturalo { get; set; }

        #endregion
        public BLLPegaso()
        {
            BllLogErrores = new BLLBitacoraErrores();
            try
            {

                ListaParametros = new List<ENTParametrosCnf>();

                BLLParametrosCnf bllParam = new BLL.BLLParametrosCnf();
                BLLFacturacion bllFac = new BLLFacturacion();
                ListaParametros = bllFac.RecuperarParametros();

                if (ListaParametros.Where(x => x.Nombre == "MsjErrorPortal").Count() > 0)
                {
                    MensajeErrorUsuario = ListaParametros.Where(x => x.Nombre == "MsjErrorPortal").FirstOrDefault().Valor;
                }
                else
                {
                    MensajeErrorUsuario = "Por el momento no es posible procesar su solicitud por favor intente más tarde...";

                }

                //LCI. INI. 2018-04-11 Implementar PAC Facturalo
                if (ListaParametros.Where(x => x.Nombre == "ApiKeyFact").Count() > 0)
                {
                    ApiKeyFacturalo = ListaParametros.Where(x => x.Nombre == "ApiKeyFact").FirstOrDefault().Valor;
                }

                if (ListaParametros.Where(x => x.Nombre == "PAC").Count() > 0)
                {
                    PAC = ListaParametros.Where(x => x.Nombre == "PAC").FirstOrDefault().Valor;
                }



                //LCI. FIN. 2018-04-11 Implementar PAC Facturalo

                //LCI. INI. 2018-08-07 Enviar CFDI a FTP SAT
                if (ListaParametros.Where(x => x.Nombre == "RutaCFDISAT").Count() > 0)
                {
                    RutaFTPSAT = ListaParametros.Where(x => x.Nombre == "RutaCFDISAT").FirstOrDefault().Valor;
                }
                else
                {
                    RutaFTPSAT = @"D:\Facturacion33\CFDI";

                }


                //LCI. FIN. 2018-08-07 Enviar CFDI a FTP SAT
                int numRegTimeout = 0;
                numRegTimeout = ListaParametros.Where(x => x.Nombre == "TimeoutWsFacturalo").Count();
                //LCI. INI. 2018-09-10 Recuperar parametro de Timeout para los WS de Facturalo
                if (numRegTimeout > 0)
                {
                    TimeOutWSFacturalo  = Convert.ToInt32(ListaParametros.Where(x => x.Nombre == "TimeoutWsFacturalo").FirstOrDefault().Valor);
                }
                else
                {
                    TimeOutWSFacturalo = 900000;
                }

                //LCI. INI. 2018-09-10 Recuperar parametro de Timeout para los WS de Facturalo


            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", "Constructor");
                throw new ExceptionViva(mensajeUsuario);
            }
        }


        private ENTXmlPegaso EnviaTimbradoPegaso(string xmlRequestFact, long folioCFDI, string tipoComprobante, string pnr)
        {

            ENTXmlPegaso result = new ENTXmlPegaso();
            string xmlResponse = "";
            string respuestaXML = "";
            string ClasifErrorTimbrado = "";
            bool esCorrecto = false;
            DateTime fechaPeticion = new DateTime();
            DataSet dsRespuesta = new DataSet();
            try
            {
                PNR = pnr;
                //Enviamos el XMLRequest al Proveedor Pegaso, para obtener el XMLResponse
                fechaPeticion = DateTime.Now;

                string Ambiente = string.Empty;

                Ambiente = ListaParametros.Where(x => x.Nombre == "AmbientePegasoTimbrado").FirstOrDefault().Valor;

                //ServicioPegaso QA
                EmisionBaseExternalServiceClient clientePegaso = new EmisionBaseExternalServiceClient(Ambiente);

                XmlDocument docXml = new XmlDocument();
                docXml.LoadXml(xmlRequestFact.ToString());
                XmlElement XMLRequest = docXml.DocumentElement;



                XmlElement xResponse = clientePegaso.emitirCFD(XMLRequest);

                //Obtenemos el XMLResponse de Pegaso
                xmlResponse = xResponse.InnerXml.ToString();

                //Convertimos la respuesta de pegaso en un documento de XML
                XmlDocument xmlDocuResponse = new XmlDocument();
                xmlDocuResponse.LoadXml(string.Format("<response>{0}</response>", xmlResponse));
                XmlNodeReader xmlReader = new XmlNodeReader(xmlDocuResponse);


                dsRespuesta.ReadXml(xmlReader);

                if (dsRespuesta.Tables.Contains("Error"))
                {
                    esCorrecto = false;
                    DataRow drError = dsRespuesta.Tables["Error"].Rows[0];
                    string descripError = string.Format("{0}. {1}", drError["noIdentificacion"].ToString(), drError["descripcion"].ToString());
                    respuestaXML = descripError;
                    if (drError[0].ToString() == "2002")
                    {
                        RecuperaHorasErrorFechaSAT(descripError);
                    }

                    throw new Exception(descripError);
                }
                else
                {
                    esCorrecto = true;
                    respuestaXML = dsRespuesta.Tables["Transaccion"].Rows[0]["Tipo"].ToString();
                    respuestaXML += ". " + dsRespuesta.Tables["Transaccion"].Rows[0]["Estatus"].ToString();

                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeExcepcion = ex.Message;

                ClasifErrorTimbrado = ClasificarErrorTimbrado(ex);
                //LCI. INI. 2018-02-08 PERSONALIZAR MENSAJE ERROR PEGASO
                string codigosErrorParam = string.Empty;

                codigosErrorParam = ListaParametros.Where(x => x.Nombre == "CodigosPegasoError").FirstOrDefault().Valor;

                List<string> codigosErrorPegaso = new List<string>();
                if (codigosErrorParam != null && codigosErrorParam.Length > 0)
                {
                    foreach (string codParam in codigosErrorParam.Split(','))
                    {
                        codigosErrorPegaso.Add(codParam);
                    }

                }

                string codigoErrorExistente = "";
                string mensajeUsuario = "";
                //Recorre los codigos de error para verificar si existe alguno de estos en el mensaje de retorno
                foreach (string codigoError in codigosErrorPegaso)
                {
                    if (mensajeExcepcion.Contains(codigoError))
                    {
                        codigoErrorExistente = codigoError;
                    }
                }




                if (codigoErrorExistente == null || codigoErrorExistente.Length == 0)
                {
                    //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    mensajeUsuario = MensajeErrorUsuario;
                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", ClasifErrorTimbrado);
                }
                else
                {
                    int iniciaMensaje = 0;
                    int finMensaje = 0;
                    string mensajePegaso = "";
                    iniciaMensaje = mensajeExcepcion.ToString().IndexOf(codigoErrorExistente);
                    if (iniciaMensaje > 0)
                    {
                        mensajePegaso = mensajeExcepcion.Substring(iniciaMensaje);
                        finMensaje = mensajePegaso.ToString().IndexOf("----");
                        if (finMensaje > 0)
                        {
                            mensajePegaso = mensajePegaso.Substring(0, finMensaje).Trim();
                        }
                        else
                        {
                            mensajePegaso = mensajePegaso.Trim();
                        }

                        mensajeUsuario = mensajePegaso;
                    }
                    else
                    {
                        //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                        mensajeUsuario = MensajeErrorUsuario;
                    }

                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", ClasifErrorTimbrado, false);
                }
                //LCI. FIN. 2018-02-08 PERSONALIZAR MENSAJE ERROR PEGASO
                throw new ExceptionViva(mensajeUsuario);
            }
            finally
            {
                try
                {


                    //Se recupera la informacion de la Respuesta de Pegaso
                    BLLXmlPegaso regXmlPegaso = new BLLXmlPegaso();
                    regXmlPegaso.IdPeticionPAC = 0;
                    regXmlPegaso.FechaPeticion = fechaPeticion;
                    regXmlPegaso.FolioCFDI = folioCFDI;
                    regXmlPegaso.TipoComprobante = tipoComprobante;
                    regXmlPegaso.XMLRequest = xmlRequestFact;
                    regXmlPegaso.XMLResponse = xmlResponse;
                    regXmlPegaso.MensajeRespuesta = respuestaXML;
                    regXmlPegaso.EsCorrecto = esCorrecto;
                    regXmlPegaso.PAC = "PEGASO";


                    if (dsRespuesta.Tables.Contains("Transaccion") && dsRespuesta.Tables["Transaccion"].Rows.Count > 0)
                    {
                        DataRow drTrans = dsRespuesta.Tables["Transaccion"].Rows[0];
                        if (dsRespuesta.Tables["Transaccion"].Columns.Contains("id") && !drTrans.IsNull("id")) regXmlPegaso.Transaccion_Id = drTrans["id"].ToString();
                        if (dsRespuesta.Tables["Transaccion"].Columns.Contains("tipo") && !drTrans.IsNull("tipo")) regXmlPegaso.Transaccion_Tipo = drTrans["tipo"].ToString();
                        if (dsRespuesta.Tables["Transaccion"].Columns.Contains("estatus") && !drTrans.IsNull("estatus"))
                        {
                            regXmlPegaso.Transaccion_Estatus = drTrans["estatus"].ToString();
                            regXmlPegaso.MensajeRespuesta = regXmlPegaso.Transaccion_Estatus;
                        }
                    }
                    else
                    {
                        regXmlPegaso.MensajeRespuesta = "No se obtuvo respuesta al intentar timbrar la factura...";
                    }


                    if (dsRespuesta.Tables.Contains("CFD") && dsRespuesta.Tables["CFD"].Rows.Count > 0)
                    {
                        DataRow drcfd = dsRespuesta.Tables["CFD"].Rows[0];
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("cadenaOriginal") && !drcfd.IsNull("cadenaOriginal")) regXmlPegaso.CFD_CadenaOriginal = drcfd["cadenaOriginal"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("noCertificado") && !drcfd.IsNull("noCertificado")) regXmlPegaso.CFD_NoCertificado = drcfd["noCertificado"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("comprobanteStr") && !drcfd.IsNull("comprobanteStr")) regXmlPegaso.CFD_ComprobanteStr = drcfd["comprobanteStr"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("serie") && !drcfd.IsNull("serie")) regXmlPegaso.CFD_Serie = drcfd["serie"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("codigoDeBarras") && !drcfd.IsNull("codigoDeBarras")) regXmlPegaso.CFD_CodigoDeBarras = drcfd["codigoDeBarras"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("fecha") && !drcfd.IsNull("fecha")) regXmlPegaso.CFD_Fecha = Convert.ToDateTime(drcfd["fecha"]);
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("folio") && !drcfd.IsNull("folio")) regXmlPegaso.CFD_Folio = Convert.ToInt64(drcfd["folio"]);
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("sello") && !drcfd.IsNull("sello")) regXmlPegaso.CFD_Sello = drcfd["sello"].ToString();
                    }

                    if (dsRespuesta.Tables.Contains("TFD") && dsRespuesta.Tables["TFD"].Rows.Count > 0)
                    {
                        DataRow drtfd = dsRespuesta.Tables["TFD"].Rows[0];
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("UUID") && !drtfd.IsNull("UUID")) regXmlPegaso.TFD_UUID = drtfd["UUID"].ToString();
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("FechaTimbrado") && !drtfd.IsNull("FechaTimbrado"))
                        {
                            DateTime fechaTim = Convert.ToDateTime(drtfd["FechaTimbrado"]);
                            regXmlPegaso.TFD_FechaTimbrado = fechaTim;
                            regXmlPegaso.FechaTimbrado = fechaTim;
                        }
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("noCertificadoSAT") && !drtfd.IsNull("noCertificadoSAT")) regXmlPegaso.TFD_NoCertificadoSAT = drtfd["noCertificadoSAT"].ToString();
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("selloSAT") && !drtfd.IsNull("selloSAT")) regXmlPegaso.TFD_SelloSAT = drtfd["selloSAT"].ToString();
                    }

                    if (dsRespuesta.Tables.Contains("Error") && dsRespuesta.Tables["Error"].Rows.Count > 0)
                    {
                        regXmlPegaso.EsCorrecto = false;
                        DataRow drError = dsRespuesta.Tables["Error"].Rows[0];
                        if (dsRespuesta.Tables["Error"].Columns.Contains("noIdentificacion") && !drError.IsNull("noIdentificacion")) regXmlPegaso.MensajeRespuesta = drError["noIdentificacion"].ToString() + ". ";
                        if (dsRespuesta.Tables["Error"].Columns.Contains("descripcion") && !drError.IsNull("descripcion")) regXmlPegaso.MensajeRespuesta += drError["descripcion"].ToString();
                    }



                    regXmlPegaso.FechaHoraLocal = new DateTime();
                    regXmlPegaso.Agregar();


                    result = regXmlPegaso;
                }
                catch (ExceptionViva ex)
                {
                    throw ex;
                }
                catch (SqlException ex)
                {
                    //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                    string mensajeUsuario = MensajeErrorUsuario;
                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", "BD");
                    throw new ExceptionViva(mensajeUsuario);
                }
                catch (Exception ex)
                {
                    //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    string mensajeUsuario = MensajeErrorUsuario;
                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", "EnviaTimbradoPegaso");
                    throw new ExceptionViva(mensajeUsuario);
                }
            }

            return result;

        }

        public ENTXmlPegaso EnviaTimbrado(string xmlRequestFact, long folioCFDI, string tipoComprobante, string pnr, bool flg_Terceros)
        {

            ENTXmlPegaso result = new ENTXmlPegaso();
            bool timbradoOK = false;
            try
            {
                //VERIFICA EL PROVEEDOR DE TIMBRADO
                if (PAC.Length == 0)
                {
                    //SI NO LO TIENE ASIGNADO ENTONCES PREDEFINE A PEGASO
                    PAC = "PEGASO";
                }

                if (PAC == "FACTURALO")
                {
                    result = EnviaTimbradoFacturalo(xmlRequestFact, folioCFDI, tipoComprobante, pnr);

                }
                else
                {
                    result = EnviaTimbradoPegaso(xmlRequestFact, folioCFDI, tipoComprobante, pnr);
                }
                timbradoOK = true;
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, PAC, "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, PAC, "EnviaTimbrado");
                throw new ExceptionViva(mensajeUsuario);
            }
            finally
            {
                if (timbradoOK && result.EsCorrecto)
                {
                    string xmlCfdi = result.CFD_ComprobanteStr;
                    string ruta = "";
                    string nombreArchivo = "";
                    //D:\Facturacion33\CFDI\2018\JUN\01\
                    //RutaFTPSAT = "c:\\Facturacion33\\CFDI";
                    ruta = RutaFTPSAT + "\\" + result.FechaTimbrado.Year.ToString() + "\\" + result.FechaTimbrado.Month.ToString("00") + "_" + NombreMes(result.FechaTimbrado.Month) + "\\" + result.FechaTimbrado.Day.ToString("00") + "\\";
                    string tipoCFDI = "";

                    tipoCFDI = result.TipoComprobante == "I" ? "FA" : result.TipoComprobante;

                    nombreArchivo = string.Format("{0}_{1}_{2}", result.IdPeticionPAC, tipoCFDI, result.Transaccion_Id.Replace("-", "_"));

                    try
                    {
                        string rutaCompleta = ruta + nombreArchivo + ".xml";
                        BLLXmlFtpReg bllFtp = new BLLXmlFtpReg();
                        bllFtp.IdPeticionPAC = result.IdPeticionPAC;
                        bllFtp.FechaTimbrado = result.TFD_FechaTimbrado;
                        bllFtp.RutaCFDI = rutaCompleta;

                        BLLPDFCFDI bllcfdi = new BLLPDFCFDI();
                        bllcfdi.GeneraArchivoCFDI(xmlCfdi, ruta, nombreArchivo + ".xml");
                        if (result.TipoComprobante == "I")
                        {
                            bllcfdi.GeneraPDFFactura33(xmlCfdi, result.CFD_CadenaOriginal, pnr, ruta, nombreArchivo, flg_Terceros);
                            bllFtp.RutaPDF = ruta + nombreArchivo + ".pdf";
                        }

                        bllFtp.Agregar();

                    }
                    catch (Exception ex)
                    {
                        string mensajeUsuario = MensajeErrorUsuario;
                        BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, PAC, "EnviaTimbrado");
                        throw new ExceptionViva(mensajeUsuario);
                    }
                }
            }

            return result;

        }


        private string NombreMes(int mes)
        {
            string result = "";

            switch (mes)
            {
                case 1:
                    result = "ENE";
                    break;
                case 2:
                    result = "FEB";
                    break;
                case 3:
                    result = "MAR";
                    break;
                case 4:
                    result = "ABR";
                    break;
                case 5:
                    result = "MAY";
                    break;
                case 6:
                    result = "JUN";
                    break;
                case 7:
                    result = "JUL";
                    break;
                case 8:
                    result = "AGO";
                    break;
                case 9:
                    result = "SEP";
                    break;
                case 10:
                    result = "OCT";
                    break;
                case 11:
                    result = "NOV";
                    break;
                case 12:
                    result = "DIC";
                    break;
                default:
                    result = "OTRO";
                    break;
            }
            return result;

        }
        private string ClasificarErrorTimbrado(Exception ex)
        {
            string result = "";

            if (ex.Message.Contains("This is often caused by an incorrect address or SOAP action"))
            {
                result = "WSPegaso";
            }
            else
            {
                result = "EnviaTimbradoPegaso";
            }


            return result;
        }

        public ENTXmlPegaso EnviaTimbradoPegasoGlobal(string xmlRequestFact, long folioCFDI, string tipoComprobante)
        {
            ENTXmlPegaso result = new ENTXmlPegaso();
            string xmlResponse = "";
            string respuestaXML = "";
            bool esCorrecto = false;
            DateTime fechaPeticion = new DateTime();
            DataSet dsRespuesta = new DataSet();
            try
            {

                string Ambiente = string.Empty;

                Ambiente = "QAGLOBAL";// ListaParametros.Where(x => x.Nombre == "AmbientePegasoTimbrado").FirstOrDefault().Valor;

                //ServicioPegaso QA
                PegasoGlobal.IntegracionSAPServiceClient clientePegaso = new PegasoGlobal.IntegracionSAPServiceClient(Ambiente);

                XmlDocument docXml = new XmlDocument();
                docXml.LoadXml(xmlRequestFact.ToString());
                XmlElement XMLRequest = docXml.DocumentElement;


                //Enviamos el XMLRequest al Proveedor Pegaso, para obtener el XMLResponse
                fechaPeticion = DateTime.Now;
                XmlElement xResponse = clientePegaso.EmisionRapidaCFDXml(XMLRequest);

                //Obtenemos el XMLResponse de Pegaso
                xmlResponse = xResponse.InnerXml.ToString();

                //Convertimos la respuesta de pegaso en un documento de XML
                XmlDocument xmlDocuResponse = new XmlDocument();
                xmlDocuResponse.LoadXml(string.Format("<response>{0}</response>", xmlResponse));
                XmlNodeReader xmlReader = new XmlNodeReader(xmlDocuResponse);


                dsRespuesta.ReadXml(xmlReader);

                if (dsRespuesta.Tables.Contains("Error"))
                {
                    esCorrecto = false;
                    DataRow drError = dsRespuesta.Tables["Error"].Rows[0];
                    string descripError = string.Format("{0}. {1}", drError["noIdentificacion"].ToString(), drError["descripcion"].ToString());
                    respuestaXML = descripError;
                    if (drError[0].ToString() == "2002")
                    {
                        RecuperaHorasErrorFechaSAT(descripError);
                    }

                    throw new Exception(descripError);
                }
                else
                {
                    esCorrecto = true;
                    respuestaXML = dsRespuesta.Tables["Transaccion"].Rows[0]["Tipo"].ToString();
                    respuestaXML += ". " + dsRespuesta.Tables["Transaccion"].Rows[0]["Estatus"].ToString();

                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", "EnviarTimbradoPegasoGlobal");
                throw new ExceptionViva(mensajeUsuario);
            }

            finally
            {
                try
                {


                    //Se recupera la informacion de la Respuesta de Pegaso
                    BLLXmlPegaso regXmlPegaso = new BLLXmlPegaso();
                    regXmlPegaso.IdPeticionPAC = 0;
                    regXmlPegaso.FechaPeticion = fechaPeticion;
                    regXmlPegaso.FolioCFDI = folioCFDI;
                    regXmlPegaso.TipoComprobante = tipoComprobante;
                    regXmlPegaso.XMLRequest = xmlRequestFact;
                    regXmlPegaso.XMLResponse = xmlResponse;
                    regXmlPegaso.MensajeRespuesta = respuestaXML;
                    regXmlPegaso.EsCorrecto = esCorrecto;


                    if (dsRespuesta.Tables.Contains("Transaccion") && dsRespuesta.Tables["Transaccion"].Rows.Count > 0)
                    {
                        DataRow drTrans = dsRespuesta.Tables["Transaccion"].Rows[0];
                        if (dsRespuesta.Tables["Transaccion"].Columns.Contains("id") && !drTrans.IsNull("id")) regXmlPegaso.Transaccion_Id = drTrans["id"].ToString();
                        if (dsRespuesta.Tables["Transaccion"].Columns.Contains("tipo") && !drTrans.IsNull("tipo")) regXmlPegaso.Transaccion_Tipo = drTrans["tipo"].ToString();
                        if (dsRespuesta.Tables["Transaccion"].Columns.Contains("estatus") && !drTrans.IsNull("estatus"))
                        {
                            regXmlPegaso.Transaccion_Estatus = drTrans["estatus"].ToString();
                            regXmlPegaso.MensajeRespuesta = regXmlPegaso.Transaccion_Estatus;
                        }
                    }
                    else
                    {
                        regXmlPegaso.MensajeRespuesta = "No se recibio respuesta al intentar timbrar la factura global";
                    }


                    if (dsRespuesta.Tables.Contains("CFD") && dsRespuesta.Tables["CFD"].Rows.Count > 0)
                    {
                        DataRow drcfd = dsRespuesta.Tables["CFD"].Rows[0];
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("cadenaOriginal") && !drcfd.IsNull("cadenaOriginal")) regXmlPegaso.CFD_CadenaOriginal = drcfd["cadenaOriginal"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("noCertificado") && !drcfd.IsNull("noCertificado")) regXmlPegaso.CFD_NoCertificado = drcfd["noCertificado"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("comprobanteStr") && !drcfd.IsNull("comprobanteStr")) regXmlPegaso.CFD_ComprobanteStr = drcfd["comprobanteStr"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("serie") && !drcfd.IsNull("serie")) regXmlPegaso.CFD_Serie = drcfd["serie"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("codigoDeBarras") && !drcfd.IsNull("codigoDeBarras")) regXmlPegaso.CFD_CodigoDeBarras = drcfd["codigoDeBarras"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("fecha") && !drcfd.IsNull("fecha")) regXmlPegaso.CFD_Fecha = Convert.ToDateTime(drcfd["fecha"]);
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("folio") && !drcfd.IsNull("folio")) regXmlPegaso.CFD_Folio = Convert.ToInt64(drcfd["folio"]);
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("sello") && !drcfd.IsNull("sello")) regXmlPegaso.CFD_Sello = drcfd["sello"].ToString();
                    }

                    if (dsRespuesta.Tables.Contains("TFD") && dsRespuesta.Tables["TFD"].Rows.Count > 0)
                    {
                        DataRow drtfd = dsRespuesta.Tables["TFD"].Rows[0];
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("UUID") && !drtfd.IsNull("UUID")) regXmlPegaso.TFD_UUID = drtfd["UUID"].ToString();
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("FechaTimbrado") && !drtfd.IsNull("FechaTimbrado"))
                        {
                            DateTime fechaTim = Convert.ToDateTime(drtfd["FechaTimbrado"]);
                            regXmlPegaso.TFD_FechaTimbrado = fechaTim;
                            regXmlPegaso.FechaTimbrado = fechaTim;
                        }
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("noCertificadoSAT") && !drtfd.IsNull("noCertificadoSAT")) regXmlPegaso.TFD_NoCertificadoSAT = drtfd["noCertificadoSAT"].ToString();
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("selloSAT") && !drtfd.IsNull("selloSAT")) regXmlPegaso.TFD_SelloSAT = drtfd["selloSAT"].ToString();
                    }

                    if (dsRespuesta.Tables.Contains("Error") && dsRespuesta.Tables["Error"].Rows.Count > 0)
                    {
                        regXmlPegaso.EsCorrecto = false;
                        DataRow drError = dsRespuesta.Tables["Error"].Rows[0];
                        if (dsRespuesta.Tables["Error"].Columns.Contains("noIdentificacion") && !drError.IsNull("noIdentificacion")) regXmlPegaso.MensajeRespuesta = drError["noIdentificacion"].ToString() + ". ";
                        if (dsRespuesta.Tables["Error"].Columns.Contains("descripcion") && !drError.IsNull("descripcion")) regXmlPegaso.MensajeRespuesta += drError["descripcion"].ToString();
                    }




                    regXmlPegaso.FechaHoraLocal = new DateTime();
                    regXmlPegaso.Agregar();


                    result = regXmlPegaso;
                }
                catch (ExceptionViva ex)
                {
                    throw ex;
                }
                catch (SqlException ex)
                {
                    //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                    string mensajeUsuario = MensajeErrorUsuario;
                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", "BD");
                    throw new ExceptionViva(mensajeUsuario);
                }
                catch (Exception ex)
                {
                    string mensajeUsuario = "Por el momento no es posible procesar la factura Global...";

                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", "EnviarTimbradoPegasoGlobal");
                    throw new ExceptionViva(mensajeUsuario);
                }
            }

            return result;

        }


        private void RecuperaHorasErrorFechaSAT(string msjErrorPegaso)
        {
            try
            {
                List<DateTime> listaFechas = new List<DateTime>();
                //Ejemplo de la descripcion: "La fecha de 6/19/2017 9:49:06 AM no puede ser mayor a la fecha actual : 6/19/2017 9:47:47 AM"
                List<string> sepMsj = msjErrorPegaso.Split(' ').ToList();

                for (int i = 0; i < sepMsj.Count; i++)
                {

                    string valor = sepMsj[i];
                    if (valor.Contains("/"))
                    {
                        DateTime fechaTest;
                        List<string> sepFecha = valor.Split('/').ToList();
                        int numMes = 0;
                        int numDia = 0;
                        int numAnio = 0;
                        int.TryParse(sepFecha[0], out numMes);
                        int.TryParse(sepFecha[1], out numDia);
                        int.TryParse(sepFecha[2], out numAnio);

                        string hora = sepMsj[i + 1];
                        List<string> sepHora = hora.Split(':').ToList();
                        int numHora = 0;
                        int numMin = 0;
                        int numSeg = 0;
                        int.TryParse(sepHora[0], out numHora);
                        int.TryParse(sepHora[1], out numMin);
                        int.TryParse(sepHora[2], out numSeg);

                        if (sepMsj[i + 2].ToString() == "PM")
                        {
                            numHora += 12;
                        }
                        fechaTest = new DateTime(numAnio, numMes, numDia, numHora, numMin, numSeg);

                        listaFechas.Add(fechaTest);
                        if ((i + 2) < sepMsj.Count)
                        {
                            i += 2;
                        }

                    }



                }

                if (listaFechas.Count == 2)
                {
                    TimeSpan difHoraServidor = listaFechas[0] - listaFechas[1];
                    int segundoDif = (int)difHoraServidor.TotalSeconds;
                    int difActual = ObtieneAjusteSegFechaSAT();
                    //Enviar actualizacion de Ajuste a parametro en BD.
                    BLLParametrosCnf bllParamDesfase = new BLL.BLLParametrosCnf();
                    bllParamDesfase.RecuperarParametrosCnfNombre("SegundosDesfase");
                    int nuevoTiempo = segundoDif + difActual + 40;
                    bllParamDesfase.Valor = nuevoTiempo.ToString();
                    bllParamDesfase.Actualizar();
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", "RecuperarHorasErrorFechaSAT");
                throw new ExceptionViva(mensajeUsuario);
            }
        }

        public int ObtieneAjusteSegFechaSAT()
        {
            int result = 60;

            try
            {
                BLLParametrosCnf bllParamDesfase = new BLL.BLLParametrosCnf();
                bllParamDesfase.RecuperarParametrosCnfNombre("SegundosDesfase");
                if (bllParamDesfase.IdParametro > 0)
                {
                    result = Convert.ToInt16(bllParamDesfase.Valor);
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Pegaso", "ObtieneAjusteSegFechaSAT");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }

        //*************************************************************************************************
        //LCI. INI. 218-04-10 IMPLEMENTACION DE FACTURACION CON WS DE FACTURALO
        #region WsFacturalo
        private ENTXmlPegaso EnviaTimbradoFacturalo(string xmlRequestFact, long folioCFDI, string tipoComprobante, string pnr)
        {

            ENTXmlPegaso result = new ENTXmlPegaso();
            string xmlResponse = "";
            string respuestaXML = "";
            string ClasifErrorTimbrado = "";
            bool esCorrecto = false;
            DateTime fechaPeticion = new DateTime();
            DataSet dsRespuesta = new DataSet();
            try
            {
                PNR = pnr;
                //Enviamos el XMLRequest al Proveedor Pegaso, para obtener el XMLResponse
                fechaPeticion = DateTime.Now;

                //Invocar timbrado Facturalo
                string request = "";

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(xmlRequestFact);
                request = System.Convert.ToBase64String(plainTextBytes);

                Facturalo.FacturaloTimbradoWS factEnitma = new Facturalo.FacturaloTimbradoWS();
                factEnitma.Timeout = TimeOutWSFacturalo;
                Facturalo.RespuestaTimbrado response = new Facturalo.RespuestaTimbrado();
                response = factEnitma.timbrar(ApiKeyFacturalo, request);

                //Convertimos la respuesta de pegaso en un documento de XML
                XmlDocument xmlDocuResponse = new XmlDocument();
                xmlResponse = response.response.Replace("<?xml version=\"1.0\"?>", "");
                xmlDocuResponse.LoadXml(string.Format("<response>{0}</response>", xmlResponse));
                XmlNodeReader xmlReader = new XmlNodeReader(xmlDocuResponse);

                dsRespuesta.ReadXml(xmlReader);

                if (dsRespuesta.Tables.Contains("Error"))
                {
                    esCorrecto = false;
                    DataRow drError = dsRespuesta.Tables["Error"].Rows[0];
                    string descripError = string.Format("{0}. {1}", drError["Code"].ToString(), drError["Message"].ToString());
                    respuestaXML = descripError;
                    if (drError[0].ToString() == "2002")
                    {
                        RecuperaHorasErrorFechaSAT(descripError);
                    }

                    throw new Exception(descripError);
                }
                else
                {
                    esCorrecto = true;
                    respuestaXML = dsRespuesta.Tables["Transaccion"].Rows[0]["Tipo"].ToString();
                    respuestaXML += ". " + dsRespuesta.Tables["Transaccion"].Rows[0]["Estatus"].ToString();

                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "FACTURALO", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeExcepcion = ex.Message;

                ClasifErrorTimbrado = ClasificarErrorTimbrado(ex);
                //LCI. INI. 2018-02-08 PERSONALIZAR MENSAJE ERROR PEGASO
                string codigosErrorParam = string.Empty;

                codigosErrorParam = ListaParametros.Where(x => x.Nombre == "CodigosPegasoError").FirstOrDefault().Valor;

                List<string> codigosErrorPegaso = new List<string>();
                if (codigosErrorParam != null && codigosErrorParam.Length > 0)
                {
                    foreach (string codParam in codigosErrorParam.Split(','))
                    {
                        codigosErrorPegaso.Add(codParam);
                    }

                }

                string codigoErrorExistente = "";
                string mensajeUsuario = "";
                //Recorre los codigos de error para verificar si existe alguno de estos en el mensaje de retorno
                foreach (string codigoError in codigosErrorPegaso)
                {
                    if (mensajeExcepcion.Contains(codigoError))
                    {
                        codigoErrorExistente = codigoError;
                    }
                }




                if (codigoErrorExistente == null || codigoErrorExistente.Length == 0)
                {
                    //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    mensajeUsuario = MensajeErrorUsuario;
                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "FACTURALO", ClasifErrorTimbrado);
                }
                else
                {
                    int iniciaMensaje = 0;
                    int finMensaje = 0;
                    string mensajePegaso = "";
                    iniciaMensaje = mensajeExcepcion.ToString().IndexOf(codigoErrorExistente);
                    if (iniciaMensaje > 0 || mensajeExcepcion.Contains(codigoErrorExistente))
                    {
                        mensajePegaso = mensajeExcepcion.Substring(iniciaMensaje);
                        finMensaje = mensajePegaso.ToString().IndexOf("----");
                        if (finMensaje > 0)
                        {
                            mensajePegaso = mensajePegaso.Substring(0, finMensaje).Trim();
                        }
                        else
                        {
                            mensajePegaso = mensajePegaso.Trim();
                        }

                        mensajeUsuario = mensajePegaso;
                    }
                    else
                    {
                        //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                        mensajeUsuario = MensajeErrorUsuario;
                    }

                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "FACTURALO", ClasifErrorTimbrado, false);
                }
                //LCI. FIN. 2018-02-08 PERSONALIZAR MENSAJE ERROR PEGASO
                throw new ExceptionViva(mensajeUsuario);
            }
            finally
            {
                try
                {


                    //Se recupera la informacion de la Respuesta de Pegaso
                    BLLXmlPegaso regXmlPegaso = new BLLXmlPegaso();
                    regXmlPegaso.IdPeticionPAC = 0;
                    regXmlPegaso.FechaPeticion = fechaPeticion;
                    regXmlPegaso.FolioCFDI = folioCFDI;
                    regXmlPegaso.TipoComprobante = tipoComprobante;
                    regXmlPegaso.XMLRequest = xmlRequestFact;
                    regXmlPegaso.XMLResponse = xmlResponse;
                    regXmlPegaso.MensajeRespuesta = respuestaXML;
                    regXmlPegaso.EsCorrecto = esCorrecto;
                    regXmlPegaso.PAC = "FACTURALO";


                    if (dsRespuesta.Tables.Contains("Transaccion") && dsRespuesta.Tables["Transaccion"].Rows.Count > 0)
                    {
                        DataRow drTrans = dsRespuesta.Tables["Transaccion"].Rows[0];
                        if (dsRespuesta.Tables["Transaccion"].Columns.Contains("id") && !drTrans.IsNull("id")) regXmlPegaso.Transaccion_Id = drTrans["id"].ToString();
                        if (dsRespuesta.Tables["Transaccion"].Columns.Contains("tipo") && !drTrans.IsNull("tipo")) regXmlPegaso.Transaccion_Tipo = drTrans["tipo"].ToString();
                        if (dsRespuesta.Tables["Transaccion"].Columns.Contains("estatus") && !drTrans.IsNull("estatus"))
                        {
                            regXmlPegaso.Transaccion_Estatus = drTrans["estatus"].ToString();
                            regXmlPegaso.MensajeRespuesta = regXmlPegaso.Transaccion_Estatus;
                        }
                    }
                    else
                    {
                        regXmlPegaso.MensajeRespuesta = "No se obtuvo respuesta al intentar timbrar la factura...";
                    }


                    if (dsRespuesta.Tables.Contains("CFD") && dsRespuesta.Tables["CFD"].Rows.Count > 0)
                    {
                        DataRow drcfd = dsRespuesta.Tables["CFD"].Rows[0];
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("cadenaOriginal") && !drcfd.IsNull("cadenaOriginal")) regXmlPegaso.CFD_CadenaOriginal = drcfd["cadenaOriginal"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("noCertificado") && !drcfd.IsNull("noCertificado")) regXmlPegaso.CFD_NoCertificado = drcfd["noCertificado"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("comprobanteStr") && !drcfd.IsNull("comprobanteStr")) regXmlPegaso.CFD_ComprobanteStr = drcfd["comprobanteStr"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("serie") && !drcfd.IsNull("serie")) regXmlPegaso.CFD_Serie = drcfd["serie"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("codigoDeBarras") && !drcfd.IsNull("codigoDeBarras")) regXmlPegaso.CFD_CodigoDeBarras = drcfd["codigoDeBarras"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("fecha") && !drcfd.IsNull("fecha")) regXmlPegaso.CFD_Fecha = Convert.ToDateTime(drcfd["fecha"]);
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("folio") && !drcfd.IsNull("folio")) regXmlPegaso.CFD_Folio = Convert.ToInt64(drcfd["folio"]);
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("sello") && !drcfd.IsNull("sello")) regXmlPegaso.CFD_Sello = drcfd["sello"].ToString();
                    }

                    if (dsRespuesta.Tables.Contains("TFD") && dsRespuesta.Tables["TFD"].Rows.Count > 0)
                    {
                        DataRow drtfd = dsRespuesta.Tables["TFD"].Rows[0];
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("UUID") && !drtfd.IsNull("UUID")) regXmlPegaso.TFD_UUID = drtfd["UUID"].ToString();
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("FechaTimbrado") && !drtfd.IsNull("FechaTimbrado"))
                        {
                            DateTime fechaTim = Convert.ToDateTime(drtfd["FechaTimbrado"]);
                            regXmlPegaso.TFD_FechaTimbrado = fechaTim;
                            regXmlPegaso.FechaTimbrado = fechaTim;
                        }
                        else
                        {
                            regXmlPegaso.TFD_FechaTimbrado = regXmlPegaso.CFD_Fecha;
                            regXmlPegaso.FechaTimbrado = regXmlPegaso.CFD_Fecha;
                        }
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("noCertificadoSAT") && !drtfd.IsNull("noCertificadoSAT")) regXmlPegaso.TFD_NoCertificadoSAT = drtfd["noCertificadoSAT"].ToString();
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("selloSAT") && !drtfd.IsNull("selloSAT")) regXmlPegaso.TFD_SelloSAT = drtfd["selloSAT"].ToString();
                    }

                    if (dsRespuesta.Tables.Contains("Error") && dsRespuesta.Tables["Error"].Rows.Count > 0)
                    {
                        regXmlPegaso.EsCorrecto = false;
                        DataRow drError = dsRespuesta.Tables["Error"].Rows[0];
                        if (dsRespuesta.Tables["Error"].Columns.Contains("Code") && !drError.IsNull("Code")) regXmlPegaso.MensajeRespuesta = drError["Code"].ToString() + ". ";
                        if (dsRespuesta.Tables["Error"].Columns.Contains("Message") && !drError.IsNull("Message")) regXmlPegaso.MensajeRespuesta += drError["Message"].ToString();
                    }



                    regXmlPegaso.FechaHoraLocal = new DateTime();
                    regXmlPegaso.Agregar();


                    result = regXmlPegaso;
                }
                catch (ExceptionViva ex)
                {
                    throw ex;
                }
                catch (SqlException ex)
                {
                    //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                    string mensajeUsuario = MensajeErrorUsuario;
                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "FACTURALO", "BD");
                    throw new ExceptionViva(mensajeUsuario);
                }
                catch (Exception ex)
                {
                    //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    string mensajeUsuario = MensajeErrorUsuario;
                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "FACTURALO", "EnviaTimbradoPegaso");
                    throw new ExceptionViva(mensajeUsuario);
                }
            }

            return result;

        }




        #endregion
        //LCI. FIN. 218-04-10 IMPLEMENTACION DE FACTURACION CON WS DE FACTURALO









    }
}

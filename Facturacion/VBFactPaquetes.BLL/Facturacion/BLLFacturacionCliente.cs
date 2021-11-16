using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VBFactPaquetes.Comun;
using VBFactPaquetes.Comun.Log;
//using VBFactPaquetes.Comun.PDF;
using VBFactPaquetes.DAO.Facturacion;
using VBFactPaquetes.Model.Facturacion;

namespace VBFactPaquetes.BLL.Facturacion
{
    public class BLLFacturacionCliente
    {
        DAOFacturacion daoFacturacion = new DAOFacturacion();

        public String GenerarFactura(Pago pago)
        {
            daoFacturacion = new DAOFacturacion();
            DataTable dtWs = new DataTable();
            string xmlResponse = "";
            string respuestaXML = "";
            //bool esCorrecto = false;
            DataSet dsRespuesta = new DataSet();

            /*CREO LA INSTANCIA A LA NUEVA CLASE GENERARFACTURAEGRESOPDF*/
            //GeneraFactura PDFFactura = new GeneraFactura();

            /*CREO LA INSTANCIA A LA NUEVA CLASE GENERARYCONSTRUIRXML*/
            GenerarYConstruirXML XML = new GenerarYConstruirXML();

            /*CREO LA INSTANCIA A LA NUEVA CLASE BLLDatosYArchivosFactura*/
            BLLDatosYArchivosFactura DYAF = new BLLDatosYArchivosFactura();

            XmlDocument xmlDocuResponse = new XmlDocument();
            String xmlTimbrar = String.Empty;
            /* SE INSTANCIA EL WS DE PAC*/
            //FacturaloTimbradoWSPortTypeClient timbrado = new FacturaloTimbradoWSPortTypeClient();
            ws_Timbrado.FacturaloTimbradoWSPortTypeClient timbrado = new BLL.ws_Timbrado.FacturaloTimbradoWSPortTypeClient();

            /*SE INSTANCIA EL OBJETO QUE CONTENDRÁ LA RESPUESTA DEL TIMBRADO*/
            ws_Timbrado.RespuestaTimbrado respuestaTimbrado = new ws_Timbrado.RespuestaTimbrado();

            MemoryStream ms = new MemoryStream();

            try
            {
                /* ENTRA SIEMPRE Y CUANDO EL PAGO NO HAYA SIDO FACTURADO */
                if (!pago.PNRFacturado)
                {

                    /*  TIMBRADO DEL XMLREQUEST */
                    /*Asigno la instancia a la nueva clase "GenerarYConstruirXML".*/
                    xmlTimbrar = XML.GeneraXMLRequest(pago, "PAQUETE");

                    //CVG convertir xml a base64
                    string strXMLBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlTimbrar));
                    //string apikey = "17320f4108d5b5f291109e8216a8d992";
                    respuestaTimbrado = timbrado.timbrar(pago.LstDatosGralDTO[0].ApikeyPAC, strXMLBase64);

                    /* SI EL TIMBRADO ES CORRECTO SE GUARDAN LOS DATOS DEL PAGO EN BD*/
                    if(!String.IsNullOrEmpty(respuestaTimbrado.response))
                    {
                        xmlResponse = respuestaTimbrado.response.Replace("<?xml version=\"1.0\"?>", "");
                        xmlDocuResponse.LoadXml(string.Format("<response>{0}</response>", xmlResponse));
                        XmlNodeReader xmlReader = new XmlNodeReader(xmlDocuResponse);
                        dsRespuesta.ReadXml(xmlReader);
                    }
                    if (!String.IsNullOrEmpty(respuestaTimbrado.response) && respuestaTimbrado.subcodde == "200")
                    {
                        //if (respuestaTimbrado.codigo.Trim() != "200")
                        //    throw new Exception(respuestaTimbrado.codigo + " - " + respuestaTimbrado.message);

                        respuestaXML = dsRespuesta.Tables["Transaccion"].Rows[0]["Tipo"].ToString().Trim();
                        respuestaXML += "." + dsRespuesta.Tables["Transaccion"].Rows[0]["Estatus"].ToString().Trim();

                        if (dsRespuesta.Tables.Contains("CFD") && dsRespuesta.Tables["CFD"].Rows.Count > 0)
                        {
                            DataRow drcfd = dsRespuesta.Tables["CFD"].Rows[0];
                            if (dsRespuesta.Tables["CFD"].Columns.Contains("comprobanteStr") && !drcfd.IsNull("comprobanteStr"))
                            {
                                xmlDocuResponse.LoadXml(drcfd["comprobanteStr"].ToString());
                                pago.XMLResponse = (drcfd["comprobanteStr"].ToString());

                                /*SE ELIMINA EL ARCHIVO XMLREQUEST PARA GENERAR EL ARCHIVO XMLRESPONSE EN LA MISMA RUTA CON EL MISMO NOMBRE*/
                                System.IO.File.Delete(pago.RutaArchivoXML);

                                FileStream fs = System.IO.File.Create(pago.RutaArchivoXML);
                                fs.Close();

                                /* SE ABRE EL ARCHIVO XMLRESPONSE QUE SE CREO ANTERIORMENTE PARA, LLENARLO CON EL XML TIMBRADO */
                                StreamWriter writer = new StreamWriter(pago.RutaArchivoXML);
                                writer.Write(pago.XMLResponse);
                                writer.Close();                                    
                            }

                            if (dsRespuesta.Tables["CFD"].Columns.Contains("cadenaOriginal") && !drcfd.IsNull("cadenaOriginal"))    pago.CadenaOriginal = drcfd["cadenaOriginal"].ToString();
                            //if (dsRespuesta.Tables["CFD"].Columns.Contains("noCertificado") && !drcfd.IsNull("noCertificado"))      pagosDTO.NoCertificadoSAT = drcfd["noCertificado"].ToString();
                            //if (dsRespuesta.Tables["CFD"].Columns.Contains("comprobanteStr") && !drcfd.IsNull("comprobanteStr"))    pagosDTO.XMLResponse = drcfd["comprobanteStr"].ToString();
                            //if (dsRespuesta.Tables["CFD"].Columns.Contains("serie") && !drcfd.IsNull("serie"))                      regXmlPegaso.CFD_Serie = drcfd["serie"].ToString();
                            //if (dsRespuesta.Tables["CFD"].Columns.Contains("codigoDeBarras") && !drcfd.IsNull("codigoDeBarras"))    regXmlPegaso.CFD_CodigoDeBarras = drcfd["codigoDeBarras"].ToString();
                            //if (dsRespuesta.Tables["CFD"].Columns.Contains("fecha") && !drcfd.IsNull("fecha"))                      regXmlPegaso.CFD_Fecha = Convert.ToDateTime(drcfd["fecha"]);
                            //if (dsRespuesta.Tables["CFD"].Columns.Contains("folio") && !drcfd.IsNull("folio"))                      regXmlPegaso.CFD_Folio = Convert.ToInt64(drcfd["folio"]);
                            //if (dsRespuesta.Tables["CFD"].Columns.Contains("sello") && !drcfd.IsNull("sello"))                      regXmlPegaso.CFD_Sello = drcfd["sello"].ToString();

                        }

                        /*RECUPERAMOS LOS DATOS DEL XMLRESPONSE Y LOS GUARDAMOS EN EL OBJETODTO PARA SU ALMACENAMIENTO EN BD EN PAGOSFACTURA*/
                        XmlNodeList TimbreFiscalDigital = xmlDocuResponse.GetElementsByTagName("tfd:TimbreFiscalDigital");

                        foreach (XmlElement nodo in TimbreFiscalDigital)
                        {
                            pago.FechaTimbrado = nodo.GetAttribute("FechaTimbrado");
                            pago.UUID = nodo.GetAttribute("UUID");
                            pago.NoCertificadoSAT = nodo.GetAttribute("NoCertificadoSAT");
                            pago.SelloSAT = nodo.GetAttribute("SelloSAT");
                            pago.SelloDigital = nodo.GetAttribute("SelloCFD");
                            pago.RFCProveedorCertifica = nodo.GetAttribute("RfcProvCertif");
                            pago.SelloComprobante = nodo.GetAttribute("SelloCFD");
                        }

                        XmlNodeList Comprobante = xmlDocuResponse.GetElementsByTagName("cfdi:Comprobante");

                        foreach (XmlElement nodo in Comprobante)
                        {
                            pago.NoCertificado = nodo.GetAttribute("NoCertificado");
                            pago.CertificadoComprobante = nodo.GetAttribute("Certificado");
                        }

                        //Creo la instancia a la nueva clase GeneraFacturaEgresoPDF
                        //GeneraFactura.GeneraPDFFactura(ref pago);
                        //pago.ArchivoPDF = pago.ArchivoPDF.ToArray ms.ToArray();

                        /*INSTANCIAMOS AL METODO INSERTARDATOSFACTURA, EN EL CUAL INSERTAREMOS LA INFORMACIÓN DEL TIMBRADO EN BD*/
                        //Asigno la instancia de la nueva clase InsertarDatosFactura.
                        //if (InsertarDatosFactura(pagosDTO)) (Original)
                        if (DYAF.InsertarDatosFactura(pago))
                        {
                            return "OK";
                        }
                        else
                        {
                            return "Error al guardar datos de la factura";
                        }
                    }
                    else 
                    // Error en la estructura
                    if (!String.IsNullOrEmpty(respuestaTimbrado.response) && respuestaTimbrado.subcodde == "301")
                    {
                        if (dsRespuesta.Tables.Contains("Error"))
                        {
                            DataRow drError = dsRespuesta.Tables["Error"].Rows[0];
                            string descripError = string.Format("{0}. {1}", drError["Code"].ToString(), drError["Message"].ToString());
                            respuestaXML = descripError;
                            //if (drError[0].ToString() == "2002")
                            //{
                            //    RecuperaHorasErrorFechaSAT(descripError);
                            //}

                            throw new Exception(descripError);
                        }

                        ///*AGREGAR AL LOG DE ERRORES, QUE FALLÓ EL PROCESO DE TIMBRADO Y ENVIAR MAIL A SOPORTE TÉCNICO INFORMANDO QUE FALLÓ EL TIMBRADO*/

                        //EnviarMailSoporte(lstPagosDTO, respuestaTimbrado.message);

                        /*INSTANCIAMOS AL MÉTODO MOVERARCHIVOSFACTURA, PARA MOVER LOS ARCHIVOS A CARPETA DE ARCHIVOSNOPROCESADOS*/
                        /*Asigno la instancia a la nueva clase "MoverArchivosFactura".*/
                        DYAF.MoverArchivosFactura(pago, "Fallo", pago.RutaArchivoXML);
                        //MoverArchivosFactura(pagosDTO, "Fallo", pagosDTO.RutaArchivoXML); (Original)

                        //return false;
                        //throw new Exception(respuestaTimbrado.codigo + " - " + respuestaTimbrado.message);
                    }
                    else
                    // Error en la información del XML
                    if(!String.IsNullOrEmpty(respuestaTimbrado.response) && respuestaTimbrado.subcodde.Contains("CFD"))
                    {
                        if (dsRespuesta.Tables.Contains("Error"))
                        {
                            DataRow drError = dsRespuesta.Tables["Error"].Rows[0];
                            string descripError = string.Format("{0}. {1}", drError["Code"].ToString(), drError["Message"].ToString());
                            respuestaXML = descripError;
                            //if (drError[0].ToString() == "2002")
                            //{
                            //    RecuperaHorasErrorFechaSAT(descripError);
                            //}

                            throw new Exception(descripError);
                        }
                    }

                    else
                    {
                        if (dsRespuesta.Tables.Contains("Error"))
                        {
                            DataRow drError = dsRespuesta.Tables["Error"].Rows[0];
                            string descripError = string.Format("{0}. {1}", drError["Code"].ToString(), drError["Message"].ToString());
                            respuestaXML = descripError;
                            //if (drError[0].ToString() == "2002")
                            //{
                            //    RecuperaHorasErrorFechaSAT(descripError);
                            //}

                            throw new Exception(descripError);
                        }
                    }
                    

                }
                else
                {
                    //Creo la instancia a la nueva clase "GenerarYConstruirXML".
                    XML.GeneraXMLRequest(pago, "PAQUETE");
                    //GeneraXMLRequest(pagosDTO, "YAVAS"); (Original)

                    /*SE ELIMINA EL ARCHIVO XMLREQUEST PARA GENERAR EL ARCHIVO XMLRESPONSE EN LA MISMA RUTA CON EL MISMO NOMBRE*/
                    System.IO.File.Delete(pago.RutaArchivoXML);

                    FileStream fs = System.IO.File.Create(pago.RutaArchivoXML);
                    fs.Close();

                    /* SE ABRE EL ARCHIVO XMLRESPONSE QUE SE CREO ANTERIORMENTE PARA, LLENARLO CON EL XML TIMBRADO */
                    StreamWriter writer = new StreamWriter(pago.RutaArchivoXML);
                    writer.Write(pago.XMLResponse);
                    writer.Close();

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(pago.RutaArchivoXML);

                    /*MANDAMOS A LLAMAR EL METODO (GuardaArchivosFactura) Y LE PASAMOS EL OBJETO*/
                    //Se instancia a la nueva clase que se encuentra en "GeneraFacturaEgresoPDF".
                    //GeneraFactura Fe = new GeneraFactura();
                    //pagosDTO.RutaArchivoPDF = GuardaArchivosFactura(pagosDTO, "PDF","YAVAS");
                    //pago.RutaArchivoPDF = GeneraFactura.GuardaArchivosFactura(pago, "PDF");

                    System.IO.File.WriteAllBytes(pago.RutaArchivoPDF, pago.ArchivoPDF);

                    return "OK";
                }
            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pagosDTO", pago);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }

            return "OK";

        }


        private string RespuestaTimbradoBoleto(ref Pago pagosDTO, String PNRVOI)
        {
            DataTable dtWs = new DataTable();
            String RFCTercero = String.Empty;
            String RazonSocialTercero = String.Empty;
            String razonSocial = "", rfcReceptor = "", pais = "", estado = "", municipio = "", cp = ""; ;
            String email = "", direccion = "", colonia = "", usoCFDI = "", descUSOCFDI = "";

            //Resultado res = new Resultado();

            try
            {


                //rfcReceptor = pagosDTO.LstDatosFiscales[0].RFC;
                //razonSocial = String.IsNullOrEmpty(pagosDTO.LstDatosFiscales[0].RazonSocial) ? "" : pagosDTO.LstDatosFiscales[0].RazonSocial.ToString();
                //direccion = String.IsNullOrEmpty(pagosDTO.LstDatosFiscales[0].DireccionFiscal) ? "" : pagosDTO.LstDatosFiscales[0].DireccionFiscal.ToString();
                //colonia = String.IsNullOrEmpty(pagosDTO.LstDatosFiscales[0].Colonia) ? "" : pagosDTO.LstDatosFiscales[0].Colonia.ToString();
                //email = pagosDTO.LstDatosFiscales[0].Email;
                //usoCFDI = pagosDTO.LstDatosFiscales[0].CodigoUsoCFDI;
                //descUSOCFDI = pagosDTO.LstDatosFiscales[0].DescUsoCFDI;
                //pais = pagosDTO.LstDatosFiscales[0].Pais;
                //estado = String.IsNullOrEmpty(pagosDTO.LstDatosFiscales[0].Estado) ? "" : pagosDTO.LstDatosFiscales[0].Estado.ToString();
                //municipio = String.IsNullOrEmpty(pagosDTO.LstDatosFiscales[0].Municipio) ? "" : pagosDTO.LstDatosFiscales[0].Municipio.ToString();
                //cp = String.IsNullOrEmpty(pagosDTO.LstDatosFiscales[0].CodigoPostal.ToString()) ? "" : pagosDTO.LstDatosFiscales[0].CodigoPostal.ToString();

                //InvoiceOnlineServiceClient wsFacturacionBoletosYavas = new InvoiceOnlineServiceClient();


                //res = wsFacturacionBoletosYavas.FacturarBoletoVOI(PNRVOI, razonSocial, email, rfcReceptor,
                //    usoCFDI, descUSOCFDI, pais, direccion, estado, municipio, cp);


                //if (res != null && res.Mensaje == "200")
                //{
                //    pagosDTO.XMLResponseTercero = res.xml.ToString();
                //    pagosDTO.ArchivoPDFTercero = String.IsNullOrEmpty(res.pdf.ToString()) ? null : ((byte[])res.pdf);
                //    RFCTercero = res.RFCTercero;
                //    RazonSocialTercero = res.RazonSocialTercero;

                //    if (pagosDTO.LstConcepto.Exists(x => x.PNRVOI == PNRVOI))
                //    {
                //        int Index = pagosDTO.LstConcepto.FindIndex(x => x.PNRVOI == PNRVOI);

                //        pagosDTO.LstConcepto[Index].RFCTercero = RFCTercero;
                //        pagosDTO.LstConcepto[Index].RazonSocialTercero = RazonSocialTercero;
                //    }
                //}
                //else if (res != null && res.Mensaje == "300")
                //{
                //    return res.xml;
                //}
                //else
                //    throw new Exception("Error al facturar boleto de avión: " + PNRVOI + ", " + res.xml);

                return "OK";


            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pagosDTO", pagosDTO);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }

        }

        public void RecrearXMLPDF(ref Pago pago)
        {
            try
            {
                // Configura la ruta de los archivos
                //GeneraFactura Fe = new GeneraFactura();
                //pago.RutaArchivoXML = GeneraFactura.GuardaArchivosFactura(pago, "XML");

                // Crea el archivo XML
                FileStream fs = System.IO.File.Create(pago.RutaArchivoXML);
                fs.Close();
                StreamWriter writer = new StreamWriter(pago.RutaArchivoXML);
                writer.Write(pago.XMLResponse);
                writer.Close();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(pago.RutaArchivoXML);

                // Crea el archivo PDF
                //pago.RutaArchivoPDF = GeneraFactura.GuardaArchivosFactura(pago, "PDF");
                System.IO.File.WriteAllBytes(pago.RutaArchivoPDF, pago.ArchivoPDF);
            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pago", pago);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }
        }
    }
}

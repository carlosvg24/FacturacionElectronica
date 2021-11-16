using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VBFactPaquetes.BLL.Facturacion;
using VBFactPaquetes.BLL.ws_Timbrado;
using VBFactPaquetes.Comun.Log;
using VBFactPaquetes.DAO;
using VBFactPaquetes.Model.Facturacion;

namespace VBFactPaquetes.ProcesosBatch
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main(string[] args)
        {

            try
            {
                var handle = GetConsoleWindow();
                // Hide
                ShowWindow(handle, SW_HIDE);
                // Show
                //ShowWindow(handle, SW_SHOW);

                if (args.Length > 0)
                {
                    switch (args[0])
                    {
                        case "EP":
                            //ExtraccionPagos();
                            break;
                        case "FG":
                            GeneraFacturaGlobal();
                            break;
                        default:
                            Console.WriteLine("Argumento " + args[0]);
                            break;
                    }

                }
                else
                    Console.WriteLine("Sin argumentos");

            }
            catch (Excepciones ex)
            {
                Console.WriteLine("Excepciones: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            Console.WriteLine("Ejecutó");
        }

        private static void GeneraFacturaGlobal()
        {
            try
            {
                List<FacturaGlobal> lstGlobal = new List<FacturaGlobal>();
                DateTime fechaConsulta = new DateTime();
                BLLFacturaGlobal bllFacturaGlobal = new BLLFacturaGlobal();
                MemoryStream ms = new MemoryStream();
                FacturaGlobal facturaGlobal = new FacturaGlobal();
                /* SE INSTANCIA EL WS DE PAC*/
                FacturaloTimbradoWSPortTypeClient timbrado = new FacturaloTimbradoWSPortTypeClient();
                /*SE INSTANCIA EL OBJETO QUE CONTENDRÁ LA RESPUESTA DEL TIMBRADO*/
                RespuestaTimbrado respuestaTimbrado = new RespuestaTimbrado();

                String xml = String.Empty;

                Console.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
                Console.WriteLine("  Generando Factura " + "----" + DateTime.Now);
                Console.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");

                lstGlobal = bllFacturaGlobal.ConsultaConceptosGlobal(2021, 03, "I", "MXN");

                if (lstGlobal.Count > 0)
                {
                    facturaGlobal = lstGlobal[0];
                    /*  TIMBRADO DEL XMLREQUEST */
                    xml = bllFacturaGlobal.GeneraXMLRequest(facturaGlobal);

                    //CVG convertir xml a base64
                    string xml64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xml));
                    string apikey = "";
                    DAOConfiguracion daoConf = new DAOConfiguracion();
                    DataTable dtResult = daoConf.ConsultarConfiguracion("VBPAQ");

                    try
                    {
                        apikey = dtResult.Select("Nombre = 'apikey'").CopyToDataTable().Rows[0]["Valor"].ToString();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    //sustituir en lugar del apikey facturaGlobal.LstDatosGralDTO[0].ApikeyPAC

                    respuestaTimbrado = timbrado.timbrar(apikey, xml64);

                    /* SI EL TIMBRADO ES CORRECTO SE GUARDAN LOS DATOS DEL PAGO EN BD*/
                    if (!String.IsNullOrEmpty(respuestaTimbrado.response)) /*respuestaTimbrado.cfdi*/
                    {
                        facturaGlobal.XMLResponse = respuestaTimbrado.response; /*respuestaTimbrado.cfdi*/

                        /*SE ELIMINA EL ARCHIVO XMLREQUEST PARA GENERAR EL ARCHIVO XMLRESPONSE EN LA MISMA RUTA CON EL MISMO NOMBRE*/
                        System.IO.File.Delete(facturaGlobal.RutaArchivoXML);

                        FileStream fs = System.IO.File.Create(facturaGlobal.RutaArchivoXML);
                        fs.Close();

                        /* SE ABRE EL ARCHIVO XMLRESPONSE QUE SE CREO ANTERIORMENTE PARA, LLENARLO CON EL XML TIMBRADO */
                        //StreamWriter writer = new StreamWriter(facturaGlobal.RutaArchivoXML);
                        //writer.Write(facturaGlobal.XMLResponse);
                        //writer.Close();

                        XmlDocument xmlDocuResponse = new XmlDocument();
                        xmlDocuResponse.LoadXml(string.Format("<response>{0}</response>", facturaGlobal.XMLResponse));
                        XmlNodeReader xmlReader = new XmlNodeReader(xmlDocuResponse);
                        DataSet dsRepuesta = new DataSet();

                        dsRepuesta.ReadXml(xmlReader);

                        if (dsRepuesta.Tables.Contains("CFD") && dsRepuesta.Tables["CFD"].Rows.Count > 0)
                        {
                            DataRow drcfd = dsRepuesta.Tables["CFD"].Rows[0];
                            if (dsRepuesta.Tables["CFD"].Columns.Contains("comprobanteStr") && !drcfd.IsNull("comprobanteStr"))
                            {
                                xmlDocuResponse.LoadXml(drcfd["comprobanteStr"].ToString());
                                using (XmlTextWriter writ = new XmlTextWriter(facturaGlobal.RutaArchivoXML, System.Text.Encoding.UTF8))
                                {
                                    writ.Formatting = Formatting.Indented;

                                    xmlDocuResponse.Save(writ);
                                }

                                /* SE ABRE EL ARCHIVO XMLRESPONSE QUE SE CREO ANTERIORMENTE PARA, LLENARLO CON EL XML TIMBRADO */
                                //StreamWriter writers = new StreamWriter(facturaGlobal.RutaArchivoXML);
                                //writers.Write(drcfd["comprobanteStr"]);
                                //writers.Close();
                            }
                        }

                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.Load(string.Format(facturaGlobal.RutaArchivoXML));


                        /*RECUPERAMOS LOS DATOS DEL XMLRESPONSE Y LOS GUARDAMOS EN EL OBJETODTO PARA SU ALMACENAMIENTO EN BD EN PAGOSFACTURA*/
                        XmlNodeList TimbreFiscalDigital = xmlDocument.GetElementsByTagName("tfd:TimbreFiscalDigital");

                        foreach (XmlElement nodo in TimbreFiscalDigital)
                        {
                            facturaGlobal.FechaTimbrado = nodo.GetAttribute("FechaTimbrado");
                            facturaGlobal.UUID = nodo.GetAttribute("UUID");
                            facturaGlobal.NoCertificadoSAT = nodo.GetAttribute("NoCertificadoSAT");
                            facturaGlobal.SelloSAT = nodo.GetAttribute("SelloSAT");
                            facturaGlobal.SelloDigital = nodo.GetAttribute("SelloCFD");
                            facturaGlobal.RFCProveedorCertifica = nodo.GetAttribute("RfcProvCertif");
                            facturaGlobal.SelloComprobante = nodo.GetAttribute("SelloCFD");
                        }

                        XmlNodeList Comprobante = xmlDocument.GetElementsByTagName("cfdi:Comprobante");

                        foreach (XmlElement nodo in Comprobante)
                        {
                            facturaGlobal.NoCertificado = nodo.GetAttribute("NoCertificado");
                            facturaGlobal.CertificadoComprobante = nodo.GetAttribute("Certificado");
                        }

                        //Se carga el response del WS para consumir la cadena original
                        xmlDocuResponse.LoadXml(string.Format("<response>{0}</response>", facturaGlobal.XMLResponse));
                        XmlNodeList cadena = xmlDocuResponse.GetElementsByTagName("CFD");

                        foreach (XmlElement nodo in cadena)
                        {
                            facturaGlobal.CadenaOriginal = nodo.GetAttribute("cadenaOriginal");
                        }

                        Console.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
                        Console.WriteLine("  Termina XML" + "----" + DateTime.Now);
                        Console.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");


                        Boolean pdfGenerado = new Boolean();
                        string pdf = "";
                        //pdfGenerado = Comun.PDF.FacturaGlobal.GeneraPDFFactura(ref facturaGlobal);

                        Console.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
                        Console.WriteLine("  Actualizando Datos en BD " + "----" + DateTime.Now);
                        Console.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");

                        /*INSTANCIAMOS AL METODO INSERTARDATOSFACTURA, EN EL CUAL INSERTAREMOS LA INFORMACIÓN DEL TIMBRADO EN BD*/
                        if (pdfGenerado && bllFacturaGlobal.InsertarDatosFactura(facturaGlobal))
                        {
                            Console.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
                            Console.WriteLine("  Termina Datos en BD " + "----" + DateTime.Now);
                            Console.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
                        }
                    }
                    else
                    {
                        ///*AGREGAR AL LOG DE ERRORES, QUE FALLÓ EL PROCESO DE TIMBRADO Y ENVIAR MAIL A SOPORTE TÉCNICO INFORMANDO QUE FALLÓ EL TIMBRADO*/

                        //EnviarMailSoporte(lstPagosDTO, respuestaTimbrado.message);

                        /*INSTANCIAMOS AL MÉTODO MOVERARCHIVOSFACTURA, PARA MOVER LOS ARCHIVOS A CARPETA DE ARCHIVOSNOPROCESADOS*/
                        //MoverArchivosFactura(globalDTO, "Fallo", globalDTO.RutaArchivoXML);

                        Dictionary<string, object> parametros = new Dictionary<string, object>();
                        parametros.Add("xml", xml64);
                        parametros.Add("facturaGlobal.LstDatosGralDTO[0]", facturaGlobal.LstDatosGralDTO[0]);

                        throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                              MethodBase.GetCurrentMethod().Name,
                                              parametros,
                                              new Exception("" /*respuestaTimbrado.message*/), Excepciones.TipoPortal.VivaPaquetes);
                    }
                }

                Console.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
                Console.WriteLine("  Termina Factura " + "----" + DateTime.Now);
                Console.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
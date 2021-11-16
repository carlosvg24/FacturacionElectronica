using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VBFactPaquetes.BLL.ws_Timbrado;
using VBFactPaquetes.Comun;
using VBFactPaquetes.Comun.Log;
//using VBFactPaquetes.Comun.PDF;
using VBFactPaquetes.DAO.Facturacion;
using VBFactPaquetes.Model;
using VBFactPaquetes.Model.Facturacion;

namespace VBFactPaquetes.BLL.BDFacturacion
{
    public class BLLNotaCredito
    {


        public bool GenerarCFDIEgreso(Model.Facturacion.Pago pago)
        {
            String xml = String.Empty;
            MemoryStream ms = new MemoryStream();

            /* SE INSTANCIA EL WS DE PAC*/
            FacturaloTimbradoWSPortTypeClient timbrado = new FacturaloTimbradoWSPortTypeClient();

            /*SE INSTANCIA EL OBJETO QUE CONTENDRÁ LA RESPUESTA DEL TIMBRADO*/
            RespuestaTimbrado response = new RespuestaTimbrado();
            GenerarYConstruirXML generarConstruirXML = new GenerarYConstruirXML();

            String request = "";
            string xmlResponse = "";
            DataSet dsRespuesta = new DataSet();
            string xmlRequestFact = "";
            DateTime fechaPeticion = new DateTime();
            string respuestaXML = "";

            try
            {
                /*  TIMBRADO DEL XMLREQUEST */
                xml = generarConstruirXML.GeneraXMLRequestEgresos(pago);

                /*
                 *  Ejemplo de un XML Request
                 * 
                 <RequestCFD version="3.3">
	                <Comprobante Version="3.3" Serie="B" Folio="55706667" Fecha="2021-05-03T14:50:03" FormaPago="28" NoCertificado="00001000000301999105"  CondicionesDePago="Inmediato" SubTotal="2892.12" Moneda="MXN" Total="3290.40" TipoDeComprobante="I" MetodoPago="PUE" LugarExpedicion="66600" >
		                <Emisor Rfc="ANA050518RL1" Nombre="AEROENLACES NACIONALES SA DE CV" RegimenFiscal="601"/>
		                <Receptor Rfc="DMS090804M8" UsoCFDI="G03"/>
		                <Conceptos>
                            <Concepto ClaveProdServ="78111500" NoIdentificacion="001" Cantidad="1" ClaveUnidad="E48" Unidad="Unidad de servicio" Descripcion="TARIFA AEREA PNR: FYJFVG" ValorUnitario="2489.28" Importe="2489.28" >
				                <Impuestos>
                                    <Traslados>
                                        <Traslado Base="2489.28" Impuesto="002" TipoFactor="Tasa" TasaOCuota="0.160000" Importe="398.28" />
					                </Traslados>
                                </Impuestos>
                            </Concepto>
                            <Concepto ClaveProdServ="78111500" NoIdentificacion="003" Cantidad="1" ClaveUnidad="E48" Unidad="Unidad de servicio" Descripcion="CARGOS AEROPORTUARIOS" ValorUnitario="402.84" Importe="402.84" >
				                <Impuestos>
                                    <Traslados>
                                        <Traslado Base="402.84" Impuesto="002" TipoFactor="Tasa" TasaOCuota="0.000000" Importe="0.00" />
					                </Traslados>
                                </Impuestos>
                            </Concepto>
                        </Conceptos>
                        <Impuestos TotalImpuestosTrasladados="398.28">
			                <Traslados>
                                <Traslado Impuesto="002" TipoFactor="Tasa" TasaOCuota="0.000000" Importe="0.00"/>
                                <Traslado Impuesto="002" TipoFactor="Tasa" TasaOCuota="0.160000" Importe="398.28"/>
			                </Traslados>
                        </Impuestos>
                        <Complemento>
                            <Aerolineas Version="1.0" TUA="343.82">
				                <OtrosCargos TotalCargos="59.02">
					                <Cargo CodigoCargo="BS" Importe="53.77"/>
					                <Cargo CodigoCargo="SS" Importe="5.25"/>
				                </OtrosCargos>
                            </Aerolineas>
                        </Complemento>
                    </Comprobante>
                    <Transaccion id="ANA3003245210503FYJFVG-55706667"/>
	                <TipoComprobante clave="Factura B" nombre="Factura B"/>
	                <Sucursal nombre="MATRIZ"/>
	                <Receptor emailReceptor="str1234" codigoReceptor="str1234"/>
                </RequestCFD>
                 */
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(xml);
                request = System.Convert.ToBase64String(plainTextBytes);

                response = timbrado.timbrar(pago.LstDatosGralDTO[0].ApikeyPAC, request);

                //Convertimos la respuesta en un documento de XML
                XmlDocument xmlDocuResponse = new XmlDocument();
                xmlResponse = response.response.Replace("<?xml version=\"1.0\"?>", "");
                xmlDocuResponse.LoadXml(string.Format("<response>{0}</response>", xmlResponse));
                XmlNodeReader xmlReader = new XmlNodeReader(xmlDocuResponse);
                dsRespuesta.ReadXml(xmlReader);

                // Cacha, almacena y envia emial por error
                if (dsRespuesta.Tables.Contains("Error"))
                {
                    /*
                     * Ejemplo de response con error
                     * 
                     <Transaccion id="ANA3003245210503FYJFVG-55706667" tipo="EMISION" estatus="ERROR"/>
                    <ERROR Code="300" SubCode="165" Message="API KEY inv&#xE1;lido o inexistente."/>
                    <Transaccion id="ANA3003245210503FYJFVG-55706667" tipo="EMISION" estatus="ERROR"/>
                    <ERROR Code="301" SubCode="VALIDADORLIBRARY:130" Message="XML mal formado&#10;&#10;Ocurrieron errores en la validaci&#xF3;n XSD&#10;[1831] Element 'Receptor@Rfc': [facet 'minLength'] The value 'DMS090804M8' has a length of '11'; this underruns the allowed minimum length of '12'."/>

                     */

                    DataRow drError = dsRespuesta.Tables["Error"].Rows[0];
                    string descripError = string.Format("{0}. {1}", drError["Code"].ToString(), drError["Message"].ToString());
                    //respuestaXML = descripError;

                    if (drError[0].ToString() == "2002")
                    {
                        //RecuperaHorasErrorFechaSAT(descripError);
                    }

                    ///*AGREGAR AL LOG DE ERRORES, QUE FALLÓ EL PROCESO DE TIMBRADO Y ENVIAR MAIL A SOPORTE TÉCNICO INFORMANDO QUE FALLÓ EL TIMBRADO*/

                    //EnviarMailSoporte(lstpago, respuestaTimbrado.message);

                    /*INSTANCIAMOS AL MÉTODO MOVERARCHIVOSFACTURA, PARA MOVER LOS ARCHIVOS A CARPETA DE ARCHIVOSNOPROCESADOS*/
                    MoverArchivosFactura(pago, "Fallo", pago.RutaArchivoXML);
                    throw new Exception(descripError);
                    //return false;
                }
                /* SI EL TIMBRADO ES CORRECTO SE GUARDAN LOS DATOS DEL PAGO EN BD*/
                else
                {
                    fechaPeticion = DateTime.Now;

                    respuestaXML = dsRespuesta.Tables["Transaccion"].Rows[0]["Tipo"].ToString();
                    respuestaXML += "." + dsRespuesta.Tables["Transaccion"].Rows[0]["Estatus"].ToString();

                    //Se recupera la informacion de la Respuesta de Pegaso
                    BLLXml_WS_Facturalo regXml = new BLLXml_WS_Facturalo();
                    regXml.IdPeticionPAC = 0;
                    regXml.FechaPeticion = fechaPeticion;
                    regXml.FolioCFDI = 12345678; //long.Parse(pago.NoFolio);
                    regXml.TipoComprobante = "E";
                    regXml.XMLRequest = xml;
                    regXml.XMLResponse = xmlResponse;
                    regXml.MensajeRespuesta = respuestaXML;
                    regXml.EsCorrecto = true;
                    regXml.PAC = "FACTURALO";


                    if (dsRespuesta.Tables.Contains("Transaccion") && dsRespuesta.Tables["Transaccion"].Rows.Count > 0)
                    {
                        DataRow drTrans = dsRespuesta.Tables["Transaccion"].Rows[0];
                        if (dsRespuesta.Tables["Transaccion"].Columns.Contains("id") && !drTrans.IsNull("id")) regXml.Transaccion_Id = drTrans["id"].ToString();
                        if (dsRespuesta.Tables["Transaccion"].Columns.Contains("tipo") && !drTrans.IsNull("tipo")) regXml.Transaccion_Tipo = drTrans["tipo"].ToString();
                        if (dsRespuesta.Tables["Transaccion"].Columns.Contains("estatus") && !drTrans.IsNull("estatus"))
                        {
                            regXml.Transaccion_Estatus = drTrans["estatus"].ToString();
                            regXml.MensajeRespuesta = regXml.Transaccion_Estatus;
                        }
                    }
                    else
                    {
                        regXml.MensajeRespuesta = "No se obtuvo respuesta al intentar timbrar la factura...";
                    }


                    if (dsRespuesta.Tables.Contains("CFD") && dsRespuesta.Tables["CFD"].Rows.Count > 0)
                    {
                        DataRow drcfd = dsRespuesta.Tables["CFD"].Rows[0];
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("cadenaOriginal") && !drcfd.IsNull("cadenaOriginal")) regXml.CFD_CadenaOriginal = drcfd["cadenaOriginal"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("noCertificado") && !drcfd.IsNull("noCertificado")) regXml.CFD_NoCertificado = drcfd["noCertificado"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("comprobanteStr") && !drcfd.IsNull("comprobanteStr")) regXml.CFD_ComprobanteStr = drcfd["comprobanteStr"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("serie") && !drcfd.IsNull("serie")) regXml.CFD_Serie = drcfd["serie"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("codigoDeBarras") && !drcfd.IsNull("codigoDeBarras")) regXml.CFD_CodigoDeBarras = drcfd["codigoDeBarras"].ToString();
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("fecha") && !drcfd.IsNull("fecha")) regXml.CFD_Fecha = Convert.ToDateTime(drcfd["fecha"]);
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("folio") && !drcfd.IsNull("folio")) regXml.CFD_Folio = 12345678;//Convert.ToInt64(drcfd["folio"]);
                        if (dsRespuesta.Tables["CFD"].Columns.Contains("sello") && !drcfd.IsNull("sello")) regXml.CFD_Sello = drcfd["sello"].ToString();
                    }

                    if (dsRespuesta.Tables.Contains("TFD") && dsRespuesta.Tables["TFD"].Rows.Count > 0)
                    {
                        DataRow drtfd = dsRespuesta.Tables["TFD"].Rows[0];
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("UUID") && !drtfd.IsNull("UUID")) regXml.TFD_UUID = drtfd["UUID"].ToString();
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("FechaTimbrado") && !drtfd.IsNull("FechaTimbrado"))
                        {
                            DateTime fechaTim = Convert.ToDateTime(drtfd["FechaTimbrado"]);
                            regXml.TFD_FechaTimbrado = fechaTim;
                            regXml.FechaTimbrado = fechaTim;
                        }
                        else
                        {
                            regXml.TFD_FechaTimbrado = regXml.CFD_Fecha;
                            regXml.FechaTimbrado = regXml.CFD_Fecha;
                        }
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("noCertificadoSAT") && !drtfd.IsNull("noCertificadoSAT")) regXml.TFD_NoCertificadoSAT = drtfd["noCertificadoSAT"].ToString();
                        if (dsRespuesta.Tables["TFD"].Columns.Contains("selloSAT") && !drtfd.IsNull("selloSAT")) regXml.TFD_SelloSAT = drtfd["selloSAT"].ToString();
                    }

                    if (dsRespuesta.Tables.Contains("Error") && dsRespuesta.Tables["Error"].Rows.Count > 0)
                    {
                        regXml.EsCorrecto = false;
                        DataRow drError = dsRespuesta.Tables["Error"].Rows[0];
                        if (dsRespuesta.Tables["Error"].Columns.Contains("Code") && !drError.IsNull("Code")) regXml.MensajeRespuesta = drError["Code"].ToString() + ". ";
                        if (dsRespuesta.Tables["Error"].Columns.Contains("Message") && !drError.IsNull("Message")) regXml.MensajeRespuesta += drError["Message"].ToString();
                    }

                    regXml.FechaHoraLocal = new DateTime();
                    //regXml.Agregar();

                    if (regXml.EsCorrecto)
                    {
                        pago.XMLResponse = regXml.CFD_ComprobanteStr;

                        /*SE ELIMINA EL ARCHIVO XMLREQUEST PARA GENERAR EL ARCHIVO XMLRESPONSE EN LA MISMA RUTA CON EL MISMO NOMBRE*/
                        File.Delete(pago.RutaArchivoXML);

                        FileStream fs = File.Create(pago.RutaArchivoXML);
                        fs.Close();

                        /* SE ABRE EL ARCHIVO XMLRESPONSE QUE SE CREO ANTERIORMENTE PARA, LLENARLO CON EL XML TIMBRADO */
                        StreamWriter writer = new StreamWriter(pago.RutaArchivoXML);
                        writer.Write(pago.XMLResponse);
                        writer.Close();

                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.Load(pago.RutaArchivoXML);

                        /*RECUPERAMOS LOS DATOS DEL XMLRESPONSE Y LOS GUARDAMOS EN EL OBJETODTO PARA SU ALMACENAMIENTO EN BD EN PAGOSFACTURA*/
                        XmlNodeList TimbreFiscalDigital = xmlDocument.GetElementsByTagName("tfd:TimbreFiscalDigital");

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

                        XmlNodeList Comprobante = xmlDocument.GetElementsByTagName("cfdi:Comprobante");

                        foreach (XmlElement nodo in Comprobante)
                        {
                            pago.NoCertificado = nodo.GetAttribute("NoCertificado");
                            pago.CertificadoComprobante = nodo.GetAttribute("Certificado");
                        }

                        XmlNodeList cadena = xmlDocuResponse.GetElementsByTagName("CFD");

                        foreach (XmlElement nodo in cadena)
                        {
                            pago.CadenaOriginal = nodo.GetAttribute("cadenaOriginal");
                        }

                        //pago.CadenaOriginal = response.response;

                        //GeneraFactura pdf = new GeneraFactura();
                        //ms = pdf.GeneraPDFEgreso(pago);
                        //pago.ArchivoPDF = ms.ToArray();

                        ///*INSTANCIAMOS AL METODO INSERTARDATOSFACTURA, EN EL CUAL INSERTAREMOS LA INFORMACIÓN DEL TIMBRADO EN BD*/
                        if (InsertarDatosFactura(pago))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                        return false;

                }


                //else
                //{
                //    GeneraXMLRequest(pago);

                //    /*SE ELIMINA EL ARCHIVO XMLREQUEST PARA GENERAR EL ARCHIVO XMLRESPONSE EN LA MISMA RUTA CON EL MISMO NOMBRE*/
                //    System.IO.File.Delete(pago.RutaArchivoXML);

                //    FileStream fs = System.IO.File.Create(pago.RutaArchivoXML);
                //    fs.Close();

                //    /* SE ABRE EL ARCHIVO XMLRESPONSE QUE SE CREO ANTERIORMENTE PARA, LLENARLO CON EL XML TIMBRADO */
                //    StreamWriter writer = new StreamWriter(pago.RutaArchivoXML);
                //    writer.Write(pago.XMLResponse);
                //    writer.Close();

                //    XmlDocument xmlDocument = new XmlDocument();
                //    xmlDocument.Load(pago.RutaArchivoXML);

                //    /*MANDAMOS A LLAMAR EL METODO (GuardaArchivosFactura) Y LE PASAMOS EL OBJETO*/
                //    pago.RutaArchivoPDF = GuardaArchivosFactura(pago, "PDF");

                //    System.IO.File.WriteAllBytes(pago.RutaArchivoPDF, pago.ArchivoPDF);

                //    return true;
                //}
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


        private bool InsertarDatosFactura(Model.Facturacion.Pago pago)
        {
            DAONotaCredito daoNotaCredito = new DAONotaCredito();
            DataTable dtPagos = new DataTable();

            try
            {
                dtPagos = daoNotaCredito.CRUDNotasCredito(pago, "E");

                if (dtPagos.Rows[0][0].ToString() != "ERROR")
                    return true;
                else
                {
                    Dictionary<string, object> parametros = new Dictionary<string, object>();
                    parametros.Add("pago", pago);
                    parametros.Add("dtPagos", dtPagos);
                    new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                          MethodBase.GetCurrentMethod().Name,
                                          parametros,
                                          new Exception(dtPagos.Rows[0][0].ToString())
                                          , Excepciones.TipoPortal.VivaPaquetes);
                    return false;
                }

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


        private void MoverArchivosFactura(Model.Facturacion.Pago pago, string TipoProceso, string rutaArchivoXML)
        {
            String rutaCarpetaDestino = String.Empty;
            String rutaArchivoDestino = String.Empty;

            try
            {
                /*EN CASO DE QUE EL TIPOPROCESO SEA "FALLO", MOVEMOS LOS ARCHIVOS QUE TUVIERON PROBLEMAS AL FACTURAR, EN LA CARPETA DE ARCHIVOSNOPROCESADOS*/
                if (TipoProceso == "Fallo")
                {
                    /*OBTENEMOS LA RUTA DESTINO DE LA CARPETA DONDE MOVEREMOS LOS ARCHIVOS QUE TIENEN PROBLEMAS PARA PROCESAR*/
                    rutaCarpetaDestino = pago.LstDatosGralDTO[0].CarpetaNoProcesados;
                    if (!Directory.Exists(rutaCarpetaDestino))
                        Directory.CreateDirectory(rutaCarpetaDestino);

                    rutaArchivoDestino = Path.Combine(rutaCarpetaDestino, rutaArchivoXML.Substring(rutaArchivoXML.LastIndexOf("\\") + 1));

                    if (File.Exists(rutaArchivoDestino))
                        File.Delete(rutaArchivoDestino);
                    //System.IO.File.Delete(".zip");

                    File.Move(rutaArchivoXML, rutaArchivoDestino);
                }
            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pago", pago);
                parametros.Add("TipoProceso", TipoProceso);
                parametros.Add("rutaArchivoXML", rutaArchivoXML);
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

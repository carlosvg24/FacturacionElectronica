using ExecuteProcess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExecuteProcess.ServiceEnt;
using Facturacion.BLL.NotaCredito;
using Facturacion.ENT.Comun;
using MansLog.Error;
using MansLog.ConfigXML;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Facturacion.ENT.NotaCredito;
using System.Data.SqlClient;
using Facturacion.ENT;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.BLL;
using System.Web.Script.Serialization;
using Process.Entidades;

namespace ExecuteProcess.Process
{
    public class NotaCreditoCliente : IProcess
    {
        #region Propiedades Publicas

        public bool ModeDebug { get; set; }

        public bool OnDemand { get; set; }

        public DateTime FechaInicial { get; set; }

        public DateTime FechaFinal { get; set; }

        public Dictionary<string, Dictionary<string, string>> Parametros { get; set; }

        #endregion

        #region Propiedades Privadas

        private LogError log { get; set; }

        #endregion

        #region Constructor

        public NotaCreditoCliente()
        {
            try
            {
                this.OnDemand = false;
                this.Parametros = new Dictionary<string, Dictionary<string, string>>();
                LogMansSection sectionLog = (LogMansSection)ConfigurationManager.GetSection("FacturacionServiceSettings/LogMans");

                this.log = new LogError(sectionLog, this.GetType().Name);
                this.log.StackTraceCompleto = true;
                this.log.TipoStackTrace = MansLog.EL.TypeStackTrace.Default;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sucedio un error al instanciar NotaCreditoCliente.cs : {0}", ex.Message);
                throw ex;
            }
        }

        #endregion

        public event ShowPorcentProgress OnShowPorcentProgress;

        #region Metodos Publicos        

        public ResponseTask ValidationsBeforeExecution()
        {
            try
            {
                var resultConexion = BLLNotaCredito.TestConexionBD();

                if (resultConexion.Succes)
                {
                    List<StoredProcedureValidation> sps = new List<StoredProcedureValidation>();

                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetPagosClienteParaNotaCredito" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetReservaCab_POR_BookingID" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetPagosCab_POR_PaymentID" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertaNotasCreditoCab" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertNotasCredito_Det" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertNotasCreditoIVA_Det" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertaNotasCreditoCargos_Det" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertaNotaCreditoCFDIDet" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsCFDIRelacionadosDet" });

                    var resultSps = BLLNotaCredito.ValidarSpNecesarios(sps);

                    if (resultSps.Succes)
                    {
                        return new ResponseTask()
                        {
                            Succes = true
                        };
                    }
                    else
                    {
                        return new ResponseTask()
                        {
                            Succes = false,
                            Message = resultSps.Message
                        };
                    }
                }
                else
                {
                    return new ResponseTask()
                    {
                        Succes = false,
                        Message = resultConexion.Message,
                        StackTrace = string.Empty
                    };
                }
            }
            catch (Exception ex)
            {
                log.escribir(ex, "Validaciones previas a \"MainProcess\"");

                return new ResponseTask()
                {
                    Succes = false,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };
            }
        }

        public ResponseTask MainProcess()
        {
            string messageCorreo = string.Empty;
            int contadorNC = 0;
            decimal currentPorcent = 12;

            ResponseTask response = new ResponseTask();

            SetFechas();

            PrintProgressbar(10, currentPorcent, "Se setean rago de fechas");

            List<PagosParaNotaCredito> pagos = BLLNotaCredito.GetPagosFacturasClienteParaNotaCredito(this.FechaInicial, this.FechaFinal);

            PrintProgressbar(currentPorcent,(currentPorcent += 8), string.Format("Se obtienen {0} pago(s) para generar NC", pagos.Count));

            if (pagos.Count > 0)
            {
                BLLReservaCab reservaCab = new BLLReservaCab();

                decimal percentXpaso = (decimal)(75.0M / (decimal)(pagos.Count * 5.0M));

                WriteBitacora(string.Format("percentXpaso = {0}", percentXpaso),true);

                foreach (PagosParaNotaCredito pago in pagos)
                {
                    string mensajePago = ProcesarNotaDeCreditocliente(pago, reservaCab, ref contadorNC, percentXpaso, ref currentPorcent);
                    messageCorreo = string.Format("{0}{1}", messageCorreo, mensajePago);
                }
            }

            response.Succes = true;
            response.Message = GenerarMensajeSucces(messageCorreo, contadorNC);

            return response;
        }

        #endregion

        #region Metodos Privados
        private void SetFechas()
        {
            if (!OnDemand)
            {
                int dias = int.Parse(Parametros["TAREA"]["CantDiasNotaCreditoCliente"]);
                this.FechaInicial = DateTime.Today.AddDays(-dias);
                this.FechaFinal = DateTime.Today;
            }
            else
            {
                //Solo aceptara fechas en formato numerico YYYY/MM/dd
                Regex fechaRegex = new Regex(@"^\d{1,4}-\d{1,2}-\d{1,2}");

                if (fechaRegex.IsMatch(Parametros["ONDEMAND"]["FechaInicial"]))
                {
                    string[] arrayFecha = (Parametros["ONDEMAND"]["FechaInicial"]).Split('-').ToArray();

                    this.FechaInicial = new DateTime(
                        int.Parse(arrayFecha[0]),
                        int.Parse(arrayFecha[1]),
                        int.Parse(arrayFecha[2])
                        );
                }
                else
                {
                    throw new Exception("No se encontro en los parametros la key \"FECHAINICIAL\"") { Source = "NotaCredito.cs" };
                }

                if (fechaRegex.IsMatch(Parametros["ONDEMAND"]["FechaFinal"]))
                {
                    string[] arrayFecha = (Parametros["ONDEMAND"]["FechaFinal"]).Split('-').ToArray();

                    this.FechaFinal = new DateTime(
                        int.Parse(arrayFecha[0]),
                        int.Parse(arrayFecha[1]),
                        int.Parse(arrayFecha[2])
                        );
                }
                else
                {
                    throw new Exception("No se encontro en los parametros la key \"FECHAFINAL\"") { Source = "NotaCredito.cs" };
                }
            }
        }        

        private string ProcesarNotaDeCreditocliente (PagosParaNotaCredito pago, BLLReservaCab reservaCab,ref int contadorNC, decimal porcentXPaso,ref decimal currentPercent)
        {
            Int64 currentPaymentId = 0;
            
            string messageCorreo = string.Empty;

            try
            {
                long folio = BLLNotaCredito.GetFolio();

                string pnr = string.Empty;
                currentPaymentId = pago.PaymentId;
               
                var resultReservaCab = reservaCab.RecuperarReservaCabBookingid(pago.BookingId);                

                if (resultReservaCab.Count == 1)
                    pnr = resultReservaCab[0].RecordLocator;
                //paso 1
                PrintProgressbar(currentPercent, currentPercent += (int)porcentXPaso, "Se obtiene el PNR");
                WriteBitacora("Se obtiene el PNR", true);

                XElement xml = EditXML(pago, folio);
                //paso2
                PrintProgressbar(currentPercent, currentPercent += (int)porcentXPaso, "Se edito XML");
                WriteBitacora("Se edito XML", true);

                List<XElement> conceptos = xml.Element("Comprobante").Element("Conceptos").Elements("Concepto").ToList();
                List<XElement> traslados = new List<XElement>();

                foreach (XElement concepto in conceptos)
                {
                    traslados.AddRange(concepto.Element("Impuestos").Element("Traslados").Elements("Traslado").ToList());
                }

                //paso 3
                PrintProgressbar(currentPercent, currentPercent += (int)porcentXPaso, "Se ontuvieron conceptos y traslados");
                WriteBitacora("Se ontuvieron conceptos y traslados", true);

                var responsePegaso=TimbrarNotaCreditoCliente(xml.ToString(), folio, "NC", pnr);

                //BLLXmlPegaso bll = new BLLXmlPegaso();
                //ENTXmlPegaso responsePegaso = bll.RecuperarXmlPegasoFoliocfdi(10003284)[0];
                //var json = new JavaScriptSerializer().Serialize(responsePegaso);
                if (responsePegaso.Transaccion_Estatus.ToUpper() == "EXITO")
                {
                    contadorNC++;
                    //paso4
                    PrintProgressbar(currentPercent, currentPercent += (int)porcentXPaso, string.Format("Se timbro la Nota de credito del pago {0}",pago.PaymentId));
                    WriteBitacora(string.Format("Se timbro la Nota de credito del pago {0}", pago.PaymentId), true);

                    var jsonParam = new JavaScriptSerializer().Serialize(this.Parametros);

                    WriteBitacora(string.Format("Parametros:\n\r {0}", jsonParam), true);
                    WriteBitacora(string.Format("XML:\n\r {0}", xml), true);

                    var resSQL = BLLNotaCredito.GuardarNotaCreditoCliente(
                        responsePegaso,
                        xml,
                        this.Parametros,
                        GetConceptos(conceptos),
                        GetTraslados(traslados),
                        pago
                        );

                    if (resSQL.Succes)
                    {
                        PrintProgressbar(currentPercent, currentPercent += (int)porcentXPaso, "Se guardaron los datos correctaente");

                        WriteBitacora("Toda la informacon de la Nota de Credito fue guardada sastifactoriamente", true);
                        messageCorreo = string.Format("Se genero y guardo exitosamente la Nota de Credito del pago No. {0} con IdFacturaCab={1}\n\r", pago.PaymentId, resSQL.Message);
                    }
                    else
                    {
                        PrintProgressbar(currentPercent, currentPercent += (int)(porcentXPaso), string.Format("No se pudo guardar los datos del tmbrado del pago {0}", pago.PaymentId));
                        WriteBitacora(resSQL.Message, false);
                        messageCorreo = string.Format("{0}   - No se pudo guardar la información de la Nota de Credito generada del pago {1} \n\r", messageCorreo, pago.PaymentId);
                    }
                }
                else
                {
                    PrintProgressbar(currentPercent, currentPercent += (int)(porcentXPaso*2), string.Format("No se pudo timbrar el pago {0}", pago.PaymentId));
                    messageCorreo = string.Format("{0}Error al intentar timbrar la Nota de Credito del cliente con PaymentId {2}: {1} \n\r", messageCorreo, pago.PaymentId, responsePegaso.MensajeRespuesta);
                    WriteBitacora(string.Format("{0}Error al intentar timbrar la Nota de Credito del cliente con PaymentId {2}: {1} \n\r", messageCorreo, pago.PaymentId, responsePegaso.MensajeRespuesta), false);
                }
            }
            catch (SqlException ex)
            {
                messageCorreo = string.Format("{0}El pago {1} no se pudo generar su NC por un problema de BD \n\r", messageCorreo, currentPaymentId);
                log.escribir(ex, string.Format("Error al procesar el pago {0}", currentPaymentId));
            }
            catch (Exception ex)
            {
                messageCorreo = string.Format("{0}El pago {1} no se pudo generar su NC\n\r", messageCorreo, currentPaymentId);
                log.escribir(ex, string.Format("Error al procesar el pago {0}", currentPaymentId));
            }            

            return messageCorreo;
        }

        /// <summary>
        /// Genera notas informativas para el correo de succes
        /// </summary>
        /// <param name="fechaPago"></param>
        /// <param name="msgLotes"></param>
        /// <param name="cantidadPagos"></param>
        /// <returns></returns>
        private string GenerarMensajeSucces( string msgCorreo, int cantidadNC)
        {
            string message = string.Empty;
            message = string.Format("Se timbraron {0} Nota(s) de Credito del {1:dd-MMM-yyyy} a {2:dd-MMM-yyyy}\n\r", cantidadNC, this.FechaInicial,this.FechaFinal);
            message = string.Format("{0}{1}", message, msgCorreo);

            return message;
        }

        private XElement EditXML(PagosParaNotaCredito pago, long folio)
        {
            System.Xml.XmlDocument xmltest = new System.Xml.XmlDocument();
            xmltest.LoadXml(pago.XMLRequest);

            XmlNodeList nodoComprobante = xmltest.GetElementsByTagName("Comprobante");            

            nodoComprobante[0].Attributes["Serie"].Value =this.Parametros["COMPROBANTE"]["Serie"];
            nodoComprobante[0].Attributes["Folio"].Value = folio.ToString();
            nodoComprobante[0].Attributes["Fecha"].Value = string.Format("{0:yyyy-MM-dd}T{0:hh:mm:ss}", DateTime.Now);
            nodoComprobante[0].Attributes["TipoDeComprobante"].Value = this.Parametros["COMPROBANTE"]["TipoComprobante"];

            var objTipoCambio = nodoComprobante[0].Attributes["TipoCambio"];
            XmlAttribute attributeTipoCambio = xmltest.CreateAttribute("TipoCambio");
            attributeTipoCambio.Value = (objTipoCambio != null) ? (objTipoCambio.Value) : (
                (nodoComprobante[0].Attributes["Moneda"].Value == "MXN") ? ("1") : (BLLNotaCredito.GetDolarEnPesos(this.FechaInicial).ToString())
                );
            nodoComprobante[0].Attributes.Append(attributeTipoCambio);

            var t = GetCfdiRelacionados(pago.FacturaUUID, xmltest);            

            XmlNode nodoRespaldo = nodoComprobante[0].ChildNodes[0];
            XmlNode nodoNewValor = t;//nodoComprobante[0].ChildNodes[5];
            nodoComprobante[0].ReplaceChild(nodoNewValor, nodoComprobante[0].ChildNodes[0]);

            for(int i=1;i<=4;i++)
            {
                nodoNewValor = nodoRespaldo;
                nodoRespaldo = nodoComprobante[0].ChildNodes[i];
                nodoComprobante[0].ReplaceChild(nodoNewValor, nodoComprobante[0].ChildNodes[i]);
            }
            
            nodoNewValor = nodoRespaldo;
            nodoComprobante[0].AppendChild(nodoNewValor);

            XmlNodeList nodoTransaccion = xmltest.GetElementsByTagName("Transaccion");

            nodoTransaccion[0].Attributes["id"].Value = string.Format("ANA4000902{0:yyMMdd}CREDIT-{1}", DateTime.Now, folio.ToString().PadLeft(8, '0'));
            
            XElement nodoPrincipal = XElement.Load(new XmlNodeReader(xmltest));
            
            return nodoPrincipal;
        }

        private ENTXmlPegaso TimbrarNotaCreditoCliente(string xmlRequestFact, long folioCFDI, string tipoComprobante, string pnr)
        {
            BLLPegaso pegaso = new BLLPegaso();
            ENTXmlPegaso xmlPegaso = pegaso.EnviaTimbrado(xmlRequestFact, folioCFDI, tipoComprobante, pnr, false);
            //ENTXmlPegaso xmlPegaso = pegaso.EnviaTimbradoPegaso(xmlRequestFact, folioCFDI, tipoComprobante, pnr);
            //ENTXmlPegaso xmlPegaso = pegaso.EnviaTimbradoPegasoGlobal(nodo.ToString(), long.Parse(nodo.Element("Comprobante").Attribute("Folio").Value), "FG");            

            return xmlPegaso;
        }        

        private XmlNode GetCfdiRelacionados(string uuidFacturaGlobal, System.Xml.XmlDocument xmltest)
        {
            XmlNode newSub = xmltest.CreateNode(XmlNodeType.Element, "CfdiRelacionados", null);

            XmlAttribute attTipoRelacion = xmltest.CreateAttribute("TipoRelacion");
            attTipoRelacion.Value = Parametros["CFDIRELACIONADOS"]["TipoRelacion"];

            newSub.Attributes.Append(attTipoRelacion);

            XmlNode CfdiRelacionado = xmltest.CreateNode(XmlNodeType.Element, "CfdiRelacionado", null);
            XmlAttribute attUUID = xmltest.CreateAttribute("UUID");
            attUUID.Value = uuidFacturaGlobal;
            CfdiRelacionado.Attributes.Append(attUUID);

            newSub.AppendChild(CfdiRelacionado);

            return newSub;            
        }

        private void WriteBitacora(string txt, bool onlyDebug)
        {
            try
            {
                if (onlyDebug && txt != string.Empty)
                {
                    if (this.ModeDebug)
                    {
                        log.escribir(txt);
                        Console.WriteLine("{0:dd-MM-yyyy}   {0:HH:mm:ss.ffff} : {1}", DateTime.Now, txt);
                    }
                }
                else if (txt != string.Empty)
                {
                    log.escribir(txt);
                    Console.WriteLine("{0:dd-MM-yyyy}   {0:HH:mm:ss.ffff} : {1}", DateTime.Now, txt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0:dd-MM-yyyy}   {0:HH:mm:ss.ffff} : {1}", DateTime.Now, string.Format("No se pudo escribir en el log por lo siguiente: {0}", ex.Message));
            }
        }

        private List<Concepto> GetConceptos(List<XElement> xElementConceptos)
        {
            List<Concepto> conceptos = new List<Concepto>();

            for(int i=0;i<= xElementConceptos.Count-1;i++)
            {
                Concepto temp = new Concepto()
                {
                    IdFacturaDet = i + 1,
                    ClaveProdServ = xElementConceptos[i].Attribute("ClaveProdServ").Value,
                    NoIdentificacion = xElementConceptos[i].Attribute("NoIdentificacion").Value,
                    Cantidad = int.Parse(xElementConceptos[i].Attribute("Cantidad").Value),
                    ClaveUnidad = xElementConceptos[i].Attribute("ClaveUnidad").Value,
                    Descripcion= xElementConceptos[i].Attribute("Descripcion").Value,
                    Importe= decimal.Parse( xElementConceptos[i].Attribute("Importe").Value),
                    ValorUnitario=decimal.Parse( xElementConceptos[i].Attribute("ValorUnitario").Value)
                    //Unidad= hay q ver que pedo con la unidad
                };

                conceptos.Add(temp);
            }

            return conceptos;
        }

        private List<Traslado> GetTraslados(List<XElement> xElementTraslados)
        {
            List<Traslado> traslados = new List<Traslado>();

            for (int i = 0; i <= xElementTraslados.Count - 1; i++)
            {
                Traslado temp = new Traslado()
                {
                    IdFacturaDet = i + 1,
                    Base = xElementTraslados[i].Attribute("Base").Value,
                    Importe = decimal.Parse(xElementTraslados[i].Attribute("Importe").Value),
                    Impuesto = xElementTraslados[i].Attribute("Impuesto").Value,
                    TasaOCuota = decimal.Parse(xElementTraslados[i].Attribute("TasaOCuota").Value),
                    TipoFactor = xElementTraslados[i].Attribute("TipoFactor").Value
                };

                traslados.Add(temp);
            }

            return traslados;
        }

        private void PrintProgressbar(decimal oldValor, decimal newValor, string msj)
        {
            if (OnShowPorcentProgress != null)
                OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs((int)oldValor,(int) newValor, msj));
        }

        #endregion

    }
}

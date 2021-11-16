using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Facturacion.ENT.NotaCredito;
using Facturacion.BLL.NotaCredito;
using Facturacion.ENT;
using ExecuteProcess.ServiceEnt;
using ExecuteProcess.Interfaces;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.ENT.Comun;
using MansLog.Error;
using MansLog.ConfigXML;
using System.Configuration;
using System.Data.SqlClient;
using Process.Entidades;
using System.Text.RegularExpressions;
using Facturacion.BLL;

namespace ExecuteProcess.Process
{
    public class NotaCredito : IProcess
    {
        #region Propiedades Publicas
        public bool OnDemand { get; set; }

        /// <summary>
        /// Bandera de activacion Modo debug
        /// </summary>
        public bool ModeDebug { get; set; }

        public Dictionary<string, Dictionary<string, string>> Parametros { get; set; }

        #endregion

        #region Propiedades Privadas
        private LogError log { get; set; }

        private DateTime FechaInicial { get; set; }

        public DateTime FechaFinal { get; set; }


        #endregion

        public event ShowPorcentProgress OnShowPorcentProgress;

        #region Metodos Publicos
        public NotaCredito()
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
                Console.WriteLine("Sucedio un error al instanciar NotaCredito.cs : {0}", ex.Message);
                throw ex;
            }
        }

        public ResponseTask ValidationsBeforeExecution()
        {
            try
            {
                var resultConexion = BLLNotaCredito.TestConexionBD();

                if (resultConexion.Succes)
                {
                    List<StoredProcedureValidation> sps = new List<StoredProcedureValidation>();

                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertaNotasCreditoCab" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertNotasCredito_Det" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertNotasCreditoIVA_Det" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertaNotaCreditoCFDIDet" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertUpdateGlobalticketsDet" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsCFDIRelacionadosDet" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetPagosFacturadosDiferenteDia" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetFacturasDetByPaymentId" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetFacturasIVADetByPaymentId" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetFacturasCab_POR_PK" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetFacturasDet_ByLote" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetGlobalTicketsByIdFacturaCabStringPayment" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetPagosHijosFacturadosDiferenteDiaPadre" });
                    
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
            var response = new ResponseTask();
            string messageLotes = string.Empty;
            int /*totalPagos = 0*/ contador = 0;
            decimal percentXpaso =0, percentAnterior = 10.0M;
            XElement xml;

            try
            {
                WriteBitacora(string.Format("Inicia \"MainProcess()\" de {0}", this.GetType().Name), false);

                SetFechas();

                if (OnShowPorcentProgress != null)
                    OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs((int)Math.Floor(percentAnterior), (int)Math.Floor(percentAnterior + 2), string.Format("Proceso Inicializado con generacion de Notas de credito desde {0:dd-MMM-yyyy} hasta {1:dd-MMM-yyyy} con {2}", this.FechaInicial, this.FechaFinal, this.Parametros["TAREA"]["StoredProcedure"].Replace("uspFac_", string.Empty))));

                percentAnterior = percentAnterior + 2;

                WriteBitacora(string.Format("Se generaran notas de credito desde {0:dd-MMM-yyyy} hasta {1:dd-MMM-yyyy} con {2}", this.FechaInicial, this.FechaFinal, this.Parametros["TAREA"]["StoredProcedure"].Replace("uspFac_",string.Empty)), false);

                List<PagosParaNotaCredito> facturasDiferenteDia = BLLNotaCredito.GetPagosNotaCredito(this.FechaInicial,this.FechaFinal,this.Parametros["TAREA"]["StoredProcedure"]);

                ///Se obtiene un enumerable con solo IdfacturaCab Global
                var groupedGlobalIdFacturaCab = facturasDiferenteDia.OrderBy(x => x.GlobalIdFacturaCab)
                   .GroupBy(x => x.GlobalIdFacturaCab).ToList();

                if (OnShowPorcentProgress != null)
                    OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs((int)Math.Floor(percentAnterior), (int)Math.Floor(percentAnterior + 8), string.Format("Se encontraron {0} pagos para generar {1} Notas de credito", facturasDiferenteDia.Count, groupedGlobalIdFacturaCab.Count)));

                percentAnterior = percentAnterior + 8;

                WriteBitacora(string.Format("Se encontraron {0} pagos para generar {1} Notas de credito", facturasDiferenteDia.Count, groupedGlobalIdFacturaCab.Count), false);

                if(groupedGlobalIdFacturaCab != null && groupedGlobalIdFacturaCab.Count > 0)
                    percentXpaso = (decimal)(75.0M / (decimal)(groupedGlobalIdFacturaCab.Count * 4.0M));

                foreach (var idFacturaCab in groupedGlobalIdFacturaCab)
                {
                    contador++;

                    try
                    {

                        List<PagosParaNotaCredito> lote = facturasDiferenteDia.FindAll(f => f.GlobalIdFacturaCab == idFacturaCab.Key);

                        WriteBitacora(string.Format("Se generara la Notade Credito con {0} pagos", lote.Count), true);

                        InfoNotaCredito info = BLLNotaCredito.GetInfoNotaCredito(idFacturaCab.Key, lote.Select(o => o.PaymentId).ToArray());

                        if (OnShowPorcentProgress != null)
                            OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs((int)Math.Floor(percentAnterior), (int)Math.Floor(percentAnterior + percentXpaso), "Se obtiene la informacion necesaria para la construccion de la Nota de Credito"));

                        percentAnterior = percentAnterior + percentXpaso;
                        WriteBitacora("Se obtiene la informacion necesaria para la construccion de la Nota de Credito", true);

                        xml = BuildXML(lote, idFacturaCab, info);

                        if (OnShowPorcentProgress != null)
                            OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs((int)Math.Floor(percentAnterior), (int)Math.Floor(percentAnterior + percentXpaso), "Se construyo el XML"));

                        percentAnterior = percentAnterior + percentXpaso;
                        WriteBitacora("Se construyo el XML", true);

                        var responsePegaso = TimbrarFacturaGlobal(xml);

                        if (responsePegaso.Transaccion_Estatus.ToUpper() == "EXITO")
                        {
                            if (OnShowPorcentProgress != null)
                                OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs((int)Math.Floor(percentAnterior), (int)Math.Floor(percentAnterior + percentXpaso), string.Format("El lote con IdPeticionPAC={0} fue timbrado con un UUID={1}", responsePegaso.IdPeticionPAC, responsePegaso.TFD_UUID)));

                            percentAnterior = percentAnterior + percentXpaso;

                            WriteBitacora(string.Format("El lote con IdPeticionPAC={0} fue timbrado con un UUID={1}", responsePegaso.IdPeticionPAC, responsePegaso.TFD_UUID), true);
                            messageLotes = string.Format("{0}Se creo la Nota de Credito con IdPeticionPAC = {1} con lo siguiente:\n\r   - {2} pagos en Nota de Credito el cual pertenecen de la Factura Global con IdFacturaCab = {3} \n\r   - UUID = {4}\n\r   - Folio CFDI = {5}\n\r", messageLotes, responsePegaso.IdPeticionPAC, lote.Count, idFacturaCab.Key, responsePegaso.TFD_UUID, responsePegaso.FolioCFDI);

                            var resSQL = BLLNotaCredito.GuardarNotaCredito(responsePegaso, xml, this.Parametros, info, lote, idFacturaCab.Key);

                            if (resSQL.Succes)
                            {
                                if (OnShowPorcentProgress != null)
                                    OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs((int)Math.Floor(percentAnterior), (int)Math.Floor(percentAnterior + percentXpaso), "Se guardaron datos"));

                                percentAnterior = percentAnterior + percentXpaso;

                                WriteBitacora(resSQL.Message, true);
                                WriteBitacora(string.Format("Se termino exitosamente el lote No. {0}", contador), false);
                            }
                            else
                            {
                                if (OnShowPorcentProgress != null)
                                    OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs((int)Math.Floor(percentAnterior), (int)Math.Floor(percentAnterior + percentXpaso), "No se pudo guardar"));

                                percentAnterior = percentAnterior + percentXpaso;

                                WriteBitacora(resSQL.Message, false);
                                messageLotes = string.Format("{0}   - No se pudo guardar la información de este lote\n\r", messageLotes);
                            }
                        }
                        else
                        {
                            if (OnShowPorcentProgress != null)
                                OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs((int)Math.Floor(percentAnterior), (int)Math.Floor(percentAnterior + (percentXpaso*2)), "No se pudo timbrar"));

                            percentAnterior = percentAnterior + (percentXpaso*2);

                            messageLotes = string.Format("{0}Error al intentar realizar la Nota de Credito del lote: {1} con IdFacturaCab {2}\n\r", messageLotes, responsePegaso.MensajeRespuesta, idFacturaCab.Key);
                            WriteBitacora(string.Format("{0}Error al intentar facturar el lote: {1} con IdFacturaCab {2}\n\r", messageLotes, responsePegaso.MensajeRespuesta, idFacturaCab.Key), false);
                        }
                    }
                    catch(SqlException ex)
                    {
                        messageLotes = string.Format("{0}El lote {1}  no se genero por un problema en BD \n\r", messageLotes, contador);
                        log.escribir(ex, string.Format("Error al procesar el lote {0}", contador));
                    }
                    catch(Exception ex)
                    {
                        messageLotes = string.Format("{0}El lote {1} no se genero\n\r", messageLotes, contador);
                        log.escribir(ex, string.Format("Error al procesar el lote {0}", contador));
                    }
                }

                response.Succes = true;
                response.Message = GenerarMensajeSucces(this.FechaInicial,this.FechaFinal, messageLotes, facturasDiferenteDia.Count, groupedGlobalIdFacturaCab.Count);

                if (OnShowPorcentProgress != null)
                    OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs((int)Math.Floor(percentAnterior), 95, "Fin de Nota de Credito"));
            }
            catch (Exception ex)
            {
                response.Succes = false;
                response.StackTrace = ex.StackTrace;
                response.Message = string.Format(" - Error: {0}\n\r - Stack Trace: {1}", ex.Message,ex.StackTrace);

                log.escribir(ex, "MainProcess");
            }

            return response;
        }

        #endregion

        #region Metodos Privados

        private void SetFechas()
        {
            //DateTime fecha = DateTime.Today.AddDays(
            //       -int.Parse(
            //           (OnDemand) ? (Parametros["OnDemand"]["CANTIDADDIASOBTENERPAGOSFACT"]) : (Parametros["Tarea"]["CANTDIAS_GETFACTURAS_DIFERENTEDIA"])
            //           ));

            if (!OnDemand)
            {
                int dias = int.Parse(Parametros["TAREA"]["CantDias_GetFacturas_DiferenteDia"]);
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

        private XElement UpdateTagComprobante(Comprobante tagComprobante, decimal total, decimal subTotal)
        {
            //tagComprobante.Version = Parametros["Comprobante"]["VERSION"];
            tagComprobante.Serie = Parametros["COMPROBANTE"]["Serie"];
            tagComprobante.Folio = BLLNotaCredito.GetFolio();

            // DHV INI 29-OCT-2019 Fecha de envío según huso horario del lugar exp
            //tagComprobante.Fecha = string.Format("{0:yyyy-MM-dd}T{0:hh:mm:ss}", DateTime.Now);
            BLLCpSatCat bllCPsat = new BLLCpSatCat();
            BLLEntidadesfedSatCat bllEntidadesFed = new BLLEntidadesfedSatCat();
            List<ENTCpSatCat> listaCP = new List<ENTCpSatCat>();
            List<ENTEntidadesfedSatCat> listaEntFed = new List<ENTEntidadesfedSatCat>();

            listaCP = bllCPsat.RecuperarCpSatCatPorLlavePrimaria(tagComprobante.LugarExpedicion);
            String entidadFede = listaCP != null && listaCP.Count > 0 ? listaCP.FirstOrDefault().ClaveEntidadFed : "";
            if (String.IsNullOrEmpty(entidadFede))
                throw new Exception("Lugar de Expedicion: " + tagComprobante.LugarExpedicion + ", no dado de alta");

            listaEntFed = bllEntidadesFed.RecuperarEntidadesfedSatCatPorLlavePrimaria(entidadFede);
            String husoHorario = listaEntFed != null && listaEntFed.Count > 0 ? listaEntFed.FirstOrDefault().HusoHorario : "";

            if (String.IsNullOrEmpty(husoHorario))
                throw new Exception("Entidad federativa: " + entidadFede + ", no dado de alta");

            DateTime fechaEnvio = DateTime.Now;
            //fechaEnvio = fechaEnvio.AddSeconds(numSegDesfaseSAT * -1);

            if (!String.IsNullOrEmpty(tagComprobante.LugarExpedicion) && int.Parse(tagComprobante.LugarExpedicion) > 0)
            {
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(husoHorario);
                fechaEnvio = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, tzi);
            }

            tagComprobante.Fecha = string.Format("{0:yyyy-MM-dd}T{0:hh:mm:ss}", fechaEnvio);
            //xmlFact.Append("Fecha=\"" + fechaEnvio.ToString("yyyy-MM-ddTHH:mm:ss") + "\" ");
            // DHV FIN 29-OCT-2019 Fecha de envío según huso horario del lugar exp

            //tagComprobante.FormaPago = pagoMax.IdFormaPago;
            //tagComprobante.NoCertificado = empresa.NoCertificado;
            tagComprobante.Total = total;
            tagComprobante.SubTotal = subTotal;
            //tagComprobante.Moneda = moneda;
            tagComprobante.TipoDeComprobante = Parametros["COMPROBANTE"]["TipoComprobante"];
            //tagComprobante.MetodoPago = Parametros["Comprobante"]["METODOPAGO"];
            //tagComprobante.LugarExpedicion = Parametros["Comprobante"]["LUGAREXPEDICION"];
            tagComprobante.permitirConfirmacion = "false";
            tagComprobante.TipoCambio = decimal.Parse((tagComprobante.Moneda.ToUpper() == "MXN") ? ("1") : tagComprobante.TipoCambio.ToString());

            return tagComprobante.GetXml();
        }

        /// <summary>
        /// Genera la factura
        /// </summary>
        /// <param name="pagos">Pagos a facturar</param>
        /// <param name="moneda">moneda de a factura</param>
        /// <param name="fechaPagos">fecha de los pagos</param>
        /// <returns>IdPeticionPAC que regresa serv de pegaso</returns>
        private ENTXmlPegaso TimbrarFacturaGlobal(XElement xmlFacturaGloba)
        {
            BLLPegaso pegaso = new BLLPegaso();
            ENTXmlPegaso xmlPegaso = pegaso.EnviaTimbrado(xmlFacturaGloba.ToString(), long.Parse(xmlFacturaGloba.Element("Comprobante").Attribute("Folio").Value), "NG", "", false);
            //ENTXmlPegaso xmlPegaso = pegaso.EnviaTimbradoPegaso(xmlFacturaGloba.ToString(), long.Parse(xmlFacturaGloba.Element("Comprobante").Attribute("Folio").Value), "NG", "");
            //ENTXmlPegaso xmlPegaso = pegaso.EnviaTimbradoPegasoGlobal(nodo.ToString(), long.Parse(nodo.Element("Comprobante").Attribute("Folio").Value), "FG");            

            return xmlPegaso;
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

        /// <summary>
        /// Genera notas informativas para el correo de succes
        /// </summary>
        /// <param name="fechaPago"></param>
        /// <param name="msgLotes"></param>
        /// <param name="cantidadPagos"></param>
        /// <returns></returns>
        private string GenerarMensajeSucces(DateTime fechaInicio,DateTime fechaFin, string msgLotes, int cantidadPagos, int cantidadNotasCredito)
        {
            string message = string.Empty, tipoCambio = string.Empty;
            message = string.Format("Se obtuvieron {0} pagos en {1} Nota(s) de Credito de {2:dd-MMM-yyyy} a {3:dd-MMM-yyyy}\n\r", cantidadPagos, cantidadNotasCredito, fechaInicio,fechaFin);
            message = string.Format("{0}{1}", message, msgLotes);

            return message;
        }
        
        #region Construccion XML

        private XElement BuildXML(List<PagosParaNotaCredito> facturasDiferenteDia, IGrouping<long, PagosParaNotaCredito> idFacturaCab, InfoNotaCredito info)
        {
            XElement tagMain = new XElement("RequestCFD", new XAttribute("version", Parametros["COMPROBANTE"]["Version"]));

            decimal sumConceptos = info.Conceptos.Sum(s => s.ValorUnitario), sumImporte = info.Traslados.Sum(s => s.Importe);

            XElement tagComprobante = UpdateTagComprobante(info.Comprobante, sumConceptos + sumImporte, sumConceptos);

            tagComprobante.SetAttributeValue("Version", Parametros["COMPROBANTE"]["Version"]);

            tagComprobante.Add(
                GetCfdiRelacionados(facturasDiferenteDia.Find(f => f.GlobalIdFacturaCab == idFacturaCab.Key).GlobalUUID)
                );

            tagComprobante.LastNode.AddAfterSelf(
          GetTagEmisor(BLLNotaCredito.GetAllEmpresas().Find(f => f.Rfc == Parametros["EMISOR"]["RFC"]))
              );

            tagComprobante.LastNode.AddAfterSelf(
                GetTagReceptor()
                );

            XElement tagConceptos = new XElement("Conceptos");

            foreach (Concepto concepto in info.Conceptos)
            {
                List<Traslado> trasladosByConcepto = info.Traslados.FindAll(f => f.IdFacturaDet == concepto.IdFacturaDet).ToList();

                concepto.ClaveProdServ = Parametros["CONCEPTO"]["ClaveProdServ"];
                //concepto.NoIdentificacion = pago.PaymentId.ToString();
                //concepto.Cantidad = int.Parse(Parametros["Concepto"]["CANTIDAD"]);
                //concepto.ClaveUnidad = Parametros["Concepto"]["CLAVEUNIDAD"];
                //concepto.Descripcion = Parametros["Concepto"]["DESCRIPCION"];
                concepto.ValorUnitario = info.FacturaDet.Find(f => f.IdFacturaDet == concepto.IdFacturaDet).Importe;//trasladosByConcepto.Sum(s=> Decimal.Parse(s.Base));
                concepto.Importe = (concepto.ValorUnitario * concepto.Cantidad);

                XElement tagConcepto = concepto.GetXml();
                XElement tagImpuestos = new XElement("Impuestos");

                XElement tagTraslados = new XElement("Traslados");

                if (decimal.Parse(trasladosByConcepto[0].Base) > 0)
                    tagTraslados.Add(trasladosByConcepto[0].GetXml());

                if (decimal.Parse(trasladosByConcepto[1].Base) > 0)
                    tagTraslados.Add(trasladosByConcepto[1].GetXml());

                tagImpuestos.Add(tagTraslados);


                tagConcepto.Add(tagImpuestos);

                tagConceptos.Add(tagConcepto);
            }

            tagComprobante.LastNode.AddAfterSelf(tagConceptos);

            tagComprobante.LastNode.AddAfterSelf(GetTagImpuestos(tagConceptos));

            tagComprobante.LastNode.AddAfterSelf(
            GetTagComplemento(BLLNotaCredito.GetCargosComplemento(string.Join("|", facturasDiferenteDia.Select(s => s.IdPagosCab.ToString()).ToArray()), info.Comprobante.Moneda))
            );

            tagMain.Add(tagComprobante);

            tagMain.LastNode.AddAfterSelf(GetTagTrasaccion(tagComprobante.Attribute("Folio").Value));

            tagMain.LastNode.AddAfterSelf(GetTagTipoComprobante());

            tagMain.LastNode.AddAfterSelf(GetTagSucursal());

            return tagMain;
        }

        private XElement GetTagEmisor(ENTEmpresaCat empresa)
        {
            Emisor tagEmisor = new Emisor();

            tagEmisor.Nombre = empresa.RazonSocial;
            tagEmisor.RegimenFiscal = empresa.IdRegimenFiscal;
            tagEmisor.Rfc = empresa.Rfc;

            return tagEmisor.GetXml();
        }

        private XElement GetTagReceptor()
        {
            Receptor tagReceptor = new Receptor();

            tagReceptor.Rfc = Parametros["RECEPTOR"]["RFC"];
            tagReceptor.UsoCFDI = Parametros["RECEPTOR"]["UsoCFDI"];
            tagReceptor.emailReceptor = string.Empty;
            tagReceptor.codigoReceptor = string.Empty;

            return tagReceptor.GetXml();
        }

        private XElement GetTagImpuestos(XElement tagConceptos)
        {
            List<XElement> traslados = new List<XElement>();
            XElement impuestosTag = new XElement("Impuestos");
            XElement trasladosTag = new XElement("Traslados");

            foreach (XElement concepto in tagConceptos.Elements("Concepto").ToList())
            {
                traslados.AddRange(concepto.Element("Impuestos").Elements("Traslados").ToList());
            }

            var trasladosElementsList = traslados.Elements("Traslado").ToList();
            var traslado16 = trasladosElementsList.FindAll(f => float.Parse(f.Attribute("TasaOCuota").Value) == 0.16f).ToList();

            decimal importe16 = traslado16.Sum(s => decimal.Parse(s.Attribute("Importe").Value));

            var traslado0 = trasladosElementsList.FindAll(f => float.Parse(f.Attribute("TasaOCuota").Value) == 0.00f).ToList();
            decimal base0 = traslado0.Sum(s => decimal.Parse(s.Attribute("Base").Value));
            //decimal base16 = traslado16.Sum(s => decimal.Parse(s.Attribute("Base").Value));

            Traslado traslado0Tag = new Traslado();
            traslado0Tag.Importe = 0.0M;
            traslado0Tag.Impuesto = Parametros["TRASLADO"]["Impuesto"];
            traslado0Tag.TasaOCuota = 0.0M;
            traslado0Tag.TipoFactor = Parametros["TRASLADO"]["TipoFactor"];
            traslado0Tag.Base = string.Empty;

            Traslado traslado16Tag = new Traslado();
            traslado16Tag.Importe = importe16;
            traslado16Tag.Impuesto = Parametros["TRASLADO"]["Impuesto"];
            traslado16Tag.TasaOCuota = Decimal.Parse(Parametros["TRASLADO"]["IVA"]);
            traslado16Tag.TipoFactor = Parametros["TRASLADO"]["TipoFactor"];
            traslado16Tag.Base = string.Empty;

            if (base0 > 0)
                trasladosTag.Add(traslado0Tag.GetXml());

            trasladosTag.Add(traslado16Tag.GetXml());

            impuestosTag.Add(trasladosTag);
            impuestosTag.Add(new XAttribute("TotalImpuestosTrasladados", importe16));

            return impuestosTag;
        }

        private XElement GetTagTipoComprobante()
        {
            XElement tipoComprobanteTag = new XElement("TipoComprobante");
            XAttribute clave = new XAttribute("clave", Parametros["TIPOCOMPROBANTE"]["Clave"]);
            XAttribute nombre = new XAttribute("nombre", Parametros["TIPOCOMPROBANTE"]["Nombre"]);

            tipoComprobanteTag.Add(clave, nombre);

            return tipoComprobanteTag;
        }

        private XElement GetTagSucursal()
        {
            XElement sucursalTag = new XElement("Sucursal");
            XAttribute nombre = new XAttribute("nombre", Parametros["SUCURSAL"]["Nombre"]);

            sucursalTag.Add(nombre);

            return sucursalTag;
        }

        private XElement GetTagTrasaccion(string folio)
        {
            string id = string.Empty;

            id = string.Format("ANA4000902{0:yyMMdd}CREDIT-{1}", DateTime.Now, folio.PadLeft(8, '0'));

            XElement transaccionTag = new XElement("Transaccion");
            transaccionTag.Add(new XAttribute("id", id));

            return transaccionTag;
        }

        private XElement GetCfdiRelacionados(string uuidFacturaGlobal)
        {
            XElement cfdiRelacionados = new XElement("CfdiRelacionados");
            cfdiRelacionados.Add(new XAttribute("TipoRelacion", Parametros["CFDIRELACIONADOS"]["TipoRelacion"]));

            XElement cfdiRelacionado = new XElement("CfdiRelacionado");
            cfdiRelacionado.Add(new XAttribute("UUID", uuidFacturaGlobal));

            cfdiRelacionados.Add(cfdiRelacionado);

            return cfdiRelacionados;
        }

        private XElement GetTagComplemento(List<Cargo> cargos)
        {
            XElement elementComplemento = new XElement("Complemento");
            XElement elementAerolineas = new XElement("Aerolineas");
            XElement elementOtrosCargos = new XElement("OtrosCargos");

            Cargo cargoTUA = cargos.Find(f => f.CodigoCargo.ToUpper() == "XV");

            elementAerolineas.Add(new XAttribute("Version", Parametros["COMPLEMENTO"]["Version"]));
            elementAerolineas.Add(new XAttribute("TUA", (cargoTUA == null) ? (0.00M) : (cargoTUA.Importe)));

            if (cargoTUA != null)
            {
                cargos.Remove(cargoTUA);
            }

            elementOtrosCargos.Add(new XAttribute("TotalCargos", cargos.Sum(s => s.Importe)));

            foreach (Cargo cargo in cargos)
            {
                elementOtrosCargos.Add(cargo.GetXml());
            }

            if (cargos.Count > 0)
            {
                elementAerolineas.Add(elementOtrosCargos);
            }

            elementComplemento.Add(elementAerolineas);

            return elementComplemento;
        }

        #endregion

        #endregion
    }
}

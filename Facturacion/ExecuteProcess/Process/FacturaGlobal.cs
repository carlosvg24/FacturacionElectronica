using ExecuteProcess.Interfaces;
using ExecuteProcess.ServiceEnt;
using Facturacion.BLL;
using Facturacion.BLL.FacturaGlobal;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.ENT;
using Facturacion.ENT.Comun;
using Facturacion.ENT.FacturaGlobal;
using MansLog.ConfigXML;
using MansLog.Error;
using MetodosComunes;
using Process.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExecuteProcess.Process
{
    public class FacturaGlobal : IProcess
    {
        #region Propiedades Publicas

        /// <summary>
        /// Parametros de la tarea
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Parametros { get; set; }

        /// <summary>
        /// Bandera de activacion Modo debug
        /// </summary>
        public bool ModeDebug { get; set; }

        /// <summary>
        /// Bandera indicacion bajo demanda
        /// </summary>
        public bool OnDemand { get; set; }

        #endregion

        #region Propiedades Privadas

        private LogError log { get; set; }

        private DateTime FechaInicial { get; set; }

        private DateTime FechaFinal { get; set; }

        #endregion

        public event ShowPorcentProgress OnShowPorcentProgress;

        #region Metodos Publicos
        public FacturaGlobal()
        {
            this.OnDemand = false;
            this.Parametros = new Dictionary<string, Dictionary<string, string>>();
            LogMansSection sectionLog = (LogMansSection)ConfigurationManager.GetSection("FacturacionServiceSettings/LogMans");

            this.log = new LogError(sectionLog, this.GetType().Name);
            this.log.StackTraceCompleto = true;
            this.log.TipoStackTrace = MansLog.EL.TypeStackTrace.Default;
        }

        /// <summary>
        /// Validaciones previas para poder correr el Main Procees de la tarea
        /// </summary>
        /// <returns></returns>
        public ResponseTask ValidationsBeforeExecution()
        {
            try
            {
                var resultConexion = BLLFacturaGlobal.TestConexionBD();

                if (resultConexion.Succes)
                {
                    List<StoredProcedureValidation> sps = new List<StoredProcedureValidation>();

                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertaFacturasCab" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertaFacturasDet" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertarFacturasIVADet" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertUpdateGlobalticketsDet" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsertaFacturasCargos_Det" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_InsFacturasCFDIDet" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetPagosFactGlobal" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetTipoMonedaPagos" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetReservaDetalleFactGlobal" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetComplementoByIdPagosCab" });
                    sps.Add(new StoredProcedureValidation() { NameSp = "uspFac_GetPagosHijosPorProcesoAtrasado" });

                    var resultSps = BLLFacturaGlobal.ValidarSpNecesarios(sps);

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

        /// <summary>
        /// Proceso Principal
        /// </summary>
        /// <returns></returns>
        public ResponseTask MainProcess()
        {
            var response = new ResponseTask();
            string messageLotes = string.Empty;
            int totalPagosFacturados = 0, percentAnterior = 10;

            try
            {
                WriteBitacora(string.Format("Inicia \"MainProcess()\" de {0}", this.GetType().Name), false);

                SetFechas();

                if (OnShowPorcentProgress != null)
                    OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs(percentAnterior, percentAnterior + 2, string.Format("Proceso Inicializado con fecha de proceso de {0:dd-MMM-yyyy} a {1:dd-MMM-yyyy}", this.FechaInicial, this.FechaFinal)));

                percentAnterior = percentAnterior + 2;

                WriteBitacora(string.Format("Se procesaran pagos con fecha de {0:dd-MMM-yyyy} al {1:dd-MMM-yyyy}", this.FechaInicial, this.FechaFinal), false);

                bool activarProcesoGlobal33 = false;
                BLLParametrosCnf bllParam = new BLLParametrosCnf();
                List<ENTParametrosCnf> listaParametros = new List<ENTParametrosCnf>();
                listaParametros = bllParam.RecuperarParametrosCnfNombre("ActivaGlobal33");
                if (listaParametros.Count == 1)
                {
                    if (bllParam.Activo == true)
                    {
                        if (bllParam.Valor == "true")
                        {
                            activarProcesoGlobal33 = true;
                        }
                    }
                }

                //Dependiendo del parametro se identifica que proceso se va a ejecutar, si el proceso anterior o el nuevo
                if (activarProcesoGlobal33 == false)
                {
                    //En este caso se dispara el proceso anterior donde se utiliza solo los ivas 0 y 16 como base
                    List<MonedaPagos> monedas = BLLFacturaGlobal.BLGetPagosByCurrencyCode(this.FechaInicial, this.FechaFinal);

                    if (OnShowPorcentProgress != null)
                        OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs(percentAnterior, percentAnterior + 3, string.Format("Se encontraron los siguientes Monedas: {0}", string.Join(",", monedas.Select(m => m.Moneda).ToArray()))));

                    percentAnterior = percentAnterior + 3;

                    WriteBitacora(string.Format("Se encontraron los siguientes Monedas: {0}", string.Join(",", monedas.Select(m => m.Moneda).ToArray())), false);

                    foreach (MonedaPagos moneda in monedas)
                    {
                        GeneracionFacturasGlobalesByMoneda(this.FechaInicial, this.FechaFinal, moneda, ref totalPagosFacturados, ref messageLotes, ref percentAnterior);
                    }

                    response.Succes = true;
                    response.Message = GenerarMensajeSucces(messageLotes, totalPagosFacturados);
                }
                else
                {
                    //En este caso se disparara el nuevo proceso que analiza diferentes porcentajes de IVA
                    BLLFacturaGlobal33 bllFactGlobal33 = new BLLFacturaGlobal33();
                    bllFactGlobal33.AsignarEsFacturableGlobal(FechaInicial, FechaFinal);
                    bllFactGlobal33.GeneracionFacturasGlobales(FechaInicial, FechaFinal);
                    response.Succes = true;
                    response.Message = "FacturaGlobal33";
                }

            }
            catch (Exception ex)
            {
                response.Succes = false;
                response.StackTrace = ex.StackTrace;
                response.Message = ex.Message;

                log.escribir(ex, "MainProcess");
            }

            WriteBitacora("Proceso finalizado", false);

            return response;
        }

        #endregion

        #region Metodos Privados

        private void GeneracionFacturasGlobalesByMoneda(DateTime fechaInicial, DateTime fechaFinal, MonedaPagos moneda, ref int totalPagosFacturados, ref string messageLotes, ref int percentAnterior)
        {
            int percentFinal = percentAnterior + 40, percentXpaso = 0;
            List<PagoTraslados> pagosAll = new List<PagoTraslados>();
            List<PagoTraslados> lotePagos = new List<PagoTraslados>();
            List<Cargo> cargos = new List<Cargo>();

            byte splitPays = byte.Parse(Parametros["TAREA"]["SplitPayX1000"]);

            pagosAll = VerificadorCuadranteCantidades(BLLFacturaGlobal.BLGetPagos(/*log,*/Decimal.Parse(Parametros["TRASLADO"]["IVA"]), fechaInicial, fechaFinal, moneda.Moneda, null));

            if (OnShowPorcentProgress != null)
                OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs(percentAnterior, percentAnterior + 5, string.Format("Se encontraron {0} pagos ya cuadrados para facturarse", pagosAll.Count)));

            percentAnterior = percentAnterior + 5;

            WriteBitacora(string.Format("Se encontraron {0} pagos ya cuadrados para facturarse", pagosAll.Count), false);

            int cantidadXfactura = (splitPays * 1000), numeroFacturasGlobal = pagosAll.Count / cantidadXfactura, pico = pagosAll.Count % cantidadXfactura, inicio = 0;

            WriteBitacora(string.Format("Cada Factura Global contendra maximo {0} pagos", cantidadXfactura), true);

            numeroFacturasGlobal = (pico == 0) ? (numeroFacturasGlobal) : (numeroFacturasGlobal + 1);

            if (numeroFacturasGlobal > 0)
                percentXpaso = (int)Math.Floor((decimal)(35 / (numeroFacturasGlobal * 4)));

            for (int i = 1; i <= numeroFacturasGlobal; i++)
            {
                WriteBitacora(string.Format("Inicio del lote No. {0}", i), true);

                lotePagos = (i == numeroFacturasGlobal && pico != 0) ? (pagosAll.GetRange(inicio, pico)) : (pagosAll.GetRange(inicio, cantidadXfactura));

                cargos = BLLFacturaGlobal.BLCargoComplemento<Cargo>(string.Join("|", lotePagos.Select(s => s.IdPagosCab.ToString()).ToArray()), moneda.Moneda);

                if (OnShowPorcentProgress != null)
                    OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs(percentAnterior, percentAnterior + percentXpaso, string.Format("Inicio del lote No. {0} se encontraron {1} cargos", i, cargos.Count)));

                percentAnterior = percentAnterior + percentXpaso;

                try
                {
                    XElement nodo = BuildXML(moneda.Moneda, lotePagos, cargos);

                    if (OnShowPorcentProgress != null)
                        OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs(percentAnterior, percentAnterior + percentXpaso, "Se construyo el XML"));

                    percentAnterior = percentAnterior + percentXpaso;

                    var responsePegaso = TimbrarFacturaGlobal(nodo);

                    if (responsePegaso.Transaccion_Estatus.ToUpper() == "EXITO")
                    {
                        if (OnShowPorcentProgress != null)
                            OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs(percentAnterior, percentAnterior + percentXpaso, string.Format("El lote con IdPeticionPAC={0} fue timbrado con un UUID={1}", responsePegaso.IdPeticionPAC, responsePegaso.TFD_UUID)));

                        percentAnterior = percentAnterior + percentXpaso;

                        WriteBitacora(string.Format("El lote con IdPeticionPAC={0} fue timbrado con un UUID={1}", responsePegaso.IdPeticionPAC, responsePegaso.TFD_UUID), true);

                        totalPagosFacturados = totalPagosFacturados + lotePagos.Count;

                        messageLotes = string.Format("{0}Se facturo el lote con IdPeticionPAC = {1} con lo siguiente:\n\r   - {2} pagos facturados\n\r   - Pagos en {3} \n\r   - UUID = {4}\n\r   - Folio CFDI = {5}\n\r", messageLotes, responsePegaso.IdPeticionPAC, lotePagos.Count, moneda.Moneda, responsePegaso.TFD_UUID, responsePegaso.FolioCFDI);

                        var resSQL = BLLFacturaGlobal.GuardarFacturaGlobal(nodo, responsePegaso, lotePagos, Parametros);

                        string mensajeDisparadorManual = string.Empty;

                        if (resSQL.Succes)
                        {
                            mensajeDisparadorManual = "Datos Guardados en la BD";
                            WriteBitacora(resSQL.Message, true);
                            WriteBitacora(string.Format("Se termino exitosamente el lote No. {0}", i), false);
                        }
                        else
                        {
                            mensajeDisparadorManual = string.Format("{0}   - No se pudo guardar la información de este lote\n\r", messageLotes);
                            WriteBitacora(resSQL.Message, false);
                            messageLotes = mensajeDisparadorManual;
                        }

                        if (OnShowPorcentProgress != null)
                            OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs(percentAnterior, percentAnterior + percentXpaso, mensajeDisparadorManual));

                        percentAnterior = percentAnterior + percentXpaso;
                    }
                    else
                    {
                        messageLotes = string.Format("{0}Error al intentar facturar el lote: {1}\n\r", messageLotes, responsePegaso.MensajeRespuesta);
                        WriteBitacora(string.Format("{0}Error al intentar facturar el lote: {1}\n\r", messageLotes, responsePegaso.MensajeRespuesta), false);
                    }
                }
                catch (Exception ex)
                {
                    messageLotes = string.Format("{0}El lote {1} en divisa {2} no se genero\n\r", messageLotes, i, moneda.Moneda);
                    log.escribir(ex, string.Format("Error al procesar el lote {0}", i));
                }

                inicio = cantidadXfactura * i;
            }

            if (percentAnterior < percentFinal)
            {
                if (OnShowPorcentProgress != null)
                    OnShowPorcentProgress(this, new ShowPorcentProgressEventArgs(percentAnterior, percentFinal, string.Format("Facturas Globales en la moneda \"{0}\" finalizadas ", moneda.Moneda)));
            }
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
            ENTXmlPegaso xmlPegaso = pegaso.EnviaTimbrado(xmlFacturaGloba.ToString(), long.Parse(xmlFacturaGloba.Element("Comprobante").Attribute("Folio").Value), "FG", "", false);
            return xmlPegaso;
        }

        /// <summary>
        /// Genera notas informativas para el correo de succes
        /// </summary>
        /// <param name="fechaPago"></param>
        /// <param name="msgLotes"></param>
        /// <param name="cantidadPagos"></param>
        /// <returns></returns>
        private string GenerarMensajeSucces(string msgLotes, int cantidadPagos)
        {
            string message = string.Empty, tipoCambio = string.Empty;

            if (this.OnDemand)
                message = string.Format("Se facturaron {2} pagos del dia {0:dd-MMM-yyyy} al {1:dd-MMM-yyyy} \n\r", this.FechaInicial, this.FechaFinal, cantidadPagos);
            else
                message = string.Format("Se facturaron {1} pagos del dia {0:dd-MMM-yyyy} \n\r", this.FechaInicial, cantidadPagos);

            message = string.Format("{0}{1}", message, msgLotes);

            return message;
        }

        /// <summary>
        /// Construye eñ xml
        /// </summary>
        /// <param name="tipoMoneda"></param>
        /// <param name="fechaPago"></param>
        /// <param name="pagos"></param>
        /// <returns></returns>
        private XElement BuildXML(string tipoMoneda, /*DateTime fechaPago,*/ List<PagoTraslados> pagos, List<Cargo> cargos)
        {
            PagoTraslados pagoMax = pagos[0];
            ENTEmpresaCat empresa = BLLFacturaGlobal.GetAllEmpresas().Find(f => f.Rfc == Parametros["EMISOR"]["RFC"]);

            XElement tagMain = new XElement("RequestCFD", new XAttribute("version", Parametros["COMPROBANTE"]["Version"]));

            decimal subtotal = 0;
            decimal total = 0;
            XElement tagConceptos = GetTagConceptos(pagos, ref subtotal, ref total);

            XElement tagComprobante = GetTagComprobante(empresa, pagoMax, pagos, tipoMoneda, subtotal, total);

            tagComprobante.Attribute("Version").Value = Parametros["COMPROBANTE"]["Version"];


            tagComprobante.Add(
                GetTagEmisor(empresa),
                GetTagReceptor(),
                tagConceptos,
                GetTagImpuestos(tagConceptos),
                GetTagComplemento(cargos)
                );

            tagMain.Add(
                tagComprobante,
                GetTagTrasaccion(tagComprobante.Attribute("Folio").Value),
                GetTagTipoComprobante(),
                GetTagSucursal()
                );



            return tagMain;

        }

        /// <summary>
        /// Cuadra cantidades y el que no cuadre lo filtra
        /// </summary>
        /// <returns>Todos los pagos cuadrados</returns>
        private List<PagoTraslados> VerificadorCuadranteCantidades(List<PagoTraslados> pagos)
        {
            Decimal IVA = Decimal.Parse(Parametros["TRASLADO"]["IVA"]);
            List<PagoTraslados> pagosCuadrados = new List<PagoTraslados>();
            string mensajote = string.Empty;

            foreach (PagoTraslados temp in pagos)
            {
                Decimal totalIVA = Decimal.Round(temp.Base * IVA, 2);

                if (totalIVA >= 0 && (temp.MontoTotal - temp.MontoIVA) >= 0)
                {
                    decimal diferencia = Math.Abs(totalIVA - temp.MontoIVA);

                    if (diferencia == 0)
                    {
                        Decimal total = temp.Base0 + temp.Base + temp.MontoIVA;

                        if (total == temp.MontoTotal)
                        {
                            pagosCuadrados.Add(temp);
                        }
                        else
                        {
                            mensajote = string.Format("{0}El Payment ID \"{1}\" no cuadro en la sumatoria  Base0+Base16+IVA=Total {2}+{3}+{4}={5}\r\n", mensajote, temp.PaymentId, temp.Base0, temp.Base, temp.MontoIVA, temp.MontoTotal);
                        }
                    }
                    else if (0.0M <= diferencia && diferencia <= 1.0M)
                    {
                        Decimal nuevaBase = Math.Round((temp.MontoIVA / IVA), 2);

                        mensajote = string.Format("{0}El pago con PaymentID \"{1}\" se actualiza la base de {2} a {3} para que cuadre conel IVA navitaire\r\n", mensajote, temp.PaymentId, temp.Base, nuevaBase);

                        temp.Base = nuevaBase;

                        pagosCuadrados.Add(temp);
                    }
                    else
                    {
                        mensajote = string.Format("{0}Se descartara el pago con PaymentID \"{1}\" tiene una diferencia de \"{2}\" entre \"MontoIVA\" y \"Base16*IVA\"\r\n", mensajote, temp.PaymentId, diferencia);
                    }
                }
                else
                {
                    mensajote = string.Format("{0}Se descartara el pago con PaymentID \"{1}\" tiene un Base16/IVA negativo de {2}\r\n", mensajote, temp.PaymentId, totalIVA);
                }
            }

            WriteBitacora(mensajote, true);

            return pagosCuadrados;
        }

        private void SetFechas()
        {


            if (!OnDemand)
            {
                this.FechaInicial = DateTime.Today.AddDays(-1);
                this.FechaFinal = this.FechaInicial.AddDays(1);
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

        #region Construir Tags XML

        private XElement GetTagReceptor()
        {
            Receptor tagReceptor = new Receptor();

            tagReceptor.Rfc = Parametros["RECEPTOR"]["RFC"];
            tagReceptor.UsoCFDI = Parametros["RECEPTOR"]["UsoCFDI"];
            tagReceptor.emailReceptor = string.Empty;
            tagReceptor.codigoReceptor = string.Empty;

            return tagReceptor.GetXml();
        }

        private XElement GetTagEmisor(ENTEmpresaCat empresa)
        {
            Emisor tagEmisor = new Emisor();

            tagEmisor.Nombre = empresa.RazonSocial;
            tagEmisor.RegimenFiscal = empresa.IdRegimenFiscal;
            tagEmisor.Rfc = empresa.Rfc;

            return tagEmisor.GetXml();
        }

        private XElement GetTagComprobante(ENTEmpresaCat empresa, PagoTraslados pagoMax, List<PagoTraslados> pagos, string moneda)
        {
            Comprobante tagComprobante = new Comprobante();

            DateTime fechaGlobal = new DateTime(pagoMax.FechaPago.Year, pagoMax.FechaPago.Month, pagoMax.FechaPago.Day).AddDays(1).AddSeconds(-1);

            tagComprobante.Version = Parametros["COMPROBANTE"]["Version"];
            tagComprobante.Serie = Parametros["COMPROBANTE"]["Serie"];
            tagComprobante.Folio = BLLFacturaGlobal.GetFolio();                   ////Validar que la fecha del pago aun esta dentro de las 72 hrs de limite
            tagComprobante.Fecha = string.Format("{0:yyyy-MM-dd}T{0:HH:mm:ss}", (fechaGlobal.AddHours(72) > DateTime.Now) ? fechaGlobal : DateTime.Now);
            tagComprobante.FormaPago = pagoMax.IdFormaPago;
            tagComprobante.NoCertificado = empresa.NoCertificado;
            tagComprobante.Total = pagos.Sum(s => Math.Round(s.MontoTotal, 2));
            tagComprobante.SubTotal = tagComprobante.Total - pagos.Sum(s => Math.Round(s.MontoIVA, 2));
            tagComprobante.Moneda = moneda;
            tagComprobante.TipoDeComprobante = Parametros["COMPROBANTE"]["TipoComprobante"];
            tagComprobante.MetodoPago = Parametros["COMPROBANTE"]["MetodoPago"];
            tagComprobante.LugarExpedicion = Parametros["COMPROBANTE"]["LugarExpedicion"];
            tagComprobante.permitirConfirmacion = /*"false"*/string.Empty;
            tagComprobante.TipoCambio = (moneda == "MXN") ? (1) : (BLLFacturaGlobal.GetDolarEnPesos(this.FechaInicial));

            return tagComprobante.GetXml();
        }



        private XElement GetTagComprobante(ENTEmpresaCat empresa, PagoTraslados pagoMax, List<PagoTraslados> pagos, string moneda, decimal subtotal, decimal total)
        {
            Comprobante tagComprobante = new Comprobante();

            DateTime fechaGlobal = new DateTime(pagoMax.FechaPago.Year, pagoMax.FechaPago.Month, pagoMax.FechaPago.Day).AddDays(1).AddSeconds(-1);

            tagComprobante.Version = Parametros["COMPROBANTE"]["Version"];
            tagComprobante.Serie = Parametros["COMPROBANTE"]["Serie"];
            tagComprobante.Folio = BLLFacturaGlobal.GetFolio();                   ////Validar que la fecha del pago aun esta dentro de las 72 hrs de limite
            tagComprobante.Fecha = string.Format("{0:yyyy-MM-dd}T{0:HH:mm:ss}", (fechaGlobal.AddHours(72) > DateTime.Now) ? fechaGlobal : DateTime.Now);
            tagComprobante.FormaPago = pagoMax.IdFormaPago;
            tagComprobante.NoCertificado = empresa.NoCertificado;
            tagComprobante.Total = total;//pagos.Sum(s => Math.Round(s.MontoTotal, 2));
            tagComprobante.SubTotal = subtotal;// tagComprobante.Total - pagos.Sum(s => Math.Round(s.MontoIVA, 2));
            tagComprobante.Moneda = moneda;
            tagComprobante.TipoDeComprobante = Parametros["COMPROBANTE"]["TipoComprobante"];
            tagComprobante.MetodoPago = Parametros["COMPROBANTE"]["MetodoPago"];
            tagComprobante.LugarExpedicion = Parametros["COMPROBANTE"]["LugarExpedicion"];
            tagComprobante.permitirConfirmacion = /*"false"*/string.Empty;
            tagComprobante.TipoCambio = (moneda == "MXN") ? (1) : (BLLFacturaGlobal.GetDolarEnPesos(this.FechaInicial));

            //decimal.Parse((moneda == "MXN") ? ("1") : ("19"));

            return tagComprobante.GetXml();
        }


        private XElement GetTagConceptos(List<PagoTraslados> pagos, decimal subTotal)
        {
            XElement tagConceptos = new XElement("Conceptos");

            List<Concepto> conceptos = new List<Concepto>();

            foreach (PagoTraslados pago in pagos)
            {
                Concepto tagConcepto = new Concepto();

                tagConcepto.ClaveProdServ = Parametros["CONCEPTO"]["ClaveProdServ"];
                tagConcepto.NoIdentificacion = pago.PaymentId.ToString();
                tagConcepto.Cantidad = int.Parse(Parametros["CONCEPTO"]["Cantidad"]);
                tagConcepto.ClaveUnidad = Parametros["CONCEPTO"]["ClaveUnidad"];
                tagConcepto.Descripcion = Parametros["CONCEPTO"]["Descripcion"];
                tagConcepto.ValorUnitario = pago.MontoTotal - pago.MontoIVA;//tagComprobante.SubTotal;
                tagConcepto.Importe = (tagConcepto.ValorUnitario * tagConcepto.Cantidad);

                XElement tagImpuesto = new XElement("Impuestos");
                tagImpuesto.Add(GetTagTraslados(pago));

                XElement xmlConcepto = tagConcepto.GetXml();
                xmlConcepto.Add(tagImpuesto);

                tagConceptos.Add(xmlConcepto);
            }

            return tagConceptos;
        }

        private XElement GetTagConceptos(List<PagoTraslados> pagos, ref decimal subTotal, ref decimal total)
        {
            subTotal = 0;
            total = 0;
            XElement tagConceptos = new XElement("Conceptos");

            List<Concepto> conceptos = new List<Concepto>();

            foreach (PagoTraslados pago in pagos)
            {
                if ((Math.Round(pago.MontoTotal, 2) - Math.Round(pago.MontoIVA, 2)) > 0)
                {
                    Concepto tagConcepto = new Concepto();

                    tagConcepto.ClaveProdServ = Parametros["CONCEPTO"]["ClaveProdServ"];
                    tagConcepto.NoIdentificacion = pago.PaymentId.ToString();
                    tagConcepto.Cantidad = int.Parse(Parametros["CONCEPTO"]["Cantidad"]);
                    tagConcepto.ClaveUnidad = Parametros["CONCEPTO"]["ClaveUnidad"];
                    tagConcepto.Descripcion = Parametros["CONCEPTO"]["Descripcion"];
                    tagConcepto.ValorUnitario = Math.Round(pago.MontoTotal, 2) - Math.Round(pago.MontoIVA, 2);//tagComprobante.SubTotal;
                    tagConcepto.Importe = (tagConcepto.ValorUnitario * tagConcepto.Cantidad);

                    subTotal += tagConcepto.Importe;
                    total += Math.Round(pago.MontoTotal, 2);


                    XElement tagImpuesto = new XElement("Impuestos");
                    tagImpuesto.Add(GetTagTraslados(pago));

                    XElement xmlConcepto = tagConcepto.GetXml();
                    xmlConcepto.Add(tagImpuesto);

                    tagConceptos.Add(xmlConcepto);
                }
            }

            return tagConceptos;
        }


        private XElement GetTagTraslados(PagoTraslados pago)
        {
            Traslado trasladoBase16 = new Traslado();
            Traslado trasladoBase0 = new Traslado();

            trasladoBase16.Base = Math.Round(pago.Base, 2).ToString(); //infoTraslado.Find(f => f.Tipo == "BASE" && f.PorcIva == 16).Total.ToString();
            trasladoBase16.Impuesto = Parametros["TRASLADO"]["Impuesto"];
            trasladoBase16.TipoFactor = Parametros["TRASLADO"]["TipoFactor"];
            trasladoBase16.TasaOCuota = Decimal.Parse(Parametros["TRASLADO"]["IVA"]);
            trasladoBase16.Importe = Math.Round(pago.MontoIVA, 2);//infoTraslado.Find(f => f.Tipo.ToUpper() == "IVA" && f.PorcIva == 16).Total;

            trasladoBase0.Base = Math.Round(pago.Base0, 2).ToString();//infoTraslado.Find(f => f.Tipo == "BASE" && f.PorcIva == 0).Total.ToString();
            trasladoBase0.Impuesto = Parametros["TRASLADO"]["Impuesto"];
            trasladoBase0.TipoFactor = Parametros["TRASLADO"]["TipoFactor"];
            trasladoBase0.TasaOCuota = 0.0M;
            trasladoBase0.Importe = 0;

            XElement tagTraslados = new XElement("Traslados");

            if (Math.Round(decimal.Parse(trasladoBase0.Base), 2) > 0)
                tagTraslados.Add(trasladoBase0.GetXml());

            if (Math.Round(decimal.Parse(trasladoBase16.Base), 2) > 0)
                tagTraslados.Add(trasladoBase16.GetXml());

            return tagTraslados;
        }


        private XElement GetTagTrasaccion(string folio)
        {
            string id = string.Empty;

            id = string.Format("ANA4000902{0:yyMMdd}GLOBAL-{1}", DateTime.Now, folio.PadLeft(8, '0'));

            XElement transaccionTag = new XElement("Transaccion");
            transaccionTag.Add(new XAttribute("id", id));

            return transaccionTag;
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

            var traslado0 = trasladosElementsList.FindAll(f => float.Parse(f.Attribute("TasaOCuota").Value) == 0.00f).ToList();
            decimal base0 = traslado0.Sum(s => decimal.Parse(s.Attribute("Base").Value));

            decimal importe16 = traslado16.Sum(s => decimal.Parse(s.Attribute("Importe").Value));
            decimal base16 = traslado16.Sum(s => decimal.Parse(s.Attribute("Base").Value));

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
            impuestosTag.Add(new XAttribute("TotalImpuestosTrasladados", importe16.ToString("0.00")));

            return impuestosTag;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.DAL.ProcesoFacturacion;
using Facturacion.ENT;
using System.Data.SqlClient;
using System.Data;
using Facturacion.ENT.ProcesoFacturacion;
using VBFactPaquetes.Model;
using Newtonsoft.Json;

namespace Facturacion.BLL.ProcesoFacturacion
{
    public class BLLFacturaGlobal33 : DALFacturaGlobal33
    {

        #region Variables Globales
        private BLLBitacoraErrores BllLogErrores { get; set; }
        public List<ENTParametrosCnf> ListaParametros = new List<ENTParametrosCnf>();
        public List<string> ListaCodigosMonedaGlobal = new List<string>();
        public List<ENTEmpresaCat> ListaCatalogoEmpresas = new List<ENTEmpresaCat>();
        private List<string> ListaLocationFronterizo = new List<string>();
        private List<ENTLocationCat> ListaLocationCode = new List<ENTLocationCat>();
        public List<ENT.ENTGendescripcionesCat> ListaGenDescripciones = new List<ENT.ENTGendescripcionesCat>();
        public List<ENTFormapagoCat> ListaCatalogoFormasPago = new List<ENTFormapagoCat>();
        public List<string> ListaCodigosTUA = new List<string>();
        public List<ENTIvaporcpCat> ListaCodigosPostales = new List<ENTIvaporcpCat>();
        public string PNR { get; set; }
        public ENTEmpresaCat EmpresaEmisora { get; set; }
        //private bool ActivarProcesoGlobal33 { get; set; }
        private int NumLimitePagosFG { get; set; }
        public string MensajeErrorUsuario { get; set; }
        public string RFCReceptorFG { get; set; }
        #endregion



        #region Constructor
        public BLLFacturaGlobal33()
        : base(BLLConfiguracion.Conexion)
        {
            BllLogErrores = new BLLBitacoraErrores();
            InicializaVariablesGlobales();
        }

        private void InicializaVariablesGlobales()
        {
            try
            {

                //Iniciar Catalogo Formas de Pago
                if (ListaCatalogoFormasPago.Count() == 0)
                {
                    BLLFormapagoCat bllFormaPago = new BLLFormapagoCat();
                    ListaCatalogoFormasPago = bllFormaPago.RecuperarTodo();
                }

                if (ListaCatalogoEmpresas.Count() == 0)
                {
                    BLLEmpresaCat bllCatEmpresa = new BLLEmpresaCat();
                    ListaCatalogoEmpresas = bllCatEmpresa.RecuperarTodo();
                }

                if (ListaGenDescripciones.Count() == 0)
                {
                    BLLGendescripcionesCat bllDescripciones = new BLLGendescripcionesCat();
                    ListaGenDescripciones = bllDescripciones.RecuperarTodo();
                }

                if (ListaLocationCode.Count() == 0)
                {
                    BLLLocationCat bllLocat = new BLLLocationCat();
                    ListaLocationCode = bllLocat.RecuperarTodo();
                }

                if (ListaParametros.Count() == 0)
                {
                    BLLParametrosCnf bllParam = new BLL.BLLParametrosCnf();
                    ListaParametros = RecuperarParametros();

                }

                string paramCodigosMoneda = "";
                if (ListaParametros.Where(x => x.Nombre == "MonedasValidas").Count() > 0)
                {
                    paramCodigosMoneda = ListaParametros.Where(x => x.Nombre == "MonedasValidas").FirstOrDefault().Valor;
                }
                else
                {
                    paramCodigosMoneda = "MXN,USD";
                }

                if (paramCodigosMoneda.Length > 0)
                {
                    string[] listCodMoneda = paramCodigosMoneda.Split(',');
                    foreach (string codMoneda in listCodMoneda)
                    {
                        ListaCodigosMonedaGlobal.Add(codMoneda);
                    }

                }

                if (ListaParametros.Where(x => x.Nombre == "NumLimitePagosFG").Count() > 0)
                {
                    NumLimitePagosFG = Convert.ToInt16((ListaParametros.Where(x => x.Nombre == "NumLimitePagosFG").FirstOrDefault().Valor));
                }
                else
                {
                    NumLimitePagosFG = 5000;
                }

                //RFC Receptor de la factura Global
                if (ListaParametros.Where(x => x.Nombre == "RFCReceptorFG").Count() > 0)
                {
                    RFCReceptorFG = (ListaParametros.Where(x => x.Nombre == "RFCReceptorFG").FirstOrDefault().Valor.ToString());
                }
                else
                {
                    RFCReceptorFG = "XAXX010101000";
                }
                string paramCodigosTUA = ListaParametros.Where(x => x.Nombre == "CodigosTUA").FirstOrDefault().Valor;

                if (paramCodigosTUA.Length > 0)
                {
                    string[] listCodTua = paramCodigosTUA.Split(',');
                    foreach (string codTUA in listCodTua)
                    {
                        ListaCodigosTUA.Add(codTUA);
                    }
                }


                byte idEmpresa = Convert.ToByte(ListaParametros.Where(x => x.Nombre == "IdEmpresa").FirstOrDefault().Valor); // IdEmpresa
                EmpresaEmisora = ListaCatalogoEmpresas.Where(x => x.IdEmpresa == idEmpresa).FirstOrDefault();


                if (ListaCodigosPostales.Count() == 0)
                {
                    BLLIvaporcpCat bllCodPostal = new BLLIvaporcpCat();
                    ListaCodigosPostales = bllCodPostal.RecuperarTodo();
                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "FacturacionGlobal", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "FacturacionGlobal", "InicializaVariablesGlobales");
                throw new ExceptionViva(mensajeUsuario);

            }
        }
        #endregion


        /// <summary>
        /// Metodo principal que se invoca desde la clase de Factura Global
        /// </summary>
        /// <param name="fechaInicial">Fecha inicial que se envio a facturar</param>
        /// <param name="fechaFinal">Fecha Final del Pago que se envio a facturar</param>
        /// <param name="totalPagosFacturados">Indica la cantidad de pagos facturados</param>
        /// <param name="messageLotes">Mensaje que indica el resultado de la facturación</param>
        public List<ENTResultadoFacturaGlobal33> GeneracionFacturasGlobales(DateTime fechaInicial, DateTime fechaFinal)
        {
            //Se inicializa la lista de Resultados de factura Global
            List<ENTResultadoFacturaGlobal33> listaResultadoFG = new List<ENTResultadoFacturaGlobal33>();
            List<ENTResultadoFacturaGlobal33> listaResultadoFG_Paquetes = new List<ENTResultadoFacturaGlobal33>();
            try
            {
                //Se recupera la informacion de los pagos a facturar en la global
                DataSet dsPagosGlobal = new DataSet();
                dsPagosGlobal = RecuperarPagosFacturaGlobal(fechaInicial, fechaFinal, false);
                listaResultadoFG = Get(dsPagosGlobal);

                DataSet dsPagosPaquetesGlobal = new DataSet();
                dsPagosPaquetesGlobal = RecuperarPagosFacturaGlobal(fechaInicial, fechaFinal, true);
                listaResultadoFG_Paquetes = Get(dsPagosPaquetesGlobal);

                foreach(ENTResultadoFacturaGlobal33 entR in listaResultadoFG_Paquetes)
                {
                    listaResultadoFG.Add(entR);
                }

                //Se contabilizan los pagos que fueron enviados en Factura Global
                Comun.Email.EnviarEmail enviarEmail = new Comun.Email.EnviarEmail(ListaParametros, EmpresaEmisora);
                BLLParametrosCnf bllParam = new BLLParametrosCnf();
                ENTParametrosCnf emails = bllParam.RecuperarParametrosCnfNombre("EmailFinanzasFacGlobal").FirstOrDefault();

                enviarEmail.sendEmailFacturaGlobal(EmpresaEmisora, listaResultadoFG, fechaInicial, fechaFinal, emails.Valor);
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "FacturacionGlobal", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "FacturacionGlobal", "InicializaVariablesGlobales");
                throw new ExceptionViva(mensajeUsuario);
            }
            return listaResultadoFG;
        }

        public List<ENTResultadoFacturaGlobal33> Get(DataSet dsPagosGlobal)
        {
            List<ENTEnvioFacturaGlobal> listaResult = new List<ENTEnvioFacturaGlobal>();
            List<ENTResultadoFacturaGlobal33> listaResultadoFG = new List<ENTResultadoFacturaGlobal33>();

            //Se renombran las tablas para facilitar la identificacion de los datos
            dsPagosGlobal.Tables[0].TableName = "dtPagosOmitidosDeGlobal";
            dsPagosGlobal.Tables[1].TableName = "dtMoneda_CP";
            dsPagosGlobal.Tables[2].TableName = "dtPagos";
            dsPagosGlobal.Tables[3].TableName = "dtMontosPorTipoAcumulado";
            dsPagosGlobal.Tables[4].TableName = "dtIvaTrasladado";
            dsPagosGlobal.Tables[5].TableName = "dtCargosPorFolioPrefactura";
            dsPagosGlobal.Tables[6].TableName = "dtListaPaymentId";

            //Se recorre la informacion de las facturas que se deben generar clasificadas por Moneda, Codigo Postal, Fecha Pagos
            foreach (DataRow drMonedaCP in dsPagosGlobal.Tables["dtMoneda_CP"].Rows)
            {
                string moneda = "";
                DateTime fechaPagosFG = new DateTime();
                bool esFrontera = false;
                string codigoPostalFG = "";
                int numPagosInicial = 0;
                int numPagosEnviados = 0;
                int numPagosOmitidos = 0;
                int numFacturasGeneradas = 0;
                string resultadoEnvio = "";
                string recordLocator = string.Empty;
                List<ENTGlobalpagosnoenviadosReg> listaPagosNoEnviados = new List<ENTGlobalpagosnoenviadosReg>();


                try
                {
                    //Obtiene la moneda que se va a procesar
                    fechaPagosFG = (DateTime)drMonedaCP["FechaPagoFG"];
                    moneda = drMonedaCP["CurrencyCode"].ToString();
                    codigoPostalFG = drMonedaCP["LugarExpPublicoGeneral"].ToString();
                    ENTIvaporcpCat ivaPorCP = ListaCodigosPostales.Where(x => x.CodigoPostal == codigoPostalFG).FirstOrDefault();
                    if (ivaPorCP != null)
                    {
                        esFrontera = ivaPorCP.EsFrontera;
                    }

                    //Se verifica si el codigo de la moneda esta considerada para enviar en la factura global
                    if (ListaCodigosMonedaGlobal.Contains(moneda) == true)
                    {
                        List<ENTIvaporcpCat> listaCodigosValidos = new List<ENTIvaporcpCat>();
                        listaCodigosValidos = ListaCodigosPostales.Where(x => x.CodigoPostal == codigoPostalFG).ToList();

                        numPagosInicial = (int)drMonedaCP["TotRegistros"];


                        List<long> listaPagosFG = new List<long>();
                        List<string> RecordsLocator = new List<string>();
                        string filtro = "";
                        filtro = "FechaPago >= '" + fechaPagosFG.ToString("yyyy-MM-dd") + " 00:00:00.000" + "' AND FechaPago < '" + fechaPagosFG.AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00.000" + "' AND  CurrencyCode = '" + moneda + "' AND LugarExpPublicoGeneral = '" + codigoPostalFG + "'";
                        foreach (DataRow drPago in dsPagosGlobal.Tables["dtPagos"].Select(filtro))
                        {
                            recordLocator = drPago["RecordLocator"].ToString();
                            RecordsLocator.Add(recordLocator);
                            //Valida la informacion del pago para saber si puede ser procesado
                            long paymentId = 0;
                            paymentId = Convert.ToInt64(drPago["PaymentID"].ToString());
                            string mensajeValidacion = "";

                            mensajeValidacion = ValidaPagoParaGlobal(drPago, dsPagosGlobal, listaCodigosValidos, codigoPostalFG);
                            if (mensajeValidacion == "OK")
                            {
                                //Si el pago es valido entonces se agrega a la lista que se enviara a facturar.
                                listaPagosFG.Add(paymentId);

                            }
                            else
                            {
                                numPagosOmitidos++;

                                //Agregar el motivo de la omision de este pago en la Factura Global
                                ENTGlobalpagosnoenviadosReg pagoOmitido = new ENTGlobalpagosnoenviadosReg();

                                long bookingPag = 0;
                                DateTime fechaPago = new DateTime();

                                fechaPago = (DateTime)drPago["FechaPago"];
                                recordLocator = drPago["RecordLocator"].ToString();
                                bookingPag = (long)drPago["BookingID"];

                                pagoOmitido.BookingID = bookingPag;
                                pagoOmitido.PaymentID = paymentId;
                                pagoOmitido.RecordLocator = recordLocator;
                                pagoOmitido.FechaPago = fechaPago;
                                pagoOmitido.CodigoMoneda = moneda;
                                pagoOmitido.LugarExpedicion = codigoPostalFG;
                                pagoOmitido.FechaEnvioFG = DateTime.Now;
                                pagoOmitido.MotivoOmision = mensajeValidacion;
                                pagoOmitido.FechaHoraLocal = DateTime.Now;

                                listaPagosNoEnviados.Add(pagoOmitido);

                            }
                        }


                        //Se envia a generar la factura Global
                        if (listaPagosFG.Count > 0)
                        {

                            listaResult = GenerarFacturasPorBloque(moneda, fechaPagosFG, codigoPostalFG, numPagosInicial, listaPagosFG, dsPagosGlobal, RecordsLocator);
                            //listaResult.AddRange(listaResult);
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("El código de moneda {0} no se encuentra registrado para envio a Factura Global", moneda));
                    }

                }
                catch (Exception ex)
                {
                    string mensaje = ex.Message;


                }
                finally
                {
                    numPagosEnviados = listaResult.Where(x => x.TimbradoExitoso == true).Sum(x => x.NumPagosEnviados);
                    numPagosOmitidos += listaResult.Where(x => x.TimbradoExitoso == false).Sum(x => x.NumPagosEnviados);
                    numFacturasGeneradas = listaResult.Count();
                    ENTResultadoFacturaGlobal33 resultFactOmi = new ENTResultadoFacturaGlobal33();
                    resultFactOmi.FechaPagos = fechaPagosFG;
                    resultFactOmi.Moneda = moneda;
                    resultFactOmi.CodigoPostal = codigoPostalFG;
                    resultFactOmi.EsFrontera = esFrontera;
                    resultFactOmi.NumPagosInicial = numPagosInicial;
                    resultFactOmi.NumPagosEnviadosFG = numPagosEnviados;
                    resultFactOmi.NumPagosOmitidos = numPagosOmitidos;
                    resultFactOmi.NumFacturasGeneradas = numFacturasGeneradas;
                    resultFactOmi.Resultado = resultadoEnvio;
                    resultFactOmi.ListaEnviosFG = listaResult;
                    resultFactOmi.ListaPagosNoEnviados = listaPagosNoEnviados;

                    GuardarPagoNoEnviado(listaPagosNoEnviados);

                    listaResultadoFG.Add(resultFactOmi);
                }
            }
            return listaResultadoFG;
        }

        private string InsertaSangria(int numSangrias)
        {
            string result = "";
            StringBuilder sangria = new StringBuilder();
            for (int i = 0; i < numSangrias; i++)
            {
                sangria.Append("   ");
            }
            result = sangria.ToString();
            return result;
        }


        private void GuardarPagoNoEnviado(List<ENTGlobalpagosnoenviadosReg> listaPagosNoEnviados)
        {

            foreach (ENTGlobalpagosnoenviadosReg pagoOmitido in listaPagosNoEnviados)
            {
                try
                {

                    BLLGlobalpagosnoenviadosReg bllpagoOmitido = new BLLGlobalpagosnoenviadosReg();

                    bllpagoOmitido.BookingID = pagoOmitido.BookingID;
                    bllpagoOmitido.PaymentID = pagoOmitido.PaymentID;
                    bllpagoOmitido.RecordLocator = pagoOmitido.RecordLocator;
                    bllpagoOmitido.FechaPago = pagoOmitido.FechaPago;
                    bllpagoOmitido.CodigoMoneda = pagoOmitido.CodigoMoneda;
                    bllpagoOmitido.LugarExpedicion = pagoOmitido.LugarExpedicion;
                    bllpagoOmitido.FechaEnvioFG = pagoOmitido.FechaEnvioFG;
                    bllpagoOmitido.MotivoOmision = pagoOmitido.MotivoOmision;
                    bllpagoOmitido.FechaHoraLocal = pagoOmitido.FechaHoraLocal;
                    bllpagoOmitido.Agregar();
                }
                catch (ExceptionViva ex)
                {
                    throw ex;
                }
                catch (SqlException ex)
                {
                    string mensajeUsuario = MensajeErrorUsuario;
                    BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "GuardarPago", "BD");
                    throw new ExceptionViva(mensajeUsuario);
                }
                catch (Exception ex)
                {
                    string mensajeUsuario = MensajeErrorUsuario;
                    BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "FacturacionGlobal", "InicializaVariablesGlobales");
                    throw new ExceptionViva(mensajeUsuario);

                }

            }

        }

        private string ValidaPagoParaGlobal(DataRow drPago, DataSet dsPagosGlobal, List<ENTIvaporcpCat> listaCodigosValidos, string lugarExp)
        {
            string result = string.Empty;
            List<string> listaErrores = new List<string>();
            //Aplicar todas las reglas y validaciones necesarias para decidir que el pago es valido para enviarse a la global

            try
            {
                long paymentId = 0;
                paymentId = Convert.ToInt64(drPago["PaymentID"].ToString());

                decimal montoTarifaPag = 0;
                decimal montoServAdicPag = 0;
                decimal montoTUAPag = 0;
                decimal montoOtrosCargosPag = 0;
                decimal montoIVAPag = 0;
                decimal montoSubTotalPag = 0;
                decimal montoTotalPag = 0;
                string recordLocator = "";


                montoTarifaPag = Convert.ToDecimal(drPago["MontoTarifa"].ToString());
                montoServAdicPag = Convert.ToDecimal(drPago["MontoServAdic"].ToString());
                montoTUAPag = Convert.ToDecimal(drPago["MontoTUA"].ToString());
                montoOtrosCargosPag = Convert.ToDecimal(drPago["MontoOtrosCargos"].ToString());
                montoIVAPag = Convert.ToDecimal(drPago["MontoIVA"].ToString());
                montoSubTotalPag = Convert.ToDecimal(drPago["SubTotal"].ToString());
                montoTotalPag = Convert.ToDecimal(drPago["MontoTotal"].ToString());
                recordLocator = drPago["RecordLocator"].ToString();

                //Valida que el monto Subtotal sea correcto
                decimal subtotalCalc = 0;
                subtotalCalc = montoTarifaPag + montoServAdicPag + montoTUAPag + montoOtrosCargosPag;
                if (subtotalCalc != montoSubTotalPag)
                {
                    listaErrores.Add(string.Format("Diferencia Subtotal: BD {0}, App {1}", montoSubTotalPag.ToString(), subtotalCalc.ToString()));
                }

                //Valida que al sumar todos los conceptos sea igual al MontoTotal 
                decimal totalCalc = 0;
                totalCalc = montoTarifaPag + montoServAdicPag + montoTUAPag + montoOtrosCargosPag + montoIVAPag;
                if (totalCalc != montoTotalPag)
                {
                    listaErrores.Add(string.Format("Diferencia Total: BD {0}, App {1}", montoTotalPag, totalCalc));
                }


                decimal acumTAR = 0;
                decimal acumIMP = 0;
                decimal acumTUA = 0;
                decimal acumSVA = 0;
                decimal acumIVA = 0;
                Dictionary<int, decimal> listaIVAAcum = new Dictionary<int, decimal>();

                //Verifica que la sumatoria de los conceptos sea igual al acumulado
                foreach (DataRow drMontosPorTipo in dsPagosGlobal.Tables["dtMontosPorTipoAcumulado"].Select("FolioPrefactura = " + paymentId.ToString()))
                {
                    string tipoAcum = "";
                    int porcIVA = 0;
                    decimal montoCargo = 0;
                    decimal montoIVA = 0;

                    tipoAcum = drMontosPorTipo["TipoAcumulado"].ToString();
                    porcIVA = Convert.ToInt16(drMontosPorTipo["PorcIva"].ToString());
                    montoCargo = Convert.ToDecimal(drMontosPorTipo["MontoCargo"].ToString());
                    montoIVA = Convert.ToDecimal(drMontosPorTipo["MontoIVA"].ToString());

                    switch (tipoAcum)
                    {
                        case "TAR":
                            acumTAR += montoCargo;
                            break;
                        case "IMP":
                            acumIMP += montoCargo;
                            break;
                        case "TUA":
                            acumTUA += montoCargo;
                            break;
                        case "SVA":
                            acumSVA += montoCargo;
                            break;
                    }

                    acumIVA += montoIVA;
                    if (listaIVAAcum.ContainsKey(porcIVA))
                    {
                        listaIVAAcum[porcIVA] = listaIVAAcum[porcIVA] + montoIVA;
                    }
                    else
                    {
                        listaIVAAcum.Add(porcIVA, montoIVA);
                    }



                }

                //Verifica si los porcentajes de IVA asignados en la reserva son validos de acuerdo a su codigo postal
                //En caso de no ser validos se omitira el envio
                foreach (int porcIVAVal in listaIVAAcum.Keys)
                {
                    if (listaCodigosValidos.Where(x => x.PorcIVA == porcIVAVal).Count() == 0)
                    {
                        listaErrores.Add(string.Format("La tasa de IVA {0}% no es valida para el Lugar de Expedicion del Pago CP:{1} Codigo Postal ", porcIVAVal.ToString(), lugarExp));
                        break;
                    }
                }



                //Compara los montos registrados en el Pago y los acumulados calculados
                if (montoTarifaPag != acumTAR)
                {
                    listaErrores.Add(string.Format("Diferencia Monto Tarifa: BD {0}, App {1}", montoTarifaPag.ToString(), acumTAR.ToString()));
                }

                if (montoServAdicPag != acumSVA)
                {
                    listaErrores.Add(string.Format("Diferencia Monto Servicios Adic. : BD {0}, App {1}", montoServAdicPag.ToString(), acumSVA.ToString()));
                }

                if (montoTUAPag != acumTUA)
                {
                    listaErrores.Add(string.Format("Diferencia Monto TUA: BD {0}, App {1}", montoTUAPag.ToString(), acumTUA.ToString()));
                }

                if (montoOtrosCargosPag != acumIMP)
                {
                    listaErrores.Add(string.Format("Diferencia Monto Cargos Aero.: BD {0}, App {1}", montoOtrosCargosPag.ToString(), acumIMP.ToString()));
                }

                if (montoIVAPag != acumIVA)
                {
                    listaErrores.Add(string.Format("Diferencia Monto IVA: BD {0}, App {1}", montoIVAPag.ToString(), acumIVA.ToString()));
                }


                decimal montoIVATrasladado = 0;

                Dictionary<int, decimal> listaIvaTras = new Dictionary<int, decimal>();
                foreach (DataRow drMontosPorTipo in dsPagosGlobal.Tables["dtIvaTrasladado"].Select("FolioPrefactura = " + paymentId.ToString()))
                {
                    int porcIVA = 0;
                    decimal montoBase = 0;
                    decimal montoIVA = 0;

                    porcIVA = Convert.ToInt16(drMontosPorTipo["PorcIva"].ToString());
                    montoBase = Convert.ToDecimal(drMontosPorTipo["Base"].ToString());
                    montoIVA = Convert.ToDecimal(drMontosPorTipo["MontoIVA"].ToString());

                    montoIVATrasladado += montoIVA;

                    if (listaIvaTras.ContainsKey(porcIVA))
                    {
                        listaIvaTras[porcIVA] += montoIVA;
                    }
                    else
                    {
                        listaIvaTras.Add(porcIVA, montoIVA);
                    }


                    if (montoBase < montoIVA)
                    {
                        listaErrores.Add(string.Format("El monto de la Base no puede ser menor al IVA calculado. PorcIVA {0}, Base {1} < IVA{2}", porcIVA.ToString(), montoBase.ToString(), montoIVA.ToString()));
                    }

                    if (montoBase < 0)
                    {
                        listaErrores.Add(string.Format("El importe de Monto Base no puede ser menor a Cero, Base {0}", montoBase.ToString()));
                    }

                    if (porcIVA == 0 && montoIVA > 0)
                    {
                        listaErrores.Add(string.Format("El importe del IVA no puede ser mayor a cero cuando la tasa es del 0%(cero), Monto IVA: {0}", montoIVA.ToString()));
                    }

                }

                //Comparativo entre tablas deIVA trasladado e IVAS por tipoAcumulado
                if (montoIVATrasladado != acumIVA)
                {
                    listaErrores.Add(string.Format("El monto en IVA Trasladado {0} es diferente al IVA acumulado por bloque {1}", montoIVATrasladado.ToString(), acumIVA.ToString()));
                }


                //Se compara IVA generado por Porc de IVA
                foreach (int porcIVA in listaIvaTras.Keys)
                {
                    if (listaIVAAcum.ContainsKey(porcIVA))
                    {
                        if (listaIVAAcum[porcIVA] != listaIvaTras[porcIVA])
                        {
                            listaErrores.Add(string.Format("Diferencia en el IVA, tasa {0}, Sum por Bloque: {1}, Sum Trasladado {2}", listaIVAAcum[porcIVA].ToString(), listaIvaTras[porcIVA].ToString()));
                        }
                    }
                    else
                    {
                        listaErrores.Add(string.Format("La tasa de IVA {0}, existe en Trasladado pero no en Acumulados por Bloque", porcIVA.ToString()));
                    }
                }



                //Validando Cargos TUA y Cargos aeroportuarios o IMP
                decimal acumTUAPorCod = 0;
                decimal acumIMPPorCod = 0;
                foreach (DataRow drCargo in dsPagosGlobal.Tables["dtCargosPorFolioPrefactura"].Select("FolioPrefactura = " + paymentId.ToString()))
                {
                    string tipoAcum = "";
                    decimal montoCargo = 0;

                    tipoAcum = drCargo["TipoAcumulado"].ToString();
                    montoCargo = Convert.ToDecimal(drCargo["MontoCargo"].ToString());

                    switch (tipoAcum)
                    {
                        case "TUA":
                            acumTUAPorCod += montoCargo;
                            break;
                        case "IMP":
                            acumIMPPorCod += montoCargo;
                            break;
                    }
                }

                if (acumTUAPorCod != acumTUA)
                {
                    listaErrores.Add(string.Format("Diferencia entre Acum Detalle Cargos TUA {0} y Sum TUA por Bloques {1}", acumTUAPorCod.ToString(), acumTUA.ToString()));
                }

                if (acumIMPPorCod != acumIMP)
                {
                    listaErrores.Add(string.Format("Diferencia entre Acum Detalle Cargos IMP {0} y Sum IMP por Bloques {1}", acumIMPPorCod.ToString(), acumIMP.ToString()));
                }
            }
            catch (Exception ex)
            {
                listaErrores.Add(ex.Message);
            }
            finally
            {
                if (listaErrores.Count() == 0)
                {
                    result = "OK";
                }
                else
                {
                    int contError = 0;
                    string sep = "";
                    foreach (string errorVal in listaErrores)
                    {
                        contError++;
                        result += sep + contError.ToString() + ". " + errorVal;
                        sep = ". ";
                    }
                }
            }


            return result;
        }

        private List<ENTEnvioFacturaGlobal> GenerarFacturasPorBloque(string moneda, DateTime fechaPagosFG, string codigoPostalFG, int numPagos, List<long> listaPagos, DataSet dsPagosGlobal, List<string> RecordsLocator)
        {
            List<ENTEnvioFacturaGlobal> listaResultado = new List<ENTEnvioFacturaGlobal>();

            try
            {
                int numPagosPorGlobal = NumLimitePagosFG;
                int folioFactura = 1;

                while (((folioFactura - 1) * numPagosPorGlobal) < listaPagos.Count)
                {

                    List<long> listaPagosFG = new List<long>();
                    listaPagosFG = listaPagos.Skip((folioFactura - 1) * numPagosPorGlobal).Take(numPagosPorGlobal).ToList();

                    //Se genera la factura Global
                    ENTEnvioFacturaGlobal resultadoFG = new ENTEnvioFacturaGlobal();

                    resultadoFG = GenerarFacturaGlobal33(moneda, fechaPagosFG, codigoPostalFG, numPagos, listaPagosFG, dsPagosGlobal, RecordsLocator);
                    listaResultado.Add(resultadoFG);

                    folioFactura++; //Se incrementa el numero de folio en la factura
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "FacturacionGlobal", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "FacturacionGlobal", "InicializaVariablesGlobales");
                throw new ExceptionViva(mensajeUsuario);

            }
            return listaResultado;

        }

        private ENTEnvioFacturaGlobal GenerarFacturaGlobal33(string moneda, DateTime fechaPagosFG, string codigoPostalFG, int numPagos, List<long> listaPagos, DataSet dsPagosGlobal, List<string> RecordsLocator)
        {
            ENTEnvioFacturaGlobal result = new ENTEnvioFacturaGlobal();
            string mensaje = "";
            ENTFacturaGlobal entFacturaGlobal = new ENTFacturaGlobal();
            bool flg_Terceros = false;

            try
            {
                //Se genera la facturaGlobal
                entFacturaGlobal = GenerarInformacionFacturaGlobal(moneda, fechaPagosFG, codigoPostalFG, numPagos, listaPagos, dsPagosGlobal, ref flg_Terceros, RecordsLocator);

                //Enviar a timbrar la factura global
                mensaje = EnviarATimbrarFacturaGlobal(ref entFacturaGlobal, flg_Terceros);

                //Guardar la factura y actualizar la informacion de los pagos timbrados en la Global
                //Se guarda la informacion de la factura
                if (mensaje == "OK")
                {
                    GuardarFacturaGlobal(ref entFacturaGlobal);
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "FacturacionGlobal", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "FacturacionGlobal", "InicializaVariablesGlobales");
                throw new ExceptionViva(mensajeUsuario);

            }
            finally
            {
                //Generar Resultado
                result.FolioFG = entFacturaGlobal.FolioFactura;
                result.LugarExp = codigoPostalFG;
                result.NumPagosEnviados = listaPagos.Count();
                result.IdPeticionPAC = entFacturaGlobal.IdPeticionPAC;
                result.FechaEmisionFG = entFacturaGlobal.FechaHoraExpedicion;
                result.UUID = entFacturaGlobal.UUID;
                result.MontoTotalFactura = entFacturaGlobal.Total;
                result.TimbradoExitoso = (mensaje == "OK");
                result.Mensaje = mensaje;
            }

            return result;
        }

        private string EnviarATimbrarFacturaGlobal(ref ENTFacturaGlobal entFacturaGlobal, bool flg_Terceros)
        {
            string result = "";

            try
            {
                string xmlRequest = "";
                string xmlResponse = "";


                xmlRequest = GenerarXMLRequestFG(entFacturaGlobal, flg_Terceros);

                //Se genera el Request para enviar a timbrar el comprobante
                ENTXmlPegaso resultTimbrado = new ENTXmlPegaso();

                //Se invoca el metodo principal de timbrado
                BLLPegaso bllTim = new BLLPegaso();

                resultTimbrado = bllTim.EnviaTimbrado(xmlRequest, entFacturaGlobal.FolioFactura, entFacturaGlobal.TipoFacturacion, "GLOBAL", false);

                xmlResponse = resultTimbrado.XMLResponse;


                if (resultTimbrado.EsCorrecto)
                {

                    //Se genera la informacion del CFDI
                    ENTFacturascfdiDet cfdiDet = new ENTFacturascfdiDet();
                    cfdiDet.IdFacturaCab = entFacturaGlobal.IdFacturaCab;
                    cfdiDet.TransaccionID = resultTimbrado.Transaccion_Id;
                    cfdiDet.CFDI = resultTimbrado.CFD_ComprobanteStr;
                    cfdiDet.CadenaOriginal = resultTimbrado.CFD_CadenaOriginal;
                    cfdiDet.FechaTimbrado = resultTimbrado.TFD_FechaTimbrado;
                    cfdiDet.FechaHoraLocal = DateTime.Now;

                    entFacturaGlobal.FacturasCFDIDet = cfdiDet;

                    entFacturaGlobal.EsFacturado = true;
                    entFacturaGlobal.IdPeticionPAC = resultTimbrado.IdPeticionPAC;
                    entFacturaGlobal.FechaHoraExpedicion = resultTimbrado.FechaTimbrado;
                    entFacturaGlobal.UUID = resultTimbrado.TFD_UUID;
                    entFacturaGlobal.Estatus = "FA";

                    //Se genera el archivo CFDI
                    BLLPDFCFDI bllPdf = new ProcesoFacturacion.BLLPDFCFDI();

                    string rutaArchivoCFDI = bllPdf.GeneraArchivoCFDI(resultTimbrado.CFD_ComprobanteStr, entFacturaGlobal.FolioFactura, "GLOBAL");

                    entFacturaGlobal.FacturasCFDIDet = cfdiDet;

                    result = "OK";
                }
                else
                {
                    result = "Por el momento no fue posible generar la factura, por favor intente más tarde...";
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "FacturacionGlobal", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "FacturacionGlobal", "InicializaVariablesGlobales");
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;

        }

        private bool GuardarFacturaGlobal(ref ENTFacturaGlobal entFacturaGlobal)
        {
            //Se guarda la informacion de la factura generada

            bool result = false;
            try
            {


                //Recuperar el folio Fiscal que se utilizara para enviar la factura global

                //1. [dbo].[VBFac_Facturas_Cab]
                //Se guarda el Cabecero
                BLLFacturasCab bllFacturaCab = new BLL.BLLFacturasCab();

                long idFacturaCabOri = entFacturaGlobal.IdFacturaCab;
                bllFacturaCab.IdFacturaCab = entFacturaGlobal.IdFacturaCab;
                bllFacturaCab.IdEmpresa = entFacturaGlobal.IdEmpresa;
                bllFacturaCab.BookingID = entFacturaGlobal.BookingID;
                bllFacturaCab.FechaHoraExpedicion = entFacturaGlobal.FechaHoraExpedicion;
                bllFacturaCab.TipoFacturacion = entFacturaGlobal.TipoFacturacion;
                bllFacturaCab.Version = entFacturaGlobal.Version;
                bllFacturaCab.Serie = entFacturaGlobal.Serie;
                bllFacturaCab.FolioFactura = entFacturaGlobal.FolioFactura;
                bllFacturaCab.UUID = entFacturaGlobal.UUID;
                bllFacturaCab.TransactionID = entFacturaGlobal.TransactionID;
                bllFacturaCab.IdPeticionPAC = entFacturaGlobal.IdPeticionPAC;
                bllFacturaCab.Estatus = entFacturaGlobal.Estatus;
                bllFacturaCab.RfcEmisor = entFacturaGlobal.RfcEmisor;
                bllFacturaCab.RazonSocialEmisor = entFacturaGlobal.RazonSocialEmisor;
                bllFacturaCab.NoCertificado = entFacturaGlobal.NoCertificado;
                bllFacturaCab.IdRegimenFiscal = entFacturaGlobal.IdRegimenFiscal;
                bllFacturaCab.RfcReceptor = entFacturaGlobal.RfcReceptor;
                bllFacturaCab.RazonSocialReceptor = entFacturaGlobal.RazonSocialReceptor;
                bllFacturaCab.EmailReceptor = entFacturaGlobal.EmailReceptor;
                bllFacturaCab.EsExtranjero = entFacturaGlobal.EsExtranjero;
                bllFacturaCab.IdPaisResidenciaFiscal = entFacturaGlobal.IdPaisResidenciaFiscal;
                bllFacturaCab.NumRegIdTrib = entFacturaGlobal.NumRegIdTrib;
                bllFacturaCab.UsoCFDI = entFacturaGlobal.UsoCFDI;
                bllFacturaCab.FormaPago = entFacturaGlobal.FormaPago;
                bllFacturaCab.MetodoPago = entFacturaGlobal.MetodoPago;
                bllFacturaCab.TipoComprobante = entFacturaGlobal.TipoComprobante;
                bllFacturaCab.LugarExpedicion = entFacturaGlobal.LugarExpedicion;
                bllFacturaCab.CondicionesPago = entFacturaGlobal.CondicionesPago;
                bllFacturaCab.Moneda = entFacturaGlobal.Moneda;
                bllFacturaCab.TipoCambio = entFacturaGlobal.TipoCambio;
                bllFacturaCab.SubTotal = entFacturaGlobal.SubTotal;
                bllFacturaCab.Descuento = entFacturaGlobal.Descuento;
                bllFacturaCab.Total = entFacturaGlobal.Total;
                bllFacturaCab.MontoTarifa = entFacturaGlobal.MontoTarifa;
                bllFacturaCab.MontoServAdic = entFacturaGlobal.MontoServAdic;
                bllFacturaCab.MontoTUA = entFacturaGlobal.MontoTUA;
                bllFacturaCab.MontoOtrosCargos = entFacturaGlobal.MontoOtrosCargos;
                bllFacturaCab.MontoIVA = entFacturaGlobal.MontoIVA;
                bllFacturaCab.IdAgente = entFacturaGlobal.IdAgente;
                bllFacturaCab.IdUsuario = entFacturaGlobal.IdUsuario;
                bllFacturaCab.FechaHoraLocal = entFacturaGlobal.FechaHoraLocal;
                bllFacturaCab.IdUsuarioCancelo = entFacturaGlobal.IdUsuarioCancelo;
                bllFacturaCab.FechaHoraCancelLocal = entFacturaGlobal.FechaHoraCancelLocal;
                bllFacturaCab.FechaHoraPago = entFacturaGlobal.FechaHoraPago;

                bllFacturaCab.Agregar();

                entFacturaGlobal.IdFacturaCab = bllFacturaCab.IdFacturaCab;


                //2. [dbo].[VBFac_Facturas_Det]
                int idDetFinal = 0;
                //Agregando el detalle de la factura
                foreach (ENTFacturasDet entFactDet in entFacturaGlobal.ListaFacturasDet.OrderByDescending(x => x.IdFacturaDet))
                {
                    idDetFinal++;
                    BLLFacturasDet facturaDet = new BLLFacturasDet();
                    entFactDet.IdFacturaCab = entFacturaGlobal.IdFacturaCab;
                    facturaDet.IdFacturaCab = entFactDet.IdFacturaCab;
                    facturaDet.IdFacturaDet = idDetFinal;
                    facturaDet.ClaveProdServ = entFactDet.ClaveProdServ;
                    facturaDet.NoIdentificacion = entFactDet.NoIdentificacion;
                    facturaDet.Cantidad = entFactDet.Cantidad;
                    facturaDet.ClaveUnidad = entFactDet.ClaveUnidad;
                    facturaDet.Unidad = entFactDet.Unidad;
                    facturaDet.Descripcion = entFactDet.Descripcion;
                    facturaDet.ValorUnitario = entFactDet.ValorUnitario;
                    facturaDet.Importe = entFactDet.Importe;
                    facturaDet.Descuento = entFactDet.Descuento;
                    facturaDet.FechaHoraLocal = DateTime.Now;
                    facturaDet.Agregar();

                    //3. [dbo].[VBFac_FacturasIVA_Det]
                    //Se registra el IVA
                    var ivasDet = entFacturaGlobal.ListaIVAPorDetalle.Where(x => x.IdFacturaCab == idFacturaCabOri && x.IdFacturaDet == entFactDet.IdFacturaDet);
                    foreach (ENTFacturasivaDet ivaDet in ivasDet)
                    {
                        BLLFacturasivaDet bllIvaDet = new BLLFacturasivaDet();
                        bllIvaDet.IdFacturaCab = entFacturaGlobal.IdFacturaCab;
                        bllIvaDet.IdFacturaDet = idDetFinal;
                        bllIvaDet.TipoFactor = ivaDet.TipoFactor;
                        bllIvaDet.TasaOCuota = ivaDet.TasaOCuota;
                        bllIvaDet.Base = ivaDet.Base;
                        bllIvaDet.Impuesto = ivaDet.Impuesto;
                        bllIvaDet.Importe = ivaDet.Importe;
                        bllIvaDet.FechaHoraLocal = DateTime.Now;
                        bllIvaDet.Agregar();
                    }

                    //4. [dbo].[VBFac_FacturasCargos_Det]
                    var cargosDet = entFacturaGlobal.ListaCargosAero.Where(x => x.IdFacturaCab == idFacturaCabOri && x.IdFacturaDet == entFactDet.IdFacturaDet);

                    foreach (ENTFacturascargosDet factCargosDet in cargosDet)
                    {
                        //Agregando los Cargos Adicionales
                        BLLFacturascargosDet facCargosDet = new BLLFacturascargosDet();
                        facCargosDet.IdFacturaCab = entFacturaGlobal.IdFacturaCab;
                        facCargosDet.IdFacturaDet = factCargosDet.IdFacturaDet;
                        facCargosDet.CodigoCargo = factCargosDet.CodigoCargo;
                        facCargosDet.EsTua = factCargosDet.EsTua;
                        facCargosDet.Importe = factCargosDet.Importe;
                        facCargosDet.FechaHoraLocal = DateTime.Now;
                        facCargosDet.Agregar();
                    }

                }

                //Se agrega el CFDI
                BLLFacturascfdiDet cfdiFact = new BLLFacturascfdiDet();
                cfdiFact.IdFacturaCab = entFacturaGlobal.IdFacturaCab;
                cfdiFact.TransaccionID = entFacturaGlobal.TransactionID;
                cfdiFact.CFDI = entFacturaGlobal.FacturasCFDIDet.CFDI;
                cfdiFact.CadenaOriginal = entFacturaGlobal.FacturasCFDIDet.CadenaOriginal;
                cfdiFact.FechaTimbrado = entFacturaGlobal.FacturasCFDIDet.FechaTimbrado;
                cfdiFact.FechaHoraLocal = DateTime.Now;
                cfdiFact.Agregar();


                //6. [dbo].[VBFac_GlobalTickets_Det]
                foreach (ENTGlobalticketsDet globalTkt in entFacturaGlobal.ListaGlobalTicketsDet)
                {
                    BLLGlobalticketsDet bllGlobal = new BLLGlobalticketsDet();

                    bllGlobal.IdFacturaCab = entFacturaGlobal.IdFacturaCab;
                    bllGlobal.IdFacturaDet = globalTkt.IdFacturaDet;
                    bllGlobal.BookingID = globalTkt.BookingID;
                    bllGlobal.PaymentID = globalTkt.PaymentID;
                    bllGlobal.IdNotaCredito = globalTkt.IdNotaCredito;
                    bllGlobal.IdFacturaCabCliente = globalTkt.IdFacturaCabCliente;
                    bllGlobal.FechaHoraLocal = globalTkt.FechaHoraLocal;
                    bllGlobal.MontoTarifa = globalTkt.MontoTarifa;
                    bllGlobal.MontoServAdic = globalTkt.MontoServAdic;
                    bllGlobal.MontoTUA = globalTkt.MontoTUA;
                    bllGlobal.MontoOtrosCargos = globalTkt.MontoOtrosCargos;
                    bllGlobal.MontoIVA = globalTkt.MontoIVA;
                    bllGlobal.MontoTotal = globalTkt.MontoTotal;
                    bllGlobal.Agregar();

                    //SE ACTUALIZA EL NUMERO DE FOLIO DE FACTURA GLOBAL EN QUE SE ASIGNO EL PAGO 
                    //Se buscan los pagos que se encuentran agrupados ya sea por multipago o por ser pagos divididos
                    //8. [dbo].[VBFac_Pagos_Cab]
                    BLLPagosCab bllPago = new BLLPagosCab();
                    List<ENTPagosCab> listaPagos = new List<ENTPagosCab>();
                    listaPagos = bllPago.RecuperarPagosCabFolioprefactura(globalTkt.PaymentID);
                    foreach (ENTPagosCab entPago in listaPagos)
                    {
                        BLLPagosCab bllPagoAct = new BLLPagosCab();
                        bllPagoAct.RecuperarPagosCabPorLlavePrimaria(entPago.IdPagosCab);
                        if (bllPagoAct != null)
                        {
                            bllPagoAct.IdFacturaGlobal = entFacturaGlobal.IdFacturaCab;
                            bllPagoAct.Actualizar();
                        }
                    }
                }


                //7. [dbo].[VBFac_GlobalCargosAero_Det]
                foreach (ENTGlobalcargosaeroDet globalCargo in entFacturaGlobal.ListaGlobalcargosaeroDet)
                {
                    BLLGlobalcargosaeroDet bllGlobalCargos = new BLLGlobalcargosaeroDet();

                    bllGlobalCargos.IdFacturaCab = entFacturaGlobal.IdFacturaCab;
                    bllGlobalCargos.IdFacturaDet = globalCargo.IdFacturaDet;
                    bllGlobalCargos.PaymentID = globalCargo.PaymentID;
                    bllGlobalCargos.CodigoCargo = globalCargo.CodigoCargo;
                    bllGlobalCargos.Importe = globalCargo.Importe;
                    bllGlobalCargos.EsTua = globalCargo.EsTua;
                    bllGlobalCargos.FechaHoraLocal = globalCargo.FechaHoraLocal;
                    bllGlobalCargos.Agregar();
                }

                result = true;
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "Facturacion", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "Facturacion", "GuardarFactura");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result;


        }

        private long RecuperarFolioFiscal(string tipoComprobante)
        {
            long result = 0;
            try
            {
                BLLFoliosfiscalesCnf bllFolio = new BLL.BLLFoliosfiscalesCnf();
                List<ENTFoliosfiscalesCnf> listaFolios = new List<ENTFoliosfiscalesCnf>();
                listaFolios = bllFolio.RecuperarTodo();
                ENTFoliosfiscalesCnf folio = listaFolios.Where(x => x.TipoComprobante == tipoComprobante && x.Activo == true && x.FolioFinal > x.FolioActual).FirstOrDefault();
                if (folio != null)
                {
                    result = folio.FolioActual + 1;
                    bllFolio.RecuperarFoliosfiscalesCnfPorLlavePrimaria(folio.IdFolioFiscal);
                    bllFolio.FolioActual = result;
                    bllFolio.Actualizar();
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "FacturacionGlobal", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "FacturacionGlobal", "InicializaVariablesGlobales");
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;
        }


        #region FacturacionGlobal

        private ENTFacturaGlobal GenerarInformacionFacturaGlobal(string moneda, DateTime fechaPagosFG, string codigoPostalFG, int numPagos, List<long> listaPagos, DataSet dsPagosGlobal, ref bool flg_Terceros, List<string> RecordsLocator)
        {
            //Inicia la generacion de los datos de la factura
            ENTFacturaGlobal facturaGlobal = new ENTFacturaGlobal();

            try
            {
                DateTime fechaHoraLocal = DateTime.Now;
                //Recuperar Parametros Facturacion
                string versionFac = ListaParametros.Where(x => x.Nombre == "VersionFactura").FirstOrDefault().Valor; // "3.3";
                string serieFac = ListaParametros.Where(x => x.Nombre == "SerieFactura").FirstOrDefault().Valor; //"B";

                string tipoComprobante = ListaParametros.Where(x => x.Nombre == "CveTipoComprobanteFac").FirstOrDefault().Valor; // "I";
                string permitirConfirmacion = ListaParametros.Where(x => x.Nombre == "PermitirConfirmacion").FirstOrDefault().Valor; //"false";
                byte idEmpresa = Convert.ToByte(ListaParametros.Where(x => x.Nombre == "IdEmpresa").FirstOrDefault().Valor); // IdEmpresa
                ENTEmpresaCat empresa = new ENTEmpresaCat();
                //decimal limiteFacturacion = Convert.ToDecimal(ListaParametros.Where(x => x.Nombre == "LimiteImporteFactura").FirstOrDefault().Valor);
                double porcMargenTC = ((Convert.ToDouble(ListaParametros.Where(x => x.Nombre == "LimiteVariacionUSD").FirstOrDefault().Valor)) / 100);
                byte porcIVAParam = Convert.ToByte(ListaParametros.Where(x => x.Nombre == "PorcentajeIVAGeneral").FirstOrDefault().Valor); // IdEmpresa

                decimal montoPagoMayor = 0;
                long paymentIdBase = 0;

                //Identificar el Pago con mayor importe
                foreach (long paymentId in listaPagos)
                {

                    DataRow drPago = dsPagosGlobal.Tables["dtPagos"].Select("PaymentID = " + paymentId.ToString()).FirstOrDefault();
                    if (drPago != null)
                    {
                        decimal montoTotalPago = 0;
                        montoTotalPago = Convert.ToDecimal(drPago["MontoTotal"].ToString());
                        if (montoTotalPago > montoPagoMayor)
                        {
                            montoPagoMayor = montoTotalPago;
                            paymentIdBase = paymentId;
                        }
                    }

                }

                ENTPagosCab entPagoBase = new ENTPagosCab();
                BLLPagosCab bllPago = new BLL.BLLPagosCab();
                entPagoBase = bllPago.RecuperarPagosCabPaymentid(paymentIdBase).FirstOrDefault();



                ENTFormapagoCat entFormaPago = new ENTFormapagoCat();
                int idFormaPago = entPagoBase.IdFormaPago;
                entFormaPago = ListaCatalogoFormasPago.Where(x => x.IdFormaPago == idFormaPago).FirstOrDefault();

                string metodoPago = "PUE";

                empresa = ListaCatalogoEmpresas.Where(x => x.IdEmpresa == idEmpresa).FirstOrDefault(); //"601";

                BLLDistribucionPagos bllDistribucion = new BLLDistribucionPagos();
                decimal tipoCambioActual = bllDistribucion.RecuperarTipoCambioPorFecha(entPagoBase.FechaPago); //Recupera el tipo de cambio del dia
                decimal limiteSupTC = 0;
                decimal limiteInfTC = 0;

                limiteSupTC = tipoCambioActual * Convert.ToDecimal(1 + porcMargenTC);
                limiteInfTC = tipoCambioActual * Convert.ToDecimal(1 - porcMargenTC);




                long folioFiscal = RecuperarFolioFiscal("G");


                bool solicitarConfirmacion = false;
                solicitarConfirmacion = (entPagoBase.TipoCambio < limiteInfTC
                                        || entPagoBase.TipoCambio > limiteSupTC
                                        );


                facturaGlobal.IdFacturaCab = 1;
                facturaGlobal.IdEmpresa = idEmpresa;
                facturaGlobal.BookingID = 0; //Se predefine 0 porque van los pagos de mas de una reservacion
                facturaGlobal.FechaHoraExpedicion = fechaHoraLocal;
                facturaGlobal.TipoFacturacion = "FG";
                facturaGlobal.Version = versionFac;
                facturaGlobal.Serie = serieFac;// "B";
                facturaGlobal.FolioFactura = folioFiscal;//  12860589;
                facturaGlobal.UUID = "";
                facturaGlobal.TransactionID = GeneratransactionID(empresa.Rfc, folioFiscal, fechaHoraLocal);
                facturaGlobal.IdPeticionPAC = 0;
                facturaGlobal.Estatus = "";
                facturaGlobal.RfcEmisor = empresa.Rfc;// "ANA050518RL1";
                facturaGlobal.RazonSocialEmisor = empresa.RazonSocial;
                facturaGlobal.NoCertificado = empresa.NoCertificado;
                facturaGlobal.IdRegimenFiscal = empresa.IdRegimenFiscal;//  "601";
                facturaGlobal.RfcReceptor = RFCReceptorFG;//  "CAIL770623L23";
                facturaGlobal.RazonSocialReceptor = "";
                facturaGlobal.EmailReceptor = "";//  "luis.carrasco@vivaaerobus.com";
                facturaGlobal.EsExtranjero = false;//  
                facturaGlobal.IdPaisResidenciaFiscal = string.Empty;
                facturaGlobal.NumRegIdTrib = string.Empty;
                facturaGlobal.UsoCFDI = "P01";//  "P01";


                facturaGlobal.FormaPago = entFormaPago.CveFormaPagoSAT;//  "01";
                facturaGlobal.MetodoPago = metodoPago;// "PUE";
                facturaGlobal.TipoComprobante = tipoComprobante;// "I";
                facturaGlobal.LugarExpedicion = codigoPostalFG;//  "66600";
                facturaGlobal.CondicionesPago = string.Empty;// "";
                facturaGlobal.Moneda = entPagoBase.CurrencyCode;
                facturaGlobal.TipoCambio = entPagoBase.TipoCambio;

                facturaGlobal.IdAgente = 0;
                facturaGlobal.IdUsuario = 0;
                facturaGlobal.FechaHoraLocal = fechaHoraLocal;
                facturaGlobal.FechaHoraPago = fechaPagosFG;
                facturaGlobal.SolicitaConfirmacion = solicitarConfirmacion;


                List<ENTFacturasDet> listaFacDet = new List<ENTFacturasDet>();
                List<ENTFacturasivaDet> listaIVADet = new List<ENTFacturasivaDet>();
                List<ENTFacturascargosDet> listaCargosDet = new List<ENTFacturascargosDet>();
                List<ENTGlobalticketsDet> listaGlobalticket = new List<ENTGlobalticketsDet>();
                List<ENTGlobalcargosaeroDet> listaGlobalCargosDet = new List<ENTGlobalcargosaeroDet>();


                //Se recorren los pagos para generar el detalle de conceptos

                //Identificar el Pago con mayor importe
                int cont = 0;
                string claveProdServ = "01010101";

                decimal montoTarifaFG = 0;
                decimal montoServAdicFG = 0;
                decimal montoTUAFG = 0;
                decimal montoOtrosCargosFG = 0;
                decimal montoIVAFG = 0;
                decimal montoSubTotalFG = 0;
                decimal montoDescuentoFG = 0;
                decimal montoTotalFG = 0;

                foreach (long paymentId in listaPagos)
                {
                    cont++;
                    DataRow drPago = dsPagosGlobal.Tables["dtPagos"].Select("PaymentID = " + paymentId.ToString()).FirstOrDefault();
                    if (drPago != null)
                    {
                        decimal montoTarifaPag = 0;
                        decimal montoServAdicPag = 0;
                        decimal montoTUAPag = 0;
                        decimal montoOtrosCargosPag = 0;
                        decimal montoIVAPag = 0;
                        decimal montoSubTotalPag = 0;
                        decimal montoDescuentoPag = 0;
                        decimal montoTotalPag = 0;
                        long bookingId = 0;

                        //Se obtienen los importes por cada concepto
                        montoTarifaPag = Math.Round(Convert.ToDecimal(drPago["MontoTarifa"].ToString()), 2);
                        montoServAdicPag = Math.Round(Convert.ToDecimal(drPago["MontoServAdic"].ToString()), 2);
                        montoTUAPag = Math.Round(Convert.ToDecimal(drPago["MontoTUA"].ToString()), 2);
                        montoOtrosCargosPag = Math.Round(Convert.ToDecimal(drPago["MontoOtrosCargos"].ToString()), 2);
                        montoIVAPag = Math.Round(Convert.ToDecimal(drPago["MontoIVA"].ToString()), 2);
                        montoSubTotalPag = Math.Round(Convert.ToDecimal(drPago["Subtotal"].ToString()), 2);
                        //montoDescuentoPag = Math.Round(Convert.ToDecimal(drPago["Descuento"].ToString()), 2);
                        montoTotalPag = Math.Round(Convert.ToDecimal(drPago["MontoTotal"].ToString()), 2);

                        bookingId = Convert.ToInt64(drPago["BookingID"].ToString());

                        //Se acumulan en los montos totales de la Factura Global
                        montoTarifaFG += montoTarifaPag;
                        montoServAdicFG += montoServAdicPag;
                        montoTUAFG += montoTUAPag;
                        montoOtrosCargosFG += montoOtrosCargosPag;
                        montoIVAFG += montoIVAPag;
                        montoSubTotalFG += montoSubTotalPag;
                        montoDescuentoFG += montoDescuentoPag;
                        montoTotalFG += montoTotalPag;

                        //Registro del pago como concepto o partida en la factura
                        ENTFacturasDet entFacturaDet = new ENTFacturasDet();
                        entFacturaDet.ValorUnitario = montoSubTotalPag;
                        entFacturaDet.Importe = montoSubTotalPag;
                        entFacturaDet.Descuento = montoDescuentoPag;
                        entFacturaDet.IdFacturaCab = facturaGlobal.IdFacturaCab;
                        entFacturaDet.IdFacturaDet = cont;
                        entFacturaDet.ClaveProdServ = claveProdServ;
                        entFacturaDet.NoIdentificacion = paymentId.ToString();
                        entFacturaDet.Cantidad = 1;
                        entFacturaDet.ClaveUnidad = "ACT";
                        entFacturaDet.Unidad = "";
                        entFacturaDet.Descripcion = "Venta";
                        entFacturaDet.FechaHoraLocal = fechaHoraLocal;
                        listaFacDet.Add(entFacturaDet);


                        decimal montoIVAPagAcum = 0;

                        //Registro del IVA vinculado al Pago
                        foreach (DataRow drIVA in dsPagosGlobal.Tables["dtIvaTrasladado"].Select("FolioPrefactura = " + paymentId.ToString()))
                        {
                            decimal tasaOCuota = 0;
                            decimal baseCalculo = 0;
                            decimal importeIVA = 0;

                            tasaOCuota = Math.Round((Convert.ToDecimal(drIVA["PorcIva"].ToString()) / 100), 4);
                            baseCalculo = Math.Round(Convert.ToDecimal(drIVA["Base"].ToString()), 2);
                            importeIVA = Math.Round(Convert.ToDecimal(drIVA["MontoIVA"].ToString()), 2);
                            if (tasaOCuota > 0 && importeIVA > 0)
                            {
                                baseCalculo = Math.Round((importeIVA / tasaOCuota), 2);
                            }

                            if (baseCalculo > 0)
                            {
                                ENTFacturasivaDet entFacturaIVA = new ENTFacturasivaDet();
                                entFacturaIVA.IdFacturaCab = entFacturaDet.IdFacturaCab;
                                entFacturaIVA.IdFacturaDet = entFacturaDet.IdFacturaDet;
                                entFacturaIVA.TipoFactor = "Tasa";
                                entFacturaIVA.TasaOCuota = tasaOCuota;
                                entFacturaIVA.Base = baseCalculo;
                                entFacturaIVA.Impuesto = "002";
                                entFacturaIVA.Importe = importeIVA;
                                entFacturaIVA.FechaHoraLocal = fechaHoraLocal;
                                listaIVADet.Add(entFacturaIVA);
                                montoIVAPagAcum += importeIVA;
                            }
                        }

                        //Registro del IVA vinculado al Pago
                        foreach (DataRow drCargo in dsPagosGlobal.Tables["dtCargosPorFolioPrefactura"].Select("FolioPrefactura = " + paymentId.ToString()))
                        {
                            string codigoCargo = "";
                            decimal importeCargo = 0;
                            string tipoAcumulado = "";
                            bool esTUA = false;

                            codigoCargo = drCargo["TicketCode"].ToString();
                            importeCargo = Math.Round(Convert.ToDecimal(drCargo["MontoCargo"].ToString()), 2);
                            tipoAcumulado = drCargo["TipoAcumulado"].ToString();
                            esTUA = (tipoAcumulado == "TUA");

                            ENTFacturascargosDet entFacturaCargosDet = new ENTFacturascargosDet();
                            entFacturaCargosDet = listaCargosDet.Where(x => x.CodigoCargo == codigoCargo).FirstOrDefault();
                            if (entFacturaCargosDet != null)
                            {
                                entFacturaCargosDet.Importe += importeCargo;
                            }
                            else
                            {
                                entFacturaCargosDet = new ENTFacturascargosDet();
                                entFacturaCargosDet.IdFacturaCab = entFacturaDet.IdFacturaCab;
                                entFacturaCargosDet.IdFacturaDet = 1;
                                entFacturaCargosDet.CodigoCargo = codigoCargo;
                                entFacturaCargosDet.Importe = importeCargo;
                                entFacturaCargosDet.EsTua = esTUA;
                                entFacturaCargosDet.FechaHoraLocal = fechaHoraLocal;
                                listaCargosDet.Add(entFacturaCargosDet);
                            }



                            //Registra el detalle por cada cargo
                            ENTGlobalcargosaeroDet entGlobalCargosDet = new ENTGlobalcargosaeroDet();
                            entGlobalCargosDet.IdFacturaCab = entFacturaDet.IdFacturaCab;
                            entGlobalCargosDet.IdFacturaDet = entFacturaDet.IdFacturaDet;
                            entGlobalCargosDet.PaymentID = paymentId;
                            entGlobalCargosDet.CodigoCargo = codigoCargo;
                            entGlobalCargosDet.Importe = importeCargo;
                            entGlobalCargosDet.EsTua = esTUA;
                            entGlobalCargosDet.FechaHoraLocal = fechaHoraLocal;
                            listaGlobalCargosDet.Add(entGlobalCargosDet);


                        }

                        //Genera el registro en la tabla de pagos acumulados
                        ENTGlobalticketsDet entGlobalTicket = new ENTGlobalticketsDet();
                        entGlobalTicket.IdFacturaCab = facturaGlobal.IdFacturaCab;
                        entGlobalTicket.IdFacturaDet = entFacturaDet.IdFacturaDet;
                        entGlobalTicket.BookingID = bookingId;
                        entGlobalTicket.PaymentID = paymentId;
                        entGlobalTicket.FechaHoraLocal = fechaHoraLocal;
                        entGlobalTicket.MontoTarifa = montoTarifaPag;
                        entGlobalTicket.MontoServAdic = montoServAdicPag;
                        entGlobalTicket.MontoTUA = montoTUAPag;
                        entGlobalTicket.MontoOtrosCargos = montoOtrosCargosPag;
                        entGlobalTicket.MontoIVA = montoIVAPag;
                        entGlobalTicket.MontoTotal = montoTotalPag;
                        listaGlobalticket.Add(entGlobalTicket);



                    }

                }

                VBFactPaquetes.PortalWeb.Controllers.HomeController get_Token = new VBFactPaquetes.PortalWeb.Controllers.HomeController();
                Paquete paquete = new Paquete();
                foreach (string pnr in RecordsLocator)
                {
                    string token = get_Token.getAuthToken();
                    paquete = JsonConvert.DeserializeObject<Paquete>(get_Token.getBookingByPNR(token, pnr));
                    if (paquete.items != null)
                    {
                        facturaGlobal.Total_Terceros += Convert.ToDecimal(Math.Round(paquete.items[1].sellingPrice, 2));
                        flg_Terceros = true;
                    }
                }



                if (flg_Terceros)
                {
                    List<ENTConceptosCat> ListaCatalogoConceptos = new List<ENTConceptosCat>();
                    List<ENTHotel> ListaCatalogoConceptosHotel = new List<ENTHotel>();



                    //Iniciar Catalogo Conceptos de Facturacion
                    if (ListaCatalogoConceptos.Count() == 0)
                    {
                        BLLConceptosCat bllConceptosFac = new BLLConceptosCat();
                        ListaCatalogoConceptos = bllConceptosFac.RecuperarTodo();
                    }
                    //Iniciar Catalogo Conceptos de Facturacion
                    if (ListaCatalogoConceptosHotel.Count() == 0)
                    {
                        BLLHotel bllConceptosFacHotel = new BLLHotel();
                        ListaCatalogoConceptosHotel = bllConceptosFacHotel.RecuperarInfoProveedor();
                    }



                    ENTConceptosCat entConcepto = new ENTConceptosCat();
                    entConcepto = ListaCatalogoConceptos.Where(x => x.Descripcion == "TARIFA HOTELERA").FirstOrDefault();



                    ENTFacturasDet entFacturaDet = new ENTFacturasDet();
                    entFacturaDet.ClaveProdServ = entConcepto.ClaveProdServ;
                    entFacturaDet.NoIdentificacion = entConcepto.NoIdentificacion;
                    entFacturaDet.Cantidad = 1;
                    entFacturaDet.ClaveUnidad = entConcepto.ClaveUnidad;
                    entFacturaDet.Unidad = entConcepto.Unidad;
                    entFacturaDet.Descripcion = entConcepto.Descripcion;// "TARIFA AEREA PNR: VBJKJN";



                    ENTHotel entConceptoHotel = new ENTHotel();
                    entConceptoHotel = ListaCatalogoConceptosHotel.FirstOrDefault();
                    entFacturaDet.RFC = entConceptoHotel.RFC;
                    entFacturaDet.Nombre = entConceptoHotel.Nombre;
                    entFacturaDet.Calle = entConceptoHotel.Calle;
                    entFacturaDet.NumExt = entConceptoHotel.NumExt;
                    entFacturaDet.Municipio = entConceptoHotel.Municipio;
                    entFacturaDet.Estado = entConceptoHotel.Estado;
                    entFacturaDet.Pais = entConceptoHotel.Pais;
                    entFacturaDet.CodigoPostal = entConceptoHotel.CodigoPostal;



                    listaFacDet.Add(entFacturaDet);
                }

                facturaGlobal.MontoTarifa = montoTarifaFG;
                facturaGlobal.MontoServAdic = montoServAdicFG;
                facturaGlobal.MontoTUA = montoTUAFG;
                facturaGlobal.MontoOtrosCargos = montoOtrosCargosFG;
                facturaGlobal.MontoIVA = montoIVAFG;
                facturaGlobal.SubTotal = montoSubTotalFG;
                facturaGlobal.Total = montoTotalFG;


                facturaGlobal.ListaFacturasDet = listaFacDet;
                facturaGlobal.ListaIVAPorDetalle = listaIVADet;
                facturaGlobal.ListaCargosAero = listaCargosDet;
                facturaGlobal.ListaGlobalTicketsDet = listaGlobalticket;
                facturaGlobal.ListaGlobalcargosaeroDet = listaGlobalCargosDet;

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "Facturacion", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "Facturacion", "GenerarInformacionFactura");
                throw new ExceptionViva(mensajeUsuario);
            }


            return facturaGlobal;

        }



        public string GeneratransactionID(string rfcEmisor, long folioFiscal, DateTime fechaFactura)
        {
            string result = "";

            try
            {
                string codigoSAP = "";
                string leyenda = "GLOBAL";

                codigoSAP = ListaParametros.Where(x => x.Nombre == "CodigoSAPGenerico").FirstOrDefault().Valor; //"4000902"; 

                result = rfcEmisor.Substring(0, 3) + codigoSAP + fechaFactura.ToString("yyMMdd") + leyenda + "-" + folioFiscal.ToString("00000000");
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "Facturacion", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "Facturacion", "GeneratransactionID");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result;
        }

        private string GenerarXMLRequestFG(ENTFacturaGlobal factura, bool flg_Terceros)
        {
            string result = "";
            string PNR = "";
            int numSegDesfaseSAT = 0;
            PNR = "GLOBAL";
            try
            {
                BLLPegaso bllPeg = new ProcesoFacturacion.BLLPegaso();

                numSegDesfaseSAT = bllPeg.ObtieneAjusteSegFechaSAT();

                System.Text.StringBuilder xmlFact = new System.Text.StringBuilder();

                xmlFact.Append("<RequestCFD ");
                xmlFact.Append("version=\"" + factura.Version + "\">");
                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("	<Comprobante ");
                xmlFact.Append("Version=\"" + factura.Version + "\" ");
                if (flg_Terceros)
                {
                    factura.Serie = factura.Serie.Split(',')[2];
                    xmlFact.Append("Serie=\"" + factura.Serie + "\" ");
                }
                else
                {
                    factura.Serie = factura.Serie.Split(',')[0];
                    xmlFact.Append("Serie=\"" + factura.Serie + "\" ");
                }
                //xmlFact.Append("Serie=\"" + factura.Serie + "\" ");
                xmlFact.Append("Folio=\"" + factura.FolioFactura.ToString() + "\" ");

                // DHV INI 24-OCT-2019 Fecha de envío según huso horario del lugar exp
                BLLCpSatCat bllCPsat = new BLLCpSatCat();
                BLLEntidadesfedSatCat bllEntidadesFed = new BLLEntidadesfedSatCat();
                List<ENTCpSatCat> listaCP = new List<ENTCpSatCat>();
                List<ENTEntidadesfedSatCat> listaEntFed = new List<ENTEntidadesfedSatCat>();

                listaCP = bllCPsat.RecuperarCpSatCatPorLlavePrimaria(factura.LugarExpedicion);
                String entidadFede = listaCP != null && listaCP.Count > 0 ? listaCP.FirstOrDefault().ClaveEntidadFed : "";
                if (String.IsNullOrEmpty(entidadFede))
                    throw new Exception("Lugar de Expedicion: " + factura.LugarExpedicion + ", no dado de alta");

                listaEntFed = bllEntidadesFed.RecuperarEntidadesfedSatCatPorLlavePrimaria(entidadFede);
                String husoHorario = listaEntFed != null && listaEntFed.Count > 0 ? listaEntFed.FirstOrDefault().HusoHorario : "";

                if (String.IsNullOrEmpty(husoHorario))
                    throw new Exception("Entidad federativa: " + entidadFede + ", no dado de alta");

                DateTime fechaEnvio = DateTime.Now;
                fechaEnvio = fechaEnvio.AddSeconds(numSegDesfaseSAT * -1);

                if (!String.IsNullOrEmpty(factura.LugarExpedicion) && int.Parse(factura.LugarExpedicion) > 0)
                {
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(husoHorario);
                    fechaEnvio = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, tzi);
                }

                xmlFact.Append("Fecha=\"" + fechaEnvio.ToString("yyyy-MM-ddTHH:mm:ss") + "\" ");
                // DHV FIN 24-OCT-2019 Fecha de envío según huso horario del lugar exp

                //Se debe crear el metodo que determine la forma de pago
                xmlFact.Append("FormaPago=\"" + factura.FormaPago + "\" ");
                xmlFact.Append("NoCertificado=\"" + factura.NoCertificado + "\"  ");
                //xmlFact.Append("CondicionesDePago=\"" + factura.CondicionesPago + "\" ");
                if (flg_Terceros)
                {
                    decimal subTotal = factura.SubTotal + factura.Total_Terceros;
                    xmlFact.Append("SubTotal=\"" + subTotal.ToString("0.00") + "\" ");
                }
                else
                {
                    xmlFact.Append("SubTotal=\"" + factura.SubTotal.ToString("0.00") + "\" ");
                }
                //xmlFact.Append("SubTotal=\"" + factura.SubTotal.ToString("0.00") + "\" ");

                if (factura.Descuento > 0)
                {
                    xmlFact.Append("Descuento=\"" + factura.Descuento.ToString("0.00") + "\" ");
                }

                if (factura.Descuento > 0)
                {
                    xmlFact.Append("Descuento=\"" + factura.Descuento.ToString("0.00") + "\" ");
                }

                string moneda = factura.Moneda;
                xmlFact.Append("Moneda=\"" + moneda + "\" ");

                decimal tipoCambio = 0;

                if (moneda.ToUpper() == "MXN")
                {
                    tipoCambio = 1;
                    xmlFact.Append("TipoCambio=\"" + tipoCambio.ToString("0.00") + "\" ");
                }
                else
                {

                    if (factura.TipoCambio > 0)
                    {
                        tipoCambio = factura.TipoCambio;
                    }
                    else
                    {
                        tipoCambio = 1;
                    }

                    xmlFact.Append("TipoCambio=\"" + tipoCambio.ToString("0.00") + "\" ");
                }

                if (flg_Terceros)
                {
                    decimal Total = factura.Total + factura.Total_Terceros;
                    xmlFact.Append("Total=\"" + Total.ToString("0.00") + "\" ");
                }
                else
                {
                    xmlFact.Append("Total=\"" + factura.Total.ToString("0.00") + "\" ");
                }

                //xmlFact.Append("Total=\"" + factura.Total.ToString("0.00") + "\" ");
                xmlFact.Append("TipoDeComprobante=\"" + factura.TipoComprobante + "\" ");
                xmlFact.Append("MetodoPago=\"" + factura.MetodoPago + "\" ");
                xmlFact.Append("LugarExpedicion=\"" + factura.LugarExpedicion + "\" ");

                //Se desactiva hasta confirmacion del SAT para el proceso 
                //xmlFact.Append("permitirConfirmacion=\"" + (factura.SolicitaConfirmacion ? "true" : "false") + "\"");

                xmlFact.Append(">");

                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("		<Emisor ");
                xmlFact.Append("Rfc=\"" + factura.RfcEmisor + "\" ");
                xmlFact.Append("Nombre=\"" + factura.RazonSocialEmisor + "\" ");
                xmlFact.Append("RegimenFiscal=\"" + factura.IdRegimenFiscal + "\"/>");
                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("		<Receptor ");

                //LCI. INI. 20180207 CORRECCION DE RFC CON CARACTER &
                /*
                 En el caso del & se debe usar la secuencia &amp; 
                 */
                string rfcFinal = "";
                rfcFinal = factura.RfcReceptor.Replace("&", "&amp;");

                xmlFact.Append("Rfc=\"" + rfcFinal + "\" ");



                xmlFact.Append("UsoCFDI=\"" + factura.UsoCFDI + "\"/>");
                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("		<Conceptos>");
                xmlFact.Append(Environment.NewLine);


                decimal impteAplicaIva = 0;  //Se acumulan los conceptos que aplican iva, todos excepto el de cargos aeroportuarios
                foreach (var pagoAI in factura.ListaFacturasDet)
                {
                    impteAplicaIva += pagoAI.Importe;
                }

                foreach (var concepto in factura.ListaFacturasDet.OrderByDescending(x => x.IdFacturaDet))
                {
                    string descripcion = concepto.Descripcion;
                    string[] listaDesc = descripcion.Split(' ');
                    string noIdentif = "";
                    bool generaIVA = false;

                    //Se verifica que el importe del concepto sea mayor a cero, en caso contrario no se envia como concepto.
                    if (concepto.Importe > 0)
                    {
                        noIdentif = concepto.NoIdentificacion;
                        xmlFact.Append("            <Concepto ");
                        xmlFact.Append("ClaveProdServ=\"" + concepto.ClaveProdServ + "\" ");
                        xmlFact.Append("NoIdentificacion=\"" + concepto.NoIdentificacion + "\" ");
                        xmlFact.Append("Cantidad=\"" + concepto.Cantidad.ToString() + "\" ");
                        xmlFact.Append("ClaveUnidad=\"" + concepto.ClaveUnidad + "\" ");
                        //xmlFact.Append("Unidad=\"" + concepto.Unidad + "\" ");
                        xmlFact.Append("Descripcion=\"" + concepto.Descripcion + "\" ");
                        xmlFact.Append("ValorUnitario=\"" + concepto.ValorUnitario.ToString("0.00") + "\" ");
                        xmlFact.Append("Importe=\"" + concepto.Importe.ToString("0.00") + "\" ");
                        xmlFact.Append(">");
                        xmlFact.Append(Environment.NewLine);
                        xmlFact.Append("				<Impuestos>");
                        xmlFact.Append(Environment.NewLine);
                        xmlFact.Append("                    <Traslados>");

                        List<ENTFacturasivaDet> listaIVAPorConcepto = new List<ENTFacturasivaDet>();

                        listaIVAPorConcepto = factura.ListaIVAPorDetalle.Where(x => x.IdFacturaCab == concepto.IdFacturaCab && x.IdFacturaDet == concepto.IdFacturaDet).DefaultIfEmpty().ToList();

                        foreach (ENTFacturasivaDet ivaConcepto in listaIVAPorConcepto)
                        {

                            xmlFact.Append(Environment.NewLine);
                            xmlFact.Append("                        <Traslado ");
                            xmlFact.Append("Base=\"" + ivaConcepto.Base.ToString("0.00") + "\" ");
                            xmlFact.Append("Impuesto=\"" + ivaConcepto.Impuesto.ToString() + "\" ");
                            xmlFact.Append("TipoFactor=\"" + ivaConcepto.TipoFactor.ToString() + "\" ");
                            xmlFact.Append("TasaOCuota=\"" + ivaConcepto.TasaOCuota.ToString("0.000000") + "\" ");
                            xmlFact.Append("Importe=\"" + ivaConcepto.Importe.ToString("0.00") + "\" ");
                            xmlFact.Append("/>");
                        }


                        xmlFact.Append(Environment.NewLine);
                        xmlFact.Append("					</Traslados>");
                        xmlFact.Append(Environment.NewLine);
                        xmlFact.Append("                </Impuestos>");
                        xmlFact.Append(Environment.NewLine);
                        //}
                        xmlFact.Append("            </Concepto>");
                        xmlFact.Append(Environment.NewLine);
                    }
                    else if(concepto.Importe < 0)
                    {
                        throw new Exception(String.Format("El concepto {0}, contiene un importe negativo {1}, y no es posible generar la solicitud de timbrado, verifique...", concepto.Descripcion, concepto.Importe));
                    }
                }

                #region TERCEROS
                if (flg_Terceros)
                {
                    int cnL = 0;
                    foreach (var concepto in factura.ListaFacturasDet)
                    {
                        if (concepto.Descripcion == "TARIFA HOTELERA")
                        {
                            break;
                        }
                        cnL++;
                    }
                    xmlFact.Append("            <Concepto ");
                    xmlFact.Append("ClaveProdServ=\"" + factura.ListaFacturasDet[cnL].ClaveProdServ + "\" ");
                    xmlFact.Append("NoIdentificacion=\"" + factura.ListaFacturasDet[cnL].NoIdentificacion + "\" ");
                    xmlFact.Append("Cantidad=\"" + factura.ListaFacturasDet[cnL].Cantidad + "\" ");
                    xmlFact.Append("ClaveUnidad=\"" + factura.ListaFacturasDet[cnL].ClaveUnidad + "\" ");
                    xmlFact.Append("Unidad=\"" + factura.ListaFacturasDet[cnL].Unidad + "\" ");
                    xmlFact.Append("Descripcion=\"" + factura.ListaFacturasDet[cnL].Descripcion + "\" ");
                    xmlFact.Append("ValorUnitario=\"" + factura.Total_Terceros + "\" ");
                    xmlFact.Append("Importe=\"" + factura.Total_Terceros + "\" ");
                    xmlFact.Append(">");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                        <Impuestos>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                            <Traslados>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                                <Traslado ");
                    xmlFact.Append("Base=\"" + factura.Total_Terceros + "\" ");
                    xmlFact.Append("Impuesto=\"002\" ");
                    xmlFact.Append("TipoFactor=\" Tasa\" ");
                    xmlFact.Append("TasaOCuota=\"0.000000\" ");
                    xmlFact.Append("Importe=\"0.00\"");
                    xmlFact.Append("/>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                            </Traslados>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                        </Impuestos>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("				<ComplementoConcepto>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                    <PorCuentadeTerceros ");
                    xmlFact.Append("version=\"1.1\" ");
                    xmlFact.Append("rfc=\"" + factura.ListaFacturasDet[cnL].RFC + "\" ");
                    xmlFact.Append("nombre=\"" + factura.ListaFacturasDet[cnL].Nombre + "\"");
                    xmlFact.Append(">");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                        <InformacionFiscalTercero ");
                    xmlFact.Append("calle=\"" + factura.ListaFacturasDet[cnL].Calle + "\" ");
                    xmlFact.Append("noExterior=\"" + factura.ListaFacturasDet[cnL].NumExt + "\" ");
                    xmlFact.Append("municipio=\"" + factura.ListaFacturasDet[cnL].Municipio + "\" ");
                    xmlFact.Append("estado=\"" + factura.ListaFacturasDet[cnL].Estado + "\" ");
                    xmlFact.Append("pais=\"" + factura.ListaFacturasDet[cnL].Pais + "\" ");
                    xmlFact.Append("codigoPostal=\"" + factura.ListaFacturasDet[cnL].CodigoPostal + "\"");
                    xmlFact.Append("/>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                        <Parte ");
                    xmlFact.Append("cantidad=\"" + factura.ListaFacturasDet[cnL].Cantidad + "\" ");
                    xmlFact.Append("unidad=\"" + factura.ListaFacturasDet[cnL].Unidad + "\" ");
                    xmlFact.Append("descripcion=\"" + factura.ListaFacturasDet[cnL].Descripcion + "\" ");
                    xmlFact.Append("valorUnitario=\"" + factura.Total_Terceros + "\" ");
                    xmlFact.Append("importe=\"" + factura.Total_Terceros + "\" ");
                    xmlFact.Append("/>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                    </PorCuentadeTerceros>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("				</ComplementoConcepto>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("            </Concepto>");
                    xmlFact.Append(Environment.NewLine);
                }
                #endregion

                xmlFact.Append("        </Conceptos>");

                //Finaliza bloque de Conceptos

                //Inicia bloque de Impuestos

                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("        <Impuestos ");
                xmlFact.Append("TotalImpuestosTrasladados=\"" + factura.MontoIVA.ToString("0.00") + "\">");
                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("			<Traslados>");
                xmlFact.Append(Environment.NewLine);

                //Obtiene una lista del impuesto a desglosar

                var listaImpuesto = factura.ListaIVAPorDetalle.Select(x => x.Impuesto).Distinct();


                foreach (string cveImpuesto in listaImpuesto)
                {
                    var listaFactor = factura.ListaIVAPorDetalle.Where(x => x.Impuesto == cveImpuesto).Select(x => x.TipoFactor).Distinct();
                    foreach (string factor in listaFactor)
                    {
                        var listaTasa = factura.ListaIVAPorDetalle.Where(x => x.Impuesto == cveImpuesto && x.TipoFactor == factor).Select(x => x.TasaOCuota).Distinct();

                        foreach (decimal tasa in listaTasa)
                        {
                            decimal sumIVA = 0;
                            sumIVA = factura.ListaIVAPorDetalle.Where(x => x.Impuesto == cveImpuesto && x.TipoFactor == factor && x.TasaOCuota == tasa).Sum(x => x.Importe);

                            xmlFact.Append("                <Traslado ");
                            xmlFact.Append("Impuesto=\"" + cveImpuesto.ToString() + "\" ");
                            xmlFact.Append("TipoFactor=\"" + factor.ToString() + "\" ");
                            xmlFact.Append("TasaOCuota=\"" + tasa.ToString("0.000000") + "\" ");
                            xmlFact.Append("Importe=\"" + sumIVA.ToString("0.00") + "\"/>");
                            xmlFact.Append(Environment.NewLine);
                        }

                    }
                }

                xmlFact.Append("			</Traslados>");
                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("        </Impuestos>");
                xmlFact.Append(Environment.NewLine);

                //Finaliza bloque de impuestos acumulados

                //Inicia Bloque de Complemento

                decimal totalCargos = 0;
                decimal totalOtrosCargos = 0;
                decimal totalTua = 0;

                totalTua = factura.MontoTUA;
                totalCargos = factura.ListaCargosAero.Sum(x => x.Importe);
                totalOtrosCargos = totalCargos - totalTua;

                xmlFact.Append("        <Complemento>");
                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("            <Aerolineas ");
                xmlFact.Append("Version=\"1.0\" ");
                xmlFact.Append("TUA=\"" + totalTua.ToString("0.00") + "\">");
                xmlFact.Append(Environment.NewLine);


                if (totalOtrosCargos > 0)
                {
                    xmlFact.Append("				<OtrosCargos ");
                    xmlFact.Append("TotalCargos=\"" + totalOtrosCargos.ToString("0.00") + "\">");
                    xmlFact.Append(Environment.NewLine);
                    foreach (var cargoItem in factura.ListaCargosAero.Where(x => x.EsTua == false))
                    {
                        string codigoCargo = cargoItem.CodigoCargo;
                        xmlFact.Append("					<Cargo ");
                        xmlFact.Append("CodigoCargo=\"" + codigoCargo + "\" ");
                        xmlFact.Append("Importe=\"" + cargoItem.Importe.ToString("0.00") + "\"/>");
                        xmlFact.Append(Environment.NewLine);
                    }
                    xmlFact.Append("				</OtrosCargos>");
                }
                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("            </Aerolineas>");
                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("        </Complemento>");
                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("    </Comprobante>");
                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("    <Transaccion ");


                //Se agregan los valores adicionales para la facturacion

                string claveFacPegaso = "";
                string nombreFacPegaso = "";
                string sucursal = "";
                string emailReceptor = "";
                string codigoReceptor = "";
                bool activarCorreoGen = true;
                bool activarSucMatriz = true;


                claveFacPegaso = ListaParametros.Where(x => x.Nombre == "ClaveFacPegaso").FirstOrDefault().Valor; //"false";
                nombreFacPegaso = ListaParametros.Where(x => x.Nombre == "NombreFacPegaso").FirstOrDefault().Valor;
                sucursal = ListaParametros.Where(x => x.Nombre == "NombreSucursalGenerica").FirstOrDefault().Valor;
                emailReceptor = ListaParametros.Where(x => x.Nombre == "EmailReceptor").FirstOrDefault().Valor;
                codigoReceptor = ListaParametros.Where(x => x.Nombre == "CodigoReceptor").FirstOrDefault().Valor;

                activarCorreoGen = Convert.ToBoolean(ListaParametros.Where(x => x.Nombre == "ActivarEmailGen").FirstOrDefault().Valor);
                activarSucMatriz = Convert.ToBoolean(ListaParametros.Where(x => x.Nombre == "ActivarSucursalMatriz").FirstOrDefault().Valor);


                xmlFact.Append("id=\"" + factura.TransactionID.ToString() + "\"/>");
                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("	<TipoComprobante ");
                xmlFact.Append("clave=\"" + claveFacPegaso + "\" ");
                xmlFact.Append("nombre=\"" + nombreFacPegaso + "\"/>");
                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("	<Sucursal ");
                if (activarSucMatriz)
                {
                    xmlFact.Append("nombre=\"" + sucursal + "\"/>");
                }
                else
                {
                    //Falta definir el catalogo de sucursales por location cuando se implemente este cambio
                    xmlFact.Append("nombre=\"" + sucursal + "\"/>");
                }


                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("	<Receptor ");
                if (activarCorreoGen)
                {
                    xmlFact.Append("emailReceptor=\"" + emailReceptor + "\" ");
                    xmlFact.Append("codigoReceptor=\"" + codigoReceptor + "\"/>");
                }
                else
                {
                    xmlFact.Append("emailReceptor=\"" + factura.EmailReceptor + "\" ");
                    xmlFact.Append("codigoReceptor=\"" + factura.EmailReceptor + "\"/>");
                }


                xmlFact.Append(Environment.NewLine);
                xmlFact.Append("</RequestCFD>");

                result = xmlFact.ToString();

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "GeneraXMLRequestFactura");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result;
        }


        #endregion


    }
}

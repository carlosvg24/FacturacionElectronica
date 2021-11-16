using Facturacion.DAL.ProcesoFacturacion;
using Facturacion.ENT;
using Facturacion.ENT.Portal.Facturacion;
using Facturacion.ENT.ProcesoFacturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.BLL.ProcesoFacturacion
{
    public class BLLComplementoPago : DALComplementoPago
    {
        #region Propiedades privadas ListasCatalogos
        //public List<string> ListaCodigosTUA = new List<string>();
        public List<ENTFeeCat> ListaCatalogoFees = new List<ENTFeeCat>();
        public List<ENTFormapagoCat> ListaCatalogoFormasPago = new List<ENTFormapagoCat>();
        //public List<ENTConceptosCat> ListaCatalogoConceptos = new List<ENTConceptosCat>();
        //public List<ENTAgentesCat> ListaCatalogoAgentes = new List<ENTAgentesCat>();
        public List<ENTEmpresaCat> ListaCatalogoEmpresas = new List<ENTEmpresaCat>();
        public List<ENTGencatalogosCat> ListaGenCatalogos = new List<ENTGencatalogosCat>();
        public List<ENTGendescripcionesCat> ListaGenDescripciones = new List<ENTGendescripcionesCat>();
        public List<ENTParametrosCnf> ListaParametros = new List<ENTParametrosCnf>();
        public List<string> ListaErrores = new List<string>();
        public ENTEmpresaCat EmpresaEmisora { get; set; }
        private BLLBitacoraErrores BllLogErrores { get; set; }
        public string PNR { get; set; }
        public string MensajeErrorUsuario { get; set; }
        public string TipoProceso { get; set; }
        public bool EnviarCorreoErrores { get; set; }
        public BLLDistribucionPagos bllDistribucion { get; set; }
        public List<decimal> ListaPorcIVAValidos = new List<decimal>();
        public List<decimal> ListaPorcIVAAplicados = new List<decimal>();
        #endregion ListasCatalogos

        #region propiedades Privadas
        //private string cnfEmailUser;
        //private string cnfEmailSourceName;
        //private string cnfSendGridHost;
        //private string cnfSendGridPort;
        //private string cnfSendGridUser;
        //private string cnfSendGridPass;
        //private string cnfCategoria;
        //private string cnfEmailContactoTestFac;
        //private string cnfEmailCopiaOcultaFac;
        //private string cnfEmailErrorFacturacion;
        //private string cnfRegistrarLog;
        //private string cnfRutaLog;
        //private decimal margenSupIVA;
        //private decimal margenInfIVA;
        //private int toleranciaDiasFacturacion;
        //private string cnfPAC;
        #endregion

        public BLLComplementoPago()
        : base(BLLConfiguracion.ConexionNavitaireWB)
        {
            BllLogErrores = new BLLBitacoraErrores();
            InicializaVariablesGlobales();
            EnviarCorreoErrores = true;
            TipoProceso = "";
            bllDistribucion = new BLLDistribucionPagos();
        }

        private void InicializaVariablesGlobales()
        {
            try
            {
                PNR = "";
                if (ListaCatalogoFees.Count() == 0)
                {
                    //Inicializar Catalogo Fees
                    BLLFeeCat bllFee = new BLLFeeCat();
                    ListaCatalogoFees = bllFee.RecuperarTodo();
                }


                //Iniciar Catalogo Formas de Pago
                if (ListaCatalogoFormasPago.Count() == 0)
                {
                    BLLFormapagoCat bllFormaPago = new BLLFormapagoCat();
                    ListaCatalogoFormasPago = bllFormaPago.RecuperarTodo();
                }

                ////Iniciar Catalogo Conceptos de Facturacion
                //if (ListaCatalogoConceptos.Count() == 0)
                //{
                //    BLLConceptosCat bllConceptosFac = new BLLConceptosCat();
                //    ListaCatalogoConceptos = bllConceptosFac.RecuperarTodo();
                //}

                //if (ListaCatalogoAgentes.Count() == 0)
                //{
                //    BLLAgentesCat bllCatalogoAgentes = new BLLAgentesCat();
                //    ListaCatalogoAgentes = bllCatalogoAgentes.RecuperarTodo();
                //}

                if (ListaCatalogoEmpresas.Count() == 0)
                {
                    BLLEmpresaCat bllCatEmpresa = new BLLEmpresaCat();
                    ListaCatalogoEmpresas = bllCatEmpresa.RecuperarTodo();
                }

                if (ListaGenCatalogos.Count() == 0)
                {
                    BLLGencatalogosCat bllGenCatalogos = new BLLGencatalogosCat();
                    ListaGenCatalogos = bllGenCatalogos.RecuperarTodo();
                }


                if (ListaGenDescripciones.Count() == 0)
                {
                    BLLGendescripcionesCat bllDescripciones = new BLLGendescripcionesCat();
                    ListaGenDescripciones = bllDescripciones.RecuperarTodo();
                }


                if (ListaParametros.Count() == 0)
                {
                    BLLParametrosCnf bllParam = new BLL.BLLParametrosCnf();
                    ListaParametros = RecuperarParametros();

                    //Asignar variables globales de la aplicacion
                    //cnfEmailUser = ListaParametros.Where(x => x.Nombre == "emailUser").FirstOrDefault().Valor;
                    //cnfEmailSourceName = ListaParametros.Where(x => x.Nombre == "emailSourceName").FirstOrDefault().Valor;
                    //cnfSendGridHost = ListaParametros.Where(x => x.Nombre == "SendGridHost").FirstOrDefault().Valor;
                    //cnfSendGridPort = ListaParametros.Where(x => x.Nombre == "SendGridPort").FirstOrDefault().Valor;
                    //cnfSendGridUser = ListaParametros.Where(x => x.Nombre == "SendGridUser").FirstOrDefault().Valor;
                    //cnfSendGridPass = ListaParametros.Where(x => x.Nombre == "SendGridPass").FirstOrDefault().Valor;
                    //cnfCategoria = ListaParametros.Where(x => x.Nombre == "cnfCategoria").FirstOrDefault().Valor;
                    //cnfEmailContactoTestFac = ListaParametros.Where(x => x.Nombre == "emailContactoTestFac").FirstOrDefault().Valor;
                    //cnfEmailCopiaOcultaFac = ListaParametros.Where(x => x.Nombre == "emailCopiaOcultaFac").FirstOrDefault().Valor;
                    //cnfEmailErrorFacturacion = ListaParametros.Where(x => x.Nombre == "emailErrorFacturacion").FirstOrDefault().Valor;
                    //cnfRegistrarLog = ListaParametros.Where(x => x.Nombre == "registrarLog").FirstOrDefault().Valor;
                    //cnfRutaLog = ListaParametros.Where(x => x.Nombre == "rutaLog").FirstOrDefault().Valor;

                    //margenInfIVA = Convert.ToDecimal(ListaParametros.Where(x => x.Nombre == "margenInfIVA").FirstOrDefault().Valor);
                    //margenSupIVA = Convert.ToDecimal(ListaParametros.Where(x => x.Nombre == "margenSupIVA").FirstOrDefault().Valor);
                    //toleranciaDiasFacturacion = Convert.ToInt16(ListaParametros.Where(x => x.Nombre == "ToleranciaDiasFacturacion").FirstOrDefault().Valor);

                    if (ListaParametros.Where(x => x.Nombre == "MsjErrorPortal").Count() > 0)
                    {
                        MensajeErrorUsuario = ListaParametros.Where(x => x.Nombre == "MsjErrorPortal").FirstOrDefault().Valor;
                    }
                    else
                    {
                        MensajeErrorUsuario = "Por el momento no es posible procesar su solicitud por favor intente más tarde...";

                    }
                }
                ////Genera una lista de los codigos que corresponden al TUA, este bloque debe cambiarse a un nivel superior
                //string paramCodigosTUA = ListaParametros.Where(x => x.Nombre == "CodigosTUA").FirstOrDefault().Valor;

                //if (paramCodigosTUA.Length > 0)
                //{
                //    string[] listCodTua = paramCodigosTUA.Split(',');
                //    foreach (string codTUA in listCodTua)
                //    {
                //        ListaCodigosTUA.Add(codTUA);
                //    }
                //}

                byte idEmpresa = Convert.ToByte(ListaParametros.Where(x => x.Nombre == "IdEmpresa").FirstOrDefault().Valor); // IdEmpresa
                EmpresaEmisora = ListaCatalogoEmpresas.Where(x => x.IdEmpresa == idEmpresa).FirstOrDefault();

                //Genera una lista de los porcentajes de IVA validos
                ENTParametrosCnf paramIVA = new ENTParametrosCnf();
                paramIVA = ListaParametros.Where(x => x.Nombre == "PorcentajeIVA").FirstOrDefault();


                if (paramIVA != null && paramIVA.Valor.Length > 0)
                {
                    string paramPorcentajeIVA = paramIVA.Valor;
                    string[] listPorcIVA = paramPorcentajeIVA.Split(',');
                    foreach (string porcIVA in listPorcIVA)
                    {
                        decimal porcIVAVal = 0;
                        if (decimal.TryParse(porcIVA, out porcIVAVal))
                        {
                            if (!ListaPorcIVAValidos.Contains(porcIVAVal))
                            {
                                ListaPorcIVAValidos.Add(porcIVAVal);
                            }
                        }

                    }
                }
                else
                {
                    //En caso de que no exista el parametro del porcentaje de IVAS creara por default los actuales al 2017
                    ListaPorcIVAValidos.Add(0);
                    ListaPorcIVAValidos.Add(4);
                    ListaPorcIVAValidos.Add(16);
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "InicializaVariablesGlobales");
                throw new ExceptionViva(mensajeUsuario);

            }
        }

        public List<ENTReservaPPD> RecuperaReservasPPDPorFiltros(string pnr, string codigoOrgPPD, DateTime FechaIni, DateTime FechaFin, string estatusPago, string estatusFacturacion)
        {
            List<ENTReservaPPD> result = new List<ENTReservaPPD>();

            //En caso de que no se envie un Codigo de Organizacion en especifico se recupera el catalogo y se envian todos los codigos
            if (codigoOrgPPD.Length == 0)
            {
                BLLOrganppdCat bllOrgPPD = new BLL.BLLOrganppdCat();
                List<ENTOrganppdCat> listaOrganization = new List<ENTOrganppdCat>();
                listaOrganization = bllOrgPPD.RecuperarTodo();
                StringBuilder codigos = new StringBuilder();
                string sep = "";
                foreach (ENTOrganppdCat entOrg in listaOrganization)
                {
                    codigos.Append(sep + entOrg.OrganizationCode);
                    sep = ",";
                }
                codigoOrgPPD = codigos.ToString();
            }



            DataTable dtReservasPPD = RecuperarReservasPPDPorFiltros(pnr, codigoOrgPPD, FechaIni, FechaFin, estatusPago);


            BLLFacturasCab bllFac = new BLL.BLLFacturasCab();
            BLLNotascreditoCab bllNc = new BLLNotascreditoCab();
            foreach (DataRow drRes in dtReservasPPD.Rows)
            {
                long bookingId = (long)drRes["BookingID"];
                decimal montoFacturadoReal = 0;
                decimal montoPagadoReal = 0;
                decimal montoTotalReserva = 0;
                bool conFacturaBase = false;
                string estatusFactura = "";

                List<ENTFacturasCab> listaFacturas = new List<ENTFacturasCab>();
                listaFacturas = bllFac.RecuperarFacturasCabBookingid(bookingId);


                conFacturaBase = listaFacturas.Where(x => x.TipoFacturacion == "FB").Count() > 0;
                montoFacturadoReal = listaFacturas.Where(x => x.TipoFacturacion == "FB").Sum(x => x.Total);
                montoPagadoReal = (decimal)drRes["MontoPagado"];
                montoTotalReserva = (decimal)drRes["MontoTotal"];


                if (conFacturaBase)
                {
                    estatusFactura = "FA";
                }
                else
                {
                    if (montoTotalReserva == 0)
                    {
                        estatusFactura = "NA";
                    }
                    else
                    {
                        estatusFactura = "PE";
                    }
                }


                if (estatusFacturacion.Length == 0 || estatusFacturacion == estatusFactura)
                {

                    string descEstatusFactura = ListaGenDescripciones.Where(x => x.CveTabla == "STAFAC" && x.CveValor == estatusFactura).FirstOrDefault().Descripcion;
                    ENTReservaPPD entRes = new ENTReservaPPD();
                    entRes.BookingID = bookingId;
                    entRes.PNR = drRes["PNR"].ToString();
                    entRes.BookingStatus = drRes["BookingStatus"].ToString();
                    entRes.CreatedOrganizationCode = drRes["CreatedOrganizationCode"].ToString();
                    entRes.Organizacion = drRes["Organizacion"].ToString();
                    entRes.NumPasajeros = (int)drRes["NumPasajeros"];
                    entRes.Moneda = drRes["Moneda"].ToString();
                    entRes.MontoTotal = montoTotalReserva;
                    entRes.PaidStatus = Convert.ToInt16(drRes["PaidStatus"].ToString());
                    entRes.EstatusPago = drRes["EstatusPago"].ToString();
                    entRes.FechaCreacion = (DateTime)drRes["FechaCreacion"];
                    entRes.FechaModificacion = (DateTime)drRes["FechaModificacion"];
                    entRes.MontoPagado = montoPagadoReal;
                    entRes.MontoFacturado = montoFacturadoReal;
                    entRes.EstatusFacturado = estatusFactura;
                    entRes.DescEstatusFacturado = descEstatusFactura;
                    entRes.EstaMarcadoParaFacturacion = false;
                    entRes.IdFacturaCab = 0;
                    result.Add(entRes);
                }
            }


            return result;
        }


        public Int64 GetFolio()
        {
            BLLFoliosfiscalesCnf folioFiscalBLL = new BLLFoliosfiscalesCnf();
            folioFiscalBLL.RecuperarFoliosfiscalesCnfPorLlavePrimaria(4);

            folioFiscalBLL.FolioActual++;
            folioFiscalBLL.Actualizar();

            return folioFiscalBLL.FolioActual;
        }

        //public List<ENTEmpresaCat> GetAllEmpresas()
        //{
        //    BLLEmpresaCat bllEmresasCat = new BLLEmpresaCat();
        //    return bllEmresasCat.RecuperarTodo();
        //}

        public void GeneraFacturaBase(ENTSolicitudesfacCab datosSolFactura)
        {

            try
            {
                List<ENTPagosPorFacturar> listaPagosPorFacturar = new List<ENTPagosPorFacturar>();

                List<ENTPagosFacturados> listaPagosDistribuidos = new List<ENTPagosFacturados>();

                //Se invoca la Distribucion de los cargos en la reserva
                listaPagosDistribuidos = DistribuirPagosEnReservacionPPD(datosSolFactura.PNR, false);

                //Se identifican los Pagos que se deben facturar
                foreach (ENTPagosFacturados pagoFac in listaPagosDistribuidos)
                {
                    if(pagoFac.PaymentMethodCode == "PPD" && !pagoFac.EsFacturado)
                    {
                        ENTPagosPorFacturar pagoPorFacturar = new ENTPagosPorFacturar();
                        //pagoPorFacturar = (ENTPagosPorFacturar)pagoFac;
                        listaPagosPorFacturar.Add(pagoPorFacturar);

                    }
                }




                ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();
                datosCliente.ApellidosPasajero = datosSolFactura.ApellidosPasajero;
                datosCliente.EmailReceptor = datosSolFactura.emailReceptor;
                datosCliente.EsExtranjero = datosSolFactura.EsExtranjero;
                datosCliente.NombrePasajero = datosSolFactura.NombrePasajero;
                datosCliente.TAXID = datosSolFactura.NumRegIdTrib_TAXID;
                datosCliente.ClaveReservacion = datosSolFactura.PNR;
                datosCliente.PaisResidenciaFiscal = datosSolFactura.ResidenciaFiscal;
                datosCliente.RFCReceptor = datosSolFactura.RFCReceptor;
                datosCliente.UsoCFDI = datosSolFactura.UsoCFDI;


                BLLFacturacion bllFacturacion = new BLLFacturacion();
                List<ENTPagosPorFacturar> listaPagosFacturados = new List<ENTPagosPorFacturar>();

                //listaPagosFacturados = bllFacturacion.GenerarFacturaCliente(datosCliente, listaPagos);

            }
            catch (ExceptionViva ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "lkbFacturar_Click");
                throw new Exception(mensajeUsuario);
            }

        }
        #region Procesos Pagos Complementarios
        public List<ENTPagosFacturados> DistribuirPagosEnReservacionPPD(string pnr, bool esPorProcesoDiario)
        {
            List<ENTPagosFacturados> listaPagos = new List<ENTPagosFacturados>();
            BLLDistribucionPagos bllDistribucion = new ProcesoFacturacion.BLLDistribucionPagos();

            pnr = pnr.ToUpper();
            PNR = pnr;
            try
            {

                //Verifica si ya estan asignados los catalogos generales, en caso de que no los recupera
                InicializaVariablesGlobales();

                //Recupera la informacion de la reservacion a partir del pnr

                DataSet dsReservacion = new DataSet();
                dsReservacion = bllDistribucion.RecuperarReservacionPorPNR(pnr);

                //Separa por DataTables la informacion recuperada de la reservacion desde Navitaire
                DataTable dtPagos = dsReservacion.Tables[0];
                DataTable dtPNRDiv = dsReservacion.Tables[1];
                DataTable dtFeeTar = dsReservacion.Tables[2];
                DataTable dtFeeSSR = dsReservacion.Tables[3];
                DataTable dtVuelosCab = dsReservacion.Tables[4];
                DataTable dtVuelosPorPasajero = dsReservacion.Tables[5];
                DataTable dtReservacionCab = dsReservacion.Tables[6];


                //Verifica si ya se proceso el PNR o si tuvo algun cambio
                bool conModificacion = false;
                conModificacion = bllDistribucion.ExisteCambioEnPNR(pnr, dtPNRDiv);

                //Si existe cambio entonces se continua con el proceso
                if (conModificacion)
                {
                    List<string> listaPNRDiv = new List<string>();
                    List<string> listaPNRDivProcesados = new List<string>();

                    listaPNRDivProcesados.Add(pnr);

                    foreach (DataRow drDivididos in dtPNRDiv.Rows)
                    {
                        string pnrDivIni = drDivididos["RecordLocator"].ToString();
                        //Se agrega condicion para los casos en que se detectaron vinculos entre pagos, pero que es el mismo pnr en proceso
                        if (!listaPNRDivProcesados.Contains(pnrDivIni))
                        {
                            if (!listaPNRDiv.Contains(pnrDivIni) && !listaPNRDivProcesados.Contains(pnrDivIni))
                            {
                                listaPNRDiv.Add(pnrDivIni);
                            }
                        }

                    }

                    //Recorre los pnr divididos mientras existan herencia de pnr sin procesar
                    while (listaPNRDiv.Count > 0)
                    {
                        //Obtiene la informacion del PNR
                        string pnrDiv = listaPNRDiv.Take(1).First();

                        DataSet dsReservacionDiv = new DataSet();
                        dsReservacionDiv = bllDistribucion.RecuperarReservacionPorPNR(pnrDiv);

                        DataTable dtPagosDiv = dsReservacionDiv.Tables[0];
                        DataTable dtPNRDivDiv = dsReservacionDiv.Tables[1];
                        DataTable dtFeeTarDiv = dsReservacionDiv.Tables[2];
                        DataTable dtFeeSSRDiv = dsReservacionDiv.Tables[3];
                        DataTable dtVuelosCabDiv = dsReservacionDiv.Tables[4];
                        DataTable dtVuelosPorPasajeroDiv = dsReservacionDiv.Tables[5];
                        DataTable dtReservacionCabDiv = dsReservacionDiv.Tables[6];

                        dtPagos.Merge(dtPagosDiv);
                        dtFeeTar.Merge(dtFeeTarDiv);
                        dtFeeSSR.Merge(dtFeeSSRDiv);
                        dtVuelosCab.Merge(dtVuelosCabDiv);
                        dtVuelosPorPasajero.Merge(dtVuelosPorPasajeroDiv);
                        dtReservacionCab.Merge(dtReservacionCabDiv);

                        listaPNRDivProcesados.Add(pnrDiv);
                        listaPNRDiv.Remove(pnrDiv);

                    }

                    //Se valida que la Reservacion cumpla con los criterios necesarios para continuar con el proceso
                    ValidarInformacionPNRPPD(pnr, dtReservacionCab, dtVuelosPorPasajero, dtPagos, dtFeeTar, dtFeeSSR);

                    //Analiza los pagos e identifica las caracteristicas de cada uno, si es facturable, si esta vinculado, etc
                    DateTime fechaPrimerCargo = new DateTime();
                    bool primerCargo = true;
                    foreach (DataRow drFeeFecha in dtFeeTar.Rows)
                    {
                        DateTime fechaCargo = Convert.ToDateTime(drFeeFecha["CreatedDate"]);
                        if (primerCargo)
                        {
                            fechaPrimerCargo = fechaCargo;
                            primerCargo = false;
                        }
                        else
                        {
                            if (fechaPrimerCargo > fechaCargo)
                            {
                                fechaPrimerCargo = fechaCargo;
                            }
                        }

                    }

                    foreach (DataRow drFeeFecha in dtFeeSSR.Rows)
                    {
                        DateTime fechaCargo = Convert.ToDateTime(drFeeFecha["CreatedDate"]);
                        if (fechaPrimerCargo > fechaCargo)
                        {
                            fechaPrimerCargo = fechaCargo;
                        }
                    }



                    listaPagos = GenerarListaPagosPPD(dtPagos, dtPNRDiv, fechaPrimerCargo);

                    //Genera los datos de la Reservacion
                    List<ENTReservacion> listaEntReserva = new List<ENTReservacion>();
                    listaEntReserva = GenerarReservacionPPD(dtReservacionCab, dtFeeTar, dtFeeSSR, dtVuelosCab, dtVuelosPorPasajero, ref listaPagos);

                    //Guarda la informacion de los Pagos en la BD.
                    GuardarReserva(ref listaEntReserva, ref listaPagos);

                    //Valida que la vinculacion de Componentes se realizo correctamente
                    foreach (ENTPagosFacturados pagoVin in listaPagos)
                    {
                        decimal montoPorAplicar = 0;
                        montoPorAplicar = pagoVin.MontoPorAplicar;
                        decimal acumuladoDetalle = 0;

                        foreach (ENTReservacion entReserva in listaEntReserva)
                        {
                            acumuladoDetalle += entReserva.ListaReservaDet.Where(x => x.IdPagosCab == pagoVin.IdPagosCab).Sum(x => x.ChargeAmount);
                        }
                        pagoVin.EsVinculacionCorrecta = (montoPorAplicar == acumuladoDetalle);
                    }
                }
            }
            catch (ExceptionViva ex)
            {
                if (esPorProcesoDiario)
                {
                    ListaErrores.Add(ex.Message);
                }
                else
                {
                    throw ex;
                }
            }
            catch (SqlException ex)
            {

                if (esPorProcesoDiario)
                {
                    ListaErrores.Add(ex.Message);
                }
                else
                {
                    //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    string mensajeUsuario = "";
                    if (TipoProceso == "ProcesoGlobal")
                    {
                        mensajeUsuario = ex.Message;
                    }
                    else
                    {
                        //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                        mensajeUsuario = MensajeErrorUsuario;
                    }

                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "BD", EnviarCorreoErrores);
                    throw new ExceptionViva(mensajeUsuario);
                }

            }
            catch (Exception ex)
            {
                if (esPorProcesoDiario)
                {
                    ListaErrores.Add(ex.Message);
                }
                else
                {
                    //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    string mensajeUsuario = "";
                    if (TipoProceso == "ProcesoGlobal")
                    {
                        mensajeUsuario = ex.Message;
                    }
                    else
                    {
                        //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                        mensajeUsuario = MensajeErrorUsuario;
                    }

                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "DistribuirPagosEnReservacion", EnviarCorreoErrores);
                    throw new ExceptionViva(mensajeUsuario);


                }
            }
            return listaPagos;
        }

        private bool ValidarInformacionPNRPPD(string pnr, DataTable dtReservacionCab, DataTable dtVuelosPorPasajero, DataTable dtPagos, DataTable dtFeeTar, DataTable dtFeeSSR)
        {
            bool result = false;
            try
            {
                //Verifica que el PNR exista en Navitaire
                if (dtReservacionCab.Rows.Count <= 0)
                {
                    throw new ExceptionViva(string.Format("La reservación con código {0} no existe, favor de verificar", pnr));
                }
                else
                {
                    //Verifica que el Estatus de la reservacion sea Confirmed, en caso contrario rechaza el PNR
                    DataRow drRes = dtReservacionCab.Rows[0];
                    int status = 0;
                    List<int> estatusValido = new List<int>();
                    estatusValido.Add(2);
                    estatusValido.Add(3);

                    status = Convert.ToInt16(drRes["Estatus"]);
                    if (!estatusValido.Contains(status))
                    {
                        throw new ExceptionViva(string.Format("La reservación {0} debe tener estatus de Confirmed o Closed...", pnr, status));
                    }
                }

                //*************************************************************************************************************
                //LCI. INI. 2018-04-11 AJUSTES PAGO SIN SEGMENTOS

                //Validando que la reservacion tenga al menos un pasajero asignado a un vuelo
                //Se elimina la validacion de que existan pasajeros debido a que algunos pagos cancelados deben ajustarse a importe 0 y ya no tienen pasajeros
                //Con esto se le proporciona informacion al proceso de Nota Credito para que las tome en cuenta
                //if (dtVuelosPorPasajero.Rows.Count <= 0)
                //{
                //    throw new ExceptionViva(string.Format("La reservacion {0} no tiene pasajeros asignados en ningún vuelo,", pnr));
                //}
                //*************************************************************************************************************
                //LCI. FIN. 2018-04-11 AJUSTES PAGO SIN SEGMENTOS


                //Verificando que todos los codigos de tarifa existan en el catalogo
                List<string> listaCodigosFeeSinCatalogar = new List<string>();

                //Se recorren todos los fees de Tarifa para validar la existencia de los codigos asignados
                foreach (DataRow drFeeTar in dtFeeTar.Rows)
                {

                    string codigoFee = "";
                    //Los Cargos donde el ChargeType sean igual a Cero aplican para las tarifas, por lo cual estas no se validan
                    int chargeType = 0;
                    chargeType = Convert.ToInt16(drFeeTar["ChargeType"]);
                    if (chargeType > 0)
                    {
                        //Si el cargo no es una tarifa entonces se verifica que el FeeCode exista en el catalogo de Facturacion
                        codigoFee = drFeeTar["ChargeCode"].ToString();
                        if (codigoFee.Length > 0 && ListaCatalogoFees.Where(x => x.FeeCode == codigoFee).Count() == 0)
                        {
                            decimal montoFee = Convert.ToDecimal(drFeeTar["ChargeAmount"].ToString());
                            //Si no existe el codigo en el catalogo entonces se genera una lista para reportarlo
                            if (!listaCodigosFeeSinCatalogar.Contains(codigoFee) && (codigoFee.Length < 6 && montoFee > 0))
                            {
                                //Se valida que no se repitan en la lista
                                listaCodigosFeeSinCatalogar.Add(codigoFee);
                            }
                        }
                    }
                }


                //Se recorren los codigos de servicios Adicionales por Pasajero
                foreach (DataRow drFeeSSR in dtFeeSSR.Rows)
                {
                    string codigoSSR = "";
                    //Se verifica si el codigo pertenece a una tarifa, en teoria aqui no deben existir las tarifas 
                    int chargeType = 0;
                    chargeType = Convert.ToInt16(drFeeSSR["ChargeType"]);
                    if (chargeType > 0)
                    {
                        //En cada Fee que no sea tarifa se verifica que ya exista en el catalogo de Facturacion
                        codigoSSR = drFeeSSR["ChargeCode"].ToString();
                        if (ListaCatalogoFees.Where(x => x.FeeCode == codigoSSR).Count() == 0)
                        {
                            //Si no existe en el catalogo entonces se registra en una lista para reportar el faltante
                            if (!listaCodigosFeeSinCatalogar.Contains(codigoSSR))
                            {
                                //Solo se insertan los codigos una vez, por lo que si ya existe no se agrega otra vez
                                listaCodigosFeeSinCatalogar.Add(codigoSSR);
                            }
                        }
                    }
                }

                //Al terminar de analizar los Codigos de los cargos de Tarifa y SSR se verifica si hay alguno que no este en el 
                //Catalogo de Facturacion de Fee´s
                if (listaCodigosFeeSinCatalogar.Count > 0)
                {
                    //En caso de existir al menos un codigo que no este en el catalogo de facturacion entonces, se arma una
                    //lista para reportarlo como faltante y provocar el error
                    StringBuilder listaCodigosFaltantes = new StringBuilder();
                    string sep = "";
                    foreach (string codigoFee in listaCodigosFeeSinCatalogar)
                    {
                        listaCodigosFaltantes.Append(sep + codigoFee);
                        sep = ", ";
                    }

                    //Se provoca el error indicando la lista de codigos faltantes
                    throw new Exception(string.Format("Los siguientes códigos Fee no se encuentran registrados en el catálogo: {0}", listaCodigosFaltantes.ToString()));
                }

                //Verificando que los codigos de pago se encuentren en el catalogo correspondiente
                List<string> listaFormaPagoSinRegistro = new List<string>();

                //Revisando los codigos de pago existentes
                foreach (DataRow drPago in dtPagos.Rows)
                {
                    string codigoPago = "";
                    codigoPago = drPago["PaymentMethodCode"].ToString();
                    //Se revisa que cada codigo de Pago existente en la reservacion se encuentre registrado en el catalogo de Pagos
                    //De la facturacion.
                    if (ListaCatalogoFormasPago.Where(x => x.PaymentMethodCode == codigoPago).Count() <= 0)
                    {
                        //Cada pago que no se encuentre catalogado se agregara a una lista para reportar que falta
                        if (!listaFormaPagoSinRegistro.Contains(codigoPago))
                        {
                            //Solo se agrega una vez el codigo de pago faltante
                            listaFormaPagoSinRegistro.Add(codigoPago);
                        }
                    }
                }


                //Si hace falta al menos un codigo de Pago entonces se reporta el faltante.
                if (listaFormaPagoSinRegistro.Count > 0)
                {
                    StringBuilder listaFormasPagoFaltantes = new StringBuilder();
                    string sep = "";
                    foreach (string codigoPago in listaFormaPagoSinRegistro)
                    {
                        listaFormasPagoFaltantes.Append(sep + codigoPago);
                        sep = ", ";
                    }

                    throw new Exception(string.Format("Las siguientes formas de pago no se encuentran registrados en el catalogo: {0}", listaFormasPagoFaltantes.ToString()));
                }
                result = true;
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = "";
                if (TipoProceso == "ProcesoGlobal")
                {
                    mensajeUsuario = ex.Message;
                }
                else
                {
                    //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    mensajeUsuario = MensajeErrorUsuario;
                }

                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "BD", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = "";
                if (TipoProceso == "ProcesoGlobal")
                {
                    mensajeUsuario = ex.Message;
                }
                else
                {
                    //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    mensajeUsuario = MensajeErrorUsuario;
                }
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "ValidarInformacionPNR", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }

            return result;
        }


        public List<ENTPagosFacturados> GenerarListaPagosPPD(DataTable dtPagos, DataTable dtPNRDiv, DateTime fechaPrimerCargo)
        {
            List<ENTPagosFacturados> listaPagos = new List<ENTPagosFacturados>();

            try
            {


                //Se ordenan los pagos por secuencia de Pago, en teoria se ordenan por cronologia
                dtPagos.DefaultView.Sort = "FechaPago";
                int secPago = 0;

                foreach (DataRow dr in dtPagos.DefaultView.ToTable().Rows)
                {
                    //Se crea la entidad de Pagos Facturados
                    ENTPagosFacturados entPago = new ENTPagosFacturados();
                    secPago++;

                    //Verifica si el pago ya se encuentra registrado en la BD
                    long paymentIdPago = 0;
                    paymentIdPago = Convert.ToInt64(dr["PaymentID"]);
                    BLLPagosCab bllPagoBD = new BLLPagosCab();
                    bllPagoBD.RecuperarPagosCabPaymentid(paymentIdPago);
                    bool pagoResgistrado = (bllPagoBD.IdPagosCab != 0);


                    if (bllPagoBD.IdPagosCab != 0)
                    {
                        //ENTPagosFacturados entPagoFac33 = new ENTPagosFacturados();
                        entPago = bllDistribucion.RecuperarPagoFacturado33(bllPagoBD);
                        //listaPagos.Add(entPagoFac33);
                    }




                    //else
                    //{

                    //Se asignan los valores a cada propiedad
                    entPago.IdPagosCab = (pagoResgistrado ? entPago.IdPagosCab : secPago);
                    if (!dr.IsNull("BookingID")) entPago.BookingID = Convert.ToInt64(dr["BookingId"]);
                    if (!dr.IsNull("PaymentID")) entPago.PaymentID = Convert.ToInt64(dr["PaymentID"]);
                    if (!dr.IsNull("FechaPago")) entPago.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
                    if (!dr.IsNull("FechaPagoUTC")) entPago.FechaPagoUTC = Convert.ToDateTime(dr["FechaPagoUTC"]);

                    //Recupera la informacion de la forma de pago a partir del PaymentMethodCode
                    if (!dr.IsNull("PaymentMethodCode"))
                    {
                        ENTFormapagoCat entFormaPago = ListaCatalogoFormasPago.Where(x => x.PaymentMethodCode == dr["PaymentMethodCode"].ToString()).FirstOrDefault();

                        if (entFormaPago != null)
                        {
                            entPago.ENTFormaPago = entFormaPago;
                            entPago.IdFormaPago = entPago.ENTFormaPago.IdFormaPago;
                            entPago.PaymentMethodCode = entPago.ENTFormaPago.PaymentMethodCode;

                        }
                    }

                    //Recupera los montos del pago
                    if (!dr.IsNull("CurrencyCode")) entPago.CurrencyCode = dr["CurrencyCode"].ToString();
                    if (!dr.IsNull("PaymentAmount")) entPago.PaymentAmount = Convert.ToDecimal(dr["PaymentAmount"]);
                    if (!dr.IsNull("CollectedCurrencyCode")) entPago.CollectedCurrencyCode = dr["CollectedCurrencyCode"].ToString();
                    if (!dr.IsNull("CollectedAmount")) entPago.CollectedAmount = Convert.ToDecimal(dr["CollectedAmount"]);


                    //Solo en caso de que no este registrado en BD entonces se calculara el tipo de cambio
                    if (!pagoResgistrado)
                    {
                        //Calcula el tipo de cambio en funcion del tipo de moneda que le corresponde al PNR (CurrencyCode) 
                        //y el tipo de moneda con que se realizo el pago (CollectedCurrencyCode)
                        if (entPago.CurrencyCode != entPago.CollectedCurrencyCode)
                        {
                            if (entPago.CollectedAmount >= entPago.PaymentAmount)
                            {
                                entPago.TipoCambio = entPago.CollectedAmount / entPago.PaymentAmount;
                            }
                            else
                            {
                                entPago.TipoCambio = entPago.PaymentAmount / entPago.CollectedAmount;
                            }
                        }
                        else
                        {
                            if (entPago.CurrencyCode == "USD")
                            {
                                //Solo en caso de que el tipo de moneda sea USD y que el pago sea con USD tambien entonces
                                // se Recupera el tipo de cambio del dia en que se realizo el pago para que no se asigne el 1
                                entPago.TipoCambio = bllDistribucion.RecuperarTipoCambioPorFecha(entPago.FechaPago);

                            }
                            else
                            {
                                entPago.TipoCambio = 1;
                            }
                        }
                    }
                    //Se recupera el estatus de pago
                    if (!dr.IsNull("Estatus")) entPago.Estatus = Convert.ToInt16(dr["Estatus"]);

                    //Recuperar la informacion del agente que creo el registro
                    if (!dr.IsNull("IdAgenteCreacion"))
                    {
                        long agentID = Convert.ToInt64(dr["IdAgenteCreacion"]);
                        entPago.IdAgente = bllDistribucion.RecuperarIdAgente(agentID);
                    }

                    //Verifica si el PNR esta vinculado con otras Reservaciones, 
                    //en cuyo caso se tiene que realizar el calculo de distribucion de esos PNR tambien

                    long parentPayment = 0;
                    if (!dr.IsNull("ParentPaymentId"))
                    {
                        parentPayment = Convert.ToInt64(dr["ParentPaymentId"]);
                        entPago.ParentPaymentID = parentPayment;
                    }

                    //LCI. INI. 2018-04-12
                    //Verifica si el pago padre esta en  mas de un booking para saber si es dividido o solo es un ajuste del Pago un reembolso

                    //if (dtPNRDiv.Select("PaymentId = " + entPago.PaymentID).Count() > 0)
                    //{
                    //    DataRow drDiv = dtPNRDiv.Select("PaymentId = " + entPago.PaymentID)[0];
                    //    entPago.EsPagoDividido = true;
                    //    entPago.EsPagoPadre = (bool)drDiv["EsPadre"];
                    //    entPago.EsPagoHijo = !(bool)drDiv["EsPadre"];
                    //}

                    if (dtPNRDiv.Select("PaymentId = " + entPago.PaymentID).Count() > 0)
                    {
                        bool esDividido = false;
                        bool esPadre = false;
                        bool esHijo = false;

                        DataRow drDiv = dtPNRDiv.Select("PaymentId = " + entPago.PaymentID)[0];
                        esPadre = (bool)drDiv["EsPadre"];
                        esHijo = !(bool)drDiv["EsPadre"];


                        List<long> listaBooking = new List<long>();
                        if (esPadre)
                        {
                            foreach (DataRow drPagDiv in dtPNRDiv.Select("ParentPaymentId = " + entPago.PaymentID))
                            {
                                long bookingIdPagDiv = Convert.ToInt64(drPagDiv["BookingId"]);
                                if (!listaBooking.Contains(bookingIdPagDiv))
                                {
                                    listaBooking.Add(bookingIdPagDiv);
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow drPagDiv in dtPNRDiv.Select("ParentPaymentId = " + entPago.ParentPaymentID))
                            {
                                long bookingIdPagDiv = Convert.ToInt64(drPagDiv["BookingId"]);
                                if (!listaBooking.Contains(bookingIdPagDiv))
                                {
                                    listaBooking.Add(bookingIdPagDiv);
                                }
                            }
                        }

                        //Solo en caso de que el pago se encuentre en mas de una reservacion se considerara como Dividido de otra forma solo es un pago vinculado
                        esDividido = (listaBooking.Count > 1);
                        entPago.EsPagoDividido = esDividido;
                        if (esDividido)
                        {
                            entPago.EsPagoPadre = esPadre;
                            entPago.EsPagoHijo = esHijo;
                        }
                        else
                        {
                            entPago.EsPagoPadre = false;
                            entPago.EsPagoHijo = false;
                        }
                    }


                    //LCI. FIN. 2018-04-12


                    //Calcular el monto que se va a aplicar para la distribucion de los componentes de la reservacion
                    decimal montoPorAplicar = 0;

                    if (
                        //Se identifican los pagos que no cuentan con un monto por vincular a los cargos
                        //Si el pago es de ajuste y tiene un importe negativo no se vinculara
                        (parentPayment > 0 && entPago.EsPagoDividido == false && entPago.PaymentAmount < 0)
                        ||
                        //Si el pago es dividido, es el hijo y el monto del pago es menor a cero, 
                        //es decir, que es el pago con que se hereda una porcion al PNR Hijo 
                        //entonces tampoco se toma en cuenta para vincular
                        (entPago.EsPagoDividido && entPago.EsPagoHijo && entPago.PaymentAmount < 0))
                    {
                        //Se asigna el monto por aplicar en Cero por lo que no se tomara en cuenta este pago 
                        //para distribuirlo entre los cargos
                        entPago.MontoPorAplicar = 0;
                    }
                    else
                    {

                        //Se asigna el monto del pago completo para vincularlo con los cargos
                        montoPorAplicar = entPago.PaymentAmount;

                        entPago.ListaDesglosePago = new List<ENTPagosDet>();

                        //Se registra el pago actual en el Detalle
                        //Esto se hace para justificar la cantidad del monto por aplicar en caso de pagos vinculados
                        ENTPagosDet pagoDet0 = new ENTPagosDet();
                        pagoDet0.IdPagosCab = entPago.IdPagosCab;
                        pagoDet0.PaymentID = entPago.PaymentID;
                        pagoDet0.BookingID = entPago.BookingID;
                        pagoDet0.FechaPago = entPago.FechaPago;
                        pagoDet0.PaymentMethodCode = entPago.PaymentMethodCode;
                        pagoDet0.CurrencyCode = entPago.CurrencyCode;
                        pagoDet0.PaymentAmount = entPago.PaymentAmount;
                        pagoDet0.CollectedCurrencyCode = entPago.CollectedCurrencyCode;
                        pagoDet0.CollectedAmount = entPago.CollectedAmount;
                        pagoDet0.IdAgente = entPago.IdAgente;

                        entPago.ListaDesglosePago.Add(pagoDet0);

                        string filtroDivididos = "";
                        filtroDivididos =  //Busca solo los que le corresponden al Booking en el que se encuentra el pago
                            "BookingId = " + entPago.BookingID.ToString() +
                            //Busca los pagos con una vinculacion directa esten o no divididos
                            " AND ((ParentPaymentId = " + entPago.PaymentID.ToString() +
                            //Busca los pagos que vienen de una division de PNR a multiples niveles
                            ") OR (EsDividido = true AND  ParentPaymentId > 0 "
                                  + (entPago.ParentPaymentID > 0 ? " AND  ParentPaymentId = " + entPago.ParentPaymentID.ToString() : " AND  ParentPaymentId = " + entPago.PaymentID.ToString()) + " AND PaymentId <> " + entPago.PaymentID.ToString() + "))";

                        //Se buscan los pagos que esten vinculados para ajustar el monto
                        foreach (var drPagoVin in dtPagos.Select(filtroDivididos))
                        {
                            decimal montoPagoVinculado = Convert.ToDecimal(drPagoVin["PaymentAmount"]);
                            //Solo se vinculan los pagos con importe negativo
                            if (montoPagoVinculado < 0 || (entPago.EsPagoDividido && montoPagoVinculado != 0))
                            {
                                //Se ajusta el monto del pago principal y que se ve afectado por el pago vinculado
                                montoPorAplicar += montoPagoVinculado;

                                //Se registra el detalle del pago vinculado para justificar la diferencia

                                ENTPagosDet pagoDet = new ENTPagosDet();
                                pagoDet.IdPagosCab = entPago.IdPagosCab;
                                pagoDet.PaymentID = Convert.ToInt64(drPagoVin["PaymentID"]);
                                pagoDet.BookingID = Convert.ToInt64(drPagoVin["BookingId"]);
                                pagoDet.FechaPago = Convert.ToDateTime(drPagoVin["FechaPago"]);
                                pagoDet.PaymentMethodCode = drPagoVin["PaymentMethodCode"].ToString();
                                pagoDet.CurrencyCode = drPagoVin["CurrencyCode"].ToString();
                                pagoDet.PaymentAmount = Convert.ToDecimal(drPagoVin["PaymentAmount"]);
                                pagoDet.CollectedCurrencyCode = drPagoVin["CollectedCurrencyCode"].ToString();
                                pagoDet.CollectedAmount = Convert.ToDecimal(drPagoVin["CollectedAmount"]);
                                long agentID = Convert.ToInt64(drPagoVin["IdAgenteCreacion"]);
                                pagoDet.IdAgente = bllDistribucion.RecuperarIdAgente(agentID);
                                entPago.ListaDesglosePago.Add(pagoDet);

                            }
                        }

                        //Al terminar de validar y buscar los pagos vinculados entonces se asigna el monto por aplicar final
                        entPago.MontoPorAplicar = montoPorAplicar;
                    }

                    //Determina si el pago se utilizara para vincular con algun componente de la reservacion
                    //esto solo pasa si el monto por aplicar es mayor a cero
                    entPago.EsParaAplicar = (montoPorAplicar > 0);

                    //Determina si el pago es facturable, en funcion del catalogo y del monto por aplicar
                    entPago.EsFacturable = (entPago.ENTFormaPago.EsFacturable  //Si en el catalogo de pagos tiene la propiedad facturable
                                            && montoPorAplicar > 0    //Si el monto por aplicar es mayor a cero
                                                                      //&& entPago.EsPagoHijo == false  //Si el pago no pertenece a un pago Hijo dividido
                                            );



                    //Se registra la fecha del ultimo analisis del Pago
                    if (!dr.IsNull("FechaModificacion")) entPago.FechaUltimaActualizacion = DateTime.Now;


                    //Se asigna el folio de factura que se utilizara para cada pago
                    long folioPreFac = entPago.PaymentID;
                    decimal montoPagoMayor = 0;

                    //Se recorren los pagos buscando la existencia de multipagos, y queda el que tenga mayor importe de pago
                    foreach (DataRow drMultipago in dtPagos.Select("BookingID = " + entPago.BookingID.ToString()))
                    {
                        DateTime fechaPagoMulti = Convert.ToDateTime(drMultipago["FechaPago"]);
                        decimal montoPago = Convert.ToDecimal(drMultipago["PaymentAmount"]);
                        long paymentIdMulti = Convert.ToInt64(drMultipago["PaymentId"]);

                        //LCI. INI. 2018-06-22 VALIDACION PARA MULTIPAGOS
                        //SE AGREGA VALIDACION DE LA FORMA DE PAGO EN CASO DE QUE NO SEA FACTURABLE SE EXCLUYE DE LA VALIDACION.
                        ENTFormapagoCat entFormaPagoMulti = ListaCatalogoFormasPago.Where(x => x.PaymentMethodCode == drMultipago["PaymentMethodCode"].ToString()).FirstOrDefault();

                        bool esFacturable = entFormaPagoMulti.EsFacturable;

                        //if (entPago.FechaPago == fechaPagoMulti && montoPagoMayor < montoPago)
                        if (entPago.FechaPago == fechaPagoMulti && montoPagoMayor < montoPago && esFacturable == true)
                        {
                            folioPreFac = paymentIdMulti;
                            montoPagoMayor = montoPago;
                        }
                        //LCI. FIN. 2018-06-22 VALIDACION PARA MULTIPAGOS
                    }


                    //Solo en caso de que el tipo de pago sea facturable entonces se asignara un folio de Prefactura
                    if (entPago.EsFacturable)// || entPago.EsPagoHijo)
                    {
                        //Se asigna el PaymentId del Pago que se utilizara para facturar
                        if (folioPreFac > 0 //Existe un folio de Pago mayor a cero
                            && montoPorAplicar > 0  //El monto por aplicar es Mayor a Cero
                            && (
                                    entPago.EsPagoDividido == false //No se trata de un pago dividido
                                    ||                              //O
                                    (entPago.EsPagoDividido && entPago.EsPagoPadre) // Si es un pago dividido pero se trata del pago Padre
                                )
                           )
                        {
                            entPago.FolioPrefactura = folioPreFac; //Se asigna el paymentId del pago
                        }
                        else if (folioPreFac > 0 && montoPorAplicar > 0 && entPago.EsPagoDividido && entPago.EsPagoHijo)
                        {
                            //En caso de que sea un pago dividido y sea el pago Hijo entonces se facturara con el padre
                            entPago.FolioPrefactura = entPago.ParentPaymentID;
                        }
                    }

                    else
                    {
                        entPago.FolioPrefactura = 0;
                    }

                    if (dr["FolioFactura"] != null && dr["FolioFactura"].ToString().Trim().Length > 0)
                    {
                        if (!dr.IsNull("FechaFacturaUTC")) entPago.FechaFacturaUTC = Convert.ToDateTime(dr["FechaFacturaUTC"]);
                        if (!dr.IsNull("FechaFactura")) entPago.FechaFactura = Convert.ToDateTime(dr["FechaFactura"]);
                        if (!dr.IsNull("FolioFactura")) entPago.FolioFactura = Convert.ToInt64(dr["FolioFactura"]);

                        //Se verifica si el pago ya se encuentra facturado en la version anterior
                        entPago.EsFacturado = false;
                        if (entPago.FolioFactura > 0)
                        {
                            entPago.EsFacturado = true;
                            entPago.VersionFacturacion = "3.2";
                            entPago.FolioPrefactura = folioPreFac;
                        }

                        //Se recupera la informacion del pago en caso de que se facturo en la version 3.2
                        if (!dr.IsNull("Tarifa")) entPago.MontoTarifa = Convert.ToDecimal(dr["Tarifa"]);
                        if (!dr.IsNull("ServiciosAdicionales")) entPago.MontoServAdic = Convert.ToDecimal(dr["ServiciosAdicionales"]);
                        if (!dr.IsNull("TUA")) entPago.MontoTUA = Convert.ToDecimal(dr["TUA"]);
                        if (!dr.IsNull("OtrosCargos")) entPago.MontoOtrosCargos = Convert.ToDecimal(dr["OtrosCargos"]);
                        if (!dr.IsNull("IVA")) entPago.MontoIVA = Convert.ToDecimal(dr["IVA"]);
                        if (!dr.IsNull("Total")) entPago.MontoTotal = Convert.ToDecimal(dr["Total"]);

                    }
                    //Se registra el pago analizado
                    listaPagos.Add(entPago);
                    //}
                }

                //Revisando si al momento de facturar en la version 3.2 se agruparon pagos

                decimal montosFacturadosAcum = 0;
                long prefolio = 0;
                long foliofactura = 0;
                DateTime fechaFactura = new DateTime();
                DateTime fechaFacturaUTC = new DateTime();
                string versionFactura = "";

                decimal montoTarifa = 0;
                decimal montoServAdic = 0;
                decimal montoTUA = 0;
                decimal montoOtrosCargos = 0;
                decimal montoIVA = 0;


                //Evalua solo los pagos divididos para vincularlos
                foreach (ENTPagosFacturados pagoFactDiv in listaPagos.Where(x => x.EsFacturado && x.EsPagoPadre))
                {
                    decimal montoPagoPorAplicar = 0;
                    //Se calculan los montos facturados en el pago
                    decimal montosFacturadosAcumDiv = 0;
                    montoPagoPorAplicar = pagoFactDiv.MontoPorAplicar;
                    montosFacturadosAcumDiv = pagoFactDiv.MontoTotal - montoPagoPorAplicar;

                    foreach (ENTPagosFacturados pagoFactDivHijos in listaPagos.Where(x => x.EsPagoHijo && x.FolioPrefactura == pagoFactDiv.PaymentID && x.MontoPorAplicar > 0))
                    {
                        if (montosFacturadosAcumDiv > 0)
                        {
                            pagoFactDivHijos.EsFacturado = pagoFactDiv.EsFacturado;
                            pagoFactDivHijos.FolioPrefactura = pagoFactDiv.FolioPrefactura;
                            pagoFactDivHijos.FolioFactura = pagoFactDiv.FolioFactura;
                            pagoFactDivHijos.FechaFactura = pagoFactDiv.FechaFactura;
                            pagoFactDivHijos.FechaFacturaUTC = pagoFactDiv.FechaFacturaUTC;
                            pagoFactDivHijos.VersionFacturacion = pagoFactDiv.VersionFacturacion;

                            pagoFactDivHijos.MontoTarifa = pagoFactDiv.MontoTarifa;
                            pagoFactDivHijos.MontoServAdic = pagoFactDiv.MontoServAdic;
                            pagoFactDivHijos.MontoTUA = pagoFactDiv.MontoTUA;
                            pagoFactDivHijos.MontoOtrosCargos = pagoFactDiv.MontoOtrosCargos;
                            pagoFactDivHijos.MontoIVA = pagoFactDiv.MontoIVA;

                            montosFacturadosAcumDiv -= pagoFactDivHijos.MontoPorAplicar;
                        }
                    }

                }



                //Evalua los pagos que no estan divididos
                foreach (ENTPagosFacturados pagoFact in listaPagos.Where(x => x.EsPagoDividido == false && x.EsFacturable && x.MontoPorAplicar > 0))
                {
                    decimal montoPagoPorAplicar = 0;
                    //Se calculan los montos facturados en el pago
                    decimal montosFacturados = 0;

                    montoPagoPorAplicar = pagoFact.MontoPorAplicar;

                    if (pagoFact.VersionFacturacion == "3.2")
                    {
                        montosFacturados += pagoFact.MontoTarifa;
                        montosFacturados += pagoFact.MontoServAdic;
                        montosFacturados += pagoFact.MontoTUA;
                        montosFacturados += pagoFact.MontoOtrosCargos;
                        montosFacturados += pagoFact.MontoIVA;

                        montoTarifa = pagoFact.MontoTarifa;
                        montoServAdic = pagoFact.MontoServAdic;
                        montoTUA = pagoFact.MontoTUA;
                        montoOtrosCargos = pagoFact.MontoOtrosCargos;
                        montoIVA = pagoFact.MontoIVA;
                    }



                    if (montosFacturados > 0 && montoPagoPorAplicar <= montosFacturados)
                    {
                        montosFacturadosAcum = montosFacturados - montoPagoPorAplicar;
                        prefolio = pagoFact.FolioPrefactura;
                        foliofactura = pagoFact.FolioFactura;
                        fechaFactura = pagoFact.FechaFactura;
                        fechaFacturaUTC = pagoFact.FechaFacturaUTC;
                        pagoFact.EsFacturado = true;
                        versionFactura = pagoFact.VersionFacturacion;
                    }
                    else if (montosFacturados == 0 && montosFacturadosAcum > 0)
                    {
                        pagoFact.EsFacturado = true;
                        pagoFact.FolioPrefactura = prefolio;
                        pagoFact.FolioFactura = foliofactura;
                        pagoFact.FechaFactura = fechaFactura;
                        pagoFact.FechaFacturaUTC = fechaFacturaUTC;

                        pagoFact.MontoTarifa = montoTarifa;
                        pagoFact.MontoServAdic = montoServAdic;
                        pagoFact.MontoTUA = montoTUA;
                        pagoFact.MontoOtrosCargos = montoOtrosCargos;
                        pagoFact.MontoIVA = montoIVA;

                        montosFacturadosAcum -= montoPagoPorAplicar;

                        pagoFact.VersionFacturacion = versionFactura;
                    }

                    if (montosFacturadosAcum == 0)
                    {
                        montoTarifa = 0;
                        montoServAdic = 0;
                        montoTUA = 0;
                        montoOtrosCargos = 0;
                        montoIVA = 0;
                    }
                }


                //Se recorren los pagos y se asignan las fechas limite por evento
                DateTime ultimaFecha = fechaPrimerCargo;
                foreach (ENTPagosFacturados pagosPorEvento in listaPagos.Where(x => x.MontoPorAplicar > 0).OrderBy(x => x.FechaPagoUTC))
                {
                    pagosPorEvento.FechaIniEvento = ultimaFecha;
                    pagosPorEvento.FechaFinEvento = pagosPorEvento.FechaPagoUTC;
                    ultimaFecha = pagosPorEvento.FechaPagoUTC.AddMilliseconds(1);
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = "";
                if (TipoProceso == "ProcesoGlobal")
                {
                    mensajeUsuario = ex.Message;
                }
                else
                {
                    //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    mensajeUsuario = MensajeErrorUsuario;
                }

                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "BD", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = "";
                if (TipoProceso == "ProcesoGlobal")
                {
                    mensajeUsuario = ex.Message;
                }
                else
                {
                    //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    mensajeUsuario = MensajeErrorUsuario;
                }
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "GenerarListaPagos", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            return listaPagos;
        }

        private List<ENTReservacion> GenerarReservacionPPD(DataTable dtReservacionCab, DataTable dtFeeTar, DataTable dtFeeSSR, DataTable dtVuelosCab, DataTable dtVuelosPorPasajero, ref List<ENTPagosFacturados> listaPagos)
        {
            List<ENTReservacion> listaResult = new List<ENTReservacion>();

            try
            {



                List<ENTReservaDet> listaUnirTarifasYSSRsFinal = new List<ENTReservaDet>();

                int contReserva = 0;
                dtReservacionCab.DefaultView.Sort = "BookingID";


                foreach (DataRow drReserva in dtReservacionCab.DefaultView.ToTable().Rows)
                {
                    //DataRow drReserva = dtReservacionCab.Rows[0];
                    //Verifica si ya existe la reservacion en la BD Facturacion
                    contReserva++;
                    long bookingId = 0;
                    ENTReservacion result = new ENTReservacion();

                    if (!drReserva.IsNull("BookingID")) bookingId = Convert.ToInt64(drReserva["BookingID"]);
                    ENTReservaCab reservaBD = new ENTReservaCab();
                    bool existeReservaBD = false;
                    if (bookingId > 0)
                    {
                        BLLReservaCab bllReservaC = new BLLReservaCab();
                        List<ENTReservaCab> listaReserva = new List<ENTReservaCab>();

                        listaReserva = bllReservaC.RecuperarReservaCabBookingid(bookingId);
                        if (listaReserva.Count > 0)
                        {
                            existeReservaBD = true;
                            reservaBD = listaReserva.First();

                        }
                    }



                    //Se registra la informacion del Cabecero de la Reservacion
                    result.IdReservaCab = existeReservaBD ? reservaBD.IdReservaCab : contReserva;
                    if (!drReserva.IsNull("IdEmpresa")) result.IdEmpresa = existeReservaBD ? reservaBD.IdEmpresa : Convert.ToByte(drReserva["IdEmpresa"]);
                    if (!drReserva.IsNull("BookingID")) result.BookingID = existeReservaBD ? reservaBD.BookingID : Convert.ToInt64(drReserva["BookingID"]);
                    if (!drReserva.IsNull("RecordLocator")) result.RecordLocator = existeReservaBD ? reservaBD.RecordLocator : drReserva["RecordLocator"].ToString();
                    if (!drReserva.IsNull("Estatus")) result.Estatus = Convert.ToInt16(drReserva["Estatus"]);
                    if (!drReserva.IsNull("NumJourneys")) result.NumJourneys = Convert.ToByte(drReserva["NumJourneys"]);
                    if (!drReserva.IsNull("CurrencyCode")) result.CurrencyCode = existeReservaBD ? reservaBD.CurrencyCode : drReserva["CurrencyCode"].ToString();
                    if (!drReserva.IsNull("OwningCarrierCode")) result.OwningCarrierCode = existeReservaBD ? reservaBD.OwningCarrierCode : drReserva["OwningCarrierCode"].ToString();
                    if (!drReserva.IsNull("CreatedAgentID")) result.CreatedAgentID = existeReservaBD ? reservaBD.CreatedAgentID : Convert.ToInt64(drReserva["CreatedAgentID"]);
                    if (!drReserva.IsNull("CreatedDate")) result.CreatedDate = existeReservaBD ? reservaBD.CreatedDate : Convert.ToDateTime(drReserva["CreatedDate"]);
                    if (!drReserva.IsNull("ModifiedAgentID")) result.ModifiedAgentID = Convert.ToInt64(drReserva["ModifiedAgentID"]);
                    if (!drReserva.IsNull("ModifiedDate")) result.ModifiedDate = Convert.ToDateTime(drReserva["ModifiedDate"]);
                    if (!drReserva.IsNull("ChannelTypeID")) result.ChannelTypeID = existeReservaBD ? reservaBD.ChannelTypeID : Convert.ToByte(drReserva["ChannelTypeID"]);
                    if (!drReserva.IsNull("CreatedOrganizationCode")) result.CreatedOrganizationCode = existeReservaBD ? reservaBD.CreatedOrganizationCode : drReserva["CreatedOrganizationCode"].ToString();
                    if (!drReserva.IsNull("MontoTotal")) result.MontoTotal = Convert.ToDecimal(drReserva["MontoTotal"]);
                    if (!drReserva.IsNull("MontoPagado")) result.MontoPagado = Convert.ToDecimal(drReserva["MontoPagado"]);
                    if (!drReserva.IsNull("MontoFacturado")) result.MontoFacturado = existeReservaBD ? reservaBD.MontoFacturado : Convert.ToDecimal(drReserva["MontoFacturado"]);
                    if (!drReserva.IsNull("FoliosFacturacion")) result.FoliosFacturacion = existeReservaBD ? reservaBD.FoliosFacturacion : drReserva["FoliosFacturacion"].ToString();
                    if (!drReserva.IsNull("EmailAddress")) result.FoliosFacturacion = existeReservaBD ? reservaBD.EmailAddress : drReserva["EmailAddress"].ToString();
                    result.FechaModificacion = DateTime.Now;

                    //Registra los vuelos
                    result.ListaVuelos = new List<ENTVuelosCab>();
                    //Invoca el Metodo que genera la lista de Vuelos   
                    List<ENTVuelosCab> listaVuelos = new List<ENTVuelosCab>();
                    listaVuelos = bllDistribucion.GeneraListaVuelos(dtVuelosCab);
                    result.ListaVuelos = listaVuelos;


                    //Genera el detalle de los componentes de la reservacion
                    //Se analizan y actualizan los datos de los Fees tanto de tarifas como de SSR's
                    List<ENTReservaDet> listaDetalleReservaTarifas = new List<ENTReservaDet>();
                    List<ENTReservaDet> listaDetalleReservaSSRs = new List<ENTReservaDet>();
                    List<ENTReservaDet> listaUnirTarifasYSSRs = new List<ENTReservaDet>();
                    List<ENTReservaDet> listaPagosVinculados = new List<ENTReservaDet>();
                    //Se analizan los cargos de Tarifa
                    listaDetalleReservaTarifas = bllDistribucion.GenerarListaDetalleReserva("TAR", result, dtFeeTar, dtFeeSSR, dtVuelosPorPasajero, listaVuelos, listaPagos);

                    //Se analizan los cargos de SSR
                    listaDetalleReservaSSRs = bllDistribucion.GenerarListaDetalleReserva("SSR", result, dtFeeTar, dtFeeSSR, dtVuelosPorPasajero, listaVuelos, listaPagos);

                    //Se unifican los componentes en una sola lista para integrarlos al Detalle y se ordenan en base a la prioridad de asignacion
                    listaUnirTarifasYSSRs = bllDistribucion.OrdenaListaCargosPorPrioridad(listaDetalleReservaTarifas, listaDetalleReservaSSRs);

                    //Antes de unificar la lista de detalles por tarifas o por SSR se verifica si los totales por cada concepto estan en negativo

                    decimal montoTarifasGlobal = 0m;
                    decimal montoSSRGlobal = 0m;

                    montoTarifasGlobal = listaDetalleReservaTarifas.Sum(x => x.ChargeAmount);
                    montoSSRGlobal = listaDetalleReservaSSRs.Sum(x => x.ChargeAmount);

                    //Solo en caso de que los servicios adicionales se conviertan en negativos entonces se buscan los posibles ajustes que provoquen el desbalance para
                    //reasignarlos como parte de la tarifa y equilibrar los importes
                    if (montoSSRGlobal < 0)
                    {
                        bllDistribucion.ReasignarAjustesNegativos(montoSSRGlobal, ref listaDetalleReservaTarifas, ref listaDetalleReservaSSRs);
                        //Se reasigna el orden para considerar el fee en las tarifas correspondientes
                        listaUnirTarifasYSSRs = bllDistribucion.OrdenaListaCargosPorPrioridad(listaDetalleReservaTarifas, listaDetalleReservaSSRs);
                    }

                    listaUnirTarifasYSSRsFinal.AddRange(listaUnirTarifasYSSRs);

                    listaResult.Add(result);

                }


                //Se corre el proceso para vincular el pago a cada componente
                //listaPagosVinculados = GeneraVinculoPagosComponentes(listaUnirTarifasYSSRs, ref listaPagos);

                //GeneraVinculoPagosComponentesFact33(ref listaUnirTarifasYSSRsFinal, ref listaPagos, listaResult);
                bllDistribucion.GeneraVinculoPagosPrincipal(ref listaUnirTarifasYSSRsFinal, ref listaPagos, listaResult);

                List<long> listaReservas = new List<long>();

                listaReservas = listaUnirTarifasYSSRsFinal.Select(x => x.IdReservaCab).Distinct().ToList();

                foreach (ENTReservacion entReserv in listaResult)
                {
                    entReserv.ListaReservaDet = new List<ENTReservaDet>();
                    entReserv.ListaReservaDet = listaUnirTarifasYSSRsFinal.Where(x => x.IdReservaCab == entReserv.IdReservaCab).OrderBy(x => x.Orden).ToList();

                }

                //SE ASIGNAN LOS MONTOS FACTURADOS

                foreach (ENTReservacion entReserv in listaResult)
                {
                    long idReservaCab = entReserv.IdReservaCab;
                    decimal montoFacturado = listaUnirTarifasYSSRsFinal.Where(x => x.IdReservaCab == idReservaCab && x.EstatusFacturacion == "FA").Sum(x => x.ChargeAmount);

                    StringBuilder listaFoliosFactura = new StringBuilder();
                    List<long> listaFoliosPrefactura = listaUnirTarifasYSSRsFinal.Where(x => x.IdReservaCab == idReservaCab && x.EstatusFacturacion == "FA").Select(x => x.FolioPreFactura).Distinct().ToList();
                    string sep = "";
                    foreach (long folioFactura in listaFoliosPrefactura)
                    {
                        listaFoliosFactura.Append(sep + folioFactura.ToString());
                        sep = ",";
                    }

                    entReserv.ListaReservaDet = new List<ENTReservaDet>();
                    entReserv.ListaReservaDet = listaUnirTarifasYSSRsFinal.Where(x => x.IdReservaCab == entReserv.IdReservaCab).OrderBy(x => x.Orden).ToList();
                    entReserv.MontoFacturado = montoFacturado;
                    entReserv.FoliosFacturacion = listaFoliosFactura.ToString();
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = "";
                if (TipoProceso == "ProcesoGlobal")
                {
                    mensajeUsuario = ex.Message;
                }
                else
                {
                    //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    mensajeUsuario = MensajeErrorUsuario;
                }

                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "BD", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = "";
                if (TipoProceso == "ProcesoGlobal")
                {
                    mensajeUsuario = ex.Message;
                }
                else
                {
                    //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    mensajeUsuario = MensajeErrorUsuario;
                }
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "GenerarReservacion", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }


            return listaResult;
        }

        private void GuardarReserva(ref List<ENTReservacion> ListaEntReserva, ref List<ENTPagosFacturados> listaPagos)
        {
            //ENTReservacion result = new ENTReservacion();

            try
            {

                //VALIDA LA INFORMACION PARA CHECAR QUE LA DISTRIBUCION SE REALIZO DE FORMA CORRECTA
                ValidaDistribucionPago(ListaEntReserva, listaPagos);

                /*INICIA EL PROCESO DE REGISTRO DE PAGOS*/
                //Se registra la informacion del Pago
                //Se obtienen los pagos registrados para cada PNR en la BD
                List<ENTPagosCab> listaPagosReg = new List<ENTPagosCab>();

                List<long> listaBooking = new List<long>();

                //LCI INI. 2018-04-11 PAGOS CON AJUSTES
                //Se cambia la forma de recuperar los pagos registrados en la BD, se utilizaba la lista de pagos en su lugar
                //se utilizara la lista de reservaciones

                //foreach (ENTPagosFacturados pagFac in listaPagos)
                //{
                //    long booking = pagFac.BookingID;
                //    if (!listaBooking.Contains(booking))
                //    {
                //        listaBooking.Add(booking);
                //        List<ENTPagosCab> listaPagosTmp = new List<ENTPagosCab>();
                //        BLLPagosCab bllPagos = new BLLPagosCab();
                //        listaPagosTmp = bllPagos.RecuperarPagosCabBookingid(booking);
                //        listaPagosReg.AddRange(listaPagosTmp);
                //    }
                //}
                foreach (ENTReservacion entReserva in ListaEntReserva)
                {
                    long booking = entReserva.BookingID;
                    if (!listaBooking.Contains(booking))
                    {
                        listaBooking.Add(booking);
                        List<ENTPagosCab> listaPagosTmp = new List<ENTPagosCab>();
                        BLLPagosCab bllPagos = new BLLPagosCab();
                        listaPagosTmp = bllPagos.RecuperarPagosCabBookingid(booking);
                        listaPagosReg.AddRange(listaPagosTmp);
                    }
                }



                //Se recorren los pagos recuperados del PNR desde Navitaire
                foreach (ENTPagosFacturados pago in listaPagos)
                {
                    //Se recupera la informacion del Pago en la BD
                    BLLPagosCab bllPago = new BLLPagosCab();
                    ENTPagosCab pagoBD = new ENTPagosCab();
                    pagoBD = listaPagosReg.Where(x => x.PaymentID == pago.PaymentID).FirstOrDefault();

                    //En caso de existir el pago para la reservacion entonces se actualiza
                    if (pagoBD != null)
                    {
                        bllPago.RecuperarPagosCabPorLlavePrimaria(pagoBD.IdPagosCab);
                    }
                    else
                    {
                        //Si no existe en BD se prepara para crearlo como nuevo
                        bllPago.IdPagosCab = pago.IdPagosCab;
                    }

                    //Se pasa la informacion del Pago
                    bllPago.BookingID = pago.BookingID;
                    bllPago.PaymentID = pago.PaymentID;
                    bllPago.FechaPago = pago.FechaPago;
                    bllPago.FechaPagoUTC = pago.FechaPagoUTC;
                    bllPago.IdFormaPago = pago.IdFormaPago;
                    bllPago.PaymentMethodCode = pago.PaymentMethodCode;
                    bllPago.CurrencyCode = pago.CurrencyCode;
                    bllPago.PaymentAmount = pago.PaymentAmount;
                    bllPago.MontoPorAplicar = pago.MontoPorAplicar;
                    bllPago.CollectedCurrencyCode = pago.CollectedCurrencyCode;
                    bllPago.CollectedAmount = pago.CollectedAmount;
                    bllPago.TipoCambio = pago.TipoCambio;
                    bllPago.Estatus = pago.Estatus;
                    bllPago.ParentPaymentID = pago.ParentPaymentID;
                    bllPago.EsPagoDividido = pago.EsPagoDividido;
                    bllPago.EsPagoPadre = pago.EsPagoPadre;
                    bllPago.EsPagoHijo = pago.EsPagoHijo;
                    bllPago.EsFacturable = pago.EsFacturable;
                    bllPago.EsParaAplicar = pago.EsParaAplicar;
                    bllPago.EsFacturado = pago.EsFacturado;
                    bllPago.IdFacturaCab = pago.IdFacturaCab;
                    bllPago.FolioPrefactura = pago.FolioPrefactura;
                    if (pago.FolioFactura > 0)
                    {
                        bllPago.FolioFactura = pago.FolioFactura;
                        bllPago.FechaFactura = pago.FechaFactura;
                        bllPago.FechaFacturaUTC = pago.FechaFacturaUTC;
                        bllPago.VersionFacturacion = pago.VersionFacturacion;
                    }
                    bllPago.MontoTarifa = pago.MontoTarifa;
                    bllPago.MontoServAdic = pago.MontoServAdic;
                    bllPago.MontoTUA = pago.MontoTUA;
                    bllPago.MontoOtrosCargos = pago.MontoOtrosCargos;
                    bllPago.MontoIVA = pago.MontoIVA;
                    bllPago.MontoTotal = pago.MontoTotal;
                    bllPago.IdAgente = pago.IdAgente;
                    bllPago.FechaHoraLocal = pago.FechaHoraLocal;
                    bllPago.FechaUltimaActualizacion = pago.FechaUltimaActualizacion;

                    if (pagoBD != null)
                    {
                        //Si ya existe en la BD se actualiza
                        bllPago.Actualizar();
                    }
                    else
                    {
                        //Si no existe el pago entonces se agrega a la BD
                        bllPago.Agregar();
                    }


                    long idPagoOriginal = pago.IdPagosCab;

                    //Se actualiza el idPago asignado en la BD
                    pago.IdPagosCab = bllPago.IdPagosCab;


                    //Se actualiza el idPago en el detalle de la reservacion con el que se vinculo

                    foreach (ENTReservacion entReserva in ListaEntReserva)
                    {
                        var detVinPago = entReserva.ListaReservaDet.Where(x => x.IdPagosCab == idPagoOriginal);
                        foreach (ENTReservaDet resDetProc in detVinPago)
                        {
                            resDetProc.IdPagosCab = pago.IdPagosCab;
                        }
                    }

                    //Se recorre el detalle de cada PAgo
                    BLLPagosDet bllPagDet = new BLLPagosDet();

                    //Se elimina la informacion del Detalle existente en la BD para ese pago
                    if (pagoBD != null)
                    {
                        List<ENTPagosDet> listaPagDet = new List<ENTPagosDet>();
                        listaPagDet = bllPagDet.RecuperarPagosDetIdpagoscab(pagoBD.IdPagosCab);

                        //Se elimina la informacion detallada del pago
                        foreach (ENTPagosDet pagD in listaPagDet)
                        {
                            BLLPagosDet bllPagDel = new BLLPagosDet();
                            bllPagDel.IdPagosCab = pagD.IdPagosCab;
                            bllPagDel.PaymentID = pagD.PaymentID;
                            bllPagDel.Eliminar(pagD.IdPagosCab, pagD.PaymentID);
                        }
                    }
                    if (pago.ListaDesglosePago != null && pago.ListaDesglosePago.Count > 0)
                    {
                        //Registrar el detalle de pago.
                        foreach (ENTPagosDet pagDet in pago.ListaDesglosePago)
                        {
                            BLLPagosDet bllPagoDet = new BLLPagosDet();
                            bllPagoDet.IdPagosCab = bllPago.IdPagosCab;
                            bllPagoDet.PaymentID = pagDet.PaymentID;
                            bllPagoDet.BookingID = pagDet.BookingID;
                            bllPagoDet.FechaPago = pagDet.FechaPago;
                            bllPagoDet.PaymentMethodCode = pagDet.PaymentMethodCode;
                            bllPagoDet.CurrencyCode = pagDet.CurrencyCode;
                            bllPagoDet.PaymentAmount = pagDet.PaymentAmount;
                            bllPagoDet.CollectedCurrencyCode = pagDet.CollectedCurrencyCode;
                            bllPagoDet.CollectedAmount = pagDet.CollectedAmount;
                            bllPagoDet.IdAgente = pagDet.IdAgente;
                            bllPagoDet.FechaHoraLocal = pagDet.FechaHoraLocal;
                            bllPagoDet.Agregar();
                        }
                    }

                    //Se elimina el pago de la lista de los registrados en BD
                    listaPagosReg.Remove(pagoBD);
                }


                //LCI. INI. 2018-04-11 AJUSTES DE PAGOS FACTURADOS EN GLOBAL O CLIENTE

                List<long> listaPagosPorEliminar = new List<long>();

                foreach (ENTPagosCab pago in listaPagosReg)
                {
                    //Se recupera la informacion del Pago en la BD
                    BLLPagosCab bllPago = new BLLPagosCab();
                    bllPago.RecuperarPagosCabPorLlavePrimaria(pago.IdPagosCab);
                    BLLGlobalticketsDet bllGlobal = new BLLGlobalticketsDet();

                    List<ENTGlobalticketsDet> listaPagoGlobal = new List<ENTGlobalticketsDet>();
                    listaPagoGlobal = bllGlobal.RecuperarGlobalticketsDetPaymentid(pago.PaymentID);

                    ENTGlobalticketsDet pagoEnGlobal = new ENTGlobalticketsDet();
                    pagoEnGlobal = listaPagoGlobal.FirstOrDefault();


                    if (bllPago.EsFacturado || (pagoEnGlobal != null && pagoEnGlobal.IdNotaCredito == 0))
                    {
                        bllPago.MontoPorAplicar = 0;
                        bllPago.MontoTarifa = 0;
                        bllPago.MontoServAdic = 0;
                        bllPago.MontoTUA = 0;
                        bllPago.MontoOtrosCargos = 0;
                        bllPago.MontoIVA = 0;
                        bllPago.MontoTotal = 0;
                        bllPago.Estatus = 4;
                        bllPago.EsFacturable = false;
                        bllPago.EsParaAplicar = false;
                        bllPago.FechaUltimaActualizacion = DateTime.Now;
                        bllPago.Actualizar();
                    }
                    else if (bllPago.EsFacturado == false && pagoEnGlobal == null)
                    {
                        //En caso de que no se encuentra facturado y que no se envio en una global entonces se elimina el pago
                        List<ENTPagosDet> listaPagDet = new List<ENTPagosDet>();
                        BLLPagosDet bllPagoDetElim = new BLLPagosDet();
                        listaPagDet = bllPagoDetElim.RecuperarPagosDetIdpagoscab(bllPago.IdPagosCab);

                        //Se elimina la informacion detallada del pago
                        foreach (ENTPagosDet pagD in listaPagDet)
                        {
                            BLLPagosDet bllPagDel = new BLLPagosDet();
                            bllPagDel.IdPagosCab = pagD.IdPagosCab;
                            bllPagDel.PaymentID = pagD.PaymentID;
                            bllPagDel.Eliminar(pagD.IdPagosCab, pagD.PaymentID);
                        }
                        listaPagosPorEliminar.Add(bllPago.IdPagosCab);
                        //bllPago.Eliminar(bllPago.IdPagosCab);
                    }
                }

                //LCI. INI. 2018-04-11 AJUSTES DE PAGOS FACTURADOS EN GLOBAL O CLIENTE

                /*FINALIZA EL REGISTRO DE PAGOS*/



                foreach (ENTReservacion entReserva in ListaEntReserva)
                {




                    //Se recupera la informacion de la reservacion en caso de existir
                    BLLReservaCab bllReservaCab = new BLLReservaCab();
                    bllReservaCab.RecuperarReservaCabBookingid(entReserva.BookingID);


                    bool exisResCab;

                    if (bllReservaCab.IdReservaCab == 0)
                    {
                        exisResCab = false;
                        bllReservaCab = new BLLReservaCab();
                    }
                    else
                    {
                        exisResCab = true;
                    }

                    //En caso de que exista la reservacion entonces solo se actualizaran los datos extraidos
                    bllReservaCab.IdEmpresa = entReserva.IdEmpresa;
                    bllReservaCab.BookingID = entReserva.BookingID;
                    bllReservaCab.RecordLocator = entReserva.RecordLocator;
                    bllReservaCab.Estatus = entReserva.Estatus;
                    bllReservaCab.NumJourneys = entReserva.NumJourneys;
                    bllReservaCab.CurrencyCode = entReserva.CurrencyCode;
                    bllReservaCab.OwningCarrierCode = entReserva.OwningCarrierCode;
                    bllReservaCab.CreatedAgentID = entReserva.CreatedAgentID;
                    bllReservaCab.CreatedDate = entReserva.CreatedDate;
                    bllReservaCab.ModifiedAgentID = entReserva.ModifiedAgentID;
                    bllReservaCab.ModifiedDate = entReserva.ModifiedDate;
                    bllReservaCab.ChannelTypeID = entReserva.ChannelTypeID;
                    bllReservaCab.CreatedOrganizationCode = entReserva.CreatedOrganizationCode;
                    bllReservaCab.MontoTotal = entReserva.MontoTotal;
                    bllReservaCab.MontoPagado = entReserva.MontoPagado;
                    bllReservaCab.MontoFacturado = entReserva.MontoFacturado;
                    bllReservaCab.FoliosFacturacion = entReserva.FoliosFacturacion;
                    bllReservaCab.FechaHoraLocal = entReserva.FechaHoraLocal;
                    bllReservaCab.FechaModificacion = entReserva.FechaModificacion;
                    bllReservaCab.EmailAddress = entReserva.EmailAddress;

                    //Verifica si el registro ya se encuentra en la BD, si ya existe lo actualiza,en caso contrario lo crea
                    if (exisResCab)
                    {
                        bllReservaCab.Actualizar();
                    }
                    else
                    {
                        bllReservaCab.Agregar();
                    }


                    //Se Actualiza la informacion del Cabecero
                    entReserva.IdReservaCab = bllReservaCab.IdReservaCab;

                    //Se guarda la informacion del Vuelo

                    //Se recorre la lista de Vuelos
                    foreach (ENTVuelosCab entVuelo in entReserva.ListaVuelos)
                    {
                        BLLVuelosCab bllVueloBD = new BLLVuelosCab();
                        bllVueloBD.RecuperarVuelosCabInventorylegid(entVuelo.InventoryLegId);
                        long idVueloOri = entVuelo.IdVuelo;

                        //En caso de que el vuelo no exista entonces lo registra en la BD
                        if (bllVueloBD.IdVuelo == 0)
                        {
                            bllVueloBD.IdVuelo = entVuelo.IdVuelo;
                            bllVueloBD.InventoryLegId = entVuelo.InventoryLegId;
                            bllVueloBD.InventoryLegKey = entVuelo.InventoryLegKey;
                            bllVueloBD.DepartureDate = entVuelo.DepartureDate;
                            bllVueloBD.CarrierCode = entVuelo.CarrierCode;
                            bllVueloBD.FlightNumber = entVuelo.FlightNumber;
                            bllVueloBD.DepartureStation = entVuelo.DepartureStation;
                            bllVueloBD.STD = entVuelo.STD;
                            bllVueloBD.DepartureTerminal = entVuelo.DepartureTerminal;
                            bllVueloBD.ArrivalStation = entVuelo.ArrivalStation;
                            bllVueloBD.STA = entVuelo.STA;
                            bllVueloBD.ArrivalTerminal = entVuelo.ArrivalTerminal;
                            bllVueloBD.CityPair = entVuelo.CityPair;
                            bllVueloBD.Agregar();

                        }

                        List<ENTVuelosCab> listaVuelos = bllVueloBD.RecuperarVuelosCabInventorylegid(entVuelo.InventoryLegId);

                        //Guarda el IdVuelo original para rastrearlo en el detalle y actualizarlo con el asignado por la BD
                        idVueloOri = entVuelo.IdVuelo;

                        int numVuelo = 0;
                        foreach (var entVueloReg in listaVuelos)
                        {
                            numVuelo++;
                            if (numVuelo == 1)
                            {
                                //Asigna el IdVuelo que tiene registrado en la BD.
                                entVuelo.IdVuelo = bllVueloBD.IdVuelo;
                            }
                            else
                            {
                                //Elimina los registros de vuelos que se hubieran repetido durante el proceso de registro
                                BLLVuelosCab bllVueloDup = new BLLVuelosCab();
                                bllVueloDup.Eliminar(entVueloReg.IdVuelo);
                            }
                        }



                        //Recorre el detalle de la reservacion y actualiza el Id del vuelo que le corresponde.
                        var listaDet = from x in entReserva.ListaReservaDet
                                       where x.IdVuelo == idVueloOri
                                       select x;
                        foreach (ENTReservaDet detVuelo in listaDet)
                        {
                            detVuelo.IdVuelo = bllVueloBD.IdVuelo;
                        }
                    }





                    /*INICIA EL REGISTRO DEL DETALLE DE LA RESERVACION*/

                    //Se recupera la informacion del detalle registrado en la BD para comparar.

                    List<ENTReservaDet> listaDetalleReservacion = new List<ENTReservaDet>();
                    BLLReservaDet bllResDet = new BLLReservaDet();

                    listaDetalleReservacion = bllResDet.RecuperarReservaDetIdreservacab(entReserva.IdReservaCab);


                    //Se genera el nuevo detalle de la reservacion
                    List<ENTReservaDet> listaDetProcesada = new List<ENTReservaDet>();
                    var listaDetalle = from x in entReserva.ListaReservaDet
                                       orderby x.Orden, x.ChargeNumber, x.IdPagosCab
                                       select x;

                    int idDetalle = 0;

                    foreach (ENTReservaDet entReservaDetalle in listaDetalle)
                    {
                        idDetalle++;
                        BLLReservaDet bllReserva = new BLLReservaDet()
                        {
                            IdReservaCab = bllReservaCab.IdReservaCab,
                            IdReservaDet = idDetalle,
                            Orden = entReservaDetalle.Orden,
                            FeeNumber = entReservaDetalle.FeeNumber,
                            PassengerID = entReservaDetalle.PassengerID,
                            SegmentID = entReservaDetalle.SegmentID,
                            ChargeNumber = entReservaDetalle.ChargeNumber,
                            ChargeType = entReservaDetalle.ChargeType,
                            IdFee = entReservaDetalle.IdFee,
                            ChargeCode = entReservaDetalle.ChargeCode,
                            ChargeDetail = (entReservaDetalle.ChargeDetail != null ? entReservaDetalle.ChargeDetail : ""),
                            TicketCode = entReservaDetalle.TicketCode,
                            CurrencyCode = entReservaDetalle.CurrencyCode,
                            ChargeAmount = entReservaDetalle.ChargeAmount,
                            ForeignCurrencyCode = entReservaDetalle.ForeignCurrencyCode,
                            ForeignAmount = entReservaDetalle.ForeignAmount,
                            FechaAplicaCompra = entReservaDetalle.FechaAplicaCompra,
                            PorcIva = entReservaDetalle.PorcIva,
                            TipoCargo = entReservaDetalle.TipoCargo,
                            TipoAcumulado = entReservaDetalle.TipoAcumulado,
                            IdPagosCab = entReservaDetalle.IdPagosCab,
                            EsPagoParcial = entReservaDetalle.EsPagoParcial,
                            MontoPagado = entReservaDetalle.MontoPagado,
                            EsFacturable = entReservaDetalle.EsFacturable,
                            EstatusFacturacion = (entReservaDetalle.EstatusFacturacion != null ? entReservaDetalle.EstatusFacturacion : "NF"),
                            IdFacturaCab = entReservaDetalle.IdFacturaCab,
                            FolioPreFactura = entReservaDetalle.FolioPreFactura,
                            ClasFact = entReservaDetalle.ClasFact,
                            IdConcepto = entReservaDetalle.IdConcepto,
                            IdFolioFacturaGlobal = entReservaDetalle.IdFolioFacturaGlobal,
                            IdVuelo = entReservaDetalle.IdVuelo,
                            NumJourney = entReservaDetalle.NumJourney,
                            LiftStatus = (entReservaDetalle.LiftStatus != null ? entReservaDetalle.LiftStatus : ""),
                            CreatedAgentID = entReservaDetalle.CreatedAgentID,
                            CreatedDate = entReservaDetalle.CreatedDate,
                            FechaHoraLocal = entReservaDetalle.FechaHoraLocal
                        };

                        listaDetProcesada.Add(bllReserva);
                        //bllReserva.Agregar();

                        entReservaDetalle.IdReservaCab = bllReserva.IdReservaCab;
                        entReservaDetalle.IdReservaDet = bllReserva.IdReservaDet;
                        entReservaDetalle.FechaHoraLocal = bllReserva.FechaHoraLocal;

                    }

                    //Se compara cada fila detalle del nuevo proceso
                    bool existenDiferencias = false;
                    if (listaDetalleReservacion.Count != listaDetProcesada.Count)
                    {
                        existenDiferencias = true;
                    }
                    else
                    {
                        foreach (BLLReservaDet detProc in listaDetProcesada)
                        {
                            long idResDet = detProc.IdReservaDet;

                            ENTReservaDet detBD = new BLLReservaDet();
                            detBD = listaDetalleReservacion.Where(x => x.IdReservaDet == idResDet).FirstOrDefault();

                            if (detBD != null)
                            {
                                existenDiferencias = bllDistribucion.ExisteDiferenciaEntreBDVsProc(detBD, detProc);
                                if (existenDiferencias)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                existenDiferencias = true;
                                break;
                            }
                        }
                    }



                    if (existenDiferencias)
                    {

                        int numFilaHis = 0;
                        int numHist = 0;
                        //En caso de existir diferencias entre los registros del detalle entonces se creara el Historico
                        foreach (ENTReservaDet resBDAct in listaDetalleReservacion)
                        {

                            numFilaHis++;

                            //Se recupera el contador de Historicos guardados del detalle de reservacion
                            if (numFilaHis == 1)
                            {
                                BLLReservaHis bllHistDet = new BLL.BLLReservaHis();
                                List<ENTReservaHis> listaHist = new List<ENTReservaHis>();
                                listaHist = bllHistDet.RecuperarReservaHisIdreservacabIdreservadet(resBDAct.IdReservaCab, resBDAct.IdReservaDet);


                                if (listaHist.Count == 0)
                                {
                                    numHist = 1;
                                }
                                else
                                {
                                    numHist = listaHist.Max(x => x.NumHistorico) + 1;
                                }
                            }


                            long inventoryLeg = 0;

                            ENTVuelosCab vueloLeg = entReserva.ListaVuelos.Where(x => x.IdVuelo == resBDAct.IdVuelo).FirstOrDefault();

                            inventoryLeg = vueloLeg != null ? inventoryLeg : 0;

                            //Se genera la fila del historico en la BD
                            BLLReservaHis bllResHis = new BLLReservaHis()
                            {
                                IdReservaCab = resBDAct.IdReservaCab,
                                IdReservaDet = resBDAct.IdReservaDet,
                                NumHistorico = numHist,
                                FechaHoraHistorico = DateTime.Now,
                                Orden = resBDAct.Orden,
                                FeeNumber = resBDAct.FeeNumber,
                                PassengerID = resBDAct.PassengerID,
                                SegmentID = resBDAct.SegmentID,
                                ChargeNumber = resBDAct.ChargeNumber,
                                ChargeType = resBDAct.ChargeType,
                                IdFee = resBDAct.IdFee,
                                ChargeCode = resBDAct.ChargeCode,
                                ChargeDetail = resBDAct.ChargeDetail,
                                TicketCode = resBDAct.TicketCode,
                                CurrencyCode = resBDAct.CurrencyCode,
                                ChargeAmount = resBDAct.ChargeAmount,
                                ForeignCurrencyCode = resBDAct.ForeignCurrencyCode,
                                ForeignAmount = resBDAct.ForeignAmount,
                                FechaAplicaCompra = resBDAct.FechaAplicaCompra,
                                PorcIva = resBDAct.PorcIva,
                                TipoCargo = resBDAct.TipoCargo,
                                TipoAcumulado = resBDAct.TipoAcumulado,
                                IdPagosCab = resBDAct.IdPagosCab,
                                EsPagoParcial = resBDAct.EsPagoParcial,
                                MontoPagado = resBDAct.MontoPagado,
                                EsFacturable = resBDAct.EsFacturable,
                                EstatusFacturacion = resBDAct.EstatusFacturacion,
                                IdFacturaCab = resBDAct.IdFacturaCab,
                                FolioPreFactura = resBDAct.FolioPreFactura,
                                ClasFact = resBDAct.ClasFact,
                                IdConcepto = resBDAct.IdConcepto,
                                IdFolioFacturaGlobal = resBDAct.IdFolioFacturaGlobal,
                                IdVuelo = resBDAct.IdVuelo,
                                NumJourney = resBDAct.NumJourney,
                                LiftStatus = resBDAct.LiftStatus,
                                CreatedAgentID = resBDAct.CreatedAgentID,
                                CreatedDate = resBDAct.CreatedDate,
                                FechaHoraLocal = resBDAct.FechaHoraLocal
                            };
                            bllResHis.Agregar();

                            //Se elimina el registro de la BD actual en el detalle
                            BLLReservaDet bllResDetElim = new BLLReservaDet();
                            bllResDetElim.Eliminar(resBDAct.IdReservaCab, resBDAct.IdReservaDet);
                        }

                        //Se crean los nuevos registros del detalle en la BD
                        foreach (BLLReservaDet reservaDetNueva in listaDetProcesada)
                        {
                            reservaDetNueva.Agregar();
                        }
                    }
                }


                //LCI. INI 2018-07-06, SE REVISA SI EXISTEN PAGOS POR ELIMINAR Y SE HACE HASTA ESTE PUNTO POR LA RELACION CON EL DETALLE DE LA RESERVA
                foreach (long idPagoElim in listaPagosPorEliminar)
                {
                    BLLPagosCab bllPago = new BLLPagosCab();
                    bllPago.Eliminar(idPagoElim);
                }

                //LCI. FIN 2018-07-06


            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = "";
                if (TipoProceso == "ProcesoGlobal")
                {
                    mensajeUsuario = ex.Message;
                }
                else
                {
                    //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    mensajeUsuario = MensajeErrorUsuario;
                }

                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "BD", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = "";
                if (TipoProceso == "ProcesoGlobal")
                {
                    mensajeUsuario = ex.Message;
                }
                else
                {
                    //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    mensajeUsuario = MensajeErrorUsuario;
                }
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "GuardarReserva", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
        }
        private void ValidaDistribucionPago(List<ENTReservacion> listaEntReserva, List<ENTPagosFacturados> listaPagos)
        {
            try
            {
                //Unifica el detalle de las reservaciones
                List<ENTReservaDet> listaDetalleReserva = new List<ENTReservaDet>();
                StringBuilder pnrs = new StringBuilder();
                string sep = "";
                foreach (ENTReservacion reserva in listaEntReserva)
                {
                    pnrs.Append(sep + reserva.RecordLocator);
                    listaDetalleReserva.AddRange(reserva.ListaReservaDet);
                    sep = ",";
                }


                foreach (ENTPagosFacturados pago in listaPagos)
                {
                    decimal pagoPorAplicar = pago.MontoPorAplicar;

                    List<byte> porcAplicados = new List<byte>();
                    porcAplicados = listaDetalleReserva.Where(x => !ListaPorcIVAValidos.Contains(x.PorcIva)).Select(x => x.PorcIva).Distinct().ToList();

                    StringBuilder listaIVASInvalidos = new StringBuilder();
                    string sepIVA = "";
                    foreach (byte porIVAApli in porcAplicados)
                    {
                        listaIVASInvalidos.Append(sepIVA + porIVAApli.ToString());
                        sepIVA = ",";
                    }


                    if (porcAplicados.Count > 0)
                    {
                        throw new Exception(string.Format("Error: Se detectaron Porcentajes de IVA invalidos que fueron aplicados: PNR:{0}, PaymentId: {1}, MontoPorAplicar: {2}, Porcentajes Invalidos: {3}"
                            , pnrs, pago.PaymentID.ToString(), pagoPorAplicar.ToString(), listaIVASInvalidos.ToString()));
                    }
                }


                foreach (ENTPagosFacturados pago in listaPagos)
                {
                    decimal pagoPorAplicar = pago.MontoPorAplicar;
                    decimal montoAsignadoDet = 0;
                    montoAsignadoDet = listaDetalleReserva.Where(x => x.IdPagosCab == pago.IdPagosCab).Sum(x => x.ChargeAmount);
                    if (montoAsignadoDet != pagoPorAplicar)
                    {
                        throw new Exception(string.Format("Error: Diferencia entre el importe del pago y el detalle de Reserva Asignado: PNR:{0}, PaymentId: {1}, MontoPorAplicar: {2}, SumDetalleReserva: {3}"
                            , pnrs, pago.PaymentID.ToString(), pagoPorAplicar.ToString(), montoAsignadoDet.ToString()));
                    }
                }


                //LCI. INI. 2018-07-11 SE AGREGA VALIDACION DE IMPORTE IVA POR TASA EN CADA PAGO PARA PREVENIR IVAS CON IMPORTE NEGATIVO
                foreach (ENTPagosFacturados pago in listaPagos)
                {

                    List<byte> porcAplicados = new List<byte>();
                    porcAplicados = listaDetalleReserva.Select(x => x.PorcIva).Distinct().ToList();

                    foreach (byte tasaIVA in porcAplicados)
                    {
                        decimal acumuladoIVA = 0;
                        acumuladoIVA = listaDetalleReserva.Where(x => x.IdPagosCab == pago.IdPagosCab && x.TipoAcumulado == "IVA" && x.PorcIva == tasaIVA).Sum(x => x.ChargeAmount);

                        if (acumuladoIVA < 0)
                        {
                            throw new Exception(string.Format("Error: IVA con monto negativo: PNR:{0}, PaymentId: {1}, TasaIVA: {2}, Monto IVA: {3}"
                           , pnrs, pago.PaymentID.ToString(), tasaIVA.ToString(), acumuladoIVA.ToString()));
                        }
                    }
                }

                //LCI. FIN. 2018-07-11 


                //LCI. INI. 2018-07-17 SE AGREGA VALIDACION DE IMPORTES NEGATIVOS EN EL PROCESO DE DISTRIBUCION POR CADA BLOQUE DE CONCEPTOS
                foreach (ENTPagosFacturados pago in listaPagos)
                {

                    decimal montoTarifa = 0;
                    decimal montoServAdic = 0;
                    decimal montoTUA = 0;
                    decimal montoOtrosCargos = 0;
                    decimal montoIVA = 0;
                    decimal montoTotal = 0;

                    montoTarifa = pago.MontoTarifa;
                    montoServAdic = pago.MontoServAdic;
                    montoTUA = pago.MontoTUA;
                    montoOtrosCargos = pago.MontoOtrosCargos;
                    montoIVA = pago.MontoIVA;
                    montoTotal = pago.MontoTotal;

                    StringBuilder mensajeError = new StringBuilder();
                    string sepError = "";

                    if (montoTarifa < 0)
                    {
                        mensajeError.Append(sepError + string.Format("montoTarifa negativo: {0}", montoTarifa.ToString()));
                        sepError = ", ";
                    }

                    if (montoServAdic < 0)
                    {
                        mensajeError.Append(sepError + string.Format("montoServAdic negativo: {0}", montoServAdic.ToString()));
                        sepError = ", ";
                    }

                    if (montoTUA < 0)
                    {
                        mensajeError.Append(sepError + string.Format("montoTUA negativo: {0}", montoTUA.ToString()));
                        sepError = ", ";
                    }

                    if (montoOtrosCargos < 0)
                    {
                        mensajeError.Append(sepError + string.Format("montoOtrosCargos negativo: {0}", montoOtrosCargos.ToString()));
                        sepError = ", ";
                    }

                    if (montoIVA < 0)
                    {
                        mensajeError.Append(sepError + string.Format("montoIVA negativo: {0}", montoIVA.ToString()));
                        sepError = ", ";
                    }

                    if (montoTotal < 0)
                    {
                        mensajeError.Append(sepError + string.Format("montoTotal negativo: {0}", montoTotal.ToString()));
                        sepError = ", ";
                    }

                    if (mensajeError.ToString().Length > 0)
                    {
                        throw new Exception(string.Format("Error: Acumulados con importes Negativos en el pago. PNR:{0}, PaymentId: {1}, Detalle: {2}"
                       , pnrs, pago.PaymentID.ToString(), mensajeError.ToString()));
                    }
                }

                //LCI. FIN. 2018-07-17 





            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = "";
                if (TipoProceso == "ProcesoGlobal")
                {
                    mensajeUsuario = ex.Message;
                }
                else
                {
                    //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    mensajeUsuario = MensajeErrorUsuario;
                }

                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "BD", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = "";
                if (TipoProceso == "ProcesoGlobal")
                {
                    mensajeUsuario = ex.Message;
                }
                else
                {
                    //mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                    mensajeUsuario = MensajeErrorUsuario;
                }
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "ValidaDistribucionPago", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
        }


        #endregion
    }
}

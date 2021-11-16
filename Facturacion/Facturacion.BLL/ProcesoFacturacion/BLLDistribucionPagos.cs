using Facturacion.DAL.ProcesoFacturacion;
using Facturacion.ENT;
using Facturacion.ENT.ProcesoFacturacion;
using Comun;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBFactPaquetes.Model;
using Newtonsoft.Json;

namespace Facturacion.BLL.ProcesoFacturacion
{

    public class BLLDistribucionPagos : DALDistribucionPagos
    {


        #region Constructor

        public BLLDistribucionPagos()
        : base(BLLConfiguracion.ConexionNavitaireWB)
        {
            EnviarCorreoErrores = true;
            TipoProceso = "";
        }

        #endregion Constructor



        #region Propiedades Publicas ListasCatalogos
        //Se crean las propiedades publicas para recuperar y trabajar con los catalogos de la aplicacion
        public List<string> ListaCodigosTUA = new List<string>();
        public List<ENTFeeCat> ListaCatalogoFees = new List<ENTFeeCat>();
        public List<ENTFormapagoCat> ListaCatalogoFormasPago = new List<ENTFormapagoCat>();
        public List<ENTConceptosCat> ListaCatalogoConceptos = new List<ENTConceptosCat>();
        public List<ENTAgentesCat> ListaCatalogoAgentes = new List<ENTAgentesCat>();
        public List<ENTEmpresaCat> ListaCatalogoEmpresas = new List<ENTEmpresaCat>();
        public List<ENT.ENTGendescripcionesCat> ListaGenDescripciones = new List<ENT.ENTGendescripcionesCat>();
        public List<ENTParametrosCnf> ListaParametros = new List<ENTParametrosCnf>();
        public List<ENTLocationCat> ListaLocaciones = new List<ENTLocationCat>(); //LCI. INI. 20181121 LugarExpedicion
        public string LugarExpedicionDefault { get; set; }//LCI. INI. 20181121 LugarExpedicion
        public List<string> ListaErrores = new List<string>();
        public List<string> ListaCodigosAjuste = new List<string>();
        public List<decimal> ListaPorcIVAValidos = new List<decimal>();
        public List<decimal> ListaPorcIVAAplicados = new List<decimal>();
        public BLLBitacoraErrores BllLogErrores = new BLLBitacoraErrores();
        public bool EnviarCorreoErrores { get; set; }
        public string TipoProceso { get; set; }
        public string PNR { get; set; }

        public string MensajeErrorUsuario { get; set; }

        public List<ENTIvaCat> ListaIVACat = new List<ENTIvaCat>();
        public List<ENTReasignaexpedporiata> ListaReasignaExpPorIATA = new List<ENTReasignaexpedporiata>();
        public bool EsReservaFrontera { get; set; }
        public string LugarExpedicionFrontera { get; set; }//LCI. INI. 20181121 LugarExpedicion
        #endregion ListasCatalogos


        /// <summary>
        /// Metodo que recupera la informacion de los catalogos de sistema y asigna valor a las variables principales
        /// </summary>
        private void InicializaVariablesGlobales()
        {
            try
            {

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

                //Iniciar Catalogo Conceptos de Facturacion
                if (ListaCatalogoConceptos.Count() == 0)
                {
                    BLLConceptosCat bllConceptosFac = new BLLConceptosCat();
                    ListaCatalogoConceptos = bllConceptosFac.RecuperarTodo();
                }

                //Inicializa el catalogo de Agentes
                if (ListaCatalogoAgentes.Count() == 0)
                {
                    BLLAgentesCat bllCatalogoAgentes = new BLLAgentesCat();
                    ListaCatalogoAgentes = bllCatalogoAgentes.RecuperarTodo();
                }

                //Inicializa el catalogo de Empresas
                if (ListaCatalogoEmpresas.Count() == 0)
                {
                    BLLEmpresaCat bllCatEmpresa = new BLLEmpresaCat();
                    ListaCatalogoEmpresas = bllCatEmpresa.RecuperarTodo();
                }

                //Inicializa el catalogo de Descripciones Generales que incluye los catalogos SAT
                if (ListaGenDescripciones.Count() == 0)
                {
                    BLLGendescripcionesCat bllDescripciones = new BLLGendescripcionesCat();
                    ListaGenDescripciones = bllDescripciones.RecuperarTodo();
                }

                //Inicializa la lista de Parametros de la aplicacion
                if (ListaParametros.Count() == 0)
                {
                    BLLParametrosCnf bllParam = new BLL.BLLParametrosCnf();
                    ListaParametros = bllParam.RecuperarTodo();
                }

                //Genera una lista de los codigos que corresponden al TUA, este bloque debe cambiarse a un nivel superior
                string paramCodigosTUA = ListaParametros.Where(x => x.Nombre == "CodigosTUA").FirstOrDefault().Valor;

                if (paramCodigosTUA.Length > 0)
                {
                    string[] listCodTua = paramCodigosTUA.Split(',');
                    foreach (string codTUA in listCodTua)
                    {
                        ListaCodigosTUA.Add(codTUA);
                    }
                }


                //Genera una lista de los codigos que corresponden al TUA, este bloque debe cambiarse a un nivel superior
                string paramCodigosAjuste = ListaParametros.Where(x => x.Nombre == "CodigosAjuste").FirstOrDefault().Valor;

                if (paramCodigosAjuste.Length > 0)
                {
                    string[] listCodAjuste = paramCodigosAjuste.Split(',');
                    foreach (string codAjuste in listCodAjuste)
                    {
                        ListaCodigosAjuste.Add(codAjuste);
                    }
                }



                //LCI. INI. 20190222 IVA FRONTERA
                if (ListaIVACat.Count() == 0)
                {
                    BLLIvaCat bllIVACat = new BLLIvaCat();
                    ListaIVACat = bllIVACat.RecuperarTodo();

                    if (ListaIVACat.Count() > 0)
                    {
                        foreach (ENTIvaCat ivaItem in ListaIVACat)
                        {
                            if (ivaItem.Activo == true)
                            {
                                ListaPorcIVAValidos.Add(ivaItem.PorcIVA);
                            }

                        }
                    }

                }

                if (ListaReasignaExpPorIATA.Count() == 0)
                {
                    BLLReasignaexpedporiata bllReasigna = new BLLReasignaexpedporiata();
                    ListaReasignaExpPorIATA = bllReasigna.RecuperarTodo();

                }



                //LCI. FIN. 20190222 IVA FRONTERA

                if (ListaPorcIVAValidos.Count() == 0)
                {
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


                if (ListaParametros.Where(x => x.Nombre == "MsjErrorPortal").Count() > 0)
                {
                    MensajeErrorUsuario = ListaParametros.Where(x => x.Nombre == "MsjErrorPortal").FirstOrDefault().Valor;
                }
                else
                {
                    MensajeErrorUsuario = "Por el momento no es posible procesar su solicitud por favor intente más tarde...";

                }

                //LCI. INI. 20181121 LugarExpedicion

                if (ListaLocaciones.Count() == 0)
                {
                    BLLLocationCat bllCatLocation = new BLLLocationCat();
                    ListaLocaciones = bllCatLocation.RecuperarTodo();
                }


                ENTParametrosCnf paramEmpresa = new ENTParametrosCnf();
                paramEmpresa = ListaParametros.Where(x => x.Nombre == "IdEmpresa").FirstOrDefault();

                ENTParametrosCnf paramLugarExpedicion = new ENTParametrosCnf();
                paramLugarExpedicion = ListaParametros.Where(x => x.Nombre == "LugarExpedicionDefault").FirstOrDefault();
                if (paramLugarExpedicion != null)
                {
                    LugarExpedicionDefault = paramLugarExpedicion.Valor;
                }
                else
                {
                    LugarExpedicionDefault = "66600";
                }

                //En caso de que se tenga definida una empresa por default entonces se recupera el lugar de expedicion que tenga configurado
                if (paramEmpresa != null)
                {
                    byte idEmpresa = 0;
                    idEmpresa = Convert.ToByte(paramEmpresa.Valor);
                    ENTEmpresaCat empresa = new ENTEmpresaCat();
                    empresa = ListaCatalogoEmpresas.Where(x => x.IdEmpresa == idEmpresa).FirstOrDefault();
                    if (empresa != null)
                    {
                        LugarExpedicionDefault = empresa.LugarExpedicion;
                    }
                }

                //LCI. INI. 20181121 LugarExpedicion








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
                string mensajeUsuario = "";
                if (TipoProceso == "ProcesoGlobal")
                {
                    mensajeUsuario = ex.Message;
                }
                else
                {
                    mensajeUsuario = "Error de Comunicación con el servidor, por favor intente mas tarde";
                }

                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "Catalogos", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
        }


        public List<ENTPagosFacturados> DistribuirPagosEnReservacion(string pnr, string tipoProceso)
        {
            List<ENTPagosFacturados> listaPagos = new List<ENTPagosFacturados>();
            bool esPorProcesoDiario = (tipoProceso == "ProcesoGlobal");
            listaPagos = DistribuirPagosEnReservacion(pnr, esPorProcesoDiario, null);
            return listaPagos;
        }

        public List<ENTPagosFacturados> DistribuirPagosEnReservacion(string pnr, Paquete paquete)
        {
            List<ENTPagosFacturados> listaPagos = new List<ENTPagosFacturados>();
            listaPagos = DistribuirPagosEnReservacion(pnr, false, paquete);
            return listaPagos;
        }

        /// <summary>
        /// Metodo que realiza el prorrateo de los pagos, vinculando cada codigo Fee, Tarifa,Impuesto, etc con el Pago que este vinculado
        /// </summary>
        /// <param name="pnr">Codigo de la Reservacion que se va a procesar</param>
        /// <returns>Regresa una lista de los Pagos Facturados que incluye la distribucion de Pagos</returns>
        public List<ENTPagosFacturados> DistribuirPagosEnReservacion(string pnr, bool esPorProcesoDiario, Paquete paquete)
        {

            List<ENTPagosFacturados> listaPagos = new List<ENTPagosFacturados>();
            pnr = pnr.ToUpper();
            PNR = pnr;
            try
            {
                if (esPorProcesoDiario)
                {
                    
                        VBFactPaquetes.PortalWeb.Controllers.HomeController x = new VBFactPaquetes.PortalWeb.Controllers.HomeController();
                        string token = x.getAuthToken();
                        paquete = new Paquete();
                        paquete = JsonConvert.DeserializeObject<Paquete>(x.getBookingByPNR(token, pnr));
                }

                //Verifica si ya estan asignados los catalogos generales, en caso de que no los recupera
                InicializaVariablesGlobales();

                //Recupera la informacion de la reservacion a partir del pnr
                DataSet dsReservacion = new DataSet();
                dsReservacion = RecuperarReservacionPorPNR(pnr);

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
                conModificacion = ExisteCambioEnPNR(pnr, dtPNRDiv);

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
                        dsReservacionDiv = RecuperarReservacionPorPNR(pnrDiv);

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
                    ValidarInformacionPNR(pnr, dtReservacionCab, dtVuelosPorPasajero, dtPagos, dtFeeTar, dtFeeSSR);

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



                    listaPagos = GenerarListaPagos(dtPagos, dtPNRDiv, fechaPrimerCargo);

                    //Genera los datos de la Reservacion
                    List<ENTReservacion> listaEntReserva = new List<ENTReservacion>();
                    listaEntReserva = GenerarReservacion(dtReservacionCab, dtFeeTar, dtFeeSSR, dtVuelosCab, dtVuelosPorPasajero, ref listaPagos);

                    //Guarda la informacion de los Pagos en la BD.
                    GuardarReserva(ref listaEntReserva, ref listaPagos);

                    if(paquete.items != null)
                    {
                        listaPagos = addInfo_Hotel(ref listaPagos, paquete);
                        if (esPorProcesoDiario)
                        {
                            listaEntReserva[0].tipoProceso = "ProcesoGlobal";
                        }
                        else
                        {
                            listaEntReserva[0].tipoProceso = "Portal";
                        }
                        //Guarda la informacion correspondiente al Hotel
                        GuardaPaqueteHotel(paquete, ref listaEntReserva);
                    }

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


        /// <summary>
        /// Metodo que verifica si el PNR tuvo algun cambio desde la ultima vez que se proceso su distribucion o si presento algun error
        /// 
        /// </summary>
        /// <param name="pnr">RecordLocator de la reservacion que se esta validando</param>
        /// <param name="dtPNRDiv">Tabla que contiene la lista de los PNR Divididos</param>
        /// <returns>true en caso de que el PNR tenga algun cambio desde la ultima vez que se proceso o si presento error, de otra forma regresa false</returns>
        public bool ExisteCambioEnPNR(string pnr, DataTable dtPNRDiv)
        {
            bool result = false;
            try
            {
                //Verifica si alguno de los pnr sufrio alguna modificacion o si se encuentra en la lista de PNR con errores
                List<string> listaPNRPorValidar = new List<string>();
                //Se agrega el PNR que se solicito
                listaPNRPorValidar.Add(pnr);

                //Se agregan los PNR que vengan como divididos
                foreach (DataRow drDivididos in dtPNRDiv.Rows)
                {
                    string pnrDivIni = drDivididos["RecordLocator"].ToString();
                    if (!listaPNRPorValidar.Contains(pnrDivIni) && !listaPNRPorValidar.Contains(pnrDivIni))
                    {
                        listaPNRPorValidar.Add(pnrDivIni);
                    }
                }

                //Verifica cada PNR para validar si existe su distribucion y si tiene diferente fecha de modificacion
                foreach (string pnrRevision in listaPNRPorValidar)
                {
                    BLL.BLLReservaCab bllReservacion = new BLLReservaCab();
                    List<ENTReservaCab> listaReserva = new List<ENTReservaCab>();
                    listaReserva = bllReservacion.RecuperarReservaCabRecordlocator(pnrRevision);

                    if (listaReserva.Count() > 0)
                    {
                        //Si existe informacion de la reservacion se continua con el proceso
                        ENTReservaCab reserva = new ENTReservaCab();
                        reserva = listaReserva[0];
                        //Se recupera la informacion de la reserva para obtener su fecha de modificacion
                        BLLFacturacion bllFactura = new BLLFacturacion();
                        DataTable dtReservaNav = new DataTable();
                        dtReservaNav = bllFactura.RecuperarReservaPorPNR(pnrRevision);
                        if (dtReservaNav.Rows.Count > 0)
                        {
                            DataRow drReserva = dtReservaNav.Rows[0];
                            DateTime fechaModNav = (DateTime)drReserva["ModifiedDate"];
                            bool esDiferenteFechaModificacion = reserva.ModifiedDate != fechaModNav;
                            bool esEnListaErrores = false;
                            //Aqui va la validacion para checar que no esta en la lista de PNR con errores de Distribucion

                            BLLDistribucionpagos bllDistribucion = new BLLDistribucionpagos();
                            List<ENTDistribucionpagos> listaDistribucion = new List<ENTDistribucionpagos>();
                            listaDistribucion = bllDistribucion.RecuperarDistribucionpagosRecordlocator(pnrRevision);

                            ENTDistribucionpagos ultIntento = listaDistribucion.OrderByDescending(x => x.FechaHoraLocal).FirstOrDefault();

                            if (ultIntento != null)
                            {
                                esEnListaErrores = ultIntento.ConDescartePorDiferencia;
                            }


                            // DHV INI Pago no trae  OrganizationCode
                            //Se recupera la lista de pagos de la reservacion
                            bool SinOrganizationCode = false;
                            long bookingID = 0;
                            bookingID = (long)drReserva["BookingId"];
                            BLLPagosCab bllPagosProc = new BLLPagosCab();
                            List<ENTPagosCab> listaPagosProcesados = new List<ENTPagosCab>();
                            listaPagosProcesados = bllPagosProc.RecuperarPagosCabBookingid(bookingID);

                            if (listaPagosProcesados.Where(x => String.IsNullOrEmpty(x.OrganizationCode)).Count() > 0)
                                SinOrganizationCode = true;

                            // DHV FIN Pago no trae OrganizationCode


                            if (esDiferenteFechaModificacion || esEnListaErrores || SinOrganizationCode)
                            {
                                result = true;
                                break;
                            }
                        }
                        else
                        {
                            result = true;
                            break;
                        }
                    }
                    else
                    {
                        result = true;
                        break;
                    }
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "ExisteCambioEnPNR", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;

        }


        /// <summary>
        /// Metodo que valida la informacion de la reservacion para definir si se puede continuar o no con el proceso de Distribucion
        /// </summary>
        /// <param name="pnr">Codigo de la reservacion</param>
        /// <param name="dtReservacionCab">DataTable que contiene la informacion de la Reservacion</param>
        /// <param name="dtVuelosPorPasajero">DataTable que contiene la informacion de los vuelos vinculados a la Reservacion</param>
        /// <param name="dtPagos">DataTable de Pagos registrados en la reservacion</param>
        /// <param name="dtFeeTar">DataTable de las tarifas incluidas en la reservacion</param>
        /// <param name="dtFeeSSR">DataTable de los Servicios Adicionales por Pasajero registrados en al Reservacion</param>
        private bool ValidarInformacionPNR(string pnr, DataTable dtReservacionCab, DataTable dtVuelosPorPasajero, DataTable dtPagos, DataTable dtFeeTar, DataTable dtFeeSSR)
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


        /// <summary>
        /// Metodo que recupera el IdAgente asignado en la BD a partir del AgentID de Navitaire
        /// </summary>
        /// <param name="agentID">Codigo asignado a un agente en Navitaire</param>
        /// <returns>Codigo asignado a un agente en Facturacion</returns>
        public long RecuperarIdAgente(long agentID)
        {
            long result = 0;
            try
            {
                BLLAgentesCat bllAgente = new BLLAgentesCat();
                //Recupera la informacion del Agente registrada en la BD de Facturacion
                bllAgente.RecuperarAgentesCatAgentid(agentID);
                result = bllAgente.IdAgente;
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "RecuperarIdAgente", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;
        }

        public List<ENTPagosFacturados> addInfo_Hotel(ref List<ENTPagosFacturados> listaPagos, Paquete paquete)
        {
            int cnL = 0;
            foreach(var x in listaPagos)
            {
                if(x.PaymentMethodCode == "X9")
                {
                    break;
                }
                cnL++;
            }
            ENTPagosFacturados entPagoHotel = new ENTPagosFacturados();
            entPagoHotel.PaymentAmount = Convert.ToDecimal(Math.Round(paquete.items[1].sellingPrice, 2));
            //entPagoHotel.CollectedCurrencyCode = paquete.items[1].sellingCurrency;
            entPagoHotel.CurrencyCode = paquete.items[1].sellingCurrency;
            entPagoHotel.EsFacturable = true;
            entPagoHotel.EsFacturableGlobal = false;
            entPagoHotel.EsFacturado = false;
            entPagoHotel.BookingID = listaPagos[0].BookingID;
            entPagoHotel.PaymentMethodCode = listaPagos[cnL].PaymentMethodCode;
            entPagoHotel.OrganizationCode = listaPagos[cnL].OrganizationCode;
            entPagoHotel.FechaPago = Convert.ToDateTime(paquete.order.createdUTC);
            entPagoHotel.ListaDesglosePago = new List<ENTPagosDet>();

            //Se registra el pago actual en el Detalle
            //Esto se hace para justificar la cantidad del monto por aplicar en caso de pagos vinculados
            ENTPagosDet pagoDetHotel = new ENTPagosDet();
            pagoDetHotel.CurrencyCode = entPagoHotel.CurrencyCode;
            //pagoDetHotel.CollectedCurrencyCode = entPagoHotel.CollectedCurrencyCode;
            pagoDetHotel.PaymentAmount = entPagoHotel.PaymentAmount;
            pagoDetHotel.PaymentMethodCode = entPagoHotel.PaymentMethodCode;
            pagoDetHotel.FechaPago = entPagoHotel.FechaPago;
            entPagoHotel.ListaDesglosePago.Add(pagoDetHotel);
            listaPagos.Add(entPagoHotel);

            return listaPagos;
        }
        /// <summary>
        /// Metodo que analiza cada pago y genera una lista de Pagos con todos las propiedades necesarias para vincular a los cargos.
        /// </summary>
        /// <param name="dtPagos">DataTable con la lista de los Pagos por analizar</param>
        /// <param name="dtPNRDiv">DataTable con la lista de los PNR hijos en caso de Division de Reservaciones</param>
        /// <returns></returns>
        public List<ENTPagosFacturados> GenerarListaPagos(DataTable dtPagos, DataTable dtPNRDiv, DateTime fechaPrimerCargo)
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
                        entPago = RecuperarPagoFacturado33(bllPagoBD);
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

                    //LCI. INI. 20181121 LugarExpedicion

                    entPago.LocationCode = "";
                    entPago.LugarExpedicion = LugarExpedicionDefault;

                    if (!dr.IsNull("CreatedLocationCode"))
                    {
                        string locationCode = dr["CreatedLocationCode"].ToString();
                        ENTLocationCat entLocation = ListaLocaciones.Where(x => x.LocationCode == locationCode && x.Activo == true).FirstOrDefault();

                        entPago.LocationCode = locationCode;
                        if (entLocation != null)
                        {
                            entPago.LugarExpedicion = entLocation.LugarExpedicion;

                        }
                    }

                    //LCI. FIN. 20181121 LugarExpedicion


                    //LCI. INI. 20190702 TicketFactura

                    if (!dr.IsNull("OrganizationCode"))
                    {
                        entPago.OrganizationCode = dr["OrganizationCode"].ToString();
                    }

                    //LCI. FIN. 20190702 TicketFactura

                    // DHV INI 20190705 Correccion a pagos con tarjeta
                    if (!dr.IsNull("BinRange")) entPago.BinRange = int.Parse(dr["BinRange"].ToString());
                    // DHV FIN 20190705 Correccion a pagos con tarjeta

                    // DHV INI 20190815 Indentificar pagos de TPV
                    if (!dr.IsNull("DepartmentCode")) entPago.DepartmentCode = dr["DepartmentCode"].ToString();
                    if (!dr.IsNull("DepartmentName")) entPago.DepartmentName = dr["DepartmentName"].ToString();
                    if (!dr.IsNull("OrganizationType")) entPago.OrganizationType = int.Parse(dr["OrganizationType"].ToString());
                    if (!dr.IsNull("OrganizationName")) entPago.OrganizationName = dr["OrganizationName"].ToString();
                    // DHV FIN 20190815 Indentificar pagos de TPV

                    //Recupera la informacion de la forma de pago a partir del PaymentMethodCode
                    if (!dr.IsNull("PaymentMethodCode"))
                    {
                        ENTFormapagoCat entFormaPago = ListaCatalogoFormasPago.Where(x => x.PaymentMethodCode == dr["PaymentMethodCode"].ToString()).FirstOrDefault();

                        if (entFormaPago != null)
                        {
                            entPago.ENTFormaPago = entFormaPago;
                            entPago.IdFormaPago = entPago.ENTFormaPago.IdFormaPago;
                            entPago.PaymentMethodCode = entPago.ENTFormaPago.PaymentMethodCode;
                            entPago.EsFacturableGlobal = entFormaPago.EsFacturableGlobal;
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
                                entPago.TipoCambio = RecuperarTipoCambioPorFecha(entPago.FechaPago);

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
                        entPago.IdAgente = RecuperarIdAgente(agentID);
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
                                pagoDet.IdAgente = RecuperarIdAgente(agentID);
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
                    entPago.EsFacturableGlobal = entPago.EsFacturable;


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

                    entPago.EsFacturableGlobal = entPago.EsFacturable;

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

        public ENTPagosFacturados RecuperarPagoFacturado33(BLLPagosCab bllPagoBD)
        {

            ENTPagosFacturados result = new ENTPagosFacturados();
            try
            {


                result.IdPagosCab = bllPagoBD.IdPagosCab;
                result.BookingID = bllPagoBD.BookingID;
                result.PaymentID = bllPagoBD.PaymentID;
                result.FechaPago = bllPagoBD.FechaPago;
                result.FechaPagoUTC = bllPagoBD.FechaPagoUTC;
                result.IdFormaPago = bllPagoBD.IdFormaPago;
                result.PaymentMethodCode = bllPagoBD.PaymentMethodCode;
                result.CurrencyCode = bllPagoBD.CurrencyCode;
                result.PaymentAmount = bllPagoBD.PaymentAmount;
                result.MontoPorAplicar = bllPagoBD.MontoPorAplicar;
                result.CollectedCurrencyCode = bllPagoBD.CollectedCurrencyCode;
                result.CollectedAmount = bllPagoBD.CollectedAmount;
                result.TipoCambio = bllPagoBD.TipoCambio;
                result.Estatus = bllPagoBD.Estatus;
                result.ParentPaymentID = bllPagoBD.ParentPaymentID;
                result.EsPagoDividido = bllPagoBD.EsPagoDividido;
                result.EsPagoPadre = bllPagoBD.EsPagoPadre;
                result.EsPagoHijo = bllPagoBD.EsPagoHijo;
                result.EsFacturable = bllPagoBD.EsFacturable;
                result.EsFacturableGlobal = bllPagoBD.EsFacturable;
                result.EsParaAplicar = bllPagoBD.EsParaAplicar;
                result.EsFacturado = bllPagoBD.EsFacturado;
                result.IdFacturaCab = bllPagoBD.IdFacturaCab;
                result.IdFacturaGlobal = bllPagoBD.IdFacturaGlobal;
                result.FolioPrefactura = bllPagoBD.FolioPrefactura;
                result.FolioFactura = bllPagoBD.FolioFactura;
                result.FechaFactura = bllPagoBD.FechaFactura;
                result.FechaFacturaUTC = bllPagoBD.FechaFacturaUTC;
                result.VersionFacturacion = bllPagoBD.VersionFacturacion;
                result.MontoTarifa = bllPagoBD.MontoTarifa;
                result.MontoServAdic = bllPagoBD.MontoServAdic;
                result.MontoTUA = bllPagoBD.MontoTUA;
                result.MontoOtrosCargos = bllPagoBD.MontoOtrosCargos;
                result.MontoIVA = bllPagoBD.MontoIVA;
                result.MontoTotal = bllPagoBD.MontoTotal;
                result.IdAgente = bllPagoBD.IdAgente;
                result.FechaHoraLocal = bllPagoBD.FechaHoraLocal;
                result.FechaUltimaActualizacion = bllPagoBD.FechaUltimaActualizacion;
                result.ListaDesglosePago = new List<ENTPagosDet>();
                BLLPagosDet bllPagDet = new BLLPagosDet();
                List<ENTPagosDet> listaPagos = new List<ENTPagosDet>();
                listaPagos = bllPagDet.RecuperarPagosDetIdpagoscab(bllPagoBD.IdPagosCab);
                result.ListaDesglosePago.AddRange(listaPagos);
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "RecuperarPagoFacturado33", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;
        }



        /// <summary>
        /// Metodo que genera la informacion de la reservacion con lo registrado en Navitaire
        /// </summary>
        /// <param name="dtReservacionCab">Informacion principal de la reservacion registrada en Navitaire</param>
        /// <param name="dtFeeTar">Informacion de los cargos por Tarifa y/o modalidad registrados en Navitaire</param>
        /// <param name="dtFeeSSR">Informacion de los servicios adicionales asignados por pasajero en Navitaire</param>
        /// <param name="dtVuelosCab">Informacion de los vuelos en que se encuentra registrada la reservacion</param>
        /// <param name="dtVuelosPorPasajero">Informacion de los pasajeros indicando el vuelo en que esta registrado cada uno</param>
        /// <param name="listaPagos">Lista de Pagos que tiene la reservación</param>
        /// <returns>Reservacion con todos los datos vinculados y distribuidos por pago</returns>
        private List<ENTReservacion> GenerarReservacion(DataTable dtReservacionCab, DataTable dtFeeTar, DataTable dtFeeSSR, DataTable dtVuelosCab, DataTable dtVuelosPorPasajero, ref List<ENTPagosFacturados> listaPagos)
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
                    if (String.IsNullOrEmpty(result.EmailAddress) && !drReserva.IsNull("EmailAddress") && existeReservaBD) result.EmailAddress = drReserva["EmailAddress"].ToString();
                    else if (!drReserva.IsNull("EmailAddress")) result.EmailAddress = reservaBD.EmailAddress = drReserva["EmailAddress"].ToString();
                    result.FechaModificacion = DateTime.Now;

                    //Registra los vuelos
                    result.ListaVuelos = new List<ENTVuelosCab>();
                    //Invoca el Metodo que genera la lista de Vuelos   
                    List<ENTVuelosCab> listaVuelos = new List<ENTVuelosCab>();
                    listaVuelos = GeneraListaVuelos(dtVuelosCab);
                    result.ListaVuelos = listaVuelos;



                    //Genera el detalle de los componentes de la reservacion
                    //Se analizan y actualizan los datos de los Fees tanto de tarifas como de SSR's
                    List<ENTReservaDet> listaDetalleReservaTarifas = new List<ENTReservaDet>();
                    List<ENTReservaDet> listaDetalleReservaSSRs = new List<ENTReservaDet>();
                    List<ENTReservaDet> listaUnirTarifasYSSRs = new List<ENTReservaDet>();
                    List<ENTReservaDet> listaPagosVinculados = new List<ENTReservaDet>();

                    DataTable dtTarifas = new DataTable();
                    DataTable dtReservas = new DataTable();
                    DataTable dtTarifyServ = new DataTable();

                    //Se analizan los cargos de Tarifa
                    listaDetalleReservaTarifas = GenerarListaDetalleReserva("TAR", result, dtFeeTar, dtFeeSSR, dtVuelosPorPasajero, listaVuelos, listaPagos);
                    dtTarifas = Comun.Utils.Diversos.ToDataTable(listaDetalleReservaTarifas);
                    //Se analizan los cargos de SSR
                    listaDetalleReservaSSRs = GenerarListaDetalleReserva("SSR", result, dtFeeTar, dtFeeSSR, dtVuelosPorPasajero, listaVuelos, listaPagos);
                    dtReservas = Comun.Utils.Diversos.ToDataTable(listaDetalleReservaSSRs);

                    //Se unifican los componentes en una sola lista para integrarlos al Detalle y se ordenan en base a la prioridad de asignacion
                    listaUnirTarifasYSSRs = OrdenaListaCargosPorPrioridad(listaDetalleReservaTarifas, listaDetalleReservaSSRs);
                    dtTarifyServ = Comun.Utils.Diversos.ToDataTable(listaUnirTarifasYSSRs);
                    //Antes de unificar la lista de detalles por tarifas o por SSR se verifica si los totales por cada concepto estan en negativo

                    decimal montoTarifasGlobal = 0m;
                    decimal montoSSRGlobal = 0m;

                    montoTarifasGlobal = listaDetalleReservaTarifas.Sum(x => x.ChargeAmount);
                    montoSSRGlobal = listaDetalleReservaSSRs.Sum(x => x.ChargeAmount);

                    //Solo en caso de que los servicios adicionales se conviertan en negativos entonces se buscan los posibles ajustes que provoquen el desbalance para
                    //reasignarlos como parte de la tarifa y equilibrar los importes
                    if (montoSSRGlobal < 0)
                    {
                        ReasignarAjustesNegativos(montoSSRGlobal, ref listaDetalleReservaTarifas, ref listaDetalleReservaSSRs);
                        //Se reasigna el orden para considerar el fee en las tarifas correspondientes
                        listaUnirTarifasYSSRs = OrdenaListaCargosPorPrioridad(listaDetalleReservaTarifas, listaDetalleReservaSSRs);
                    }

                    listaUnirTarifasYSSRsFinal.AddRange(listaUnirTarifasYSSRs);

                    //LCI. INI. 20190222 IVA FRONTERA

                    List<byte> listaJourney = new List<byte>();
                    listaJourney = listaUnirTarifasYSSRsFinal.Select(x => x.NumJourney).Distinct().ToList();

                    foreach (byte numJourney in listaJourney)
                    {
                        List<long> listaIDVuelos = new List<long>();
                        listaIDVuelos = listaUnirTarifasYSSRs.Where(x => x.NumJourney == numJourney).Select(x => x.IdVuelo).Distinct().ToList();

                        ENTVuelosCab entVuelo = new ENTVuelosCab();
                        entVuelo = listaVuelos.Where(x => listaIDVuelos.Contains(x.IdVuelo)).OrderBy(x => x.STD).FirstOrDefault();

                        if (entVuelo != null)
                        {
                            ENTReasignaexpedporiata entVueloFrontera = new ENTReasignaexpedporiata();
                            entVueloFrontera = ListaReasignaExpPorIATA.Where(x => x.CodigoIATA == entVuelo.DepartureStation).FirstOrDefault();
                            if (entVueloFrontera != null)
                            {
                                EsReservaFrontera = true;
                                LugarExpedicionFrontera = entVueloFrontera.LugarExpedicion;
                                foreach (ENTPagosFacturados pagoFact in listaPagos)
                                {
                                    pagoFact.LugarExpedicion = LugarExpedicionFrontera;
                                    pagoFact.LocationCode = entVueloFrontera.CodigoIATA;
                                }
                                break;
                            }
                        }

                    }




                    //LCI. FIN. 20190222 IVA FRONTERA


                    listaResult.Add(result);

                }


                //Se corre el proceso para vincular el pago a cada componente
                //listaPagosVinculados = GeneraVinculoPagosComponentes(listaUnirTarifasYSSRs, ref listaPagos);

                //GeneraVinculoPagosComponentesFact33(ref listaUnirTarifasYSSRsFinal, ref listaPagos, listaResult);
                GeneraVinculoPagosPrincipal(ref listaUnirTarifasYSSRsFinal, ref listaPagos, listaResult);

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




        public void GeneraVinculoPagosPrincipal(ref List<ENTReservaDet> listaUnirTarifasYSSRs, ref List<ENTPagosFacturados> listaPagos, List<ENTReservacion> listaReservas)
        {
            try
            {
                //Recupera los vinculos que ya existan en la BD
                //BuscarVinculosExistentesEnBD(ref listaPagos, ref listaUnirTarifasYSSRs);
                //GenerarVinculosPreExistentesEnBD(ref listaPagos, ref listaUnirTarifasYSSRs);

                DataTable dtTarifRes = new DataTable();

                //Generar Vinculos de pagos Facturados a partir de Eventos de creacion de componentes
                GenerarVinculosPagosFacturados(ref listaPagos, ref listaUnirTarifasYSSRs, listaReservas, false);
                dtTarifRes = Comun.Utils.Diversos.ToDataTable(listaUnirTarifasYSSRs);

                //Generar Vinculos para los pagos que aun no estan completos y de los componentes que aun no tienen vinculos asignados a un pago
                //GenerarVinculosPagosFacturados(ref listaPagos, ref listaUnirTarifasYSSRs, listaReservas,true);

                //Se generan los vinculos para los pagos que aun tienen montos pendientes por cubrir utilizando el metodo de Eventos
                GenerarVinculosPagosComponentes(ref listaPagos, ref listaUnirTarifasYSSRs, listaReservas, "EVENTO");
                dtTarifRes = Comun.Utils.Diversos.ToDataTable(listaUnirTarifasYSSRs);

                //Se generan los vinculos para los pagos que aun tienen montos pendientes por cubrir sin tomar en cuenta los Eventos 
                GenerarVinculosPagosComponentes(ref listaPagos, ref listaUnirTarifasYSSRs, listaReservas, "SINVINCULO");
                dtTarifRes = Comun.Utils.Diversos.ToDataTable(listaUnirTarifasYSSRs);

                //Se calculan los montos asignados por bloque para cada pago.              
                foreach (ENTPagosFacturados pago in listaPagos)
                {
                    pago.MontoTarifa = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pago.IdPagosCab && x.TipoAcumulado == "TAR").Sum(x => x.ChargeAmount);
                    pago.MontoServAdic = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pago.IdPagosCab && x.TipoAcumulado == "SVA").Sum(x => x.ChargeAmount);
                    pago.MontoTUA = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pago.IdPagosCab && x.TipoAcumulado == "TUA").Sum(x => x.ChargeAmount);
                    pago.MontoOtrosCargos = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pago.IdPagosCab && x.TipoAcumulado == "IMP").Sum(x => x.ChargeAmount);
                    pago.MontoIVA = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pago.IdPagosCab && x.TipoAcumulado == "IVA").Sum(x => x.ChargeAmount);
                    pago.MontoTotal = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pago.IdPagosCab).Sum(x => x.ChargeAmount);
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "GenerarVinculoPagosPrincipal", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
        }




        private void AsignaPago(decimal importeFee, decimal montoFeeSinIVA, decimal montoIVAFee, ref List<ENTReservaDet> listaUnirTarifasYSSRs, int orden, ENTPagosFacturados pagoCab
           , string tipoAcum, long idReservaCab)
        {
            try
            {


                decimal restantePago = pagoCab.MontoPorAplicar - pagoCab.MontoAplicado;
                //Si el monto del fee completo es menor o igual que el remanente del pago entonces entra completo
                if (importeFee <= restantePago)
                {
                    var listaFeeCompleto = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.Orden == orden && x.IdPagosCab == 0);
                    foreach (ENTReservaDet reservaDetComp in listaFeeCompleto)
                    {
                        pagoCab.MontoAplicado += reservaDetComp.ChargeAmount;
                        reservaDetComp.IdPagosCab = pagoCab.IdPagosCab;
                        reservaDetComp.FolioPreFactura = (pagoCab.EsFacturable || pagoCab.EsPagoHijo || pagoCab.EsFacturado) ? pagoCab.FolioPrefactura : 0;
                        reservaDetComp.ForeignAmount = Math.Round((reservaDetComp.ChargeAmount * pagoCab.TipoCambio), 2);
                        reservaDetComp.EsFacturable = pagoCab.EsFacturable;

                        if (pagoCab.EsFacturado)
                        {
                            reservaDetComp.EstatusFacturacion = "FA";
                            reservaDetComp.IdFacturaCab = pagoCab.IdFacturaCab;
                        }
                    }

                }
                else
                {
                    //En caso de que el remanente del pago sea menor al importe del fee entonces se tiene que dividir
                    decimal montoSinIvaPorAplicar = 0;
                    decimal montoIVAPorAplicar = 0;
                    decimal montoSinIvaRestante = 0;
                    decimal montoIvaRestante = 0;
                    decimal porIVAPorDesglose = 0;

                    //Se recupera el componente perteneciente al IVA
                    ENTReservaDet componenteIVA = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.Orden == orden && x.IdPagosCab == 0 && x.TipoAcumulado == "IVA").FirstOrDefault();

                    if (componenteIVA != null)
                    {
                        porIVAPorDesglose = (Convert.ToDecimal(componenteIVA.PorcIva) / 100);
                    }

                    //Se calculan los importes a aplicar para el componente
                    montoSinIvaPorAplicar = Math.Round((restantePago / (1 + porIVAPorDesglose)), 2);
                    montoIVAPorAplicar = Math.Round((montoSinIvaPorAplicar * porIVAPorDesglose), 2);


                    //Se valida que no se rebase el restante de pago y se ajusta el IVA en caso de que sobrepase por maximo 2 centavos
                    if (Math.Abs(restantePago - (montoSinIvaPorAplicar + montoIVAPorAplicar)) <= 0.02m)
                    {
                        montoIVAPorAplicar = restantePago - montoSinIvaPorAplicar;
                    }

                    //Calcula los montos que quedan pendientes para el siguiente pago
                    montoSinIvaRestante = montoFeeSinIVA - montoSinIvaPorAplicar;
                    montoIvaRestante = montoIVAFee - montoIVAPorAplicar;

                    //Clonando los componentes para asignar el remanente
                    ENTReservaDet componentePrincipalRemanente = new ENTReservaDet();
                    ENTReservaDet componenteIVARemanente = new ENTReservaDet();

                    var listaCompParticionar = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.Orden == orden && x.IdPagosCab == 0 && x.TipoAcumulado != "IVA").OrderBy(x => x.ChargeAmount);
                    decimal montoTotalFee = listaCompParticionar.Sum(x => x.ChargeAmount);
                    decimal montoFeeAplicar = montoSinIvaPorAplicar + montoSinIvaRestante;

                    foreach (ENTReservaDet entResParticionar in listaCompParticionar.OrderBy(x => x.ChargeAmount))
                    {
                        if (entResParticionar.ChargeAmount > 0)
                        {
                            ClonarReservaDet(ref componentePrincipalRemanente, entResParticionar);
                        }

                        //Asignando importes a los componentes vinculados
                        entResParticionar.IdPagosCab = pagoCab.IdPagosCab;
                        entResParticionar.FolioPreFactura = (pagoCab.EsFacturable || pagoCab.EsPagoHijo || pagoCab.EsFacturado) ? pagoCab.FolioPrefactura : 0;
                        entResParticionar.EsFacturable = pagoCab.EsFacturable;

                        if (montoFeeAplicar > entResParticionar.ChargeAmount)
                        {
                            montoFeeAplicar -= entResParticionar.ChargeAmount;
                        }
                        else
                        {
                            entResParticionar.ChargeAmount = entResParticionar.ChargeAmount - montoSinIvaRestante;
                        }

                        entResParticionar.TipoAcumulado = tipoAcum;
                        entResParticionar.EsPagoParcial = true;
                        entResParticionar.ForeignAmount = Math.Round((entResParticionar.ChargeAmount * pagoCab.TipoCambio), 2);
                        if (pagoCab.EsFacturado)
                        {
                            entResParticionar.EstatusFacturacion = "FA";
                            entResParticionar.IdFacturaCab = pagoCab.IdFacturaCab;
                        }
                        //Se incrementa el monto del Bloque vinculado
                        pagoCab.MontoAplicado += entResParticionar.ChargeAmount;
                    }

                    //Se genera el componente con el remanente y se agrega a la lista principal
                    componentePrincipalRemanente.ChargeAmount = montoSinIvaRestante;
                    componentePrincipalRemanente.EsPagoParcial = true;
                    componentePrincipalRemanente.ForeignAmount = Math.Round((componentePrincipalRemanente.ChargeAmount * pagoCab.TipoCambio), 2);
                    listaUnirTarifasYSSRs.Add(componentePrincipalRemanente);

                    //En caso de que exista IVA
                    if (componenteIVA != null)
                    {
                        var listaIVAParticionar = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.Orden == orden && x.IdPagosCab == 0 && x.TipoAcumulado == "IVA").OrderBy(x => x.ChargeAmount);
                        decimal montoIVAAplicar = montoIVAPorAplicar + montoIvaRestante;
                        //Se clona el componente del IVA
                        foreach (ENTReservaDet entIVAParticionar in listaIVAParticionar.OrderBy(x => x.ChargeAmount))
                        {
                            if (entIVAParticionar.ChargeAmount > 0)
                            {
                                ClonarReservaDet(ref componenteIVARemanente, entIVAParticionar);
                            }
                            //Se actualiza la informacion del IVA original para que solo quede el iva aplicado
                            entIVAParticionar.IdPagosCab = pagoCab.IdPagosCab;
                            entIVAParticionar.FolioPreFactura = (pagoCab.EsFacturable || pagoCab.EsPagoHijo || pagoCab.EsFacturado) ? pagoCab.FolioPrefactura : 0;
                            entIVAParticionar.EsFacturable = pagoCab.EsFacturable;
                            if (montoIVAAplicar > entIVAParticionar.ChargeAmount)
                            {
                                montoIVAAplicar -= entIVAParticionar.ChargeAmount;
                            }
                            else
                            {
                                entIVAParticionar.ChargeAmount = entIVAParticionar.ChargeAmount - montoIvaRestante;
                            }
                            entIVAParticionar.EsPagoParcial = true;
                            entIVAParticionar.ForeignAmount = Math.Round((entIVAParticionar.ChargeAmount * pagoCab.TipoCambio), 2);
                            if (pagoCab.EsFacturado)
                            {
                                entIVAParticionar.EstatusFacturacion = "FA";
                                entIVAParticionar.IdFacturaCab = pagoCab.IdFacturaCab;
                            }

                            //Incrementa  los Acumulados del pago
                            pagoCab.MontoAplicado += entIVAParticionar.ChargeAmount;
                        }
                        //Se genera y agrega el componente del IVA con el monto restante
                        componenteIVARemanente.ChargeAmount = montoIvaRestante;
                        componenteIVARemanente.EsPagoParcial = true;
                        componenteIVARemanente.ForeignAmount = Math.Round((componenteIVARemanente.ChargeAmount * pagoCab.TipoCambio), 2);
                        listaUnirTarifasYSSRs.Add(componenteIVARemanente);

                    }


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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "AsignaPago", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }

        }



        private void AsignaPago(decimal importeFee, decimal montoFeeSinIVA, decimal montoIVAFee, decimal restantePago, decimal restanteBloque
            , ref List<ENTReservaDet> listaUnirTarifasYSSRs, long idReservaCab, int orden, ref decimal acumuladoCargos, ENTPagosFacturados pagoCab
            , string tipoAcum, bool omitirBloquesFacturados, ref Dictionary<string, decimal> listaMontoBloqueVin
            , Dictionary<string, decimal> listaMontosPorBloque)
        {

            try
            {


                //Verifica el monto pendiente por asignar del iva
                decimal montoBloqueIvaRestante = 0;
                montoBloqueIvaRestante = listaMontosPorBloque["IVA"] - listaMontoBloqueVin["IVA"];


                //Si el monto del fee completo es menor o igual que el remanente del pago entonces entra completo
                if (importeFee <= restantePago && (restanteBloque >= montoFeeSinIVA || pagoCab.EsFacturado == false || omitirBloquesFacturados))
                {
                    var listaFeeCompleto = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.Orden == orden && x.IdPagosCab == 0);
                    foreach (ENTReservaDet reservaDetComp in listaFeeCompleto)
                    {
                        pagoCab.MontoAplicado += reservaDetComp.ChargeAmount;
                        reservaDetComp.IdPagosCab = pagoCab.IdPagosCab;
                        reservaDetComp.FolioPreFactura = (pagoCab.EsFacturable || pagoCab.EsPagoHijo || pagoCab.EsFacturado) ? pagoCab.FolioPrefactura : 0;
                        reservaDetComp.ForeignAmount = Math.Round((reservaDetComp.ChargeAmount * pagoCab.TipoCambio), 2);
                        reservaDetComp.EsFacturable = pagoCab.EsFacturable;
                        if (pagoCab.EsFacturado)
                        {
                            reservaDetComp.EstatusFacturacion = "FA";
                            reservaDetComp.IdFacturaCab = pagoCab.IdFacturaCab;

                        }

                        //Se incrementa el monto del Bloque vinculado
                        listaMontoBloqueVin[reservaDetComp.TipoAcumulado] += reservaDetComp.ChargeAmount;
                    }

                }
                else
                {
                    //En caso de que el remanente del pago sea menor al importe del fee entonces se tiene que dividir
                    decimal montoSinIvaPorAplicar = 0;
                    decimal montoIVAPorAplicar = 0;
                    decimal montoSinIvaRestante = 0;
                    decimal montoIvaRestante = 0;
                    decimal porIVAPorDesglose = 0;

                    ENTReservaDet componenteIVA = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.Orden == orden && x.IdPagosCab == 0 && x.TipoAcumulado == "IVA").FirstOrDefault();
                    if (componenteIVA != null)
                    {
                        porIVAPorDesglose = (Convert.ToDecimal(componenteIVA.PorcIva) / 100);
                    }



                    //Calcula los montos que se van a aplicar
                    if (restanteBloque <= montoFeeSinIVA && restantePago >= (montoFeeSinIVA + montoIVAFee) && pagoCab.EsFacturado && omitirBloquesFacturados == false)
                    {

                        montoSinIvaPorAplicar = Math.Round((restanteBloque), 2);
                        montoIVAPorAplicar = Math.Round((montoSinIvaPorAplicar * porIVAPorDesglose), 2);
                    }
                    else
                    {

                        montoSinIvaPorAplicar = Math.Round((restantePago / (1 + porIVAPorDesglose)), 2);
                        montoIVAPorAplicar = Math.Round((montoSinIvaPorAplicar * porIVAPorDesglose), 2);
                    }

                    if (Math.Abs(restantePago - (montoSinIvaPorAplicar + montoIVAPorAplicar)) <= 0.02m)
                    {
                        montoIVAPorAplicar = restantePago - montoSinIvaPorAplicar;
                    }
                    else if (Math.Abs(montoBloqueIvaRestante - montoIVAPorAplicar) <= 0.02m)
                    {
                        montoIVAPorAplicar = montoBloqueIvaRestante;
                    }

                    //Calcula los montos que quedan pendientes para el siguiente pago
                    montoSinIvaRestante = montoFeeSinIVA - montoSinIvaPorAplicar;
                    montoIvaRestante = montoIVAFee - montoIVAPorAplicar;


                    ENTReservaDet componentePrincipalRemanente = new ENTReservaDet();
                    ENTReservaDet componenteIVARemanente = new ENTReservaDet();

                    var listaCompParticionar = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.Orden == orden && x.IdPagosCab == 0 && x.TipoAcumulado != "IVA").OrderBy(x => x.ChargeAmount);

                    foreach (ENTReservaDet entResParticionar in listaCompParticionar.OrderBy(x => x.ChargeAmount))
                    {
                        if (entResParticionar.ChargeAmount > 0)
                        {
                            ClonarReservaDet(ref componentePrincipalRemanente, entResParticionar);
                        }

                        //Asignando importes a los componentes vinculados
                        entResParticionar.IdPagosCab = pagoCab.IdPagosCab;
                        entResParticionar.FolioPreFactura = (pagoCab.EsFacturable || pagoCab.EsPagoHijo || pagoCab.EsFacturado) ? pagoCab.FolioPrefactura : 0;
                        entResParticionar.EsFacturable = pagoCab.EsFacturable;
                        if (entResParticionar.ChargeAmount < montoSinIvaPorAplicar)
                        {
                            montoSinIvaPorAplicar -= entResParticionar.ChargeAmount;
                            entResParticionar.EsPagoParcial = false;
                        }
                        else
                        {
                            entResParticionar.ChargeAmount = montoSinIvaPorAplicar;
                            entResParticionar.EsPagoParcial = true;
                        }

                        //entResParticionar.EsPagoParcial = true;
                        entResParticionar.ForeignAmount = Math.Round((entResParticionar.ChargeAmount * pagoCab.TipoCambio), 2);
                        if (pagoCab.EsFacturado)
                        {
                            entResParticionar.EstatusFacturacion = "FA";
                            entResParticionar.IdFacturaCab = pagoCab.IdFacturaCab;
                        }
                        //Se incrementa el monto del Bloque vinculado
                        listaMontoBloqueVin[entResParticionar.TipoAcumulado] += entResParticionar.ChargeAmount;
                        pagoCab.MontoAplicado += entResParticionar.ChargeAmount;
                    }


                    //Se genera el componente con el remanente y se agrega a la lista principal
                    componentePrincipalRemanente.ChargeAmount = montoSinIvaRestante;
                    componentePrincipalRemanente.EsPagoParcial = true;
                    componentePrincipalRemanente.ForeignAmount = Math.Round((componentePrincipalRemanente.ChargeAmount * pagoCab.TipoCambio), 2);
                    listaUnirTarifasYSSRs.Add(componentePrincipalRemanente);



                    //En caso de que exista IVA
                    if (componenteIVA != null)
                    {
                        var listaIVAParticionar = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.Orden == orden && x.IdPagosCab == 0 && x.TipoAcumulado == "IVA").OrderBy(x => x.ChargeAmount);
                        decimal montoIVAAplicar = montoIVAPorAplicar + montoIvaRestante;
                        //Se clona el componente del IVA
                        foreach (ENTReservaDet entIVAParticionar in listaIVAParticionar)
                        {
                            if (entIVAParticionar.ChargeAmount > 0)
                            {
                                ClonarReservaDet(ref componenteIVARemanente, componenteIVA);
                            }
                            //Se actualiza la informacion del IVA original para que solo quede el iva aplicado
                            componenteIVA.IdPagosCab = pagoCab.IdPagosCab;
                            componenteIVA.FolioPreFactura = (pagoCab.EsFacturable || pagoCab.EsPagoHijo || pagoCab.EsFacturado) ? pagoCab.FolioPrefactura : 0;
                            componenteIVA.EsFacturable = pagoCab.EsFacturable;
                            if (montoIVAAplicar > entIVAParticionar.ChargeAmount)
                            {
                                montoIVAAplicar -= entIVAParticionar.ChargeAmount;
                            }
                            else
                            {
                                entIVAParticionar.ChargeAmount = entIVAParticionar.ChargeAmount - montoIvaRestante;
                            }

                            //componenteIVA.ChargeAmount = montoIVAPorAplicar;
                            componenteIVA.EsPagoParcial = true;
                            componenteIVA.ForeignAmount = Math.Round((componenteIVA.ChargeAmount * pagoCab.TipoCambio), 2);
                            if (pagoCab.EsFacturado)
                            {
                                componenteIVA.EstatusFacturacion = "FA";
                                componenteIVA.IdFacturaCab = pagoCab.IdFacturaCab;
                            }
                            //Incrementa  los Acumulados del pago
                            listaMontoBloqueVin[componenteIVA.TipoAcumulado] += componenteIVA.ChargeAmount;
                            pagoCab.MontoAplicado += componenteIVA.ChargeAmount;
                        }
                        //Se genera y agrega el componente del IVA con el monto restante
                        componenteIVARemanente.ChargeAmount = montoIvaRestante;
                        componenteIVARemanente.EsPagoParcial = true;
                        componenteIVARemanente.ForeignAmount = Math.Round((componenteIVARemanente.ChargeAmount * pagoCab.TipoCambio), 2);
                        listaUnirTarifasYSSRs.Add(componenteIVARemanente);

                    }

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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "AsignaPago", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
        }



        public List<ENTReservaDet> GenerarListaDetalleReserva(string tarifaOSSR, ENTReservacion entReservacion, DataTable dtFeeTar, DataTable dtFeeSSR, DataTable dtVuelosPorPasajero, List<ENTVuelosCab> listaVuelos, List<ENTPagosFacturados> listaPagos)
        {
            List<ENTReservaDet> result = new List<ENTReservaDet>();

            try
            {
                int idDet = 0;

                DataTable dtComponentes = new DataTable();
                if (tarifaOSSR.Equals("TAR"))
                {
                    dtComponentes = dtFeeTar;
                }
                else if (tarifaOSSR.Equals("SSR"))
                {
                    dtComponentes = dtFeeSSR;
                }



                //Recupera la informacion de la BD y genera la primer version de los detalles de la reserva
                foreach (DataRow drDetalle in dtComponentes.Select("BookingId = " + entReservacion.BookingID.ToString()))
                {
                    idDet++;
                    ENTReservaDet detReserva = new ENTReservaDet();

                    detReserva.IdReservaCab = entReservacion.IdReservaCab;
                    detReserva.IdReservaDet = idDet;
                    if (!drDetalle.IsNull("FeeNumber")) detReserva.FeeNumber = Convert.ToInt16(drDetalle["FeeNumber"]);
                    if (!drDetalle.IsNull("PassengerID")) detReserva.PassengerID = Convert.ToInt64(drDetalle["PassengerID"]);
                    if (!drDetalle.IsNull("CreatedAgentID")) detReserva.CreatedAgentID = Convert.ToInt64(drDetalle["CreatedAgentID"]);
                    if (!drDetalle.IsNull("CreatedDate")) detReserva.CreatedDate = Convert.ToDateTime(drDetalle["CreatedDate"]);
                    if (!drDetalle.IsNull("SegmentID")) detReserva.SegmentID = Convert.ToInt64(drDetalle["SegmentID"]);
                    if (!drDetalle.IsNull("ChargeNumber")) detReserva.ChargeNumber = Convert.ToInt16(drDetalle["ChargeNumber"]);
                    if (!drDetalle.IsNull("ChargeType")) detReserva.ChargeType = Convert.ToInt16(drDetalle["ChargeType"]);
                    if (!drDetalle.IsNull("ChargeCode")) detReserva.ChargeCode = drDetalle["ChargeCode"].ToString();
                    //if (!drDetalle.IsNull("Descripcion")) detReserva.ChargeDetail = drDetalle["Descripcion"].ToString();
                    //if (!drDetalle.IsNull("TicketCode")) detReserva.TicketCode = drDetalle["TicketCode"].ToString();
                    if (!drDetalle.IsNull("CurrencyCode")) detReserva.CurrencyCode = drDetalle["CurrencyCode"].ToString();
                    if (!drDetalle.IsNull("ChargeAmount")) detReserva.ChargeAmount = Convert.ToDecimal(drDetalle["ChargeAmount"]);
                    if (!drDetalle.IsNull("ForeignCurrencyCode")) detReserva.ForeignCurrencyCode = drDetalle["ForeignCurrencyCode"].ToString();
                    if (!drDetalle.IsNull("ForeignAmount")) detReserva.ForeignAmount = Convert.ToDecimal(drDetalle["ForeignAmount"]);
                    if (!drDetalle.IsNull("TipoCargo")) detReserva.TipoCargo = drDetalle["TipoCargo"].ToString();


                    //LCI. INI. 20180606 Se recupera la clasificacion del catalogo de facturacion
                    //if (!drDetalle.IsNull("ClasFact")) detReserva.ClasFact = Convert.ToByte(drDetalle["ClasFact"]);
                    ENTFeeCat feeCat = new ENTFeeCat();
                    if (ListaCatalogoFees.Where(x => x.FeeCode == detReserva.ChargeCode).Count() > 0)
                    {
                        feeCat = ListaCatalogoFees.Where(x => x.FeeCode == detReserva.ChargeCode).FirstOrDefault();
                        detReserva.ClasFact = feeCat.ClasFact;
                        detReserva.TicketCode = feeCat.DisplayCode;
                        detReserva.ChargeDetail = feeCat.Name;
                    }

                    ////CVG
                    //else if (detReserva.ChargeCode == "VBABFB")
                    //{
                    //    detReserva.ClasFact = 3;
                    //    if (!drDetalle.IsNull("TicketCode")) detReserva.TicketCode = drDetalle["TicketCode"].ToString();
                    //    if (!drDetalle.IsNull("Descripcion")) detReserva.ChargeDetail = drDetalle["Descripcion"].ToString();
                    //}
                    ////CVG Fin

                    else
                    {
                        if (!drDetalle.IsNull("ClasFact")) detReserva.ClasFact = Convert.ToByte(drDetalle["ClasFact"]);
                        if (!drDetalle.IsNull("TicketCode")) detReserva.TicketCode = drDetalle["TicketCode"].ToString();
                        if (!drDetalle.IsNull("Descripcion")) detReserva.ChargeDetail = drDetalle["Descripcion"].ToString();
                    }

                    //LCI. FIN. 20180606 Se recupera la clasificacion del catalogo de facturacion
                    //if (!drDetalle.IsNull("Orden")) detReserva.Orden = Convert.ToInt16(drDetalle["Orden"]);

                    detReserva.FechaHoraLocal = DateTime.Now;


                    //Procesando la informacion del Fee que no viene de la BD
                    ENTVuelosCab entVuelo = new ENTVuelosCab();
                    entVuelo = RecuperaDatosVuelo(detReserva.SegmentID, detReserva.PassengerID, detReserva.CreatedDate, dtVuelosPorPasajero, listaVuelos);

                    if (entVuelo != null)
                    {
                        detReserva.IdVuelo = entVuelo.IdVuelo;
                    }

                    //Recupera la informacion del vuelo para el pasajero asignado
                    foreach (var drVueloPax in dtVuelosPorPasajero.Select("BookingId = " + entReservacion.BookingID.ToString() + " AND PassengerId = " + detReserva.PassengerID.ToString() + " AND InventoryLegID = " + entVuelo.InventoryLegId.ToString()))
                    {
                        detReserva.NumJourney = Convert.ToByte(drVueloPax["JourneyNumber"]);
                        detReserva.LiftStatus = drVueloPax["EstatusAbordaje"].ToString();
                    }


                    //Identifica el tipo de acumulado en funcion de la clasificacion del Fee
                    switch (detReserva.ClasFact)
                    {
                        case 1:
                            if (detReserva.TicketCode == "XV" || detReserva.TicketCode == "XD")
                            {
                                detReserva.TipoAcumulado = "TUA";
                            }
                            else
                            {
                                detReserva.TipoAcumulado = "IMP";
                            }
                            break;
                        case 2:
                            //Es un servicio Adicional
                            detReserva.TipoAcumulado = "SVA";
                            break;
                        case 3:
                            //Es tarifa
                            detReserva.TipoAcumulado = "TAR";
                            break;
                        case 4:
                            //Es IVA
                            detReserva.TipoAcumulado = "IVA";
                            break;
                        default:
                            detReserva.TipoAcumulado = "";
                            break;
                    }

                    ENTFeeCat entFee = new ENTFeeCat();

                    entFee = ListaCatalogoFees.Where(x => x.FeeCode == detReserva.ChargeCode).FirstOrDefault();

                    if (entFee != null)
                    {
                        detReserva.IdFee = entFee.IdFee;
                    }

                    ENTConceptosCat entConcepto = new ENTConceptosCat();
                    entConcepto = ListaCatalogoConceptos.Where(x => x.ClasFact == detReserva.ClasFact && x.TipoComprobante == "F").FirstOrDefault();

                    if (entConcepto != null)
                    {
                        detReserva.IdConcepto = entConcepto.IdConcepto;
                    }

                    //Falta identificar como llenar este campo cuando un Fee ya existe registrado por otro agente y solo es la diferencia del nuevo cargo
                    detReserva.FechaAplicaCompra = detReserva.CreatedDate;

                    result.Add(detReserva);
                }

                //Analiza los IVA's y asigna a cada Fee su Fila de IVA que le corresponde.
                if (tarifaOSSR == "TAR")
                {
                    //En caso de ser la informacion de las tarifas se invoca este metodo
                    //LCI. INI. CONEXIONES
                    result = AsignarIVAPorTarifa(result);
                    //bool EsTarifaPorSegmento = false;

                    //if (EsTarifaPorSegmento)
                    //{
                    //    result = AsignarIVAPorTarifaPorSegmento(result);
                    //}
                    //else
                    //{
                    //    result = AsignarIVAPorTarifaPorJourney(result);
                    //}

                    //LCI. FIN. 

                }
                else
                {
                    //En caso de que se trate de SSR se invoca este otro metodo para separar o segmentar el IVA
                    result = AsignarIVAPorSSR(result);
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "GenerarListaDetalleReserva", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }

            return result;
        }


        private ENTVuelosCab RecuperaDatosVuelo(long segmentID, long passengerID, DateTime fechaCreacionFee, DataTable dtVuelosPorPasajero, List<ENTVuelosCab> listaVuelosCab)
        {
            ENTVuelosCab result = new ENTVuelosCab();
            try
            {
                if (listaVuelosCab.Count() == 1)
                {
                    result = listaVuelosCab[0];
                }
                else
                {
                    string qryBusqueda = "";

                    if (segmentID > 0)
                    {
                        qryBusqueda = "SegmentID = " + segmentID.ToString() + " AND PassengerID = " + passengerID.ToString();
                    }
                    else
                    {
                        qryBusqueda = "PassengerID = " + passengerID.ToString();
                    }


                    foreach (DataRow drVueloPas in dtVuelosPorPasajero.Select(qryBusqueda))
                    {
                        long inventoryLegID = 0;
                        inventoryLegID = Convert.ToInt64(drVueloPas["InventoryLegID"]);

                        ENTVuelosCab vuelo = listaVuelosCab.Where(x => x.InventoryLegId == inventoryLegID).FirstOrDefault();

                        if (vuelo != null)
                        {
                            //LCI. INI. IDENTIFICAR VUELO PARA DEFINIR EL JOURNEY AL QUE PERTENECE
                            //if (vuelo.STD >= fechaCreacionFee.AddHours(-6))
                            //{
                            //    result = vuelo;
                            //}
                            result = vuelo;
                            //LCI. FIN.
                        }

                    }

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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "RecuperaDatosVuelo", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;
        }


        private void GuardaPaqueteHotel(Paquete paquete, ref List<ENTReservacion> listaEntReserva)
        {
            DALFacturacion FPG = new DALFacturacion(null);
            BLLHotel bllHotel = new BLLHotel();
            if(listaEntReserva[0].tipoProceso == "ProcesoGlobal")
            {
                bllHotel.FolioPG = FPG.GetFolioProcesoGlobal(listaEntReserva[0].tipoProceso);
            }
            else
            {
                bllHotel.FolioPG = 0;
            }
            bllHotel.idReservaCab = (int)listaEntReserva[0].IdReservaCab;
            bllHotel.pnr = listaEntReserva[0].RecordLocator;
            bllHotel.superPNR = paquete.order.bookingId;
            bllHotel.referenceID = paquete.items[1].productType.referenceId;
            bllHotel.Type = paquete.items[1].productType.productType;
            bllHotel.currencyCode = paquete.items[1].sellingCurrency;
            bllHotel.chargeAmount = Math.Round(paquete.items[1].sellingPrice, 2);
            bllHotel.createdDate = Convert.ToDateTime(paquete.order.createdUTC);
            bllHotel.tipoProceso = listaEntReserva[0].tipoProceso;
            bllHotel.AgregaInfoHotel();
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
                    bllPago.EsFacturableGlobal = pago.EsFacturableGlobal;
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
                    //LCI. INI. 20181121 LugarExpedicion
                    bllPago.LocationCode = pago.LocationCode;
                    bllPago.LugarExpedicion = pago.LugarExpedicion;
                    //LCI. FIN. 20181121 LugarExpedicion

                    //LCI. INI. 2019-07-02 TicketFactura
                    bllPago.OrganizationCode = pago.OrganizationCode;
                    //LCI. FIN. 2019-07-02 TicketFactura

                    // DHV INI 20190705
                    bllPago.BinRange = pago.BinRange;
                    bllPago.IdUpdFormaPago = pago.IdUpdFormaPago;
                    bllPago.UpdFormaPagModificadoPor = pago.UpdFormaPagModificadoPor == null ? "" : pago.UpdFormaPagModificadoPor;
                    bllPago.FechaUpdaFormaPag = pago.FechaUpdaFormaPag;
                    // DHV FIN 20190705

                    // DHV INI 20190815 Indentificar pagos de TPV
                    bllPago.DepartmentCode = pago.DepartmentCode;
                    bllPago.DepartmentName = pago.DepartmentName;
                    bllPago.OrganizationName = pago.OrganizationName;
                    bllPago.OrganizationType = pago.OrganizationType;
                    // DHV FIN 20190815 Indentificar pagos de TPV

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
                                existenDiferencias = ExisteDiferenciaEntreBDVsProc(detBD, detProc);
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
                    List<ENTReservaDet> detalle = new List<ENTReservaDet>();
                    detalle = listaDetalleReserva.Where(x => x.IdPagosCab == pago.IdPagosCab).ToList();
                    List<ENTReservaDet> detallevacio = new List<ENTReservaDet>();
                    detallevacio = listaDetalleReserva.Where(x => x.IdPagosCab <= 0 || x.IdPagosCab == null || x.IdPagosCab >= 2).ToList();
                    if (Math.Round(montoAsignadoDet, 2) != Math.Round(pagoPorAplicar, 2))
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

        public bool ExisteDiferenciaEntreBDVsProc(ENTReservaDet detBD, BLLReservaDet detProc)
        {
            bool result = false;
            try
            {
                result =
                !(detBD.IdReservaCab == detProc.IdReservaCab
                && detBD.IdReservaDet == detProc.IdReservaDet
                && detBD.Orden == detProc.Orden
                && detBD.FeeNumber == detProc.FeeNumber
                && detBD.PassengerID == detProc.PassengerID
                && detBD.SegmentID == detProc.SegmentID
                && detBD.ChargeNumber == detProc.ChargeNumber
                && detBD.ChargeType == detProc.ChargeType
                && detBD.IdFee == detProc.IdFee
                && detBD.ChargeCode == detProc.ChargeCode
                && detBD.ChargeDetail == detProc.ChargeDetail
                && detBD.TicketCode == detProc.TicketCode
                && detBD.CurrencyCode == detProc.CurrencyCode
                && detBD.ChargeAmount == detProc.ChargeAmount
                && detBD.ForeignCurrencyCode == detProc.ForeignCurrencyCode
                && detBD.ForeignAmount == detProc.ForeignAmount
                && detBD.FechaAplicaCompra == detProc.FechaAplicaCompra
                && detBD.PorcIva == detProc.PorcIva
                && detBD.TipoCargo == detProc.TipoCargo
                && detBD.TipoAcumulado == detProc.TipoAcumulado
                && detBD.IdPagosCab == detProc.IdPagosCab
                && detBD.EsPagoParcial == detProc.EsPagoParcial
                && detBD.MontoPagado == detProc.MontoPagado
                && detBD.EsFacturable == detProc.EsFacturable
                && detBD.EstatusFacturacion == detProc.EstatusFacturacion
                && detBD.IdFacturaCab == detProc.IdFacturaCab
                && detBD.FolioPreFactura == detProc.FolioPreFactura
                && detBD.ClasFact == detProc.ClasFact
                && detBD.IdConcepto == detProc.IdConcepto
                && detBD.IdFolioFacturaGlobal == detProc.IdFolioFacturaGlobal
                && detBD.IdVuelo == detProc.IdVuelo
                && detBD.NumJourney == detProc.NumJourney
                && detBD.LiftStatus == detProc.LiftStatus
                && detBD.CreatedAgentID == detProc.CreatedAgentID
                && detBD.CreatedDate == detProc.CreatedDate
                //&& detBD.FechaHoraLocal == detProc.FechaHoraLocal
                );
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "ExisteDiferenciaEntreBDVsProc", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;
        }


        private List<ENTReservaDet> AsignarIVAPorTarifa(List<ENTReservaDet> listaCompTarifa)
        {
            List<ENTReservaDet> result = new List<ENTReservaDet>();
            List<ENTReservaDet> listaIvasAgregados = new List<ENTReservaDet>();
            List<ENTReservaDet> listaUnificada = new List<ENTReservaDet>();
            List<ENTReservaDet> lsteliminar = new List<ENTReservaDet>();

            try
            {
                //LCI. INI. CAMBIAR CALCULO DEL IVA PASANDO DE SEGMENTO A JOURNEY

                var pasajeros = (from x in listaCompTarifa
                                 select x.PassengerID).Distinct();
                int c = 0;

                //Se recorre cada uno de los segmentos para identificar los importes de tarifa + modalidad y el importe global del IVA
                foreach (long pax in pasajeros)
                {


                    var journeys = (from x in listaCompTarifa
                                    where x.PassengerID == pax
                                    select x.NumJourney).Distinct();

                    //Se recorre cada uno de los segmentos para identificar los importes de tarifa + modalidad y el importe global del IVA
                    foreach (byte journey in journeys)
                    {
                        decimal montoTarifaConMod = 0;
                        decimal montoTarifaSinMod = 0;
                        decimal montoIVA = 0;
                        bool modalidadConIVA = true;
                        decimal porIVA = 0;
                        int cantRHS = listaCompTarifa.Where(x => x.PassengerID == pax && x.NumJourney == journey && x.ClasFact == 3 && x.IdConcepto == 1 && x.ChargeCode == "VBABET").Count();
                        int cantIVAS = listaCompTarifa.Where(x => x.PassengerID == pax && x.NumJourney == journey && x.ClasFact == 4).Count();
                        //Verifica si existe una fila de IVA para el segmento que se esta revisando
                        if (cantIVAS > 0)
                        {
                            if (cantIVAS > cantRHS)
                            {
                                montoTarifaConMod = listaCompTarifa.Where(x => x.PassengerID == pax && x.NumJourney == journey && (x.ClasFact == 2 || x.ClasFact == 3)).Sum(x => x.ChargeAmount);
                                montoIVA = listaCompTarifa.Where(x => x.PassengerID == pax && x.NumJourney == journey && x.ClasFact == 4).Sum(x => x.ChargeAmount);
                                //LCI. INI. 2018-06-13. Se omite el filtro del chargetype, considerando que al agregar otros no se tiene como parametro y no es comun que avisen del cambio, ocasionando un riesgo de descuadre al calcular el iva
                                //montoTarifaSinMod = listaCompTarifa.Where(x => x.NumJourney == journey && x.ClasFact == 3 && (x.ChargeType == 0 || x.ChargeType == 1 || x.ChargeType == 8) && x.ChargeCode.Length == 0).Sum(x => x.ChargeAmount);
                                montoTarifaSinMod = listaCompTarifa.Where(x => x.PassengerID == pax && x.NumJourney == journey && x.ClasFact == 3 && x.ChargeCode.Length == 0).Sum(x => x.ChargeAmount);
                                //LCI. FIN. 2018-06-13.

                                if (montoTarifaConMod != 0)
                                {
                                    porIVA = Math.Round(((((montoTarifaConMod + montoIVA) - montoTarifaConMod) / montoTarifaConMod) * 100), 0);

                                }
                                else
                                {
                                    porIVA = 0;
                                }


                                if (!ListaPorcIVAValidos.Contains(porIVA))
                                {
                                    Dictionary<decimal, decimal> listaDifIVA = new Dictionary<decimal, decimal>();

                                    foreach (decimal ivaVal in ListaPorcIVAValidos)
                                    {
                                        if (ivaVal > 0)
                                        {
                                            decimal dif = 0;
                                            dif = Math.Abs(ivaVal - porIVA);
                                            listaDifIVA.Add(ivaVal, dif);
                                        }
                                    }

                                    decimal porIVAVal = 0;
                                    porIVAVal = listaDifIVA.OrderBy(x => x.Value).Select(x => x.Key).FirstOrDefault();
                                    if (ListaPorcIVAValidos.Contains(porIVAVal))
                                    {
                                        porIVA = porIVAVal;
                                        //modalidadConIVA = false;

                                    }
                                }

                                //Registra el porcentaje de IVA aplicado
                                if (!ListaPorcIVAAplicados.Contains(porIVA) && ListaPorcIVAValidos.Contains(porIVA))
                                {
                                    ListaPorcIVAAplicados.Add(porIVA);
                                }
                            }
                            else
                            {
                                montoIVA = listaCompTarifa.Where(x => x.PassengerID == pax && x.NumJourney == journey && x.ClasFact == 4).Sum(x => x.ChargeAmount);
                                porIVA = 0;
                                ListaPorcIVAAplicados.Add(porIVA);
                            }

                            //Se recuperan los segmentos que le corresponden al journey que se esta validando
                            var segmentos = (from x in listaCompTarifa
                                             where x.PassengerID == pax && x.NumJourney == journey
                                             select x.SegmentID).Distinct();

                            foreach (long segmentoID in segmentos)
                            {
                                decimal montoTarifa = 0;
                                decimal ivaSoloTarifa = 0;
                                //CVG
                                bool modifico = false;
                                //MASS,CGVG 2021
                                //Identificar los elementos que tienen RHS del pax
                                ENTReservaDet CargosRHS = new ENTReservaDet();
                                CargosRHS = listaCompTarifa.Where(x => x.PassengerID == pax && x.SegmentID == segmentoID && x.ClasFact == 3 && x.IdConcepto == 1 && x.ChargeCode == "VBABET").FirstOrDefault();
                                if (CargosRHS != null)
                                {
                                    decimal chargeamount = Math.Round(CargosRHS.ChargeAmount * (decimal)0.1600, 2);
                                    CargosRHS = listaCompTarifa.Where(x => x.PassengerID == pax && x.SegmentID == segmentoID && x.ClasFact == 4 && (x.ChargeCode == "VBAAWT" || x.ChargeCode == "VBAAXR") && (x.TicketCode == "MX" || x.TicketCode == "XO") && x.ChargeAmount == chargeamount).FirstOrDefault();
                                    lsteliminar.Add(CargosRHS);
                                }

                                List<ENTReservaDet> listaDetalleTar = new List<ENTReservaDet>();
                                //LCI. INI. 2018-06-13. Se omite el filtro del chargetype, considerando que al agregar otros no se tiene como parametro y no es comun que avisen del cambio, ocasionando un riesgo de descuadre al calcular el iva
                                //listaDetalleTar = listaCompTarifa.Where(x => x.SegmentID == segmentoID && x.ClasFact == 3 && (x.ChargeType == 0 || x.ChargeType == 1) && x.ChargeCode.Length == 0).ToList();
                                listaDetalleTar = listaCompTarifa.Where(x => x.PassengerID == pax && x.SegmentID == segmentoID && x.ClasFact == 3 && x.ChargeCode.Length == 0).ToList();
                                //LCI. FIN. 2018-06-13
                                foreach (ENTReservaDet entTar in listaDetalleTar)
                                {
                                    entTar.PorcIva = Convert.ToByte(porIVA);
                                }


                                decimal ivaAplicado = porIVA / 100;

                                ENTReservaDet entTarifa = new ENTReservaDet();

                                //LCI. INI. 2018-06-13. Se omite el filtro del chargetype, considerando que al agregar otros no se tiene como parametro y no es comun que avisen del cambio, ocasionando un riesgo de descuadre al calcular el iva
                                //entTarifa = listaCompTarifa.Where(x => x.SegmentID == segmentoID && x.ClasFact == 3 && (x.ChargeType == 0 || x.ChargeType == 1 || x.ChargeType == 8) && x.ChargeCode.Length == 0).FirstOrDefault();
                                //montoTarifa = listaCompTarifa.Where(x => x.SegmentID == segmentoID && x.ClasFact == 3 && (x.ChargeType == 0 || x.ChargeType == 1 || x.ChargeType == 8) && x.ChargeCode.Length == 0).Sum(x => x.ChargeAmount);
                                entTarifa = listaCompTarifa.Where(x => x.PassengerID == pax && x.SegmentID == segmentoID && x.ClasFact == 3 && x.ChargeCode.Length == 0).FirstOrDefault();
                                montoTarifa = listaCompTarifa.Where(x => x.PassengerID == pax && x.SegmentID == segmentoID && x.ClasFact == 3 && x.ChargeCode.Length == 0).Sum(x => x.ChargeAmount);
                                //LCI. FIN. 2018-06-13

                                ENTReservaDet entIvaTarifa = new ENTReservaDet();
                                if (CargosRHS != null)
                                {
                                    entIvaTarifa = listaCompTarifa.Where(x => x.PassengerID == pax && x.SegmentID == segmentoID && x.ClasFact == 4 && x.ChargeAmount != CargosRHS.ChargeAmount).FirstOrDefault() == null ? null : listaCompTarifa.Where(x => x.PassengerID == pax && x.SegmentID == segmentoID && x.ClasFact == 4 && x.ChargeAmount != CargosRHS.ChargeAmount).First();
                                }
                                else
                                {
                                    entIvaTarifa = listaCompTarifa.Where(x => x.PassengerID == pax && x.SegmentID == segmentoID && x.ClasFact == 4).FirstOrDefault();
                                }

                                //CVG
                                if (entTarifa != null && entIvaTarifa == null)
                                {
                                    ivaSoloTarifa = Math.Round((montoTarifa * ivaAplicado), 2);

                                    if (Math.Abs(ivaSoloTarifa - montoIVA) <= .02M)
                                    {
                                        ivaSoloTarifa = montoIVA;
                                    }

                                    //En caso de que la entidad del iva no se encuentre se genera un nuevo cargo

                                    if (entIvaTarifa != null)
                                    {
                                        entIvaTarifa.ChargeAmount = ivaSoloTarifa;
                                        entIvaTarifa.PorcIva = Convert.ToByte(porIVA);
                                        entIvaTarifa.IdConcepto = entTarifa.IdConcepto;
                                    }
                                    else
                                    {
                                        entIvaTarifa = listaCompTarifa.Where(x => x.PassengerID == pax && x.NumJourney == journey && x.ClasFact == 4).FirstOrDefault();
                                        ENTReservaDet entDetIvaFeeMod = new ENTReservaDet();
                                        ClonarReservaDet(ref entDetIvaFeeMod, entIvaTarifa);
                                        entDetIvaFeeMod.FeeNumber = entTarifa.FeeNumber;
                                        entDetIvaFeeMod.ChargeAmount = ivaSoloTarifa;
                                        entDetIvaFeeMod.PorcIva = Convert.ToByte(porIVA);
                                        entDetIvaFeeMod.IdConcepto = entTarifa.IdConcepto;
                                        entDetIvaFeeMod.SegmentID = entTarifa.SegmentID;
                                        entDetIvaFeeMod.PassengerID = entTarifa.PassengerID;
                                        listaIvasAgregados.Add(entDetIvaFeeMod);
                                    }

                                    modifico = true;
                                }

                                if (entTarifa != null && entIvaTarifa != null && modifico != true)
                                {

                                    if (entIvaTarifa.ChargeCode == "VBAAWT")
                                    {
                                        ivaSoloTarifa = Math.Round((montoTarifa * (decimal)0.16), 2);
                                    }
                                    else if (entIvaTarifa.ChargeCode == "VBAAWU")
                                    {
                                        ivaSoloTarifa = Math.Round((montoTarifa * (decimal)0.04), 2);
                                    }
                                    else
                                    {
                                        ivaSoloTarifa = Math.Round((montoTarifa * ivaAplicado), 2);
                                    }

                                    //if (ivaSoloTarifa > montoIVA && ivaSoloTarifa > 0 && Math.Abs(ivaSoloTarifa - montoIVA) <= .02M)
                                    //{
                                    //    ivaSoloTarifa = montoIVA;
                                    //}
                                    //else if (Math.Abs(ivaSoloTarifa - montoIVA) <= .02M)
                                    //{
                                    //    ivaSoloTarifa = montoIVA;
                                    //}

                                    if (Math.Abs(ivaSoloTarifa - montoIVA) <= .02M)
                                    {
                                        ivaSoloTarifa = montoIVA;
                                    }

                                    //En caso de que la entidad del iva no se encuentre se genera un nuevo cargo

                                    if (entIvaTarifa != null)
                                    {
                                        entIvaTarifa.ChargeAmount = ivaSoloTarifa;
                                        entIvaTarifa.PorcIva = Convert.ToByte(porIVA);
                                        entIvaTarifa.IdConcepto = entTarifa.IdConcepto;
                                    }
                                    else
                                    {
                                        entIvaTarifa = listaCompTarifa.Where(x => x.PassengerID == pax && x.NumJourney == journey && x.ClasFact == 4).FirstOrDefault();
                                        ENTReservaDet entDetIvaFeeMod = new ENTReservaDet();
                                        ClonarReservaDet(ref entDetIvaFeeMod, entIvaTarifa);
                                        entDetIvaFeeMod.FeeNumber = entTarifa.FeeNumber;
                                        entDetIvaFeeMod.ChargeAmount = ivaSoloTarifa;
                                        entDetIvaFeeMod.PorcIva = Convert.ToByte(porIVA);
                                        entDetIvaFeeMod.IdConcepto = entTarifa.IdConcepto;
                                        entDetIvaFeeMod.SegmentID = entTarifa.SegmentID;
                                        entDetIvaFeeMod.PassengerID = entTarifa.PassengerID;
                                        listaIvasAgregados.Add(entDetIvaFeeMod);
                                    }
                                }
                                montoIVA = montoIVA - ivaSoloTarifa;

                                //if (montoIVA > 0)
                                //{
                                var listaCompTar = listaCompTarifa.Where(x => x.PassengerID == pax && x.SegmentID == segmentoID && x.ChargeType != 0 && x.ChargeCode.Length > 0 && (x.ClasFact == 2 || x.ClasFact == 3)).OrderBy(x => x.ChargeAmount);


                                int numFilas = listaCompTar.Count();
                                int numFilaActual = 0;


                                foreach (ENTReservaDet entModalidad in listaCompTar)
                                {
                                    //numFilaActual++;
                                    decimal montoFeeMod = 0;
                                    decimal ivaMontoFee = 0;
                                    int feeNumber = 0;
                                    feeNumber = entModalidad.FeeNumber;

                                    //Asigna el porcentaje de IVA aplicado a la tarifa
                                    entModalidad.PorcIva = entModalidad.ChargeCode == "VBABET" ? Convert.ToByte(16) : Convert.ToByte(porIVA);

                                    montoFeeMod = entModalidad.ChargeAmount;
                                    ivaMontoFee = entModalidad.ChargeCode == "VBABET" ? Math.Round((montoFeeMod * (decimal)0.16), 2) : Math.Round((montoFeeMod * ivaAplicado), 2);
                                    if (ivaMontoFee > montoIVA && ivaMontoFee > 0)
                                    {
                                        ivaMontoFee = montoIVA;
                                    }
                                    else
                                    if (Math.Abs(ivaMontoFee - montoIVA) <= .02M)
                                    {
                                        ivaMontoFee = montoIVA;
                                    }
                                    //LCI. INI. 2018-06-29 SE OMITE REGLA DE ULTIMA FILA, PORQUE EN OCASIONES EL IVA NO SE CALCULA PARA TODOS LOS COMPONENTES DE LA TARIFA Y SOLO SE LLEGA HASTA DONDE ALCANCE
                                    //else if (Math.Abs(ivaMontoFee - montoIVA) <= .02M && numFilaActual == numFilas)
                                    //{
                                    //    ivaMontoFee = montoIVA;
                                    //}
                                    //LCI. FIN 2018-06-29


                                    //Actualiza el monto total del IVA
                                    montoIVA = montoIVA - ivaMontoFee;

                                    //Se incrementa el ordenamiento de los Fees para identificar la secuencia de asignacion al pago.

                                    ENTReservaDet entIVAFeeMod = listaCompTarifa.Where(x => x.PassengerID == pax && x.SegmentID == segmentoID && x.FeeNumber == feeNumber && x.ClasFact == 4).FirstOrDefault();
                                    if (entIVAFeeMod != null)
                                    {
                                        entIVAFeeMod.PorcIva = Convert.ToByte(porIVA);
                                        entIVAFeeMod.ChargeAmount = ivaMontoFee;
                                        entIVAFeeMod.IdConcepto = entModalidad.IdConcepto;
                                    }
                                    else
                                    {

                                        ENTReservaDet entDetIvaFeeMod = new ENTReservaDet();
                                        ENTReservaDet entAClonar = new ENTReservaDet();
                                        if (entModalidad.ChargeCode == "VBABET")
                                        {
                                            entAClonar = lsteliminar[c];
                                            c++;
                                        }
                                        else
                                        {
                                            entAClonar = entIvaTarifa;
                                        }
                                        if (entAClonar != null)
                                        {
                                            ClonarReservaDet(ref entDetIvaFeeMod, entAClonar);
                                            entDetIvaFeeMod.FeeNumber = feeNumber;
                                            entDetIvaFeeMod.ChargeAmount = ivaMontoFee;
                                            entDetIvaFeeMod.PorcIva = entModalidad.ChargeCode == "VBABET" ? Convert.ToByte((decimal)16.00) : Convert.ToByte(porIVA);
                                            entDetIvaFeeMod.IdConcepto = entModalidad.IdConcepto;
                                            entDetIvaFeeMod.PassengerID = entModalidad.PassengerID;
                                            entDetIvaFeeMod.SegmentID = entModalidad.SegmentID;
                                            listaIvasAgregados.Add(entDetIvaFeeMod);
                                        }
                                    }

                                    numFilas++;
                                }
                                //}
                            }
                        }
                    }

                }

                //Eliminar los registros de IVA del RHS que se duplican
                foreach (ENTReservaDet item in lsteliminar)
                {
                    listaCompTarifa.Remove(item);
                }

                result.AddRange(listaCompTarifa);
                result.AddRange(listaIvasAgregados);
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "AsignarIVAPorTarifa", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;
        }



        private List<ENTReservaDet> AsignarIVAPorTarifaPorSegmento(List<ENTReservaDet> listaCompTarifa)
        {
            List<ENTReservaDet> result = new List<ENTReservaDet>();
            List<ENTReservaDet> listaIvasAgregados = new List<ENTReservaDet>();
            List<ENTReservaDet> listaUnificada = new List<ENTReservaDet>();

            try
            {
                //LCI. INI. CAMBIAR CALCULO DEL IVA PASANDO DE SEGMENTO A JOURNEY
                var segmentos = (from x in listaCompTarifa
                                 select x.SegmentID).Distinct();


                //Se recorre cada uno de los segmentos para identificar los importes de tarifa + modalidad y el importe global del IVA
                foreach (long segmentoID in segmentos)
                {
                    decimal montoTarifaConMod = 0;
                    decimal montoTarifaSinMod = 0;
                    decimal montoIVA = 0;
                    bool modalidadConIVA = true;
                    decimal porIVA = 0;

                    //Verifica si existe una fila de IVA para el segmento que se esta revisando
                    if (listaCompTarifa.Where(x => x.SegmentID == segmentoID && x.ClasFact == 4).Count() > 0)
                    {

                        montoTarifaConMod = listaCompTarifa.Where(x => x.SegmentID == segmentoID && (x.ClasFact == 2 || x.ClasFact == 3)).Sum(x => x.ChargeAmount);
                        montoIVA = listaCompTarifa.Where(x => x.SegmentID == segmentoID && x.ClasFact == 4).Sum(x => x.ChargeAmount);
                        montoTarifaSinMod = listaCompTarifa.Where(x => x.SegmentID == segmentoID && x.ClasFact == 3 && (x.ChargeType == 0 || x.ChargeType == 1) && x.ChargeCode.Length == 0).Sum(x => x.ChargeAmount);


                        if (montoTarifaConMod != 0)
                        {
                            porIVA = Math.Round(((((montoTarifaConMod + montoIVA) - montoTarifaConMod) / montoTarifaConMod) * 100), 0);

                        }
                        else
                        {
                            porIVA = 0;
                        }


                        if (!ListaPorcIVAValidos.Contains(porIVA))
                        {
                            Dictionary<decimal, decimal> listaDifIVA = new Dictionary<decimal, decimal>();

                            foreach (decimal ivaVal in ListaPorcIVAValidos)
                            {
                                if (ivaVal > 0)
                                {
                                    decimal dif = 0;
                                    dif = Math.Abs(ivaVal - porIVA);
                                    listaDifIVA.Add(ivaVal, dif);
                                }
                            }

                            decimal porIVAVal = 0;
                            porIVAVal = listaDifIVA.OrderBy(x => x.Value).Select(x => x.Key).FirstOrDefault();
                            if (ListaPorcIVAValidos.Contains(porIVAVal))
                            {
                                porIVA = porIVAVal;
                                modalidadConIVA = false;

                            }
                        }

                        //Registra el porcentaje de IVA aplicado
                        if (!ListaPorcIVAAplicados.Contains(porIVA) && ListaPorcIVAValidos.Contains(porIVA))
                        {
                            ListaPorcIVAAplicados.Add(porIVA);
                        }



                        decimal montoTarifa = 0;
                        decimal ivaSoloTarifa = 0;


                        List<ENTReservaDet> listaDetalleTar = new List<ENTReservaDet>();
                        listaDetalleTar = listaCompTarifa.Where(x => x.SegmentID == segmentoID && x.ClasFact == 3 && (x.ChargeType == 0 || x.ChargeType == 1) && x.ChargeCode.Length == 0).ToList();
                        foreach (ENTReservaDet entTar in listaDetalleTar)
                        {
                            entTar.PorcIva = Convert.ToByte(porIVA);
                        }


                        ENTReservaDet entTarifa = new ENTReservaDet();
                        entTarifa = listaCompTarifa.Where(x => x.SegmentID == segmentoID && x.ClasFact == 3 && (x.ChargeType == 0 || x.ChargeType == 1) && x.ChargeCode.Length == 0).FirstOrDefault();

                        montoTarifa = listaCompTarifa.Where(x => x.SegmentID == segmentoID && x.ClasFact == 3 && (x.ChargeType == 0 || x.ChargeType == 1) && x.ChargeCode.Length == 0).Sum(x => x.ChargeAmount);

                        decimal ivaAplicado = porIVA / 100;
                        ivaSoloTarifa = Math.Round((montoTarifa * ivaAplicado), 2);



                        ENTReservaDet entIvaTarifa = new ENTReservaDet();
                        entIvaTarifa = listaCompTarifa.Where(x => x.SegmentID == segmentoID && x.ClasFact == 4).FirstOrDefault();
                        entIvaTarifa.ChargeAmount = ivaSoloTarifa;
                        entIvaTarifa.PorcIva = Convert.ToByte(porIVA);
                        entIvaTarifa.IdConcepto = entTarifa.IdConcepto;


                        montoIVA = montoIVA - ivaSoloTarifa;

                        if (modalidadConIVA)
                        {
                            var listaCompTar = listaCompTarifa.Where(x => x.SegmentID == segmentoID && x.ChargeType != 0 && x.ChargeCode.Length > 0 && (x.ClasFact == 2 || x.ClasFact == 3)).OrderBy(x => x.ChargeAmount);


                            int numFilas = listaCompTar.Count();
                            int numFilaActual = 0;


                            foreach (ENTReservaDet entModalidad in listaCompTar)
                            {
                                numFilaActual++;
                                decimal montoFeeMod = 0;
                                decimal ivaMontoFee = 0;
                                int feeNumber = 0;
                                feeNumber = entModalidad.FeeNumber;

                                //Asigna el porcentaje de IVA aplicado a la tarifa
                                entModalidad.PorcIva = Convert.ToByte(porIVA);

                                montoFeeMod = entModalidad.ChargeAmount;
                                ivaMontoFee = Math.Round((montoFeeMod * ivaAplicado), 2);
                                if (ivaMontoFee > montoIVA && ivaMontoFee > 0)
                                {
                                    ivaMontoFee = montoIVA;
                                }
                                else if (Math.Abs(ivaMontoFee - montoIVA) <= .02M && numFilaActual == numFilas)
                                {
                                    ivaMontoFee = montoIVA;
                                }



                                //Actualiza el monto total del IVA
                                montoIVA = montoIVA - ivaMontoFee;

                                //Se incrementa el ordenamiento de los Fees para identificar la secuencia de asignacion al pago.

                                ENTReservaDet entIVAFeeMod = listaCompTarifa.Where(x => x.SegmentID == segmentoID && x.FeeNumber == feeNumber && x.ClasFact == 4).FirstOrDefault();
                                if (entIVAFeeMod != null)
                                {
                                    entIVAFeeMod.PorcIva = Convert.ToByte(porIVA);
                                    entIVAFeeMod.ChargeAmount = ivaMontoFee;
                                    entIVAFeeMod.IdConcepto = entModalidad.IdConcepto;
                                }
                                else
                                {

                                    ENTReservaDet entDetIvaFeeMod = new ENTReservaDet();
                                    ClonarReservaDet(ref entDetIvaFeeMod, entIvaTarifa);
                                    entDetIvaFeeMod.FeeNumber = feeNumber;
                                    entDetIvaFeeMod.ChargeAmount = ivaMontoFee;
                                    entDetIvaFeeMod.PorcIva = Convert.ToByte(porIVA);
                                    entDetIvaFeeMod.IdConcepto = entModalidad.IdConcepto;
                                    listaIvasAgregados.Add(entDetIvaFeeMod);

                                }


                            }
                        }
                    }
                }

                result.AddRange(listaCompTarifa);
                result.AddRange(listaIvasAgregados);
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "AsignarIVAPorTarifa", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;
        }


        private List<ENTReservaDet> AsignarIVAPorTarifaPorJourney(List<ENTReservaDet> listaCompTarifa)
        {
            List<ENTReservaDet> result = new List<ENTReservaDet>();
            List<ENTReservaDet> listaIvasAgregados = new List<ENTReservaDet>();
            List<ENTReservaDet> listaUnificada = new List<ENTReservaDet>();

            try
            {
                //LCI. INI. CAMBIAR CALCULO DEL IVA PASANDO DE SEGMENTO A JOURNEY
                var journeys = (from x in listaCompTarifa
                                select x.NumJourney).Distinct();


                //Se recorre cada uno de los segmentos para identificar los importes de tarifa + modalidad y el importe global del IVA
                foreach (byte journey in journeys)
                {
                    decimal montoTarifaConMod = 0;
                    decimal montoTarifaSinMod = 0;
                    decimal montoIVA = 0;
                    bool modalidadConIVA = true;
                    decimal porIVA = 0;

                    //Verifica si existe una fila de IVA para el segmento que se esta revisando
                    if (listaCompTarifa.Where(x => x.NumJourney == journey && x.ClasFact == 4).Count() > 0)
                    {

                        montoTarifaConMod = listaCompTarifa.Where(x => x.NumJourney == journey && (x.ClasFact == 2 || x.ClasFact == 3)).Sum(x => x.ChargeAmount);
                        montoIVA = listaCompTarifa.Where(x => x.NumJourney == journey && x.ClasFact == 4).Sum(x => x.ChargeAmount);
                        montoTarifaSinMod = listaCompTarifa.Where(x => x.NumJourney == journey && x.ClasFact == 3 && (x.ChargeType == 0 || x.ChargeType == 1) && x.ChargeCode.Length == 0).Sum(x => x.ChargeAmount);


                        if (montoTarifaConMod != 0)
                        {
                            porIVA = Math.Round(((((montoTarifaConMod + montoIVA) - montoTarifaConMod) / montoTarifaConMod) * 100), 0);

                        }
                        else
                        {
                            porIVA = 0;
                        }


                        if (!ListaPorcIVAValidos.Contains(porIVA))
                        {
                            Dictionary<decimal, decimal> listaDifIVA = new Dictionary<decimal, decimal>();

                            foreach (decimal ivaVal in ListaPorcIVAValidos)
                            {
                                if (ivaVal > 0)
                                {
                                    decimal dif = 0;
                                    dif = Math.Abs(ivaVal - porIVA);
                                    listaDifIVA.Add(ivaVal, dif);
                                }
                            }

                            decimal porIVAVal = 0;
                            porIVAVal = listaDifIVA.OrderBy(x => x.Value).Select(x => x.Key).FirstOrDefault();
                            if (ListaPorcIVAValidos.Contains(porIVAVal))
                            {
                                porIVA = porIVAVal;
                                modalidadConIVA = false;

                            }
                        }

                        //Registra el porcentaje de IVA aplicado
                        if (!ListaPorcIVAAplicados.Contains(porIVA) && ListaPorcIVAValidos.Contains(porIVA))
                        {
                            ListaPorcIVAAplicados.Add(porIVA);
                        }



                        decimal montoTarifa = 0;
                        decimal ivaSoloTarifa = 0;


                        List<ENTReservaDet> listaDetalleTar = new List<ENTReservaDet>();
                        listaDetalleTar = listaCompTarifa.Where(x => x.NumJourney == journey && x.ClasFact == 3 && (x.ChargeType == 0 || x.ChargeType == 1) && x.ChargeCode.Length == 0).ToList();
                        foreach (ENTReservaDet entTar in listaDetalleTar)
                        {
                            entTar.PorcIva = Convert.ToByte(porIVA);
                        }


                        ENTReservaDet entTarifa = new ENTReservaDet();
                        entTarifa = listaCompTarifa.Where(x => x.NumJourney == journey && x.ClasFact == 3 && (x.ChargeType == 0 || x.ChargeType == 1) && x.ChargeCode.Length == 0).FirstOrDefault();

                        montoTarifa = listaCompTarifa.Where(x => x.NumJourney == journey && x.ClasFact == 3 && (x.ChargeType == 0 || x.ChargeType == 1) && x.ChargeCode.Length == 0).Sum(x => x.ChargeAmount);

                        decimal ivaAplicado = porIVA / 100;
                        ivaSoloTarifa = Math.Round((montoTarifa * ivaAplicado), 2);



                        ENTReservaDet entIvaTarifa = new ENTReservaDet();
                        entIvaTarifa = listaCompTarifa.Where(x => x.NumJourney == journey && x.ClasFact == 4).FirstOrDefault();
                        entIvaTarifa.ChargeAmount = ivaSoloTarifa;
                        entIvaTarifa.PorcIva = Convert.ToByte(porIVA);
                        entIvaTarifa.IdConcepto = entTarifa.IdConcepto;


                        montoIVA = montoIVA - ivaSoloTarifa;

                        if (modalidadConIVA)
                        {
                            var listaCompTar = listaCompTarifa.Where(x => x.NumJourney == journey && x.ChargeType != 0 && x.ChargeCode.Length > 0 && (x.ClasFact == 2 || x.ClasFact == 3)).OrderBy(x => x.ChargeAmount);


                            int numFilas = listaCompTar.Count();
                            int numFilaActual = 0;


                            foreach (ENTReservaDet entModalidad in listaCompTar)
                            {
                                numFilaActual++;
                                decimal montoFeeMod = 0;
                                decimal ivaMontoFee = 0;
                                int feeNumber = 0;
                                feeNumber = entModalidad.FeeNumber;

                                //Asigna el porcentaje de IVA aplicado a la tarifa
                                entModalidad.PorcIva = Convert.ToByte(porIVA);

                                montoFeeMod = entModalidad.ChargeAmount;
                                ivaMontoFee = Math.Round((montoFeeMod * ivaAplicado), 2);
                                if (ivaMontoFee > montoIVA && ivaMontoFee > 0)
                                {
                                    ivaMontoFee = montoIVA;
                                }
                                else if (Math.Abs(ivaMontoFee - montoIVA) <= .02M && numFilaActual == numFilas)
                                {
                                    ivaMontoFee = montoIVA;
                                }



                                //Actualiza el monto total del IVA
                                montoIVA = montoIVA - ivaMontoFee;

                                //Se incrementa el ordenamiento de los Fees para identificar la secuencia de asignacion al pago.

                                ENTReservaDet entIVAFeeMod = listaCompTarifa.Where(x => x.NumJourney == journey && x.FeeNumber == feeNumber && x.ClasFact == 4).FirstOrDefault();
                                if (entIVAFeeMod != null)
                                {
                                    entIVAFeeMod.PorcIva = Convert.ToByte(porIVA);
                                    entIVAFeeMod.ChargeAmount = ivaMontoFee;
                                    entIVAFeeMod.IdConcepto = entModalidad.IdConcepto;
                                }
                                else
                                {

                                    ENTReservaDet entDetIvaFeeMod = new ENTReservaDet();
                                    ClonarReservaDet(ref entDetIvaFeeMod, entIvaTarifa);
                                    entDetIvaFeeMod.FeeNumber = feeNumber;
                                    entDetIvaFeeMod.ChargeAmount = ivaMontoFee;
                                    entDetIvaFeeMod.PorcIva = Convert.ToByte(porIVA);
                                    entDetIvaFeeMod.IdConcepto = entModalidad.IdConcepto;
                                    listaIvasAgregados.Add(entDetIvaFeeMod);

                                }


                            }
                        }
                    }
                }

                result.AddRange(listaCompTarifa);
                result.AddRange(listaIvasAgregados);
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "AsignarIVAPorTarifa", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;
        }

        private List<ENTReservaDet> AsignarIVAPorSSR(List<ENTReservaDet> listaCompSSR)
        {
            List<ENTReservaDet> result = new List<ENTReservaDet>();
            List<ENTReservaDet> listaIvasAgregados = new List<ENTReservaDet>();
            List<ENTReservaDet> listaUnificada = new List<ENTReservaDet>();

            try
            {
                DataTable dtCompSSR = new DataTable();
                dtCompSSR = Comun.Utils.Diversos.ToDataTable(listaCompSSR);

                var listaFees = (from x in listaCompSSR
                                 orderby x.FeeNumber
                                 select x.FeeNumber).Distinct();

                //Se recorre cada uno de los segmentos para identificar los importes de tarifa + modalidad y el importe global del IVA
                foreach (long feeNumber in listaFees)
                {
                    //Se recorren los Fees por pasajero
                    var listaFeePorPax = listaCompSSR.Where(x => x.FeeNumber == feeNumber).OrderBy(x => x.PassengerID).Select(x => x.PassengerID).Distinct();

                    foreach (long paxId in listaFeePorPax)
                    {
                        decimal montoFee = 0;
                        decimal montoIVAFee = 0;
                        decimal porIVAFee = 0;

                        int numFilasIVA = 0;

                        numFilasIVA = listaCompSSR.Where(x => x.FeeNumber == feeNumber && x.PassengerID == paxId && x.ClasFact == 4).Count();

                        //Se deben identificar si vienen diferente codigos de IVA y procesarse de forma independiente
                        List<string> listaCodigosIVA = new List<string>();
                        listaCodigosIVA = listaCompSSR.Where(x => x.FeeNumber == feeNumber && x.PassengerID == paxId && x.ClasFact == 4).Select(x => x.ChargeCode).Distinct().ToList();

                        montoFee = listaCompSSR.Where(x => x.FeeNumber == feeNumber && x.PassengerID == paxId && x.ClasFact != 4).Sum(x => x.ChargeAmount);


                        //montoIVAFee = listaCompSSR.Where(x => x.FeeNumber == feeNumber && x.PassengerID == paxId && x.ClasFact == 4).Sum(x => x.ChargeAmount);

                        foreach (string codigoIVA in listaCodigosIVA)
                        {
                            montoIVAFee = listaCompSSR.Where(x => x.FeeNumber == feeNumber && x.PassengerID == paxId && x.ClasFact == 4 && x.ChargeCode == codigoIVA).Sum(x => x.ChargeAmount);

                            if (montoFee != 0 && numFilasIVA > 0)
                            {
                                //Se realiza el calculo del porcentaje de IVA aplicado al Fee
                                porIVAFee = Math.Round(((((montoFee + montoIVAFee) - montoFee) / montoFee) * 100), 0);


                                if (montoIVAFee < 1 && !ListaPorcIVAValidos.Contains(porIVAFee))
                                {
                                    foreach (decimal porcIvaRev in ListaPorcIVAAplicados.OrderByDescending(x => x))
                                    {
                                        decimal montoIVAnuevo = 0;
                                        montoIVAnuevo = Math.Round((montoFee * (porcIvaRev / 100)), 2);
                                        if (Math.Abs(montoIVAFee - montoIVAnuevo) <= .02m)
                                        {
                                            porIVAFee = porcIvaRev;
                                        }
                                    }

                                    if (!ListaPorcIVAValidos.Contains(porIVAFee))
                                    {
                                        foreach (decimal porcIvaRev in ListaPorcIVAValidos.OrderByDescending(x => x))
                                        {
                                            decimal montoIVAnuevo = 0;
                                            montoIVAnuevo = Math.Round((montoFee * (porcIvaRev / 100)), 2);
                                            if (Math.Abs(montoIVAFee - montoIVAnuevo) < .02m)
                                            {
                                                porIVAFee = porcIvaRev;
                                            }
                                        }
                                    }

                                }

                                //Registra el porcentaje de IVA aplicado
                                if (!ListaPorcIVAAplicados.Contains(porIVAFee) && ListaPorcIVAValidos.Contains(porIVAFee))
                                {
                                    ListaPorcIVAAplicados.Add(porIVAFee);
                                }

                                decimal ivaAplicadoFee = porIVAFee / 100;

                                //Se recuperan las filas del Fee por pasajero para distribuir el importe de IVA en caso de ser necesario
                                var listaCompFees = listaCompSSR.Where(x => x.FeeNumber == feeNumber && x.PassengerID == paxId && x.ClasFact != 4);
                                var listaIVAFees = listaCompSSR.Where(x => x.FeeNumber == feeNumber && x.PassengerID == paxId && x.ClasFact == 4 && x.ChargeCode == codigoIVA);
                                int numFilas = listaCompFees.Count();
                                int numFilasIVAExistentes = listaIVAFees.Count();

                                if (numFilas == 1)
                                {
                                    if (numFilasIVAExistentes == 1)
                                    {
                                        //En caso de que solo sea una fila el que compone al Fee entonces solo se actualiza el porcentaje de IVA aplicado
                                        ENTReservaDet entIVAFeeMod = listaCompSSR.Where(x => x.FeeNumber == feeNumber && x.PassengerID == paxId && x.ClasFact == 4 && x.ChargeCode == codigoIVA).FirstOrDefault();
                                        if (entIVAFeeMod != null)
                                        {

                                            //Asigna el porcentaje de IVA aplicado en la fila del Fee
                                            ENTReservaDet entFeeMod = listaCompSSR.Where(x => x.FeeNumber == feeNumber && x.PassengerID == paxId && x.ClasFact != 4).FirstOrDefault();
                                            entFeeMod.PorcIva = Convert.ToByte(porIVAFee);

                                            //Asigna el porcentaje de IVA aplicado en la fila de IVA
                                            entIVAFeeMod.PorcIva = Convert.ToByte(porIVAFee);
                                            entIVAFeeMod.IdConcepto = entFeeMod.IdConcepto;
                                        }
                                    }
                                    else
                                    {
                                        ENTReservaDet entFeeModMultIVA = listaCompSSR.Where(x => x.FeeNumber == feeNumber && x.PassengerID == paxId && x.ClasFact != 4).FirstOrDefault();
                                        foreach (ENTReservaDet filaIVA in listaIVAFees)
                                        {
                                            //Se realiza el calculo del porcentaje de IVA aplicado al Fee
                                            porIVAFee = Math.Round(((((montoFee + filaIVA.ChargeAmount) - montoFee) / montoFee) * 100), 0);
                                            //Asigna el porcentaje de IVA aplicado en la fila del Fee
                                            entFeeModMultIVA.PorcIva = Convert.ToByte(porIVAFee);

                                            //Asigna el porcentaje de IVA aplicado en la fila de IVA
                                            filaIVA.PorcIva = Convert.ToByte(porIVAFee);
                                            filaIVA.IdConcepto = entFeeModMultIVA.IdConcepto;
                                        }


                                    }
                                }
                                else
                                {
                                    //En caso de que el componente este formado por mas de una fila entonces el IVA se distribuye y se genera una fila de iva por cada codigo
                                    int numFilaActual = 0;
                                    foreach (ENTReservaDet entFeePax in listaCompFees.OrderBy(x => x.ChargeAmount))
                                    {
                                        numFilaActual++;
                                        decimal montoFeePax = 0;
                                        decimal ivaMontoFeePax = 0;
                                        int feeNumberSSR = 0;
                                        feeNumberSSR = entFeePax.FeeNumber;

                                        montoFeePax = entFeePax.ChargeAmount;
                                        ivaMontoFeePax = Math.Round((montoFeePax * ivaAplicadoFee), 2);
                                        if (ivaMontoFeePax > montoIVAFee)
                                        {
                                            ivaMontoFeePax = montoIVAFee;
                                        }
                                        else if (Math.Abs(ivaMontoFeePax - montoIVAFee) <= .02M && numFilaActual == numFilas)
                                        {
                                            ivaMontoFeePax = montoIVAFee;
                                        }

                                        //Actualiza el monto total del IVA
                                        montoIVAFee = montoIVAFee - ivaMontoFeePax;


                                        if (ivaMontoFeePax != 0)
                                        {
                                            ENTReservaDet entIVAFeeMod = listaCompSSR.Where(x => x.FeeNumber == feeNumber && x.PassengerID == paxId && x.ClasFact == 4 && x.PorcIva == 0 && x.ChargeCode == codigoIVA).FirstOrDefault();
                                            entFeePax.PorcIva = Convert.ToByte(porIVAFee);

                                            if (entIVAFeeMod != null)
                                            {
                                                entIVAFeeMod.PorcIva = Convert.ToByte(porIVAFee);
                                                entIVAFeeMod.ChargeAmount = ivaMontoFeePax;
                                                entIVAFeeMod.IdConcepto = entFeePax.IdConcepto;
                                            }
                                            else
                                            {
                                                ENTReservaDet entIvaFeePax = listaCompSSR.Where(x => x.FeeNumber == feeNumber && x.PassengerID == paxId && x.ClasFact == 4 && x.PorcIva != 0 && x.ChargeCode == codigoIVA).FirstOrDefault();
                                                ENTReservaDet entDetIvaFeeSSR = new ENTReservaDet();
                                                ClonarReservaDet(ref entDetIvaFeeSSR, entIvaFeePax);
                                                entDetIvaFeeSSR.ChargeAmount = ivaMontoFeePax;
                                                entDetIvaFeeSSR.PorcIva = Convert.ToByte(porIVAFee);
                                                entDetIvaFeeSSR.IdConcepto = entFeePax.IdConcepto;

                                                listaIvasAgregados.Add(entDetIvaFeeSSR);

                                            }
                                        }

                                    }
                                }
                            }

                        }
                    }
                }

                result.AddRange(listaCompSSR);
                result.AddRange(listaIvasAgregados);
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "AsignarIVAPorSSR", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;
        }

        private void ClonarReservaDet(ref ENTReservaDet reservaDest, ENTReservaDet reservaDet)
        {
            try
            {


                reservaDest.IdReservaCab = reservaDet.IdReservaCab;
                reservaDest.IdReservaDet = reservaDet.IdReservaDet;
                reservaDest.Orden = reservaDet.Orden;
                reservaDest.FeeNumber = reservaDet.FeeNumber;
                reservaDest.PassengerID = reservaDet.PassengerID;
                reservaDest.SegmentID = reservaDet.SegmentID;
                reservaDest.ChargeNumber = reservaDet.ChargeNumber;
                reservaDest.ChargeType = reservaDet.ChargeType;
                reservaDest.IdFee = reservaDet.IdFee;
                reservaDest.ChargeCode = reservaDet.ChargeCode;
                reservaDest.ChargeDetail = reservaDet.ChargeDetail;
                reservaDest.TicketCode = reservaDet.TicketCode;
                reservaDest.CurrencyCode = reservaDet.CurrencyCode;
                reservaDest.ChargeAmount = reservaDet.ChargeAmount;
                reservaDest.ForeignCurrencyCode = reservaDet.ForeignCurrencyCode;
                reservaDest.ForeignAmount = reservaDet.ForeignAmount;
                reservaDest.FechaAplicaCompra = reservaDet.FechaAplicaCompra;
                reservaDest.PorcIva = reservaDet.PorcIva;
                reservaDest.TipoCargo = reservaDet.TipoCargo;
                reservaDest.TipoAcumulado = reservaDet.TipoAcumulado;
                reservaDest.IdPagosCab = reservaDet.IdPagosCab;
                reservaDest.EsPagoParcial = reservaDet.EsPagoParcial;
                reservaDest.MontoPagado = reservaDet.MontoPagado;
                reservaDest.EsFacturable = reservaDet.EsFacturable;
                reservaDest.EstatusFacturacion = reservaDet.EstatusFacturacion;
                reservaDest.IdFacturaCab = reservaDet.IdFacturaCab;
                reservaDest.FolioPreFactura = reservaDet.FolioPreFactura;
                reservaDest.ClasFact = reservaDet.ClasFact;
                reservaDest.IdConcepto = reservaDet.IdConcepto;
                reservaDest.IdFolioFacturaGlobal = reservaDet.IdFolioFacturaGlobal;
                reservaDest.IdVuelo = reservaDet.IdVuelo;
                reservaDest.NumJourney = reservaDet.NumJourney;
                reservaDest.LiftStatus = reservaDet.LiftStatus;
                reservaDest.CreatedAgentID = reservaDet.CreatedAgentID;
                reservaDest.CreatedDate = reservaDet.CreatedDate;
                reservaDest.FechaHoraLocal = reservaDet.FechaHoraLocal;
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "ClonarReservaDet", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }


        }

        public List<ENTVuelosCab> GeneraListaVuelos(DataTable dtVuelosCab)
        {
            List<ENTVuelosCab> result = new List<ENTVuelosCab>();
            try
            {


                long numVuelo = 0;

                foreach (DataRow dr in dtVuelosCab.Rows)
                {
                    long inventoryLegId = 0;
                    inventoryLegId = Convert.ToInt64(dr["InventoryLegId"]);

                    if (result.Where(x => x.InventoryLegId == inventoryLegId).Count() == 0)
                    {
                        numVuelo++;
                        ENTVuelosCab item = new ENTVuelosCab();
                        item.IdVuelo = numVuelo;
                        if (!dr.IsNull("InventoryLegId")) item.InventoryLegId = inventoryLegId;
                        if (!dr.IsNull("InventoryLegKey")) item.InventoryLegKey = dr["InventoryLegKey"].ToString();
                        if (!dr.IsNull("DepartureDate")) item.DepartureDate = Convert.ToDateTime(dr["DepartureDate"]);
                        if (!dr.IsNull("CarrierCode")) item.CarrierCode = dr["CarrierCode"].ToString();
                        if (!dr.IsNull("FlightNumber")) item.FlightNumber = dr["FlightNumber"].ToString();
                        if (!dr.IsNull("DepartureStation")) item.DepartureStation = dr["DepartureStation"].ToString();
                        if (!dr.IsNull("STD")) item.STD = Convert.ToDateTime(dr["STD"]);
                        if (!dr.IsNull("DepartureTerminal")) item.DepartureTerminal = dr["DepartureTerminal"].ToString();
                        if (!dr.IsNull("ArrivalStation")) item.ArrivalStation = dr["ArrivalStation"].ToString();
                        if (!dr.IsNull("STA")) item.STA = Convert.ToDateTime(dr["STA"]);
                        if (!dr.IsNull("ArrivalTerminal")) item.ArrivalTerminal = dr["ArrivalTerminal"].ToString();
                        if (!dr.IsNull("CityPair")) item.CityPair = dr["CityPair"].ToString();
                        result.Add(item);
                    }
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "GeneraListaVuelos", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;
        }


        public List<ENTReservaDet> OrdenaListaCargosPorPrioridad(List<ENTReservaDet> listaDetalleReservaTarifas,
            List<ENTReservaDet> listaDetalleReservaSSRs)
        {
            List<ENTReservaDet> result = new List<ENTReservaDet>();

            try
            {


                var listaOrdenadaTarifas = from x in listaDetalleReservaTarifas
                                           orderby x.IdReservaCab, x.FeeNumber, x.PassengerID, x.ChargeNumber
                                           select x;
                int ordenRevision = 0;
                long idPasajero = 0;
                int feeNum = 0;
                foreach (ENTReservaDet resDetOrden in listaOrdenadaTarifas)
                {
                    if (feeNum != resDetOrden.FeeNumber)
                    {
                        feeNum = resDetOrden.FeeNumber;
                        idPasajero = resDetOrden.PassengerID;
                        ordenRevision++;
                    }
                    else if (idPasajero != resDetOrden.PassengerID)
                    {
                        feeNum = resDetOrden.FeeNumber;
                        idPasajero = resDetOrden.PassengerID;
                        ordenRevision++;
                    }

                    resDetOrden.Orden = ordenRevision;
                    result.Add(resDetOrden);
                }

                var listaOrdenadaSSR = from x in listaDetalleReservaSSRs
                                       orderby x.IdReservaCab, x.FeeNumber, x.PassengerID
                                       select x;


                foreach (ENTReservaDet resDetOrdenSSR in listaOrdenadaSSR)
                {
                    if (feeNum != resDetOrdenSSR.FeeNumber)
                    {
                        feeNum = resDetOrdenSSR.FeeNumber;
                        idPasajero = resDetOrdenSSR.PassengerID;
                        ordenRevision++;
                    }
                    else if (idPasajero != resDetOrdenSSR.PassengerID)
                    {
                        feeNum = resDetOrdenSSR.FeeNumber;
                        idPasajero = resDetOrdenSSR.PassengerID;
                        ordenRevision++;
                    }

                    resDetOrdenSSR.Orden = ordenRevision;
                    result.Add(resDetOrdenSSR);
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "OrdenaListaCargosPorPrioridad", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;

        }



        private void BuscarVinculosExistentesEnBD(ref List<ENTPagosFacturados> listaPagos, ref List<ENTReservaDet> listaUnirTarifasYSSRs)
        {
            try
            {
                List<ENTPagosCab> listaPagosRegBD = new List<ENTPagosCab>();
                List<ENTReservaDet> listaReservaDetVinculada = new List<ENTReservaDet>();
                //Se recupera la informacion de los pagos que ya se encuentra registrados en la BD.
                foreach (var pagoFac in listaPagos)
                {
                    long paymentIdFac = pagoFac.PaymentID;
                    BLLPagosCab bllPagRegBD = new BLLPagosCab();
                    bllPagRegBD.RecuperarPagosCabPaymentid(paymentIdFac);
                    //BLLGlobalticketsDet bllGlobal = new BLLGlobalticketsDet();
                    //List<ENTGlobalticketsDet> listaGlobal = new List<ENTGlobalticketsDet>();
                    //listaGlobal = bllGlobal.RecuperarGlobalticketsDetPaymentid(paymentIdFac);
                    if (bllPagRegBD != null)
                    {
                        listaPagosRegBD.Add(bllPagRegBD);
                    }
                }

                //Se recuperan los registros del detalle existentes en la BD para cualquiera de los pagos que pertenecen a la reservacion
                foreach (ENTPagosCab pagoBD in listaPagosRegBD)
                {
                    List<ENTReservaDet> listaResDetVin = new List<ENTReservaDet>();
                    BLLReservaDet bllResDetVinRec = new BLLReservaDet();
                    listaResDetVin = bllResDetVinRec.RecuperarReservaDetIdpagoscab(pagoBD.IdPagosCab);
                    if (listaResDetVin.Count > 0)
                    {
                        listaReservaDetVinculada.AddRange(listaResDetVin);
                    }
                }

                //Se recorre la lista de cargos nueva para buscar si ya estan registrados y heredar la dependencia y los datos de facturacion o particionamiento

                if (listaReservaDetVinculada.Count > 0)
                {
                    List<int> listaOrden = new List<int>();
                    listaOrden = listaReservaDetVinculada.Select(x => x.Orden).Distinct().ToList();

                    //Se obtiene el orden de los cargos que se vincularon
                    foreach (int idOrden in listaOrden)
                    {
                        List<ENTReservaDet> listaCargosPorOrden = new List<ENTReservaDet>();
                        listaCargosPorOrden = listaReservaDetVinculada.Where(x => x.Orden == idOrden).ToList();

                        //Verifica si existen todos los cargos en la ultima version de la Reservacion
                        bool cargoCompleto = false;
                        foreach (ENTReservaDet cargoPorOrden in listaCargosPorOrden)
                        {
                            decimal montoCargo = 0;

                            montoCargo = (listaReservaDetVinculada.Where(x => x.IdReservaCab == cargoPorOrden.IdReservaCab
                                                                        && x.Orden == cargoPorOrden.Orden
                                                                        && x.PassengerID == cargoPorOrden.PassengerID
                                                                        && x.ChargeNumber == cargoPorOrden.ChargeNumber
                                                                        && x.ChargeType == cargoPorOrden.ChargeType
                                                                        && x.ChargeCode == cargoPorOrden.ChargeCode
                                                                        && x.TicketCode == cargoPorOrden.TicketCode
                                                                        && x.CurrencyCode == cargoPorOrden.CurrencyCode
                                                                        && x.NumJourney == cargoPorOrden.NumJourney
                                                                        && x.CreatedDate == cargoPorOrden.CreatedDate
                                                                        )).Sum(x => x.ChargeAmount);

                            int numCargosExis = 0;
                            numCargosExis = (listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == cargoPorOrden.IdReservaCab
                                                                        && x.PassengerID == cargoPorOrden.PassengerID
                                                                        && x.ChargeNumber == cargoPorOrden.ChargeNumber
                                                                        && x.ChargeType == cargoPorOrden.ChargeType
                                                                        && x.ChargeCode == cargoPorOrden.ChargeCode
                                                                        && x.TicketCode == cargoPorOrden.TicketCode
                                                                        && x.CurrencyCode == cargoPorOrden.CurrencyCode
                                                                        && x.NumJourney == cargoPorOrden.NumJourney
                                                                        && x.ChargeAmount == montoCargo
                                                                        && x.CreatedDate == cargoPorOrden.CreatedDate
                                                                        )).Count();
                            if (numCargosExis > 0)
                            {
                                cargoCompleto = true;
                            }
                            else
                            {
                                //En cuanto algun cargo no se encuentre completo entonces se sale del ciclo
                                cargoCompleto = false;
                                break;
                            }

                        }

                        //En caso de que todos los cargos registrados en la orden se encuentren disponibles entonces se procede a reasignarlo
                        if (cargoCompleto)
                        {
                            foreach (ENTReservaDet cargoPorOrden in listaCargosPorOrden)
                            {
                                bool esParcial = cargoPorOrden.EsPagoParcial;

                                //Verifica si se trata de un pago parcial
                                if (esParcial)
                                {

                                }
                                else
                                {
                                    //En caso de que no se encuentre particionado el cargo entonces se busca en la ultima version del PNR y se asigna

                                }

                            }


                        }


                    }



                    //List<ENTReservaDet> listaCargosParciales = new List<ENTReservaDet>();

                    ////Se realiza una copia de los componentes de navitaire para poder recorrerlos y agregar los componentes particionados
                    //List<ENTReservaDet> listaComponentesNavitaire = new List<ENTReservaDet>();
                    //listaComponentesNavitaire = listaUnirTarifasYSSRs.ToList();

                    //Recorre la lista de cargos ya asignados para buscarlos en la ultima version de cargos recuperada del booking
                    foreach (ENTReservaDet entResNueva in listaReservaDetVinculada.OrderBy(x => x.Orden))
                    {

                        decimal montoCargo = 0;
                        if (entResNueva.EsPagoParcial)
                        {

                        }
                        else
                        {
                            montoCargo = entResNueva.ChargeAmount;
                        }

                        /*
                        List<ENTReservaDet> entResExist = new List<ENTReservaDet>();
                        //Se compara si existe el componente entre los que ya estan vinculados en Facturacion
                        entResExist = (listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == entResNueva.IdReservaCab
                                                                        && x.PassengerID == entResNueva.PassengerID
                                                                        && x.SegmentID == entResNueva.SegmentID
                                                                        && x.ChargeNumber == entResNueva.ChargeNumber
                                                                        && x.ChargeType == entResNueva.ChargeType
                                                                        && x.ChargeCode == entResNueva.ChargeCode
                                                                        && x.TicketCode == entResNueva.TicketCode
                                                                        && x.CurrencyCode == entResNueva.CurrencyCode
                                                                        && x.CreatedDate == entResNueva.CreatedDate
                                                                        && x.ChargeAmount == montoCargo
                                                                        )).ToList();
                        if (entResExist.Count > 0)
                        {
                            ENTReservaDet primerDet = new ENTReservaDet();
                            primerDet = entResExist.FirstOrDefault();

                            int orden = primerDet.Orden;
                            if (primerDet.EsPagoParcial == false)
                            {
                                //El componente se vinculo completo al pago
                                long idpago = primerDet.IdPagosCab;
                                decimal montoComponente = 0;
                                decimal montoDisponiblePago = 0;
                                decimal montoComponenteBD = 0;
                                ENTPagosFacturados pago = listaPagos.Where(x => x.IdPagosCab == idpago).FirstOrDefault();
                                montoDisponiblePago = pago.MontoPorAplicar - pago.MontoAplicado;
                                montoComponente = listaUnirTarifasYSSRs.Where(x => x.Orden == orden && x.IdPagosCab == 0).Sum(x => x.ChargeAmount);
                                montoComponenteBD = listaReservaDetVinculada.Where(x => x.Orden == orden && x.IdPagosCab == idpago).Sum(x => x.ChargeAmount);

                                if (montoComponente == montoComponenteBD && montoDisponiblePago >= montoComponente)
                                {
                                    List<ENTReservaDet> listaComponentesOrden = new List<ENTReservaDet>();
                                    listaComponentesOrden = listaUnirTarifasYSSRs.Where(x => x.Orden == orden && x.IdPagosCab == 0).ToList();
                                    foreach (ENTReservaDet entCompoVin in listaComponentesOrden)
                                    {
                                        entCompoVin.EsFacturable = pago.EsFacturable;
                                        entCompoVin.EsPagoParcial = false;
                                        entCompoVin.EstatusFacturacion = pago.EsFacturado ? "FA" : "NF";
                                        entCompoVin.FechaAplicaCompra = pago.FechaPago;
                                        entCompoVin.FolioPreFactura = pago.FolioPrefactura;
                                        entCompoVin.IdFacturaCab = pago.IdFacturaCab;
                                        entCompoVin.IdFolioFacturaGlobal = 0;
                                        entCompoVin.IdPagosCab = pago.IdPagosCab;
                                        entCompoVin.MontoPagado = entCompoVin.ChargeAmount;
                                        pago.MontoAplicado += entCompoVin.ChargeAmount;
                                    }
                                }
                            }
                        }
                    }
                    */

                        //Al terminar de recorrer todos los componentes buscando los que ya existen en la base entonces
                        //Se agregan los que fueron particionados

                        //Se recorren los pagos para asignar los montos que ya se aplicaron
                        foreach (ENTPagosFacturados pagoVinculado in listaPagos)
                        {
                            decimal montoVinculado = 0;
                            montoVinculado = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pagoVinculado.IdPagosCab).Sum(x => x.ChargeAmount);
                            pagoVinculado.MontoAplicado = montoVinculado;
                        }


                    }
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "BuscarVinculosExistentesEnBD", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
        }




        private void GenerarVinculosPreExistentesEnBD(ref List<ENTPagosFacturados> listaPagos, ref List<ENTReservaDet> listaUnirTarifasYSSRs)
        {
            try
            {
                List<ENTPagosCab> listaPagosRegBD = new List<ENTPagosCab>();
                List<ENTReservaDet> listaReservaDetVinculada = new List<ENTReservaDet>();
                //Se recupera la informacion de los pagos que ya se encuentra registrados en la BD.
                foreach (var pagoFac in listaPagos)
                {
                    long paymentIdFac = pagoFac.PaymentID;
                    BLLPagosCab bllPagRegBD = new BLLPagosCab();
                    bllPagRegBD.RecuperarPagosCabPaymentid(paymentIdFac);
                    if (bllPagRegBD != null && bllPagRegBD.PaymentID > 0)
                    {
                        listaPagosRegBD.Add(bllPagRegBD);
                    }
                }

                //LCI. INI. 2018-07-26 VALIDAR SI EXISTEN ERRORES EN LA DISTRIBUCION ACTUAL, DE SER ASI SE OMITE LA RECUPERACION Y SE PROCESA DE NUEVO
                bool errorActual = false;

                foreach (ENTPagosCab pagoActual in listaPagosRegBD)
                {
                    if (pagoActual.MontoTarifa < 0 || pagoActual.MontoServAdic < 0 || pagoActual.MontoTUA < 0 || pagoActual.MontoOtrosCargos < 0 || pagoActual.MontoIVA < 0 || pagoActual.MontoTotal < 0)
                    {
                        errorActual = true;
                        break;
                    }
                }
                //LCI. FIN. 2018-07-26

                if (listaPagosRegBD.Count > 0 && errorActual == false)
                {

                    //Se recuperan los registros del detalle existentes en la BD para cualquiera de los pagos que pertenecen a la reservacion
                    foreach (ENTPagosCab pagoBD in listaPagosRegBD)
                    {
                        List<ENTReservaDet> listaResDetVin = new List<ENTReservaDet>();
                        BLLReservaDet bllResDetVinRec = new BLLReservaDet();
                        listaResDetVin = bllResDetVinRec.RecuperarReservaDetIdpagoscab(pagoBD.IdPagosCab);
                        if (listaResDetVin.Count > 0)
                        {
                            listaReservaDetVinculada.AddRange(listaResDetVin);
                        }
                    }


                    //Se reasigna el consecutivo de cada cargo para poder identificarlo al momento de asignar pago
                    int idDet = 0;
                    foreach (ENTReservaDet detOrdenar in listaUnirTarifasYSSRs)
                    {
                        idDet++;
                        detOrdenar.IdReservaDet = idDet;
                    }


                    //Se recorren los pagos existentes con monto por aplicar
                    for (int nivelBusqueda = 1; nivelBusqueda < 3; nivelBusqueda++)
                    {
                        RecuperarDetallePreRegistradoBD(listaPagosRegBD, nivelBusqueda, ref listaReservaDetVinculada, ref listaUnirTarifasYSSRs);
                    }


                    //Se recorren los pagos para asignar los montos que ya se aplicaron
                    foreach (ENTPagosFacturados pagoVinculado in listaPagos)
                    {
                        decimal montoVinculado = 0;
                        montoVinculado = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pagoVinculado.IdPagosCab).Sum(x => x.ChargeAmount);
                        pagoVinculado.MontoAplicado = montoVinculado;
                    }
                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "GenerarVinculosPreExistentesEnBD", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
        }

        private void RecuperarDetallePreRegistradoBD(List<ENTPagosCab> listaPagosRegBD, int nivelBusqueda, ref List<ENTReservaDet> listaReservaDetVinculada, ref List<ENTReservaDet> listaUnirTarifasYSSRs)
        {
            //Se reasigna el consecutivo de cada cargo para poder identificarlo al momento de asignar pago
            //int idDet = 0;
            //foreach (ENTReservaDet detOrdenar in listaUnirTarifasYSSRs)
            //{
            //    idDet++;
            //    detOrdenar.IdReservaDet = idDet;
            //}

            //Se recorren los pagos existentes con monto por aplicar
            foreach (ENTPagosCab pagoBD in listaPagosRegBD.Where(x => x.EsParaAplicar == true && x.MontoPorAplicar > 0))
            {
                //Identifica por numero de orden los grupos que deben asignarse en bloque
                List<int> listaOrden = new List<int>();
                listaOrden = listaReservaDetVinculada.Where(x => x.IdPagosCab == pagoBD.IdPagosCab).Select(x => x.Orden).Distinct().ToList();


                //Se recorre la lista de cargos ya registrados en la BD
                foreach (int idOrden in listaOrden)
                {
                    //Se busca si el mismo componente se encuentra en los cargos de la ultima modificacion a la reserva

                    List<ENTReservaDet> listaCargosOrden = new List<ENTReservaDet>();
                    listaCargosOrden = listaReservaDetVinculada.Where(x => x.Orden == idOrden && x.IdPagosCab == pagoBD.IdPagosCab).ToList();

                    //Se recorren todos los cargos que le corresponden a la orden y el pago vinculado
                    Dictionary<int, ENTReservaDet> listaCargosCoincidentes = new Dictionary<int, ENTReservaDet>();
                    foreach (ENTReservaDet dtCargosOrden in listaCargosOrden)
                    {
                        ENTReservaDet detNuevaVersion = new ENTReservaDet();
                        //Buscar el mismo componente dependiendo el nivel
                        switch (nivelBusqueda)
                        {
                            case 1:
                                detNuevaVersion = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == dtCargosOrden.IdReservaCab
                                                                                && x.PassengerID == dtCargosOrden.PassengerID
                                                                                && x.SegmentID == dtCargosOrden.SegmentID
                                                                                && x.ChargeNumber == dtCargosOrden.ChargeNumber
                                                                                && x.ChargeType == dtCargosOrden.ChargeType
                                                                                && x.ChargeCode == dtCargosOrden.ChargeCode
                                                                                && x.TicketCode == dtCargosOrden.TicketCode
                                                                                && x.CurrencyCode == dtCargosOrden.CurrencyCode
                                                                                && x.NumJourney == dtCargosOrden.NumJourney
                                                                                && x.ChargeAmount == dtCargosOrden.ChargeAmount
                                                                                && x.PorcIva == dtCargosOrden.PorcIva
                                                                                && x.CreatedDate == dtCargosOrden.CreatedDate
                                                                                && x.IdPagosCab == 0
                                                                                && !listaCargosCoincidentes.ContainsKey(x.IdReservaDet)
                                                                                ).FirstOrDefault();
                                break;
                            case 2:
                                detNuevaVersion = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == dtCargosOrden.IdReservaCab
                                                                                && x.PassengerID == dtCargosOrden.PassengerID
                                                                                && x.SegmentID == dtCargosOrden.SegmentID
                                                                                && x.ChargeNumber == dtCargosOrden.ChargeNumber
                                                                                && x.ChargeType == dtCargosOrden.ChargeType
                                                                                && x.ChargeCode == dtCargosOrden.ChargeCode
                                                                                && x.TicketCode == dtCargosOrden.TicketCode
                                                                                && x.CurrencyCode == dtCargosOrden.CurrencyCode
                                                                                && x.NumJourney == dtCargosOrden.NumJourney
                                                                                && x.ChargeAmount == dtCargosOrden.ChargeAmount
                                                                                && x.PorcIva == dtCargosOrden.PorcIva
                                                                                //&& x.CreatedDate == dtCargosOrden.CreatedDate
                                                                                && x.IdPagosCab == 0
                                                                                && !listaCargosCoincidentes.ContainsKey(x.IdReservaDet)
                                                                                ).FirstOrDefault();
                                break;
                            case 3:
                                detNuevaVersion = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == dtCargosOrden.IdReservaCab
                                                                                && x.PassengerID == dtCargosOrden.PassengerID
                                                                                //&& x.SegmentID == dtCargosOrden.SegmentID
                                                                                && x.ChargeNumber == dtCargosOrden.ChargeNumber
                                                                                && x.ChargeType == dtCargosOrden.ChargeType
                                                                                && x.ChargeCode == dtCargosOrden.ChargeCode
                                                                                && x.TicketCode == dtCargosOrden.TicketCode
                                                                                && x.CurrencyCode == dtCargosOrden.CurrencyCode
                                                                                && x.NumJourney == dtCargosOrden.NumJourney
                                                                                && x.ChargeAmount == dtCargosOrden.ChargeAmount
                                                                                && x.PorcIva == dtCargosOrden.PorcIva
                                                                                //&& x.CreatedDate == dtCargosOrden.CreatedDate
                                                                                && x.IdPagosCab == 0
                                                                                && !listaCargosCoincidentes.ContainsKey(x.IdReservaDet)
                                                                                ).FirstOrDefault();
                                break;
                            default:
                                break;
                        }

                        if (detNuevaVersion != null)
                        {
                            listaCargosCoincidentes.Add(detNuevaVersion.IdReservaDet, dtCargosOrden);
                        }
                        else
                        {
                            listaCargosCoincidentes.Clear();
                            break;
                        }
                    }
                    //En caso de que se encuentre el mismo cargo entonces se vincula
                    foreach (int idReservaActual in listaCargosCoincidentes.Keys)
                    {

                        ENTReservaDet detCargoActual = new ENTReservaDet();
                        ENTReservaDet detCargoRegBD = new ENTReservaDet();
                        detCargoActual = listaUnirTarifasYSSRs.Where(x => x.IdReservaDet == idReservaActual).FirstOrDefault();
                        detCargoRegBD = listaCargosCoincidentes[idReservaActual];

                        detCargoActual.IdPagosCab = detCargoRegBD.IdPagosCab;
                        detCargoActual.EstatusFacturacion = detCargoRegBD.EstatusFacturacion;
                        detCargoActual.FolioPreFactura = detCargoRegBD.FolioPreFactura;
                        detCargoActual.IdFacturaCab = detCargoRegBD.IdFacturaCab;
                        detCargoActual.IdFolioFacturaGlobal = detCargoRegBD.IdFolioFacturaGlobal;
                        detCargoActual.EsFacturable = detCargoRegBD.EsFacturable;

                        //Se elimina el cargo que ya se vinculo
                        listaReservaDetVinculada.RemoveAll(x => x.IdReservaDet == detCargoRegBD.IdReservaDet);

                    }

                }
            }
        }

        private void GenerarVinculosPagosComponentes(ref List<ENTPagosFacturados> listaPagos, ref List<ENTReservaDet> listaUnirTarifasYSSRs, List<ENTReservacion> listaReservas, string tipoVinculacion)
        {
            try
            {
                DataTable dtTarifas = new DataTable();

                //Recupera todos los pagos existentes y que tengan montos pendientes por aplicar
                var listaPagosPorAplicar = (from x in listaPagos
                                            where x.MontoPorAplicar > 0 && x.EsParaAplicar
                                            orderby x.BookingID, x.PaymentID
                                            select x);


                int contPagos = 0;
                long bookingId = 0;

                //Se recorre la lista de pagos identificados
                foreach (ENTPagosFacturados pago in listaPagosPorAplicar)
                {
                    //En caso de las reservaciones divididas pueden venir pagos de mas de un pnr por lo que se tiene que identificar si es el mismo
                    if (bookingId != pago.BookingID)
                    {
                        //En caso de que cambie el bookingid entonces se inicializa el contador de pagos procesados
                        contPagos = 0;
                        //Se actualiza el bookingID
                        bookingId = pago.BookingID;
                    }

                    //Se incrementa el contador de pagos para saber cuantos llevamos procesados
                    contPagos++;

                    //Con el bookingId del pago se recupera el Id de la reservacion, esto para tomar solo los detalles de la reserva a la que
                    //pertenece el pago
                    long idReservaCab = 0;
                    idReservaCab = listaReservas.Where(x => x.BookingID == pago.BookingID).FirstOrDefault().IdReservaCab;

                    //Se verifica cuantos pagos tiene por aplicar la reservacion para saber si este es el ultimo pago o quedan mas por recorrer
                    int numPagos = 0;
                    numPagos = listaPagosPorAplicar.Where(x => x.BookingID == pago.BookingID).Count();


                    //Se recuperan los montos totales por bloque existentes hasta el momento

                    decimal montoTarifaAct = listaUnirTarifasYSSRs.Where(x => x.TipoAcumulado == "TAR" && x.IdPagosCab == pago.IdPagosCab).Sum(x => x.ChargeAmount);
                    decimal montoServAdicAct = listaUnirTarifasYSSRs.Where(x => x.TipoAcumulado == "SVA" && x.IdPagosCab == pago.IdPagosCab).Sum(x => x.ChargeAmount);
                    decimal montoTuaAct = listaUnirTarifasYSSRs.Where(x => x.TipoAcumulado == "TUA" && x.IdPagosCab == pago.IdPagosCab).Sum(x => x.ChargeAmount);
                    decimal montoOtrosCAct = listaUnirTarifasYSSRs.Where(x => x.TipoAcumulado == "IMP" && x.IdPagosCab == pago.IdPagosCab).Sum(x => x.ChargeAmount);
                    decimal montoIVAAct = listaUnirTarifasYSSRs.Where(x => x.TipoAcumulado == "IVA" && x.IdPagosCab == pago.IdPagosCab).Sum(x => x.ChargeAmount);

                    //Se verifica si existe algun bloque con importe negativo para que tenga prioridad sobre la asignacion
                    Dictionary<string, decimal> listaPrioridadBloques = new Dictionary<string, decimal>();
                    bool acumNegativos = false;
                    acumNegativos = (montoTarifaAct < 0 || montoServAdicAct < 0 || montoTuaAct < 0 || montoOtrosCAct < 0 || montoIVAAct < 0);


                    //LCI. INI CORRECCION IMPORTES NEGATIVOS POR BLOQUE
                    List<ENTReservaDet> listaComponentesNeg = new List<ENTReservaDet>();
                    int numCargosNeg = 0;
                    listaComponentesNeg = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.IdPagosCab == 0 && x.ChargeAmount < 0).ToList();
                    numCargosNeg = listaComponentesNeg.Count();
                    if (numCargosNeg > 0)
                    {
                        tipoVinculacion = "SINVINCULO";
                    }
                    //LCI. FIN


                    //Recupera todos los numeros de ordenamiento de los componentes que aun no tengan un pago asignado
                    List<int> listaOrdenamiento = new List<int>();
                    List<ENTReservaDet> listaComponentes = new List<ENTReservaDet>();
                    int numCargos = 0;


                    if (tipoVinculacion == "EVENTO")
                    {
                        //Se recuperan las fechas que cubre el pago actual
                        DateTime fechaPagoIni = pago.FechaIniEvento;
                        DateTime fechaPagoFin = pago.FechaFinEvento;

                        //Se recuperan los componentes que cumplen con las fechas del evento y que pertenecen a la misma reservacion que el pago
                        listaComponentes = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.CreatedDate >= fechaPagoIni && x.CreatedDate <= fechaPagoFin && x.IdPagosCab == 0).ToList();
                        numCargos = listaComponentes.Count();
                    }
                    else if (tipoVinculacion == "SINVINCULO")
                    {
                        //Se recuperan los componentes que aun no tienen asignado un pago sin importar las fechas de creacion
                        listaComponentes = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.IdPagosCab == 0).ToList();
                        numCargos = listaComponentes.Count();
                    }

                    //Se verifica si entre los componentes existen importes negativos
                    bool existenPagosNeg = false;
                    existenPagosNeg = (listaComponentes.Where(x => x.TipoAcumulado != "IVA" && x.ChargeAmount <= 0).Count() > 0);

                    //En caso de que se trate del ultimo pago y que existan montos negativos 
                    //entonces se ordenaran dando prioridad a los componentes negativos o iguales a cero

                    //if ((contPagos == numPagos && existenPagosNeg) || acumNegativos)
                    //if ((contPagos == numPagos && existenPagosNeg) || ((tipoVinculacion == "EVENTO" && existenPagosNeg)) || acumNegativos)
                    if (existenPagosNeg || ((tipoVinculacion == "EVENTO" && existenPagosNeg)) || acumNegativos)
                    {
                        //Primero se colocan los componentes negativos o iguales a cero 
                        //para que no queden fuera de la asignacion cuando se cubra el monto del pago
                        var listaOrdenamientoMenorIgualCero = (from x in listaComponentes
                                                               where x.TipoAcumulado != "IVA" && x.ChargeAmount <= 0
                                                               orderby x.ChargeAmount
                                                               select x.Orden).Distinct();

                        //Se agregan a la lista de ordenamiento respetando el orden asignado por la consulta anterior
                        foreach (int ordenUP in listaOrdenamientoMenorIgualCero)
                        {
                            //Se verifica que no exista ya en la lista para no duplicarlos
                            if (!listaOrdenamiento.Contains(ordenUP))
                            {
                                listaOrdenamiento.Add(ordenUP);
                            }
                        }

                        //Se agregan los componentes con importe mayor a cero
                        List<int> listaOrdenamientoMayorCero = new List<int>();

                        //Se verifica si existen componentes de IVA con montos negativos que faltan por procesar
                        bool conIVANegativo = false;
                        conIVANegativo = (listaComponentes.Where(x => listaOrdenamiento.Contains(x.Orden) && x.TipoAcumulado == "IVA" && x.ChargeAmount <= 0).Count() > 0);

                        //Verifica si se tienen montos acumulados negativos o componentes pendientes por ordenar con importe negativo
                        if (conIVANegativo || acumNegativos)
                        {
                            //Se realiza el calculo de los montos negativos que ya se consideraron para procesar
                            decimal montoTarifaAsig = listaUnirTarifasYSSRs.Where(x => listaOrdenamiento.Contains(x.Orden) && x.TipoAcumulado == "TAR" && x.IdPagosCab == 0).Sum(x => x.ChargeAmount);
                            decimal montoServAdicAsig = listaUnirTarifasYSSRs.Where(x => listaOrdenamiento.Contains(x.Orden) && x.TipoAcumulado == "SVA" && x.IdPagosCab == 0).Sum(x => x.ChargeAmount);
                            decimal montoTuaAsig = listaUnirTarifasYSSRs.Where(x => listaOrdenamiento.Contains(x.Orden) && x.TipoAcumulado == "TUA" && x.IdPagosCab == 0).Sum(x => x.ChargeAmount);
                            decimal montoOtrosCAsig = listaUnirTarifasYSSRs.Where(x => listaOrdenamiento.Contains(x.Orden) && x.TipoAcumulado == "IMP" && x.IdPagosCab == 0).Sum(x => x.ChargeAmount);

                            //Se genera la lista de bloques unificando los montos y los totales negativos de los que estan por procesarse
                            listaPrioridadBloques.Add("TAR", montoTarifaAct + montoTarifaAsig);
                            listaPrioridadBloques.Add("TUA", montoTuaAct + montoTuaAsig);
                            listaPrioridadBloques.Add("IMP", montoOtrosCAct + montoOtrosCAsig);
                            listaPrioridadBloques.Add("SVA", montoServAdicAct + montoServAdicAsig);

                            //Se ordenan los bloques procesando primero los que tengan un importe negativo mayor
                            foreach (var bloque in listaPrioridadBloques.OrderBy(x => x.Value))
                            {
                                string codigo = bloque.Key;
                                //List<int> listaOrdenamientoPorBloque = (from x in listaComponentes
                                //                                        where x.TipoAcumulado == codigo && x.TipoAcumulado != "IVA" && x.ChargeAmount > 0
                                //                                        orderby x.Orden
                                //                                        select x.Orden).Distinct().ToList();

                                List<int> listaOrdenamientoPorBloque = (from x in listaComponentes
                                                                        where x.TipoAcumulado == codigo
                                                                        orderby x.ChargeAmount
                                                                        select x.Orden).Distinct().ToList();

                                //List<int> listaOrdenamientoPorBloque = (from x in listaUnirTarifasYSSRs
                                //                                        where x.IdReservaCab == idReservaCab && x.IdPagosCab == 0 && x.TipoAcumulado == codigo//&& x.ChargeAmount > 0
                                //                                        orderby x.ChargeAmount
                                //                                        select x.Orden).Distinct().ToList();

                                //Se incluyen los numeros de ordenamiento en funcion al bloque 
                                foreach (int ordenUP in listaOrdenamientoPorBloque)
                                {
                                    if (!listaOrdenamiento.Contains(ordenUP))
                                    {
                                        listaOrdenamiento.Add(ordenUP);
                                    }
                                }
                            }
                        }
                        else
                        {
                            listaOrdenamientoMayorCero = (from x in listaComponentes
                                                          where x.TipoAcumulado != "IVA" && x.ChargeAmount > 0
                                                          orderby x.Orden
                                                          select x.Orden).Distinct().ToList();


                            foreach (int ordenUP in listaOrdenamientoMayorCero)
                            {
                                if (!listaOrdenamiento.Contains(ordenUP))
                                {
                                    listaOrdenamiento.Add(ordenUP);
                                }
                            }

                        }

                    }
                    else
                    {
                        //En caso de que no existan montos negativos o iguales a cero entonces se respeta el orden de creacion del componente
                        var listaOrdenamientoSinNegativosOCero = (from x in listaComponentes
                                                                  orderby x.Orden
                                                                  select x.Orden).Distinct();

                        foreach (int ordenUP in listaOrdenamientoSinNegativosOCero)
                        {
                            if (!listaOrdenamiento.Contains(ordenUP))
                            {
                                listaOrdenamiento.Add(ordenUP);
                            }
                        }
                    }

                    //Recorre los numeros de ordenamiento
                    int numCompPendientes = 0;
                    numCompPendientes = listaOrdenamiento.Count();
                    int conteo = 0;

                    foreach (int orden in listaOrdenamiento)
                    {
                        conteo++;
                        //Recupera el componente principal del fee es decir el que no corresponde a un IVA
                        ENTReservaDet compPrin = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.Orden == orden && x.ClasFact != 4 && x.IdPagosCab == 0).FirstOrDefault();
                        ENTReservaDet compIVA = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.Orden == orden && x.ClasFact == 4 && x.IdPagosCab == 0).FirstOrDefault();

                        //Obtiene el monto total del cargo incluyendo el IVA
                        decimal importeFee = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.Orden == orden && x.IdPagosCab == 0).Sum(x => x.ChargeAmount);

                        if (pago.MontoPorAplicar == pago.MontoAplicado && importeFee != 0
                             &&
                            //AUN QUEDAN PAGOS PENDIENTES Y SU SUMATORIA ES IGUAL A CERO
                            !(
                            //Es el ultimo pago
                            (numPagos == contPagos)
                            &&
                            //La sumatoria de los pagos restantes es igual a cero
                            (listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.IdPagosCab == 0).Sum(x => x.ChargeAmount) <= 0)))
                        {
                            pago.MontoTarifa = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pago.IdPagosCab && x.TipoAcumulado == "TAR").Sum(x => x.ChargeAmount);
                            pago.MontoServAdic = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pago.IdPagosCab && x.TipoAcumulado == "SVA").Sum(x => x.ChargeAmount);
                            pago.MontoTUA = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pago.IdPagosCab && x.TipoAcumulado == "TUA").Sum(x => x.ChargeAmount);
                            pago.MontoOtrosCargos = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pago.IdPagosCab && x.TipoAcumulado == "IMP").Sum(x => x.ChargeAmount);
                            pago.MontoIVA = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pago.IdPagosCab && x.TipoAcumulado == "IVA").Sum(x => x.ChargeAmount);
                            pago.MontoTotal = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pago.IdPagosCab).Sum(x => x.ChargeAmount);
                            break;
                        }
                        else
                        {


                            //Obtiene el monto del cargo sin IVA
                            decimal montoFeeSinIVA = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.Orden == orden && x.ClasFact != 4 && x.IdPagosCab == 0).Sum(x => x.ChargeAmount);

                            //Obtiene el monto del IVA
                            decimal montoIVAFee = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaCab && x.Orden == orden && x.ClasFact == 4 && x.IdPagosCab == 0).Sum(x => x.ChargeAmount);

                            //Recupera el porcentaje del IVA asignado al componente
                            int porcIVA = 0;
                            if (montoIVAFee != 0)
                            {
                                porcIVA = compIVA.PorcIva;
                            }

                            //Identifica el tipo de acumulado que se utilizara para validar en caso de que el pago se encuentre facturado
                            string tipoAcum = compPrin.TipoAcumulado;

                            //Asigna el componente al pago
                            AsignaPago(importeFee, montoFeeSinIVA, montoIVAFee, ref listaUnirTarifasYSSRs, orden, pago, tipoAcum, idReservaCab);

                        }
                    }

                }


                dtTarifas = Comun.Utils.Diversos.ToDataTable(listaUnirTarifasYSSRs);
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "GenerarVinculosPagosComponentes", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            finally
            {
                //Se recorren los pagos para asignar los montos que ya se aplicaron
                foreach (ENTPagosFacturados pagoVinculado in listaPagos)
                {
                    decimal montoVinculado = 0;
                    montoVinculado = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pagoVinculado.IdPagosCab).Sum(x => x.ChargeAmount);
                    pagoVinculado.MontoAplicado = montoVinculado;
                }
            }

        }



        private void GenerarVinculosPagosFacturados(ref List<ENTPagosFacturados> listaPagos, ref List<ENTReservaDet> listaUnirTarifasYSSRs, List<ENTReservacion> listaReservas, bool ignorarMontosBloque)
        {
            try
            {

                //Recorre todos los pagos existentes y que tengan montos por aplicar
                List<ENTPagosFacturados> listaPagosPorAplicar = new List<ENTPagosFacturados>();

                //Se recuperan los pagos facturados
                listaPagosPorAplicar = (from x in listaPagos
                                        where x.MontoPorAplicar > 0 && x.EsParaAplicar && x.EsFacturado == true
                                        orderby x.BookingID, x.PaymentID
                                        select x).ToList();

                //Se identifican los foliosPreFactura con lo que se agrupan los pagos vinculados
                List<long> listaFoliosPrefactura = new List<long>();
                listaFoliosPrefactura = listaPagosPorAplicar.Where(x => x.FolioPrefactura > 0).Select(x => x.FolioPrefactura).Distinct().ToList();

                //Se asignan las prioridades de como se van a asignar los montos facturados
                Dictionary<int, string> listaPrioridadBloques = new Dictionary<int, string>();
                listaPrioridadBloques.Add(1, "TUA");
                listaPrioridadBloques.Add(2, "IMP");
                listaPrioridadBloques.Add(3, "TAR");
                listaPrioridadBloques.Add(4, "SVA");


                //Recorre los diferentes folios de prefactura
                foreach (long folioPreFacturaPagos in listaFoliosPrefactura)
                {
                    List<ENTPagosFacturados> listaPagosVinculados = new List<ENTPagosFacturados>();
                    listaPagosVinculados = listaPagosPorAplicar.Where(x => x.FolioPrefactura == folioPreFacturaPagos).ToList();

                    //Se recuperan los montos por bloque que se van a validar
                    ENTPagosFacturados pagoBaseMontos = new ENTPagosFacturados();
                    pagoBaseMontos = listaPagosVinculados.FirstOrDefault();

                    decimal montoTarifa = pagoBaseMontos.MontoTarifa;
                    decimal montoServAdic = pagoBaseMontos.MontoServAdic;
                    decimal montoTua = pagoBaseMontos.MontoTUA;
                    decimal montoOtrosC = pagoBaseMontos.MontoOtrosCargos;
                    decimal montoIVA = pagoBaseMontos.MontoIVA;


                    //Se calculan los montos que ya se encuentran vinculados
                    decimal montoTarifaVinc = listaUnirTarifasYSSRs.Where(x => x.TipoAcumulado == "TAR" && x.IdPagosCab > 0).Sum(x => x.ChargeAmount);
                    decimal montoServAdicVinc = listaUnirTarifasYSSRs.Where(x => x.TipoAcumulado == "SVA" && x.IdPagosCab > 0).Sum(x => x.ChargeAmount);
                    decimal montoTuaVinc = listaUnirTarifasYSSRs.Where(x => x.TipoAcumulado == "TUA" && x.IdPagosCab > 0).Sum(x => x.ChargeAmount);
                    decimal montoOtrosCVinc = listaUnirTarifasYSSRs.Where(x => x.TipoAcumulado == "IMP" && x.IdPagosCab > 0).Sum(x => x.ChargeAmount);
                    decimal montoIVAVinc = listaUnirTarifasYSSRs.Where(x => x.TipoAcumulado == "IVA" && x.IdPagosCab > 0).Sum(x => x.ChargeAmount);


                    Dictionary<string, decimal> listaMontosPorBloque = new Dictionary<string, decimal>();
                    listaMontosPorBloque.Add("TUA", montoTua);
                    listaMontosPorBloque.Add("IMP", montoOtrosC);
                    listaMontosPorBloque.Add("TAR", montoTarifa);
                    listaMontosPorBloque.Add("SVA", montoServAdic);
                    listaMontosPorBloque.Add("IVA", montoIVA);

                    Dictionary<string, decimal> listaMontosPorBloqueVin = new Dictionary<string, decimal>();
                    listaMontosPorBloqueVin.Add("TUA", montoTuaVinc);
                    listaMontosPorBloqueVin.Add("IMP", montoOtrosCVinc);
                    listaMontosPorBloqueVin.Add("TAR", montoTarifaVinc);
                    listaMontosPorBloqueVin.Add("SVA", montoServAdicVinc);
                    listaMontosPorBloqueVin.Add("IVA", montoIVAVinc);


                    foreach (string bloque in listaPrioridadBloques.OrderBy(x => x.Key).Select(x => x.Value))
                    {
                        //Se recorren los detalles de la reservacion que aun no se encuentran asignados
                        List<ENTReservaDet> listaComponentes = new List<ENTReservaDet>();
                        listaComponentes = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == 0 && x.TipoAcumulado == bloque).ToList();

                        //Genera la lista de orden que se va a procesar
                        List<string> listaOrdenamiento = new List<string>();
                        //Primero se colocan los componentes negativos
                        var listaOrdenamientoMenorCero = (from x in listaComponentes
                                                          where x.ChargeAmount <= 0
                                                          orderby x.ChargeAmount
                                                          select x).Distinct();

                        foreach (ENTReservaDet ordenUP in listaOrdenamientoMenorCero)
                        {
                            string reservOrden = ordenUP.IdReservaCab + "|" + ordenUP.Orden;
                            if (!listaOrdenamiento.Contains(reservOrden))
                            {
                                listaOrdenamiento.Add(reservOrden);
                            }
                        }

                        //Se incluyen los componentes con importe mayor a cero para ser procesados despues
                        var listaOrdenAcum = (from x in listaComponentes
                                              where x.IdPagosCab == 0 && x.ChargeAmount > 0
                                              orderby x.ChargeAmount, x.Orden
                                              select x).Distinct().ToList();

                        foreach (ENTReservaDet ordenUP in listaOrdenAcum)
                        {
                            string reservOrden = ordenUP.IdReservaCab + "|" + ordenUP.Orden;
                            if (!listaOrdenamiento.Contains(reservOrden))
                            {
                                listaOrdenamiento.Add(reservOrden);
                            }
                        }


                        //Se recorren los componentes en base al ordenamiento
                        foreach (string resOrden in listaOrdenamiento)
                        {
                            long idReservaComp = 0;
                            int orden = 0;
                            string[] datosResorden = resOrden.Split('|');
                            idReservaComp = Convert.ToInt64(datosResorden[0]);
                            orden = Convert.ToInt32(datosResorden[1]);
                            decimal montoSinIVA = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaComp && x.Orden == orden && x.IdPagosCab == 0 && x.TipoAcumulado != "IVA").Sum(x => x.ChargeAmount);
                            decimal montoSoloIVA = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaComp && x.Orden == orden && x.IdPagosCab == 0 && x.TipoAcumulado == "IVA").Sum(x => x.ChargeAmount);
                            decimal montoCompleto = listaUnirTarifasYSSRs.Where(x => x.IdReservaCab == idReservaComp && x.Orden == orden && x.IdPagosCab == 0).Sum(x => x.ChargeAmount);
                            decimal montoBloque = listaMontosPorBloque[bloque];
                            decimal montoBloqueIVA = listaMontosPorBloque["IVA"];

                            //long idReservaComp = listaComponentes.Where(x => x.IdReservaCab == idReserva && x.Orden == orden && x.IdPagosCab == 0 && x.TipoAcumulado != "IVA").Select(x => x.IdReservaCab).FirstOrDefault();
                            long bookingIdComp = listaReservas.Where(x => x.IdReservaCab == idReservaComp).Select(x => x.BookingID).FirstOrDefault();
                            ENTPagosFacturados pagoDisponible = listaPagosPorAplicar.Where(x => x.BookingID == bookingIdComp && x.MontoAplicado < x.MontoPorAplicar).FirstOrDefault();


                            decimal montoBloqueTarifaVinc = listaMontosPorBloqueVin[bloque];
                            decimal montoRestanteBloque = montoBloque - montoBloqueTarifaVinc;
                            decimal montoRestanteBloqueIVA = montoBloqueIVA - listaMontosPorBloqueVin["IVA"];

                            if (montoRestanteBloque > 0 || ignorarMontosBloque || montoCompleto == 0)
                            {
                                //Se validar el componente solo si existe algun pago que tenga monto pendiente por aplicar
                                if (pagoDisponible != null)
                                {
                                    decimal montoPagoDisponible = pagoDisponible.MontoPorAplicar - pagoDisponible.MontoAplicado;
                                    decimal montoAplicado = pagoDisponible.MontoAplicado;
                                    if (montoPagoDisponible > 0)
                                    {
                                        if (montoCompleto <= montoPagoDisponible && montoSinIVA <= montoRestanteBloque && montoSoloIVA <= montoRestanteBloqueIVA)
                                        {
                                            //En este caso entra el componente completo
                                            //En caso de que el componente no rebase el monto por bloque entonces se realizara la vinculacion del componente
                                            AsignaPago(montoCompleto, montoSinIVA, montoSoloIVA, montoPagoDisponible, montoRestanteBloque, ref listaUnirTarifasYSSRs, idReservaComp, orden, ref montoAplicado, pagoDisponible
                                            , bloque, ignorarMontosBloque, ref listaMontosPorBloqueVin, listaMontosPorBloque);
                                        }
                                        else
                                        {
                                            //En este caso entra el componente particionado
                                            //En caso de que el componente no rebase el monto por bloque entonces se realizara la vinculacion del componente
                                            AsignaPago(montoCompleto, montoSinIVA, montoSoloIVA, montoPagoDisponible, montoRestanteBloque, ref listaUnirTarifasYSSRs, idReservaComp, orden, ref montoAplicado, pagoDisponible
                                            , bloque, ignorarMontosBloque, ref listaMontosPorBloqueVin, listaMontosPorBloque);
                                        }
                                    }
                                }
                            }
                        }
                    }

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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "GenerarVinculosPagosFacturados", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }
            finally
            {
                //Se recorren los pagos para asignar los montos que ya se aplicaron
                foreach (ENTPagosFacturados pagoVinculado in listaPagos)
                {
                    decimal montoVinculado = 0;
                    montoVinculado = listaUnirTarifasYSSRs.Where(x => x.IdPagosCab == pagoVinculado.IdPagosCab).Sum(x => x.ChargeAmount);
                    pagoVinculado.MontoAplicado = montoVinculado;
                }
            }
        }


        public void ReasignarAjustesNegativos(decimal montoSSRGlobal, ref List<ENTReservaDet> listaDetalleReservaTarifas, ref List<ENTReservaDet> listaDetalleReservaSSRs)
        {
            try
            {


                decimal diferenciaSSR = montoSSRGlobal;
                List<ENTReservaDet> listaAjustes = listaDetalleReservaSSRs.Where(x => ListaCodigosAjuste.Contains(x.ChargeCode) && x.ChargeAmount < 0).ToList();
                List<int> listaOrdenComp = new List<int>();
                foreach (ENTReservaDet entResAjuste in listaAjustes)
                {
                    int orden = entResAjuste.Orden;
                    if (!listaOrdenComp.Contains(orden))
                    {
                        listaOrdenComp.Add(orden);
                    }
                }

                foreach (int ordenAjuNeg in listaOrdenComp)
                {
                    decimal impAjusteNeg = 0;
                    impAjusteNeg = listaDetalleReservaSSRs.Where(x => x.Orden == ordenAjuNeg).Sum(x => x.ChargeAmount);
                    if (Math.Abs(impAjusteNeg) <= Math.Abs(diferenciaSSR))
                    {
                        //List<ENTReservaDet> listaEliminarSSR = new List<ENTReservaDet>();
                        //Se cambia por completo el Ajuste a la tarifa
                        foreach (ENTReservaDet entResDetAjuNeg in listaDetalleReservaSSRs.Where(x => x.Orden == ordenAjuNeg))
                        {

                            if (entResDetAjuNeg.ChargeAmount != 0)
                            {
                                ENTReservaDet compTar = new ENTReservaDet();
                                ClonarReservaDet(ref compTar, entResDetAjuNeg);

                                //Se elimina el id de la reserva como una marca para identificar cuales elementos se van a eliminar
                                entResDetAjuNeg.IdReservaCab = 0;

                                int ordenTarifa = 0;
                                ordenTarifa = listaDetalleReservaTarifas.Max(x => x.Orden) + 1;

                                compTar.ClasFact = 3;
                                compTar.TipoAcumulado = "TAR";
                                compTar.TipoCargo = "TRAVELFEE";
                                compTar.IdConcepto = 1;
                                compTar.Orden = ordenTarifa;
                                compTar.FeeNumber = ordenTarifa;
                                diferenciaSSR -= compTar.ChargeAmount;
                                listaDetalleReservaTarifas.Add(compTar);
                            }
                        }

                        listaDetalleReservaSSRs.RemoveAll(x => x.IdReservaCab == 0);
                    }
                    else
                    {
                        //En caso de que el remanente del pago sea menor al importe del fee entonces se tiene que dividir
                        decimal montoSinIvaPorAplicar = 0;
                        decimal montoIVAPorAplicar = 0;
                        decimal montoSinIvaRestante = 0;
                        decimal montoIvaRestante = 0;
                        decimal porIVAPorDesglose = 0;

                        decimal montoFeeSinIVA = 0;
                        decimal montoIVAFee = 0;

                        ENTReservaDet componenteIVA = new ENTReservaDet();
                        //ENTReservaDet componentePrincipal = new ENTReservaDet();
                        ENTReservaDet componenteIVARemanente = new ENTReservaDet();


                        //Se recuperan los componentes principales
                        componenteIVA = listaDetalleReservaSSRs.Where(x => x.Orden == ordenAjuNeg && x.ClasFact == 4).FirstOrDefault();




                        montoFeeSinIVA = listaDetalleReservaSSRs.Where(x => x.Orden == ordenAjuNeg && x.ClasFact != 4).Sum(x => x.ChargeAmount);
                        montoIVAFee = listaDetalleReservaSSRs.Where(x => x.Orden == ordenAjuNeg && x.ClasFact == 4).Sum(x => x.ChargeAmount);

                        if (componenteIVA != null)
                        {
                            porIVAPorDesglose = (Convert.ToDecimal(componenteIVA.PorcIva) / 100);

                        }

                        //Se calculan los montos que se van a mover a la tarifa para equilibrar los servicios adicionales
                        montoSinIvaPorAplicar = Math.Round((diferenciaSSR / (1 + porIVAPorDesglose)), 2);
                        montoIVAPorAplicar = Math.Round((montoSinIvaPorAplicar * porIVAPorDesglose), 2);

                        //En caso de que se genere una diferencia por un centavo entre lo que esta pendiente por ajustar se cuadra el IVA
                        if (Math.Abs(diferenciaSSR - (montoSinIvaPorAplicar + montoIVAPorAplicar)) == 0.01m)
                        {
                            montoIVAPorAplicar = diferenciaSSR - montoSinIvaPorAplicar;
                        }

                        //Calcula los montos que quedan pendientes para el siguiente pago
                        montoSinIvaRestante = montoFeeSinIVA - montoSinIvaPorAplicar;
                        montoIvaRestante = montoIVAFee - montoIVAPorAplicar;


                        int ordenConsec = 0;
                        ordenConsec = listaDetalleReservaTarifas.Max(x => x.Orden) + 1;


                        //Clonando los componentes para asignar el remanente
                        foreach (ENTReservaDet entReservaDetP in listaDetalleReservaSSRs.Where(x => x.Orden == ordenAjuNeg && x.ClasFact != 4))
                        {
                            if (entReservaDetP.ChargeAmount != 0)
                            {
                                if (Math.Abs(diferenciaSSR) > Math.Abs(entReservaDetP.ChargeAmount))
                                {
                                    ENTReservaDet compTar = new ENTReservaDet();
                                    ClonarReservaDet(ref compTar, entReservaDetP);


                                    entReservaDetP.IdReservaCab = 0;

                                    compTar.ClasFact = 3;
                                    compTar.TipoAcumulado = "TAR";
                                    compTar.TipoCargo = "TRAVELFEE";
                                    compTar.IdConcepto = 1;
                                    compTar.Orden = ordenConsec;
                                    compTar.FeeNumber = ordenConsec;
                                    diferenciaSSR -= compTar.ChargeAmount;
                                    montoSinIvaPorAplicar -= compTar.ChargeAmount;
                                    listaDetalleReservaTarifas.Add(compTar);
                                }
                                else
                                {
                                    ENTReservaDet componentePrincipalRemanente = new ENTReservaDet();
                                    ClonarReservaDet(ref componentePrincipalRemanente, entReservaDetP);
                                    diferenciaSSR -= montoSinIvaPorAplicar;
                                    entReservaDetP.ChargeAmount = montoSinIvaRestante;

                                    //Asignando importes a los componentes vinculados
                                    componentePrincipalRemanente.ChargeAmount = montoSinIvaPorAplicar;
                                    componentePrincipalRemanente.ClasFact = 3;
                                    componentePrincipalRemanente.TipoAcumulado = "TAR";
                                    componentePrincipalRemanente.TipoCargo = "TRAVELFEE";
                                    componentePrincipalRemanente.IdConcepto = 1;
                                    componentePrincipalRemanente.Orden = ordenConsec;
                                    componentePrincipalRemanente.FeeNumber = ordenConsec;
                                    listaDetalleReservaTarifas.Add(componentePrincipalRemanente);

                                }
                            }
                        }


                        listaDetalleReservaSSRs.RemoveAll(x => x.IdReservaCab == 0);


                        if (componenteIVA != null)
                        {
                            //Se clona la fila del IVA para el remanente
                            ClonarReservaDet(ref componenteIVARemanente, componenteIVA);
                            diferenciaSSR -= montoIVAPorAplicar;

                            //Actualiza la informacion del monto del IVA
                            componenteIVARemanente.ChargeAmount = montoIvaRestante;

                            //Agrega la nueva fila a las tarifas
                            componenteIVARemanente.ChargeAmount = montoIVAPorAplicar;
                            componenteIVARemanente.ClasFact = 3;
                            componenteIVARemanente.TipoAcumulado = "TAR";
                            componenteIVARemanente.TipoCargo = "TRAVELFEE";
                            componenteIVARemanente.IdConcepto = 1;
                            componenteIVARemanente.Orden = ordenConsec;
                            listaDetalleReservaTarifas.Add(componenteIVARemanente);
                        }
                    }

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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "DistribucionPagos", "ReasignarAjustesNegativos", EnviarCorreoErrores);
                throw new ExceptionViva(mensajeUsuario);

            }

        }
    }
}

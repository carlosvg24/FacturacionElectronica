using Facturacion.DAL.ProcesoFacturacion;
using Facturacion.ENT;
using Facturacion.ENT.ProcesoFacturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Facturacion.BLL.Pegaso;
using System.Net.Mail;
using System.Net.Mime;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Comun.Utils;
using static System.Net.Mime.MediaTypeNames;
using Facturacion.ENT.Portal.Facturacion;
using Comun.Email;
using System.Data.SqlClient;
using System.Net;
using System.Web.Script.Serialization;
using VBFactPaquetes.Model;

namespace Facturacion.BLL.ProcesoFacturacion
{
    public class BLLFacturacion : DALFacturacion
    {
        public BLLFacturacion()
        : base(BLLConfiguracion.ConexionNavitaireWB)
        {
            BllLogErrores = new BLLBitacoraErrores();
            InicializaVariablesGlobales();
        }

        #region Propiedades privadas ListasCatalogos
        public List<string> ListaCodigosTUA = new List<string>();
        public List<ENTFeeCat> ListaCatalogoFees = new List<ENTFeeCat>();
        public List<ENTFormapagoCat> ListaCatalogoFormasPago = new List<ENTFormapagoCat>();
        public List<ENTConceptosCat> ListaCatalogoConceptos = new List<ENTConceptosCat>();
        public List<ENTHotel> ListaCatalogoConceptosHotel = new List<ENTHotel>();
        public List<ENTAgentesCat> ListaCatalogoAgentes = new List<ENTAgentesCat>();
        public List<ENTEmpresaCat> ListaCatalogoEmpresas = new List<ENTEmpresaCat>();
        public List<ENTGencatalogosCat> ListaGenCatalogos = new List<ENTGencatalogosCat>();
        public List<ENT.ENTGendescripcionesCat> ListaGenDescripciones = new List<ENT.ENTGendescripcionesCat>();
        public List<ENTParametrosCnf> ListaParametros = new List<ENTParametrosCnf>();
        public List<string> ListaErrores = new List<string>();
        public ENTEmpresaCat EmpresaEmisora { get; set; }
        private BLLBitacoraErrores BllLogErrores { get; set; }
        public string PNR { get; set; }
        public string MensajeErrorUsuario { get; set; }


        public List<string> ListaCatalogoMonedas = new List<string>();

        #endregion ListasCatalogos
        #region propiedades Privadas
        private string cnfEmailUser;
        private string cnfEmailSourceName;
        private string cnfSendGridHost;
        private string cnfSendGridPort;
        private string cnfSendGridUser;
        private string cnfSendGridPass;


        private string cnfCategoria;
        private string cnfEmailContactoTestFac;
        private string cnfEmailCopiaOcultaFac;
        private string cnfEmailErrorFacturacion;
        private string cnfRegistrarLog;
        private string cnfRutaLog;
        private decimal margenSupIVA;
        private decimal margenInfIVA;
        private int toleranciaDiasFacturacion;
        private string cnfPAC;

        //LCI. INI. 2018-02-12 BLOQUEO IVA FRONTERIZO
        public List<decimal> ListaPorcIVABloqueado = new List<decimal>();
        public string MensajeFacturaBloqueadaPorIVA { get; set; }
        public List<ENTIvaCat> ListaIVACat = new List<ENTIvaCat>();
        //LCI. FIN. 2018-02-12 BLOQUEO IVA FRONTERIZO


        #endregion

        DALFacturacion DF = new DALFacturacion(new SqlConnection(""));
        // DHV INI 09-07-2019 Correccion de forma de pago
        public bool ActualizarFormaPago(ENTPagosPorFacturar pago)
        {
            bool seCorrigio = false; ;

            try
            {
                BLLPagosCab bllPago = new BLLPagosCab();
                bllPago.IniciarPropiedades();
                bllPago.BinRange = pago.BinRange;
                bllPago.UpdFormaPagModificadoPor = pago.UpdFormaPagModificadoPor;
                bllPago.FechaUpdaFormaPag = pago.FechaUpdaFormaPag;
                bllPago.IdUpdFormaPago = pago.IdUpdFormaPago;
                bllPago.IdPagosCab = pago.IdPagosCab;

                bllPago.ActualizarFormaPago();

                BLLFormapagoCat bllFormasPago = new BLLFormapagoCat();
                var formaPago = bllFormasPago.RecuperarTodo()
                                             .Where(f => f.IdFormaPago == pago.IdUpdFormaPago)
                                             .FirstOrDefault();
                List<ENTPagosPorFacturar> listaPagoPorFac = new List<ENTPagosPorFacturar>();
                BLLPagosDet bllPagosDet = new BLLPagosDet();
                //var listaPagosDet = bllPagosDet.RecuperarTodo();
                //var pagoDet = listaPagosDet.Where(p => p.PaymentID == pago.PaymentID).FirstOrDefault();
                var pagoDet = bllPagosDet.RecuperarPagosDetIdpagoscab(pago.IdPagosCab).Where(p => p.PaymentID == pago.PaymentID).FirstOrDefault();

                bllPagosDet.BookingID = pagoDet.BookingID;
                bllPagosDet.CollectedAmount = pagoDet.CollectedAmount;
                bllPagosDet.CollectedCurrencyCode = pagoDet.CollectedCurrencyCode;
                bllPagosDet.CurrencyCode = pagoDet.CurrencyCode;
                bllPagosDet.FechaHoraLocal = DateTime.Now;
                bllPagosDet.FechaPago = pagoDet.FechaPago;
                bllPagosDet.IdAgente = pagoDet.IdAgente;
                bllPagosDet.IdPagosCab = pagoDet.IdPagosCab;
                bllPagosDet.PaymentAmount = pagoDet.PaymentAmount;
                bllPagosDet.PaymentMethodCode = formaPago.PaymentMethodCode;
                bllPagosDet.PaymentID = pagoDet.PaymentID;
                bllPagosDet.Actualizar();
                seCorrigio = true;
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                BllLogErrores.RegistrarError(PNR, MensajeErrorUsuario, ex, "Portal", "CorregirFormaPagoAutomatico");
                throw ex;
            }

            return seCorrigio;
        }

        public ResponseBINRest InvocarBINRest(string BIN)
        {
            string result = String.Empty;
            ResponseBINRest response = new ResponseBINRest();

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://lookup.binlist.net/" + BIN);
                httpWebRequest.Timeout = 10000;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Accept = "application/json";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var resultado = streamReader.ReadToEnd();
                    //result = resultado;
                    response = new JavaScriptSerializer().Deserialize<ResponseBINRest>(resultado);
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                BllLogErrores.RegistrarError(PNR, MensajeErrorUsuario, ex, "Portal", "InvocarBINRest");
                throw ex;
            }

            return response;
        }
        // DHV FIN 09-07-2019 Correccion de forma de pago

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

                //Iniciar Catalogo Conceptos de Facturacion
                if (ListaCatalogoConceptosHotel.Count() == 0)
                {
                    BLLHotel bllConceptosFacHotel = new BLLHotel();
                    ListaCatalogoConceptosHotel = bllConceptosFacHotel.RecuperarInfoProveedor();
                }

                if (ListaCatalogoAgentes.Count() == 0)
                {
                    BLLAgentesCat bllCatalogoAgentes = new BLLAgentesCat();
                    ListaCatalogoAgentes = bllCatalogoAgentes.RecuperarTodo();
                }

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
                    cnfEmailUser = ListaParametros.Where(x => x.Nombre == "emailUser").FirstOrDefault().Valor;
                    cnfEmailSourceName = ListaParametros.Where(x => x.Nombre == "emailSourceName").FirstOrDefault().Valor;
                    cnfSendGridHost = ListaParametros.Where(x => x.Nombre == "SendGridHost").FirstOrDefault().Valor;
                    cnfSendGridPort = ListaParametros.Where(x => x.Nombre == "SendGridPort").FirstOrDefault().Valor;
                    cnfSendGridUser = ListaParametros.Where(x => x.Nombre == "SendGridUser").FirstOrDefault().Valor;
                    cnfSendGridPass = ListaParametros.Where(x => x.Nombre == "SendGridPass").FirstOrDefault().Valor;
                    cnfCategoria = ListaParametros.Where(x => x.Nombre == "cnfCategoria").FirstOrDefault().Valor;
                    cnfEmailContactoTestFac = ListaParametros.Where(x => x.Nombre == "emailContactoTestFac").FirstOrDefault().Valor;
                    cnfEmailCopiaOcultaFac = ListaParametros.Where(x => x.Nombre == "emailCopiaOcultaFac").FirstOrDefault().Valor;
                    cnfEmailErrorFacturacion = ListaParametros.Where(x => x.Nombre == "emailErrorFacturacion").FirstOrDefault().Valor;
                    cnfRegistrarLog = ListaParametros.Where(x => x.Nombre == "registrarLog").FirstOrDefault().Valor;
                    cnfRutaLog = ListaParametros.Where(x => x.Nombre == "rutaLog").FirstOrDefault().Valor;

                    margenInfIVA = Convert.ToDecimal(ListaParametros.Where(x => x.Nombre == "margenInfIVA").FirstOrDefault().Valor);
                    margenSupIVA = Convert.ToDecimal(ListaParametros.Where(x => x.Nombre == "margenSupIVA").FirstOrDefault().Valor);
                    toleranciaDiasFacturacion = Convert.ToInt16(ListaParametros.Where(x => x.Nombre == "ToleranciaDiasFacturacion").FirstOrDefault().Valor);

                    ////LCI INI. 2018-04-11 Se implementa parametro para indicar el Proveedor de timbrado 
                    //if (ListaParametros.Where(x => x.Nombre == "PAC").Count() > 0)
                    //{
                    //    cnfPAC = ListaParametros.Where(x => x.Nombre == "PAC").FirstOrDefault().Valor;
                    //}
                    ////LCI FIN. 2018-04-11 Se implementa parametro para indicar el Proveedor de timbrado 

                    if (ListaParametros.Where(x => x.Nombre == "MsjErrorPortal").Count() > 0)
                    {
                        MensajeErrorUsuario = ListaParametros.Where(x => x.Nombre == "MsjErrorPortal").FirstOrDefault().Valor;
                    }
                    else
                    {
                        MensajeErrorUsuario = "Por el momento no es posible procesar su solicitud por favor intente más tarde...";

                    }

                    //LCI INI. 2019-01-10 FACTURA GLOBAL
                    //Recupera el catalogo de Monedas del catalogo de descripciones
                    ListaCatalogoMonedas = new List<string>();
                    ListaCatalogoMonedas = ListaGenDescripciones.Where(x => x.CveTabla == "MONEDA" && x.Activo == true).Select(x => x.CveValor).ToList();

                    //LCI FIN. 2019-01-10 FACTURA GLOBAL

                    //LCI. INI. 2018-02-12 BLOQUEO IVA FRONTERIZO

                    ListaPorcIVABloqueado = new List<decimal>();


                    if (ListaIVACat.Count() == 0)
                    {
                        BLLIvaCat bllIVACat = new BLLIvaCat();
                        ListaIVACat = bllIVACat.RecuperarTodo();

                        if (ListaIVACat.Count() > 0)
                        {
                            foreach (ENTIvaCat ivaItem in ListaIVACat)
                            {
                                if (ivaItem.Activo == true && ivaItem.BloquearFacturacion == true)
                                {
                                    ListaPorcIVABloqueado.Add(ivaItem.PorcIVA);
                                }

                            }
                        }

                    }
                    else
                    {
                        //Genera una lista de los porcentajes de IVA validos
                        ENTParametrosCnf paramIVABloq = new ENTParametrosCnf();
                        paramIVABloq = ListaParametros.Where(x => x.Nombre == "PorcIVABloqueado").FirstOrDefault();
                        ListaPorcIVABloqueado = new List<decimal>();

                        if (paramIVABloq != null)
                        {
                            if (paramIVABloq.Valor.Length > 0)
                            {
                                string paramPorcentajeIVABloq = paramIVABloq.Valor;
                                string[] listPorcIVABloq = paramPorcentajeIVABloq.Split(',');
                                foreach (string porcIVA in listPorcIVABloq)
                                {
                                    decimal porcIVAVal = 0;
                                    if (decimal.TryParse(porcIVA, out porcIVAVal))
                                    {
                                        if (!ListaPorcIVABloqueado.Contains(porcIVAVal))
                                        {
                                            ListaPorcIVABloqueado.Add(porcIVAVal);
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            //En caso de que no exista el parametro del porcentaje de IVAS creara por default los actuales al 2017
                            ListaPorcIVABloqueado.Add(2);
                            ListaPorcIVABloqueado.Add(8);
                        }
                    }





                    if (ListaParametros.Where(x => x.Nombre == "MensajeBloqueFactIVA").Count() > 0)
                    {
                        MensajeFacturaBloqueadaPorIVA = ListaParametros.Where(x => x.Nombre == "MensajeBloqueFactIVA").FirstOrDefault().Valor;
                    }
                    else
                    {
                        MensajeFacturaBloqueadaPorIVA = "Estimado Usuario, te informamos que los CFDI's correspondientes a esta reservación no se encuentran disponibles por el momento, esto debido a la implementación de las nuevas disposiciones Fiscales en la Franja Fronteriza, se estan realizando los ajustes necesarios para que puedas generarlos a la brevedad, te agradeceremos intentes nuevamente a partir del dia 1 de Mayo del 2019.";

                    }

                    //LCI. FIN. 2018-02-12 BLOQUEO IVA FRONTERIZO

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

                byte idEmpresa = Convert.ToByte(ListaParametros.Where(x => x.Nombre == "IdEmpresa").FirstOrDefault().Valor); // IdEmpresa
                EmpresaEmisora = ListaCatalogoEmpresas.Where(x => x.IdEmpresa == idEmpresa).FirstOrDefault();


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



        public List<ENTPagosFacturados> RecuperarPagosPNR(string pnr)
        {
            PNR = pnr;
            List<ENTPagosFacturados> result = new List<ENTPagosFacturados>();
            try
            {
                BLLDistribucionPagos bllDistPag = new ProcesoFacturacion.BLLDistribucionPagos();

                result = bllDistPag.DistribuirPagosEnReservacion(pnr, string.Empty);
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "RecuperarPagosPNR");
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;

        }

        public bool ValidarNombrePasajero(ref ENTDatosFacturacion datosFacturacion)
        {
            bool result = false;

            DataTable dtPasajeros = new DataTable();
            try
            {
                PNR = datosFacturacion.ClaveReservacion;

                dtPasajeros = RecuperarPasajerosPorPNR(datosFacturacion.ClaveReservacion);
                //LCI. INI. 2019-05-21. Mejora en Busqueda de Pasajero por Nombre

                /*
                DataRow drPasajero = null;
                foreach (DataRow DR in dtPasajeros.Select("FirstName = '" + datosFacturacion.NombrePasajero.ToUpper() + "' AND LastName = '" + datosFacturacion.ApellidosPasajero.ToUpper() + "'"))
                {
                    drPasajero = DR;
                    break;
                }
                 result = (drPasajero != null);
                 
                 */

                List<string> paramNombrePasajero = ConvertirNombre(datosFacturacion.NombrePasajero).Split(' ').ToList();
                List<string> paramApePasajero = ConvertirNombre(datosFacturacion.ApellidosPasajero).Split(' ').ToList();

                foreach (DataRow drPasajero in dtPasajeros.Rows)
                {
                    string nombrePassBD = "";
                    string apellidoPassBD = "";

                    nombrePassBD = drPasajero["FirstName"].ToString();
                    apellidoPassBD = drPasajero["LastName"].ToString();

                    List<string> firstName = ConvertirNombre(drPasajero["FirstName"].ToString()).Split(' ').ToList();
                    List<string> lastName = ConvertirNombre(drPasajero["LastName"].ToString()).Split(' ').ToList();

                    bool nombreOK = false;
                    bool apellidoOK = false;

                    foreach (string nomPass in paramNombrePasajero)
                    {
                        if (nomPass.Length > 0)
                        {
                            nombreOK = firstName.Contains(nomPass);
                            if (nombreOK == false)
                            {
                                break;
                            }
                        }
                    }

                    foreach (string apePass in paramApePasajero)
                    {
                        if (apePass.Length > 0)
                        {
                            apellidoOK = lastName.Contains(apePass);
                            if (apellidoOK == false)
                            {
                                break;
                            }
                        }
                    }

                    if (nombreOK && apellidoOK)
                    {
                        result = true;
                        datosFacturacion.NombrePasajero = nombrePassBD;
                        datosFacturacion.ApellidosPasajero = apellidoPassBD;
                        break;
                    }
                }


                //LCI. FIN. 2019-05-21. Mejora en Busqueda de Pasajero por Nombre


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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "ValidarNombrePasajero");
                throw new ExceptionViva(mensajeUsuario);

            }
            return result;
        }

        private string ConvertirNombre(string valor)
        {
            string result = "";

            result = valor.ToUpper();
            result = result.Replace("Á", "A");
            result = result.Replace("É", "E");
            result = result.Replace("Í", "I");
            result = result.Replace("Ó", "O");
            result = result.Replace("Ú", "U");

            result = Regex.Replace(result, "[^a-zA-Z0-9 ]+", "", RegexOptions.Compiled);

            return result;

        }


        //LCI. INI. 31-01-2018 VALIDACION CAPTCHA PARA SOCIOS COMERCIALES
        public bool OcultarCaptchaPorPNR(string pnr, ref string passwordOTA)
        {
            bool result = false;

            DataTable dtOta = new DataTable();

            try
            {
                PNR = pnr;

                //Se busca la informacion del PNR para recuperar el organization code al que pertenece
                dtOta = RecuperarReservaPorPNR(pnr);

                //Se verifica si se recupero informacion de la reservacion
                if (dtOta != null && dtOta.Rows.Count > 0)
                {
                    string organizationCode = "";
                    DataRow drOta = dtOta.Rows[0];
                    organizationCode = drOta["CreatedOrganizationCode"].ToString();

                    //Se recupera la informacion de las OTAS con convenio para Omision de Captcha
                    BLLOtasCat bllOtas = new BLLOtasCat();
                    List<ENTOtasCat> listaOtas = new List<ENTOtasCat>();
                    listaOtas = bllOtas.RecuperarOtasCatOrganizationcode(organizationCode);

                    if (listaOtas != null && listaOtas.Count > 0)
                    {
                        ENTOtasCat entOta = listaOtas.First();
                        if (entOta.Activo == true)
                        {
                            result = true;
                            passwordOTA = entOta.Password;
                        }
                    }
                }
                else
                {
                    throw new ExceptionViva("La clave de Reservación no existe o no es válida, favor de verificar...");
                }
            }
            catch (ExceptionViva ex)
            {
                //throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "BD");
                //throw new ExceptionViva(mensajeUsuario);
                //result = false;
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "OcultarCaptchaPorPNR");
                //throw new ExceptionViva(mensajeUsuario);

            }
            return result;
        }
        //LCI. FIN. 31-01-2018


        /// <summary>
        /// Metodo que genera la(s) factura(s) en base a la lista de Pagos que pasa por parametro
        /// </summary>
        /// <param name="PNR">Codigo de la reservacion a facturar</param>
        /// <param name="listaPagosPorFacturar">Lista de los PaymentId que se van a facturar</param>
        /// <returns>Regresa la lista actulizada con los datos de las facturas generadas</returns>
        public List<ENTPagosPorFacturar> GenerarFacturaCliente(ENTDatosFacturacion datosCliente, List<ENTPagosPorFacturar> listaPagosPorFacturar, Paquete paquete)
        {
            List<ENTPagosPorFacturar> result = new List<ENTPagosPorFacturar>();

            try
            {
                PNR = datosCliente.ClaveReservacion;

                var listaPagosAFacturar = listaPagosPorFacturar.Where(x => x.EstaMarcadoParaFacturacion == true);

                List<ENTFacturaCliente> listaFacturasCliente = new List<ENTFacturaCliente>();

                long bookingId = 0;

                bool procesoFactura = false;

                if (listaPagosAFacturar.Count() > 0)
                {
                    bookingId = listaPagosPorFacturar[0].BookingID;

                    //Se recupera la informacion de todos los pagos vinculados a la reservacion
                    List<ENTPagosCab> listaPagosCab = new List<ENTPagosCab>();
                    BLLPagosCab bllPagosCab = new BLLPagosCab();
                    listaPagosCab = bllPagosCab.RecuperarPagosCabBookingid(bookingId);

                    foreach (ENTPagosPorFacturar pagoPorFacturar in listaPagosPorFacturar)
                    {
                        bool flg_Terceros = false;
                        if (pagoPorFacturar.EstaMarcadoParaFacturacion == false || pagoPorFacturar.EsFacturado)
                        {
                            result.Add(pagoPorFacturar);
                        }
                        else
                        {

                            ENTPagosPorFacturar pago = pagoPorFacturar;
                            try
                            {
                                //if ((Convert.ToInt32(paquete.items[0].sellingPrice) == Convert.ToInt32(pagoPorFacturar.PaymentAmount)))
                                if (pagoPorFacturar.PaymentMethodCode == "X9")
                                {
                                    flg_Terceros = true;
                                }

                                ENTFacturaCliente facturaCliente = new ENTFacturaCliente();
                                facturaCliente = GenerarInformacionFactura(ref pago, datosCliente, listaPagosCab, flg_Terceros);

                                decimal montoPagoUnificado = 0;

                                BLLPagosCab bllPagos = new BLLPagosCab();
                                List<ENTPagosCab> listaPagosPorFolioPrefactura = new List<ENTPagosCab>();
                                listaPagosPorFolioPrefactura = bllPagos.RecuperarPagosCabFolioprefactura(pago.FolioPrefactura);

                                //CVG Timbrar reservas divididas
                                if (pago.CollectedAmount != pago.MontoTotal && pago.EsPagoDividido)
                                {
                                    montoPagoUnificado = listaPagosPorFolioPrefactura.Where(x => x.FolioPrefactura == pago.FolioPrefactura).Sum(x => x.MontoTotal);
                                }
                                //CVG Fin

                                else
                                {
                                    montoPagoUnificado = listaPagosPorFolioPrefactura.Where(x => x.FolioPrefactura == pago.FolioPrefactura && x.BookingID == pago.BookingID).Sum(x => x.MontoTotal);
                                }
                                //montoPagoUnificado = listaPagosCab.Where(x => x.FolioPrefactura == pago.FolioPrefactura).Sum(x => x.PaymentAmount);


                                if (montoPagoUnificado != (facturaCliente.Total + facturaCliente.Descuento))
                                {
                                    throw new Exception(string.Format("Existe una diferencia entre el Total del Pago {0} y el Total de la Factura {1}, Diferencia: {2}", montoPagoUnificado.ToString("$###,###,##0.00"), (facturaCliente.Total + facturaCliente.Descuento).ToString("$###,###,##0.00"), (montoPagoUnificado - facturaCliente.Total).ToString("$###,###,##0.00")));
                                }

                                string xmlRequest = "";
                                string xmlResponse = "";

                                //Se genera el Request para enviar a timbrar el comprobante
                                ENTXmlPegaso resultTimbrado = new ENTXmlPegaso();
                                xmlRequest = GenerarXMLRequestFactura(datosCliente, facturaCliente, flg_Terceros, paquete);


                                //LCI. INI. 2018-04-11 Se agrega configuracion para definir el PAC (Proveedor de Timbrado) que se invocara
                                //Se invoca el metodo principal de timbrado
                                BLLPegaso bllTim = new BLLPegaso();
                                resultTimbrado = bllTim.EnviaTimbrado(xmlRequest, facturaCliente.FolioFactura, facturaCliente.TipoComprobante, PNR, flg_Terceros);
                                xmlResponse = resultTimbrado.XMLResponse;
                                //LCI. FIN. 2018-04-11 Se agrega configuracion para definir el PAC (Proveedor de Timbrado) que se invocara


                                ENTPagosPorFacturar pagoActualizado = new ENTPagosPorFacturar();
                                if (resultTimbrado.EsCorrecto)
                                {

                                    //Se genera la informacion del CFDI
                                    ENTFacturascfdiDet cfdiDet = new ENTFacturascfdiDet();
                                    cfdiDet.IdFacturaCab = facturaCliente.IdFacturaCab;
                                    cfdiDet.TransaccionID = resultTimbrado.Transaccion_Id;
                                    cfdiDet.CFDI = resultTimbrado.CFD_ComprobanteStr;
                                    cfdiDet.CadenaOriginal = resultTimbrado.CFD_CadenaOriginal;
                                    cfdiDet.FechaTimbrado = resultTimbrado.TFD_FechaTimbrado;
                                    cfdiDet.FechaHoraLocal = DateTime.Now;
                                    facturaCliente.FacturasCFDIDet = cfdiDet;

                                    facturaCliente.EsFacturado = true;
                                    facturaCliente.IdPeticionPAC = resultTimbrado.IdPeticionPAC;
                                    facturaCliente.FechaHoraExpedicion = resultTimbrado.FechaTimbrado;
                                    facturaCliente.UUID = resultTimbrado.TFD_UUID;
                                    facturaCliente.Estatus = "FA";
                                    //Se guarda la informacion de la factura
                                    GuardarFactura(ref facturaCliente);

                                    //Se actualiza la informacion del Pago
                                    pagoActualizado = ActualizarPagoFacturado(ref listaPagosCab, pagoPorFacturar, facturaCliente);

                                    //Se actualiza la informacion de la reservacion
                                    ActualizarReservacion(bookingId, pagoActualizado);

                                    // DHV
                                    List<String> listPNR = new List<String>();
                                    int contPNR = 0;
                                    foreach (ENTPagosPorFacturar pagoPorFac in listaPagosPorFacturar)
                                    {
                                        if (!String.IsNullOrEmpty(pagoPorFac.PNR) && !listPNR.Contains(pagoPorFac.PNR.Trim().ToUpper()))
                                        {
                                            contPNR++;
                                            listPNR.Add(pagoPorFac.PNR.Trim().ToUpper());
                                        }
                                    }

                                    if (contPNR > 1)
                                        datosCliente.ClaveReservacion = datosCliente.RFCReceptor + "|" + String.Join("_", listPNR.ToArray());
                                    // DHV

                                    //Se genera el archivo CFDI
                                    BLLPDFCFDI bllPdf = new ProcesoFacturacion.BLLPDFCFDI();

                                    string rutaArchivoCFDI = bllPdf.GeneraArchivoCFDI(resultTimbrado.CFD_ComprobanteStr, facturaCliente.FolioFactura, datosCliente.ClaveReservacion);

                                    facturaCliente.FacturasCFDIDet = cfdiDet;

                                    string rutaArchivoPDF = "";
                                    rutaArchivoPDF = bllPdf.GeneraPDFFactura33(resultTimbrado.CFD_ComprobanteStr, resultTimbrado.CFD_CadenaOriginal, datosCliente.ClaveReservacion, flg_Terceros);

                                    pagoActualizado.EsFacturado = true;
                                    pagoActualizado.FechaFactura = facturaCliente.FechaHoraExpedicion;
                                    pagoActualizado.RutaCFDI = rutaArchivoCFDI;
                                    pagoActualizado.RutaPDF = rutaArchivoPDF;
                                    pagoActualizado.Mensaje = "OK";
                                    procesoFactura = true;
                                }
                                else
                                {
                                    pagoActualizado = pagoPorFacturar;
                                    pagoActualizado.Mensaje = "Por el momento no fue posible generar la factura, por favor intente más tarde...";
                                }
                                result.Add(pagoActualizado);


                            }
                            catch (ExceptionViva ex)
                            {
                                result.Add(pagoPorFacturar);
                                throw ex;
                            }
                            catch (SqlException ex)
                            {
                                result.Add(pagoPorFacturar);
                                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                                string mensajeUsuario = MensajeErrorUsuario;
                                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "BD");
                                throw new ExceptionViva(mensajeUsuario);
                            }
                            catch (Exception ex)
                            {
                                result.Add(pagoPorFacturar);
                                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                                string mensajeUsuario = MensajeErrorUsuario;
                                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "GenerarFacturaCliente");

                                // DHV  05-02-2020 < ------------------>
                                throw new Exception(MensajeErrorUsuario);
                            }

                        }
                    }

                    //Se envian los archivos por correo
                    if (result.Where(x => x.Mensaje == "OK").Count() > 0)
                    {

                        EnviarEmail email = new EnviarEmail(ListaParametros, EmpresaEmisora);
                        string envioEmail = EnviarCorreoArchivos(result, datosCliente);

                        if (envioEmail == "FALLA")
                        {
                            foreach (ENTPagosPorFacturar pagoSinEnvio in result)
                            {
                                pagoSinEnvio.Mensaje = "SinEnvio";
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
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "GenerarFacturaCliente");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }

        private ENTFacturaCliente GenerarInformacionFactura(ref ENTPagosPorFacturar pago, ENTDatosFacturacion datosCliente, List<ENTPagosCab> listaPagos, bool flg_Terceros)
        {

            //Inicia la generacion de los datos de la factura
            ENTFacturaCliente facturaCliente = new ENTFacturaCliente();

            try
            {
                PNR = datosCliente.ClaveReservacion;
                DateTime fechaHoraLocal = DateTime.Now;
                //Recuperar Parametros Facturacion
                string versionFac = ListaParametros.Where(x => x.Nombre == "VersionFactura").FirstOrDefault().Valor; // "3.3";
                string serieFac = ListaParametros.Where(x => x.Nombre == "SerieFactura").FirstOrDefault().Valor; //"B";
                string tipoComprobante = ListaParametros.Where(x => x.Nombre == "CveTipoComprobanteFac").FirstOrDefault().Valor; // "I";
                string condicionesPago = ListaParametros.Where(x => x.Nombre == "CondicionesPago").FirstOrDefault().Valor; // "I";
                string permitirConfirmacion = ListaParametros.Where(x => x.Nombre == "PermitirConfirmacion").FirstOrDefault().Valor; //"false";
                byte idEmpresa = Convert.ToByte(ListaParametros.Where(x => x.Nombre == "IdEmpresa").FirstOrDefault().Valor); // IdEmpresa
                ENTEmpresaCat empresa = new ENTEmpresaCat();
                decimal limiteFacturacion = Convert.ToDecimal(ListaParametros.Where(x => x.Nombre == "LimiteImporteFactura").FirstOrDefault().Valor);
                double porcMargenTC = ((Convert.ToDouble(ListaParametros.Where(x => x.Nombre == "LimiteVariacionUSD").FirstOrDefault().Valor)) / 100);

                byte porcIVAParam = Convert.ToByte(ListaParametros.Where(x => x.Nombre == "PorcentajeIVAGeneral").FirstOrDefault().Valor); // IdEmpresa

                ENTFormapagoCat entFormaPago = new ENTFormapagoCat();
                //DHV INI 09-07-2019  Corregir forma de pago
                int idFormaPago = pago.IdUpdFormaPago > 0 ? pago.IdUpdFormaPago : pago.IdFormaPago;
                //DHV FIN 09-07-2019  Corregir forma de pago
                entFormaPago = ListaCatalogoFormasPago.Where(x => x.IdFormaPago == idFormaPago).FirstOrDefault();

                string metodoPago = (entFormaPago.CveFormaPagoSAT == "99" ? "PPD" : "PUE");
                empresa = ListaCatalogoEmpresas.Where(x => x.IdEmpresa == idEmpresa).FirstOrDefault(); //"601";

                BLLDistribucionPagos bllDistribucion = new BLLDistribucionPagos();
                decimal tipoCambioActual = bllDistribucion.RecuperarTipoCambioPorFecha(pago.FechaPago); //Recupera el tipo de cambio del dia
                decimal limiteSupTC = 0;
                decimal limiteInfTC = 0;

                limiteSupTC = tipoCambioActual * Convert.ToDecimal(1 + porcMargenTC);
                limiteInfTC = tipoCambioActual * Convert.ToDecimal(1 - porcMargenTC);




                long folioPrefactura = pago.FolioPrefactura;
                //Monto Total Factura
                decimal montoTotalFactura = 0;
                montoTotalFactura = listaPagos.Where(x => x.FolioPrefactura == folioPrefactura).Sum(x => x.MontoPorAplicar);
                decimal montoDescuento = 0;
                montoDescuento = listaPagos.Where(d => d.PaymentMethodCode == "W4" || d.PaymentMethodCode == "W5").Sum(d => d.MontoPorAplicar);

                bool solicitarConfirmacion = false;
                solicitarConfirmacion = (pago.TipoCambio < limiteInfTC
                                        || pago.TipoCambio > limiteSupTC
                                        || limiteFacturacion < montoTotalFactura
                                        );


                facturaCliente.IdFacturaCab = 1;
                facturaCliente.IdEmpresa = idEmpresa;
                facturaCliente.BookingID = pago.BookingID;
                facturaCliente.FechaHoraExpedicion = fechaHoraLocal;
                facturaCliente.TipoFacturacion = "FA";
                facturaCliente.Version = versionFac;
                facturaCliente.Serie = serieFac;// "B";
                if (flg_Terceros)
                {
                    long folioPaquete = DF.GetFolio(PNR);
                    facturaCliente.FolioFactura = folioPaquete;
                }
                else
                {
                    facturaCliente.FolioFactura = pago.FolioPrefactura;//  12860589;
                }
                facturaCliente.UUID = "";
                facturaCliente.TransactionID = GeneratransactionID(empresa.Rfc, pago.PaymentID, DateTime.Now, datosCliente.ClaveReservacion, pago.IdAgente);
                facturaCliente.IdPeticionPAC = 0;
                facturaCliente.Estatus = "";
                facturaCliente.RfcEmisor = empresa.Rfc;// "ANA050518RL1";
                facturaCliente.RazonSocialEmisor = empresa.RazonSocial;
                facturaCliente.NoCertificado = empresa.NoCertificado;
                facturaCliente.IdRegimenFiscal = empresa.IdRegimenFiscal;//  "601";
                facturaCliente.RfcReceptor = datosCliente.RFCReceptor;//  "CAIL770623L23";
                facturaCliente.RazonSocialReceptor = "";
                facturaCliente.EmailReceptor = datosCliente.EmailReceptor;//  "luis.carrasco@vivaaerobus.com";
                facturaCliente.EsExtranjero = datosCliente.EsExtranjero;//  false;
                facturaCliente.IdPaisResidenciaFiscal = datosCliente.PaisResidenciaFiscal;//  "MEX";
                facturaCliente.NumRegIdTrib = datosCliente.TAXID;//  "";
                facturaCliente.UsoCFDI = datosCliente.UsoCFDI;//  "P01";


                facturaCliente.FormaPago = entFormaPago.CveFormaPagoSAT;//  "01";
                facturaCliente.MetodoPago = metodoPago;// "PUE";
                facturaCliente.TipoComprobante = tipoComprobante;// "I";
                                                                 //LCI. INI. 20181121 LugarExpedicion
                                                                 //facturaCliente.LugarExpedicion = empresa.LugarExpedicion;//  "66600";
                if (pago.LugarExpedicion == null || pago.LugarExpedicion.Length == 0)
                {
                    facturaCliente.LugarExpedicion = empresa.LugarExpedicion;//  "66600";
                }
                else
                {
                    // validar que sí el CP no es mexicano facturen en el CP 66600

                    // Si no se mantiene el CP indicado en LugarExpedicion
                    facturaCliente.LugarExpedicion = pago.LugarExpedicion;
                }
                //LCI. INI. 20181121 LugarExpedicion

                facturaCliente.CondicionesPago = condicionesPago;// "";
                facturaCliente.Moneda = pago.CurrencyCode;// "MXN";
                facturaCliente.TipoCambio = pago.TipoCambio;// 1M;

                facturaCliente.IdAgente = Convert.ToInt32(pago.IdAgente);
                facturaCliente.IdUsuario = 0;
                facturaCliente.FechaHoraLocal = fechaHoraLocal;

                facturaCliente.SolicitaConfirmacion = solicitarConfirmacion;


                List<ENTFacturasDet> listaFacDet = new List<ENTFacturasDet>();
                List<ENTFacturasDet> listaServAdic = new List<ENTFacturasDet>();
                List<ENTFacturasivaDet> listaIVADet = new List<ENTFacturasivaDet>();
                List<ENTFacturascargosDet> listaCargosDet = new List<ENTFacturascargosDet>();


                ENTReservacion reservacion = new ENTReservacion();

                reservacion = RecuperarDatosReservacion(facturaCliente.BookingID, pago);

                decimal montoTarifa = 0;
                decimal montoServAdic = 0;
                decimal montoTua = 0;
                decimal montoOtrosCar = 0;
                decimal montoIva = 0;
                decimal montoSubTotal = 0;
                decimal montoTotal = 0;
                decimal remanteMilesimasSubtotal = 0;
                decimal remanteMilesimasIVA = 0;



                ENTReservaDet entDetalleIVA = new ENTReservaDet();

                entDetalleIVA = reservacion.ListaReservaDet.Where(x => x.FolioPreFactura == folioPrefactura && x.TipoAcumulado == "IVA").OrderBy(x => x.PorcIva).FirstOrDefault();
                byte porcIVAGeneral = entDetalleIVA != null ? entDetalleIVA.PorcIva : porcIVAParam;

                if (porcIVAGeneral < porcIVAParam)
                {
                    porcIVAGeneral = porcIVAParam;
                }

                //Agrupar por conceptos
                List<int> listaConceptos = new List<int>();
                listaConceptos = (from x in reservacion.ListaReservaDet
                                  where x.FolioPreFactura == folioPrefactura
                                  orderby x.IdConcepto descending
                                  select x.IdConcepto).Distinct().ToList();

                int idFacturaDet = 0;
                int totConceptos = listaConceptos.Count();
                int numConceptoAct = 0;


                foreach (int idConc in listaConceptos)
                {
                    numConceptoAct++;

                    ENTConceptosCat entConcepto = new ENTConceptosCat();
                    entConcepto = ListaCatalogoConceptos.Where(x => x.IdConcepto == idConc && x.TipoComprobante == "F").FirstOrDefault();

                    decimal valorUnitarioReal = 0;
                    decimal valorUnitarioRound = 0;
                    decimal importe = 0;

                    //Se obtiene la sumatoria del concepto
                    valorUnitarioReal = reservacion.ListaReservaDet.Where(x => x.FolioPreFactura == folioPrefactura && x.IdConcepto == idConc && x.TipoAcumulado != "IVA").Sum(x => x.ChargeAmount);
                    //Se acumulan los remanentes de milesimas del concepto anterior
                    valorUnitarioReal += remanteMilesimasSubtotal;
                    //Se obtiene el monto redondeado para el concepto
                    valorUnitarioRound = Math.Round(valorUnitarioReal, 2);

                    remanteMilesimasSubtotal = valorUnitarioReal - valorUnitarioRound;


                    importe = valorUnitarioRound;//Math.Round(reservacion.ListaReservaDet.Where(x => x.FolioPreFactura == folioPrefactura && x.IdConcepto == idConc && x.TipoAcumulado != "IVA").Sum(x => x.ChargeAmount), 2);

                    //Identifica el concepto
                    string tipoAcum = (from x in reservacion.ListaReservaDet
                                       where x.IdConcepto == idConc
                                       select x.TipoAcumulado).FirstOrDefault();

                    //Incrementa acumulado tarifa
                    switch (tipoAcum)
                    {
                        case "TAR":
                            montoTarifa += importe;
                            break;
                        case "SVA":
                            montoServAdic += importe;
                            break;
                    }



                    if (valorUnitarioRound > 0)
                    {
                        idFacturaDet++;
                        montoTotal += valorUnitarioRound;
                        montoSubTotal += valorUnitarioRound;

                        ENTFacturasDet factDet = new ENTFacturasDet();
                        factDet.IdFacturaCab = facturaCliente.IdFacturaCab;
                        factDet.IdFacturaDet = idFacturaDet;
                        factDet.ClaveProdServ = entConcepto.ClaveProdServ;
                        factDet.NoIdentificacion = entConcepto.NoIdentificacion;
                        factDet.Cantidad = 1;
                        factDet.ClaveUnidad = entConcepto.ClaveUnidad;
                        factDet.Unidad = entConcepto.Unidad;
                        factDet.Descripcion = entConcepto.Descripcion.Replace("*", reservacion.RecordLocator.ToUpper());//  "TARIFA AEREA PNR: VBJKJN";
                        factDet.ValorUnitario = valorUnitarioRound;
                        factDet.Importe = importe;
                        factDet.Descuento = 0M;
                        factDet.FechaHoraLocal = fechaHoraLocal;

                        listaFacDet.Add(factDet);

                        //Generando informacion del IVA

                        List<byte> listaIvas = new List<byte>();
                        listaIvas = (from x in reservacion.ListaReservaDet
                                     where x.FolioPreFactura == folioPrefactura
                                     && x.IdConcepto == idConc
                                     orderby x.PorcIva
                                     select x.PorcIva
                                     ).Distinct().ToList();


                        foreach (byte porcIVA in listaIvas)
                        {
                            decimal montoBase = 0;
                            decimal tasaIVA = 0;
                            decimal montoIVARound = 0;
                            decimal montoIVAReal = 0;

                            ENTIvaCat entIVAcnf = new ENTIvaCat();
                            entIVAcnf = ListaIVACat.Where(x => x.PorcIVA == porcIVA).FirstOrDefault();

                            if (entIVAcnf != null)
                            {
                                porcIVAGeneral = (byte)entIVAcnf.PorcTransformacion;
                            }
                            else
                            {
                                porcIVAGeneral = porcIVAParam;
                            }

                            montoBase = Math.Round(reservacion.ListaReservaDet
                                        .Where(x => x.FolioPreFactura == folioPrefactura && x.IdConcepto == idConc && x.TipoAcumulado != "IVA" && x.PorcIva == porcIVA).Sum(x => x.ChargeAmount), 2);

                            montoIVAReal = reservacion.ListaReservaDet
                                       .Where(x => x.FolioPreFactura == folioPrefactura && x.IdConcepto == idConc && x.TipoAcumulado == "IVA" && x.PorcIva == porcIVA).Sum(x => x.ChargeAmount);

                            montoIVAReal += remanteMilesimasIVA;



                            montoIVARound = Math.Round(montoIVAReal, 2);
                            remanteMilesimasIVA = montoIVAReal - montoIVARound;


                            if (totConceptos == numConceptoAct && Math.Abs((montoTotal + montoIVARound) - Math.Round(montoTotalFactura, 2)) <= .01m && montoIVARound > 0)
                            {
                                montoIVAReal += remanteMilesimasSubtotal;
                                montoIVARound = Math.Round(montoIVAReal, 2);
                            }

                            montoIva += montoIVARound;
                            montoTotal += montoIVARound;

                            if (montoBase > 0)
                            {
                                if (porcIVA == porcIVAGeneral)
                                {
                                    tasaIVA = Convert.ToDecimal(porcIVA) / 100;
                                    ENTFacturasivaDet entIVADet16 = new ENTFacturasivaDet();

                                    if (listaIVADet.Where(x => x.IdFacturaDet == idFacturaDet && x.TasaOCuota == tasaIVA).Count() == 0)
                                    {
                                        entIVADet16.IdFacturaCab = facturaCliente.IdFacturaCab;
                                        entIVADet16.IdFacturaDet = idFacturaDet;
                                        entIVADet16.TipoFactor = "Tasa";    //Fijo el tipo de factor para TASA
                                        entIVADet16.TasaOCuota = tasaIVA;
                                        entIVADet16.Base = CalculaMontoBaseIVA(montoBase, tasaIVA, montoIVARound);
                                        entIVADet16.Impuesto = "002";  //Fijo el valor del impuesto para el IVA
                                        entIVADet16.Importe = montoIVARound;//Math.Round((montoBase * tasaIVA),2); 
                                        entIVADet16.FechaHoraLocal = fechaHoraLocal;
                                        listaIVADet.Add(entIVADet16);
                                    }
                                    else
                                    {
                                        entIVADet16 = listaIVADet.Where(x => x.IdFacturaDet == idFacturaDet && x.TasaOCuota == tasaIVA).FirstOrDefault();
                                        entIVADet16.Importe += montoIVARound;
                                        entIVADet16.Base = CalculaMontoBaseIVA(entIVADet16.Base + montoBase, tasaIVA, entIVADet16.Importe);
                                    }


                                }
                                else if (porcIVA == 0)
                                {

                                    ENTFacturasivaDet entIVADet0 = new ENTFacturasivaDet();

                                    if (listaIVADet.Where(x => x.IdFacturaDet == idFacturaDet && x.TasaOCuota == 0.0M).Count() == 0)
                                    {
                                        entIVADet0.IdFacturaCab = facturaCliente.IdFacturaCab;
                                        entIVADet0.IdFacturaDet = idFacturaDet;
                                        entIVADet0.TipoFactor = "Tasa";    //Fijo el tipo de factor para TASA
                                        entIVADet0.TasaOCuota = 0.000000M;
                                        entIVADet0.Base = montoBase;
                                        entIVADet0.Impuesto = "002";  //Fijo el valor del impuesto para el IVA
                                        entIVADet0.Importe = 0;
                                        entIVADet0.FechaHoraLocal = fechaHoraLocal;
                                        listaIVADet.Add(entIVADet0);
                                    }
                                    else
                                    {
                                        entIVADet0 = listaIVADet.Where(x => x.IdFacturaDet == idFacturaDet && x.TasaOCuota == 0.0M).FirstOrDefault();
                                        entIVADet0.Base += montoBase;
                                    }

                                }
                                else
                                {
                                    tasaIVA = Convert.ToDecimal(porcIVAGeneral) / 100;
                                    decimal porcBase = 0;
                                    porcBase = (((Convert.ToDecimal(porcIVA) * 100) / porcIVAGeneral) / 100);
                                    decimal montoBaseIvaGen = Math.Round((montoBase * porcBase), 3);

                                    ENTFacturasivaDet entIVADetGral = new ENTFacturasivaDet();

                                    if (listaIVADet.Where(x => x.IdFacturaDet == idFacturaDet && x.TasaOCuota == tasaIVA).Count() == 0)
                                    {
                                        entIVADetGral.IdFacturaCab = facturaCliente.IdFacturaCab;
                                        entIVADetGral.IdFacturaDet = idFacturaDet;
                                        entIVADetGral.TipoFactor = "Tasa";    //Fijo el tipo de factor para TASA
                                        entIVADetGral.TasaOCuota = tasaIVA;
                                        entIVADetGral.Base = CalculaMontoBaseIVA(montoBaseIvaGen, tasaIVA, montoIVARound);
                                        entIVADetGral.Impuesto = "002";  //Fijo el valor del impuesto para el IVA
                                        entIVADetGral.Importe = montoIVARound;
                                        entIVADetGral.FechaHoraLocal = fechaHoraLocal;
                                        listaIVADet.Add(entIVADetGral);

                                    }
                                    else
                                    {
                                        entIVADetGral = listaIVADet.Where(x => x.IdFacturaDet == idFacturaDet && x.TasaOCuota == tasaIVA).FirstOrDefault();
                                        entIVADetGral.Importe += montoIVARound;
                                        entIVADetGral.Base = CalculaMontoBaseIVA(montoBaseIvaGen, tasaIVA, entIVADetGral.Importe);

                                    }



                                    ENTFacturasivaDet entIVADet0 = new ENTFacturasivaDet();

                                    if (listaIVADet.Where(x => x.IdFacturaDet == idFacturaDet && x.TasaOCuota == 0.0M).Count() == 0)
                                    {
                                        entIVADet0.IdFacturaCab = facturaCliente.IdFacturaCab;
                                        entIVADet0.IdFacturaDet = idFacturaDet;
                                        entIVADet0.TipoFactor = "Tasa";    //Fijo el tipo de factor para TASA
                                        entIVADet0.TasaOCuota = 0.000000M;
                                        entIVADet0.Base = Math.Round((montoBase * (1 - porcBase)), 2);
                                        entIVADet0.Impuesto = "002";  //Fijo el valor del impuesto para el IVA
                                        entIVADet0.Importe = 0;
                                        entIVADet0.FechaHoraLocal = fechaHoraLocal;
                                        listaIVADet.Add(entIVADet0);
                                    }
                                    else
                                    {
                                        entIVADet0 = listaIVADet.Where(x => x.IdFacturaDet == idFacturaDet && x.TasaOCuota == 0.0M).FirstOrDefault();
                                        entIVADet0.Base += Math.Round((montoBase * (1 - porcBase)), 2);
                                    }

                                }
                            }
                        }

                        //Generando la informacion de los cargos aeroportuarios
                        if (idConc == 3)
                        {
                            var listaCargosAero = (from x in reservacion.ListaReservaDet
                                                   where x.FolioPreFactura == folioPrefactura && x.IdConcepto == idConc
                                                   orderby x.TicketCode
                                                   select x.TicketCode).Distinct().ToList();

                            decimal remanenteMilesimasCargo = 0;

                            foreach (string codigoCargo in listaCargosAero)
                            {

                                ENTFacturascargosDet factCargos = new ENTFacturascargosDet();
                                decimal montoCargoReal = 0;
                                decimal montoCargoRound = 0;
                                //MASS & CVG
                                List<string> lstChargeCodes = new List<string>();

                                montoCargoReal = reservacion.ListaReservaDet
                                       .Where(x => x.FolioPreFactura == folioPrefactura && x.IdConcepto == idConc && x.TicketCode == codigoCargo).Sum(x => x.ChargeAmount);

                                lstChargeCodes = (from x in reservacion.ListaReservaDet
                                                  where x.FolioPreFactura == folioPrefactura && x.IdConcepto == idConc && x.TicketCode == codigoCargo
                                                  select x.ChargeCode).Distinct().ToList();

                                montoCargoReal += remanenteMilesimasCargo;

                                montoCargoRound = Math.Round(montoCargoReal, 2);
                                remanenteMilesimasCargo = montoCargoReal - montoCargoRound;

                                if (ListaCodigosTUA.Contains(codigoCargo))
                                {
                                    montoTua += montoCargoRound;
                                }
                                else
                                {
                                    montoOtrosCar += montoCargoRound;
                                }


                                factCargos.IdFacturaCab = facturaCliente.IdFacturaCab;
                                factCargos.IdFacturaDet = idFacturaDet;

                                if (lstChargeCodes.Contains("VBABFE") || lstChargeCodes.Contains("VBABFJ") || lstChargeCodes.Contains("VBABFN") || lstChargeCodes.Contains("VBABGW") || lstChargeCodes.Contains("VBABGX") || lstChargeCodes.Contains("VBABGY"))
                                {
                                    factCargos.CodigoCargo = "TAX CO";
                                }
                                else
                                {
                                    factCargos.CodigoCargo = codigoCargo;
                                }

                                factCargos.Importe = montoCargoRound;
                                factCargos.FechaHoraLocal = fechaHoraLocal;
                                listaCargosDet.Add(factCargos);
                            }

                        }
                    }

                    //CVG Descontar montos negativos de servicios adicionales al total
                    //else
                    //{
                    //    idFacturaDet++;
                    //    montoTotal += valorUnitarioRound;
                    //    montoSubTotal += valorUnitarioRound;

                    //    ENTFacturasDet factDet = new ENTFacturasDet();
                    //    factDet.IdFacturaCab = facturaCliente.IdFacturaCab;
                    //    factDet.IdFacturaDet = idFacturaDet;
                    //    factDet.ClaveProdServ = entConcepto.ClaveProdServ;
                    //    factDet.NoIdentificacion = entConcepto.NoIdentificacion;
                    //    factDet.Cantidad = 1;
                    //    factDet.ClaveUnidad = entConcepto.ClaveUnidad;
                    //    factDet.Unidad = entConcepto.Unidad;
                    //    factDet.Descripcion = entConcepto.Descripcion.Replace("*", reservacion.RecordLocator.ToUpper());//  "TARIFA AEREA PNR: VBJKJN";
                    //    factDet.ValorUnitario = valorUnitarioRound;
                    //    factDet.Importe = importe;
                    //    factDet.Descuento = 0M;
                    //    factDet.FechaHoraLocal = fechaHoraLocal;

                    //    listaServAdic.Add(factDet);
                    //}
                }

                //if (listaServAdic.Count > 0)
                //{
                //    for(int valorNegativo = 0; valorNegativo < listaServAdic.Count; valorNegativo++)
                //    {
                //        if (listaFacDet.Where(x => x.Descripcion.Contains("TARIFA AEREA")).FirstOrDefault() != null)
                //        {
                //            listaFacDet.Where(x => x.Descripcion.Contains("TARIFA AEREA")).First().Importe += listaServAdic[valorNegativo].Importe;
                //            listaFacDet.Where(x => x.Descripcion.Contains("TARIFA AEREA")).First().ValorUnitario += listaServAdic[valorNegativo].Importe;
                //        }

                //    }   
                //}

                if (flg_Terceros)
                {
                    ENTConceptosCat entConcepto = new ENTConceptosCat();
                    entConcepto = ListaCatalogoConceptos.Where(x => x.Descripcion == "TARIFA HOTELERA").FirstOrDefault();

                    ENTFacturasDet factDet = new ENTFacturasDet();
                    factDet.ClaveProdServ = entConcepto.ClaveProdServ;
                    factDet.NoIdentificacion = entConcepto.NoIdentificacion;
                    factDet.Cantidad = 1;
                    factDet.ClaveUnidad = entConcepto.ClaveUnidad;
                    factDet.Unidad = entConcepto.Unidad;
                    factDet.Descripcion = entConcepto.Descripcion.Replace("*", reservacion.RecordLocator.ToUpper());//  "TARIFA AEREA PNR: VBJKJN";

                    ENTHotel entConceptoHotel = new ENTHotel();
                    entConceptoHotel = ListaCatalogoConceptosHotel.FirstOrDefault();
                    factDet.RFC = entConceptoHotel.RFC;
                    factDet.Nombre = entConceptoHotel.Nombre;
                    factDet.Calle = entConceptoHotel.Calle;
                    factDet.NumExt = entConceptoHotel.NumExt;
                    factDet.Municipio = entConceptoHotel.Municipio;
                    factDet.Estado = entConceptoHotel.Estado;
                    factDet.Pais = entConceptoHotel.Pais;
                    factDet.CodigoPostal = entConceptoHotel.CodigoPostal;

                    listaFacDet.Add(factDet);
                }
                // Agregar descuento al concepto con mayor importe
                var IdFacturaDetConDescuento = (from x in listaFacDet
                                                orderby x.Importe descending
                                                select x.IdFacturaDet).FirstOrDefault();

                listaFacDet.Where(x => x.IdFacturaCab == facturaCliente.IdFacturaCab
                                    && x.IdFacturaDet == IdFacturaDetConDescuento)
                                    .FirstOrDefault()
                                    .Descuento = montoDescuento;

                facturaCliente.MontoTarifa = montoTarifa;
                facturaCliente.MontoServAdic = montoServAdic;
                facturaCliente.MontoTUA = montoTua;
                facturaCliente.MontoOtrosCargos = montoOtrosCar;
                facturaCliente.MontoIVA = montoIva;
                facturaCliente.SubTotal = montoSubTotal;
                facturaCliente.Descuento = montoDescuento;
                facturaCliente.Total = montoTotal - montoDescuento;


                facturaCliente.ListaFacturasDet = listaFacDet;
                facturaCliente.ListaIVAPorDetalle = listaIVADet;
                facturaCliente.ListaCargosAero = listaCargosDet;

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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "GenerarInformacionFactura");
                throw new ExceptionViva(mensajeUsuario);
            }

            return facturaCliente;

        }

        private string GenerarXMLRequestFactura(ENTDatosFacturacion datosCliente, ENTFacturaCliente factura, bool flg_Terceros, Paquete paquete)
        {
            string result = "";
            

            if (flg_Terceros)
            {
                double ivaHotel;
                if (paquete.items[1].location.countryCode == "MX")
                {
                    ivaHotel = Math.Round(paquete.items[1].sellingPrice * 0.16, 2);
                }
                else
                {
                    ivaHotel = Convert.ToDouble("0.00");
                }
            }

            int numSegDesfaseSAT = 0;
            PNR = datosCliente.ClaveReservacion;
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
                    factura.Serie = factura.Serie.Split(',')[1];
                    xmlFact.Append("Serie=\"" + factura.Serie + "\" ");
                }
                else
                {
                    factura.Serie = factura.Serie.Split(',')[0];
                    xmlFact.Append("Serie=\"" + factura.Serie + "\" ");
                }
                xmlFact.Append("Folio=\"" + factura.FolioFactura.ToString() + "\" ");

                // DHV INI 16-OCT-2019 Fecha de envío según huso horario del lugar exp
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
                // DHV FIN 16-OCT-2019 Fecha de envío según huso horario del lugar exp


                //Se debe crear el metodo que determine la forma de pago
                xmlFact.Append("FormaPago=\"" + factura.FormaPago + "\" ");
                xmlFact.Append("NoCertificado=\"" + factura.NoCertificado + "\" ");
                xmlFact.Append("CondicionesDePago=\"" + factura.CondicionesPago + "\" ");
                if (flg_Terceros)
                {
                    decimal subTotal = factura.SubTotal + Math.Round(Convert.ToDecimal(paquete.items[1].sellingPrice), 2);
                    xmlFact.Append("SubTotal=\"" + subTotal.ToString("0.00") + "\" ");
                }
                else
                {
                    xmlFact.Append("SubTotal=\"" + factura.SubTotal.ToString("0.00") + "\" ");
                }

                if (factura.Descuento > 0)
                {
                    xmlFact.Append("Descuento=\"" + factura.Descuento.ToString("0.00") + "\" ");
                }

                string moneda = factura.Moneda;
                xmlFact.Append("Moneda=\"" + moneda + "\" ");

                decimal tipoCambio = 0;

                if (moneda.ToUpper() != "MXN")
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
                    decimal Total;

                    if (paquete.items[1].location.countryCode == "MX")
                    {
                        double ivaHotel = ivaHotel = Math.Round(paquete.items[1].sellingPrice * 0.16, 2);
                        Total = factura.Total + Math.Round(Convert.ToDecimal(paquete.items[1].sellingPrice), 2) + Convert.ToDecimal(ivaHotel);
                        xmlFact.Append("Total=\"" + Total.ToString("0.00") + "\" ");
                    }

                    else
                    {
                        Total = factura.Total + Math.Round(Convert.ToDecimal(paquete.items[1].sellingPrice), 2);
                        xmlFact.Append("Total=\"" + Total.ToString("0.00") + "\" ");
                    }
                }
                else
                {
                    xmlFact.Append("Total=\"" + factura.Total.ToString("0.00") + "\" ");
                }
                xmlFact.Append("TipoDeComprobante=\"" + factura.TipoComprobante + "\" ");
                xmlFact.Append("MetodoPago=\"" + factura.MetodoPago + "\" ");
                xmlFact.Append("LugarExpedicion=\"" + factura.LugarExpedicion + "\"");

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

                //LCI. FIN 20180207 CORRECCION DE RFC CON CARACTER &

                if (factura.EsExtranjero)
                {
                    string regExPais = "";
                    if (datosCliente.PaisResidenciaFiscal.ToUpper() == "CAN" || datosCliente.PaisResidenciaFiscal.ToUpper() == "USD")
                    {
                        regExPais = "[0-9]{9}";
                        Regex rgx = new Regex(regExPais);

                        if (rgx.IsMatch(datosCliente.TAXID) == false)
                        {
                            throw new Exception("El Formato de Registro de Identidad Tributaria es incorrecto, verifique...");
                        }

                    }

                    //LCI. INI. 20180207 CORRECCION DE RFC CON CARACTER &
                    //xmlFact.Append("Rfc=\"" + factura.RfcReceptor + "\" ");
                    xmlFact.Append("Rfc=\"" + rfcFinal + "\" ");
                    //LCI. FIN. 20180207 CORRECCION DE RFC CON CARACTER &

                    xmlFact.Append("ResidenciaFiscal=\"" + factura.IdPaisResidenciaFiscal + "\" ");
                    xmlFact.Append("NumRegIdTrib=\"" + factura.NumRegIdTrib + "\" ");
                }
                else
                {
                    //LCI. INI. 20180207 CORRECCION DE RFC CON CARACTER &
                    //xmlFact.Append("Rfc=\"" + factura.RfcReceptor + "\" ");
                    xmlFact.Append("Rfc=\"" + rfcFinal + "\" ");
                    //LCI. FIN. 20180207 CORRECCION DE RFC CON CARACTER &
                }


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
                        xmlFact.Append("Unidad=\"" + concepto.Unidad + "\" ");
                        xmlFact.Append("Descripcion=\"" + concepto.Descripcion + "\" ");
                        xmlFact.Append("ValorUnitario=\"" + concepto.ValorUnitario.ToString("0.00") + "\" ");
                        xmlFact.Append("Importe=\"" + concepto.Importe.ToString("0.00") + "\"");

                        if (concepto.Descuento > 0)
                            xmlFact.Append("Descuento=\"" + concepto.Descuento.ToString("0.00") + "\" ");

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
                            xmlFact.Append("Importe=\"" + ivaConcepto.Importe.ToString("0.00") + "\"");
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
                    else if (concepto.Importe < 0)
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
                    xmlFact.Append("ValorUnitario=\"" + Math.Round(paquete.items[1].sellingPrice, 2) + "\" ");
                    xmlFact.Append("Importe=\"" + Math.Round(paquete.items[1].sellingPrice, 2) + "\"");
                    xmlFact.Append(">");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("				<Impuestos>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                    <Traslados>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                        <Traslado ");
                    xmlFact.Append("Base=\"" + Math.Round(paquete.items[1].sellingPrice, 2) + "\" ");
                    xmlFact.Append("Impuesto=\"002\" ");
                    xmlFact.Append("TipoFactor=\"Tasa\" ");
                    if(paquete.items[1].location.countryCode == "MX")
                    {
                        double ivaHotelP = Math.Round(paquete.items[1].sellingPrice * 0.16, 2);
                        xmlFact.Append("TasaOCuota=\"0.160000\" ");
                        xmlFact.Append("Importe=\"" + ivaHotelP + "\"");
                    }
                    else
                    {
                        xmlFact.Append("TasaOCuota=\"0.000000\" ");
                        xmlFact.Append("Importe=\"0.00\"");
                    }
                    
                    xmlFact.Append("/>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("					</Traslados>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                </Impuestos>");
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
                    xmlFact.Append("valorUnitario=\"" + Math.Round(paquete.items[1].sellingPrice, 2) + "\" ");
                    xmlFact.Append("importe=\"" + Math.Round(paquete.items[1].sellingPrice, 2) + "\"");
                    xmlFact.Append("/>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                        <Impuestos>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                            <Traslados>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                                <Traslado ");
                    //xmlFact.Append("Base=\"" + Math.Round(paquete.items[1].sellingPrice, 2) + "\" ");
                    xmlFact.Append("impuesto=\"IVA\" ");
                    //xmlFact.Append("TipoFactor=\"Tasa\" ");
                    if (paquete.items[1].location.countryCode == "MX")
                    {
                        double ivaHotelP = Math.Round(paquete.items[1].sellingPrice * 0.16, 2);
                        xmlFact.Append("tasa=\"16\" ");
                        xmlFact.Append("importe=\"" + ivaHotelP + "\"");
                    }
                    else
                    {
                        xmlFact.Append("tasa=\"0\" ");
                        xmlFact.Append("importe=\"0.00\"");
                    }

                    xmlFact.Append("/>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                            </Traslados>");
                    xmlFact.Append(Environment.NewLine);
                    xmlFact.Append("                        </Impuestos>");
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
                if (flg_Terceros)
                {
                    if (paquete.items[1].location.countryCode == "MX")
                    {
                        double iva = Math.Round(paquete.items[1].sellingPrice * 0.16, 2);
                        decimal totalImpuestos = factura.MontoIVA + Convert.ToDecimal(iva);
                        xmlFact.Append("TotalImpuestosTrasladados=\"" + totalImpuestos + "\">");
                    }
                    else
                    {
                        xmlFact.Append("TotalImpuestosTrasladados=\"" + factura.MontoIVA.ToString("0.00") + "\">");
                    }
                }
                
                else
                {
                    xmlFact.Append("TotalImpuestosTrasladados=\"" + factura.MontoIVA.ToString("0.00") + "\">");
                }
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
                            if (tasa == Convert.ToDecimal(0.16))
                            {
                                if (flg_Terceros)
                                {
                                    if (paquete.items[1].location.countryCode == "MX")
                                    {
                                        double iva = Math.Round(paquete.items[1].sellingPrice * 0.16, 2);
                                        decimal importeIVA = factura.ListaIVAPorDetalle.Where(x => x.Impuesto == cveImpuesto && x.TipoFactor == factor && x.TasaOCuota == tasa).Sum(x => x.Importe);
                                        sumIVA = Convert.ToDecimal(iva) + importeIVA;
                                    }

                                    else
                                    {
                                        sumIVA = factura.ListaIVAPorDetalle.Where(x => x.Impuesto == cveImpuesto && x.TipoFactor == factor && x.TasaOCuota == tasa).Sum(x => x.Importe);
                                    }
                                }
                                else
                                {
                                    sumIVA = factura.ListaIVAPorDetalle.Where(x => x.Impuesto == cveImpuesto && x.TipoFactor == factor && x.TasaOCuota == tasa).Sum(x => x.Importe);
                                }
                            }
                            else
                            {
                                sumIVA = factura.ListaIVAPorDetalle.Where(x => x.Impuesto == cveImpuesto && x.TipoFactor == factor && x.TasaOCuota == tasa).Sum(x => x.Importe);
                            }

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
                    foreach (var cargoItem in factura.ListaCargosAero)
                    {
                        string codigoCargo = cargoItem.CodigoCargo;
                        if (!ListaCodigosTUA.Contains(codigoCargo))
                        {
                            xmlFact.Append("					<Cargo ");

                            xmlFact.Append("CodigoCargo=\"" + codigoCargo + "\" ");
                            xmlFact.Append("Importe=\"" + cargoItem.Importe.ToString("0.00") + "\"/>");
                            xmlFact.Append(Environment.NewLine);
                        }


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


                xmlFact.Append("id=\"" + GeneratransactionID(factura.RfcEmisor, factura.FolioFactura, fechaEnvio, datosCliente.ClaveReservacion, factura.IdAgente).ToString() + "\"/>");
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



        public string GeneratransactionID(string rfcEmisor, long paymentID, DateTime fechaFactura, string pnr, long idAgente)
        {
            string result = "";

            try
            {
                string codigoSAP = "";
                PNR = pnr;
                ENTAgentesCat agenteSAP = new ENTAgentesCat();
                agenteSAP = ListaCatalogoAgentes.Where(x => x.IdAgente == idAgente).FirstOrDefault();

                if (agenteSAP != null)
                {
                    codigoSAP = agenteSAP.CodigoSAP;
                }
                else
                {
                    codigoSAP = ListaParametros.Where(x => x.Nombre == "CodigoSAPGenerico").FirstOrDefault().Valor; //"4000902"; 
                }

                result = rfcEmisor.Substring(0, 3) + codigoSAP + fechaFactura.ToString("yyMMdd") + pnr + "-" + paymentID.ToString();
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "GeneratransactionID");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result;
        }


        private string EnviarCorreoArchivos(List<ENTPagosPorFacturar> listaPagosFacturados, ENTDatosFacturacion datosCliente)
        {
            string result = "";

            List<string> listaArchivosAtach = new List<string>();
            List<ENTPagosPorFacturar> listaPagosSolicitados = new List<ENTPagosPorFacturar>();

            try
            {
                PNR = datosCliente.ClaveReservacion;
                //Se identifican solo los pagos que fueron solicitados para facturar en el portal

                var listaSolicitados = listaPagosFacturados.Where(x => x.EstaMarcadoParaFacturacion == true);
                foreach (ENTPagosPorFacturar pagoSel in listaSolicitados)
                {
                    listaPagosSolicitados.Add(pagoSel);
                }


                //Se genera la lista de los archivos que se van a enviar
                foreach (ENTPagosPorFacturar pago in listaPagosSolicitados)
                {
                    if (pago.RutaCFDI != null && pago.RutaCFDI.Length > 0) listaArchivosAtach.Add(pago.RutaCFDI);
                    if (pago.RutaPDF != null && pago.RutaPDF.Length > 0) listaArchivosAtach.Add(pago.RutaPDF);
                }

                //Se invoca el envio de correo
                EnviarEmail enviarCorreo = new EnviarEmail(ListaParametros, EmpresaEmisora);
                result = enviarCorreo.sendEmailFactura(datosCliente, EmpresaEmisora, listaPagosSolicitados, listaArchivosAtach);


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
                //string mensajeUsuario = "Por el momento no fue posible enviarle sus facturas por correo, sin embargo puede descargarlos desde este portal...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "EnviarCorreoArchivos");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result;
        }


        private ENTReservacion RecuperarDatosReservacion(long bookingID, ENTPagosPorFacturar pago)
        {
            ENTReservacion reserva = new ENTReservacion();
            try
            {

                //Recuperar el cabecero de la reservacion
                BLLReservaCab bllReserva = new BLL.BLLReservaCab();
                List<ENTReservaCab> listaReserva = new List<ENTReservaCab>();
                listaReserva = bllReserva.RecuperarReservaCabBookingid(bookingID);

                if (listaReserva.Count > 0)
                {
                    ENTReservaCab reservaRec = new ENTReservaCab();
                    reservaRec = listaReserva[0];

                    reserva.IdReservaCab = reservaRec.IdReservaCab;
                    reserva.IdEmpresa = reservaRec.IdEmpresa;
                    reserva.BookingID = reservaRec.BookingID;
                    reserva.RecordLocator = reservaRec.RecordLocator;
                    reserva.Estatus = reservaRec.Estatus;
                    reserva.NumJourneys = reservaRec.NumJourneys;
                    reserva.CurrencyCode = reservaRec.CurrencyCode;
                    reserva.OwningCarrierCode = reservaRec.OwningCarrierCode;
                    reserva.CreatedAgentID = reservaRec.CreatedAgentID;
                    reserva.CreatedDate = reservaRec.CreatedDate;
                    reserva.ModifiedAgentID = reservaRec.ModifiedAgentID;
                    reserva.ModifiedDate = reservaRec.ModifiedDate;
                    reserva.ChannelTypeID = reservaRec.ChannelTypeID;
                    reserva.CreatedOrganizationCode = reservaRec.CreatedOrganizationCode;
                    reserva.MontoTotal = reservaRec.MontoTotal;
                    reserva.MontoPagado = reservaRec.MontoPagado;
                    reserva.MontoFacturado = reservaRec.MontoFacturado;
                    reserva.FoliosFacturacion = reservaRec.FoliosFacturacion;
                    reserva.FechaHoraLocal = reservaRec.FechaHoraLocal;
                    reserva.FechaModificacion = reservaRec.FechaModificacion;
                    reserva.EmailAddress = reservaRec.EmailAddress;

                    PNR = reservaRec.RecordLocator;
                    List<ENTReservaDet> listaDetalleReserva = new List<ENTReservaDet>();


                    if (pago.EsPagoDividido && pago.EsPagoPadre)
                    {
                        BLLReservaDet bllDetReserva = new BLLReservaDet();
                        listaDetalleReserva = bllDetReserva.RecuperarReservaDetFolioprefactura(pago.FolioPrefactura);
                    }
                    else
                    {
                        BLLReservaDet bllDetReserva = new BLLReservaDet();
                        listaDetalleReserva = bllDetReserva.RecuperarReservaDetIdreservacab(reserva.IdReservaCab);
                    }

                    reserva.ListaReservaDet = listaDetalleReserva;
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
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "RecuperarDatosReservacion");
                throw new ExceptionViva(mensajeUsuario);
            }
            return reserva;
        }

        public void CorregirTipoTarjeta(long bookingID)
        {
            try
            {
                // https://lookup.binlist.net/549949
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<ENTPagosPorFacturar> RecuperarPagosParaFacturar(ENTDatosFacturacion datosFacturacion, Paquete paquete)
        {
            List<ENTPagosPorFacturar> result = new List<ENTPagosPorFacturar>();

            try
            {

                BLLDistribucionPagos bllDistribuirPago = new BLLDistribucionPagos();
                List<ENTPagosFacturados> listaPagos = new List<ENTPagosFacturados>();

                PNR = datosFacturacion.ClaveReservacion;

                //Se verifica si ya se proceso el PNR
                BLLReservaCab bllReserva = new BLLReservaCab();
                bllReserva.RecuperarReservaCabRecordlocator(datosFacturacion.ClaveReservacion);

                bool seModificoReserva = false;
                DataTable dtReservaNav = new DataTable();
                dtReservaNav = RecuperarReservaPorPNR(datosFacturacion.ClaveReservacion);

                long bookingID = 0;


                if (dtReservaNav.Rows.Count > 0)
                {
                    DataRow drReser = dtReservaNav.Rows[0];
                    DateTime fechaModif = (DateTime)drReser["ModifiedDate"];
                    bookingID = Convert.ToInt64(drReser["BookingID"].ToString());
                    seModificoReserva = (bllReserva.ModifiedDate != fechaModif);
                }

                //Verifica la reservacion ya fue procesada y no ha sido modificada
                if (bllReserva.IdReservaCab > 0 && seModificoReserva == false)
                {
                    //Se recupera la lista de pagos de la reservacion
                    BLLPagosCab bllPagosProc = new BLLPagosCab();
                    List<ENTPagosCab> listaPagosProcesados = new List<ENTPagosCab>();
                    listaPagosProcesados = bllPagosProc.RecuperarPagosCabBookingid(bllReserva.BookingID);

                    if (listaPagosProcesados.Where(x => String.IsNullOrEmpty(x.OrganizationCode)).Count() > 0)
                        seModificoReserva = true;


                    if (seModificoReserva == false && listaPagosProcesados.Where(x => x.EsPagoPadre == true).Count() > 0)
                    {
                        List<long> listaPagosPadres = new List<long>();
                        listaPagosPadres = listaPagosProcesados.Where(x => x.EsPagoPadre == true).Select(x => x.PaymentID).Distinct().ToList();

                        BLLPagosCab bllPagos = new BLL.BLLPagosCab();
                        foreach (long pagoPadre in listaPagosPadres)
                        {
                            List<ENTPagosCab> listaPagosProcesadosHijos = new List<ENTPagosCab>();
                            listaPagosProcesadosHijos = bllPagos.RecuperarPagosCabFolioprefactura(pagoPadre);
                            if (listaPagosProcesadosHijos.Count > 0)
                            {
                                foreach (ENTPagosCab pagoHijo in listaPagosProcesadosHijos)
                                {
                                    if (pagoHijo.EsPagoHijo && pagoHijo.PaymentID != pagoPadre)
                                    {
                                        listaPagosProcesados.Add(pagoHijo);

                                    }
                                }

                            }
                        }
                    }


                    if (seModificoReserva == false)
                    {
                        foreach (var pagoProc in listaPagosProcesados)
                        {
                            ENTPagosFacturados pagoFac = new ENTPagosFacturados();
                            //Se convierte el pago 
                            pagoFac = CrearPagoFacturado(pagoProc);

                            //Se recupera la entidad de forma de pago
                            pagoFac.ENTFormaPago = ListaCatalogoFormasPago.Where(x => x.IdFormaPago == pagoFac.IdFormaPago).FirstOrDefault();

                            //Se recupera el desglose del pago
                            List<ENTPagosDet> listaDetallePagosProc = new List<ENTPagosDet>();
                            BLLPagosDet bllPagosDet = new BLLPagosDet();
                            listaDetallePagosProc = bllPagosDet.RecuperarPagosDetIdpagoscab(pagoFac.IdPagosCab);
                            pagoFac.ListaDesglosePago = listaDetallePagosProc;
                            //Se agrega el pago a la lista
                            listaPagos.Add(pagoFac);
                        }
                    }
                }

                if (seModificoReserva)
                {
                    //Se envia a procesar la Reservacion
                    listaPagos = bllDistribuirPago.DistribuirPagosEnReservacion(datosFacturacion.ClaveReservacion, paquete);
                }


                //LCI. INI. IVA FRANJA FRONTERIZA
                //SE ANALIZA LA INFORMACION DE LOS PAGOS IDENTIFICANDO SI ALGUNO DE ELLOS PERTENECE A LA FRANJA FRONTERIZA Y SI TIENE PORCENTAJES DE IVA POR BLOQUEAR
                bool bloquearPorIVAFrontera = false;

                //Solo en caso de que el parametro de IVA por Bloqueo tenga algun valor entonces se validara el comportamiento por IVA
                if (ListaPorcIVABloqueado.Count > 0)
                {
                    List<ENTReservaDet> listaDetRes = new List<ENTReservaDet>();
                    //Se recorren todos los pagos vinculados con la reservación
                    foreach (ENTPagosFacturados pagoFactRev in listaPagos)
                    {
                        //Por cada pago se recupera el detalle de cargos
                        BLLReservaDet bllReservaDet = new BLLReservaDet();
                        List<ENTReservaDet> listaDetPago = new List<ENTReservaDet>();
                        listaDetPago = bllReservaDet.RecuperarReservaDetIdpagoscab(pagoFactRev.IdPagosCab);
                        listaDetRes.AddRange(listaDetPago);
                    }

                    //Se busca si existe algun cargo con IVA Bloqueado
                    int numCargosIVABloqueo = 0;
                    numCargosIVABloqueo = listaDetRes.Where(x => ListaPorcIVABloqueado.Contains(x.PorcIva)).Count();
                    bloquearPorIVAFrontera = (numCargosIVABloqueo > 0);
                }
                //LCI. FIN. IVA FRANJA FRONTERIZA


                //Obtiene la lista de FoliosPrefactura

                List<long> listaPrefolios = new List<long>();

                var prefolios = listaPagos.Where(x => x.BookingID == bookingID && x.EsFacturable == true).OrderBy(x => x.FolioPrefactura).Select(x => x.FolioPrefactura).Distinct();

                //CVG
                //ENTPagosFacturados test = new ENTPagosFacturados();
                //test = listaPagos.Where(x => x.BookingID == bookingID && x.PaymentID == 0).FirstOrDefault();
                //decimal montoHotel = 0;

                //if(test != null)
                //{
                //    if (test.FolioPrefactura == 0)
                //    {
                //        montoHotel = test.PaymentAmount;
                //    }
                //}
                
                foreach (long preFol in prefolios)
                {
                    ENTPagosFacturados pagoProcesado = new ENTPagosFacturados();

                    pagoProcesado = listaPagos.Where(x => x.BookingID == bookingID && x.PaymentID == preFol).FirstOrDefault();
                    //if (pagoProcesado != null && pagoProcesado.IdPagosCab > 0 && (pagoProcesado.FechaPago >= (DateTime.Now.AddDays(toleranciaDiasFacturacion * -1)) || pagoProcesado.EsFacturado))
                    if (pagoProcesado.FolioPrefactura == 0)
                    {
                        ENTPagosPorFacturar pagoPorFacturarHotel = new ENTPagosPorFacturar();
                        pagoPorFacturarHotel.PaymentAmount = pagoProcesado.PaymentAmount;
                        //pagoPorFacturarHotel.CollectedCurrencyCode = pagoProcesado.CollectedCurrencyCode;
                        pagoPorFacturarHotel.CurrencyCode = pagoProcesado.CurrencyCode;
                        pagoPorFacturarHotel.EsFacturable = pagoProcesado.EsFacturable;
                        pagoPorFacturarHotel.EsFacturableGlobal = pagoProcesado.EsFacturableGlobal;
                        pagoPorFacturarHotel.EsFacturado = pagoProcesado.EsFacturado;
                        pagoPorFacturarHotel.BookingID = pagoProcesado.BookingID;
                        pagoPorFacturarHotel.OrganizationCode = pagoProcesado.OrganizationCode;
                        pagoPorFacturarHotel.PaymentMethodCode = pagoProcesado.PaymentMethodCode;
                        pagoPorFacturarHotel.ListaDesglosePago = pagoProcesado.ListaDesglosePago;
                        pagoPorFacturarHotel.FechaPago = pagoProcesado.FechaPago;

                        //Verifica si existe registro de prorroga en la fecha de facturacion
                        BLLProrrogafactReg bllProrroga = new BLLProrrogafactReg();
                        List<ENTProrrogafactReg> listaRegPro = new List<ENTProrrogafactReg>();

                        listaRegPro = bllProrroga.RecuperarProrrogafactRegBookingid(pagoPorFacturarHotel.BookingID);
                        int numDiasProrroga = 0;


                        if (listaRegPro.Where(x => x.Activo == true).Count() > 0)
                        {
                            numDiasProrroga = listaRegPro.Where(x => x.Activo == true).OrderByDescending(x => x.FechaHoraLocal).Select(x => x.NumDiasFacturacion).FirstOrDefault();
                            toleranciaDiasFacturacion = numDiasProrroga;
                        }

                        //Verifica la vigencia para Facturar
                        DateTime fechaLimiteBase = new DateTime();
                        fechaLimiteBase = pagoProcesado.FechaPago.AddDays(toleranciaDiasFacturacion);
                        DateTime fechaLimiteFact = new DateTime(fechaLimiteBase.Year, fechaLimiteBase.Month, fechaLimiteBase.Day, 23, 59, 59);

                        pagoPorFacturarHotel.EnVigenciaParaFacturacion = (pagoProcesado.EsFacturado || fechaLimiteFact >= (DateTime.Now));
                        if (!pagoPorFacturarHotel.EnVigenciaParaFacturacion)
                        {
                            pagoPorFacturarHotel.Mensaje = string.Format("Excedio la fecha limite para facturar: {0}", fechaLimiteFact.ToString("dd/MM/yyyy HH:mm"));
                        }

                        //result.Add(pagoPorFacturarHotel);
                    }
                    else if (pagoProcesado != null && pagoProcesado.IdPagosCab > 0)
                    {

                        decimal montoPago = 0;
                        int numPagosAgrupados = 0;
                        numPagosAgrupados = listaPagos.Where(x => x.FolioPrefactura == preFol).Count();
                        montoPago = listaPagos.Where(x => x.FolioPrefactura == preFol).Sum(x => x.MontoPorAplicar);

                        ENTPagosPorFacturar pagoPorFacturar = new ENTPagosPorFacturar();
                        pagoPorFacturar.IdPagosCab = pagoProcesado.IdPagosCab;
                        pagoPorFacturar.BookingID = pagoProcesado.BookingID;
                        pagoPorFacturar.PaymentID = pagoProcesado.PaymentID;
                        pagoPorFacturar.FechaPago = pagoProcesado.FechaPago;
                        pagoPorFacturar.FechaPagoUTC = pagoProcesado.FechaPagoUTC;
                        pagoPorFacturar.IdFormaPago = pagoProcesado.IdFormaPago;
                        pagoPorFacturar.PaymentMethodCode = pagoProcesado.PaymentMethodCode;
                        pagoPorFacturar.CurrencyCode = pagoProcesado.CurrencyCode;

                        //Se asigna el PaymentAmount con el monto por pagar acumulado con el Prefolio
                        if (pagoProcesado.PaymentMethodCode == "X9" && paquete.order != null && paquete.items[1].location.countryCode == "US")
                        {
                            //if (paquete.order != null)
                            //{
                            pagoPorFacturar.PaymentAmount = montoPago + Convert.ToDecimal(paquete.items[1].sellingPrice);
                            pagoPorFacturar.MontoPorAplicar = pagoProcesado.MontoPorAplicar + Convert.ToDecimal(paquete.items[1].sellingPrice);
                            pagoPorFacturar.CollectedAmount = pagoProcesado.CollectedAmount + Convert.ToDecimal(paquete.items[1].sellingPrice);
                            pagoPorFacturar.MontoTotal = pagoProcesado.MontoTotal + Convert.ToDecimal(paquete.items[1].sellingPrice);
                            //}
                        }

                        else if (pagoProcesado.PaymentMethodCode == "X9" && paquete.order != null && paquete.items[1].location.countryCode == "MX")
                        {
                            decimal ivaHotel;
                            ivaHotel = Convert.ToDecimal(Math.Round(paquete.items[1].sellingPrice * 0.16, 2));
                            //if (paquete.order != null)
                            //{
                            pagoPorFacturar.PaymentAmount = montoPago + Convert.ToDecimal(paquete.items[1].sellingPrice) + ivaHotel;
                            pagoPorFacturar.MontoPorAplicar = pagoProcesado.MontoPorAplicar + Convert.ToDecimal(paquete.items[1].sellingPrice) + ivaHotel;
                            pagoPorFacturar.CollectedAmount = pagoProcesado.CollectedAmount + Convert.ToDecimal(paquete.items[1].sellingPrice) + ivaHotel;
                            pagoPorFacturar.MontoTotal = pagoProcesado.MontoTotal + Convert.ToDecimal(paquete.items[1].sellingPrice) + ivaHotel;
                            //}
                        }

                        else
                        {
                            pagoPorFacturar.PaymentAmount = montoPago;
                            pagoPorFacturar.MontoPorAplicar = pagoProcesado.MontoPorAplicar;
                            pagoPorFacturar.CollectedAmount = pagoProcesado.CollectedAmount;
                            pagoPorFacturar.MontoTotal = pagoProcesado.MontoTotal;
                        }

                        pagoPorFacturar.CollectedCurrencyCode = pagoProcesado.CollectedCurrencyCode;
                        pagoPorFacturar.TipoCambio = pagoProcesado.TipoCambio;
                        pagoPorFacturar.Estatus = pagoProcesado.Estatus;
                        pagoPorFacturar.ParentPaymentID = pagoProcesado.ParentPaymentID;
                        pagoPorFacturar.EsPagoDividido = pagoProcesado.EsPagoDividido;
                        pagoPorFacturar.EsPagoPadre = pagoProcesado.EsPagoPadre;
                        pagoPorFacturar.EsPagoHijo = pagoProcesado.EsPagoHijo;
                        pagoPorFacturar.EsFacturable = pagoProcesado.EsFacturable;
                        pagoPorFacturar.EsParaAplicar = pagoProcesado.EsParaAplicar;
                        pagoPorFacturar.EsFacturado = pagoProcesado.EsFacturado;
                        pagoPorFacturar.IdFacturaCab = pagoProcesado.IdFacturaCab;
                        pagoPorFacturar.FolioPrefactura = pagoProcesado.FolioPrefactura;
                        pagoPorFacturar.FolioFactura = pagoProcesado.FolioFactura;
                        pagoPorFacturar.FechaFactura = pagoProcesado.FechaFactura;
                        pagoPorFacturar.FechaFacturaUTC = pagoProcesado.FechaFacturaUTC;
                        pagoPorFacturar.VersionFacturacion = pagoProcesado.VersionFacturacion;
                        pagoPorFacturar.MontoTarifa = pagoProcesado.MontoTarifa;
                        pagoPorFacturar.MontoServAdic = pagoProcesado.MontoServAdic;
                        pagoPorFacturar.MontoTUA = pagoProcesado.MontoTUA;
                        pagoPorFacturar.MontoOtrosCargos = pagoProcesado.MontoOtrosCargos;
                        pagoPorFacturar.MontoIVA = pagoProcesado.MontoIVA;
                        pagoPorFacturar.IdAgente = pagoProcesado.IdAgente;
                        pagoPorFacturar.FechaHoraLocal = pagoProcesado.FechaHoraLocal;
                        pagoPorFacturar.FechaUltimaActualizacion = pagoProcesado.FechaUltimaActualizacion;

                        //Se selecciona por default el pago para ser facturado                    
                        pagoPorFacturar.EstaMarcadoParaFacturacion = pagoProcesado.EsVinculacionCorrecta;
                        pagoPorFacturar.ListaDesglosePago = pagoProcesado.ListaDesglosePago;

                        //LCI. INI. 20181121 LugarExpedicion
                        pagoPorFacturar.LocationCode = pagoProcesado.LocationCode;
                        pagoPorFacturar.LugarExpedicion = pagoProcesado.LugarExpedicion;
                        //LCI. INI. 20181121 LugarExpedicion

                        //LCI. INI. 20190702 TicketFactura
                        pagoPorFacturar.OrganizationCode = pagoProcesado.OrganizationCode;
                        //LCI. FIN. 20190702 TicketFactura

                        // DHV INI 15-07-2019 Corrección forma de pago
                        pagoPorFacturar.BinRange = pagoProcesado.BinRange;
                        pagoPorFacturar.IdUpdFormaPago = pagoProcesado.IdUpdFormaPago;
                        pagoPorFacturar.UpdFormaPagModificadoPor = pagoProcesado.UpdFormaPagModificadoPor;
                        pagoPorFacturar.FechaUpdaFormaPag = pagoProcesado.FechaUpdaFormaPag;

                        if (pagoPorFacturar.IdFormaPago != pagoPorFacturar.IdUpdFormaPago)
                        {

                        }
                        // DHV FIN 15-07-2019 Corrección forma de pago

                        // DHV INI 20190815 Indentificar pagos de TPV
                        pagoPorFacturar.DepartmentCode = pagoProcesado.DepartmentCode;
                        pagoPorFacturar.DepartmentName = pagoProcesado.DepartmentName;
                        pagoPorFacturar.OrganizationName = pagoProcesado.OrganizationName;
                        pagoPorFacturar.OrganizationType = pagoProcesado.OrganizationType;
                        // DHV FIN 20190815 Indentificar pagos de TPV

                        //Verifica si existe registro de prorroga en la fecha de facturacion
                        BLLProrrogafactReg bllProrroga = new BLLProrrogafactReg();
                        List<ENTProrrogafactReg> listaRegPro = new List<ENTProrrogafactReg>();

                        listaRegPro = bllProrroga.RecuperarProrrogafactRegBookingid(pagoPorFacturar.BookingID);
                        int numDiasProrroga = 0;


                        if (listaRegPro.Where(x => x.Activo == true).Count() > 0)
                        {
                            numDiasProrroga = listaRegPro.Where(x => x.Activo == true).OrderByDescending(x => x.FechaHoraLocal).Select(x => x.NumDiasFacturacion).FirstOrDefault();
                            toleranciaDiasFacturacion = numDiasProrroga;
                        }

                        //Verifica la vigencia para Facturar
                        DateTime fechaLimiteBase = new DateTime();
                        fechaLimiteBase = pagoProcesado.FechaPago.AddDays(toleranciaDiasFacturacion);
                        DateTime fechaLimiteFact = new DateTime(fechaLimiteBase.Year, fechaLimiteBase.Month, fechaLimiteBase.Day, 23, 59, 59);

                        pagoPorFacturar.EnVigenciaParaFacturacion = (pagoProcesado.EsFacturado || fechaLimiteFact >= (DateTime.Now));
                        if (!pagoPorFacturar.EnVigenciaParaFacturacion)
                        {
                            pagoPorFacturar.Mensaje = string.Format("Excedio la fecha limite para facturar: {0}", fechaLimiteFact.ToString("dd/MM/yyyy HH:mm"));
                        }

                        //En caso de que sean pagos agrupados se muestra el desglose de todos los pagos
                        if (numPagosAgrupados > 0)
                        {
                            List<ENTPagosFacturados> listaDesglose = new List<ENTPagosFacturados>();
                            listaDesglose = listaPagos.Where(x => x.FolioPrefactura == preFol).DefaultIfEmpty().ToList();
                            foreach (ENTPagosFacturados pago in listaDesglose)
                            {
                                if (pago.PaymentID != pagoPorFacturar.PaymentID)
                                {
                                    pagoPorFacturar.ListaDesglosePago.AddRange(pago.ListaDesglosePago);
                                }
                            }
                        }


                        //Se recorre el desglose de pago para asignar las descripciones de las claves
                        foreach (ENTPagosDet detPago in pagoPorFacturar.ListaDesglosePago)
                        {
                            string moneda = detPago.CurrencyCode;
                            string formaPago = detPago.PaymentMethodCode;
                            string descFormaViva = "";
                            string desMetodoPago = "";

                            //Recupera la descripcion de la moneda
                            string desMoneda = RecuperarDescripcionCodigo("MONEDA", moneda);

                            //Recupera la descripcion de la forma de pago
                            ENTFormapagoCat formaVB = new ENTFormapagoCat();
                            formaVB = ListaCatalogoFormasPago.Where(x => x.PaymentMethodCode == formaPago).FirstOrDefault();
                            if (formaVB != null)
                            {
                                descFormaViva = formaVB.Descripcion.ToString();
                                desMetodoPago = RecuperarDescripcionCodigo("FRMPAG", formaVB.CveFormaPagoSAT);
                            }


                            detPago.CurrencyCode = string.Format("{0} ({1})", moneda, desMoneda.ToString().Trim());
                            detPago.PaymentMethodCode = string.Format("{0} ({1})", formaVB.Descripcion, desMetodoPago.ToString().Trim());

                        }

                        //LCI. INI. IVA FRANJA FRONTERIZA
                        //En caso de que existan Porcentajes de IVA que no estan liberados para facturar entonces se bloquearan los pagos.
                        if (bloquearPorIVAFrontera)
                        {
                            pagoPorFacturar.ConBloqueoFacturacion = bloquearPorIVAFrontera;
                            pagoPorFacturar.EnVigenciaParaFacturacion = false;
                            pagoPorFacturar.Mensaje = MensajeFacturaBloqueadaPorIVA;
                        }

                        //LCI. FIN. IVA FRANJA FRONTERIZA
                        //Se agrega al monto por facturar
                        result.Add(pagoPorFacturar);
                    }
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
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "RecuperarPagosParaFacturar");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }


        private string RecuperarDescripcionCodigo(string cveTabla, string codigo)
        {
            string result = "";
            ENT.ENTGendescripcionesCat descCat = new ENT.ENTGendescripcionesCat();
            descCat = ListaGenDescripciones.Where(x => x.CveTabla == cveTabla && x.CveValor == codigo).FirstOrDefault();
            if (descCat != null)
            {
                result = descCat.Descripcion.ToString();
            }

            return result;
        }


        private ENTPagosFacturados CrearPagoFacturado(ENTPagosCab pago)
        {
            ENTPagosFacturados result = new ENTPagosFacturados();

            try
            {
                result.IdPagosCab = pago.IdPagosCab;
                result.BookingID = pago.BookingID;
                result.PaymentID = pago.PaymentID;
                result.FechaPago = pago.FechaPago;
                result.FechaPagoUTC = pago.FechaPagoUTC;
                result.IdFormaPago = pago.IdFormaPago;
                result.PaymentMethodCode = pago.PaymentMethodCode;
                result.CurrencyCode = pago.CurrencyCode;
                result.PaymentAmount = pago.PaymentAmount;
                result.MontoPorAplicar = pago.MontoPorAplicar;
                result.CollectedCurrencyCode = pago.CollectedCurrencyCode;
                result.CollectedAmount = pago.CollectedAmount;
                result.TipoCambio = pago.TipoCambio;
                result.Estatus = pago.Estatus;
                result.ParentPaymentID = pago.ParentPaymentID;
                result.EsPagoDividido = pago.EsPagoDividido;
                result.EsPagoPadre = pago.EsPagoPadre;
                result.EsPagoHijo = pago.EsPagoHijo;
                result.EsFacturable = pago.EsFacturable;
                result.EsParaAplicar = pago.EsParaAplicar;
                result.EsFacturado = pago.EsFacturado;
                result.IdFacturaCab = pago.IdFacturaCab;
                result.FolioPrefactura = pago.FolioPrefactura;
                result.FolioFactura = pago.FolioFactura;
                result.FechaFactura = pago.FechaFactura;
                result.FechaFacturaUTC = pago.FechaFacturaUTC;
                result.VersionFacturacion = pago.VersionFacturacion;
                result.MontoTarifa = pago.MontoTarifa;
                result.MontoServAdic = pago.MontoServAdic;
                result.MontoTUA = pago.MontoTUA;
                result.MontoOtrosCargos = pago.MontoOtrosCargos;
                result.MontoIVA = pago.MontoIVA;
                result.MontoTotal = pago.MontoTotal;
                result.IdAgente = pago.IdAgente;
                result.FechaHoraLocal = pago.FechaHoraLocal;
                result.FechaUltimaActualizacion = pago.FechaUltimaActualizacion;
                //LCI. INI. 20181121 LugarExpedicion
                result.LocationCode = pago.LocationCode;
                result.LugarExpedicion = pago.LugarExpedicion;
                //LCI. FIN. 20181121 LugarExpedicion

                //LCI. INI. 20190702 TICKETFACTURA
                result.OrganizationCode = pago.OrganizationCode;
                //LCI. FIN. 20190702 TICKETFACTURA

                // DHV INI 15-07-2019 Corrección de forma de pago
                result.BinRange = pago.BinRange;
                result.IdUpdFormaPago = pago.IdUpdFormaPago;
                result.UpdFormaPagModificadoPor = pago.UpdFormaPagModificadoPor;
                result.FechaUpdaFormaPag = pago.FechaUpdaFormaPag;
                // DHV FIN 15-07-2019 Corrección de forma de pago

                // DHV INI 20190815 Indentificar pagos de TPV
                result.DepartmentCode = pago.DepartmentCode;
                result.DepartmentName = pago.DepartmentName;
                result.OrganizationName = pago.OrganizationName;
                result.OrganizationType = pago.OrganizationType;
                // DHV FIN 20190815 Indentificar pagos de TPV
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "CrearPagoFacturado");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;

        }

        private bool GuardarFactura(ref ENTFacturaCliente facturaCliente)
        {
            bool result = false;
            try
            {
                //Se guarda el Cabecero
                BLLFacturasCab bllFacturaCab = new BLL.BLLFacturasCab();

                long idFacturaCabOri = facturaCliente.IdFacturaCab;
                bllFacturaCab.IdFacturaCab = facturaCliente.IdFacturaCab;
                bllFacturaCab.IdEmpresa = facturaCliente.IdEmpresa;
                bllFacturaCab.BookingID = facturaCliente.BookingID;
                bllFacturaCab.FechaHoraExpedicion = facturaCliente.FechaHoraExpedicion;
                bllFacturaCab.TipoFacturacion = facturaCliente.TipoFacturacion;
                bllFacturaCab.Version = facturaCliente.Version;
                bllFacturaCab.Serie = facturaCliente.Serie;
                bllFacturaCab.FolioFactura = facturaCliente.FolioFactura;
                bllFacturaCab.UUID = facturaCliente.UUID;
                bllFacturaCab.TransactionID = facturaCliente.TransactionID;
                bllFacturaCab.IdPeticionPAC = facturaCliente.IdPeticionPAC;
                bllFacturaCab.Estatus = facturaCliente.Estatus;
                bllFacturaCab.RfcEmisor = facturaCliente.RfcEmisor;
                bllFacturaCab.RazonSocialEmisor = facturaCliente.RazonSocialEmisor;
                bllFacturaCab.NoCertificado = facturaCliente.NoCertificado;
                bllFacturaCab.IdRegimenFiscal = facturaCliente.IdRegimenFiscal;
                bllFacturaCab.RfcReceptor = facturaCliente.RfcReceptor;
                bllFacturaCab.RazonSocialReceptor = facturaCliente.RazonSocialReceptor;
                bllFacturaCab.EmailReceptor = facturaCliente.EmailReceptor;
                bllFacturaCab.EsExtranjero = facturaCliente.EsExtranjero;
                bllFacturaCab.IdPaisResidenciaFiscal = facturaCliente.IdPaisResidenciaFiscal;
                bllFacturaCab.NumRegIdTrib = facturaCliente.NumRegIdTrib;
                bllFacturaCab.UsoCFDI = facturaCliente.UsoCFDI;
                bllFacturaCab.FormaPago = facturaCliente.FormaPago;
                bllFacturaCab.MetodoPago = facturaCliente.MetodoPago;
                bllFacturaCab.TipoComprobante = facturaCliente.TipoComprobante;
                bllFacturaCab.LugarExpedicion = facturaCliente.LugarExpedicion;
                bllFacturaCab.CondicionesPago = facturaCliente.CondicionesPago;
                bllFacturaCab.Moneda = facturaCliente.Moneda;
                bllFacturaCab.TipoCambio = facturaCliente.TipoCambio;
                bllFacturaCab.SubTotal = facturaCliente.SubTotal;
                bllFacturaCab.Descuento = facturaCliente.Descuento;
                bllFacturaCab.Total = facturaCliente.Total;
                bllFacturaCab.MontoTarifa = facturaCliente.MontoTarifa;
                bllFacturaCab.MontoServAdic = facturaCliente.MontoServAdic;
                bllFacturaCab.MontoTUA = facturaCliente.MontoTUA;
                bllFacturaCab.MontoOtrosCargos = facturaCliente.MontoOtrosCargos;
                bllFacturaCab.MontoIVA = facturaCliente.MontoIVA;
                bllFacturaCab.IdAgente = facturaCliente.IdAgente;
                bllFacturaCab.IdUsuario = facturaCliente.IdUsuario;
                bllFacturaCab.FechaHoraLocal = facturaCliente.FechaHoraLocal;
                bllFacturaCab.IdUsuarioCancelo = facturaCliente.IdUsuarioCancelo;
                bllFacturaCab.FechaHoraCancelLocal = facturaCliente.FechaHoraCancelLocal;

                bllFacturaCab.Agregar();

                facturaCliente.IdFacturaCab = bllFacturaCab.IdFacturaCab;


                int idDetFinal = 0;
                //Agregando el detalle de la factura
                foreach (ENTFacturasDet entFactDet in facturaCliente.ListaFacturasDet.OrderByDescending(x => x.IdFacturaDet))
                {
                    idDetFinal++;
                    BLLFacturasDet facturaDet = new BLLFacturasDet();
                    entFactDet.IdFacturaCab = facturaCliente.IdFacturaCab;
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

                    //Se registra el IVA
                    var ivasDet = facturaCliente.ListaIVAPorDetalle.Where(x => x.IdFacturaCab == idFacturaCabOri && x.IdFacturaDet == entFactDet.IdFacturaDet);
                    foreach (ENTFacturasivaDet ivaDet in ivasDet)
                    {
                        BLLFacturasivaDet bllIvaDet = new BLLFacturasivaDet();
                        bllIvaDet.IdFacturaCab = facturaCliente.IdFacturaCab;
                        bllIvaDet.IdFacturaDet = idDetFinal;
                        bllIvaDet.TipoFactor = ivaDet.TipoFactor;
                        bllIvaDet.TasaOCuota = ivaDet.TasaOCuota;
                        bllIvaDet.Base = ivaDet.Base;
                        bllIvaDet.Impuesto = ivaDet.Impuesto;
                        bllIvaDet.Importe = ivaDet.Importe;
                        bllIvaDet.FechaHoraLocal = DateTime.Now;
                        bllIvaDet.Agregar();
                    }

                    var cargosDet = facturaCliente.ListaCargosAero.Where(x => x.IdFacturaCab == idFacturaCabOri && x.IdFacturaDet == entFactDet.IdFacturaDet);

                    foreach (ENTFacturascargosDet factCargosDet in cargosDet)
                    {
                        //Agregando los Cargos Adicionales
                        BLLFacturascargosDet facCargosDet = new BLLFacturascargosDet();
                        facCargosDet.IdFacturaCab = facturaCliente.IdFacturaCab;
                        facCargosDet.IdFacturaDet = idDetFinal;
                        facCargosDet.CodigoCargo = factCargosDet.CodigoCargo;
                        facCargosDet.EsTua = (ListaCodigosTUA.Contains(factCargosDet.CodigoCargo));
                        facCargosDet.Importe = factCargosDet.Importe;
                        facCargosDet.FechaHoraLocal = DateTime.Now;
                        facCargosDet.Agregar();
                    }

                }

                //Se agrega el CFDI
                BLLFacturascfdiDet cfdiFact = new BLLFacturascfdiDet();
                cfdiFact.IdFacturaCab = facturaCliente.IdFacturaCab;
                cfdiFact.TransaccionID = facturaCliente.TransactionID;
                cfdiFact.CFDI = facturaCliente.FacturasCFDIDet.CFDI;
                cfdiFact.CadenaOriginal = facturaCliente.FacturasCFDIDet.CadenaOriginal;
                cfdiFact.FechaTimbrado = facturaCliente.FacturasCFDIDet.FechaTimbrado;
                cfdiFact.FechaHoraLocal = DateTime.Now;
                cfdiFact.Agregar();

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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "GuardarFactura");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result;
        }


        private ENTPagosPorFacturar ActualizarPagoFacturado(ref List<ENTPagosCab> listaPagos, ENTPagosPorFacturar pagoFacturado, ENTFacturaCliente factura)
        {
            ENTPagosPorFacturar result = new ENTPagosPorFacturar();

            try
            {
                long folioPrefactura = pagoFacturado.FolioPrefactura;

                pagoFacturado.FolioFactura = factura.FolioFactura;
                pagoFacturado.EsFacturado = true;
                pagoFacturado.FechaFactura = factura.FechaHoraExpedicion;
                int horarioVerano = RecuperarHorasPorCambioHorario(factura.FechaHoraExpedicion);
                pagoFacturado.FechaFacturaUTC = factura.FechaHoraExpedicion.AddHours(horarioVerano);
                pagoFacturado.IdFacturaCab = factura.IdFacturaCab;
                pagoFacturado.VersionFacturacion = factura.Version;

                var folioFacturados = listaPagos.Where(x => x.FolioPrefactura == folioPrefactura);
                foreach (ENTPagosCab pago in folioFacturados)
                {
                    BLLPagosCab bllPago = new BLL.BLLPagosCab();
                    bllPago.RecuperarPagosCabPorLlavePrimaria(pago.IdPagosCab);
                    bllPago.FolioFactura = pagoFacturado.FolioFactura;
                    bllPago.EsFacturado = pagoFacturado.EsFacturado;
                    bllPago.FechaFactura = pagoFacturado.FechaFactura;
                    bllPago.FechaFacturaUTC = pagoFacturado.FechaFacturaUTC;
                    bllPago.IdFacturaCab = pagoFacturado.IdFacturaCab;
                    bllPago.VersionFacturacion = pagoFacturado.VersionFacturacion;
                    bllPago.Actualizar();
                }

                result = pagoFacturado;
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "ActualizarPagoFacturado");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }

        private int RecuperarHorasPorCambioHorario(DateTime fecha)
        {
            int result = 0;
            try
            {
                result = 6;
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "RecuperarHorasPorCambioHorario");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }

        private bool ActualizarReservacion(long bookingId, ENTPagosPorFacturar pagoFacturado)
        {
            bool result = false;
            try
            {
                //Recuperar el detalle de las reservaciones que se facturaron para cubrir las reservaciones divididas
                List<ENTReservaDet> listaDetalle = new List<ENTReservaDet>();

                BLLReservaDet bllDetReserva = new BLLReservaDet();
                listaDetalle = bllDetReserva.RecuperarReservaDetFolioprefactura(pagoFacturado.FolioPrefactura);

                //Se actualiza la informacion del detalle facturado
                var detPagados = listaDetalle.Where(x => x.FolioPreFactura == pagoFacturado.FolioPrefactura);
                foreach (ENTReservaDet resDet in detPagados)
                {
                    BLLReservaDet bllResDet = new BLL.BLLReservaDet();
                    bllResDet.RecuperarReservaDetIdreservacabIdreservadet(resDet.IdReservaCab, resDet.IdReservaDet);
                    if (bllResDet != null)
                    {
                        bllResDet.IdFacturaCab = pagoFacturado.IdFacturaCab;
                        bllResDet.EstatusFacturacion = "FA";
                        bllResDet.Actualizar();
                    }
                }


                //Genera lista de las reservaciones trabajadas
                List<long> listaReservas = new List<long>();
                listaReservas = listaDetalle.Select(x => x.IdReservaCab).Distinct().ToList();

                //Por cada reservacion se recorren los montos facturados para actualizar los totales de la reservacion
                foreach (long idResFact in listaReservas)
                {
                    //Se recupera la informacion de la reservacion
                    BLLReservaCab bllReservaCab = new BLL.BLLReservaCab();
                    bllReservaCab.RecuperarReservaCabPorLlavePrimaria(idResFact);


                    //Se recupera el detalle de la reservacion para identificar los que ya estan facturados
                    List<ENTReservaDet> listaDetalleFact = new List<ENTReservaDet>();
                    BLLReservaDet bllDetReservaFac = new BLLReservaDet();
                    listaDetalleFact = bllDetReservaFac.RecuperarReservaDetIdreservacab(idResFact);

                    //Se obtiene el monto total facturado de la reservacion
                    decimal montoFacturado = 0;
                    montoFacturado = listaDetalleFact.Where(x => x.EstatusFacturacion == "FA").Sum(x => x.ChargeAmount);

                    //Se genera la lista de los folios de factura en donde se encuentran los detalles de la reservacion
                    System.Text.StringBuilder listaFoliosFac = new System.Text.StringBuilder();

                    //Se identifican los IdFacturacionCab que estan asignados a los detalles
                    List<long> listaFolios = new List<long>();
                    listaFolios = listaDetalleFact.Where(x => x.EstatusFacturacion == "FA").Select(x => x.IdFacturaCab).Distinct().ToList();

                    //Se recorren los IdFactura para recuperar los folios de factura
                    string sepFolios = "";
                    foreach (long folioFac in listaFolios)
                    {
                        BLLFacturasCab bllFac = new BLLFacturasCab();
                        bllFac.RecuperarFacturasCabPorLlavePrimaria(folioFac);
                        listaFoliosFac.Append(sepFolios + bllFac.FolioFactura.ToString());
                        sepFolios = ",";
                    }

                    //Se actualizan los montos facturados de cada reserva y la lista de folios de factura
                    bllReservaCab.MontoFacturado = montoFacturado;
                    bllReservaCab.FoliosFacturacion = listaFoliosFac.ToString();
                    bllReservaCab.Actualizar();
                }

                //En caso de que no se presenten errores entonces se regresa la bandera en true
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "ActualizarReservacion");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }



        private decimal CalculaMontoBaseIVA(decimal montoBase, decimal tasaIVA, decimal montoIVAPorTasa)
        {
            decimal result = 0;
            try
            {
                //decimal calcIVA = 0;
                //calcIVA = Math.Round((montoBase * tasaIVA), 3);
                montoBase = Math.Round(montoBase, 2);

                decimal limiteSuperior = Math.Round(((montoBase + margenSupIVA) * tasaIVA), 2);// (calcIVA + margenSupIVA);
                decimal limiteInferior = Math.Truncate(((montoBase - margenInfIVA) * tasaIVA) * 100) / 100;//  (calcIVA - margenInfIVA);

                if (
                   (montoIVAPorTasa <= limiteSuperior)  //Valida el limite Inferior del importe de IVA
                    &&
                    (montoIVAPorTasa >= limiteInferior)  //Valida el limite superior del importe de IVA 
                   )
                {
                    result = montoBase;
                }
                else
                {
                    result = Math.Round((montoIVAPorTasa / tasaIVA), 2);
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
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "CalculaMontoBaseIVA");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;

        }


        public string GeneraArchivosFactura(long folioFactura)
        {
            string result = "";
            try
            {
                BLLPagosCab bllPago = new BLL.BLLPagosCab();
                bllPago.RecuperarPagosCabPaymentid(folioFactura);

                string rutaCFDI = "";

                if (bllPago.IdPagosCab > 0)
                {
                    BLLPDFCFDI bllPdf = new BLLPDFCFDI();
                    if (bllPago.VersionFacturacion == "3.3")
                    {
                        BLLReservaCab bllReserva = new BLLReservaCab();
                        bllReserva.RecuperarReservaCabBookingid(bllPago.BookingID);
                        BLLFacturascfdiDet bllFacturaCFDI = new BLL.BLLFacturascfdiDet();
                        bllFacturaCFDI.RecuperarFacturascfdiDetIdfacturacab(bllPago.IdFacturaCab);

                        rutaCFDI = bllPdf.GeneraArchivoCFDI(bllFacturaCFDI.CFDI, folioFactura, bllReserva.RecordLocator);
                        bllPdf.GeneraPDFFactura33(bllFacturaCFDI.CFDI, bllFacturaCFDI.CadenaOriginal, bllReserva.RecordLocator, false);
                    }
                    else if (bllPago.VersionFacturacion == "3.2")
                    {
                        string xmlCFDI = "";
                        string pnr = "";

                        //Recupero la informacion de la factura de la version 3.2 que se encuentra en la WB
                        DataTable dtPagoFacturado32 = new DataTable();
                        dtPagoFacturado32 = RecuperarDatosFacturaPorFolio(bllPago.BookingID, bllPago.FolioFactura);

                        if (dtPagoFacturado32.Rows.Count > 0)
                        {
                            DataRow drPagoFactura = dtPagoFacturado32.Rows[0];
                            xmlCFDI = drPagoFactura["XmlCFDI"].ToString();
                            pnr = drPagoFactura["RecordLocator"].ToString();
                            //Inicia proceso de Generacion de Archivos.
                            rutaCFDI = bllPdf.GeneraArchivoCFDI(xmlCFDI, folioFactura, pnr);
                            bllPdf.GeneraPDF32(xmlCFDI, pnr);
                        }
                    }

                }

                FileInfo archivoCFDI = new FileInfo(rutaCFDI);
                if (archivoCFDI.Exists)
                {
                    result = archivoCFDI.DirectoryName;
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
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "GeneraArchivosFactura");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }

        //LCI. INI. 2019-05-22. ENVIO DE CFDI POR FOLIO FACTURA


        public string EnviarCFDIPorCorreo(List<ENTPagosPorFacturar> listaPagosFacturados, ENTDatosFacturacion datosCliente)
        {
            string result = "";

            List<string> listaArchivosAtach = new List<string>();
            List<ENTPagosPorFacturar> listaPagosSolicitados = new List<ENTPagosPorFacturar>();

            try
            {
                PNR = datosCliente.ClaveReservacion;
                //Se identifican solo los pagos que fueron solicitados para facturar en el portal

                foreach (ENTPagosPorFacturar pagoSel in listaPagosFacturados)
                {
                    pagoSel.EstaMarcadoParaFacturacion = pagoSel.EsFacturado;
                    listaPagosSolicitados.Add(pagoSel);
                }



                //Se genera la lista de los archivos que se van a enviar
                foreach (ENTPagosPorFacturar pago in listaPagosSolicitados)
                {
                    if (pago.RutaCFDI != null && pago.RutaCFDI.Length > 0) listaArchivosAtach.Add(pago.RutaCFDI);
                    if (pago.RutaPDF != null && pago.RutaPDF.Length > 0) listaArchivosAtach.Add(pago.RutaPDF);
                }

                //Se invoca el envio de correo
                EnviarEmail enviarCorreo = new EnviarEmail(ListaParametros, EmpresaEmisora);
                result = enviarCorreo.sendEmailFactura(datosCliente, EmpresaEmisora, listaPagosSolicitados, listaArchivosAtach);


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
                //string mensajeUsuario = "Por el momento no fue posible enviarle sus facturas por correo, sin embargo puede descargarlos desde este portal...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "EnviarCorreoArchivos");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result;
        }


        public string GeneraArchivosFactura(ref ENTPagosPorFacturar pagoPorEnviar)
        {
            string result = "";
            try
            {
                BLLPagosCab bllPago = new BLL.BLLPagosCab();
                bllPago.RecuperarPagosCabPaymentid(pagoPorEnviar.FolioFactura);

                string rutaCFDI = "";

                if (bllPago.IdPagosCab > 0)
                {
                    BLLPDFCFDI bllPdf = new BLLPDFCFDI();
                    if (bllPago.VersionFacturacion == "3.3")
                    {
                        BLLReservaCab bllReserva = new BLLReservaCab();
                        bllReserva.RecuperarReservaCabBookingid(bllPago.BookingID);
                        BLLFacturascfdiDet bllFacturaCFDI = new BLL.BLLFacturascfdiDet();
                        bllFacturaCFDI.RecuperarFacturascfdiDetIdfacturacab(bllPago.IdFacturaCab);

                        rutaCFDI = bllPdf.GeneraArchivoCFDI(bllFacturaCFDI.CFDI, pagoPorEnviar.FolioFactura, bllReserva.RecordLocator);
                        string rutaPDF = "";
                        rutaPDF = bllPdf.GeneraPDFFactura33(bllFacturaCFDI.CFDI, bllFacturaCFDI.CadenaOriginal, bllReserva.RecordLocator, false);
                        pagoPorEnviar.RutaCFDI = rutaCFDI;
                        pagoPorEnviar.RutaPDF = rutaPDF;
                    }
                    else if (bllPago.VersionFacturacion == "3.2")
                    {
                        string xmlCFDI = "";
                        string pnr = "";

                        //Recupero la informacion de la factura de la version 3.2 que se encuentra en la WB
                        DataTable dtPagoFacturado32 = new DataTable();
                        dtPagoFacturado32 = RecuperarDatosFacturaPorFolio(bllPago.BookingID, bllPago.FolioFactura);

                        if (dtPagoFacturado32.Rows.Count > 0)
                        {
                            DataRow drPagoFactura = dtPagoFacturado32.Rows[0];
                            xmlCFDI = drPagoFactura["XmlCFDI"].ToString();
                            pnr = drPagoFactura["RecordLocator"].ToString();
                            //Inicia proceso de Generacion de Archivos.
                            rutaCFDI = bllPdf.GeneraArchivoCFDI(xmlCFDI, pagoPorEnviar.FolioFactura, pnr);
                            string rutaArcPDF = "";
                            rutaArcPDF = bllPdf.GeneraPDF32(xmlCFDI, pnr);

                            pagoPorEnviar.RutaCFDI = rutaCFDI;
                            pagoPorEnviar.RutaPDF = rutaArcPDF;
                        }
                    }

                }

                FileInfo archivoCFDI = new FileInfo(rutaCFDI);
                if (archivoCFDI.Exists)
                {
                    result = archivoCFDI.DirectoryName;
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
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "GeneraArchivosFactura");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }
        //LCI. INI. 2019-05-22. ENVIO DE CFDI POR FOLIO FACTURA


        //DHV INI 2019-07-09 LOGIN
        public string EnviarCorreoConfirmaAltaUsuario(String email, Guid codigoVerificacion)
        {
            string result = "";
            BLLParametrosCnf param = new BLLParametrosCnf();
            List<ENTParametrosCnf> listParam = new List<ENTParametrosCnf>();
            string url = String.Empty;

            try
            {
                listParam = param.RecuperarParametrosCnfNombre("UriRestFacturacion");
                url = listParam.FirstOrDefault().Valor;

                //Se invoca el envio de correo
                EnviarEmail enviarCorreo = new EnviarEmail(ListaParametros, EmpresaEmisora);
                result = enviarCorreo.sendConfirmarAlta(url, email, codigoVerificacion);
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
                //string mensajeUsuario = "Por el momento no fue posible enviarle sus facturas por correo, sin embargo puede descargarlos desde este portal...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "EnviarCorreoArchivos");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result;
        }

        public string EnviarCorreoRecordarPassword(string email, string pass)
        {
            string result = string.Empty;

            try
            {
                //Se invoca el envio de correo
                EnviarEmail enviarCorreo = new EnviarEmail(ListaParametros, EmpresaEmisora);
                result = enviarCorreo.sendContraseniaUsuario(email, pass);
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
                //string mensajeUsuario = "Por el momento no fue posible enviarle sus facturas por correo, sin embargo puede descargarlos desde este portal...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "EnviarCorreoArchivos");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result;
        }
        //DHV FIN

        public DataTable RecuperarPagosSinFacturarSinEmail()
        {
            DataTable dt = new DataTable();

            try
            {
                dt = GetPagosSinFacturarSinEmail();
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
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Facturacion", "RecuperarPagosSinFacturarSinEmail");
                throw new ExceptionViva(mensajeUsuario);
            }

            return dt;
        }
    }
}

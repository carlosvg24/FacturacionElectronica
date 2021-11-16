using Facturacion.DAL.ProcesoFacturacion;
using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.BLL.ProcesoFacturacion
{
    public class BLLProcesarPagos : DALProcesarPagos
    {

        #region Constructor

        public BLLProcesarPagos()
        : base(BLLConfiguracion.ConexionNavitaireWB)
        {
            BllLogErrores = new ProcesoFacturacion.BLLBitacoraErrores();
            InicializaVariablesGlobales();
        }

        #endregion Constructor

        #region Propiedades privadas ListasCatalogos
        public List<string> ListaCodigosTUA = new List<string>();
        public List<ENTFeeCat> ListaCatalogoFees = new List<ENTFeeCat>();
        public List<ENTFormapagoCat> ListaCatalogoFormasPago = new List<ENTFormapagoCat>();
        public List<ENTConceptosCat> ListaCatalogoConceptos = new List<ENTConceptosCat>();
        public List<ENTAgentesCat> ListaCatalogoAgentes = new List<ENTAgentesCat>();
        public List<ENTEmpresaCat> ListaCatalogoEmpresas = new List<ENTEmpresaCat>();
        public List<ENTGendescripcionesCat> ListaGenDescripciones = new List<ENTGendescripcionesCat>();
        public List<ENTParametrosCnf> ListaParametros = new List<ENTParametrosCnf>();
        private BLLBitacoraErrores BllLogErrores { get; set; }
        public string PNR { get; set; }
        #endregion ListasCatalogos


        private void InicializaVariablesGlobales()
        {
            try
            {


                //Inicializar Catalogo Fees
                BLLFeeCat bllFee = new BLLFeeCat();
                ListaCatalogoFees = bllFee.RecuperarTodo();

                //Iniciar Catalogo Formas de Pago
                BLLFormapagoCat bllFormaPago = new BLLFormapagoCat();
                ListaCatalogoFormasPago = bllFormaPago.RecuperarTodo();

                //Iniciar Catalogo Conceptos de Facturacion
                BLLConceptosCat bllConceptosFac = new BLLConceptosCat();
                ListaCatalogoConceptos = bllConceptosFac.RecuperarTodo();

                //Inicializa el catalogo de Agentes
                BLLAgentesCat bllCatalogoAgentes = new BLLAgentesCat();
                ListaCatalogoAgentes = bllCatalogoAgentes.RecuperarTodo();

                //Inicializa el catalogo de Empresas
                BLLEmpresaCat bllCatEmpresa = new BLLEmpresaCat();
                ListaCatalogoEmpresas = bllCatEmpresa.RecuperarTodo();

                //Inicializa el catalogo de Descripciones Generales que incluye los catalogos SAT
                BLLGendescripcionesCat bllDescripciones = new BLLGendescripcionesCat();
                ListaGenDescripciones = bllDescripciones.RecuperarTodo();

                //Inicializa la lista de Parametros de la aplicacion
                BLLParametrosCnf bllParam = new BLL.BLLParametrosCnf();
                ListaParametros = bllParam.RecuperarTodo();
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "ProcesarPagos", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "ProcesarPagos", "IniciarVariablesGlobales");
                throw new ExceptionViva(mensajeUsuario);
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fecha"></param>
        ///// <returns></returns>
        //public List<ENTDistribucionpagos> BLLRecuperarPNRConPagosPorFecha(DateTime fecha)
        //{
        //    return RecuperarPNRConPagosPorFecha(fecha);
        //}

        public string EjecutarDistribucionPagos(List<ENTDistribucionpagos> listaPNRConPagos, DateTime fecha)
        {
            string result = "";
            List<string> ListaErrores = new List<string>();
            try
            {
                //Recorre por multihilos todos los PNR que tuvieron al menos un pago en el dia indicado
                ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 20 };
                Parallel.ForEach(listaPNRConPagos, options, (pnr) =>
                {
                    string errorProc = "";
                    try
                    {

                        pnr.FechaProcesamiento = DateTime.Now;
                        BLLDistribucionPagos bllDist = new BLLDistribucionPagos();
                        bllDist.EnviarCorreoErrores = false;
                        bllDist.TipoProceso = "ProcesoGlobal";
                        //Se asignan los catalogos comunes para que no se consulte a la BD mas de lo necesario
                        bllDist.ListaCodigosTUA = ListaCodigosTUA;
                        bllDist.ListaCatalogoFees = ListaCatalogoFees;
                        bllDist.ListaCatalogoFormasPago = ListaCatalogoFormasPago;
                        bllDist.ListaCatalogoConceptos = ListaCatalogoConceptos;
                        bllDist.ListaCatalogoAgentes = ListaCatalogoAgentes;
                        bllDist.ListaCatalogoEmpresas = ListaCatalogoEmpresas;
                        bllDist.ListaGenDescripciones = ListaGenDescripciones;
                        bllDist.ListaParametros = ListaParametros;

                        //Se ejecuta la distribucion de pagos
                        bllDist.DistribuirPagosEnReservacion(pnr.RecordLocator, "ProcesoGlobal");
                        if (bllDist.ListaErrores.Count > 0)
                        {
                            pnr.ProcesoExitoso = false;
                            StringBuilder errores = new StringBuilder();
                            string sep = "";
                            foreach (string error in bllDist.ListaErrores)
                            {
                                errores.Append(sep);
                                errores.Append(error);
                                sep = ",\n";
                            }
                            errorProc = string.Format("{0}|PNR:{1}|Mensaje Error Distribucion:{2}", DateTime.Now, pnr.RecordLocator, errores.ToString());
                            pnr.MensajeError = errorProc;
                            pnr.ProcesoExitoso = false;
                        }
                        else
                        {
                            pnr.ProcesoExitoso = true;
                        }

                    }
                    catch (ExceptionViva ex)
                    {
                        BllLogErrores.RegistrarError(pnr.RecordLocator, ex.Message, ex, "ProcesarPagos", "EjecutarDistribucionPagos", false);
                    }
                    catch (SqlException ex)
                    {
                        string mensajeUsuario = "Se genero un error de comunicacion con la BD al procesar el PNR: " + pnr.RecordLocator;
                        BllLogErrores.RegistrarError(pnr.RecordLocator, mensajeUsuario, ex, "ProcesarPagos", "BD", false);//Lo unicoq le cambie
                    }
                    catch (Exception ex)
                    {
                        string mensajeUsuario = "Se genero un error al procesar el PNR: " + pnr.RecordLocator;
                        BllLogErrores.RegistrarError(pnr.RecordLocator, mensajeUsuario, ex, "ProcesarPagos", "EjecutarDistribucionPagos", false);
                    }
                    finally
                    {
                        if (errorProc.Length > 0)
                        {
                            pnr.ProcesoExitoso = false;
                            BllLogErrores.RegistrarError(pnr.RecordLocator, "", errorProc, "ProcesarPagos", "EjecutarDistribucionPagos", false);
                        }

                        BLLDistribucionpagos bllRegDisPagos = new BLLDistribucionpagos();
                        bllRegDisPagos.IdDistribucionPagos = pnr.IdDistribucionPagos;
                        bllRegDisPagos.BookingID = pnr.BookingID;
                        bllRegDisPagos.RecordLocator = pnr.RecordLocator;
                        bllRegDisPagos.CreatedDate = pnr.CreatedDate;
                        bllRegDisPagos.ModifiedDate = pnr.ModifiedDate;
                        bllRegDisPagos.FechaProcesamiento = fecha;
                        bllRegDisPagos.ProcesoExitoso = pnr.ProcesoExitoso;
                        bllRegDisPagos.ConDescartePorDiferencia = pnr.ConDescartePorDiferencia;
                        bllRegDisPagos.MensajeError = pnr.MensajeError;
                        bllRegDisPagos.FechaHoraLocal = DateTime.Now;
                        bllRegDisPagos.Agregar();
                    }

                });
                DateTime fechaInvalida = new DateTime();
                int procesadas = listaPNRConPagos.Where(x => x.FechaProcesamiento != fechaInvalida).Count();
                int conError = listaPNRConPagos.Where(x => x.ProcesoExitoso == false && x.MensajeError.Length > 0).Count();
                int procesadasOK = listaPNRConPagos.Where(x => x.ProcesoExitoso == true).Count();
                result = string.Format("true|{0}|{1}|{2}", procesadas, conError, procesadasOK, fecha);
            }
            catch (ExceptionViva ex)
            {
                result = string.Format("false|" + "Error al procesar la distribucion de pagos: Message: {0}|StackTrace: {1}", ex.Message, ex.StackTrace);
            }
            catch (SqlException ex)
            {
                result = string.Format("false|" + "Error de comunicación con la BD al procesar la distribucion de pagos: Message: {0}|StackTrace: {1}", ex.Message, ex.StackTrace);
                string mensajeUsuario = "";
                BllLogErrores.RegistrarError("", mensajeUsuario, ex, "ProcesarPagos", "BD");
            }
            catch (Exception ex)
            {
                result = string.Format("false|" + "Error al procesar la distribucion de pagos: Message: {0}|StackTrace: {1}", ex.Message, ex.StackTrace);
                string mensajeUsuario = "";
                BllLogErrores.RegistrarError("", mensajeUsuario, ex, "ProcesarPagos", "EjecutarDistribucionPagos");
            }
            finally
            {
                if (BllLogErrores.ListaErroresAcumulados.Count > 0)
                {
                    BllLogErrores.EnviarCorreoErrores(string.Format("ProcesoDistribucion: {0}", fecha.ToShortDateString()));
                }
            }
            return result;
        }

        public string EjecutarDistribucionPagos(List<string> listaPNR)
        {
            string result = "";
            List<string> ListaErrores = new List<string>();
            try
            {
                //Recorre por multihilos todos los PNR que tuvieron al menos un pago en el dia indicado
                ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 20 };
                Parallel.ForEach(listaPNR, options, (pnr) =>
                {
                    try
                    {


                        BLLDistribucionPagos bllDist = new BLLDistribucionPagos();
                        //Se asignan los catalogos comunes para que no se consulte a la BD mas de lo necesario
                        bllDist.ListaCodigosTUA = ListaCodigosTUA;
                        bllDist.ListaCatalogoFees = ListaCatalogoFees;
                        bllDist.ListaCatalogoFormasPago = ListaCatalogoFormasPago;
                        bllDist.ListaCatalogoConceptos = ListaCatalogoConceptos;
                        bllDist.ListaCatalogoAgentes = ListaCatalogoAgentes;
                        bllDist.ListaCatalogoEmpresas = ListaCatalogoEmpresas;
                        bllDist.ListaGenDescripciones = ListaGenDescripciones;
                        bllDist.ListaParametros = ListaParametros;

                        //Se ejecuta la distribucion de pagos
                        bllDist.DistribuirPagosEnReservacion(pnr, string.Empty);
                        if (bllDist.ListaErrores.Count > 0)
                        {
                            StringBuilder errores = new StringBuilder();
                            string sep = "";
                            foreach (string error in bllDist.ListaErrores)
                            {
                                errores.Append(sep);
                                errores.Append(error);
                                sep = ",\n";
                            }
                            ListaErrores.Add(string.Format("PNR: {0}, Mensaje Error Distribucion: {1}", pnr, errores.ToString()));
                        }

                    }
                    catch (ExceptionViva ex)
                    {
                        ListaErrores.Add(string.Format("PNR: {0}, Mensaje: {1}, StackTrace: {2}", pnr, ex.Message, ex.StackTrace));
                    }
                    catch (SqlException ex)
                    {
                        string mensajeUsuario = "Se genero un error de comunicación con la BD al procesar el PNR: " + pnr;
                        ListaErrores.Add(string.Format("PNR: {0}, Mensaje: {1}, StackTrace: {2}", pnr, ex.Message, ex.StackTrace));
                        BllLogErrores.RegistrarError(pnr, mensajeUsuario, ex, "ProcesarPagos", "BD");
                    }
                    catch (Exception ex)
                    {
                        string mensajeUsuario = "Se genero un error al procesar el PNR: " + pnr;
                        ListaErrores.Add(string.Format("PNR: {0}, Mensaje: {1}, StackTrace: {2}", pnr, ex.Message, ex.StackTrace));
                        BllLogErrores.RegistrarError(pnr, mensajeUsuario, ex, "ProcesarPagos", "EjecutarDistribucionPagos");
                    }
                });
                int procesadas = listaPNR.Count();
                int conError = ListaErrores.Count();
                int procesadasOK = procesadas - conError;
                result = string.Format("true|" + "Se concluyo la distribución de pagos, \n Reservaciones procesadas : {0},\nReservaciones con error : {1}, \nReservaciones OK : {2}", procesadas, conError, procesadasOK);
            }
            catch (Exception ex)
            {
                result = string.Format("false|" + "Error al procesar la distribucion de pagos: Message: {0}|StackTrace: {1}", ex.Message, ex.StackTrace);
                Console.Write(ex.Message);
            }
            return result;

        }

        public void SincronizarPagosNavitaire()
        {
            //Se ejecuta la Actualizacion de pagos en el servidor de Navitaire
            ActualizarPagosPorFacturarNav();

            //Se invoca la importacion de pagos de Navitaire hacia Facturacion
            SqlConnection conn = new SqlConnection();
            conn = BLLConfiguracion.Conexion; 
            DALDistribucionPagos dalDis = new DALDistribucionPagos(conn);
            dalDis.SincronizarPagosNav();
        }

        public void ProcesarPagosFaltantes()
        {
            SqlConnection conn = new SqlConnection();
            conn = BLLConfiguracion.Conexion;
            DALDistribucionPagos dalDis = new DALDistribucionPagos(conn);
            List<ENTDistribucionpagos> listaPnr = new List<ENTDistribucionpagos>();

            listaPnr = dalDis.RecuperarListaPNRRemanentes();
            DateTime fechaProceso = DateTime.Now; 
            EjecutarDistribucionPagos(listaPnr, fechaProceso);
        }


        #region Metodos Para Facturacion Process Manager

        #endregion
    }
}

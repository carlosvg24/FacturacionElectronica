using Comun.Email;
using Facturacion.DAL.ProcesoFacturacion;
using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.BLL.ProcesoFacturacion
{
    public class BLLBitacoraErrores : DALBitacoraErrores
    {

        private List<ENTParametrosCnf> ListaParametros { get; set; }
        private List<ENTEmpresaCat> ListaCatalogoEmpresas = new List<ENTEmpresaCat>();
        private ENTEmpresaCat EmpresaEmisora { get; set; }
        private string cnfRegistrarLog;
        private string cnfRutaLog;
        private string nombreArchivo;

        private string listaCorreosNivel0 { get; set; }
        private string listaCorreosNivel1 { get; set; }
        private string listaCorreosNivel2 { get; set; }
        private string listaCorreosNivel3 { get; set; }
        private string listaCorreosNivel4 { get; set; }
        public Dictionary<byte, string> ListaCorreosPorNivel { get; set; }
        public List<string> ListaErroresAcumulados { get; set; }



        public BLLBitacoraErrores()
        : base(BLLConfiguracion.Conexion)
        {
            try
            {
                //Recupera los parametros de la BD
                ListaParametros = RecuperarParametros();
                cnfRegistrarLog = ListaParametros.Where(x => x.Nombre == "registrarLog").FirstOrDefault().Valor;
                cnfRutaLog = ListaParametros.Where(x => x.Nombre == "rutaLog").FirstOrDefault().Valor;
                //Verifica la existencia del archivo Log
                nombreArchivo = VerificaExisteArchivoLog(cnfRutaLog);
                ListaErroresAcumulados = new List<string>();

                BLLEmpresaCat bllCatEmpresa = new BLLEmpresaCat();
                ListaCatalogoEmpresas = bllCatEmpresa.RecuperarTodo();

                byte idEmpresa = Convert.ToByte(ListaParametros.Where(x => x.Nombre == "IdEmpresa").FirstOrDefault().Valor); // IdEmpresa
                EmpresaEmisora = ListaCatalogoEmpresas.Where(x => x.IdEmpresa == idEmpresa).FirstOrDefault();

                //Recupera la lista de correos por nivel de Error
                listaCorreosNivel0 = ListaParametros.Where(x => x.Nombre == "CorreosErrorNivel0").FirstOrDefault().Valor;
                listaCorreosNivel1 = ListaParametros.Where(x => x.Nombre == "CorreosErrorNivel1").FirstOrDefault().Valor;
                listaCorreosNivel2 = ListaParametros.Where(x => x.Nombre == "CorreosErrorNivel2").FirstOrDefault().Valor;
                listaCorreosNivel3 = ListaParametros.Where(x => x.Nombre == "CorreosErrorNivel3").FirstOrDefault().Valor;
                listaCorreosNivel4 = ListaParametros.Where(x => x.Nombre == "CorreosErrorNivel4").FirstOrDefault().Valor;

                ListaCorreosPorNivel = new Dictionary<byte, string>();
                ListaCorreosPorNivel.Add(0, listaCorreosNivel0);
                ListaCorreosPorNivel.Add(1, listaCorreosNivel1);
                ListaCorreosPorNivel.Add(2, listaCorreosNivel2);
                ListaCorreosPorNivel.Add(3, listaCorreosNivel3);
                ListaCorreosPorNivel.Add(4, listaCorreosNivel4);

            }
            catch (Exception ex)
            {
                RegistrarError("", "Error al abrir bitacora de errores", ex, "BitacoraErrores", "InicializarLog");
            }
        }



        public void RegistrarError(string mensajeUsuario, string mensajeDev, string modulo, string tipoError)
        {
            RegistrarError("", mensajeUsuario, mensajeDev, modulo, tipoError, true);
        }

        public void RegistrarError(string pnr, string mensajeUsuario, Exception ex, string modulo, string tipoError)
        {
            string mensajeDev = string.Empty;
            if (tipoError == "BD")
            {
                string errors = "";
                string sepError = "";
                foreach (var error in ((SqlException)ex).Errors)
                {
                    errors += sepError + error.ToString();
                    sepError = ", ";
                }
                mensajeDev = string.Format("Servidor --> {0}.\r\n\t\t\tSP --> {1}.\r\n\t\t\tLinea --> {2}.\r\n\t\t\tErrores --> {3}.\r\n\t\t\tMessage -> {4}.\r\n\t\t\tInnerException -> {5}.\r\n\t\t\tStackTrace -> {6}. \r\n", ((SqlException)ex).Server.ToString(),(((SqlException)ex).Procedure != null ? ((SqlException)ex).Procedure.ToString():""), ((SqlException)ex).LineNumber.ToString(), errors, ex.Message, ex.InnerException, ex.StackTrace);
            }
            else
            {
                mensajeDev = string.Format("Message -> {0}.\r\n\t\t\tInnerException -> {1}.\r\n\t\t\tStackTrace -> {2}. \r\n", ex.Message, ex.InnerException, ex.StackTrace);
            }

            RegistrarError(pnr, mensajeUsuario, mensajeDev.ToString(), modulo, tipoError, true);
        }

        public void RegistrarError(string pnr, string mensajeUsuario, Exception ex, string modulo, string tipoError, bool enviarCorreo)
        {
            string mensajeDev = string.Empty;

            if (tipoError == "BD")
            {
                string errors = "";
                string sepError = "";
                foreach (var error in ((SqlException)ex).Errors)
                {
                    errors += sepError + error.ToString();
                    sepError = ", ";
                }
                mensajeDev = string.Format("Servidor --> {0}.\r\n\t\t\tSP --> {1}.\r\n\t\t\tLinea --> {2}.\r\n\t\t\tErrores --> {3}.\r\n\t\t\tMessage -> {4}.\r\n\t\t\tInnerException -> {5}.\r\n\t\t\tStackTrace -> {6}. \r\n", ((SqlException)ex).Server.ToString(), ((SqlException)ex).Procedure.ToString(), ((SqlException)ex).LineNumber.ToString(), errors, ex.Message, ex.InnerException, ex.StackTrace);
            }
            else
            {
                mensajeDev = string.Format("Message -> {0}.\r\n\t\t\tInnerException -> {1}.\r\n\t\t\tStackTrace -> {2}. \r\n", ex.Message, ex.InnerException, ex.StackTrace);
            }


            RegistrarError(pnr, mensajeUsuario, mensajeDev.ToString(), modulo, tipoError, enviarCorreo);
        }

        public void RegistrarError(string pnr, string mensajeUsuario, string mensajeDev, string modulo, string tipoError)
        {
            RegistrarError(pnr, mensajeUsuario, mensajeDev, modulo, tipoError, true);
        }

        public void RegistrarError(string pnr, string mensajeUsuario, string mensajeDev, string modulo, string tipoError, bool enviarErrorPorCorreo)
        {
            try
            {
                pnr = pnr != null ? pnr : "";
                string separadorMensaje = "****************************************************************************************************************************************************************\r\n";
                string mensajeError = String.Format("{0}\tMensaje Usuario:{1}\r\n\t\t\tMensaje Dev:{2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), mensajeUsuario, mensajeDev.Replace("Error ->", "\r\n\t\t\tError ->"));

                //ASIGNANDO EL NIVEL DE IMPORTANCIA DEL ERROR DE ACUERDO AL TIPO, DONDE EL TIPO ES LA FUNCION DE C# DONDE SE PROVOCO EL ERROR
                byte nivelImportancia = 0;
                nivelImportancia = AsignaNivelImportanciaError(tipoError);


                try
                {
                    using (var fileStream = File.Open(nombreArchivo, FileMode.Append))
                    {
                        byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                        fileStream.Write(newline, 0, newline.Length);

                        var texto = new UTF8Encoding(true).GetBytes(separadorMensaje + mensajeError);
                        fileStream.Write(texto, 0, texto.Length);
                        fileStream.Flush();
                        fileStream.Close();


                    }
                }
                catch (Exception)
                {
                }

                try
                {




                    BLLLog bllRegLog = new BLL.BLLLog();
                    bllRegLog.Modulo = modulo;
                    bllRegLog.TipoError = tipoError;
                    bllRegLog.NivelImportancia = nivelImportancia;
                    bllRegLog.Descripcion = mensajeError;
                    bllRegLog.PNR = pnr != null ? pnr : "";
                    bllRegLog.FechaHoraLocal = DateTime.Now;
                    bllRegLog.Agregar();
                }
                catch (Exception)
                {

                }

                try
                {
                    ListaErroresAcumulados.Add(mensajeError);

                    if (enviarErrorPorCorreo)
                    {
                        string listaCorreosErrores = ListaCorreosPorNivel[nivelImportancia];
                        EnviarCorreoErrores(modulo, pnr, listaCorreosErrores);
                    }
                }
                catch (Exception)
                {

                }
            }
            catch (Exception)
            {
                //throw new Exception(String.Format("Comun.VerificaExisteArchivoLog. Error -> {0}, Parametros({1},{2})", ex.Message, mensajeUsuario, mensajeDev));
            }
        }

        private byte AsignaNivelImportanciaError(string tipoError)
        {
            byte result = 0;
            switch (tipoError)
            {
                case "ExisteCambioEnPNR":
                    result = 1;
                    break;
                case "Catalogos":
                    result = 1;
                    break;
                case "DistribuirPagosEnReservacion":
                    result = 1;
                    break;
                case "ValidarInformacionPNR":
                    result = 1;
                    break;
                case "RecuperarIdAgente":
                    result = 1;
                    break;
                case "GenerarListaPagos":
                    result = 1;
                    break;
                case "RecuperarPagoFacturado33":
                    result = 1;
                    break;
                case "GenerarReservacion":
                    result = 1;
                    break;
                case "GenerarVinculoPagosPrincipal":
                    result = 1;
                    break;
                case "AsignaPago":
                    result = 1;
                    break;
                case "GenerarListaDetalleReserva":
                    result = 1;
                    break;
                case "RecuperaDatosVuelo":
                    result = 1;
                    break;
                case "GuardarReserva":
                    result = 1;
                    break;
                case "ValidaDistribucionPago":
                    result = 1;
                    break;
                case "ExisteDiferenciaEntreBDVsProc":
                    result = 1;
                    break;
                case "AsignarIVAPorTarifa":
                    result = 1;
                    break;
                case "AsignarIVAPorSSR":
                    result = 1;
                    break;
                case "ClonarReservaDet":
                    result = 1;
                    break;
                case "GeneraListaVuelos":
                    result = 1;
                    break;
                case "OrdenaListaCargosPorPrioridad":
                    result = 1;
                    break;
                case "BuscarVinculosExistentesEnBD":
                    result = 1;
                    break;
                case "GenerarVinculosPagosComponentes":
                    result = 1;
                    break;
                case "GenerarVinculosPagosFacturados":
                    result = 1;
                    break;
                case "ReasignarAjustesNegativos":
                    result = 1;
                    break;
                case "InicializaVariablesGlobales":
                    result = 1;
                    break;
                case "RecuperarPagosPNR":
                    result = 1;
                    break;
                case "ValidarNombrePasajero":
                    result = 1;
                    break;
                case "GenerarFacturaCliente":
                    result = 1;
                    break;
                case "GenerarInformacionFactura":
                    result = 1;
                    break;
                case "GeneraXMLRequestFactura":
                    result = 1;
                    break;
                case "GeneratransactionID":
                    result = 1;
                    break;
                case "EnviarCorreoArchivos":
                    result = 1;
                    break;
                case "RecuperarDatosReservacion":
                    result = 1;
                    break;
                case "RecuperarPagosParaFacturar":
                    result = 1;
                    break;
                case "CrearPagoFacturado":
                    result = 1;
                    break;
                case "GuardarFactura":
                    result = 1;
                    break;
                case "ActualizarPagoFacturado":
                    result = 1;
                    break;
                case "RecuperarHorasPorCambioHorario":
                    result = 1;
                    break;
                case "ActualizarReservacion":
                    result = 1;
                    break;
                case "CalculaMontoBaseIVA":
                    result = 1;
                    break;
                case "GeneraArchivosFactura":
                    result = 1;
                    break;
                case "Constructor":
                    result = 1;
                    break;
                case "GeneraPDFFactura33":
                    result = 1;
                    break;
                case "GeneraArchivoCFDI":
                    result = 1;
                    break;
                case "CrearTituloBloque":
                    result = 1;
                    break;
                case "InsertarImagen":
                    result = 1;
                    break;
                case "RecuperarTituloDato":
                    result = 1;
                    break;
                case "RecuperarDescSAT":
                    result = 1;
                    break;
                case "CrearBloqueDatos":
                    result = 1;
                    break;
                case "CrearBloqueReceptor":
                    result = 1;
                    break;
                case "CrearLeyendaFija":
                    result = 1;
                    break;
                case "CrearBloqueTimbrado":
                    result = 1;
                    break;
                case "CrearBloqueTotales":
                    result = 1;
                    break;
                case "RecCamposOmitidosPorBloque":
                    result = 1;
                    break;
                case "CrearDatosEmisor":
                    result = 1;
                    break;
                case "CrearSerieFolio":
                    result = 1;
                    break;
                case "CrearBloqueConceptos":
                    result = 1;
                    break;
                case "CrearBloqueComplementosTUA":
                    result = 1;
                    break;
                case "CrearBloqueComplementosAero":
                    result = 1;
                    break;
                case "InsertarTituloCol":
                    result = 1;
                    break;
                case "InsertarDatoCelda":
                    result = 1;
                    break;
                case "RecDescripcionSAT":
                    result = 1;
                    break;
                case "GenerarPDF32":
                    result = 1;
                    break;
                case "RecuperarValorDR":
                    result = 1;
                    break;
                case "WSPegaso":
                    result = 4;
                    break;
                case "EnviaTimbradoPegaso":
                    result = 2;
                    break;
                case "EnviarTimbradoPegasoGlobal":
                    result = 2;
                    break;
                case "RecuperarHorasErrorFechaSAT":
                    result = 2;
                    break;
                case "ObtieneAjusteSegFechaSAT":
                    result = 2;
                    break;
                case "IniciarVariablesGlobales":
                    result = 2;
                    break;
                case "EjecutarDistribucionPagos":
                    result = 2;
                    break;
                case "Page_Load":
                    result = 0;
                    break;
                case "lkbBuscar_Click":
                    result = 0;
                    break;
                case "grvFaturas_RowDataBound":
                    result = 0;
                    break;
                case "grvFaturas_RowCommand":
                    result = 0;
                    break;
                case "grvFaturas_PageIndexChanging":
                    result = 0;
                    break;
                case "ConsultaDetalle":
                    result = 0;
                    break;
                case "lkbCerrar_Click":
                    result = 0;
                    break;
                case "lkbCancelar_Click":
                    result = 0;
                    break;
                case "lkbFacturar_Click":
                    result = 0;
                    break;
                case "GenerarZipFacturas":
                    result = 0;
                    break;
                case "grvDetalle_RowDataBound":
                    result = 0;
                    break;
                case "DescargarFactura":
                    result = 0;
                    break;
                case "BD":
                    result = 3;
                    break;
                default:
                    result = 1;
                    break;
            }
            return result;
        }

        public bool EnviarCorreoErrores(string procesoActual)
        {
            bool result = false;
            result = EnviarCorreoErrores(procesoActual, "", "");
            return result;
        }

        public bool EnviarCorreoErrores(string procesoActual, string pnr, string listaCorreos)
        {
            bool result = false;
            try
            {
                pnr = pnr != null ? pnr : "";
                if (ListaErroresAcumulados.Count > 0)
                {

                    EnviarEmail email = new EnviarEmail(ListaParametros, EmpresaEmisora);
                    email.sendEmailErrores(ListaErroresAcumulados, procesoActual, pnr, listaCorreos);
                    ListaErroresAcumulados = new List<string>();
                }
                result = true;
            }
            catch (Exception ex)
            {
                string mensajeLog = String.Format("Email.GenerarBodyErrores. Error -> {0}", ex.Message + "|| InnerException --> " + ex.InnerException);
                RegistrarError(pnr, "", mensajeLog, "BitacoraErrores", "Email", false);
            }
            return result;
        }


        private string VerificaExisteArchivoLog(string ruta)
        {
            string result = "";
            try
            {
                string nombreArchivo = "LogError" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".txt";

                //Verifica si existe el subdirectorio
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }

                //Verifica si existe el archivo
                string rutaCompletaArchivo = ruta + "\\" + nombreArchivo;
                if (!File.Exists(rutaCompletaArchivo))
                {
                    // crear el fichero
                    using (var fileStream = File.Create(rutaCompletaArchivo))
                    {
                        var texto = new UTF8Encoding(true).GetBytes(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " Creacion de archivo Log.");
                        fileStream.Write(texto, 0, texto.Length);
                        fileStream.Flush();
                        fileStream.Close();

                    }
                }
                result = rutaCompletaArchivo;
            }
            catch (Exception ex)
            {
                //throw new Exception(String.Format("Comun.VerificaExisteArchivoLog. Error -> {0}, Parametros({1})", ex.Message, ruta));
            }

            return result;
        }

    }
}

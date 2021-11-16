using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturacion.BLL;
using Facturacion.ENT;
using System.Web.Services;
using System.Net;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.ENT.ProcesoFacturacion;
using FacturacionOnLine.Class;
using Facturacion.ENT.Portal.Facturacion;
using System.Configuration;
using System.Web;
using System.IO;
using System.Text;
using System.Linq;
using System.Data;
using Facturacion.DAL;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Comun.Email;
using System.Text.RegularExpressions;
using VBFactPaquetes;
using VBFactPaquetes.Model;
using Newtonsoft.Json;

namespace FacturacionOnLine
{

    public class ReservacionesFacturar
    {
        public bool Facturar
        {
            get;
            set;
        }

        public String PNR
        {
            get;
            set;
        }
    }

    public partial class Default : System.Web.UI.Page
    {

        protected static string ReCaptcha_Key = "6Lcd3zIUAAAAAIf53dpsBtDYLzxKZtdOerXljq8p";
        protected static string ReCaptcha_Secret = "6Lcd3zIUAAAAAGxrXc1p7hkpVizTdci48K0Ex4K2";
        private static string email = String.Empty;
        private static bool fueExtranjero = false;
        Exportar exp = new Exportar();
        private BLLBitacoraErrores BllLogErrores = new BLLBitacoraErrores();

        #region "P R O P I E D A D E S"
        public string PNR
        {
            get
            {
                if (Session["PNR"] == null || Session["PNR"].ToString() == "")
                {
                    Session["PNR"] = txtPNR.Text.ToString().ToUpper();
                }

                return Session["PNR"].ToString();
            }

            set
            {
                Session["PNR"] = value;
            }
        }

        //public string PasswordOTA
        //{
        //    get
        //    {
        //        if (Session["PasswordOTA"] == null)
        //        {
        //            return "";
        //        }
        //        else
        //        {
        //            return Session["PasswordOTA"].ToString();
        //        }
        //    }

        //    set
        //    {
        //        Session["PasswordOTA"] = value;
        //    }
        //}

        public string MensajeErrorUsuario
        {
            get
            {
                if (Session["MensajeErrorUsuario"] == null)
                {
                    BLLParametrosCnf bllParam = new BLLParametrosCnf();
                    string valor = "";
                    List<ENTParametrosCnf> listaParam = new List<ENTParametrosCnf>();
                    listaParam = bllParam.RecuperarParametrosCnfNombre("MsjErrorPortal");
                    if (listaParam.Count > 0)
                    {
                        valor = listaParam.Where(x => x.Nombre == "MsjErrorPortal").FirstOrDefault().Valor;
                    }
                    else
                    {
                        valor = "Por el momento no es posible procesar su solicitud por favor intente más tarde...";

                    }
                    Session["MensajeErrorUsuario"] = valor;
                    return valor;
                }
                else
                {
                    return Session["MensajeErrorUsuario"].ToString();
                }
            }

        }

        public string RutaArchivoDescarga
        {
            get
            {
                if (Session["RutaArchivoDescarga"] == null)
                {
                    return "";
                }
                else
                {
                    return Session["RutaArchivoDescarga"].ToString();
                }
            }

            set
            {
                Session["RutaArchivoDescarga"] = value;
            }
        }

        public List<ENTPagosPorFacturar> ListaPagos
        {
            get
            {
                if (Session["ListaPagos"] == null)
                {
                    Session["ListaPagos"] = new List<ENTPagosPorFacturar>();
                }
                return (List<ENTPagosPorFacturar>)Session["ListaPagos"];
            }

            set
            {
                Session["ListaPagos"] = value;
            }
        }

        //public List<String> ListaPNRs
        //{
        //    get
        //    {
        //        if (Session["ListaPNRs"] == null)
        //        {
        //            Session["ListaPNRs"] = new List<String>();
        //        }

        //        return (List<String>)Session["ListaPNRs"];
        //    }

        //    set
        //    {
        //        Session["ListaPNRs"] = value;
        //    }
        //}

        public List<ReservacionesFacturar> ListaPNRs
        {
            get
            {
                if (Session["ListaPNRs"] == null)
                {
                    Session["ListaPNRs"] = new List<ReservacionesFacturar>();
                }

                return (List<ReservacionesFacturar>)Session["ListaPNRs"];
            }

            set
            {
                Session["ListaPNRs"] = value;
            }
        }
        #endregion


        [WebMethod]
        public static string VerifyCaptcha(string response)
        {
            // https://www.c-sharpcorner.com/article/integration-of-google-recaptcha-in-websites/


            string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + ReCaptcha_Secret + "&response=" + response;
            return (new WebClient()).DownloadString(url);
        }

        public bool IsReCaptchValid()
        {
            var result = false;
            var captchaResponse = Request.Form["g-recaptcha-response"];
            var secretKey = ReCaptcha_Secret;
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                    txtCaptcha.Text = result.ToString();
                }
            }
            return result;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Label mpLabel = (Label)Page.Master.FindControl("lblTitulo");
            //Response.Redirect("~/Default.aspx");

            try
            {
                txtRFC.Enabled = true;
                dvFacExtranjero.Visible = false;

                if (Session["Descargar"] != null && Session["Descargar"].ToString().Length > 0)
                {
                    OpenNewWindow(HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, @"/DownloadFact.aspx"));
                    Session["Descargar"] = String.Empty;
                    MostrarDialogo("informacion", "Sus facturas fueron descargadas.");
                }

                if (Session["Facturo"] != null && Session["Facturo"].ToString().Length > 0)
                {
                    OpenNewWindow(HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, @"/DownloadFact.aspx"));
                    MostrarDialogo("informacion", "Sus facturas fueron enviadas a <b>" + Session["Facturo"].ToString() + "</b>");
                    Session["Facturo"] = String.Empty;
                    btnFacturarPNRs.Visible = true;
                }

                if (Session["Reenviar"] != null && Session["Reenviar"].ToString().Length > 0)
                {
                    MostrarDialogo("informacion", "Sus facturas fueron enviadas a <b>" + Session["Reenviar"].ToString() + "</b>");
                    Session["Reenviar"] = String.Empty;
                }

                //Session["Login"] = "etickets@e-travelsolution.com";
                if (Session.Count > 0)
                {
                    if (Session["Login"] != null && Session["Login"].ToString().Length > 0)
                    {
                        email = Session["Login"].ToString();
                        pnlReservaPendFac.Visible = true;
                        pnlDatosReservacion.Visible = false;

                    }
                    else
                    {
                        email = String.Empty;
                        pnlReservaPendFac.Visible = false;
                        pnlDatosReservacion.Visible = true;
                    }
                }
                else
                {
                    email = String.Empty;
                    pnlReservaPendFac.Visible = false;
                    pnlDatosReservacion.Visible = true;
                }

                if (!Page.IsPostBack)
                {
                    if (!String.IsNullOrEmpty(email) && email.Length > 0)
                        ConsultarReservasPendFacturar();

                    //Carga del catalogo de Usos del CFDI publicado por el SAT
                    BLLGendescripcionesCat bllDescCat = new BLLGendescripcionesCat();
                    List<Facturacion.ENT.ENTGendescripcionesCat> listDescCat = new List<Facturacion.ENT.ENTGendescripcionesCat>();
                    ddlUsoCFDI.DataTextField = "Descripcion";
                    ddlUsoCFDI.DataValueField = "CveValor";
                    bllDescCat.CveTabla = "USOCFD";
                    ddlUsoCFDI.DataSource = bllDescCat.RecuperarGendescripcionesCatCvetabla("USOCFD");
                    ddlUsoCFDI.DataBind();

                    //Carga del catalogo de Pais de Referencia en caso de extranjeros
                    ddlPaisReferencia.DataTextField = "Descripcion";
                    ddlPaisReferencia.DataValueField = "CveValor";
                    bllDescCat.CveTabla = "PAISES";
                    ddlPaisReferencia.DataSource = bllDescCat.RecuperarGendescripcionesCatCvetabla("PAISES");
                    ddlPaisReferencia.DataBind();

                    //Se inicializan los valores de la pantalla de captura
                    //LimpiarPantalla();

                }
            }
            catch (ExceptionViva ex)
            {
                //ShowFalla("Portal de Facturación", ex.Message);
                MostrarDialogo("informacion", ex.Message);
            }
            catch (Exception ex)
            {

                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "Page_Load");
                //ShowFalla("Portal de Facturación", mensajeUsuario);
                MostrarDialogo("error", ex.Message);
            }
        }


        #region "F U N C I O N E S"
        protected string Mensaje_Texto(string cadena)
        {
            return cadena.Replace("'", @"\'");
        }

        private void MostrarDialogo(String tipo, String mensaje, String hdn_value = null)
        {
            try
            {
                // tipo: informacion, pregunta, alerta, error
                // hdn_value: es opcional (se ocupa para definir la sucesion de funciones en el boton aceptar del mensaje)
                //-- ejemplo: mostrar_dialogo("informacion", "datos guardados correctamente")

                // guarda valor en el control hidden
                //hdn_confirmacion.Value = hdn_value;

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "DIALOGO", "Mostrar_Dialogo('" + tipo + "','" + Mensaje_Texto(mensaje) + "');", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void OpenNewWindow(string url)
        {
            ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), String.Format("<script>window.open('{0}');</script>", url));
        }




        private void ConsultaDetalle(string Folio)
        {
            try
            {
                List<ENTPagosPorFacturar> listaPagos = new List<ENTPagosPorFacturar>();
                listaPagos = ListaPagos;

                ENTPagosPorFacturar pagoSel = new ENTPagosPorFacturar();
                pagoSel = listaPagos.Where(x => x.FolioPrefactura == Convert.ToInt64(Folio)).FirstOrDefault();

                pnlDetallePagos.Visible = (pagoSel != null);
                if (pagoSel != null)
                {
                    if(pagoSel.FolioPrefactura == 0)
                    {
                        lblDetNum.Text = "Detalle concepto Hotel";
                    }
                    else
                    {
                        lblDetNum.Text = "Detalle de la factura: " + Folio;
                    }
                    grvDetalle.DataSource = pagoSel.ListaDesglosePago;
                    grvDetalle.DataBind();
                }
            }
            catch (ExceptionViva ex)
            {
                MostrarDialogo("informacion", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "ConsultaDetalle");
                MostrarDialogo("error", mensajeUsuario);
            }
        }

        private void DescargarFactura(long folioFactura)
        {
            BLLFacturacion bllFact = new BLLFacturacion();
            string rutaArchivos = "";

            try
            {
                List<ENTPagosPorFacturar> pagos = new List<ENTPagosPorFacturar>();
                pagos = ListaPagos;

                ENTPagosPorFacturar pagoSel = new ENTPagosPorFacturar();
                pagoSel = pagos.Where(x => x.PaymentID == folioFactura).FirstOrDefault();

                if (pagoSel != null && pagoSel.RutaZip != null && pagoSel.RutaZip.Length > 0)
                {
                    rutaArchivos = pagoSel.RutaZip;
                }
                else
                {
                    rutaArchivos = bllFact.GeneraArchivosFactura(folioFactura);
                }

                if (Directory.Exists(rutaArchivos))
                {
                    RutaArchivoDescarga = rutaArchivos;

                    Session["Descargar"] = "SI";
                    Page.Response.Redirect(Page.Request.Url.ToString(), false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    throw new Exception("No se logro identificar el folio de factura: " + folioFactura.ToString());
                }

            }
            catch (ExceptionViva ex)
            {
                MostrarDialogo("informacion", ex.Message);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "DescargarFactura");
                MostrarDialogo("error", mensajeUsuario);
            }


        }

        //DHV INI 18-07-2019 Separar datos facturacion / datos pasajero
        private void DescargarZip(string rutaArchivos)
        {
            OpenNewWindow(HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, @"/DownloadFact.aspx"));
            /*DirectoryInfo carpeta = new DirectoryInfo(rutaArchivos);
            MemoryStream stream = new MemoryStream();

            if (carpeta.Exists)
            {
                string zipName = String.Format("CFDI_{0}.zip", carpeta.Name);
                Response.Clear();
                //Response.ClearContent();
                //Response.ClearHeaders();
                Response.ContentType = "application/zip";
                Response.AppendHeader("content-disposition", "attachment; filename=" + zipName);

                using (ZipFile zip = new ZipFile())
                {
                    foreach (FileInfo archivo in carpeta.GetFiles())
                    {
                        if (archivo.Extension.ToString() != "zip")
                            zip.AddEntry(archivo.Name, archivo.OpenRead());
                    }

                    foreach (DirectoryInfo subCarpeta in carpeta.GetDirectories())
                    {
                        foreach (FileInfo archivo in subCarpeta.GetFiles())
                        {
                            if (archivo.Extension.ToString() != "zip")
                                zip.AddEntry(archivo.Name, archivo.OpenRead());
                        }
                    }
                    zip.Save(carpeta +"\\"+ zipName);//.Save(Response.OutputStream);                  
                }

                Response.TransmitFile(carpeta + "\\" + zipName);
                Response.Flush();
                Response.End();
            }*/

        }
        //DHV FIN 18-07-2019 Separar datos facturacion / datos pasajero



        // DHV INI 18-08-2019 Consultar PNR por email de usuario
        private void ConsultarReservasPendFacturar()
        {

            try
            {
                List<String> listaPNRs = new List<String>();
                SqlConnection conn = new SqlConnection();
                DataTable dt = new DataTable();
                conn = BLLConfiguracion.Conexion;
                DALReservaCab dalReservaCab = new DALReservaCab(conn);
                listaPNRs = new List<String>();

                if (!Page.IsPostBack)
                {
                    listaPNRs = dalReservaCab.RecuperarReservaPendFacPorEmail(email);
                }
                else
                {
                    if (ListaPNRs == null || (ListaPNRs != null && ListaPNRs.Count < 1))
                        listaPNRs = dalReservaCab.RecuperarReservaPendFacPorEmail(email);
                }

                if (listaPNRs != null && listaPNRs.Count > 0)
                {
                    listaPNRs = (from pnr in listaPNRs select pnr).Distinct().OrderBy(r => r).ToList();
                    ListaPNRs = new List<ReservacionesFacturar>();
                }

                foreach (string PNR in listaPNRs)
                {
                    /*long bookingID;                    
                    int contPagos = 0;
                    int contPagosOTA = 0;

                    List<ENTPagosCab> listaPagos = new List<ENTPagosCab>();
                    
                    BLLReservaCab bllReservas = new BLLReservaCab();
                    BLLPagosCab bllPagos = new BLLPagosCab();
                    bookingID = bllReservas.RecuperarReservaCabRecordlocator(PNR)
                                            .FirstOrDefault()
                                            .BookingID;
                    listaPagos = bllPagos.RecuperarPagosCabBookingid(bookingID).ToList();
                    contPagos = listaPagos.Count;

                    foreach (ENTPagosCab pago in listaPagos)
                    {
                        BLLOtasCat bllOTAS = new BLLOtasCat();
                        string organizationCode;
                        organizationCode = pago.OrganizationCode;
                        if(organizationCode != null && organizationCode.Length > 0 
                            && bllOTAS.RecuperarOtasCatOrganizationcode(organizationCode).Where(o => o.Activo == true).Count() > 0)
                            contPagosOTA++;
                    }

                    if(contPagos > contPagosOTA)*/

                    ReservacionesFacturar res = new ReservacionesFacturar();
                    res.Facturar = true;
                    res.PNR = PNR;
                    ListaPNRs.Add(res);
                }

                dt.Columns.Add("RecordLocator", typeof(string));
                dt.Columns.Add("Facturar", typeof(bool));

                foreach (ReservacionesFacturar res in ListaPNRs)
                {
                    DataRow row = dt.NewRow();
                    row["RecordLocator"] = res.PNR;
                    row["Facturar"] = res.Facturar;
                    dt.Rows.Add(row);
                }



                if (dt.Rows.Count > 0)
                    btnFacturarPNRs.Visible = true;
                else
                    btnFacturarPNRs.Visible = false;

                grvReservaPendFac.DataSource = null;
                grvReservaPendFac.DataSource = dt;
                grvReservaPendFac.DataBind();

            }
            catch (ExceptionViva ex)
            {
                MostrarDialogo("informacion", ex.Message);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "ConsultarReservasPendFacturar");
                MostrarDialogo("error", mensajeUsuario);
            }
        }


        private void MostrarPagos_PorPNR(List<ENTPagosPorFacturar> pagos)
        {
            //DHV INI 09-07-2019 Corregir forma de pago
            foreach (ENTPagosPorFacturar pago in pagos)
            {
                String PNR = String.Empty;
                BLLReservaCab bllReservaCab = new BLLReservaCab();
                ENTReservaCab reservaCab = new ENTReservaCab();
                bllReservaCab.BookingID = pago.BookingID;
                PNR = bllReservaCab.RecuperarReservaCabBookingid(pago.BookingID)
                                    .FirstOrDefault()
                                    .RecordLocator;
                pago.PNR = PNR;

                // Determina sí el pago corresponde al OTA
                BLLOtasCat bllOtasCat = new BLLOtasCat();
                var numSocComEncon = bllOtasCat.RecuperarOtasCatOrganizationcode(pago.OrganizationCode)
                                    .ToList().Where(o => o.Activo == true).Count();
                //Sí el pago NO es de la OTA se podrá facturar
                pago.EsFacturable = numSocComEncon > 0 ? false : true;
                pago.Mensaje = pago.Mensaje != null && pago.Mensaje.Length > 0 ? pago.Mensaje : numSocComEncon > 0 ? "Éste pago lo realizó un tercero" : String.Empty;

                // corrige forma de pago según valor BIN
                String formaPagoSAT = String.Empty;
                String cveFormaPago = String.Empty;
                bool seCorrigio = false;
                BLLFormapagoCat bllFormaPagoSAT = new BLLFormapagoCat();



                var formasPagoSAT = bllFormaPagoSAT.RecuperarTodo();
                formaPagoSAT = formasPagoSAT.Where(f => f.PaymentMethodCode == pago.PaymentMethodCode)
                    .FirstOrDefault().CveFormaPagoSAT;

                if (String.IsNullOrEmpty(pago.UpdFormaPagModificadoPor))
                {
                    if (!String.IsNullOrEmpty(pago.PaymentMethodCode))
                    {
                        // Pagos con tarjeta de debito o crédito
                        if (formaPagoSAT == "04" || formaPagoSAT == "28")
                        {
                            if (pago.BinRange > 0)
                            {
                                BLLFacturacion bllFacturacion = new BLLFacturacion();
                                BLLBinCat bllBinCat = new BLLBinCat();

                                // Validar BIN
                                List<ENTBinCat> listTipo = new List<ENTBinCat>();
                                listTipo = bllBinCat.RecuperarBinCatPorLlavePrimaria(pago.BinRange);

                                string tipo = listTipo != null && listTipo.Count > 0 ? listTipo.FirstOrDefault().TIPO : "";



                                // ResponseBINRest response = bllFacturacion.InvocarBINRest(pago.BinRange.ToString());
                                // 04 -> TC  |   28 -> TD   |   01 -> Efectivo
                                //cveFormaPago = response.type != null ? response.type.ToLower() == "credit" ? "04" : "28" : "";
                                cveFormaPago = !String.IsNullOrEmpty(tipo) ? tipo.ToLower() == "credit" ? "04" : "28" : "";
                                // Corregir en automático el método de pago




                                if (!String.IsNullOrEmpty(cveFormaPago))
                                {
                                    pago.UpdFormaPagModificadoPor = Comun.Utils.Tipo.ClientePortal.Sistema;
                                    pago.FechaUpdaFormaPag = DateTime.Now;
                                    // 25	MC	Master Card
                                    // 84	VY	Cualquier Débito
                                    BLLFormapagoCat formasPago = new BLLFormapagoCat();

                                    var idFormaPago = formasPago.RecuperarTodo()
                                        .Where(f => f.PaymentMethodCode == (cveFormaPago == "04" ? "MC" : "VY"))
                                        .FirstOrDefault()
                                        .IdFormaPago;

                                    if (cveFormaPago != formaPagoSAT)
                                        pago.IdUpdFormaPago = idFormaPago;
                                    else
                                        pago.IdUpdFormaPago = pago.IdFormaPago;


                                    seCorrigio = bllFacturacion.ActualizarFormaPago(pago);
                                }
                            }
                        }
                    }
                }
            }
            //DHV FIN 09-07-2019  Corregir forma de pago


            //Guarda la informacion en Session
            //Session["Fact" + txtPNR.Text.Trim()] = Pagos;
            ListaPagos = pagos;

            //Muestra la informacion en el Grid de Pagos
            grvFacturas.DataSource = pagos;
            grvFacturas.DataBind();
            //divGrid.Visible = true;
            hdnPostback.Value = "true";


            //Analiza la información de los pagos para enviar aviso segun sea el caso
            int numPagosFac = pagos.Where(x => x.EsFacturable == true && x.EsFacturado == true).Count();
            int numPagosSinVigencia = pagos.Where(x => x.EsFacturable == true && x.EsFacturado == false && x.EnVigenciaParaFacturacion == false).Count();
            bool existenPagosPorFacturar = pagos.Where(x => x.EsFacturable == true && x.EsFacturado == false && x.EnVigenciaParaFacturacion == true).Count() > 0;
            bool existenPagosFacturables = pagos.Where(x => x.EsFacturable == true).Count() > 0;
            btnFacturar.Visible = existenPagosPorFacturar;
            btnCancelarFacturar.Visible = true;
            //pnlDatosFact.Visible = existenPagosPorFacturar;
            pnlPagos.Visible = existenPagosFacturables;

            string mensaje = "";
            string saltolinea = "";

            //En caso de no existir pagos pendientes por facturar enviara el siguiente mensaje de aviso
            if (!existenPagosPorFacturar)
            {
                mensaje = "No existen pagos pendientes por facturar.";
                saltolinea = " ";
            }

            //Valida la vigencia de los pagos pendientes por facturar
            if (numPagosSinVigencia > 0)
            {
                if (numPagosSinVigencia == 1)
                {
                    mensaje += string.Format("{0}Se tiene {1} pago donde ya se venció la fecha límite de facturación.", saltolinea, numPagosSinVigencia.ToString());
                }
                else
                {
                    mensaje += string.Format("{0}Se tienen {1} pagos donde ya se venció la fecha límite de facturación.", saltolinea, numPagosSinVigencia.ToString());
                }

                saltolinea = " ";
            }

            //Analiza la informacion de los pagos que ya se encuentran facturados
            if (numPagosFac > 0)
            {
                if (numPagosFac == 1)
                {
                    mensaje += string.Format("{0}Se tiene {1} pago facturado que puede recuperar dando click en la imagen de descarga.", saltolinea, numPagosFac.ToString());
                }
                else
                {
                    mensaje += string.Format("{0}Se tienen {1} pagos facturados que puede recuperar dando click en la imagen de descarga.", saltolinea, numPagosFac.ToString());
                }

                saltolinea = " ";
            }

            //En caso de que exista algun mensaje por mostrar al cliente se enviara por pantalla
            if (mensaje.Length > 0)
            {
                throw new ExceptionViva(mensaje);
            }


            txtApellidos.ReadOnly = true;
            txtNombre.ReadOnly = true;
            txtPNR.ReadOnly = true;
            btnBuscarReserva.Visible = false;
        }

        private void MostrarDatosFacturacion()
        {
            string hfValor = hfPuedeFacturar.Value;

            if (hfValor != String.Empty)
            {
                pnlDatosFact.Visible = true;
                pnlDatosFact.Focus();
                btnFacturar.Visible = false;
            }

            txtEMail.Text = email;
            txtEmailConf.Text = email;
            txtRFC.Focus();

            if (!String.IsNullOrEmpty(email))
                EmailConfirmacion.Visible = false;

        }
        // DHV FIN 22-08-2019 Consultar PNR por email de usuario
        #endregion

        #region "E V E N T O S   P O S T - B A C K"
        protected void btnBuscarReserva_Click(object sender, EventArgs e)
        {
            bool clienteComercial = false;
            //bool pagosPendCliente = false;
            string passwordOTA = String.Empty;

            VBFactPaquetes.PortalWeb.Controllers.HomeController x = new VBFactPaquetes.PortalWeb.Controllers.HomeController();
            string token = x.getAuthToken();
            Paquete paquete = new Paquete();
            //BLLDistribucionPagos bll = new BLLDistribucionPagos();

            try
            {
                if (Page.IsValid)
                {
                    if (txtPNR.Text.Length == 8)
                    {
                        paquete = JsonConvert.DeserializeObject<Paquete>(x.getBookingByID(token, txtPNR.Text));

                        if (paquete != null)
                        {
                            foreach (var pack in paquete.items)
                            {
                                if (pack.productType.productType == "Flight")
                                {
                                    txtPNR.Text = pack.productType.referenceId;
                                }
                            }
                        }
                    }
                    else
                    {
                        paquete = JsonConvert.DeserializeObject<Paquete>(x.getBookingByPNR(token, txtPNR.Text));
                    }

                    BLLFacturacion Factura = new BLLFacturacion();
                    //se valida el usuario
                    ENTDatosFacturacion objDatosFac = new ENTDatosFacturacion();
                    objDatosFac.ClaveReservacion = txtPNR.Text.Trim().ToUpper();
                    objDatosFac.NombrePasajero = txtNombre.Text.Trim().ToUpper();
                    objDatosFac.ApellidosPasajero = txtApellidos.Text.Trim().ToUpper();
                    PNR = objDatosFac.ClaveReservacion;

                    // DHV INI "Separar pagos de los cliente" 02.08.2019
                    clienteComercial = Factura.OcultarCaptchaPorPNR(PNR, ref passwordOTA);
                    // pagosPendCliente

                    // buscar pagos realizados por el cliente

                    // DHV FIN "Separar pagos de los cliente" 02.08.2019

                    if (Factura.ValidarNombrePasajero(ref objDatosFac))
                    {
                        //si es correcta la validación se inactivan campos y se carga el grid
                        txtNombre.Text = objDatosFac.NombrePasajero.ToUpper();
                        txtApellidos.Text = objDatosFac.ApellidosPasajero.ToUpper();
                        txtPNR.Text = txtPNR.Text.Trim().ToUpper();

                        ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();
                        datosCliente.NombrePasajero = txtNombre.Text.Trim().ToUpper();
                        datosCliente.ApellidosPasajero = txtApellidos.Text.Trim().ToUpper();
                        datosCliente.ClaveReservacion = txtPNR.Text.Trim();

                        //if (datosCliente.ClaveReservacion.Length == 6)
                        //{

                        //Recupera la informacion de los pagos que tiene registrados en Navitaire 
                        BLLFacturacion bllFacturacion = new BLLFacturacion();
                        List<ENTPagosPorFacturar> pagos = new List<ENTPagosPorFacturar>();
                        pagos = bllFacturacion.RecuperarPagosParaFacturar(datosCliente, paquete);

                        int pagosPendFactCliente = pagos.Where(p => p.EsFacturado == false).Count();
                        if (clienteComercial && pagosPendFactCliente >= 1)
                        {
                            foreach (var p in pagos)
                            {
                                // Determina sí el pago corresponde al OTA
                                BLLOtasCat bllOtasCat = new BLLOtasCat();
                                var numSocComEncon = bllOtasCat.RecuperarOtasCatOrganizationcode(p.OrganizationCode)
                                                    .ToList().Where(o => o.Activo == true).Count();
                                bool pagoPenEsOTA = numSocComEncon > 0 ? true : false;

                                if (p.EsFacturado == false && pagoPenEsOTA)
                                {
                                    throw new ExceptionViva("La reservación a facturar a sido adquirida mediante una agencia de viajes externa a VivaAerobus, favor de contactar a la misma para la adecuada emisión de su factura solicitada.");
                                }
                            }
                        }
                        MostrarPagos_PorPNR(pagos);
                    }
                    else
                    {
                        txtCaptcha.Text = "";
                        throw new ExceptionViva("Los datos del pasajero no corresponden a la reservación solicitada. Favor de verificar la información.");
                    }
                }
            }
            catch (ExceptionViva ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= txtCaptcha]').val(''); renderRecaptcha();", true);
                MostrarDialogo("informacion", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "btnBuscarReserva_Click");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= txtCaptcha]').val(''); renderRecaptcha();", true);
                MostrarDialogo("error", mensajeUsuario);
            }
        }



        protected void grvFacturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            long folioPago = 0;
            string formaPagoModPor = String.Empty;
            ENTPagosPorFacturar regPago;

            try
            {
                //aqui van los formatos y condiciones de cada column
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl_FactPNR = e.Row.FindControl("lblFactPNR") as Label;
                    lbl_FactPNR.Text = ((Facturacion.ENT.Portal.Facturacion.ENTPagosPorFacturar)e.Row.DataItem).PNR;
                    folioPago = ((Facturacion.ENT.ENTPagosCab)e.Row.DataItem).PaymentID;

                    // DHV INI 16-07-2019 Corrección Forma de Pago
                    BLLFormapagoCat bllFormasPago = new BLLFormapagoCat();
                    Label lbl_FormaPago = e.Row.FindControl("lblFormaPago") as Label;
                    String paymentMethodCode = string.Empty;
                    int IdFormaPago;

                    if (String.IsNullOrEmpty(((Facturacion.ENT.ENTPagosCab)e.Row.DataItem).UpdFormaPagModificadoPor))
                    {
                        paymentMethodCode = ((Facturacion.ENT.ENTPagosCab)e.Row.DataItem).PaymentMethodCode;
                        IdFormaPago = ((Facturacion.ENT.ENTPagosCab)e.Row.DataItem).IdFormaPago;
                    }
                    else
                    {
                        IdFormaPago = ((Facturacion.ENT.ENTPagosCab)e.Row.DataItem).IdUpdFormaPago;
                        var formaPago = bllFormasPago.RecuperarTodo().Where(f => f.IdFormaPago == IdFormaPago).FirstOrDefault();
                        paymentMethodCode = formaPago.PaymentMethodCode;
                    }

                    var formasPago = bllFormasPago.RecuperarTodo()
                        .Where(f => f.PaymentMethodCode == paymentMethodCode)
                        .FirstOrDefault();
                    var formaPagoSAT = formasPago.CveFormaPagoSAT;
                    BLLGendescripcionesCat bllGenDesc = new BLLGendescripcionesCat();
                    var descFormaPagoSAT = bllGenDesc.RecuperarGendescripcionesCatCvetabla("FRMPAG")
                                            .Where(f => f.CveValor == formaPagoSAT)
                                            .FirstOrDefault().Descripcion;

                    lbl_FormaPago.Text = descFormaPagoSAT;

                    Label lbl_IdPagosCab = e.Row.FindControl("lblIdPagosCab") as Label;
                    lbl_IdPagosCab.Text = ((Facturacion.ENT.ENTPagosCab)e.Row.DataItem).IdPagosCab.ToString();

                    BLLFormapagoCat bllFormaPago = new BLLFormapagoCat();
                    List<ENTFormapagoCat> listaFormasPago = new List<ENTFormapagoCat>();
                    formaPagoModPor = ((Facturacion.ENT.ENTPagosCab)e.Row.DataItem).UpdFormaPagModificadoPor;

                    listaFormasPago = bllFormaPago.RecuperarTodo();
                    DropDownList ddl_FormaPago = e.Row.FindControl("ddlFormaPago") as DropDownList;

                    ddl_FormaPago.DataValueField = "PaymentMethodCode";
                    ddl_FormaPago.DataTextField = "Descripcion";
                    ddl_FormaPago.DataSource = listaFormasPago;
                    ddl_FormaPago.DataBind();
                    ddl_FormaPago.SelectedIndex = ddl_FormaPago.Items.IndexOf(ddl_FormaPago.Items.FindByValue(paymentMethodCode));

                    string departmentCode = ((Facturacion.ENT.ENTPagosCab)e.Row.DataItem).DepartmentCode;

                    if (String.IsNullOrEmpty(formaPagoModPor)
                        && (formaPagoSAT == "04" || formaPagoSAT == "28")
                        && (departmentCode == "VBZH" || departmentCode == "VBZL" || departmentCode == "VBZA"))
                    {
                        ddl_FormaPago.Visible = true;
                        lbl_FormaPago.Visible = false;

                    }
                    else
                    {
                        ddl_FormaPago.Visible = false;
                        lbl_FormaPago.Visible = true;
                    }
                    // DHV FIN 16-07-2019 Corrección Forma de Pago


                    regPago = ListaPagos.Where(x => x.FolioPrefactura == folioPago).FirstOrDefault();

                    if (folioPago == 0 || regPago == null)
                    {
                        e.Row.Visible = true;
                    }

                    //Da formato a la fecha de facturación
                    if (regPago.FechaFactura == new DateTime())
                    {
                        e.Row.Cells[7].Text = "";
                    }
                    else
                    {
                        e.Row.Cells[7].Text = regPago.FechaFactura.ToString("dd/MM/yyyy");
                    }

                    LinkButton lkbReenviar = e.Row.Cells[1].Controls[1] as LinkButton;
                    LinkButton lkbDescargar = e.Row.Cells[2].Controls[1] as LinkButton;
                    if (regPago.EsFacturado)
                    {
                        lkbDescargar.Visible = true;
                        lkbReenviar.Visible = true;
                    }
                    else
                    {
                        lkbDescargar.Visible = false;
                        lkbReenviar.Visible = false;
                    }

                    bool esVigente = false;
                    string mensaje = "";

                    esVigente = regPago.EnVigenciaParaFacturacion;

                    mensaje = regPago.Mensaje != null ? regPago.Mensaje : "";

                    if (e.Row.Cells.Count > 0 && e.Row.Cells[0] != null && e.Row.Cells[0].Controls.Count > 0)
                    {
                        CheckBox chk = e.Row.FindControl("chkEsFacturado") as CheckBox;
                        Image img = e.Row.FindControl("imgError") as Image;
                        if (esVigente)
                        {
                            img.Visible = false;
                            if (regPago.EsFacturable == false)
                            {
                                bool deOTA = false;
                                BLLOtasCat bllOtasCat = new BLLOtasCat();
                                var numSocComEncon = bllOtasCat.RecuperarOtasCatOrganizationcode(regPago.OrganizationCode)
                                                    .ToList().Where(o => o.Activo == true).Count();
                                deOTA = numSocComEncon > 0 ? true : false;
                                if (deOTA)
                                {
                                    img.Visible = true;
                                    img.ToolTip = regPago.Mensaje;
                                    img.ImageUrl = Page.ResolveUrl("~/Contents/Images/img_dialogo_error.png");
                                    e.Row.Visible = false;
                                }

                                chk.Checked = false;
                                chk.Text = " ";//No Facturable";
                                chk.ForeColor = System.Drawing.Color.Transparent;
                                chk.Enabled = false;
                                chk.Visible = false;

                            }
                            else
                            {
                                if (regPago.FolioPrefactura == 0)
                                {
                                    chk.Checked = false;
                                    chk.Text = " ";//No Facturado";
                                    chk.ForeColor = System.Drawing.Color.Transparent;
                                    chk.Enabled = false;
                                    chk.Visible = true;
                                    btnFacturar.Visible = true;
                                }
                                else if (regPago.EsFacturado == false)
                                {
                                    chk.Checked = true;
                                    chk.Text = " ";//No Facturado";
                                    chk.ForeColor = System.Drawing.Color.Transparent;
                                    chk.Enabled = true;
                                    chk.Visible = true;
                                    btnFacturar.Visible = true;
                                }
                                else if (regPago.EsFacturado == true)
                                {
                                    chk.Checked = false;
                                    chk.Text = " ";//Facturado";
                                    chk.ForeColor = System.Drawing.Color.Transparent;
                                    chk.Enabled = false;
                                    chk.Visible = false;
                                }
                            }


                        }
                        else
                        {
                            //Cuando el pago se encuentra fuera de vigencia
                            img.Visible = true;
                            img.ToolTip = mensaje;

                            chk.Checked = false;
                            chk.Text = " ";//No Facturable";
                            chk.ForeColor = System.Drawing.Color.Transparent;
                            chk.Enabled = false;
                            chk.Visible = false;
                        }
                    }
                }

            }
            catch (ExceptionViva ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= rfvCaptcha]').show(); grecaptcha.render( $('#dvCaptcha')[0], { sitekey : '" + ReCaptcha_Key + "' });", true);
                MostrarDialogo("informacion", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "grvFaturas_RowDataBound");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= rfvCaptcha]').show(); grecaptcha.render( $('#dvCaptcha')[0], { sitekey : '" + ReCaptcha_Key + "' });", true);
                MostrarDialogo("error", mensajeUsuario);
            }
        }


        protected void grvFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long folioFactura;

            try
            {
                PNR = txtPNR.Text.Trim();
                switch (e.CommandName)
                {
                    case "Reimpresion":
                        folioFactura = long.Parse(e.CommandArgument.ToString());
                        DescargarFactura(folioFactura);
                        break;

                    case "Select":
                        ConsultaDetalle(e.CommandArgument.ToString());
                        var row = ((System.Web.UI.Control)e.CommandSource).NamingContainer as GridViewRow;
                        DropDownList ddl_FormaPago = (DropDownList)row.FindControl("ddlFormaPago");
                        ddl_FormaPago.Enabled = false;
                        break;

                    case "Reenviar":
                        List<ENTParametrosCnf> ListaParametros = new List<ENTParametrosCnf>();
                        BLLFacturacion bllFacturacion = new BLLFacturacion();
                        ENTEmpresaCat EmpresaEmisora;
                        BLLEmpresaCat bllCatEmpresa = new BLLEmpresaCat();
                        ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();
                        List<ENTPagosPorFacturar> listaPagosSolicitados = new List<ENTPagosPorFacturar>();
                        List<string> listaArchivosAtach = new List<string>();
                        BLLFacturasCab bllFactura = new BLLFacturasCab();
                        ENTFacturasCab factura = new ENTFacturasCab();
                        BLLReservaCab bllReserva = new BLLReservaCab();
                        ENTReservaCab reserva = new ENTReservaCab();
                        BLLPagosCab bllPagos = new BLLPagosCab();
                        List<ENTPagosCab> pagos = new List<ENTPagosCab>();

                        folioFactura = long.Parse(e.CommandArgument.ToString());
                        factura = bllFactura.RecuperarFacturasCabFoliofactura(folioFactura).FirstOrDefault();
                        reserva = bllReserva.RecuperarReservaCabBookingid(factura.BookingID).FirstOrDefault();

                        ListaParametros = bllFacturacion.RecuperarParametros();
                        byte idEmpresa = Convert.ToByte(ListaParametros.Where(x => x.Nombre == "IdEmpresa").FirstOrDefault().Valor);

                        EmpresaEmisora = bllCatEmpresa.RecuperarTodo().Where(x => x.IdEmpresa == idEmpresa).FirstOrDefault();
                        EnviarEmail enviarCorreo = new EnviarEmail(ListaParametros, EmpresaEmisora);

                        datosCliente.ClaveReservacion = reserva.RecordLocator;
                        datosCliente.EmailReceptor = factura.EmailReceptor;
                        datosCliente.RFCReceptor = factura.RfcReceptor;

                        pagos = bllPagos.RecuperarPagosCabIdfacturacab(factura.IdFacturaCab);

                        foreach (ENTPagosCab pago in pagos)
                        {
                            ENTPagosPorFacturar pagoFac = new ENTPagosPorFacturar();
                            pagoFac.BinRange = pago.BinRange;
                            pagoFac.BookingID = pago.BookingID;
                            pagoFac.CollectedAmount = pago.CollectedAmount;
                            pagoFac.CollectedCurrencyCode = pago.CollectedCurrencyCode;
                            pagoFac.CurrencyCode = pago.CurrencyCode;
                            pagoFac.DepartmentCode = pago.DepartmentCode;
                            pagoFac.DepartmentName = pago.DepartmentName;
                            pagoFac.EsFacturable = pago.EsFacturable;
                            pagoFac.EsFacturableGlobal = pago.EsFacturableGlobal;
                            pagoFac.EsFacturado = pago.EsFacturado;
                            pagoFac.EsPagoDividido = pago.EsPagoDividido;
                            pagoFac.EsPagoHijo = pago.EsPagoHijo;
                            pagoFac.EsPagoPadre = pago.EsPagoPadre;
                            pagoFac.EsParaAplicar = pago.EsParaAplicar;
                            pagoFac.Estatus = pago.Estatus;
                            pagoFac.FechaFactura = pago.FechaFactura;
                            pagoFac.FechaFacturaUTC = pago.FechaFacturaUTC;
                            pagoFac.FechaHoraLocal = pago.FechaHoraLocal;
                            pagoFac.FechaPago = pago.FechaPago;
                            pagoFac.FechaPagoUTC = pago.FechaPagoUTC;
                            pagoFac.FechaUltimaActualizacion = pago.FechaUltimaActualizacion;
                            pagoFac.FechaUpdaFormaPag = pago.FechaUpdaFormaPag;
                            pagoFac.FolioFactura = pago.FolioFactura;
                            pagoFac.FolioPrefactura = pago.FolioPrefactura;
                            pagoFac.IdAgente = pago.IdAgente;
                            pagoFac.IdFacturaCab = pago.IdFacturaCab;
                            pagoFac.IdFacturaGlobal = pago.IdFacturaGlobal;
                            pagoFac.IdFormaPago = pago.IdFormaPago;
                            pagoFac.IdNotaCreditoCab = pago.IdNotaCreditoCab;
                            pagoFac.IdPagosCab = pago.IdPagosCab;
                            pagoFac.IdUpdFormaPago = pago.IdUpdFormaPago;
                            pagoFac.LocationCode = pago.LocationCode;
                            pagoFac.LugarExpedicion = pago.LugarExpedicion;
                            pagoFac.Mensaje = "OK";
                            pagoFac.MontoIVA = pago.MontoIVA;
                            pagoFac.MontoOtrosCargos = pago.MontoOtrosCargos;
                            pagoFac.MontoPorAplicar = pago.MontoPorAplicar;
                            pagoFac.MontoServAdic = pago.MontoServAdic;
                            pagoFac.MontoTarifa = pago.MontoTarifa;
                            pagoFac.MontoTotal = pago.MontoTotal;
                            pagoFac.MontoTUA = pago.MontoTUA;
                            pagoFac.OrganizationCode = pago.OrganizationCode;
                            pagoFac.OrganizationName = pago.OrganizationName;
                            pagoFac.OrganizationType = pago.OrganizationType;
                            pagoFac.ParentPaymentID = pago.ParentPaymentID;
                            pagoFac.PaymentAmount = pago.PaymentAmount;
                            pagoFac.PaymentID = pago.PaymentID;
                            pagoFac.PaymentMethodCode = pago.PaymentMethodCode;
                            pagoFac.PNR = datosCliente.ClaveReservacion;

                            //Se genera el archivo CFDI
                            BLLPDFCFDI bllPdf = new Facturacion.BLL.ProcesoFacturacion.BLLPDFCFDI();
                            BLLXmlPegaso bllXML = new BLLXmlPegaso();
                            ENTXmlPegaso xml = new ENTXmlPegaso();
                            xml = bllXML.RecuperarXmlPegasoFoliocfdi(pagoFac.FolioFactura).FirstOrDefault();

                            string rutaArchivoCFDI = bllPdf.GeneraArchivoCFDI(xml.CFD_ComprobanteStr, folioFactura, datosCliente.ClaveReservacion);
                            string rutaArchivoPDF = bllPdf.GeneraPDFFactura33(xml.CFD_ComprobanteStr, xml.CFD_CadenaOriginal, datosCliente.ClaveReservacion, false);

                            pagoFac.RutaCFDI = rutaArchivoCFDI;
                            pagoFac.RutaPDF = rutaArchivoPDF;
                            //pagoFac.RutaZip
                            pagoFac.TipoCambio = pago.TipoCambio;
                            pagoFac.UpdFormaPagModificadoPor = pago.UpdFormaPagModificadoPor;
                            pagoFac.VersionFacturacion = pago.VersionFacturacion;

                            listaPagosSolicitados.Add(pagoFac);
                        }

                        //Se genera la lista de los archivos que se van a enviar
                        foreach (ENTPagosPorFacturar pago in listaPagosSolicitados)
                        {
                            if (pago.RutaCFDI != null && pago.RutaCFDI.Length > 0) listaArchivosAtach.Add(pago.RutaCFDI);
                            if (pago.RutaPDF != null && pago.RutaPDF.Length > 0) listaArchivosAtach.Add(pago.RutaPDF);
                        }

                        //datosCliente.EmailReceptor = "daniel.vargas@vivaaerobus.com";
                        String result = enviarCorreo.sendEmailFactura(datosCliente, EmpresaEmisora, listaPagosSolicitados, listaArchivosAtach);
                        Session["Reenviar"] = datosCliente.EmailReceptor;
                        Page.Response.Redirect(Page.Request.Url.ToString(), false);
                        Context.ApplicationInstance.CompleteRequest();
                        break;
                }

            }
            catch (ExceptionViva ex)
            {
                MostrarDialogo("informacion", ex.Message);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "grvFaturas_RowCommand");
                MostrarDialogo("error", mensajeUsuario);
            }

        }

        protected void grvFacturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                List<ENTPagosPorFacturar> listaPagos = new List<ENTPagosPorFacturar>();
                listaPagos = ListaPagos;
                if (listaPagos.Count > 0)
                {
                    grvFacturas.PageIndex = e.NewPageIndex;
                    grvFacturas.DataSource = listaPagos;
                    grvFacturas.DataBind();
                }
                else
                {
                    //lkbBuscar_Click(sender, e);
                }
            }
            catch (ExceptionViva ex)
            {
                MostrarDialogo("informacion", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "grvFaturas_PageIndexChanging");
                MostrarDialogo("informacion", mensajeUsuario);
            }
        }

        protected void grvDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[3].Text = "$" + decimal.Parse(e.Row.Cells[3].Text).ToString("#,###,##0.00");
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                }
            }
            catch (ExceptionViva ex)
            {
                MostrarDialogo("informacion", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "grvDetalle_RowDataBound");
                MostrarDialogo("informacion", mensajeUsuario);
            }
        }

        protected void lblConsultar_Click(object sender, EventArgs e)
        {
            pnlDetallePagos.Visible = true;
        }


        protected void btnFacturar_Click(object sender, EventArgs e)
        {
            MostrarDatosFacturacion();
        }

        // DHV INI 10-07-2019 Separar Datos Pasajero
        protected void btnCerrarDetallesPago_Click(object sender, EventArgs e)
        {
            pnlDetallePagos.Visible = false;


            foreach (GridViewRow row in grvFacturas.Rows)
            {

                if (row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList combo = row.FindControl("ddlFormaPago") as DropDownList;
                    combo.Enabled = true;
                }

            }
        }

        protected void btnCerrarDatosFactura_Click(object sender, EventArgs e)
        {
            pnlDatosFact.Visible = false;
            btnFacturar.Visible = true;
            btnCancelarFacturar.Visible = true;

            txtRFC.Text = String.Empty;
            txtEMail.Text = String.Empty;
            txtEmailConf.Text = String.Empty;
        }



        private bool validarRFC(string RFCval)
        {


            RegexStringValidator rsv = new RegexStringValidator("[A-Z&Ñ]{3,4}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[A-Z0-9]{2}[0-9A]");
            try
            {
                rsv.Validate(RFCval);
                return true;
            }
            catch
            {
                return false;
            }
        }



        protected void btnGenerarFactura_Click(object sender, EventArgs e)
        {
            int facturasSolicitadas = 0;
            int facturasGeneradas = 0;
            int facturasSinEnvio = 0;
            string RFC_Extranjero = "XEXX010101000";
            bool okok;

            //RegexStringValidator expReg = new RegexStringValidator("[A - Z & Ñ]{ 3,4}[0-9](0[1-9]|1[012])(0[1-9]|[12] [0-9]|3[01])[A-Z0-9]{2}[0-9A]");
            //tring expReg = "[A - Z & Ñ]{ 3,4}[0-9](0[1-9]|1[012])(0[1-9]|[12] [0-9]|3[01])[A-Z0-9]{2}[0-9A]";
            //RegexStringValidator valExpReg = new RegexStringValidator(expReg);
            //RFCVal = txtRFC.Text;
            //Regex valExpReg = new Regex(expReg);
            //Se recupera la lista de pagos que resulto de la busqueda
            List<ENTPagosPorFacturar> listaPagos = new List<ENTPagosPorFacturar>();
            // if (Regex.IsMatch(txtRFC.Text, expReg))
            //   okok = true;

            okok = validarRFC(txtRFC.Text.ToUpper());
            try
            {
                if (String.IsNullOrEmpty(txtRFC.Text))
                    throw new ExceptionViva("Debes ingresa RFC");
                if (String.IsNullOrEmpty(txtEMail.Text))
                    throw new ExceptionViva("Debes ingresa Email");
                if (!okok)
                    throw new ExceptionViva("El RFC está mal capturado, favor de verificar");
                if (txtRFC.Text.ToUpper().Trim() == RFC_Extranjero && String.IsNullOrEmpty(txtTaxID.Text))
                    throw new ExceptionViva("Debes ingresa TAX");

                GridViewRowCollection items = grvFacturas.Rows;
                StringBuilder strMessage = new StringBuilder();
                //listaPagos = ListaPagos;

                //Se genera una lista de los pagos que se envian a facturar
                foreach (GridViewRow item in items)
                {
                    if (item.Cells.Count > 0 && item.Cells[0] != null && item.Cells[0].Controls.Count > 0)
                    {

                        CheckBox chk = item.Cells[0].Controls[1] as CheckBox;
                        long folioPrefactura = 0;
                        long.TryParse(item.Cells[6].Text.ToString(), out folioPrefactura);

                        ENTPagosPorFacturar pago = new ENTPagosPorFacturar();
                        pago = ListaPagos.Where(x => x.FolioPrefactura == folioPrefactura).FirstOrDefault();
                        pago.PNR = (item.FindControl("lblFactPNR") as Label).Text;
                        if (pago != null) { pago.EstaMarcadoParaFacturacion = chk.Checked; }
                        listaPagos.Add(pago);
                    }
                }

                //Se actualiza la lista de pagos que fueron seleccionados por el cliente
                ListaPagos = listaPagos;

                //List<ENTPagosPorFacturar> ListaPagosPorEnviar = ListaPagos;
                facturasSolicitadas = listaPagos.Where(x => x.EstaMarcadoParaFacturacion).Count();

                //Verifica si existen pagos seleccionados
                if (facturasSolicitadas == 0)
                {
                    //ShowFalla("Sin selección de Pagos", "No existen pagos seleccionados, favor de seleccionar alguno marcando la casilla en la columna Facturar...");
                    MostrarDialogo("informacion", "No existen pagos seleccionados, favor de seleccionar alguno marcando la casilla en la columna Facturar...");
                }
                else
                {
                    //Se recupera la informacion capturada por el cliente
                    ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();
                    txtRFC.Text = txtRFC.Text.Replace(" ", "");
                    txtRFC.Text = txtRFC.Text.Replace("-", "");
                    txtRFC.Text = txtRFC.Text.Trim().ToUpper();
                    datosCliente.RFCReceptor = txtRFC.Text;
                    datosCliente.NombrePasajero = txtNombre.Text.Trim().ToUpper();
                    datosCliente.ApellidosPasajero = txtApellidos.Text.Trim().ToUpper();
                    datosCliente.ClaveReservacion = txtPNR.Text.Trim();
                    datosCliente.EmailReceptor = txtEMail.Text.Trim();
                    datosCliente.EsExtranjero = txtRFC.Text.ToUpper() == RFC_Extranjero ? true : false;
                    datosCliente.UsoCFDI = ddlUsoCFDI.SelectedValue;

                    if (datosCliente.EsExtranjero == false)
                    {
                        datosCliente.PaisResidenciaFiscal = "";
                        datosCliente.TAXID = "";
                    }
                    else
                    {
                        datosCliente.PaisResidenciaFiscal = ddlPaisReferencia.SelectedValue;
                        datosCliente.TAXID = txtTaxID.Text.Trim();
                    }

                    BLLFacturacion bllFacturacion = new BLLFacturacion();
                    List<ENTPagosPorFacturar> listaPagosFacturados = new List<ENTPagosPorFacturar>();

                    VBFactPaquetes.PortalWeb.Controllers.HomeController HController = new VBFactPaquetes.PortalWeb.Controllers.HomeController();
                    string token = HController.getAuthToken();
                    Paquete paquete = new Paquete();
                    paquete = JsonConvert.DeserializeObject<Paquete>(HController.getBookingByPNR(token, datosCliente.ClaveReservacion));

                    listaPagosFacturados = bllFacturacion.GenerarFacturaCliente(datosCliente, listaPagos, paquete);
                    listaPagos = listaPagosFacturados;
                    ListaPagos = listaPagos;
                    grvFacturas.DataSource = listaPagos;
                    grvFacturas.DataBind();

                    hdnPostback.Value = "true";

                    facturasGeneradas = listaPagos.Where(x => x.Mensaje == "OK" && x.EstaMarcadoParaFacturacion == true).Count();
                    facturasSinEnvio = listaPagos.Where(x => x.Mensaje == "SinEnvio").Count();

                    if (facturasSolicitadas == 0)
                    {
                        MostrarDialogo("informacion", "No existen pagos seleccionados, favor de seleccionar alguno marcando la casilla en la columna Facturar...");
                    }
                    else if (facturasGeneradas == 0 && facturasSinEnvio == 0)
                    {
                        MostrarDialogo("informacion", "No fue posible generar las facturas seleccionadas, por favor intente mas tarde...");
                    }
                    else if (facturasSinEnvio > 0)
                    {
                        MostrarDialogo("informacion", "No fue posible enviarlas por Correo, por favor intente descargarlas");
                    }
                    else if (facturasSolicitadas > facturasGeneradas)
                    {
                        MostrarDialogo("informacion", "Sus facturas fueron enviadas a " + txtEMail.Text.Trim());
                        //hfFacturo.Value = txtEMail.Text.Trim();
                    }
                    else if (facturasSolicitadas == facturasGeneradas)
                    {
                        MostrarDialogo("informacion", "Sus facturas fueron enviadas a " + txtEMail.Text.Trim());
                    }

                    //Verifica si existen pagos que aun se pueden facturar
                    bool existenPagosPorFacturar = listaPagos.Where(x => x.EsFacturable == true && x.EsFacturado == false).Count() > 0;
                    btnGenerarFactura.Visible = existenPagosPorFacturar;

                    //Inicia el proceso de descarga de los archivos Zip
                    if (listaPagos.Where(x => x.RutaCFDI != null && x.RutaCFDI != "").Count() > 0)
                    {
                        try
                        {
                            string rutaArchivos = "";

                            ENTPagosPorFacturar pagoFacturado = listaPagos.Where(x => x.RutaCFDI != null && x.RutaCFDI != "").FirstOrDefault();
                            rutaArchivos = pagoFacturado.RutaCFDI;

                            FileInfo archivoCfdi = new FileInfo(rutaArchivos);
                            string PathFact = archivoCfdi.Directory.Parent.FullName.ToString();
                            RutaArchivoDescarga = PathFact;
                            //DescargarZip(PathFact);
                            //GeneraZipDescarga(PathFact);
                            //OpenNewWindow(HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, @"/DownloadFact.aspx"));
                            Session["Facturo"] = txtEMail.Text.Trim();
                            Page.Response.Redirect(Page.Request.Url.ToString(), false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        catch (Exception ex)
                        {
                            if (ex.HResult == -2146233040 || !ex.Message.Contains("Thread was being aborted") && ex.Message.Length > 0)
                            {
                                BllLogErrores.RegistrarError(PNR, "", ex, "Portal", "btnGenerarFactura_Redirect");
                                MostrarDialogo("error", "No fue posible generar la facturación");
                            }
                            else
                                MostrarDialogo("error", ex.Message);
                        }
                    }
                }
            }
            catch (ExceptionViva ex)
            {

                MostrarDialogo("informacion", ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233040 || !ex.Message.Contains("Thread was being aborted") || !ex.Message.Contains("Subproceso anulado."))
                {
                    string mensajeUsuario = MensajeErrorUsuario;
                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "btnGenerarFactura");
                    //ShowFalla("", mensajeUsuario);
                    MostrarDialogo("informacion", MensajeErrorUsuario);
                }
                else
                    MostrarDialogo("error", ex.Message);
            }

            btnGenerarFactura.Visible = true;
        }
        // DHV FIN 10-07-2019 Separar Datos Pasajero

        // DHV INI 16-07-2019 Corregir forma de pago 
        protected void ddlFormaPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddl_FormaPago = (DropDownList)sender;
                GridViewRow row = (GridViewRow)ddl_FormaPago.NamingContainer;
                Label lblPrice = (Label)row.FindControl("lblIdPagosCab");

                BLLPagosCab bllPago = new BLLPagosCab();
                BLLFormapagoCat formasPago = new BLLFormapagoCat();
                BLLFacturacion bllFacturacion = new BLLFacturacion();

                foreach (ENTPagosPorFacturar pago in ListaPagos)
                {
                    if (pago.IdPagosCab.ToString() == lblPrice.Text)
                    {
                        pago.UpdFormaPagModificadoPor = Comun.Utils.Tipo.ClientePortal.Cliente;
                        pago.FechaUpdaFormaPag = DateTime.Now;
                        var idFormaPago = formasPago.RecuperarTodo()
                                                    .Where(f => f.PaymentMethodCode == ddl_FormaPago.SelectedItem.Value)
                                                    .FirstOrDefault()
                                                    .IdFormaPago;
                        pago.IdUpdFormaPago = idFormaPago;

                        bool seCorrige = bllFacturacion.ActualizarFormaPago(pago);
                    }
                }

                ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();
                datosCliente.NombrePasajero = txtNombre.Text.Trim().ToUpper();
                datosCliente.ApellidosPasajero = txtApellidos.Text.Trim().ToUpper();
                datosCliente.ClaveReservacion = txtPNR.Text.Trim();

                //ListaPagos = bllFacturacion.RecuperarPagosParaFacturar(datosCliente);
                ListaPagos = bllFacturacion.RecuperarPagosParaFacturar(datosCliente, null);

            }
            catch (Exception ex)
            {
                BllLogErrores.RegistrarError(PNR, MensajeErrorUsuario, ex, "Portal", "ddlFormaPago_SelectedIndexChanged");
            }
        }
        // DHV FIN 16-07-2019 Corregir forma de pago

        // DHV INI 18-08-2019 Consultar PNR por email de usuario
        protected void grvReservaPendFac_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbl_PNR = e.Row.FindControl("lblPNR") as Label;
                    CheckBox chk = e.Row.FindControl("chkPorFacturar") as CheckBox;

                    lbl_PNR.Text = ((DataRowView)e.Row.DataItem).Row.ItemArray[0].ToString(); //.ToString(); ((ReservacionesFacturar)e.Row.DataItem).PNR;
                    chk.Checked = bool.Parse(((DataRowView)e.Row.DataItem).Row.ItemArray[1].ToString());
                    chk.Visible = true;
                }
            }
            catch (ExceptionViva ex)
            {
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= rfvCaptcha]').show(); grecaptcha.render( $('#dvCaptcha')[0], { sitekey : '" + ReCaptcha_Key + "' });", true);
                MostrarDialogo("informacion", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "grvReservaPendFac_RowDataBound");

                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= rfvCaptcha]').show(); grecaptcha.render( $('#dvCaptcha')[0], { sitekey : '" + ReCaptcha_Key + "' });", true);
                MostrarDialogo("error", mensajeUsuario);
            }
        }

        protected void grvReservaPendFac_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dt = new DataTable();

            try
            {

                if (ListaPNRs.Count > 0)
                {
                    //listaPNRs = (from pnr in listaPNRs select pnr).Distinct().ToList();

                    dt.Columns.Add("RecordLocator", typeof(string));
                    dt.Columns.Add("Facturar", typeof(string));
                    foreach (ReservacionesFacturar res in ListaPNRs)
                    {
                        DataRow row = dt.NewRow();
                        row["RecordLocator"] = res.PNR;
                        row["Facturar"] = res.Facturar;
                        dt.Rows.Add(row);
                    }

                    grvReservaPendFac.PageIndex = e.NewPageIndex;
                    grvReservaPendFac.DataSource = dt;
                    grvReservaPendFac.DataBind();

                    pnlPagos.Visible = false;
                    pnlDatosFact.Visible = false;
                }
                else
                {
                    //lkbBuscar_Click(sender, e);
                }
            }
            catch (ExceptionViva ex)
            {
                MostrarDialogo("informacion", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "grvFaturas_PageIndexChanging");
                MostrarDialogo("informacion", mensajeUsuario);
            }
        }

        protected void grvReservaPendFac_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "PNR":
                        txtPNR.Text = e.CommandArgument.ToString().Trim();

                        ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();
                        datosCliente.NombrePasajero = txtNombre.Text.Trim().ToUpper();
                        datosCliente.ApellidosPasajero = txtApellidos.Text.Trim().ToUpper();
                        datosCliente.ClaveReservacion = e.CommandArgument.ToString().Trim();

                        //Recupera la informacion de los pagos que tiene registrados en Navitaire 
                        BLLFacturacion bllFacturacion = new BLLFacturacion();
                        List<ENTPagosPorFacturar> pagos = new List<ENTPagosPorFacturar>();
                        //pagos = bllFacturacion.RecuperarPagosParaFacturar(datosCliente);
                        pagos = bllFacturacion.RecuperarPagosParaFacturar(datosCliente, null);
                        pnlPagosHeader.InnerText = "Pagos Facturables de la reservación: " + e.CommandArgument.ToString().Trim();
                        MostrarPagos_PorPNR(pagos);
                        btnFacturarPNRs.Visible = false;
                        return;

                    case "Delete":
                        return;
                    default:
                        return;
                }
            }
            catch (ExceptionViva ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= rfvCaptcha]').show(); grecaptcha.render( $('#dvCaptcha')[0], { sitekey : '" + ReCaptcha_Key + "' });", true);
                MostrarDialogo("informacion", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "lblPNR_Click");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= rfvCaptcha]').show(); grecaptcha.render( $('#dvCaptcha')[0], { sitekey : '" + ReCaptcha_Key + "' });", true);
                MostrarDialogo("error", mensajeUsuario);
            }

        }

        protected void btnFacturarPNRs_Click(object sender, EventArgs e)
        {
            try
            {
                ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();
                BLLFacturacion bllFacturacion = new BLLFacturacion();
                List<ENTPagosPorFacturar> pagos = new List<ENTPagosPorFacturar>();
                String pnr = String.Empty;
                int totReser = 0;

                foreach (GridViewRow dr in grvReservaPendFac.Rows)
                {
                    CheckBox chk = (CheckBox)dr.Cells[0].FindControl("chkPorFacturar");
                    pnr = ((Label)dr.Cells[0].FindControl("lblPNR")).Text.Trim();
                    ListaPNRs.First(p => p.PNR == pnr).Facturar = chk.Checked;

                    if (chk.Checked)
                    {
                        datosCliente.NombrePasajero = String.Empty;
                        datosCliente.ApellidosPasajero = String.Empty;
                        datosCliente.ClaveReservacion = pnr;

                        //Recupera la informacion de los pagos que tiene registrados en Navitaire                 
                        //pagos.AddRange(bllFacturacion.RecuperarPagosParaFacturar(datosCliente));
                        pagos.AddRange(bllFacturacion.RecuperarPagosParaFacturar(datosCliente, null));
                        totReser++;
                    }

                }

                MostrarPagos_PorPNR(pagos);
                pnlDatosFact.Visible = true;
                MostrarDatosFacturacion();

                if (!String.IsNullOrEmpty(email))
                {
                    btnCancelarFacturar.Visible = false;
                    btnFacturar.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MostrarDialogo("error", ex.Message);
            }
        }

        protected void btnCancelarFacturar_Click(object sender, EventArgs e)
        {
            btnFacturarPNRs.Visible = true;
            pnlPagos.Visible = false;
            pnlDetallePagos.Visible = false;
            pnlDatosFact.Visible = false;
            btnBuscarReserva.Visible = true;
            pnlCaptcha.Visible = true;

            txtApellidos.ReadOnly = false;
            txtNombre.ReadOnly = false;
            txtPNR.ReadOnly = false;
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= txtCaptcha]').val(''); renderRecaptcha();", true);
        }

        protected void chkPorFacturar_CheckedChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    CheckBox chk = (CheckBox)sender;
            //    String PNR = ((Label)(chk).Parent.FindControl("lblPNR")).Text;

            //    ListaPNRs.First(p => p.PNR == PNR).Facturar = chk.Checked;
            //}
            //catch (Exception ex)
            //{
            //    MostrarDialogo("error", ex.Message);
            //}

        }

        protected void lblReenviar_Click(object sender, EventArgs e)
        {
            try
            {
                var closeLink = (Control)sender;
                GridViewRow row = (GridViewRow)closeLink.NamingContainer;
                string pnr = row.Cells[6].Text;
            }
            catch (Exception ex)
            {
                MostrarDialogo("error", ex.Message);
            }
        }

        protected void btnSoyExtranjero_Click(object sender, EventArgs e)
        {
            if (fueExtranjero == false)
            {
                fueExtranjero = true;
                txtRFC.Text = "XEXX010101000";
                txtRFC.Enabled = false;
                dvFacExtranjero.Visible = true;
                btnSoyExtranjero.Text = "Soy Nacional";
            }
            else
            {
                fueExtranjero = false;
                txtRFC.Text = String.Empty;
                txtRFC.Enabled = true;
                dvFacExtranjero.Visible = false;
                btnSoyExtranjero.Text = "Soy Extranjero";
            }

        }

        protected void txtRFC_TextChanged(object sender, EventArgs e)
        {
            if (txtRFC.Text.ToUpper() == "XEXX010101000")
            {
                fueExtranjero = true;
                txtRFC.Enabled = false;
                dvFacExtranjero.Visible = true;
                btnSoyExtranjero.Text = "Soy Nacional";
            }
            else
            {
                fueExtranjero = false;
                txtRFC.Enabled = true;
                dvFacExtranjero.Visible = false;
                btnSoyExtranjero.Text = "Soy Extranjero";
            }
        }


        // DHV FIN 22-08-2019 Consultar PNR por email de usuario

        #endregion

        //[WebMethod]
        //public static string OcultaCaptcha(string response)
        //{

        //    bool ocultarCaptcha = false;
        //    string passwordOTA = "";
        //    BLLFacturacion bllFac = new BLLFacturacion();
        //    ocultarCaptcha = bllFac.OcultarCaptchaPorPNR(response, ref passwordOTA);
        //    string result = (ocultarCaptcha ? "true" : "false");
        //    return result;
        //}

        //protected void rdbExtranjero_CheckedChanged(object sender, EventArgs e)
        //{
        //    pnlTaxId.Visible = rdbExtranjero.Checked;
        //    pnlPaisRes.Visible = rdbExtranjero.Checked;
        //    if (rdbExtranjero.Checked)
        //    {
        //        txtRFC.Text = "XEXX010101000";
        //        txtRFC.Enabled = false;
        //    }
        //    else
        //    {
        //        txtRFC.Text = "";
        //        txtRFC.Enabled = true;
        //    }

        //}

        //protected void rdbNacional_CheckedChanged(object sender, EventArgs e)
        //{
        //    pnlTaxId.Visible = !rdbNacional.Checked;
        //    pnlPaisRes.Visible = !rdbNacional.Checked;
        //    if (rdbNacional.Checked)
        //    {
        //        txtRFC.Text = "";
        //        txtRFC.Enabled = true;
        //    }
        //    else
        //    {
        //        txtRFC.Text = "XEXX010101000";
        //        txtRFC.Enabled = false;
        //    }
        //}

    }
}
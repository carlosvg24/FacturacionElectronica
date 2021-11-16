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
using System.Net.Mail;
using Comun.Email;
using Facturacion.DAL.ProcesoFacturacion;

namespace FacturacionOnLine
{
    public partial class A : System.Web.UI.Page
    {
        protected static string ReCaptcha_Key = "6Lcd3zIUAAAAAIf53dpsBtDYLzxKZtdOerXljq8p";
        protected static string ReCaptcha_Secret = "6Lcd3zIUAAAAAGxrXc1p7hkpVizTdci48K0Ex4K2";
        Exportar exp = new Exportar();
        private BLLBitacoraErrores BllLogErrores = new BLLBitacoraErrores();
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


        public string PasswordOTA
        {
            get
            {
                if (Session["PasswordOTA"] == null)
                {
                    return "";
                }
                else
                {
                    return Session["PasswordOTA"].ToString();
                }
            }

            set
            {
                Session["PasswordOTA"] = value;
            }
        }

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




        public ENTDatosFacturacion DatosCliente
        {
            get
            {
                if (Session["DatosCliente"] == null)
                {
                    Session["DatosCliente"] = new List<ENTPagosPorFacturar>();
                }
                return (ENTDatosFacturacion)Session["DatosCliente"];
            }

            set
            {
                Session["DatosCliente"] = value;
            }
        }





        [WebMethod]
        public static string VerifyCaptcha(string response)
        {
            string url = "https://www.google.com/recaptcha/api/siteverify?secret=" + ReCaptcha_Secret + "&response=" + response;
            return (new WebClient()).DownloadString(url);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //btnRegistrarse.Text = "Registrarse en la nueva " + System.Environment.NewLine + " experiencia de facturación";

                if (!Page.IsPostBack)
                {


                    //Carga del catalogo de Usos del CFDI publicado por el SAT
                    BLLGendescripcionesCat bllDescCat = new BLLGendescripcionesCat();
                    List<ENTGendescripcionesCat> listDescCat = new List<ENTGendescripcionesCat>();
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
                    LimpiarPantalla();

                    //String mensaje = "<b>Pronto estrenaremos nuevo portal de facturación</b>";
                    //mensaje += "<br><br>En el nuevo portal será necesario estar dado de alta";
                    //mensaje += "<br><br>¿Quisieras darte de alta ahora?";
                    //MostrarDialogo("pregunta", mensaje);
                }
            }
            catch (ExceptionViva ex)
            {
                ShowFalla("Portal de Facturación", ex.Message);
            }
            catch (Exception ex)
            {

                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "Page_Load");
                ShowFalla("Portal de Facturación", mensajeUsuario);
            }
        }


        /// <summary>
        /// Inicializa la pantalla para capturar una nueva reservacion
        /// </summary>
        private void LimpiarPantalla()
        {
            //Limpiando Variables de Session
            PNR = "";
            PasswordOTA = "";
            RutaArchivoDescarga = "";
            ListaPagos = new List<ENTPagosPorFacturar>();


            //Limpiando textbox de la pantalla de captura
            txtApellidos.Text = "";
            txtCaptcha.Text = "";
            txtEMail.Text = "";
            txtEmailConf.Text = "";
            txtNombre.Text = "";
            txtPasswordOTA.Text = "";
            txtPNR.Text = "";
            txtRFC.Text = "";
            txtTaxID.Text = "";

            ddlUsoCFDI.SelectedValue = "P01";
            ddlPaisReferencia.ClearSelection();

            txtApellidos.ReadOnly = false;
            txtNombre.ReadOnly = false;
            txtEMail.ReadOnly = false;
            txtEmailConf.ReadOnly = false;
            txtPNR.ReadOnly = false;
            txtRFC.ReadOnly = false;
            txtTaxID.ReadOnly = false;
            ddlPaisReferencia.Enabled = true;
            ddlUsoCFDI.Enabled = true;
            chkSoyExtranjero.Enabled = true;
            chkSoyExtranjero.Checked = false;
            hdnPostback.Value = "";
            lblDetNum.Text = "";
            bool ocultarCaptcha = false;
            bool.TryParse(ConfigurationManager.AppSettings["OcultarCaptcha"], out ocultarCaptcha);
            pnlCaptcha.Visible = !ocultarCaptcha;

        }

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


        //Realiza la busqueda de la reservacion
        protected void lkbBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                if (chkSoyExtranjero.Checked == true)
                {
                    if (txtRFC.Text.Trim().ToUpper() != "XEXX010101000")
                    {
                        throw new ExceptionViva("El RFC debe ser el genérico para extranjeros, favor de verificar...");
                    }
                    else if (txtTaxID.Text.Trim().Length == 0)
                    {
                        throw new ExceptionViva("El RFC capturado es para extranjeros, favor de indicar el TAX ID correspondiente...");
                    }
                }
                else
                {
                    if (txtRFC.Text.Trim().ToUpper().Length == 0)
                    {
                        throw new ExceptionViva("El RFC es un dato requerido, favor de capturarlo.");
                    }
                    if (txtRFC.Text.Trim().ToUpper() == "XEXX010101000")
                    {
                        chkSoyExtranjero.Checked = true;
                        throw new ExceptionViva("El RFC capturado es para extranjeros, favor de validar que la información sea correcta.");
                    }
                }



                //LCI INI. 01-02-2018 VALIDA CONTRASEÑA SOCIO COMERCIAL PARA OMITIR CAPTCHA
                bool requierePassOTA = false;
                string passwordOTA = "";

                //Verifica si el PNR pertenece a un Socio Comercial que utiliza Procesos automatizados para facturar
                BLLFacturacion bllFac = new BLLFacturacion();
                requierePassOTA = bllFac.OcultarCaptchaPorPNR(txtPNR.Text.Trim().ToUpper(), ref passwordOTA);
                hdnOcultarCaptcha.Value = (requierePassOTA ? "true" : "false");
                rfvCaptcha.Enabled = !requierePassOTA;
                rfvPassOTA.Enabled = requierePassOTA;

                if (requierePassOTA && txtPasswordOTA.Text != passwordOTA)
                {
                    pnlCaptcha.Visible = false;
                    pnlPassOTA.Visible = true;
                    throw new ExceptionViva("El password indicado no es válido, verifique...");

                }
                //LCI INI. 01-02-2018

                if (Page.IsValid)
                {
                    BLLFacturacion Factura = new BLLFacturacion();
                    //se valida el usuario
                    ENTDatosFacturacion objDatosFac = new ENTDatosFacturacion();
                    objDatosFac.ClaveReservacion = txtPNR.Text.Trim().ToUpper();
                    objDatosFac.NombrePasajero = txtNombre.Text.Trim().ToUpper();
                    objDatosFac.ApellidosPasajero = txtApellidos.Text.Trim().ToUpper();
                    PNR = objDatosFac.ClaveReservacion;
                    if (Factura.ValidarNombrePasajero(ref objDatosFac))
                    {
                        //si es correcta la validación se inactivan campos y se carga el grid

                        txtNombre.Text = objDatosFac.NombrePasajero.ToUpper();
                        txtApellidos.Text = objDatosFac.ApellidosPasajero.ToUpper();
                        txtPNR.Text = txtPNR.Text.Trim().ToUpper();

                        txtApellidos.ReadOnly = true;
                        txtNombre.ReadOnly = true;
                        txtEMail.ReadOnly = true;
                        txtEmailConf.ReadOnly = true;
                        txtPNR.ReadOnly = true;
                        txtRFC.ReadOnly = true;
                        txtTaxID.ReadOnly = true;
                        ddlPaisReferencia.Enabled = false;
                        ddlUsoCFDI.Enabled = false;
                        chkSoyExtranjero.Enabled = false;

                        lkbBuscar.Visible = false;
                        divCaptcha.Visible = false;
                        lblDetNum.Text = "";

                        ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();
                        txtRFC.Text = txtRFC.Text.Replace(" ", "");
                        txtRFC.Text = txtRFC.Text.Replace("-", "");
                        datosCliente.RFCReceptor = txtRFC.Text.Trim();
                        datosCliente.NombrePasajero = txtNombre.Text.Trim().ToUpper();
                        datosCliente.ApellidosPasajero = txtApellidos.Text.Trim().ToUpper();
                        datosCliente.ClaveReservacion = txtPNR.Text.Trim();
                        datosCliente.EmailReceptor = txtEMail.Text.Trim();
                        datosCliente.EsExtranjero = chkSoyExtranjero.Checked;
                        datosCliente.UsoCFDI = ddlUsoCFDI.SelectedValue;

                        if (chkSoyExtranjero.Checked == false)
                        {
                            datosCliente.PaisResidenciaFiscal = "";
                            datosCliente.TAXID = "";
                        }
                        else
                        {
                            datosCliente.PaisResidenciaFiscal = ddlPaisReferencia.SelectedValue;
                            datosCliente.TAXID = txtTaxID.Text.Trim();
                        }

                        DatosCliente = datosCliente;

                        //Recupera la informacion de los pagos que tiene registrados en Navitaire 
                        BLLFacturacion bllFacturacion = new BLLFacturacion();
                        List<ENTPagosPorFacturar> Pagos = new List<ENTPagosPorFacturar>();
                        Pagos = bllFacturacion.RecuperarPagosParaFacturar(datosCliente, null);

                        //Guarda la informacion en Session
                        //Session["Fact" + txtPNR.Text.Trim()] = Pagos;
                        ListaPagos = Pagos;

                        //Muestra la informacion en el Grid de Pagos
                        grvFaturas.DataSource = Pagos;
                        grvFaturas.DataBind();
                        divGrid.Visible = true;
                        hdnPostback.Value = "true";



                        //Analiza la información de los pagos para enviar aviso segun sea el caso
                        int numPagosFac = Pagos.Where(x => x.EsFacturable == true && x.EsFacturado == true).Count();
                        int numPagosSinVigencia = Pagos.Where(x => x.EsFacturable == true && x.EsFacturado == false && x.EnVigenciaParaFacturacion == false).Count();
                        bool existenPagosPorFacturar = Pagos.Where(x => x.EsFacturable == true && x.EsFacturado == false && x.EnVigenciaParaFacturacion == true).Count() > 0;
                        lkbFacturar.Visible = existenPagosPorFacturar;

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


                        //LCI. INI. 20190213 BLOQUEO IVA FRONTERIZO
                        if (Pagos.Where(x => x.ConBloqueoFacturacion == true).Count() > 0)
                        {
                            //En caso de que algun pago se encuentre bloqueado por contener un iva fronterizo no permitido se emite mensaje
                            ENTPagosPorFacturar entPagoBloqueo = new ENTPagosPorFacturar();
                            entPagoBloqueo = Pagos.Where(x => x.ConBloqueoFacturacion == true).FirstOrDefault();
                            if (entPagoBloqueo != null)
                            {
                                mensaje = entPagoBloqueo.Mensaje;
                            }
                        }

                        //LCI. FIN. 20190213 BLOQUEO IVA FRONTERIZO


                        //En caso de que exista algun mensaje por mostrar al cliente se enviara por pantalla
                        if (mensaje.Length > 0)
                        {
                            throw new ExceptionViva(mensaje);
                        }

                    }
                    else
                    {
                        txtCaptcha.Text = "";//se borra el contenido para que solicite la validación del captcha nuevamente
                        throw new ExceptionViva("Los datos del pasajero no corresponden a la reservación solicitada. Favor de verificar la información.");
                    }
                }
            }
            catch (ExceptionViva ex)
            {
                ShowFalla("", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "lkbBuscar_Click");
                ShowFalla("", mensajeUsuario);
            }
        }
        protected void grvFaturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //aqui van los formatos y condiciones de cada column
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.Cells[2].Text == "0")
                    {
                        e.Row.Visible = false;
                    }

                    string fechaFacturacion = e.Row.Cells[3].Text;
                    DateTime fechaGen = new DateTime();
                    if (DateTime.TryParse(fechaFacturacion, out fechaGen))
                    {
                        if (fechaGen == new DateTime())
                        {
                            e.Row.Cells[3].Text = "";
                        }
                        else
                        {
                            e.Row.Cells[3].Text = fechaGen.ToString();
                        }
                    }
                    else
                    {
                        e.Row.Cells[3].Text = "";
                    }


                    e.Row.Cells[4].Text = "$" + decimal.Parse(e.Row.Cells[4].Text).ToString("#,###,##0.00");
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;

                    if (e.Row.Cells.Count > 0 && e.Row.Cells[6] != null && e.Row.Cells[6].Controls.Count > 0)
                    {
                        LinkButton lkb = e.Row.Cells[6].Controls[1] as LinkButton;
                        LinkButton lkbEmail = e.Row.Cells[7].Controls[1] as LinkButton;
                        if (e.Row.Cells[8].Text.ToLower() == "true")
                        {
                            lkb.Visible = true;
                            lkbEmail.Visible = true;
                        }
                        else
                        {
                            lkb.Visible = false;
                            lkbEmail.Visible = false;
                        }
                    }

                    bool esVigente = false;
                    string mensaje = "";

                    esVigente = (e.Row.Cells[11].Text.ToLower() == "true");
                    mensaje = e.Row.Cells[12].Text.ToLower();




                    if (e.Row.Cells.Count > 0 && e.Row.Cells[0] != null && e.Row.Cells[0].Controls.Count > 0)
                    {
                        CheckBox chk = e.Row.Cells[0].Controls[1] as CheckBox;
                        Image img = e.Row.Cells[0].Controls[3] as Image;
                        if (esVigente)
                        {
                            img.Visible = false;
                            if (chk.Checked == false && e.Row.Cells[8].Text.ToLower() == "false" && e.Row.Cells[9].Text.ToLower() == "true")
                            {
                                chk.Checked = true;
                                chk.Text = " ";//No Facturado";
                                chk.ForeColor = System.Drawing.Color.Transparent;
                                chk.Enabled = true;
                                chk.Visible = true;
                            }
                            else if (chk.Checked == false && e.Row.Cells[8].Text.ToLower() == "true" && e.Row.Cells[9].Text.ToLower() == "true")
                            {
                                chk.Checked = false;
                                chk.Text = " ";//Facturado";
                                chk.ForeColor = System.Drawing.Color.Transparent;
                                chk.Enabled = false;
                                chk.Visible = false;
                            }
                            else
                            {
                                chk.Checked = false;
                                chk.Text = " ";//No Facturable";
                                chk.ForeColor = System.Drawing.Color.Transparent;
                                chk.Enabled = false;
                                chk.Visible = false;
                            }
                        }
                        else
                        {
                            img.Visible = true;
                            img.ToolTip = mensaje;

                            chk.Checked = false;
                            chk.Text = " ";//No Facturable";
                            chk.ForeColor = System.Drawing.Color.Transparent;
                            chk.Enabled = false;
                            chk.Visible = false;
                        }
                    }

                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[11].Visible = false;
                    e.Row.Cells[12].Visible = false;
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[8].Visible = false;
                    e.Row.Cells[9].Visible = false;
                    e.Row.Cells[11].Visible = false;
                    e.Row.Cells[12].Visible = false;
                }
            }
            catch (ExceptionViva ex)
            {
                ShowFalla("", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "grvFaturas_RowDataBound");
                ShowFalla("", mensajeUsuario);
            }
        }
        protected void grvFaturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            try
            {

                PNR = txtPNR.Text.Trim();
                if (e.CommandName == "Reimpresion")
                {

                    long folioFactura = long.Parse(e.CommandArgument.ToString());
                    DescargarFactura(folioFactura);


                }
                else if (e.CommandName == "EnviarEmail")
                {

                    long folioFactura = long.Parse(e.CommandArgument.ToString());
                    ReenviarPorCorreo(folioFactura);


                }

                else if (e.CommandName == "Select")
                {
                    ConsultaDetalle(e.CommandArgument.ToString());
                }
            }
            catch (ExceptionViva ex)
            {
                ShowFalla("", ex.Message);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "grvFaturas_RowCommand");
                ShowFalla("", mensajeUsuario);
            }

        }

        private void ReenviarPorCorreo(long folioFactura)
        {
            BLLFacturacion bllFact = new BLLFacturacion();
            string rutaArchivos = "";

            try
            {
                List<ENTPagosPorFacturar> pagos = new List<ENTPagosPorFacturar>();

                ENTPagosPorFacturar pagoSel = new ENTPagosPorFacturar();
                pagoSel = ListaPagos.Where(x => x.PaymentID == folioFactura).FirstOrDefault();
                pagoSel.EstaMarcadoParaFacturacion = true;

                rutaArchivos = bllFact.GeneraArchivosFactura(ref pagoSel);

                pagos.Add(pagoSel);
                ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();
                datosCliente = DatosCliente;

                if (Directory.Exists(rutaArchivos))
                {
                    try
                    {
                        bllFact.EnviarCFDIPorCorreo(pagos, datosCliente);

                        Show("Factura enviada correctamente.", "Su factura fue enviada por correo a la cuenta " + datosCliente.EmailReceptor);
                    }
                    catch (Exception ex)
                    {

                        ShowFalla("Error en el envío.", "Por el momento no es posible enviar su factura por correo, favor de intentar mas tarde...");
                    }

                }
                else
                {
                    throw new Exception("No se logro identificar el folio de factura: " + folioFactura.ToString());
                }

            }
            catch (ExceptionViva ex)
            {
                ShowFalla("", ex.Message);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "EnviarFacturaEmail");
                ShowFalla("", mensajeUsuario);
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

                    OpenNewWindow(HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, @"/DownloadFact.aspx"));

                    Show("Facturas generadas correctamente.", "Sus facturas fueron descargadas.");
                }
                else
                {
                    throw new Exception("No se logro identificar el folio de factura: " + folioFactura.ToString());
                }

            }
            catch (ExceptionViva ex)
            {
                ShowFalla("", ex.Message);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "DescargarFactura");
                ShowFalla("", mensajeUsuario);
            }


        }


        protected void grvFaturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                List<ENTPagosPorFacturar> listaPagos = new List<ENTPagosPorFacturar>();
                listaPagos = ListaPagos;
                if (listaPagos.Count > 0)
                {
                    grvFaturas.PageIndex = e.NewPageIndex;
                    grvFaturas.DataSource = listaPagos;
                    grvFaturas.DataBind();
                }
                else
                {
                    lkbBuscar_Click(sender, e);
                }
            }
            catch (ExceptionViva ex)
            {
                ShowFalla("", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "grvFaturas_PageIndexChanging");
                ShowFalla("", mensajeUsuario);
            }
        }
        private void ConsultaDetalle(string Folio)
        {
            try
            {
                List<ENTPagosPorFacturar> listaPagos = new List<ENTPagosPorFacturar>();
                listaPagos = ListaPagos;

                if (listaPagos.Count > 0)
                {
                    List<ENTPagosPorFacturar> DetFind = listaPagos;

                    lblDetNum.Text = "";

                    foreach (ENTPagosPorFacturar obj in DetFind)
                    {
                        if (obj.FolioPrefactura.ToString() == Folio)
                        {
                            lblDetNum.Text = "Detalle de la factura: " + Folio;

                            grvDetalle.DataSource = obj.ListaDesglosePago;
                            grvDetalle.DataBind();
                            lkbCerrar.Visible = true;
                            break;
                        }
                    }
                }
            }
            catch (ExceptionViva ex)
            {
                ShowFalla("", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "ConsultaDetalle");
                ShowFalla("", mensajeUsuario);
            }
        }
        protected void lkbCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                lblDetNum.Text = "";
                grvDetalle.DataSource = null;
                grvDetalle.DataBind();
                lkbCerrar.Visible = false;
            }
            catch (ExceptionViva ex)
            {
                ShowFalla("", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "lkbCerrar_Click");
                ShowFalla("", mensajeUsuario);
            }
        }
        protected void lkbCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarPantalla();
                Response.Redirect("Default.aspx", true);
            }
            catch (ExceptionViva ex)
            {
                ShowFalla("", ex.Message);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("Thread was being aborted") && !ex.Message.Contains("Subproceso anulado."))
                {
                    //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                    string mensajeUsuario = MensajeErrorUsuario;
                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "lkbCancelar_Click");
                    ShowFalla("", mensajeUsuario);
                }
            }
        }
        protected void lkbFacturar_Click(object sender, EventArgs e)
        {
            int facturasSolicitadas = 0;
            int facturasGeneradas = 0;
            int facturasSinEnvio = 0;

            //Se recupera la lista de pagos que resulto de la busqueda
            List<ENTPagosPorFacturar> listaPagos = new List<ENTPagosPorFacturar>();

            try
            {
                GridViewRowCollection items = grvFaturas.Rows;
                StringBuilder strMessage = new StringBuilder();

                listaPagos = ListaPagos;

                //Se genera una lista de los pagos que se envian a facturar

                foreach (GridViewRow item in items)
                {
                    if (item.Cells.Count > 0 && item.Cells[0] != null && item.Cells[0].Controls.Count > 0)
                    {
                        CheckBox chk = item.Cells[0].Controls[1] as CheckBox;
                        long folioPrefactura = 0;
                        long.TryParse(item.Cells[2].Text.ToString(), out folioPrefactura);

                        ENTPagosPorFacturar pago = new ENTPagosPorFacturar();
                        pago = listaPagos.Where(x => x.FolioPrefactura == folioPrefactura).FirstOrDefault();
                        if (pago != null)
                        {
                            pago.EstaMarcadoParaFacturacion = chk.Checked;
                        }
                    }
                }

                //Se actualiza la lista de pagos que fueron seleccionados por el cliente
                ListaPagos = listaPagos;



                //List<ENTPagosPorFacturar> ListaPagosPorEnviar = ListaPagos;
                facturasSolicitadas = listaPagos.Where(x => x.EstaMarcadoParaFacturacion).Count();

                //Verifica si existen pagos seleccionados
                if (facturasSolicitadas == 0)
                {
                    ShowFalla("Sin selección de Pagos", "No existen pagos seleccionados, favor de seleccionar alguno marcando la casilla en la columna Facturar...");
                }
                else
                {

                    //Se recupera la informacion capturada por el cliente
                    ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();
                    datosCliente.RFCReceptor = txtRFC.Text.Trim();
                    datosCliente.NombrePasajero = txtNombre.Text.Trim().ToUpper();
                    datosCliente.ApellidosPasajero = txtApellidos.Text.Trim().ToUpper();
                    datosCliente.ClaveReservacion = txtPNR.Text.Trim();
                    datosCliente.EmailReceptor = txtEMail.Text.Trim();
                    datosCliente.EsExtranjero = chkSoyExtranjero.Checked;
                    datosCliente.UsoCFDI = ddlUsoCFDI.SelectedValue;

                    if (chkSoyExtranjero.Checked == false)
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

                    listaPagosFacturados = bllFacturacion.GenerarFacturaCliente(datosCliente, listaPagos, null);
                    listaPagos = listaPagosFacturados;
                    ListaPagos = listaPagos;
                    grvFaturas.DataSource = listaPagos;
                    grvFaturas.DataBind();

                    hdnPostback.Value = "true";


                    facturasGeneradas = listaPagos.Where(x => x.Mensaje == "OK" && x.EstaMarcadoParaFacturacion == true).Count();
                    facturasSinEnvio = listaPagos.Where(x => x.Mensaje == "SinEnvio").Count();

                    if (facturasSolicitadas == 0)
                    {
                        ShowFalla("Sin selección de Pagos", "No existen pagos seleccionados, favor de seleccionar alguno marcando la casilla en la columna Facturar...");
                    }
                    else if (facturasGeneradas == 0 && facturasSinEnvio == 0)
                    {
                        ShowFalla("No se generaron las Facturas", "No fue posible generar las facturas seleccionadas, por favor intente mas tarde...");
                    }
                    else if (facturasSinEnvio > 0)
                    {
                        Show("Facturas generadas correctamente.", "No fue posible enviarlas por Correo, por favor intente descargarlas");
                    }
                    else if (facturasSolicitadas > facturasGeneradas)
                    {
                        Show("Facturas generadas correctamente. Algunas facturas presentaron errores", "Sus facturas fueron enviadas a " + txtEMail.Text.Trim());
                    }
                    else if (facturasSolicitadas == facturasGeneradas)
                    {
                        Show("Facturas generadas correctamente.", "Sus facturas fueron enviadas a " + txtEMail.Text.Trim());
                    }


                    //Verifica si existen pagos que aun se pueden facturar
                    bool existenPagosPorFacturar = listaPagos.Where(x => x.EsFacturable == true && x.EsFacturado == false).Count() > 0;
                    lkbFacturar.Visible = existenPagosPorFacturar;

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
                            OpenNewWindow(HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, @"/DownloadFact.aspx"));

                        }
                        catch (Exception ex)
                        {
                            if (!ex.Message.Contains("Thread was being aborted") && ex.Message.Length > 0)
                            {
                                BllLogErrores.RegistrarError(PNR, "", ex, "Portal", "GenerarZipFacturas");
                            }
                        }
                    }
                }
            }
            catch (ExceptionViva ex)
            {
                if (ex.Message.Length > 0)
                {
                    ShowFalla("", ex.Message);
                }

            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("Thread was being aborted") || !ex.Message.Contains("Subproceso anulado."))
                {
                    string mensajeUsuario = MensajeErrorUsuario;
                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "lkbFacturar_Click");
                    ShowFalla("", mensajeUsuario);
                }
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
                ShowFalla("", ex.Message);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "grvDetalle_RowDataBound");
                ShowFalla("", mensajeUsuario);
            }
        }
        public static void Show(string title, string message)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            string script = string.Format("title: '{0}', text:'{1}',icon: 'success'", title.Replace("'", "\'"), message.Replace("'", "\'"));
            script = "swal({" + script + "});";
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
            }
        }

        public static void ShowFalla(string title, string message)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            string script = string.Format("title: '{0}', text:'{1}',icon: 'warning'", title.Replace("'", "\'"), message.Replace("'", "\'"));
            script = "swal({" + script + "});";
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
            }
        }


        void OpenNewWindow(string url)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
        }



        [WebMethod]
        public static string OcultaCaptcha(string response)
        {

            bool ocultarCaptcha = false;
            string passwordOTA = "";
            BLLFacturacion bllFac = new BLLFacturacion();
            ocultarCaptcha = bllFac.OcultarCaptchaPorPNR(response, ref passwordOTA);
            string result = (ocultarCaptcha ? "true" : "false");
            return result;
        }

        protected void btnAceptarDialogo_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(), "Mostrar_Capa('dvRegistrar'); $get('txt_dialogo').innerHTML = '';", true);  // 
            btnCancelarDialogo.Text = "CANCELAR";
            btnGuardar.Visible = true;
            btnAceptarDialogo.Visible = false;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            bool datosValidos = true;
            string mensaje = String.Empty;

            try
            {
                if (!String.IsNullOrEmpty(txtEmailAlta.Text) && !String.IsNullOrEmpty(txtConfirmarEmailAlta.Text))
                {
                    object email1 = new StringBuilder(txtEmailAlta.Text).ToString();
                    object email2 = new StringBuilder(txtConfirmarEmailAlta.Text).ToString();

                    if (!email1.Equals(email2))
                    {
                        mensaje = "El e-mail de confirmación no coincide";
                        datosValidos = false;
                    }
                }
                else
                {
                    mensaje = "El e-mail de confirmación no coincide";
                    datosValidos = false;
                }

                if (!String.IsNullOrEmpty(txtBxContrasenia.Text) && !String.IsNullOrEmpty(txtBxConfirmarContrasenia.Text))
                {
                    object pass1 = new StringBuilder(txtBxContrasenia.Text).ToString();
                    object pass2 = new StringBuilder(txtBxConfirmarContrasenia.Text).ToString();

                    if (!pass1.Equals(pass2))
                    {
                        mensaje += (mensaje.Length > 0 ? "<br>" : "") + "La contraseña no coincide";
                        datosValidos = false;
                    }
                }
                else
                {
                    mensaje += (mensaje.Length > 0 ? "<br>" : "") + "La contraseña no coincide";
                    datosValidos = false;
                }

                if (mensaje.Length > 0)
                    ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(),
                        "Mostrar_Capa('dvRegistrar'); $('#txt_dialogo').addClass('error'); $get('txt_dialogo').innerHTML = '" + mensaje + "';", true);

                if (datosValidos)
                {
                    Comun.Security.Encrypt enc = new Comun.Security.Encrypt();
                    Guid codigoVerificacion = Guid.NewGuid();
                    String email = txtConfirmarEmailAlta.Text;


                    BLLClientesportalCat clientes = new BLLClientesportalCat();
                    BLLClientestipoportalCat tipoCliente = new BLLClientestipoportalCat();
                    List<ENTClientestipoportalCat> listaTiposCliente = new List<ENTClientestipoportalCat>();

                    clientes.UsuarioActivo = true;
                    clientes.UsuarioVerificado = false;
                    clientes.CodigoVerificacion = codigoVerificacion;
                    clientes.Contrasenia = enc.EncryptKey(txtBxConfirmarContrasenia.Text);
                    clientes.Email = email;
                    clientes.Nombre = String.Empty;
                    clientes.Pais = "MXN";
                    clientes.RFC = String.Empty;
                    clientes.TAXID = String.Empty;
                    clientes.UsoCFDI = String.Empty;                    
                    listaTiposCliente = tipoCliente.RecuperarClientestipoportalCatNombre(Comun.Utils.Tipo.ClientePortal.Cliente);
                    clientes.ClienteTipoId = listaTiposCliente.FirstOrDefault().Id;
                    clientes.Agregar();

                    BLLFacturacion bllFac = new BLLFacturacion();
                    bllFac.EnviarCorreoConfirmaAltaUsuario(email, codigoVerificacion);

                    ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(), "$('#txt_dialogo').removeClass('error');", true);
                    btnGuardar.Visible = false;
                    btnCancelarDialogo.Text = "ACEPTAR";
                    MostrarDialogo("informacion", "Usuario Creado exitosamente<br><br>Revisa tu e-mail: <b>"+ email + "</b> y sigue las instrucciones de activación de usuario");
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(),
                        "Mostrar_Capa('dvRegistrar');", true);
            }
            catch (ExceptionViva ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(), "$('#txt_dialogo').removeClass('error'); $('#dv_dialogo').modal('hide');", true);
                //ShowFalla("", ex.Message);
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(), "$get('txt_dialogo').innerHTML = '" + ex.Message + "';", true);
                btnGuardar.Visible = false;
                
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(), "$('#txt_dialogo').removeClass('error'); $('#dv_dialogo').modal('hide');", true);
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(), "$get('txt_dialogo').innerHTML = '" + ex.Message + "';", true);
                btnGuardar.Visible = false;
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "AgregarClientePortal");
                //ShowFalla("", mensajeUsuario);
            }
        }
         
        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AdminUsuariosPortal/Registrarse.aspx");
        }
    }
}
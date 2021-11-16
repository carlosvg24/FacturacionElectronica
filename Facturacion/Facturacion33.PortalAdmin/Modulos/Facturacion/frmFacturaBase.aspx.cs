using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturacion.BLL;
using Facturacion.ENT;
using Facturacion33.PortalAdmin.Models.ENT;
using Facturacion.BLL.ProcesoFacturacion;
using System.IO;
using Facturacion.ENT.Portal.Facturacion;

namespace Facturacion33.PortalAdmin.Modulos.Facturacion
{
    public partial class frmFacturaBase : System.Web.UI.Page
    {
        #region Propiedades
        private BLLBitacoraErrores BllLogErrores = new BLLBitacoraErrores();
        public ENTFiltrosConsulta FiltrosBusqueda
        {
            get
            {
                if (ViewState["FiltrosBusqueda"] != null)
                {
                    return (ENTFiltrosConsulta)ViewState["FiltrosBusqueda"];
                }
                else
                {
                    return new ENTFiltrosConsulta();
                }
            }
            set
            {
                ViewState["FiltrosBusqueda"] = value;
            }
        }

        public List<ENTReservaPPD> ListaReservas
        {
            get
            {
                if (ViewState["ListaReservas"] != null)
                {
                    return (List<ENTReservaPPD>)ViewState["ListaReservas"];
                }
                else
                {
                    return new List<ENTReservaPPD>();
                }
            }
            set
            {
                ViewState["ListaReservas"] = value;
            }
        }
        public List<ENTCatalogoDDL> CatalogoEstatusPago
        {
            get
            {
                if (ViewState["CatalogoEstatusPago"] != null)
                {
                    return (List<ENTCatalogoDDL>)ViewState["CatalogoEstatusPago"];
                }
                else
                {
                    return new List<ENTCatalogoDDL>();
                }
            }
            set
            {
                ViewState["CatalogoEstatusPago"] = value;
            }
        }

        public List<ENTCatalogoDDL> CatalogoEstatusFacturacion
        {
            get
            {
                if (ViewState["CatalogoEstatusFacturacion"] != null)
                {
                    return (List<ENTCatalogoDDL>)ViewState["CatalogoEstatusFacturacion"];
                }
                else
                {
                    return new List<ENTCatalogoDDL>();
                }
            }
            set
            {
                ViewState["CatalogoEstatusFacturacion"] = value;
            }
        }



        public List<ENTCatalogoDDL> CatalogoOrgPPD
        {
            get
            {
                if (ViewState["CatalogoOrgPPD"] != null)
                {
                    return (List<ENTCatalogoDDL>)ViewState["CatalogoOrgPPD"];
                }
                else
                {
                    return new List<ENTCatalogoDDL>();
                }
            }
            set
            {
                ViewState["CatalogoOrgPPD"] = value;
            }
        }




        public List<ENTCatalogoDDL> CatalogoPaisesRes
        {
            get
            {
                if (ViewState["CatalogoPaisesRes"] != null)
                {
                    return (List<ENTCatalogoDDL>)ViewState["CatalogoPaisesRes"];
                }
                else
                {
                    return new List<ENTCatalogoDDL>();
                }
            }
            set
            {
                ViewState["CatalogoPaisesRes"] = value;
            }
        }

        //public List<ENTCatalogoQueue> CatalogoQueue
        //{
        //    get
        //    {
        //        if (ViewState["CatalogoQueue"] != null)
        //        {
        //            return (List<ENTCatalogoQueue>)ViewState["CatalogoQueue"];
        //        }
        //        else
        //        {
        //            return new List<ENTCatalogoQueue>();
        //        }
        //    }
        //    set
        //    {
        //        ViewState["CatalogoQueue"] = value;
        //    }
        //}
        public List<ENTPaginas> ListaPaginas
        {
            get
            {
                if (ViewState["ListaPaginas"] != null)
                {
                    return (List<ENTPaginas>)ViewState["ListaPaginas"];
                }
                else
                {
                    return new List<ENTPaginas>();
                }
            }
            set
            {
                ViewState["ListaPaginas"] = value;
            }
        }
        public int PnrPorPagina
        {
            get
            {
                if (ViewState["PnrPorPagina"] != null)
                {
                    return (int)ViewState["PnrPorPagina"];
                }
                else
                {
                    return 200;
                }
            }
            set
            {
                ViewState["PnrPorPagina"] = value;
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
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {


                    //Cargar catalogos iniciales
                    BLLOrganppdCat bllOrgPPD = new BLLOrganppdCat();
                    List<ENTOrganppdCat> listaOrgPPD = new List<ENTOrganppdCat>();
                    CatalogoOrgPPD = new List<ENTCatalogoDDL>();

                    CatalogoOrgPPD.Add(new ENTCatalogoDDL() { Codigo = "", Descripcion = "" });

                    listaOrgPPD = bllOrgPPD.RecuperarTodo();
                    foreach (ENTOrganppdCat org in listaOrgPPD)
                    {
                        ENTCatalogoDDL itemDdl = new Models.ENT.ENTCatalogoDDL();
                        CatalogoOrgPPD.Add(new ENTCatalogoDDL() { Codigo = org.OrganizationCode, Descripcion = org.OrganizationName });
                    }


                    ddlOrganization.DataSource = CatalogoOrgPPD;
                    ddlOrganization.DataValueField = "Codigo";
                    ddlOrganization.DataTextField = "Descripcion";
                    ddlOrganization.DataBind();

                    //Recuperar Catalogos
                    BLLComplementoPago bllCompl = new BLLComplementoPago();
                    var catEstPago = bllCompl.ListaGenDescripciones.Where(x => x.CveTabla == "STAPAG").ToList();
                    CatalogoEstatusPago = new List<ENTCatalogoDDL>();

                    CatalogoEstatusPago.Add(new ENTCatalogoDDL() { Codigo = "", Descripcion = "" });
                    foreach (ENTGendescripcionesCat catPag in catEstPago)
                    {
                        ENTCatalogoDDL itemDdl = new Models.ENT.ENTCatalogoDDL();
                        CatalogoEstatusPago.Add(new ENTCatalogoDDL() { Codigo = catPag.CveValor, Descripcion = catPag.Descripcion });
                    }

                    ddlEstatusPago.DataSource = CatalogoEstatusPago;
                    ddlEstatusPago.DataValueField = "Codigo";
                    ddlEstatusPago.DataTextField = "Descripcion";
                    ddlEstatusPago.DataBind();

                    var catEstFactura = bllCompl.ListaGenDescripciones.Where(x => x.CveTabla == "STAFAC").ToList();
                    CatalogoEstatusFacturacion = new List<ENTCatalogoDDL>();

                    CatalogoEstatusFacturacion.Add(new ENTCatalogoDDL() { Codigo = "", Descripcion = "" });
                    foreach (ENTGendescripcionesCat catFac in catEstFactura)
                    {
                        ENTCatalogoDDL itemDdl = new Models.ENT.ENTCatalogoDDL();
                        CatalogoEstatusFacturacion.Add(new ENTCatalogoDDL() { Codigo = catFac.CveValor, Descripcion = catFac.Descripcion });
                    }

                    ddlEstatusFactura.DataSource = CatalogoEstatusFacturacion;
                    ddlEstatusFactura.DataValueField = "Codigo";
                    ddlEstatusFactura.DataTextField = "Descripcion";
                    ddlEstatusFactura.DataBind();


                    var catPaisesRes = bllCompl.ListaGenDescripciones.Where(x => x.CveTabla == "PAISES").ToList();
                    CatalogoPaisesRes = new List<ENTCatalogoDDL>();

                    CatalogoPaisesRes.Add(new ENTCatalogoDDL() { Codigo = "", Descripcion = "" });
                    foreach (ENTGendescripcionesCat catPais in catPaisesRes)
                    {
                        ENTCatalogoDDL itemDdl = new Models.ENT.ENTCatalogoDDL();
                        CatalogoPaisesRes.Add(new ENTCatalogoDDL() { Codigo = catPais.CveValor, Descripcion = catPais.Descripcion });
                    }

                    ddlResidencia.DataSource = CatalogoPaisesRes;
                    ddlResidencia.DataValueField = "Codigo";
                    ddlResidencia.DataTextField = "Descripcion";
                    ddlResidencia.DataBind();



                    ListaReservas = new List<ENTReservaPPD>();
                    gvListaReservaciones.DataSource = new List<ENTReservaPPD>();
                    gvListaReservaciones.DataBind();

                    int filasPorPagina = 0;
                    int.TryParse(System.Configuration.ConfigurationManager.AppSettings["FilasPorPagina"], out filasPorPagina);
                    PnrPorPagina = filasPorPagina > 0 ? filasPorPagina : 200;
                }
                catch (Exception ex)
                {
                    btnExportarAExcel.Enabled = false;
                    hfErrorBusqueda.Value = ex.Message;
                    EnviarErrores(ex.Message);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                ENTFiltrosConsulta filtrosBusqueda = new ENTFiltrosConsulta();
                BLLComplementoPago bllFacPPD = new BLLComplementoPago();

                string fechaProcesIni = "";
                string fechaProcesFin = "";


                fechaProcesIni = Request.Form[txtFechaProcIni.UniqueID];
                fechaProcesFin = Request.Form[txtFechaProcFin.UniqueID];

                if (fechaProcesIni.Length > 0 && fechaProcesFin.Length == 0)
                {
                    fechaProcesFin = fechaProcesIni;
                }
                else if (fechaProcesIni.Length == 0 && fechaProcesFin.Length > 0)
                {
                    fechaProcesIni = fechaProcesFin;
                }

                txtFechaProcIni.Text = fechaProcesIni;
                txtFechaProcFin.Text = fechaProcesFin;

                DateTime fechaProcIni = new DateTime();
                DateTime fechaProcFin = new DateTime();

                if (!String.IsNullOrEmpty(fechaProcesIni))
                {
                    if (DateTime.TryParse(fechaProcesIni, out fechaProcIni))
                    {
                        fechaProcesIni = fechaProcIni.ToString("yyyy-MM-dd");
                    }
                }

                if (!String.IsNullOrEmpty(fechaProcesFin))
                {
                    if (DateTime.TryParse(fechaProcesFin, out fechaProcFin))
                    {
                        fechaProcesFin = fechaProcFin.ToString("yyyy-MM-dd");
                    }
                }

                if (fechaProcIni > fechaProcFin)
                {
                    throw new Exception("Verifique Fechas de procesamiento, Fecha Inicial no puede ser mayor a Fecha Final...");
                }


                filtrosBusqueda.PNR = txtPNR.Text;
                filtrosBusqueda.CodigoGrupoPPD = ddlOrganization.SelectedValue;
                filtrosBusqueda.FechaIni = fechaProcIni;
                filtrosBusqueda.FechaFin = fechaProcFin;
                filtrosBusqueda.EstatusPago = ddlEstatusPago.SelectedValue;
                filtrosBusqueda.EstatusFacturacion = ddlEstatusFactura.SelectedValue;
                FiltrosBusqueda = filtrosBusqueda;




                List<ENTReservaPPD> listaReservas = new List<ENTReservaPPD>();
                listaReservas = bllFacPPD.RecuperaReservasPPDPorFiltros(filtrosBusqueda.PNR, filtrosBusqueda.CodigoGrupoPPD, filtrosBusqueda.FechaIni, filtrosBusqueda.FechaFin, filtrosBusqueda.EstatusPago, filtrosBusqueda.EstatusFacturacion);
                ListaReservas = listaReservas;
                //ListaPaginas = new List<ENTPaginas>();

                int totalPag = 0;
                int numFilasTotal = 0;
                numFilasTotal = listaReservas.Count();

                lblTotal.Text = numFilasTotal.ToString();
                btnExportarAExcel.Enabled = (numFilasTotal > 0);

                List<ENTPaginas> listaPaginas = new List<ENTPaginas>();
                if (numFilasTotal > 0)
                {
                    totalPag = (numFilasTotal / PnrPorPagina);
                    if ((numFilasTotal % PnrPorPagina) > 0)
                    {
                        totalPag += 1;
                    }
                    for (int i = 1; i <= totalPag; i++)
                    {
                        ENTPaginas pag = new ENTPaginas();
                        pag.PaginaActual = i;
                        pag.NumPaginaDeTotal = i.ToString() + " de " + totalPag.ToString();
                        listaPaginas.Add(pag);
                    }
                    ListaPaginas = listaPaginas;
                }

                ddlPagina.DataSource = ListaPaginas;
                ddlPagina.DataValueField = "PaginaActual";
                ddlPagina.DataTextField = "NumPaginaDeTotal";
                ddlPagina.DataBind();

                //Muestra la información en el Grid
                MostrarGrid(1);

            }
            catch (Exception ex)
            {
                btnExportarAExcel.Enabled = false;
                hfErrorBusqueda.Value = ex.Message;
                EnviarErrores(ex.Message);
            }
        }
        private void MostrarGrid(int pagina)
        {

            List<ENTReservaPPD> listaPorPagina = new List<ENTReservaPPD>();

            int saltoReg = (pagina - 1) * PnrPorPagina;
            listaPorPagina = ListaReservas.Skip(saltoReg).Take(PnrPorPagina).ToList();
            gvListaReservaciones.DataSource = listaPorPagina;
            gvListaReservaciones.DataBind();


            //List<ENTReservaPPD> lstEncabezados = new List<ENTReservaPPD>();

            //Solo en caso de que existan registros por mostrar se tomara el valor con mas caracteres de cada campo
            //if (listaPorPagina.Count > 0)
            //{

            //    long BookingID = listaPorPagina.Where(x => x.BookingID.ToString().Length == listaPorPagina.Max(y => (y.BookingID.ToString().Length))).FirstOrDefault().BookingID;
            //    string PNR = listaPorPagina.Where(x => x.PNR.Length == listaPorPagina.Max(y => (y.PNR.Length))).FirstOrDefault().PNR;
            //    string BookingStatus = listaPorPagina.Where(x => x.BookingStatus.Length == listaPorPagina.Max(y => (y.BookingStatus.Length))).FirstOrDefault().BookingStatus;
            //    string CreatedOrganizationCode = listaPorPagina.Where(x => x.CreatedOrganizationCode.Length == listaPorPagina.Max(y => (y.CreatedOrganizationCode.Length))).FirstOrDefault().CreatedOrganizationCode;
            //    string Organizacion = listaPorPagina.Where(x => x.Organizacion.Length == listaPorPagina.Max(y => (y.Organizacion.Length))).FirstOrDefault().Organizacion;
            //    int NumPasajeros = listaPorPagina.FirstOrDefault().NumPasajeros;
            //    string Moneda = listaPorPagina.Where(x => x.Moneda.Length == listaPorPagina.Max(y => (y.Moneda.Length))).FirstOrDefault().Moneda;
            //    decimal  MontoTotal = listaPorPagina.Where(x => x.MontoTotal.ToString().Length == listaPorPagina.Max(y => (y.MontoTotal.ToString().Length))).FirstOrDefault().MontoTotal;
            //    int PaidStatus = listaPorPagina.Where(x => x.PaidStatus.ToString().Length == listaPorPagina.Max(y => (y.PaidStatus.ToString().Length))).FirstOrDefault().PaidStatus;
            //    string EstatusPago = listaPorPagina.Where(x => x.EstatusPago.Length == listaPorPagina.Max(y => (y.EstatusPago.Length))).FirstOrDefault().EstatusPago;
            //    DateTime FechaCreacion = listaPorPagina.FirstOrDefault().FechaCreacion;
            //    DateTime FechaModificacion = listaPorPagina.FirstOrDefault().FechaModificacion;
            //    decimal MontoFacturado = listaPorPagina.Where(x => x.MontoFacturado.ToString().Length == listaPorPagina.Max(y => (y.MontoFacturado.ToString().Length))).FirstOrDefault().MontoFacturado;
            //    bool EstaMarcadoParaFacturacion = listaPorPagina.Where(x => x.EstaMarcadoParaFacturacion.ToString().Length == listaPorPagina.Max(y => (y.EstaMarcadoParaFacturacion.ToString().Length))).FirstOrDefault().EstaMarcadoParaFacturacion;
            //    long  IdFacturaCab = listaPorPagina.Where(x => x.IdFacturaCab.ToString().Length == listaPorPagina.Max(y => (y.IdFacturaCab.ToString().Length))).FirstOrDefault().IdFacturaCab;

            //    ENTReservaPPD lstEnc = new ENTReservaPPD();
            //    lstEnc.BookingID = BookingID;
            //    lstEnc.PNR = PNR;
            //    lstEnc.BookingStatus = BookingStatus;
            //    lstEnc.CreatedOrganizationCode = CreatedOrganizationCode;
            //    lstEnc.Organizacion = Organizacion;
            //    lstEnc.NumPasajeros = NumPasajeros;
            //    lstEnc.Moneda = Moneda;
            //    lstEnc.MontoTotal = MontoTotal;
            //    lstEnc.PaidStatus = PaidStatus;
            //    lstEnc.EstatusPago = EstatusPago;
            //    lstEnc.FechaCreacion = FechaCreacion;
            //    lstEnc.FechaModificacion = FechaModificacion;
            //    lstEnc.MontoFacturado = MontoFacturado;
            //    lstEnc.EstaMarcadoParaFacturacion = EstaMarcadoParaFacturacion;
            //    lstEnc.IdFacturaCab = IdFacturaCab;

            //    lstEncabezados.Add(lstEnc);
            //}

            //gvEnc.DataSource = lstEncabezados;

            //gvEnc.DataBind();
        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            ddlOrganization.SelectedIndex = 0;
            txtPNR.Text = "";
            txtFechaProcIni.Text = "";
            txtFechaProcFin.Text = "";
            ddlEstatusPago.SelectedIndex = 0;
            ddlEstatusFactura.SelectedIndex = 0;

            lblTotal.Text = "0";
            ListaPaginas = new List<ENTPaginas>();
            ddlPagina.DataSource = ListaPaginas;
            ddlPagina.DataBind();

            ListaReservas = new List<ENTReservaPPD>();
            gvListaReservaciones.DataSource = new List<ENTReservaPPD>();
            gvListaReservaciones.DataBind();

            btnExportarAExcel.Enabled = false;

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        private DataTable CrearLayoutReporte()
        {
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("BookingID", typeof(long));
            dtResult.Columns.Add("PNR");
            dtResult.Columns.Add("BookingStatus");
            dtResult.Columns.Add("CreatedOrganizationCode");
            dtResult.Columns.Add("Organizacion");
            dtResult.Columns.Add("NumPasajeros", typeof(int));
            dtResult.Columns.Add("Moneda");
            dtResult.Columns.Add("MontoTotal", typeof(decimal));
            dtResult.Columns.Add("MontoPagado", typeof(decimal));
            dtResult.Columns.Add("PaidStatus", typeof(int));
            dtResult.Columns.Add("EstatusPago");
            dtResult.Columns.Add("FechaCreacion", typeof(DateTime));
            dtResult.Columns.Add("FechaModificacion", typeof(DateTime));
            dtResult.Columns.Add("MontoFacturado", typeof(decimal));
            dtResult.Columns.Add("EstaMarcadoParaFacturacion", typeof(bool));
            dtResult.Columns.Add("IdFacturaCab", typeof(long));
            dtResult.Columns.Add("EstatusFacturado");
            dtResult.Columns.Add("DescEstatusFacturado");
            return dtResult;

        }
        protected void btnExportarAExcel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                List<ENTReservaPPD> listaReservasRep = new List<ENTReservaPPD>();
                listaReservasRep = ListaReservas;
                if (listaReservasRep.Count() > 0)
                {
                    //BUSCANDO LA INFORMACION QUE SERA ENVIADA AL ARCHIVO EXCEL
                    BLLComplementoPago bllCP = new BLLComplementoPago();
                    DataTable resCon = new DataTable();


                    DataTable dtReporte = new DataTable();
                    dtReporte = CrearLayoutReporte();

                    foreach (ENTReservaPPD fila in listaReservasRep)
                    {
                        DataRow dr = dtReporte.NewRow();
                        dr["BookingID"] = fila.BookingID;
                        dr["PNR"] = fila.PNR;
                        dr["BookingStatus"] = fila.BookingStatus;
                        dr["CreatedOrganizationCode"] = fila.CreatedOrganizationCode;
                        dr["Organizacion"] = fila.Organizacion;
                        dr["NumPasajeros"] = fila.NumPasajeros;
                        dr["Moneda"] = fila.Moneda;
                        dr["MontoTotal"] = fila.MontoTotal;
                        dr["MontoPagado"] = fila.MontoPagado;
                        dr["PaidStatus"] = fila.PaidStatus;
                        dr["EstatusPago"] = fila.EstatusPago;
                        dr["FechaCreacion"] = fila.FechaCreacion;
                        dr["FechaModificacion"] = fila.FechaModificacion;
                        dr["MontoFacturado"] = fila.MontoFacturado;
                        dr["EstaMarcadoParaFacturacion"] = fila.EstaMarcadoParaFacturacion;
                        dr["IdFacturaCab"] = fila.IdFacturaCab;
                        dr["EstatusFacturado"] = fila.EstatusFacturado;
                        dr["DescEstatusFacturado"] = fila.DescEstatusFacturado;
                        dtReporte.Rows.Add(dr);
                    }

                    Comun.Utils.ExportarExcel expExc = new Comun.Utils.ExportarExcel();
                    expExc.ExporttoExcelDataTable(dtReporte, "Reservas_PPD");
                }
            }
            catch (Exception ex)
            {
                hfErrorBusqueda.Value = ex.Message;
                //EnviarErrores(ex.Message);
            }
        }
        private void EnviarErrores(string error)
        {
            //BLLCambioItinerario bllCambio = new BLL.CambiosItinerario.BLLCambioItinerario();
            //if (bllCambio.RegistrarLog)
            //{
            //    Dictionary<string, string> listaErroresEnvio = new Dictionary<string, string>();
            //    listaErroresEnvio.Add("Consulta", error);
            //    bllCambio.EnviarErrores("", listaErroresEnvio, "Consulta de Notificaciones");
            //}
        }
        protected void gvListaNotificaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected void ddlPagina_SelectedIndexChanged(object sender, EventArgs e)
        {
            int numPagina = 0;
            numPagina = Convert.ToInt16(ddlPagina.SelectedValue.ToString());
            MostrarGrid(numPagina);
            //List<ENTListaNotificaciones> listaPorPagina = new List<ENTListaNotificaciones>();
            //listaPorPagina = ListaNotificaciones.OrderBy(x => x.Orden).Skip((numPagina - 1) * PnrPorPagina).Take(PnrPorPagina).ToList<ENTListaNotificaciones>();
            //gvListaNotificaciones.DataSource = listaPorPagina;
            //gvListaNotificaciones.DataBind();
        }

        protected void ddlOrganization_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvListaReservaciones_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvListaReservaciones.Rows)
            {
                string statusFacturacion = row.Cells[10].Text.ToUpper();
                foreach (var ctrl in row.Cells[0].Controls)
                {
                    if (ctrl is CheckBox)
                    {
                        var checkBox = (CheckBox)ctrl;
                        checkBox.Enabled = true;
                        checkBox.Visible = (statusFacturacion == "PENDIENTE");
                    }
                }

                foreach (var ctrl in row.Cells[1].Controls)
                {
                    if (ctrl is LinkButton)
                    {
                        var linkButon = (LinkButton)ctrl;
                        linkButon.Visible = (statusFacturacion == "FACTURADA");
                    }
                }

            }
        }

        protected void chkSeleccion_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvListaReservaciones.Rows)
            {
                foreach (var ctrl in row.Cells[0].Controls)
                {
                    if (ctrl is CheckBox)
                    {
                        var checkBox = (CheckBox)ctrl;
                        if (checkBox.Visible)
                        {
                            checkBox.Checked = ((CheckBox)sender).Checked;
                        }

                    }
                }
            }
        }

        protected void btnFacturar_Click(object sender, EventArgs e)
        {
            try
            {
                //Recupera los datos para la facturación.
                if (this.IsValid)
                {
                    string rfc = txtRFC.Text;
                    string taxId = txtTaxID.Text;
                    string pais = ddlResidencia.SelectedValue;
                    string correo = txtEMail.Text;
                    string confirmCorreo = txtEmailConf.Text;



                    List<string> listaPNRSel = new List<string>();

                    foreach (GridViewRow row in gvListaReservaciones.Rows)
                    {
                        foreach (var ctrl in row.Cells[0].Controls)
                        {
                            if (ctrl is CheckBox)
                            {
                                var checkBox = (CheckBox)ctrl;
                                if (checkBox.Checked)
                                {
                                    listaPNRSel.Add(row.Cells[2].Text);
                                }

                            }
                        }
                    }


                    foreach (string pnr in listaPNRSel)
                    {
                        ENTSolicitudesfacCab entSol = new ENTSolicitudesfacCab();
                        try
                        {

                            entSol.PNR = pnr;
                            entSol.RFCReceptor = rfc;
                            entSol.NumRegIdTrib_TAXID = taxId;
                            entSol.ResidenciaFiscal = pais;
                            entSol.UsoCFDI = "01";
                            entSol.emailReceptor = correo;
                            entSol.EsExtranjero = rdbExtranjero.Checked;
                            entSol.EsRobot = false;
                            entSol.NombrePasajero = "";
                            entSol.ApellidosPasajero = "";

                            BLLComplementoPago bllComFacPPD = new BLLComplementoPago();

                            bllComFacPPD.GeneraFacturaBase(entSol);

                            ////Inicia el proceso de descarga de los archivos Zip
                            //if (listaPagos.Where(x => x.RutaCFDI != null && x.RutaCFDI != "").Count() > 0)
                            //{
                            //    try
                            //    {
                            //        string rutaArchivos = "";

                            //        ENTPagosPorFacturar pagoFacturado = listaPagos.Where(x => x.RutaCFDI != null && x.RutaCFDI != "").FirstOrDefault();
                            //        rutaArchivos = pagoFacturado.RutaCFDI;

                            //        FileInfo archivoCfdi = new FileInfo(rutaArchivos);


                            //        string PathFact = archivoCfdi.Directory.Parent.FullName.ToString();
                            //        RutaArchivoDescarga = PathFact;
                            //        OpenNewWindow(HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, @"/DownloadFact.aspx"));

                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        if (!ex.Message.Contains("Thread was being aborted") && ex.Message.Length > 0)
                            //        {
                            //            BllLogErrores.RegistrarError(PNR, "", ex, "Portal", "GenerarZipFacturas");
                            //        }
                            //    }
                            //}

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
                                BllLogErrores.RegistrarError(entSol.PNR, mensajeUsuario, ex, "PortalAdmin", "btnFacturar_Click");
                                ShowFalla("", mensajeUsuario);
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
                    BllLogErrores.RegistrarError("", mensajeUsuario, ex, "PortalAdmin", "btnFacturar_Click");
                    ShowFalla("", mensajeUsuario);
                }
            }
        }

        protected void rdbExtranjero_CheckedChanged(object sender, EventArgs e)
        {
            pnlTaxId.Visible = rdbExtranjero.Checked;
            pnlPaisRes.Visible = rdbExtranjero.Checked;
            if (rdbExtranjero.Checked)
            {
                txtRFC.Text = "XEXX010101000";
                txtRFC.Enabled = false;
            }
            else
            {
                txtRFC.Text = "";
                txtRFC.Enabled = true;
            }

        }

        protected void rdbNacional_CheckedChanged(object sender, EventArgs e)
        {
            pnlTaxId.Visible = !rdbNacional.Checked;
            pnlPaisRes.Visible = !rdbNacional.Checked;
            if (rdbNacional.Checked)
            {
                txtRFC.Text = "";
                txtRFC.Enabled = true;
            }
            else
            {
                txtRFC.Text = "XEXX010101000";
                txtRFC.Enabled = false;
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
    }
}

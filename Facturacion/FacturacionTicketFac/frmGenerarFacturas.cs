//using Facturacion.BLL;
//using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.ENT;
using Facturacion.ENT.Portal.Facturacion;
using Facturacion.ENT.ProcesoFacturacion;
using Facturacion.InterfaceRestViva;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FacturacionTicketFac
{
    public partial class frmGenerarFacturas : Form
    {
        #region propiedades
        public string MensajeErrorUsuario { get; set; }
        public ENTDatosFacturacionPorRest DatosCliente { get; set; }
        public List<ENTPagosSinFacturar> ListaPagos { get; set; }
        public BLLBitacoraErrores BllLogErrores { get; set; }
        public string PNR { get; set; }

        #endregion

        public frmGenerarFacturas()
        {
            try
            {
                InitializeComponent();
                CargarCatalogos();
                LimpiarPantalla();
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "TicketFactura", "frmGenerarFacturas");
                ShowFalla("", mensajeUsuario);
            }

        }

        private void chkExtranjero_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblLeyRFC.Text = chkExtranjero.Checked ? "TAX ID" : "RFC";
                ddlPaisResidencia.Visible = chkExtranjero.Checked;
                txtRFC_TAXID.MaxLength = chkExtranjero.Checked ? 40 : 13;

            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "TicketFactura", "chkExtranjero_CheckedChanged");
                ShowFalla("", mensajeUsuario);
            }


        }

        private void LimpiarPantalla()
        {
            try
            {
                txtPNR.Text = "";
                txtEmail.Text = "";
                txtRFC_TAXID.Text = "";
                lblNumPagos.Text = "";
                chkExtranjero.Checked = false;
                ddlPaisResidencia.Visible = false;
                rdbGastosGral.Checked = true;
                rdbPorDefinir.Checked = false;
                dgPagos.DataSource = new DataTable();

                DatosCliente = new ENTDatosFacturacionPorRest();
                ListaPagos = new List<ENTPagosSinFacturar>();
                BllLogErrores = new BLLBitacoraErrores();
                txtPNR.Focus();
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "TicketFactura", "LimpiarPantalla");
                ShowFalla("", mensajeUsuario);
            }


        }

        private void BuscarPagos()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dgPagos.DataSource = new DataTable();
                PNR = txtPNR.Text;
                ENTDatosFacturacionPorRest datosCliente = new ENTDatosFacturacionPorRest();

                datosCliente.ClaveReservacion = txtPNR.Text.Trim();
                DatosCliente = datosCliente;



                //Recupera la informacion de los pagos que tiene registrados en Navitaire 
                MWFacturacionViva mwFacturacionViva = new MWFacturacionViva();
                List<ENTPagosSinFacturar> Pagos = new List<ENTPagosSinFacturar>();
                Pagos = mwFacturacionViva.BuscarPagosPorPNRDeTrafico(PNR);
                ListaPagos = new List<ENTPagosSinFacturar>();

                foreach (ENTPagosSinFacturar pagoPendiente in Pagos)
                {
                    pagoPendiente.EstaMarcadoParaFacturacion = true;
                    ListaPagos.Add(pagoPendiente);
                }

                dgPagos.DataSource = ListaPagos;
                lblNumPagos.Text = ListaPagos.Count.ToString();

                //En caso de no existir pagos pendientes por facturar enviara el siguiente mensaje de aviso
                if (ListaPagos.Count == 0)
                {
                    Show("", "No existen pagos pendientes por facturar.");
                    txtPNR.Focus();
                }
                else
                {
                    if (mwFacturacionViva.ListaImpresorasDisponibles.Count == 0)
                    {
                        Show("Sin impresoras...", "No se detectaron impresoras de Ticket conectadas al equipo, verifique por favor...");
                    }
                 

                    txtEmail.Focus();
                }



            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "TicketFactura", "BuscarPagos");
                ShowFalla("", mensajeUsuario);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private bool ValidarInformacionRequerida()
        {
            bool result = false;
            try
            {

                if (txtPNR.Text.Trim().Length == 0)
                {
                    throw new Exception("El PNR es requerido, favor de capturarlo.");
                }

                if (txtEmail.Text.Trim().Length == 0)
                {
                    throw new Exception("El correo electrónico es requerido, favor de capturarlo.");
                }
                else
                {
                    //Valida la estructura del correo
                    string expreReg = @"[a-z0-9]+([-+._][a-z0-9]+){0,2}@.*?(\.(a(?:[cdefgilmnoqrstuwxz]|ero|(?:rp|si)a)|b(?:[abdefghijmnorstvwyz]iz)|c(?:[acdfghiklmnoruvxyz]|at|o(?:m|op))|d[ejkmoz]|e(?:[ceghrstu]|du)|f[ijkmor]|g(?:[abdefghilmnpqrstuwy]|ov)|h[kmnrtu]|i(?:[delmnoqrst]|n(?:fo|t))|j(?:[emop]|obs)|k[eghimnprwyz]|l[abcikrstuvy]|m(?:[acdeghklmnopqrstuvwxyz]|il|obi|useum)|n(?:[acefgilopruz]|ame|et)|o(?:m|rg)|p(?:[aefghklmnrstwy]|ro)|qa|r[eosuw]|s[abcdeghijklmnortuvyz]|t(?:[cdfghjklmnoprtvwz]|(?:rav)?el)|u[agkmsyz]|v[aceginu]|w[fs]|y[etu]|z[amw])\b){1,2}";
                    if (!Regex.IsMatch(txtEmail.Text, expreReg))
                    {
                        throw new Exception("El email capturado no es válido, favor de verificar.");
                    }
                }


                if (chkExtranjero.Checked == true)
                {
                    if (txtRFC_TAXID.Text.Trim().ToUpper().Length == 0)
                    {
                        throw new Exception("El TAX ID es requerido, favor de capturarlo.");
                    }
                }
                else
                {

                    string expreReg = @"^([A-ZÑ&]{3,4}) ?(?:- ?)?(\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])) ?(?:- ?)?([A-Z\d]{2})([A\d])$";


                    if (!Regex.IsMatch(txtRFC_TAXID.Text, expreReg))
                    {
                        throw new Exception("El RFC capturado no es válido, favor de verificar.");
                    }



                    if (txtRFC_TAXID.Text.Trim().ToUpper().Length == 0)
                    {
                        throw new Exception("El RFC es requerido, favor de capturarlo.");
                    }
                    else if (txtRFC_TAXID.Text.Trim().ToUpper() == "XEXX010101000")
                    {
                        chkExtranjero.Checked = true;
                        throw new Exception("El RFC capturado es para extranjeros, favor de capturar un RFC Nacional válido...");
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "TicketFactura", "ValidarInformacionRequerida");
                ShowFalla("", mensajeUsuario);
            }


            return result;
        }

        private void ShowFalla(string titulo, string mensajeError)
        {
            if (titulo.Trim().Length == 0)
            {
                titulo = "Facturación Viva Aerobus";
            }
            MessageBox.Show(mensajeError, titulo, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        private void Show(string titulo, string mensaje)
        {
            if (titulo.Trim().Length == 0)
            {
                titulo = "Facturación Viva Aerobus";
            }
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnFacturar_Click(object sender, EventArgs e)
        {
            int facturasSolicitadas = 0;
            int facturasGeneradas = 0;
            int facturasSinEnvio = 0;

            

            if (ValidarInformacionRequerida())
            {
                //Se recupera la lista de pagos que resulto de la busqueda
                List<ENTPagosSinFacturar> listaPagos = new List<ENTPagosSinFacturar>();
                listaPagos = ListaPagos;
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    facturasSolicitadas = listaPagos.Where(x => x.EstaMarcadoParaFacturacion).Count();

                    //Verifica si existen pagos seleccionados
                    if (facturasSolicitadas == 0)
                    {
                        ShowFalla("Sin selección de Pagos", "No existen pagos seleccionados, favor de seleccionar alguno marcando la casilla en la columna Facturar...");
                    }
                    else
                    {
                        //Recupera la informacion de los datos de facturacion
                        DatosCliente.NombrePasajero = "";
                        DatosCliente.ApellidosPasajero = "";
                        DatosCliente.EmailReceptor = txtEmail.Text;
                        DatosCliente.EsExtranjero = chkExtranjero.Checked;
                        if (DatosCliente.EsExtranjero)
                        {
                            DatosCliente.PaisResidenciaFiscal = ((ENTGendescripcionesCat)ddlPaisResidencia.SelectedItem).CveValor;
                            DatosCliente.RFCReceptor = "XEXX010101000";
                            DatosCliente.TAXID = txtRFC_TAXID.Text.Trim();
                        }
                        else
                        {
                            DatosCliente.PaisResidenciaFiscal = "";
                            DatosCliente.TAXID = "";
                            DatosCliente.RFCReceptor = txtRFC_TAXID.Text.Trim();
                        }

                        DatosCliente.UsoCFDI = rdbGastosGral.Checked ? "G03" : "P01";
                        DatosCliente.Pagos = listaPagos;

                        MWFacturacionViva mWFacturacionViva = new MWFacturacionViva();
                        List<ENTPagosFacturadosREST> listaPagosFacturados = new List<ENTPagosFacturadosREST>();
                        listaPagosFacturados = mWFacturacionViva.GenerarFacturasPorListaPagos(DatosCliente);

                        facturasGeneradas = listaPagosFacturados.Where(x => x.Mensaje == "OK").Count();
                        facturasSinEnvio = listaPagosFacturados.Where(x => x.Mensaje == "SinEnvio").Count();


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
                            Show("Facturas generadas correctamente. Algunas facturas presentaron errores", "Sus facturas fueron enviadas a " + txtEmail.Text.Trim());
                        }
                        else if (facturasSolicitadas == facturasGeneradas)
                        {
                            Show("Facturas generadas correctamente.", "Sus facturas fueron enviadas a " + txtEmail.Text.Trim());
                        }
                        LimpiarPantalla();
                    }
                }
                catch (Exception ex)
                {
                    string mensajeUsuario = MensajeErrorUsuario;
                    BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "TicketFactura", "btnFacturar_Click");
                    ShowFalla("", mensajeUsuario);
                }
                finally
                {

                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void CargarCatalogos()
        {
            try
            {
                BLLGendescripcionesCat bllDescCat = new BLLGendescripcionesCat();

                //Carga del catalogo de Pais de Referencia en caso de extranjeros
                ddlPaisResidencia.DisplayMember = "Descripcion";
                ddlPaisResidencia.ValueMember = "CveValor";
                bllDescCat.CveTabla = "PAISES";
                ddlPaisResidencia.DataSource = bllDescCat.RecuperarGendescripcionesCatCvetabla("PAISES");
                lblVersion.Text = string.Format("Versión {0}", this.ProductVersion.ToString());


                MensajeErrorUsuario = "Por el momento no es posible procesar su solicitud por favor intente mas tarde...";
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "TicketFactura", "CargarCatalogos");
                ShowFalla("", mensajeUsuario);
            }

        }


        private void dgPagos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                string nombreCol = this.dgPagos.Columns[e.ColumnIndex].DataPropertyName;

                if (e.Value != null)
                {
                    switch (nombreCol)
                    {

                        case "MontoTotal":
                            FormateaMoneda(e);

                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "TicketFactura", "dgPagos_CellFormatting");
                ShowFalla("", mensajeUsuario);
            }

        }


        private string FormateaMoneda(DataGridViewCellFormattingEventArgs formatting)
        {
            string result = "";
            try
            {
                System.Text.StringBuilder dateString = new System.Text.StringBuilder();
                decimal dmonto = decimal.Parse(formatting.Value.ToString());
                result = dmonto.ToString("$ ###,###,###,##0.00");
                formatting.Value = result;
                formatting.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                formatting.FormattingApplied = true;
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "TicketFactura", "FormateaMoneda");
                ShowFalla("", mensajeUsuario);
            }


            return result;
        }

        private void txtPNR_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPNR.Text.Trim().Length == 6)
                {
                    BuscarPagos();

                }
                else
                {
                    if (ListaPagos.Count > 0)
                    {
                        ListaPagos = new List<ENTPagosSinFacturar>();
                        dgPagos.DataSource = ListaPagos;
                    }

                }
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "TicketFactura", "txtPNR_TextChanged");
                ShowFalla("", mensajeUsuario);
            }



        }

        private void txtPNR_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!(char.IsLetter(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && !(char.IsNumber(e.KeyChar)) && (e.KeyChar != '\u0016'))
            {
                e.Handled = true;
                return;
            }
        }

        private void frmGenerarFacturas_Load(object sender, EventArgs e)
        {
            try
            {
                BuscarTexBox(this);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "TicketFactura", "frmGenerarFacturas_Load");
                ShowFalla("", mensajeUsuario);
            }


        }
        private void BuscarTexBox(Control ctrl)
        {
            try
            {
                foreach (Control c in ctrl.Controls)
                {
                    if (c is TextBox)
                    {
                        if (c.Name.ToUpper().Contains("EMAIL"))
                        {
                            ((TextBox)c).CharacterCasing = CharacterCasing.Normal;
                        }
                        else
                        {
                            ((TextBox)c).CharacterCasing = CharacterCasing.Upper;
                        }
                        //Empleamos un casteo

                    }
                    else if (c.Controls.Count > 1)
                    {
                        BuscarTexBox(c);
                    }
                }
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "TicketFactura", "frmGenerarFacturas_Load");
                ShowFalla("", mensajeUsuario);
            }


        }

        private void txtRFC_TAXID_KeyPress(object sender, KeyPressEventArgs e)
        {
            string caracteresRFC = "abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ1234567890&\"<>\'";

            if (!caracteresRFC.Contains(e.KeyChar) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != '\u0016'))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtRFC_TAXID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

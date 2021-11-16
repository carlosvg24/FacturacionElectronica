using Comun.Utils;
using Facturacion.BLL;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.ENT;
using Facturacion.ENT.Portal.Facturacion;
using Facturacion.ENT.ProcesoFacturacion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinanzasTools
{
    public partial class frmFinanzasTools : Form
    {

        public string PNR { get; set; }
        public ENTReservaCab ReservaCab { get; set; }
        public List<ENTPagosCambiaFormaPago> ListaPagos { get; set; }
        public List<ENTFormapagoCat> ListaFormasPago { get; set; }
        public ENTPagosCab PagoActual { get; set; }

        public List<ENTGendescripcionesCat> ListaDescSAT { get; set; }

        public static int ExtNac = 1;

        public frmFinanzasTools()
        {
            InitializeComponent();
            txtTAX.Enabled = false;
            cbPais.Enabled = false;
            ExtNac = 1;
            btnSoyExtranjero.Text = "Extranjero";
            txtRFC.Enabled = true;
            txtRFC.Text = "";
            gpFacturar.Visible = false;
            CargarCombos();
        }

        private void CargarCombos()
        {
            //Carga del catalogo de Usos del CFDI publicado por el SAT
            BLLGendescripcionesCat bllDescCat = new BLLGendescripcionesCat();
            List<ENTGendescripcionesCat> listDescCat = new List<ENTGendescripcionesCat>();
            bllDescCat.CveTabla = "USOCFD";
            listDescCat = bllDescCat.RecuperarGendescripcionesCatCvetabla("USOCFD");
            cbUsoCFDI.DataSource = listDescCat;
            cbUsoCFDI.ValueMember = "CveValor";
            cbUsoCFDI.DisplayMember = "Descripcion";
            cbUsoCFDI.SelectedValue = 0;

            //Carga del catalogo de Pais de Referencia en caso de extranjeros
            listDescCat = new List<ENTGendescripcionesCat>();
            bllDescCat.CveTabla = "PAISES";
            listDescCat = bllDescCat.RecuperarGendescripcionesCatCvetabla("PAISES");
            cbPais.DataSource = listDescCat;
            cbPais.ValueMember = "CveValor";
            cbPais.DisplayMember = "Descripcion";
            cbPais.SelectedValue = 0;

            //Recupera el catalogo de descripciones de las formas de pago definidas por el SAT
            BLLGendescripcionesCat bllDescSAT = new BLLGendescripcionesCat();
            List<ENTGendescripcionesCat> listaDescripciones = new List<ENTGendescripcionesCat>();
            listaDescripciones = bllDescSAT.RecuperarGendescripcionesCatCvetabla("FRMPAG");
            ListaDescSAT = listaDescripciones.Where(x => x.Activo == true).OrderBy(x => x.Descripcion).ToList();

            //Recupera la descripcion del catalogo de formas de pago de Navitaire
            BLLFormapagoCat bllFormas = new BLLFormapagoCat();
            List<ENTFormapagoCat> listaPagosBD = new List<ENTFormapagoCat>();
            listaPagosBD = new List<ENTFormapagoCat>();
            listaPagosBD = bllFormas.RecuperarTodo();

            //Agrega un elemento vacio como elemento raiz del combo
            ENTFormapagoCat entFormaVacio = new ENTFormapagoCat();
            entFormaVacio.IdFormaPago = 0;
            entFormaVacio.Descripcion = "";
            ListaFormasPago = new List<ENTFormapagoCat>();
            ListaFormasPago.Add(entFormaVacio);

            BLLParametrosCnf bllParam = new BLLParametrosCnf();
            bllParam.RecuperarParametrosCnfNombre("CodFmaPagoGrupos");
            //Se filtran las formas de pago activas y ordenadas de forma alfabetica por descripcion
            string configFormasPago = "";
            if (bllParam != null && bllParam.IdParametro > 0)
            {
                configFormasPago = bllParam.Valor;
            }
            else
            {
                configFormasPago = "CK,Y9,AM,XU,CH";
            }

            List<string> listaFormasPagoValidas = new List<string>();

            foreach (string cve in configFormasPago.Split(','))
            {
                listaFormasPagoValidas.Add(cve);
            }



            ListaFormasPago.AddRange(listaPagosBD.Where(x => x.Activo == true && listaFormasPagoValidas.Contains(x.PaymentMethodCode)).OrderBy(x => x.Descripcion).ToList());

            //Asigna la lista de formas de pago al combo
            cmbMetodoPagoFact.DataSource = ListaFormasPago;
            cmbMetodoPagoFact.ValueMember = "IdFormaPago";
            cmbMetodoPagoFact.DisplayMember = "Descripcion";
            cmbMetodoPagoFact.SelectedValue = 0;


        }

        private void LimpiarPantalla()
        {
            ReservaCab = new ENTReservaCab();
            ListaPagos = new List<ENTPagosCambiaFormaPago>();
            lblFormaPagoSAT.Text = "";
            lblPaymentID.Text = "";
            cmbMetodoPagoFact.SelectedValue = 0;

            dgViewPagos.DataSource = ListaPagos;
        }


        private void HabilitarTextos(string accion)
        {
            switch (accion)
            {
                case "Buscar":
                    cmbMetodoPagoFact.Enabled = false;
                    btnProcesarPNR.Visible = true;
                    btnEditar.Visible = true;
                    btnCancelar.Visible = false;
                    btnGuardar.Visible = false;
                    gpFacturar.Visible = false;
                    break;
                case "Editar":
                    cmbMetodoPagoFact.Enabled = true;
                    btnProcesarPNR.Visible = false;
                    btnEditar.Visible = false;
                    btnCancelar.Visible = true;
                    btnGuardar.Visible = true;
                    break;
                case "Guardar":
                    cmbMetodoPagoFact.Enabled = false;
                    btnProcesarPNR.Visible = true;
                    btnEditar.Visible = true;
                    btnCancelar.Visible = false;
                    btnGuardar.Visible = false;
                    break;
                case "Cancelar":
                    cmbMetodoPagoFact.Enabled = false;
                    btnProcesarPNR.Visible = true;
                    btnEditar.Visible = true;
                    btnCancelar.Visible = false;
                    btnGuardar.Visible = false;
                    break;

                case "ActivarFact":
                    cmbMetodoPagoFact.Enabled = false;
                    btnProcesarPNR.Visible = true;
                    btnEditar.Visible = true;
                    btnCancelar.Visible = false;
                    btnGuardar.Visible = false;
                    break;


                default:
                    break;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                BuscarReservacion();
                if (!String.IsNullOrEmpty(lblFormaPagoSAT.Text))
                    gpFacturar.Visible = true;
                else
                    gpFacturar.Visible = false;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al realizar la busqueda del PNR...," + ex.Message,"Reservas Grupos",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void dgViewPagos_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                PagoActual = (ENTPagosCab)(dgViewPagos.Rows[e.RowIndex].DataBoundItem);

                SeleccionarPago();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al seleccionar un pago," + ex.Message, "Reservas Grupos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void SeleccionarPago()
        {
            try
            {
                lblPaymentID.Text = PagoActual.PaymentID.ToString();
                int idFormaPago = PagoActual.IdFormaPago;
                ENTFormapagoCat entFormaActual = ListaFormasPago.Where(x => x.IdFormaPago == idFormaPago).FirstOrDefault();
                if (entFormaActual != null)
                {
                    cmbMetodoPagoFact.SelectedValue = idFormaPago;
                }
                else
                {
                    cmbMetodoPagoFact.SelectedValue = 0;
                }
            
            }
            catch (Exception)
            {


            }

        }

        private void BuscarReservacion()
        {
            BLLReservaCab bllRes = new BLLReservaCab();
            BLLPagosCab bllPagos = new BLLPagosCab();
            BLLFinanzas bllFin = new FinanzasTools.BLLFinanzas();
            PNR = txtPNR.Text.Trim().ToUpper();
            LimpiarPantalla();

            List<ENTReservaCab> listaReserva = new List<ENTReservaCab>();

            listaReserva = bllRes.RecuperarReservaCabRecordlocator(PNR);

            if (listaReserva.Count > 0)
            {
                ReservaCab = listaReserva.FirstOrDefault();
                ListaPagos = bllFin.RecuperarPagosPorFacturar(ReservaCab.BookingID);

                dgViewPagos.DataSource = ListaPagos;
                HabilitarTextos("Buscar");

            }
            else
            {
                MessageBox.Show(string.Format("La reserva {0} no existe, favor de procesar...", PNR), "Reservas Grupos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                btnProcesarPNR.Visible = true;
                btnEditar.Visible = false;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                HabilitarTextos("Editar");
                cmbMetodoPagoFact.Focus();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al Editar un pago," + ex.Message, "Reservas Grupos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            int idFormaPago = 0;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (cmbMetodoPagoFact.SelectedValue != null && int.TryParse(cmbMetodoPagoFact.SelectedValue.ToString(), out idFormaPago))
                {
                    if (int.TryParse(cmbMetodoPagoFact.SelectedValue.ToString(), out idFormaPago))
                    {
                        ActualizarMetodoPago(idFormaPago);
                        HabilitarTextos("Guardar");
                        gpFacturar.Visible = true;
                        MessageBox.Show("Cambio aplicado exitosamente...", "Reservas Grupos", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }

                }
                else
                {
                    MessageBox.Show("Por favor seleccione una forma de pago valida...", "Reservas Grupos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al Editar un pago," + ex.Message, "Reservas Grupos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }





        }


        private void ActualizarMetodoPago(int idFormaPago)
        {

            ENTFormapagoCat entFormaNueva = new ENTFormapagoCat();
            entFormaNueva = ListaFormasPago.Where(x => x.IdFormaPago == idFormaPago).FirstOrDefault();
            if (entFormaNueva != null)
            {

                BLLPagosCab bllPago = new BLLPagosCab();
                bllPago.RecuperarPagosCabPorLlavePrimaria(PagoActual.IdPagosCab);


                bllPago.IdFormaPago = idFormaPago;
                bllPago.PaymentMethodCode = entFormaNueva.PaymentMethodCode;
                bllPago.Actualizar();

                BLLPagosDet bllPagoDet = new BLLPagosDet();
                bllPagoDet.RecuperarPagosDetPorLlavePrimaria(bllPago.IdPagosCab, bllPago.PaymentID);
                if (bllPagoDet != null)
                {
                    bllPagoDet.PaymentMethodCode = entFormaNueva.PaymentMethodCode;
                    bllPagoDet.Actualizar();
                }

                BuscarReservacion();
            }
            else
            {
                MessageBox.Show("La forma de pago seleccionada no se encuentra registrada en la BD...", "Reservas Grupos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void cmbMetodoPagoFact_SelectedIndexChanged(object sender, EventArgs e)
        {

            int formaPago = 0;
            lblFormaPagoSAT.Text = "";
            btnGuardar.Enabled = false;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (int.TryParse(cmbMetodoPagoFact.SelectedValue.ToString(), out formaPago))
                {
                    ENTFormapagoCat entFormaPago = new ENTFormapagoCat();
                    entFormaPago = ListaFormasPago.Where(x => x.IdFormaPago == formaPago).FirstOrDefault();

                    if (entFormaPago.CveFormaPagoSAT != null)
                    {
                        ENTGendescripcionesCat entDesc = new ENTGendescripcionesCat();
                        entDesc = ListaDescSAT.Where(x => x.CveValor == entFormaPago.CveFormaPagoSAT).FirstOrDefault();
                        lblFormaPagoSAT.Text = entDesc.Descripcion;
                        btnGuardar.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al seleccionar la forma de pago," + ex.Message, "Reservas Grupos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {


            try
            {
                Cursor.Current = Cursors.WaitCursor;
                HabilitarTextos("Cancelar");
                BuscarReservacion();
                txtPNR.Focus();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al Editar un pago," + ex.Message, "Reservas Grupos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnActivarFact_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //Reprocesa la reservacion
                BLLFinanzas bllFin = new BLLFinanzas();
                bllFin.ReprocesarReserva(PNR);
                //Recupera la información de la reservacion
                BuscarReservacion();
                HabilitarTextos("ActivarFact");
                SeleccionarPago();
                MessageBox.Show("Proceso Finalizado...", "Distribucion de Pagos", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error al reprocesar la reservación, " + ex.Message, "Reservas Grupos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void gpPago_Enter(object sender, EventArgs e)
        {

        }

        private void btnSoyExtranjero_Click(object sender, EventArgs e)
        {
            if (ExtNac == 0)
            {
                ExtNac = 1;
                btnSoyExtranjero.Text = "Extranjero";
                txtRFC.Enabled = true;
                txtRFC.Text = "";
                txtTAX.Enabled = false;
                cbPais.Enabled = false;
            }
            else
            {
                ExtNac = 0;
                btnSoyExtranjero.Text = "Nacional";
                txtRFC.Enabled = false;
                txtRFC.Text = "XEXX010101000";
                txtTAX.Enabled = true;
                cbPais.Enabled = true;
            }
        }

        private void btnFacturar_Click(object sender, EventArgs e)
        {
            String errores = String.Empty;
            List<ENTPagosFacturadosREST> listaResult = new List<ENTPagosFacturadosREST>();
            ENTDatosFacturacion datosFacturacion = new ENTDatosFacturacion();
            List<ENTPagosPorFacturar> listaPagos = new List<ENTPagosPorFacturar>();
            BLLFacturacion bllFacturacion = new BLLFacturacion();
            List<ENTPagosPorFacturar> listaPagosSel = new List<ENTPagosPorFacturar>();

            try
            {
                
                /*
               {
                    "ListaPagosSeleccionados": null,
                    "Usuario": null,
                    "Password": null,
                    "ClaveReservacion": null,
                    "NombrePasajero": null,
                    "ApellidosPasajero": null,
                    "UsoCFDI": null,
                    "EsExtranjero": false,
                    "RFCReceptor": null,
                    "PaisResidenciaFiscal": null,
                    "TAXID": null,
                    "emailReceptor": null
                }
            */
            
                datosFacturacion.ClaveReservacion = txtPNR.Text;
                datosFacturacion.NombrePasajero = "";
                datosFacturacion.ApellidosPasajero = "";
                datosFacturacion.UsoCFDI = cbUsoCFDI.SelectedValue != null ? cbUsoCFDI.SelectedValue.ToString() : String.Empty;
                datosFacturacion.EsExtranjero = ExtNac == 0 ? true : false;
                datosFacturacion.RFCReceptor = txtRFC.Text;
                datosFacturacion.PaisResidenciaFiscal = datosFacturacion.EsExtranjero == true?cbPais.SelectedValue.ToString():String.Empty;
                datosFacturacion.TAXID = txtTAX.Text;
                datosFacturacion.EmailReceptor = txtEmail.Text;

                ///  Validar datos de facturación
                ///  
                

                if (String.IsNullOrEmpty(datosFacturacion.UsoCFDI))
                    errores += "Debe indicar el valor de Uso de CFDI" + Environment.NewLine;

                if (!String.IsNullOrEmpty(datosFacturacion.RFCReceptor))
                {
                    var regexRFC = @"[A-Z&Ñ]{3,4}[0-9]{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[01])[A-Z0-9]{2}[0-9A]";
                    var matchRFC = Regex.Match(datosFacturacion.RFCReceptor, regexRFC, RegexOptions.IgnoreCase);
                    if (!matchRFC.Success) errores += "El RFC no es valido" + Environment.NewLine;
                } else
                    errores += "Debe indicar el valor de RFC" + Environment.NewLine;


                if (!String.IsNullOrEmpty(datosFacturacion.EmailReceptor))
                {
                    var regexEMAIL = @"^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$";
                    var matchEMAIL = Regex.Match(datosFacturacion.EmailReceptor, regexEMAIL, RegexOptions.IgnoreCase);
                    if (!matchEMAIL.Success) errores += "El email no es valido" + Environment.NewLine;
                }
                else
                    errores += "Debe indicar el valor de Email" + Environment.NewLine;

                //if (String.IsNullOrEmpty(datosFacturacion.NombrePasajero))
                //    errores += "Debe indicar el valor de NombrePasajero" + Environment.NewLine;
                //if (String.IsNullOrEmpty(datosFacturacion.ApellidosPasajero))
                //    errores += "Debe indicar el valor de ApellidosPasajero" + Environment.NewLine;
                if (String.IsNullOrEmpty(datosFacturacion.ClaveReservacion))
                    errores += "Debe indicar el valor del PNR" + Environment.NewLine;

                if (datosFacturacion.EsExtranjero)
                {
                    if(String.IsNullOrEmpty(datosFacturacion.TAXID))
                        errores += "Debe indicar TAX" + Environment.NewLine;
                    if (String.IsNullOrEmpty(datosFacturacion.PaisResidenciaFiscal))
                        errores += "Debe indicar Pais" + Environment.NewLine;
                }

                if (errores.Length > 0)
                    MessageBox.Show(errores);
                else
                {
                    //Recupera la información de los pagos que aún no están facturados
                    //listaPagos = bllFacturacion.RecuperarPagosParaFacturar(datosFacturacion);

                    //Se identifican los pagos que pueden ser facturados
                    foreach (var pago in listaPagos)
                    {
                        pago.PNR = datosFacturacion.ClaveReservacion;
                        // Determina sí el pago corresponde al OTA
                        BLLOtasCat bllOtasCat = new BLLOtasCat();
                        var numSocComEncon = bllOtasCat.RecuperarOtasCatOrganizationcode(pago.OrganizationCode)
                                            .ToList().Where(o => o.Activo == true).Count();
                        //pago.EsFacturable = numSocComEncon > 0 ? true : false;
                        //pago.Mensaje = pago.Mensaje != null && pago.Mensaje.Length > 0 ? pago.Mensaje : numSocComEncon < 1 ? "Éste pago no puede facturarse por éste medio" : String.Empty;

                        if (pago.EsFacturable == true
                            && pago.EsFacturado == false
                            && pago.EnVigenciaParaFacturacion == true)
                        {

                            pago.EstaMarcadoParaFacturacion = true;

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
                                            //copia de Default

                                            BLLFacturacion bllFacturacionFC = new BLLFacturacion();
                                            BLLBinCat bllBinCatFC = new BLLBinCat();

                                            // Validar BIN
                                            List<ENTBinCat> listTipoFC = new List<ENTBinCat>();
                                            listTipoFC = bllBinCatFC.RecuperarBinCatPorLlavePrimaria(pago.BinRange);

                                            string tipoFC = listTipoFC != null && listTipoFC.Count > 0 ? listTipoFC.FirstOrDefault().TIPO : "";
                                            //string tipoFC = listTipoFC.FirstOrDefault().TIPO;

                                            // Validar BIN
                                            // ResponseBINRest response = bllFacturacion.InvocarBINRest(pago.BinRange.ToString());
                                            // 04 -> TC  |   28 -> TD   |   01 -> Efectivo
                                            //cveFormaPago = response.type != null ? response.type.ToLower() == "credit" ? "04" : "28" : "";
                                            cveFormaPago = !String.IsNullOrEmpty(tipoFC) ? tipoFC.ToLower() == "credit" ? "04" : "28" : "";

                                            // Corregir en automático el método de pago
                                            if (!String.IsNullOrEmpty(cveFormaPago))
                                            {
                                                pago.UpdFormaPagModificadoPor = Tipo.ClientePortal.Sistema;
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

                                                seCorrigio = bllFacturacionFC.ActualizarFormaPago(pago);
                                            }
                                        }
                                    }
                                }
                            }

                            listaPagosSel.Add(pago);
                        }
                    }



                    if (listaPagosSel.Count() > 0)
                    {
                        try
                        {
                            //listaPagosSel = bllFacturacion.GenerarFacturaCliente(datosFacturacion, listaPagosSel);

                            //Se recorren los pagos facturados para armar la lista de resultado
                            foreach (ENTPagosPorFacturar pagoProc in listaPagosSel)
                            {
                                long idFacturaCab = pagoProc.IdFacturaCab;

                                //Se asignan los datos correspondientes al pago
                                ENTPagosFacturadosREST pagoFactRes = new ENTPagosFacturadosREST();
                                pagoFactRes.IdPagosCab = pagoProc.IdPagosCab;
                                pagoFactRes.BookingID = pagoProc.BookingID;
                                pagoFactRes.PaymentID = pagoProc.PaymentID;
                                pagoFactRes.MontoTotal = pagoProc.MontoTotal;
                                pagoFactRes.RutaCFDI = pagoProc.RutaCFDI;
                                pagoFactRes.RutaPDF = pagoProc.RutaPDF;
                                pagoFactRes.Mensaje = pagoProc.Mensaje;
                                pagoFactRes.Moneda = pagoProc.CurrencyCode;
                                pagoFactRes.TipoCambio = pagoProc.TipoCambio;

                                //Se recupera la informacion de la factura
                                BLLFacturasCab bllFactura = new BLLFacturasCab();
                                bllFactura.RecuperarFacturasCabPorLlavePrimaria(idFacturaCab);
                                if (bllFactura.IdFacturaCab > 0)
                                {
                                    pagoFactRes.IdPeticionPAC = bllFactura.IdPeticionPAC;
                                    pagoFactRes.Serie = bllFactura.Serie;
                                    pagoFactRes.FolioFactura = bllFactura.FolioFactura;
                                    pagoFactRes.UUID = bllFactura.UUID;
                                    pagoFactRes.SubTotal = bllFactura.SubTotal;
                                    pagoFactRes.Descuento = bllFactura.Descuento;
                                    pagoFactRes.MontoIVA = bllFactura.MontoIVA;
                                    pagoFactRes.Total = bllFactura.Total;

                                    //Se recupera la informacion del CFDI
                                    BLLFacturascfdiDet bllCFDI = new BLLFacturascfdiDet();
                                    bllCFDI.RecuperarFacturascfdiDetIdfacturacab(idFacturaCab);
                                    if (bllCFDI.IdFacturaCab > 0)
                                    {
                                        pagoFactRes.FechaTimbrado = bllCFDI.FechaTimbrado;
                                        pagoFactRes.CFDI = bllCFDI.CFDI;
                                        pagoFactRes.CadenaOriginal = bllCFDI.CadenaOriginal;
                                    }
                                }
                                listaResult.Add(pagoFactRes);
                            }

                            //// Generar ZIP 
                            //string rutaArchivos = "";

                            //ENTPagosPorFacturar pagoFacturado = listaPagosSel.Where(x => x.RutaCFDI != null && x.RutaCFDI != "").FirstOrDefault();
                            //rutaArchivos = pagoFacturado.RutaCFDI;

                            //FileInfo archivoCfdi = new FileInfo(rutaArchivos);
                            //rutaArchivos = archivoCfdi.Directory.Parent.FullName.ToString();

                            //DirectoryInfo carpeta = new DirectoryInfo(rutaArchivos);
                            //if (carpeta.Exists)
                            //{
                            //    string zipName = String.Format("CFDI_{0}.zip", carpeta.Name);
                            //    using (ZipFile zip = new ZipFile())
                            //    {

                            //        foreach (FileInfo archivo in carpeta.GetFiles())
                            //        {
                            //            if (archivo.Extension.ToString() != "zip")
                            //                zip.AddEntry(archivo.Name, archivo.OpenRead());
                            //        }

                            //        foreach (DirectoryInfo subCarpeta in carpeta.GetDirectories())
                            //        {
                            //            foreach (FileInfo archivo in subCarpeta.GetFiles())
                            //            {
                            //                if (archivo.Extension.ToString() != "zip")
                            //                    zip.AddEntry(archivo.Name, archivo.OpenRead());
                            //            }
                            //        }

                            //        zip.Save(carpeta + "\\" + zipName);
                            //    }

                            //    String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery
                            //                                                                      , HttpContext.Current.Request.ApplicationPath) + "/";
                            //    strUrl = Path.Combine(strUrl, @"Archivos/" + DateTime.Now.ToString("dd-MM-yyyy") + "/" + carpeta.Name); //
                            //    strUrl = strUrl + "/" + zipName;

                            //        MessageBox.Show("Se facturó correctamente: " + strUrl);
                            //}
                            //else
                            //{
                            //    MessageBox.Show("No existe el path para generar ZIP");
                            //}
                            MessageBox.Show("El PNR se facturó correctamente");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hubo un error al facturar" + Environment.NewLine + ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No hay pagos pendientes que facturar");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            ExtNac = 1;
            btnSoyExtranjero.Text = "Extranjero";
            txtTAX.Enabled = false;
            cbPais.Enabled = false;
            txtRFC.Text = "";
            txtRFC.Enabled = true;
        }
    }






}

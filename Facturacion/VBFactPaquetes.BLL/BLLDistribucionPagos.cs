using System;
using System.Collections.Generic;
using System.Data;
using VBFactPaquetes.BLL.BDFacturacion;
using VBFactPaquetes.DAO;
using VBFactPaquetes.Model;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using VBFactPaquetes.Model.Facturacion;

namespace VBFactPaquetes.BLL
{
    public class BLLDistribucionPagos
    {
        private DataSet dsResultado = new DataSet();

        public  String getEstatus(int estatus)
        {
            if (estatus == 1)
                return "OK";
            else if (estatus == 2)
                return "Mod";
            else if (estatus == 3)
                return "Can";
            else
                return "";
        }

        public Paquete BookingFromREST(string pnr)
        {
            DataSet dsPaquete = new DataSet();
            DataSet dsResultado = new DataSet();
            //string token = getAuthToken();
            Paquete paquete = new Paquete();
            try
            {
                //if (token != "")
                //{
                //IRestResponse response = getBookingByID(token, pnr);
                //Obtener el paquete del servicio REST
                string text = System.IO.File.ReadAllText(@"C:\Users\carlos.galindo\Desktop\VivaPaquete.json");
                paquete = Newtonsoft.Json.JsonConvert.DeserializeObject<Paquete>(text);
                //Almacena el paquete en una variable de sesión para consultas posteriores
                System.Web.HttpContext.Current.Session["paquete"] = paquete;
                //Convertir el paquete a un DataSet
                dsResultado = FromREST_ToDataSet(paquete);
                //Insertar en BD
                dsResultado = Insertar(dsResultado.Tables["Reserva"], dsResultado.Tables["Fees"], dsResultado.Tables["Pagos"]);
                //}
            }
            catch (Exception ex)
            {

            }
            return paquete;
        }

        public DataSet FromREST_ToDataSet(Paquete paquete)
        {
            DataSet dsResultado = new DataSet();
            DataTable dtReserva = new DataTable("Reserva");
            DataTable dtFees = new DataTable("Fees");
            DataTable dtPagos = new DataTable("Pagos");
            Order order = new Order();
            List<Payment> listPayments = new List<Payment>();
            List<Item> listItems = new List<Item>();

            try
            {
                order = paquete.order;
                listItems = paquete.items;
                listPayments = paquete.payments;

                // Pasar de order, payments y Items a DataTables

                /* MAPEAR y Asignar valores a la Reserva */
                dtReserva.Columns.Add("BookingId", typeof(System.String));
                dtReserva.Columns.Add("PNR", typeof(System.String));
                dtReserva.Columns.Add("Estatus", typeof(System.String));
                dtReserva.Columns.Add("CurrencyCode", typeof(System.String));
                dtReserva.Columns.Add("CreatedUTC", typeof(System.DateTime));
                dtReserva.Columns.Add("CreatedLocal", typeof(System.DateTime));
                dtReserva.Columns.Add("ModifiedUTC", typeof(System.DateTime));
                dtReserva.Columns.Add("ModifiedLocal", typeof(System.DateTime));
                dtReserva.Columns.Add("Amount", typeof(System.Double));
                dtReserva.Columns.Add("ConversionFactor", typeof(System.Decimal));

                DataRow drReserva = dtReserva.NewRow();
                drReserva["BookingId"] = order.bookingId;
                drReserva["PNR"] = order.bookingId;
                drReserva["Estatus"] = getEstatus(order.status);
                drReserva["CurrencyCode"] = order.currencyCode;
                drReserva["CreatedUTC"] = order.createdUTC;//.ToString("dd/MM/yyyy hh:mm:ss");
                //Agregado
                drReserva["CreatedLocal"] = DateTime.Now;
                drReserva["ModifiedUTC"] = order.modifiedUTC;
                //Agregado
                drReserva["ModifiedLocal"] = DateTime.Now;
                drReserva["Amount"] = order.amount;
                drReserva["ConversionFactor"] = order.conversionFactor;
                dtReserva.Rows.Add(drReserva);

                /* MAPEAR y Asignar valores al Pago */
                dtPagos.Columns.Add("BookingId", typeof(System.String));
                dtPagos.Columns.Add("PaymentId", typeof(System.String));
                dtPagos.Columns.Add("PaymentMethodCode", typeof(System.String));
                dtPagos.Columns.Add("PaymentMethodType", typeof(System.String));
                dtPagos.Columns.Add("PaymentAmount", typeof(System.Decimal));
                dtPagos.Columns.Add("ApprovalUTC", typeof(System.DateTime));

                //dtPagos.Columns.Add("lastFourDigits", typeof(System.String));
                //dtPagos.Columns.Add("holderName", typeof(System.String));
                //dtPagos.Columns.Add("transactionId", typeof(System.String));
                //dtPagos.Columns.Add("currency", typeof(System.String));
                //dtPagos.Columns.Add("state", typeof(System.Int32));

                foreach (Payment p in listPayments)
                {
                    DataRow drPago = dtPagos.NewRow();
                    drPago["BookingId"] = order.bookingId;
                    drPago["PaymentId"] = p.paymentId;
                    drPago["PaymentMethodCode"] = "UI";
                    drPago["PaymentMethodType"] = "Credit Card";
                    drPago["PaymentAmount"] = p.amount;
                    drPago["ApprovalUTC"] = p.updated;//.ToString("dd/MM/yyyy hh:mm:ss");
                    //drPago["lastFourDigits"] = p.lastFourDigits;
                    //drPago["holderName"] = p.holderName;
                    //drPago["transactionId"] = p.transactionId;
                    //drPago["currency"] = p.currency;
                    //drPago["state"] = p.state;
                    dtPagos.Rows.Add(drPago);
                }

                /* MAPEAR Items y Asignar Valores*/
                dtFees.Columns.Add("BookingId", typeof(System.String));
                dtFees.Columns.Add("PaymentId", typeof(System.String));
                dtFees.Columns.Add("LineId", typeof(System.String));
                dtFees.Columns.Add("LineDateUTC", typeof(System.DateTime));
                dtFees.Columns.Add("LineCancelled", typeof(System.Int16));
                dtFees.Columns.Add("LineCancelledDate", typeof(System.DateTime));
                dtFees.Columns.Add("ExternalReference", typeof(System.String));
                dtFees.Columns.Add("ProductTypeId", typeof(System.Int32));
                dtFees.Columns.Add("ProductType", typeof(System.String));
                dtFees.Columns.Add("SellingPrice", typeof(System.Decimal));
                dtFees.Columns.Add("IVASellingPrice", typeof(System.Decimal));
                dtFees.Columns.Add("CurrencyCode", typeof(System.String));

                DataRow drFee = null;
                foreach (Item i in listItems)
                {
                    drFee = dtFees.NewRow();
                    drFee["BookingId"] = order.bookingId;
                    drFee["PaymentId"] = listPayments[0].paymentId;
                    drFee["LineId"] = i.itemId;
                    drFee["LineDateUTC"] = listPayments[0].updated;//.ToString("dd/MM/yyyy hh:mm:ss");
                    drFee["LineCancelled"] = (i.status == 1 || i.status == 2) ? 0 : 1;
                    drFee["LineCancelledDate"] = i.status == 3 ? listPayments[0].updated : DateTime.MaxValue;
                    drFee["ExternalReference"] = i.productType.referenceId;
                    drFee["ProductTypeId"] = i.productType.productTypeId;
                    drFee["ProductType"] = i.productType.productType;
                    drFee["SellingPrice"] = i.sellingPrice;
                    drFee["IVASellingPrice"] = i.tax;
                    drFee["CurrencyCode"] = i.sellingCurrency;
                    dtFees.Rows.Add(drFee);
                }

                // Agregar DataTables a DataSet
                dsResultado.Tables.Add(dtReserva);
                dsResultado.Tables.Add(dtPagos);
                dsResultado.Tables.Add(dtFees);
            }

            catch (Exception ex)
            {
                throw ex;
            }

            //catch (Excepciones ex)
            //{
            //    throw ex;
            //}
            //catch (Exception ex)
            //{
            //    Dictionary<string, object> parametros = new Dictionary<string, object>();
            //    //parametros.Add("dtReserva", dtReserva);
            //    //parametros.Add("dtFees", dtFees);
            //    //parametros.Add("dtPagos", dtPagos);
            //    throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
            //                          MethodBase.GetCurrentMethod().Name,
            //                          parametros,
            //                          ex, Excepciones.TipoPortal.VivaPaquetes);
            //}

            return dsResultado;
        }

        public DataTable InsertarDatosEnPaquetesPorFecha(DateTime fechaConsulta, bool extraccionPorFecha)
        {
            DataTable dtReserva = new DataTable();
            DataTable dtFees = new DataTable();
            DataTable dtPagos = new DataTable();
            List<String> listPNRs = new List<String>();
            bool hayCancelacion = false;
            String errorSQL = String.Empty;
            ErrorExtraccion errExt = new ErrorExtraccion();
            List<ErrorExtraccion> listErrExt = new List<ErrorExtraccion>();

            DataTable dtFeesQuery = new DataTable();
            DataTable dtNuevoFee = new DataTable();

            decimal montoPago = 0;
            String statusReserva = String.Empty;
            String codigoMonedaReserva = String.Empty;

            dsResultado = new DataSet();

            try
            {
                // Obtener reservas por fecha
                // Método pendiente

                // Recorre las reservas y 1 por 1 obtiene sus pagos
                foreach (String pnr in listPNRs)
                {
                    hayCancelacion = false;
                    errorSQL = String.Empty;

                    // Obtener valores del servico REST
                    //dsResultado = FromREST_ToDataSet(pnr);


                    // Si no se obtienen datos de la reserva se omite, sino obtenemos la moneda
                    if (dtReserva == null || (dtReserva != null && dtReserva.Rows.Count < 1))
                    {
                        if (dtReserva == null || (dtReserva != null && dtReserva.Rows.Count < 1))
                            errorSQL = "No hay datos de reservación";
                    }

                    // Si no se localizaron pagos se omite Reserva
                    if (String.IsNullOrEmpty(errorSQL) && dtPagos == null || (dtPagos != null && dtPagos.Rows.Count < 1))
                        errorSQL = "No hay datos de pago";

                    // Valida si la reserva tiene cancelación
                    if (errorSQL.Length < 1)
                        hayCancelacion = TieneCancelacíon(dtReserva, dtPagos, dtFees);

                    // Prepara el informe que enviará
                    #region "INFORME"
                    String lines = String.Empty;
                    errExt = new ErrorExtraccion();
                    errExt.BookingId = pnr;
                    errExt.TieneCancelacion = hayCancelacion ? "SI" : "NO";
                    errExt.FechaExtraccion = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                    if (dtReserva != null && dtReserva.Rows.Count > 0)
                    {
                        DateTime fechaCreacion = new DateTime();
                        if (dtReserva.Rows[0]["CreatedUTC"] != null)
                            DateTime.TryParse(dtReserva.Rows[0]["CreatedUTC"].ToString(), out fechaCreacion);

                        errExt.PNR = dtReserva.Rows[0]["PNR"].ToString();
                        errExt.FechaRegistroJun = fechaCreacion.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    #endregion

                    if (errorSQL.Length < 1)
                    {
                        //  **CANCELACIÓN * *Guardar info de Juniper en la BD de VBFactPaqQA
                        if (hayCancelacion)
                        {
                            dsResultado = Exchange(dtReserva, dtPagos, dtFees);
                           
                            /*
                            if (dsResultado != null && dsResultado.Tables != null && dsResultado.Tables.Count > 3 && !dsResultado.Tables[0].Columns.Contains("Error"))
                            {
                                // Obtiene Products que se agregaron en la misma reserva
                                dtFees = daoInfoDeJuniper.ObtenerProducts(pnr.bookingid, paymentId, codigoMonedaReserva, fechaConsulta, false);
                                if (dtFees.Rows.Count > 0)
                                    dsResultado = Insertar(dtReserva, dtPagos, dtFees);
                            }
                            */
                        }

                        // ** NUEVO **     Guardar info de Juniper en la BD VBFactPaqQA
                        if (!hayCancelacion)
                        {
                            if (dtFees == null || (dtFees != null && dtFees.Rows.Count > 1))
                                dsResultado = Insertar(dtReserva, dtPagos, dtFees);
                            else
                            {
                                DataTable errDT = new DataTable("Errores");
                                errDT.Columns.Add("ERROR", typeof(System.String));
                                DataRow newRow = errDT.NewRow();
                                newRow["ERROR"] = "No hay Productos en Reserva sin Cancelar";
                                errDT.Rows.Add(newRow);
                                dsResultado = new DataSet();
                                dsResultado.Tables.Add(errDT);
                            }
                        }
                    }
                    else
                    {
                        DataTable errDT = new DataTable("Errores");
                        errDT.Columns.Add("ERROR", typeof(System.String));
                        DataRow newRow = errDT.NewRow();
                        newRow["ERROR"] = errorSQL;
                        errDT.Rows.Add(newRow);
                        dsResultado = new DataSet();
                        dsResultado.Tables.Add(errDT);
                    }



                    #region "INFORME"
                    // Obtiene el error que se genera en la BD
                    if (dsResultado != null && dsResultado.Tables != null && dsResultado.Tables.Count < 2 && dsResultado.Tables.Count > 0)
                        errExt.Error = dsResultado.Tables[0].Rows[0][0].ToString();

                    // Loguea los lines 
                    if (dtPagos != null && dtPagos.Rows.Count > 0)
                    {
                        errExt.PaymentId = dtPagos.Rows[0]["PaymentID"].ToString();
                        errExt.MontoTotal = dtPagos.Rows[0]["PaymentAmount"].ToString();

                        if (dtFees != null && dtFees.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtFees.Rows)
                            {
                                lines += lines == String.Empty ? dr["LineId"].ToString() : ", " + dr["LineId"].ToString();
                            }

                            errExt.Lines = lines;
                        }
                    }

                    if (dsResultado != null && dsResultado.Tables != null && dsResultado.Tables.Count > 0)
                    {
                        if (dsResultado.Tables[0].Columns.Contains("ErrorProcedure"))
                        {
                            errorSQL =
                            dsResultado.Tables[0].Rows[0]["ErrorProcedure"].ToString() + " - " +
                            dsResultado.Tables[0].Rows[0]["Error"].ToString();

                            errExt.Error = errorSQL;

                            //throw new Exception(errorSQL);
                        }
                        else if (dsResultado.Tables[0].Columns.Contains("Error"))
                            errExt.Error = dsResultado.Tables[0].Rows[0]["Error"].ToString();
                    }

                    listErrExt.Add(errExt);
                    #endregion
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //catch (Excepciones ex)
            //{
            //    throw ex;
            //}
            //catch (Exception ex)
            //{
            //    Dictionary<string, object> parametros = new Dictionary<string, object>();
            //    //parametros.Add("dtReserva", dtReserva);
            //    //parametros.Add("dtFees", dtFees);
            //    //parametros.Add("dtPagos", dtPagos);
            //    throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
            //                          MethodBase.GetCurrentMethod().Name,
            //                          parametros,
            //                          ex, Excepciones.TipoPortal.VivaPaquetes);
            //}

            return new DataTable();
        }

        public bool TieneCancelacíon(DataTable dtReserva, DataTable dtPagos, DataTable dtFees)
        {
            bool hayCancelacion = false;
            String statusReserva = String.Empty;
            decimal montoPago = 0;

            try
            {
                // Valida el Status de la reservación
                foreach (DataRow dr in dtReserva.Rows)
                {
                    statusReserva = dtReserva.Rows[0]["Estatus"].ToString();

                    if (statusReserva == "Can")
                        hayCancelacion = true;
                }

                // Valida el pago, sí el monto es negativo se condiera como cancelación
                foreach (DataRow dr in dtPagos.Rows)
                {
                    Decimal.TryParse(dr["PaymentAmount"].ToString(), out montoPago);

                    if (montoPago <= 0)
                        hayCancelacion = true;
                }

                // Valida sí los Fees traen cancelación
                if (dtFees != null && dtFees.Rows.Count > 0)
                {
                    for (int x = 0; x < dtFees.Rows.Count; x++)
                    {
                        if (bool.Parse(dtFees.Rows[x]["LineCancelled"].ToString()))
                            hayCancelacion = true;
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return hayCancelacion;
        }

        public DataSet Insertar(DataTable dtReserva, DataTable dtFees, DataTable dtPagos)
        {
            DataSet dsResultado = new DataSet();
            DAOExtraccionPagos daoExtPagos = new DAOExtraccionPagos();

            try
            {
                dsResultado = daoExtPagos.InsertarPagos(dtReserva, dtFees, dtPagos);
            }
            catch (Exception ex)
            {
                dsResultado = new DataSet();
                DataTable errDT = new DataTable("Errores");

                errDT.Columns.Add("ERROR", typeof(System.String));
                DataRow newRow = errDT.NewRow();
                newRow["ERROR"] = ex.Message + " - " + ex.StackTrace;
                errDT.Rows.Add(newRow);

                dsResultado = new DataSet();
                dsResultado.Tables.Add(errDT);
            }

            return dsResultado;
        }

        public DataSet Exchange(DataTable dtReserva, DataTable dtPagos, DataTable dtFees)
        {
            DAOVivaPaquetes daoVivaPaq = new DAOVivaPaquetes();
            DataTable dtConsultarReserva = new DataTable();
            dsResultado = new DataSet();
            string bookingId;
            List<long> feesDelPNR = new List<long>();
            DataTable dtResultado = new DataTable();
            object objTotal;
            Decimal total = 0;
            string paymentId = "";
            Model.Facturacion.Pago pago = new Model.Facturacion.Pago();
            DatosFiscales DatosFiscalesEgreso = new DatosFiscales();
            List<Model.Facturacion.Pago> lstPago = new List<Model.Facturacion.Pago>();
            List<DatosFiscales> listDatosFiscales = new List<DatosFiscales>();
            Conceptos concepto = new Conceptos();
            List<Conceptos> listConceptos = new List<Conceptos>();
            List<DatosGenerales> listDatosGenerales = new List<DatosGenerales>();
            DatosGenerales datosGenerales = new DatosGenerales();
            List<Pago.PagosEgresosGlobal> listEgresosGlobal = new List<Pago.PagosEgresosGlobal>();
            Pago.PagosEgresosGlobal pagoEgresosGlobal = new Pago.PagosEgresosGlobal();
            bool seGeneroCFDI = false;
            DataTable errDT = new DataTable("Errores");
            String strFaltaTablas = String.Empty;
            bool estaFacturado = false;
            BLLNotaCredito bllNotaCredito = new BLLNotaCredito();
            long IdFactGlobal = 0;
            string strIdFactGlobal = String.Empty;
            DAOExtraccionPagos daoExtPagos = new DAOExtraccionPagos();
            int idfactpaqpago = 0;

            try
            {
                daoExtPagos = new DAOExtraccionPagos();

                bookingId = dtReserva.Rows[0]["BookingId"].ToString();
                //paymentId = dtPagos.Rows[0]["PaymentId"].ToString();

                // Busca la reserva en la BD
                dtConsultarReserva = daoVivaPaq.SeLocalizaReserva(bookingId);

                // La reservación ya está dada de alta, requiere modificar pagos
                if (dtConsultarReserva != null && dtConsultarReserva.Rows.Count > 0)
                {
                    if (dtFees.Rows.Count > 0)
                        dsResultado = daoExtPagos.ActualizarReserva(dtReserva, dtPagos, dtFees);
                    else
                    {
                        errDT.Columns.Add("ERROR", typeof(System.String));
                        DataRow newRow = errDT.NewRow();
                        newRow["ERROR"] = "La Tabla de Products no contiene registro";
                        errDT.Rows.Add(newRow);
                        dsResultado = new DataSet();
                        dsResultado.Tables.Add(errDT);
                        return dsResultado;
                    }

                    // ¿ El pago está facturado ?  / ¿Tiene Factura Global ?
                    if (dsResultado.Tables.Count > 1 && dsResultado.Tables[1].Columns.Contains("EsFacturado"))
                    {
                        foreach (DataRow row in dsResultado.Tables[3].Rows)
                        {
                            if (Convert.ToInt32(row["IdFactPaqClaveProdSer"].ToString()) == 2)
                            {
                                idfactpaqpago = int.Parse((row["IdFactPaqPagos"].ToString()));
                                break;
                            }
                        }
                        IdFactGlobal = string.IsNullOrEmpty(dsResultado.Tables[1].Select("IdFactPaqPagos = " + idfactpaqpago).CopyToDataTable().Rows[0]["IdFactPaqGlobal"].ToString()) ? 0 : long.Parse(dsResultado.Tables[1].Select("IdFactPaqPagos = " + idfactpaqpago).CopyToDataTable().Rows[0]["IdFactPaqGlobal"].ToString());
                        //IdFactGlobal = strIdFactGlobal == "" ? 0 : long.Parse(dsResultado.Tables[1].Rows[0]["IdFactGlobal"].ToString());
                        estaFacturado = string.IsNullOrEmpty(dsResultado.Tables[1].Select("IdFactPaqPagos = " + idfactpaqpago).CopyToDataTable().Rows[0]["EsFacturado"].ToString()) ? false : bool.Parse(dsResultado.Tables[1].Select("IdFactPaqPagos = " + idfactpaqpago).CopyToDataTable().Rows[0]["EsFacturado"].ToString());
                    }


                    if (dsResultado != null && dsResultado.Tables != null && dsResultado.Tables.Count > 0)
                    {
                        // Valida sí se produjó algún error en la actualización
                        if (dsResultado.Tables[0].Columns.Contains("Error"))
                            return dsResultado;

                        // Valida sí se generó NC
                        if (dsResultado.Tables[2].Columns.Contains("NOTA CREDITO") && dsResultado.Tables[2].Rows[0]["Serie"].ToString().Length < 1)
                            return dsResultado;
                        else if (estaFacturado || IdFactGlobal > 0)
                        {
                            pago.IdFactPaqReserva = int.Parse(dsResultado.Tables[2].Rows[0]["IdFactPaqReserva"].ToString());
                            pago.IdFactPaqPagos = idfactpaqpago;
                            pago.IdFactPaqNotaCredito = int.Parse(dsResultado.Tables[2].Rows[0]["IdFactPaqNotaCredito"].ToString());
                        }
                    }

                    // Si el pago ya está facturado se generará NC
                    if (estaFacturado || IdFactGlobal > 0)
                    {
                        //// Consultar datos del pago a generar la NC

                        paymentId = dsResultado.Tables[1].Select("IdFactPaqPagos = " + idfactpaqpago).CopyToDataTable().Rows[0]["PaymentId"].ToString();
                        dsResultado = new DataSet();
                        dsResultado = daoVivaPaq.ObtenerDatosParaGenerarNC(bookingId, paymentId);

                        if (dsResultado != null && dsResultado.Tables.Count > 4)
                        {
                            DataTable dtPago = dsResultado.Tables[1];
                            DataTable dtEmisor = dsResultado.Tables[3];
                            DataTable dtConceptos = dsResultado.Tables[5];

                            errDT.Columns.Add("ERROR", typeof(System.String));

                            if (dtPago.Rows.Count < 1)
                                strFaltaTablas += " *** No hay información de los Pagos *** ";
                            if (dtEmisor.Rows.Count < 1)
                                strFaltaTablas += " *** No hay información de Emisor *** ";
                            if (dtConceptos.Rows.Count < 1)
                                strFaltaTablas += " *** No hay información de Conceptos *** ";

                            DataRow newRow = errDT.NewRow();
                            newRow["ERROR"] = strFaltaTablas;
                            errDT.Rows.Add(newRow);

                            if (strFaltaTablas.Length > 0)
                            {
                                dsResultado = new DataSet();
                                dsResultado.Tables.Add(errDT);
                                return dsResultado;
                            }



                            //// Agregar los datos fiscales del Receptor
                            DatosFiscalesEgreso.RFC = dsResultado.Tables[0].Rows[0]["RFC"].ToString(); // "XAXX010101000";
                            DatosFiscalesEgreso.CodigoUsoCFDI = dsResultado.Tables[4].Rows[0]["CodigoUsoCFDI"].ToString(); //"P01";
                            DatosFiscalesEgreso.DescUsoCFDI = "P01 - Por definir.";

                            listDatosFiscales.Add(DatosFiscalesEgreso);
                            pago.LstDatosFiscales = listDatosFiscales;


                            pago.Serie = dtPago.Rows[0]["Serie"].ToString();
                            pago.NoFolio = long.Parse(dtPago.Rows[0]["NoFolio"].ToString());
                            pago.FechaEmision = dtPago.Rows[0]["FechaEmision"].ToString();
                            pago.CodigoFP = dtPago.Rows[0]["CodigoFP"].ToString();
                            pago.DescFP = dtPago.Rows[0]["DescFP"].ToString();
                            pago.CodigoM = dtPago.Rows[0]["CodigoM"].ToString();
                            pago.DescM = dtPago.Rows[0]["DescM"].ToString();
                            pago.CodigoMP = dtPago.Rows[0]["CodigoMP"].ToString();
                            pago.DescMP = dtPago.Rows[0]["DescMP"].ToString();
                            pago.SubTotalPago = decimal.Parse(dtPago.Rows[0]["SubTotalPago"].ToString());
                            pago.TotalPago = decimal.Parse(dtPago.Rows[0]["TotalPago"].ToString());
                            pago.DescuentoPago = decimal.Parse(dtPago.Rows[0]["DescuentoPago"].ToString());
                            pago.TipoCambio = decimal.Parse(dtPago.Rows[0]["TipoCambio"].ToString());
                            pago.CodigoTC = dtPago.Rows[0]["CodigoTC"].ToString();
                            pago.DescTC = dtPago.Rows[0]["DescTC"].ToString();
                            //pago.LugarExpedicion = dtPago.Rows[0]["LugarExpedicion"].ToString();--MASS
                            pago.PNR = dtPago.Rows[0]["PNR"].ToString();
                            pago.FechaPago = DateTime.Parse(dtPago.Rows[0]["FechaPago"].ToString());

                            pago.RFCEmisor = dtEmisor.Rows[0]["RFCEmisor"].ToString();
                            pago.CodigoRFEmisor = long.Parse(dtEmisor.Rows[0]["CodigoRFEmisor"].ToString());
                            pago.RazonSocialEmisor = dtEmisor.Rows[0]["RazonSocialEmisor"].ToString();

                            DAOConfiguracion daoConfiguracion = new DAOConfiguracion();
                            dtResultado = daoConfiguracion.ConsultarConfiguracion("VBPAQ");

                            datosGenerales.VersionCFDI = dtResultado.Select("Nombre = 'VersionCFDI'").CopyToDataTable().Rows[0]["valor"].ToString(); //"3.3";
                            datosGenerales.ApikeyPAC = "17320f4108d5b5f291109e8216a8d992"; // dtResultado.Select("Nombre = 'apikey'").CopyToDataTable().Rows[0]["valor"].ToString();
                            datosGenerales.CarpetaArchivosCFDI = dtResultado.Select("TipoParametro = 'ArchivosCFDI' and Nombre = 'CarpetaArchivosCFDI'").CopyToDataTable().Rows[0]["valor"].ToString();
                            datosGenerales.CarpetaNoProcesados = dtResultado.Select("TipoParametro = 'ArchivosCFDI' and Nombre = 'carpetaNoProcesados'").CopyToDataTable().Rows[0]["valor"].ToString();
                            datosGenerales.CarpetaPDF = dtResultado.Select("TipoParametro = 'ArchivosCFDI' and Nombre = 'carpetaPDF'").CopyToDataTable().Rows[0]["valor"].ToString();
                            datosGenerales.CarpetaXML = dtResultado.Select("TipoParametro = 'ArchivosCFDI' and Nombre = 'carpetaXML'").CopyToDataTable().Rows[0]["valor"].ToString();
                            listDatosGenerales.Add(datosGenerales);

                            pago.LstDatosGralDTO = listDatosGenerales;

                            pagoEgresosGlobal.UUID = dsResultado.Tables[2].Rows[0]["UUIDRelacionado"].ToString();
                            listEgresosGlobal.Add(pagoEgresosGlobal);
                            pago.LstPagosEgresosGlobal = listEgresosGlobal;


                            foreach (DataRow dr in dtConceptos.Rows)
                            {
                                concepto = new Conceptos();

                                concepto.CodigoProdSer = dr["CodigoProdSer"].ToString();
                                concepto.Cantidad = int.Parse(dr["Cantidad"].ToString());
                                concepto.ClaveUnidad = dr["ClaveUnidad"].ToString();
                                concepto.Unidad = dr["Unidad"].ToString();
                                concepto.DescProdSer = dr["DescProdSer"].ToString();
                                concepto.PrecioUnitario = decimal.Parse(dr["PrecioUnitario"].ToString());
                                concepto.Importe = decimal.Parse(dr["Importe"].ToString());
                                concepto.Impuesto = dr["Impuesto"].ToString();
                                concepto.TasaOCuota = decimal.Parse(dr["TasaOCuota"].ToString());
                                concepto.Factor = dr["Factor"].ToString();
                                concepto.ImporteTotal = decimal.Parse(dr["ImporteTotal"].ToString());
                                concepto.RFCTercero = dr["RFCTercero"].ToString();
                                concepto.BaseTraslado = decimal.Parse(dr["ImporteBase"].ToString());
                                concepto.DescImpuesto = dr["DescImpuesto"].ToString();

                                listConceptos.Add(concepto);
                            }
                            pago.LstConcepto = listConceptos;
                            lstPago.Add(pago);

                            //// Genera XML de Egreso
                            seGeneroCFDI = bllNotaCredito.GenerarCFDIEgreso(lstPago[0]);
                            if (!seGeneroCFDI)
                            {
                                lstPago[0].PNRError = true;
                            }
                        }
                        else
                            return dsResultado;
                    }

                }
                else
                {
                    errDT.Columns.Add("ERROR", typeof(System.String));
                    DataRow newRow = errDT.NewRow();
                    newRow["ERROR"] = "El PNR no está registrado en la BD, por lo que nose puede modificar";
                    errDT.Rows.Add(newRow);
                    dsResultado = new DataSet();
                    dsResultado.Tables.Add(errDT);
                    return dsResultado;
                }

            }
            catch (Exception ex)
            {
                dsResultado = new DataSet();
                errDT = new DataTable();
                errDT.Columns.Add("ERROR", typeof(System.String));
                DataRow newRow = errDT.NewRow();
                newRow["ERROR"] = ex.Message + " - " + ex.StackTrace;
                errDT.Rows.Add(newRow);

                dsResultado.Tables.Add(errDT);
            }

            return dsResultado;
        }

    }
}

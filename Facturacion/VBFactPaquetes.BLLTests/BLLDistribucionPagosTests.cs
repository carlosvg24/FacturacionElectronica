using Microsoft.VisualStudio.TestTools.UnitTesting;
using VBFactPaquetes.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using VBFactPaquetes.BLL.BDFacturacion;
using VBFactPaquetes.DAO;
using VBFactPaquetes.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VBFactPaquetes.Model.FacturacionBoletos;
using VBFactPaquetes.BLL.Facturacion;
using VBFactPaquetes.DAO.Facturacion;
using VBFactPaquetes.Model.Facturacion;
using VBFactPaquetes.Comun.Utilerias;
using VBFactPaquetes.Comun.Log;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using RestSharp;

namespace VBFactPaquetes.BLL.Tests
{
    [TestClass()]
    public class BLLDistribucionPagosTests
    {
        [TestMethod]
        public void AgregarReserva()
        {
            try
            {
                DataSet dsPaquete = new DataSet();
                DataSet dsResultado = new DataSet();
                string token = getAuthToken();
                Paquete paquete = new Paquete();

                BLLDistribucionPagos bll = new BLLDistribucionPagos();

                paquete = JsonConvert.DeserializeObject<Paquete>(getBookingByPNR(token, "MEREMX"));

                //string text = System.IO.File.ReadAllText(@"C:\Users\carlos.galindo\Desktop\Json_Pruebas\Reserva5.json");
                //paquete = Newtonsoft.Json.JsonConvert.DeserializeObject<Paquete>(text);
                //Almacena el paquete en una variable de sesión para consultas posteriores
                //System.Web.HttpContext.Current.Session["paquete"] = paquete;
                //Convertir el paquete a un DataSet

                dsResultado = bll.FromREST_ToDataSet(paquete);
                //Insertar en BD
                dsResultado = bll.Insertar(dsResultado.Tables["Reserva"], dsResultado.Tables["Fees"], dsResultado.Tables["Pagos"]);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [TestMethod()]
        public void ActualizaReserva()
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
            string pnr = "";
            int contpago = 0;
            //bool variospagos = false;

            BLLDistribucionPagos bll = new BLLDistribucionPagos();

            DataSet dsResultado = new DataSet();

            hayCancelacion = false;
            errorSQL = String.Empty;

            // Obtener valores del servico REST
            Order order = new Order();
            List<Payment> listPayments = new List<Payment>();
            List<Item> listItems = new List<Item>();

            Paquete paquete = new Paquete();
            string text = System.IO.File.ReadAllText(@"C:\Users\miguel.santes\Downloads\Reserva16.json");
            paquete = Newtonsoft.Json.JsonConvert.DeserializeObject<Paquete>(text);

            order = paquete.order;
            listItems = paquete.items;
            listPayments = paquete.payments;

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
            drReserva["Estatus"] = bll.getEstatus(order.status);
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
                if (p.state == 1)
                {
                    DataRow drPago = dtPagos.NewRow();
                    drPago["BookingId"] = order.bookingId;
                    drPago["PaymentId"] = p.paymentId;
                    drPago["PaymentMethodCode"] = "UI";
                    drPago["PaymentMethodType"] = "Credit Card";
                    drPago["PaymentAmount"] = p.amount;
                    drPago["ApprovalUTC"] = p.updated;
                    //.ToString("dd/MM/yyyy hh:mm:ss");
                    //drPago["lastFourDigits"] = p.lastFourDigits;
                    //drPago["holderName"] = p.holderName;
                    //drPago["transactionId"] = p.transactionId;
                    //drPago["currency"] = p.currency;
                    //drPago["state"] = p.state;
                    dtPagos.Rows.Add(drPago);
                    break;
                }
                contpago++;
            }

            /* MAPEAR Items y Asignar Valores*/
            dtFees.Columns.Add("BookingId", typeof(System.String));
            dtFees.Columns.Add("PaymentId", typeof(System.String));
            dtFees.Columns.Add("LineId", typeof(System.String));
            dtFees.Columns.Add("LineDateUTC", typeof(System.DateTime));
            dtFees.Columns.Add("LineCancelled", typeof(System.Boolean));
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
                drFee["PaymentId"] = listPayments[contpago].paymentId;
                drFee["LineDateUTC"] = listPayments[contpago].updated;//.ToString("dd/MM/yyyy hh:mm:ss");
                drFee["LineCancelledDate"] = i.status == 3 ? listPayments[contpago].updated : DateTime.MaxValue;
                drFee["LineId"] = i.itemId;
                drFee["LineCancelled"] = (i.status == 1 || i.status == 2) ? 0 : 1;
                drFee["ExternalReference"] = i.productType.referenceId;
                drFee["ProductTypeId"] = i.productType.productTypeId;
                drFee["ProductType"] = i.productType.productType;
                drFee["SellingPrice"] = i.sellingPrice;
                drFee["IVASellingPrice"] = i.tax;
                drFee["CurrencyCode"] = i.sellingCurrency;
                dtFees.Rows.Add(drFee);
            }
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
                hayCancelacion = bll.TieneCancelacíon(dtReserva, dtPagos, dtFees);

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
                    dsResultado = bll.Exchange(dtReserva, dtPagos, dtFees);

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
                        dsResultado = bll.Insertar(dtReserva, dtPagos, dtFees);
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

        [TestMethod()]
        public void FacturarBoleto()
        {
            DatosFacturacion datosFacturacion = new DatosFacturacion();
            RespuestaRESTFactura respuestaRESTFactura = new RespuestaRESTFactura();

            try
            {

                datosFacturacion.ClaveReservacion = "ZE68PN";
                datosFacturacion.UsoCFDI = "G03";
                datosFacturacion.EsExtranjero = false;
                datosFacturacion.RFCReceptor = "XAXX010101000";
                datosFacturacion.EmailReceptor = "daniel.vargas@vivaaerobus.com";
                datosFacturacion.Proceso = 1;

                respuestaRESTFactura = BLLFacturacionBoletos.FacturarBoletoAvion(datosFacturacion);
                if(respuestaRESTFactura.Codigo == 201)
                {

                }else if (respuestaRESTFactura.Codigo == 0)
                {

                }
            }
            catch (Exception ex)
            {

                throw;
            }

           
        }

        [TestMethod()]
        public void FacturarReserva()
        {
            #region DeclaracionVariables
            Pago pago = new Pago();
            DataSet dsResultado = new DataSet();
            DataTable dtResultado = new DataTable();
            String idReserva = String.Empty;
            String idsPagos = String.Empty;
            String resultado = String.Empty;
            List<Pago> listPagos = new List<Pago>();
            DAOFacturacion daoFacturacion = new DAOFacturacion();
            BLLFacturacionCliente bllFactCte = new BLLFacturacionCliente();

            DatosFiscales datosFiscales = new DatosFiscales();
            List<DatosFiscales> listDatosFiscales = new List<DatosFiscales>();
            DatosGenerales datosGenerales = new DatosGenerales();
            List<DatosGenerales> listDatosGenerales = new List<DatosGenerales>();
            DAOConfiguracion daoConf = new DAOConfiguracion();
            Conceptos concepto = new Conceptos();
            List<Conceptos> listCon = new List<Conceptos>();

            String boletoAvion = String.Empty;
            DatosFacturacion datosFacturacion = new DatosFacturacion();
            RespuestaRESTFactura respuestaRESTFactura = new RespuestaRESTFactura();
            #endregion

            try
            {
                idReserva = "HG58MOP1";
                idsPagos = "1,2";
                string[] arrayIdPagos = idsPagos.Split(',');
                bool seFacturoBoleto = false;
                String NoFolio = String.Empty;

                pago = new Pago();
                pago.PNR = idReserva;
                dsResultado = daoFacturacion.ConsultarPagosVivaPaquetes(pago);
                listPagos = Convertidor.ToList<Pago>(dsResultado.Tables[0]);

                foreach (string id in arrayIdPagos)
                {
                    boletoAvion = String.Empty;
                    pago = listPagos.Where(x => x.IdFactPaqPagos.ToString() == id).ToList().FirstOrDefault();
                    dtResultado = daoFacturacion.PagosFactura(pago, "UF");
                    pago.NoFolio = long.Parse(dtResultado.Rows[0]["NoFolio"].ToString());


                    listCon = Convertidor.ToList<Conceptos>(dsResultado.Tables[1]);
                    listCon = listCon.FindAll(x => x.IdFactPaqReserva == pago.IdFactPaqReserva && x.IdFactPaqPagos == pago.IdFactPaqPagos);
                    pago.LstConcepto = listCon;

                    foreach(Conceptos c in pago.LstConcepto)
                    {
                        if (c.PNRBoleto != null && c.PNRBoleto.Length > 0)
                            boletoAvion = c.PNRBoleto.Trim().ToUpper();
                    }

                    datosFiscales.CodigoUsoCFDI = "G03";
                    datosFiscales.DescUsoCFDI = "Gastos en general";
                    datosFiscales.RFC = "XAXX010101000";
                    datosFiscales.Email = "daniel.hernandez@enitma.com";
                    datosFiscales.Pais = "";
                    listDatosFiscales.Add(datosFiscales);
                    pago.LstDatosFiscales = listDatosFiscales;

                    dtResultado = daoConf.ConsultarConfiguracion("VBPAQ");
                    pago.RFCEmisor = dtResultado.Select("TipoParametro = 'Timbrado' and Nombre = 'RFCEmisor'").FirstOrDefault()["valor"].ToString();
                    pago.RazonSocialEmisor = dtResultado.Select("TipoParametro = 'Timbrado' and Nombre = 'RazonSocialEmisor'").FirstOrDefault()["valor"].ToString();
                    pago.CodigoRFEmisor = int.Parse(dtResultado.Select("TipoParametro = 'Timbrado' and Nombre = 'CodigoRFEmisor'").FirstOrDefault()["valor"].ToString());
                    pago.NoCertificado = dtResultado.Select("TipoParametro = 'Timbrado' and Nombre = 'NoCertificado'").FirstOrDefault()["valor"].ToString();

                    datosGenerales.VersionCFDI = dtResultado.Select("Nombre = 'VersionCFDI'").FirstOrDefault()["valor"].ToString(); //"3.3";
                    datosGenerales.ApikeyPAC = dtResultado.Select("Nombre = 'apikey'").FirstOrDefault()["valor"].ToString();
                    datosGenerales.CarpetaArchivosCFDI = dtResultado.Select("TipoParametro = 'ArchivosCFDI' and Nombre = 'CarpetaArchivosCFDI'").FirstOrDefault()["valor"].ToString();
                    datosGenerales.CarpetaNoProcesados = dtResultado.Select("TipoParametro = 'ArchivosCFDI' and Nombre = 'carpetaNoProcesados'").FirstOrDefault()["valor"].ToString();
                    datosGenerales.CarpetaPDF = dtResultado.Select("TipoParametro = 'ArchivosCFDI' and Nombre = 'carpetaPDF'").FirstOrDefault()["valor"].ToString();
                    datosGenerales.CarpetaXML = dtResultado.Select("TipoParametro = 'ArchivosCFDI' and Nombre = 'carpetaXML'").FirstOrDefault()["valor"].ToString();
                    listDatosGenerales.Add(datosGenerales);
                    pago.LstDatosGralDTO = listDatosGenerales;


                    if (boletoAvion.Length > 0)
                    {
                        // Busca el CFDI en la Base de Datos, sí previamente ya fue timbrado se extrae de la BD
                        dsResultado = daoFacturacion.CRUDFacturaBoleto(pago, "CI");
                        if (dsResultado != null && dsResultado.Tables != null && dsResultado.Tables.Count > 0
                                    && dsResultado.Tables[0] != null && dsResultado.Tables[0].Rows.Count > 0
                                    && dsResultado.Tables[0].Columns.Contains("ErrorProcedure"))
                        {
                            seFacturoBoleto = false;
                        }
                        else if (dsResultado != null && dsResultado.Tables != null && dsResultado.Tables.Count > 0
                                    && dsResultado.Tables[0] != null && dsResultado.Tables[0].Rows.Count > 0
                                    && dsResultado.Tables[0].Columns.Contains("ArchivoPDF"))
                        {
                            seFacturoBoleto = true;
                            pago.LstConcepto.Find(x => x.PNRBoleto.Length > 0).XML = dsResultado.Tables[0].Rows[0]["Xml"].ToString();
                            pago.LstConcepto.Find(x => x.PNRBoleto.Length > 0).PDF = (byte[])dsResultado.Tables[0].Rows[0]["ArchivoPDF"];
                            break;
                        }

                        datosFacturacion.ClaveReservacion = boletoAvion;
                        datosFacturacion.UsoCFDI = "G03";
                        datosFacturacion.EsExtranjero = false;
                        datosFacturacion.RFCReceptor = "XAXX010101000";
                        datosFacturacion.EmailReceptor = "daniel.hernandez@enitma.com";
                        datosFacturacion.Proceso = 1;

                        try
                        {
                            respuestaRESTFactura = BLLFacturacionBoletos.FacturarBoletoAvion(datosFacturacion);
                        }
                        catch (Excepciones ex)
                        {
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            Dictionary<string, object> parametros = new Dictionary<string, object>();
                            parametros.Add("pago", pago);
                            throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                                  MethodBase.GetCurrentMethod().Name,
                                                  parametros,
                                                  ex, Excepciones.TipoPortal.VivaPaquetes);

                        }

                        // Éxito
                        if (respuestaRESTFactura.Codigo == 210)
                        {
                            pago.LstConcepto.Find(x => x.PNRBoleto.Length > 0).XML = respuestaRESTFactura.XML;
                            pago.LstConcepto.Find(x => x.PNRBoleto.Length > 0).PDF = respuestaRESTFactura.PDF;
                            // Guardar XML y PDF en la BD
                            dsResultado = daoFacturacion.CRUDFacturaBoleto(pago, "A");

                            if (dsResultado != null && dsResultado.Tables != null && dsResultado.Tables.Count > 0
                                    && dsResultado.Tables[0] != null && dsResultado.Tables[0].Rows.Count > 0
                                    && dsResultado.Tables[0].Columns.Contains("ErrorProcedure"))
                            {
                                //DataTable errDT = new DataTable("Errores");
                                //errDT.Columns.Add("ERROR", typeof(System.String));
                                //DataRow newRow = errDT.NewRow();
                                //newRow["ERROR"] = "No hay Products en Reserva sin Cancelar";
                                //errDT.Rows.Add(newRow);
                                //dsResultado = new DataSet();
                                //dsResultado.Tables.Add(errDT);

                                seFacturoBoleto = false;

                                Dictionary<string, object> parametros = new Dictionary<string, object>();
                                parametros.Add("pago", pago);
                                parametros.Add("dtResultado", dsResultado.Tables[0]);
                                new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                                      MethodBase.GetCurrentMethod().Name,
                                                      parametros,
                                                      new Exception(dsResultado.Tables[0].Columns["ERORR"].ToString()),
                                                      Excepciones.TipoPortal.VivaPaquetes);
                            }
                            else
                                seFacturoBoleto = true;

                        }
                        // Errores controlados
                        else if (respuestaRESTFactura.Codigo == 0 || respuestaRESTFactura.Codigo >= 900)
                        {
                            seFacturoBoleto = false;
                            break;
                        }
                        // Errores de validación
                        else
                        {
                            seFacturoBoleto = false;
                            break;
                        }
                    }
                    else
                        resultado = bllFactCte.GenerarFactura(pago);
                }
                
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [TestMethod]
        public string getAuthToken()
        {
            var client = new RestClient("https://vivaaerobus.app.bidflyer.com/manage/login");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Cookie", "__cfduid=d97bf9b8cbf70305258e69072daea5e2a1620227808; PLAY_SESSION=eyJhbGciOiJIUzI1NiJ9.eyJkYXRhIjp7ImF1dGgiOiIyLU5pcFlwMUw0UEowclowZ3JZVDBseVBUd3hhRy9FT2lmWG1xNm1veGUzRUtTT0YzTGtPOEVCMnVuU3c1NTI1eEtMdVFBdmYxVDJiY2VTbHlOSDhucW00eEFJQU5oRk9kaUptNjVLVTRiaGFSeXlkWENGZlFiejNSb25rYTd3VUNIQXBKTGJxczUwZm1zOS9ibnZ2eW1PSlo0UnNLK0JQQXROQzArLytESXZoZFRHa0FmOTdOQm5XS2JiM1RPcWlrOXJpRm5zSlJMMENCOHlxaTgvZldvN2VFeFJoOHQ0ZjlTNFBVYkduNktZMS9aSkp5TkRBVT0lODZhNTUzYjdkODJiZmM0ODVkMjc3MGQyZGI0MzcwYzViNWFiMWU0NSIsInYiOiIyLVRRMUZFQzAreFVCMzNISW1xYlFBdVdSa3laM09CNy9iRXNlYkVCR1Q3cHkwZmpaemlNT3hRRTFoTXBwMEtMZnElMTU2MDVmMzc1ZjA3MGVmYjk3YTg3NzhkNTI4MWY2MzM1NWVhNGI2ZCJ9LCJleHAiOjE2NTM1OTUyOTcsIm5iZiI6MTYyMTQ1NDQ5NywiaWF0IjoxNjIxNDU0NDk3fQ.6hcGSMLPrak9AAaVJw-pPWxOw6Uti9dzM9rZvhWvig8");
            request.AddParameter("application/json", "{\"email\":\"carlos.vazquez@enitma.com\",\"password\":\"PwUBufEP\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            string xflyerValue = "";
            xflyerValue = response.Headers.Where(x => x.Name == "X-Bidflyer-Auth").First().Value.ToString();

            return xflyerValue;
        }

        [TestMethod()]
        public string getBookingByDate(string xflyerValue, string fecha)
        {
            string reservas = "";
            var client = new RestClient("https://vivaaerobus.app.bidflyer.com/manage/bookings/GetBookingsByDate/" + fecha);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-bidflyer-auth", xflyerValue);
            //request.AddHeader("Cookie", "__cfduid=d97bf9b8cbf70305258e69072daea5e2a1620227808; PLAY_SESSION=eyJhbGciOiJIUzI1NiJ9.eyJkYXRhIjp7ImF1dGgiOiIyLUpITEZyZXJmVXJnUTd2SzI1RGlQNGlRNHVNSGR2UnZUSm9JV3hpRUl4U20wYTdEZFd6aU9EZzdXaTU1dXU5bEZIRzdLYnBHbVp6UDJ3QU5JMjlNbkhqVVcyQ2pWVUpTRHIvamFQS21HdkVGVktXa2dlMUdIYi8vWFY3YVl2QWlWYS9UWVVtZlhSZ2Y5amhjYm44ZGNVRE1mM0N2QkFHaVJuUWFqWUcxeTQrS3V2OGxoc3dQQThOV0tmUFo3NEk3aGtYOTUvYnJFazNYVUx2SWhEWEFzdVhyVkE2WUExNXpHbzhpZFY0SUdlaEFSRTFDdHBxaz0lNjdlN2JhMTYzNDZhODY5ODZjNGM4MGMxNWZhZTAyZTc0OWM4ZjU2ZSIsInYiOiIyLVRRMUZFQzAreFVCMzNISW1xYlFBdVdSa3laM09CNy9iRXNlYkVCR1Q3cHkwZmpaemlNT3hRRTFoTXBwMEtMZnElMTU2MDVmMzc1ZjA3MGVmYjk3YTg3NzhkNTI4MWY2MzM1NWVhNGI2ZCJ9LCJleHAiOjE2NTQzNTg1MTYsIm5iZiI6MTYyMjIxNzcxNiwiaWF0IjoxNjIyMjE3NzE2fQ.yamkp2H44gikdHVl8f8rwtDTJrjH6RQJmqbLWjNB9Ck");
            IRestResponse response = client.Execute(request);

            reservas = response.Content;

            return reservas;
        }

        [TestMethod()]
        public string getBookingByID(string xflyerValue, string ID)
        {
            string jsonstr = "";
            var client = new RestClient("https://vivaaerobus.app.bidflyer.com/manage/bookings/GetBookingById/" + ID);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-bidflyer-auth", xflyerValue);
            //request.AddHeader("Cookie", "__cfduid=d97bf9b8cbf70305258e69072daea5e2a1620227808; PLAY_SESSION=eyJhbGciOiJIUzI1NiJ9.eyJkYXRhIjp7ImF1dGgiOiIyLUpITEZyZXJmVXJnUTd2SzI1RGlQNGlRNHVNSGR2UnZUSm9JV3hpRUl4U20wYTdEZFd6aU9EZzdXaTU1dXU5bEZIRzdLYnBHbVp6UDJ3QU5JMjlNbkhqVVcyQ2pWVUpTRHIvamFQS21HdkVGVktXa2dlMUdIYi8vWFY3YVl2QWlWYS9UWVVtZlhSZ2Y5amhjYm44ZGNVRE1mM0N2QkFHaVJuUWFqWUcxeTQrS3V2OGxoc3dQQThOV0tmUFo3NEk3aGtYOTUvYnJFazNYVUx2SWhEWEFzdVhyVkE2WUExNXpHbzhpZFY0SUdlaEFSRTFDdHBxaz0lNjdlN2JhMTYzNDZhODY5ODZjNGM4MGMxNWZhZTAyZTc0OWM4ZjU2ZSIsInYiOiIyLVRRMUZFQzAreFVCMzNISW1xYlFBdVdSa3laM09CNy9iRXNlYkVCR1Q3cHkwZmpaemlNT3hRRTFoTXBwMEtMZnElMTU2MDVmMzc1ZjA3MGVmYjk3YTg3NzhkNTI4MWY2MzM1NWVhNGI2ZCJ9LCJleHAiOjE2NTQzNTg1MTYsIm5iZiI6MTYyMjIxNzcxNiwiaWF0IjoxNjIyMjE3NzE2fQ.yamkp2H44gikdHVl8f8rwtDTJrjH6RQJmqbLWjNB9Ck");
            IRestResponse response = client.Execute(request);

            jsonstr = response.Content;

            return jsonstr;
        }

        public string getBookingByPNR(string xflyerValue, string PNR)
        {
            string jsonstr = "";
            var client = new RestClient("https://vivaaerobus.app.bidflyer.com/manage/bookings/GetBookingByPNR/" + PNR);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-bidflyer-auth", xflyerValue);
            IRestResponse response = client.Execute(request);
            
            return jsonstr;
        }
    }
}
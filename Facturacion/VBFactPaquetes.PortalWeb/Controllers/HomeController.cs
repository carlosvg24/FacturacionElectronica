using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VBFactPaquetes.BLL;
using VBFactPaquetes.BLL.BDFacturacion;
using VBFactPaquetes.Model;
using System.Data;
using VBFactPaquetes.Model.PantallaFacturacion;
using VBFactPaquetes.Comun.Log;
using VBFactPaquetes.Comun.Utilerias;
using System.Reflection;
using VBFactPaquetes.DAO.Facturacion;
using VBFactPaquetes.Model.Facturacion;
using VBFactPaquetes.BLL.Facturacion;
using VBFactPaquetes.DAO;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using VBFactPaquetes.Model.FacturacionBoletos;
using RestSharp;
using System.Threading;

namespace VBFactPaquetes.PortalWeb.Controllers
{
    public class HomeController : Controller
    {
        Pago pago = new Pago();
        private DataSet dsReserva = new DataSet();
        private Paquete paquete = new Paquete();
        //private DataTable dtResultado;
        private DAOFacturacion daoFacturacion = new DAOFacturacion();
        private List<ReservaPorFacturar> lstReservaModel = new List<ReservaPorFacturar>();

        [HttpGet]
        public ActionResult Index()
        {

            //pago = new ReservaPorFacturar();
            ReservaPorFacturar reservaporfactura = new ReservaPorFacturar();

            try
            {

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(reservaporfactura);
        }

        [HttpPost]
        public ActionResult Index(ReservaPorFacturar reservaporfactura)
        {

            #region DeclaracionVariables
            int PNRError = 1;
            int idPagoBoletoAvion = 0;
            String idReserva = String.Empty;
            String idsPagos = String.Empty;
            String resultado = String.Empty;
            String boletoAvion = String.Empty;
            String NoFolio = String.Empty;
            bool seFacturoBoleto = false;
            DataSet dsResultado = new DataSet();
            DataTable dtResultado = new DataTable();
            //Pago pago = new Pago();
            DatosFiscales datosFiscales = new DatosFiscales();
            DatosGenerales datosGenerales = new DatosGenerales();
            DatosFacturacion datosFacturacion = new DatosFacturacion();
            RespuestaRESTFactura respuestaRESTFactura = new RespuestaRESTFactura();
            List<Pago> listPagos = new List<Pago>();
            List<DatosFiscales> listDatosFiscales = new List<DatosFiscales>();
            List<DatosGenerales> listDatosGenerales = new List<DatosGenerales>();
            List<Conceptos> listCon = new List<Conceptos>();
            BLLFacturacionCliente bllFactCte = new BLLFacturacionCliente();
            DAOFacturacion daoFacturacion = new DAOFacturacion();
            DAOConfiguracion daoConf = new DAOConfiguracion();
            #endregion

            try
            {
                if (reservaporfactura.AccionBoton == "Buscar")
                {
                    pago.PNR = reservaporfactura.PNR;
                    AgregarReserva();
                    /*Drop Down List Descripción Uso CFDI Fijo*/
                    List<SelectListItem> lstUSOCFDI = new List<SelectListItem>();
                    lstUSOCFDI.Add(new SelectListItem() { Text = "Gastos en general", Value = "G03" });
                    lstUSOCFDI.Add(new SelectListItem() { Text = "Por definir", Value = "P01" });
                    reservaporfactura.listDescUsoCFDI = lstUSOCFDI;

                    /*Drop Down List Codigo Pais fijo*/
                    List<SelectListItem> lstcodigopais = new List<SelectListItem>();
                    lstcodigopais.Add(new SelectListItem() { Text = "México", Value = "MX" });
                    lstcodigopais.Add(new SelectListItem() { Text = "Estados Unidos", Value = "US" });
                    reservaporfactura.listCodigoPais = lstcodigopais;


                    dsResultado = daoFacturacion.ConsultarPagosVivaPaquetes(pago);

                    if (dsResultado != null && dsResultado.Tables.Count > 0 && dsResultado.Tables[0].Rows.Count > 0 && dsResultado.Tables[1].Rows.Count > 0)
                    {
                        DataTable TablePagosFacturables = dsResultado.Tables[0];
                        DataTable TablePagosDetalle = dsResultado.Tables[1];

                        for (int i = 0; i < TablePagosFacturables.Rows.Count; i++)
                        {
                            PagoPorFacturar pagoPorFacturar = new PagoPorFacturar();

                            pagoPorFacturar.IdFactReserva = Convert.ToInt64(TablePagosFacturables.Rows[i]["IdFactPaqReserva"]);
                            pagoPorFacturar.IdFactPagos = Convert.ToInt32(TablePagosFacturables.Rows[i]["IdFactPaqPagos"]);
                            pagoPorFacturar.FechaPago = Convert.ToDateTime(TablePagosFacturables.Rows[i]["FechaPago"]);
                            pagoPorFacturar.FormaPago = Convert.ToString(TablePagosFacturables.Rows[i]["DescFP"]);
                            pagoPorFacturar.NoFolio = Convert.ToString(TablePagosFacturables.Rows[i]["NoFolio"]);
                            //pagoPorFacturar.FechaFacturacion = Convert.ToDateTime(TablePagosFacturables.Rows[i]["FechaFacturacion"]);
                            pagoPorFacturar.Moneda = Convert.ToString(TablePagosFacturables.Rows[i]["CodigoM"]);
                            pagoPorFacturar.Total = Convert.ToDecimal(TablePagosFacturables.Rows[i]["TotalPago"]);
                            pagoPorFacturar.LugarExpedicion = Convert.ToString(TablePagosFacturables.Rows[i]["LugarExpedicion"]);
                            pagoPorFacturar.EsFacturado = Convert.ToInt32(TablePagosFacturables.Rows[i]["EsFacturado"]);
                            pagoPorFacturar.SeMandaraFacturar = pagoPorFacturar.EsFacturado != 0 ? false : true;
                            reservaporfactura.listPagos.Add(pagoPorFacturar);
                        }

                        ViewBag.Reserva = reservaporfactura;
                    }
                    else

                        //ModelState.AddModelError("", Recurso.Textos.PNRNoLocalizado);
                        ViewBag.Error = PNRError;
                }

                else if (reservaporfactura.AccionBoton == "GenerarFactura")
                {
                    /*Acción que se realizara con la información proveniente de la vista al precionar el boton Generar Factura*/
                    idReserva = Session["IdReserva"].ToString();
                    idsPagos = Session["IdsPagos"].ToString();
                    string[] arrayIdPagos = idsPagos.Split(',');


                    pago = new Pago();
                    pago.PNR = idReserva;
                    dsResultado = daoFacturacion.ConsultarPagosVivaPaquetes(pago);
                    listPagos = Convertidor.ToList<Pago>(dsResultado.Tables[0]);

                    /* PRIMERO FACTURA EL BOLETO DE AVIÓN */
                    #region Facturacion de Boleto Avion
                    boletoAvion = String.Empty;

                    //pago = listPagos.Where(x => x.IdFactPaqPagos.ToString() == "1").ToList().FirstOrDefault();
                    foreach (Pago p in listPagos)
                    {
                        foreach (Conceptos c in p.LstConcepto)
                        {
                            if (c.PNRBoleto != null && c.PNRBoleto.Length > 0)
                            {
                                idPagoBoletoAvion = c.IdFactPaqPagos;
                                boletoAvion = c.PNRBoleto.Trim().ToUpper();
                            }

                        }
                    }

                    // Utiliza sólo el pago que tiene el boleto de avión
                    pago = listPagos.Where(x => x.IdFactPaqPagos == idPagoBoletoAvion).ToList().FirstOrDefault();
                    // Quita el pago que tiene el boleto de avión
                    listPagos.Remove(pago);
                    arrayIdPagos = arrayIdPagos.Where(n => n != idPagoBoletoAvion.ToString()).ToArray();

                    // Busca el CFDI en la Base de Datos, sí previamente ya fue timbrado se extrae de la BD
                    dsResultado = daoFacturacion.CRUDFacturaBoleto(pago, "CI");
                    if (dsResultado != null && dsResultado.Tables != null && dsResultado.Tables.Count > 0
                                && dsResultado.Tables[0] != null && dsResultado.Tables[0].Rows.Count > 0
                                && dsResultado.Tables[0].Columns.Contains("ErrorProcedure"))
                    {
                        seFacturoBoleto = false;
                        //break;
                    }
                    else if (dsResultado != null && dsResultado.Tables != null && dsResultado.Tables.Count > 0
                                && dsResultado.Tables[0] != null && dsResultado.Tables[0].Rows.Count > 0
                                && dsResultado.Tables[0].Columns.Contains("ArchivoPDF"))
                    {
                        seFacturoBoleto = true;
                        pago.LstConcepto.Find(x => x.PNRBoleto.Length > 0).XML = dsResultado.Tables[0].Rows[0]["Xml"].ToString();
                        pago.LstConcepto.Find(x => x.PNRBoleto.Length > 0).PDF = (byte[])dsResultado.Tables[0].Rows[0]["ArchivoPDF"];

                    }

                    datosFacturacion.ClaveReservacion = boletoAvion;
                    datosFacturacion.UsoCFDI = Session["codigoUsoCfdi"].ToString();
                    datosFacturacion.EsExtranjero = false;
                    datosFacturacion.RFCReceptor = Session["rfc"].ToString();
                    datosFacturacion.EmailReceptor = Session["email"].ToString();
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
                            //break;
                        }
                        else
                            seFacturoBoleto = true;

                    }
                    // Errores controlados
                    else if (respuestaRESTFactura.Codigo == 0 || respuestaRESTFactura.Codigo >= 900)
                    {
                        seFacturoBoleto = false;
                        //break;
                    }
                    // Errores de validación
                    else
                    {
                        seFacturoBoleto = false;
                        //break;
                    }


                    #endregion

                    #region Facturacion de Paquete
                    if (seFacturoBoleto)
                    {
                        foreach (string id in arrayIdPagos)
                        {
                            pago = listPagos.Where(x => x.IdFactPaqPagos.ToString() == id).ToList().FirstOrDefault();
                            dtResultado = daoFacturacion.PagosFactura(pago, "UF");
                            pago.NoFolio = long.Parse(dtResultado.Rows[0]["NoFolio"].ToString());


                            listCon = Convertidor.ToList<Conceptos>(dsResultado.Tables[1]);
                            listCon = listCon.FindAll(x => x.IdFactPaqReserva == pago.IdFactPaqReserva && x.IdFactPaqPagos == pago.IdFactPaqPagos);
                            pago.LstConcepto = listCon;


                            datosFiscales.CodigoUsoCFDI = Session["codigoUsoCfdi"].ToString();
                            datosFiscales.DescUsoCFDI = Session["descUsoCfdi"].ToString();
                            datosFiscales.RFC = Session["rfc"].ToString();
                            datosFiscales.Email = Session["email"].ToString();
                            datosFiscales.Pais = Session["descPais"].ToString();
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

                            resultado = bllFactCte.GenerarFactura(pago);
                        }
                    }
                    #endregion

                    /*Se retorna al index al precionar el boton Generar Factura para evitar error.*/
                    return RedirectToAction("Index");
                }
            }


            catch (Excepciones ex)
            {
                ViewBag.Error = 1;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("reservaporfactura", reservaporfactura);
                new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                                      MethodBase.GetCurrentMethod().Name,
                                                      parametros,
                                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }

            return View("Index", reservaporfactura);
        }


        [HttpPost]
        public ActionResult SetViewBag(string idReserva, string idsPagos, string email, string rfc, string codigoUsoCfdi, string descUsoCfdi, string taxId, string codigoPais, string descPais)
        {
            Session["IdReserva"] = idReserva;
            Session["IdsPagos"] = idsPagos;
            Session["email"] = email;
            Session["rfc"] = rfc;
            Session["codigoUsoCfdi"] = codigoUsoCfdi;
            Session["descUsoCfdi"] = descUsoCfdi;
            Session["taxId"] = taxId;
            Session["codigoPais"] = codigoPais;
            Session["descPais"] = descPais;

            return new EmptyResult();
        }


        public ActionResult DescargarFactura(long IdReserva, int IdPago)
        {
            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);
            StringBuilder xml = new StringBuilder();
            DAOFacturacion daoFacturacion = new DAOFacturacion();
            Pago pago = new Pago();
            DataTable dt = new DataTable();

            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
            byte[] bytesxml = null;
            byte[] bytespdf = null;

            try
            {
                pago.IdFactPaqReserva = IdReserva;
                pago.IdFactPaqPagos = IdPago;
                dt = daoFacturacion.ObtenerFacturasComprobante(pago);

                if (dt.Rows.Count > 0)
                {
                    ZipEntry xmlEntry = new ZipEntry(dt.Rows[0]["PNRPaquete"].ToString() + "_" +
                                                     dt.Rows[0]["SeriePaquete"].ToString() + dt.Rows[0]["NoFolioPaquete"].ToString() + ".xml");
                    xmlEntry.DateTime = DateTime.Now;
                    zipStream.PutNextEntry(xmlEntry);
                    bytesxml = Encoding.UTF8.GetBytes(dt.Rows[0]["XmlPaquete"].ToString());

                    MemoryStream stream = new MemoryStream(bytesxml);
                    StreamUtils.Copy(stream, zipStream, bytesxml);
                    stream.Close();


                    ZipEntry pdfEntry = new ZipEntry(dt.Rows[0]["PNRPaquete"].ToString() + "_" +
                                                     dt.Rows[0]["SeriePaquete"].ToString() + dt.Rows[0]["NoFolioPaquete"].ToString() + ".pdf");
                    pdfEntry.DateTime = DateTime.Now;
                    zipStream.PutNextEntry(pdfEntry);

                    bytespdf = (byte[])dt.Rows[0]["PDFPaquete"];
                    stream = new MemoryStream(bytespdf);
                    StreamUtils.Copy(stream, zipStream, bytespdf);
                    stream.Close();

                    zipStream.CloseEntry();
                    zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.
                    zipStream.Close();

                    outputMemStream.Position = 0;

                    return File(outputMemStream.ToArray(), "application/octet-stream", dt.Rows[0]["PNRPaquete"].ToString() + "_" +
                                                     dt.Rows[0]["SeriePaquete"].ToString() + dt.Rows[0]["NoFolioPaquete"].ToString() + ".zip");
                }
            }
            catch (Excepciones ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("Index");
        }

        //CVG
        public void AgregarReserva()
        {
            try
            {
                DataSet dsPaquete = new DataSet();
                DataSet dsResultado = new DataSet();
                string token = getAuthToken();
                Paquete paquete = new Paquete();

                BLLDistribucionPagos bll = new BLLDistribucionPagos();

                paquete = JsonConvert.DeserializeObject<Paquete>(getBookingByID(token, pago.PNR));

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

        [HttpGet]
        public string getAuthToken()
        {
            int intentos = 0;
            bool exito = false;
            string xflyerValue = "";
            while (intentos <= 5 && exito == false)
            {
                try
                {
                    var client = new RestClient("https://vivaaerobus.app.bidflyer.com/manage/login");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    //request.AddHeader("Cookie", "__cfduid=d97bf9b8cbf70305258e69072daea5e2a1620227808; PLAY_SESSION=eyJhbGciOiJIUzI1NiJ9.eyJkYXRhIjp7ImF1dGgiOiIyLU5pcFlwMUw0UEowclowZ3JZVDBseVBUd3hhRy9FT2lmWG1xNm1veGUzRUtTT0YzTGtPOEVCMnVuU3c1NTI1eEtMdVFBdmYxVDJiY2VTbHlOSDhucW00eEFJQU5oRk9kaUptNjVLVTRiaGFSeXlkWENGZlFiejNSb25rYTd3VUNIQXBKTGJxczUwZm1zOS9ibnZ2eW1PSlo0UnNLK0JQQXROQzArLytESXZoZFRHa0FmOTdOQm5XS2JiM1RPcWlrOXJpRm5zSlJMMENCOHlxaTgvZldvN2VFeFJoOHQ0ZjlTNFBVYkduNktZMS9aSkp5TkRBVT0lODZhNTUzYjdkODJiZmM0ODVkMjc3MGQyZGI0MzcwYzViNWFiMWU0NSIsInYiOiIyLVRRMUZFQzAreFVCMzNISW1xYlFBdVdSa3laM09CNy9iRXNlYkVCR1Q3cHkwZmpaemlNT3hRRTFoTXBwMEtMZnElMTU2MDVmMzc1ZjA3MGVmYjk3YTg3NzhkNTI4MWY2MzM1NWVhNGI2ZCJ9LCJleHAiOjE2NTM1OTUyOTcsIm5iZiI6MTYyMTQ1NDQ5NywiaWF0IjoxNjIxNDU0NDk3fQ.6hcGSMLPrak9AAaVJw-pPWxOw6Uti9dzM9rZvhWvig8");
                    request.AddParameter("application/json", "{\"email\":\"carlos.vazquez@enitma.com\",\"password\":\"PwUBufEP\"}", ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    xflyerValue = response.Headers.Where(x => x.Name == "X-Bidflyer-Auth").First().Value.ToString();
                    exito = true;
                }
                catch (Exception ex)
                {
                    //if (ex.Message.Contains("bad request"))
                    //{
                        intentos++;
                        Thread.Sleep(10000);
                    //}
                }
            }
            return xflyerValue;
        }

        [HttpGet]
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

        public string getBookingByPNR(string xflyerValue, string PNR)
        {
            string jsonstr = "";
            var client = new RestClient("https://vivaaerobus.app.bidflyer.com/manage/bookings/GetBookingByPNR/" + PNR);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-bidflyer-auth", xflyerValue);
            IRestResponse response = client.Execute(request);
            jsonstr = response.Content;
            return jsonstr;
        }

        [HttpGet]
        public string getBookingByID(string xflyerValue, string ID)
        {
            int intentos = 0;
            bool exito = false;
            string jsonstr = "";
            while (intentos <= 5 && exito == false)
            {
                try
                {
                    var client = new RestClient("https://vivaaerobus.app.bidflyer.com/manage/bookings/GetBookingById/" + ID);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("x-bidflyer-auth", xflyerValue);
                    //request.AddHeader("Cookie", "__cfduid=d97bf9b8cbf70305258e69072daea5e2a1620227808; PLAY_SESSION=eyJhbGciOiJIUzI1NiJ9.eyJkYXRhIjp7ImF1dGgiOiIyLUpITEZyZXJmVXJnUTd2SzI1RGlQNGlRNHVNSGR2UnZUSm9JV3hpRUl4U20wYTdEZFd6aU9EZzdXaTU1dXU5bEZIRzdLYnBHbVp6UDJ3QU5JMjlNbkhqVVcyQ2pWVUpTRHIvamFQS21HdkVGVktXa2dlMUdIYi8vWFY3YVl2QWlWYS9UWVVtZlhSZ2Y5amhjYm44ZGNVRE1mM0N2QkFHaVJuUWFqWUcxeTQrS3V2OGxoc3dQQThOV0tmUFo3NEk3aGtYOTUvYnJFazNYVUx2SWhEWEFzdVhyVkE2WUExNXpHbzhpZFY0SUdlaEFSRTFDdHBxaz0lNjdlN2JhMTYzNDZhODY5ODZjNGM4MGMxNWZhZTAyZTc0OWM4ZjU2ZSIsInYiOiIyLVRRMUZFQzAreFVCMzNISW1xYlFBdVdSa3laM09CNy9iRXNlYkVCR1Q3cHkwZmpaemlNT3hRRTFoTXBwMEtMZnElMTU2MDVmMzc1ZjA3MGVmYjk3YTg3NzhkNTI4MWY2MzM1NWVhNGI2ZCJ9LCJleHAiOjE2NTQzNTg1MTYsIm5iZiI6MTYyMjIxNzcxNiwiaWF0IjoxNjIyMjE3NzE2fQ.yamkp2H44gikdHVl8f8rwtDTJrjH6RQJmqbLWjNB9Ck");
                    IRestResponse response = client.Execute(request);

                    jsonstr = response.Content;
                    exito = true;
                }
                catch (Exception ex)
                {
                    //if (ex.Message.Contains("bad request"))
                    //{
                        intentos++;
                        Thread.Sleep(10000);
                    //}
                }
            }
            return jsonstr;
        }

        //public String detalle_seccion(string PNR)
        //{
        //    string html = "";
        //    String DetalleContenido = "";
        //    Pago pago = new Pago();
        //    DataSet dsdetalle = new DataSet();
        //    ReservaPorFacturar reservaporfactura = new ReservaPorFacturar();

        //    pago.PNR = PNR;
        //    dsdetalle = daoFacturacion.DetalleSeccion(pago);

        //    DataTable TablePagosDetalle = dsdetalle.Tables[1];

        //    if (TablePagosDetalle.Rows.Count > 0)
        //    {
        //        int DetallePago = Convert.ToInt32(TablePagosDetalle.Rows.Count) - 1;
        //        for (int i = 0; i <= DetallePago; i++)
        //        {

        //            String DescProdSer = Convert.ToString(TablePagosDetalle.Rows[i]["DescProdSer"].ToString());
        //            String ClaveUnidad = Convert.ToString(TablePagosDetalle.Rows[i]["ClaveUnidad"].ToString());
        //            String Moneda = "";
        //            String Importe = Convert.ToString(TablePagosDetalle.Rows[i]["Importe"].ToString());

        //             DetalleContenido = DetalleContenido +
        //                "<tr><td align='center'>" + DescProdSer + "</td>" +
        //                "<td align='center'>" + ClaveUnidad + "</td>" +
        //                "<td align='center'>" + Moneda + "</td>" +
        //                "<td align='center'>" + Importe + "</td> </tr>";
        //        }
        //    }

        //    return DetalleContenido;
        //}

    }
}
//Danys

using Facturacion.BLL;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.ENT;
using Comun.Utils;
using Facturacion.ENT.Portal.Facturacion;
using Facturacion.ENT.ProcesoFacturacion;
using Facturacion.ENT.RestFul;
using REST.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.IO;
using Ionic.Zip;
using Newtonsoft.Json.Linq;
using System.Web;
using Comun.Security;
using Facturacion.InterfaceRestViva;

namespace REST.Services.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/facturacion")]
    public class FacturacionController : ApiController
    {
        #region Metodos Publicos
        [HttpGet]
        //[Route("get")]
        public string Get()
        {
            try
            {
                return "okey";
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }

        [HttpPost]
        [ResponseType(typeof(List<ENTPagosPorFacturar>))]
        public IHttpActionResult Post(DatosParaFacturar datosParaFacturar)
        {
            try
            {
                /*
                 {
                    "UsoCFDI": "123",
                    "EsExtranjero": false,
                    "RFCReceptor": "HEVD830710",
                    "ResidenciaFiscal": "MX",
                    "TAXid": "PY9",
                    "EmailReceptor": "daniel.vargas@vivaaerobus.com",
                    "NombrePasajero": "Daniel",
                    "ApellidoPasajero": "Hernandez",
                    "PNR": "PNR001",
                    "Password": "wij832nd3i"
                    }
                 */
                List<string> listaPNRFactura = new List<string>();
                List<ENTPagosPorFacturar> listaPagos = new List<ENTPagosPorFacturar>();
                List<string> listaPNRError = new List<string>();

                try
                {
                    ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();
                    datosCliente.UsoCFDI = datosParaFacturar.UsoCFDI;
                    datosCliente.EsExtranjero = datosParaFacturar.EsExtranjero;
                    datosCliente.RFCReceptor = datosParaFacturar.RFCReceptor;
                    datosCliente.PaisResidenciaFiscal = datosParaFacturar.PaisResidenciaFiscal;
                    datosCliente.TAXID = datosParaFacturar.TAXID;
                    datosCliente.EmailReceptor = datosParaFacturar.EmailReceptor;
                    datosCliente.NombrePasajero = datosParaFacturar.NombrePasajero;
                    datosCliente.ApellidosPasajero = datosParaFacturar.ApellidosPasajero;
                    datosCliente.ClaveReservacion = datosParaFacturar.ClaveReservacion;

                    BLLFacturacion bllFacturacion = new BLLFacturacion();
                    //listaPagos = bllFacturacion.RecuperarPagosParaFacturar(datosCliente);

                    //Se simulan los pagos seleccionados
                    foreach (var item in listaPagos)
                    {
                        if (item.EsFacturable == true && item.EsFacturado == false)
                        {
                            item.EstaMarcadoParaFacturacion = true;
                        }
                    }
                    if (listaPagos.Where(x => x.EstaMarcadoParaFacturacion == true).Count() > 0)
                    {
                        try
                        {
                            //listaPagos = bllFacturacion.GenerarFacturaCliente(datosCliente, listaPagos);
                        }
                        catch (Exception ex)
                        {

                            listaPNRError.Add(datosParaFacturar.ClaveReservacion);
                        }

                    }
                    Console.WriteLine("Concluyo proceso facturacion PNR:" + datosParaFacturar.ClaveReservacion);
                }
                catch (Exception ex)
                {
                    listaPNRError.Add(datosParaFacturar.ClaveReservacion);
                }

                return Ok(listaPagos);
            }
            catch (Exception ex)
            {

                throw new Exception("Error: " + ex.Message);
            }
        }


        [HttpPost]
        [ResponseType(typeof(List<ENTPagosSinFacturar>))]
        public IHttpActionResult BuscarPagosPorPNRDeTrafico(ENTDatosFacturacionPorRest datosFacturacion)
        {

            List<ENTPagosSinFacturar> listaResult = new List<ENTPagosSinFacturar>();
            try
            {
                if (ValidarUsuarioFacturacion(datosFacturacion.Usuario, datosFacturacion.Password))
                {
                    List<string> listaPNRFactura = new List<string>();
                    List<ENTPagosPorFacturar> listaPagos = new List<ENTPagosPorFacturar>();



                    ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();


                    //code s
                    
                    //code e
                    datosCliente.ClaveReservacion = datosFacturacion.ClaveReservacion;


                    //code s

                    
                    //code e

                    //Recuperar el parametro de los organizationCode de trafico 
                    List<string> listaOrgCode = new List<string>();
                    listaOrgCode = RecuperarListaOrgCode("RESTOrgCodeTraficoKiosko");

                    //Recupera la informacion de los pagos que aun no estan facturados
                    BLLFacturacion bllFacturacion = new BLLFacturacion();
                    //listaPagos = bllFacturacion.RecuperarPagosParaFacturar(datosCliente);

                    List<ENTPagosPorFacturar> listaPagosSel = new List<ENTPagosPorFacturar>();

                    
                    //Se identifican los pagos que pueden ser facturados
                    foreach (var pago in listaPagos)
                    {

                        //code s



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

                        //code e

                        //Valida si el pago corresponde a la organizacion asignada por parametro
                        string deptOrg = pago.OrganizationCode != null ? pago.OrganizationCode : "";
                        //Verifica si el organization code esta habilitado o si estan permitidos todos("*")
                        if ((listaOrgCode.Count == 1 && listaOrgCode.Contains("*")) || (listaOrgCode.Count > 0 && listaOrgCode.Contains(deptOrg)))
                        {
                            //En caso de pertenecer a la organizacion Code entonces se permite incluir el pago
                            if (pago.EsFacturable == true && pago.EsFacturado == false && pago.EnVigenciaParaFacturacion == true)
                            {
                                ENTPagosSinFacturar entPagoSinFac = new ENTPagosSinFacturar();
                                entPagoSinFac.IdPagosCab = pago.IdPagosCab;
                                entPagoSinFac.BookingID = pago.BookingID;
                                entPagoSinFac.PaymentID = pago.PaymentID;
                                entPagoSinFac.MontoTotal = pago.MontoTotal;
                                entPagoSinFac.FolioPrefactura = pago.FolioPrefactura;
                                entPagoSinFac.EnVigenciaParaFacturacion = pago.EnVigenciaParaFacturacion;
                                entPagoSinFac.EstaMarcadoParaFacturacion = true;
                                listaResult.Add(entPagoSinFac);

                            }
                        }


                        //code s
                        
                        //code e
                    } //termina for each



                    //code s
                    
                    //code e


                    return Ok(listaResult);
                    // return Ok(new RespuestaRESTFactura(200, "OK", "http://facturacion.vivaaerobus.com:8011/Archivos/04-09-2019/TEST12/CFDI_TEST34.zip"));

                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Usuario no autorizado para consumir el metodo."));
                }

            }
            catch (Exception ex)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }


            //code s
         
            //code e
        }



        [HttpPost]
        [ResponseType(typeof(List<ENTPagosFacturadosREST>))]
        public IHttpActionResult GenerarFacturasDeTrafico(ENTDatosFacturacionPorRest datosFacturacion)
        {
            List<ENTPagosFacturadosREST> listaResult = new List<ENTPagosFacturadosREST>();
            try
            {
                if (ValidarUsuarioFacturacion(datosFacturacion.Usuario, datosFacturacion.Password))
                {

                    List<string> listaPNRFactura = new List<string>();
                    List<ENTPagosPorFacturar> listaPagos = new List<ENTPagosPorFacturar>();

                    ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();
                    datosCliente.UsoCFDI = datosFacturacion.UsoCFDI;
                    datosCliente.EsExtranjero = datosFacturacion.EsExtranjero;
                    datosCliente.RFCReceptor = datosFacturacion.RFCReceptor;
                    datosCliente.PaisResidenciaFiscal = datosFacturacion.PaisResidenciaFiscal;
                    datosCliente.TAXID = datosFacturacion.TAXID;
                    datosCliente.EmailReceptor = datosFacturacion.EmailReceptor;
                    datosCliente.NombrePasajero = datosFacturacion.NombrePasajero;
                    datosCliente.ApellidosPasajero = datosFacturacion.ApellidosPasajero;
                    datosCliente.ClaveReservacion = datosFacturacion.ClaveReservacion;

                    if (datosFacturacion.Pagos != null && datosFacturacion.Pagos.Count == 0)
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error. No se tienen pagos seleccionados por facturar..."));
                    }

                    //Recupera la informacion de los pagos que aun no estan facturados
                    BLLFacturacion bllFacturacion = new BLLFacturacion();
                    //listaPagos = bllFacturacion.RecuperarPagosParaFacturar(datosCliente);

                    List<ENTPagosPorFacturar> listaPagosSel = new List<ENTPagosPorFacturar>();
                    //Se identifican los pagos que pueden ser facturados
                    foreach (var pago in listaPagos)
                    {

                        if (datosFacturacion.Pagos.Where(x => x.IdPagosCab == pago.IdPagosCab && x.EstaMarcadoParaFacturacion == true).Count() > 0)
                        {
                            pago.EstaMarcadoParaFacturacion = true;
                            listaPagosSel.Add(pago);
                        }

                    }

                    if (listaPagosSel.Count() > 0)
                    {
                        try
                        {
                            //listaPagosSel = bllFacturacion.GenerarFacturaCliente(datosCliente, listaPagosSel);

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
                                    listaResult.Add(pagoFactRes);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
                        }
                    }
                    return Ok(listaResult);
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Usuario no autorizado para consumir el metodo."));
                }


            }
            catch (Exception ex)
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [HttpPost]
        [ResponseType(typeof(ENTDatosFacturacionPorRest))]
        public IHttpActionResult DatosFacturacion(ENTDatosFacturacionPorRest datosFacturacion)
        {
            //ENTDatosFacturacionPorRest datosFacturacion = new ENTDatosFacturacionPorRest();
            return Ok(datosFacturacion);
        }

        [HttpPost]
        [ResponseType(typeof(List<ENTPagosFacturadosREST>))]
        public IHttpActionResult GenerarFacturasPorPNR(ENTDatosFacturacionPorRest datosFacturacion)

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

            List<ENTPagosFacturadosREST> listaResult = new List<ENTPagosFacturadosREST>();
            string RFC_Extranjero = "XEXX010101000";
            int facturasSolicitadas = 0;
            int facturasGeneradas = 0;
            int facturasSinEnvio = 0;
            bool esRFC_ext = false;

            try
            {

                ///  Validar datos de facturación
                if (String.IsNullOrEmpty(datosFacturacion.UsoCFDI))
                    return Content(HttpStatusCode.BadRequest, new RespuestaRESTFactura(11, "Debe indicar el valor de UsoCFDI", ""));
                if (String.IsNullOrEmpty(datosFacturacion.RFCReceptor))
                    return Content(HttpStatusCode.BadRequest, new RespuestaRESTFactura(12, "Debe indicar el valor de RFCReceptor", ""));
                if (String.IsNullOrEmpty(datosFacturacion.EmailReceptor))
                    return Content(HttpStatusCode.BadRequest, new RespuestaRESTFactura(13, "Debe indicar el valor de EmailReceptor", ""));
                if (String.IsNullOrEmpty(datosFacturacion.NombrePasajero))
                    return Content(HttpStatusCode.BadRequest, new RespuestaRESTFactura(14, "Debe indicar el valor de NombrePasajero", ""));
                if (String.IsNullOrEmpty(datosFacturacion.ApellidosPasajero))
                    return Content(HttpStatusCode.BadRequest, new RespuestaRESTFactura(15, "Debe indicar el valor de ApellidosPasajero", ""));
                if (String.IsNullOrEmpty(datosFacturacion.ClaveReservacion))
                    return Content(HttpStatusCode.BadRequest, new RespuestaRESTFactura(16, "Debe indicar el valor de ClaveReservacion", ""));

                // Valida que el USO del CFDI sea los permitidos por el SAT
                Facturacion.BLL.BLLGendescripcionesCat bllGenDesc = new Facturacion.BLL.BLLGendescripcionesCat();
                int resBusqUsoCFDI = bllGenDesc.RecuperarGendescripcionesCatCvetabla("USOCFD")
                    .Where(c => c.CveValor == datosFacturacion.UsoCFDI).Count();
                if (resBusqUsoCFDI < 1)
                    return Content(HttpStatusCode.BadRequest, new RespuestaRESTFactura(17, "Valores validos para UsoCFDI son: [G03 / P01]", ""));

                esRFC_ext = datosFacturacion.RFCReceptor.ToUpper() == RFC_Extranjero ? true : false;


                if (esRFC_ext)
                {
                    // Valida que la clave del país sea deacuerdo al 3166-2
                    int resBusqISOPais = bllGenDesc.RecuperarGendescripcionesCatCvetabla("PAISES")
                        .Where(c => c.CveValor == datosFacturacion.PaisResidenciaFiscal).Count();
                    if (resBusqISOPais < 1)
                        return Content(HttpStatusCode.BadRequest, new RespuestaRESTFactura(18, "El valor de PaisResidenciaFiscal no es válido", ""));

                    // Valida TAX ID
                    if (esRFC_ext && String.IsNullOrEmpty(datosFacturacion.TAXID))
                        return Content(HttpStatusCode.BadRequest, new RespuestaRESTFactura(18, "Debe indicar el valor de TAXID", ""));
                }

                List<string> listaPNRFactura = new List<string>();
                List<ENTPagosPorFacturar> listaPagos = new List<ENTPagosPorFacturar>();


                ENTDatosFacturacion datosCliente = new ENTDatosFacturacion();
                datosCliente.UsoCFDI = datosFacturacion.UsoCFDI;
                datosCliente.EsExtranjero = esRFC_ext; //datosFacturacion.EsExtranjero;
                datosCliente.RFCReceptor = datosFacturacion.RFCReceptor;
                datosCliente.PaisResidenciaFiscal = datosFacturacion.PaisResidenciaFiscal;
                datosCliente.TAXID = datosFacturacion.TAXID;
                datosCliente.EmailReceptor = datosFacturacion.EmailReceptor;
                datosCliente.NombrePasajero = datosFacturacion.NombrePasajero;
                datosCliente.ApellidosPasajero = datosFacturacion.ApellidosPasajero;
                datosCliente.ClaveReservacion = datosFacturacion.ClaveReservacion;

                if (!esRFC_ext)
                {
                    datosCliente.PaisResidenciaFiscal = "";
                    datosCliente.TAXID = "";
                }

                // Validar que el PNR sea de una OTA
                BLLFacturacion bllFacturacion = new BLLFacturacion();
                string passwordOTA = String.Empty;
                bool clienteComercial = bllFacturacion.OcultarCaptchaPorPNR(datosFacturacion.ClaveReservacion, ref passwordOTA);
                if (!clienteComercial)
                    return Content(HttpStatusCode.BadRequest, new RespuestaRESTFactura(21, "Tu solicitud de facturación no puede ser atendida por éste medio", ""));

                Encrypt encrypt = new Encrypt();
                if (datosFacturacion.Password != encrypt.DecryptKey(passwordOTA))
                    return Content(HttpStatusCode.BadRequest, new RespuestaRESTFactura(31, "Contraseña invalida", ""));


                // Validar PNR y datos de pasajero
                ENTDatosFacturacion objDatosFac = new ENTDatosFacturacion();
                objDatosFac.ClaveReservacion = datosFacturacion.ClaveReservacion.Trim().ToUpper();
                objDatosFac.NombrePasajero = datosFacturacion.NombrePasajero.Trim().ToUpper();
                objDatosFac.ApellidosPasajero = datosFacturacion.ApellidosPasajero.Trim().ToUpper();
                if (!bllFacturacion.ValidarNombrePasajero(ref objDatosFac))
                    return Content(HttpStatusCode.BadRequest, new RespuestaRESTFactura(41, "Los datos del pasajero no corresponden a la reservación solicitada. Favor de verificar la información.", ""));

                datosCliente.ClaveReservacion = objDatosFac.ClaveReservacion.Trim().ToUpper();
                datosCliente.NombrePasajero = objDatosFac.NombrePasajero.Trim().ToUpper();
                datosCliente.ApellidosPasajero = objDatosFac.ApellidosPasajero.Trim().ToUpper();

                
                //Recupera la información de los pagos que aún no están facturados
                //listaPagos = bllFacturacion.RecuperarPagosParaFacturar(datosCliente);

                long folioFactura =  0;
                List<ENTPagosPorFacturar> listaPagosSel = new List<ENTPagosPorFacturar>();
                //Se identifican los pagos que pueden ser facturados
                foreach (var pago in listaPagos)
                {
                    // Se guarda el folio de la factura generada
                    folioFactura = pago.FolioFactura;

                    pago.PNR = datosCliente.ClaveReservacion;
                    // Determina sí el pago corresponde al OTA
                    BLLOtasCat bllOtasCat = new BLLOtasCat();
                    var numSocComEncon = bllOtasCat.RecuperarOtasCatOrganizationcode(pago.OrganizationCode)
                                        .ToList().Where(o => o.Activo == true).Count();
                    //Sí el pago SI es de la OTA se podrá facturar
                    pago.EsFacturable = numSocComEncon > 0 ? true : false;
                    //pago.Mensaje = pago.Mensaje != null && pago.Mensaje.Length > 0 ? pago.Mensaje : numSocComEncon < 1 ? "Éste pago no puede facturarse por éste medio" : String.Empty;

                    #region " P A G O S "
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


                                        // Validar BIN
                                        // ResponseBINRest response = bllFacturacion.InvocarBINRest(pago.BinRange.ToString());
                                        // 04 -> TC  |   28 -> TD   |   01 -> Efectivo
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
                    #endregion

                }

                String strUrl = String.Empty;
                string rutaArchivos = String.Empty;

                if (listaPagosSel.Count() > 0)
                {
                    try
                    {
                        //listaPagosSel = bllFacturacion.GenerarFacturaCliente(datosCliente, listaPagosSel);

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

                        // Generar ZIP 
                        ENTPagosPorFacturar pagoFacturado = listaPagosSel.Where(x => x.RutaCFDI != null && x.RutaCFDI != "").FirstOrDefault();
                        rutaArchivos = pagoFacturado.RutaCFDI;

                        FileInfo archivoCfdi = new FileInfo(rutaArchivos);
                        rutaArchivos = archivoCfdi.Directory.Parent.FullName.ToString();

                        DirectoryInfo carpeta = new DirectoryInfo(rutaArchivos);
                        if (carpeta.Exists)
                        {
                            strUrl = CrearRutaArchivosFactura(carpeta, datosFacturacion.ClaveReservacion.Trim().ToUpper());

                            return Ok(new RespuestaRESTFactura(200, "OK", strUrl));
                        }
                        else
                            return Content(HttpStatusCode.InternalServerError, new RespuestaRESTFactura(101, "No existe el path para generar ZIP", ""));

                    }
                    catch (Exception ex)
                    {
                        return Content(HttpStatusCode.InternalServerError, new RespuestaRESTFactura(0, ex.Message, ""));
                    }
                }
                else
                {
                    if(folioFactura > 0)
                    {
                        BLLFacturacion bllFact = new BLLFacturacion();
                        rutaArchivos = bllFact.GeneraArchivosFactura(folioFactura);
                        FileInfo archivoCfdi = new FileInfo(rutaArchivos);
                        rutaArchivos = archivoCfdi.Directory.ToString();
                        DirectoryInfo carpeta = new DirectoryInfo(rutaArchivos);

                        if (carpeta.Exists)
                        {
                            strUrl = CrearRutaArchivosFactura(carpeta, datosFacturacion.ClaveReservacion.Trim().ToUpper());
                            return Content(HttpStatusCode.Accepted, new RespuestaRESTFactura(210, "OK", strUrl));
                        } else
                            return Content(HttpStatusCode.InternalServerError, new RespuestaRESTFactura(101, "No existe el path para generar ZIP", ""));

                    } else
                        return Content(HttpStatusCode.Accepted, new RespuestaRESTFactura(202, "No hay pagos pendientes que facturar", ""));
                }
                

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new RespuestaRESTFactura(0, ex.Message, ""));
                //throw new Exception("Error: " + ex.Message);
            }
        }

        #endregion

        #region Metodos Privados

        private List<string> RecuperarListaOrgCode(string nombreParam)
        {
            List<string> listaResult = new List<string>();
            string paramDepartTrafico = "";


            //Recuperar el parametro de los organizationCode que se consideraran para estos pagos
            Facturacion.BLL.BLLParametrosCnf bllParam = new Facturacion.BLL.BLLParametrosCnf();
            bllParam.RecuperarParametrosCnfNombre(nombreParam);

            if (bllParam.IdParametro > 0)
            {
                paramDepartTrafico = bllParam.Valor;
            }
            else
            {
                //En caso de que no este registrado el valor se asigna un valor por default
                paramDepartTrafico = "";
            }


            foreach (string paramOrg in paramDepartTrafico.Split(','))
            {
                if (paramOrg.Trim().Length > 0)
                {
                    listaResult.Add(paramOrg);
                }

            }
            return listaResult;
        }

        private bool ValidarUsuarioFacturacion(string usuario, string pwd)
        {
            bool result = false;
            string paramUsuarioREST = "";

            try
            {


                //Recuperar el parametro de los organizationCode que se consideraran para estos pagos
                Facturacion.BLL.BLLParametrosCnf bllParam = new Facturacion.BLL.BLLParametrosCnf();
                bllParam.RecuperarParametrosCnfNombre("USERREST");

                if (bllParam.IdParametro > 0)
                {
                    paramUsuarioREST = bllParam.Valor;
                }
                else
                {
                    //En caso de que no este registrado el valor se asigna un valor por default
                    paramUsuarioREST = "VBREST";
                }

                //Recuperar el parametro de los organizationCode que se consideraran para estos pagos
                Facturacion.BLL.BLLUsuariosCat bllUsuario = new Facturacion.BLL.BLLUsuariosCat();
                bllUsuario.RecuperarUsuariosCatUsuario("VBREST");

                if (bllUsuario.IdUsuario > 0)
                {
                    string pass = "";
                    pass = bllUsuario.Password;

                    if (bllUsuario.Usuario != usuario)
                    {
                        throw new Exception("El usuario no cuenta con permisos para utilizar este proceso...");
                    }
                    if (bllUsuario.Password != pwd)
                    {
                        throw new Exception("El password es incorrecto...");
                    }

                    if (bllUsuario.FechaIniVigencia > DateTime.Now)
                    {
                        throw new Exception("La vigencia del usuario aun no comienza...");
                    }

                    if (bllUsuario.FechaFinVigencia < DateTime.Now)
                    {
                        throw new Exception("La vigencia del usuario ya concluyo...");
                    }



                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        private String CrearRutaArchivosFactura(DirectoryInfo carpeta, String PNR)
        {
            String strUrl = String.Empty;
            string zipName = String.Format("CFDI_{0}.zip", carpeta.Name);

            if(carpeta.GetFiles(zipName).Length < 1)
            {
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

                    zip.Save(carpeta + "\\" + zipName);
                }
            }

            strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery
                                                                              , HttpContext.Current.Request.ApplicationPath);
            strUrl = Path.Combine(strUrl, @"Archivos/" + DateTime.Now.ToString("dd-MM-yyyy") + "/" + PNR); //  + "/"+ carpeta.Name
            strUrl = strUrl + "/" + zipName;

            return strUrl;
        }
        #endregion
    }
}

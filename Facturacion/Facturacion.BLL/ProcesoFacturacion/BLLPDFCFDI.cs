using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using System.IO;
using Facturacion.ENT;
using Comun.Utils;
using System.Data.SqlClient;
using System.Reflection;

namespace Facturacion.BLL.ProcesoFacturacion
{
    public class BLLPDFCFDI
    {

        #region Propiedades privadas
        private BLLBitacoraErrores BllLogErrores { get; set; }
        private List<ENTParametrosCnf> ListaParametros { get; set; }
        private List<ENTGencatalogosCat> ListaGenCatalogos { get; set; }
        private List<ENTGendescripcionesCat> ListaGenDescripciones { get; set; }
        private string RutaFuentePDF { get; set; }
        public string PNR { get; set; }
        public string MensajeErrorUsuario { get; set; }
        #endregion

        #region Constructor
        public BLLPDFCFDI()
        {
            BllLogErrores = new BLLBitacoraErrores();
            try
            {

                ListaParametros = new List<ENTParametrosCnf>();
                ListaGenCatalogos = new List<ENTGencatalogosCat>();
                ListaGenDescripciones = new List<ENTGendescripcionesCat>();

                BLLParametrosCnf bllParam = new BLL.BLLParametrosCnf();
                BLLFacturacion bllFac = new BLLFacturacion();
                ListaParametros = bllFac.RecuperarParametros();

                BLLGencatalogosCat bllGenCatalogos = new BLLGencatalogosCat();
                ListaGenCatalogos = bllGenCatalogos.RecuperarTodo();

                BLLGendescripcionesCat bllDescripciones = new BLLGendescripcionesCat();
                ListaGenDescripciones = bllDescripciones.RecuperarTodo();

                RutaFuentePDF = ListaParametros.Where(x => x.Nombre == "RutaFontPDF").FirstOrDefault().Valor;

                if (ListaParametros.Where(x => x.Nombre == "MsjErrorPortal").Count() > 0)
                {
                    MensajeErrorUsuario = ListaParametros.Where(x => x.Nombre == "MsjErrorPortal").FirstOrDefault().Valor;
                }
                else
                {
                    MensajeErrorUsuario = "Por el momento no es posible procesar su solicitud por favor intente más tarde...";

                }


            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "Constructor");
                throw new ExceptionViva(mensajeUsuario);
            }
        }
        #endregion


        #region Metodos Publicos Factura Version 3.3
        public string GeneraPDFFactura33(string xmlCFDI, string cadenaOriginal, string pnr, bool flg_Terceros)
        {
            string result = "";
            string archivoFuente = "";
            string copy_CFDI = string.Empty;

            try
            {
                if (flg_Terceros)
                {
                    int index = xmlCFDI.IndexOf("<terceros:Impuestos>");
                    int index3 = xmlCFDI.LastIndexOf("<cfdi:ComplementoConcepto>");

                    string tag_Impuestos = xmlCFDI.Substring(index);

                    int index2 = tag_Impuestos.IndexOf("</terceros:PorCuentadeTerceros>");

                    tag_Impuestos = tag_Impuestos.Substring(0, index2);

                    copy_CFDI = xmlCFDI.Replace(tag_Impuestos, "");

                    copy_CFDI = copy_CFDI.Insert(index3, tag_Impuestos);

                    copy_CFDI = copy_CFDI.Replace("terceros:Impuestos", "cfdi:Impuestos").Replace("terceros:Traslados", "cfdi:Traslados").Replace("terceros:Traslado", "cfdi:Traslado");

                }
                else
                {
                    copy_CFDI = xmlCFDI;
                }

                

                //Convertimos la respuesta de pegaso en un documento de XML
                XmlDocument xmlDocuResponse = new XmlDocument();
                //xmlDocuResponse.LoadXml(xmlCFDI);
                xmlDocuResponse.LoadXml(copy_CFDI);
                XmlNodeReader xmlReader = new XmlNodeReader(xmlDocuResponse);

                DataSet dsRespuesta = new DataSet();
                dsRespuesta.ReadXml(xmlReader);

                string rutaRelativa = Environment.CurrentDirectory;

                //Declaramos el Tipo de letra que utilizaremos para el Documento PDF

                archivoFuente = Path.Combine(
                    System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath == null ? Environment.CurrentDirectory : System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath
                    , @"Fonts\ARIALN.TTF");

                BaseFont baseTimes = BaseFont.CreateFont(archivoFuente, BaseFont.WINANSI, true);
                //BaseFont f_cb = BaseFont.CreateFont(archivoFuente, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //BaseFont f_cn = BaseFont.CreateFont(archivoFuente, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //BaseFont bf = FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC, Font.DEFAULTSIZE).GetCalculatedBaseFont(false);

                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font _TituloFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


                //Tipo de letra de los sellos digitales y cadenas del SAT.

                Font fuenteFolioFactura = new Font(baseTimes, 14, Font.BOLD);
                Font fuenteFactura = new Font(baseTimes, 7, Font.NORMAL);
                Font fuenteEncConceptos = new Font(baseTimes, 7, Font.NORMAL);
                Font fuenteValConceptos = new Font(baseTimes, 7, Font.NORMAL);
                Font fuenteFilasVacias = new Font(baseTimes, 7, Font.NORMAL, BaseColor.WHITE);
                Font fuenteTotales = new Font(baseTimes, 8, Font.NORMAL);
                Font fuenteTotalLetras = new Font(baseTimes, 8, Font.NORMAL);
                Font fuenteGeneral = new Font(baseTimes, 8, Font.NORMAL);
                Font fuenteCursiva = new Font(baseTimes, 8, Font.ITALIC);
                Font fuenteTituloBloque = new Font(baseTimes, 9, Font.BOLDITALIC);

                Font fuenteLeyRepGrafica = new Font(baseTimes, 10, Font.BOLD);
                Font fuenteTimbrado = new Font(baseTimes, 7, Font.NORMAL);

                //Recuperar informacion de cada tabla en el timbrado
                DataTable dtComprobante = dsRespuesta.Tables["Comprobante"];//OK drDatosComprobante
                DataTable dtEmisor = dsRespuesta.Tables["Emisor"];//OK drDatosEmisor
                DataTable dtReceptor = dsRespuesta.Tables["Receptor"];//OK drDatosReceptor
                DataTable dtConceptos = dsRespuesta.Tables["Conceptos"];
                DataTable dtConcepto = dsRespuesta.Tables["Concepto"];
                DataTable dtImpuestos = dsRespuesta.Tables["Impuestos"];
                DataTable dtTraslados = dsRespuesta.Tables["Traslados"];
                DataTable dtTraslado = dsRespuesta.Tables["Traslado"]; // ok drTraslado
                DataTable dtComplemento = dsRespuesta.Tables["Complemento"];
                DataTable dtAerolineas = dsRespuesta.Tables["Aerolineas"];//OK drTua
                DataTable dtOtrosCargos = (dsRespuesta.Tables.Contains("OtrosCargos") ? dsRespuesta.Tables["OtrosCargos"] : null);
                DataTable dtCargo = dsRespuesta.Tables["Cargo"];
                DataTable dtTimbreFiscalDigital = dsRespuesta.Tables["TimbreFiscalDigital"]; //OK drDatosTfd


                //Referencias a las tablas del XML Del Formato CFDI
                DataRow drDatosTfd = dtTimbreFiscalDigital.Rows[0];
                DataRow drDatosEmisor = dtEmisor.Rows[0];
                DataRow drDatosReceptor = dtReceptor.Rows[0];
                DataRow drDatosComprobante = dtComprobante.Rows[0];
                DataRow drTua = dtAerolineas.Rows[0];

                //Se recupera la ruta donde se deben guardar los archivos del CFDI
                string rutaPDF = ListaParametros.Where(x => x.Nombre == "RutaArchivosFactura").FirstOrDefault().Valor;
                string ruta = String.Empty;
                string nombreCarpetaToday = String.Empty;

                if (!pnr.Contains("|"))
                {
                    PNR = pnr;
                    nombreCarpetaToday = string.Format("{0:dd-MM-yyyy}", DateTime.Today).ToUpper();
                }
                else
                {
                    PNR = pnr.Split('|')[1].ToString();
                    nombreCarpetaToday = pnr.Split('|')[0].ToString();
                }

                string folio = drDatosComprobante["Folio"].ToString();
                ruta = Path.Combine(rutaPDF, nombreCarpetaToday, PNR, folio);

                DirectoryInfo carpeta = new DirectoryInfo(ruta);

                if (!carpeta.Exists)
                    carpeta.Create();


                result = Path.Combine(ruta, string.Format("PDF_{0}.pdf", folio));

                using (System.IO.FileStream fileStrem = new FileStream(result, FileMode.Create))
                {
                    Document documentPDF = new Document(PageSize.LETTER);
                    PdfWriter writer = PdfWriter.GetInstance(documentPDF, fileStrem);

                    documentPDF.AddCreator("VivaAerobus");

                    //Abrimos el documento para comenzar a escribir
                    documentPDF.SetMargins(25f, 25f, 25f, 25f);
                    documentPDF.Open();

                    PdfPTable tblLogoEmisor = new PdfPTable(2) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
                    float[] widths = new float[] { 350f, 300f };
                    tblLogoEmisor.SetWidths(widths);

                    string rutaImagenes = Path.Combine(
                        System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath == null ? Environment.CurrentDirectory : System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath
                        , @"Imagenes\PDF33\");
                    //ListaParametros.Where(x => x.Nombre == "RutaImagenesPDF33").FirstOrDefault().Valor;
                    PdfPCell Logo = new PdfPCell(InsertaImagen(rutaImagenes, "logo.png", 200f)) { VerticalAlignment = Element.ALIGN_TOP, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
                    tblLogoEmisor.AddCell(Logo);

                    Logo = new PdfPCell(CrearSerieFolio(fuenteFolioFactura, dtComprobante)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_RIGHT, BorderWidth = 0 };

                    tblLogoEmisor.AddCell(Logo);

                    documentPDF.Add(tblLogoEmisor);

                    documentPDF.Add(CrearDatosEmisor(fuenteGeneral, dtEmisor));


                    documentPDF.Add(InsertaSaltoLinea(3f));
                    documentPDF.Add(CrearTituloBloque("CLIENTE", fuenteTituloBloque));
                    documentPDF.Add(InsertaSaltoLinea(2f));

                    documentPDF.Add(CrearBloqueReceptor("RECEPTOR", fuenteGeneral, dtReceptor));

                    documentPDF.Add(InsertaSaltoLinea(6f));
                    documentPDF.Add(CrearTituloBloque("DATOS COMPROBANTE", fuenteTituloBloque));
                    documentPDF.Add(InsertaSaltoLinea(2f));
                    documentPDF.Add(CrearBloqueDatos("COMPROBANTE", fuenteGeneral, dtComprobante));

                    documentPDF.Add(InsertaSaltoLinea(6f));
                    documentPDF.Add(CrearTituloBloque("PRODUCTOS / SERVICIOS", fuenteTituloBloque));
                    documentPDF.Add(InsertaSaltoLinea(2f));
                    PdfPTable tblConceptos = CrearBloqueConceptos(fuenteEncConceptos, fuenteValConceptos, fuenteFilasVacias, dtConceptos, dtConcepto, dtImpuestos, dtTraslados, dtTraslado);

                    documentPDF.Add(tblConceptos);


                    documentPDF.Add(InsertaSaltoLinea(5f));
                    PdfPTable tblTotales = new PdfPTable(2) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
                    float[] widthTotales = new float[] { 450f, 200f };
                    tblTotales.SetWidths(widthTotales);

                    //DATOS DEL IMPORTE CON LETRA
                    FormatoNumLetra ConvertirNumLetra = new FormatoNumLetra();
                    string numero = drDatosComprobante["total"].ToString();
                    string codMoneda = drDatosComprobante["Moneda"].ToString();

                    PdfPCell clTab = new PdfPCell(new Phrase(string.Format("Importe con Letra: {0}", ConvertirNumLetra.Convertir(numero, true, codMoneda)), fuenteTotalLetras)) { VerticalAlignment = Element.ALIGN_TOP, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0f, BorderWidthTop = 1f, PaddingRight = 15f };

                    clTab.PaddingTop = 5f;

                    tblTotales.AddCell(clTab);

                    clTab = new PdfPCell(CrearBloqueTotales(fuenteTotales, drDatosComprobante, dtTraslado)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_RIGHT, BorderWidth = 0f, BorderWidthTop = 1f };
                    clTab.PaddingTop = 5f;
                    tblTotales.AddCell(clTab);

                    documentPDF.Add(tblTotales);

                    //Leyenda de IVA Fronterizo
                    documentPDF.Add(InsertaSaltoLinea(10f));
                    PdfPTable tblLeyIVA = new PdfPTable(1) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
                    //"*Tratándose de Transportación Aérea Internacional y Franja Fronteriza, el IVA se calculará al 25% de la tasa general del 16%, de conformidad del Artículo 16 de la ley del IVA vigente.";
                    string leyendaIVA = ListaParametros.Where(x => x.Nombre == "LeyendaIVAPDF").FirstOrDefault().Valor;
                    PdfPCell clLeyIVA = new PdfPCell(new Phrase(leyendaIVA, fuenteCursiva)) { VerticalAlignment = Element.ALIGN_TOP, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0f, BorderWidthTop = 1f, PaddingRight = 15f };
                    clLeyIVA.Padding = 5f;
                    clLeyIVA.BorderWidthTop = 1f;
                    clLeyIVA.BorderWidthBottom = 1f;
                    clLeyIVA.BackgroundColor = new BaseColor(229, 232, 232);
                    tblLeyIVA.AddCell(clLeyIVA);
                    documentPDF.Add(tblLeyIVA);

                    //CREAR BLOQUE DE COMPLEMENTOS DE AEROLINEA.
                    documentPDF.Add(InsertaSaltoLinea(6f));
                    documentPDF.Add(CrearTituloBloque("COMPLEMENTO AEROLINEAS", fuenteTituloBloque));
                    documentPDF.Add(InsertaSaltoLinea(2f));
                    bool mostrarTUA = dtCargo != null && dtCargo.Select("CodigoCargo = 'TUA'").Count() > 0;
                    PdfPTable tblComplementoTUA = CrearBloqueComplementosTUA(fuenteTotales, dtOtrosCargos, drTua, mostrarTUA);

                    documentPDF.Add(tblComplementoTUA);
                    documentPDF.Add(InsertaSaltoLinea(2f));

                    if (dtCargo != null && dtCargo.Rows.Count > 0)
                    {
                        PdfPTable tblComplementoAero = CrearBloqueComplementosAero(fuenteValConceptos, dtCargo);
                        documentPDF.Add(tblComplementoAero);
                    }

                    //CREAR BLOQUE DE COMPLEMENTOS DE AEROLINEA.
                    documentPDF.Add(InsertaSaltoLinea(6f));
                    documentPDF.Add(CrearTituloBloque("DATOS TIMBRADO CFDI", fuenteTituloBloque));
                    documentPDF.Add(InsertaSaltoLinea(2f));


                    PdfPTable tblTimbrado = new PdfPTable(2) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
                    float[] widthsTim = new float[] { 100f, 500f };
                    tblTimbrado.SetWidths(widthsTim);

                    string urlValidacionCFDI = ListaParametros.Where(x => x.Nombre == "UrlValidacionCFDI").FirstOrDefault().Valor;

                    if (urlValidacionCFDI.Length == 0)
                    {
                        urlValidacionCFDI = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx";
                    }

                    System.Text.StringBuilder codigoBi = new System.Text.StringBuilder();
                    codigoBi.Append(urlValidacionCFDI);
                    codigoBi.Append("?id=" + drDatosTfd["UUID"].ToString());
                    codigoBi.Append("&re=" + drDatosEmisor["rfc"].ToString());
                    codigoBi.Append("&rr=" + drDatosReceptor["rfc"].ToString());
                    codigoBi.Append("&tt=" + drDatosComprobante["total"].ToString());
                    string sello = drDatosComprobante["Sello"].ToString();
                    string ult8carSello = sello.Substring(sello.Length - 8, 8);
                    codigoBi.Append("&fe=" + ult8carSello);

                    BarcodeQRCode qrcodes = new BarcodeQRCode(codigoBi.ToString(), 1, 1, null);
                    iTextSharp.text.Image imgCB = qrcodes.GetImage();
                    imgCB.ScaleAbsolute(92, 92);

                    PdfPCell celTim = new PdfPCell(imgCB) { VerticalAlignment = Element.ALIGN_TOP, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0 };
                    tblTimbrado.AddCell(celTim);

                    celTim = new PdfPCell(CrearBloqueTimbrado("TIMBRADO", fuenteTimbrado, dtTimbreFiscalDigital, dtComprobante, cadenaOriginal)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_RIGHT, BorderWidth = 0 };
                    tblTimbrado.AddCell(celTim);
                    documentPDF.Add(tblTimbrado);

                    //Agregar leyenda de que es una representacion grafica del CFDI
                    documentPDF.Add(InsertaSaltoLinea(6f));
                    string textoRepresentacionGraficaCFDI = "Este documento es una representación impresa de un CFDI";
                    documentPDF.Add(CrearLeyendaFija(textoRepresentacionGraficaCFDI, fuenteLeyRepGrafica, Element.ALIGN_CENTER));

                    //Cerramos el documento
                    documentPDF.CloseDocument();
                    documentPDF.Close();
                    writer.Close();
                    fileStrem.Close();
                    fileStrem.Dispose();
                    documentPDF.Dispose();
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "GeneraPDFFactura33");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;


        }
        public string GeneraArchivoCFDI(string xmlCFDI, long folioCFDI, string pnr)
        {
            string result = "";

            try
            {
                //Se recupera la ruta donde se deben guardar los archivos del CFDI
                string rutaCFDI = ListaParametros.Where(x => x.Nombre == "RutaArchivosFactura").FirstOrDefault().Valor;
                string ruta = String.Empty;
                string nombreCarpetaToday = String.Empty;

                if (!pnr.Contains("|"))
                {
                    PNR = pnr;
                    nombreCarpetaToday = string.Format("{0:dd-MM-yyyy}", DateTime.Today).ToUpper();
                }
                else
                {
                    PNR = pnr.Split('|')[1].ToString();
                    nombreCarpetaToday = pnr.Split('|')[0].ToString();
                }

                ruta = Path.Combine(rutaCFDI, nombreCarpetaToday, PNR, folioCFDI.ToString());

                DirectoryInfo carpeta = new DirectoryInfo(ruta);

                if (!carpeta.Exists)
                    carpeta.Create();

                result = Path.Combine(ruta, string.Format("CFDI_{0}.xml", folioCFDI.ToString()));

                XmlDocument xmlDocuResponse = new XmlDocument();
                xmlDocuResponse.LoadXml(xmlCFDI);
                xmlDocuResponse.Save(result);
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "GeneraArchivoCFDI");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }

        public string GeneraArchivoCFDI(string xmlCFDI, string ruta, string nombreArchivo)
        {
            string result = "";

            try
            {
                if (nombreArchivo.Contains("|"))
                {
                    nombreArchivo = nombreArchivo.Split('|')[1].ToString();
                }

                DirectoryInfo carpeta = new DirectoryInfo(ruta);

                if (!carpeta.Exists)
                    carpeta.Create();

                result = Path.Combine(ruta, nombreArchivo);

                XmlDocument xmlDocuResponse = new XmlDocument();
                xmlDocuResponse.LoadXml(xmlCFDI);
                xmlDocuResponse.Save(result);
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "GeneraArchivoCFDI");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }

        public string GeneraPDFFactura33(string xmlCFDI, string cadenaOriginal, string pnr, string ruta, string nombrePDF, bool flg_Terceros)
        {
            string result = "";
            string copy_CFDI = string.Empty;
            try
            {
                if (nombrePDF.Contains("|"))
                {
                    nombrePDF = nombrePDF.Split('|')[1].ToString();
                }

                if (flg_Terceros)
                {
                    int index = xmlCFDI.IndexOf("<terceros:Impuestos>");
                    int index3 = xmlCFDI.LastIndexOf("<cfdi:ComplementoConcepto>");

                    string tag_Impuestos = xmlCFDI.Substring(index);

                    int index2 = tag_Impuestos.IndexOf("</terceros:PorCuentadeTerceros>");

                    tag_Impuestos = tag_Impuestos.Substring(0, index2);

                    copy_CFDI = xmlCFDI.Replace(tag_Impuestos, "");

                    copy_CFDI = copy_CFDI.Insert(index3, tag_Impuestos);

                    copy_CFDI = copy_CFDI.Replace("terceros:Impuestos", "cfdi:Impuestos").Replace("terceros:Traslados", "cfdi:Traslados").Replace("terceros:Traslado", "cfdi:Traslado");
                }

                else
                {
                    copy_CFDI = xmlCFDI;
                }

                PNR = pnr;
                //Convertimos la respuesta de pegaso en un documento de XML
                XmlDocument xmlDocuResponse = new XmlDocument();
                xmlDocuResponse.LoadXml(copy_CFDI);
                XmlNodeReader xmlReader = new XmlNodeReader(xmlDocuResponse);

                DataSet dsRespuesta = new DataSet();
                dsRespuesta.ReadXml(xmlReader);

                string rutaRelativa = Environment.CurrentDirectory;

                //Declaramos el Tipo de letra que utilizaremos para el Documento PDF
                string archivoFuente = "";
                archivoFuente = Path.Combine(
                    System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath == null ? Environment.CurrentDirectory : System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath
                    , @"Fonts\ARIALN.TTF");

                BaseFont baseTimes = BaseFont.CreateFont(archivoFuente, BaseFont.WINANSI, true);
                //BaseFont f_cb = BaseFont.CreateFont(archivoFuente, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //BaseFont f_cn = BaseFont.CreateFont(archivoFuente, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                //BaseFont bf = FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC, Font.DEFAULTSIZE).GetCalculatedBaseFont(false);

                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font _TituloFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


                //Tipo de letra de los sellos digitales y cadenas del SAT.

                Font fuenteFolioFactura = new Font(baseTimes, 14, Font.BOLD);
                Font fuenteFactura = new Font(baseTimes, 7, Font.NORMAL);
                Font fuenteEncConceptos = new Font(baseTimes, 7, Font.NORMAL);
                Font fuenteValConceptos = new Font(baseTimes, 7, Font.NORMAL);
                Font fuenteFilasVacias = new Font(baseTimes, 7, Font.NORMAL, BaseColor.WHITE);
                Font fuenteTotales = new Font(baseTimes, 8, Font.NORMAL);
                Font fuenteTotalLetras = new Font(baseTimes, 8, Font.NORMAL);
                Font fuenteGeneral = new Font(baseTimes, 8, Font.NORMAL);
                Font fuenteCursiva = new Font(baseTimes, 8, Font.ITALIC);
                Font fuenteTituloBloque = new Font(baseTimes, 9, Font.BOLDITALIC);

                Font fuenteLeyRepGrafica = new Font(baseTimes, 10, Font.BOLD);
                Font fuenteTimbrado = new Font(baseTimes, 7, Font.NORMAL);

                //Recuperar informacion de cada tabla en el timbrado
                DataTable dtComprobante = dsRespuesta.Tables["Comprobante"];//OK drDatosComprobante
                DataTable dtEmisor = dsRespuesta.Tables["Emisor"];//OK drDatosEmisor
                DataTable dtReceptor = dsRespuesta.Tables["Receptor"];//OK drDatosReceptor
                DataTable dtConceptos = dsRespuesta.Tables["Conceptos"];
                DataTable dtConcepto = dsRespuesta.Tables["Concepto"];
                DataTable dtImpuestos = dsRespuesta.Tables["Impuestos"];
                DataTable dtTraslados = dsRespuesta.Tables["Traslados"];
                DataTable dtTraslado = dsRespuesta.Tables["Traslado"]; // ok drTraslado
                DataTable dtComplemento = dsRespuesta.Tables["Complemento"];
                DataTable dtAerolineas = dsRespuesta.Tables["Aerolineas"];//OK drTua
                DataTable dtOtrosCargos = (dsRespuesta.Tables.Contains("OtrosCargos") ? dsRespuesta.Tables["OtrosCargos"] : null);
                DataTable dtCargo = dsRespuesta.Tables["Cargo"];
                DataTable dtTimbreFiscalDigital = dsRespuesta.Tables["TimbreFiscalDigital"]; //OK drDatosTfd


                //Referencias a las tablas del XML Del Formato CFDI
                DataRow drDatosTfd = dtTimbreFiscalDigital.Rows[0];
                DataRow drDatosEmisor = dtEmisor.Rows[0];
                DataRow drDatosReceptor = dtReceptor.Rows[0];
                DataRow drDatosComprobante = dtComprobante.Rows[0];
                DataRow drTua = dtAerolineas.Rows[0];

                //string rutaPDF = ListaParametros.Where(x => x.Nombre == "RutaArchivosFactura").FirstOrDefault().Valor;
                //string nombreCarpetaToday = string.Format("{0:dd-MM-yyyy}", DateTime.Today).ToUpper();
                //string folio = drDatosComprobante["Folio"].ToString();
                //string ruta = Path.Combine(rutaPDF, nombreCarpetaToday, pnr, folio);

                DirectoryInfo carpeta = new DirectoryInfo(ruta);

                if (!carpeta.Exists)
                    carpeta.Create();


                result = Path.Combine(ruta, string.Format("{0}.pdf", nombrePDF));

                using (System.IO.FileStream fileStrem = new FileStream(result, FileMode.Create))
                {
                    Document documentPDF = new Document(PageSize.LETTER);
                    PdfWriter writer = PdfWriter.GetInstance(documentPDF, fileStrem);

                    documentPDF.AddCreator("VivaAerobus");

                    //Abrimos el documento para comenzar a escribir
                    documentPDF.SetMargins(25f, 25f, 25f, 25f);
                    documentPDF.Open();

                    PdfPTable tblLogoEmisor = new PdfPTable(2) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
                    float[] widths = new float[] { 350f, 300f };
                    tblLogoEmisor.SetWidths(widths);

                    string rutaImagenes = Path.Combine(
                        System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath == null ? Environment.CurrentDirectory : System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath
                        , @"Imagenes\PDF33\");
                    //string rutaImagenes = ListaParametros.Where(x => x.Nombre == "RutaImagenesPDF33").FirstOrDefault().Valor;
                    PdfPCell Logo = new PdfPCell(InsertaImagen(rutaImagenes, "logo.png", 200f)) { VerticalAlignment = Element.ALIGN_TOP, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0 };
                    tblLogoEmisor.AddCell(Logo);

                    Logo = new PdfPCell(CrearSerieFolio(fuenteFolioFactura, dtComprobante)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_RIGHT, BorderWidth = 0 };

                    tblLogoEmisor.AddCell(Logo);

                    documentPDF.Add(tblLogoEmisor);

                    documentPDF.Add(CrearDatosEmisor(fuenteGeneral, dtEmisor));


                    documentPDF.Add(InsertaSaltoLinea(3f));
                    documentPDF.Add(CrearTituloBloque("CLIENTE", fuenteTituloBloque));
                    documentPDF.Add(InsertaSaltoLinea(2f));

                    documentPDF.Add(CrearBloqueReceptor("RECEPTOR", fuenteGeneral, dtReceptor));

                    documentPDF.Add(InsertaSaltoLinea(6f));
                    documentPDF.Add(CrearTituloBloque("DATOS COMPROBANTE", fuenteTituloBloque));
                    documentPDF.Add(InsertaSaltoLinea(2f));
                    documentPDF.Add(CrearBloqueDatos("COMPROBANTE", fuenteGeneral, dtComprobante));

                    documentPDF.Add(InsertaSaltoLinea(6f));
                    documentPDF.Add(CrearTituloBloque("PRODUCTOS / SERVICIOS", fuenteTituloBloque));
                    documentPDF.Add(InsertaSaltoLinea(2f));
                    PdfPTable tblConceptos = CrearBloqueConceptos(fuenteEncConceptos, fuenteValConceptos, fuenteFilasVacias, dtConceptos, dtConcepto, dtImpuestos, dtTraslados, dtTraslado);

                    documentPDF.Add(tblConceptos);


                    documentPDF.Add(InsertaSaltoLinea(5f));
                    PdfPTable tblTotales = new PdfPTable(2) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
                    float[] widthTotales = new float[] { 450f, 200f };
                    tblTotales.SetWidths(widthTotales);

                    //DATOS DEL IMPORTE CON LETRA
                    FormatoNumLetra ConvertirNumLetra = new FormatoNumLetra();
                    string numero = drDatosComprobante["total"].ToString();
                    string codMoneda = drDatosComprobante["Moneda"].ToString();

                    PdfPCell clTab = new PdfPCell(new Phrase(string.Format("Importe con Letra: {0}", ConvertirNumLetra.Convertir(numero, true, codMoneda)), fuenteTotalLetras)) { VerticalAlignment = Element.ALIGN_TOP, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0f, BorderWidthTop = 1f, PaddingRight = 15f };

                    clTab.PaddingTop = 5f;

                    tblTotales.AddCell(clTab);

                    clTab = new PdfPCell(CrearBloqueTotales(fuenteTotales, drDatosComprobante, dtTraslado)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_RIGHT, BorderWidth = 0f, BorderWidthTop = 1f };
                    clTab.PaddingTop = 5f;
                    tblTotales.AddCell(clTab);

                    documentPDF.Add(tblTotales);

                    //Leyenda de IVA Fronterizo
                    documentPDF.Add(InsertaSaltoLinea(10f));
                    PdfPTable tblLeyIVA = new PdfPTable(1) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
                    //"*Tratándose de Transportación Aérea Internacional y Franja Fronteriza, el IVA se calculará al 25% de la tasa general del 16%, de conformidad del Artículo 16 de la ley del IVA vigente.";
                    string leyendaIVA = ListaParametros.Where(x => x.Nombre == "LeyendaIVAPDF").FirstOrDefault().Valor;
                    PdfPCell clLeyIVA = new PdfPCell(new Phrase(leyendaIVA, fuenteCursiva)) { VerticalAlignment = Element.ALIGN_TOP, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0f, BorderWidthTop = 1f, PaddingRight = 15f };
                    clLeyIVA.Padding = 5f;
                    clLeyIVA.BorderWidthTop = 1f;
                    clLeyIVA.BorderWidthBottom = 1f;
                    clLeyIVA.BackgroundColor = new BaseColor(229, 232, 232);
                    tblLeyIVA.AddCell(clLeyIVA);
                    documentPDF.Add(tblLeyIVA);

                    //CREAR BLOQUE DE COMPLEMENTOS DE AEROLINEA.
                    documentPDF.Add(InsertaSaltoLinea(6f));
                    documentPDF.Add(CrearTituloBloque("COMPLEMENTO AEROLINEAS", fuenteTituloBloque));
                    documentPDF.Add(InsertaSaltoLinea(2f));
                    bool mostrarTUA = dtCargo != null && dtCargo.Select("CodigoCargo = 'TUA'").Count() > 0;
                    PdfPTable tblComplementoTUA = CrearBloqueComplementosTUA(fuenteTotales, dtOtrosCargos, drTua, mostrarTUA);

                    documentPDF.Add(tblComplementoTUA);
                    documentPDF.Add(InsertaSaltoLinea(2f));

                    if (dtCargo != null && dtCargo.Rows.Count > 0)
                    {
                        PdfPTable tblComplementoAero = CrearBloqueComplementosAero(fuenteValConceptos, dtCargo);
                        documentPDF.Add(tblComplementoAero);
                    }

                    //CREAR BLOQUE DE COMPLEMENTOS DE AEROLINEA.
                    documentPDF.Add(InsertaSaltoLinea(6f));
                    documentPDF.Add(CrearTituloBloque("DATOS TIMBRADO CFDI", fuenteTituloBloque));
                    documentPDF.Add(InsertaSaltoLinea(2f));


                    PdfPTable tblTimbrado = new PdfPTable(2) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
                    float[] widthsTim = new float[] { 100f, 500f };
                    tblTimbrado.SetWidths(widthsTim);

                    string urlValidacionCFDI = ListaParametros.Where(x => x.Nombre == "UrlValidacionCFDI").FirstOrDefault().Valor;

                    if (urlValidacionCFDI.Length == 0)
                    {
                        urlValidacionCFDI = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx";
                    }

                    System.Text.StringBuilder codigoBi = new System.Text.StringBuilder();
                    codigoBi.Append(urlValidacionCFDI);
                    codigoBi.Append("?id=" + drDatosTfd["UUID"].ToString());
                    codigoBi.Append("&re=" + drDatosEmisor["rfc"].ToString());
                    codigoBi.Append("&rr=" + drDatosReceptor["rfc"].ToString());
                    codigoBi.Append("&tt=" + drDatosComprobante["total"].ToString());
                    string sello = drDatosComprobante["Sello"].ToString();
                    string ult8carSello = sello.Substring(sello.Length - 8, 8);
                    codigoBi.Append("&fe=" + ult8carSello);

                    BarcodeQRCode qrcodes = new BarcodeQRCode(codigoBi.ToString(), 1, 1, null);
                    iTextSharp.text.Image imgCB = qrcodes.GetImage();
                    imgCB.ScaleAbsolute(92, 92);

                    PdfPCell celTim = new PdfPCell(imgCB) { VerticalAlignment = Element.ALIGN_TOP, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0 };
                    tblTimbrado.AddCell(celTim);

                    celTim = new PdfPCell(CrearBloqueTimbrado("TIMBRADO", fuenteTimbrado, dtTimbreFiscalDigital, dtComprobante, cadenaOriginal)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_RIGHT, BorderWidth = 0 };
                    tblTimbrado.AddCell(celTim);
                    documentPDF.Add(tblTimbrado);

                    //Agregar leyenda de que es una representacion grafica del CFDI
                    documentPDF.Add(InsertaSaltoLinea(6f));
                    string textoRepresentacionGraficaCFDI = "Este documento es una representación impresa de un CFDI";
                    documentPDF.Add(CrearLeyendaFija(textoRepresentacionGraficaCFDI, fuenteLeyRepGrafica, Element.ALIGN_CENTER));

                    //Cerramos el documento
                    documentPDF.CloseDocument();
                    documentPDF.Close();
                    writer.Close();
                    fileStrem.Close();
                    fileStrem.Dispose();
                    documentPDF.Dispose();
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "GeneraPDFFactura33");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;


        }

        #endregion

        #region Metodos Factura Version 3.3
        #region Metodos Privados Factura Version 3.3
        private PdfPTable CrearTituloBloque(string titulo, Font fuente)
        {

            PdfPTable tblEncabezado = new PdfPTable(1);
            try
            {
                tblEncabezado.WidthPercentage = 100;
                fuente.Size = 10;
                PdfPCell clTitulo = new PdfPCell(new Phrase(titulo, fuente));

                clTitulo.BorderWidth = 0;
                clTitulo.BackgroundColor = new BaseColor(218, 247, 166);
                clTitulo.HorizontalAlignment = Element.ALIGN_LEFT;
                tblEncabezado.AddCell(clTitulo);
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearTituloBloque");
                throw new ExceptionViva(mensajeUsuario);
            }
            return tblEncabezado;
        }

        private Paragraph InsertaSaltoLinea(float alturaSalto)
        {
            Paragraph result = new Paragraph(string.Format("\n"), new Font(iTextSharp.text.Font.FontFamily.HELVETICA, alturaSalto, Font.NORMAL));
            return result;
        }

        private Image InsertaImagen(string rutaImagen, string nombreImagen, float anchoImagen)
        {
            Image imagen;
            try
            {
                imagen = iTextSharp.text.Image.GetInstance(rutaImagen + @"\" + nombreImagen);
                imagen.BorderWidth = 0;
                imagen.Alignment = Element.ALIGN_LEFT;
                float percentage = 0.0f;
                percentage = anchoImagen / imagen.Width;
                imagen.ScalePercent(percentage * 100);
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "InsertarImagen");
                throw new ExceptionViva(mensajeUsuario);
            }
            return imagen;
        }

        private string RecuperarTituloDato(string nomColumna, string bloque)
        {
            string result = "";
            try
            {
                Dictionary<string, string> listaTitulosCol = new Dictionary<string, string>();
                switch (bloque.ToUpper())
                {
                    case "EMISOR":
                        listaTitulosCol.Add("Rfc", "RFC");
                        listaTitulosCol.Add("Nombre", "RAZÓN SOCIAL");
                        listaTitulosCol.Add("RegimenFiscal", "REGIMEN FISCAL");
                        break;
                    case "RECEPTOR":
                        listaTitulosCol.Add("Rfc", "RFC");
                        listaTitulosCol.Add("UsoCFDI", "Uso del CFDI");
                        listaTitulosCol.Add("NumRegIdTrib", "Tax Id");
                        listaTitulosCol.Add("ResidenciaFiscal", "Residencia Fiscal");
                        break;

                    case "COMPROBANTE":
                        listaTitulosCol.Add("Version", "Versión");
                        listaTitulosCol.Add("Fecha", "Fecha Emisión");
                        listaTitulosCol.Add("FormaPago", "Forma de Pago");
                        listaTitulosCol.Add("CondicionesDePago", "Condiciones de Pago");
                        listaTitulosCol.Add("TipoDeComprobante", "Tipo de Comprobante");
                        listaTitulosCol.Add("MetodoPago", "Metodo de Pago");
                        listaTitulosCol.Add("LugarExpedicion", "Lugar de Expedición");

                        break;

                    case "TIMBRADO":
                        listaTitulosCol.Add("Version", "Versión");
                        listaTitulosCol.Add("UUID", "Folio Fiscal");
                        listaTitulosCol.Add("FechaTimbrado", "Fecha y Hora de Certificación");
                        listaTitulosCol.Add("RfcProvCertif", "RFC Proveedor Certificado");
                        listaTitulosCol.Add("NoCertificadoSAT", "No. de Serie del Certificado del SAT");
                        listaTitulosCol.Add("SelloSAT", "Sello Digital del SAT");
                        listaTitulosCol.Add("NoCertificado", "No. de Serie del Certificado del Emisor");
                        listaTitulosCol.Add("Sello", "Sello Digital del Emisor");

                        break;

                    default:
                        break;
                }


                if (listaTitulosCol.ContainsKey(nomColumna))
                {
                    result = listaTitulosCol[nomColumna];
                }
                else
                {
                    result = nomColumna;
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "RecuperarTituloDato");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result;
        }

        private string RecuperarDescSAT(string cveValor, string dato)
        {
            string result = "";
            try
            {
                Dictionary<string, string> listaCodigoSAT = new Dictionary<string, string>();
                listaCodigoSAT.Add("RegimenFiscal", "c_RegimenFiscal");
                listaCodigoSAT.Add("FormaPago", "c_FormaPago");
                listaCodigoSAT.Add("TipoDeComprobante", "c_TipoDeComprobante");
                listaCodigoSAT.Add("MetodoPago", "c_MetodoPago");
                listaCodigoSAT.Add("UsoCFDI", "c_UsoCFDI");
                listaCodigoSAT.Add("ResidenciaFiscal", "c_Pais");

                if (listaCodigoSAT.ContainsKey(cveValor))
                {
                    string nombreTabla = "";
                    nombreTabla = listaCodigoSAT[cveValor];
                    result = RecDescripcionSAT(nombreTabla, dato);
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "RecuperarDescSAT");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }

        private PdfPTable CrearBloqueDatos(string bloque, Font fuente, DataTable dtDatos)
        {
            PdfPTable tblDatos = new PdfPTable(3);
            try
            {
                tblDatos.WidthPercentage = 100;
                float[] widths = new float[] { 100f, 10f, 500f };
                tblDatos.SetWidths(widths);

                List<string> camposOmitidos = new List<string>();
                camposOmitidos = RecCamposOmitidosPorBloque(bloque);

                DataRow drDatos = dtDatos.Rows[0];

                foreach (DataColumn drCol in dtDatos.Columns)
                {
                    string nombreCol = drCol.ColumnName;
                    if (!nombreCol.Contains("_") && !camposOmitidos.Contains(nombreCol))
                    {
                        string valorDato = drDatos.IsNull(nombreCol) ? "" : drDatos[nombreCol].ToString();
                        string tituloColumna = RecuperarTituloDato(nombreCol, bloque);
                        string descCatSAT = RecuperarDescSAT(nombreCol, valorDato);

                        PdfPCell clLeyenda = new PdfPCell(new Phrase(tituloColumna, fuente));
                        clLeyenda.BorderWidth = 0;
                        clLeyenda.BackgroundColor = BaseColor.WHITE;
                        clLeyenda.HorizontalAlignment = Element.ALIGN_LEFT;
                        clLeyenda.VerticalAlignment = Element.ALIGN_TOP;
                        clLeyenda.Padding = 1;

                        PdfPCell clSep = new PdfPCell(new Phrase(":", fuente));
                        clSep.BorderWidth = 0;
                        clSep.BackgroundColor = BaseColor.WHITE;
                        clSep.HorizontalAlignment = Element.ALIGN_CENTER;
                        clSep.VerticalAlignment = Element.ALIGN_TOP;
                        clSep.Padding = 1;

                        string valorMostrar = "";
                        valorMostrar = string.Format("{0} {1}", valorDato, descCatSAT);

                        PdfPCell clValor = new PdfPCell(new Phrase(valorMostrar, fuente));
                        clValor.BorderWidth = 0;
                        clValor.BackgroundColor = BaseColor.WHITE;
                        clValor.HorizontalAlignment = Element.ALIGN_LEFT;
                        clValor.VerticalAlignment = Element.ALIGN_TOP;
                        clValor.Padding = 1;

                        tblDatos.AddCell(clLeyenda);
                        tblDatos.AddCell(clSep);
                        tblDatos.AddCell(clValor);
                    }
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueDatos");
                throw new ExceptionViva(mensajeUsuario);
            }

            return tblDatos;
        }

        private PdfPTable CrearBloqueReceptor(string bloque, Font fuente, DataTable dtDatos)
        {
            PdfPTable tblDatos;
            try
            {
                List<string> camposOmitidos = new List<string>();
                camposOmitidos = RecCamposOmitidosPorBloque(bloque);

                int numCampos = 0;
                foreach (DataColumn drCol in dtDatos.Columns)
                {
                    string nombreCol = drCol.ColumnName;
                    if (!nombreCol.Contains("_") && !camposOmitidos.Contains(nombreCol))
                    {
                        numCampos++;
                    }
                }

                int numColumnas = numCampos <= 2 ? 3 : 6;

                tblDatos = new PdfPTable(numColumnas);
                tblDatos.WidthPercentage = 100;

                float[] widths;
                if (numColumnas == 3)
                {
                    widths = new float[] { 100f, 10f, 500f };
                }
                else
                {
                    widths = new float[] { 100f, 10f, 200f, 100f, 10f, 200f };
                }

                tblDatos.SetWidths(widths);


                DataRow drDatos = dtDatos.Rows[0];

                foreach (DataColumn drCol in dtDatos.Columns)
                {
                    string nombreCol = drCol.ColumnName;
                    if (!nombreCol.Contains("_") && !camposOmitidos.Contains(nombreCol))
                    {
                        string valorDato = drDatos.IsNull(nombreCol) ? "" : drDatos[nombreCol].ToString();
                        string tituloColumna = RecuperarTituloDato(nombreCol, bloque);
                        string descCatSAT = RecuperarDescSAT(nombreCol, valorDato);

                        PdfPCell clLeyenda = new PdfPCell(new Phrase(tituloColumna, fuente));
                        clLeyenda.BorderWidth = 0;
                        clLeyenda.BackgroundColor = BaseColor.WHITE;
                        clLeyenda.HorizontalAlignment = Element.ALIGN_LEFT;
                        clLeyenda.VerticalAlignment = Element.ALIGN_TOP;
                        clLeyenda.Padding = 1;

                        PdfPCell clSep = new PdfPCell(new Phrase(":", fuente));
                        clSep.BorderWidth = 0;
                        clSep.BackgroundColor = BaseColor.WHITE;
                        clSep.HorizontalAlignment = Element.ALIGN_CENTER;
                        clSep.VerticalAlignment = Element.ALIGN_TOP;
                        clSep.Padding = 1;

                        string valorMostrar = "";
                        valorMostrar = string.Format("{0} {1}", valorDato, descCatSAT);

                        PdfPCell clValor = new PdfPCell(new Phrase(valorMostrar, fuente));
                        clValor.BorderWidth = 0;
                        clValor.BackgroundColor = BaseColor.WHITE;
                        clValor.HorizontalAlignment = Element.ALIGN_LEFT;
                        clValor.VerticalAlignment = Element.ALIGN_TOP;
                        clValor.Padding = 1;

                        tblDatos.AddCell(clLeyenda);
                        tblDatos.AddCell(clSep);
                        tblDatos.AddCell(clValor);
                    }
                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueReceptor");
                throw new ExceptionViva(mensajeUsuario);
            }

            return tblDatos;
        }


        private PdfPTable CrearLeyendaFija(string leyenda, Font fuente, int alineacionHor)
        {
            PdfPTable tblLeyenda = new PdfPTable(1) { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
            try
            {
                PdfPCell clLeyenda = new PdfPCell(new Phrase(leyenda, fuente)) { VerticalAlignment = Element.ALIGN_TOP, HorizontalAlignment = Element.ALIGN_LEFT, BorderWidth = 0f, BorderWidthTop = 1f, PaddingRight = 15f };
                clLeyenda.Padding = 5f;
                clLeyenda.BorderWidthTop = 1f;
                clLeyenda.BorderWidthBottom = 1f;
                clLeyenda.BackgroundColor = new BaseColor(229, 232, 232);
                clLeyenda.HorizontalAlignment = alineacionHor;
                tblLeyenda.AddCell(clLeyenda);
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearLeyendaFija");
                throw new ExceptionViva(mensajeUsuario);
            }

            return tblLeyenda;

        }

        private PdfPTable CrearBloqueTimbrado(string bloque, Font fuente, DataTable dtTimbrado, DataTable dtComprobante, string cadenaOriginal)
        {
            PdfPTable tblDatos = new PdfPTable(3);
            try
            {
                tblDatos.WidthPercentage = 100;
                float[] widths = new float[] { 140f, 10f, 450f };
                tblDatos.SetWidths(widths);

                //Agregar cadena Original
                PdfPCell clLeyCadOri = new PdfPCell(new Phrase("Cadena Original", fuente));
                clLeyCadOri.BorderWidth = 0;
                clLeyCadOri.BackgroundColor = BaseColor.WHITE;
                clLeyCadOri.HorizontalAlignment = Element.ALIGN_LEFT;
                clLeyCadOri.VerticalAlignment = Element.ALIGN_TOP;
                clLeyCadOri.Padding = 1;

                PdfPCell clSepCadOri = new PdfPCell(new Phrase(":", fuente));
                clSepCadOri.BorderWidth = 0;
                clSepCadOri.BackgroundColor = BaseColor.WHITE;
                clSepCadOri.HorizontalAlignment = Element.ALIGN_CENTER;
                clSepCadOri.VerticalAlignment = Element.ALIGN_TOP;
                clSepCadOri.Padding = 1;

                PdfPCell clValorCadOri = new PdfPCell(new Phrase(cadenaOriginal, fuente));
                clValorCadOri.BorderWidth = 0;
                clValorCadOri.BackgroundColor = BaseColor.WHITE;
                clValorCadOri.HorizontalAlignment = Element.ALIGN_LEFT;
                clValorCadOri.VerticalAlignment = Element.ALIGN_TOP;
                clValorCadOri.Padding = 1;

                tblDatos.AddCell(clLeyCadOri);
                tblDatos.AddCell(clSepCadOri);
                tblDatos.AddCell(clValorCadOri);

                List<string> camposOmitidosTimbrado = new List<string>();
                camposOmitidosTimbrado = RecCamposOmitidosPorBloque("TIMBRADO");

                DataRow drDatos = dtTimbrado.Rows[0];

                foreach (DataColumn drCol in dtTimbrado.Columns)
                {
                    string nombreCol = drCol.ColumnName;
                    if (!nombreCol.Contains("_") && !camposOmitidosTimbrado.Contains(nombreCol))
                    {
                        string valorDato = drDatos.IsNull(nombreCol) ? "" : drDatos[nombreCol].ToString();
                        string tituloColumna = RecuperarTituloDato(nombreCol, bloque);
                        string descCatSAT = RecuperarDescSAT(nombreCol, valorDato);

                        PdfPCell clLeyenda = new PdfPCell(new Phrase(tituloColumna, fuente));
                        clLeyenda.BorderWidth = 0;
                        clLeyenda.BackgroundColor = BaseColor.WHITE;
                        clLeyenda.HorizontalAlignment = Element.ALIGN_LEFT;
                        clLeyenda.VerticalAlignment = Element.ALIGN_TOP;
                        clLeyenda.Padding = 1;

                        PdfPCell clSep = new PdfPCell(new Phrase(":", fuente));
                        clSep.BorderWidth = 0;
                        clSep.BackgroundColor = BaseColor.WHITE;
                        clSep.HorizontalAlignment = Element.ALIGN_CENTER;
                        clSep.VerticalAlignment = Element.ALIGN_TOP;
                        clSep.Padding = 1;

                        string valorMostrar = "";
                        valorMostrar = string.Format("{0} {1}", valorDato, descCatSAT);

                        PdfPCell clValor = new PdfPCell(new Phrase(valorMostrar, fuente));
                        clValor.BorderWidth = 0;
                        clValor.BackgroundColor = BaseColor.WHITE;
                        clValor.HorizontalAlignment = Element.ALIGN_LEFT;
                        clValor.VerticalAlignment = Element.ALIGN_TOP;
                        clValor.Padding = 1;

                        tblDatos.AddCell(clLeyenda);
                        tblDatos.AddCell(clSep);
                        tblDatos.AddCell(clValor);
                    }
                }

                List<string> camposOmitidosComprobante = new List<string>();
                camposOmitidosComprobante = RecCamposOmitidosPorBloque("COMPROBANTETIMBRADO");

                DataRow drDatosCom = dtComprobante.Rows[0];

                foreach (DataColumn drCol in dtComprobante.Columns)
                {
                    string nombreCol = drCol.ColumnName;
                    if (!nombreCol.Contains("_") && !camposOmitidosComprobante.Contains(nombreCol))
                    {
                        string valorDato = drDatosCom.IsNull(nombreCol) ? "" : drDatosCom[nombreCol].ToString();
                        string tituloColumna = RecuperarTituloDato(nombreCol, bloque);
                        string descCatSAT = RecuperarDescSAT(nombreCol, valorDato);

                        PdfPCell clLeyenda = new PdfPCell(new Phrase(tituloColumna, fuente));
                        clLeyenda.BorderWidth = 0;
                        clLeyenda.BackgroundColor = BaseColor.WHITE;
                        clLeyenda.HorizontalAlignment = Element.ALIGN_LEFT;
                        clLeyenda.VerticalAlignment = Element.ALIGN_TOP;
                        clLeyenda.Padding = 1;

                        PdfPCell clSep = new PdfPCell(new Phrase(":", fuente));
                        clSep.BorderWidth = 0;
                        clSep.BackgroundColor = BaseColor.WHITE;
                        clSep.HorizontalAlignment = Element.ALIGN_CENTER;
                        clSep.VerticalAlignment = Element.ALIGN_TOP;
                        clSep.Padding = 1;

                        string valorMostrar = "";
                        valorMostrar = string.Format("{0} {1}", valorDato, descCatSAT);

                        PdfPCell clValor = new PdfPCell(new Phrase(valorMostrar, fuente));
                        clValor.BorderWidth = 0;
                        clValor.BackgroundColor = BaseColor.WHITE;
                        clValor.HorizontalAlignment = Element.ALIGN_LEFT;
                        clValor.VerticalAlignment = Element.ALIGN_TOP;
                        clValor.Padding = 1;

                        tblDatos.AddCell(clLeyenda);
                        tblDatos.AddCell(clSep);
                        tblDatos.AddCell(clValor);
                    }
                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueTimbrado");
                throw new ExceptionViva(mensajeUsuario);
            }

            return tblDatos;
        }


        private PdfPTable CrearBloqueTotales(Font fuente, DataRow drDatosComprobante, DataTable dtIvas)
        {
            PdfPTable tblDatos = new PdfPTable(2);

            try
            {
                tblDatos.WidthPercentage = 100;
                float[] widths = new float[] { 80f, 120f };
                tblDatos.SetWidths(widths);

                //DATOS DEL SUBTOTAL
                tblDatos.AddCell(InsertarDatoCelda("SubTotal", fuente, Element.ALIGN_LEFT, false, 1f));
                tblDatos.AddCell(InsertarDatoCelda(Convert.ToDecimal(drDatosComprobante["SubTotal"].ToString()).ToString("$###,###,##0.00"), fuente, Element.ALIGN_RIGHT, false, 1f));

                //DATOS DEL DESCUENTO
                tblDatos.AddCell(InsertarDatoCelda("Descuento", fuente, Element.ALIGN_LEFT, false, 1f));

                String descuento = "";
                try
                {
                    descuento = drDatosComprobante["Descuento"].ToString();
                }
                catch
                {
                    descuento = "0.00";
                }
                tblDatos.AddCell(InsertarDatoCelda(Convert.ToDecimal(descuento).ToString("$###,###,##0.00"), fuente, Element.ALIGN_RIGHT, false, 1f));

                //DATOS DEL IVA
                //GENERA UNA LISTA DE LOS DIFERENTES TIPOS DE TASA
                List<string> listaTasas = new List<string>();
                foreach (DataRow drTras in dtIvas.Rows)
                {
                    string tipoFactor = drTras["TipoFactor"].ToString();
                    if (tipoFactor.ToUpper() == "TASA")
                    {
                        string tasa = drTras["TasaOCuota"].ToString();
                        if (!listaTasas.Contains(tasa))
                        {
                            listaTasas.Add(tasa);
                        }
                    }

                }

                //PINTA LOS IVAS
                var qry = listaTasas.OrderBy(x => x);

                foreach (var item in qry)
                {
                    decimal importeIva = 0;
                    decimal porcIva = 0;
                    porcIva = Convert.ToDecimal(item) * 100;

                    foreach (DataRow drItem in dtIvas.Select("TasaOCuota = '" + item + "'"))
                    {
                        if (drItem["Base"].ToString().Length == 0)
                        {
                            if (!string.IsNullOrEmpty(drItem["Importe"].ToString()))
                            {
                                importeIva += Convert.ToDecimal(drItem["Importe"].ToString());
                            }
                        }
                    }

                    tblDatos.AddCell(InsertarDatoCelda(String.Format("IVA ({0} % *)", porcIva.ToString("0.00")), fuente, Element.ALIGN_LEFT, false, 1f));
                    tblDatos.AddCell(InsertarDatoCelda(importeIva.ToString("$###,###,##0.00"), fuente, Element.ALIGN_RIGHT, false, 1f));
                    //tblDatos.AddCell(InsertarDatoCelda(importeIva.ToString("9999999, #0.00"), fuente, Element.ALIGN_RIGHT, false, 1f));

                }


                //DATOS DEL TOTAL
                PdfPCell clLeyTotal = InsertarDatoCelda("Total", fuente, Element.ALIGN_LEFT, true, 1f);
                PdfPCell clValTotal = InsertarDatoCelda(Convert.ToDecimal(drDatosComprobante["total"].ToString()).ToString("$###,###,##0.00"), fuente, Element.ALIGN_RIGHT, true, 1f);
                clLeyTotal.BorderWidthBottom = 0.75f;
                clLeyTotal.PaddingBottom = 3f;
                clValTotal.BorderWidthBottom = 0.75f;

                tblDatos.AddCell(clLeyTotal);
                tblDatos.AddCell(clValTotal);

                //DATOS DEL TIPO DE MONEDA
                string codMoneda = drDatosComprobante["Moneda"].ToString();
                string descMoneda = RecDescripcionSAT("c_Moneda", codMoneda);

                PdfPCell clLeyMoneda = InsertarDatoCelda("Moneda", fuente, Element.ALIGN_LEFT, false, 1f);
                PdfPCell clValMoneda = InsertarDatoCelda(string.Format("{0} {1}", codMoneda, descMoneda), fuente, Element.ALIGN_RIGHT, false, 1f);

                clLeyMoneda.PaddingTop = 5f;
                clValMoneda.PaddingTop = 5f;

                tblDatos.AddCell(clLeyMoneda);
                tblDatos.AddCell(clValMoneda);

                //DATOS DEL TIPO DE CAMBIO
                tblDatos.AddCell(InsertarDatoCelda("Tipo Cambio", fuente, Element.ALIGN_LEFT, false, 1f));
                if (drDatosComprobante.Table.Columns.Contains("TipoCambio"))
                {
                    tblDatos.AddCell(InsertarDatoCelda(Convert.ToDecimal(drDatosComprobante["TipoCambio"].ToString()).ToString("$###,##0.00"), fuente, Element.ALIGN_RIGHT, false, 1f));
                }

                else
                {
                    tblDatos.AddCell(InsertarDatoCelda(Convert.ToDecimal("1.00").ToString("$###,##0.00"), fuente, Element.ALIGN_RIGHT, false, 1f));
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueTotales");
                throw new ExceptionViva(mensajeUsuario);
            }
            return tblDatos;
        }



        private List<string> RecCamposOmitidosPorBloque(string bloque)
        {
            List<string> result = new List<string>();
            try
            {
                switch (bloque.ToUpper())
                {
                    case "COMPROBANTE":
                        result.Add("Serie");
                        result.Add("Folio");
                        result.Add("Moneda");
                        result.Add("TipoCambio");
                        result.Add("SubTotal");
                        result.Add("Total");
                        result.Add("NoCertificado");
                        result.Add("Certificado");
                        result.Add("Sello");
                        result.Add("Descuento");
                        break;
                    case "COMPROBANTETIMBRADO":
                        result.Add("Serie");
                        result.Add("Folio");
                        result.Add("Moneda");
                        result.Add("TipoCambio");
                        result.Add("SubTotal");
                        result.Add("Total");
                        result.Add("Version");
                        result.Add("Fecha");
                        result.Add("FormaPago");
                        result.Add("CondicionesDePago");
                        result.Add("TipoDeComprobante");
                        result.Add("MetodoPago");
                        result.Add("LugarExpedicion");
                        result.Add("Certificado");
                        break;
                    case "TIMBRADO":
                        result.Add("SelloCFD");
                        break;

                    default:
                        break;
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "RecCamposOmitidosPorBloque");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }

        private PdfPTable CrearDatosEmisor(Font fuente, DataTable dtDatos)
        {
            PdfPTable tblDatos = new PdfPTable(4);
            try
            {
                tblDatos.WidthPercentage = 100;
                float[] widths = new float[] { 300f, 100f, 10f, 190f };
                tblDatos.SetWidths(widths);

                DataRow drDatos = dtDatos.Rows[0];

                foreach (DataColumn drCol in dtDatos.Columns)
                {
                    string nombreCol = drCol.ColumnName;
                    if (!nombreCol.Contains("_"))
                    {
                        string valorDato = drDatos.IsNull(nombreCol) ? "" : drDatos[nombreCol].ToString();
                        string tituloColumna = RecuperarTituloDato(nombreCol, "EMISOR");
                        string descCatSAT = RecuperarDescSAT(nombreCol, valorDato);

                        PdfPCell clTab = new PdfPCell(new Phrase("", fuente));
                        clTab.BorderWidth = 0;
                        clTab.BackgroundColor = BaseColor.WHITE;

                        PdfPCell clLeyenda = new PdfPCell(new Phrase(tituloColumna, fuente));
                        clLeyenda.BorderWidth = 0;
                        clLeyenda.BackgroundColor = BaseColor.WHITE;
                        clLeyenda.HorizontalAlignment = Element.ALIGN_LEFT;
                        clLeyenda.VerticalAlignment = Element.ALIGN_TOP;
                        clLeyenda.Padding = 0;

                        PdfPCell clSep = new PdfPCell(new Phrase(":", fuente));
                        clSep.BorderWidth = 0;
                        clSep.BackgroundColor = BaseColor.WHITE;
                        clSep.HorizontalAlignment = Element.ALIGN_CENTER;
                        clSep.VerticalAlignment = Element.ALIGN_TOP;
                        clSep.Padding = 0;

                        string valorMostrar = "";
                        valorMostrar = string.Format("{0} {1}", valorDato, descCatSAT).ToUpper();

                        PdfPCell clValor = new PdfPCell(new Phrase(valorMostrar, fuente));
                        clValor.BorderWidth = 0;
                        clValor.BackgroundColor = BaseColor.WHITE;
                        clValor.HorizontalAlignment = Element.ALIGN_LEFT;
                        clValor.VerticalAlignment = Element.ALIGN_TOP;
                        clValor.Padding = 0;
                        tblDatos.AddCell(clTab);
                        tblDatos.AddCell(clLeyenda);
                        tblDatos.AddCell(clSep);
                        tblDatos.AddCell(clValor);
                    }
                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearDatosEmisor");
                throw new ExceptionViva(mensajeUsuario);
            }
            return tblDatos;
        }

        private PdfPTable CrearSerieFolio(Font fuente, DataTable dtDatos)
        {
            PdfPTable tblDatos = new PdfPTable(3);
            try
            {
                tblDatos.WidthPercentage = 100;
                float[] widths = new float[] { 80f, 5f, 120f };
                tblDatos.SetWidths(widths);

                DataRow drDatos = dtDatos.Rows[0];

                string serie = "";
                serie = "Serie: " + drDatos["Serie"].ToString().ToUpper();
                PdfPCell clSerie = new PdfPCell(new Phrase(serie, fuente));
                clSerie.BorderWidth = 0;
                clSerie.BackgroundColor = BaseColor.WHITE;
                clSerie.HorizontalAlignment = Element.ALIGN_RIGHT;
                clSerie.VerticalAlignment = Element.ALIGN_TOP;

                PdfPCell clEspacio2 = new PdfPCell(new Phrase("", fuente));
                clEspacio2.BorderWidth = 0;
                clEspacio2.BackgroundColor = BaseColor.WHITE;
                clEspacio2.HorizontalAlignment = Element.ALIGN_LEFT;
                clEspacio2.VerticalAlignment = Element.ALIGN_TOP;

                string folio = "";

                folio = "Factura No: " + drDatos["Folio"].ToString().ToUpper();
                PdfPCell clFolio = new PdfPCell(new Phrase(folio, fuente));

                clFolio.BorderWidth = 0;
                clFolio.BackgroundColor = BaseColor.WHITE;
                clFolio.HorizontalAlignment = Element.ALIGN_CENTER;
                clFolio.VerticalAlignment = Element.ALIGN_TOP;

                tblDatos.AddCell(clSerie);
                tblDatos.AddCell(clEspacio2);
                tblDatos.AddCell(clFolio);
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearSerieFolio");
                throw new ExceptionViva(mensajeUsuario);
            }

            return tblDatos;
        }


        private PdfPTable CrearBloqueConceptos(Font fuenteEnc, Font fuenteVal, Font fuenteFilasVacias, DataTable dtConceptos, DataTable dtConcepto, DataTable dtImpuestos, DataTable dtTraslados, DataTable dtTraslado)
        {
            PdfPTable tblDatos = new PdfPTable(13);
            try
            {
                tblDatos.WidthPercentage = 100;
                float[] widths = new float[] { 38f, 25f, 24f, 32f, 60f, 120f, 50f, 50f, 50f, 25f, 35f, 25f, 40f };
                tblDatos.SetWidths(widths);

                PdfPCell colCveServ = InsertarTituloCol("CVESERV", fuenteEnc);
                PdfPCell colNoId = InsertarTituloCol("NO.ID", fuenteEnc);
                PdfPCell colCant = InsertarTituloCol("CANT", fuenteEnc);
                PdfPCell colCveUni = InsertarTituloCol("CVE.UNI", fuenteEnc);
                PdfPCell colUnidad = InsertarTituloCol("UNIDAD", fuenteEnc);
                PdfPCell colDescripcion = InsertarTituloCol("DESCRIPCIÓN", fuenteEnc);
                PdfPCell colValorUni = InsertarTituloCol("VALOR UNI", fuenteEnc);
                colValorUni.HorizontalAlignment = Element.ALIGN_RIGHT;
                PdfPCell colImporte = InsertarTituloCol("IMPORTE", fuenteEnc);
                colImporte.HorizontalAlignment = Element.ALIGN_RIGHT;
                PdfPCell colBaseIVA = InsertarTituloCol("BASE IVA", fuenteEnc);
                colBaseIVA.HorizontalAlignment = Element.ALIGN_RIGHT;
                PdfPCell colImpto = InsertarTituloCol("IMPTO", fuenteEnc);
                PdfPCell colFactor = InsertarTituloCol("FACTOR", fuenteEnc);
                PdfPCell colTasa = InsertarTituloCol("TASA", fuenteEnc);
                PdfPCell colIVA = InsertarTituloCol("IMPT.IVA", fuenteEnc);
                colIVA.HorizontalAlignment = Element.ALIGN_RIGHT;


                tblDatos.AddCell(colCveServ);
                tblDatos.AddCell(colNoId);
                tblDatos.AddCell(colCant);
                tblDatos.AddCell(colCveUni);
                tblDatos.AddCell(colUnidad);
                tblDatos.AddCell(colDescripcion);
                tblDatos.AddCell(colValorUni);
                tblDatos.AddCell(colImporte);
                tblDatos.AddCell(colBaseIVA);
                tblDatos.AddCell(colImpto);
                tblDatos.AddCell(colFactor);
                tblDatos.AddCell(colTasa);
                tblDatos.AddCell(colIVA);


                int numFilasBloqueConcepto = 0;
                foreach (DataRow drConceptos in dtConceptos.Rows)
                {
                    string conceptosId = drConceptos["Conceptos_Id"].ToString();
                    int numFila = 0;
                    foreach (DataRow drConcepto in dtConcepto.Select("Conceptos_Id = " + conceptosId))
                    {
                        numFila++;
                        numFilasBloqueConcepto++;
                        bool esFilaPar = (numFila % 2 == 0);
                        string conceptoId = drConcepto["Concepto_Id"].ToString();

                        tblDatos.AddCell(InsertarDatoCelda(drConcepto["ClaveProdServ"].ToString(), fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                        tblDatos.AddCell(InsertarDatoCelda(drConcepto["NoIdentificacion"].ToString(), fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                        tblDatos.AddCell(InsertarDatoCelda(drConcepto["Cantidad"].ToString(), fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                        tblDatos.AddCell(InsertarDatoCelda(drConcepto["ClaveUnidad"].ToString(), fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                        tblDatos.AddCell(InsertarDatoCelda(drConcepto["Unidad"].ToString(), fuenteVal, Element.ALIGN_LEFT, esFilaPar, 2f));
                        tblDatos.AddCell(InsertarDatoCelda(drConcepto["Descripcion"].ToString(), fuenteVal, Element.ALIGN_LEFT, esFilaPar, 2f));
                        tblDatos.AddCell(InsertarDatoCelda(Convert.ToDecimal(drConcepto["ValorUnitario"].ToString()).ToString("$###,###,##0.00"), fuenteVal, Element.ALIGN_RIGHT, esFilaPar, 2f));
                        tblDatos.AddCell(InsertarDatoCelda(Convert.ToDecimal(drConcepto["Importe"].ToString()).ToString("$###,###,##0.00"), fuenteVal, Element.ALIGN_RIGHT, esFilaPar, 2f));

                        // Espacio que le vamos a restar a cada salto de línea
                        foreach (DataRow drImpuestos in dtImpuestos.Select("Concepto_Id = " + conceptoId))
                        {
                            string impuestosId = "";
                            impuestosId = drImpuestos["Impuestos_Id"].ToString();
                            foreach (DataRow drTraslados in dtTraslados.Select("Impuestos_Id = " + impuestosId))
                            {
                                string trasladosId = "";
                                trasladosId = drTraslados["Traslados_Id"].ToString();
                                int numfilasIVA = 0;
                                foreach (DataRow drIVA in dtTraslado.Select("Traslados_Id = " + trasladosId))
                                {
                                    if (numfilasIVA > 0)
                                    {
                                        tblDatos.AddCell(InsertarDatoCelda("", fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                        tblDatos.AddCell(InsertarDatoCelda("", fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                        tblDatos.AddCell(InsertarDatoCelda("", fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                        tblDatos.AddCell(InsertarDatoCelda("", fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                        tblDatos.AddCell(InsertarDatoCelda("", fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                        tblDatos.AddCell(InsertarDatoCelda("", fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                        tblDatos.AddCell(InsertarDatoCelda("", fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                        tblDatos.AddCell(InsertarDatoCelda("", fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                    }
                                    //bool flg_Hot = false;
                                    //foreach (var item in drConcepto.ItemArray)
                                    //{
                                    //    switch (item.ToString())
                                    //    {
                                    //        case "TARIFA HOTELERA":
                                    //            flg_Hot = true;
                                    //            break;
                                    //    }
                                    //}
                                    //if (flg_Hot)
                                    //{
                                    //tblDatos.AddCell(InsertarDatoCelda(Convert.ToDecimal(drConcepto["Importe"].ToString()).ToString("$###,###,##0.00"), fuenteVal, Element.ALIGN_RIGHT, esFilaPar, 2f));
                                    //tblDatos.AddCell(InsertarDatoCelda("002", fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                    //tblDatos.AddCell(InsertarDatoCelda("Tasa", fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                    //if(drIVA["tasa"].ToString() == "0")
                                    //{
                                    //    tblDatos.AddCell(InsertarDatoCelda("0.00", fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                    //}
                                    //else
                                    //{
                                    //    tblDatos.AddCell(InsertarDatoCelda("0.16", fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                    //}

                                    //tblDatos.AddCell(InsertarDatoCelda(drIVA.IsNull("importe") ? "" : Convert.ToDecimal(drIVA["importe"].ToString()).ToString("$###,###,##0.00"), fuenteVal, Element.ALIGN_RIGHT, esFilaPar, 2f));

                                    ////tblDatos.AddCell(InsertarDatoCelda(drIVA["impuesto"].ToString(), fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                    ////tblDatos.AddCell(InsertarDatoCelda(drIVA.IsNull("tasa") ? "" : Convert.ToDecimal(drIVA["tasa"].ToString()).ToString("0.00"), fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                    ////tblDatos.AddCell(InsertarDatoCelda(drIVA.IsNull("importe") ? "" : Convert.ToDecimal(drIVA["importe"].ToString()).ToString("$###,###,##0.00"), fuenteVal, Element.ALIGN_RIGHT, esFilaPar, 2f));
                                    //numFilasBloqueConcepto += numfilasIVA;
                                    //numfilasIVA++;
                                    //}
                                    //else
                                    //{
                                    if (drIVA["impuesto"].ToString() == "IVA")
                                    {

                                    }
                                    else
                                    {
                                        tblDatos.AddCell(InsertarDatoCelda(Convert.ToDecimal(drIVA["Base"].ToString()).ToString("$###,###,##0.00"), fuenteVal, Element.ALIGN_RIGHT, esFilaPar, 2f));
                                        tblDatos.AddCell(InsertarDatoCelda(drIVA["Impuesto"].ToString(), fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                        tblDatos.AddCell(InsertarDatoCelda(drIVA["TipoFactor"].ToString(), fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                        tblDatos.AddCell(InsertarDatoCelda(drIVA.IsNull("TasaOCuota") ? "" : Convert.ToDecimal(drIVA["TasaOCuota"].ToString()).ToString("0.00"), fuenteVal, Element.ALIGN_CENTER, esFilaPar, 2f));
                                        tblDatos.AddCell(InsertarDatoCelda(drIVA.IsNull("Importe") ? "" : Convert.ToDecimal(drIVA["Importe"].ToString()).ToString("$###,###,##0.00"), fuenteVal, Element.ALIGN_RIGHT, esFilaPar, 2f));
                                        numFilasBloqueConcepto += numfilasIVA;
                                        numfilasIVA++;
                                    }
                                   
                                    //}
                                }
                            }
                        }

                    }
                }

                int numFilasConceptoFijas = 6;

                while (numFilasConceptoFijas > numFilasBloqueConcepto)
                {
                    bool esFilaPar = false;
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteFilasVacias, Element.ALIGN_CENTER, esFilaPar, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteFilasVacias, Element.ALIGN_CENTER, esFilaPar, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteFilasVacias, Element.ALIGN_CENTER, esFilaPar, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteFilasVacias, Element.ALIGN_CENTER, esFilaPar, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteFilasVacias, Element.ALIGN_CENTER, esFilaPar, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteFilasVacias, Element.ALIGN_CENTER, esFilaPar, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteFilasVacias, Element.ALIGN_CENTER, esFilaPar, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteFilasVacias, Element.ALIGN_CENTER, esFilaPar, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteFilasVacias, Element.ALIGN_RIGHT, esFilaPar, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteFilasVacias, Element.ALIGN_CENTER, esFilaPar, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteFilasVacias, Element.ALIGN_CENTER, esFilaPar, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteFilasVacias, Element.ALIGN_CENTER, esFilaPar, 2f));
                    tblDatos.AddCell(InsertarDatoCelda(".", fuenteFilasVacias, Element.ALIGN_RIGHT, esFilaPar, 2f));
                    numFilasBloqueConcepto++;
                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueConceptos");
                throw new ExceptionViva(mensajeUsuario);
            }

            return tblDatos;
        }


        private PdfPTable CrearBloqueComplementosTUA(Font fuenteVal, DataTable dtOtrosCargos, DataRow drTUA, bool mostrarTUA)
        {
            PdfPTable tblDatos = new PdfPTable(3);
            try
            {
                tblDatos.WidthPercentage = 100;
                float[] widths = new float[] { 60f, 60f, 580f };
                tblDatos.SetWidths(widths);

                //DATOS DEL COMPLEMENTO DE AEROLINEAS
                String strTotalCargos = dtOtrosCargos != null ? dtOtrosCargos.Rows[0]["TotalCArgos"].ToString() : drTUA["TUA"].ToString();
                if (mostrarTUA)
                {
                    tblDatos.AddCell(InsertarDatoCelda("TUA:", fuenteVal, Element.ALIGN_LEFT, false, 2f));
                    //tblDatos.AddCell(InsertarDatoCelda(Convert.ToDecimal(drTUA["TUA"].ToString()).ToString("$###,###,##0.00"), fuenteVal, Element.ALIGN_RIGHT, false, 2f));
                    tblDatos.AddCell(InsertarDatoCelda(Convert.ToDecimal(strTotalCargos).ToString("$###,###,##0.00"), fuenteVal, Element.ALIGN_RIGHT, false, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteVal, Element.ALIGN_LEFT, false, 2f));
                }
                else
                {
                    tblDatos.AddCell(InsertarDatoCelda("Otros Cargos:", fuenteVal, Element.ALIGN_LEFT, false, 2f));
                    tblDatos.AddCell(InsertarDatoCelda(Convert.ToDecimal(strTotalCargos).ToString("$###,###,##0.00"), fuenteVal, Element.ALIGN_RIGHT, false, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteVal, Element.ALIGN_CENTER, false, 2f));
                }

                if (mostrarTUA == false && drTUA["TUA"] != null)
                {
                    tblDatos.AddCell(InsertarDatoCelda("TUA:", fuenteVal, Element.ALIGN_LEFT, false, 2f));
                    tblDatos.AddCell(InsertarDatoCelda(Convert.ToDecimal(drTUA["TUA"].ToString()).ToString("$###,###,##0.00"), fuenteVal, Element.ALIGN_RIGHT, false, 2f));
                    //tblDatos.AddCell(InsertarDatoCelda(Convert.ToDecimal(strTotalCargos).ToString("$###,###,##0.00"), fuenteVal, Element.ALIGN_RIGHT, false, 2f));
                    tblDatos.AddCell(InsertarDatoCelda("", fuenteVal, Element.ALIGN_LEFT, false, 2f));
                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueComplementosTUA");
                throw new ExceptionViva(mensajeUsuario);
            }
            return tblDatos;
        }

        private PdfPTable CrearBloqueComplementosAero(Font fuenteVal, DataTable dtCargo)
        {
            PdfPTable tblDatos;
            try
            {
                int numFilasBloque = 5;
                int numCodigos = dtCargo.Rows.Count;
                int numBloquesCol = (numCodigos / numFilasBloque);
                if (numBloquesCol == 0)
                {
                    numBloquesCol = 1;
                }
                float anchoTotalPag = 700f;

                int numColumnasTot = numBloquesCol * 2;
                tblDatos = new PdfPTable(numColumnasTot + 1);
                tblDatos.WidthPercentage = 100;

                float[] widths = new float[numColumnasTot + 1];

                float anchoAcum = 0;

                for (int i = 0; i < numBloquesCol; i++)
                {
                    widths[(i * 2) + 0] = 40f;
                    widths[(i * 2) + 1] = 80f;
                    anchoAcum += 40f;
                    anchoAcum += 80f;
                }

                widths[numColumnasTot] = anchoTotalPag - anchoAcum;

                tblDatos.SetWidths(widths);

                //DESGLOCE DE LOS IMPUESTOS COMO SON BS, DS, ETC.
                if (dtCargo != null)
                {
                    int numFila = 1;
                    int numBloquePintado = 0;
                    foreach (DataRow dtCargosAero in dtCargo.Rows)
                    {
                        bool esPar = (numFila % 2 == 0);
                        PdfPCell clLey = InsertarDatoCelda(dtCargosAero["CodigoCargo"].ToString(), fuenteVal, Element.ALIGN_RIGHT, esPar, 2f);
                        PdfPCell clVal = InsertarDatoCelda(Convert.ToDecimal(dtCargosAero["Importe"].ToString()).ToString("$###,###,##0.00"), fuenteVal, Element.ALIGN_RIGHT, esPar, 2f);
                        clLey.Padding = 2f;
                        clVal.Padding = 2f;

                        if (numFila == 1)
                        {
                            clLey.BorderWidthTop = 1f;
                            clLey.PaddingTop = 5f;
                            clVal.BorderWidthTop = 1f;
                            clVal.PaddingTop = 5f;

                        }

                        tblDatos.AddCell(clLey);
                        tblDatos.AddCell(clVal);
                        numBloquePintado++;
                        if (numBloquePintado % numBloquesCol == 0)
                        {
                            tblDatos.AddCell(InsertarDatoCelda("", fuenteVal, Element.ALIGN_LEFT, false, 2f));
                            numBloquePintado = 0;
                            numFila++;
                        }

                    }


                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueComplementosAero");
                throw new ExceptionViva(mensajeUsuario);
            }
            return tblDatos;
        }


        private PdfPCell InsertarTituloCol(string descEncCol, Font fuente)
        {
            PdfPCell clEncCol = new PdfPCell(new Phrase(descEncCol, fuente));
            try
            {
                clEncCol.BorderWidth = 0f;
                clEncCol.BackgroundColor = new BaseColor(229, 232, 232);
                clEncCol.BorderWidthBottom = 0.75f;
                clEncCol.HorizontalAlignment = Element.ALIGN_CENTER;
                clEncCol.VerticalAlignment = Element.ALIGN_MIDDLE;
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "InsertarTituloCol");
                throw new ExceptionViva(mensajeUsuario);
            }
            return clEncCol;
        }

        private PdfPCell InsertarDatoCelda(string valor, Font fuente, int alineacion, bool filaPar, float padding)
        {
            PdfPCell clDatoCel = new PdfPCell(new Phrase(valor, fuente));
            try
            {
                clDatoCel.BorderWidth = 0;
                clDatoCel.BackgroundColor = filaPar ? new BaseColor(244, 246, 246) : BaseColor.WHITE;
                clDatoCel.BorderWidth = 0f;
                clDatoCel.HorizontalAlignment = alineacion;
                clDatoCel.VerticalAlignment = Element.ALIGN_MIDDLE;
                clDatoCel.Padding = padding;
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "InsertarDatoCelda");
                throw new ExceptionViva(mensajeUsuario);
            }
            return clDatoCel;
        }


        private string RecDescripcionSAT(string catalogoSAT, string codigo)
        {
            string result = "";
            try
            {
                string codigoCatSAT = "";
                ENTGencatalogosCat entCatSAT = new ENTGencatalogosCat();
                entCatSAT = ListaGenCatalogos.Where(x => x.Nombre.ToUpper() == catalogoSAT.ToUpper()).FirstOrDefault();
                if (entCatSAT != null)
                {
                    codigoCatSAT = entCatSAT.CveTabla;
                    ENTGendescripcionesCat entDesc = new ENTGendescripcionesCat();
                    entDesc = ListaGenDescripciones.Where(x => x.CveTabla == codigoCatSAT && x.CveValor == codigo).FirstOrDefault();
                    if (entDesc != null)
                    {
                        result = entDesc.Descripcion;
                    }
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "RecDescripcionSAT");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }

        #endregion


        #endregion

        #region Metodos Factura Version 3.2
        public string GeneraPDF32(string xmlCFDI, string pnr)
        {
            string result = "";

            string rutaImagenes = ListaParametros.Where(x => x.Nombre == "RutaImagenesPDF32").FirstOrDefault().Valor;
            string rutaArchivosFac = ListaParametros.Where(x => x.Nombre == "RutaArchivosFactura").FirstOrDefault().Valor;
            string rutaLogo = Path.Combine(rutaImagenes, "logo.png");
            string rutaRemate1 = Path.Combine(rutaImagenes, "remate_1.jpg");
            string rutaRemate2 = Path.Combine(rutaImagenes, "remate_2.jpg");

            try
            {
                PNR = pnr;
                string Respuesta = string.Empty;
                string totalCargos = string.Empty;

                //Declaramos el Tipo de letra que utilizaremos para el Documento PDF
                BaseFont baseTimes = BaseFont.CreateFont(Path.Combine(RutaFuentePDF, "Times.ttf"), BaseFont.WINANSI, true);
                BaseFont f_cb = BaseFont.CreateFont(Path.Combine(RutaFuentePDF, "Timesbd.ttf"), BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                BaseFont f_cn = BaseFont.CreateFont(Path.Combine(RutaFuentePDF, "Times.ttf"), BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                BaseFont bf = FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC, Font.DEFAULTSIZE).GetCalculatedBaseFont(false);

                Font fuenteFactura = new Font(baseTimes, 7, Font.NORMAL);

                //Convertimos la respuesta de pegaso en un documento de XML
                XmlDocument xmlDocuResponse = new XmlDocument();
                xmlDocuResponse.LoadXml(xmlCFDI);
                XmlNodeReader xmlReader = new XmlNodeReader(xmlDocuResponse);

                DataSet dsRespuesta = new DataSet();
                dsRespuesta.ReadXml(xmlReader);

                //Recuperar informacion de cada tabla en el timbrado
                DataTable dtGetDatosEmisor = dsRespuesta.Tables["Emisor"];//OK drDatosEmisor
                DataTable dtGetDatosDomicilioFiscal = dsRespuesta.Tables["DomicilioFiscal"];
                DataTable dtGetDatosExpedidoEn = dsRespuesta.Tables["ExpedidoEn"];
                DataTable dtGetDatosRegimenFiscal = dsRespuesta.Tables["RegimenFiscal"];
                DataTable dtGetDatosReceptor = dsRespuesta.Tables["Receptor"];
                DataTable dtGetDatosDomReceptor = dsRespuesta.Tables["Domicilio"];
                DataTable dtGetDatosTimbrado = dsRespuesta.Tables["TimbreFiscalDigital"];
                DataTable dtGetDatosComprobante = dsRespuesta.Tables["Comprobante"];
                DataTable dtGetDatosConcepto = dsRespuesta.Tables["Concepto"];
                DataTable dtGetTraslado = dsRespuesta.Tables["Traslado"];
                DataTable dtGetTua = dsRespuesta.Tables["Aerolineas"];
                DataTable dtGetTotalCargos = dsRespuesta.Tables["OtrosCargos"];
                DataTable dtGetCargo = dsRespuesta.Tables["Cargo"];


                //Referencias a las tablas del XML Del Formato CFDI
                DataRow drDatosTfd = dtGetDatosTimbrado.Rows.Count > 0 ? dtGetDatosTimbrado.Rows[0] : null;
                DataRow drDatosEmisor = dtGetDatosEmisor.Rows.Count > 0 ? dtGetDatosEmisor.Rows[0] : null;
                DataRow drDatosDomicilio = dtGetDatosDomicilioFiscal.Rows.Count > 0 ? dtGetDatosDomicilioFiscal.Rows[0] : null;
                DataRow drDatosExpedido = dtGetDatosExpedidoEn.Rows.Count > 0 ? dtGetDatosExpedidoEn.Rows[0] : null;
                DataRow drDatosRegimenF = dtGetDatosRegimenFiscal.Rows.Count > 0 ? dtGetDatosRegimenFiscal.Rows[0] : null;
                DataRow drDatosReceptor = dtGetDatosReceptor.Rows.Count > 0 ? dtGetDatosReceptor.Rows[0] : null;
                DataRow drDatosDomReceptor = dtGetDatosDomReceptor.Rows.Count > 0 ? dtGetDatosDomReceptor.Rows[0] : null;
                DataRow drDatosComprobante = dtGetDatosComprobante.Rows.Count > 0 ? dtGetDatosComprobante.Rows[0] : null;
                DataRow drTua = dtGetTua.Rows.Count > 0 ? dtGetTua.Rows[0] : null;

                string metodoPago = RecuperarValorDR(drDatosComprobante, "metodoDePago");
                string tipoCambio = RecuperarValorDR(drDatosComprobante, "TipoCambio");
                string moneda = RecuperarValorDR(drDatosComprobante, "Moneda");


                StringBuilder cadenaOriginal = new StringBuilder();

                cadenaOriginal.Append("||");
                cadenaOriginal.Append(RecuperarValorDR(drDatosTfd, "Version").Trim());
                cadenaOriginal.Append("|");
                cadenaOriginal.Append(RecuperarValorDR(drDatosTfd, "UUID").Trim());
                cadenaOriginal.Append("|");
                cadenaOriginal.Append(RecuperarValorDR(drDatosTfd, "FechaTimbrado").Trim());
                cadenaOriginal.Append("|");
                cadenaOriginal.Append(RecuperarValorDR(drDatosTfd, "SelloCFD").Trim());
                cadenaOriginal.Append("|");
                cadenaOriginal.Append(RecuperarValorDR(drDatosTfd, "NoCertificadoSAT").Trim());
                cadenaOriginal.Append("||");


                if (dtGetTotalCargos != null)
                {
                    DataRow drTotalCargos = dtGetTotalCargos.Rows[0];

                    totalCargos = Convert.ToDecimal(RecuperarValorDR(drTotalCargos, "TotalCargos")).ToString("#,##0.00");
                }

                DataRow drTraslado = dtGetTraslado.Rows[0];

                //Referencias a las tablas del XML Del Formato CFDI
                string rutaPDF = rutaArchivosFac;
                string nombreCarpetaToday = string.Format("{0:dd-MM-yyyy}", DateTime.Today).ToUpper();
                string folio = RecuperarValorDR(drDatosComprobante, "Folio");
                string ruta = Path.Combine(rutaPDF, nombreCarpetaToday, pnr, folio);

                DirectoryInfo carpeta = new DirectoryInfo(ruta);

                if (!carpeta.Exists)
                    carpeta.Create();


                result = Path.Combine(ruta, string.Format("PDF_{0}.pdf", folio));

                using (System.IO.FileStream fileStrem = new FileStream(result, FileMode.Create))
                {
                    Document documentPDF = new Document(PageSize.LETTER);
                    PdfWriter writer = PdfWriter.GetInstance(documentPDF, fileStrem);

                    //Abrimos el documento para comenzar a escribir
                    documentPDF.Open();

                    PdfContentByte cb = writer.DirectContent;
                    cb.SetFontAndSize(f_cn, 9);

                    //Activar la escruitura
                    cb.BeginText();
                    cb.SetColorFill(iTextSharp.text.BaseColor.WHITE);
                    writeText(cb, "CLIENTE:", 38, 655, bf, 12);

                    cb.SetTextMatrix(38, 577);
                    cb.ShowText("Productos/Servicios");

                    cb.SetTextMatrix(415, 762);

                    cb.ShowText("FACTURA No. " + RecuperarValorDR(drDatosComprobante, "folio"));

                    //Indicaremos las posiciones X y Y de nuestro texto
                    cb.SetColorFill(iTextSharp.text.BaseColor.BLACK);
                    writeText(cb, RecuperarValorDR(drDatosEmisor, "nombre"), 165, 741, f_cn, 8);

                    cb.SetTextMatrix(165, 732);
                    cb.ShowText("R.F.C: " + RecuperarValorDR(drDatosEmisor, "rfc").ToString());

                    writeText(cb, RecuperarValorDR(drDatosDomicilio, "calle") + " " + RecuperarValorDR(drDatosDomicilio, "noExterior"), 165, 723, f_cn, 8);

                    cb.SetTextMatrix(165, 714);
                    cb.ShowText(RecuperarValorDR(drDatosDomicilio, "colonia"));

                    cb.SetTextMatrix(165, 705);
                    cb.ShowText(RecuperarValorDR(drDatosDomicilio, "municipio") + ", " + RecuperarValorDR(drDatosDomicilio, "estado") +
                                " C.P " + RecuperarValorDR(drDatosDomicilio, "codigoPostal"));

                    cb.SetTextMatrix(165, 696);
                    cb.ShowText(RecuperarValorDR(drDatosDomicilio, "pais"));

                    cb.SetTextMatrix(165, 687);
                    cb.ShowText("Régimen fiscal: " + RecuperarValorDR(drDatosRegimenF, "Regimen"));

                    cb.SetTextMatrix(165, 678);
                    cb.ShowText("Lugar, Fecha y Hora de Emisión: " + RecuperarValorDR(drDatosDomicilio, "municipio") + ", " +
                                 RecuperarValorDR(drDatosDomicilio, "estado") + " A " + RecuperarValorDR(drDatosComprobante, "fecha"));

                    /**********************************************************
                    **  Datos del cliente obtenidos desde la base de datos.  
                    ***********************************************************/
                    if (dtGetDatosReceptor.Columns.Contains("nombre"))
                    {
                        cb.SetTextMatrix(26, 640);
                        cb.ShowText(RecuperarValorDR(drDatosReceptor, "nombre"));

                        cb.SetTextMatrix(26, 631);
                        cb.ShowText("RFC: " + RecuperarValorDR(drDatosReceptor, "rfc"));
                    }
                    else
                    {
                        cb.SetTextMatrix(26, 640);
                        cb.ShowText("RFC: " + RecuperarValorDR(drDatosReceptor, "rfc"));
                    }

                    if (dtGetDatosDomReceptor.Columns.Contains("calle"))
                    {
                        cb.SetTextMatrix(26, 622);
                        cb.ShowText(RecuperarValorDR(drDatosDomReceptor, "calle"));
                    }

                    if (dtGetDatosDomReceptor.Columns.Contains("colonia"))
                    {
                        cb.SetTextMatrix(26, 613);
                        cb.ShowText(RecuperarValorDR(drDatosDomReceptor, "colonia"));
                    }

                    string localidad = "";
                    if (dtGetDatosDomReceptor.Columns.Contains("municipio"))
                    {
                        localidad = RecuperarValorDR(drDatosDomReceptor, "municipio");
                    }

                    string estado = "";
                    if (dtGetDatosDomReceptor.Columns.Contains("estado"))
                    {
                        estado = RecuperarValorDR(drDatosDomReceptor, "estado");
                    }

                    string codigoPostal = "";
                    if (dtGetDatosDomReceptor.Columns.Contains("codigoPostal"))
                    {
                        codigoPostal = RecuperarValorDR(drDatosDomReceptor, "codigoPostal");
                    }

                    cb.SetTextMatrix(26, 604);
                    cb.ShowText(localidad + ' ' + estado + " C.P " +
                                codigoPostal);

                    cb.SetTextMatrix(26, 595);
                    cb.ShowText(RecuperarValorDR(drDatosDomReceptor, "pais").ToUpper());

                    cb.SetTextMatrix(40, 558);
                    cb.ShowText("Cantidad");

                    cb.SetTextMatrix(109, 558);
                    cb.ShowText("Concepto");

                    cb.SetTextMatrix(293, 558);
                    cb.ShowText("Unidad de Medida");

                    cb.SetTextMatrix(385, 558);
                    cb.ShowText("Precio Unitario");

                    cb.SetTextMatrix(452, 558);
                    cb.ShowText("Importe");

                    int top_margin = 549;

                    foreach (DataRow drItem in dtGetDatosConcepto.Rows)
                    {
                        writeText(cb, RecuperarValorDR(drItem, "cantidad"), 53, top_margin, f_cn, 8);
                        writeText(cb, RecuperarValorDR(drItem, "descripcion"), 109, top_margin, f_cn, 8);
                        writeText(cb, RecuperarValorDR(drItem, "unidad"), 293, top_margin, f_cn, 8);
                        writeText(cb, "$ " + Convert.ToDecimal(RecuperarValorDR(drItem, "valorUnitario")).ToString("#,##0.00"), 385, top_margin, f_cn, 8);
                        writeText(cb, "$ " + Convert.ToDecimal(RecuperarValorDR(drItem, "importe")).ToString("#,##0.00"), 452, top_margin, f_cn, 8);
                        // Espacio que le vamos a restar a cada salto de línea
                        top_margin -= 9;
                    }

                    cb.SetTextMatrix(385, 349);
                    cb.ShowText("Subtotal");

                    cb.SetTextMatrix(459, 349);
                    cb.ShowText("$" + Convert.ToDecimal(RecuperarValorDR(drDatosComprobante, "subTotal")).ToString("#,##0.00"));
                    int top_marginIva = 340;

                    foreach (DataRow drItem in dtGetTraslado.Rows)
                    {
                        writeText(cb, "IVA (16.00 % *)", 385, top_marginIva, f_cn, 8);
                        writeText(cb, "$" + Convert.ToDecimal(RecuperarValorDR(drItem, "importe")).ToString("#,##0.00"), 459, top_marginIva, f_cn, 8);

                        // Espacio que le vamos a restar a cada salto de línea
                        top_marginIva -= 9;
                    }

                    FormatoNumLetra ConvertirNumLetra = new FormatoNumLetra();
                    string numero = RecuperarValorDR(drDatosComprobante, "total");

                    cb.SetTextMatrix(26, top_marginIva);
                    cb.ShowText("Importe con Letra: " + ConvertirNumLetra.Convertir(numero, true, moneda.ToString()));

                    cb.SetTextMatrix(385, top_marginIva);
                    cb.ShowText("Total");

                    cb.SetTextMatrix(459, top_marginIva);
                    cb.ShowText("$" + Convert.ToDecimal(RecuperarValorDR(drDatosComprobante, "total")).ToString("#,##0.00"));

                    cb.SetTextMatrix(385, 322);
                    cb.ShowText("Moneda");

                    cb.SetTextMatrix(459, 322);
                    cb.ShowText(moneda.ToString());

                    for (var i = 26; i <= 570; i++)
                    {
                        cb.SetLineWidth(0.8f);
                        cb.MoveTo(26, 308);
                        cb.LineTo(i, 308);

                    }

                    cb.SetTextMatrix(26, 297);
                    cb.SetFontAndSize(f_cn, 7.35f);
                    cb.ShowText("*Tratándose de Transportación Aérea Internacional y Franja Fronteriza, el IVA se calculará al 25% de la tasa general del 16%, de conformidad del Artículo 16 de la ley del IVA vigente.");
                    cb.SetFontAndSize(f_cn, 9);


                    cb.SetTextMatrix(385, 313);
                    cb.ShowText("tipoCambio");

                    cb.SetTextMatrix(459, 313);
                    cb.ShowText(tipoCambio.ToString());


                    if (dtGetTotalCargos != null)
                    {
                        writeText(cb, "Complemento Aerolineas", 40, 445, f_cn, 7);
                        writeText(cb, "TUA:", 26, 435, f_cn, 6);
                        writeText(cb, "Otros Cargos:", 26, 427, f_cn, 6);
                        writeText(cb, "$" + Convert.ToDecimal(RecuperarValorDR(drTua, "TUA")).ToString("#,##0.00"), 100, 435, f_cn, 6);
                        writeText(cb, "$" + totalCargos, 100, 427, f_cn, 6);
                    }

                    int valorX = 26;
                    int valorXImp = 100;

                    if (dtGetTotalCargos != null)
                    {
                        for (int i = 0; i < dtGetCargo.Rows.Count; i++)
                        {
                            int top_margin1 = 411;

                            for (int y = 0; y < 5 && i < dtGetCargo.Rows.Count; y++)
                            {
                                writeText(cb, RecuperarValorDR(dtGetCargo.Rows[i], "CodigoCargo"), valorX, top_margin1, f_cn, 7);
                                writeText(cb, "$" + Convert.ToDecimal(RecuperarValorDR(dtGetCargo.Rows[i], "Importe")).ToString("#,##0.00"), valorXImp, top_margin1, f_cn, 7);
                                top_margin1 -= 8;
                                i++;
                            }

                            i--;

                            valorX += 148;
                            valorXImp += 148;

                            cb.SetLineWidth(1);
                            cb.MoveTo(26, 419);
                            cb.LineTo(valorX, 419);
                        }
                    }

                    //Datos de XML
                    writeText(cb, "Folio Fiscal: " + RecuperarValorDR(drDatosTfd, "UUID"), 109, 163, f_cn, 6);
                    writeText(cb, "Fecha y Hora de Certificación: " + RecuperarValorDR(drDatosComprobante, "fecha"), 109, 155, f_cn, 6);
                    writeText(cb, "No. de Serie del Certificado del Emisor: " + RecuperarValorDR(drDatosComprobante, "noCertificado"), 109, 147, f_cn, 6);
                    writeText(cb, "No. de Serie del Certificado del SAT: " + RecuperarValorDR(drDatosTfd, "noCertificadoSAT"), 109, 139, f_cn, 6);
                    writeText(cb, "Condiciones de Pago: ", 109, 131, f_cn, 6);
                    writeText(cb, "Forma de Pago: " + RecuperarValorDR(drDatosComprobante, "formaDePago"), 109, 123, f_cn, 6);
                    writeText(cb, "Método de Pago: " + RecuperarValorDR(drDatosComprobante, "metodoDePago") + "-" + metodoPago, 109, 115, f_cn, 6);
                    writeText(cb, "Número de Cuenta: " + RecuperarValorDR(drDatosComprobante, "NumCtaPago"), 109, 107, f_cn, 6);
                    writeText(cb, "Este documento es una representación impresa de un CFDI", 261, 102, f_cn, 6);
                    writeText(cb, "Cadena Original del Complemento de Certificación Digital del SAT:", 109, 283, f_cb, 6);
                    writeText(cb, "Sello Digital del Emisor:", 109, 223, f_cb, 6);
                    writeText(cb, "Sello Digital del SAT:", 109, 193, f_cb, 6);

                    ColumnText cadenaSat = new ColumnText(cb);
                    cadenaSat.SetSimpleColumn(109, 280, 560, 1, 6, Element.ALIGN_LEFT);
                    cadenaSat.AddText(new Phrase(cadenaOriginal.ToString(), fuenteFactura));
                    cadenaSat.Go();

                    ColumnText selloEmisor = new ColumnText(cb);
                    selloEmisor.SetSimpleColumn(109, 220, 560, 1, 6, Element.ALIGN_LEFT);
                    selloEmisor.AddText(new Phrase(RecuperarValorDR(drDatosTfd, "selloCFD"), fuenteFactura));
                    selloEmisor.Go();

                    ColumnText selloSat = new ColumnText(cb);
                    selloSat.SetSimpleColumn(109, 190, 560, 1, 6, Element.ALIGN_LEFT);
                    selloSat.AddText(new Phrase(RecuperarValorDR(drDatosTfd, "selloSAT"), fuenteFactura));
                    selloSat.Go();

                    cb.SetLineWidth(1f);
                    cb.MoveTo(25, 289);
                    cb.LineTo(570, 289);
                    cb.Stroke();

                    cb.EndText();

                    ///Inserción de imágenes
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(rutaLogo);
                    logo.SetAbsolutePosition(38, 741); //Posicion en el eje cartesiano de X y Y
                    logo.ScaleAbsolute(200, 40);//Ancho y altura de la imagen
                    documentPDF.Add(logo);

                    iTextSharp.text.Image remate1 = iTextSharp.text.Image.GetInstance(rutaRemate1);
                    remate1.SetAbsolutePosition(238, 755);
                    remate1.ScaleAbsolute(313, 19);
                    documentPDF.Add(remate1);

                    iTextSharp.text.Image remate2 = iTextSharp.text.Image.GetInstance(rutaRemate2);
                    remate2.SetAbsolutePosition(22, 649);
                    remate2.ScaleAbsolute(451, 19);
                    documentPDF.Add(remate2);

                    remate2.SetAbsolutePosition(22, 571);
                    remate2.ScaleAbsolute(451, 19);
                    documentPDF.Add(remate2);

                    remate2.SetAbsolutePosition(22, 359);
                    remate2.ScaleAbsolute(451, 19);
                    documentPDF.Add(remate2);

                    BarcodeQRCode qrcodes = new BarcodeQRCode("?re=" + RecuperarValorDR(drDatosEmisor, "rfc") +
                                                              "&rr=" + RecuperarValorDR(drDatosReceptor, "rfc") +
                                                              "&tt=" + RecuperarValorDR(drDatosComprobante, "total").PadLeft(13, '0') + "0000" +
                                                              "&id=" + RecuperarValorDR(drDatosTfd, "UUID"), 1, 1, null);
                    iTextSharp.text.Image img = qrcodes.GetImage();
                    img.SetAbsolutePosition(440, 83);
                    img.ScaleAbsolute(92, 92);
                    documentPDF.Add(img);

                    //Cerramos el documento
                    documentPDF.Close();
                    writer.Close();
                    fileStrem.Dispose();
                    fileStrem.Close();

                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "GenerarPDF32");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result;
        }

        //Metodo que pinta en Negritas, y le indicas las coordenadas para pintar l Texto.
        private void writeText(PdfContentByte cb, string Text, int X, int Y, BaseFont font, int Size)
        {
            cb.SetFontAndSize(font, Size);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Text, X, Y, 0);
        }

        private string RecuperarValorDR(DataRow dr, string nombreColumna)
        {
            string result = "";

            try
            {
                if (dr != null && !dr.IsNull(nombreColumna))
                {
                    result = dr[nombreColumna].ToString().Trim();
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "RecuperarValorDR");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result;
        }


        #endregion
    }
}

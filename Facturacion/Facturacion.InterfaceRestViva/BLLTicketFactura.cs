using Comun.Utils;
using Facturacion.ENT;
using Facturacion.ENT.ProcesoFacturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Management;
using System.Text;
using System.Xml;

namespace Facturacion.InterfaceRestViva
{
    public class BLLTicketFactura
    {
        #region Propiedades privadas
        private BLLBitacoraErrores BllLogErrores { get; set; }
        private List<ENTParametrosCnf> ListaParametros { get; set; }
        private List<ENTGencatalogosCat> ListaGenCatalogos { get; set; }
        private List<ENTGendescripcionesCat> ListaGenDescripciones { get; set; }
        public string PNR { get; set; }
        public string MensajeErrorUsuario { get; set; }

        public List<string> ListaDriverImpresorasTermicas { get; set; }

        public string SaltoLinea { get; set; }

        public ImpresoraTermicaEpson ImpresoraTicket { get; set; }

        public string IPEstacion { get; set; }

        public List<ENTImpresoraTermica> ListaImpresorasTermicas = new List<ENTImpresoraTermica>();
        #endregion

        #region Constructor
        public BLLTicketFactura()
        {
            //BllLogErrores = new BLLBitacoraErrores();
            SaltoLinea = "\n\r";
            try
            {

                ListaParametros = new List<ENTParametrosCnf>();
                ListaGenCatalogos = new List<ENTGencatalogosCat>();
                ListaGenDescripciones = new List<ENTGendescripcionesCat>();

                BLLParametrosCnf bllParam = new BLLParametrosCnf();
                ListaParametros = bllParam.RecuperarTodo().Where(x => x.Activo == true).ToList();

                BLLGencatalogosCat bllGenCatalogos = new BLLGencatalogosCat();
                ListaGenCatalogos = bllGenCatalogos.RecuperarTodo();

                BLLGendescripcionesCat bllDescripciones = new BLLGendescripcionesCat();
                ListaGenDescripciones = bllDescripciones.RecuperarTodo();

                //RutaFuentePDF = ListaParametros.Where(x => x.Nombre == "RutaFontPDF").FirstOrDefault().Valor;

                if (ListaParametros.Where(x => x.Nombre == "MsjErrorPortal").Count() > 0)
                {
                    MensajeErrorUsuario = ListaParametros.Where(x => x.Nombre == "MsjErrorPortal").FirstOrDefault().Valor;
                }
                else
                {
                    MensajeErrorUsuario = "Por el momento no es posible procesar su solicitud por favor intente más tarde...";

                }


                //Asigna la impresora local por default
                string listaDriverImpTermicas = "";
                ListaDriverImpresorasTermicas = new List<string>();
                if (ListaParametros.Where(x => x.Nombre == "ListaDriverImpresorasTermicas").Count() > 0)
                {
                    listaDriverImpTermicas = ListaParametros.Where(x => x.Nombre == "ListaDriverImpresorasTermicas").FirstOrDefault().Valor;
                    foreach (string driver in listaDriverImpTermicas.Split('|'))
                    {
                        ListaDriverImpresorasTermicas.Add(driver);
                    }

                }
                else
                {
                    ListaDriverImpresorasTermicas.Add("EPSON TM-T88V");
                }


                ListaImpresorasTermicas = ListaImpresorasTermicasDisponibles();


                string nombreImpresoraTermica = "";
                if (ListaImpresorasTermicas.Count() > 0)
                {
                    ENTImpresoraTermica impTermica = new ENTImpresoraTermica();


                    if (ListaImpresorasTermicas.Where(x => x.EsDefault).Count() > 0)
                    {
                        impTermica = ListaImpresorasTermicas.Where(x => x.EsDefault).FirstOrDefault();
                    }
                    else
                    {
                        impTermica = ListaImpresorasTermicas.Where(x => x.EsLocal == true).FirstOrDefault();
                    }

                    if (impTermica.EsLocal == true)
                    {
                        nombreImpresoraTermica = impTermica.NombreImpresora;
                    }
                    else
                    {
                        nombreImpresoraTermica = impTermica.RutaImpresora;
                    }

                }

                if (nombreImpresoraTermica.Length > 0)
                {
                    ImpresoraTicket = new ImpresoraTermicaEpson(nombreImpresoraTermica);
                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "Constructor");
                throw new ExceptionViva(mensajeUsuario);
            }
        }
        #endregion


        #region Metodos Publicos Factura Version 3.3



        public string GeneraBodyTicketFactura(string xmlCFDI, string cadenaOriginal, ref string cadenaQR)
        {

            StringBuilder result = new StringBuilder();

            try
            {
                //Convertimos la respuesta de pegaso en un documento de XML
                XmlDocument xmlDocuResponse = new XmlDocument();
                xmlDocuResponse.LoadXml(xmlCFDI);
                XmlNodeReader xmlReader = new XmlNodeReader(xmlDocuResponse);

                DataSet dsRespuesta = new DataSet();
                dsRespuesta.ReadXml(xmlReader);

                //Recuperar informacion de cada tabla en el timbrado
                DataTable dtComprobante = dsRespuesta.Tables["Comprobante"];
                DataTable dtEmisor = dsRespuesta.Tables["Emisor"];
                DataTable dtReceptor = dsRespuesta.Tables["Receptor"];
                DataTable dtConceptos = dsRespuesta.Tables["Conceptos"];
                DataTable dtConcepto = dsRespuesta.Tables["Concepto"];
                DataTable dtImpuestos = dsRespuesta.Tables["Impuestos"];
                DataTable dtTraslados = dsRespuesta.Tables["Traslados"];
                DataTable dtTraslado = dsRespuesta.Tables["Traslado"];
                DataTable dtComplemento = dsRespuesta.Tables["Complemento"];
                DataTable dtAerolineas = dsRespuesta.Tables["Aerolineas"];
                DataTable dtOtrosCargos = (dsRespuesta.Tables.Contains("OtrosCargos") ? dsRespuesta.Tables["OtrosCargos"] : null);
                DataTable dtCargo = dsRespuesta.Tables["Cargo"];
                DataTable dtTimbreFiscalDigital = dsRespuesta.Tables["TimbreFiscalDigital"];

                //Referencias a las tablas del XML Del Formato CFDI
                DataRow drDatosTfd = dtTimbreFiscalDigital.Rows[0];
                DataRow drDatosEmisor = dtEmisor.Rows[0];
                DataRow drDatosReceptor = dtReceptor.Rows[0];
                DataRow drDatosComprobante = dtComprobante.Rows[0];
                DataRow drTua = dtAerolineas.Rows[0];


                //Mostrar serie y Folio de la factura
                result.Append(CrearSerieFolio(drDatosComprobante));
                result.Append(ImpresoraTicket.SeparadorLineaSencilla);

                //Mostrar Datos del Emisor
                result.Append(CrearDatosEmisor(dtEmisor));

                //MostrarDatos del cliente
                result.Append(MostrarTituloBloque("CLIENTE"));
                result.Append(CrearBloqueReceptor("RECEPTOR", dtReceptor));

                //MostrarDatos del Comprobante
                result.Append(MostrarTituloBloque("DATOS COMPROBANTE"));
                result.Append(CrearBloqueDatos("COMPROBANTE", dtComprobante));



                //MostrarDatos de PRODUCTOS / SERVICIOS
                result.Append(MostrarTituloBloque("PRODUCTOS / SERVICIOS"));
                result.Append(CrearBloqueConceptos(dtConceptos, dtConcepto, dtImpuestos, dtTraslados, dtTraslado));

                //Agregar bloque de subtotales
                result.Append(ImpresoraTicket.SeparadorLineaSencilla);
                result.Append(CrearBloqueTotales(drDatosComprobante, dtTraslado));
                result.Append(ImpresoraTicket.SeparadorLineaSencilla);

                //Bloque de importe con letra
                FormatoNumLetra ConvertirNumLetra = new FormatoNumLetra();
                string numero = drDatosComprobante["total"].ToString();
                string codMoneda = drDatosComprobante["Moneda"].ToString();

                result.Append("Importe con Letra:\n\r");
                result.Append(string.Format("{0}\n\r", ConvertirNumLetra.Convertir(numero, true, codMoneda)));
                result.Append(ImpresoraTicket.SeparadorLineaSencilla);

                //Bloque Leyenda IVA Frontera
                string leyendaIVA = ListaParametros.Where(x => x.Nombre == "LeyendaIVAPDF").FirstOrDefault().Valor;
                result.Append(string.Format("{0}\n\r", leyendaIVA));


                //MostrarDatos de COMPLEMENTO AEROLINEAS
                result.Append(MostrarTituloBloque("COMPLEMENTO AEROLINEAS"));

                result.Append(CrearBloqueComplementosTUA(dtOtrosCargos, drTua));

                if (dtCargo != null && dtCargo.Rows.Count > 0)
                {
                    result.Append(ImpresoraTicket.SeparadorLineaSencilla);
                    result.Append(CrearBloqueComplementosAero(dtCargo));
                }



                //MostrarDatos de DATOS TIMBRADO CFDI
                result.Append(MostrarTituloBloque("DATOS TIMBRADO CFDI"));

                result.Append(CrearBloqueTimbrado("TIMBRADO", dtTimbreFiscalDigital, dtComprobante, cadenaOriginal));

                //Agregar leyenda de que es una representacion grafica del CFDI
                result.Append(ImpresoraTicket.SeparadorLineaDoble);
                string textoRepresentacionGraficaCFDI = "Este documento es una representación impresa de un CFDI\n\r";
                result.Append(textoRepresentacionGraficaCFDI);
                result.Append(ImpresoraTicket.SeparadorLineaDoble);




                //Envia imagen a la impresora
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

                cadenaQR = codigoBi.ToString();


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

            return result.ToString();


        }

        private string MostrarTituloBloque(string titulo)
        {
            StringBuilder result = new StringBuilder();
            result.Append(ImpresoraTicket.SeparadorLineaDoble);
            result.Append(ImpresoraTicket.CmdActivarNegrita);
            result.Append(ImpresoraTicket.AsignaTipoFuenteGrande);
            result.Append(titulo + ImpresoraTicket.SaltoLineaRetorno);
            result.Append(ImpresoraTicket.CmdDesactivarNegrita);
            result.Append(ImpresoraTicket.AsignaTipoFuenteNormal);
            result.Append(ImpresoraTicket.SeparadorLineaSencilla);
            return result.ToString();
        }


        #endregion

        #region Metodos Factura Version 3.3
        #region Metodos Privados Factura Version 3.3


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
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
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
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "RecuperarDescSAT");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }

        private string CrearBloqueDatos(string bloque, DataTable dtDatos)
        {
            StringBuilder tblDatos = new StringBuilder();
            try
            {


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

                        tblDatos.Append(tituloColumna);
                        tblDatos.Append(" : ");
                        string valorMostrar = "";
                        valorMostrar = string.Format("{0} {1}", valorDato, descCatSAT);

                        tblDatos.Append(valorMostrar);
                        tblDatos.Append(ImpresoraTicket.SaltoLineaRetorno);

                    }
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueDatos");
                throw new ExceptionViva(mensajeUsuario);
            }

            return tblDatos.ToString();
        }

        private string CrearBloqueReceptor(string bloque, DataTable dtDatos)
        {
            StringBuilder tblDatos = new StringBuilder();
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

                DataRow drDatos = dtDatos.Rows[0];

                foreach (DataColumn drCol in dtDatos.Columns)
                {
                    string nombreCol = drCol.ColumnName;
                    if (!nombreCol.Contains("_") && !camposOmitidos.Contains(nombreCol))
                    {
                        string valorDato = drDatos.IsNull(nombreCol) ? "" : drDatos[nombreCol].ToString();
                        string tituloColumna = RecuperarTituloDato(nombreCol, bloque);
                        string descCatSAT = RecuperarDescSAT(nombreCol, valorDato);

                        tblDatos.Append(tituloColumna);

                        tblDatos.Append(":");


                        string valorMostrar = "";
                        valorMostrar = string.Format("{0} {1}", valorDato, descCatSAT);
                        tblDatos.Append(valorMostrar);
                        tblDatos.Append(ImpresoraTicket.SaltoLineaRetorno);
                    }
                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueReceptor");
                throw new ExceptionViva(mensajeUsuario);
            }

            return tblDatos.ToString();
        }


        private string CrearBloqueTimbrado(string bloque, DataTable dtTimbrado, DataTable dtComprobante, string cadenaOriginal)
        {
            StringBuilder tblDatos = new StringBuilder();
            try
            {
                //Agregar cadena Original
                tblDatos.Append(ImpresoraTicket.CmdActivarNegrita);
                tblDatos.Append("Cadena Original:\n\r");
                tblDatos.Append(ImpresoraTicket.CmdDesactivarNegrita);
                tblDatos.Append(string.Format("{0}\n\r", cadenaOriginal));


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

                        tblDatos.Append(ImpresoraTicket.CmdActivarNegrita);
                        tblDatos.Append(string.Format("{0}:\n\r", tituloColumna));
                        tblDatos.Append(ImpresoraTicket.CmdDesactivarNegrita);

                        string valorMostrar = "";
                        valorMostrar = string.Format("{0} {1}\n\r", valorDato, descCatSAT);

                        tblDatos.Append(valorMostrar);
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

                        tblDatos.Append(ImpresoraTicket.CmdActivarNegrita);
                        tblDatos.Append(string.Format("{0}:\n\r", tituloColumna));
                        tblDatos.Append(ImpresoraTicket.CmdDesactivarNegrita);


                        string valorMostrar = "";
                        valorMostrar = string.Format("{0} {1}\n\r", valorDato, descCatSAT);

                        tblDatos.Append(valorMostrar);
                    }
                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueTimbrado");
                throw new ExceptionViva(mensajeUsuario);
            }

            return tblDatos.ToString();
        }


        private string CrearBloqueTotales(DataRow drDatosComprobante, DataTable dtIvas)
        {
            StringBuilder tblDatos = new StringBuilder(2);

            try
            {
                int anchoEtiDatosSub = 25;
                int anchoMontos = 20;
                string margenIzq = "";
                margenIzq = margenIzq.PadRight(5, ' ');


                //DATOS DEL SUBTOTAL
                string subtotal = string.Format("{0}{1}{2}\n\r", margenIzq, ImpresoraTicket.Izquierda("SubTotal", anchoEtiDatosSub, true)
                    , ImpresoraTicket.Derecha(Convert.ToDecimal(drDatosComprobante["SubTotal"].ToString()).ToString("$###,###,##0.00"), anchoMontos, true));

                tblDatos.Append(subtotal);


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
                            importeIva += Convert.ToDecimal(drItem["importe"].ToString());
                        }
                    }

                    string textoIva = String.Format("IVA ({0} % *)", porcIva.ToString("0.00"));
                    string valorIva = importeIva.ToString("$###,###,##0.00");


                    string iva = string.Format("{0}{1}{2}\n\r", margenIzq, ImpresoraTicket.Izquierda(textoIva, anchoEtiDatosSub, true)
                    , ImpresoraTicket.Derecha(valorIva, anchoMontos, true));

                    tblDatos.Append(iva);

                }


                //DATOS DEL TOTAL
                string total = string.Format("{0}{1}{2}\n\r", margenIzq, ImpresoraTicket.Izquierda("Total", anchoEtiDatosSub, true)
                  , ImpresoraTicket.Derecha(Convert.ToDecimal(drDatosComprobante["total"].ToString()).ToString("$###,###,##0.00"), anchoMontos, true));

                tblDatos.Append(total);


                //DATOS DEL TIPO DE MONEDA
                string codMoneda = drDatosComprobante["Moneda"].ToString();
                string descMoneda = RecDescripcionSAT("c_Moneda", codMoneda);

                string textoMoneda = string.Format("{0}Moneda: {1} {2}\n\r", margenIzq, codMoneda, descMoneda);

                tblDatos.Append(textoMoneda);



                //DATOS DEL TIPO DE CAMBIO
                string textoTipoCambio = "";
                if (drDatosComprobante.Table.Columns.Contains("TipoCambio"))
                {
                    textoTipoCambio = string.Format("{0}Tipo Cambio: {1}\n\r", margenIzq, Convert.ToDecimal(drDatosComprobante["TipoCambio"].ToString()).ToString("$###,##0.00"));
                }

                else
                {
                    textoTipoCambio = string.Format("{0}Tipo Cambio : {1}\n\r", margenIzq, Convert.ToDecimal("1.00").ToString("$###,##0.00"));
                }
                tblDatos.Append(textoTipoCambio);

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueTotales");
                throw new ExceptionViva(mensajeUsuario);
            }
            return tblDatos.ToString();
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
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "RecCamposOmitidosPorBloque");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }

        private string CrearDatosEmisor(DataTable dtDatos)
        {
            StringBuilder result = new StringBuilder();
            try
            {

                DataRow drDatos = dtDatos.Rows[0];

                foreach (DataColumn drCol in dtDatos.Columns)
                {
                    string nombreCol = drCol.ColumnName;
                    if (!nombreCol.Contains("_"))
                    {
                        string valorDato = drDatos.IsNull(nombreCol) ? "" : drDatos[nombreCol].ToString();
                        string tituloColumna = RecuperarTituloDato(nombreCol, "EMISOR");
                        string descCatSAT = RecuperarDescSAT(nombreCol, valorDato);


                        result.Append(tituloColumna);
                        result.Append(":");
                        string valorMostrar = "";
                        valorMostrar = string.Format("{0} {1}", valorDato, descCatSAT).ToUpper();
                        result.Append(valorMostrar);
                        result.Append("\n\r");
                    }
                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearDatosEmisor");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result.ToString();
        }

        private string CrearSerieFolio(DataRow drDatos)
        {
            StringBuilder result = new StringBuilder();
            try
            {

                string serie = "";
                string folio = "";

                serie = drDatos["Serie"].ToString().ToUpper();
                folio = drDatos["Folio"].ToString().ToUpper();

                string serieFolio = string.Format("Serie: {0}    Factura No: {1}", serie, folio);

                result.Append(ImpresoraTicket.CmdActivarNegrita);
                result.Append(ImpresoraTicket.AsignaTipoFuenteGrande);
                result.Append(ImpresoraTicket.Derecha(serieFolio + ImpresoraTicket.SaltoLineaRetorno, 0, false));
                result.Append(ImpresoraTicket.AsignaTipoFuenteNormal);
                result.Append(ImpresoraTicket.CmdDesactivarNegrita);

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearSerieFolio");
                throw new ExceptionViva(mensajeUsuario);
            }

            return result.ToString();
        }


        private string CrearBloqueConceptos(DataTable dtConceptos, DataTable dtConcepto, DataTable dtImpuestos, DataTable dtTraslados, DataTable dtTraslado)
        {
            StringBuilder tblDatos = new StringBuilder();
            try
            {

                StringBuilder encabezadoProductos = new StringBuilder();
                StringBuilder encabezadoIVAS = new StringBuilder();
                string colCveServ = "CVE SERV";
                string colNoId = "NO.ID";
                string colCant = "CANT";
                string colValorUni = "VALOR UNI";
                string colImporte = "IMPORTE";

                int anchoColCveServ = 11;
                int anchoColNoId = 7;
                int anchoColCant = 6;
                int anchoColValorUni = 15;
                int anchoColImporte = 16;

                encabezadoProductos.Append(ImpresoraTicket.Izquierda(colCveServ, anchoColCveServ, true));
                encabezadoProductos.Append(ImpresoraTicket.Centrar(colNoId, anchoColNoId, true));
                encabezadoProductos.Append(ImpresoraTicket.Centrar(colCant, anchoColCant, true));
                encabezadoProductos.Append(ImpresoraTicket.Derecha(colValorUni, anchoColValorUni, true));
                encabezadoProductos.Append(ImpresoraTicket.Derecha(colImporte, anchoColImporte, true));

                //Se agregan los encabezados de los productos
                tblDatos.Append(encabezadoProductos.ToString() + ImpresoraTicket.SaltoLineaRetorno);
                tblDatos.Append(ImpresoraTicket.SeparadorLineaDoble);


                string colUnidad = "Unidad";
                string colDescripcion = "Descripcion";
                string colCveUni = "Clave Unidad";

                //***********************************************************************************
                //Se generan los encabezados del bloque para los IVAS
                //***********************************************************************************
                string colBaseIVA = "Base IVA";
                string colImpto = "Impto";
                string colFactor = "Factor";
                string colTasa = "Tasa";
                string colIVA = "Impt.IVA";
                string sepCol = " ";

                int anchoColBaseIVA = 13;
                int anchoColImpto = 8;
                int anchoColFactor = 8;
                int anchoColTasa = 8;
                int anchoColIVA = 13;
                int anchoSepCol = 1;

                encabezadoIVAS.Append(ImpresoraTicket.Derecha(colBaseIVA, anchoColBaseIVA, true));
                encabezadoIVAS.Append(ImpresoraTicket.Centrar(sepCol, anchoSepCol, true));
                encabezadoIVAS.Append(ImpresoraTicket.Centrar(colImpto, anchoColImpto, true));
                encabezadoIVAS.Append(ImpresoraTicket.Centrar(sepCol, anchoSepCol, true));
                encabezadoIVAS.Append(ImpresoraTicket.Centrar(colFactor, anchoColFactor, true));
                encabezadoIVAS.Append(ImpresoraTicket.Centrar(sepCol, anchoSepCol, true));
                encabezadoIVAS.Append(ImpresoraTicket.Centrar(colTasa, anchoColTasa, true));
                encabezadoIVAS.Append(ImpresoraTicket.Centrar(sepCol, anchoSepCol, true));
                encabezadoIVAS.Append(ImpresoraTicket.Derecha(colIVA, anchoColIVA, true));
                encabezadoIVAS.Append(ImpresoraTicket.SaltoLineaRetorno);


                //Se recorren los conceptos por registrar
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

                        string valorClaveProdServ = drConcepto["ClaveProdServ"].ToString();
                        string valorNoIdentificacion = drConcepto["NoIdentificacion"].ToString();
                        string valorCantidad = drConcepto["Cantidad"].ToString();
                        string valorClaveUnidad = drConcepto["ClaveUnidad"].ToString();
                        string valorUnidad = drConcepto["Unidad"].ToString();
                        string valorDescripcion = drConcepto["Descripcion"].ToString();
                        string valorValorUnitario = Convert.ToDecimal(drConcepto["ValorUnitario"].ToString()).ToString("$###,###,##0.00");
                        string valorImporte = Convert.ToDecimal(drConcepto["Importe"].ToString()).ToString("$###,###,##0.00");


                        //Se agrega bloque con los datos del concepto

                        StringBuilder filaImportes = new StringBuilder();
                        StringBuilder filaDescripcion = new StringBuilder();
                        StringBuilder filaCveUnidad = new StringBuilder();

                        //Genera fila con los importes del concepto
                        filaImportes.Append(ImpresoraTicket.Izquierda(valorClaveProdServ, anchoColCveServ, true));
                        filaImportes.Append(ImpresoraTicket.Centrar(valorNoIdentificacion, anchoColNoId, true));
                        filaImportes.Append(ImpresoraTicket.Centrar(valorCantidad, anchoColCant, true));
                        filaImportes.Append(ImpresoraTicket.Derecha(valorValorUnitario, anchoColValorUni, true));
                        filaImportes.Append(ImpresoraTicket.Derecha(valorImporte, anchoColImporte, true));
                        filaImportes.Append(ImpresoraTicket.SaltoLineaRetorno);

                        tblDatos.Append(filaImportes.ToString());

                        //Se agrega la fila de la descripcion
                        filaDescripcion.Append(string.Format("{0} : {1}{2}", colDescripcion, valorDescripcion, ImpresoraTicket.SaltoLineaRetorno));
                        tblDatos.Append(filaDescripcion.ToString());

                        //Se agrega la fila con la clave y descripcion de la unidad
                        filaCveUnidad.Append(string.Format("{0}: {1}  {2}: {3}{4}", colCveUni, valorClaveUnidad, colUnidad, valorUnidad, ImpresoraTicket.SaltoLineaRetorno));
                        tblDatos.Append(filaCveUnidad.ToString());


                        // Espacio que le vamos a restar a cada salto de línea
                        foreach (DataRow drImpuestos in dtImpuestos.Select("Concepto_Id = " + conceptoId))
                        {

                            string impuestosId = "";
                            impuestosId = drImpuestos["Impuestos_Id"].ToString();
                            foreach (DataRow drTraslados in dtTraslados.Select("Impuestos_Id = " + impuestosId))
                            {

                                //Se agregan las columnas de los bloques para el IVA
                                tblDatos.Append(encabezadoIVAS.ToString());

                                string trasladosId = "";
                                trasladosId = drTraslados["Traslados_Id"].ToString();

                                foreach (DataRow drIVA in dtTraslado.Select("Traslados_Id = " + trasladosId))
                                {
                                    StringBuilder filaDetalleIVA = new StringBuilder();
                                    string valorBase = "";
                                    string valorImpuesto = "";
                                    string valorFactor = "";
                                    string valorTasaOCuota = "";
                                    string valorImporteIVA = "";


                                    valorBase = Convert.ToDecimal(drIVA["Base"].ToString()).ToString("$###,###,##0.00");
                                    valorImpuesto = drIVA["Impuesto"].ToString();
                                    valorFactor = drIVA["TipoFactor"].ToString();
                                    valorTasaOCuota = drIVA.IsNull("TasaOCuota") ? "" : Convert.ToDecimal(drIVA["TasaOCuota"].ToString()).ToString("0.00");
                                    valorImporteIVA = drIVA.IsNull("Importe") ? "" : Convert.ToDecimal(drIVA["Importe"].ToString()).ToString("$###,###,##0.00");


                                    filaDetalleIVA.Append(ImpresoraTicket.Derecha(valorBase, anchoColBaseIVA, true));
                                    filaDetalleIVA.Append(ImpresoraTicket.Centrar(sepCol, anchoSepCol, true));
                                    filaDetalleIVA.Append(ImpresoraTicket.Centrar(valorImpuesto, anchoColImpto, true));
                                    filaDetalleIVA.Append(ImpresoraTicket.Centrar(sepCol, anchoSepCol, true));
                                    filaDetalleIVA.Append(ImpresoraTicket.Centrar(valorFactor, anchoColFactor, true));
                                    filaDetalleIVA.Append(ImpresoraTicket.Centrar(sepCol, anchoSepCol, true));
                                    filaDetalleIVA.Append(ImpresoraTicket.Centrar(valorTasaOCuota, anchoColTasa, true));
                                    filaDetalleIVA.Append(ImpresoraTicket.Centrar(sepCol, anchoSepCol, true));
                                    filaDetalleIVA.Append(ImpresoraTicket.Derecha(valorImporteIVA, anchoColIVA, true));
                                    filaDetalleIVA.Append(ImpresoraTicket.SaltoLineaRetorno);
                                    tblDatos.Append(filaDetalleIVA.ToString());
                                }
                            }
                        }
                        //Agrega un salto de linea entre cada concepto
                        tblDatos.Append(ImpresoraTicket.SaltoLineaRetorno);
                    }
                }



            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueConceptos");
                throw new ExceptionViva(mensajeUsuario);
            }

            return tblDatos.ToString();
        }


        private string CrearBloqueComplementosTUA(DataTable dtOtrosCargos, DataRow drTUA)
        {
            StringBuilder tblDatos = new StringBuilder();
            try
            {
                //DATOS DEL COMPLEMENTO DE AEROLINEAS
                if (dtOtrosCargos != null && dtOtrosCargos.Rows.Count > 0)
                {
                    DataRow drTotalCargos = dtOtrosCargos.Rows[0];

                    tblDatos.Append(string.Format("TUA: {0}      ", Convert.ToDecimal(drTUA["TUA"].ToString()).ToString("$###,###,##0.00")));
                    tblDatos.Append(string.Format("Otros Cargos : {0}\n\r", Convert.ToDecimal(drTotalCargos["TotalCargos"].ToString()).ToString("$###,###,##0.00")));
                }
            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueComplementosTUA");
                throw new ExceptionViva(mensajeUsuario);
            }
            return tblDatos.ToString();
        }

        private string CrearBloqueComplementosAero(DataTable dtCargo)
        {
            StringBuilder tblDatos = new StringBuilder(); ;
            try
            {
                int anchoCodigoCargo = 2;
                int anchoMontoCargo = 11;

                string saltoLinea = "";
                //DESGLOCE DE LOS IMPUESTOS COMO SON BS, DS, ETC.
                int contCargo = 0;
                if (dtCargo != null)
                {
                    foreach (DataRow dtCargosAero in dtCargo.Rows)
                    {
                        contCargo++;
                        if (contCargo == 4)
                        {
                            contCargo = 0;
                            saltoLinea = "\n\r";
                        }
                        else
                        {
                            saltoLinea = "";
                        }


                        string clLey = ImpresoraTicket.Derecha(dtCargosAero["CodigoCargo"].ToString(), anchoCodigoCargo, true);
                        string clVal = ImpresoraTicket.Izquierda(Convert.ToDecimal(dtCargosAero["Importe"].ToString()).ToString("$###,###,##0.00"), anchoMontoCargo, true);

                        string lineaCargo = string.Format("{0} {1}{2}", clLey, clVal, saltoLinea);
                        tblDatos.Append(lineaCargo);
                    }
                    if (saltoLinea.Length == 0)
                    {
                        tblDatos.Append(ImpresoraTicket.SaltoLineaRetorno);
                    }


                }

            }
            catch (ExceptionViva ex)
            {
                throw ex;
            }
            catch (SqlException ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "CrearBloqueComplementosAero");
                throw new ExceptionViva(mensajeUsuario);
            }
            return tblDatos.ToString();
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
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "BD");
                throw new ExceptionViva(mensajeUsuario);
            }
            catch (Exception ex)
            {
                string mensajeUsuario = MensajeErrorUsuario;
                BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "PDFCFDI", "RecDescripcionSAT");
                throw new ExceptionViva(mensajeUsuario);
            }
            return result;
        }

        #endregion


        #endregion

        public void GenerarTicketFactura(string infoTicketFactura, string codigoQR)
        {
            StringBuilder datosImpresion = new StringBuilder();
            if (ListaImpresorasTermicas.Count == 0)
            {
                ListaImpresorasTermicas = ListaImpresorasTermicasDisponibles();
            }

            //Margen superior
            datosImpresion.Append(ImpresoraTicket.CmdInterlineado);
            datosImpresion.Append(ImpresoraTicket.ComandoSaltoLinea(2));

            //Se eliminan acentos para que pueda imprimir sin caracteres especiales
            infoTicketFactura = ImpresoraTicket.QuitarAcentos(infoTicketFactura);

            //Envia a imprimir informacion

            datosImpresion.Append(infoTicketFactura);

            //Margen Inferior
            datosImpresion.Append(ImpresoraTicket.ComandoSaltoLinea(1));

            ImpresoraTicket.EnviarImpresion(datosImpresion.ToString());

            ImpresoraTicket.ImprimeQR(codigoQR);



        }

        public List<ENTImpresoraTermica> ListaImpresorasTermicasDisponibles()
        {

            List<ENTImpresoraTermica> listaResult = new List<ENTImpresoraTermica>();


            //Revisar las impresoras que se tienen de forma local, para actualizar
            string nombreImpresora = "";
            ManagementScope scope = new ManagementScope(ManagementPath.DefaultPath);
            scope.Connect();

            //Consulta para obtener las impresoras, en la API Win32
            SelectQuery query = new SelectQuery("select * from Win32_Printer");

            ManagementClass m = new ManagementClass("Win32_Printer");

            ManagementObjectSearcher obj = new ManagementObjectSearcher(scope, query);

            //Obtenemos cada instancia del objeto ManagementObjectSearcher
            using (ManagementObjectCollection printers = m.GetInstances())
                foreach (ManagementObject printer in printers)
                {
                    if (printer != null)
                    {
                        //Obtenemos la primera impresora en el bucle
                        nombreImpresora = printer["Name"].ToString();
                        bool local = (bool)printer["Local"];
                        var driverName = printer.GetPropertyValue("DriverName");

                        if (ListaDriverImpresorasTermicas.Where(x => driverName.ToString().ToUpper().Contains(x.ToString().ToUpper())).Count() > 0)
                        {

                            //Una vez encontrada verificamos el estado de ésta
                            if (!printer["WorkOffline"].ToString().ToLower().Equals("true"))
                            {

                                string status = printer["Status"].ToString();
                                bool esCompartida = (bool)printer["Shared"];
                                string nombreImpCompartida = "";
                                if (esCompartida)
                                {
                                    nombreImpCompartida = printer["ShareName"] != null ? printer["ShareName"].ToString() : "";
                                }

                                ENTImpresoraTermica impTermica = new ENTImpresoraTermica();
                                impTermica = listaResult.Where(x => x.NombreImpresora == nombreImpresora && x.EsLocal == true).FirstOrDefault();

                                int numImpLocal = listaResult.Count(x => x.EsLocal == true && x.EsDefault == true);

                                //Valida si la impresora no se encuentra registrada
                                if (impTermica == null)
                                {
                                    //Esta en linea
                                    ENTImpresoraTermica impTermicaNueva = new ENTImpresoraTermica();
                                    impTermicaNueva.NombreImpresora = nombreImpresora;
                                    impTermicaNueva.DisponibleEnRed = esCompartida;
                                    impTermicaNueva.EsLocal = true;
                                    impTermicaNueva.EsDefault = (numImpLocal == 0 ? true : false);
                                    impTermicaNueva.RutaImpresora = esCompartida ? string.Format(@"\\{0}\{1}", IPEstacion, nombreImpCompartida) : "";

                                    listaResult.Add(impTermicaNueva);

                                }
                            }

                        }


                    }
                    else
                        throw new Exception("No fueron encontradas impresoras instaladas en el equipo");
                }



            return listaResult;
        }






    }
}
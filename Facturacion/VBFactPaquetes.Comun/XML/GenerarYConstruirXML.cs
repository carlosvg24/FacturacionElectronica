using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VBFactPaquetes.Comun.Log;
//using VBFactPaquetes.Comun.PDF;
using VBFactPaquetes.Model.Facturacion;

namespace VBFactPaquetes.Comun
{
    public class GenerarYConstruirXML
    {

        public String GeneraXMLRequest(Pago pagosDTO, String tipoProceso)
        {
            String rutaXML = String.Empty;

            try
            {
                //Se instancia a la nueva clase que se encuentra en "GeneraFacturaEgresoPDF".
                //GeneraFactura Fe = new GeneraFactura();
                //FileStream fs = System.IO.File.Create(GuardaArchivosFactura(pagosDTO, "XML", tipoProceso));
                //FileStream fs = System.IO.File.Create(GeneraFactura.GuardaArchivosFactura(pagosDTO, "XML"));
                //fs.Close();

                if (!pagosDTO.PagoFacturado)
                {
                    /*SE CONSTRUYE EL ARCHIVO XML REQUEST*/
                    ConstruirXML(pagosDTO, pagosDTO.RutaArchivoXML);
                }

                /*SE PASA EL XML REQUEST A UN STRING PARA SER RETORNADO AL PROCESO ANTERIOR */
                return System.IO.File.ReadAllText(pagosDTO.RutaArchivoXML);
            }
            catch (Exception ex)
            {
                //throw ex;
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("tipoProceso", tipoProceso);
                parametros.Add("pagosDTO", pagosDTO);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }
        }

        public string GeneraXMLRequestEgresos(Pago pago)
        {
            String rutaXML = String.Empty;

            try
            {
                FileStream fs = System.IO.File.Create(GuardaArchivosEgresos(pago, "XML"));
                fs.Close();

                /*SE CONSTRUYE EL ARCHIVO XML REQUEST*/
                ConstruirXMLEgreso(pago, pago.RutaArchivoXML);

                /*SE PASA EL XML REQUEST A UN STRING PARA SER RETORNADO AL PROCESO ANTERIOR */
                return System.IO.File.ReadAllText(pago.RutaArchivoXML);
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
        }



        private void ConstruirXML(Pago pagosDTO, String rutaXML)
        {
            try
            {
                List<Impuestos> lstImpuestos = new List<Impuestos>();
                Impuestos impuestoDTO;

                /* SE LLENAN LOS DATOS PARA FORMAR EL XML */
                StringBuilder xmlRequest = new StringBuilder();
                pagosDTO.FechaEmision = DateTime.Now.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss");

                xmlRequest.Append("<RequestCFD version=\"3.3\">");
                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("<Comprobante ");
                xmlRequest.Append("Version=\"" + pagosDTO.LstDatosGralDTO[0].VersionCFDI + "\" ");
                xmlRequest.Append("Serie=\"" + pagosDTO.Serie + "\" ");
                xmlRequest.Append("Folio=\"" + pagosDTO.NoFolio.ToString() + "\" ");
                xmlRequest.Append("Fecha=\"" + pagosDTO.FechaEmision + "\" ");
                xmlRequest.Append("FormaPago=\"" + pagosDTO.CodigoFP + "\" ");
                xmlRequest.Append("Moneda=\"" + pagosDTO.CodigoM + "\" ");
                xmlRequest.Append("MetodoPago=\"" + pagosDTO.CodigoMP + "\" ");
                xmlRequest.Append("SubTotal=\"" + pagosDTO.SubTotalPago.ToString(new CultureInfo("es-MX")) + "\" ");
                xmlRequest.Append("Total=\"" + pagosDTO.TotalPago.ToString(new CultureInfo("es-MX")) + "\" ");

                if (pagosDTO.DescuentoPago > 0)
                {
                    xmlRequest.Append("Descuento=\"" + pagosDTO.DescuentoPago.ToString(new CultureInfo("es-MX")) + "\" ");
                }

                if (pagosDTO.TipoCambio == 1)
                {
                    xmlRequest.Append("TipoCambio=\"" + 1 + "\" ");
                }
                else
                {
                    xmlRequest.Append("TipoCambio=\"" + pagosDTO.TipoCambio.ToString(new CultureInfo("es-MX")) + "\" ");
                }

                xmlRequest.Append("TipoDeComprobante=\"" + pagosDTO.CodigoTC + "\" ");
                xmlRequest.Append("LugarExpedicion=\"" + pagosDTO.LugarExpedicion + "\" ");
                xmlRequest.Append("NoCertificado=\"" + pagosDTO.NoCertificado + "\" >");
                //xmlRequest.Append("Certificado=\"VOLARIS\" ");
                //xmlRequest.Append("Sello=\"VOLARIS\" ");
                //xmlRequest.Append("xmlns:aerolineas=\"" + "http://www.sat.gob.mx/aerolineas" + "\" ");
                //xmlRequest.Append("xmlns:xsi=\"" + "http://www.w3.org/2001/XMLSchema-instance" + "\" ");
                //xmlRequest.Append("xmlns:tfd=\"" + "http://www.sat.gob.mx/TimbreFiscalDigital" + "\" ");
                //xmlRequest.Append("xmlns:xs=\"" + "http://www.w3.org/2001/XMLSchema" + "\" ");
                //xmlRequest.Append("xmlns:cfdi=\"" + "http://www.sat.gob.mx/cfd/3" + "\" ");
                //xmlRequest.Append("xmlns:terceros = \"" + "http://www.sat.gob.mx/terceros" + "\" ");
                //xmlRequest.Append("xsi:schemaLocation=\"" + "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd http://www.sat.gob.mx/terceros http://www.sat.gob.mx/sitio_internet/cfd/terceros/terceros11.xsd http://www.sat.gob.mx/aerolineas http://www.sat.gob.mx/sitio_internet/cfd/aerolineas/aerolineas.xsd" + "\">");

                //if (pagosDTO.LstPagosEgresos.Count > 0)
                //{
                //    if (!String.IsNullOrEmpty(pagosDTO.LstPagosEgresos[0].UUID))
                //    {
                //        /*CFDIRELACIONADO*/
                //        xmlRequest.Append(Environment.NewLine);
                //        xmlRequest.Append(" <CfdiRelacionados ");
                //        xmlRequest.Append("TipoRelacion=\"04\" >");
                //        xmlRequest.Append(Environment.NewLine);
                //        xmlRequest.Append("   <CfdiRelacionado ");
                //        xmlRequest.Append("UUID=\"" + pagosDTO.LstPagosEgresos[0].UUID + "\" /> ");
                //        xmlRequest.Append(Environment.NewLine);
                //        xmlRequest.Append(" </CfdiRelacionados >");
                //    }
                //}

                /*EMISOR*/
                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append(" <Emisor ");
                xmlRequest.Append("Rfc=\"" + pagosDTO.RFCEmisor + "\" ");
                xmlRequest.Append("RegimenFiscal=\"" + pagosDTO.CodigoRFEmisor + "\" ");
                xmlRequest.Append("Nombre=\"" + pagosDTO.RazonSocialEmisor + "\"/> ");
                xmlRequest.Append(Environment.NewLine);

                /*RECEPTOR*/
                xmlRequest.Append(" <Receptor ");
                string rfcTReceptor = pagosDTO.LstDatosFiscales[0].RFC.Replace("&", "&amp;");
                xmlRequest.Append("Rfc=\"" + rfcTReceptor + "\" ");

                if (!String.IsNullOrEmpty(pagosDTO.LstDatosFiscales[0].RazonSocial))
                {
                    xmlRequest.Append("Nombre=\"" + pagosDTO.LstDatosFiscales[0].RazonSocial + "\" ");
                }

                xmlRequest.Append("UsoCFDI=\"" + pagosDTO.LstDatosFiscales[0].CodigoUsoCFDI + "\"/> ");
                xmlRequest.Append(Environment.NewLine);

                /*CONCEPTO*/
                xmlRequest.Append(" <Conceptos>");
                xmlRequest.Append(Environment.NewLine);

                /*CICLO PARA RECORRER LA LISTA DE LOS DATOS PARA CONCEPTO.*/
                for (int i = 0; i < pagosDTO.LstConcepto.Count; i++)
                {
                    if (pagosDTO.LstConcepto[i].PrecioUnitario > 0 && pagosDTO.CodigoTC == pagosDTO.LstConcepto[i].CodigoTC)
                    {
                        xmlRequest.Append("     <Concepto ");
                        xmlRequest.Append("ClaveProdServ=\"" + pagosDTO.LstConcepto[i].CodigoProdSer + "\" ");
                        xmlRequest.Append("NoIdentificacion=\"" + pagosDTO.LstConcepto[i].CodigoProdSer + "\" ");
                        xmlRequest.Append("Cantidad=\"" + pagosDTO.LstConcepto[i].Cantidad + "\" ");
                        xmlRequest.Append("ClaveUnidad=\"" + pagosDTO.LstConcepto[i].ClaveUnidad + "\" ");
                        xmlRequest.Append("Unidad=\"" + pagosDTO.LstConcepto[i].Unidad + "\" ");
                        xmlRequest.Append("Descripcion=\"" + pagosDTO.LstConcepto[i].DescProdSer + "\" ");

                        if (pagosDTO.DescuentoPago > 0)
                        {
                            xmlRequest.Append("Descuento=\"" + pagosDTO.DescuentoPago.ToString(new CultureInfo("es-MX")) + "\" ");
                        }

                        xmlRequest.Append("ValorUnitario=\"" + pagosDTO.LstConcepto[i].PrecioUnitario.ToString(new CultureInfo("es-MX")) + "\" ");
                        xmlRequest.Append("Importe=\"" + pagosDTO.LstConcepto[i].Importe.ToString(new CultureInfo("es-MX")) + "\"> ");

                        if (pagosDTO.LstConcepto[i].ImporteBase > 0)
                        {
                            /*IMPUESTOS TRASLADO*/
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("         <Impuestos>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("             <Traslados>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("             <Traslado ");
                            xmlRequest.Append("Base=\"" + pagosDTO.LstConcepto[i].ImporteBase.ToString(new CultureInfo("es-MX")) + "\" ");
                            xmlRequest.Append("Importe=\"" + pagosDTO.LstConcepto[i].ImporteTotal.ToString(new CultureInfo("es-MX")) + "\" ");
                            xmlRequest.Append("Impuesto=\"" + pagosDTO.LstConcepto[i].Impuesto + "\" ");

                            //xmlRequest.Append("TasaOCuota=\"" + pagosDTO.LstConcepto[i].TasaOCuota.ToString(new CultureInfo("es-MX")) + "\" " );
                            xmlRequest.Append("TasaOCuota=\"" + "0.160000" + "\" ");
                            xmlRequest.Append("TipoFactor=\"" + pagosDTO.LstConcepto[i].Factor + "\"/>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("             </Traslados>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("         </Impuestos>");

                            impuestoDTO = new Impuestos();

                            impuestoDTO.Importe = pagosDTO.LstConcepto[i].ImporteTotal;
                            impuestoDTO.CodigoImpuesto = pagosDTO.LstConcepto[i].Impuesto.ToString();
                            impuestoDTO.TasaOCuota = pagosDTO.LstConcepto[i].TasaOCuota;
                            impuestoDTO.DescripcionTipoFactor = pagosDTO.LstConcepto[i].Factor;

                            if (!lstImpuestos.Exists(x => x.TasaOCuota == impuestoDTO.TasaOCuota))
                            {
                                lstImpuestos.Add(impuestoDTO);
                            }
                            else
                            {
                                int index = lstImpuestos.FindIndex(element => element.TasaOCuota == impuestoDTO.TasaOCuota);

                                lstImpuestos[index].Importe += impuestoDTO.Importe;
                            }
                        }

                        if (!String.IsNullOrEmpty(pagosDTO.LstConcepto[i].RFCTercero))
                        {
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("         <ComplementoConcepto>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("             <terceros:PorCuentadeTerceros ");
                            xmlRequest.Append("version=\"" + "1.1\" ");
                            xmlRequest.Append("rfc=\"" + pagosDTO.LstConcepto[i].RFCTercero + "\">");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("                 <terceros:Parte ");
                            xmlRequest.Append("cantidad =\"" + pagosDTO.LstConcepto[i].Cantidad + "\" ");
                            xmlRequest.Append("unidad=\"" + pagosDTO.LstConcepto[i].Unidad + "\" ");
                            xmlRequest.Append("noIdentificacion=\"" + pagosDTO.LstConcepto[i].CodigoProdSer + "\" ");
                            xmlRequest.Append("descripcion=\"" + pagosDTO.LstConcepto[i].DescProdSer + "\" ");
                            xmlRequest.Append("valorUnitario=\"" + pagosDTO.LstConcepto[i].PrecioUnitario.ToString(new CultureInfo("es-MX")) + "\" ");
                            xmlRequest.Append("importe=\"" + pagosDTO.LstConcepto[i].Importe.ToString(new CultureInfo("es-MX")) + "\"/> ");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("                 <terceros:Impuestos/>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("             </terceros:PorCuentadeTerceros>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("         </ComplementoConcepto>");
                        }

                        xmlRequest.Append(Environment.NewLine);
                        xmlRequest.Append("     </Concepto>");
                        xmlRequest.Append(Environment.NewLine);
                    }
                }

                xmlRequest.Append(" </Conceptos>");

                if (lstImpuestos.Count > 0)
                {
                    /*IMPORTE*/
                    xmlRequest.Append(Environment.NewLine);
                    xmlRequest.Append(" <Impuestos ");

                    xmlRequest.Append("TotalImpuestosTrasladados=\"" + lstImpuestos[0].Importe.ToString(new CultureInfo("es-MX")) + "\">");

                    xmlRequest.Append(Environment.NewLine);
                    xmlRequest.Append("     <Traslados>");
                    xmlRequest.Append(Environment.NewLine);
                    xmlRequest.Append("         <Traslado ");
                    xmlRequest.Append("Impuesto=\"" + lstImpuestos[0].CodigoImpuesto + "\" ");
                    xmlRequest.Append("TipoFactor=\"" + lstImpuestos[0].DescripcionTipoFactor + "\" ");

                    //xmlRequest.Append("TasaOCuota=\"" + lstImpuestos[0].TasaOCuota.ToString(new CultureInfo("es-MX")) + "\" ");
                    xmlRequest.Append("TasaOCuota=\"" + "0.160000" + "\" ");
                    xmlRequest.Append("Importe=\"" + lstImpuestos[0].Importe.ToString(new CultureInfo("es-MX")) + "\"/>");

                    xmlRequest.Append(Environment.NewLine);
                    xmlRequest.Append("     </Traslados>");
                    xmlRequest.Append(Environment.NewLine);
                    xmlRequest.Append(" </Impuestos>");
                }

                //if (pagosDTO.LstCargos.Count > 0)
                //{
                //    /*RECORREMOS LA LISTA PARA OBTENER LA INFORMACIÓN DEL TUA*/
                //    for (int a = 0; a < pagosDTO.LstCargos.Count; a++)
                //    {
                //        if (pagosDTO.LstCargos[a].CodigoCargo == "TUA")
                //        {
                //            TUA = pagosDTO.LstCargos[a].CodigoCargo + "=\"" + pagosDTO.LstCargos[a].TotalCargo.ToString(new CultureInfo("es-MX")) + "\"";
                //        }
                //        else
                //        {
                //            TotalCargos += pagosDTO.LstCargos[a].TotalCargo;
                //            OtroCargos = "  <aerolineas:OtrosCargos TotalCargos=\"" + TotalCargos.ToString(new CultureInfo("es-MX")) + "\">";
                //            Cargos += "     <aerolineas:Cargo CodigoCargo" + "=\"" + pagosDTO.LstCargos[a].CodigoCargo + "\" Importe" + "=\"" + pagosDTO.LstCargos[a].TotalCargo.ToString(new CultureInfo("es-MX")) + "\"/>" + Environment.NewLine;
                //        }

                //        if (String.IsNullOrEmpty(TUA))
                //        {
                //            TUA = "TUA=\"" + "0".ToString(new CultureInfo("es-MX")) + "\"";
                //        }
                //    }

                //    /*COMPLEMENTO DE PAGO*/
                //    xmlRequest.Append(Environment.NewLine);
                //    xmlRequest.Append(" <Complemento>");
                //    xmlRequest.Append(Environment.NewLine);

                //    if (TotalCargos == 0)
                //    {
                //        xmlRequest.Append("   <aerolineas:Aerolineas Version =\"1.0\" " + TUA + " />");
                //    }
                //    else
                //    {
                //        xmlRequest.Append("   <aerolineas:Aerolineas Version =\"1.0\" " + TUA + ">");
                //        xmlRequest.Append(Environment.NewLine);
                //        xmlRequest.Append(OtroCargos);
                //        xmlRequest.Append(Environment.NewLine);
                //        xmlRequest.Append(Cargos);
                //        xmlRequest.Append("     </aerolineas:OtrosCargos>");
                //        xmlRequest.Append(Environment.NewLine);
                //        xmlRequest.Append(" </aerolineas:Aerolineas>");
                //    }

                //    xmlRequest.Append(Environment.NewLine);
                //    xmlRequest.Append(" </Complemento>");
                //}

                /*ADENDA*/
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append(" <Addenda>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append("   <customized>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append("      <VOLARIS>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append("         <PNR>" + pagosDTO.PNR + "</PNR>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append("         <FECHA>" + pagosDTO.FechaPago + "</FECHA>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append("      </VOLARIS>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append("   </customized>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append(" </Addenda>");

                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("</Comprobante>");
                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("</RequestCFD>");

                StreamWriter writer = new StreamWriter(rutaXML);
                writer.Write(xmlRequest.ToString());
                writer.Close();
            }
            catch (Exception ex)
            {
                //throw ex;
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pagosDTO", pagosDTO);
                parametros.Add("rutaXML", rutaXML);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }
        }

        private void ConstruirXMLEgreso(Pago pago, string rutaXML)
        {
            try
            {
                List<Impuestos> lstImpuestos = new List<Impuestos>();
                Impuestos impuestoDTO = new Impuestos(); ;
                String Cargos = String.Empty;
                String OtroCargos = String.Empty;
                String TUA = String.Empty;
                Decimal TotalCargos = 0;

                /* SE LLENAN LOS DATOS PARA FORMAR EL XML */
                StringBuilder xmlRequest = new StringBuilder();
                pago.FechaEmision = DateTime.Now.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss");

                //xmlRequest.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                xmlRequest.Append("<RequestCFD version=\"3.3\">");
                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("<Comprobante ");
                xmlRequest.Append("Version=\"" + pago.LstDatosGralDTO[0].VersionCFDI + "\" ");
                xmlRequest.Append("Serie=\"" + pago.Serie + "\" ");
                xmlRequest.Append("Folio=\"" + pago.NoFolio + "\" ");
                xmlRequest.Append("Fecha=\"" + pago.FechaEmision + "\" ");
                xmlRequest.Append("FormaPago=\"" + pago.CodigoFP + "\" ");
                xmlRequest.Append("NoCertificado=\"00001000000301999105\" "); //MASS
                xmlRequest.Append("SubTotal=\"" + pago.SubTotalPago.ToString(new CultureInfo("es-MX")) + "\" ");
                xmlRequest.Append("Moneda=\"" + pago.CodigoM + "\" ");
                if (pago.TipoCambio == 1)
                {
                    xmlRequest.Append("TipoCambio=\"" + 1 + "\" ");
                }
                else
                {
                    xmlRequest.Append("TipoCambio=\"" + pago.TipoCambio.ToString(new CultureInfo("es-MX")) + "\" ");
                }

                xmlRequest.Append("Total=\"" + pago.TotalPago.ToString(new CultureInfo("es-MX")) + "\" ");

                if (pago.DescuentoPago > 0)
                {
                    xmlRequest.Append("Descuento=\"" + pago.DescuentoPago.ToString(new CultureInfo("es-MX")) + "\" ");
                }
                xmlRequest.Append("TipoDeComprobante=\"" + pago.CodigoTC + "\" ");
                xmlRequest.Append("MetodoPago=\"" + pago.CodigoMP + "\" ");
                xmlRequest.Append("LugarExpedicion=\"66600\" ");
                xmlRequest.Append("permitirConfirmacion=\"false\">");
                //xmlRequest.Append("NoCertificado=\"VOLARIS\" ");
                //xmlRequest.Append("Certificado=\"VOLARIS\" ");
                //xmlRequest.Append("Sello=\"VOLARIS\" ");
                //xmlRequest.Append("xmlns:aerolineas=\"" + "http://www.sat.gob.mx/aerolineas" + "\" ");
                //xmlRequest.Append("xmlns:xsi=\"" + "http://www.w3.org/2001/XMLSchema-instance" + "\" ");
                //xmlRequest.Append("xmlns:tfd=\"" + "http://www.sat.gob.mx/TimbreFiscalDigital" + "\" ");
                //xmlRequest.Append("xmlns:xs=\"" + "http://www.w3.org/2001/XMLSchema" + "\" ");
                //xmlRequest.Append("xmlns:cfdi=\"" + "http://www.sat.gob.mx/cfd/3" + "\" ");
                //xmlRequest.Append("xmlns:terceros = \"" + "http://www.sat.gob.mx/terceros" + "\" ");
                //xmlRequest.Append("xsi:schemaLocation=\"" + "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd http://www.sat.gob.mx/terceros http://www.sat.gob.mx/sitio_internet/cfd/terceros/terceros11.xsd http://www.sat.gob.mx/aerolineas http://www.sat.gob.mx/sitio_internet/cfd/aerolineas/aerolineas.xsd" + "\">");

                if (pago.LstPagosEgresosGlobal.Count > 0)
                {
                    if (!String.IsNullOrEmpty(pago.LstPagosEgresosGlobal[0].UUID))
                    {
                        /*CFDIRELACIONADO*/
                        xmlRequest.Append(Environment.NewLine);
                        xmlRequest.Append(" <CfdiRelacionados ");
                        xmlRequest.Append("TipoRelacion=\"01\">");
                        xmlRequest.Append(Environment.NewLine);
                        xmlRequest.Append("   <CfdiRelacionado ");
                        xmlRequest.Append("UUID=\"" + pago.LstPagosEgresosGlobal[0].UUID + "\"/> ");
                        xmlRequest.Append(Environment.NewLine);
                        xmlRequest.Append(" </CfdiRelacionados>");
                    }
                }

                /*EMISOR*/
                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append(" <Emisor ");
                xmlRequest.Append("Rfc=\"" + pago.RFCEmisor + "\" ");
                xmlRequest.Append("Nombre=\"" + pago.RazonSocialEmisor + "\" ");
                xmlRequest.Append("RegimenFiscal=\"" + pago.CodigoRFEmisor + "\"/>");
                xmlRequest.Append(Environment.NewLine);

                /*RECEPTOR*/
                xmlRequest.Append(" <Receptor ");
                string rfcTReceptor = pago.LstDatosFiscales[0].RFC.Replace("&", "&amp;");
                xmlRequest.Append("Rfc=\"" + rfcTReceptor + "\" ");

                //if (!String.IsNullOrEmpty(pago.LstDatosFiscales[0].RazonSocial))
                //{
                //    xmlRequest.Append("Nombre=\"" + pago.LstDatosFiscales[0].RazonSocial + "\" ");
                //}

                xmlRequest.Append("UsoCFDI=\"" + pago.LstDatosFiscales[0].CodigoUsoCFDI + "\"/> ");
                xmlRequest.Append(Environment.NewLine);

                /*CONCEPTO*/
                xmlRequest.Append(" <Conceptos>");
                xmlRequest.Append(Environment.NewLine);

                /*CICLO PARA RECORRER LA LISTA DE LOS DATOS PARA CONCEPTO.*/
                for (int i = 0; i < pago.LstConcepto.Count; i++)
                {
                    if (pago.LstConcepto[i].PrecioUnitario > 0)
                    {
                        xmlRequest.Append("     <Concepto ");
                        xmlRequest.Append("ClaveProdServ=\"" + pago.LstConcepto[i].CodigoProdSer + "\" ");
                        xmlRequest.Append("NoIdentificacion=\"" + pago.LstConcepto[i].CodigoProdSer + "\" ");
                        xmlRequest.Append("Cantidad=\"" + pago.LstConcepto[i].Cantidad + "\" ");
                        xmlRequest.Append("ClaveUnidad=\"" + pago.LstConcepto[i].ClaveUnidad + "\" ");
                        xmlRequest.Append("Unidad=\"" + pago.LstConcepto[i].Unidad + "\" ");
                        xmlRequest.Append("Descripcion=\"" + pago.LstConcepto[i].DescProdSer + "\" ");

                        if (pago.LstConcepto[i].TipoConcepto == 1 && pago.DescuentoPago > 0)
                        {
                            xmlRequest.Append("Descuento=\"" + pago.DescuentoPago.ToString(new CultureInfo("es-MX")) + "\" ");
                        }

                        xmlRequest.Append("ValorUnitario=\"" + pago.LstConcepto[i].PrecioUnitario.ToString(new CultureInfo("es-MX")) + "\" ");
                        xmlRequest.Append("Importe=\"" + pago.LstConcepto[i].Importe.ToString(new CultureInfo("es-MX")) + "\"> ");

                        if (pago.LstConcepto[i].ImporteTotal > 0)
                        {
                            /*IMPUESTOS TRASLADO*/
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("         <Impuestos>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("             <Traslados>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("             <Traslado ");
                            xmlRequest.Append("Base=\"" + pago.LstConcepto[i].BaseTraslado.ToString(new CultureInfo("es-MX")) + "\" ");
                            xmlRequest.Append("Impuesto=\"" + pago.LstConcepto[i].Impuesto + "\" ");
                            xmlRequest.Append("TipoFactor=\"" + pago.LstConcepto[i].Factor + "\" ");
                            xmlRequest.Append("TasaOCuota=\"" + Math.Round(pago.LstConcepto[i].TasaOCuota, 2).ToString(new CultureInfo("es-MX")) + "\" ");
                            xmlRequest.Append("Importe=\"" + pago.LstConcepto[i].ImporteTotal.ToString(new CultureInfo("es-MX")) + "\"/>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("             </Traslados>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("         </Impuestos>");


                        }

                        if (!String.IsNullOrEmpty(pago.LstConcepto[i].RFCTercero))
                        {
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("         <ComplementoConcepto>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("             <terceros:PorCuentadeTerceros ");
                            xmlRequest.Append("version=\"" + "1.1\" ");
                            xmlRequest.Append("rfc=\"" + pago.LstConcepto[i].RFCTercero + "\">");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("                 <terceros:Parte ");
                            xmlRequest.Append("cantidad =\"" + pago.LstConcepto[i].Cantidad + "\" ");
                            xmlRequest.Append("unidad=\"" + pago.LstConcepto[i].Unidad + "\" ");
                            xmlRequest.Append("noIdentificacion=\"" + pago.LstConcepto[i].CodigoProdSer + "\" ");
                            xmlRequest.Append("descripcion=\"" + pago.LstConcepto[i].DescProdSer + "\" ");
                            xmlRequest.Append("valorUnitario=\"" + pago.LstConcepto[i].PrecioUnitario.ToString(new CultureInfo("es-MX")) + "\" ");
                            xmlRequest.Append("importe=\"" + pago.LstConcepto[i].Importe.ToString(new CultureInfo("es-MX")) + "\"/> ");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("                 <terceros:Impuestos/>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("             </terceros:PorCuentadeTerceros>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("         </ComplementoConcepto>");
                        }

                        xmlRequest.Append(Environment.NewLine);
                        xmlRequest.Append("     </Concepto>");
                        xmlRequest.Append(Environment.NewLine);
                    }

                }

                xmlRequest.Append(" </Conceptos>");

                decimal totalImporteTraslado = 0;
                for (int i = 0; i < pago.LstConcepto.Count; i++)
                {
                    if (pago.LstConcepto[i].ImporteTotal > 0)
                    {
                        impuestoDTO = new Impuestos();

                        totalImporteTraslado += pago.LstConcepto[i].ImporteTotal;
                        impuestoDTO.CodigoImpuesto = pago.LstConcepto[i].Impuesto;
                        impuestoDTO.TasaOCuota = pago.LstConcepto[i].TasaOCuota;
                        impuestoDTO.DescripcionTipoFactor = pago.LstConcepto[i].Factor;


                    }
                }
                impuestoDTO.Importe = totalImporteTraslado;
                lstImpuestos.Add(impuestoDTO);

                if (lstImpuestos.Count > 0 && lstImpuestos[0].DescripcionTipoFactor.Length > 0)
                {
                    /*IMPORTE*/
                    xmlRequest.Append(Environment.NewLine);
                    xmlRequest.Append(" <Impuestos ");

                    xmlRequest.Append("TotalImpuestosTrasladados=\"" + lstImpuestos[0].Importe.ToString(new CultureInfo("es-MX")) + "\">");

                    xmlRequest.Append(Environment.NewLine);
                    xmlRequest.Append("     <Traslados>");
                    xmlRequest.Append(Environment.NewLine);
                    xmlRequest.Append("         <Traslado ");
                    xmlRequest.Append("Impuesto=\"" + lstImpuestos[0].CodigoImpuesto + "\" ");
                    xmlRequest.Append("TipoFactor=\"" + lstImpuestos[0].DescripcionTipoFactor + "\" ");
                    xmlRequest.Append("TasaOCuota=\"" + Math.Round(lstImpuestos[0].TasaOCuota, 2).ToString(new CultureInfo("es-MX")) + "\" ");
                    xmlRequest.Append("Importe=\"" + lstImpuestos[0].Importe.ToString(new CultureInfo("es-MX")) + "\"/>");

                    xmlRequest.Append(Environment.NewLine);
                    xmlRequest.Append("     </Traslados>");
                    xmlRequest.Append(Environment.NewLine);
                    xmlRequest.Append(" </Impuestos>");
                }

                //if (pago.LstCargos.Count > 0)
                //{
                //    /*RECORREMOS LA LISTA PARA OBTENER LA INFORMACIÓN DEL TUA*/
                //    for (int a = 0; a < pago.LstCargos.Count; a++)
                //    {
                //        if (pago.LstCargos[a].CodigoCargo == "TUA")
                //        {
                //            TUA = pago.LstCargos[a].CodigoCargo + "=\"" + pago.LstCargos[a].TotalCargo.ToString(new CultureInfo("es-MX")) + "\"";
                //        }
                //        else
                //        {
                //            TotalCargos += pago.LstCargos[a].TotalCargo;
                //            OtroCargos = "  <aerolineas:OtrosCargos TotalCargos=\"" + TotalCargos.ToString(new CultureInfo("es-MX")) + "\">";
                //            Cargos += "     <aerolineas:Cargo CodigoCargo" + "=\"" + pago.LstCargos[a].CodigoCargo + "\" Importe" + "=\"" + pago.LstCargos[a].TotalCargo.ToString(new CultureInfo("es-MX")) + "\"/>" + Environment.NewLine;
                //        }

                //        if (String.IsNullOrEmpty(TUA))
                //        {
                //            TUA = "TUA=\"" + "0".ToString(new CultureInfo("es-MX")) + "\"";
                //        }
                //    }

                //    /*COMPLEMENTO DE PAGO*/
                //    xmlRequest.Append(Environment.NewLine);
                //    xmlRequest.Append(" <Complemento>");
                //    xmlRequest.Append(Environment.NewLine);

                //    if (TotalCargos == 0)
                //    {
                //        xmlRequest.Append("   <aerolineas:Aerolineas Version =\"1.0\" " + TUA + " />");
                //    }
                //    else
                //    {
                //        xmlRequest.Append("   <aerolineas:Aerolineas Version =\"1.0\" " + TUA + ">");
                //        xmlRequest.Append(Environment.NewLine);
                //        xmlRequest.Append(OtroCargos);
                //        xmlRequest.Append(Environment.NewLine);
                //        xmlRequest.Append(Cargos);
                //        xmlRequest.Append("     </aerolineas:OtrosCargos>");
                //        xmlRequest.Append(Environment.NewLine);
                //        xmlRequest.Append(" </aerolineas:Aerolineas>");
                //    }

                //    xmlRequest.Append(Environment.NewLine);
                //    xmlRequest.Append(" </Complemento>");
                //}

                /*ADENDA*/
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append(" <Addenda>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append("   <customized>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append("      <VOLARIS>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append("         <PNR>" + pago.PNR + "</PNR>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append("         <FECHA>" + pago.FechaPago + "</FECHA>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append("      </VOLARIS>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append("   </customized>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append(" </Addenda>");

                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("</Comprobante>");

                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("    <Transaccion ");

                //CVG    
                //Se agregan los valores adicionales para la facturacion

                xmlRequest.Append("id=\"" + GeneratransactionID(pago.RFCEmisor, pago.NoFolio.ToString(), pago.FechaEmision).ToString() + "\"/>");
                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("	<TipoComprobante ");
                xmlRequest.Append("clave=\"Factura B\" ");
                xmlRequest.Append("nombre=\"Factura B\"/>");
                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("	<Sucursal ");
                xmlRequest.Append("nombre=\"MATRIZ\"/>");
                //xmlRequest.Append(Environment.NewLine);
                //xmlRequest.Append("	<Receptor ");
                //xmlRequest.Append("emailReceptor=\"str1234\" ");
                //xmlRequest.Append("codigoReceptor=\"str1234\"/>");

                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("</RequestCFD>");

                StreamWriter writer = new StreamWriter(rutaXML);
                writer.Write(xmlRequest.ToString());
                writer.Close();
            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pago", pago);
                parametros.Add("rutaXML", rutaXML);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }

        }

        public string GeneratransactionID(string rfcEmisor, string folioFiscal, string fechaFactura)
        {
            string result = "";

            try
            {
                string codigoSAP = "";
                //string leyenda = "CREDIT";

                //codigoSAP = dtResult.Select("Nombre = 'CodigoSAPGenerico'").CopyToDataTable().Rows[0]["Valor"].ToString();

                result = string.Format("ANA4000902{0:yyMMdd}CREDIT-{1}", DateTime.Now, folioFiscal.PadLeft(8, '0'));
                //rfcEmisor.Substring(0, 3)+ "4000902" + fechaFactura + leyenda + "-" + folioFiscal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //catch (SqlException ex)
            //{
            //    //string mensajeUsuario = "Por el momento no es posible procesar su solicitud, por favor intente mas tarde, si el problema continua comuniquese con el área de Call Center...";
            //    string mensajeUsuario = MensajeErrorUsuario;
            //    BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "Facturacion", "BD");
            //    throw new ExceptionViva(mensajeUsuario);
            //}
            //catch (Exception ex)
            //{
            //    //string mensajeUsuario = "Por el momento no es posible procesar sus facturas, intente mas tarde, si el problema continua por favor comuniquese con el area de Call Center...";
            //    string mensajeUsuario = MensajeErrorUsuario;
            //    BllLogErrores.RegistrarError("GLOBAL", mensajeUsuario, ex, "Facturacion", "GeneratransactionID");
            //    throw new ExceptionViva(mensajeUsuario);
            //}

            return result;
        }

        private string GuardaArchivosEgresos(Model.Facturacion.Pago pago, string tipoArchivo)
        {
            try
            {
                pago.RutaArchivosCFDI = pago.LstDatosGralDTO[0].CarpetaArchivosCFDI + @"\" + pago.PNR + "_E";

                /*VALIDAMOS SI EXISTE LA CARPETA DEL PNR, SI NO EXISTE SE CREA*/
                if (!Directory.Exists(pago.RutaArchivosCFDI))
                {
                    /*CREAMOS LA CARPETA DEL PNR*/
                    Directory.CreateDirectory(pago.RutaArchivosCFDI);
                }

                /*ASIGNAMOS EL NOMBRE DEL ARCHIVO EN LA CARPETA ASIGNADA*/
                pago.RutaArchivoXML = pago.RutaArchivosCFDI + "\\" + pago.LstDatosFiscales[0].RFC + "_" + pago.PNR + "_" + pago.NoFolio + "." + tipoArchivo;

                return pago.RutaArchivoXML;
            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pago", pago);
                parametros.Add("tipoArchivo", tipoArchivo);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }
        }

    }
}

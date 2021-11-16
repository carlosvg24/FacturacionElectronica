using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using VBFactPaquetes.Comun.Log;
using VBFactPaquetes.Comun.Utilerias;
using VBFactPaquetes.Model.Facturacion;

namespace VBFactPaquetes.Comun.PDF
{
    public class GeneraFactura
    {
        /* MÉTODO PARA ALMACENAR LOS ARCHIVOS DE LA FACTURA (XML Y PDF)*/
        public static String GuardaArchivosFactura(Pago pago, String tipoArchivo)
        {
            pago.RutaArchivosCFDI = pago.LstDatosGralDTO[0].CarpetaArchivosCFDI + @"\" + pago.PNR;

            /*VALIDAMOS SI EXISTE LA CARPETA DEL PNR, SI NO EXISTE SE CREA*/
            if (!Directory.Exists(pago.RutaArchivosCFDI))
            {
                /*CREAMOS LA CARPETA DEL PNR*/
                Directory.CreateDirectory(pago.RutaArchivosCFDI);
            }

            /*ASIGNAMOS EL NOMBRE DEL ARCHIVO EN LA CARPETA ASIGNADA*/
            if(tipoArchivo == "XML")
                return pago.RutaArchivoXML = pago.RutaArchivosCFDI + "\\" + pago.Serie + pago.NoFolio.ToString() + "." + tipoArchivo;
            else //if (tipoArchivo == "PDF")
                return pago.RutaArchivoPDF = pago.RutaArchivosCFDI + "\\" + pago.Serie + pago.NoFolio.ToString() + "." + tipoArchivo;

            //return pago.RutaArchivoXML;
        }

        public String GuardaArchivosFactura(Pago pagosDTO, String tipoArchivo, String tipoProceso)
        {
            pagosDTO.RutaArchivosCFDI = pagosDTO.LstDatosGralDTO[0].CarpetaArchivosCFDI + @"\" + pagosDTO.PNR;

            /*VALIDAMOS SI EXISTE LA CARPETA DEL PNR, SI NO EXISTE SE CREA*/
            if (!Directory.Exists(pagosDTO.RutaArchivosCFDI))
            {
                /*CREAMOS LA CARPETA DEL PNR*/
                Directory.CreateDirectory(pagosDTO.RutaArchivosCFDI);
            }

            if (tipoProceso == "PAQUETE")
            {
                /*ASIGNAMOS EL NOMBRE DEL ARCHIVO EN LA CARPETA ASIGNADA*/
                pagosDTO.RutaArchivoXML = pagosDTO.RutaArchivosCFDI + "\\" + pagosDTO.LstDatosFiscales[0].RFC + "_" + pagosDTO.PNR + "_" + pagosDTO.NoFolio + "." + tipoArchivo;
            }
            else if (tipoProceso == "VB")
            {
                int i = 0;

                foreach (Conceptos concepto in pagosDTO.LstConcepto)
                {
                    if (!String.IsNullOrEmpty(concepto.RFCTercero))
                        break;
                    i++;
                }

                /*ASIGNAMOS EL NOMBRE DEL ARCHIVO EN LA CARPETA ASIGNADA*/
                pagosDTO.RutaArchivoXML = pagosDTO.RutaArchivosCFDI + "\\" + pagosDTO.LstDatosFiscales[0].RFC + "_" + pagosDTO.LstConcepto[i].PNRBoleto + "_" + tipoProceso + "." + tipoArchivo;
            }

            return pagosDTO.RutaArchivoXML;
        }

        public static Boolean GeneraPDFFactura(ref Pago pago)
        {

            List<ImpuestosDTO> lstImpuestos = new List<ImpuestosDTO>();
            String rutaPDF = String.Empty;
            Decimal totalIVA = 0;
            ConceptosDTO totalConceptos = new ConceptosDTO();
            ImpuestosDTO impuestoDTO;
            string pathFiles = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                /*MANDAMOS A LLAMAR EL METODO (GuardaArchivosFactura) Y LE PASAMOS EL OBJETO*/
                pago.RutaArchivoPDF = GuardaArchivosFactura(pago, "PDF");

                /*PASAMOS LA RUTA DEL ARCHIVO PARA CREARLO*/
                /*PASAMOS LA RUTA DEL ARCHIVO PDF PARA CREARLO*/
                //FileStream fs = new FileStream(pagosDTO.RutaArchivoPDF, FileMode.Create);
                MemoryStream msPDF = new MemoryStream();
                //Document document = new Document(iTextSharp.text.PageSize.LETTER, 0, 0, 35, 40);
                Document document = new Document(PageSize.LETTER);
                document.SetMargins(0f, 0f, 35f, 40f);
                PdfWriter pw = PdfWriter.GetInstance(document, msPDF);

                pw.PageEvent = new EventoTitulos();

                document.Open();

                /*GUARDAMOS LA RUTA DE LA IMAGEN EN LA VARIABLE (rutaImgen)*/
                iTextSharp.text.Image itextLogo = Image.GetInstance(pathFiles + "/assets/images/logo.png");
                //itextLogo.SetAbsolutePosition(25, 725); //(Cordenada X, Cordenada Y)
                itextLogo.ScaleToFit(200f, 80f);
                //document.Add(itextLogo);

                //CVG Inicio
                var tbl = new PdfPTable(new float[] { 60f, 40f }) { WidthPercentage = 95 };


                tbl.AddCell(new PdfPCell(itextLogo) { Border = 0, Rowspan = 3, VerticalAlignment = Element.ALIGN_CENTER });
                tbl.AddCell(new PdfPCell(new Phrase("RFC                                      : " + pago.RFCEmisor, FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL))) { Border = 0 });
                tbl.AddCell(new PdfPCell(new Phrase("RAZÓN SOCIAL                   : " + pago.RazonSocialEmisor, FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL))) { Border = 0 });
                tbl.AddCell(new PdfPCell(new Phrase("REGIMEN FISCAL                : " + pago.CodigoRFEmisor.ToString(), FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL))) { Border = 0 });
                //CVG Fin

                document.Add(tbl);

                /*SE AGREGA ESPACIO ENTRE LAS IMAGENES Y LA TABLA*/
                document.Add(new Paragraph(10, "\u00a0"));

                //AGREGAMOS UNA LINEA AL DOCUMENTO
                //Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(13.0F, 95.0F, new BaseColor(163, 34, 136), Element.ALIGN_CENTER, 1)));
                //document.Add(p);

                /*TABLA PRINCIPAL QUE ALMACENARA LOS DATOS DEL CLIENTE*/
                PdfPTable tablaTituloCliente = new PdfPTable(1);
                tablaTituloCliente.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaTituloCliente.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(219, 249, 178);
                tablaTituloCliente.HorizontalAlignment = 1;
                tablaTituloCliente.WidthPercentage = 95;

                PdfPTable tablaTitulo = new PdfPTable(1);
                tablaTitulo.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell cliente = new PdfPCell(new Phrase("CLIENTE", FontFactory.GetFont("Times New Roman", 9, iTextSharp.text.Font.BOLDITALIC, new BaseColor(0, 0, 0))));
                cliente.Border = 0;

                tablaTitulo.AddCell(cliente);

                tablaTituloCliente.AddCell(tablaTitulo);

                document.Add(tablaTituloCliente);

                //TABLA PRINCIPAL DEL CLIENTE
                PdfPTable tablaClientePrincipal = new PdfPTable(1);
                tablaClientePrincipal.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaClientePrincipal.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                tablaClientePrincipal.HorizontalAlignment = 1;
                tablaClientePrincipal.WidthPercentage = 95;

                /*TABLA QUE CONTIENE LA INFORMACION DEL LADO IZQUIERDO*/
                PdfPTable tablaCliente = new PdfPTable(1);
                tablaCliente.DefaultCell.Border = Rectangle.NO_BORDER;

                /*CELDAS FILA IZQUIERDA*/
                PdfPCell cell1 = new PdfPCell(new Phrase("RFC                                            : " + pago.LstDatosFiscales[0].RFC, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0))));
                PdfPCell cell2 = new PdfPCell(new Phrase("Uso del CFDI                                   : " + pago.LstDatosFiscales[0].UsoCFDI + " - " + pago.LstDatosFiscales[0].DescUsoCFDI, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0))));

                //ALINEAMOS EL TEXTO DE LAS CELDAS AL LADO IZQUIERDO
                cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                /*ELIMINAMOS LOS BARDES DE CADA CELDA*/
                cell1.Border = 0;
                cell2.Border = 0;

                /*AGREGAMOS A LAS CELDAS A LA FILA IZQUIERDA*/
                tablaCliente.AddCell(cell1);
                tablaCliente.AddCell(cell2);

                tablaClientePrincipal.AddCell(tablaCliente);

                document.Add(tablaClientePrincipal);

                document.Add(new Paragraph(5, "\u00a0"));

                /*TABLA INFO FISCAL INICIO*/

                PdfPTable tablaTituloFiscal = new PdfPTable(1);
                tablaTituloFiscal.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaTituloFiscal.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(219, 249, 178);
                tablaTituloFiscal.HorizontalAlignment = 1;
                tablaTituloFiscal.WidthPercentage = 95;

                PdfPTable tablaFiscal = new PdfPTable(1);
                tablaFiscal.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell fiscal = new PdfPCell(new Phrase("DATOS COMPROBANTE", FontFactory.GetFont("Times New Roman", 9, iTextSharp.text.Font.BOLDITALIC, new BaseColor(0, 0, 0))));
                fiscal.Border = 0;

                tablaFiscal.AddCell(fiscal);

                tablaTituloFiscal.AddCell(tablaFiscal);

                document.Add(tablaTituloFiscal);

                PdfPTable tablaFiscalesPrincipal = new PdfPTable(1);
                tablaFiscalesPrincipal.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaFiscalesPrincipal.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                tablaFiscalesPrincipal.HorizontalAlignment = 1;
                tablaFiscalesPrincipal.WidthPercentage = 95;

                PdfPTable tablaFiscales = new PdfPTable(1);
                tablaFiscales.DefaultCell.Border = Rectangle.NO_BORDER;

                ///*CELDAS FILA IZQUIERDA*/
                PdfPCell cell3 = new PdfPCell(new Phrase("Versión                                             : 3.3", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell cell4 = new PdfPCell(new Phrase("Fecha Emisión                                 : " + pago.FechaEmision, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell cell5 = new PdfPCell(new Phrase("Forma de Pago                                : " + pago.DescFP, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                //PdfPCell cell6 = new PdfPCell(new Phrase("Condiciones de Pago           : ", FontFactory.GetFont("Times New Roman", 8, Font.NORMAL)));
                PdfPCell cell7 = new PdfPCell(new Phrase("Tipo de Comprobante                      : " + pago.DescTC, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell cell8 = new PdfPCell(new Phrase("Método de Pago                              : " + pago.DescMP, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell cell9 = new PdfPCell(new Phrase("Lugar de Expedición                        : " + pago.LstDatosFiscales[0].CodigoPostal, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));

                //ALINEAMOS EL TEXTO DE LAS CELDAS AL LADO IZQUIERDO
                cell3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //cell6.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell8.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                ///*ELIMINAMOS LOS BARDES DE CADA CELDA*/
                cell3.Border = 0;
                cell4.Border = 0;
                cell5.Border = 0;
                //cell6.Border = 0;
                cell7.Border = 0;
                cell8.Border = 0;
                cell9.Border = 0;

                /*AGREGAMOS A LAS CELDAS A LA FILA IZQUIERDA*/
                tablaFiscales.AddCell(cell3);
                tablaFiscales.AddCell(cell4);
                tablaFiscales.AddCell(cell5);
                //tablaFiscales.AddCell(cell6);
                tablaFiscales.AddCell(cell7);
                tablaFiscales.AddCell(cell8);
                tablaFiscales.AddCell(cell9);

                tablaFiscalesPrincipal.AddCell(tablaFiscales);

                document.Add(tablaFiscalesPrincipal);
                ///*TABLA INFO FISCAL FIN*/

                document.Add(new Paragraph(5, "\u00a0"));

                /*PRODUCTOS SERVICIOS*/
                PdfPTable tablaTituloProdSer = new PdfPTable(1);
                tablaTituloProdSer.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaTituloProdSer.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(219, 249, 178);
                tablaTituloProdSer.HorizontalAlignment = 1;
                tablaTituloProdSer.WidthPercentage = 95;

                PdfPTable tablaProdSer = new PdfPTable(1);
                tablaProdSer.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell prodSer = new PdfPCell(new Phrase("PRODUCTOS / SERVICIOS", FontFactory.GetFont("Times New Roman", 9, iTextSharp.text.Font.BOLDITALIC, new BaseColor(0, 0, 0))));
                prodSer.Border = 0;

                tablaProdSer.AddCell(prodSer);

                tablaTituloProdSer.AddCell(tablaProdSer);

                document.Add(tablaTituloProdSer);

                //TABLA CONTENIDO TITULOS DE LA FACTURA
                PdfPTable tablaDatosGen = new PdfPTable(1);
                tablaDatosGen.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaDatosGen.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                tablaDatosGen.WidthPercentage = 95;

                ///*AGREGAMOS LA TABLA PARA MOSTRAR LOS CONCEPTOS*/
                PdfPTable tablaConceptos = new PdfPTable(11);
                tablaConceptos.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaConceptos.WidthPercentage = 95;

                /*CREAMOS LAS CELDAS PARA MOSTRAR LA INFORMACIÓN DE LOS CONCEPTOS*/
                PdfPCell cellClavSer = new PdfPCell(new Phrase("CVESERV", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                //PdfPCell cellNumID = new PdfPCell(new Phrase("NO.ID", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellCant = new PdfPCell(new Phrase("CANT", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellClaveUnidad = new PdfPCell(new Phrase("CVE.UNI", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellUnidad = new PdfPCell(new Phrase("UNIDAD", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellDes = new PdfPCell(new Phrase("DESCRIPCIÓN.", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellValUni = new PdfPCell(new Phrase("VALOR UNI", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellImporte = new PdfPCell(new Phrase("IMPORTE", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellBaseIva = new PdfPCell(new Phrase("BASE IVA", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                //PdfPCell cellImpto = new PdfPCell(new Phrase("IMPTO", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellFactor = new PdfPCell(new Phrase("FACTOR", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellTasa = new PdfPCell(new Phrase("TASA", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellImptIva = new PdfPCell(new Phrase("IMPT.IVA", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));

                /*INDICAMOS LAS COLUMNAS QUE MANEJARA CADA CELDA*/
                cellClavSer.Colspan = 1;
                //cellNumID.Colspan = 1;
                cellCant.Colspan = 1;
                cellClaveUnidad.Colspan = 1;
                cellUnidad.Colspan = 1;
                cellDes.Colspan = 1;
                cellValUni.Colspan = 1;
                cellImporte.Colspan = 1;
                cellBaseIva.Colspan = 1;
                //cellImpto.Colspan = 1;
                cellFactor.Colspan = 1;
                cellTasa.Colspan = 1;
                cellImptIva.Colspan = 1;

                /*CENTRAR LOS TEXTOS*/
                cellClavSer.HorizontalAlignment = 1;
                //cellNumID.HorizontalAlignment = 1;
                cellCant.HorizontalAlignment = 1;
                cellClaveUnidad.HorizontalAlignment = 1;
                cellUnidad.HorizontalAlignment = 1;
                cellDes.HorizontalAlignment = 1;
                cellValUni.HorizontalAlignment = 1;
                cellImporte.HorizontalAlignment = 1;
                cellBaseIva.HorizontalAlignment = 1;
                //cellImpto.HorizontalAlignment = 1;
                cellFactor.HorizontalAlignment = 1;
                cellTasa.HorizontalAlignment = 1;
                cellImptIva.HorizontalAlignment = 1;

                /*AGREGAR COLOR DE FONDO*/
                cellClavSer.BackgroundColor = (new BaseColor(223, 223, 223));
                //cellNumID.BackgroundColor = (new BaseColor(223, 223, 223));
                cellCant.BackgroundColor = (new BaseColor(223, 223, 223));
                cellClaveUnidad.BackgroundColor = (new BaseColor(223, 223, 223));
                cellUnidad.BackgroundColor = (new BaseColor(223, 223, 223));
                cellDes.BackgroundColor = (new BaseColor(223, 223, 223));
                cellValUni.BackgroundColor = (new BaseColor(223, 223, 223));
                cellImporte.BackgroundColor = (new BaseColor(223, 223, 223));
                cellBaseIva.BackgroundColor = (new BaseColor(223, 223, 223));
                //cellImpto.BackgroundColor = (new BaseColor(223, 223, 223));
                cellFactor.BackgroundColor = (new BaseColor(223, 223, 223));
                cellTasa.BackgroundColor = (new BaseColor(223, 223, 223));
                cellImptIva.BackgroundColor = (new BaseColor(223, 223, 223));

                /*ELIMINAR LOS BORDESDES DE CADA CELDA*/

                cellClavSer.Border = 0;
                //cellNumID.Border = 0;
                cellCant.Border = 0;
                cellClaveUnidad.Border = 0;
                cellUnidad.Border = 0;
                cellDes.Border = 0;
                cellValUni.Border = 0;
                cellImporte.Border = 0;
                cellBaseIva.Border = 0;
                //cellImpto.Border = 0;
                cellFactor.Border = 0;
                cellTasa.Border = 0;
                cellImptIva.Border = 0;

                /*AGREGAMOS A LA TABLA tablaDescFactura LOS DATOS DE LAS CELDAS*/
                tablaConceptos.AddCell(cellClavSer);
                //tablaConceptos.AddCell(cellNumID);
                tablaConceptos.AddCell(cellCant);
                tablaConceptos.AddCell(cellClaveUnidad);
                tablaConceptos.AddCell(cellUnidad);
                tablaConceptos.AddCell(cellDes);
                tablaConceptos.AddCell(cellValUni);
                tablaConceptos.AddCell(cellImporte);
                tablaConceptos.AddCell(cellBaseIva);
                //tablaConceptos.AddCell(cellImpto);
                tablaConceptos.AddCell(cellFactor);
                tablaConceptos.AddCell(cellTasa);
                tablaConceptos.AddCell(cellImptIva);

                ///*AGREGAMR A LA TABLA tablaDescFacturaGen LA TABLA tablaDescFactura*/
                tablaDatosGen.AddCell(tablaConceptos);
                document.Add(tablaDatosGen);

                ////TABLA CONTENIDO DATOS DE LA FACTURA

                PdfPTable tablaDatosColumna = new PdfPTable(1);
                tablaDatosColumna.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaDatosColumna.WidthPercentage = 95;
                //tablaDatosGen.DefaultCell.BorderColor = (new BaseColor(46, 100, 254));

                //DATOS DE CELDA DE TITULOS (CREAMOS UNA TABLA tablaDescFactura CON UNA SOLA COLUMNA)
                PdfPTable tablaDatosProd = new PdfPTable(11);
                tablaDatosProd.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaDatosProd.WidthPercentage = 95;

                for (int i = 0; i < pago.LstConcepto.Count; i++)
                {

                    impuestoDTO = new ImpuestosDTO();

                    impuestoDTO.Importe = pago.LstConcepto[i].Importe;
                    impuestoDTO.CodigoImpuesto = pago.LstConcepto[i].Impuesto;
                    impuestoDTO.TasaOCuota = pago.LstConcepto[i].TasaOCuota;
                    impuestoDTO.DescripcionTipoFactor = pago.LstConcepto[i].Factor;
                    impuestoDTO.DescripcionImpuesto = pago.LstConcepto[i].DescImpuesto;

                    if (!lstImpuestos.Exists(x => x.TasaOCuota == impuestoDTO.TasaOCuota))
                    {
                        lstImpuestos.Add(impuestoDTO);
                    }
                    else
                    {
                        int index = lstImpuestos.FindIndex(element => element.TasaOCuota == impuestoDTO.TasaOCuota);

                        lstImpuestos[index].Importe += impuestoDTO.Importe;
                    }

                    totalIVA += pago.LstConcepto[i].Importe;

                    PdfPCell clavSer1 = new PdfPCell(new Phrase(pago.LstConcepto[i].CodigoProdSer, FontFactory.GetFont("Times New Roman", 6, BaseColor.BLACK)));
                    //PdfPCell numId1 = new PdfPCell(new Phrase(pago.LstConcepto[i].Cantidad.ToString(), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell cant1 = new PdfPCell(new Phrase(pago.LstConcepto[i].Cantidad.ToString(), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell claveUni1 = new PdfPCell(new Phrase(pago.LstConcepto[i].ClaveUnidad, FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell unidad1 = new PdfPCell(new Phrase(pago.LstConcepto[i].Unidad, FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell desc1 = new PdfPCell(new Phrase(pago.LstConcepto[i].DescProdSer, FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell valUni1 = new PdfPCell(new Phrase("$" + string.Format("{0:###,###,###,###.##}", Math.Round(pago.LstConcepto[i].ImporteBase, 2)), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell import1 = new PdfPCell(new Phrase("$" + string.Format("{0:###,###,###,###.##}", Math.Round(pago.LstConcepto[i].ImporteBase, 2)), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell baseIva1 = new PdfPCell(new Phrase("$" + string.Format("{0:###,###,###,###.##}", Math.Round(pago.LstConcepto[i].ImporteBase, 2)), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    //PdfPCell impto1 = new PdfPCell(new Phrase("0.00", FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell factor1 = new PdfPCell(new Phrase(pago.LstConcepto[i].Factor, FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell tasa1 = new PdfPCell(new Phrase(string.Format("{0:###,###.##}", Math.Round(pago.LstConcepto[i].TasaOCuota, 2)), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell imptIva1 = new PdfPCell(new Phrase("$" + string.Format("{0:###,###,###,###.##}", Math.Round(pago.LstConcepto[i].Importe, 2)), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));

                    //string.Format("{ 0:###,###.##}", pago.LstConcepto[i].TasaOCuota).ToString();
                    //pago.LstConcepto[i].TasaOCuota.ToString()

                    clavSer1.Colspan = 1;
                    //numId1.Colspan = 1;
                    cant1.Colspan = 1;
                    claveUni1.Colspan = 1;
                    unidad1.Colspan = 1;
                    desc1.Colspan = 1;
                    valUni1.Colspan = 1;
                    import1.Colspan = 1;
                    baseIva1.Colspan = 1;
                    //impto1.Colspan = 1;
                    factor1.Colspan = 1;
                    tasa1.Colspan = 1;
                    imptIva1.Colspan = 1;

                    /*ALINEMOS EL TEXTO DE FORMA CENTRADO*/
                    clavSer1.HorizontalAlignment = 1;
                    //numId1.HorizontalAlignment = 1;
                    cant1.HorizontalAlignment = 1;
                    claveUni1.HorizontalAlignment = 1;
                    unidad1.HorizontalAlignment = 1;
                    desc1.HorizontalAlignment = 1;
                    valUni1.HorizontalAlignment = 1;
                    import1.HorizontalAlignment = 1;
                    baseIva1.HorizontalAlignment = 1;
                    //impto1.HorizontalAlignment = 1;
                    factor1.HorizontalAlignment = 1;
                    tasa1.HorizontalAlignment = 1;
                    imptIva1.HorizontalAlignment = 1;

                    /*ELIMINAMOS LOS BORDESDES DE CADA CELDA*/
                    clavSer1.BorderWidthLeft = 0;
                    clavSer1.BorderWidthRight = 0;
                    clavSer1.BorderWidthBottom = 0;
                    clavSer1.BorderWidthTop = 0;

                    //numId1.BorderWidthLeft = 1;
                    //numId1.BorderWidthRight = 1;
                    //numId1.BorderWidthBottom = 1;
                    //numId1.BorderWidthTop = 0;

                    cant1.BorderWidthLeft = 0;
                    cant1.BorderWidthRight = 0;
                    cant1.BorderWidthBottom = 0;
                    cant1.BorderWidthTop = 0;

                    claveUni1.BorderWidthLeft = 0;
                    claveUni1.BorderWidthRight = 0;
                    claveUni1.BorderWidthBottom = 0;
                    claveUni1.BorderWidthTop = 0;

                    unidad1.BorderWidthLeft = 0;
                    unidad1.BorderWidthRight = 0;
                    unidad1.BorderWidthBottom = 0;
                    unidad1.BorderWidthTop = 0;

                    desc1.BorderWidthLeft = 0;
                    desc1.BorderWidthRight = 0;
                    desc1.BorderWidthBottom = 0;
                    desc1.BorderWidthTop = 0;

                    valUni1.BorderWidthLeft = 0;
                    valUni1.BorderWidthRight = 0;
                    valUni1.BorderWidthBottom = 0;
                    valUni1.BorderWidthTop = 0;

                    import1.BorderWidthLeft = 0;
                    import1.BorderWidthRight = 0;
                    import1.BorderWidthBottom = 0;
                    import1.BorderWidthTop = 0;

                    baseIva1.BorderWidthLeft = 0;
                    baseIva1.BorderWidthRight = 0;
                    baseIva1.BorderWidthBottom = 0;
                    baseIva1.BorderWidthTop = 0;

                    //impto1.BorderWidthLeft = 1;
                    //impto1.BorderWidthRight = 1;
                    //impto1.BorderWidthBottom = 1;
                    //impto1.BorderWidthTop = 0;

                    factor1.BorderWidthLeft = 0;
                    factor1.BorderWidthRight = 0;
                    factor1.BorderWidthBottom = 0;
                    factor1.BorderWidthTop = 0;

                    tasa1.BorderWidthLeft = 0;
                    tasa1.BorderWidthRight = 0;
                    tasa1.BorderWidthBottom = 0;
                    tasa1.BorderWidthTop = 0;

                    imptIva1.BorderWidthLeft = 0;
                    imptIva1.BorderWidthRight = 0;
                    imptIva1.BorderWidthBottom = 0;
                    imptIva1.BorderWidthTop = 0;

                    /*AGREGAMOS A LA TABLA tablaDatosProd LOS DATOS DE LAS CELDAS*/
                    tablaDatosProd.AddCell(clavSer1);
                    //tablaDatosProd.AddCell(numId1);
                    tablaDatosProd.AddCell(cant1);
                    tablaDatosProd.AddCell(claveUni1);
                    tablaDatosProd.AddCell(unidad1);
                    tablaDatosProd.AddCell(desc1);
                    tablaDatosProd.AddCell(valUni1);
                    tablaDatosProd.AddCell(import1);
                    tablaDatosProd.AddCell(baseIva1);
                    //tablaDatosProd.AddCell(impto1);
                    tablaDatosProd.AddCell(factor1);
                    tablaDatosProd.AddCell(tasa1);
                    tablaDatosProd.AddCell(imptIva1);
                }

                /*AGREGAMOS A LA TABLA tablaDatosColumna LA TABLA tablaDatosProd*/
                tablaDatosColumna.AddCell(tablaDatosProd);
                document.Add(tablaDatosColumna);

                document.Add(new Paragraph(20, "\u00a0"));

                /*CREAMOS UNA TABLA DE TOTALES, DONDE SE MOSTRARAN TODOS LOS DATOS DE LOS TOTALES Y EL IMPORTE TOTAL CON LETRA*/
                PdfPTable tablaTotalesBorde = new PdfPTable(1);
                tablaTotalesBorde.DefaultCell.BorderWidthTop = 1;
                tablaTotalesBorde.DefaultCell.BorderWidthBottom = 1;
                //tablaTotalesBorde.DefaultCell.BorderWidthRight = 1;
                //tablaTotalesBorde.DefaultCell.BorderWidthLeft = 1;
                tablaTotalesBorde.DefaultCell.BorderColorTop = new BaseColor(0, 0, 0);
                tablaTotalesBorde.DefaultCell.BorderColorBottom = new BaseColor(0, 0, 0);
                //tablaTotalesBorde.DefaultCell.BorderColorRight = new BaseColor(0, 0, 0);
                //tablaTotalesBorde.DefaultCell.BorderColorLeft = new BaseColor(0, 0, 0);
                tablaTotalesBorde.WidthPercentage = 95;

                /*TABLA total QUE ALMACENARA LOS DATOS TOTALES*/
                PdfPTable tablaTotales = new PdfPTable(2);
                tablaTotales.DefaultCell.Border = 0;
                tablaTotales.WidthPercentage = 95;
                tablaTotales.SetTotalWidth(new float[] { 25f, 15f });

                PdfPTable tablaTotalLetra = new PdfPTable(1);
                tablaTotalLetra.DefaultCell.Border = 0;

                PdfPTable tablaTotalLetraIzq = new PdfPTable(1);
                tablaTotalLetraIzq.DefaultCell.Border = 0;

                /*MANDAMOS A PINTAR LOS DATOS EN SUS RESPECTIVAS CELDAS*/
                ToCFDI toCFDI = new ToCFDI();
                PdfPCell importeLetra = new PdfPCell(new Phrase("Importe con Letra: " + toCFDI.ConvertirNumLetras(pago.TotalPago.ToString(), true, pago.CodigoM), FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));

                //ALINEAMOS EL TEXTO DE LAS CELDAS AL LADO IZQUIERDO;
                importeLetra.HorizontalAlignment = 0;
                importeLetra.Border = 0;

                /*AGREGAMOS LA TABLA total LOS DATOS DE LA CELDA totalLetra*/
                tablaTotalLetraIzq.AddCell(importeLetra);
                tablaTotalLetra.AddCell(tablaTotalLetraIzq);
                tablaTotales.AddCell(tablaTotalLetra);

                /*TABLA General QUE ALMACENARA LOS DATOS PERTENECIENTES A LA CELDA*/
                PdfPTable TablaTotal = new PdfPTable(2);
                TablaTotal.DefaultCell.Border = Rectangle.NO_BORDER;
                TablaTotal.WidthPercentage = 95;

                //DATOS DE CELDA2  (CREAMOS UNA TABALA CON UNA SOLA COLUMNA)
                PdfPTable tablaTotalIzq = new PdfPTable(1);
                tablaTotalIzq.HorizontalAlignment = 2;
                tablaTotalIzq.WidthPercentage = 70;

                /*MANDAMOS A PINTAR LOS DATOS EN SUS RESPECTIVAS CELDAS*/
                PdfPCell SubTotales = new PdfPCell(new Phrase("SubTotal", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell Descuento = new PdfPCell(new Phrase("Descuento", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell IvaTraslado = new PdfPCell(new Phrase("IVA(16.00 % *)", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell Total = new PdfPCell(new Phrase("Total", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell Moneda = new PdfPCell(new Phrase("Moneda", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell TipoCambio = new PdfPCell(new Phrase("Tipo Cambio", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));

                //ALINEAMOS EL TEXTO DE LAS CELDAS AL LADO IZQUIERDO;
                /*ELIMINAMOS LOS BORDES DE DE CADA CELDA*/
                SubTotales.Right = 0;
                Descuento.Right = 0;
                IvaTraslado.Right = 0;
                Total.Right = 0;
                Moneda.Right = 0;
                TipoCambio.Right = 0;

                //ALINEAMOS EL TEXTO DE LAS CELDAS AL LADO IZQUIERDO;
                /*ELIMINAMOS LOS BORDESDES DE CADA CELDA*/
                SubTotales.Border = 0;
                Descuento.Border = 0;
                IvaTraslado.Border = 0;
                Total.Border = 0;
                Moneda.Border = 0;
                TipoCambio.Border = 0;

                /*ELIMINAMOS LOS BORDESDES DE CADA CELDA*/
                tablaTotalIzq.AddCell(SubTotales);
                tablaTotalIzq.AddCell(Descuento);
                tablaTotalIzq.AddCell(IvaTraslado);
                tablaTotalIzq.AddCell(Total);
                tablaTotalIzq.AddCell(Moneda);
                tablaTotalIzq.AddCell(TipoCambio);

                TablaTotal.AddCell(tablaTotalIzq);

                //DATOS DE CELDA2  (CREAMOS UNA TABALA CON UNA SOLA COLUMNA)
                PdfPTable tablaTotalDer = new PdfPTable(1);
                tablaTotalDer.HorizontalAlignment = 2;
                tablaTotalDer.WidthPercentage = 30;

                /*MANDAMOS A PINTAR LOS DATOS EN SUS RESPECTIVAS CELDAS*/
                PdfPCell SubTotalesD = new PdfPCell(new Phrase(pago.SubTotalPago.ToString("C", new CultureInfo("es-MX")), FontFactory.GetFont("Arial", 7)));
                PdfPCell DescuentoD = new PdfPCell(new Phrase(pago.DescuentoPago.ToString("C", new CultureInfo("es-MX")), FontFactory.GetFont("Arial", 7)));
                PdfPCell IvaD = new PdfPCell(new Phrase(totalIVA.ToString("C", new CultureInfo("es-MX")), FontFactory.GetFont("Arial", 7)));
                PdfPCell TotalD = new PdfPCell(new Phrase(pago.TotalPago.ToString("C", new CultureInfo("es-MX")), FontFactory.GetFont("Arial", 7)));
                PdfPCell MonedaD = new PdfPCell(new Phrase(pago.CodigoM + " " + pago.DescM, FontFactory.GetFont("Arial", 7)));
                PdfPCell TipoCambioD = new PdfPCell(new Phrase(pago.TipoCambio.ToString(), FontFactory.GetFont("Arial", 7)));

                /*ALINEAMOS EL TEXTO DE LAS CELDAS AL LADO DERECHO*/
                SubTotalesD.HorizontalAlignment = 2;
                DescuentoD.HorizontalAlignment = 2;
                IvaD.HorizontalAlignment = 2;
                TotalD.HorizontalAlignment = 2;
                MonedaD.HorizontalAlignment = 2;
                TipoCambioD.HorizontalAlignment = 2;

                /*ELIMINAMOS LOS BORDESDES DE CADA CELDA*/
                SubTotalesD.Border = 0;
                DescuentoD.Border = 0;
                IvaD.Border = 0;
                TotalD.Border = 0;
                MonedaD.Border = 0;
                TipoCambioD.Border = 0;

                /*AGREGAMOS LAS CELDAS A LA TABLA*/
                tablaTotalDer.AddCell(SubTotalesD);
                tablaTotalDer.AddCell(DescuentoD);
                tablaTotalDer.AddCell(IvaD);
                tablaTotalDer.AddCell(TotalD);
                tablaTotalDer.AddCell(MonedaD);
                tablaTotalDer.AddCell(TipoCambioD);

                TablaTotal.AddCell(tablaTotalDer);
                /*AGREGAMOS A LA TABLA Principal LOS DATOS DE LAS CELDAS*/
                tablaTotales.AddCell(TablaTotal);
                tablaTotalesBorde.AddCell(tablaTotales);

                document.Add(tablaTotalesBorde);

                document.Add(new Paragraph(5, "\u00a0"));

                //LEYENDA
                PdfPTable tablaLeyenda = new PdfPTable(1);
                tablaLeyenda.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaLeyenda.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(194, 194, 193);
                tablaLeyenda.HorizontalAlignment = 1;
                tablaLeyenda.WidthPercentage = 95;

                PdfPTable tablaTexto = new PdfPTable(1);
                tablaTexto.DefaultCell.BorderWidthLeft = 1;
                tablaTexto.DefaultCell.BorderColorLeft = new BaseColor(0, 0, 0);
                tablaTexto.DefaultCell.BorderWidthRight = 1;
                tablaTexto.DefaultCell.BorderColorRight = new BaseColor(0, 0, 0);

                PdfPCell texto = new PdfPCell(new Phrase("*Tratándose de Transportación Aérea Internacional y Franja Fronteriza, el IVA se calculará al 25% de la tasa general del 16%, de conformidad con el Artículo 16 de la ley del IVA.", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.ITALIC, new BaseColor(0, 0, 0))));
                texto.Border = 0;
                texto.HorizontalAlignment = 1;

                tablaTexto.AddCell(texto);

                tablaLeyenda.AddCell(tablaTexto);

                document.Add(tablaLeyenda);

                document.Add(new Paragraph(5, "\u00a0"));

                //DATOS TIMBRADO CFDI (QR)
                PdfPTable tablaTituloDatosTimbrado = new PdfPTable(1);
                tablaTituloDatosTimbrado.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaTituloDatosTimbrado.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(219, 249, 178);
                tablaTituloDatosTimbrado.HorizontalAlignment = 1;
                tablaTituloDatosTimbrado.WidthPercentage = 95;

                PdfPTable TablaTimbrado = new PdfPTable(1);
                TablaTimbrado.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell timbrado = new PdfPCell(new Phrase("DATOS TIMBRADO CFDI", FontFactory.GetFont("Times New Roman", 9, iTextSharp.text.Font.BOLDITALIC, new BaseColor(0, 0, 0))));
                timbrado.Border = 0;

                TablaTimbrado.AddCell(timbrado);

                tablaTituloDatosTimbrado.AddCell(TablaTimbrado);

                document.Add(tablaTituloDatosTimbrado);

                PdfPTable contQR = new PdfPTable(1);
                contQR.WidthPercentage = 95;
                contQR.DefaultCell.BorderColor = new iTextSharp.text.BaseColor(229, 231, 233);

                PdfPTable contenedorQr = new PdfPTable(2);
                contenedorQr.WidthPercentage = 95;
                contenedorQr.DefaultCell.Border = Rectangle.NO_BORDER;

                float[] widths = new float[] { 8, 30 };
                contenedorQr.SetWidths(widths);

                PdfPTable imagenQR = new PdfPTable(1);

                BarcodeQRCode qrcodes = new BarcodeQRCode(GenerarQR(pago), 1, 1, null);
                iTextSharp.text.Image imgCB = qrcodes.GetImage();
                //imgCB.SetAbsolutePosition(8, 89);
                imgCB.ScaleAbsolute(110f, 120f);

                PdfPCell imgqr = new PdfPCell(imgCB);
                imgqr.Border = 0;
                imagenQR.AddCell(imgqr);
                contenedorQr.AddCell(imagenQR);

                PdfPTable QR = new PdfPTable(1);
                QR.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell cadena = new PdfPCell(new Phrase("Cadena Original:", FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                PdfPCell noSerie = new PdfPCell(new Phrase(pago.CadenaOriginal, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell serie = new PdfPCell(new Phrase("No. de Serie del Certificado del SAT:", FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                PdfPCell certSat = new PdfPCell(new Phrase(pago.NoCertificadoSAT, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell rfcNo = new PdfPCell(new Phrase("RFC proveedor de certificado:", FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                PdfPCell rfcCert = new PdfPCell(new Phrase("SAD110722MQA", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell sat = new PdfPCell(new Phrase("Sello Digital del SAT:", FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                PdfPCell selloSat = new PdfPCell(new Phrase(pago.SelloSAT, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell cdfi = new PdfPCell(new Phrase("Sello Digital del Emisor:", FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                PdfPCell noCertSat = new PdfPCell(new Phrase(pago.SelloDigital, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell csd = new PdfPCell(new Phrase("No. de Serie del Certificado del Emisor:", FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                PdfPCell certEmi = new PdfPCell(new Phrase(pago.NoCertificado, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));

                /*ELIMINAMOS Y ASIGNAMOS LOS BORDES DE CADA CELDA*/
                cadena.Border = 0;
                noSerie.Border = 0;
                serie.Border = 0;
                certSat.Border = 0;
                rfcNo.Border = 0;
                rfcCert.Border = 0;
                sat.Border = 0;
                selloSat.Border = 0;
                cdfi.Border = 0;
                noCertSat.Border = 0;
                csd.Border = 0;
                certEmi.Border = 0;

                QR.AddCell(cadena);
                QR.AddCell(noSerie);
                QR.AddCell(serie);
                QR.AddCell(certSat);
                QR.AddCell(rfcNo);
                QR.AddCell(rfcCert);
                QR.AddCell(sat);
                QR.AddCell(selloSat);
                QR.AddCell(cdfi);
                QR.AddCell(noCertSat);
                QR.AddCell(csd);
                QR.AddCell(certEmi);

                contenedorQr.AddCell(QR);
                contQR.AddCell(contenedorQr);
                document.Add(contQR);

                document.Add(new Paragraph(5, "\u00a0"));

                //LEYENDA2
                PdfPTable tablaTituloDocumentoLeyenda = new PdfPTable(1);
                tablaTituloDocumentoLeyenda.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaTituloDocumentoLeyenda.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(194, 194, 193);
                tablaTituloDocumentoLeyenda.HorizontalAlignment = 1;
                tablaTituloDocumentoLeyenda.WidthPercentage = 95;

                PdfPTable tablaTextoCFDI = new PdfPTable(1);
                tablaTextoCFDI.DefaultCell.BorderWidthLeft = 1;
                tablaTextoCFDI.DefaultCell.BorderColorLeft = new BaseColor(0, 0, 0);
                tablaTextoCFDI.DefaultCell.BorderWidthRight = 1;
                tablaTextoCFDI.DefaultCell.BorderColorRight = new BaseColor(0, 0, 0);

                PdfPCell textoCfdi = new PdfPCell(new Phrase("Este documento es una representación impresa de un CFDI", FontFactory.GetFont("Times New Roman", 9, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                textoCfdi.Border = 0;
                textoCfdi.HorizontalAlignment = 1;

                tablaTextoCFDI.AddCell(textoCfdi);

                tablaTituloDocumentoLeyenda.AddCell(tablaTextoCFDI);

                document.Add(tablaTituloDocumentoLeyenda);

                document.Close();

                System.IO.File.WriteAllBytes(pago.RutaArchivoPDF, msPDF.ToArray());
                pago.ArchivoPDF = msPDF.ToArray();

                return true;

            }
            catch(Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //GENERAR PDF PARA LA NOTA DE CREDITO
        public MemoryStream GeneraPDFEgreso(Pago pagosDTO)
        {

            List<ImpuestosDTO> lstImpuestos = new List<ImpuestosDTO>();
            String rutaPDF = String.Empty;
            Decimal totalIVA = 0;
            ConceptosDTO totalConceptos = new ConceptosDTO();
            ImpuestosDTO impuestoDTO;
            string pathFiles = AppDomain.CurrentDomain.BaseDirectory;

            try
            {
                /*MANDAMOS A LLAMAR EL METODO (GuardaArchivosFactura) Y LE PASAMOS EL OBJETO*/
                pagosDTO.RutaArchivoPDF = GuardaArchivosFactura(pagosDTO, "PDF", "PAQUETE");

                /*PASAMOS LA RUTA DEL ARCHIVO PARA CREARLO*/
                /*PASAMOS LA RUTA DEL ARCHIVO PDF PARA CREARLO*/
                //FileStream fs = new FileStream(pagosDTO.RutaArchivoPDF, FileMode.Create);
                MemoryStream msPDF = new MemoryStream();
                //Document document = new Document(iTextSharp.text.PageSize.LETTER, 0, 0, 35, 40);
                Document document = new Document(PageSize.LETTER);
                document.SetMargins(0f, 0f, 35f, 40f);
                PdfWriter pw = PdfWriter.GetInstance(document, msPDF);

                pw.PageEvent = new EventoTitulos();

                document.Open();

                /*GUARDAMOS LA RUTA DE LA IMAGEN EN LA VARIABLE (rutaImgen)*/
                string path =Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\\..\\assets\\Images\\logo.png"));
                iTextSharp.text.Image itextLogo = Image.GetInstance(path);
                //itextLogo.SetAbsolutePosition(25, 725); //(Cordenada X, Cordenada Y)
                itextLogo.ScaleToFit(200f, 80f);
                //document.Add(itextLogo);

                //CVG Inicio
                var tbl = new PdfPTable(new float[] { 60f, 40f }) { WidthPercentage = 95 };


                tbl.AddCell(new PdfPCell(itextLogo) { Border = 0, Rowspan = 3, VerticalAlignment = Element.ALIGN_CENTER });
                tbl.AddCell(new PdfPCell(new Phrase("RFC                                      : " + pagosDTO.RFCEmisor, FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL))) { Border = 0 });
                tbl.AddCell(new PdfPCell(new Phrase("RAZÓN SOCIAL                   : " + pagosDTO.RazonSocialEmisor, FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL))) { Border = 0 });
                tbl.AddCell(new PdfPCell(new Phrase("REGIMEN FISCAL                : " + pagosDTO.CodigoRFEmisor, FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL))) { Border = 0 });
                //CVG Fin

                document.Add(tbl);

                /*SE AGREGA ESPACIO ENTRE LAS IMAGENES Y LA TABLA*/
                document.Add(new Paragraph(10, "\u00a0"));

                //AGREGAMOS UNA LINEA AL DOCUMENTO
                //Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(13.0F, 95.0F, new BaseColor(163, 34, 136), Element.ALIGN_CENTER, 1)));
                //document.Add(p);

                /*TABLA PRINCIPAL QUE ALMACENARA LOS DATOS DEL CLIENTE*/
                PdfPTable tablaTituloCliente = new PdfPTable(1);
                tablaTituloCliente.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaTituloCliente.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(219, 249, 178);
                tablaTituloCliente.HorizontalAlignment = 1;
                tablaTituloCliente.WidthPercentage = 95;

                PdfPTable tablaTitulo = new PdfPTable(1);
                tablaTitulo.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell cliente = new PdfPCell(new Phrase("CLIENTE", FontFactory.GetFont("Times New Roman", 9, iTextSharp.text.Font.BOLDITALIC, new BaseColor(0, 0, 0))));
                cliente.Border = 0;

                tablaTitulo.AddCell(cliente);

                tablaTituloCliente.AddCell(tablaTitulo);

                document.Add(tablaTituloCliente);

                //TABLA PRINCIPAL DEL CLIENTE
                PdfPTable tablaClientePrincipal = new PdfPTable(1);
                tablaClientePrincipal.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaClientePrincipal.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                tablaClientePrincipal.HorizontalAlignment = 1;
                tablaClientePrincipal.WidthPercentage = 95;

                /*TABLA QUE CONTIENE LA INFORMACION DEL LADO IZQUIERDO*/
                PdfPTable tablaCliente = new PdfPTable(1);
                tablaCliente.DefaultCell.Border = Rectangle.NO_BORDER;

                /*CELDAS FILA IZQUIERDA*/
                PdfPCell cell1 = new PdfPCell(new Phrase("RFC                                                 : " + pagosDTO.LstDatosFiscales[0].RFC, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0))));
                PdfPCell cell2 = new PdfPCell(new Phrase("Uso del CFDI                                   : " + pagosDTO.LstDatosFiscales[0].CodigoUsoCFDI, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0))));

                //ALINEAMOS EL TEXTO DE LAS CELDAS AL LADO IZQUIERDO
                cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                /*ELIMINAMOS LOS BARDES DE CADA CELDA*/
                cell1.Border = 0;
                cell2.Border = 0;

                /*AGREGAMOS A LAS CELDAS A LA FILA IZQUIERDA*/
                tablaCliente.AddCell(cell1);
                tablaCliente.AddCell(cell2);

                tablaClientePrincipal.AddCell(tablaCliente);

                document.Add(tablaClientePrincipal);

                document.Add(new Paragraph(5, "\u00a0"));

                /*TABLA INFO FISCAL INICIO*/

                PdfPTable tablaTituloFiscal = new PdfPTable(1);
                tablaTituloFiscal.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaTituloFiscal.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(219, 249, 178);
                tablaTituloFiscal.HorizontalAlignment = 1;
                tablaTituloFiscal.WidthPercentage = 95;

                PdfPTable tablaFiscal = new PdfPTable(1);
                tablaFiscal.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell fiscal = new PdfPCell(new Phrase("DATOS COMPROBANTE", FontFactory.GetFont("Times New Roman", 9, iTextSharp.text.Font.BOLDITALIC, new BaseColor(0, 0, 0))));
                fiscal.Border = 0;

                tablaFiscal.AddCell(fiscal);

                tablaTituloFiscal.AddCell(tablaFiscal);

                document.Add(tablaTituloFiscal);

                PdfPTable tablaFiscalesPrincipal = new PdfPTable(1);
                tablaFiscalesPrincipal.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaFiscalesPrincipal.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                tablaFiscalesPrincipal.HorizontalAlignment = 1;
                tablaFiscalesPrincipal.WidthPercentage = 95;

                PdfPTable tablaFiscales = new PdfPTable(1);
                tablaFiscales.DefaultCell.Border = Rectangle.NO_BORDER;

                ///*CELDAS FILA IZQUIERDA*/
                PdfPCell cell3 = new PdfPCell(new Phrase("Versión                                             : 3.3", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell cell4 = new PdfPCell(new Phrase("Fecha Emisión                                 : " + pagosDTO.FechaEmision, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell cell5 = new PdfPCell(new Phrase("Forma de Pago                                : " + pagosDTO.DescFP, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                //PdfPCell cell6 = new PdfPCell(new Phrase("Condiciones de Pago           : ", FontFactory.GetFont("Times New Roman", 8, Font.NORMAL)));
                PdfPCell cell7 = new PdfPCell(new Phrase("Tipo de Comprobante                      : " + pagosDTO.DescTC, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell cell8 = new PdfPCell(new Phrase("Método de Pago                              : " + pagosDTO.DescMP, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell cell9 = new PdfPCell(new Phrase("Lugar de Expedición                        : 66600", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));

                //ALINEAMOS EL TEXTO DE LAS CELDAS AL LADO IZQUIERDO
                cell3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                //cell6.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell7.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell8.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell9.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                ///*ELIMINAMOS LOS BARDES DE CADA CELDA*/
                cell3.Border = 0;
                cell4.Border = 0;
                cell5.Border = 0;
                //cell6.Border = 0;
                cell7.Border = 0;
                cell8.Border = 0;
                cell9.Border = 0;

                /*AGREGAMOS A LAS CELDAS A LA FILA IZQUIERDA*/
                tablaFiscales.AddCell(cell3);
                tablaFiscales.AddCell(cell4);
                tablaFiscales.AddCell(cell5);
                //tablaFiscales.AddCell(cell6);
                tablaFiscales.AddCell(cell7);
                tablaFiscales.AddCell(cell8);
                tablaFiscales.AddCell(cell9);

                tablaFiscalesPrincipal.AddCell(tablaFiscales);

                document.Add(tablaFiscalesPrincipal);
                ///*TABLA INFO FISCAL FIN*/

                document.Add(new Paragraph(5, "\u00a0"));

                /*PRODUCTOS SERVICIOS*/
                PdfPTable tablaTituloProdSer = new PdfPTable(1);
                tablaTituloProdSer.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaTituloProdSer.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(219, 249, 178);
                tablaTituloProdSer.HorizontalAlignment = 1;
                tablaTituloProdSer.WidthPercentage = 95;

                PdfPTable tablaProdSer = new PdfPTable(1);
                tablaProdSer.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell prodSer = new PdfPCell(new Phrase("PRODUCTOS / SERVICIOS", FontFactory.GetFont("Times New Roman", 9, iTextSharp.text.Font.BOLDITALIC, new BaseColor(0, 0, 0))));
                prodSer.Border = 0;

                tablaProdSer.AddCell(prodSer);

                tablaTituloProdSer.AddCell(tablaProdSer);

                document.Add(tablaTituloProdSer);

                //TABLA CONTENIDO TITULOS DE LA FACTURA
                PdfPTable tablaDatosGen = new PdfPTable(1);
                tablaDatosGen.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaDatosGen.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                tablaDatosGen.WidthPercentage = 95;

                ///*AGREGAMOS LA TABLA PARA MOSTRAR LOS CONCEPTOS*/
                PdfPTable tablaConceptos = new PdfPTable(11);
                tablaConceptos.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaConceptos.WidthPercentage = 95;

                /*CREAMOS LAS CELDAS PARA MOSTRAR LA INFORMACIÓN DE LOS CONCEPTOS*/
                PdfPCell cellClavSer = new PdfPCell(new Phrase("CVESERV", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                //PdfPCell cellNumID = new PdfPCell(new Phrase("NO.ID", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellCant = new PdfPCell(new Phrase("CANT", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellClaveUnidad = new PdfPCell(new Phrase("CVE.UNI", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellUnidad = new PdfPCell(new Phrase("UNIDAD", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellDes = new PdfPCell(new Phrase("DESCRIPCIÓN.", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellValUni = new PdfPCell(new Phrase("VALOR UNI", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellImporte = new PdfPCell(new Phrase("IMPORTE", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellBaseIva = new PdfPCell(new Phrase("BASE IVA", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                //PdfPCell cellImpto = new PdfPCell(new Phrase("IMPTO", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellFactor = new PdfPCell(new Phrase("FACTOR", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellTasa = new PdfPCell(new Phrase("TASA", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));
                PdfPCell cellImptIva = new PdfPCell(new Phrase("IMPT.IVA", FontFactory.GetFont("Arial", 6, Font.NORMAL, BaseColor.BLACK)));

                /*INDICAMOS LAS COLUMNAS QUE MANEJARA CADA CELDA*/
                cellClavSer.Colspan = 1;
                //cellNumID.Colspan = 1;
                cellCant.Colspan = 1;
                cellClaveUnidad.Colspan = 1;
                cellUnidad.Colspan = 1;
                cellDes.Colspan = 1;
                cellValUni.Colspan = 1;
                cellImporte.Colspan = 1;
                cellBaseIva.Colspan = 1;
                //cellImpto.Colspan = 1;
                cellFactor.Colspan = 1;
                cellTasa.Colspan = 1;
                cellImptIva.Colspan = 1;

                /*CENTRAR LOS TEXTOS*/
                cellClavSer.HorizontalAlignment = 1;
                //cellNumID.HorizontalAlignment = 1;
                cellCant.HorizontalAlignment = 1;
                cellClaveUnidad.HorizontalAlignment = 1;
                cellUnidad.HorizontalAlignment = 1;
                cellDes.HorizontalAlignment = 1;
                cellValUni.HorizontalAlignment = 1;
                cellImporte.HorizontalAlignment = 1;
                cellBaseIva.HorizontalAlignment = 1;
                //cellImpto.HorizontalAlignment = 1;
                cellFactor.HorizontalAlignment = 1;
                cellTasa.HorizontalAlignment = 1;
                cellImptIva.HorizontalAlignment = 1;

                /*AGREGAR COLOR DE FONDO*/
                cellClavSer.BackgroundColor = (new BaseColor(223, 223, 223));
                //cellNumID.BackgroundColor = (new BaseColor(223, 223, 223));
                cellCant.BackgroundColor = (new BaseColor(223, 223, 223));
                cellClaveUnidad.BackgroundColor = (new BaseColor(223, 223, 223));
                cellUnidad.BackgroundColor = (new BaseColor(223, 223, 223));
                cellDes.BackgroundColor = (new BaseColor(223, 223, 223));
                cellValUni.BackgroundColor = (new BaseColor(223, 223, 223));
                cellImporte.BackgroundColor = (new BaseColor(223, 223, 223));
                cellBaseIva.BackgroundColor = (new BaseColor(223, 223, 223));
                //cellImpto.BackgroundColor = (new BaseColor(223, 223, 223));
                cellFactor.BackgroundColor = (new BaseColor(223, 223, 223));
                cellTasa.BackgroundColor = (new BaseColor(223, 223, 223));
                cellImptIva.BackgroundColor = (new BaseColor(223, 223, 223));

                /*ELIMINAR LOS BORDESDES DE CADA CELDA*/

                cellClavSer.Border = 0;
                //cellNumID.Border = 0;
                cellCant.Border = 0;
                cellClaveUnidad.Border = 0;
                cellUnidad.Border = 0;
                cellDes.Border = 0;
                cellValUni.Border = 0;
                cellImporte.Border = 0;
                cellBaseIva.Border = 0;
                //cellImpto.Border = 0;
                cellFactor.Border = 0;
                cellTasa.Border = 0;
                cellImptIva.Border = 0;

                /*AGREGAMOS A LA TABLA tablaDescFactura LOS DATOS DE LAS CELDAS*/
                tablaConceptos.AddCell(cellClavSer);
                //tablaConceptos.AddCell(cellNumID);
                tablaConceptos.AddCell(cellCant);
                tablaConceptos.AddCell(cellClaveUnidad);
                tablaConceptos.AddCell(cellUnidad);
                tablaConceptos.AddCell(cellDes);
                tablaConceptos.AddCell(cellValUni);
                tablaConceptos.AddCell(cellImporte);
                tablaConceptos.AddCell(cellBaseIva);
                //tablaConceptos.AddCell(cellImpto);
                tablaConceptos.AddCell(cellFactor);
                tablaConceptos.AddCell(cellTasa);
                tablaConceptos.AddCell(cellImptIva);

                ///*AGREGAMR A LA TABLA tablaDescFacturaGen LA TABLA tablaDescFactura*/
                tablaDatosGen.AddCell(tablaConceptos);
                document.Add(tablaDatosGen);

                ////TABLA CONTENIDO DATOS DE LA FACTURA

                PdfPTable tablaDatosColumna = new PdfPTable(1);
                tablaDatosColumna.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaDatosColumna.WidthPercentage = 95;
                //tablaDatosGen.DefaultCell.BorderColor = (new BaseColor(46, 100, 254));

                //DATOS DE CELDA DE TITULOS (CREAMOS UNA TABLA tablaDescFactura CON UNA SOLA COLUMNA)
                PdfPTable tablaDatosProd = new PdfPTable(11);
                tablaDatosProd.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaDatosProd.WidthPercentage = 95;

                for (int i = 0; i < pagosDTO.LstConcepto.Count; i++)
                {

                    impuestoDTO = new ImpuestosDTO();

                    impuestoDTO.Importe = pagosDTO.LstConcepto[i].ImporteTotal;  //MASS
                    impuestoDTO.CodigoImpuesto = pagosDTO.LstConcepto[i].Impuesto;
                    impuestoDTO.TasaOCuota = pagosDTO.LstConcepto[i].TasaOCuota;
                    impuestoDTO.DescripcionTipoFactor = pagosDTO.LstConcepto[i].Factor;
                    impuestoDTO.DescripcionImpuesto = pagosDTO.LstConcepto[i].DescImpuesto;

                    if (!lstImpuestos.Exists(x => x.TasaOCuota == impuestoDTO.TasaOCuota))
                    {
                        lstImpuestos.Add(impuestoDTO);
                    }
                    else
                    {
                        int index = lstImpuestos.FindIndex(element => element.TasaOCuota == impuestoDTO.TasaOCuota);

                        lstImpuestos[index].Importe += impuestoDTO.Importe;
                    }

                    totalIVA += pagosDTO.LstConcepto[i].ImporteTotal; //MASS

                    PdfPCell clavSer1 = new PdfPCell(new Phrase(pagosDTO.LstConcepto[i].CodigoProdSer, FontFactory.GetFont("Times New Roman", 6, BaseColor.BLACK))); //MASS
                    //PdfPCell numId1 = new PdfPCell(new Phrase(pagosDTO.LstConcepto[i].Cantidad.ToString(), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell cant1 = new PdfPCell(new Phrase(pagosDTO.LstConcepto[i].Cantidad.ToString(), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell claveUni1 = new PdfPCell(new Phrase(pagosDTO.LstConcepto[i].ClaveUnidad, FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell unidad1 = new PdfPCell(new Phrase(pagosDTO.LstConcepto[i].Unidad, FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell desc1 = new PdfPCell(new Phrase(pagosDTO.LstConcepto[i].DescProdSer, FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell valUni1 = new PdfPCell(new Phrase("$" + string.Format("{0:###,###,###,###.##}", Math.Round(pagosDTO.LstConcepto[i].BaseTraslado, 2)), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell import1 = new PdfPCell(new Phrase("$" + string.Format("{0:###,###,###,###.##}", Math.Round(pagosDTO.LstConcepto[i].BaseTraslado, 2)), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell baseIva1 = new PdfPCell(new Phrase("$" + string.Format("{0:###,###,###,###.##}", Math.Round(pagosDTO.LstConcepto[i].BaseTraslado, 2)), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    //PdfPCell impto1 = new PdfPCell(new Phrase("0.00", FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell factor1 = new PdfPCell(new Phrase(pagosDTO.LstConcepto[i].Factor, FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell tasa1 = new PdfPCell(new Phrase(string.Format("{0:###,###.##}", Math.Round(pagosDTO.LstConcepto[i].TasaOCuota, 2)), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK)));
                    PdfPCell imptIva1 = new PdfPCell(new Phrase("$" + string.Format("{0:###,###,###,###.##}", Math.Round(pagosDTO.LstConcepto[i].ImporteTotal, 2)), FontFactory.GetFont("Times New Roman", 7, BaseColor.BLACK))); //MASS

                    //string.Format("{ 0:###,###.##}", pagosDTO.LstConcepto[i].TasaOCuota).ToString();
                    //pagosDTO.LstConcepto[i].TasaOCuota.ToString()

                    clavSer1.Colspan = 1;
                    //numId1.Colspan = 1;
                    cant1.Colspan = 1;
                    claveUni1.Colspan = 1;
                    unidad1.Colspan = 1;
                    desc1.Colspan = 1;
                    valUni1.Colspan = 1;
                    import1.Colspan = 1;
                    baseIva1.Colspan = 1;
                    //impto1.Colspan = 1;
                    factor1.Colspan = 1;
                    tasa1.Colspan = 1;
                    imptIva1.Colspan = 1;

                    /*ALINEMOS EL TEXTO DE FORMA CENTRADO*/
                    clavSer1.HorizontalAlignment = 1;
                    //numId1.HorizontalAlignment = 1;
                    cant1.HorizontalAlignment = 1;
                    claveUni1.HorizontalAlignment = 1;
                    unidad1.HorizontalAlignment = 1;
                    desc1.HorizontalAlignment = 1;
                    valUni1.HorizontalAlignment = 1;
                    import1.HorizontalAlignment = 1;
                    baseIva1.HorizontalAlignment = 1;
                    //impto1.HorizontalAlignment = 1;
                    factor1.HorizontalAlignment = 1;
                    tasa1.HorizontalAlignment = 1;
                    imptIva1.HorizontalAlignment = 1;

                    /*ELIMINAMOS LOS BORDESDES DE CADA CELDA*/
                    clavSer1.BorderWidthLeft = 0;
                    clavSer1.BorderWidthRight = 0;
                    clavSer1.BorderWidthBottom = 0;
                    clavSer1.BorderWidthTop = 0;

                    //numId1.BorderWidthLeft = 1;
                    //numId1.BorderWidthRight = 1;
                    //numId1.BorderWidthBottom = 1;
                    //numId1.BorderWidthTop = 0;

                    cant1.BorderWidthLeft = 0;
                    cant1.BorderWidthRight = 0;
                    cant1.BorderWidthBottom = 0;
                    cant1.BorderWidthTop = 0;

                    claveUni1.BorderWidthLeft = 0;
                    claveUni1.BorderWidthRight = 0;
                    claveUni1.BorderWidthBottom = 0;
                    claveUni1.BorderWidthTop = 0;

                    unidad1.BorderWidthLeft = 0;
                    unidad1.BorderWidthRight = 0;
                    unidad1.BorderWidthBottom = 0;
                    unidad1.BorderWidthTop = 0;

                    desc1.BorderWidthLeft = 0;
                    desc1.BorderWidthRight = 0;
                    desc1.BorderWidthBottom = 0;
                    desc1.BorderWidthTop = 0;

                    valUni1.BorderWidthLeft = 0;
                    valUni1.BorderWidthRight = 0;
                    valUni1.BorderWidthBottom = 0;
                    valUni1.BorderWidthTop = 0;

                    import1.BorderWidthLeft = 0;
                    import1.BorderWidthRight = 0;
                    import1.BorderWidthBottom = 0;
                    import1.BorderWidthTop = 0;

                    baseIva1.BorderWidthLeft = 0;
                    baseIva1.BorderWidthRight = 0;
                    baseIva1.BorderWidthBottom = 0;
                    baseIva1.BorderWidthTop = 0;

                    //impto1.BorderWidthLeft = 1;
                    //impto1.BorderWidthRight = 1;
                    //impto1.BorderWidthBottom = 1;
                    //impto1.BorderWidthTop = 0;

                    factor1.BorderWidthLeft = 0;
                    factor1.BorderWidthRight = 0;
                    factor1.BorderWidthBottom = 0;
                    factor1.BorderWidthTop = 0;

                    tasa1.BorderWidthLeft = 0;
                    tasa1.BorderWidthRight = 0;
                    tasa1.BorderWidthBottom = 0;
                    tasa1.BorderWidthTop = 0;

                    imptIva1.BorderWidthLeft = 0;
                    imptIva1.BorderWidthRight = 0;
                    imptIva1.BorderWidthBottom = 0;
                    imptIva1.BorderWidthTop = 0;

                    /*AGREGAMOS A LA TABLA tablaDatosProd LOS DATOS DE LAS CELDAS*/
                    tablaDatosProd.AddCell(clavSer1);
                    //tablaDatosProd.AddCell(numId1);
                    tablaDatosProd.AddCell(cant1);
                    tablaDatosProd.AddCell(claveUni1);
                    tablaDatosProd.AddCell(unidad1);
                    tablaDatosProd.AddCell(desc1);
                    tablaDatosProd.AddCell(valUni1);
                    tablaDatosProd.AddCell(import1);
                    tablaDatosProd.AddCell(baseIva1);
                    //tablaDatosProd.AddCell(impto1);
                    tablaDatosProd.AddCell(factor1);
                    tablaDatosProd.AddCell(tasa1);
                    tablaDatosProd.AddCell(imptIva1);
                }

                /*AGREGAMOS A LA TABLA tablaDatosColumna LA TABLA tablaDatosProd*/
                tablaDatosColumna.AddCell(tablaDatosProd);
                document.Add(tablaDatosColumna);

                document.Add(new Paragraph(20, "\u00a0"));

                /*CREAMOS UNA TABLA DE TOTALES, DONDE SE MOSTRARAN TODOS LOS DATOS DE LOS TOTALES Y EL IMPORTE TOTAL CON LETRA*/
                PdfPTable tablaTotalesBorde = new PdfPTable(1);
                tablaTotalesBorde.DefaultCell.BorderWidthTop = 1;
                tablaTotalesBorde.DefaultCell.BorderWidthBottom = 1;
                //tablaTotalesBorde.DefaultCell.BorderWidthRight = 1;
                //tablaTotalesBorde.DefaultCell.BorderWidthLeft = 1;
                tablaTotalesBorde.DefaultCell.BorderColorTop = new BaseColor(0, 0, 0);
                tablaTotalesBorde.DefaultCell.BorderColorBottom = new BaseColor(0, 0, 0);
                //tablaTotalesBorde.DefaultCell.BorderColorRight = new BaseColor(0, 0, 0);
                //tablaTotalesBorde.DefaultCell.BorderColorLeft = new BaseColor(0, 0, 0);
                tablaTotalesBorde.WidthPercentage = 95;

                /*TABLA total QUE ALMACENARA LOS DATOS TOTALES*/
                PdfPTable tablaTotales = new PdfPTable(2);
                tablaTotales.DefaultCell.Border = 0;
                tablaTotales.WidthPercentage = 95;
                tablaTotales.SetTotalWidth(new float[] { 25f, 15f });

                PdfPTable tablaTotalLetra = new PdfPTable(1);
                tablaTotalLetra.DefaultCell.Border = 0;

                PdfPTable tablaTotalLetraIzq = new PdfPTable(1);
                tablaTotalLetraIzq.DefaultCell.Border = 0;

                /*MANDAMOS A PINTAR LOS DATOS EN SUS RESPECTIVAS CELDAS*/
                ToCFDI toCFDI = new ToCFDI();
                PdfPCell importeLetra = new PdfPCell(new Phrase("Importe con Letra: " + toCFDI.ConvertirNumLetras(pagosDTO.LstConcepto[0].Importe.ToString(), true, pagosDTO.CodigoM), FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));

                //ALINEAMOS EL TEXTO DE LAS CELDAS AL LADO IZQUIERDO;
                importeLetra.HorizontalAlignment = 0;
                importeLetra.Border = 0;

                /*AGREGAMOS LA TABLA total LOS DATOS DE LA CELDA totalLetra*/
                tablaTotalLetraIzq.AddCell(importeLetra);
                tablaTotalLetra.AddCell(tablaTotalLetraIzq);
                tablaTotales.AddCell(tablaTotalLetra);

                /*TABLA General QUE ALMACENARA LOS DATOS PERTENECIENTES A LA CELDA*/
                PdfPTable TablaTotal = new PdfPTable(2);
                TablaTotal.DefaultCell.Border = Rectangle.NO_BORDER;
                TablaTotal.WidthPercentage = 95;

                //DATOS DE CELDA2  (CREAMOS UNA TABALA CON UNA SOLA COLUMNA)
                PdfPTable tablaTotalIzq = new PdfPTable(1);
                tablaTotalIzq.HorizontalAlignment = 2;
                tablaTotalIzq.WidthPercentage = 70;

                /*MANDAMOS A PINTAR LOS DATOS EN SUS RESPECTIVAS CELDAS*/
                PdfPCell SubTotales = new PdfPCell(new Phrase("SubTotal", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell Descuento = new PdfPCell(new Phrase("Descuento", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell IvaTraslado = new PdfPCell(new Phrase("IVA(16.00 % *)", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell Total = new PdfPCell(new Phrase("Total", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell Moneda = new PdfPCell(new Phrase("Moneda", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell TipoCambio = new PdfPCell(new Phrase("Tipo Cambio", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));

                //ALINEAMOS EL TEXTO DE LAS CELDAS AL LADO IZQUIERDO;
                /*ELIMINAMOS LOS BORDES DE DE CADA CELDA*/
                SubTotales.Right = 0;
                Descuento.Right = 0;
                IvaTraslado.Right = 0;
                Total.Right = 0;
                Moneda.Right = 0;
                TipoCambio.Right = 0;

                //ALINEAMOS EL TEXTO DE LAS CELDAS AL LADO IZQUIERDO;
                /*ELIMINAMOS LOS BORDESDES DE CADA CELDA*/
                SubTotales.Border = 0;
                Descuento.Border = 0;
                IvaTraslado.Border = 0;
                Total.Border = 0;
                Moneda.Border = 0;
                TipoCambio.Border = 0;

                /*ELIMINAMOS LOS BORDESDES DE CADA CELDA*/
                tablaTotalIzq.AddCell(SubTotales);
                tablaTotalIzq.AddCell(Descuento);
                tablaTotalIzq.AddCell(IvaTraslado);
                tablaTotalIzq.AddCell(Total);
                tablaTotalIzq.AddCell(Moneda);
                tablaTotalIzq.AddCell(TipoCambio);

                TablaTotal.AddCell(tablaTotalIzq);

                //DATOS DE CELDA2  (CREAMOS UNA TABALA CON UNA SOLA COLUMNA)
                PdfPTable tablaTotalDer = new PdfPTable(1);
                tablaTotalDer.HorizontalAlignment = 2;
                tablaTotalDer.WidthPercentage = 30;

                /*MANDAMOS A PINTAR LOS DATOS EN SUS RESPECTIVAS CELDAS*/
                PdfPCell SubTotalesD = new PdfPCell(new Phrase(pagosDTO.SubTotalPago.ToString("C", new CultureInfo("es-MX")), FontFactory.GetFont("Arial", 7)));
                PdfPCell DescuentoD = new PdfPCell(new Phrase(pagosDTO.DescuentoPago.ToString("C", new CultureInfo("es-MX")), FontFactory.GetFont("Arial", 7)));
                PdfPCell IvaD = new PdfPCell(new Phrase(totalIVA.ToString("C", new CultureInfo("es-MX")), FontFactory.GetFont("Arial", 7)));
                PdfPCell TotalD = new PdfPCell(new Phrase(pagosDTO.TotalPago.ToString("C", new CultureInfo("es-MX")), FontFactory.GetFont("Arial", 7)));
                PdfPCell MonedaD = new PdfPCell(new Phrase(pagosDTO.CodigoM + " " + pagosDTO.DescM, FontFactory.GetFont("Arial", 7)));
                PdfPCell TipoCambioD = new PdfPCell(new Phrase(pagosDTO.TipoCambio.ToString(), FontFactory.GetFont("Arial", 7)));

                /*ALINEAMOS EL TEXTO DE LAS CELDAS AL LADO DERECHO*/
                SubTotalesD.HorizontalAlignment = 2;
                DescuentoD.HorizontalAlignment = 2;
                IvaD.HorizontalAlignment = 2;
                TotalD.HorizontalAlignment = 2;
                MonedaD.HorizontalAlignment = 2;
                TipoCambioD.HorizontalAlignment = 2;

                /*ELIMINAMOS LOS BORDESDES DE CADA CELDA*/
                SubTotalesD.Border = 0;
                DescuentoD.Border = 0;
                IvaD.Border = 0;
                TotalD.Border = 0;
                MonedaD.Border = 0;
                TipoCambioD.Border = 0;

                /*AGREGAMOS LAS CELDAS A LA TABLA*/
                tablaTotalDer.AddCell(SubTotalesD);
                tablaTotalDer.AddCell(DescuentoD);
                tablaTotalDer.AddCell(IvaD);
                tablaTotalDer.AddCell(TotalD);
                tablaTotalDer.AddCell(MonedaD);
                tablaTotalDer.AddCell(TipoCambioD);

                TablaTotal.AddCell(tablaTotalDer);
                /*AGREGAMOS A LA TABLA Principal LOS DATOS DE LAS CELDAS*/
                tablaTotales.AddCell(TablaTotal);
                tablaTotalesBorde.AddCell(tablaTotales);

                document.Add(tablaTotalesBorde);

                document.Add(new Paragraph(5, "\u00a0"));

                //LEYENDA
                //PdfPTable tablaLeyenda = new PdfPTable(1);
                //tablaLeyenda.DefaultCell.Border = Rectangle.NO_BORDER;
                //tablaLeyenda.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(194, 194, 193);
                //tablaLeyenda.HorizontalAlignment = 1;
                //tablaLeyenda.WidthPercentage = 95;

                //PdfPTable tablaTexto = new PdfPTable(1);
                //tablaTexto.DefaultCell.BorderWidthLeft = 1;
                //tablaTexto.DefaultCell.BorderColorLeft = new BaseColor(0, 0, 0);
                //tablaTexto.DefaultCell.BorderWidthRight = 1;
                //tablaTexto.DefaultCell.BorderColorRight = new BaseColor(0, 0, 0);

                //PdfPCell texto = new PdfPCell(new Phrase("*Tratándose de Transportación Aérea Internacional y Franja Fronteriza, el IVA se calculará al 25% de la tasa general del 16%, de conformidad con el Artículo 16 de la ley del IVA.", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.ITALIC, new BaseColor(0, 0, 0))));
                //texto.Border = 0;
                //texto.HorizontalAlignment = 1;

                //tablaTexto.AddCell(texto);

                //tablaLeyenda.AddCell(tablaTexto);

                //document.Add(tablaLeyenda);

                //document.Add(new Paragraph(5, "\u00a0"));

                //DATOS TIMBRADO CFDI (QR)
                PdfPTable tablaTituloDatosTimbrado = new PdfPTable(1);
                tablaTituloDatosTimbrado.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaTituloDatosTimbrado.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(219, 249, 178);
                tablaTituloDatosTimbrado.HorizontalAlignment = 1;
                tablaTituloDatosTimbrado.WidthPercentage = 95;

                PdfPTable TablaTimbrado = new PdfPTable(1);
                TablaTimbrado.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell timbrado = new PdfPCell(new Phrase("DATOS TIMBRADO CFDI", FontFactory.GetFont("Times New Roman", 9, iTextSharp.text.Font.BOLDITALIC, new BaseColor(0, 0, 0))));
                timbrado.Border = 0;

                TablaTimbrado.AddCell(timbrado);

                tablaTituloDatosTimbrado.AddCell(TablaTimbrado);

                document.Add(tablaTituloDatosTimbrado);

                PdfPTable contQR = new PdfPTable(1);
                contQR.WidthPercentage = 95;
                contQR.DefaultCell.BorderColor = new iTextSharp.text.BaseColor(229, 231, 233);

                PdfPTable contenedorQr = new PdfPTable(2);
                contenedorQr.WidthPercentage = 95;
                contenedorQr.DefaultCell.Border = Rectangle.NO_BORDER;

                float[] widths = new float[] { 8, 30 };
                contenedorQr.SetWidths(widths);

                PdfPTable imagenQR = new PdfPTable(1);

                BarcodeQRCode qrcodes = new BarcodeQRCode(GenerarQR(pagosDTO), 1, 1, null);
                iTextSharp.text.Image imgCB = qrcodes.GetImage();
                //imgCB.SetAbsolutePosition(8, 89);
                imgCB.ScaleAbsolute(110f, 120f);

                PdfPCell imgqr = new PdfPCell(imgCB);
                imgqr.Border = 0;
                imagenQR.AddCell(imgqr);
                contenedorQr.AddCell(imagenQR);

                PdfPTable QR = new PdfPTable(1);
                QR.DefaultCell.Border = Rectangle.NO_BORDER;

                PdfPCell cadena = new PdfPCell(new Phrase("Cadena Original:", FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                PdfPCell noSerie = new PdfPCell(new Phrase(pagosDTO.CadenaOriginal, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell serie = new PdfPCell(new Phrase("No. de Serie del Certificado del SAT:", FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                PdfPCell certSat = new PdfPCell(new Phrase(pagosDTO.NoCertificadoSAT, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell rfcNo = new PdfPCell(new Phrase("RFC proveedor de certificado:", FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                PdfPCell rfcCert = new PdfPCell(new Phrase("SAD110722MQA", FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell sat = new PdfPCell(new Phrase("Sello Digital del SAT:", FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                PdfPCell selloSat = new PdfPCell(new Phrase(pagosDTO.SelloSAT, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell cdfi = new PdfPCell(new Phrase("Sello Digital del Emisor:", FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                PdfPCell noCertSat = new PdfPCell(new Phrase(pagosDTO.SelloDigital, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));
                PdfPCell csd = new PdfPCell(new Phrase("No. de Serie del Certificado del Emisor:", FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                PdfPCell certEmi = new PdfPCell(new Phrase(pagosDTO.NoCertificado, FontFactory.GetFont("Times New Roman", 7, iTextSharp.text.Font.NORMAL)));

                /*ELIMINAMOS Y ASIGNAMOS LOS BORDES DE CADA CELDA*/
                cadena.Border = 0;
                noSerie.Border = 0;
                serie.Border = 0;
                certSat.Border = 0;
                rfcNo.Border = 0;
                rfcCert.Border = 0;
                sat.Border = 0;
                selloSat.Border = 0;
                cdfi.Border = 0;
                noCertSat.Border = 0;
                csd.Border = 0;
                certEmi.Border = 0;

                QR.AddCell(cadena);
                QR.AddCell(noSerie);
                QR.AddCell(serie);
                QR.AddCell(certSat);
                QR.AddCell(rfcNo);
                QR.AddCell(rfcCert);
                QR.AddCell(sat);
                QR.AddCell(selloSat);
                QR.AddCell(cdfi);
                QR.AddCell(noCertSat);
                QR.AddCell(csd);
                QR.AddCell(certEmi);

                contenedorQr.AddCell(QR);
                contQR.AddCell(contenedorQr);
                document.Add(contQR);

                document.Add(new Paragraph(5, "\u00a0"));

                //LEYENDA2
                PdfPTable tablaTituloDocumentoLeyenda = new PdfPTable(1);
                tablaTituloDocumentoLeyenda.DefaultCell.Border = Rectangle.NO_BORDER;
                tablaTituloDocumentoLeyenda.DefaultCell.BackgroundColor = new iTextSharp.text.BaseColor(194, 194, 193);
                tablaTituloDocumentoLeyenda.HorizontalAlignment = 1;
                tablaTituloDocumentoLeyenda.WidthPercentage = 95;

                PdfPTable tablaTextoCFDI = new PdfPTable(1);
                tablaTextoCFDI.DefaultCell.BorderWidthLeft = 1;
                tablaTextoCFDI.DefaultCell.BorderColorLeft = new BaseColor(0, 0, 0);
                tablaTextoCFDI.DefaultCell.BorderWidthRight = 1;
                tablaTextoCFDI.DefaultCell.BorderColorRight = new BaseColor(0, 0, 0);

                PdfPCell textoCfdi = new PdfPCell(new Phrase("Este documento es una representación impresa de un CFDI", FontFactory.GetFont("Times New Roman", 9, iTextSharp.text.Font.BOLD, new BaseColor(0, 0, 0))));
                textoCfdi.Border = 0;
                textoCfdi.HorizontalAlignment = 1;

                tablaTextoCFDI.AddCell(textoCfdi);

                tablaTituloDocumentoLeyenda.AddCell(tablaTextoCFDI);

                document.Add(tablaTituloDocumentoLeyenda);

                document.Close();

                System.IO.File.WriteAllBytes(pagosDTO.RutaArchivoPDF, msPDF.ToArray());
                pagosDTO.ArchivoPDF = msPDF.ToArray();

                return msPDF;

            }
            catch (Exception ex)
            {
                throw ex;

            //C:\Users\miguel.santes\Desktop\PaquetesVivaAerobus\VBFactPaquetes.BLLTests\assets\Images\logo.png
            }

        }

        /*INICIA METODO PARA GENERAR QR*/
        private static String GenerarQR(Pago pago)
        {
            String URL = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx";
            StringBuilder codigoBi = new StringBuilder();

            codigoBi.Append(URL);
            codigoBi.Append("?id=" + pago.UUID);
            codigoBi.Append("&re=" + pago.RFCEmisor);
            codigoBi.Append("&rr=" + pago.LstDatosFiscales[0].RFC); // RFC Receptor
            codigoBi.Append("&tt=" + pago.TotalPago);
            string sello = pago.SelloComprobante;
            string ult8carSello = sello.Substring(sello.Length - 8, 8);
            codigoBi.Append("&fe=" + ult8carSello);

            return codigoBi.ToString();
        }

        public class EventoTitulos : PdfPageEventHelper
        {

            protected PdfTemplate total;
            protected BaseFont helv;
            private bool settingFont = false;

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {// Se crea el template
                total = writer.DirectContent.CreateTemplate(100, 100);
                total.BoundingBox = new Rectangle(-20, -20, 100, 100);
                helv = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);
            }
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                //string sTitulo = "", sLinea = "";
                //float textSize = 6;
                //float textBase = 600; // Este pone la informacion en la parte superior

                //PdfContentByte cb = writer.DirectContent;
                //cb.SaveState();
                //cb.BeginText();
                //cb.SetFontAndSize(helv, 6);

                //sTitulo = " ";
                //cb.SetTextMatrix(document.Left, textBase);
                //cb.ShowText(sTitulo);
                //cb.AddTemplate(total, document.Left + textSize, textBase);
                //textBase = textBase - 5;

                //sLinea = " ";
                //cb.SetTextMatrix(document.Left, textBase - 5);
                //cb.ShowText(sLinea);
                //cb.EndText(); //Este es necesario para cerrar el BeginText, pero solo se pone en el ultimo texto a anexar, sino marcara error
                //cb.AddTemplate(total, document.Left + textSize, textBase);

                //cb.RestoreState();
            }
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                //PdfContentByte cb = writer.DirectContent;
                //cb.SaveState();
                //cb.BeginText();
                //cb.SetFontAndSize(helv, 6);
                //string sPiePagina = "";

                //float textSize = 10;
                //float textBase = 30; // Este lo pone la informacion en la parte inferior

                //sPiePagina = "         " + "ESTE DOCUMENTO ES UNA REPRESENTACIÓN IMPRESA DE UN CFDI" + "                                                                                                                                                                 " + "Serie:    " + "           " + "Folio:    ";
                //cb.SetTextMatrix(document.Left, textBase);
                //cb.ShowText(sPiePagina);
                //cb.AddTemplate(total, document.Left + textSize, textBase);

                //sPiePagina = "         " + "El registro de este documento puede ser verificado en la página de internet del SAT" + "                                                                                                                                                                                                    " + "Página: " + writer.PageNumber;
                //cb.SetTextMatrix(document.Left, textBase - 7);
                //cb.ShowText(sPiePagina);
                //cb.EndText();
                //cb.AddTemplate(total, document.Left + textSize, textBase);

                //cb.RestoreState();
            }
        }
    }
}

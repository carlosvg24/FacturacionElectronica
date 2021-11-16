using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBFactPaquetes.DAO;
using VBFactPaquetes.DAO.Facturacion;
using VBFactPaquetes.Model.Facturacion;

namespace VBFactPaquetes.BLL.Facturacion
{
    public class BLLFacturaGlobal
    {
        DAOConfiguracion daoConf = new DAOConfiguracion();

        public List<FacturaGlobal> ConsultaConceptosGlobal(int anio, int mes, String CodigoTC, String CodigoMoneda)
        {
            DAOFacturaGlobal daoFacturaGlobal = new DAOFacturaGlobal();
            DatosGeneralesDTO datosGral = new DatosGeneralesDTO();
            List<ConceptosDTO> lstConceptoDTO = new List<ConceptosDTO>();
            List<FacturaGlobal> lstGlobal = new List<FacturaGlobal>();
            List<DatosGeneralesDTO> lstDatosGral = new List<DatosGeneralesDTO>();
            DataSet dsResultado = new DataSet();
            DataTable dtGlobal = new DataTable();
            FacturaGlobal globalDTO;

            try
            {
                dsResultado = daoFacturaGlobal.ConsultaConceptosGlobal(anio, mes, CodigoTC, CodigoMoneda);

                if (dsResultado.Tables.Count > 0)
                {
                    dtGlobal = dsResultado.Tables[0];

                    if (dtGlobal.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtGlobal.Rows.Count; i++)
                        {
                            lstConceptoDTO = new List<ConceptosDTO>();
                            globalDTO = new FacturaGlobal();

                            globalDTO.IdFactPaqGlobal = Convert.ToInt64(dtGlobal.Rows[i]["IdFactPaqGlobal"]);
                            globalDTO.CodigoTC = dtGlobal.Rows[i]["TipoComprobante"].ToString();
                            globalDTO.TotalFactura = Convert.ToDecimal(dtGlobal.Rows[i]["TotalFactura"]);
                            globalDTO.TotalIva = Convert.ToDecimal(dtGlobal.Rows[i]["TotalIVA"]);
                            globalDTO.SubTotal = Convert.ToDecimal(dtGlobal.Rows[i]["Subtotal"]);
                            globalDTO.TotalDescuento = Convert.ToDecimal(dtGlobal.Rows[i]["TotalDescuento"]);
                            globalDTO.Folio = dtGlobal.Rows[i]["Folio"].ToString();
                            globalDTO.Serie = dtGlobal.Rows[i]["Serie"].ToString();
                            globalDTO.TipoCambio = dtGlobal.Rows[i]["TipoCambio"].ToString();
                            globalDTO.CodigoMoneda = dtGlobal.Rows[i]["CodigoMoneda"].ToString();
                            globalDTO.DescMoneda = dtGlobal.Rows[i]["DescMoneda"].ToString();
                            globalDTO.CodigoMP = dtGlobal.Rows[i]["CodigoMP"].ToString();
                            globalDTO.DescMP = dtGlobal.Rows[i]["DescMP"].ToString();
                            globalDTO.CodigoFP = dtGlobal.Rows[i]["CodigoFP"].ToString();
                            globalDTO.DescFP = dtGlobal.Rows[i]["DescFP"].ToString();
                            globalDTO.LugarExp = dtGlobal.Rows[i]["LugarExpedicion"].ToString();
                            globalDTO.RFCEmisor = dtGlobal.Rows[i]["RFCEmisor"].ToString();
                            globalDTO.RazonSocialEmisor = dtGlobal.Rows[i]["RazonSocialEmisor"].ToString();
                            globalDTO.CodigoRF = dtGlobal.Rows[i]["CodigoRF"].ToString();
                            globalDTO.DescRF = dtGlobal.Rows[i]["DescRF"].ToString();
                            globalDTO.RFCReceptor = dtGlobal.Rows[i]["RFCReceptor"].ToString();
                            globalDTO.CodigoUsoCFDI = dtGlobal.Rows[i]["CodigoUsoCFDI"].ToString();
                            globalDTO.DescUsoCFDI = dtGlobal.Rows[i]["DescUsoCFDI"].ToString();
                            globalDTO.DescTC = dtGlobal.Rows[i]["DescTC"].ToString();

                            lstConceptoDTO = ObtieneConceptosGlobal(dsResultado.Tables[1]);
                            lstDatosGral.Add(ObtieneDatosGenerales(dsResultado.Tables[2]));

                            globalDTO.LstConcepto = lstConceptoDTO;
                            globalDTO.LstDatosGralDTO = lstDatosGral;

                            lstGlobal.Add(globalDTO);
                        }
                    }
                }

                return lstGlobal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*OBTENEMOS LOS DATOS GENERALES PARA LA FACTURA*/
        public DatosGeneralesDTO ObtieneDatosGenerales(DataTable dtDatosGral)
        {
            DatosGeneralesDTO datosGral = new DatosGeneralesDTO();

            try
            {
                for (int i = 0; i < dtDatosGral.Rows.Count; i++)
                {
                    if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "ArchivosGlobal" && dtDatosGral.Rows[i]["Nombre"].ToString() == "carpetaPDF")
                    {
                        datosGral.CarpetaPDF = dtDatosGral.Rows[i]["Valor"].ToString();
                    }
                    else if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "ArchivosGlobal" && dtDatosGral.Rows[i]["Nombre"].ToString() == "carpetaXML")
                    {
                        datosGral.CarpetaXML = dtDatosGral.Rows[i]["Valor"].ToString();
                    }
                    else if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "ArchivosGlobal" && dtDatosGral.Rows[i]["Nombre"].ToString() == "carpetaNoProcesados")
                    {
                        datosGral.CarpetaNoProcesados = dtDatosGral.Rows[i]["Valor"].ToString();
                    }
                    else if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "ArchivosGlobal" && dtDatosGral.Rows[i]["Nombre"].ToString() == "CarpetaArchivosCFDI")
                    {
                        datosGral.CarpetaArchivosCFDI = dtDatosGral.Rows[i]["Valor"].ToString();
                    }
                    else if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "ClienteSMTP" && dtDatosGral.Rows[i]["Nombre"].ToString() == "host")
                    {
                        datosGral.HostSMTP = dtDatosGral.Rows[i]["Valor"].ToString();
                    }
                    else if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "ClienteSMTP" && dtDatosGral.Rows[i]["Nombre"].ToString() == "puerto")
                    {
                        datosGral.PuertoSMTP = Convert.ToInt32(dtDatosGral.Rows[i]["Valor"]);
                    }
                    else if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "ClienteSMTP" && dtDatosGral.Rows[i]["Nombre"].ToString() == "usuario")
                    {
                        datosGral.UsuarioSMTP = dtDatosGral.Rows[i]["Valor"].ToString();
                    }
                    else if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "ClienteSMTP" && dtDatosGral.Rows[i]["Nombre"].ToString() == "contraseña")
                    {
                        datosGral.ContraseñaSMTP = dtDatosGral.Rows[i]["Valor"].ToString();
                    }
                    else if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "ClienteSMTP" && dtDatosGral.Rows[i]["Nombre"].ToString() == "headers")
                    {
                        datosGral.CategoriaSMTP = dtDatosGral.Rows[i]["Valor"].ToString();
                    }
                    else if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "ClienteSMTP" && dtDatosGral.Rows[i]["Nombre"].ToString() == "mailEmpresa")
                    {
                        datosGral.MailEmpresaSMTP = dtDatosGral.Rows[i]["Valor"].ToString();
                    }
                    else if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "ClienteSMTP" && dtDatosGral.Rows[i]["Nombre"].ToString() == "nombreResponsable")
                    {
                        datosGral.NombreResponsableSMTP = dtDatosGral.Rows[i]["Valor"].ToString();
                    }
                    else if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "ClienteSMTP" && dtDatosGral.Rows[i]["Nombre"].ToString() == "mailSoporte")
                    {
                        datosGral.MailSoporteSMTP = dtDatosGral.Rows[i]["Valor"].ToString();
                    }
                    else if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "Timbrado" && dtDatosGral.Rows[i]["Nombre"].ToString() == "apikey")
                    {
                        datosGral.ApikeyPAC = dtDatosGral.Rows[i]["Valor"].ToString();
                    }
                    else if (dtDatosGral.Rows[i]["TipoParametro"].ToString() == "Timbrado" && dtDatosGral.Rows[i]["Nombre"].ToString() == "VersionCFDI")
                    {
                        datosGral.VersionCFDI = dtDatosGral.Rows[i]["Valor"].ToString();
                    }
                }

                return datosGral;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ConceptosDTO> ObtieneConceptosGlobal(DataTable dtConceptos)
        {
            ConceptosDTO conceptosDTO;
            List<ConceptosDTO> lstConceptosDTO = new List<ConceptosDTO>();

            for (int i = 0; i < dtConceptos.Rows.Count; i++)
            {
                conceptosDTO = new ConceptosDTO();

                conceptosDTO.IdConceptos = Convert.ToInt64(dtConceptos.Rows[i]["IdFactPaqGlobalDet"]);
                conceptosDTO.BookingID = dtConceptos.Rows[i]["BookingID"].ToString();
                conceptosDTO.PNR = dtConceptos.Rows[i]["PNR"].ToString();
                conceptosDTO.IdFactPaqPagos = Convert.ToInt64(dtConceptos.Rows[i]["IDFactPaqPagos"]);
                conceptosDTO.Cantidad = Convert.ToInt16(dtConceptos.Rows[i]["Cantidad"]);
                conceptosDTO.ClaveUnidad = dtConceptos.Rows[i]["ClaveUnidad"].ToString();
                conceptosDTO.Unidad = dtConceptos.Rows[i]["Unidad"].ToString();
                conceptosDTO.PrecioUnitario = Convert.ToDecimal(dtConceptos.Rows[i]["PrecioUnitario"]);
                conceptosDTO.Descuento = Convert.ToDecimal(dtConceptos.Rows[i]["Descuento"]);
                conceptosDTO.NoIdentificacion = dtConceptos.Rows[i]["NoIdentificacion"].ToString();
                conceptosDTO.DescProdSer = dtConceptos.Rows[i]["DescProdSer"].ToString();
                conceptosDTO.DescripcionConcepto = dtConceptos.Rows[i]["Descripcion"].ToString();
                conceptosDTO.ImporteBase = Convert.ToDecimal(dtConceptos.Rows[i]["ImporteBase"]);
                conceptosDTO.ImporteIva = Convert.ToDecimal(dtConceptos.Rows[i]["ImporteIva"]);
                conceptosDTO.Impuesto = dtConceptos.Rows[i]["Impuesto"].ToString();
                conceptosDTO.DescImpuesto = dtConceptos.Rows[i]["DescImpuesto"].ToString();
                conceptosDTO.Factor = dtConceptos.Rows[i]["Factor"].ToString();
                conceptosDTO.TasaOCuota = Convert.ToDecimal(dtConceptos.Rows[i]["TasaOCuota"]);
                conceptosDTO.DescTipoImpuesto = dtConceptos.Rows[i]["DescTipoImpuesto"].ToString();
                //conceptosDTO.TipoConcepto = Convert.ToInt16(dtConceptos.Rows[i]["TipoConcepto"]);

                lstConceptosDTO.Add(conceptosDTO);
            }

            return lstConceptosDTO;
        }



        /* 2.- MÉTODO PARA GENERAR XMLREQUEST*/
        public String GeneraXMLRequest(FacturaGlobal globalDTO)
        {
            String rutaXML = String.Empty;

            try
            {
                //FileStream fs = System.IO.File.Create(Comun.PDF.FacturaGlobal.GuardaArchivosFactura(globalDTO, "XML"));
                //fs.Close();

                /*SE CONSTRUYE EL ARCHIVO XML REQUEST*/
                ConstruirXML(globalDTO, globalDTO.RutaArchivoXML);


                /*SE PASA EL XML REQUEST A UN STRING PARA SER RETORNADO AL PROCESO ANTERIOR */
                return System.IO.File.ReadAllText(globalDTO.RutaArchivoXML);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /* 3.- MÉTODO PARA CONSTRUIR EL XML*/
        public void ConstruirXML(FacturaGlobal globalDTO, String rutaXML)
        {
            try
            {
                string numCertificado = "00001000000301999105";
                //string certificado = "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";
                //string sellofake = "Ug31Sdl5YrRqgBEXoORAYR5w7KUjdFICBMix3nMFyi1KPtu8/zCfCMOEbVeROIia5Bq7bgzmwre0s5VosQ+etzU8yCWJDkbJJ0PDOBSDIplj4PrRxIO3bft5ejAOkOXO94+87x+Yvvr0UzoDtRuy5lgxYlAy60I4BXj1WzCekki31xt3IT1QhA6fPZ8YdgnhjynTE4bcaFo7CQAkcQmgNa40ufqd9vX9x85uJcqVrp7tHDsbd0DZH3du/W9aEQQdtiVQvBB/90aj+tFpTVVhmO7E4SOeHjVkB/jJ0PvuOVWO7+k18gTqt95ecXLmojaYz5SDoKUY+2CJH3rCpuGSZg==";

                Console.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");
                Console.WriteLine("  Generando XML Y4-I-MXN" + "----" + DateTime.Now);
                Console.WriteLine("''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''");

                List<ImpuestosDTO> lstImpuestos = new List<ImpuestosDTO>();
                ImpuestosDTO impuestoDTO;

                /* SE LLENAN LOS DATOS PARA FORMAR EL XML */
                StringBuilder xmlRequest = new StringBuilder();
                globalDTO.FechaEmision = DateTime.Now.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss");

                xmlRequest.Append("<RequestCFD version=\"3.3\">");
                //xmlRequest.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("<Comprobante ");
                //xmlRequest.Append("xmlns:aerolineas=\"" + "http://www.sat.gob.mx/aerolineas" + "\" ");
                //xmlRequest.Append("xmlns:xsi=\"" + "http://www.w3.org/2001/XMLSchema-instance" + "\" ");
                //xmlRequest.Append("xmlns:tfd=\"" + "http://www.sat.gob.mx/TimbreFiscalDigital" + "\" ");
                //xmlRequest.Append("xmlns:xs=\"" + "http://www.w3.org/2001/XMLSchema" + "\" ");
                //xmlRequest.Append("xmlns:cfdi=\"" + "http://www.sat.gob.mx/cfd/3" + "\" ");
                //xmlRequest.Append("xmlns:terceros=\"" + "http://www.sat.gob.mx/terceros" + "\" ");
                //xmlRequest.Append("Certificado=\"" + certificado + "\" ");
                //xmlRequest.Append("xsi:schemaLocation=\"" + "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd http://www.sat.gob.mx/terceros http://www.sat.gob.mx/sitio_internet/cfd/terceros/terceros11.xsd http://www.sat.gob.mx/aerolineas http://www.sat.gob.mx/sitio_internet/cfd/aerolineas/aerolineas.xsd" + "\" ");
                //xmlRequest.Append("Sello=\"" + sellofake + "\" ");
                xmlRequest.Append("Version=\"" + globalDTO.LstDatosGralDTO[0].VersionCFDI + "\" ");
                xmlRequest.Append("Serie=\"" + globalDTO.Serie + "\" ");
                xmlRequest.Append("Folio=\"" + globalDTO.Folio + "\" ");
                xmlRequest.Append("Fecha=\"" + globalDTO.FechaEmision + "\" ");
                xmlRequest.Append("FormaPago=\"" + globalDTO.CodigoFP + "\" ");
                xmlRequest.Append("NoCertificado=\"" + numCertificado + "\" ");
                xmlRequest.Append("SubTotal=\"" + globalDTO.SubTotal.ToString(new CultureInfo("es-MX")) + "\" ");
                xmlRequest.Append("Moneda=\"" + globalDTO.CodigoMoneda + "\" ");

                if (globalDTO.TipoCambio == "1.000000")
                {
                    xmlRequest.Append("TipoCambio=\"" + 1 + "\" ");
                }
                else
                {
                    xmlRequest.Append("TipoCambio=\"" + globalDTO.TipoCambio.ToString(new CultureInfo("es-MX")) + "\" ");
                }

                xmlRequest.Append("Total=\"" + globalDTO.TotalFactura.ToString(new CultureInfo("es-MX")) + "\" ");

                if (globalDTO.TotalDescuento > 0)
                {
                    xmlRequest.Append("Descuento=\"" + globalDTO.TotalDescuento.ToString(new CultureInfo("es-MX")) + "\" ");
                }

                xmlRequest.Append("TipoDeComprobante=\"" + globalDTO.CodigoTC + "\" ");
                xmlRequest.Append("MetodoPago=\"" + globalDTO.CodigoMP + "\" ");
                xmlRequest.Append("LugarExpedicion=\"" + globalDTO.LugarExp + "\">");

                /*EMISOR*/
                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append(" <Emisor ");
                xmlRequest.Append("Rfc=\"" + globalDTO.RFCEmisor + "\" ");
                xmlRequest.Append("RegimenFiscal=\"" + globalDTO.CodigoRF + "\" ");
                xmlRequest.Append("Nombre=\"" + globalDTO.RazonSocialEmisor + "\"/> ");
                xmlRequest.Append(Environment.NewLine);

                /*RECEPTOR*/
                xmlRequest.Append(" <Receptor ");
                xmlRequest.Append("Rfc=\"" + globalDTO.RFCReceptor + "\" ");

                xmlRequest.Append("UsoCFDI=\"" + globalDTO.CodigoUsoCFDI + "\"/> ");
                xmlRequest.Append(Environment.NewLine);

                /*CONCEPTO*/
                xmlRequest.Append(" <Conceptos>");
                xmlRequest.Append(Environment.NewLine);

                /*CICLO PARA RECORRER LA LISTA DE LOS DATOS PARA CONCEPTO.*/
                for (int i = 0; i < globalDTO.LstConcepto.Count; i++)
                {
                    if (globalDTO.LstConcepto[i].PrecioUnitario > 0)
                    {
                        xmlRequest.Append("     <Concepto ");
                        xmlRequest.Append("ClaveProdServ=\"" + globalDTO.LstConcepto[i].NoIdentificacion + "\" ");
                        xmlRequest.Append("NoIdentificacion=\"" + globalDTO.LstConcepto[i].NoIdentificacion + "\" ");
                        xmlRequest.Append("Cantidad=\"" + globalDTO.LstConcepto[i].Cantidad + "\" ");
                        xmlRequest.Append("ClaveUnidad=\"" + globalDTO.LstConcepto[i].ClaveUnidad + "\" ");
                        xmlRequest.Append("Unidad=\"" + globalDTO.LstConcepto[i].Unidad + "\" ");
                        xmlRequest.Append("Descripcion=\"" + globalDTO.LstConcepto[i].DescripcionConcepto + "\" ");

                        if (globalDTO.LstConcepto[i].Descuento > 0)
                        {
                            xmlRequest.Append("Descuento=\"" + globalDTO.LstConcepto[i].Descuento.ToString(new CultureInfo("es-MX")) + "\" ");
                        }

                        xmlRequest.Append("ValorUnitario=\"" + globalDTO.LstConcepto[i].PrecioUnitario.ToString(new CultureInfo("es-MX")) + "\" ");
                        xmlRequest.Append("Importe=\"" + globalDTO.LstConcepto[i].PrecioUnitario.ToString(new CultureInfo("es-MX")) + "\"> ");

                        if (globalDTO.LstConcepto[i].ImporteIva > 0)
                        {
                            /*IMPUESTOS TRASLADO*/
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("         <Impuestos>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("             <Traslados>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("             <Traslado ");
                            xmlRequest.Append("Base=\"" + globalDTO.LstConcepto[i].ImporteBase.ToString(new CultureInfo("es-MX")) + "\" ");
                            xmlRequest.Append("Importe=\"" + globalDTO.LstConcepto[i].ImporteIva.ToString(new CultureInfo("es-MX")) + "\" ");
                            xmlRequest.Append("Impuesto=\"" + globalDTO.LstConcepto[i].Impuesto + "\" ");
                            xmlRequest.Append("TasaOCuota=\"" + globalDTO.LstConcepto[i].TasaOCuota.ToString(new CultureInfo("es-MX")) + "\" ");
                            xmlRequest.Append("TipoFactor=\"" + globalDTO.LstConcepto[i].Factor + "\"/>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("             </Traslados>");
                            xmlRequest.Append(Environment.NewLine);
                            xmlRequest.Append("         </Impuestos>");

                            impuestoDTO = new ImpuestosDTO();

                            impuestoDTO.Importe = globalDTO.LstConcepto[i].ImporteIva;
                            impuestoDTO.CodigoImpuesto = globalDTO.LstConcepto[i].Impuesto;
                            impuestoDTO.TasaOCuota = globalDTO.LstConcepto[i].TasaOCuota;
                            impuestoDTO.DescripcionTipoFactor = globalDTO.LstConcepto[i].Factor;

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
                    xmlRequest.Append("TasaOCuota=\"" + lstImpuestos[0].TasaOCuota.ToString(new CultureInfo("es-MX")) + "\" ");
                    xmlRequest.Append("Importe=\"" + lstImpuestos[0].Importe.ToString(new CultureInfo("es-MX")) + "\"/>");

                    xmlRequest.Append(Environment.NewLine);
                    xmlRequest.Append("     </Traslados>");
                    xmlRequest.Append(Environment.NewLine);
                    xmlRequest.Append(" </Impuestos>");
                }

                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("</Comprobante>");
                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("    <Transaccion ");

                //CVG    
                //Se agregan los valores adicionales para la facturacion

                string claveFacFacturalo = "";
                string nombreFacFacturalo = "";
                string sucursal = "";
                string emailReceptor = "";
                string codigoReceptor = "";
                bool activarCorreoGen = true;
                bool activarSucMatriz = true;

                DataTable dtResult = daoConf.ConsultarConfiguracion("VBPAQ");

                try
                {
                    claveFacFacturalo = dtResult.Select("Nombre = 'ClaveFacFacturalo'").CopyToDataTable().Rows[0]["Valor"].ToString();
                    nombreFacFacturalo = dtResult.Select("Nombre = 'NombreFacFacturalo'").CopyToDataTable().Rows[0]["Valor"].ToString();
                    sucursal = dtResult.Select("Nombre = 'NombreSucursalGenerica'").CopyToDataTable().Rows[0]["Valor"].ToString();
                    emailReceptor = dtResult.Select("Nombre = 'EmailReceptor'").CopyToDataTable().Rows[0]["Valor"].ToString();
                    codigoReceptor = dtResult.Select("Nombre = 'CodigoReceptor'").CopyToDataTable().Rows[0]["Valor"].ToString();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                xmlRequest.Append("id=\"" + GeneratransactionID(globalDTO.RFCEmisor, globalDTO.Folio, globalDTO.FechaEmision).ToString() + "\"/>");
                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("	<TipoComprobante ");
                xmlRequest.Append("clave=\"" + claveFacFacturalo + "\" ");
                xmlRequest.Append("nombre=\"" + nombreFacFacturalo + "\"/>");
                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("	<Sucursal ");

                if (activarSucMatriz)
                {
                    xmlRequest.Append("nombre=\"" + sucursal + "\"/>");
                }
                else
                {
                    //Falta definir el catalogo de sucursales por location cuando se implemente este cambio
                    xmlRequest.Append("nombre=\"" + sucursal + "\"/>");
                }


                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("	<Receptor ");
                if (activarCorreoGen)
                {
                    xmlRequest.Append("emailReceptor=\"" + emailReceptor + "\" ");
                    xmlRequest.Append("codigoReceptor=\"" + codigoReceptor + "\"/>");
                }
                //else
                //{
                //    xmlRequest.Append("emailReceptor=\"" + globalDTO.EmailReceptor + "\" ");
                //    xmlRequest.Append("codigoReceptor=\"" + factura.EmailReceptor + "\"/>");
                //}

                xmlRequest.Append(Environment.NewLine);
                xmlRequest.Append("</RequestCFD>");

                StreamWriter writer = new StreamWriter(rutaXML);
                writer.Write(xmlRequest.ToString());
                writer.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* 4.- MÉTODO PARA GENERAR ARCHIVO PDF*/
        //public MemoryStream GeneraPDFFactura(GlobalDTO globalDTO)


        /* 5.- MÉTODO PARA INSERTAR EN LA TABLA DE PAGOSFACTURA LA INFORMACIÓN DEL TIMBRADO*/
        public Boolean InsertarDatosFactura(FacturaGlobal globalDTO)
        {
            Boolean pdf = new Boolean();
            try
            {
                DAOFacturaGlobal daoFacturaGlobal = new DAOFacturaGlobal();
                DataTable dtGlobal = new DataTable();

                //pdf = Comun.PDF.FacturaGlobal.GeneraPDFFactura(ref globalDTO);

                dtGlobal = daoFacturaGlobal.ActualizarFactura(globalDTO, "E");

                if (dtGlobal.Rows[0][0].ToString() != "ERROR")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        //Generar transaccionID
        public string GeneratransactionID(string rfcEmisor, string folioFiscal, string fechaFactura)
        {
            string result = "";

            DataTable dtResult = daoConf.ConsultarConfiguracion("VBPAQ");

            try
            {
                string codigoSAP = "";
                string leyenda = "GLOBAL";

                codigoSAP = dtResult.Select("Nombre = 'CodigoSAPGenerico'").CopyToDataTable().Rows[0]["Valor"].ToString();

                result = rfcEmisor.Substring(0, 3) + codigoSAP + fechaFactura + leyenda + "-" + folioFiscal;
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


        #region ComplementosPDFFactura





        #endregion
    }
}

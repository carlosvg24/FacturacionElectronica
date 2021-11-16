using Comun.Security;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.DAL.FacturaGlobal;
using Facturacion.ENT;
using Facturacion.ENT.Comun;
using Facturacion.ENT.FacturaGlobal;
using MetodosComunes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace Facturacion.BLL.FacturaGlobal
{
    public static class BLLFacturaGlobal
    {
        #region Metodos Privados

        private static FacturasCab.UtFacturasCab GetUTFacturasCab(XElement nodoMain, ENTXmlPegaso xmlPegaso, Dictionary<string, Dictionary<string, string>> parametros)
        {
            FacturasCab facturaCab = new FacturasCab();

            facturaCab.BookingID = 0;
            facturaCab.CondicionesPago = string.Empty;
            facturaCab.Descuento = 0;
            facturaCab.EmailReceptor = string.Empty;
            facturaCab.EsExtranjero = false;
            facturaCab.Estatus = "V";
            facturaCab.FechaHoraCancelLocal = new DateTime();
            facturaCab.FechaHoraExpedicion = xmlPegaso.FechaTimbrado;
            facturaCab.FechaHoraLocal = DateTime.Now;
            facturaCab.FolioFactura = long.Parse(nodoMain.Element("Comprobante").Attribute("Folio").Value);
            facturaCab.FormaPago = nodoMain.Element("Comprobante").Attribute("FormaPago").Value;
            facturaCab.IdAgente = 0;
            facturaCab.IdEmpresa = byte.Parse(parametros["OTROS"]["IdEmpresa"]);
            facturaCab.IdFacturaCab = 0;//Identity
            facturaCab.IdPaisResidenciaFiscal = string.Empty;
            facturaCab.IdPeticionPAC = xmlPegaso.IdPeticionPAC;
            facturaCab.IdRegimenFiscal = nodoMain.Element("Comprobante").Element("Emisor").Attribute("RegimenFiscal").Value;
            facturaCab.IdUsuario = 0;
            facturaCab.IdUsuarioCancelo = 0;
            facturaCab.LugarExpedicion = parametros["COMPROBANTE"]["LugarExpedicion"];
            facturaCab.MetodoPago = parametros["COMPROBANTE"]["MetodoPago"];
            facturaCab.Moneda = nodoMain.Element("Comprobante").Attribute("Moneda").Value;
            facturaCab.MontoIVA = decimal.Parse(nodoMain.Element("Comprobante").Element("Impuestos").Attribute("TotalImpuestosTrasladados").Value);
            facturaCab.MontoOtrosCargos = decimal.Parse(nodoMain.Element("Comprobante").Element("Complemento").Element("Aerolineas").Element("OtrosCargos").Attribute("TotalCargos").Value);
            facturaCab.MontoServAdic = 0;
            facturaCab.MontoTarifa = 0;
            facturaCab.MontoTUA = decimal.Parse(nodoMain.Element("Comprobante").Element("Complemento").Element("Aerolineas").Attribute("TUA").Value);
            facturaCab.NoCertificado = nodoMain.Element("Comprobante").Attribute("NoCertificado").Value;
            facturaCab.NumRegIdTrib = string.Empty;
            facturaCab.RazonSocialEmisor = nodoMain.Element("Comprobante").Element("Emisor").Attribute("Nombre").Value;
            facturaCab.RazonSocialReceptor = string.Empty;
            facturaCab.RfcEmisor = parametros["EMISOR"]["RFC"];
            facturaCab.RfcReceptor = parametros["RECEPTOR"]["RFC"];
            facturaCab.Serie = parametros["COMPROBANTE"]["Serie"];
            facturaCab.SubTotal = Decimal.Parse(nodoMain.Element("Comprobante").Attribute("SubTotal").Value);
            facturaCab.TipoCambio = Decimal.Parse(nodoMain.Element("Comprobante").Attribute("TipoCambio").Value);
            facturaCab.TipoComprobante = parametros["COMPROBANTE"]["TipoComprobante"];
            facturaCab.TipoFacturacion = "FG";
            facturaCab.Total = decimal.Parse(nodoMain.Element("Comprobante").Attribute("Total").Value);
            facturaCab.TransactionID = nodoMain.Element("Transaccion").Attribute("id").Value;
            facturaCab.UsoCFDI = nodoMain.Element("Comprobante").Element("Receptor").Attribute("UsoCFDI").Value;
            facturaCab.Version = parametros["COMPROBANTE"]["Version"];
            facturaCab.UUID = xmlPegaso.TFD_UUID;

            FacturasCab.UtFacturasCab utFacturasCab = new FacturasCab.UtFacturasCab();
            utFacturasCab.AddRange(new List<FacturasCab>() { facturaCab });

            return utFacturasCab;
        }

        private static FacturasDet.UtFacturasDet GetUtFacturasDet(List<PagoTraslados> pagos, Dictionary<string, Dictionary<string, string>> parametros, Int64 idFacturaCab)
        {

            List<FacturasDet> facturasDetalle = new List<FacturasDet>();
            int contador = 0;

            foreach (PagoTraslados pago in pagos)
            {
                FacturasDet facturaDetalle = new FacturasDet();
                facturaDetalle.Cantidad = int.Parse(parametros["CONCEPTO"]["Cantidad"]);
                facturaDetalle.ClaveProdServ = parametros["CONCEPTO"]["ClaveProdServ"];
                facturaDetalle.ClaveUnidad = parametros["CONCEPTO"]["ClaveUnidad"];
                facturaDetalle.Descripcion = parametros["CONCEPTO"]["Descripcion"];
                facturaDetalle.FechaHoraLocal = DateTime.Now;
                facturaDetalle.IdFacturaCab = idFacturaCab;
                facturaDetalle.NoIdentificacion = pago.PaymentId.ToString();
                facturaDetalle.Unidad = string.Empty;
                facturaDetalle.ValorUnitario = pago.MontoTotal - pago.MontoIVA;
                facturaDetalle.Importe = (facturaDetalle.ValorUnitario * facturaDetalle.Cantidad);
                facturaDetalle.IdFacturaDet = ++contador;

                facturasDetalle.Add(facturaDetalle);
            }

            FacturasDet.UtFacturasDet utFacturasDet = new FacturasDet.UtFacturasDet();
            utFacturasDet.AddRange(facturasDetalle);

            return utFacturasDet;
        }

        private static FacturaIVADet.UtFacturasivaDet GetUtFacturaIvaDet(List<PagoTraslados> pagos, Dictionary<string, Dictionary<string, string>> parametros, Int64 idFacturaCab)
        {
            List<FacturaIVADet> facturasIvaDet = new List<FacturaIVADet>();
            int contador = 0;

            foreach (PagoTraslados pago in pagos)
            {
                ++contador;

                FacturaIVADet factIVADet = new FacturaIVADet();
                factIVADet.Base = pago.Base;
                factIVADet.FechaHoraLocal = DateTime.Now;
                factIVADet.IdFacturaCab = idFacturaCab;
                factIVADet.IdFacturaDet = contador;
                factIVADet.Importe = Math.Round(pago.MontoIVA,2);
                factIVADet.Impuesto = parametros["TRASLADO"]["Impuesto"];
                factIVADet.TasaOCuota = Decimal.Parse(parametros["TRASLADO"]["IVA"]);
                factIVADet.TipoFactor = parametros["TRASLADO"]["TipoFactor"];

                facturasIvaDet.Add(factIVADet);

                FacturaIVADet factIVA0Det = new FacturaIVADet();
                factIVA0Det.Base = pago.Base0;
                factIVA0Det.FechaHoraLocal = DateTime.Now;
                factIVA0Det.IdFacturaCab = idFacturaCab;
                factIVA0Det.IdFacturaDet = contador;
                factIVA0Det.Importe = 0;
                factIVA0Det.Impuesto = parametros["TRASLADO"]["Impuesto"];
                factIVA0Det.TasaOCuota = 0.0M;
                factIVA0Det.TipoFactor = parametros["TRASLADO"]["TipoFactor"];

                facturasIvaDet.Add(factIVA0Det);
            }

            FacturaIVADet.UtFacturasivaDet utFacturaIVADet = new FacturaIVADet.UtFacturasivaDet();
            utFacturaIVADet.AddRange(facturasIvaDet);

            return utFacturaIVADet;
        }

        private static GlobalticketsDet.UtGlobalticketsDet GetUtGlobalticketsDet(List<PagoTraslados> pagos, Int64 idFacturaCab)
        {
            List<GlobalticketsDet> globalTickets = new List<GlobalticketsDet>();
            int contador = 0;

            foreach (PagoTraslados pago in pagos)
            {
                ++contador;

                GlobalticketsDet globalTicket = new GlobalticketsDet();
                globalTicket.BookingID = pago.BookingId;
                globalTicket.FechaHoraLocal = DateTime.Now;
                globalTicket.IdFacturaCab = idFacturaCab;
                globalTicket.IdFacturaDet = contador;
                globalTicket.PaymentID = pago.PaymentId;

                globalTickets.Add(globalTicket);
            }

            GlobalticketsDet.UtGlobalticketsDet utGlobalticketsDet = new GlobalticketsDet.UtGlobalticketsDet();
            utGlobalticketsDet.AddRange(globalTickets);

            return utGlobalticketsDet;
        }


        private static FacturasCargoDet.UtFacturasCargoDet GetUtFacturasCargoDet(XElement nodoComplemento, Int64 idFacturaCab)
        {
            List<XElement> Cargos = nodoComplemento.Element("Aerolineas").Element("OtrosCargos").Elements("Cargo").ToList();
            string TUA = nodoComplemento.Element("Aerolineas").Attribute("TUA").Value;
            List<FacturasCargoDet> cargosDet = new List<FacturasCargoDet>();
            DateTime ahora = DateTime.Now;

            foreach (XElement cargo in Cargos)
            {
                FacturasCargoDet cargoDet = new FacturasCargoDet();
                cargoDet.CodigoCargo = cargo.Attribute("CodigoCargo").Value;
                cargoDet.EsTua = (cargoDet.CodigoCargo.ToUpper() == "XV") ? (true) : (false);
                cargoDet.IdFacturaCab = idFacturaCab;
                cargoDet.IdFacturaDet = 1;
                cargoDet.Importe = decimal.Parse(cargo.Attribute("Importe").Value);
                cargoDet.FechaHoraLocal = ahora;

                cargosDet.Add(cargoDet);
            }

            FacturasCargoDet cargoTUA = new FacturasCargoDet()
            {
                CodigoCargo = "XV",
                EsTua = true,
                IdFacturaCab = idFacturaCab,
                IdFacturaDet = 1,
                Importe = decimal.Parse(TUA),
                FechaHoraLocal = ahora
            };

            cargosDet.Add(cargoTUA);

            FacturasCargoDet.UtFacturasCargoDet ut = new FacturasCargoDet.UtFacturasCargoDet();
            ut.AddRange(cargosDet);

            return ut;
        }

        #endregion


        #region Metodos Publicos

        public static List<MonedaPagos> BLGetPagosByCurrencyCode(DateTime fechaInicial,DateTime fechaFinal)
        {
            return DALFacturaGlobal.DALGetPagosByCurrencyCode(fechaInicial,fechaFinal);
        }

        public static List<PagoTraslados> BLGetPagos(/*MansLog.Error.LogError log,*/decimal IVA, DateTime fechaInicial,DateTime fechaFinal, string moneda = null, Int64? folioPrefacatura = null)
        {                                                                               //Cuando el proceso diario se atrasa se factura un pago que ya se dividio una fecha posterior por lo tanto
            List<ENTPagosCab> pagosHijosPorAtrasoDelProceso = new List<ENTPagosCab>();  //sus hijos no exiten en esta fecha sinoen fecha posteriores y por eso hay que buscar los hijos para sumar 
            List<PagoTraslados> pagosDivididos = new List<PagoTraslados>();             //cantidades y facturarlo
            List<PagoTraslados> pagosAll = new List<PagoTraslados>();
            
            pagosDivididos.AddRange(DALFacturaGlobal.DALGetPagos(IVA, true, fechaInicial,fechaFinal, moneda, folioPrefacatura));
            //log.escribir(string.Format("Seo btuvieron {0} pagos dividios", pagosDivididos.Count));
            pagosAll = pagosDivididos.FindAll(f => f.EsPadre == true).ToList();
            //log.escribir(string.Format("Pagos padre {0}", pagosAll.Count));
            pagosHijosPorAtrasoDelProceso = DALFacturaGlobal.DALGetPagosHijosPorProcesoAtrasado(fechaFinal, string.Join("|", pagosAll.Select(m => m.PaymentId).ToArray()));//Obtiene Pagos hijos
            //log.escribir(string.Format("Hijos adelantados por poroceso atrasado {0}", pagosHijosPorAtrasoDelProceso.Count));
            //log.escribir("Hijos== " + new JavaScriptSerializer().Serialize(pagosDivididos));
            //Obtiene pagos hijos de fechas mas adelante para sacar su pagoTraslado
            foreach (PagoTraslados padre in pagosAll)
            {
                //log.escribir("Padre== " + new JavaScriptSerializer().Serialize(padre));                
                var hijos = pagosDivididos.FindAll(f => f.PaymentIdPadre == padre.PaymentId);
                //log.escribir(string.Format("Hijos encontrados normales {0}",string.Join(",", hijos.Select(s => s.PaymentId).ToArray())));
                List<ENTPagosCab> hijosDeFechasAdelante = pagosHijosPorAtrasoDelProceso.FindAll(f => f.ParentPaymentID == padre.PaymentId).ToList();
                //log.escribir(string.Format("Hijos encontrados {0}", string.Join(",", hijosDeFechasAdelante.Select(s => s.PaymentID).ToArray())));
                foreach (ENTPagosCab hijoAdelantado in hijosDeFechasAdelante)
                {
                    try
                    {
                        PagoTraslados pagoTrasladoHijoAdelantado = DALFacturaGlobal.DALGetPagos(IVA, true, hijoAdelantado.FechaPago, hijoAdelantado.FechaPago, moneda, hijoAdelantado.FolioPrefactura)[0];
                        hijos.Add(pagoTrasladoHijoAdelantado);
                    }
                    catch(Exception){ }
                }
                // var json = new JavaScriptSerializer().Serialize(obj);
                //log.escribir(new JavaScriptSerializer().Serialize(hijos));
                padre.MontoTotal = padre.MontoTotal + hijos.Sum(s => s.MontoTotal);
                padre.Base = padre.Base + hijos.Sum(s => s.Base);
                padre.Base0 = padre.Base0 + hijos.Sum(s => s.Base0);
                padre.MontoIVA = padre.MontoIVA + hijos.Sum(s => s.MontoIVA);
            }

            pagosAll.AddRange( DALFacturaGlobal.DALGetPagos(IVA,false ,fechaInicial,fechaFinal, moneda, folioPrefacatura));

            return pagosAll;
        }

        public static List<T> BLCargoComplemento<T>(string cadenaIdPagosCab, string moneda)
        {
            return DALFacturaGlobal.DALGetCargosComplemento<T>(cadenaIdPagosCab, moneda);
        }

        /// <summary>
        /// Obtiene las configuraciones y parametros generales del servicio
        /// </summary>
        /// <param name="getWebConfig">True obtiene del web.config la cadena. False desde BD </param>
        /// <returns></returns>
        public static List<ENTParametrosCnf> GETStringsConfigurationAndParams()
        {
            BLLBitacoraErrores param = new BLLBitacoraErrores();

            return param.RecuperarParametros();
        }

        /// <summary>
        /// Se obtiene folio del elemento "Comprobante"
        /// </summary>
        /// <returns></returns>
        public static Int64 GetFolio()
        {
            BLLFoliosfiscalesCnf folioFiscalBLL = new BLLFoliosfiscalesCnf();
            folioFiscalBLL.RecuperarFoliosfiscalesCnfPorLlavePrimaria(2);

            folioFiscalBLL.FolioActual++;
            folioFiscalBLL.Actualizar();

            return folioFiscalBLL.FolioActual;
        }

        public static List<ENTEmpresaCat> GetAllEmpresas()
        {
            BLLEmpresaCat bllEmresasCat = new BLLEmpresaCat();
            return bllEmresasCat.RecuperarTodo();
        }

        public static ResponseGeneric GuardarFacturaGlobal(XElement nodoMain, ENTXmlPegaso xmlPegaso, List<PagoTraslados> pagos, Dictionary<string, Dictionary<string, string>> parametros)
        {
            ResponseGeneric response = new ResponseGeneric();
            Encrypt encrypt = new Encrypt();
            Int64 IdFacturaCab = 0;

            DALFacturaGlobal dal = new DALFacturaGlobal(encrypt.DecryptKey(DALFacturaGlobal.DALGetCadenaConexion("DBFacturacion")));

            dal.BeginTran();

            try
            {
                IdFacturaCab = Int64.Parse(dal.InsertUtFacturasCab(GetUTFacturasCab(nodoMain, xmlPegaso, parametros)).ToString());

                dal.InsertUtENTFacturasDet(GetUtFacturasDet(pagos, parametros, IdFacturaCab));

                dal.InsertUtENTFacturasivaDet(GetUtFacturaIvaDet(pagos, parametros, IdFacturaCab));

                dal.InsertUtENTGlobalticketsDet(GetUtGlobalticketsDet(pagos, IdFacturaCab));

                dal.InsertUtENTFacturasCargos_Det(GetUtFacturasCargoDet(nodoMain.Element("Comprobante").Element("Complemento"), IdFacturaCab));

                dal.InsertFacturasCFDIDet(IdFacturaCab, xmlPegaso);

                dal.CommitTran();

                response.Succes = true;
                response.Message = string.Format("Toda la informacon de la Factura Global fue guardada sastifactoriamente");
            }
            catch(SqlException ex)
            {
                response.Succes = false;
                dal.RollBackTran();
                response.Message = string.Format("No se pudo guardar la informacion de la factura global.\n\r  -  {0}\n\r  - {1}\n\r Servidor {2}", ex.Message, ex.StackTrace,ex.Server);
            }
            catch (Exception ex)
            {
                response.Succes = false;
                dal.RollBackTran();
                response.Message = string.Format("No se pudo guardar la informacion de la factura global.\n\r  -  {0}\n\r  - {1}", ex.Message, ex.StackTrace);
            }


            return response;
        }

        public static ResponseGeneric TestConexionBD()
        {
            var result = DALFacturaGlobal.DALTestConection("DBFacturacion");

            if (result.Succes == false)
                return result;

            result = DALFacturaGlobal.DALTestConection("DBNavitaire");

            if (result.Succes == false)
                return result;

            return new ResponseGeneric() { Succes = true };
        }

        public static ResponseGeneric ValidarSpNecesarios(List<StoredProcedureValidation> sps)
        {
            ResponseGeneric response = new ResponseGeneric();
            StoredProcedureValidation.UtStoredProcedureValidation utSps = new StoredProcedureValidation.UtStoredProcedureValidation();
            utSps.AddRange(sps);

            var validaciones = DALFacturaGlobal.DALValidarExistenciaSp(utSps);

            if (validaciones.FindAll(f => f.IsExist == false).ToList().Count > 0)
            {
                string spsFaltantes = string.Empty;
                response.Succes = false;

                foreach (StoredProcedureValidation sp in validaciones.FindAll(f => f.IsExist == false).ToList())
                {
                    spsFaltantes = string.Format("{0},{1}", spsFaltantes, sp.NameSp);
                }

                response.Message = string.Format("No es posible correr la tarea de Factura Global si hace falta los siguientes Stored procedures: {0}", spsFaltantes.Substring(1));
            }
            else
            {
                response.Succes = true;
            }

            return response;
        }

        public static Decimal GetDolarEnPesos(DateTime fechaPago)
        {
            BLLDistribucionPagos bllDist = new BLLDistribucionPagos();
            decimal tipoCambio = 0;

            return bllDist.RecuperarTipoCambioPorFecha(fechaPago);
        }

        #endregion
    }
}

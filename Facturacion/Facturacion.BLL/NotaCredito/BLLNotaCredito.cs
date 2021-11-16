using Comun.Security;
using Facturacion.BLL.FacturaGlobal;
using Facturacion.DAL;
using Facturacion.DAL.FacturaGlobal;
using Facturacion.DAL.NotaCredito;
using Facturacion.ENT;
using Facturacion.ENT.Comun;

using Facturacion.ENT.NotaCredito;
using MansLog.Error;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Facturacion.BLL.NotaCredito
{
    public static class BLLNotaCredito
    {
        public static ResponseGeneric ValidarSpNecesarios(List<StoredProcedureValidation> sps)
        {
            ResponseGeneric response = new ResponseGeneric();
            StoredProcedureValidation.UtStoredProcedureValidation utSps = new StoredProcedureValidation.UtStoredProcedureValidation();
            utSps.AddRange(sps);

            var validaciones = DALNotaCredito.DALValidarExistenciaSp(utSps);

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
        public static ResponseGeneric TestConexionBD()
        {
            var result = DALNotaCredito.DALTestConection("DBFacturacion");

            if (result.Succes == false)
                return result;

            result = DALNotaCredito.DALTestConection("DBNavitaire");

            if (result.Succes == false)
                return result;

            return new ResponseGeneric() { Succes = true };
        }

        public static List<Cargo> GetCargosComplemento(string cadenaIdPagosCab,string moneda)
        {
            return FacturaGlobal.BLLFacturaGlobal.BLCargoComplemento<Cargo>(cadenaIdPagosCab, moneda);
        }

        public static List<PagosParaNotaCredito> GetPagosNotaCredito(DateTime fechaIni,DateTime fechaFin,string nameSP)
        {
            return DALNotaCredito.DALGetPagosParaNotaCredito(fechaIni,fechaFin,nameSP);
        }

        public static Int64 GetFolio()
        {
            BLLFoliosfiscalesCnf folioFiscalBLL = new BLLFoliosfiscalesCnf();
            folioFiscalBLL.RecuperarFoliosfiscalesCnfPorLlavePrimaria(1);

            folioFiscalBLL.FolioActual++;
            folioFiscalBLL.Actualizar();

            return folioFiscalBLL.FolioActual;
        }

        public static InfoNotaCredito GetInfoNotaCredito(long idFacturaCab, Int64[] paymentId)
        {
            InfoNotaCredito info = new InfoNotaCredito();

            string cadena = string.Join("|", paymentId);

            info.Comprobante = DALNotaCredito.DALGetFacturaCab(idFacturaCab);
            info.Conceptos = DALNotaCredito.DALGetFacturaDetByPaymentID(cadena);
            info.Traslados = DALNotaCredito.DALGetFacturaIVADetByPaymentID(cadena);

            List<int> listFactDet = info.Traslados.Select(s => s.IdFacturaDet).Distinct().ToList();

            info.FacturaDet = DALNotaCredito.DALGetFacturasDet(idFacturaCab, string.Join("|", listFactDet.ToArray()));

            return info;
        }

        public static List<ENTEmpresaCat> GetAllEmpresas()
        {
            BLLEmpresaCat bllEmresasCat = new BLLEmpresaCat();
            return bllEmresasCat.RecuperarTodo();
        }

        

        public static ResponseGeneric GuardarNotaCredito(ENTXmlPegaso xmlPegaso, XElement nodoMain, Dictionary<string, Dictionary<string, string>> parametros, InfoNotaCredito info, List<PagosParaNotaCredito> pagosFacturadosDiferenteDia, Int64 idFacturaCab)
        {
            ResponseGeneric response = new ResponseGeneric();
            Encrypt encrypt = new Encrypt();

            Int64 idNotaCreditoCab = 0;

            DALNotaCredito dal = new DALNotaCredito(encrypt.DecryptKey(DALNotaCredito.DALGetCadenaConexion("DBFacturacion")));

            dal.BeginTran();

            try
            {
                List<GlobalTickets_Det> globalTickets = DALNotaCredito.DALGetGlobalTicketsDet(string.Join("|", pagosFacturadosDiferenteDia.Select(o => o.PaymentId).ToArray()), idFacturaCab);
                idNotaCreditoCab = dal.InsertUtNotasCreditoCab(GetUtNotasCreditoCab(xmlPegaso, nodoMain, parametros,0,"NG"));
                dal.InsertUtNotasCreditoDet(GetUtNotasCreditoDet(info.Conceptos, idNotaCreditoCab));
                dal.InsertUtNotasCreditoIVADet(GetUtNotaCreditoIvaDet(info.Conceptos, info.Traslados, idNotaCreditoCab));
                dal.InsertUpdateUtENTGlobalticketsDet(GetUtGlobalTicketsDet(globalTickets, pagosFacturadosDiferenteDia, idNotaCreditoCab));
                dal.InsertNotaCreditoCFDIDet(idNotaCreditoCab, xmlPegaso);
                //dal.InsertUtNotasCreditoCargo
                dal.InsertCFDIRelacionadosDet(GetENTCfdirelacionadosDet(idNotaCreditoCab, idFacturaCab, nodoMain));

                dal.CommitTran();

                response.Succes = true;
                response.Message = string.Format("Toda la informacon de la Nota de Credito fue guardada sastifactoriamente");
            }
            catch (SqlException ex)
            {
                response.Succes = false;
                dal.RollBackTran();
                response.Message = string.Format("No se pudo guardar la informacion de la Nota de Credito.\n\r  -  {0}\n\r  - {1}\n\r Servidor {2}", ex.Message, ex.StackTrace, ex.Server);
            }
            catch (Exception ex)
            {
                response.Succes = false;
                dal.RollBackTran();
                response.Message = string.Format("No se pudo guardar la informacion de la Nota de Credito.\n\r  -  {0}\n\r  - {1}", ex.Message, ex.StackTrace);
            }

            return response;
        }

        public static ResponseGeneric GuardarNotaCreditoCliente(ENTXmlPegaso xmlPegaso, XElement nodoMain, Dictionary<string, Dictionary<string, string>> parametros, List<Concepto> conceptos, List<Traslado> traslados, PagosParaNotaCredito pago)
        {
            ResponseGeneric response = new ResponseGeneric();
            Encrypt encrypt = new Encrypt();

            Int64 idNotaCreditoCab = 0;

            DALNotaCredito dal = new DALNotaCredito(encrypt.DecryptKey(DALNotaCredito.DALGetCadenaConexion("DBFacturacion")));

            dal.BeginTran();

            try
            {
                BLLPagosCab bllPagosCab = new BLLPagosCab();
                List<ENTPagosCab> pagosCab= bllPagosCab.RecuperarPagosCabPaymentid(pago.PaymentId);

                idNotaCreditoCab = dal.InsertUtNotasCreditoCab(GetUtNotasCreditoCab(xmlPegaso, nodoMain, parametros,pago.BookingId,"NC"));

                //************************* ACtualizando PagosCab *****************************

                if (pagosCab.Count == 1)
                    pagosCab[0].IdNotaCreditoCab = idNotaCreditoCab;
                else
                    throw new Exception(string.Format("No se encontro la entidad del pago con PamnetId={0}", pago.PaymentId));

                object classInstance = bllPagosCab;

                foreach (PropertyInfo prop in typeof(ENTPagosCab).GetProperties())
                {                    
                    PropertyInfo property=classInstance.GetType().GetProperty(prop.Name);
                    var valor = pagosCab[0].GetType().GetProperty(prop.Name).GetValue(pagosCab[0]);
                    property.SetValue(classInstance, valor);
                }

                ((BLLPagosCab)classInstance).Actualizar();
                //*******************************************************************************

                NotasCreditoDet.UtNotasCreditoDet utCargosDet = GetUtNotasCreditoDet(conceptos, idNotaCreditoCab);
                int idFacturaDetCargosAereopuertarios= utCargosDet.Find(f => f.NoIdentificacion == "003").IdNotaCreditoDet;

                dal.InsertUtNotasCreditoDet(utCargosDet);
                dal.InsertUtNotasCreditoIVADet(GetUtNotaCreditoIvaDet(conceptos, traslados, idNotaCreditoCab));
                dal.InsertUtNotasCreditoCargo(GetUtNotasCreditoCargoDet(nodoMain.Element("Comprobante").Element("Complemento"), idNotaCreditoCab,idFacturaDetCargosAereopuertarios));
                dal.InsertNotaCreditoCFDIDet(idNotaCreditoCab, xmlPegaso);
                dal.InsertCFDIRelacionadosDet(GetENTCfdirelacionadosDet(idNotaCreditoCab,pagosCab[0].IdFacturaCab , nodoMain));

                dal.CommitTran();

                response.Succes = true;
                response.Message = idNotaCreditoCab.ToString();
            }
            catch (SqlException ex)
            {
                response.Succes = false;
                dal.RollBackTran();
                response.Message = string.Format("No se pudo guardar la informacion de la Nota de Credito.\n\r  -  {0}\n\r  - {1}\n\r Servidor {2}", ex.Message, ex.StackTrace, ex.Server);
            }
            catch (Exception ex)
            {
                response.Succes = false;
                dal.RollBackTran();
                response.Message = string.Format("No se pudo guardar la informacion de la Nota de Credito.\n\r  -  {0}\n\r  - {1}", ex.Message, ex.StackTrace);
            }

            return response;
        }

        public static List<PagosParaNotaCredito> GetPagosFacturasClienteParaNotaCredito(DateTime fechaInicial,DateTime fechaFinal)
        {
            return DALNotaCredito.DalGetPagosFacturasClienteParaNotaCredito(fechaInicial, fechaFinal);
        }

        private static NotasCreditoCab.UtNotasCreditoCab GetUtNotasCreditoCab(ENTXmlPegaso xmlPegaso, XElement nodoMain, Dictionary<string, Dictionary<string, string>> parametros, Int64 bookingID, string tipoFacturacion)
        {
            NotasCreditoCab notaCreditoCab = new NotasCreditoCab();

            notaCreditoCab.BookingID = bookingID;
            notaCreditoCab.CondicionesPago = string.Empty;
            notaCreditoCab.Descuento = 0;
            notaCreditoCab.EmailReceptor = string.Empty;
            notaCreditoCab.EsExtranjero = false;
            notaCreditoCab.Estatus = "V";
            notaCreditoCab.FechaHoraCancelLocal = new DateTime();
            notaCreditoCab.FechaHoraExpedicion = xmlPegaso.FechaTimbrado;
            notaCreditoCab.FechaHoraLocal = DateTime.Now;
            notaCreditoCab.FolioFiscal = long.Parse(nodoMain.Element("Comprobante").Attribute("Folio").Value);
            notaCreditoCab.FormaPago= nodoMain.Element("Comprobante").Attribute("FormaPago").Value;
            notaCreditoCab.IdAgente = 0;
            notaCreditoCab.IdEmpresa= byte.Parse(parametros["OTROS"]["IdEmpresa"]);
            notaCreditoCab.IdNotaCreditoCab = 0;
            notaCreditoCab.IdPaisResidenciaFiscal = string.Empty;
            notaCreditoCab.IdPeticionPAC = xmlPegaso.IdPeticionPAC;
            notaCreditoCab.IdUsuario = 0;
            notaCreditoCab.IdUsuarioCancelo = 0;
            notaCreditoCab.LugarExpedicion = nodoMain.Element("Comprobante").Attribute("LugarExpedicion").Value;
            notaCreditoCab.MetodoPago= nodoMain.Element("Comprobante").Attribute("MetodoPago").Value;
            notaCreditoCab.Moneda= nodoMain.Element("Comprobante").Attribute("Moneda").Value;
            notaCreditoCab.MontoIVA= decimal.Parse(nodoMain.Element("Comprobante").Element("Impuestos").Attribute("TotalImpuestosTrasladados").Value);
            notaCreditoCab.MontoOtrosCargos = 0; //decimal.Parse(nodoMain.Element("Comprobante").Element("Complemento").Element("Aerolineas").Element("OtrosCargos").Attribute("TotalCargos").Value);
            notaCreditoCab.MontoServAdic = 0;
            notaCreditoCab.MontoTarifa = 0;
            notaCreditoCab.MontoTUA = 0;
            notaCreditoCab.NumRegIdTrib = string.Empty;
            notaCreditoCab.RfcReceptor= nodoMain.Element("Comprobante").Element("Receptor").Attribute("Rfc").Value;
            notaCreditoCab.Serie= parametros["COMPROBANTE"]["Serie"];
            notaCreditoCab.SubTotal = Decimal.Parse(nodoMain.Element("Comprobante").Attribute("SubTotal").Value);
            notaCreditoCab.TipoCambio= Decimal.Parse(nodoMain.Element("Comprobante").Attribute("TipoCambio").Value);//*
            notaCreditoCab.TipoComprobante= parametros["COMPROBANTE"]["TipoComprobante"];
            notaCreditoCab.TipoFacturacion = tipoFacturacion;
            notaCreditoCab.Total= decimal.Parse(nodoMain.Element("Comprobante").Attribute("Total").Value);
            notaCreditoCab.TransactionID = nodoMain.Element("Transaccion").Attribute("id").Value;
            notaCreditoCab.UsoCFDI= nodoMain.Element("Comprobante").Element("Receptor").Attribute("UsoCFDI").Value;
            notaCreditoCab.UUID = xmlPegaso.TFD_UUID;
            notaCreditoCab.Version= parametros["COMPROBANTE"]["Version"];

            NotasCreditoCab.UtNotasCreditoCab ut = new NotasCreditoCab.UtNotasCreditoCab();
            ut.AddRange(new List<NotasCreditoCab>() { notaCreditoCab });

            return ut;
        }

        private static NotasCreditoDet.UtNotasCreditoDet GetUtNotasCreditoDet(List<Concepto> conceptos, Int64 idNotaCreditoCab)
        {
            List<NotasCreditoDet> notaCreditodet = new List<NotasCreditoDet>();
            int contador = 0;

            foreach(Concepto temp in conceptos)
            {
                contador++;

                NotasCreditoDet det = new NotasCreditoDet();
                det.Cantidad = temp.Cantidad;
                det.ClaveProdServ = temp.ClaveProdServ;
                det.ClaveUnidad = temp.ClaveUnidad;
                det.Descripcion = temp.Descripcion;
                det.Descuento = 0;
                det.FechaHoraLocal = DateTime.Now;
                det.IdNotaCreditoCab = idNotaCreditoCab;
                det.IdNotaCreditoDet = contador;
                det.Importe = temp.Importe;
                det.NoIdentificacion = temp.NoIdentificacion;
                det.Unidad = string.Empty;
                det.ValorUnitario = temp.ValorUnitario;

                notaCreditodet.Add(det);
            }

            NotasCreditoDet.UtNotasCreditoDet ut = new NotasCreditoDet.UtNotasCreditoDet();
            ut.AddRange(notaCreditodet);

            return ut;
        }

        private static NotaCreditoIVADet.UtNotasCreditoivaDet GetUtNotaCreditoIvaDet(List<Concepto> conceptos,List<Traslado> allTraslados, Int64 idNotacreditoCab)
        {
            List<NotaCreditoIVADet> lista = new List<NotaCreditoIVADet>();
            int contador = 0;

            foreach (Concepto concepto in conceptos)    
            {
                contador++;
                List<Traslado> traslados = allTraslados.FindAll(f => f.IdFacturaDet == concepto.IdFacturaDet).ToList();

                foreach(Traslado temp in traslados)
                {
                    NotaCreditoIVADet notaCreditoIvaDet = new NotaCreditoIVADet();

                    notaCreditoIvaDet.Base = Decimal.Parse(temp.Base);
                    notaCreditoIvaDet.FechaHoraLocal = DateTime.Now;
                    notaCreditoIvaDet.IdNotaCreditoCab = idNotacreditoCab;
                    notaCreditoIvaDet.IdNotaCreditoDet = contador;//se puede saber el IdNotaCreditoDet ya que es el mismo orden de la lista conceptos  a la hora de formar UtNotaCreditoDet 
                    notaCreditoIvaDet.Importe = temp.Importe;
                    notaCreditoIvaDet.Impuesto = temp.Impuesto;
                    notaCreditoIvaDet.TasaOCuota = temp.TasaOCuota;
                    notaCreditoIvaDet.TipoFactor = temp.TipoFactor;

                    lista.Add(notaCreditoIvaDet);
                }

            }            

            NotaCreditoIVADet.UtNotasCreditoivaDet ut = new NotaCreditoIVADet.UtNotasCreditoivaDet();
            ut.AddRange(lista);

            return ut;
        }

        private static GlobalTickets_Det.UtGlobalticketsDet GetUtGlobalTicketsDet(List<GlobalTickets_Det> globalTickets, List<PagosParaNotaCredito> pagosFacturadosDiferenteDia, Int64 idNotaCredito)
        {
            PagosParaNotaCredito[] copy= new PagosParaNotaCredito[pagosFacturadosDiferenteDia.Count];
            pagosFacturadosDiferenteDia.CopyTo(copy);

            List<PagosParaNotaCredito> lista = copy.ToList();

            foreach (GlobalTickets_Det temp in globalTickets)
            {
                PagosParaNotaCredito pagoDiferenteDia = lista.Find(f => f.BookingId == temp.BookingID && f.PaymentId == temp.PaymentID);

                temp.IdFacturaCabCliente = pagoDiferenteDia.FacturaIdFacturaCab;
                temp.IdNotaCredito = idNotaCredito;

                lista.Remove(pagoDiferenteDia);
            }

            GlobalTickets_Det.UtGlobalticketsDet ut = new GlobalTickets_Det.UtGlobalticketsDet();
            ut.AddRange(globalTickets);

            return ut;
        }

        private static ENTCfdirelacionadosDet GetENTCfdirelacionadosDet(Int64 idNotaCredito, Int64 idFacturaCab,XElement nodoMain)
        {
            ENTCfdirelacionadosDet entidad = new ENTCfdirelacionadosDet();
            entidad.FechaHoraLocal = DateTime.Now;
            entidad.IdCFDI = idNotaCredito;
            entidad.IdCfdiRel = 0;
            entidad.IdCFDIVinculado = idFacturaCab;
            entidad.TipoComprobante = "E";
            entidad.TipoRelacion = "01";
            entidad.UUIDVinculado= nodoMain.Element("Comprobante").Element("CfdiRelacionados").Element("CfdiRelacionado").Attribute("UUID").Value;

            return entidad;
        }

        private static NotasCreditoCargo.UtNotasCreditoCargo GetUtNotasCreditoCargoDet(XElement nodoComplemento, Int64 idNotaCreditoCab,int idFacturaDet)
        {
            List<XElement> Cargos = nodoComplemento.Element("Aerolineas").Element("OtrosCargos").Elements("Cargo").ToList();
            
            List<NotasCreditoCargo> cargosDet = new List<NotasCreditoCargo>();
            DateTime ahora = DateTime.Now;

            foreach (XElement cargo in Cargos)
            {
                NotasCreditoCargo cargoDet = new NotasCreditoCargo();
                cargoDet.CodigoCargo = cargo.Attribute("CodigoCargo").Value;                
                cargoDet.IdNotaCreditoCab = idNotaCreditoCab;
                cargoDet.IdNotaCreditoDet =idFacturaDet;
                cargoDet.Importe = decimal.Parse(cargo.Attribute("Importe").Value);
                cargoDet.FechaHoraLocal = ahora;

                cargosDet.Add(cargoDet);
            }            

            NotasCreditoCargo.UtNotasCreditoCargo ut = new NotasCreditoCargo.UtNotasCreditoCargo();
            ut.AddRange(cargosDet);

            return ut;
        }


        public static Decimal GetDolarEnPesos(DateTime fechaPago)
        {
            return BLLFacturaGlobal.GetDolarEnPesos(fechaPago);
        }


    }
}

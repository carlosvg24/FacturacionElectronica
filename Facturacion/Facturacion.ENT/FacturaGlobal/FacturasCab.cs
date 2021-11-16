using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.FacturaGlobal
{
    public class FacturasCab:ENTFacturasCab
    {
      
        public class UtFacturasCab : List<FacturasCab>, IEnumerable<SqlDataRecord>
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlDataRecord = new SqlDataRecord(
                                              new SqlMetaData("IdFacturaCab", SqlDbType.BigInt)
                                            , new SqlMetaData("IdEmpresa", SqlDbType.TinyInt)
                                            , new SqlMetaData("BookingID", SqlDbType.BigInt)
                                            , new SqlMetaData("FechaHoraExpedicion", SqlDbType.DateTime)
                                            , new SqlMetaData("TipoFacturacion", SqlDbType.VarChar,2)
                                            , new SqlMetaData("Version", SqlDbType.VarChar,4)
                                            , new SqlMetaData("Serie", SqlDbType.VarChar,3)
                                            , new SqlMetaData("FolioFactura", SqlDbType.BigInt)
                                            , new SqlMetaData("UUID", SqlDbType.VarChar,40)
                                            , new SqlMetaData("TransactionID", SqlDbType.VarChar,50)
                                            , new SqlMetaData("IdPeticionPAC", SqlDbType.BigInt)
                                            , new SqlMetaData("Estatus", SqlDbType.VarChar,1)
                                            , new SqlMetaData("RfcEmisor", SqlDbType.VarChar,13)
                                            , new SqlMetaData("RazonSocialEmisor", SqlDbType.VarChar,254)
                                            , new SqlMetaData("NoCertificado", SqlDbType.VarChar,20)
                                            , new SqlMetaData("IdRegimenFiscal", SqlDbType.VarChar,4)
                                            , new SqlMetaData("RfcReceptor", SqlDbType.VarChar,13)
                                            , new SqlMetaData("RazonSocialReceptor", SqlDbType.VarChar,254)
                                            , new SqlMetaData("EmailReceptor", SqlDbType.VarChar,256)
                                            , new SqlMetaData("EsExtranjero", SqlDbType.Bit)
                                            , new SqlMetaData("IdPaisResidenciaFisca", SqlDbType.VarChar,3)
                                            , new SqlMetaData("NumRegIdTrib", SqlDbType.VarChar,40)
                                            , new SqlMetaData("UsoCFDI", SqlDbType.VarChar,3)
                                            , new SqlMetaData("FormaPago", SqlDbType.VarChar,2)
                                            , new SqlMetaData("MetodoPago", SqlDbType.VarChar,3)
                                            , new SqlMetaData("TipoComprobante", SqlDbType.VarChar,1)
                                            , new SqlMetaData("LugarExpedicion", SqlDbType.VarChar,6)
                                            , new SqlMetaData("CondicionesPago", SqlDbType.VarChar,1000)
                                            , new SqlMetaData("Moneda", SqlDbType.VarChar,3)
                                            , new SqlMetaData("TipoCambio", SqlDbType.Money)
                                            , new SqlMetaData("SubTotal", SqlDbType.Money)
                                            , new SqlMetaData("Descuento", SqlDbType.Money)
                                            , new SqlMetaData("Total", SqlDbType.Money)
                                            , new SqlMetaData("MontoTarifa", SqlDbType.Money)
                                            , new SqlMetaData("MontoServAdic", SqlDbType.Money)
                                            , new SqlMetaData("MontoTUA", SqlDbType.Money)
                                            , new SqlMetaData("MontoOtrosCargos", SqlDbType.Money)
                                            , new SqlMetaData("MontoIVA", SqlDbType.Money)
                                            , new SqlMetaData("IdAgente", SqlDbType.Int)
                                            , new SqlMetaData("IdUsuario", SqlDbType.Int)
                                            , new SqlMetaData("FechaHoraLocal", SqlDbType.DateTime)
                                            , new SqlMetaData("IdUsuarioCancelo", SqlDbType.Int)
                                            , new SqlMetaData("FechaHoraCancelLocal", SqlDbType.DateTime)
                                            );

                foreach (FacturasCab item in this)
                {

                    sqlDataRecord.SetInt64(0, item.IdFacturaCab);
                    sqlDataRecord.SetByte(1, item.IdEmpresa);
                    sqlDataRecord.SetInt64(2, item.BookingID);
                    sqlDataRecord.SetDateTime(3, item.FechaHoraExpedicion);
                    sqlDataRecord.SetString(4, item.TipoFacturacion);
                    sqlDataRecord.SetString(5, item.Version);
                    sqlDataRecord.SetString(6, item.Serie);
                    sqlDataRecord.SetInt64(7, item.FolioFactura);
                    sqlDataRecord.SetString(8, item.UUID);
                    sqlDataRecord.SetString(9, item.TransactionID);
                    sqlDataRecord.SetInt64(10, item.IdPeticionPAC);
                    sqlDataRecord.SetString(11, item.Estatus);
                    sqlDataRecord.SetString(12, item.RfcEmisor);
                    sqlDataRecord.SetString(13, item.RazonSocialEmisor);
                    sqlDataRecord.SetString(14, item.NoCertificado);
                    sqlDataRecord.SetString(15, item.IdRegimenFiscal);
                    sqlDataRecord.SetString(16, item.RfcReceptor);
                    sqlDataRecord.SetString(17, item.RazonSocialReceptor);
                    sqlDataRecord.SetString(18, item.EmailReceptor);
                    sqlDataRecord.SetBoolean(19, item.EsExtranjero);
                    sqlDataRecord.SetString(20, item.IdPaisResidenciaFiscal);
                    sqlDataRecord.SetString(21, item.NumRegIdTrib);
                    sqlDataRecord.SetString(22, item.UsoCFDI);
                    sqlDataRecord.SetString(23, item.FormaPago);
                    sqlDataRecord.SetString(24, item.MetodoPago);
                    sqlDataRecord.SetString(25, item.TipoComprobante);
                    sqlDataRecord.SetString(26, item.LugarExpedicion);
                    sqlDataRecord.SetString(27, item.CondicionesPago);
                    sqlDataRecord.SetString(28, item.Moneda);
                    sqlDataRecord.SetDecimal(29, item.TipoCambio);
                    sqlDataRecord.SetDecimal(30, item.SubTotal);
                    sqlDataRecord.SetDecimal(31, item.Descuento);
                    sqlDataRecord.SetDecimal(32, item.Total);
                    sqlDataRecord.SetDecimal(33, item.MontoTarifa);
                    sqlDataRecord.SetDecimal(34, item.MontoServAdic);
                    sqlDataRecord.SetDecimal(35, item.MontoTUA);
                    sqlDataRecord.SetDecimal(36, item.MontoOtrosCargos);
                    sqlDataRecord.SetDecimal(37, item.MontoIVA);
                    sqlDataRecord.SetInt32(38, item.IdAgente);
                    sqlDataRecord.SetInt32(39, item.IdUsuario);
                    sqlDataRecord.SetDateTime(40, item.FechaHoraLocal);
                    sqlDataRecord.SetInt32(41, item.IdUsuarioCancelo);

                    if(item.FechaHoraCancelLocal.Year>1950)
                        sqlDataRecord.SetDateTime(42, item.FechaHoraCancelLocal);                    

                    yield return sqlDataRecord;
                }
            }
        }
    }
}

using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.NotaCredito
{
    public class NotasCreditoCab:ENTNotascreditoCab
    {
        public class UtNotasCreditoCab : List<NotasCreditoCab>, IEnumerable<SqlDataRecord>
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlDataRecord = new SqlDataRecord(
                                              new SqlMetaData("IdNotaCreditoCab", SqlDbType.BigInt)
                                            , new SqlMetaData("IdEmpresa", SqlDbType.TinyInt)
                                            , new SqlMetaData("BookingID", SqlDbType.BigInt)
                                            , new SqlMetaData("FechaHoraExpedicion", SqlDbType.DateTime)
                                            , new SqlMetaData("TipoFacturacion", SqlDbType.VarChar, 2)
                                            , new SqlMetaData("Version", SqlDbType.VarChar, 4)
                                            , new SqlMetaData("Serie", SqlDbType.VarChar, 3)
                                            , new SqlMetaData("FolioFiscal", SqlDbType.BigInt)
                                            , new SqlMetaData("UUID", SqlDbType.VarChar, 40)
                                            , new SqlMetaData("TransactionID", SqlDbType.VarChar, 50)
                                            , new SqlMetaData("IdPeticionPAC", SqlDbType.BigInt)
                                            , new SqlMetaData("Estatus", SqlDbType.VarChar, 1)
                                            , new SqlMetaData("RfcReceptor", SqlDbType.VarChar, 13)
                                            , new SqlMetaData("EmailReceptor", SqlDbType.VarChar, 256)
                                            , new SqlMetaData("EsExtranjero", SqlDbType.Bit)
                                            , new SqlMetaData("IdPaisResidenciaFiscal", SqlDbType.VarChar, 3)
                                            , new SqlMetaData("NumRegIdTrib", SqlDbType.VarChar, 40)
                                            , new SqlMetaData("UsoCFDI", SqlDbType.VarChar, 3)
                                            , new SqlMetaData("FormaPago", SqlDbType.VarChar, 2)
                                            , new SqlMetaData("MetodoPago", SqlDbType.VarChar, 3)
                                            , new SqlMetaData("TipoComprobante", SqlDbType.VarChar, 1)
                                            , new SqlMetaData("LugarExpedicion", SqlDbType.VarChar, 6)
                                            , new SqlMetaData("CondicionesPago", SqlDbType.VarChar, 1000)
                                            , new SqlMetaData("Moneda", SqlDbType.VarChar, 3)
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

                foreach (NotasCreditoCab item in this)
                {

                    sqlDataRecord.SetInt64(0, item.IdNotaCreditoCab);
                    sqlDataRecord.SetByte(1, item.IdEmpresa);
                    sqlDataRecord.SetInt64(2, item.BookingID);
                    sqlDataRecord.SetDateTime(3, item.FechaHoraExpedicion);
                    sqlDataRecord.SetString(4, item.TipoFacturacion);
                    sqlDataRecord.SetString(5, item.Version);
                    sqlDataRecord.SetString(6, item.Serie);
                    sqlDataRecord.SetInt64(7, item.FolioFiscal);
                    sqlDataRecord.SetString(8, item.UUID);
                    sqlDataRecord.SetString(9, item.TransactionID);
                    sqlDataRecord.SetInt64(10, item.IdPeticionPAC);
                    sqlDataRecord.SetString(11, item.Estatus);
                    sqlDataRecord.SetString(12, item.RfcReceptor);
                    sqlDataRecord.SetString(13, item.EmailReceptor);
                    sqlDataRecord.SetBoolean(14, item.EsExtranjero);
                    sqlDataRecord.SetString(15, item.IdPaisResidenciaFiscal);
                    sqlDataRecord.SetString(16, item.NumRegIdTrib);
                    sqlDataRecord.SetString(17, item.UsoCFDI);
                    sqlDataRecord.SetString(18, item.FormaPago);
                    sqlDataRecord.SetString(19, item.MetodoPago);
                    sqlDataRecord.SetString(20, item.TipoComprobante);
                    sqlDataRecord.SetString(21, item.LugarExpedicion);
                    sqlDataRecord.SetString(22, item.CondicionesPago);
                    sqlDataRecord.SetString(23, item.Moneda);
                    sqlDataRecord.SetDecimal(24, item.TipoCambio);
                    sqlDataRecord.SetDecimal(25, item.SubTotal);
                    sqlDataRecord.SetDecimal(26, item.Descuento);
                    sqlDataRecord.SetDecimal(27, item.Total);
                    sqlDataRecord.SetDecimal(28, item.MontoTarifa);
                    sqlDataRecord.SetDecimal(29, item.MontoServAdic);
                    sqlDataRecord.SetDecimal(30, item.MontoTUA);
                    sqlDataRecord.SetDecimal(31, item.MontoOtrosCargos);
                    sqlDataRecord.SetDecimal(32, item.MontoIVA);
                    sqlDataRecord.SetInt32(33, item.IdAgente);
                    sqlDataRecord.SetInt32(34, item.IdUsuario);
                    sqlDataRecord.SetDateTime(35, item.FechaHoraLocal);
                    sqlDataRecord.SetInt32(36, item.IdUsuarioCancelo);                    

                    if (item.FechaHoraCancelLocal.Year > 1950)
                        sqlDataRecord.SetDateTime(37, item.FechaHoraCancelLocal);

                    yield return sqlDataRecord;
                }
            }
        }
    }
}

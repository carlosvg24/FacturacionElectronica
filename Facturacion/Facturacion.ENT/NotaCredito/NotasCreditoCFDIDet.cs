using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.NotaCredito
{
    public class NotasCreditoCFDIDet:ENTNotascreditocfdiDet
    {
        public class UtNotasCreditoCFDIDet : List<NotasCreditoCFDIDet>, IEnumerable<SqlDataRecord>
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlDataRecord = new SqlDataRecord(
                                              new SqlMetaData("IdNotaCreditoCab", SqlDbType.BigInt)
                                            , new SqlMetaData("TransaccionID", SqlDbType.VarChar, 100)
                                            , new SqlMetaData("CFDI", SqlDbType.VarChar)
                                            , new SqlMetaData("CadenaOriginal", SqlDbType.VarChar)
                                            , new SqlMetaData("FechaTimbrado", SqlDbType.DateTime)
                                            , new SqlMetaData("FechaHoraLocal", SqlDbType.DateTime)
                                            );

                foreach (NotasCreditoCFDIDet item in this)
                {
                    sqlDataRecord.SetInt64(0, item.IdNotaCreditoCab);
                    sqlDataRecord.SetString(1, item.TransaccionID);
                    sqlDataRecord.SetString(2, item.CFDI);
                    sqlDataRecord.SetString(3, item.CadenaOriginal);
                    sqlDataRecord.SetDateTime(4, item.FechaTimbrado);
                    sqlDataRecord.SetDateTime(5, item.FechaHoraLocal);

                    yield return sqlDataRecord;
                }
            }
        }
    }
}

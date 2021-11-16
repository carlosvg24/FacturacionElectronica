using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.NotaCredito
{
    public class NotasCreditoCargo : ENTNotascreditocargosDet
    {
        public class UtNotasCreditoCargo : List<NotasCreditoCargo>, IEnumerable<SqlDataRecord>
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlDataRecord = new SqlDataRecord(
                   new SqlMetaData("IdNotaCreditoCab", SqlDbType.BigInt),
                   new SqlMetaData("IdNotaCreditoDet", SqlDbType.Int),
                   new SqlMetaData("CodigoCargo", SqlDbType.VarChar, 3),
                   new SqlMetaData("Importe", SqlDbType.Money),
                   new SqlMetaData("FechaHoraLocal", SqlDbType.DateTime)
                   );

                foreach (NotasCreditoCargo item in this)
                {
                    sqlDataRecord.SetInt64(0, item.IdNotaCreditoCab);
                    sqlDataRecord.SetInt32(1, item.IdNotaCreditoDet);
                    sqlDataRecord.SetString(2, item.CodigoCargo);
                    sqlDataRecord.SetDecimal(3, item.Importe);
                    sqlDataRecord.SetDateTime(4, item.FechaHoraLocal);

                    yield return sqlDataRecord;
                }
            }
        }
    }
}

using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.NotaCredito
{
    public class GlobalTickets_Det:ENTGlobalticketsDet
    {
        public class UtGlobalticketsDet : List<GlobalTickets_Det>, IEnumerable<SqlDataRecord>
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlDataRecord = new SqlDataRecord(
                    new SqlMetaData("IdFacturaCab", SqlDbType.BigInt),
                    new SqlMetaData("IdFacturaDet", SqlDbType.Int),
                    new SqlMetaData("BookingID", SqlDbType.BigInt),
                    new SqlMetaData("PaymentID", SqlDbType.BigInt),
                    new SqlMetaData("IdNotaCredito", SqlDbType.BigInt),
                    new SqlMetaData("IdFacturaCabCliente", SqlDbType.BigInt),
                    new SqlMetaData("FechaHoraLocal", SqlDbType.DateTime)
                    );

                foreach (GlobalTickets_Det item in this)
                {

                    sqlDataRecord.SetInt64(0, item.IdFacturaCab);
                    sqlDataRecord.SetInt32(1, item.IdFacturaDet);
                    sqlDataRecord.SetInt64(2, item.BookingID);
                    sqlDataRecord.SetInt64(3, item.PaymentID);
                    sqlDataRecord.SetInt64(4, item.IdNotaCredito);
                    sqlDataRecord.SetInt64(5, item.IdFacturaCabCliente);
                    sqlDataRecord.SetDateTime(6, item.FechaHoraLocal);

                    yield return sqlDataRecord;
                }

            }
        }
    }
}

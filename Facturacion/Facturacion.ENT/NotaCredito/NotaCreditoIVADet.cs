using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.NotaCredito
{
    public class NotaCreditoIVADet: ENTNotascreditoivaDet
    {
        public class UtNotasCreditoivaDet : List<NotaCreditoIVADet>, IEnumerable<SqlDataRecord>
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlDataRecord = new SqlDataRecord(
                    new SqlMetaData("IdNotaCreditoCab", SqlDbType.BigInt),
                    new SqlMetaData("IdNotaCreditoDet", SqlDbType.Int),
                    new SqlMetaData("TipoFactor", SqlDbType.VarChar, 10),                    
                    new SqlMetaData("Base", SqlDbType.Money),
                    new SqlMetaData("Impuesto", SqlDbType.VarChar, 3),
                    new SqlMetaData("TasaOCuota", SqlDbType.Decimal, 10, 6),
                    new SqlMetaData("Importe", SqlDbType.Money),
                    new SqlMetaData("FechaHoraLocal", SqlDbType.DateTime)
                    );

                foreach (NotaCreditoIVADet item in this)
                {

                    sqlDataRecord.SetInt64(0, item.IdNotaCreditoCab);
                    sqlDataRecord.SetInt32(1, item.IdNotaCreditoDet);
                    sqlDataRecord.SetString(2, item.TipoFactor);                    
                    sqlDataRecord.SetDecimal(3, item.Base);
                    sqlDataRecord.SetString(4, item.Impuesto);
                    sqlDataRecord.SetDecimal(5, item.TasaOCuota);
                    sqlDataRecord.SetDecimal(6, item.Importe);
                    sqlDataRecord.SetDateTime(7, item.FechaHoraLocal);

                    yield return sqlDataRecord;
                }
            }
        }
    }
}

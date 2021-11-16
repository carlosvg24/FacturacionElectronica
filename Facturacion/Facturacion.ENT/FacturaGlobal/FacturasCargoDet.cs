using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.FacturaGlobal
{
    public class FacturasCargoDet:ENTFacturascargosDet
    {
        public class UtFacturasCargoDet : List<FacturasCargoDet>, IEnumerable<SqlDataRecord>
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlDataRecord = new SqlDataRecord(
                    new SqlMetaData("IdFacturaCab", SqlDbType.BigInt),
                    new SqlMetaData("IdFacturaDet", SqlDbType.Int),
                    new SqlMetaData("CodigoCargo", SqlDbType.VarChar, 3),
                    new SqlMetaData("Importe", SqlDbType.Money),
                    new SqlMetaData("EsTua", SqlDbType.Bit),
                    new SqlMetaData("FechaHoraLocal", SqlDbType.DateTime)
                    );

                foreach (FacturasCargoDet item in this)
                {
                    sqlDataRecord.SetInt64(0, item.IdFacturaCab);
                    sqlDataRecord.SetInt32(1, item.IdFacturaDet);
                    sqlDataRecord.SetString(2, item.CodigoCargo);
                    sqlDataRecord.SetDecimal(3, item.Importe);
                    sqlDataRecord.SetBoolean(4, item.EsTua);
                    sqlDataRecord.SetDateTime(5, item.FechaHoraLocal);                    

                    yield return sqlDataRecord;
                }
            }
        }

    }
}

using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.FacturaGlobal
{
    public class FacturaIVADet:ENTFacturasivaDet
    {
        public class UtFacturasivaDet : List<FacturaIVADet>, IEnumerable<SqlDataRecord>
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlDataRecord = new SqlDataRecord(
                    new SqlMetaData("IdFacturaCab", SqlDbType.BigInt),
                    new SqlMetaData("IdFacturaDet", SqlDbType.Int),
                    new SqlMetaData("TipoFactor", SqlDbType.VarChar,10),
                    new SqlMetaData("TasaOCuota", SqlDbType.Decimal,10,6),
                    new SqlMetaData("Base", SqlDbType.Money),
                    new SqlMetaData("Impuesto", SqlDbType.VarChar,3),
                    new SqlMetaData("Importe", SqlDbType.Money),
                    new SqlMetaData("FechaHoraLocal", SqlDbType.DateTime)
                    );

                foreach (FacturaIVADet item in this)
                {

                    sqlDataRecord.SetInt64(0, item.IdFacturaCab);
                    sqlDataRecord.SetInt32(1, item.IdFacturaDet);
                    sqlDataRecord.SetString(2, item.TipoFactor);
                    sqlDataRecord.SetDecimal(3, item.TasaOCuota);
                    sqlDataRecord.SetDecimal(4, item.Base);
                    sqlDataRecord.SetString(5, item.Impuesto);
                    sqlDataRecord.SetDecimal(6, item.Importe);
                    sqlDataRecord.SetDateTime(7, item.FechaHoraLocal);

                    yield return sqlDataRecord;
                }
            }
        }
    }
}

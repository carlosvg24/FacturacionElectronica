using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.FacturaGlobal
{
    public class FacturasDet:ENTFacturasDet
    {
        public class UtFacturasDet : List<FacturasDet>, IEnumerable<SqlDataRecord>
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlDataRecord = new SqlDataRecord(
                    new SqlMetaData("IdFacturaCab", SqlDbType.BigInt),
                    new SqlMetaData("IdFacturaDet", SqlDbType.Int),
                    new SqlMetaData("ClaveProdServ", SqlDbType.VarChar,10),
                    new SqlMetaData("NoIdentificacion", SqlDbType.VarChar,100),
                    new SqlMetaData("Cantidad", SqlDbType.Int),
                    new SqlMetaData("ClaveUnidad", SqlDbType.VarChar,3),
                    new SqlMetaData("Unidad", SqlDbType.VarChar,20),
                    new SqlMetaData("Descripcion", SqlDbType.VarChar,1000),
                    new SqlMetaData("ValorUnitario", SqlDbType.Money),
                    new SqlMetaData("Importe", SqlDbType.Money),
                    new SqlMetaData("Descuento", SqlDbType.Money),
                    new SqlMetaData("FechaHoraLocal", SqlDbType.DateTime)
                    );

                foreach (FacturasDet item in this)
                {

                    sqlDataRecord.SetInt64(0, item.IdFacturaCab);
                    sqlDataRecord.SetInt32(1, item.IdFacturaDet);
                    sqlDataRecord.SetString(2, item.ClaveProdServ);
                    sqlDataRecord.SetString(3, item.NoIdentificacion);
                    sqlDataRecord.SetInt32(4, item.Cantidad);
                    sqlDataRecord.SetString(5, item.ClaveUnidad);
                    sqlDataRecord.SetString(6, item.Unidad);
                    sqlDataRecord.SetString(7, item.Descripcion);
                    sqlDataRecord.SetDecimal(8, item.ValorUnitario);
                    sqlDataRecord.SetDecimal(9, item.Importe);
                    sqlDataRecord.SetDecimal(10, item.Descuento);
                    sqlDataRecord.SetDateTime(11, item.FechaHoraLocal);

                    yield return sqlDataRecord;
                }
            }
        }
    }
}

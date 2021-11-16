using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.Comun
{
    public class StoredProcedureValidation
    {
        public string NameSp { get; set; }

        public bool IsExist { get; set; }

        public class UtStoredProcedureValidation : List<StoredProcedureValidation>, IEnumerable<SqlDataRecord>
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                var sqlDataRecord = new SqlDataRecord(
                                             new SqlMetaData("NameSp", SqlDbType.VarChar,100),
                                             new SqlMetaData("IsExist", SqlDbType.Bit)
                                             );

                foreach (StoredProcedureValidation item in this)
                {

                    sqlDataRecord.SetString(0, item.NameSp);
                    sqlDataRecord.SetBoolean(1, item.IsExist);

                    yield return sqlDataRecord;
                }
            }
        }
    }
}

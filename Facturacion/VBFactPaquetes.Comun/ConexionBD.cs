using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VBFactPaquetes.Comun.Log;
using VBFactPaquetes.Comun.Security;

namespace VBFactPaquetes.Comun
{
    /// <summary>
    /// Métodos para conectar a la BD
    /// </summary>
    public class ConexionBD
    {
        private SqlConnection conSQL = new SqlConnection();
        private OdbcConnection conOdbc = new OdbcConnection();
        private Encrypt encrypt = new Encrypt();
        private String cnn = String.Empty;

        public SqlConnection Conexion()
        {
            try
            {
                cnn = encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBVivaPaquetes"].ToString());
                conSQL.ConnectionString = cnn;
                return conSQL;
            }
            catch (Exception ex)
            {
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      null,
                                      ex, Excepciones.TipoPortal.Facturacion);
            }
        }

        public SqlConnection ConexionNavitaireWB()
        {
            try
            {
                cnn = encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString());
                conSQL.ConnectionString = cnn;
                return conSQL;
            }
            catch (Exception ex)
            {
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      null,
                                      ex, Excepciones.TipoPortal.Facturacion);
            }
        }

        /// <summary>
        /// Abre la conexión a la BD
        /// </summary>
        /// <param name="con"></param>
        public void openSQLConnection(SqlConnection con)
        {
            con.ConnectionString = cnn;
            con.Open();
        }

        /// <summary>
        /// Cierra la conexión a la BD
        /// </summary>
        /// <param name="con"></param>
        public void closeSQLConnection(SqlConnection con)
        {
            con.Close();
            con.Dispose();
        }

    }
}

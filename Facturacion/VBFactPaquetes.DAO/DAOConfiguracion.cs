using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VBFactPaquetes.Comun;
using VBFactPaquetes.Comun.Log;

namespace VBFactPaquetes.DAO
{
    /// <summary>
    /// Consulta, modifica, inserta el catálogo de configuraciones
    /// </summary>
    public class DAOConfiguracion
    {
        DataTable dtResultado;
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        ConexionBD connection = new ConexionBD();
        SqlDataAdapter adapter = new SqlDataAdapter();


        /// <summary>
        /// Obtiene los valores de las configuraciones
        /// </summary>
        /// <param name="accion"></param>
        /// <returns></returns>
        public DataTable ConsultarConfiguracion(String accion)
        {
            try
            {
                dtResultado = new DataTable();
                con = connection.Conexion();

                cmd = new SqlCommand("[Facturacion].[SPVBFactPaqConfiguracion]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Accion", SqlDbType.VarChar, 15).Value = accion;
                cmd.Parameters.Add("@IdConfiguracion", SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@TipoFact", SqlDbType.VarChar, 10).Value = "";
                cmd.Parameters.Add("@TipoParametro", SqlDbType.VarChar, 1000).Value = "";
                cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 1000).Value = "";
                cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 1000).Value = "";
                cmd.Parameters.Add("@Valor", SqlDbType.VarChar, 1000).Value = "";

                adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                connection.openSQLConnection(con);
                adapter.Fill(dtResultado);

                return dtResultado;

            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("accion", accion);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.Facturacion);
            }
            finally
            {
                connection.closeSQLConnection(con);
                cmd.Dispose();
            }
        }
    }
}

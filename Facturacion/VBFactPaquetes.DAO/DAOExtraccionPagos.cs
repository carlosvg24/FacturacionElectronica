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
    public class DAOExtraccionPagos
    {
        private DataSet dsResultado;
        private SqlConnection con = new SqlConnection();
        private SqlCommand cmd = new SqlCommand();
        private ConexionBD connection = new ConexionBD();
        private SqlDataAdapter adapter = new SqlDataAdapter();

        public DataSet InsertarPagos(DataTable dtReserva, DataTable dtFees, DataTable dtPagos)
        {
            con = new SqlConnection();
            dsResultado = new DataSet();

            try
            {
                con = connection.Conexion();

                cmd = new SqlCommand("[Facturacion].[SPVBFactPaqInsPagosDeJuniper]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParReserva = cmd.Parameters.AddWithValue("@Reserva", dtReserva);
                sqlParReserva.SqlDbType = SqlDbType.Structured;
                SqlParameter sqlParFees = cmd.Parameters.AddWithValue("@Fees", dtFees);
                sqlParFees.SqlDbType = SqlDbType.Structured;
                SqlParameter sqlParPagos = cmd.Parameters.AddWithValue("@Pagos", dtPagos);
                sqlParPagos.SqlDbType = SqlDbType.Structured;

                adapter.SelectCommand = cmd;
                connection.openSQLConnection(con);
                adapter.Fill(dsResultado);
            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("dtReserva", dtReserva);
                parametros.Add("dtFees", dtFees);
                parametros.Add("dtPagos", dtPagos);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }
            finally
            {
                connection.closeSQLConnection(con);
                cmd.Dispose();
            }

            return dsResultado;
        }

        public DataSet ActualizarReserva(DataTable dtReserva, DataTable dtPagos, DataTable dtFees)
        {
            con = new SqlConnection();
            dsResultado = new DataSet();

            try
            {
                con = connection.Conexion();

                cmd = new SqlCommand("[Facturacion].[SPVBFactPaqActPagosDeJuniper]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParReserva = cmd.Parameters.AddWithValue("@Reserva", dtReserva);
                sqlParReserva.SqlDbType = SqlDbType.Structured;
                SqlParameter sqlParFees = cmd.Parameters.AddWithValue("@Fees", dtFees);
                sqlParFees.SqlDbType = SqlDbType.Structured;
                SqlParameter sqlParPagos = cmd.Parameters.AddWithValue("@Pagos", dtPagos);
                sqlParPagos.SqlDbType = SqlDbType.Structured;

                adapter.SelectCommand = cmd;
                connection.openSQLConnection(con);
                adapter.Fill(dsResultado);
            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("dtReserva", dtReserva);
                parametros.Add("dtFees", dtFees);
                parametros.Add("dtPagos", dtPagos);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }
            finally
            {
                connection.closeSQLConnection(con);
                cmd.Dispose();
            }

            return dsResultado;
        }

    }
}

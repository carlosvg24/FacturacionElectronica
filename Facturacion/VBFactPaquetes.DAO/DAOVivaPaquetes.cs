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
    public class DAOVivaPaquetes
    {
        private DataTable dtResultado;
        private SqlConnection con = new SqlConnection();
        private SqlCommand cmd = new SqlCommand();
        private ConexionBD connection = new ConexionBD();
        private SqlDataAdapter adapter = new SqlDataAdapter();

        public DataTable SeLocalizaReserva(string bookingId)
        {
            con = new SqlConnection();
            dtResultado = new DataTable();

            try
            {
                con = connection.Conexion();

                cmd = new SqlCommand("[Facturacion].[SPVBFactPaqEstatusReserva]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParReserva = cmd.Parameters.AddWithValue("@BoookingId", bookingId);
                sqlParReserva.SqlDbType = SqlDbType.VarChar;
                sqlParReserva.Size = 8;

                adapter.SelectCommand = cmd;
                connection.openSQLConnection(con);
                adapter.Fill(dtResultado);


            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("bookingId", bookingId);
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

            return dtResultado;
        }

        public DataSet ObtenerDatosParaGenerarNC(string bookingId, string paymentId)
        {
            con = new SqlConnection();
            DataSet dsResultado = new DataSet();

            try
            {
                con = connection.Conexion();

                cmd = new SqlCommand("[Facturacion].[SPVBFactPaqObtDatosParaGenerarNC]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter sqlParReserva = cmd.Parameters.AddWithValue("@BoookingId", bookingId);
                sqlParReserva.SqlDbType = SqlDbType.VarChar;
                sqlParReserva.Size = 8;
                SqlParameter sqlParPagos = cmd.Parameters.AddWithValue("@PaymentId", paymentId);
                sqlParPagos.SqlDbType = SqlDbType.VarChar;
                sqlParPagos.Size = 50;

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
                parametros.Add("bookingId", bookingId);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBFactPaquetes.Comun;
using VBFactPaquetes.Model.Facturacion;

namespace VBFactPaquetes.DAO.Facturacion
{
    public class DAOFacturaGlobal
    {
        DataSet dsRes;
        SqlConnection con;
        SqlDataAdapter adapter;
        ConexionBD connection;
        SqlCommand cmd;

        public DataSet ConsultaConceptosGlobal(int anio, int mes, String CodigoTC, String CodigoMoneda)
        {
            try
            {

                dsRes = new DataSet();
                con = new SqlConnection();
                adapter = new SqlDataAdapter();
                connection = new ConexionBD();

                con = connection.Conexion();

                cmd = new SqlCommand("[Facturacion].[SPVBFactPaqConsultaGlobal]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TipoComprobante", SqlDbType.VarChar, 1).Value = CodigoTC;
                cmd.Parameters.Add("@CodigoMoneda", SqlDbType.VarChar, 3).Value = CodigoMoneda;
                cmd.Parameters.Add("@MesPago", SqlDbType.Int).Value = mes;
                cmd.Parameters.Add("@AnioPago", SqlDbType.Int).Value = anio;

                adapter.SelectCommand = cmd;

                connection.openSQLConnection(con);

                adapter.Fill(dsRes);

                return dsRes;

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                connection.closeSQLConnection(con);
                cmd.Dispose();
            }
        }

        /*INSERTAR DATOS EN TABLA PAGOS Y FACTURA*/
        public DataTable ActualizarFactura(FacturaGlobal globalDTO, String accion)
        {
            try
            {
                con = new SqlConnection();
                adapter = new SqlDataAdapter();
                connection = new ConexionBD();
                DataTable dtResultado = new DataTable();

                con = connection.Conexion();

                cmd = new SqlCommand("[Facturacion].[SPVBFactPaqGlobal]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Accion", SqlDbType.VarChar, 5).Value = accion;
                cmd.Parameters.Add("@IdFactPaqGlobal", SqlDbType.BigInt).Value = globalDTO.IdFactPaqGlobal;
                cmd.Parameters.Add("@UUID", SqlDbType.VarChar, 100).Value = globalDTO.UUID;
                cmd.Parameters.Add("@CadenaOriginal", SqlDbType.VarChar, 1000).Value = globalDTO.CadenaOriginal;
                cmd.Parameters.Add("@FechaTimbrado", SqlDbType.DateTime).Value = globalDTO.FechaTimbrado;
                cmd.Parameters.Add("@XMLResponse", SqlDbType.VarChar).Value = globalDTO.XMLResponse;
                cmd.Parameters.Add("@ArchivoPDF", SqlDbType.VarBinary).Value = globalDTO.ArchivoPDF;

                adapter = new SqlDataAdapter();

                adapter.SelectCommand = cmd;

                connection.openSQLConnection(con);

                adapter.Fill(dtResultado);

                return dtResultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.closeSQLConnection(con);
                cmd.Dispose();
            }
        }
    }
}

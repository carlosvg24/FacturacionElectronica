using Comun.Security;
using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.DAL.ProcesoFacturacion
{
    public class DALFacturaGlobal33
    {
        #region Propiedades Privadas
        protected SqlConnection _conexion;

        #endregion Propiedades Privadas

        #region Constructores
        public DALFacturaGlobal33(SqlConnection conexion)
        {
            _conexion = conexion;
        }

        #endregion Constructores

        public SqlConnection ConexionFacturacion
        {
            get
            {
                Encrypt encrypt = new Encrypt();
                SqlConnection _conexion = new SqlConnection();
                String conString = "";
                conString = encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString());
                _conexion.ConnectionString = conString;
                return _conexion;
            }
        }


        public List<ENTParametrosCnf> RecuperarParametros()
        {
            List<ENTParametrosCnf> result = new List<ENTParametrosCnf>();
            DataTable dtResultado = new DataTable();
            SqlCommand cmm = new SqlCommand();
            SqlConnection cnxFact = new SqlConnection();
            cnxFact = ConexionFacturacion;
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_GetParametros";
            cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
            cmm.Connection = cnxFact;
            bool cerrarConexion = false;

            try
            {
                if (cnxFact.State.Equals(ConnectionState.Closed))
                {
                    cnxFact.Open();
                    cerrarConexion = true;
                }
                SqlDataAdapter da = new SqlDataAdapter(cmm);
                DataSet dsResult = new DataSet();
                da.Fill(dsResult, "dtResultado");
                dtResultado = dsResult.Tables["dtResultado"];
                foreach (DataRow dr in dtResultado.Rows)
                {
                    ENTParametrosCnf item = new ENTParametrosCnf();
                    if (!dr.IsNull("IdParametro")) item.IdParametro = Convert.ToInt32(dr["IdParametro"]);
                    if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
                    if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
                    if (!dr.IsNull("Valor")) item.Valor = dr["Valor"].ToString();
                    if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
                    if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
                    result.Add(item);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmm.Dispose();
                if (cerrarConexion)
                {
                    cnxFact.Close();
                }
            }
            return result;
        }




        public DataSet RecuperarPagosFacturaGlobal(DateTime fechaInicial, DateTime fechaFinal, bool flg_Tercero)
        {

            DataSet dsResultado = new DataSet();
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@FechaIni_P", SqlDbType.Date);
            param0.Value = fechaInicial.ToString("yyyy-MM-dd");
            commandParameters.Add(param0);

            SqlParameter param1 = new SqlParameter("@FechaFin_P", SqlDbType.Date);
            param1.Value = fechaFinal.ToString("yyyy-MM-dd");
            commandParameters.Add(param1);

          
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            if(flg_Tercero)
            {
                cmm.CommandText = "uspFac_Proc_ExtraccionFG_Paquetes";
            }
            else
            {
                cmm.CommandText = "uspFac_Proc_ExtraccionFG";
            }
            cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
            cmm.Connection = ConexionFacturacion;
            bool cerrarConexion = false;
            foreach (SqlParameter p in commandParameters)
            {
                cmm.Parameters.Add(p);
            }

            try
            {
                if (_conexion.State.Equals(ConnectionState.Closed))
                {
                    _conexion.Open();
                    cerrarConexion = true;
                }
                SqlDataAdapter da = new SqlDataAdapter(cmm);
                da.Fill(dsResultado, "dtResultado");

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmm.Dispose();
                if (cerrarConexion)
                {
                    _conexion.Close();
                }
            }
            return dsResultado;
        }

        public bool AsignarEsFacturableGlobal(DateTime fechaInicial, DateTime fechaFinal)
        {
            DataSet dsResultado = new DataSet();

            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@FechaIni", SqlDbType.Date);
            param0.Value = fechaInicial.ToString("yyyy-MM-dd");
            commandParameters.Add(param0);

            SqlParameter param1 = new SqlParameter("@FechaFin", SqlDbType.Date);
            param1.Value = fechaFinal.ToString("yyyy-MM-dd");
            commandParameters.Add(param1);


            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_AsignarEsFacturableGlobal";
            cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
            cmm.Connection = ConexionFacturacion;
            bool cerrarConexion = false;
            foreach (SqlParameter p in commandParameters)
            {
                cmm.Parameters.Add(p);
            }

            try
            {
                if (_conexion.State.Equals(ConnectionState.Closed))
                {
                    _conexion.Open();
                    cerrarConexion = true;
                }
                SqlDataAdapter da = new SqlDataAdapter(cmm);
                da.Fill(dsResultado, "dtResultado");

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmm.Dispose();
                if (cerrarConexion)
                {
                    _conexion.Close();
                }
            }
            return true;
        }
    }
}

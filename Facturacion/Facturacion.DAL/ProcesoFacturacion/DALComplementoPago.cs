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
    public class DALComplementoPago
    {
        #region Propiedades Privadas
        protected SqlConnection _conexion;

        #endregion Propiedades Privadas

        #region Constructores
        public DALComplementoPago(SqlConnection conexion)
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


        protected DataTable RecuperarReservasPPDPorFiltros(string pnr, string codigoOrgPPD, DateTime FechaIni, DateTime FechaFin,string estatusPago)
        {

            DataTable dtResultado = new DataTable();
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@PNR", SqlDbType.VarChar, 6);
            param0.Value = pnr;
            commandParameters.Add(param0);
            SqlParameter param1 = new SqlParameter("@FECHAINI", SqlDbType.VarChar, 10);
            param1.Value = FechaIni.Year > 1900 ? FechaIni.ToString("yyyy-MM-dd") : "";
            commandParameters.Add(param1);
            SqlParameter param2 = new SqlParameter("@FECHAFIN", SqlDbType.VarChar, 10);
            param2.Value = FechaFin.Year > 1900 ? FechaFin.ToString("yyyy-MM-dd") : "";
            commandParameters.Add(param2);
            SqlParameter param3 = new SqlParameter("@LISTAORG", SqlDbType.VarChar, 500);
            param3.Value = codigoOrgPPD;
            commandParameters.Add(param3);
            SqlParameter param4 = new SqlParameter("@STATUSPAGO", SqlDbType.VarChar, 10);
            param4.Value = estatusPago;
            commandParameters.Add(param4);

            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "[Fnz].usp_VB_FNZ_VBFac_GetReservasPPDPorFiltros";
            cmm.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
            cmm.Connection = _conexion;
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
                DataSet dsResult = new DataSet();
                da.Fill(dsResult, "dtResultado");
                dtResultado = dsResult.Tables["dtResultado"];

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
            return dtResultado;
        }

    }
}

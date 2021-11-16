using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.DAL.ProcesoFacturacion
{
    public class DALDistribucionPagos
    {
        #region Propiedades Privadas
        protected SqlConnection _conexion;

        #endregion Propiedades Privadas

        #region Constructores
        public DALDistribucionPagos(SqlConnection conexion)
        {
            _conexion = conexion;
        }
        #endregion Constructores



        public DataSet RecuperarReservacionPorPNR(string pnr)
        {

            DataSet dtResultado = new DataSet();
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@PNR", SqlDbType.VarChar, 6);
            param0.Value = pnr;
            commandParameters.Add(param0);
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "[Fnz].usp_VB_FNZ_VBFac_GetInformacionPNR_v2";
            cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
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
                da.Fill(dsResult);
                dtResultado = dsResult;
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

        public DataTable RecuperarEmailPorPNR(string pnr)
        {
            DataTable dtResultado = new DataTable();
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@PNR", SqlDbType.VarChar, 6);
            param0.Value = pnr;
            commandParameters.Add(param0);
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "[Fnz].usp_VB_FNZ_VBFac_GetEmailPorPNR";
            cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
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
                da.Fill(dsResult);
                if(dsResult != null && dsResult.Tables.Count > 0)
                    dtResultado = dsResult.Tables[0];
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

        public decimal RecuperarTipoCambioPorFecha(DateTime fecha)
        {
            decimal result = 0;
            DataSet dtResultado = new DataSet();
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@FECHA", SqlDbType.Date);
            param0.Value = fecha.ToString("yyyy-MM-dd");
            commandParameters.Add(param0);
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "[Fnz].usp_VB_FNZ_VBFac_GetTipoCambioPorFecha";
            cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
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
                da.Fill(dsResult);
                dtResultado = dsResult;
                result = 0;
                if (dtResultado.Tables[0].Rows.Count > 0)
                {

                    DataRow dr = dtResultado.Tables[0].Rows[0];
                    if (!dr.IsNull("Solventar"))
                    {
                        result = Math.Round(Convert.ToDecimal(dr["Solventar"].ToString()),2);
                    }
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
                    _conexion.Close();
                }
            }
            return result;
        }

        public void SincronizarPagosNav()
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_Pro_SincronizarPagosNavitaire";
            cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
            cmm.Connection = _conexion;
            bool cerrarConexion = false;

            try
            {
                if (_conexion.State.Equals(ConnectionState.Closed))
                {
                    _conexion.Open();
                    cerrarConexion = true;
                }
                cmm.ExecuteNonQuery();
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
        }


        public List<ENTDistribucionpagos> RecuperarListaPNRRemanentes()
        {
            List<ENTDistribucionpagos> result = new List<ENTDistribucionpagos>();
            DataTable dtResultado = new DataTable();
            
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_Pro_GetPagosFaltantesNav";
            cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
            cmm.Connection = _conexion;
            bool cerrarConexion = false;

            try
            {
                if (_conexion.State.Equals(ConnectionState.Closed))
                {
                    _conexion.Open();
                    cerrarConexion = true;
                }
                SqlDataAdapter da = new SqlDataAdapter(cmm);
                da.Fill(dtResultado);
                int idCont = 0;

                foreach (DataRow drPNR in dtResultado.Rows)
                {
                    idCont++;
                    ENTDistribucionpagos entDist = new ENTDistribucionpagos();
                    entDist.IdDistribucionPagos = idCont;
                    entDist.BookingID = Convert.ToInt64(drPNR["BookingID"].ToString());
                    entDist.RecordLocator = drPNR["RecordLocator"].ToString();
                    entDist.CreatedDate = (DateTime)drPNR["CreatedDate"];
                    entDist.ModifiedDate = (DateTime)drPNR["ModifiedDate"];
                    //entDist.FechaProcesamiento = DateTime.Now;
                    entDist.ProcesoExitoso = false;
                    entDist.ConDescartePorDiferencia = false;
                    entDist.MensajeError = String.Empty;
                    entDist.FechaHoraLocal = DateTime.Now;
                    result.Add(entDist);
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
                    _conexion.Close();
                }
            }
            return result;
        }


        

    }
}

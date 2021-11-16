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
    public class DALFacturacion
    {
        #region Propiedades Privadas
        protected SqlConnection _conexion;

        #endregion Propiedades Privadas

        #region Constructores
        public DALFacturacion(SqlConnection conexion)
        {
            _conexion = conexion;
        }
        #endregion Constructores

        public DataTable RecuperarPasajerosPorPNR(string pnr)
        {
            
            DataTable dtResultado = new DataTable();
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@PNR", SqlDbType.VarChar,6);
            param0.Value = pnr;
            commandParameters.Add(param0);
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "[Fnz].usp_VB_FNZ_VBFac_GetPasajerosPorPNR";
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

        public void ActualizaDesfaseSAT(int segundosDesfaseSAT)
        {
            //ClaseConexion conexion = new ClaseConexion();
            bool cerrarConexion = false;
            DataTable data = new DataTable();
            SqlCommand cmd = null;

            try
            {

                cmd = new SqlCommand("udb.uspFact_ActualizaDesfaseSAT", _conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                cmd.Parameters.Add("@NumSegundos", SqlDbType.Int).Value = segundosDesfaseSAT;
                if (_conexion.State.Equals(ConnectionState.Closed))
                {
                    _conexion.Open();
                    cerrarConexion = true;
                }
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                if (cerrarConexion)
                {
                    _conexion.Close();
                }
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


        public DataTable RecuperarDatosFacturaPorFolio(long bookingId, long folioFactura)
        {

            DataTable dtResultado = new DataTable();
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@BookingId", SqlDbType.BigInt);
            param0.Value = bookingId;
            commandParameters.Add(param0);
            SqlParameter param1 = new SqlParameter("@FolioFactura", SqlDbType.BigInt);
            param1.Value = folioFactura;
            commandParameters.Add(param1);
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "[Fnz].usp_VB_FNZ_VBFac_GetFactura32PorFolio";
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


        
        public DataTable RecuperarReservaPorPNR(string pnr)
        {

            DataTable dtResultado = new DataTable();
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@PNR", SqlDbType.VarChar,6);
            param0.Value = pnr;
            commandParameters.Add(param0);
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "[Fnz].usp_VB_FNZ_VBFac_GetReservaPorPNR";
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

        public DataTable LiberarPagoParaRefacturarPorFolioFactura(long folioFactura)
        {

            DataTable dtResultado = new DataTable();
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@FOLIOFACTURA", SqlDbType.BigInt);
            param0.Value = folioFactura;
            commandParameters.Add(param0);
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_ProLiberarPagoPorFolioFactura";
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

        public DataSet RecuperarPagosParaEnviarGlobal(DateTime fechaIni, DateTime fechaFin,string cveMoneda)
        {

            DataSet dsResultado = new DataSet();
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@FechaIni_P", SqlDbType.Date);
            param0.Value = fechaIni.Date;
            commandParameters.Add(param0);
            SqlParameter param1 = new SqlParameter("@FechaFin_P", SqlDbType.Date);
            param1.Value = fechaFin.Date;
            commandParameters.Add(param1);
            SqlParameter param2 = new SqlParameter("@FechaFin_P", SqlDbType.VarChar,3);
            param2.Value = cveMoneda;
            commandParameters.Add(param2);
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_Proc_ExtraccionFG";
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
                da.Fill(dsResult, "dtResultado");
                

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

        public DataTable GetPagosSinFacturarSinEmail()
        {
            DataTable dtResultado = new DataTable();

            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_GetPagosSinFacturarSinEmail";
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

        public long GetFolio(string PNR)
        {
            /*encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["sqlViva"].ToString())*/
            Encrypt encrypt = new Encrypt();
            using (IDbConnection db = new SqlConnection(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ConnectionString)))
            {
                db.Open();
                SqlCommand _comand = new SqlCommand("select Folio from VBFac_DetalleHotelPaquete where RecordLocator = '" + PNR + "'", db as SqlConnection);

                _comand.CommandType = CommandType.Text;
                _comand.CommandTimeout = 50000;
                IDataReader _reader = _comand.ExecuteReader();

                long result = 0;
                while (_reader.Read())
                {
                    result = _reader.GetInt64(0);
                }

                db.Close();
                return result;
            }
        }
        public int GetFolioProcesoGlobal(string TipoProceso)
        {
            /*encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["sqlViva"].ToString())*/
            Encrypt encrypt = new Encrypt();
            using (IDbConnection db = new SqlConnection(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ConnectionString)))
            {
                db.Open();
                SqlCommand _comand = new SqlCommand("select top 1 Folio from VBFac_DetalleHotelPaquete where TipoProceso = '" + TipoProceso + "'" + " and cast(FechaHoraLocal as date) = '" + DateTime.Now.AddDays(-1).ToString("yyyyMMdd") + "'", db as SqlConnection);

                _comand.CommandType = CommandType.Text;
                _comand.CommandTimeout = 50000;
                IDataReader _reader = _comand.ExecuteReader();

                int result = 0;
                while (_reader.Read())
                {
                    //result = _reader.GetInt16(0);
                    result = Convert.ToInt32(_reader["Folio"]);
                }

                db.Close();
                result = result++; //0 ? result++ : result;
                return result;
            }
        }
    }
}

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
    public class DALProcesarPagos
    {
        #region Propiedades Privadas
        protected SqlConnection _conexion;

        #endregion Propiedades Privadas

        #region Constructores
        public DALProcesarPagos(SqlConnection conexion)
        {
            _conexion = conexion;
        }
        #endregion Constructores

        public List<ENTDistribucionpagos> RecuperarPNRConPagosPorFecha(DateTime fecha, string PNR)
        {
            List<ENTDistribucionpagos> result = new List<ENTDistribucionpagos>();
            DataTable dtResultado = new DataTable();
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@FECHA", SqlDbType.VarChar, 10);
            param0.Value = fecha.ToString("yyyy-MM-dd");
            commandParameters.Add(param0);
            SqlParameter param1 = new SqlParameter("@PNR", SqlDbType.VarChar, 6);
            param1.Value = PNR;
            commandParameters.Add(param1);
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            //cmm.CommandText = "[Fnz].usp_VB_FNZ_VBFac_GetPNRPagosPorFecha";
            cmm.CommandText = "[Fnz].usp_VB_FNZ_VBFac_GetPNRPagosPorFecha_Test";
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


        public void ActualizarPagosPorFacturarNav()
        {

            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "[Fnz].usp_VB_FNZ_VBFac_InsPagosFacturablesNav";
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

    }
}

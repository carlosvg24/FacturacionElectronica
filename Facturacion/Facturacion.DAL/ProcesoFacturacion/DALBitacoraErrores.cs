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
    public class DALBitacoraErrores
    {
        #region Propiedades Privadas
        protected SqlConnection _conexion;

        #endregion Propiedades Privadas

        #region Constructores
        public DALBitacoraErrores(SqlConnection conexion)
        {
            _conexion = conexion;
        }
        #endregion Constructores

        public List<ENTParametrosCnf> RecuperarParametros()
        {
            List<ENTParametrosCnf> result = new List<ENTParametrosCnf>();
            DataTable dtResultado = new DataTable();
            SqlCommand cmm = new SqlCommand();
            

            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_GetParametros";
            cmm.Connection = _conexion;
            cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
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
                    _conexion.Close();
                }
            }
            return result;
        }


       

    }
}

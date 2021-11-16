using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.ENT;
using System.Data.SqlClient;
using System.Data;

namespace Facturacion.DAL
{
    public class DALHotel : ENTHotel
    {
        #region Propiedades Privadas
        protected SqlConnection _conexion;
        protected bool _nuevo = false;
        protected string _idUsuario;
        protected string _proceso;
        protected DALPagosCab _copia;
        #endregion Propiedades Privadas

        #region Constructores
        public DALHotel(SqlConnection conexion)
        {
            _conexion = conexion;
            IniciarPropiedades();
        }
        #endregion Constructores

        #region Iniciar Propiedades
        public void IniciarPropiedades()
        {
            _copia = null;
            idReservaCab = 0;
            pnr = string.Empty;
            superPNR = string.Empty;
            referenceID = string.Empty;
            Type = string.Empty;
            currencyCode = string.Empty;
            chargeAmount = 0;
            createdDate = new DateTime();
            tipoProceso = string.Empty;
        }
        #endregion

        #region AgregaInfoHotel
        public void AgregaInfoHotel()
        {
            _nuevo = true;
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlCommand cmm = new SqlCommand();
            SqlParameter param0 = new SqlParameter("@IdReservaCab", SqlDbType.BigInt);
            //param0.Direction = ParameterDirection.InputOutput;
            param0.Value = idReservaCab;
            commandParameters.Add(param0);
            SqlParameter param1 = new SqlParameter("@SuperPNR", SqlDbType.VarChar, 8);
            param1.Value = superPNR;
            commandParameters.Add(param1);
            SqlParameter param2 = new SqlParameter("@PNR", SqlDbType.VarChar, 6);
            param2.Value = pnr;
            commandParameters.Add(param2);
            SqlParameter param3 = new SqlParameter("@ReferenceID", SqlDbType.NVarChar, 50);
            param3.Value = referenceID;
            commandParameters.Add(param3);
            SqlParameter param4 = new SqlParameter("@CurrencyCode", SqlDbType.VarChar, 3);
            param4.Value = currencyCode;
            commandParameters.Add(param4);
            SqlParameter param5 = new SqlParameter("@ChargeAmount", SqlDbType.Money);
            param5.Value = chargeAmount;
            commandParameters.Add(param5);
            SqlParameter param6 = new SqlParameter("@ProductType", SqlDbType.VarChar, 20);
            param6.Value = Type;
            commandParameters.Add(param6);
            SqlParameter param7 = new SqlParameter("@createdDate", SqlDbType.DateTime, 20);
            param7.Value = createdDate;
            commandParameters.Add(param7);
            SqlParameter param8 = new SqlParameter("@Proceso", SqlDbType.VarChar, 15);
            param8.Value = tipoProceso;
            commandParameters.Add(param8);
            SqlParameter param9 = new SqlParameter("@FolioPG", SqlDbType.Int);
            param9.Value = FolioPG;
            commandParameters.Add(param9);


            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "[dbo].[uspFac_InsDetalleHotelPaquete]";
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
                //if (dsResult.Tables["dtResultado"].Rows.Count >= 1)
                //{
                //    DataRow dr = dsResult.Tables["dtResultado"].Rows[0];
                //    int result = (int)dr[0];
                //    string mensaje = dr[1].ToString();
                //    if (result == 0)
                //    {
                //        throw new Exception(mensaje);
                //    }
                //    else
                //    {
                //        idReservaCab= Convert.ToInt64(cmm.Parameters[0].Value);

                //    }
                //}
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
        #endregion

        #region Recuperar toda la tabla
        public List<ENTHotel> RecuperarInfoProveedor()
        {
            List<ENTHotel> result = new List<ENTHotel>();
            DataTable dtResultado = new DataTable();
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_GetProveedor";
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
                foreach (DataRow dr in dtResultado.Rows)
                {
                    ENTHotel item = new ENTHotel();
                    if (!dr.IsNull("RFC")) item.RFC = Convert.ToString(dr["RFC"]);
                    if (!dr.IsNull("Nombre")) item.Nombre = Convert.ToString(dr["Nombre"]);
                    if (!dr.IsNull("Calle")) item.Calle = Convert.ToString(dr["Calle"]);
                    if (!dr.IsNull("NumExt")) item.NumExt = Convert.ToString(dr["NumExt"]);
                    if (!dr.IsNull("Municipio")) item.Municipio = Convert.ToString(dr["Municipio"]);
                    if (!dr.IsNull("Estado")) item.Estado = Convert.ToString(dr["Estado"]);
                    if (!dr.IsNull("Pais")) item.Pais = Convert.ToString(dr["Pais"]);
                    if (!dr.IsNull("CodigoPostal")) item.CodigoPostal = Convert.ToString(dr["CodigoPostal"]);
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
        #endregion
    }
}

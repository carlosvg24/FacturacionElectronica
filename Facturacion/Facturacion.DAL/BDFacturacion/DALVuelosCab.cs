using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALVuelosCab: ENTVuelosCab
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALVuelosCab _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALVuelosCab(SqlConnection conexion)
		{
			_conexion = conexion;
			IniciarPropiedades();
		}
		#endregion Constructores

		#region Miembros de IAccesoDatos
		#region Iniciar Propiedades
		public void IniciarPropiedades()
		{
			_copia = null;
			IdVuelo = 0;
			InventoryLegId = 0;
			InventoryLegKey = String.Empty;
			DepartureDate = new DateTime();
			CarrierCode = String.Empty;
			FlightNumber = String.Empty;
			DepartureStation = String.Empty;
			STD = new DateTime();
			DepartureTerminal = String.Empty;
			ArrivalStation = String.Empty;
			STA = new DateTime();
			ArrivalTerminal = String.Empty;
			CityPair = String.Empty;
		}
		#endregion

		#region Agregar VBFac_Vuelos_Cab
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdVuelo", SqlDbType.BigInt);
			param0.Value = IdVuelo;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@InventoryLegId", SqlDbType.BigInt);
			param1.Value = InventoryLegId;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@InventoryLegKey", SqlDbType.VarChar);
			param2.Value = InventoryLegKey;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@DepartureDate", SqlDbType.VarChar);
			param3.Value = DepartureDate.Year > 1900 ? DepartureDate.ToString("yyyy-MM-dd") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@CarrierCode", SqlDbType.VarChar);
			param4.Value = CarrierCode;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@FlightNumber", SqlDbType.VarChar);
			param5.Value = FlightNumber;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@DepartureStation", SqlDbType.VarChar);
			param6.Value = DepartureStation;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@STD", SqlDbType.VarChar);
			param7.Value = STD.Year > 1900 ? STD.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@DepartureTerminal", SqlDbType.VarChar);
			param8.Value = DepartureTerminal;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@ArrivalStation", SqlDbType.VarChar);
			param9.Value = ArrivalStation;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@STA", SqlDbType.VarChar);
			param10.Value = STA.Year > 1900 ? STA.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@ArrivalTerminal", SqlDbType.VarChar);
			param11.Value = ArrivalTerminal;
			commandParameters.Add(param11);
			SqlParameter param12 = new SqlParameter("@CityPair", SqlDbType.VarChar);
			param12.Value = CityPair;
			commandParameters.Add(param12);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsVuelosCab";
			cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]); 
			cmm.Connection = _conexion;
			bool cerrarConexion = false;
			foreach(SqlParameter p in commandParameters)
			{
				cmm.Parameters.Add(p);
			}
			try
			{
				if(_conexion.State.Equals(ConnectionState.Closed))
				{
					_conexion.Open();
					cerrarConexion = true;
				}
				SqlDataAdapter da = new SqlDataAdapter(cmm);
				DataSet dsResult = new DataSet();
				da.Fill(dsResult, "dtResultado");
				if (dsResult.Tables["dtResultado"].Rows.Count >= 1)
				{
					DataRow dr = dsResult.Tables["dtResultado"].Rows[0];
					int result = (int)dr[0];
					string mensaje = dr[1].ToString();
					if (result == 0)
					{
						throw new Exception(mensaje);
					}
					else
					{
						IdVuelo = Convert.ToInt64(cmm.Parameters[0].Value);

					}
				}
			}
			catch(SqlException ex)
			{
				throw ex;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cmm.Dispose();
				if(cerrarConexion)
				{
					_conexion.Close();
				}
			}
		}
		#endregion

		#region Actualizar VBFac_Vuelos_Cab
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdVuelo", SqlDbType.BigInt);
			param0.Value = IdVuelo;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@InventoryLegId", SqlDbType.BigInt);
			param1.Value = InventoryLegId;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@InventoryLegKey", SqlDbType.VarChar);
			param2.Value = InventoryLegKey;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@DepartureDate", SqlDbType.VarChar);
			param3.Value = DepartureDate.Year > 1900 ? DepartureDate.ToString("yyyy-MM-dd") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@CarrierCode", SqlDbType.VarChar);
			param4.Value = CarrierCode;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@FlightNumber", SqlDbType.VarChar);
			param5.Value = FlightNumber;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@DepartureStation", SqlDbType.VarChar);
			param6.Value = DepartureStation;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@STD", SqlDbType.VarChar);
			param7.Value = STD.Year > 1900 ? STD.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@DepartureTerminal", SqlDbType.VarChar);
			param8.Value = DepartureTerminal;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@ArrivalStation", SqlDbType.VarChar);
			param9.Value = ArrivalStation;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@STA", SqlDbType.VarChar);
			param10.Value = STA.Year > 1900 ? STA.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@ArrivalTerminal", SqlDbType.VarChar);
			param11.Value = ArrivalTerminal;
			commandParameters.Add(param11);
			SqlParameter param12 = new SqlParameter("@CityPair", SqlDbType.VarChar);
			param12.Value = CityPair;
			commandParameters.Add(param12);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdVuelosCab";
			cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]); 
			cmm.Connection = _conexion;
			bool cerrarConexion = false;
			foreach(SqlParameter p in commandParameters)
			{
				cmm.Parameters.Add(p);
			}
			try
			{
				if(_conexion.State.Equals(ConnectionState.Closed)) 
				{
					_conexion.Open();
					cerrarConexion = true;
				}
				SqlDataAdapter da = new SqlDataAdapter(cmm);
				DataSet dsResult = new DataSet();
				da.Fill(dsResult, "dtResultado");
				if (dsResult.Tables["dtResultado"].Rows.Count >= 1)
				{
					DataRow dr = dsResult.Tables["dtResultado"].Rows[0];
					int result = (int)dr[0];
					string mensaje = dr[1].ToString();
					if (result == 0)
					{
						throw new Exception(mensaje);
					}
				}
			}
			catch(SqlException ex)
			{
				throw ex;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cmm.Dispose();
				if(cerrarConexion)
				{
					_conexion.Close();
				}
			}
		}
		#endregion

		#region Eliminar VBFac_Vuelos_Cab
		public void Eliminar(long idvuelo)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdVuelo", SqlDbType.BigInt);
			param0.Value = idvuelo;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelVuelosCab";
			cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]); 
			cmm.Connection = _conexion;
			bool cerrarConexion = false;
			foreach(SqlParameter p in commandParameters)
			{
				cmm.Parameters.Add(p);
			}
			try
			{
				if(_conexion.State.Equals(ConnectionState.Closed)) 
				{
					_conexion.Open();
					cerrarConexion = true;
				}
				SqlDataAdapter da = new SqlDataAdapter(cmm);
				DataSet dsResult = new DataSet();
				da.Fill(dsResult, "dtResultado");
				if (dsResult.Tables["dtResultado"].Rows.Count >= 1)
				{
					DataRow dr = dsResult.Tables["dtResultado"].Rows[0];
					int result = (int)dr[0];
					string mensaje = dr[1].ToString();
					if (result == 0)
					{
						throw new Exception(mensaje);
					}
				}
			}
			catch(SqlException ex)
			{
				throw ex;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cmm.Dispose();
				if(cerrarConexion)
				{
					_conexion.Close();
				}
			}
		}
		#endregion

		#region Deshacer VBFac_Vuelos_Cab
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdVuelo);
			}
			else
			{
				if(_copia != null)
				{
					_copia.Actualizar();
				}
			}
		}
		#endregion

		#region Metodos Recuperar
		#region Recuperar toda la tabla
		public List<ENTVuelosCab> RecuperarTodo()
		{
			List<ENTVuelosCab> result = new List<ENTVuelosCab>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetVuelosCab_TODO";
			cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]); 
			cmm.Connection = _conexion;
			bool cerrarConexion = false;
			 
			try
			{
				if(_conexion.State.Equals(ConnectionState.Closed)) 
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
				    ENTVuelosCab item = new ENTVuelosCab();
					 if (!dr.IsNull("IdVuelo")) item.IdVuelo = Convert.ToInt64(dr["IdVuelo"]);
					 if (!dr.IsNull("InventoryLegId")) item.InventoryLegId = Convert.ToInt64(dr["InventoryLegId"]);
					 if (!dr.IsNull("InventoryLegKey")) item.InventoryLegKey = dr["InventoryLegKey"].ToString();
					 if (!dr.IsNull("DepartureDate")) item.DepartureDate = Convert.ToDateTime(dr["DepartureDate"]);
					 if (!dr.IsNull("CarrierCode")) item.CarrierCode = dr["CarrierCode"].ToString();
					 if (!dr.IsNull("FlightNumber")) item.FlightNumber = dr["FlightNumber"].ToString();
					 if (!dr.IsNull("DepartureStation")) item.DepartureStation = dr["DepartureStation"].ToString();
					 if (!dr.IsNull("STD")) item.STD = Convert.ToDateTime(dr["STD"]);
					 if (!dr.IsNull("DepartureTerminal")) item.DepartureTerminal = dr["DepartureTerminal"].ToString();
					 if (!dr.IsNull("ArrivalStation")) item.ArrivalStation = dr["ArrivalStation"].ToString();
					 if (!dr.IsNull("STA")) item.STA = Convert.ToDateTime(dr["STA"]);
					 if (!dr.IsNull("ArrivalTerminal")) item.ArrivalTerminal = dr["ArrivalTerminal"].ToString();
					 if (!dr.IsNull("CityPair")) item.CityPair = dr["CityPair"].ToString();
				    result.Add(item);
				}
			}
			catch(SqlException ex)
			{
				throw ex;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cmm.Dispose();
				if(cerrarConexion)
				{
					_conexion.Close();
				}
			}
			return result;
		}
		#endregion

		#region Recuperar RecuperarVuelosCabPorLlavePrimaria
		public List<ENTVuelosCab> RecuperarVuelosCabPorLlavePrimaria(long idvuelo)
		{
			List<ENTVuelosCab> result = new List<ENTVuelosCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdVuelo", SqlDbType.BigInt);
			param0.Value = idvuelo;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetVuelosCab_POR_PK";
			cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]); 
			cmm.Connection = _conexion;
			bool cerrarConexion = false;
			foreach(SqlParameter p in commandParameters)
			{
				cmm.Parameters.Add(p);
			}
			 
			try
			{
				if(_conexion.State.Equals(ConnectionState.Closed)) 
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
				    ENTVuelosCab item = new ENTVuelosCab();
					if (!dr.IsNull("IdVuelo")) item.IdVuelo = Convert.ToInt64(dr["IdVuelo"]);
					if (!dr.IsNull("InventoryLegId")) item.InventoryLegId = Convert.ToInt64(dr["InventoryLegId"]);
					if (!dr.IsNull("InventoryLegKey")) item.InventoryLegKey = dr["InventoryLegKey"].ToString();
					if (!dr.IsNull("DepartureDate")) item.DepartureDate = Convert.ToDateTime(dr["DepartureDate"]);
					if (!dr.IsNull("CarrierCode")) item.CarrierCode = dr["CarrierCode"].ToString();
					if (!dr.IsNull("FlightNumber")) item.FlightNumber = dr["FlightNumber"].ToString();
					if (!dr.IsNull("DepartureStation")) item.DepartureStation = dr["DepartureStation"].ToString();
					if (!dr.IsNull("STD")) item.STD = Convert.ToDateTime(dr["STD"]);
					if (!dr.IsNull("DepartureTerminal")) item.DepartureTerminal = dr["DepartureTerminal"].ToString();
					if (!dr.IsNull("ArrivalStation")) item.ArrivalStation = dr["ArrivalStation"].ToString();
					if (!dr.IsNull("STA")) item.STA = Convert.ToDateTime(dr["STA"]);
					if (!dr.IsNull("ArrivalTerminal")) item.ArrivalTerminal = dr["ArrivalTerminal"].ToString();
					if (!dr.IsNull("CityPair")) item.CityPair = dr["CityPair"].ToString();
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdVuelo")) IdVuelo = Convert.ToInt64(dtResultado.Rows[0]["IdVuelo"]);
					if (!dtResultado.Rows[0].IsNull("InventoryLegId")) InventoryLegId = Convert.ToInt64(dtResultado.Rows[0]["InventoryLegId"]);
					if (!dtResultado.Rows[0].IsNull("InventoryLegKey")) InventoryLegKey = dtResultado.Rows[0]["InventoryLegKey"].ToString();
					if (!dtResultado.Rows[0].IsNull("DepartureDate")) DepartureDate = Convert.ToDateTime(dtResultado.Rows[0]["DepartureDate"]);
					if (!dtResultado.Rows[0].IsNull("CarrierCode")) CarrierCode = dtResultado.Rows[0]["CarrierCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("FlightNumber")) FlightNumber = dtResultado.Rows[0]["FlightNumber"].ToString();
					if (!dtResultado.Rows[0].IsNull("DepartureStation")) DepartureStation = dtResultado.Rows[0]["DepartureStation"].ToString();
					if (!dtResultado.Rows[0].IsNull("STD")) STD = Convert.ToDateTime(dtResultado.Rows[0]["STD"]);
					if (!dtResultado.Rows[0].IsNull("DepartureTerminal")) DepartureTerminal = dtResultado.Rows[0]["DepartureTerminal"].ToString();
					if (!dtResultado.Rows[0].IsNull("ArrivalStation")) ArrivalStation = dtResultado.Rows[0]["ArrivalStation"].ToString();
					if (!dtResultado.Rows[0].IsNull("STA")) STA = Convert.ToDateTime(dtResultado.Rows[0]["STA"]);
					if (!dtResultado.Rows[0].IsNull("ArrivalTerminal")) ArrivalTerminal = dtResultado.Rows[0]["ArrivalTerminal"].ToString();
					if (!dtResultado.Rows[0].IsNull("CityPair")) CityPair = dtResultado.Rows[0]["CityPair"].ToString();
					this.Clone();
				}
			}
			catch(SqlException ex)
			{
				throw ex;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cmm.Dispose();
				if(cerrarConexion)
				{
					_conexion.Close();
				}
			}
			return result;
		}
		#endregion

		#region Recuperar RecuperarVuelosCabInventorylegid
		public List<ENTVuelosCab> RecuperarVuelosCabInventorylegid(long inventorylegid)
		{
			List<ENTVuelosCab> result = new List<ENTVuelosCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@InventoryLegId", SqlDbType.BigInt);
			param0.Value = inventorylegid;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetVuelosCab_POR_InventoryLegId";
			cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]); 
			cmm.Connection = _conexion;
			bool cerrarConexion = false;
			foreach(SqlParameter p in commandParameters)
			{
				cmm.Parameters.Add(p);
			}
			 
			try
			{
				if(_conexion.State.Equals(ConnectionState.Closed)) 
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
				    ENTVuelosCab item = new ENTVuelosCab();
					if (!dr.IsNull("IdVuelo")) item.IdVuelo = Convert.ToInt64(dr["IdVuelo"]);
					if (!dr.IsNull("InventoryLegId")) item.InventoryLegId = Convert.ToInt64(dr["InventoryLegId"]);
					if (!dr.IsNull("InventoryLegKey")) item.InventoryLegKey = dr["InventoryLegKey"].ToString();
					if (!dr.IsNull("DepartureDate")) item.DepartureDate = Convert.ToDateTime(dr["DepartureDate"]);
					if (!dr.IsNull("CarrierCode")) item.CarrierCode = dr["CarrierCode"].ToString();
					if (!dr.IsNull("FlightNumber")) item.FlightNumber = dr["FlightNumber"].ToString();
					if (!dr.IsNull("DepartureStation")) item.DepartureStation = dr["DepartureStation"].ToString();
					if (!dr.IsNull("STD")) item.STD = Convert.ToDateTime(dr["STD"]);
					if (!dr.IsNull("DepartureTerminal")) item.DepartureTerminal = dr["DepartureTerminal"].ToString();
					if (!dr.IsNull("ArrivalStation")) item.ArrivalStation = dr["ArrivalStation"].ToString();
					if (!dr.IsNull("STA")) item.STA = Convert.ToDateTime(dr["STA"]);
					if (!dr.IsNull("ArrivalTerminal")) item.ArrivalTerminal = dr["ArrivalTerminal"].ToString();
					if (!dr.IsNull("CityPair")) item.CityPair = dr["CityPair"].ToString();
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdVuelo")) IdVuelo = Convert.ToInt64(dtResultado.Rows[0]["IdVuelo"]);
					if (!dtResultado.Rows[0].IsNull("InventoryLegId")) InventoryLegId = Convert.ToInt64(dtResultado.Rows[0]["InventoryLegId"]);
					if (!dtResultado.Rows[0].IsNull("InventoryLegKey")) InventoryLegKey = dtResultado.Rows[0]["InventoryLegKey"].ToString();
					if (!dtResultado.Rows[0].IsNull("DepartureDate")) DepartureDate = Convert.ToDateTime(dtResultado.Rows[0]["DepartureDate"]);
					if (!dtResultado.Rows[0].IsNull("CarrierCode")) CarrierCode = dtResultado.Rows[0]["CarrierCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("FlightNumber")) FlightNumber = dtResultado.Rows[0]["FlightNumber"].ToString();
					if (!dtResultado.Rows[0].IsNull("DepartureStation")) DepartureStation = dtResultado.Rows[0]["DepartureStation"].ToString();
					if (!dtResultado.Rows[0].IsNull("STD")) STD = Convert.ToDateTime(dtResultado.Rows[0]["STD"]);
					if (!dtResultado.Rows[0].IsNull("DepartureTerminal")) DepartureTerminal = dtResultado.Rows[0]["DepartureTerminal"].ToString();
					if (!dtResultado.Rows[0].IsNull("ArrivalStation")) ArrivalStation = dtResultado.Rows[0]["ArrivalStation"].ToString();
					if (!dtResultado.Rows[0].IsNull("STA")) STA = Convert.ToDateTime(dtResultado.Rows[0]["STA"]);
					if (!dtResultado.Rows[0].IsNull("ArrivalTerminal")) ArrivalTerminal = dtResultado.Rows[0]["ArrivalTerminal"].ToString();
					if (!dtResultado.Rows[0].IsNull("CityPair")) CityPair = dtResultado.Rows[0]["CityPair"].ToString();
					this.Clone();
				}
			}
			catch(SqlException ex)
			{
				throw ex;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				cmm.Dispose();
				if(cerrarConexion)
				{
					_conexion.Close();
				}
			}
			return result;
		}
		#endregion

		#endregion

		#region Miembros de ICloneable
		public object Clone()
		{
			_copia = new DALVuelosCab(_conexion);
			Type tipo = typeof(ENTVuelosCab);
			PropertyInfo[] propiedades = tipo.GetProperties();
			foreach (PropertyInfo propiedad in propiedades)
			{
				propiedad.SetValue(_copia, propiedad.GetValue(this, null), null);
			}
			return _copia;
		}
		#endregion Miembros de ICloneable

		#endregion Miembros de IAccesoDatos

	}
}

using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Facturacion.InterfaceRestViva
{
    public class DALLog: ENTLog
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALLog _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALLog(SqlConnection conexion)
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
			IdLog = 0;
			Modulo = String.Empty;
			PNR = String.Empty;
			TipoError = String.Empty;
			NivelImportancia = 0;
			Descripcion = String.Empty;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_Log
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdLog", SqlDbType.BigInt);
			param0.Value = IdLog;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@Modulo", SqlDbType.VarChar);
			param1.Value = Modulo;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@PNR", SqlDbType.VarChar);
			param2.Value = PNR;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@TipoError", SqlDbType.VarChar);
			param3.Value = TipoError;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@NivelImportancia", SqlDbType.TinyInt);
			param4.Value = NivelImportancia;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param5.Value = Descripcion;
			commandParameters.Add(param5);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsLog";
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
						IdLog = Convert.ToInt64(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_Log
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdLog", SqlDbType.BigInt);
			param0.Value = IdLog;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@Modulo", SqlDbType.VarChar);
			param1.Value = Modulo;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@PNR", SqlDbType.VarChar);
			param2.Value = PNR;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@TipoError", SqlDbType.VarChar);
			param3.Value = TipoError;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@NivelImportancia", SqlDbType.TinyInt);
			param4.Value = NivelImportancia;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param5.Value = Descripcion;
			commandParameters.Add(param5);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdLog";
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

		#region Eliminar VBFac_Log
		public void Eliminar(long idlog)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdLog", SqlDbType.BigInt);
			param0.Value = idlog;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelLog";
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

		#region Deshacer VBFac_Log
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdLog);
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
		public List<ENTLog> RecuperarTodo()
		{
			List<ENTLog> result = new List<ENTLog>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetLog_TODO";
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
				    ENTLog item = new ENTLog();
					 if (!dr.IsNull("IdLog")) item.IdLog = Convert.ToInt64(dr["IdLog"]);
					 if (!dr.IsNull("Modulo")) item.Modulo = dr["Modulo"].ToString();
					 if (!dr.IsNull("PNR")) item.PNR = dr["PNR"].ToString();
					 if (!dr.IsNull("TipoError")) item.TipoError = dr["TipoError"].ToString();
					 if (!dr.IsNull("NivelImportancia")) item.NivelImportancia = Convert.ToByte(dr["NivelImportancia"]);
					 if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					 if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
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

		#region Recuperar RecuperarLogPorLlavePrimaria
		public List<ENTLog> RecuperarLogPorLlavePrimaria(long idlog)
		{
			List<ENTLog> result = new List<ENTLog>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdLog", SqlDbType.BigInt);
			param0.Value = idlog;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetLog_POR_PK";
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
				    ENTLog item = new ENTLog();
					if (!dr.IsNull("IdLog")) item.IdLog = Convert.ToInt64(dr["IdLog"]);
					if (!dr.IsNull("Modulo")) item.Modulo = dr["Modulo"].ToString();
					if (!dr.IsNull("PNR")) item.PNR = dr["PNR"].ToString();
					if (!dr.IsNull("TipoError")) item.TipoError = dr["TipoError"].ToString();
					if (!dr.IsNull("NivelImportancia")) item.NivelImportancia = Convert.ToByte(dr["NivelImportancia"]);
					if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdLog")) IdLog = Convert.ToInt64(dtResultado.Rows[0]["IdLog"]);
					if (!dtResultado.Rows[0].IsNull("Modulo")) Modulo = dtResultado.Rows[0]["Modulo"].ToString();
					if (!dtResultado.Rows[0].IsNull("PNR")) PNR = dtResultado.Rows[0]["PNR"].ToString();
					if (!dtResultado.Rows[0].IsNull("TipoError")) TipoError = dtResultado.Rows[0]["TipoError"].ToString();
					if (!dtResultado.Rows[0].IsNull("NivelImportancia")) NivelImportancia = Convert.ToByte(dtResultado.Rows[0]["NivelImportancia"]);
					if (!dtResultado.Rows[0].IsNull("Descripcion")) Descripcion = dtResultado.Rows[0]["Descripcion"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
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
			_copia = new DALLog(_conexion);
			Type tipo = typeof(ENTLog);
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

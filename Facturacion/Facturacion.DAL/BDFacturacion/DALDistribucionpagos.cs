using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALDistribucionpagos: ENTDistribucionpagos
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALDistribucionpagos _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALDistribucionpagos(SqlConnection conexion)
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
			IdDistribucionPagos = 0;
			BookingID = 0;
			RecordLocator = String.Empty;
			CreatedDate = new DateTime();
			ModifiedDate = new DateTime();
			FechaProcesamiento = new DateTime();
			ProcesoExitoso = false;
			ConDescartePorDiferencia = false;
			MensajeError = String.Empty;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_DistribucionPagos
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdDistribucionPagos", SqlDbType.BigInt);
			param0.Value = IdDistribucionPagos;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param1.Value = BookingID;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@RecordLocator", SqlDbType.VarChar);
			param2.Value = RecordLocator;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@CreatedDate", SqlDbType.VarChar);
			param3.Value = CreatedDate.Year > 1900 ? CreatedDate.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@ModifiedDate", SqlDbType.VarChar);
			param4.Value = ModifiedDate.Year > 1900 ? ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@FechaProcesamiento", SqlDbType.VarChar);
			param5.Value = FechaProcesamiento.Year > 1900 ? FechaProcesamiento.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@ProcesoExitoso", SqlDbType.Bit);
			param6.Value = ProcesoExitoso;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@ConDescartePorDiferencia", SqlDbType.Bit);
			param7.Value = ConDescartePorDiferencia;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@MensajeError", SqlDbType.VarChar);
			param8.Value = MensajeError;
			commandParameters.Add(param8);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsDistribucionPagos";
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
						IdDistribucionPagos = Convert.ToInt64(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_DistribucionPagos
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdDistribucionPagos", SqlDbType.BigInt);
			param0.Value = IdDistribucionPagos;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param1.Value = BookingID;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@RecordLocator", SqlDbType.VarChar);
			param2.Value = RecordLocator;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@CreatedDate", SqlDbType.VarChar);
			param3.Value = CreatedDate.Year > 1900 ? CreatedDate.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@ModifiedDate", SqlDbType.VarChar);
			param4.Value = ModifiedDate.Year > 1900 ? ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@FechaProcesamiento", SqlDbType.VarChar);
			param5.Value = FechaProcesamiento.Year > 1900 ? FechaProcesamiento.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@ProcesoExitoso", SqlDbType.Bit);
			param6.Value = ProcesoExitoso;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@ConDescartePorDiferencia", SqlDbType.Bit);
			param7.Value = ConDescartePorDiferencia;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@MensajeError", SqlDbType.VarChar);
			param8.Value = MensajeError;
			commandParameters.Add(param8);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdDistribucionPagos";
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

		#region Eliminar VBFac_DistribucionPagos
		public void Eliminar(long iddistribucionpagos)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdDistribucionPagos", SqlDbType.BigInt);
			param0.Value = iddistribucionpagos;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelDistribucionPagos";
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

		#region Deshacer VBFac_DistribucionPagos
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdDistribucionPagos);
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
		public List<ENTDistribucionpagos> RecuperarTodo()
		{
			List<ENTDistribucionpagos> result = new List<ENTDistribucionpagos>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetDistribucionPagos_TODO";
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
				    ENTDistribucionpagos item = new ENTDistribucionpagos();
					 if (!dr.IsNull("IdDistribucionPagos")) item.IdDistribucionPagos = Convert.ToInt64(dr["IdDistribucionPagos"]);
					 if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					 if (!dr.IsNull("RecordLocator")) item.RecordLocator = dr["RecordLocator"].ToString();
					 if (!dr.IsNull("CreatedDate")) item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
					 if (!dr.IsNull("ModifiedDate")) item.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
					 if (!dr.IsNull("FechaProcesamiento")) item.FechaProcesamiento = Convert.ToDateTime(dr["FechaProcesamiento"]);
					 if (!dr.IsNull("ProcesoExitoso")) item.ProcesoExitoso = Convert.ToBoolean(dr["ProcesoExitoso"]);
					 if (!dr.IsNull("ConDescartePorDiferencia")) item.ConDescartePorDiferencia = Convert.ToBoolean(dr["ConDescartePorDiferencia"]);
					 if (!dr.IsNull("MensajeError")) item.MensajeError = dr["MensajeError"].ToString();
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

		#region Recuperar RecuperarDistribucionpagosPorLlavePrimaria
		public List<ENTDistribucionpagos> RecuperarDistribucionpagosPorLlavePrimaria(long iddistribucionpagos)
		{
			List<ENTDistribucionpagos> result = new List<ENTDistribucionpagos>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdDistribucionPagos", SqlDbType.BigInt);
			param0.Value = iddistribucionpagos;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetDistribucionPagos_POR_PK";
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
				    ENTDistribucionpagos item = new ENTDistribucionpagos();
					if (!dr.IsNull("IdDistribucionPagos")) item.IdDistribucionPagos = Convert.ToInt64(dr["IdDistribucionPagos"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("RecordLocator")) item.RecordLocator = dr["RecordLocator"].ToString();
					if (!dr.IsNull("CreatedDate")) item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
					if (!dr.IsNull("ModifiedDate")) item.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
					if (!dr.IsNull("FechaProcesamiento")) item.FechaProcesamiento = Convert.ToDateTime(dr["FechaProcesamiento"]);
					if (!dr.IsNull("ProcesoExitoso")) item.ProcesoExitoso = Convert.ToBoolean(dr["ProcesoExitoso"]);
					if (!dr.IsNull("ConDescartePorDiferencia")) item.ConDescartePorDiferencia = Convert.ToBoolean(dr["ConDescartePorDiferencia"]);
					if (!dr.IsNull("MensajeError")) item.MensajeError = dr["MensajeError"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdDistribucionPagos")) IdDistribucionPagos = Convert.ToInt64(dtResultado.Rows[0]["IdDistribucionPagos"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("RecordLocator")) RecordLocator = dtResultado.Rows[0]["RecordLocator"].ToString();
					if (!dtResultado.Rows[0].IsNull("CreatedDate")) CreatedDate = Convert.ToDateTime(dtResultado.Rows[0]["CreatedDate"]);
					if (!dtResultado.Rows[0].IsNull("ModifiedDate")) ModifiedDate = Convert.ToDateTime(dtResultado.Rows[0]["ModifiedDate"]);
					if (!dtResultado.Rows[0].IsNull("FechaProcesamiento")) FechaProcesamiento = Convert.ToDateTime(dtResultado.Rows[0]["FechaProcesamiento"]);
					if (!dtResultado.Rows[0].IsNull("ProcesoExitoso")) ProcesoExitoso = Convert.ToBoolean(dtResultado.Rows[0]["ProcesoExitoso"]);
					if (!dtResultado.Rows[0].IsNull("ConDescartePorDiferencia")) ConDescartePorDiferencia = Convert.ToBoolean(dtResultado.Rows[0]["ConDescartePorDiferencia"]);
					if (!dtResultado.Rows[0].IsNull("MensajeError")) MensajeError = dtResultado.Rows[0]["MensajeError"].ToString();
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

		#region Recuperar RecuperarDistribucionpagosRecordlocator
		public List<ENTDistribucionpagos> RecuperarDistribucionpagosRecordlocator(string recordlocator)
		{
			List<ENTDistribucionpagos> result = new List<ENTDistribucionpagos>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@RecordLocator", SqlDbType.VarChar);
			param0.Value = recordlocator;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetDistribucionPagos_POR_RecordLocator";
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
				    ENTDistribucionpagos item = new ENTDistribucionpagos();
					if (!dr.IsNull("IdDistribucionPagos")) item.IdDistribucionPagos = Convert.ToInt64(dr["IdDistribucionPagos"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("RecordLocator")) item.RecordLocator = dr["RecordLocator"].ToString();
					if (!dr.IsNull("CreatedDate")) item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
					if (!dr.IsNull("ModifiedDate")) item.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
					if (!dr.IsNull("FechaProcesamiento")) item.FechaProcesamiento = Convert.ToDateTime(dr["FechaProcesamiento"]);
					if (!dr.IsNull("ProcesoExitoso")) item.ProcesoExitoso = Convert.ToBoolean(dr["ProcesoExitoso"]);
					if (!dr.IsNull("ConDescartePorDiferencia")) item.ConDescartePorDiferencia = Convert.ToBoolean(dr["ConDescartePorDiferencia"]);
					if (!dr.IsNull("MensajeError")) item.MensajeError = dr["MensajeError"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdDistribucionPagos")) IdDistribucionPagos = Convert.ToInt64(dtResultado.Rows[0]["IdDistribucionPagos"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("RecordLocator")) RecordLocator = dtResultado.Rows[0]["RecordLocator"].ToString();
					if (!dtResultado.Rows[0].IsNull("CreatedDate")) CreatedDate = Convert.ToDateTime(dtResultado.Rows[0]["CreatedDate"]);
					if (!dtResultado.Rows[0].IsNull("ModifiedDate")) ModifiedDate = Convert.ToDateTime(dtResultado.Rows[0]["ModifiedDate"]);
					if (!dtResultado.Rows[0].IsNull("FechaProcesamiento")) FechaProcesamiento = Convert.ToDateTime(dtResultado.Rows[0]["FechaProcesamiento"]);
					if (!dtResultado.Rows[0].IsNull("ProcesoExitoso")) ProcesoExitoso = Convert.ToBoolean(dtResultado.Rows[0]["ProcesoExitoso"]);
					if (!dtResultado.Rows[0].IsNull("ConDescartePorDiferencia")) ConDescartePorDiferencia = Convert.ToBoolean(dtResultado.Rows[0]["ConDescartePorDiferencia"]);
					if (!dtResultado.Rows[0].IsNull("MensajeError")) MensajeError = dtResultado.Rows[0]["MensajeError"].ToString();
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
			_copia = new DALDistribucionpagos(_conexion);
			Type tipo = typeof(ENTDistribucionpagos);
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

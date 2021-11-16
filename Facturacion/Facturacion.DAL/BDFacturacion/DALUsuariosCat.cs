using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALUsuariosCat: ENTUsuariosCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALUsuariosCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALUsuariosCat(SqlConnection conexion)
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
			IdUsuario = 0;
			Usuario = String.Empty;
			IdAgente = 0;
			Nombre = String.Empty;
			Apellidos = String.Empty;
			Password = String.Empty;
			Activo = true;
			FechaIniVigencia = new DateTime();
			FechaFinVigencia = new DateTime();
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_Usuarios_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdUsuario", SqlDbType.Int);
			param0.Value = IdUsuario;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@Usuario", SqlDbType.VarChar);
			param1.Value = Usuario;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@IdAgente", SqlDbType.Int);
			param2.Value = IdAgente;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Nombre", SqlDbType.VarChar);
			param3.Value = Nombre;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Apellidos", SqlDbType.VarChar);
			param4.Value = Apellidos;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Password", SqlDbType.VarChar);
			param5.Value = Password;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Activo", SqlDbType.Bit);
			param6.Value = Activo;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@FechaIniVigencia", SqlDbType.VarChar);
			param7.Value = FechaIniVigencia.Year > 1900 ? FechaIniVigencia.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@FechaFinVigencia", SqlDbType.VarChar);
			param8.Value = FechaFinVigencia.Year > 1900 ? FechaFinVigencia.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param8);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsUsuariosCat";
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
						IdUsuario = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_Usuarios_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdUsuario", SqlDbType.Int);
			param0.Value = IdUsuario;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@Usuario", SqlDbType.VarChar);
			param1.Value = Usuario;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@IdAgente", SqlDbType.Int);
			param2.Value = IdAgente;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Nombre", SqlDbType.VarChar);
			param3.Value = Nombre;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Apellidos", SqlDbType.VarChar);
			param4.Value = Apellidos;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Password", SqlDbType.VarChar);
			param5.Value = Password;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Activo", SqlDbType.Bit);
			param6.Value = Activo;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@FechaIniVigencia", SqlDbType.VarChar);
			param7.Value = FechaIniVigencia.Year > 1900 ? FechaIniVigencia.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@FechaFinVigencia", SqlDbType.VarChar);
			param8.Value = FechaFinVigencia.Year > 1900 ? FechaFinVigencia.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param8);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdUsuariosCat";
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

		#region Eliminar VBFac_Usuarios_Cat
		public void Eliminar(int idusuario)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdUsuario", SqlDbType.Int);
			param0.Value = idusuario;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelUsuariosCat";
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

		#region Deshacer VBFac_Usuarios_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdUsuario);
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
		public List<ENTUsuariosCat> RecuperarTodo()
		{
			List<ENTUsuariosCat> result = new List<ENTUsuariosCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetUsuariosCat_TODO";
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
				    ENTUsuariosCat item = new ENTUsuariosCat();
					 if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
					 if (!dr.IsNull("Usuario")) item.Usuario = dr["Usuario"].ToString();
					 if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt32(dr["IdAgente"]);
					 if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					 if (!dr.IsNull("Apellidos")) item.Apellidos = dr["Apellidos"].ToString();
					 if (!dr.IsNull("Password")) item.Password = dr["Password"].ToString();
					 if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					 if (!dr.IsNull("FechaIniVigencia")) item.FechaIniVigencia = Convert.ToDateTime(dr["FechaIniVigencia"]);
					 if (!dr.IsNull("FechaFinVigencia")) item.FechaFinVigencia = Convert.ToDateTime(dr["FechaFinVigencia"]);
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

		#region Recuperar RecuperarUsuariosCatPorLlavePrimaria
		public List<ENTUsuariosCat> RecuperarUsuariosCatPorLlavePrimaria(int idusuario)
		{
			List<ENTUsuariosCat> result = new List<ENTUsuariosCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdUsuario", SqlDbType.Int);
			param0.Value = idusuario;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetUsuariosCat_POR_PK";
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
				    ENTUsuariosCat item = new ENTUsuariosCat();
					if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
					if (!dr.IsNull("Usuario")) item.Usuario = dr["Usuario"].ToString();
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt32(dr["IdAgente"]);
					if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					if (!dr.IsNull("Apellidos")) item.Apellidos = dr["Apellidos"].ToString();
					if (!dr.IsNull("Password")) item.Password = dr["Password"].ToString();
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaIniVigencia")) item.FechaIniVigencia = Convert.ToDateTime(dr["FechaIniVigencia"]);
					if (!dr.IsNull("FechaFinVigencia")) item.FechaFinVigencia = Convert.ToDateTime(dr["FechaFinVigencia"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdUsuario")) IdUsuario = Convert.ToInt32(dtResultado.Rows[0]["IdUsuario"]);
					if (!dtResultado.Rows[0].IsNull("Usuario")) Usuario = dtResultado.Rows[0]["Usuario"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt32(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("Nombre")) Nombre = dtResultado.Rows[0]["Nombre"].ToString();
					if (!dtResultado.Rows[0].IsNull("Apellidos")) Apellidos = dtResultado.Rows[0]["Apellidos"].ToString();
					if (!dtResultado.Rows[0].IsNull("Password")) Password = dtResultado.Rows[0]["Password"].ToString();
					if (!dtResultado.Rows[0].IsNull("Activo")) Activo = Convert.ToBoolean(dtResultado.Rows[0]["Activo"]);
					if (!dtResultado.Rows[0].IsNull("FechaIniVigencia")) FechaIniVigencia = Convert.ToDateTime(dtResultado.Rows[0]["FechaIniVigencia"]);
					if (!dtResultado.Rows[0].IsNull("FechaFinVigencia")) FechaFinVigencia = Convert.ToDateTime(dtResultado.Rows[0]["FechaFinVigencia"]);
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

		#region Recuperar RecuperarUsuariosCatIdagente
		public List<ENTUsuariosCat> RecuperarUsuariosCatIdagente(int idagente)
		{
			List<ENTUsuariosCat> result = new List<ENTUsuariosCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdAgente", SqlDbType.Int);
			param0.Value = idagente;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetUsuariosCat_POR_IdAgente";
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
				    ENTUsuariosCat item = new ENTUsuariosCat();
					if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
					if (!dr.IsNull("Usuario")) item.Usuario = dr["Usuario"].ToString();
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt32(dr["IdAgente"]);
					if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					if (!dr.IsNull("Apellidos")) item.Apellidos = dr["Apellidos"].ToString();
					if (!dr.IsNull("Password")) item.Password = dr["Password"].ToString();
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaIniVigencia")) item.FechaIniVigencia = Convert.ToDateTime(dr["FechaIniVigencia"]);
					if (!dr.IsNull("FechaFinVigencia")) item.FechaFinVigencia = Convert.ToDateTime(dr["FechaFinVigencia"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdUsuario")) IdUsuario = Convert.ToInt32(dtResultado.Rows[0]["IdUsuario"]);
					if (!dtResultado.Rows[0].IsNull("Usuario")) Usuario = dtResultado.Rows[0]["Usuario"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt32(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("Nombre")) Nombre = dtResultado.Rows[0]["Nombre"].ToString();
					if (!dtResultado.Rows[0].IsNull("Apellidos")) Apellidos = dtResultado.Rows[0]["Apellidos"].ToString();
					if (!dtResultado.Rows[0].IsNull("Password")) Password = dtResultado.Rows[0]["Password"].ToString();
					if (!dtResultado.Rows[0].IsNull("Activo")) Activo = Convert.ToBoolean(dtResultado.Rows[0]["Activo"]);
					if (!dtResultado.Rows[0].IsNull("FechaIniVigencia")) FechaIniVigencia = Convert.ToDateTime(dtResultado.Rows[0]["FechaIniVigencia"]);
					if (!dtResultado.Rows[0].IsNull("FechaFinVigencia")) FechaFinVigencia = Convert.ToDateTime(dtResultado.Rows[0]["FechaFinVigencia"]);
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

		#region Recuperar RecuperarUsuariosCatUsuario
		public List<ENTUsuariosCat> RecuperarUsuariosCatUsuario(string usuario)
		{
			List<ENTUsuariosCat> result = new List<ENTUsuariosCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@Usuario", SqlDbType.VarChar);
			param0.Value = usuario;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetUsuariosCat_POR_Usuario";
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
				    ENTUsuariosCat item = new ENTUsuariosCat();
					if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
					if (!dr.IsNull("Usuario")) item.Usuario = dr["Usuario"].ToString();
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt32(dr["IdAgente"]);
					if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					if (!dr.IsNull("Apellidos")) item.Apellidos = dr["Apellidos"].ToString();
					if (!dr.IsNull("Password")) item.Password = dr["Password"].ToString();
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaIniVigencia")) item.FechaIniVigencia = Convert.ToDateTime(dr["FechaIniVigencia"]);
					if (!dr.IsNull("FechaFinVigencia")) item.FechaFinVigencia = Convert.ToDateTime(dr["FechaFinVigencia"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdUsuario")) IdUsuario = Convert.ToInt32(dtResultado.Rows[0]["IdUsuario"]);
					if (!dtResultado.Rows[0].IsNull("Usuario")) Usuario = dtResultado.Rows[0]["Usuario"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt32(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("Nombre")) Nombre = dtResultado.Rows[0]["Nombre"].ToString();
					if (!dtResultado.Rows[0].IsNull("Apellidos")) Apellidos = dtResultado.Rows[0]["Apellidos"].ToString();
					if (!dtResultado.Rows[0].IsNull("Password")) Password = dtResultado.Rows[0]["Password"].ToString();
					if (!dtResultado.Rows[0].IsNull("Activo")) Activo = Convert.ToBoolean(dtResultado.Rows[0]["Activo"]);
					if (!dtResultado.Rows[0].IsNull("FechaIniVigencia")) FechaIniVigencia = Convert.ToDateTime(dtResultado.Rows[0]["FechaIniVigencia"]);
					if (!dtResultado.Rows[0].IsNull("FechaFinVigencia")) FechaFinVigencia = Convert.ToDateTime(dtResultado.Rows[0]["FechaFinVigencia"]);
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
			_copia = new DALUsuariosCat(_conexion);
			Type tipo = typeof(ENTUsuariosCat);
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

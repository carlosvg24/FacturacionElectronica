using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALUsuariosrolesCnf: ENTUsuariosrolesCnf
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALUsuariosrolesCnf _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALUsuariosrolesCnf(SqlConnection conexion)
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
			IdRol = 0;
			IdUsuario = 0;
			FechaIniVigencia = new DateTime();
			FechaFinVigencia = new DateTime();
			Activo = true;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_UsuariosRoles_Cnf
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdRol", SqlDbType.SmallInt);
			param0.Value = IdRol;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdUsuario", SqlDbType.Int);
			param1.Value = IdUsuario;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@FechaIniVigencia", SqlDbType.VarChar);
			param2.Value = FechaIniVigencia.Year > 1900 ? FechaIniVigencia.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@FechaFinVigencia", SqlDbType.VarChar);
			param3.Value = FechaFinVigencia.Year > 1900 ? FechaFinVigencia.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Activo", SqlDbType.Bit);
			param4.Value = Activo;
			commandParameters.Add(param4);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsUsuariosRolesCnf";
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

		#region Actualizar VBFac_UsuariosRoles_Cnf
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdRol", SqlDbType.SmallInt);
			param0.Value = IdRol;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdUsuario", SqlDbType.Int);
			param1.Value = IdUsuario;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@FechaIniVigencia", SqlDbType.VarChar);
			param2.Value = FechaIniVigencia.Year > 1900 ? FechaIniVigencia.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@FechaFinVigencia", SqlDbType.VarChar);
			param3.Value = FechaFinVigencia.Year > 1900 ? FechaFinVigencia.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Activo", SqlDbType.Bit);
			param4.Value = Activo;
			commandParameters.Add(param4);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdUsuariosRolesCnf";
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

		#region Eliminar VBFac_UsuariosRoles_Cnf
		public void Eliminar(int idrol,int idusuario)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdRol", SqlDbType.SmallInt);
			param0.Value = idrol;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdUsuario", SqlDbType.Int);
			param1.Value = idusuario;
			commandParameters.Add(param1);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelUsuariosRolesCnf";
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

		#region Deshacer VBFac_UsuariosRoles_Cnf
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdRol,IdUsuario);
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
		public List<ENTUsuariosrolesCnf> RecuperarTodo()
		{
			List<ENTUsuariosrolesCnf> result = new List<ENTUsuariosrolesCnf>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetUsuariosRolesCnf_TODO";
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
				    ENTUsuariosrolesCnf item = new ENTUsuariosrolesCnf();
					 if (!dr.IsNull("IdRol")) item.IdRol = Convert.ToInt16(dr["IdRol"]);
					 if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
					 if (!dr.IsNull("FechaIniVigencia")) item.FechaIniVigencia = Convert.ToDateTime(dr["FechaIniVigencia"]);
					 if (!dr.IsNull("FechaFinVigencia")) item.FechaFinVigencia = Convert.ToDateTime(dr["FechaFinVigencia"]);
					 if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
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

		#region Recuperar RecuperarUsuariosrolesCnfPorLlavePrimaria
		public List<ENTUsuariosrolesCnf> RecuperarUsuariosrolesCnfPorLlavePrimaria(int idrol,int idusuario)
		{
			List<ENTUsuariosrolesCnf> result = new List<ENTUsuariosrolesCnf>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdRol", SqlDbType.SmallInt);
			param0.Value = idrol;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdUsuario", SqlDbType.Int);
			param1.Value = idusuario;
			commandParameters.Add(param1);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetUsuariosRolesCnf_POR_PK";
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
				    ENTUsuariosrolesCnf item = new ENTUsuariosrolesCnf();
					if (!dr.IsNull("IdRol")) item.IdRol = Convert.ToInt16(dr["IdRol"]);
					if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
					if (!dr.IsNull("FechaIniVigencia")) item.FechaIniVigencia = Convert.ToDateTime(dr["FechaIniVigencia"]);
					if (!dr.IsNull("FechaFinVigencia")) item.FechaFinVigencia = Convert.ToDateTime(dr["FechaFinVigencia"]);
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdRol")) IdRol = Convert.ToInt16(dtResultado.Rows[0]["IdRol"]);
					if (!dtResultado.Rows[0].IsNull("IdUsuario")) IdUsuario = Convert.ToInt32(dtResultado.Rows[0]["IdUsuario"]);
					if (!dtResultado.Rows[0].IsNull("FechaIniVigencia")) FechaIniVigencia = Convert.ToDateTime(dtResultado.Rows[0]["FechaIniVigencia"]);
					if (!dtResultado.Rows[0].IsNull("FechaFinVigencia")) FechaFinVigencia = Convert.ToDateTime(dtResultado.Rows[0]["FechaFinVigencia"]);
					if (!dtResultado.Rows[0].IsNull("Activo")) Activo = Convert.ToBoolean(dtResultado.Rows[0]["Activo"]);
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

		#region Recuperar RecuperarUsuariosrolesCnfIdusuario
		public List<ENTUsuariosrolesCnf> RecuperarUsuariosrolesCnfIdusuario(int idusuario)
		{
			List<ENTUsuariosrolesCnf> result = new List<ENTUsuariosrolesCnf>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdUsuario", SqlDbType.Int);
			param0.Value = idusuario;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetUsuariosRolesCnf_POR_IdUsuario";
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
				    ENTUsuariosrolesCnf item = new ENTUsuariosrolesCnf();
					if (!dr.IsNull("IdRol")) item.IdRol = Convert.ToInt16(dr["IdRol"]);
					if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
					if (!dr.IsNull("FechaIniVigencia")) item.FechaIniVigencia = Convert.ToDateTime(dr["FechaIniVigencia"]);
					if (!dr.IsNull("FechaFinVigencia")) item.FechaFinVigencia = Convert.ToDateTime(dr["FechaFinVigencia"]);
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdRol")) IdRol = Convert.ToInt16(dtResultado.Rows[0]["IdRol"]);
					if (!dtResultado.Rows[0].IsNull("IdUsuario")) IdUsuario = Convert.ToInt32(dtResultado.Rows[0]["IdUsuario"]);
					if (!dtResultado.Rows[0].IsNull("FechaIniVigencia")) FechaIniVigencia = Convert.ToDateTime(dtResultado.Rows[0]["FechaIniVigencia"]);
					if (!dtResultado.Rows[0].IsNull("FechaFinVigencia")) FechaFinVigencia = Convert.ToDateTime(dtResultado.Rows[0]["FechaFinVigencia"]);
					if (!dtResultado.Rows[0].IsNull("Activo")) Activo = Convert.ToBoolean(dtResultado.Rows[0]["Activo"]);
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

		#region Recuperar RecuperarUsuariosrolesCnfIdrol
		public List<ENTUsuariosrolesCnf> RecuperarUsuariosrolesCnfIdrol(int idrol)
		{
			List<ENTUsuariosrolesCnf> result = new List<ENTUsuariosrolesCnf>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdRol", SqlDbType.SmallInt);
			param0.Value = idrol;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetUsuariosRolesCnf_POR_IdRol";
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
				    ENTUsuariosrolesCnf item = new ENTUsuariosrolesCnf();
					if (!dr.IsNull("IdRol")) item.IdRol = Convert.ToInt16(dr["IdRol"]);
					if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
					if (!dr.IsNull("FechaIniVigencia")) item.FechaIniVigencia = Convert.ToDateTime(dr["FechaIniVigencia"]);
					if (!dr.IsNull("FechaFinVigencia")) item.FechaFinVigencia = Convert.ToDateTime(dr["FechaFinVigencia"]);
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdRol")) IdRol = Convert.ToInt16(dtResultado.Rows[0]["IdRol"]);
					if (!dtResultado.Rows[0].IsNull("IdUsuario")) IdUsuario = Convert.ToInt32(dtResultado.Rows[0]["IdUsuario"]);
					if (!dtResultado.Rows[0].IsNull("FechaIniVigencia")) FechaIniVigencia = Convert.ToDateTime(dtResultado.Rows[0]["FechaIniVigencia"]);
					if (!dtResultado.Rows[0].IsNull("FechaFinVigencia")) FechaFinVigencia = Convert.ToDateTime(dtResultado.Rows[0]["FechaFinVigencia"]);
					if (!dtResultado.Rows[0].IsNull("Activo")) Activo = Convert.ToBoolean(dtResultado.Rows[0]["Activo"]);
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
			_copia = new DALUsuariosrolesCnf(_conexion);
			Type tipo = typeof(ENTUsuariosrolesCnf);
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

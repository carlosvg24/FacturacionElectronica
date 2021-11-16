using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALRolesopcionesCnf: ENTRolesopcionesCnf
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALRolesopcionesCnf _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALRolesopcionesCnf(SqlConnection conexion)
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
			IdMenuOpcion = 0;
			PermisoAgregar = false;
			PermisoEditar = false;
			PermisoEliminar = false;
			PermisoConsultar = false;
			Activo = true;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_RolesOpciones_Cnf
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdRol", SqlDbType.SmallInt);
			param0.Value = IdRol;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdMenuOpcion", SqlDbType.SmallInt);
			param1.Value = IdMenuOpcion;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@PermisoAgregar", SqlDbType.Bit);
			param2.Value = PermisoAgregar;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@PermisoEditar", SqlDbType.Bit);
			param3.Value = PermisoEditar;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@PermisoEliminar", SqlDbType.Bit);
			param4.Value = PermisoEliminar;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@PermisoConsultar", SqlDbType.Bit);
			param5.Value = PermisoConsultar;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Activo", SqlDbType.Bit);
			param6.Value = Activo;
			commandParameters.Add(param6);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsRolesOpcionesCnf";
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

		#region Actualizar VBFac_RolesOpciones_Cnf
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdRol", SqlDbType.SmallInt);
			param0.Value = IdRol;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdMenuOpcion", SqlDbType.SmallInt);
			param1.Value = IdMenuOpcion;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@PermisoAgregar", SqlDbType.Bit);
			param2.Value = PermisoAgregar;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@PermisoEditar", SqlDbType.Bit);
			param3.Value = PermisoEditar;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@PermisoEliminar", SqlDbType.Bit);
			param4.Value = PermisoEliminar;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@PermisoConsultar", SqlDbType.Bit);
			param5.Value = PermisoConsultar;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Activo", SqlDbType.Bit);
			param6.Value = Activo;
			commandParameters.Add(param6);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdRolesOpcionesCnf";
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

		#region Eliminar VBFac_RolesOpciones_Cnf
		public void Eliminar(int idmenuopcion,int idrol)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdMenuOpcion", SqlDbType.SmallInt);
			param0.Value = idmenuopcion;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdRol", SqlDbType.SmallInt);
			param1.Value = idrol;
			commandParameters.Add(param1);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelRolesOpcionesCnf";
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

		#region Deshacer VBFac_RolesOpciones_Cnf
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdMenuOpcion,IdRol);
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
		public List<ENTRolesopcionesCnf> RecuperarTodo()
		{
			List<ENTRolesopcionesCnf> result = new List<ENTRolesopcionesCnf>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetRolesOpcionesCnf_TODO";
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
				    ENTRolesopcionesCnf item = new ENTRolesopcionesCnf();
					 if (!dr.IsNull("IdRol")) item.IdRol = Convert.ToInt16(dr["IdRol"]);
					 if (!dr.IsNull("IdMenuOpcion")) item.IdMenuOpcion = Convert.ToInt16(dr["IdMenuOpcion"]);
					 if (!dr.IsNull("PermisoAgregar")) item.PermisoAgregar = Convert.ToBoolean(dr["PermisoAgregar"]);
					 if (!dr.IsNull("PermisoEditar")) item.PermisoEditar = Convert.ToBoolean(dr["PermisoEditar"]);
					 if (!dr.IsNull("PermisoEliminar")) item.PermisoEliminar = Convert.ToBoolean(dr["PermisoEliminar"]);
					 if (!dr.IsNull("PermisoConsultar")) item.PermisoConsultar = Convert.ToBoolean(dr["PermisoConsultar"]);
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

		#region Recuperar RecuperarRolesopcionesCnfPorLlavePrimaria
		public List<ENTRolesopcionesCnf> RecuperarRolesopcionesCnfPorLlavePrimaria(int idrol,int idmenuopcion)
		{
			List<ENTRolesopcionesCnf> result = new List<ENTRolesopcionesCnf>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdRol", SqlDbType.SmallInt);
			param0.Value = idrol;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdMenuOpcion", SqlDbType.SmallInt);
			param1.Value = idmenuopcion;
			commandParameters.Add(param1);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetRolesOpcionesCnf_POR_PK";
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
				    ENTRolesopcionesCnf item = new ENTRolesopcionesCnf();
					if (!dr.IsNull("IdRol")) item.IdRol = Convert.ToInt16(dr["IdRol"]);
					if (!dr.IsNull("IdMenuOpcion")) item.IdMenuOpcion = Convert.ToInt16(dr["IdMenuOpcion"]);
					if (!dr.IsNull("PermisoAgregar")) item.PermisoAgregar = Convert.ToBoolean(dr["PermisoAgregar"]);
					if (!dr.IsNull("PermisoEditar")) item.PermisoEditar = Convert.ToBoolean(dr["PermisoEditar"]);
					if (!dr.IsNull("PermisoEliminar")) item.PermisoEliminar = Convert.ToBoolean(dr["PermisoEliminar"]);
					if (!dr.IsNull("PermisoConsultar")) item.PermisoConsultar = Convert.ToBoolean(dr["PermisoConsultar"]);
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdRol")) IdRol = Convert.ToInt16(dtResultado.Rows[0]["IdRol"]);
					if (!dtResultado.Rows[0].IsNull("IdMenuOpcion")) IdMenuOpcion = Convert.ToInt16(dtResultado.Rows[0]["IdMenuOpcion"]);
					if (!dtResultado.Rows[0].IsNull("PermisoAgregar")) PermisoAgregar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoAgregar"]);
					if (!dtResultado.Rows[0].IsNull("PermisoEditar")) PermisoEditar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoEditar"]);
					if (!dtResultado.Rows[0].IsNull("PermisoEliminar")) PermisoEliminar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoEliminar"]);
					if (!dtResultado.Rows[0].IsNull("PermisoConsultar")) PermisoConsultar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoConsultar"]);
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

		#region Recuperar RecuperarRolesopcionesCnfIdrol
		public List<ENTRolesopcionesCnf> RecuperarRolesopcionesCnfIdrol(int idrol)
		{
			List<ENTRolesopcionesCnf> result = new List<ENTRolesopcionesCnf>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdRol", SqlDbType.SmallInt);
			param0.Value = idrol;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetRolesOpcionesCnf_POR_IdRol";
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
				    ENTRolesopcionesCnf item = new ENTRolesopcionesCnf();
					if (!dr.IsNull("IdRol")) item.IdRol = Convert.ToInt16(dr["IdRol"]);
					if (!dr.IsNull("IdMenuOpcion")) item.IdMenuOpcion = Convert.ToInt16(dr["IdMenuOpcion"]);
					if (!dr.IsNull("PermisoAgregar")) item.PermisoAgregar = Convert.ToBoolean(dr["PermisoAgregar"]);
					if (!dr.IsNull("PermisoEditar")) item.PermisoEditar = Convert.ToBoolean(dr["PermisoEditar"]);
					if (!dr.IsNull("PermisoEliminar")) item.PermisoEliminar = Convert.ToBoolean(dr["PermisoEliminar"]);
					if (!dr.IsNull("PermisoConsultar")) item.PermisoConsultar = Convert.ToBoolean(dr["PermisoConsultar"]);
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdRol")) IdRol = Convert.ToInt16(dtResultado.Rows[0]["IdRol"]);
					if (!dtResultado.Rows[0].IsNull("IdMenuOpcion")) IdMenuOpcion = Convert.ToInt16(dtResultado.Rows[0]["IdMenuOpcion"]);
					if (!dtResultado.Rows[0].IsNull("PermisoAgregar")) PermisoAgregar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoAgregar"]);
					if (!dtResultado.Rows[0].IsNull("PermisoEditar")) PermisoEditar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoEditar"]);
					if (!dtResultado.Rows[0].IsNull("PermisoEliminar")) PermisoEliminar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoEliminar"]);
					if (!dtResultado.Rows[0].IsNull("PermisoConsultar")) PermisoConsultar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoConsultar"]);
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

		#region Recuperar RecuperarRolesopcionesCnfIdmenuopcion
		public List<ENTRolesopcionesCnf> RecuperarRolesopcionesCnfIdmenuopcion(int idmenuopcion)
		{
			List<ENTRolesopcionesCnf> result = new List<ENTRolesopcionesCnf>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdMenuOpcion", SqlDbType.SmallInt);
			param0.Value = idmenuopcion;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetRolesOpcionesCnf_POR_IdMenuOpcion";
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
				    ENTRolesopcionesCnf item = new ENTRolesopcionesCnf();
					if (!dr.IsNull("IdRol")) item.IdRol = Convert.ToInt16(dr["IdRol"]);
					if (!dr.IsNull("IdMenuOpcion")) item.IdMenuOpcion = Convert.ToInt16(dr["IdMenuOpcion"]);
					if (!dr.IsNull("PermisoAgregar")) item.PermisoAgregar = Convert.ToBoolean(dr["PermisoAgregar"]);
					if (!dr.IsNull("PermisoEditar")) item.PermisoEditar = Convert.ToBoolean(dr["PermisoEditar"]);
					if (!dr.IsNull("PermisoEliminar")) item.PermisoEliminar = Convert.ToBoolean(dr["PermisoEliminar"]);
					if (!dr.IsNull("PermisoConsultar")) item.PermisoConsultar = Convert.ToBoolean(dr["PermisoConsultar"]);
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdRol")) IdRol = Convert.ToInt16(dtResultado.Rows[0]["IdRol"]);
					if (!dtResultado.Rows[0].IsNull("IdMenuOpcion")) IdMenuOpcion = Convert.ToInt16(dtResultado.Rows[0]["IdMenuOpcion"]);
					if (!dtResultado.Rows[0].IsNull("PermisoAgregar")) PermisoAgregar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoAgregar"]);
					if (!dtResultado.Rows[0].IsNull("PermisoEditar")) PermisoEditar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoEditar"]);
					if (!dtResultado.Rows[0].IsNull("PermisoEliminar")) PermisoEliminar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoEliminar"]);
					if (!dtResultado.Rows[0].IsNull("PermisoConsultar")) PermisoConsultar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoConsultar"]);
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
			_copia = new DALRolesopcionesCnf(_conexion);
			Type tipo = typeof(ENTRolesopcionesCnf);
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

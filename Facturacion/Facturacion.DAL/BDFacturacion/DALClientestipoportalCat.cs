using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALClientestipoportalCat: ENTClientestipoportalCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALClientestipoportalCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALClientestipoportalCat(SqlConnection conexion)
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
			Id = 0;
			Nombre = String.Empty;
			Descripcion = String.Empty;
			UsuarioTipoActivo = false;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_ClientesTipoPortal_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@Id", SqlDbType.Int);
			param0.Value = Id;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@Nombre", SqlDbType.VarChar);
			param1.Value = Nombre;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param2.Value = Descripcion;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@UsuarioTipoActivo", SqlDbType.Bit);
			param3.Value = UsuarioTipoActivo;
			commandParameters.Add(param3);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsClientesTipoPortalCat";
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
						Id = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_ClientesTipoPortal_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@Id", SqlDbType.Int);
			param0.Value = Id;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@Nombre", SqlDbType.VarChar);
			param1.Value = Nombre;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param2.Value = Descripcion;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@UsuarioTipoActivo", SqlDbType.Bit);
			param3.Value = UsuarioTipoActivo;
			commandParameters.Add(param3);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdClientesTipoPortalCat";
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

		#region Eliminar VBFac_ClientesTipoPortal_Cat
		public void Eliminar(int id)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@Id", SqlDbType.Int);
			param0.Value = id;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelClientesTipoPortalCat";
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

		#region Deshacer VBFac_ClientesTipoPortal_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(Id);
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
		public List<ENTClientestipoportalCat> RecuperarTodo()
		{
			List<ENTClientestipoportalCat> result = new List<ENTClientestipoportalCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetClientesTipoPortalCat_TODO";
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
				    ENTClientestipoportalCat item = new ENTClientestipoportalCat();
					 if (!dr.IsNull("Id")) item.Id = Convert.ToInt32(dr["Id"]);
					 if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					 if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					 if (!dr.IsNull("UsuarioTipoActivo")) item.UsuarioTipoActivo = Convert.ToBoolean(dr["UsuarioTipoActivo"]);
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

		#region Recuperar RecuperarClientestipoportalCatPorLlavePrimaria
		public List<ENTClientestipoportalCat> RecuperarClientestipoportalCatPorLlavePrimaria(int id)
		{
			List<ENTClientestipoportalCat> result = new List<ENTClientestipoportalCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@Id", SqlDbType.Int);
			param0.Value = id;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetClientesTipoPortalCat_POR_PK";
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
				    ENTClientestipoportalCat item = new ENTClientestipoportalCat();
					if (!dr.IsNull("Id")) item.Id = Convert.ToInt32(dr["Id"]);
					if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					if (!dr.IsNull("UsuarioTipoActivo")) item.UsuarioTipoActivo = Convert.ToBoolean(dr["UsuarioTipoActivo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("Id")) Id = Convert.ToInt32(dtResultado.Rows[0]["Id"]);
					if (!dtResultado.Rows[0].IsNull("Nombre")) Nombre = dtResultado.Rows[0]["Nombre"].ToString();
					if (!dtResultado.Rows[0].IsNull("Descripcion")) Descripcion = dtResultado.Rows[0]["Descripcion"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsuarioTipoActivo")) UsuarioTipoActivo = Convert.ToBoolean(dtResultado.Rows[0]["UsuarioTipoActivo"]);
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

		#region Recuperar RecuperarClientestipoportalCatNombre
		public List<ENTClientestipoportalCat> RecuperarClientestipoportalCatNombre(string nombre)
		{
			List<ENTClientestipoportalCat> result = new List<ENTClientestipoportalCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@Nombre", SqlDbType.VarChar);
			param0.Value = nombre;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetClientesTipoPortalCat_POR_Nombre";
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
				    ENTClientestipoportalCat item = new ENTClientestipoportalCat();
					if (!dr.IsNull("Id")) item.Id = Convert.ToInt32(dr["Id"]);
					if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					if (!dr.IsNull("UsuarioTipoActivo")) item.UsuarioTipoActivo = Convert.ToBoolean(dr["UsuarioTipoActivo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("Id")) Id = Convert.ToInt32(dtResultado.Rows[0]["Id"]);
					if (!dtResultado.Rows[0].IsNull("Nombre")) Nombre = dtResultado.Rows[0]["Nombre"].ToString();
					if (!dtResultado.Rows[0].IsNull("Descripcion")) Descripcion = dtResultado.Rows[0]["Descripcion"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsuarioTipoActivo")) UsuarioTipoActivo = Convert.ToBoolean(dtResultado.Rows[0]["UsuarioTipoActivo"]);
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
			_copia = new DALClientestipoportalCat(_conexion);
			Type tipo = typeof(ENTClientestipoportalCat);
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

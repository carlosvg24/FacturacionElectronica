using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALOpcionesmenuCat: ENTOpcionesmenuCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALOpcionesmenuCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALOpcionesmenuCat(SqlConnection conexion)
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
			IdMenuOpcion = 0;
			IdMenuPadre = 0;
			Nombre = String.Empty;
			Descripcion = String.Empty;
			UrlMenu = String.Empty;
			UrlImagen = String.Empty;
			Orden = 0;
			Activo = true;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_OpcionesMenu_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdMenuOpcion", SqlDbType.SmallInt);
			param0.Value = IdMenuOpcion;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdMenuPadre", SqlDbType.SmallInt);
			param1.Value = IdMenuPadre;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Nombre", SqlDbType.VarChar);
			param2.Value = Nombre;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param3.Value = Descripcion;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@UrlMenu", SqlDbType.VarChar);
			param4.Value = UrlMenu;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@UrlImagen", SqlDbType.VarChar);
			param5.Value = UrlImagen;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Orden", SqlDbType.Int);
			param6.Value = Orden;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@Activo", SqlDbType.Bit);
			param7.Value = Activo;
			commandParameters.Add(param7);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsOpcionesMenuCat";
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
						IdMenuOpcion = Convert.ToInt16(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_OpcionesMenu_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdMenuOpcion", SqlDbType.SmallInt);
			param0.Value = IdMenuOpcion;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdMenuPadre", SqlDbType.SmallInt);
			param1.Value = IdMenuPadre;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Nombre", SqlDbType.VarChar);
			param2.Value = Nombre;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param3.Value = Descripcion;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@UrlMenu", SqlDbType.VarChar);
			param4.Value = UrlMenu;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@UrlImagen", SqlDbType.VarChar);
			param5.Value = UrlImagen;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Orden", SqlDbType.Int);
			param6.Value = Orden;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@Activo", SqlDbType.Bit);
			param7.Value = Activo;
			commandParameters.Add(param7);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdOpcionesMenuCat";
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

		#region Eliminar VBFac_OpcionesMenu_Cat
		public void Eliminar(int idmenuopcion)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdMenuOpcion", SqlDbType.SmallInt);
			param0.Value = idmenuopcion;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelOpcionesMenuCat";
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

		#region Deshacer VBFac_OpcionesMenu_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdMenuOpcion);
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
		public List<ENTOpcionesmenuCat> RecuperarTodo()
		{
			List<ENTOpcionesmenuCat> result = new List<ENTOpcionesmenuCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetOpcionesMenuCat_TODO";
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
				    ENTOpcionesmenuCat item = new ENTOpcionesmenuCat();
					 if (!dr.IsNull("IdMenuOpcion")) item.IdMenuOpcion = Convert.ToInt16(dr["IdMenuOpcion"]);
					 if (!dr.IsNull("IdMenuPadre")) item.IdMenuPadre = Convert.ToInt16(dr["IdMenuPadre"]);
					 if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					 if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					 if (!dr.IsNull("UrlMenu")) item.UrlMenu = dr["UrlMenu"].ToString();
					 if (!dr.IsNull("UrlImagen")) item.UrlImagen = dr["UrlImagen"].ToString();
					 if (!dr.IsNull("Orden")) item.Orden = Convert.ToInt32(dr["Orden"]);
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

		#region Recuperar RecuperarOpcionesmenuCatPorLlavePrimaria
		public List<ENTOpcionesmenuCat> RecuperarOpcionesmenuCatPorLlavePrimaria(int idmenuopcion)
		{
			List<ENTOpcionesmenuCat> result = new List<ENTOpcionesmenuCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdMenuOpcion", SqlDbType.SmallInt);
			param0.Value = idmenuopcion;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetOpcionesMenuCat_POR_PK";
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
				    ENTOpcionesmenuCat item = new ENTOpcionesmenuCat();
					if (!dr.IsNull("IdMenuOpcion")) item.IdMenuOpcion = Convert.ToInt16(dr["IdMenuOpcion"]);
					if (!dr.IsNull("IdMenuPadre")) item.IdMenuPadre = Convert.ToInt16(dr["IdMenuPadre"]);
					if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					if (!dr.IsNull("UrlMenu")) item.UrlMenu = dr["UrlMenu"].ToString();
					if (!dr.IsNull("UrlImagen")) item.UrlImagen = dr["UrlImagen"].ToString();
					if (!dr.IsNull("Orden")) item.Orden = Convert.ToInt32(dr["Orden"]);
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdMenuOpcion")) IdMenuOpcion = Convert.ToInt16(dtResultado.Rows[0]["IdMenuOpcion"]);
					if (!dtResultado.Rows[0].IsNull("IdMenuPadre")) IdMenuPadre = Convert.ToInt16(dtResultado.Rows[0]["IdMenuPadre"]);
					if (!dtResultado.Rows[0].IsNull("Nombre")) Nombre = dtResultado.Rows[0]["Nombre"].ToString();
					if (!dtResultado.Rows[0].IsNull("Descripcion")) Descripcion = dtResultado.Rows[0]["Descripcion"].ToString();
					if (!dtResultado.Rows[0].IsNull("UrlMenu")) UrlMenu = dtResultado.Rows[0]["UrlMenu"].ToString();
					if (!dtResultado.Rows[0].IsNull("UrlImagen")) UrlImagen = dtResultado.Rows[0]["UrlImagen"].ToString();
					if (!dtResultado.Rows[0].IsNull("Orden")) Orden = Convert.ToInt32(dtResultado.Rows[0]["Orden"]);
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
			_copia = new DALOpcionesmenuCat(_conexion);
			Type tipo = typeof(ENTOpcionesmenuCat);
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

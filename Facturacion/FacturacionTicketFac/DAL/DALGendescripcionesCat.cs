using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace FacturacionTicketFac
{
    public class DALGendescripcionesCat: ENTGendescripcionesCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALGendescripcionesCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALGendescripcionesCat(SqlConnection conexion)
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
			IdGenDescripcion = 0;
			CveTabla = String.Empty;
			CveValor = String.Empty;
			Descripcion = String.Empty;
			Activo = true;
			UsuarioRegistro = 0;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_GenDescripciones_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdGenDescripcion", SqlDbType.Int);
			param0.Value = IdGenDescripcion;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@CveTabla", SqlDbType.VarChar);
			param1.Value = CveTabla;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@CveValor", SqlDbType.VarChar);
			param2.Value = CveValor;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param3.Value = Descripcion;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Activo", SqlDbType.Bit);
			param4.Value = Activo;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@UsuarioRegistro", SqlDbType.Int);
			param5.Value = UsuarioRegistro;
			commandParameters.Add(param5);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsGenDescripcionesCat";
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
						IdGenDescripcion = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_GenDescripciones_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdGenDescripcion", SqlDbType.Int);
			param0.Value = IdGenDescripcion;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@CveTabla", SqlDbType.VarChar);
			param1.Value = CveTabla;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@CveValor", SqlDbType.VarChar);
			param2.Value = CveValor;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param3.Value = Descripcion;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Activo", SqlDbType.Bit);
			param4.Value = Activo;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@UsuarioRegistro", SqlDbType.Int);
			param5.Value = UsuarioRegistro;
			commandParameters.Add(param5);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdGenDescripcionesCat";
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

		#region Eliminar VBFac_GenDescripciones_Cat
		public void Eliminar(int idgendescripcion)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdGenDescripcion", SqlDbType.Int);
			param0.Value = idgendescripcion;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelGenDescripcionesCat";
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

		#region Deshacer VBFac_GenDescripciones_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdGenDescripcion);
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
		public List<ENTGendescripcionesCat> RecuperarTodo()
		{
			List<ENTGendescripcionesCat> result = new List<ENTGendescripcionesCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetGenDescripcionesCat_TODO";
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
				    ENTGendescripcionesCat item = new ENTGendescripcionesCat();
					 if (!dr.IsNull("IdGenDescripcion")) item.IdGenDescripcion = Convert.ToInt32(dr["IdGenDescripcion"]);
					 if (!dr.IsNull("CveTabla")) item.CveTabla = dr["CveTabla"].ToString();
					 if (!dr.IsNull("CveValor")) item.CveValor = dr["CveValor"].ToString();
					 if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					 if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					 if (!dr.IsNull("UsuarioRegistro")) item.UsuarioRegistro = Convert.ToInt32(dr["UsuarioRegistro"]);
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

		#region Recuperar RecuperarGendescripcionesCatPorLlavePrimaria
		public List<ENTGendescripcionesCat> RecuperarGendescripcionesCatPorLlavePrimaria(int idgendescripcion)
		{
			List<ENTGendescripcionesCat> result = new List<ENTGendescripcionesCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdGenDescripcion", SqlDbType.Int);
			param0.Value = idgendescripcion;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetGenDescripcionesCat_POR_PK";
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
				    ENTGendescripcionesCat item = new ENTGendescripcionesCat();
					if (!dr.IsNull("IdGenDescripcion")) item.IdGenDescripcion = Convert.ToInt32(dr["IdGenDescripcion"]);
					if (!dr.IsNull("CveTabla")) item.CveTabla = dr["CveTabla"].ToString();
					if (!dr.IsNull("CveValor")) item.CveValor = dr["CveValor"].ToString();
					if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("UsuarioRegistro")) item.UsuarioRegistro = Convert.ToInt32(dr["UsuarioRegistro"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdGenDescripcion")) IdGenDescripcion = Convert.ToInt32(dtResultado.Rows[0]["IdGenDescripcion"]);
					if (!dtResultado.Rows[0].IsNull("CveTabla")) CveTabla = dtResultado.Rows[0]["CveTabla"].ToString();
					if (!dtResultado.Rows[0].IsNull("CveValor")) CveValor = dtResultado.Rows[0]["CveValor"].ToString();
					if (!dtResultado.Rows[0].IsNull("Descripcion")) Descripcion = dtResultado.Rows[0]["Descripcion"].ToString();
					if (!dtResultado.Rows[0].IsNull("Activo")) Activo = Convert.ToBoolean(dtResultado.Rows[0]["Activo"]);
					if (!dtResultado.Rows[0].IsNull("UsuarioRegistro")) UsuarioRegistro = Convert.ToInt32(dtResultado.Rows[0]["UsuarioRegistro"]);
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

		#region Recuperar RecuperarGendescripcionesCatCvetabla
		public List<ENTGendescripcionesCat> RecuperarGendescripcionesCatCvetabla(string cvetabla)
		{
			List<ENTGendescripcionesCat> result = new List<ENTGendescripcionesCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@CveTabla", SqlDbType.VarChar);
			param0.Value = cvetabla;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetGenDescripcionesCat_POR_CveTabla";
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
				    ENTGendescripcionesCat item = new ENTGendescripcionesCat();
					if (!dr.IsNull("IdGenDescripcion")) item.IdGenDescripcion = Convert.ToInt32(dr["IdGenDescripcion"]);
					if (!dr.IsNull("CveTabla")) item.CveTabla = dr["CveTabla"].ToString();
					if (!dr.IsNull("CveValor")) item.CveValor = dr["CveValor"].ToString();
					if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("UsuarioRegistro")) item.UsuarioRegistro = Convert.ToInt32(dr["UsuarioRegistro"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdGenDescripcion")) IdGenDescripcion = Convert.ToInt32(dtResultado.Rows[0]["IdGenDescripcion"]);
					if (!dtResultado.Rows[0].IsNull("CveTabla")) CveTabla = dtResultado.Rows[0]["CveTabla"].ToString();
					if (!dtResultado.Rows[0].IsNull("CveValor")) CveValor = dtResultado.Rows[0]["CveValor"].ToString();
					if (!dtResultado.Rows[0].IsNull("Descripcion")) Descripcion = dtResultado.Rows[0]["Descripcion"].ToString();
					if (!dtResultado.Rows[0].IsNull("Activo")) Activo = Convert.ToBoolean(dtResultado.Rows[0]["Activo"]);
					if (!dtResultado.Rows[0].IsNull("UsuarioRegistro")) UsuarioRegistro = Convert.ToInt32(dtResultado.Rows[0]["UsuarioRegistro"]);
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
			_copia = new DALGendescripcionesCat(_conexion);
			Type tipo = typeof(ENTGendescripcionesCat);
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

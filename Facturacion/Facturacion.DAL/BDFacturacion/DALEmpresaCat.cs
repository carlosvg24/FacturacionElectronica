using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALEmpresaCat: ENTEmpresaCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALEmpresaCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALEmpresaCat(SqlConnection conexion)
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
			IdEmpresa = 0;
			IdRegimenFiscal = String.Empty;
			Rfc = String.Empty;
			RazonSocial = String.Empty;
			NoCertificado = String.Empty;
			LugarExpedicion = String.Empty;
			Activo = true;
			IdUsuarioRegistro = 0;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_Empresa_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdEmpresa", SqlDbType.TinyInt);
			param0.Value = IdEmpresa;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdRegimenFiscal", SqlDbType.VarChar);
			param1.Value = IdRegimenFiscal;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Rfc", SqlDbType.VarChar);
			param2.Value = Rfc;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@RazonSocial", SqlDbType.VarChar);
			param3.Value = RazonSocial;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@NoCertificado", SqlDbType.VarChar);
			param4.Value = NoCertificado;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@LugarExpedicion", SqlDbType.VarChar);
			param5.Value = LugarExpedicion;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Activo", SqlDbType.Bit);
			param6.Value = Activo;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@IdUsuarioRegistro", SqlDbType.Int);
			param7.Value = IdUsuarioRegistro;
			commandParameters.Add(param7);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsEmpresaCat";
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
						IdEmpresa = Convert.ToByte(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_Empresa_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdEmpresa", SqlDbType.TinyInt);
			param0.Value = IdEmpresa;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdRegimenFiscal", SqlDbType.VarChar);
			param1.Value = IdRegimenFiscal;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Rfc", SqlDbType.VarChar);
			param2.Value = Rfc;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@RazonSocial", SqlDbType.VarChar);
			param3.Value = RazonSocial;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@NoCertificado", SqlDbType.VarChar);
			param4.Value = NoCertificado;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@LugarExpedicion", SqlDbType.VarChar);
			param5.Value = LugarExpedicion;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Activo", SqlDbType.Bit);
			param6.Value = Activo;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@IdUsuarioRegistro", SqlDbType.Int);
			param7.Value = IdUsuarioRegistro;
			commandParameters.Add(param7);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdEmpresaCat";
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

		#region Eliminar VBFac_Empresa_Cat
		public void Eliminar(byte idempresa)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdEmpresa", SqlDbType.TinyInt);
			param0.Value = idempresa;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelEmpresaCat";
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

		#region Deshacer VBFac_Empresa_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdEmpresa);
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
		public List<ENTEmpresaCat> RecuperarTodo()
		{
			List<ENTEmpresaCat> result = new List<ENTEmpresaCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetEmpresaCat_TODO";
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
				    ENTEmpresaCat item = new ENTEmpresaCat();
					 if (!dr.IsNull("IdEmpresa")) item.IdEmpresa = Convert.ToByte(dr["IdEmpresa"]);
					 if (!dr.IsNull("IdRegimenFiscal")) item.IdRegimenFiscal = dr["IdRegimenFiscal"].ToString();
					 if (!dr.IsNull("Rfc")) item.Rfc = dr["Rfc"].ToString();
					 if (!dr.IsNull("RazonSocial")) item.RazonSocial = dr["RazonSocial"].ToString();
					 if (!dr.IsNull("NoCertificado")) item.NoCertificado = dr["NoCertificado"].ToString();
					 if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					 if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					 if (!dr.IsNull("IdUsuarioRegistro")) item.IdUsuarioRegistro = Convert.ToInt32(dr["IdUsuarioRegistro"]);
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

		#region Recuperar RecuperarEmpresaCatPorLlavePrimaria
		public List<ENTEmpresaCat> RecuperarEmpresaCatPorLlavePrimaria(byte idempresa)
		{
			List<ENTEmpresaCat> result = new List<ENTEmpresaCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdEmpresa", SqlDbType.TinyInt);
			param0.Value = idempresa;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetEmpresaCat_POR_PK";
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
				    ENTEmpresaCat item = new ENTEmpresaCat();
					if (!dr.IsNull("IdEmpresa")) item.IdEmpresa = Convert.ToByte(dr["IdEmpresa"]);
					if (!dr.IsNull("IdRegimenFiscal")) item.IdRegimenFiscal = dr["IdRegimenFiscal"].ToString();
					if (!dr.IsNull("Rfc")) item.Rfc = dr["Rfc"].ToString();
					if (!dr.IsNull("RazonSocial")) item.RazonSocial = dr["RazonSocial"].ToString();
					if (!dr.IsNull("NoCertificado")) item.NoCertificado = dr["NoCertificado"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("IdUsuarioRegistro")) item.IdUsuarioRegistro = Convert.ToInt32(dr["IdUsuarioRegistro"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdEmpresa")) IdEmpresa = Convert.ToByte(dtResultado.Rows[0]["IdEmpresa"]);
					if (!dtResultado.Rows[0].IsNull("IdRegimenFiscal")) IdRegimenFiscal = dtResultado.Rows[0]["IdRegimenFiscal"].ToString();
					if (!dtResultado.Rows[0].IsNull("Rfc")) Rfc = dtResultado.Rows[0]["Rfc"].ToString();
					if (!dtResultado.Rows[0].IsNull("RazonSocial")) RazonSocial = dtResultado.Rows[0]["RazonSocial"].ToString();
					if (!dtResultado.Rows[0].IsNull("NoCertificado")) NoCertificado = dtResultado.Rows[0]["NoCertificado"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
					if (!dtResultado.Rows[0].IsNull("Activo")) Activo = Convert.ToBoolean(dtResultado.Rows[0]["Activo"]);
					if (!dtResultado.Rows[0].IsNull("IdUsuarioRegistro")) IdUsuarioRegistro = Convert.ToInt32(dtResultado.Rows[0]["IdUsuarioRegistro"]);
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
			_copia = new DALEmpresaCat(_conexion);
			Type tipo = typeof(ENTEmpresaCat);
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

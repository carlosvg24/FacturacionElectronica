using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALCfdirelacionadosDet: ENTCfdirelacionadosDet
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALCfdirelacionadosDet _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALCfdirelacionadosDet(SqlConnection conexion)
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
			IdCfdiRel = 0;
			IdCFDI = 0;
			IdCFDIVinculado = 0;
			UUIDVinculado = String.Empty;
			TipoComprobante = String.Empty;
			TipoRelacion = String.Empty;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_CFDIRelacionados_Det
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdCfdiRel", SqlDbType.BigInt);
			param0.Value = IdCfdiRel;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdCFDI", SqlDbType.BigInt);
			param1.Value = IdCFDI;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@IdCFDIVinculado", SqlDbType.BigInt);
			param2.Value = IdCFDIVinculado;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@UUIDVinculado", SqlDbType.VarChar);
			param3.Value = UUIDVinculado;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@TipoComprobante", SqlDbType.VarChar);
			param4.Value = TipoComprobante;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@TipoRelacion", SqlDbType.VarChar);
			param5.Value = TipoRelacion;
			commandParameters.Add(param5);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsCFDIRelacionadosDet";
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
						IdCfdiRel = Convert.ToInt64(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_CFDIRelacionados_Det
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdCfdiRel", SqlDbType.BigInt);
			param0.Value = IdCfdiRel;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdCFDI", SqlDbType.BigInt);
			param1.Value = IdCFDI;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@IdCFDIVinculado", SqlDbType.BigInt);
			param2.Value = IdCFDIVinculado;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@UUIDVinculado", SqlDbType.VarChar);
			param3.Value = UUIDVinculado;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@TipoComprobante", SqlDbType.VarChar);
			param4.Value = TipoComprobante;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@TipoRelacion", SqlDbType.VarChar);
			param5.Value = TipoRelacion;
			commandParameters.Add(param5);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdCFDIRelacionadosDet";
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

		#region Eliminar VBFac_CFDIRelacionados_Det
		public void Eliminar(long idcfdirel)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdCfdiRel", SqlDbType.BigInt);
			param0.Value = idcfdirel;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelCFDIRelacionadosDet";
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

		#region Deshacer VBFac_CFDIRelacionados_Det
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdCfdiRel);
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
		public List<ENTCfdirelacionadosDet> RecuperarTodo()
		{
			List<ENTCfdirelacionadosDet> result = new List<ENTCfdirelacionadosDet>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetCFDIRelacionadosDet_TODO";
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
				    ENTCfdirelacionadosDet item = new ENTCfdirelacionadosDet();
					 if (!dr.IsNull("IdCfdiRel")) item.IdCfdiRel = Convert.ToInt64(dr["IdCfdiRel"]);
					 if (!dr.IsNull("IdCFDI")) item.IdCFDI = Convert.ToInt64(dr["IdCFDI"]);
					 if (!dr.IsNull("IdCFDIVinculado")) item.IdCFDIVinculado = Convert.ToInt64(dr["IdCFDIVinculado"]);
					 if (!dr.IsNull("UUIDVinculado")) item.UUIDVinculado = dr["UUIDVinculado"].ToString();
					 if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
					 if (!dr.IsNull("TipoRelacion")) item.TipoRelacion = dr["TipoRelacion"].ToString();
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

		#region Recuperar RecuperarCfdirelacionadosDetPorLlavePrimaria
		public List<ENTCfdirelacionadosDet> RecuperarCfdirelacionadosDetPorLlavePrimaria(long idcfdirel)
		{
			List<ENTCfdirelacionadosDet> result = new List<ENTCfdirelacionadosDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdCfdiRel", SqlDbType.BigInt);
			param0.Value = idcfdirel;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetCFDIRelacionadosDet_POR_PK";
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
				    ENTCfdirelacionadosDet item = new ENTCfdirelacionadosDet();
					if (!dr.IsNull("IdCfdiRel")) item.IdCfdiRel = Convert.ToInt64(dr["IdCfdiRel"]);
					if (!dr.IsNull("IdCFDI")) item.IdCFDI = Convert.ToInt64(dr["IdCFDI"]);
					if (!dr.IsNull("IdCFDIVinculado")) item.IdCFDIVinculado = Convert.ToInt64(dr["IdCFDIVinculado"]);
					if (!dr.IsNull("UUIDVinculado")) item.UUIDVinculado = dr["UUIDVinculado"].ToString();
					if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
					if (!dr.IsNull("TipoRelacion")) item.TipoRelacion = dr["TipoRelacion"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdCfdiRel")) IdCfdiRel = Convert.ToInt64(dtResultado.Rows[0]["IdCfdiRel"]);
					if (!dtResultado.Rows[0].IsNull("IdCFDI")) IdCFDI = Convert.ToInt64(dtResultado.Rows[0]["IdCFDI"]);
					if (!dtResultado.Rows[0].IsNull("IdCFDIVinculado")) IdCFDIVinculado = Convert.ToInt64(dtResultado.Rows[0]["IdCFDIVinculado"]);
					if (!dtResultado.Rows[0].IsNull("UUIDVinculado")) UUIDVinculado = dtResultado.Rows[0]["UUIDVinculado"].ToString();
					if (!dtResultado.Rows[0].IsNull("TipoComprobante")) TipoComprobante = dtResultado.Rows[0]["TipoComprobante"].ToString();
					if (!dtResultado.Rows[0].IsNull("TipoRelacion")) TipoRelacion = dtResultado.Rows[0]["TipoRelacion"].ToString();
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
			_copia = new DALCfdirelacionadosDet(_conexion);
			Type tipo = typeof(ENTCfdirelacionadosDet);
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

using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALPagosomitidosglobalporiva: ENTPagosomitidosglobalporiva
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALPagosomitidosglobalporiva _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALPagosomitidosglobalporiva(SqlConnection conexion)
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
			IdPagosCab = 0;
			FechaFacturaGlobal = new DateTime();
			BaseIVA = 0M;
			IVANavitaire = 0M;
			IVAReal = 0M;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_PagosOmitidosGlobalPorIVA
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = IdPagosCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@FechaFacturaGlobal", SqlDbType.VarChar);
			param1.Value = FechaFacturaGlobal.Year > 1900 ? FechaFacturaGlobal.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@BaseIVA", SqlDbType.Money);
			param2.Value = BaseIVA;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@IVANavitaire", SqlDbType.Money);
			param3.Value = IVANavitaire;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@IVAReal", SqlDbType.Money);
			param4.Value = IVAReal;
			commandParameters.Add(param4);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsPagosOmitidosGlobalPorIVA";
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

		#region Actualizar VBFac_PagosOmitidosGlobalPorIVA
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = IdPagosCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@FechaFacturaGlobal", SqlDbType.VarChar);
			param1.Value = FechaFacturaGlobal.Year > 1900 ? FechaFacturaGlobal.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@BaseIVA", SqlDbType.Money);
			param2.Value = BaseIVA;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@IVANavitaire", SqlDbType.Money);
			param3.Value = IVANavitaire;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@IVAReal", SqlDbType.Money);
			param4.Value = IVAReal;
			commandParameters.Add(param4);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdPagosOmitidosGlobalPorIVA";
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

		#region Eliminar VBFac_PagosOmitidosGlobalPorIVA
		public void Eliminar(DateTime fechafacturaglobal,long idpagoscab)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@FechaFacturaGlobal", SqlDbType.VarChar);
			param0.Value = fechafacturaglobal;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param1.Value = idpagoscab;
			commandParameters.Add(param1);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelPagosOmitidosGlobalPorIVA";
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

		#region Deshacer VBFac_PagosOmitidosGlobalPorIVA
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(FechaFacturaGlobal,IdPagosCab);
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
		public List<ENTPagosomitidosglobalporiva> RecuperarTodo()
		{
			List<ENTPagosomitidosglobalporiva> result = new List<ENTPagosomitidosglobalporiva>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetPagosOmitidosGlobalPorIVA_TODO";
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
				    ENTPagosomitidosglobalporiva item = new ENTPagosomitidosglobalporiva();
					 if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					 if (!dr.IsNull("FechaFacturaGlobal")) item.FechaFacturaGlobal = Convert.ToDateTime(dr["FechaFacturaGlobal"]);
					 if (!dr.IsNull("BaseIVA")) item.BaseIVA = Convert.ToDecimal(dr["BaseIVA"]);
					 if (!dr.IsNull("IVANavitaire")) item.IVANavitaire = Convert.ToDecimal(dr["IVANavitaire"]);
					 if (!dr.IsNull("IVAReal")) item.IVAReal = Convert.ToDecimal(dr["IVAReal"]);
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

		#region Recuperar RecuperarPagosomitidosglobalporivaPorLlavePrimaria
		public List<ENTPagosomitidosglobalporiva> RecuperarPagosomitidosglobalporivaPorLlavePrimaria(long idpagoscab,DateTime fechafacturaglobal)
		{
			List<ENTPagosomitidosglobalporiva> result = new List<ENTPagosomitidosglobalporiva>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = idpagoscab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@FechaFacturaGlobal", SqlDbType.VarChar);
			param1.Value = fechafacturaglobal;
			commandParameters.Add(param1);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetPagosOmitidosGlobalPorIVA_POR_PK";
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
				    ENTPagosomitidosglobalporiva item = new ENTPagosomitidosglobalporiva();
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("FechaFacturaGlobal")) item.FechaFacturaGlobal = Convert.ToDateTime(dr["FechaFacturaGlobal"]);
					if (!dr.IsNull("BaseIVA")) item.BaseIVA = Convert.ToDecimal(dr["BaseIVA"]);
					if (!dr.IsNull("IVANavitaire")) item.IVANavitaire = Convert.ToDecimal(dr["IVANavitaire"]);
					if (!dr.IsNull("IVAReal")) item.IVAReal = Convert.ToDecimal(dr["IVAReal"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("FechaFacturaGlobal")) FechaFacturaGlobal = Convert.ToDateTime(dtResultado.Rows[0]["FechaFacturaGlobal"]);
					if (!dtResultado.Rows[0].IsNull("BaseIVA")) BaseIVA = Convert.ToDecimal(dtResultado.Rows[0]["BaseIVA"]);
					if (!dtResultado.Rows[0].IsNull("IVANavitaire")) IVANavitaire = Convert.ToDecimal(dtResultado.Rows[0]["IVANavitaire"]);
					if (!dtResultado.Rows[0].IsNull("IVAReal")) IVAReal = Convert.ToDecimal(dtResultado.Rows[0]["IVAReal"]);
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
			_copia = new DALPagosomitidosglobalporiva(_conexion);
			Type tipo = typeof(ENTPagosomitidosglobalporiva);
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

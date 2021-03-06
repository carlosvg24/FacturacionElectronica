using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALReasignaexpedporiata: ENTReasignaexpedporiata
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALReasignaexpedporiata _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALReasignaexpedporiata(SqlConnection conexion)
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
			IdIATAExp = 0;
			CodigoIATA = String.Empty;
			LugarExpedicion = String.Empty;
			Activo = true;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_ReasignaExpedPorIATA
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdIATAExp", SqlDbType.Int);
			param0.Value = IdIATAExp;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@CodigoIATA", SqlDbType.VarChar);
			param1.Value = CodigoIATA;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@LugarExpedicion", SqlDbType.VarChar);
			param2.Value = LugarExpedicion;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Activo", SqlDbType.Bit);
			param3.Value = Activo;
			commandParameters.Add(param3);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsReasignaExpedPorIATA";
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
						IdIATAExp = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_ReasignaExpedPorIATA
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdIATAExp", SqlDbType.Int);
			param0.Value = IdIATAExp;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@CodigoIATA", SqlDbType.VarChar);
			param1.Value = CodigoIATA;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@LugarExpedicion", SqlDbType.VarChar);
			param2.Value = LugarExpedicion;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Activo", SqlDbType.Bit);
			param3.Value = Activo;
			commandParameters.Add(param3);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdReasignaExpedPorIATA";
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

		#region Eliminar VBFac_ReasignaExpedPorIATA
		public void Eliminar(int idiataexp)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdIATAExp", SqlDbType.Int);
			param0.Value = idiataexp;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelReasignaExpedPorIATA";
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

		#region Deshacer VBFac_ReasignaExpedPorIATA
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdIATAExp);
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
		public List<ENTReasignaexpedporiata> RecuperarTodo()
		{
			List<ENTReasignaexpedporiata> result = new List<ENTReasignaexpedporiata>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetReasignaExpedPorIATA_TODO";
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
				    ENTReasignaexpedporiata item = new ENTReasignaexpedporiata();
					 if (!dr.IsNull("IdIATAExp")) item.IdIATAExp = Convert.ToInt32(dr["IdIATAExp"]);
					 if (!dr.IsNull("CodigoIATA")) item.CodigoIATA = dr["CodigoIATA"].ToString();
					 if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
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

		#region Recuperar RecuperarReasignaexpedporiataPorLlavePrimaria
		public List<ENTReasignaexpedporiata> RecuperarReasignaexpedporiataPorLlavePrimaria(int idiataexp)
		{
			List<ENTReasignaexpedporiata> result = new List<ENTReasignaexpedporiata>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdIATAExp", SqlDbType.Int);
			param0.Value = idiataexp;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetReasignaExpedPorIATA_POR_PK";
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
				    ENTReasignaexpedporiata item = new ENTReasignaexpedporiata();
					if (!dr.IsNull("IdIATAExp")) item.IdIATAExp = Convert.ToInt32(dr["IdIATAExp"]);
					if (!dr.IsNull("CodigoIATA")) item.CodigoIATA = dr["CodigoIATA"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdIATAExp")) IdIATAExp = Convert.ToInt32(dtResultado.Rows[0]["IdIATAExp"]);
					if (!dtResultado.Rows[0].IsNull("CodigoIATA")) CodigoIATA = dtResultado.Rows[0]["CodigoIATA"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
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
			_copia = new DALReasignaexpedporiata(_conexion);
			Type tipo = typeof(ENTReasignaexpedporiata);
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

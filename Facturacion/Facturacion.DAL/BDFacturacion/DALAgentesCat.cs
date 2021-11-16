using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALAgentesCat: ENTAgentesCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALAgentesCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALAgentesCat(SqlConnection conexion)
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
			IdAgente = 0;
			AgentID = 0;
			CodigoAgente = String.Empty;
			CodigoSAP = String.Empty;
			Activo = true;
			UsuarioRegistro = 0;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_Agentes_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdAgente", SqlDbType.Int);
			param0.Value = IdAgente;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@AgentID", SqlDbType.BigInt);
			param1.Value = AgentID;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@CodigoAgente", SqlDbType.VarChar);
			param2.Value = CodigoAgente;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@CodigoSAP", SqlDbType.VarChar);
			param3.Value = CodigoSAP;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Activo", SqlDbType.Bit);
			param4.Value = Activo;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@UsuarioRegistro", SqlDbType.Int);
			param5.Value = UsuarioRegistro;
			commandParameters.Add(param5);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsAgentesCat";
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
						IdAgente = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_Agentes_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdAgente", SqlDbType.Int);
			param0.Value = IdAgente;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@AgentID", SqlDbType.BigInt);
			param1.Value = AgentID;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@CodigoAgente", SqlDbType.VarChar);
			param2.Value = CodigoAgente;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@CodigoSAP", SqlDbType.VarChar);
			param3.Value = CodigoSAP;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Activo", SqlDbType.Bit);
			param4.Value = Activo;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@UsuarioRegistro", SqlDbType.Int);
			param5.Value = UsuarioRegistro;
			commandParameters.Add(param5);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdAgentesCat";
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

		#region Eliminar VBFac_Agentes_Cat
		public void Eliminar(int idagente)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdAgente", SqlDbType.Int);
			param0.Value = idagente;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelAgentesCat";
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

		#region Deshacer VBFac_Agentes_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdAgente);
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
		public List<ENTAgentesCat> RecuperarTodo()
		{
			List<ENTAgentesCat> result = new List<ENTAgentesCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetAgentesCat_TODO";
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
				    ENTAgentesCat item = new ENTAgentesCat();
					 if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt32(dr["IdAgente"]);
					 if (!dr.IsNull("AgentID")) item.AgentID = Convert.ToInt64(dr["AgentID"]);
					 if (!dr.IsNull("CodigoAgente")) item.CodigoAgente = dr["CodigoAgente"].ToString();
					 if (!dr.IsNull("CodigoSAP")) item.CodigoSAP = dr["CodigoSAP"].ToString();
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

		#region Recuperar RecuperarAgentesCatPorLlavePrimaria
		public List<ENTAgentesCat> RecuperarAgentesCatPorLlavePrimaria(int idagente)
		{
			List<ENTAgentesCat> result = new List<ENTAgentesCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdAgente", SqlDbType.Int);
			param0.Value = idagente;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetAgentesCat_POR_PK";
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
				    ENTAgentesCat item = new ENTAgentesCat();
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt32(dr["IdAgente"]);
					if (!dr.IsNull("AgentID")) item.AgentID = Convert.ToInt64(dr["AgentID"]);
					if (!dr.IsNull("CodigoAgente")) item.CodigoAgente = dr["CodigoAgente"].ToString();
					if (!dr.IsNull("CodigoSAP")) item.CodigoSAP = dr["CodigoSAP"].ToString();
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("UsuarioRegistro")) item.UsuarioRegistro = Convert.ToInt32(dr["UsuarioRegistro"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt32(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("AgentID")) AgentID = Convert.ToInt64(dtResultado.Rows[0]["AgentID"]);
					if (!dtResultado.Rows[0].IsNull("CodigoAgente")) CodigoAgente = dtResultado.Rows[0]["CodigoAgente"].ToString();
					if (!dtResultado.Rows[0].IsNull("CodigoSAP")) CodigoSAP = dtResultado.Rows[0]["CodigoSAP"].ToString();
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

		#region Recuperar RecuperarAgentesCatAgentid
		public List<ENTAgentesCat> RecuperarAgentesCatAgentid(long agentid)
		{
			List<ENTAgentesCat> result = new List<ENTAgentesCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@AgentID", SqlDbType.BigInt);
			param0.Value = agentid;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetAgentesCat_POR_AgentID";
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
				    ENTAgentesCat item = new ENTAgentesCat();
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt32(dr["IdAgente"]);
					if (!dr.IsNull("AgentID")) item.AgentID = Convert.ToInt64(dr["AgentID"]);
					if (!dr.IsNull("CodigoAgente")) item.CodigoAgente = dr["CodigoAgente"].ToString();
					if (!dr.IsNull("CodigoSAP")) item.CodigoSAP = dr["CodigoSAP"].ToString();
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("UsuarioRegistro")) item.UsuarioRegistro = Convert.ToInt32(dr["UsuarioRegistro"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt32(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("AgentID")) AgentID = Convert.ToInt64(dtResultado.Rows[0]["AgentID"]);
					if (!dtResultado.Rows[0].IsNull("CodigoAgente")) CodigoAgente = dtResultado.Rows[0]["CodigoAgente"].ToString();
					if (!dtResultado.Rows[0].IsNull("CodigoSAP")) CodigoSAP = dtResultado.Rows[0]["CodigoSAP"].ToString();
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
			_copia = new DALAgentesCat(_conexion);
			Type tipo = typeof(ENTAgentesCat);
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

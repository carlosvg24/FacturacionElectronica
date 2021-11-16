using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALSolicitudesfacDet: ENTSolicitudesfacDet
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALSolicitudesfacDet _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALSolicitudesfacDet(SqlConnection conexion)
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
			IdSolicitudesFac = 0;
			IdPeticionPago = 0;
			PaymentId = 0;
			TipoPeticion = String.Empty;
			EsCorrecto = false;
			Mensaje = String.Empty;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_SolicitudesFac_Det
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdSolicitudesFac", SqlDbType.BigInt);
			param0.Value = IdSolicitudesFac;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdPeticionPago", SqlDbType.Int);
			param1.Value = IdPeticionPago;
			param1.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@PaymentId", SqlDbType.BigInt);
			param2.Value = PaymentId;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@TipoPeticion", SqlDbType.VarChar);
			param3.Value = TipoPeticion;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@EsCorrecto", SqlDbType.Bit);
			param4.Value = EsCorrecto;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Mensaje", SqlDbType.VarChar);
			param5.Value = Mensaje;
			commandParameters.Add(param5);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsSolicitudesFacDet";
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
						IdSolicitudesFac = Convert.ToInt64(cmm.Parameters[0].Value);
						IdPeticionPago = Convert.ToInt32(cmm.Parameters[1].Value);

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

		#region Actualizar VBFac_SolicitudesFac_Det
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdSolicitudesFac", SqlDbType.BigInt);
			param0.Value = IdSolicitudesFac;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdPeticionPago", SqlDbType.Int);
			param1.Value = IdPeticionPago;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@PaymentId", SqlDbType.BigInt);
			param2.Value = PaymentId;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@TipoPeticion", SqlDbType.VarChar);
			param3.Value = TipoPeticion;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@EsCorrecto", SqlDbType.Bit);
			param4.Value = EsCorrecto;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Mensaje", SqlDbType.VarChar);
			param5.Value = Mensaje;
			commandParameters.Add(param5);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdSolicitudesFacDet";
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

		#region Eliminar VBFac_SolicitudesFac_Det
		public void Eliminar(int idpeticionpago,long idsolicitudesfac)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdPeticionPago", SqlDbType.Int);
			param0.Value = idpeticionpago;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdSolicitudesFac", SqlDbType.BigInt);
			param1.Value = idsolicitudesfac;
			commandParameters.Add(param1);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelSolicitudesFacDet";
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

		#region Deshacer VBFac_SolicitudesFac_Det
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdPeticionPago,IdSolicitudesFac);
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
		public List<ENTSolicitudesfacDet> RecuperarTodo()
		{
			List<ENTSolicitudesfacDet> result = new List<ENTSolicitudesfacDet>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetSolicitudesFacDet_TODO";
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
				    ENTSolicitudesfacDet item = new ENTSolicitudesfacDet();
					 if (!dr.IsNull("IdSolicitudesFac")) item.IdSolicitudesFac = Convert.ToInt64(dr["IdSolicitudesFac"]);
					 if (!dr.IsNull("IdPeticionPago")) item.IdPeticionPago = Convert.ToInt32(dr["IdPeticionPago"]);
					 if (!dr.IsNull("PaymentId")) item.PaymentId = Convert.ToInt64(dr["PaymentId"]);
					 if (!dr.IsNull("TipoPeticion")) item.TipoPeticion = dr["TipoPeticion"].ToString();
					 if (!dr.IsNull("EsCorrecto")) item.EsCorrecto = Convert.ToBoolean(dr["EsCorrecto"]);
					 if (!dr.IsNull("Mensaje")) item.Mensaje = dr["Mensaje"].ToString();
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

		#region Recuperar RecuperarSolicitudesfacDetPorLlavePrimaria
		public List<ENTSolicitudesfacDet> RecuperarSolicitudesfacDetPorLlavePrimaria(long idsolicitudesfac,int idpeticionpago)
		{
			List<ENTSolicitudesfacDet> result = new List<ENTSolicitudesfacDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdSolicitudesFac", SqlDbType.BigInt);
			param0.Value = idsolicitudesfac;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdPeticionPago", SqlDbType.Int);
			param1.Value = idpeticionpago;
			commandParameters.Add(param1);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetSolicitudesFacDet_POR_PK";
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
				    ENTSolicitudesfacDet item = new ENTSolicitudesfacDet();
					if (!dr.IsNull("IdSolicitudesFac")) item.IdSolicitudesFac = Convert.ToInt64(dr["IdSolicitudesFac"]);
					if (!dr.IsNull("IdPeticionPago")) item.IdPeticionPago = Convert.ToInt32(dr["IdPeticionPago"]);
					if (!dr.IsNull("PaymentId")) item.PaymentId = Convert.ToInt64(dr["PaymentId"]);
					if (!dr.IsNull("TipoPeticion")) item.TipoPeticion = dr["TipoPeticion"].ToString();
					if (!dr.IsNull("EsCorrecto")) item.EsCorrecto = Convert.ToBoolean(dr["EsCorrecto"]);
					if (!dr.IsNull("Mensaje")) item.Mensaje = dr["Mensaje"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdSolicitudesFac")) IdSolicitudesFac = Convert.ToInt64(dtResultado.Rows[0]["IdSolicitudesFac"]);
					if (!dtResultado.Rows[0].IsNull("IdPeticionPago")) IdPeticionPago = Convert.ToInt32(dtResultado.Rows[0]["IdPeticionPago"]);
					if (!dtResultado.Rows[0].IsNull("PaymentId")) PaymentId = Convert.ToInt64(dtResultado.Rows[0]["PaymentId"]);
					if (!dtResultado.Rows[0].IsNull("TipoPeticion")) TipoPeticion = dtResultado.Rows[0]["TipoPeticion"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsCorrecto")) EsCorrecto = Convert.ToBoolean(dtResultado.Rows[0]["EsCorrecto"]);
					if (!dtResultado.Rows[0].IsNull("Mensaje")) Mensaje = dtResultado.Rows[0]["Mensaje"].ToString();
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
			_copia = new DALSolicitudesfacDet(_conexion);
			Type tipo = typeof(ENTSolicitudesfacDet);
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

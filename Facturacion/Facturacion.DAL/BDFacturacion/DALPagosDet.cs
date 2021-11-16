using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALPagosDet: ENTPagosDet
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALPagosDet _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALPagosDet(SqlConnection conexion)
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
			PaymentID = 0;
			BookingID = 0;
			FechaPago = new DateTime();
			PaymentMethodCode = String.Empty;
			CurrencyCode = String.Empty;
			PaymentAmount = 0M;
			CollectedCurrencyCode = String.Empty;
			CollectedAmount = 0M;
			IdAgente = 0;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_Pagos_Det
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = IdPagosCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@PaymentID", SqlDbType.BigInt);
			param1.Value = PaymentID;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param2.Value = BookingID;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@FechaPago", SqlDbType.VarChar);
			param3.Value = FechaPago.Year > 1900 ? FechaPago.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@PaymentMethodCode", SqlDbType.VarChar);
			param4.Value = PaymentMethodCode;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@CurrencyCode", SqlDbType.VarChar);
			param5.Value = CurrencyCode;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@PaymentAmount", SqlDbType.Money);
			param6.Value = PaymentAmount;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@CollectedCurrencyCode", SqlDbType.VarChar);
			param7.Value = CollectedCurrencyCode;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@CollectedAmount", SqlDbType.Money);
			param8.Value = CollectedAmount;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@IdAgente", SqlDbType.BigInt);
			param9.Value = IdAgente;
			commandParameters.Add(param9);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsPagosDet";
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

		#region Actualizar VBFac_Pagos_Det
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = IdPagosCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@PaymentID", SqlDbType.BigInt);
			param1.Value = PaymentID;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param2.Value = BookingID;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@FechaPago", SqlDbType.VarChar);
			param3.Value = FechaPago.Year > 1900 ? FechaPago.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@PaymentMethodCode", SqlDbType.VarChar);
			param4.Value = PaymentMethodCode;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@CurrencyCode", SqlDbType.VarChar);
			param5.Value = CurrencyCode;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@PaymentAmount", SqlDbType.Money);
			param6.Value = PaymentAmount;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@CollectedCurrencyCode", SqlDbType.VarChar);
			param7.Value = CollectedCurrencyCode;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@CollectedAmount", SqlDbType.Money);
			param8.Value = CollectedAmount;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@IdAgente", SqlDbType.BigInt);
			param9.Value = IdAgente;
			commandParameters.Add(param9);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdPagosDet";
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

		#region Eliminar VBFac_Pagos_Det
		public void Eliminar(long idpagoscab,long paymentid)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = idpagoscab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@PaymentID", SqlDbType.BigInt);
			param1.Value = paymentid;
			commandParameters.Add(param1);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelPagosDet";
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

		#region Deshacer VBFac_Pagos_Det
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdPagosCab,PaymentID);
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
		public List<ENTPagosDet> RecuperarTodo()
		{
			List<ENTPagosDet> result = new List<ENTPagosDet>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetPagosDet_TODO";
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
				    ENTPagosDet item = new ENTPagosDet();
					 if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					 if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					 if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					 if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					 if (!dr.IsNull("PaymentMethodCode")) item.PaymentMethodCode = dr["PaymentMethodCode"].ToString();
					 if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					 if (!dr.IsNull("PaymentAmount")) item.PaymentAmount = Convert.ToDecimal(dr["PaymentAmount"]);
					 if (!dr.IsNull("CollectedCurrencyCode")) item.CollectedCurrencyCode = dr["CollectedCurrencyCode"].ToString();
					 if (!dr.IsNull("CollectedAmount")) item.CollectedAmount = Convert.ToDecimal(dr["CollectedAmount"]);
					 if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt64(dr["IdAgente"]);
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

		#region Recuperar RecuperarPagosDetPorLlavePrimaria
		public List<ENTPagosDet> RecuperarPagosDetPorLlavePrimaria(long idpagoscab,long paymentid)
		{
			List<ENTPagosDet> result = new List<ENTPagosDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = idpagoscab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@PaymentID", SqlDbType.BigInt);
			param1.Value = paymentid;
			commandParameters.Add(param1);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetPagosDet_POR_PK";
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
				    ENTPagosDet item = new ENTPagosDet();
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					if (!dr.IsNull("PaymentMethodCode")) item.PaymentMethodCode = dr["PaymentMethodCode"].ToString();
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("PaymentAmount")) item.PaymentAmount = Convert.ToDecimal(dr["PaymentAmount"]);
					if (!dr.IsNull("CollectedCurrencyCode")) item.CollectedCurrencyCode = dr["CollectedCurrencyCode"].ToString();
					if (!dr.IsNull("CollectedAmount")) item.CollectedAmount = Convert.ToDecimal(dr["CollectedAmount"]);
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt64(dr["IdAgente"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("PaymentID")) PaymentID = Convert.ToInt64(dtResultado.Rows[0]["PaymentID"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("FechaPago")) FechaPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaPago"]);
					if (!dtResultado.Rows[0].IsNull("PaymentMethodCode")) PaymentMethodCode = dtResultado.Rows[0]["PaymentMethodCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("PaymentAmount")) PaymentAmount = Convert.ToDecimal(dtResultado.Rows[0]["PaymentAmount"]);
					if (!dtResultado.Rows[0].IsNull("CollectedCurrencyCode")) CollectedCurrencyCode = dtResultado.Rows[0]["CollectedCurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CollectedAmount")) CollectedAmount = Convert.ToDecimal(dtResultado.Rows[0]["CollectedAmount"]);
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt64(dtResultado.Rows[0]["IdAgente"]);
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

		#region Recuperar RecuperarPagosDetIdpagoscab
		public List<ENTPagosDet> RecuperarPagosDetIdpagoscab(long idpagoscab)
		{
			List<ENTPagosDet> result = new List<ENTPagosDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = idpagoscab;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetPagosDet_POR_IdPagosCab";
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
				    ENTPagosDet item = new ENTPagosDet();
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					if (!dr.IsNull("PaymentMethodCode")) item.PaymentMethodCode = dr["PaymentMethodCode"].ToString();
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("PaymentAmount")) item.PaymentAmount = Convert.ToDecimal(dr["PaymentAmount"]);
					if (!dr.IsNull("CollectedCurrencyCode")) item.CollectedCurrencyCode = dr["CollectedCurrencyCode"].ToString();
					if (!dr.IsNull("CollectedAmount")) item.CollectedAmount = Convert.ToDecimal(dr["CollectedAmount"]);
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt64(dr["IdAgente"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("PaymentID")) PaymentID = Convert.ToInt64(dtResultado.Rows[0]["PaymentID"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("FechaPago")) FechaPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaPago"]);
					if (!dtResultado.Rows[0].IsNull("PaymentMethodCode")) PaymentMethodCode = dtResultado.Rows[0]["PaymentMethodCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("PaymentAmount")) PaymentAmount = Convert.ToDecimal(dtResultado.Rows[0]["PaymentAmount"]);
					if (!dtResultado.Rows[0].IsNull("CollectedCurrencyCode")) CollectedCurrencyCode = dtResultado.Rows[0]["CollectedCurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CollectedAmount")) CollectedAmount = Convert.ToDecimal(dtResultado.Rows[0]["CollectedAmount"]);
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt64(dtResultado.Rows[0]["IdAgente"]);
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
			_copia = new DALPagosDet(_conexion);
			Type tipo = typeof(ENTPagosDet);
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

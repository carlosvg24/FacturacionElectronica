using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALGlobalticketsDet: ENTGlobalticketsDet
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALGlobalticketsDet _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALGlobalticketsDet(SqlConnection conexion)
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
			IdFacturaCab = 0;
			IdFacturaDet = 0;
			BookingID = 0;
			PaymentID = 0;
			IdNotaCredito = 0;
			IdFacturaCabCliente = 0;
			FechaHoraLocal = new DateTime();
			MontoTarifa = 0M;
			MontoServAdic = 0M;
			MontoTUA = 0M;
			MontoOtrosCargos = 0M;
			MontoIVA = 0M;
			MontoTotal = 0M;
		}
		#endregion

		#region Agregar VBFac_GlobalTickets_Det
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = IdFacturaCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdFacturaDet", SqlDbType.Int);
			param1.Value = IdFacturaDet;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param2.Value = BookingID;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@PaymentID", SqlDbType.BigInt);
			param3.Value = PaymentID;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@IdNotaCredito", SqlDbType.BigInt);
			param4.Value = IdNotaCredito;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@IdFacturaCabCliente", SqlDbType.BigInt);
			param5.Value = IdFacturaCabCliente;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@MontoTarifa", SqlDbType.Money);
			param6.Value = MontoTarifa;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@MontoServAdic", SqlDbType.Money);
			param7.Value = MontoServAdic;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@MontoTUA", SqlDbType.Money);
			param8.Value = MontoTUA;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@MontoOtrosCargos", SqlDbType.Money);
			param9.Value = MontoOtrosCargos;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@MontoIVA", SqlDbType.Money);
			param10.Value = MontoIVA;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@MontoTotal", SqlDbType.Money);
			param11.Value = MontoTotal;
			commandParameters.Add(param11);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsGlobalTicketsDet";
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

		#region Actualizar VBFac_GlobalTickets_Det
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = IdFacturaCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdFacturaDet", SqlDbType.Int);
			param1.Value = IdFacturaDet;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param2.Value = BookingID;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@PaymentID", SqlDbType.BigInt);
			param3.Value = PaymentID;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@IdNotaCredito", SqlDbType.BigInt);
			param4.Value = IdNotaCredito;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@IdFacturaCabCliente", SqlDbType.BigInt);
			param5.Value = IdFacturaCabCliente;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@MontoTarifa", SqlDbType.Money);
			param6.Value = MontoTarifa;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@MontoServAdic", SqlDbType.Money);
			param7.Value = MontoServAdic;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@MontoTUA", SqlDbType.Money);
			param8.Value = MontoTUA;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@MontoOtrosCargos", SqlDbType.Money);
			param9.Value = MontoOtrosCargos;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@MontoIVA", SqlDbType.Money);
			param10.Value = MontoIVA;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@MontoTotal", SqlDbType.Money);
			param11.Value = MontoTotal;
			commandParameters.Add(param11);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdGlobalTicketsDet";
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

		#region Eliminar VBFac_GlobalTickets_Det
		public void Eliminar(long idfacturacab,int idfacturadet)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = idfacturacab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdFacturaDet", SqlDbType.Int);
			param1.Value = idfacturadet;
			commandParameters.Add(param1);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelGlobalTicketsDet";
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

		#region Deshacer VBFac_GlobalTickets_Det
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdFacturaCab,IdFacturaDet);
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
		public List<ENTGlobalticketsDet> RecuperarTodo()
		{
			List<ENTGlobalticketsDet> result = new List<ENTGlobalticketsDet>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetGlobalTicketsDet_TODO";
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
				    ENTGlobalticketsDet item = new ENTGlobalticketsDet();
					 if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					 if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					 if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					 if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					 if (!dr.IsNull("IdNotaCredito")) item.IdNotaCredito = Convert.ToInt64(dr["IdNotaCredito"]);
					 if (!dr.IsNull("IdFacturaCabCliente")) item.IdFacturaCabCliente = Convert.ToInt64(dr["IdFacturaCabCliente"]);
					 if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					 if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					 if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					 if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					 if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					 if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					 if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
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

		#region Recuperar RecuperarGlobalticketsDetPorLlavePrimaria
		public List<ENTGlobalticketsDet> RecuperarGlobalticketsDetPorLlavePrimaria(long idfacturacab,int idfacturadet)
		{
			List<ENTGlobalticketsDet> result = new List<ENTGlobalticketsDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = idfacturacab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdFacturaDet", SqlDbType.Int);
			param1.Value = idfacturadet;
			commandParameters.Add(param1);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetGlobalTicketsDet_POR_PK";
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
				    ENTGlobalticketsDet item = new ENTGlobalticketsDet();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					if (!dr.IsNull("IdNotaCredito")) item.IdNotaCredito = Convert.ToInt64(dr["IdNotaCredito"]);
					if (!dr.IsNull("IdFacturaCabCliente")) item.IdFacturaCabCliente = Convert.ToInt64(dr["IdFacturaCabCliente"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaDet")) IdFacturaDet = Convert.ToInt32(dtResultado.Rows[0]["IdFacturaDet"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("PaymentID")) PaymentID = Convert.ToInt64(dtResultado.Rows[0]["PaymentID"]);
					if (!dtResultado.Rows[0].IsNull("IdNotaCredito")) IdNotaCredito = Convert.ToInt64(dtResultado.Rows[0]["IdNotaCredito"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCabCliente")) IdFacturaCabCliente = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCabCliente"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("MontoTarifa")) MontoTarifa = Convert.ToDecimal(dtResultado.Rows[0]["MontoTarifa"]);
					if (!dtResultado.Rows[0].IsNull("MontoServAdic")) MontoServAdic = Convert.ToDecimal(dtResultado.Rows[0]["MontoServAdic"]);
					if (!dtResultado.Rows[0].IsNull("MontoTUA")) MontoTUA = Convert.ToDecimal(dtResultado.Rows[0]["MontoTUA"]);
					if (!dtResultado.Rows[0].IsNull("MontoOtrosCargos")) MontoOtrosCargos = Convert.ToDecimal(dtResultado.Rows[0]["MontoOtrosCargos"]);
					if (!dtResultado.Rows[0].IsNull("MontoIVA")) MontoIVA = Convert.ToDecimal(dtResultado.Rows[0]["MontoIVA"]);
					if (!dtResultado.Rows[0].IsNull("MontoTotal")) MontoTotal = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotal"]);
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

		#region Recuperar RecuperarGlobalticketsDetIdfacturadetIdfacturacab
		public List<ENTGlobalticketsDet> RecuperarGlobalticketsDetIdfacturadetIdfacturacab(int idfacturadet,long idfacturacab)
		{
			List<ENTGlobalticketsDet> result = new List<ENTGlobalticketsDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFacturaDet", SqlDbType.Int);
			param0.Value = idfacturadet;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param1.Value = idfacturacab;
			commandParameters.Add(param1);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetGlobalTicketsDet_POR_IdFacturaDet_IdFacturaCab";
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
				    ENTGlobalticketsDet item = new ENTGlobalticketsDet();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					if (!dr.IsNull("IdNotaCredito")) item.IdNotaCredito = Convert.ToInt64(dr["IdNotaCredito"]);
					if (!dr.IsNull("IdFacturaCabCliente")) item.IdFacturaCabCliente = Convert.ToInt64(dr["IdFacturaCabCliente"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaDet")) IdFacturaDet = Convert.ToInt32(dtResultado.Rows[0]["IdFacturaDet"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("PaymentID")) PaymentID = Convert.ToInt64(dtResultado.Rows[0]["PaymentID"]);
					if (!dtResultado.Rows[0].IsNull("IdNotaCredito")) IdNotaCredito = Convert.ToInt64(dtResultado.Rows[0]["IdNotaCredito"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCabCliente")) IdFacturaCabCliente = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCabCliente"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("MontoTarifa")) MontoTarifa = Convert.ToDecimal(dtResultado.Rows[0]["MontoTarifa"]);
					if (!dtResultado.Rows[0].IsNull("MontoServAdic")) MontoServAdic = Convert.ToDecimal(dtResultado.Rows[0]["MontoServAdic"]);
					if (!dtResultado.Rows[0].IsNull("MontoTUA")) MontoTUA = Convert.ToDecimal(dtResultado.Rows[0]["MontoTUA"]);
					if (!dtResultado.Rows[0].IsNull("MontoOtrosCargos")) MontoOtrosCargos = Convert.ToDecimal(dtResultado.Rows[0]["MontoOtrosCargos"]);
					if (!dtResultado.Rows[0].IsNull("MontoIVA")) MontoIVA = Convert.ToDecimal(dtResultado.Rows[0]["MontoIVA"]);
					if (!dtResultado.Rows[0].IsNull("MontoTotal")) MontoTotal = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotal"]);
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

		#region Recuperar RecuperarGlobalticketsDetPaymentid
		public List<ENTGlobalticketsDet> RecuperarGlobalticketsDetPaymentid(long paymentid)
		{
			List<ENTGlobalticketsDet> result = new List<ENTGlobalticketsDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@PaymentID", SqlDbType.BigInt);
			param0.Value = paymentid;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetGlobalTicketsDet_POR_PaymentID";
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
				    ENTGlobalticketsDet item = new ENTGlobalticketsDet();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					if (!dr.IsNull("IdNotaCredito")) item.IdNotaCredito = Convert.ToInt64(dr["IdNotaCredito"]);
					if (!dr.IsNull("IdFacturaCabCliente")) item.IdFacturaCabCliente = Convert.ToInt64(dr["IdFacturaCabCliente"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaDet")) IdFacturaDet = Convert.ToInt32(dtResultado.Rows[0]["IdFacturaDet"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("PaymentID")) PaymentID = Convert.ToInt64(dtResultado.Rows[0]["PaymentID"]);
					if (!dtResultado.Rows[0].IsNull("IdNotaCredito")) IdNotaCredito = Convert.ToInt64(dtResultado.Rows[0]["IdNotaCredito"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCabCliente")) IdFacturaCabCliente = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCabCliente"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("MontoTarifa")) MontoTarifa = Convert.ToDecimal(dtResultado.Rows[0]["MontoTarifa"]);
					if (!dtResultado.Rows[0].IsNull("MontoServAdic")) MontoServAdic = Convert.ToDecimal(dtResultado.Rows[0]["MontoServAdic"]);
					if (!dtResultado.Rows[0].IsNull("MontoTUA")) MontoTUA = Convert.ToDecimal(dtResultado.Rows[0]["MontoTUA"]);
					if (!dtResultado.Rows[0].IsNull("MontoOtrosCargos")) MontoOtrosCargos = Convert.ToDecimal(dtResultado.Rows[0]["MontoOtrosCargos"]);
					if (!dtResultado.Rows[0].IsNull("MontoIVA")) MontoIVA = Convert.ToDecimal(dtResultado.Rows[0]["MontoIVA"]);
					if (!dtResultado.Rows[0].IsNull("MontoTotal")) MontoTotal = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotal"]);
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
			_copia = new DALGlobalticketsDet(_conexion);
			Type tipo = typeof(ENTGlobalticketsDet);
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

using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace FinanzasTools
{
	public class DALReservaCab: ENTReservaCab
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALReservaCab _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALReservaCab(SqlConnection conexion)
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
			IdReservaCab = 0;
			IdEmpresa = 0;
			BookingID = 0;
			RecordLocator = String.Empty;
			Estatus = 0;
			NumJourneys = 0;
			CurrencyCode = String.Empty;
			OwningCarrierCode = String.Empty;
			CreatedAgentID = 0;
			CreatedDate = new DateTime();
			ModifiedAgentID = 0;
			ModifiedDate = new DateTime();
			ChannelTypeID = 0;
			CreatedOrganizationCode = String.Empty;
			MontoTotal = 0M;
			MontoPagado = 0M;
			MontoFacturado = 0M;
			FoliosFacturacion = String.Empty;
			FechaHoraLocal = new DateTime();
			FechaModificacion = new DateTime();
		}
		#endregion

		#region Agregar VBFac_Reserva_Cab
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdReservaCab", SqlDbType.BigInt);
			param0.Value = IdReservaCab;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdEmpresa", SqlDbType.TinyInt);
			param1.Value = IdEmpresa;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param2.Value = BookingID;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@RecordLocator", SqlDbType.VarChar);
			param3.Value = RecordLocator;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Estatus", SqlDbType.SmallInt);
			param4.Value = Estatus;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@NumJourneys", SqlDbType.TinyInt);
			param5.Value = NumJourneys;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@CurrencyCode", SqlDbType.VarChar);
			param6.Value = CurrencyCode;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@OwningCarrierCode", SqlDbType.VarChar);
			param7.Value = OwningCarrierCode;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@CreatedAgentID", SqlDbType.BigInt);
			param8.Value = CreatedAgentID;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@CreatedDate", SqlDbType.VarChar);
			param9.Value = CreatedDate.Year > 1900 ? CreatedDate.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@ModifiedAgentID", SqlDbType.BigInt);
			param10.Value = ModifiedAgentID;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@ModifiedDate", SqlDbType.VarChar);
			param11.Value = ModifiedDate.Year > 1900 ? ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param11);
			SqlParameter param12 = new SqlParameter("@ChannelTypeID", SqlDbType.TinyInt);
			param12.Value = ChannelTypeID;
			commandParameters.Add(param12);
			SqlParameter param13 = new SqlParameter("@CreatedOrganizationCode", SqlDbType.VarChar);
			param13.Value = CreatedOrganizationCode;
			commandParameters.Add(param13);
			SqlParameter param14 = new SqlParameter("@MontoTotal", SqlDbType.Money);
			param14.Value = MontoTotal;
			commandParameters.Add(param14);
			SqlParameter param15 = new SqlParameter("@MontoPagado", SqlDbType.Money);
			param15.Value = MontoPagado;
			commandParameters.Add(param15);
			SqlParameter param16 = new SqlParameter("@MontoFacturado", SqlDbType.Money);
			param16.Value = MontoFacturado;
			commandParameters.Add(param16);
			SqlParameter param17 = new SqlParameter("@FoliosFacturacion", SqlDbType.VarChar);
			param17.Value = FoliosFacturacion;
			commandParameters.Add(param17);
			SqlParameter param18 = new SqlParameter("@FechaModificacion", SqlDbType.VarChar);
			param18.Value = FechaModificacion.Year > 1900 ? FechaModificacion.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param18);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsReservaCab";
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
						IdReservaCab = Convert.ToInt64(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_Reserva_Cab
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdReservaCab", SqlDbType.BigInt);
			param0.Value = IdReservaCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdEmpresa", SqlDbType.TinyInt);
			param1.Value = IdEmpresa;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param2.Value = BookingID;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@RecordLocator", SqlDbType.VarChar);
			param3.Value = RecordLocator;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Estatus", SqlDbType.SmallInt);
			param4.Value = Estatus;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@NumJourneys", SqlDbType.TinyInt);
			param5.Value = NumJourneys;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@CurrencyCode", SqlDbType.VarChar);
			param6.Value = CurrencyCode;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@OwningCarrierCode", SqlDbType.VarChar);
			param7.Value = OwningCarrierCode;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@CreatedAgentID", SqlDbType.BigInt);
			param8.Value = CreatedAgentID;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@CreatedDate", SqlDbType.VarChar);
			param9.Value = CreatedDate.Year > 1900 ? CreatedDate.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@ModifiedAgentID", SqlDbType.BigInt);
			param10.Value = ModifiedAgentID;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@ModifiedDate", SqlDbType.VarChar);
			param11.Value = ModifiedDate.Year > 1900 ? ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param11);
			SqlParameter param12 = new SqlParameter("@ChannelTypeID", SqlDbType.TinyInt);
			param12.Value = ChannelTypeID;
			commandParameters.Add(param12);
			SqlParameter param13 = new SqlParameter("@CreatedOrganizationCode", SqlDbType.VarChar);
			param13.Value = CreatedOrganizationCode;
			commandParameters.Add(param13);
			SqlParameter param14 = new SqlParameter("@MontoTotal", SqlDbType.Money);
			param14.Value = MontoTotal;
			commandParameters.Add(param14);
			SqlParameter param15 = new SqlParameter("@MontoPagado", SqlDbType.Money);
			param15.Value = MontoPagado;
			commandParameters.Add(param15);
			SqlParameter param16 = new SqlParameter("@MontoFacturado", SqlDbType.Money);
			param16.Value = MontoFacturado;
			commandParameters.Add(param16);
			SqlParameter param17 = new SqlParameter("@FoliosFacturacion", SqlDbType.VarChar);
			param17.Value = FoliosFacturacion;
			commandParameters.Add(param17);
			SqlParameter param18 = new SqlParameter("@FechaModificacion", SqlDbType.VarChar);
			param18.Value = FechaModificacion.Year > 1900 ? FechaModificacion.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param18);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdReservaCab";
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

		#region Eliminar VBFac_Reserva_Cab
		public void Eliminar(long idreservacab)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdReservaCab", SqlDbType.BigInt);
			param0.Value = idreservacab;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelReservaCab";
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

		#region Deshacer VBFac_Reserva_Cab
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdReservaCab);
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
		public List<ENTReservaCab> RecuperarTodo()
		{
			List<ENTReservaCab> result = new List<ENTReservaCab>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetReservaCab_TODO";
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
				    ENTReservaCab item = new ENTReservaCab();
					 if (!dr.IsNull("IdReservaCab")) item.IdReservaCab = Convert.ToInt64(dr["IdReservaCab"]);
					 if (!dr.IsNull("IdEmpresa")) item.IdEmpresa = Convert.ToByte(dr["IdEmpresa"]);
					 if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					 if (!dr.IsNull("RecordLocator")) item.RecordLocator = dr["RecordLocator"].ToString();
					 if (!dr.IsNull("Estatus")) item.Estatus = Convert.ToInt16(dr["Estatus"]);
					 if (!dr.IsNull("NumJourneys")) item.NumJourneys = Convert.ToByte(dr["NumJourneys"]);
					 if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					 if (!dr.IsNull("OwningCarrierCode")) item.OwningCarrierCode = dr["OwningCarrierCode"].ToString();
					 if (!dr.IsNull("CreatedAgentID")) item.CreatedAgentID = Convert.ToInt64(dr["CreatedAgentID"]);
					 if (!dr.IsNull("CreatedDate")) item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
					 if (!dr.IsNull("ModifiedAgentID")) item.ModifiedAgentID = Convert.ToInt64(dr["ModifiedAgentID"]);
					 if (!dr.IsNull("ModifiedDate")) item.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
					 if (!dr.IsNull("ChannelTypeID")) item.ChannelTypeID = Convert.ToByte(dr["ChannelTypeID"]);
					 if (!dr.IsNull("CreatedOrganizationCode")) item.CreatedOrganizationCode = dr["CreatedOrganizationCode"].ToString();
					 if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
					 if (!dr.IsNull("MontoPagado")) item.MontoPagado = Convert.ToDecimal(dr["MontoPagado"]);
					 if (!dr.IsNull("MontoFacturado")) item.MontoFacturado = Convert.ToDecimal(dr["MontoFacturado"]);
					 if (!dr.IsNull("FoliosFacturacion")) item.FoliosFacturacion = dr["FoliosFacturacion"].ToString();
					 if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					 if (!dr.IsNull("FechaModificacion")) item.FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]);
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

		#region Recuperar RecuperarReservaCabPorLlavePrimaria
		public List<ENTReservaCab> RecuperarReservaCabPorLlavePrimaria(long idreservacab)
		{
			List<ENTReservaCab> result = new List<ENTReservaCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdReservaCab", SqlDbType.BigInt);
			param0.Value = idreservacab;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetReservaCab_POR_PK";
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
				    ENTReservaCab item = new ENTReservaCab();
					if (!dr.IsNull("IdReservaCab")) item.IdReservaCab = Convert.ToInt64(dr["IdReservaCab"]);
					if (!dr.IsNull("IdEmpresa")) item.IdEmpresa = Convert.ToByte(dr["IdEmpresa"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("RecordLocator")) item.RecordLocator = dr["RecordLocator"].ToString();
					if (!dr.IsNull("Estatus")) item.Estatus = Convert.ToInt16(dr["Estatus"]);
					if (!dr.IsNull("NumJourneys")) item.NumJourneys = Convert.ToByte(dr["NumJourneys"]);
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("OwningCarrierCode")) item.OwningCarrierCode = dr["OwningCarrierCode"].ToString();
					if (!dr.IsNull("CreatedAgentID")) item.CreatedAgentID = Convert.ToInt64(dr["CreatedAgentID"]);
					if (!dr.IsNull("CreatedDate")) item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
					if (!dr.IsNull("ModifiedAgentID")) item.ModifiedAgentID = Convert.ToInt64(dr["ModifiedAgentID"]);
					if (!dr.IsNull("ModifiedDate")) item.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
					if (!dr.IsNull("ChannelTypeID")) item.ChannelTypeID = Convert.ToByte(dr["ChannelTypeID"]);
					if (!dr.IsNull("CreatedOrganizationCode")) item.CreatedOrganizationCode = dr["CreatedOrganizationCode"].ToString();
					if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
					if (!dr.IsNull("MontoPagado")) item.MontoPagado = Convert.ToDecimal(dr["MontoPagado"]);
					if (!dr.IsNull("MontoFacturado")) item.MontoFacturado = Convert.ToDecimal(dr["MontoFacturado"]);
					if (!dr.IsNull("FoliosFacturacion")) item.FoliosFacturacion = dr["FoliosFacturacion"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("FechaModificacion")) item.FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdReservaCab")) IdReservaCab = Convert.ToInt64(dtResultado.Rows[0]["IdReservaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdEmpresa")) IdEmpresa = Convert.ToByte(dtResultado.Rows[0]["IdEmpresa"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("RecordLocator")) RecordLocator = dtResultado.Rows[0]["RecordLocator"].ToString();
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = Convert.ToInt16(dtResultado.Rows[0]["Estatus"]);
					if (!dtResultado.Rows[0].IsNull("NumJourneys")) NumJourneys = Convert.ToByte(dtResultado.Rows[0]["NumJourneys"]);
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("OwningCarrierCode")) OwningCarrierCode = dtResultado.Rows[0]["OwningCarrierCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CreatedAgentID")) CreatedAgentID = Convert.ToInt64(dtResultado.Rows[0]["CreatedAgentID"]);
					if (!dtResultado.Rows[0].IsNull("CreatedDate")) CreatedDate = Convert.ToDateTime(dtResultado.Rows[0]["CreatedDate"]);
					if (!dtResultado.Rows[0].IsNull("ModifiedAgentID")) ModifiedAgentID = Convert.ToInt64(dtResultado.Rows[0]["ModifiedAgentID"]);
					if (!dtResultado.Rows[0].IsNull("ModifiedDate")) ModifiedDate = Convert.ToDateTime(dtResultado.Rows[0]["ModifiedDate"]);
					if (!dtResultado.Rows[0].IsNull("ChannelTypeID")) ChannelTypeID = Convert.ToByte(dtResultado.Rows[0]["ChannelTypeID"]);
					if (!dtResultado.Rows[0].IsNull("CreatedOrganizationCode")) CreatedOrganizationCode = dtResultado.Rows[0]["CreatedOrganizationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("MontoTotal")) MontoTotal = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotal"]);
					if (!dtResultado.Rows[0].IsNull("MontoPagado")) MontoPagado = Convert.ToDecimal(dtResultado.Rows[0]["MontoPagado"]);
					if (!dtResultado.Rows[0].IsNull("MontoFacturado")) MontoFacturado = Convert.ToDecimal(dtResultado.Rows[0]["MontoFacturado"]);
					if (!dtResultado.Rows[0].IsNull("FoliosFacturacion")) FoliosFacturacion = dtResultado.Rows[0]["FoliosFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaModificacion")) FechaModificacion = Convert.ToDateTime(dtResultado.Rows[0]["FechaModificacion"]);
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

		#region Recuperar RecuperarReservaCabIdempresa
		public List<ENTReservaCab> RecuperarReservaCabIdempresa(byte idempresa)
		{
			List<ENTReservaCab> result = new List<ENTReservaCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdEmpresa", SqlDbType.TinyInt);
			param0.Value = idempresa;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetReservaCab_POR_IdEmpresa";
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
				    ENTReservaCab item = new ENTReservaCab();
					if (!dr.IsNull("IdReservaCab")) item.IdReservaCab = Convert.ToInt64(dr["IdReservaCab"]);
					if (!dr.IsNull("IdEmpresa")) item.IdEmpresa = Convert.ToByte(dr["IdEmpresa"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("RecordLocator")) item.RecordLocator = dr["RecordLocator"].ToString();
					if (!dr.IsNull("Estatus")) item.Estatus = Convert.ToInt16(dr["Estatus"]);
					if (!dr.IsNull("NumJourneys")) item.NumJourneys = Convert.ToByte(dr["NumJourneys"]);
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("OwningCarrierCode")) item.OwningCarrierCode = dr["OwningCarrierCode"].ToString();
					if (!dr.IsNull("CreatedAgentID")) item.CreatedAgentID = Convert.ToInt64(dr["CreatedAgentID"]);
					if (!dr.IsNull("CreatedDate")) item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
					if (!dr.IsNull("ModifiedAgentID")) item.ModifiedAgentID = Convert.ToInt64(dr["ModifiedAgentID"]);
					if (!dr.IsNull("ModifiedDate")) item.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
					if (!dr.IsNull("ChannelTypeID")) item.ChannelTypeID = Convert.ToByte(dr["ChannelTypeID"]);
					if (!dr.IsNull("CreatedOrganizationCode")) item.CreatedOrganizationCode = dr["CreatedOrganizationCode"].ToString();
					if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
					if (!dr.IsNull("MontoPagado")) item.MontoPagado = Convert.ToDecimal(dr["MontoPagado"]);
					if (!dr.IsNull("MontoFacturado")) item.MontoFacturado = Convert.ToDecimal(dr["MontoFacturado"]);
					if (!dr.IsNull("FoliosFacturacion")) item.FoliosFacturacion = dr["FoliosFacturacion"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("FechaModificacion")) item.FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdReservaCab")) IdReservaCab = Convert.ToInt64(dtResultado.Rows[0]["IdReservaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdEmpresa")) IdEmpresa = Convert.ToByte(dtResultado.Rows[0]["IdEmpresa"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("RecordLocator")) RecordLocator = dtResultado.Rows[0]["RecordLocator"].ToString();
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = Convert.ToInt16(dtResultado.Rows[0]["Estatus"]);
					if (!dtResultado.Rows[0].IsNull("NumJourneys")) NumJourneys = Convert.ToByte(dtResultado.Rows[0]["NumJourneys"]);
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("OwningCarrierCode")) OwningCarrierCode = dtResultado.Rows[0]["OwningCarrierCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CreatedAgentID")) CreatedAgentID = Convert.ToInt64(dtResultado.Rows[0]["CreatedAgentID"]);
					if (!dtResultado.Rows[0].IsNull("CreatedDate")) CreatedDate = Convert.ToDateTime(dtResultado.Rows[0]["CreatedDate"]);
					if (!dtResultado.Rows[0].IsNull("ModifiedAgentID")) ModifiedAgentID = Convert.ToInt64(dtResultado.Rows[0]["ModifiedAgentID"]);
					if (!dtResultado.Rows[0].IsNull("ModifiedDate")) ModifiedDate = Convert.ToDateTime(dtResultado.Rows[0]["ModifiedDate"]);
					if (!dtResultado.Rows[0].IsNull("ChannelTypeID")) ChannelTypeID = Convert.ToByte(dtResultado.Rows[0]["ChannelTypeID"]);
					if (!dtResultado.Rows[0].IsNull("CreatedOrganizationCode")) CreatedOrganizationCode = dtResultado.Rows[0]["CreatedOrganizationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("MontoTotal")) MontoTotal = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotal"]);
					if (!dtResultado.Rows[0].IsNull("MontoPagado")) MontoPagado = Convert.ToDecimal(dtResultado.Rows[0]["MontoPagado"]);
					if (!dtResultado.Rows[0].IsNull("MontoFacturado")) MontoFacturado = Convert.ToDecimal(dtResultado.Rows[0]["MontoFacturado"]);
					if (!dtResultado.Rows[0].IsNull("FoliosFacturacion")) FoliosFacturacion = dtResultado.Rows[0]["FoliosFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaModificacion")) FechaModificacion = Convert.ToDateTime(dtResultado.Rows[0]["FechaModificacion"]);
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

		#region Recuperar RecuperarReservaCabBookingid
		public List<ENTReservaCab> RecuperarReservaCabBookingid(long bookingid)
		{
			List<ENTReservaCab> result = new List<ENTReservaCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param0.Value = bookingid;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetReservaCab_POR_BookingID";
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
				    ENTReservaCab item = new ENTReservaCab();
					if (!dr.IsNull("IdReservaCab")) item.IdReservaCab = Convert.ToInt64(dr["IdReservaCab"]);
					if (!dr.IsNull("IdEmpresa")) item.IdEmpresa = Convert.ToByte(dr["IdEmpresa"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("RecordLocator")) item.RecordLocator = dr["RecordLocator"].ToString();
					if (!dr.IsNull("Estatus")) item.Estatus = Convert.ToInt16(dr["Estatus"]);
					if (!dr.IsNull("NumJourneys")) item.NumJourneys = Convert.ToByte(dr["NumJourneys"]);
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("OwningCarrierCode")) item.OwningCarrierCode = dr["OwningCarrierCode"].ToString();
					if (!dr.IsNull("CreatedAgentID")) item.CreatedAgentID = Convert.ToInt64(dr["CreatedAgentID"]);
					if (!dr.IsNull("CreatedDate")) item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
					if (!dr.IsNull("ModifiedAgentID")) item.ModifiedAgentID = Convert.ToInt64(dr["ModifiedAgentID"]);
					if (!dr.IsNull("ModifiedDate")) item.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
					if (!dr.IsNull("ChannelTypeID")) item.ChannelTypeID = Convert.ToByte(dr["ChannelTypeID"]);
					if (!dr.IsNull("CreatedOrganizationCode")) item.CreatedOrganizationCode = dr["CreatedOrganizationCode"].ToString();
					if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
					if (!dr.IsNull("MontoPagado")) item.MontoPagado = Convert.ToDecimal(dr["MontoPagado"]);
					if (!dr.IsNull("MontoFacturado")) item.MontoFacturado = Convert.ToDecimal(dr["MontoFacturado"]);
					if (!dr.IsNull("FoliosFacturacion")) item.FoliosFacturacion = dr["FoliosFacturacion"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("FechaModificacion")) item.FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdReservaCab")) IdReservaCab = Convert.ToInt64(dtResultado.Rows[0]["IdReservaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdEmpresa")) IdEmpresa = Convert.ToByte(dtResultado.Rows[0]["IdEmpresa"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("RecordLocator")) RecordLocator = dtResultado.Rows[0]["RecordLocator"].ToString();
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = Convert.ToInt16(dtResultado.Rows[0]["Estatus"]);
					if (!dtResultado.Rows[0].IsNull("NumJourneys")) NumJourneys = Convert.ToByte(dtResultado.Rows[0]["NumJourneys"]);
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("OwningCarrierCode")) OwningCarrierCode = dtResultado.Rows[0]["OwningCarrierCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CreatedAgentID")) CreatedAgentID = Convert.ToInt64(dtResultado.Rows[0]["CreatedAgentID"]);
					if (!dtResultado.Rows[0].IsNull("CreatedDate")) CreatedDate = Convert.ToDateTime(dtResultado.Rows[0]["CreatedDate"]);
					if (!dtResultado.Rows[0].IsNull("ModifiedAgentID")) ModifiedAgentID = Convert.ToInt64(dtResultado.Rows[0]["ModifiedAgentID"]);
					if (!dtResultado.Rows[0].IsNull("ModifiedDate")) ModifiedDate = Convert.ToDateTime(dtResultado.Rows[0]["ModifiedDate"]);
					if (!dtResultado.Rows[0].IsNull("ChannelTypeID")) ChannelTypeID = Convert.ToByte(dtResultado.Rows[0]["ChannelTypeID"]);
					if (!dtResultado.Rows[0].IsNull("CreatedOrganizationCode")) CreatedOrganizationCode = dtResultado.Rows[0]["CreatedOrganizationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("MontoTotal")) MontoTotal = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotal"]);
					if (!dtResultado.Rows[0].IsNull("MontoPagado")) MontoPagado = Convert.ToDecimal(dtResultado.Rows[0]["MontoPagado"]);
					if (!dtResultado.Rows[0].IsNull("MontoFacturado")) MontoFacturado = Convert.ToDecimal(dtResultado.Rows[0]["MontoFacturado"]);
					if (!dtResultado.Rows[0].IsNull("FoliosFacturacion")) FoliosFacturacion = dtResultado.Rows[0]["FoliosFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaModificacion")) FechaModificacion = Convert.ToDateTime(dtResultado.Rows[0]["FechaModificacion"]);
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

		#region Recuperar RecuperarReservaCabRecordlocator
		public List<ENTReservaCab> RecuperarReservaCabRecordlocator(string recordlocator)
		{
			List<ENTReservaCab> result = new List<ENTReservaCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@RecordLocator", SqlDbType.VarChar);
			param0.Value = recordlocator;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetReservaCab_POR_RecordLocator";
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
				    ENTReservaCab item = new ENTReservaCab();
					if (!dr.IsNull("IdReservaCab")) item.IdReservaCab = Convert.ToInt64(dr["IdReservaCab"]);
					if (!dr.IsNull("IdEmpresa")) item.IdEmpresa = Convert.ToByte(dr["IdEmpresa"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("RecordLocator")) item.RecordLocator = dr["RecordLocator"].ToString();
					if (!dr.IsNull("Estatus")) item.Estatus = Convert.ToInt16(dr["Estatus"]);
					if (!dr.IsNull("NumJourneys")) item.NumJourneys = Convert.ToByte(dr["NumJourneys"]);
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("OwningCarrierCode")) item.OwningCarrierCode = dr["OwningCarrierCode"].ToString();
					if (!dr.IsNull("CreatedAgentID")) item.CreatedAgentID = Convert.ToInt64(dr["CreatedAgentID"]);
					if (!dr.IsNull("CreatedDate")) item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
					if (!dr.IsNull("ModifiedAgentID")) item.ModifiedAgentID = Convert.ToInt64(dr["ModifiedAgentID"]);
					if (!dr.IsNull("ModifiedDate")) item.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
					if (!dr.IsNull("ChannelTypeID")) item.ChannelTypeID = Convert.ToByte(dr["ChannelTypeID"]);
					if (!dr.IsNull("CreatedOrganizationCode")) item.CreatedOrganizationCode = dr["CreatedOrganizationCode"].ToString();
					if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
					if (!dr.IsNull("MontoPagado")) item.MontoPagado = Convert.ToDecimal(dr["MontoPagado"]);
					if (!dr.IsNull("MontoFacturado")) item.MontoFacturado = Convert.ToDecimal(dr["MontoFacturado"]);
					if (!dr.IsNull("FoliosFacturacion")) item.FoliosFacturacion = dr["FoliosFacturacion"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("FechaModificacion")) item.FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdReservaCab")) IdReservaCab = Convert.ToInt64(dtResultado.Rows[0]["IdReservaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdEmpresa")) IdEmpresa = Convert.ToByte(dtResultado.Rows[0]["IdEmpresa"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("RecordLocator")) RecordLocator = dtResultado.Rows[0]["RecordLocator"].ToString();
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = Convert.ToInt16(dtResultado.Rows[0]["Estatus"]);
					if (!dtResultado.Rows[0].IsNull("NumJourneys")) NumJourneys = Convert.ToByte(dtResultado.Rows[0]["NumJourneys"]);
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("OwningCarrierCode")) OwningCarrierCode = dtResultado.Rows[0]["OwningCarrierCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CreatedAgentID")) CreatedAgentID = Convert.ToInt64(dtResultado.Rows[0]["CreatedAgentID"]);
					if (!dtResultado.Rows[0].IsNull("CreatedDate")) CreatedDate = Convert.ToDateTime(dtResultado.Rows[0]["CreatedDate"]);
					if (!dtResultado.Rows[0].IsNull("ModifiedAgentID")) ModifiedAgentID = Convert.ToInt64(dtResultado.Rows[0]["ModifiedAgentID"]);
					if (!dtResultado.Rows[0].IsNull("ModifiedDate")) ModifiedDate = Convert.ToDateTime(dtResultado.Rows[0]["ModifiedDate"]);
					if (!dtResultado.Rows[0].IsNull("ChannelTypeID")) ChannelTypeID = Convert.ToByte(dtResultado.Rows[0]["ChannelTypeID"]);
					if (!dtResultado.Rows[0].IsNull("CreatedOrganizationCode")) CreatedOrganizationCode = dtResultado.Rows[0]["CreatedOrganizationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("MontoTotal")) MontoTotal = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotal"]);
					if (!dtResultado.Rows[0].IsNull("MontoPagado")) MontoPagado = Convert.ToDecimal(dtResultado.Rows[0]["MontoPagado"]);
					if (!dtResultado.Rows[0].IsNull("MontoFacturado")) MontoFacturado = Convert.ToDecimal(dtResultado.Rows[0]["MontoFacturado"]);
					if (!dtResultado.Rows[0].IsNull("FoliosFacturacion")) FoliosFacturacion = dtResultado.Rows[0]["FoliosFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaModificacion")) FechaModificacion = Convert.ToDateTime(dtResultado.Rows[0]["FechaModificacion"]);
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
			_copia = new DALReservaCab(_conexion);
			Type tipo = typeof(ENTReservaCab);
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

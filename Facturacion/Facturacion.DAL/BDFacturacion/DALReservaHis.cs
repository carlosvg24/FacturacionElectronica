﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALReservaHis: ENTReservaHis
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALReservaHis _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALReservaHis(SqlConnection conexion)
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
			IdReservaDet = 0;
			NumHistorico = 0;
			FechaHoraHistorico = new DateTime();
			Orden = 0;
			FeeNumber = 0;
			PassengerID = 0;
			SegmentID = 0;
			ChargeNumber = 0;
			ChargeType = 0;
			IdFee = 0;
			ChargeCode = String.Empty;
			ChargeDetail = String.Empty;
			TicketCode = String.Empty;
			CurrencyCode = String.Empty;
			ChargeAmount = 0M;
			ForeignCurrencyCode = String.Empty;
			ForeignAmount = 0M;
			FechaAplicaCompra = new DateTime();
			PorcIva = 0;
			TipoCargo = String.Empty;
			TipoAcumulado = String.Empty;
			IdPagosCab = 0;
			EsPagoParcial = false;
			MontoPagado = 0M;
			EsFacturable = false;
			EstatusFacturacion = String.Empty;
			IdFacturaCab = 0;
			FolioPreFactura = 0;
			ClasFact = 0;
			IdConcepto = 0;
			IdFolioFacturaGlobal = 0;
			IdVuelo = 0;
			NumJourney = 0;
			LiftStatus = String.Empty;
			CreatedAgentID = 0;
			CreatedDate = new DateTime();
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_Reserva_His
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdReservaCab", SqlDbType.BigInt);
			param0.Value = IdReservaCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdReservaDet", SqlDbType.SmallInt);
			param1.Value = IdReservaDet;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@NumHistorico", SqlDbType.SmallInt);
			param2.Value = NumHistorico;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@FechaHoraHistorico", SqlDbType.VarChar);
			param3.Value = FechaHoraHistorico.Year > 1900 ? FechaHoraHistorico.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Orden", SqlDbType.SmallInt);
			param4.Value = Orden;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@FeeNumber", SqlDbType.SmallInt);
			param5.Value = FeeNumber;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@PassengerID", SqlDbType.BigInt);
			param6.Value = PassengerID;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@SegmentID", SqlDbType.BigInt);
			param7.Value = SegmentID;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@ChargeNumber", SqlDbType.SmallInt);
			param8.Value = ChargeNumber;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@ChargeType", SqlDbType.SmallInt);
			param9.Value = ChargeType;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@IdFee", SqlDbType.Int);
			param10.Value = IdFee;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@ChargeCode", SqlDbType.VarChar);
			param11.Value = ChargeCode;
			commandParameters.Add(param11);
			SqlParameter param12 = new SqlParameter("@ChargeDetail", SqlDbType.VarChar);
			param12.Value = ChargeDetail;
			commandParameters.Add(param12);
			SqlParameter param13 = new SqlParameter("@TicketCode", SqlDbType.VarChar);
			param13.Value = TicketCode;
			commandParameters.Add(param13);
			SqlParameter param14 = new SqlParameter("@CurrencyCode", SqlDbType.VarChar);
			param14.Value = CurrencyCode;
			commandParameters.Add(param14);
			SqlParameter param15 = new SqlParameter("@ChargeAmount", SqlDbType.Money);
			param15.Value = ChargeAmount;
			commandParameters.Add(param15);
			SqlParameter param16 = new SqlParameter("@ForeignCurrencyCode", SqlDbType.VarChar);
			param16.Value = ForeignCurrencyCode;
			commandParameters.Add(param16);
			SqlParameter param17 = new SqlParameter("@ForeignAmount", SqlDbType.Money);
			param17.Value = ForeignAmount;
			commandParameters.Add(param17);
			SqlParameter param18 = new SqlParameter("@FechaAplicaCompra", SqlDbType.VarChar);
			param18.Value = FechaAplicaCompra.Year > 1900 ? FechaAplicaCompra.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param18);
			SqlParameter param19 = new SqlParameter("@PorcIva", SqlDbType.TinyInt);
			param19.Value = PorcIva;
			commandParameters.Add(param19);
			SqlParameter param20 = new SqlParameter("@TipoCargo", SqlDbType.VarChar);
			param20.Value = TipoCargo;
			commandParameters.Add(param20);
			SqlParameter param21 = new SqlParameter("@TipoAcumulado", SqlDbType.VarChar);
			param21.Value = TipoAcumulado;
			commandParameters.Add(param21);
			SqlParameter param22 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param22.Value = IdPagosCab;
			commandParameters.Add(param22);
			SqlParameter param23 = new SqlParameter("@EsPagoParcial", SqlDbType.Bit);
			param23.Value = EsPagoParcial;
			commandParameters.Add(param23);
			SqlParameter param24 = new SqlParameter("@MontoPagado", SqlDbType.Money);
			param24.Value = MontoPagado;
			commandParameters.Add(param24);
			SqlParameter param25 = new SqlParameter("@EsFacturable", SqlDbType.Bit);
			param25.Value = EsFacturable;
			commandParameters.Add(param25);
			SqlParameter param26 = new SqlParameter("@EstatusFacturacion", SqlDbType.VarChar);
			param26.Value = EstatusFacturacion;
			commandParameters.Add(param26);
			SqlParameter param27 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param27.Value = IdFacturaCab;
			commandParameters.Add(param27);
			SqlParameter param28 = new SqlParameter("@FolioPreFactura", SqlDbType.BigInt);
			param28.Value = FolioPreFactura;
			commandParameters.Add(param28);
			SqlParameter param29 = new SqlParameter("@ClasFact", SqlDbType.TinyInt);
			param29.Value = ClasFact;
			commandParameters.Add(param29);
			SqlParameter param30 = new SqlParameter("@IdConcepto", SqlDbType.SmallInt);
			param30.Value = IdConcepto;
			commandParameters.Add(param30);
			SqlParameter param31 = new SqlParameter("@IdFolioFacturaGlobal", SqlDbType.BigInt);
			param31.Value = IdFolioFacturaGlobal;
			commandParameters.Add(param31);
			SqlParameter param32 = new SqlParameter("@IdVuelo", SqlDbType.BigInt);
			param32.Value = IdVuelo;
			commandParameters.Add(param32);
			SqlParameter param33 = new SqlParameter("@NumJourney", SqlDbType.TinyInt);
			param33.Value = NumJourney;
			commandParameters.Add(param33);
			SqlParameter param34 = new SqlParameter("@LiftStatus", SqlDbType.VarChar);
			param34.Value = LiftStatus;
			commandParameters.Add(param34);
			SqlParameter param35 = new SqlParameter("@CreatedAgentID", SqlDbType.BigInt);
			param35.Value = CreatedAgentID;
			commandParameters.Add(param35);
			SqlParameter param36 = new SqlParameter("@CreatedDate", SqlDbType.VarChar);
			param36.Value = CreatedDate.Year > 1900 ? CreatedDate.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param36);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsReservaHis";
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

		#region Actualizar VBFac_Reserva_His
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdReservaCab", SqlDbType.BigInt);
			param0.Value = IdReservaCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdReservaDet", SqlDbType.SmallInt);
			param1.Value = IdReservaDet;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@NumHistorico", SqlDbType.SmallInt);
			param2.Value = NumHistorico;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@FechaHoraHistorico", SqlDbType.VarChar);
			param3.Value = FechaHoraHistorico.Year > 1900 ? FechaHoraHistorico.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Orden", SqlDbType.SmallInt);
			param4.Value = Orden;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@FeeNumber", SqlDbType.SmallInt);
			param5.Value = FeeNumber;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@PassengerID", SqlDbType.BigInt);
			param6.Value = PassengerID;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@SegmentID", SqlDbType.BigInt);
			param7.Value = SegmentID;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@ChargeNumber", SqlDbType.SmallInt);
			param8.Value = ChargeNumber;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@ChargeType", SqlDbType.SmallInt);
			param9.Value = ChargeType;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@IdFee", SqlDbType.Int);
			param10.Value = IdFee;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@ChargeCode", SqlDbType.VarChar);
			param11.Value = ChargeCode;
			commandParameters.Add(param11);
			SqlParameter param12 = new SqlParameter("@ChargeDetail", SqlDbType.VarChar);
			param12.Value = ChargeDetail;
			commandParameters.Add(param12);
			SqlParameter param13 = new SqlParameter("@TicketCode", SqlDbType.VarChar);
			param13.Value = TicketCode;
			commandParameters.Add(param13);
			SqlParameter param14 = new SqlParameter("@CurrencyCode", SqlDbType.VarChar);
			param14.Value = CurrencyCode;
			commandParameters.Add(param14);
			SqlParameter param15 = new SqlParameter("@ChargeAmount", SqlDbType.Money);
			param15.Value = ChargeAmount;
			commandParameters.Add(param15);
			SqlParameter param16 = new SqlParameter("@ForeignCurrencyCode", SqlDbType.VarChar);
			param16.Value = ForeignCurrencyCode;
			commandParameters.Add(param16);
			SqlParameter param17 = new SqlParameter("@ForeignAmount", SqlDbType.Money);
			param17.Value = ForeignAmount;
			commandParameters.Add(param17);
			SqlParameter param18 = new SqlParameter("@FechaAplicaCompra", SqlDbType.VarChar);
			param18.Value = FechaAplicaCompra.Year > 1900 ? FechaAplicaCompra.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param18);
			SqlParameter param19 = new SqlParameter("@PorcIva", SqlDbType.TinyInt);
			param19.Value = PorcIva;
			commandParameters.Add(param19);
			SqlParameter param20 = new SqlParameter("@TipoCargo", SqlDbType.VarChar);
			param20.Value = TipoCargo;
			commandParameters.Add(param20);
			SqlParameter param21 = new SqlParameter("@TipoAcumulado", SqlDbType.VarChar);
			param21.Value = TipoAcumulado;
			commandParameters.Add(param21);
			SqlParameter param22 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param22.Value = IdPagosCab;
			commandParameters.Add(param22);
			SqlParameter param23 = new SqlParameter("@EsPagoParcial", SqlDbType.Bit);
			param23.Value = EsPagoParcial;
			commandParameters.Add(param23);
			SqlParameter param24 = new SqlParameter("@MontoPagado", SqlDbType.Money);
			param24.Value = MontoPagado;
			commandParameters.Add(param24);
			SqlParameter param25 = new SqlParameter("@EsFacturable", SqlDbType.Bit);
			param25.Value = EsFacturable;
			commandParameters.Add(param25);
			SqlParameter param26 = new SqlParameter("@EstatusFacturacion", SqlDbType.VarChar);
			param26.Value = EstatusFacturacion;
			commandParameters.Add(param26);
			SqlParameter param27 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param27.Value = IdFacturaCab;
			commandParameters.Add(param27);
			SqlParameter param28 = new SqlParameter("@FolioPreFactura", SqlDbType.BigInt);
			param28.Value = FolioPreFactura;
			commandParameters.Add(param28);
			SqlParameter param29 = new SqlParameter("@ClasFact", SqlDbType.TinyInt);
			param29.Value = ClasFact;
			commandParameters.Add(param29);
			SqlParameter param30 = new SqlParameter("@IdConcepto", SqlDbType.SmallInt);
			param30.Value = IdConcepto;
			commandParameters.Add(param30);
			SqlParameter param31 = new SqlParameter("@IdFolioFacturaGlobal", SqlDbType.BigInt);
			param31.Value = IdFolioFacturaGlobal;
			commandParameters.Add(param31);
			SqlParameter param32 = new SqlParameter("@IdVuelo", SqlDbType.BigInt);
			param32.Value = IdVuelo;
			commandParameters.Add(param32);
			SqlParameter param33 = new SqlParameter("@NumJourney", SqlDbType.TinyInt);
			param33.Value = NumJourney;
			commandParameters.Add(param33);
			SqlParameter param34 = new SqlParameter("@LiftStatus", SqlDbType.VarChar);
			param34.Value = LiftStatus;
			commandParameters.Add(param34);
			SqlParameter param35 = new SqlParameter("@CreatedAgentID", SqlDbType.BigInt);
			param35.Value = CreatedAgentID;
			commandParameters.Add(param35);
			SqlParameter param36 = new SqlParameter("@CreatedDate", SqlDbType.VarChar);
			param36.Value = CreatedDate.Year > 1900 ? CreatedDate.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param36);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdReservaHis";
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

		#region Eliminar VBFac_Reserva_His
		public void Eliminar(long idreservacab,int idreservadet,int numhistorico)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdReservaCab", SqlDbType.BigInt);
			param0.Value = idreservacab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdReservaDet", SqlDbType.SmallInt);
			param1.Value = idreservadet;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@NumHistorico", SqlDbType.SmallInt);
			param2.Value = numhistorico;
			commandParameters.Add(param2);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelReservaHis";
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

		#region Deshacer VBFac_Reserva_His
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdReservaCab,IdReservaDet,NumHistorico);
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
		public List<ENTReservaHis> RecuperarTodo()
		{
			List<ENTReservaHis> result = new List<ENTReservaHis>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetReservaHis_TODO";
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
				    ENTReservaHis item = new ENTReservaHis();
					 if (!dr.IsNull("IdReservaCab")) item.IdReservaCab = Convert.ToInt64(dr["IdReservaCab"]);
					 if (!dr.IsNull("IdReservaDet")) item.IdReservaDet = Convert.ToInt16(dr["IdReservaDet"]);
					 if (!dr.IsNull("NumHistorico")) item.NumHistorico = Convert.ToInt16(dr["NumHistorico"]);
					 if (!dr.IsNull("FechaHoraHistorico")) item.FechaHoraHistorico = Convert.ToDateTime(dr["FechaHoraHistorico"]);
					 if (!dr.IsNull("Orden")) item.Orden = Convert.ToInt16(dr["Orden"]);
					 if (!dr.IsNull("FeeNumber")) item.FeeNumber = Convert.ToInt16(dr["FeeNumber"]);
					 if (!dr.IsNull("PassengerID")) item.PassengerID = Convert.ToInt64(dr["PassengerID"]);
					 if (!dr.IsNull("SegmentID")) item.SegmentID = Convert.ToInt64(dr["SegmentID"]);
					 if (!dr.IsNull("ChargeNumber")) item.ChargeNumber = Convert.ToInt16(dr["ChargeNumber"]);
					 if (!dr.IsNull("ChargeType")) item.ChargeType = Convert.ToInt16(dr["ChargeType"]);
					 if (!dr.IsNull("IdFee")) item.IdFee = Convert.ToInt32(dr["IdFee"]);
					 if (!dr.IsNull("ChargeCode")) item.ChargeCode = dr["ChargeCode"].ToString();
					 if (!dr.IsNull("ChargeDetail")) item.ChargeDetail = dr["ChargeDetail"].ToString();
					 if (!dr.IsNull("TicketCode")) item.TicketCode = dr["TicketCode"].ToString();
					 if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					 if (!dr.IsNull("ChargeAmount")) item.ChargeAmount = Convert.ToDecimal(dr["ChargeAmount"]);
					 if (!dr.IsNull("ForeignCurrencyCode")) item.ForeignCurrencyCode = dr["ForeignCurrencyCode"].ToString();
					 if (!dr.IsNull("ForeignAmount")) item.ForeignAmount = Convert.ToDecimal(dr["ForeignAmount"]);
					 if (!dr.IsNull("FechaAplicaCompra")) item.FechaAplicaCompra = Convert.ToDateTime(dr["FechaAplicaCompra"]);
					 if (!dr.IsNull("PorcIva")) item.PorcIva = Convert.ToByte(dr["PorcIva"]);
					 if (!dr.IsNull("TipoCargo")) item.TipoCargo = dr["TipoCargo"].ToString();
					 if (!dr.IsNull("TipoAcumulado")) item.TipoAcumulado = dr["TipoAcumulado"].ToString();
					 if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					 if (!dr.IsNull("EsPagoParcial")) item.EsPagoParcial = Convert.ToBoolean(dr["EsPagoParcial"]);
					 if (!dr.IsNull("MontoPagado")) item.MontoPagado = Convert.ToDecimal(dr["MontoPagado"]);
					 if (!dr.IsNull("EsFacturable")) item.EsFacturable = Convert.ToBoolean(dr["EsFacturable"]);
					 if (!dr.IsNull("EstatusFacturacion")) item.EstatusFacturacion = dr["EstatusFacturacion"].ToString();
					 if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					 if (!dr.IsNull("FolioPreFactura")) item.FolioPreFactura = Convert.ToInt64(dr["FolioPreFactura"]);
					 if (!dr.IsNull("ClasFact")) item.ClasFact = Convert.ToByte(dr["ClasFact"]);
					 if (!dr.IsNull("IdConcepto")) item.IdConcepto = Convert.ToInt16(dr["IdConcepto"]);
					 if (!dr.IsNull("IdFolioFacturaGlobal")) item.IdFolioFacturaGlobal = Convert.ToInt64(dr["IdFolioFacturaGlobal"]);
					 if (!dr.IsNull("IdVuelo")) item.IdVuelo = Convert.ToInt64(dr["IdVuelo"]);
					 if (!dr.IsNull("NumJourney")) item.NumJourney = Convert.ToByte(dr["NumJourney"]);
					 if (!dr.IsNull("LiftStatus")) item.LiftStatus = dr["LiftStatus"].ToString();
					 if (!dr.IsNull("CreatedAgentID")) item.CreatedAgentID = Convert.ToInt64(dr["CreatedAgentID"]);
					 if (!dr.IsNull("CreatedDate")) item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
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

		#region Recuperar RecuperarReservaHisPorLlavePrimaria
		public List<ENTReservaHis> RecuperarReservaHisPorLlavePrimaria(long idreservacab,int idreservadet,int numhistorico)
		{
			List<ENTReservaHis> result = new List<ENTReservaHis>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdReservaCab", SqlDbType.BigInt);
			param0.Value = idreservacab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdReservaDet", SqlDbType.SmallInt);
			param1.Value = idreservadet;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@NumHistorico", SqlDbType.SmallInt);
			param2.Value = numhistorico;
			commandParameters.Add(param2);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetReservaHis_POR_PK";
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
				    ENTReservaHis item = new ENTReservaHis();
					if (!dr.IsNull("IdReservaCab")) item.IdReservaCab = Convert.ToInt64(dr["IdReservaCab"]);
					if (!dr.IsNull("IdReservaDet")) item.IdReservaDet = Convert.ToInt16(dr["IdReservaDet"]);
					if (!dr.IsNull("NumHistorico")) item.NumHistorico = Convert.ToInt16(dr["NumHistorico"]);
					if (!dr.IsNull("FechaHoraHistorico")) item.FechaHoraHistorico = Convert.ToDateTime(dr["FechaHoraHistorico"]);
					if (!dr.IsNull("Orden")) item.Orden = Convert.ToInt16(dr["Orden"]);
					if (!dr.IsNull("FeeNumber")) item.FeeNumber = Convert.ToInt16(dr["FeeNumber"]);
					if (!dr.IsNull("PassengerID")) item.PassengerID = Convert.ToInt64(dr["PassengerID"]);
					if (!dr.IsNull("SegmentID")) item.SegmentID = Convert.ToInt64(dr["SegmentID"]);
					if (!dr.IsNull("ChargeNumber")) item.ChargeNumber = Convert.ToInt16(dr["ChargeNumber"]);
					if (!dr.IsNull("ChargeType")) item.ChargeType = Convert.ToInt16(dr["ChargeType"]);
					if (!dr.IsNull("IdFee")) item.IdFee = Convert.ToInt32(dr["IdFee"]);
					if (!dr.IsNull("ChargeCode")) item.ChargeCode = dr["ChargeCode"].ToString();
					if (!dr.IsNull("ChargeDetail")) item.ChargeDetail = dr["ChargeDetail"].ToString();
					if (!dr.IsNull("TicketCode")) item.TicketCode = dr["TicketCode"].ToString();
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("ChargeAmount")) item.ChargeAmount = Convert.ToDecimal(dr["ChargeAmount"]);
					if (!dr.IsNull("ForeignCurrencyCode")) item.ForeignCurrencyCode = dr["ForeignCurrencyCode"].ToString();
					if (!dr.IsNull("ForeignAmount")) item.ForeignAmount = Convert.ToDecimal(dr["ForeignAmount"]);
					if (!dr.IsNull("FechaAplicaCompra")) item.FechaAplicaCompra = Convert.ToDateTime(dr["FechaAplicaCompra"]);
					if (!dr.IsNull("PorcIva")) item.PorcIva = Convert.ToByte(dr["PorcIva"]);
					if (!dr.IsNull("TipoCargo")) item.TipoCargo = dr["TipoCargo"].ToString();
					if (!dr.IsNull("TipoAcumulado")) item.TipoAcumulado = dr["TipoAcumulado"].ToString();
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("EsPagoParcial")) item.EsPagoParcial = Convert.ToBoolean(dr["EsPagoParcial"]);
					if (!dr.IsNull("MontoPagado")) item.MontoPagado = Convert.ToDecimal(dr["MontoPagado"]);
					if (!dr.IsNull("EsFacturable")) item.EsFacturable = Convert.ToBoolean(dr["EsFacturable"]);
					if (!dr.IsNull("EstatusFacturacion")) item.EstatusFacturacion = dr["EstatusFacturacion"].ToString();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("FolioPreFactura")) item.FolioPreFactura = Convert.ToInt64(dr["FolioPreFactura"]);
					if (!dr.IsNull("ClasFact")) item.ClasFact = Convert.ToByte(dr["ClasFact"]);
					if (!dr.IsNull("IdConcepto")) item.IdConcepto = Convert.ToInt16(dr["IdConcepto"]);
					if (!dr.IsNull("IdFolioFacturaGlobal")) item.IdFolioFacturaGlobal = Convert.ToInt64(dr["IdFolioFacturaGlobal"]);
					if (!dr.IsNull("IdVuelo")) item.IdVuelo = Convert.ToInt64(dr["IdVuelo"]);
					if (!dr.IsNull("NumJourney")) item.NumJourney = Convert.ToByte(dr["NumJourney"]);
					if (!dr.IsNull("LiftStatus")) item.LiftStatus = dr["LiftStatus"].ToString();
					if (!dr.IsNull("CreatedAgentID")) item.CreatedAgentID = Convert.ToInt64(dr["CreatedAgentID"]);
					if (!dr.IsNull("CreatedDate")) item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdReservaCab")) IdReservaCab = Convert.ToInt64(dtResultado.Rows[0]["IdReservaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdReservaDet")) IdReservaDet = Convert.ToInt16(dtResultado.Rows[0]["IdReservaDet"]);
					if (!dtResultado.Rows[0].IsNull("NumHistorico")) NumHistorico = Convert.ToInt16(dtResultado.Rows[0]["NumHistorico"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraHistorico")) FechaHoraHistorico = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraHistorico"]);
					if (!dtResultado.Rows[0].IsNull("Orden")) Orden = Convert.ToInt16(dtResultado.Rows[0]["Orden"]);
					if (!dtResultado.Rows[0].IsNull("FeeNumber")) FeeNumber = Convert.ToInt16(dtResultado.Rows[0]["FeeNumber"]);
					if (!dtResultado.Rows[0].IsNull("PassengerID")) PassengerID = Convert.ToInt64(dtResultado.Rows[0]["PassengerID"]);
					if (!dtResultado.Rows[0].IsNull("SegmentID")) SegmentID = Convert.ToInt64(dtResultado.Rows[0]["SegmentID"]);
					if (!dtResultado.Rows[0].IsNull("ChargeNumber")) ChargeNumber = Convert.ToInt16(dtResultado.Rows[0]["ChargeNumber"]);
					if (!dtResultado.Rows[0].IsNull("ChargeType")) ChargeType = Convert.ToInt16(dtResultado.Rows[0]["ChargeType"]);
					if (!dtResultado.Rows[0].IsNull("IdFee")) IdFee = Convert.ToInt32(dtResultado.Rows[0]["IdFee"]);
					if (!dtResultado.Rows[0].IsNull("ChargeCode")) ChargeCode = dtResultado.Rows[0]["ChargeCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("ChargeDetail")) ChargeDetail = dtResultado.Rows[0]["ChargeDetail"].ToString();
					if (!dtResultado.Rows[0].IsNull("TicketCode")) TicketCode = dtResultado.Rows[0]["TicketCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("ChargeAmount")) ChargeAmount = Convert.ToDecimal(dtResultado.Rows[0]["ChargeAmount"]);
					if (!dtResultado.Rows[0].IsNull("ForeignCurrencyCode")) ForeignCurrencyCode = dtResultado.Rows[0]["ForeignCurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("ForeignAmount")) ForeignAmount = Convert.ToDecimal(dtResultado.Rows[0]["ForeignAmount"]);
					if (!dtResultado.Rows[0].IsNull("FechaAplicaCompra")) FechaAplicaCompra = Convert.ToDateTime(dtResultado.Rows[0]["FechaAplicaCompra"]);
					if (!dtResultado.Rows[0].IsNull("PorcIva")) PorcIva = Convert.ToByte(dtResultado.Rows[0]["PorcIva"]);
					if (!dtResultado.Rows[0].IsNull("TipoCargo")) TipoCargo = dtResultado.Rows[0]["TipoCargo"].ToString();
					if (!dtResultado.Rows[0].IsNull("TipoAcumulado")) TipoAcumulado = dtResultado.Rows[0]["TipoAcumulado"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoParcial")) EsPagoParcial = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoParcial"]);
					if (!dtResultado.Rows[0].IsNull("MontoPagado")) MontoPagado = Convert.ToDecimal(dtResultado.Rows[0]["MontoPagado"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturable")) EsFacturable = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturable"]);
					if (!dtResultado.Rows[0].IsNull("EstatusFacturacion")) EstatusFacturacion = dtResultado.Rows[0]["EstatusFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("FolioPreFactura")) FolioPreFactura = Convert.ToInt64(dtResultado.Rows[0]["FolioPreFactura"]);
					if (!dtResultado.Rows[0].IsNull("ClasFact")) ClasFact = Convert.ToByte(dtResultado.Rows[0]["ClasFact"]);
					if (!dtResultado.Rows[0].IsNull("IdConcepto")) IdConcepto = Convert.ToInt16(dtResultado.Rows[0]["IdConcepto"]);
					if (!dtResultado.Rows[0].IsNull("IdFolioFacturaGlobal")) IdFolioFacturaGlobal = Convert.ToInt64(dtResultado.Rows[0]["IdFolioFacturaGlobal"]);
					if (!dtResultado.Rows[0].IsNull("IdVuelo")) IdVuelo = Convert.ToInt64(dtResultado.Rows[0]["IdVuelo"]);
					if (!dtResultado.Rows[0].IsNull("NumJourney")) NumJourney = Convert.ToByte(dtResultado.Rows[0]["NumJourney"]);
					if (!dtResultado.Rows[0].IsNull("LiftStatus")) LiftStatus = dtResultado.Rows[0]["LiftStatus"].ToString();
					if (!dtResultado.Rows[0].IsNull("CreatedAgentID")) CreatedAgentID = Convert.ToInt64(dtResultado.Rows[0]["CreatedAgentID"]);
					if (!dtResultado.Rows[0].IsNull("CreatedDate")) CreatedDate = Convert.ToDateTime(dtResultado.Rows[0]["CreatedDate"]);
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

		#region Recuperar RecuperarReservaHisIdreservacabIdreservadet
		public List<ENTReservaHis> RecuperarReservaHisIdreservacabIdreservadet(long idreservacab,int idreservadet)
		{
			List<ENTReservaHis> result = new List<ENTReservaHis>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdReservaCab", SqlDbType.BigInt);
			param0.Value = idreservacab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdReservaDet", SqlDbType.SmallInt);
			param1.Value = idreservadet;
			commandParameters.Add(param1);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetReservaHis_POR_IdReservaCab_IdReservaDet";
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
				    ENTReservaHis item = new ENTReservaHis();
					if (!dr.IsNull("IdReservaCab")) item.IdReservaCab = Convert.ToInt64(dr["IdReservaCab"]);
					if (!dr.IsNull("IdReservaDet")) item.IdReservaDet = Convert.ToInt16(dr["IdReservaDet"]);
					if (!dr.IsNull("NumHistorico")) item.NumHistorico = Convert.ToInt16(dr["NumHistorico"]);
					if (!dr.IsNull("FechaHoraHistorico")) item.FechaHoraHistorico = Convert.ToDateTime(dr["FechaHoraHistorico"]);
					if (!dr.IsNull("Orden")) item.Orden = Convert.ToInt16(dr["Orden"]);
					if (!dr.IsNull("FeeNumber")) item.FeeNumber = Convert.ToInt16(dr["FeeNumber"]);
					if (!dr.IsNull("PassengerID")) item.PassengerID = Convert.ToInt64(dr["PassengerID"]);
					if (!dr.IsNull("SegmentID")) item.SegmentID = Convert.ToInt64(dr["SegmentID"]);
					if (!dr.IsNull("ChargeNumber")) item.ChargeNumber = Convert.ToInt16(dr["ChargeNumber"]);
					if (!dr.IsNull("ChargeType")) item.ChargeType = Convert.ToInt16(dr["ChargeType"]);
					if (!dr.IsNull("IdFee")) item.IdFee = Convert.ToInt32(dr["IdFee"]);
					if (!dr.IsNull("ChargeCode")) item.ChargeCode = dr["ChargeCode"].ToString();
					if (!dr.IsNull("ChargeDetail")) item.ChargeDetail = dr["ChargeDetail"].ToString();
					if (!dr.IsNull("TicketCode")) item.TicketCode = dr["TicketCode"].ToString();
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("ChargeAmount")) item.ChargeAmount = Convert.ToDecimal(dr["ChargeAmount"]);
					if (!dr.IsNull("ForeignCurrencyCode")) item.ForeignCurrencyCode = dr["ForeignCurrencyCode"].ToString();
					if (!dr.IsNull("ForeignAmount")) item.ForeignAmount = Convert.ToDecimal(dr["ForeignAmount"]);
					if (!dr.IsNull("FechaAplicaCompra")) item.FechaAplicaCompra = Convert.ToDateTime(dr["FechaAplicaCompra"]);
					if (!dr.IsNull("PorcIva")) item.PorcIva = Convert.ToByte(dr["PorcIva"]);
					if (!dr.IsNull("TipoCargo")) item.TipoCargo = dr["TipoCargo"].ToString();
					if (!dr.IsNull("TipoAcumulado")) item.TipoAcumulado = dr["TipoAcumulado"].ToString();
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("EsPagoParcial")) item.EsPagoParcial = Convert.ToBoolean(dr["EsPagoParcial"]);
					if (!dr.IsNull("MontoPagado")) item.MontoPagado = Convert.ToDecimal(dr["MontoPagado"]);
					if (!dr.IsNull("EsFacturable")) item.EsFacturable = Convert.ToBoolean(dr["EsFacturable"]);
					if (!dr.IsNull("EstatusFacturacion")) item.EstatusFacturacion = dr["EstatusFacturacion"].ToString();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("FolioPreFactura")) item.FolioPreFactura = Convert.ToInt64(dr["FolioPreFactura"]);
					if (!dr.IsNull("ClasFact")) item.ClasFact = Convert.ToByte(dr["ClasFact"]);
					if (!dr.IsNull("IdConcepto")) item.IdConcepto = Convert.ToInt16(dr["IdConcepto"]);
					if (!dr.IsNull("IdFolioFacturaGlobal")) item.IdFolioFacturaGlobal = Convert.ToInt64(dr["IdFolioFacturaGlobal"]);
					if (!dr.IsNull("IdVuelo")) item.IdVuelo = Convert.ToInt64(dr["IdVuelo"]);
					if (!dr.IsNull("NumJourney")) item.NumJourney = Convert.ToByte(dr["NumJourney"]);
					if (!dr.IsNull("LiftStatus")) item.LiftStatus = dr["LiftStatus"].ToString();
					if (!dr.IsNull("CreatedAgentID")) item.CreatedAgentID = Convert.ToInt64(dr["CreatedAgentID"]);
					if (!dr.IsNull("CreatedDate")) item.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdReservaCab")) IdReservaCab = Convert.ToInt64(dtResultado.Rows[0]["IdReservaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdReservaDet")) IdReservaDet = Convert.ToInt16(dtResultado.Rows[0]["IdReservaDet"]);
					if (!dtResultado.Rows[0].IsNull("NumHistorico")) NumHistorico = Convert.ToInt16(dtResultado.Rows[0]["NumHistorico"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraHistorico")) FechaHoraHistorico = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraHistorico"]);
					if (!dtResultado.Rows[0].IsNull("Orden")) Orden = Convert.ToInt16(dtResultado.Rows[0]["Orden"]);
					if (!dtResultado.Rows[0].IsNull("FeeNumber")) FeeNumber = Convert.ToInt16(dtResultado.Rows[0]["FeeNumber"]);
					if (!dtResultado.Rows[0].IsNull("PassengerID")) PassengerID = Convert.ToInt64(dtResultado.Rows[0]["PassengerID"]);
					if (!dtResultado.Rows[0].IsNull("SegmentID")) SegmentID = Convert.ToInt64(dtResultado.Rows[0]["SegmentID"]);
					if (!dtResultado.Rows[0].IsNull("ChargeNumber")) ChargeNumber = Convert.ToInt16(dtResultado.Rows[0]["ChargeNumber"]);
					if (!dtResultado.Rows[0].IsNull("ChargeType")) ChargeType = Convert.ToInt16(dtResultado.Rows[0]["ChargeType"]);
					if (!dtResultado.Rows[0].IsNull("IdFee")) IdFee = Convert.ToInt32(dtResultado.Rows[0]["IdFee"]);
					if (!dtResultado.Rows[0].IsNull("ChargeCode")) ChargeCode = dtResultado.Rows[0]["ChargeCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("ChargeDetail")) ChargeDetail = dtResultado.Rows[0]["ChargeDetail"].ToString();
					if (!dtResultado.Rows[0].IsNull("TicketCode")) TicketCode = dtResultado.Rows[0]["TicketCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("ChargeAmount")) ChargeAmount = Convert.ToDecimal(dtResultado.Rows[0]["ChargeAmount"]);
					if (!dtResultado.Rows[0].IsNull("ForeignCurrencyCode")) ForeignCurrencyCode = dtResultado.Rows[0]["ForeignCurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("ForeignAmount")) ForeignAmount = Convert.ToDecimal(dtResultado.Rows[0]["ForeignAmount"]);
					if (!dtResultado.Rows[0].IsNull("FechaAplicaCompra")) FechaAplicaCompra = Convert.ToDateTime(dtResultado.Rows[0]["FechaAplicaCompra"]);
					if (!dtResultado.Rows[0].IsNull("PorcIva")) PorcIva = Convert.ToByte(dtResultado.Rows[0]["PorcIva"]);
					if (!dtResultado.Rows[0].IsNull("TipoCargo")) TipoCargo = dtResultado.Rows[0]["TipoCargo"].ToString();
					if (!dtResultado.Rows[0].IsNull("TipoAcumulado")) TipoAcumulado = dtResultado.Rows[0]["TipoAcumulado"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoParcial")) EsPagoParcial = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoParcial"]);
					if (!dtResultado.Rows[0].IsNull("MontoPagado")) MontoPagado = Convert.ToDecimal(dtResultado.Rows[0]["MontoPagado"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturable")) EsFacturable = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturable"]);
					if (!dtResultado.Rows[0].IsNull("EstatusFacturacion")) EstatusFacturacion = dtResultado.Rows[0]["EstatusFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("FolioPreFactura")) FolioPreFactura = Convert.ToInt64(dtResultado.Rows[0]["FolioPreFactura"]);
					if (!dtResultado.Rows[0].IsNull("ClasFact")) ClasFact = Convert.ToByte(dtResultado.Rows[0]["ClasFact"]);
					if (!dtResultado.Rows[0].IsNull("IdConcepto")) IdConcepto = Convert.ToInt16(dtResultado.Rows[0]["IdConcepto"]);
					if (!dtResultado.Rows[0].IsNull("IdFolioFacturaGlobal")) IdFolioFacturaGlobal = Convert.ToInt64(dtResultado.Rows[0]["IdFolioFacturaGlobal"]);
					if (!dtResultado.Rows[0].IsNull("IdVuelo")) IdVuelo = Convert.ToInt64(dtResultado.Rows[0]["IdVuelo"]);
					if (!dtResultado.Rows[0].IsNull("NumJourney")) NumJourney = Convert.ToByte(dtResultado.Rows[0]["NumJourney"]);
					if (!dtResultado.Rows[0].IsNull("LiftStatus")) LiftStatus = dtResultado.Rows[0]["LiftStatus"].ToString();
					if (!dtResultado.Rows[0].IsNull("CreatedAgentID")) CreatedAgentID = Convert.ToInt64(dtResultado.Rows[0]["CreatedAgentID"]);
					if (!dtResultado.Rows[0].IsNull("CreatedDate")) CreatedDate = Convert.ToDateTime(dtResultado.Rows[0]["CreatedDate"]);
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
			_copia = new DALReservaHis(_conexion);
			Type tipo = typeof(ENTReservaHis);
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

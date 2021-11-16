using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALPagosCab: ENTPagosCab
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALPagosCab _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALPagosCab(SqlConnection conexion)
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
			BookingID = 0;
			PaymentID = 0;
			FechaPago = new DateTime();
			FechaPagoUTC = new DateTime();
			IdFormaPago = 0;
			PaymentMethodCode = String.Empty;
			CurrencyCode = String.Empty;
			PaymentAmount = 0M;
			MontoPorAplicar = 0M;
			CollectedCurrencyCode = String.Empty;
			CollectedAmount = 0M;
			TipoCambio = 0M;
			Estatus = 0;
			ParentPaymentID = 0;
			EsPagoDividido = false;
			EsPagoPadre = false;
			EsPagoHijo = false;
			EsFacturable = false;
			EsParaAplicar = false;
			EsFacturado = false;
			IdFacturaCab = 0;
			IdFacturaGlobal = 0;
			FolioPrefactura = 0;
			FolioFactura = 0;
			FechaFactura = new DateTime();
			FechaFacturaUTC = new DateTime();
			VersionFacturacion = String.Empty;
			MontoTarifa = 0M;
			MontoServAdic = 0M;
			MontoTUA = 0M;
			MontoOtrosCargos = 0M;
			MontoIVA = 0M;
			MontoTotal = 0M;
			IdAgente = 0;
			FechaHoraLocal = new DateTime();
			FechaUltimaActualizacion = new DateTime();
			IdNotaCreditoCab = 0;
			LocationCode = String.Empty;
			LugarExpedicion = String.Empty;
			OrganizationCode = String.Empty;
			BinRange = 0;
			IdUpdFormaPago = 0;
			UpdFormaPagModificadoPor = String.Empty;
			FechaUpdaFormaPag = new DateTime();
			DepartmentCode = String.Empty;
			DepartmentName = String.Empty;
			OrganizationType = 0;
			OrganizationName = String.Empty;
			EsFacturableGlobal = false;
		}
		#endregion

		#region Agregar VBFac_Pagos_Cab
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = IdPagosCab;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param1.Value = BookingID;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@PaymentID", SqlDbType.BigInt);
			param2.Value = PaymentID;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@FechaPago", SqlDbType.VarChar);
			param3.Value = FechaPago.Year > 1900 ? FechaPago.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@FechaPagoUTC", SqlDbType.VarChar);
			param4.Value = FechaPagoUTC.Year > 1900 ? FechaPagoUTC.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@IdFormaPago", SqlDbType.Int);
			param5.Value = IdFormaPago;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@PaymentMethodCode", SqlDbType.VarChar);
			param6.Value = PaymentMethodCode;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@CurrencyCode", SqlDbType.VarChar);
			param7.Value = CurrencyCode;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@PaymentAmount", SqlDbType.Money);
			param8.Value = PaymentAmount;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@MontoPorAplicar", SqlDbType.Money);
			param9.Value = MontoPorAplicar;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@CollectedCurrencyCode", SqlDbType.VarChar);
			param10.Value = CollectedCurrencyCode;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@CollectedAmount", SqlDbType.Money);
			param11.Value = CollectedAmount;
			commandParameters.Add(param11);
			SqlParameter param12 = new SqlParameter("@TipoCambio", SqlDbType.Money);
			param12.Value = TipoCambio;
			commandParameters.Add(param12);
			SqlParameter param13 = new SqlParameter("@Estatus", SqlDbType.SmallInt);
			param13.Value = Estatus;
			commandParameters.Add(param13);
			SqlParameter param14 = new SqlParameter("@ParentPaymentID", SqlDbType.BigInt);
			param14.Value = ParentPaymentID;
			commandParameters.Add(param14);
			SqlParameter param15 = new SqlParameter("@EsPagoDividido", SqlDbType.Bit);
			param15.Value = EsPagoDividido;
			commandParameters.Add(param15);
			SqlParameter param16 = new SqlParameter("@EsPagoPadre", SqlDbType.Bit);
			param16.Value = EsPagoPadre;
			commandParameters.Add(param16);
			SqlParameter param17 = new SqlParameter("@EsPagoHijo", SqlDbType.Bit);
			param17.Value = EsPagoHijo;
			commandParameters.Add(param17);
			SqlParameter param18 = new SqlParameter("@EsFacturable", SqlDbType.Bit);
			param18.Value = EsFacturable;
			commandParameters.Add(param18);
			SqlParameter param19 = new SqlParameter("@EsParaAplicar", SqlDbType.Bit);
			param19.Value = EsParaAplicar;
			commandParameters.Add(param19);
			SqlParameter param20 = new SqlParameter("@EsFacturado", SqlDbType.Bit);
			param20.Value = EsFacturado;
			commandParameters.Add(param20);
			SqlParameter param21 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param21.Value = IdFacturaCab;
			commandParameters.Add(param21);
			SqlParameter param22 = new SqlParameter("@IdFacturaGlobal", SqlDbType.BigInt);
			param22.Value = IdFacturaGlobal;
			commandParameters.Add(param22);
			SqlParameter param23 = new SqlParameter("@FolioPrefactura", SqlDbType.BigInt);
			param23.Value = FolioPrefactura;
			commandParameters.Add(param23);
			SqlParameter param24 = new SqlParameter("@FolioFactura", SqlDbType.BigInt);
			param24.Value = FolioFactura;
			commandParameters.Add(param24);
			SqlParameter param25 = new SqlParameter("@FechaFactura", SqlDbType.VarChar);
			param25.Value = FechaFactura.Year > 1900 ? FechaFactura.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param25);
			SqlParameter param26 = new SqlParameter("@FechaFacturaUTC", SqlDbType.VarChar);
			param26.Value = FechaFacturaUTC.Year > 1900 ? FechaFacturaUTC.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param26);
			SqlParameter param27 = new SqlParameter("@VersionFacturacion", SqlDbType.VarChar);
			param27.Value = VersionFacturacion;
			commandParameters.Add(param27);
			SqlParameter param28 = new SqlParameter("@MontoTarifa", SqlDbType.Money);
			param28.Value = MontoTarifa;
			commandParameters.Add(param28);
			SqlParameter param29 = new SqlParameter("@MontoServAdic", SqlDbType.Money);
			param29.Value = MontoServAdic;
			commandParameters.Add(param29);
			SqlParameter param30 = new SqlParameter("@MontoTUA", SqlDbType.Money);
			param30.Value = MontoTUA;
			commandParameters.Add(param30);
			SqlParameter param31 = new SqlParameter("@MontoOtrosCargos", SqlDbType.Money);
			param31.Value = MontoOtrosCargos;
			commandParameters.Add(param31);
			SqlParameter param32 = new SqlParameter("@MontoIVA", SqlDbType.Money);
			param32.Value = MontoIVA;
			commandParameters.Add(param32);
			SqlParameter param33 = new SqlParameter("@MontoTotal", SqlDbType.Money);
			param33.Value = MontoTotal;
			commandParameters.Add(param33);
			SqlParameter param34 = new SqlParameter("@IdAgente", SqlDbType.BigInt);
			param34.Value = IdAgente;
			commandParameters.Add(param34);
			SqlParameter param35 = new SqlParameter("@FechaUltimaActualizacion", SqlDbType.VarChar);
			param35.Value = FechaUltimaActualizacion.Year > 1900 ? FechaUltimaActualizacion.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param35);
			SqlParameter param36 = new SqlParameter("@IdNotaCreditoCab", SqlDbType.BigInt);
			param36.Value = IdNotaCreditoCab;
			commandParameters.Add(param36);
			SqlParameter param37 = new SqlParameter("@LocationCode", SqlDbType.VarChar);
			param37.Value = LocationCode;
			commandParameters.Add(param37);
			SqlParameter param38 = new SqlParameter("@LugarExpedicion", SqlDbType.VarChar);
			param38.Value = LugarExpedicion;
			commandParameters.Add(param38);
			SqlParameter param39 = new SqlParameter("@OrganizationCode", SqlDbType.VarChar);
			param39.Value = OrganizationCode;
			commandParameters.Add(param39);
			SqlParameter param40 = new SqlParameter("@BinRange", SqlDbType.Int);
			param40.Value = BinRange;
			commandParameters.Add(param40);
			SqlParameter param41 = new SqlParameter("@IdUpdFormaPago", SqlDbType.Int);
			param41.Value = IdUpdFormaPago;
			commandParameters.Add(param41);
			SqlParameter param42 = new SqlParameter("@UpdFormaPagModificadoPor", SqlDbType.VarChar);
			param42.Value = UpdFormaPagModificadoPor;
			commandParameters.Add(param42);
			SqlParameter param43 = new SqlParameter("@FechaUpdaFormaPag", SqlDbType.VarChar);
			param43.Value = FechaUpdaFormaPag.Year > 1900 ? FechaUpdaFormaPag.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param43);
			SqlParameter param44 = new SqlParameter("@DepartmentCode", SqlDbType.VarChar);
			param44.Value = DepartmentCode;
			commandParameters.Add(param44);
			SqlParameter param45 = new SqlParameter("@DepartmentName", SqlDbType.NVarChar);
			param45.Value = DepartmentName;
			commandParameters.Add(param45);
			SqlParameter param46 = new SqlParameter("@OrganizationType", SqlDbType.SmallInt);
			param46.Value = OrganizationType;
			commandParameters.Add(param46);
			SqlParameter param47 = new SqlParameter("@OrganizationName", SqlDbType.NVarChar);
			param47.Value = OrganizationName;
			commandParameters.Add(param47);
			SqlParameter param48 = new SqlParameter("@EsFacturableGlobal", SqlDbType.Bit);
			param48.Value = EsFacturableGlobal;
			commandParameters.Add(param48);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsPagosCab";
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
						IdPagosCab = Convert.ToInt64(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_Pagos_Cab
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = IdPagosCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param1.Value = BookingID;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@PaymentID", SqlDbType.BigInt);
			param2.Value = PaymentID;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@FechaPago", SqlDbType.VarChar);
			param3.Value = FechaPago.Year > 1900 ? FechaPago.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@FechaPagoUTC", SqlDbType.VarChar);
			param4.Value = FechaPagoUTC.Year > 1900 ? FechaPagoUTC.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@IdFormaPago", SqlDbType.Int);
			param5.Value = IdFormaPago;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@PaymentMethodCode", SqlDbType.VarChar);
			param6.Value = PaymentMethodCode;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@CurrencyCode", SqlDbType.VarChar);
			param7.Value = CurrencyCode;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@PaymentAmount", SqlDbType.Money);
			param8.Value = PaymentAmount;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@MontoPorAplicar", SqlDbType.Money);
			param9.Value = MontoPorAplicar;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@CollectedCurrencyCode", SqlDbType.VarChar);
			param10.Value = CollectedCurrencyCode;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@CollectedAmount", SqlDbType.Money);
			param11.Value = CollectedAmount;
			commandParameters.Add(param11);
			SqlParameter param12 = new SqlParameter("@TipoCambio", SqlDbType.Money);
			param12.Value = TipoCambio;
			commandParameters.Add(param12);
			SqlParameter param13 = new SqlParameter("@Estatus", SqlDbType.SmallInt);
			param13.Value = Estatus;
			commandParameters.Add(param13);
			SqlParameter param14 = new SqlParameter("@ParentPaymentID", SqlDbType.BigInt);
			param14.Value = ParentPaymentID;
			commandParameters.Add(param14);
			SqlParameter param15 = new SqlParameter("@EsPagoDividido", SqlDbType.Bit);
			param15.Value = EsPagoDividido;
			commandParameters.Add(param15);
			SqlParameter param16 = new SqlParameter("@EsPagoPadre", SqlDbType.Bit);
			param16.Value = EsPagoPadre;
			commandParameters.Add(param16);
			SqlParameter param17 = new SqlParameter("@EsPagoHijo", SqlDbType.Bit);
			param17.Value = EsPagoHijo;
			commandParameters.Add(param17);
			SqlParameter param18 = new SqlParameter("@EsFacturable", SqlDbType.Bit);
			param18.Value = EsFacturable;
			commandParameters.Add(param18);
			SqlParameter param19 = new SqlParameter("@EsParaAplicar", SqlDbType.Bit);
			param19.Value = EsParaAplicar;
			commandParameters.Add(param19);
			SqlParameter param20 = new SqlParameter("@EsFacturado", SqlDbType.Bit);
			param20.Value = EsFacturado;
			commandParameters.Add(param20);
			SqlParameter param21 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param21.Value = IdFacturaCab;
			commandParameters.Add(param21);
			SqlParameter param22 = new SqlParameter("@IdFacturaGlobal", SqlDbType.BigInt);
			param22.Value = IdFacturaGlobal;
			commandParameters.Add(param22);
			SqlParameter param23 = new SqlParameter("@FolioPrefactura", SqlDbType.BigInt);
			param23.Value = FolioPrefactura;
			commandParameters.Add(param23);
			SqlParameter param24 = new SqlParameter("@FolioFactura", SqlDbType.BigInt);
			param24.Value = FolioFactura;
			commandParameters.Add(param24);
			SqlParameter param25 = new SqlParameter("@FechaFactura", SqlDbType.VarChar);
			param25.Value = FechaFactura.Year > 1900 ? FechaFactura.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param25);
			SqlParameter param26 = new SqlParameter("@FechaFacturaUTC", SqlDbType.VarChar);
			param26.Value = FechaFacturaUTC.Year > 1900 ? FechaFacturaUTC.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param26);
			SqlParameter param27 = new SqlParameter("@VersionFacturacion", SqlDbType.VarChar);
			param27.Value = VersionFacturacion;
			commandParameters.Add(param27);
			SqlParameter param28 = new SqlParameter("@MontoTarifa", SqlDbType.Money);
			param28.Value = MontoTarifa;
			commandParameters.Add(param28);
			SqlParameter param29 = new SqlParameter("@MontoServAdic", SqlDbType.Money);
			param29.Value = MontoServAdic;
			commandParameters.Add(param29);
			SqlParameter param30 = new SqlParameter("@MontoTUA", SqlDbType.Money);
			param30.Value = MontoTUA;
			commandParameters.Add(param30);
			SqlParameter param31 = new SqlParameter("@MontoOtrosCargos", SqlDbType.Money);
			param31.Value = MontoOtrosCargos;
			commandParameters.Add(param31);
			SqlParameter param32 = new SqlParameter("@MontoIVA", SqlDbType.Money);
			param32.Value = MontoIVA;
			commandParameters.Add(param32);
			SqlParameter param33 = new SqlParameter("@MontoTotal", SqlDbType.Money);
			param33.Value = MontoTotal;
			commandParameters.Add(param33);
			SqlParameter param34 = new SqlParameter("@IdAgente", SqlDbType.BigInt);
			param34.Value = IdAgente;
			commandParameters.Add(param34);
			SqlParameter param35 = new SqlParameter("@FechaUltimaActualizacion", SqlDbType.VarChar);
			param35.Value = FechaUltimaActualizacion.Year > 1900 ? FechaUltimaActualizacion.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param35);
			SqlParameter param36 = new SqlParameter("@IdNotaCreditoCab", SqlDbType.BigInt);
			param36.Value = IdNotaCreditoCab;
			commandParameters.Add(param36);
			SqlParameter param37 = new SqlParameter("@LocationCode", SqlDbType.VarChar);
			param37.Value = LocationCode;
			commandParameters.Add(param37);
			SqlParameter param38 = new SqlParameter("@LugarExpedicion", SqlDbType.VarChar);
			param38.Value = LugarExpedicion;
			commandParameters.Add(param38);
			SqlParameter param39 = new SqlParameter("@OrganizationCode", SqlDbType.VarChar);
			param39.Value = OrganizationCode;
			commandParameters.Add(param39);
			SqlParameter param40 = new SqlParameter("@BinRange", SqlDbType.Int);
			param40.Value = BinRange;
			commandParameters.Add(param40);
			SqlParameter param41 = new SqlParameter("@IdUpdFormaPago", SqlDbType.Int);
			param41.Value = IdUpdFormaPago;
			commandParameters.Add(param41);
			SqlParameter param42 = new SqlParameter("@UpdFormaPagModificadoPor", SqlDbType.VarChar);
			param42.Value = UpdFormaPagModificadoPor;
			commandParameters.Add(param42);
			SqlParameter param43 = new SqlParameter("@FechaUpdaFormaPag", SqlDbType.VarChar);
			param43.Value = FechaUpdaFormaPag.Year > 1900 ? FechaUpdaFormaPag.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param43);
			SqlParameter param44 = new SqlParameter("@DepartmentCode", SqlDbType.VarChar);
			param44.Value = DepartmentCode;
			commandParameters.Add(param44);
			SqlParameter param45 = new SqlParameter("@DepartmentName", SqlDbType.NVarChar);
			param45.Value = DepartmentName;
			commandParameters.Add(param45);
			SqlParameter param46 = new SqlParameter("@OrganizationType", SqlDbType.SmallInt);
			param46.Value = OrganizationType;
			commandParameters.Add(param46);
			SqlParameter param47 = new SqlParameter("@OrganizationName", SqlDbType.NVarChar);
			param47.Value = OrganizationName;
			commandParameters.Add(param47);
			SqlParameter param48 = new SqlParameter("@EsFacturableGlobal", SqlDbType.Bit);
			param48.Value = EsFacturableGlobal;
			commandParameters.Add(param48);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdPagosCab";
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

        public void ActualizarFormaPago()
        {
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlCommand cmm = new SqlCommand();
            SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
            param0.Value = IdPagosCab;
            commandParameters.Add(param0);
            SqlParameter param40 = new SqlParameter("@BinRange", SqlDbType.Int);
            param40.Value = BinRange;
            commandParameters.Add(param40);
            SqlParameter param41 = new SqlParameter("@IdUpdFormaPago", SqlDbType.Int);
            param41.Value = IdUpdFormaPago;
            commandParameters.Add(param41);
            SqlParameter param42 = new SqlParameter("@UpdFormaPagModificadoPor", SqlDbType.VarChar);
            param42.Value = UpdFormaPagModificadoPor;
            commandParameters.Add(param42);
            SqlParameter param43 = new SqlParameter("@FechaUpdaFormaPag", SqlDbType.VarChar);
            param43.Value = FechaUpdaFormaPag.Year > 1900 ? FechaUpdaFormaPag.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
            commandParameters.Add(param43);
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_UpdFormaPago";
            cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
            cmm.Connection = _conexion;
            bool cerrarConexion = false;
            foreach (SqlParameter p in commandParameters)
            {
                cmm.Parameters.Add(p);
            }
            try
            {
                if (_conexion.State.Equals(ConnectionState.Closed))
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
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmm.Dispose();
                if (cerrarConexion)
                {
                    _conexion.Close();
                }
            }
        }
		#endregion

		#region Eliminar VBFac_Pagos_Cab
		public void Eliminar(long idpagoscab)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = idpagoscab;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelPagosCab";
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

		#region Deshacer VBFac_Pagos_Cab
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdPagosCab);
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
		public List<ENTPagosCab> RecuperarTodo()
		{
			List<ENTPagosCab> result = new List<ENTPagosCab>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetPagosCab_TODO";
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
				    ENTPagosCab item = new ENTPagosCab();
					 if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					 if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					 if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					 if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					 if (!dr.IsNull("FechaPagoUTC")) item.FechaPagoUTC = Convert.ToDateTime(dr["FechaPagoUTC"]);
					 if (!dr.IsNull("IdFormaPago")) item.IdFormaPago = Convert.ToInt32(dr["IdFormaPago"]);
					 if (!dr.IsNull("PaymentMethodCode")) item.PaymentMethodCode = dr["PaymentMethodCode"].ToString();
					 if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					 if (!dr.IsNull("PaymentAmount")) item.PaymentAmount = Convert.ToDecimal(dr["PaymentAmount"]);
					 if (!dr.IsNull("MontoPorAplicar")) item.MontoPorAplicar = Convert.ToDecimal(dr["MontoPorAplicar"]);
					 if (!dr.IsNull("CollectedCurrencyCode")) item.CollectedCurrencyCode = dr["CollectedCurrencyCode"].ToString();
					 if (!dr.IsNull("CollectedAmount")) item.CollectedAmount = Convert.ToDecimal(dr["CollectedAmount"]);
					 if (!dr.IsNull("TipoCambio")) item.TipoCambio = Convert.ToDecimal(dr["TipoCambio"]);
					 if (!dr.IsNull("Estatus")) item.Estatus = Convert.ToInt16(dr["Estatus"]);
					 if (!dr.IsNull("ParentPaymentID")) item.ParentPaymentID = Convert.ToInt64(dr["ParentPaymentID"]);
					 if (!dr.IsNull("EsPagoDividido")) item.EsPagoDividido = Convert.ToBoolean(dr["EsPagoDividido"]);
					 if (!dr.IsNull("EsPagoPadre")) item.EsPagoPadre = Convert.ToBoolean(dr["EsPagoPadre"]);
					 if (!dr.IsNull("EsPagoHijo")) item.EsPagoHijo = Convert.ToBoolean(dr["EsPagoHijo"]);
					 if (!dr.IsNull("EsFacturable")) item.EsFacturable = Convert.ToBoolean(dr["EsFacturable"]);
					 if (!dr.IsNull("EsParaAplicar")) item.EsParaAplicar = Convert.ToBoolean(dr["EsParaAplicar"]);
					 if (!dr.IsNull("EsFacturado")) item.EsFacturado = Convert.ToBoolean(dr["EsFacturado"]);
					 if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					 if (!dr.IsNull("IdFacturaGlobal")) item.IdFacturaGlobal = Convert.ToInt64(dr["IdFacturaGlobal"]);
					 if (!dr.IsNull("FolioPrefactura")) item.FolioPrefactura = Convert.ToInt64(dr["FolioPrefactura"]);
					 if (!dr.IsNull("FolioFactura")) item.FolioFactura = Convert.ToInt64(dr["FolioFactura"]);
					 if (!dr.IsNull("FechaFactura")) item.FechaFactura = Convert.ToDateTime(dr["FechaFactura"]);
					 if (!dr.IsNull("FechaFacturaUTC")) item.FechaFacturaUTC = Convert.ToDateTime(dr["FechaFacturaUTC"]);
					 if (!dr.IsNull("VersionFacturacion")) item.VersionFacturacion = dr["VersionFacturacion"].ToString();
					 if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					 if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					 if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					 if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					 if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					 if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
					 if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt64(dr["IdAgente"]);
					 if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					 if (!dr.IsNull("FechaUltimaActualizacion")) item.FechaUltimaActualizacion = Convert.ToDateTime(dr["FechaUltimaActualizacion"]);
					 if (!dr.IsNull("IdNotaCreditoCab")) item.IdNotaCreditoCab = Convert.ToInt64(dr["IdNotaCreditoCab"]);
					 if (!dr.IsNull("LocationCode")) item.LocationCode = dr["LocationCode"].ToString();
					 if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					 if (!dr.IsNull("OrganizationCode")) item.OrganizationCode = dr["OrganizationCode"].ToString();
					 if (!dr.IsNull("BinRange")) item.BinRange = Convert.ToInt32(dr["BinRange"]);
					 if (!dr.IsNull("IdUpdFormaPago")) item.IdUpdFormaPago = Convert.ToInt32(dr["IdUpdFormaPago"]);
					 if (!dr.IsNull("UpdFormaPagModificadoPor")) item.UpdFormaPagModificadoPor = dr["UpdFormaPagModificadoPor"].ToString();
					 if (!dr.IsNull("FechaUpdaFormaPag")) item.FechaUpdaFormaPag = Convert.ToDateTime(dr["FechaUpdaFormaPag"]);
					 if (!dr.IsNull("DepartmentCode")) item.DepartmentCode = dr["DepartmentCode"].ToString();
					 if (!dr.IsNull("DepartmentName")) item.DepartmentName = dr["DepartmentName"].ToString();
					 if (!dr.IsNull("OrganizationType")) item.OrganizationType = Convert.ToInt16(dr["OrganizationType"]);
					 if (!dr.IsNull("OrganizationName")) item.OrganizationName = dr["OrganizationName"].ToString();
					 if (!dr.IsNull("EsFacturableGlobal")) item.EsFacturableGlobal = Convert.ToBoolean(dr["EsFacturableGlobal"]);
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

		#region Recuperar RecuperarPagosCabPorLlavePrimaria
		public List<ENTPagosCab> RecuperarPagosCabPorLlavePrimaria(long idpagoscab)
		{
			List<ENTPagosCab> result = new List<ENTPagosCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = idpagoscab;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetPagosCab_POR_PK";
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
				    ENTPagosCab item = new ENTPagosCab();
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					if (!dr.IsNull("FechaPagoUTC")) item.FechaPagoUTC = Convert.ToDateTime(dr["FechaPagoUTC"]);
					if (!dr.IsNull("IdFormaPago")) item.IdFormaPago = Convert.ToInt32(dr["IdFormaPago"]);
					if (!dr.IsNull("PaymentMethodCode")) item.PaymentMethodCode = dr["PaymentMethodCode"].ToString();
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("PaymentAmount")) item.PaymentAmount = Convert.ToDecimal(dr["PaymentAmount"]);
					if (!dr.IsNull("MontoPorAplicar")) item.MontoPorAplicar = Convert.ToDecimal(dr["MontoPorAplicar"]);
					if (!dr.IsNull("CollectedCurrencyCode")) item.CollectedCurrencyCode = dr["CollectedCurrencyCode"].ToString();
					if (!dr.IsNull("CollectedAmount")) item.CollectedAmount = Convert.ToDecimal(dr["CollectedAmount"]);
					if (!dr.IsNull("TipoCambio")) item.TipoCambio = Convert.ToDecimal(dr["TipoCambio"]);
					if (!dr.IsNull("Estatus")) item.Estatus = Convert.ToInt16(dr["Estatus"]);
					if (!dr.IsNull("ParentPaymentID")) item.ParentPaymentID = Convert.ToInt64(dr["ParentPaymentID"]);
					if (!dr.IsNull("EsPagoDividido")) item.EsPagoDividido = Convert.ToBoolean(dr["EsPagoDividido"]);
					if (!dr.IsNull("EsPagoPadre")) item.EsPagoPadre = Convert.ToBoolean(dr["EsPagoPadre"]);
					if (!dr.IsNull("EsPagoHijo")) item.EsPagoHijo = Convert.ToBoolean(dr["EsPagoHijo"]);
					if (!dr.IsNull("EsFacturable")) item.EsFacturable = Convert.ToBoolean(dr["EsFacturable"]);
					if (!dr.IsNull("EsParaAplicar")) item.EsParaAplicar = Convert.ToBoolean(dr["EsParaAplicar"]);
					if (!dr.IsNull("EsFacturado")) item.EsFacturado = Convert.ToBoolean(dr["EsFacturado"]);
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaGlobal")) item.IdFacturaGlobal = Convert.ToInt64(dr["IdFacturaGlobal"]);
					if (!dr.IsNull("FolioPrefactura")) item.FolioPrefactura = Convert.ToInt64(dr["FolioPrefactura"]);
					if (!dr.IsNull("FolioFactura")) item.FolioFactura = Convert.ToInt64(dr["FolioFactura"]);
					if (!dr.IsNull("FechaFactura")) item.FechaFactura = Convert.ToDateTime(dr["FechaFactura"]);
					if (!dr.IsNull("FechaFacturaUTC")) item.FechaFacturaUTC = Convert.ToDateTime(dr["FechaFacturaUTC"]);
					if (!dr.IsNull("VersionFacturacion")) item.VersionFacturacion = dr["VersionFacturacion"].ToString();
					if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt64(dr["IdAgente"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("FechaUltimaActualizacion")) item.FechaUltimaActualizacion = Convert.ToDateTime(dr["FechaUltimaActualizacion"]);
					if (!dr.IsNull("IdNotaCreditoCab")) item.IdNotaCreditoCab = Convert.ToInt64(dr["IdNotaCreditoCab"]);
					if (!dr.IsNull("LocationCode")) item.LocationCode = dr["LocationCode"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("OrganizationCode")) item.OrganizationCode = dr["OrganizationCode"].ToString();
					if (!dr.IsNull("BinRange")) item.BinRange = Convert.ToInt32(dr["BinRange"]);
					if (!dr.IsNull("IdUpdFormaPago")) item.IdUpdFormaPago = Convert.ToInt32(dr["IdUpdFormaPago"]);
					if (!dr.IsNull("UpdFormaPagModificadoPor")) item.UpdFormaPagModificadoPor = dr["UpdFormaPagModificadoPor"].ToString();
					if (!dr.IsNull("FechaUpdaFormaPag")) item.FechaUpdaFormaPag = Convert.ToDateTime(dr["FechaUpdaFormaPag"]);
					if (!dr.IsNull("DepartmentCode")) item.DepartmentCode = dr["DepartmentCode"].ToString();
					if (!dr.IsNull("DepartmentName")) item.DepartmentName = dr["DepartmentName"].ToString();
					if (!dr.IsNull("OrganizationType")) item.OrganizationType = Convert.ToInt16(dr["OrganizationType"]);
					if (!dr.IsNull("OrganizationName")) item.OrganizationName = dr["OrganizationName"].ToString();
					if (!dr.IsNull("EsFacturableGlobal")) item.EsFacturableGlobal = Convert.ToBoolean(dr["EsFacturableGlobal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("PaymentID")) PaymentID = Convert.ToInt64(dtResultado.Rows[0]["PaymentID"]);
					if (!dtResultado.Rows[0].IsNull("FechaPago")) FechaPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaPago"]);
					if (!dtResultado.Rows[0].IsNull("FechaPagoUTC")) FechaPagoUTC = Convert.ToDateTime(dtResultado.Rows[0]["FechaPagoUTC"]);
					if (!dtResultado.Rows[0].IsNull("IdFormaPago")) IdFormaPago = Convert.ToInt32(dtResultado.Rows[0]["IdFormaPago"]);
					if (!dtResultado.Rows[0].IsNull("PaymentMethodCode")) PaymentMethodCode = dtResultado.Rows[0]["PaymentMethodCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("PaymentAmount")) PaymentAmount = Convert.ToDecimal(dtResultado.Rows[0]["PaymentAmount"]);
					if (!dtResultado.Rows[0].IsNull("MontoPorAplicar")) MontoPorAplicar = Convert.ToDecimal(dtResultado.Rows[0]["MontoPorAplicar"]);
					if (!dtResultado.Rows[0].IsNull("CollectedCurrencyCode")) CollectedCurrencyCode = dtResultado.Rows[0]["CollectedCurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CollectedAmount")) CollectedAmount = Convert.ToDecimal(dtResultado.Rows[0]["CollectedAmount"]);
					if (!dtResultado.Rows[0].IsNull("TipoCambio")) TipoCambio = Convert.ToDecimal(dtResultado.Rows[0]["TipoCambio"]);
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = Convert.ToInt16(dtResultado.Rows[0]["Estatus"]);
					if (!dtResultado.Rows[0].IsNull("ParentPaymentID")) ParentPaymentID = Convert.ToInt64(dtResultado.Rows[0]["ParentPaymentID"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoDividido")) EsPagoDividido = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoDividido"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoPadre")) EsPagoPadre = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoPadre"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoHijo")) EsPagoHijo = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoHijo"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturable")) EsFacturable = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturable"]);
					if (!dtResultado.Rows[0].IsNull("EsParaAplicar")) EsParaAplicar = Convert.ToBoolean(dtResultado.Rows[0]["EsParaAplicar"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturado")) EsFacturado = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturado"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaGlobal")) IdFacturaGlobal = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaGlobal"]);
					if (!dtResultado.Rows[0].IsNull("FolioPrefactura")) FolioPrefactura = Convert.ToInt64(dtResultado.Rows[0]["FolioPrefactura"]);
					if (!dtResultado.Rows[0].IsNull("FolioFactura")) FolioFactura = Convert.ToInt64(dtResultado.Rows[0]["FolioFactura"]);
					if (!dtResultado.Rows[0].IsNull("FechaFactura")) FechaFactura = Convert.ToDateTime(dtResultado.Rows[0]["FechaFactura"]);
					if (!dtResultado.Rows[0].IsNull("FechaFacturaUTC")) FechaFacturaUTC = Convert.ToDateTime(dtResultado.Rows[0]["FechaFacturaUTC"]);
					if (!dtResultado.Rows[0].IsNull("VersionFacturacion")) VersionFacturacion = dtResultado.Rows[0]["VersionFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("MontoTarifa")) MontoTarifa = Convert.ToDecimal(dtResultado.Rows[0]["MontoTarifa"]);
					if (!dtResultado.Rows[0].IsNull("MontoServAdic")) MontoServAdic = Convert.ToDecimal(dtResultado.Rows[0]["MontoServAdic"]);
					if (!dtResultado.Rows[0].IsNull("MontoTUA")) MontoTUA = Convert.ToDecimal(dtResultado.Rows[0]["MontoTUA"]);
					if (!dtResultado.Rows[0].IsNull("MontoOtrosCargos")) MontoOtrosCargos = Convert.ToDecimal(dtResultado.Rows[0]["MontoOtrosCargos"]);
					if (!dtResultado.Rows[0].IsNull("MontoIVA")) MontoIVA = Convert.ToDecimal(dtResultado.Rows[0]["MontoIVA"]);
					if (!dtResultado.Rows[0].IsNull("MontoTotal")) MontoTotal = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotal"]);
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt64(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaUltimaActualizacion")) FechaUltimaActualizacion = Convert.ToDateTime(dtResultado.Rows[0]["FechaUltimaActualizacion"]);
					if (!dtResultado.Rows[0].IsNull("IdNotaCreditoCab")) IdNotaCreditoCab = Convert.ToInt64(dtResultado.Rows[0]["IdNotaCreditoCab"]);
					if (!dtResultado.Rows[0].IsNull("LocationCode")) LocationCode = dtResultado.Rows[0]["LocationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrganizationCode")) OrganizationCode = dtResultado.Rows[0]["OrganizationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("BinRange")) BinRange = Convert.ToInt32(dtResultado.Rows[0]["BinRange"]);
					if (!dtResultado.Rows[0].IsNull("IdUpdFormaPago")) IdUpdFormaPago = Convert.ToInt32(dtResultado.Rows[0]["IdUpdFormaPago"]);
					if (!dtResultado.Rows[0].IsNull("UpdFormaPagModificadoPor")) UpdFormaPagModificadoPor = dtResultado.Rows[0]["UpdFormaPagModificadoPor"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaUpdaFormaPag")) FechaUpdaFormaPag = Convert.ToDateTime(dtResultado.Rows[0]["FechaUpdaFormaPag"]);
					if (!dtResultado.Rows[0].IsNull("DepartmentCode")) DepartmentCode = dtResultado.Rows[0]["DepartmentCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("DepartmentName")) DepartmentName = dtResultado.Rows[0]["DepartmentName"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrganizationType")) OrganizationType = Convert.ToInt16(dtResultado.Rows[0]["OrganizationType"]);
					if (!dtResultado.Rows[0].IsNull("OrganizationName")) OrganizationName = dtResultado.Rows[0]["OrganizationName"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsFacturableGlobal")) EsFacturableGlobal = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturableGlobal"]);
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

		#region Recuperar RecuperarPagosCabIdformapago
		public List<ENTPagosCab> RecuperarPagosCabIdformapago(int idformapago)
		{
			List<ENTPagosCab> result = new List<ENTPagosCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFormaPago", SqlDbType.Int);
			param0.Value = idformapago;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetPagosCab_POR_IdFormaPago";
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
				    ENTPagosCab item = new ENTPagosCab();
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					if (!dr.IsNull("FechaPagoUTC")) item.FechaPagoUTC = Convert.ToDateTime(dr["FechaPagoUTC"]);
					if (!dr.IsNull("IdFormaPago")) item.IdFormaPago = Convert.ToInt32(dr["IdFormaPago"]);
					if (!dr.IsNull("PaymentMethodCode")) item.PaymentMethodCode = dr["PaymentMethodCode"].ToString();
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("PaymentAmount")) item.PaymentAmount = Convert.ToDecimal(dr["PaymentAmount"]);
					if (!dr.IsNull("MontoPorAplicar")) item.MontoPorAplicar = Convert.ToDecimal(dr["MontoPorAplicar"]);
					if (!dr.IsNull("CollectedCurrencyCode")) item.CollectedCurrencyCode = dr["CollectedCurrencyCode"].ToString();
					if (!dr.IsNull("CollectedAmount")) item.CollectedAmount = Convert.ToDecimal(dr["CollectedAmount"]);
					if (!dr.IsNull("TipoCambio")) item.TipoCambio = Convert.ToDecimal(dr["TipoCambio"]);
					if (!dr.IsNull("Estatus")) item.Estatus = Convert.ToInt16(dr["Estatus"]);
					if (!dr.IsNull("ParentPaymentID")) item.ParentPaymentID = Convert.ToInt64(dr["ParentPaymentID"]);
					if (!dr.IsNull("EsPagoDividido")) item.EsPagoDividido = Convert.ToBoolean(dr["EsPagoDividido"]);
					if (!dr.IsNull("EsPagoPadre")) item.EsPagoPadre = Convert.ToBoolean(dr["EsPagoPadre"]);
					if (!dr.IsNull("EsPagoHijo")) item.EsPagoHijo = Convert.ToBoolean(dr["EsPagoHijo"]);
					if (!dr.IsNull("EsFacturable")) item.EsFacturable = Convert.ToBoolean(dr["EsFacturable"]);
					if (!dr.IsNull("EsParaAplicar")) item.EsParaAplicar = Convert.ToBoolean(dr["EsParaAplicar"]);
					if (!dr.IsNull("EsFacturado")) item.EsFacturado = Convert.ToBoolean(dr["EsFacturado"]);
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaGlobal")) item.IdFacturaGlobal = Convert.ToInt64(dr["IdFacturaGlobal"]);
					if (!dr.IsNull("FolioPrefactura")) item.FolioPrefactura = Convert.ToInt64(dr["FolioPrefactura"]);
					if (!dr.IsNull("FolioFactura")) item.FolioFactura = Convert.ToInt64(dr["FolioFactura"]);
					if (!dr.IsNull("FechaFactura")) item.FechaFactura = Convert.ToDateTime(dr["FechaFactura"]);
					if (!dr.IsNull("FechaFacturaUTC")) item.FechaFacturaUTC = Convert.ToDateTime(dr["FechaFacturaUTC"]);
					if (!dr.IsNull("VersionFacturacion")) item.VersionFacturacion = dr["VersionFacturacion"].ToString();
					if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt64(dr["IdAgente"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("FechaUltimaActualizacion")) item.FechaUltimaActualizacion = Convert.ToDateTime(dr["FechaUltimaActualizacion"]);
					if (!dr.IsNull("IdNotaCreditoCab")) item.IdNotaCreditoCab = Convert.ToInt64(dr["IdNotaCreditoCab"]);
					if (!dr.IsNull("LocationCode")) item.LocationCode = dr["LocationCode"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("OrganizationCode")) item.OrganizationCode = dr["OrganizationCode"].ToString();
					if (!dr.IsNull("BinRange")) item.BinRange = Convert.ToInt32(dr["BinRange"]);
					if (!dr.IsNull("IdUpdFormaPago")) item.IdUpdFormaPago = Convert.ToInt32(dr["IdUpdFormaPago"]);
					if (!dr.IsNull("UpdFormaPagModificadoPor")) item.UpdFormaPagModificadoPor = dr["UpdFormaPagModificadoPor"].ToString();
					if (!dr.IsNull("FechaUpdaFormaPag")) item.FechaUpdaFormaPag = Convert.ToDateTime(dr["FechaUpdaFormaPag"]);
					if (!dr.IsNull("DepartmentCode")) item.DepartmentCode = dr["DepartmentCode"].ToString();
					if (!dr.IsNull("DepartmentName")) item.DepartmentName = dr["DepartmentName"].ToString();
					if (!dr.IsNull("OrganizationType")) item.OrganizationType = Convert.ToInt16(dr["OrganizationType"]);
					if (!dr.IsNull("OrganizationName")) item.OrganizationName = dr["OrganizationName"].ToString();
					if (!dr.IsNull("EsFacturableGlobal")) item.EsFacturableGlobal = Convert.ToBoolean(dr["EsFacturableGlobal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("PaymentID")) PaymentID = Convert.ToInt64(dtResultado.Rows[0]["PaymentID"]);
					if (!dtResultado.Rows[0].IsNull("FechaPago")) FechaPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaPago"]);
					if (!dtResultado.Rows[0].IsNull("FechaPagoUTC")) FechaPagoUTC = Convert.ToDateTime(dtResultado.Rows[0]["FechaPagoUTC"]);
					if (!dtResultado.Rows[0].IsNull("IdFormaPago")) IdFormaPago = Convert.ToInt32(dtResultado.Rows[0]["IdFormaPago"]);
					if (!dtResultado.Rows[0].IsNull("PaymentMethodCode")) PaymentMethodCode = dtResultado.Rows[0]["PaymentMethodCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("PaymentAmount")) PaymentAmount = Convert.ToDecimal(dtResultado.Rows[0]["PaymentAmount"]);
					if (!dtResultado.Rows[0].IsNull("MontoPorAplicar")) MontoPorAplicar = Convert.ToDecimal(dtResultado.Rows[0]["MontoPorAplicar"]);
					if (!dtResultado.Rows[0].IsNull("CollectedCurrencyCode")) CollectedCurrencyCode = dtResultado.Rows[0]["CollectedCurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CollectedAmount")) CollectedAmount = Convert.ToDecimal(dtResultado.Rows[0]["CollectedAmount"]);
					if (!dtResultado.Rows[0].IsNull("TipoCambio")) TipoCambio = Convert.ToDecimal(dtResultado.Rows[0]["TipoCambio"]);
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = Convert.ToInt16(dtResultado.Rows[0]["Estatus"]);
					if (!dtResultado.Rows[0].IsNull("ParentPaymentID")) ParentPaymentID = Convert.ToInt64(dtResultado.Rows[0]["ParentPaymentID"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoDividido")) EsPagoDividido = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoDividido"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoPadre")) EsPagoPadre = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoPadre"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoHijo")) EsPagoHijo = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoHijo"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturable")) EsFacturable = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturable"]);
					if (!dtResultado.Rows[0].IsNull("EsParaAplicar")) EsParaAplicar = Convert.ToBoolean(dtResultado.Rows[0]["EsParaAplicar"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturado")) EsFacturado = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturado"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaGlobal")) IdFacturaGlobal = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaGlobal"]);
					if (!dtResultado.Rows[0].IsNull("FolioPrefactura")) FolioPrefactura = Convert.ToInt64(dtResultado.Rows[0]["FolioPrefactura"]);
					if (!dtResultado.Rows[0].IsNull("FolioFactura")) FolioFactura = Convert.ToInt64(dtResultado.Rows[0]["FolioFactura"]);
					if (!dtResultado.Rows[0].IsNull("FechaFactura")) FechaFactura = Convert.ToDateTime(dtResultado.Rows[0]["FechaFactura"]);
					if (!dtResultado.Rows[0].IsNull("FechaFacturaUTC")) FechaFacturaUTC = Convert.ToDateTime(dtResultado.Rows[0]["FechaFacturaUTC"]);
					if (!dtResultado.Rows[0].IsNull("VersionFacturacion")) VersionFacturacion = dtResultado.Rows[0]["VersionFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("MontoTarifa")) MontoTarifa = Convert.ToDecimal(dtResultado.Rows[0]["MontoTarifa"]);
					if (!dtResultado.Rows[0].IsNull("MontoServAdic")) MontoServAdic = Convert.ToDecimal(dtResultado.Rows[0]["MontoServAdic"]);
					if (!dtResultado.Rows[0].IsNull("MontoTUA")) MontoTUA = Convert.ToDecimal(dtResultado.Rows[0]["MontoTUA"]);
					if (!dtResultado.Rows[0].IsNull("MontoOtrosCargos")) MontoOtrosCargos = Convert.ToDecimal(dtResultado.Rows[0]["MontoOtrosCargos"]);
					if (!dtResultado.Rows[0].IsNull("MontoIVA")) MontoIVA = Convert.ToDecimal(dtResultado.Rows[0]["MontoIVA"]);
					if (!dtResultado.Rows[0].IsNull("MontoTotal")) MontoTotal = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotal"]);
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt64(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaUltimaActualizacion")) FechaUltimaActualizacion = Convert.ToDateTime(dtResultado.Rows[0]["FechaUltimaActualizacion"]);
					if (!dtResultado.Rows[0].IsNull("IdNotaCreditoCab")) IdNotaCreditoCab = Convert.ToInt64(dtResultado.Rows[0]["IdNotaCreditoCab"]);
					if (!dtResultado.Rows[0].IsNull("LocationCode")) LocationCode = dtResultado.Rows[0]["LocationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrganizationCode")) OrganizationCode = dtResultado.Rows[0]["OrganizationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("BinRange")) BinRange = Convert.ToInt32(dtResultado.Rows[0]["BinRange"]);
					if (!dtResultado.Rows[0].IsNull("IdUpdFormaPago")) IdUpdFormaPago = Convert.ToInt32(dtResultado.Rows[0]["IdUpdFormaPago"]);
					if (!dtResultado.Rows[0].IsNull("UpdFormaPagModificadoPor")) UpdFormaPagModificadoPor = dtResultado.Rows[0]["UpdFormaPagModificadoPor"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaUpdaFormaPag")) FechaUpdaFormaPag = Convert.ToDateTime(dtResultado.Rows[0]["FechaUpdaFormaPag"]);
					if (!dtResultado.Rows[0].IsNull("DepartmentCode")) DepartmentCode = dtResultado.Rows[0]["DepartmentCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("DepartmentName")) DepartmentName = dtResultado.Rows[0]["DepartmentName"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrganizationType")) OrganizationType = Convert.ToInt16(dtResultado.Rows[0]["OrganizationType"]);
					if (!dtResultado.Rows[0].IsNull("OrganizationName")) OrganizationName = dtResultado.Rows[0]["OrganizationName"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsFacturableGlobal")) EsFacturableGlobal = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturableGlobal"]);
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

		#region Recuperar RecuperarPagosCabIdfacturacab
		public List<ENTPagosCab> RecuperarPagosCabIdfacturacab(long idfacturacab)
		{
			List<ENTPagosCab> result = new List<ENTPagosCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = idfacturacab;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetPagosCab_POR_IdFacturaCab";
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
				    ENTPagosCab item = new ENTPagosCab();
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					if (!dr.IsNull("FechaPagoUTC")) item.FechaPagoUTC = Convert.ToDateTime(dr["FechaPagoUTC"]);
					if (!dr.IsNull("IdFormaPago")) item.IdFormaPago = Convert.ToInt32(dr["IdFormaPago"]);
					if (!dr.IsNull("PaymentMethodCode")) item.PaymentMethodCode = dr["PaymentMethodCode"].ToString();
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("PaymentAmount")) item.PaymentAmount = Convert.ToDecimal(dr["PaymentAmount"]);
					if (!dr.IsNull("MontoPorAplicar")) item.MontoPorAplicar = Convert.ToDecimal(dr["MontoPorAplicar"]);
					if (!dr.IsNull("CollectedCurrencyCode")) item.CollectedCurrencyCode = dr["CollectedCurrencyCode"].ToString();
					if (!dr.IsNull("CollectedAmount")) item.CollectedAmount = Convert.ToDecimal(dr["CollectedAmount"]);
					if (!dr.IsNull("TipoCambio")) item.TipoCambio = Convert.ToDecimal(dr["TipoCambio"]);
					if (!dr.IsNull("Estatus")) item.Estatus = Convert.ToInt16(dr["Estatus"]);
					if (!dr.IsNull("ParentPaymentID")) item.ParentPaymentID = Convert.ToInt64(dr["ParentPaymentID"]);
					if (!dr.IsNull("EsPagoDividido")) item.EsPagoDividido = Convert.ToBoolean(dr["EsPagoDividido"]);
					if (!dr.IsNull("EsPagoPadre")) item.EsPagoPadre = Convert.ToBoolean(dr["EsPagoPadre"]);
					if (!dr.IsNull("EsPagoHijo")) item.EsPagoHijo = Convert.ToBoolean(dr["EsPagoHijo"]);
					if (!dr.IsNull("EsFacturable")) item.EsFacturable = Convert.ToBoolean(dr["EsFacturable"]);
					if (!dr.IsNull("EsParaAplicar")) item.EsParaAplicar = Convert.ToBoolean(dr["EsParaAplicar"]);
					if (!dr.IsNull("EsFacturado")) item.EsFacturado = Convert.ToBoolean(dr["EsFacturado"]);
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaGlobal")) item.IdFacturaGlobal = Convert.ToInt64(dr["IdFacturaGlobal"]);
					if (!dr.IsNull("FolioPrefactura")) item.FolioPrefactura = Convert.ToInt64(dr["FolioPrefactura"]);
					if (!dr.IsNull("FolioFactura")) item.FolioFactura = Convert.ToInt64(dr["FolioFactura"]);
					if (!dr.IsNull("FechaFactura")) item.FechaFactura = Convert.ToDateTime(dr["FechaFactura"]);
					if (!dr.IsNull("FechaFacturaUTC")) item.FechaFacturaUTC = Convert.ToDateTime(dr["FechaFacturaUTC"]);
					if (!dr.IsNull("VersionFacturacion")) item.VersionFacturacion = dr["VersionFacturacion"].ToString();
					if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt64(dr["IdAgente"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("FechaUltimaActualizacion")) item.FechaUltimaActualizacion = Convert.ToDateTime(dr["FechaUltimaActualizacion"]);
					if (!dr.IsNull("IdNotaCreditoCab")) item.IdNotaCreditoCab = Convert.ToInt64(dr["IdNotaCreditoCab"]);
					if (!dr.IsNull("LocationCode")) item.LocationCode = dr["LocationCode"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("OrganizationCode")) item.OrganizationCode = dr["OrganizationCode"].ToString();
					if (!dr.IsNull("BinRange")) item.BinRange = Convert.ToInt32(dr["BinRange"]);
					if (!dr.IsNull("IdUpdFormaPago")) item.IdUpdFormaPago = Convert.ToInt32(dr["IdUpdFormaPago"]);
					if (!dr.IsNull("UpdFormaPagModificadoPor")) item.UpdFormaPagModificadoPor = dr["UpdFormaPagModificadoPor"].ToString();
					if (!dr.IsNull("FechaUpdaFormaPag")) item.FechaUpdaFormaPag = Convert.ToDateTime(dr["FechaUpdaFormaPag"]);
					if (!dr.IsNull("DepartmentCode")) item.DepartmentCode = dr["DepartmentCode"].ToString();
					if (!dr.IsNull("DepartmentName")) item.DepartmentName = dr["DepartmentName"].ToString();
					if (!dr.IsNull("OrganizationType")) item.OrganizationType = Convert.ToInt16(dr["OrganizationType"]);
					if (!dr.IsNull("OrganizationName")) item.OrganizationName = dr["OrganizationName"].ToString();
					if (!dr.IsNull("EsFacturableGlobal")) item.EsFacturableGlobal = Convert.ToBoolean(dr["EsFacturableGlobal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("PaymentID")) PaymentID = Convert.ToInt64(dtResultado.Rows[0]["PaymentID"]);
					if (!dtResultado.Rows[0].IsNull("FechaPago")) FechaPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaPago"]);
					if (!dtResultado.Rows[0].IsNull("FechaPagoUTC")) FechaPagoUTC = Convert.ToDateTime(dtResultado.Rows[0]["FechaPagoUTC"]);
					if (!dtResultado.Rows[0].IsNull("IdFormaPago")) IdFormaPago = Convert.ToInt32(dtResultado.Rows[0]["IdFormaPago"]);
					if (!dtResultado.Rows[0].IsNull("PaymentMethodCode")) PaymentMethodCode = dtResultado.Rows[0]["PaymentMethodCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("PaymentAmount")) PaymentAmount = Convert.ToDecimal(dtResultado.Rows[0]["PaymentAmount"]);
					if (!dtResultado.Rows[0].IsNull("MontoPorAplicar")) MontoPorAplicar = Convert.ToDecimal(dtResultado.Rows[0]["MontoPorAplicar"]);
					if (!dtResultado.Rows[0].IsNull("CollectedCurrencyCode")) CollectedCurrencyCode = dtResultado.Rows[0]["CollectedCurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CollectedAmount")) CollectedAmount = Convert.ToDecimal(dtResultado.Rows[0]["CollectedAmount"]);
					if (!dtResultado.Rows[0].IsNull("TipoCambio")) TipoCambio = Convert.ToDecimal(dtResultado.Rows[0]["TipoCambio"]);
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = Convert.ToInt16(dtResultado.Rows[0]["Estatus"]);
					if (!dtResultado.Rows[0].IsNull("ParentPaymentID")) ParentPaymentID = Convert.ToInt64(dtResultado.Rows[0]["ParentPaymentID"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoDividido")) EsPagoDividido = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoDividido"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoPadre")) EsPagoPadre = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoPadre"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoHijo")) EsPagoHijo = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoHijo"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturable")) EsFacturable = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturable"]);
					if (!dtResultado.Rows[0].IsNull("EsParaAplicar")) EsParaAplicar = Convert.ToBoolean(dtResultado.Rows[0]["EsParaAplicar"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturado")) EsFacturado = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturado"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaGlobal")) IdFacturaGlobal = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaGlobal"]);
					if (!dtResultado.Rows[0].IsNull("FolioPrefactura")) FolioPrefactura = Convert.ToInt64(dtResultado.Rows[0]["FolioPrefactura"]);
					if (!dtResultado.Rows[0].IsNull("FolioFactura")) FolioFactura = Convert.ToInt64(dtResultado.Rows[0]["FolioFactura"]);
					if (!dtResultado.Rows[0].IsNull("FechaFactura")) FechaFactura = Convert.ToDateTime(dtResultado.Rows[0]["FechaFactura"]);
					if (!dtResultado.Rows[0].IsNull("FechaFacturaUTC")) FechaFacturaUTC = Convert.ToDateTime(dtResultado.Rows[0]["FechaFacturaUTC"]);
					if (!dtResultado.Rows[0].IsNull("VersionFacturacion")) VersionFacturacion = dtResultado.Rows[0]["VersionFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("MontoTarifa")) MontoTarifa = Convert.ToDecimal(dtResultado.Rows[0]["MontoTarifa"]);
					if (!dtResultado.Rows[0].IsNull("MontoServAdic")) MontoServAdic = Convert.ToDecimal(dtResultado.Rows[0]["MontoServAdic"]);
					if (!dtResultado.Rows[0].IsNull("MontoTUA")) MontoTUA = Convert.ToDecimal(dtResultado.Rows[0]["MontoTUA"]);
					if (!dtResultado.Rows[0].IsNull("MontoOtrosCargos")) MontoOtrosCargos = Convert.ToDecimal(dtResultado.Rows[0]["MontoOtrosCargos"]);
					if (!dtResultado.Rows[0].IsNull("MontoIVA")) MontoIVA = Convert.ToDecimal(dtResultado.Rows[0]["MontoIVA"]);
					if (!dtResultado.Rows[0].IsNull("MontoTotal")) MontoTotal = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotal"]);
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt64(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaUltimaActualizacion")) FechaUltimaActualizacion = Convert.ToDateTime(dtResultado.Rows[0]["FechaUltimaActualizacion"]);
					if (!dtResultado.Rows[0].IsNull("IdNotaCreditoCab")) IdNotaCreditoCab = Convert.ToInt64(dtResultado.Rows[0]["IdNotaCreditoCab"]);
					if (!dtResultado.Rows[0].IsNull("LocationCode")) LocationCode = dtResultado.Rows[0]["LocationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrganizationCode")) OrganizationCode = dtResultado.Rows[0]["OrganizationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("BinRange")) BinRange = Convert.ToInt32(dtResultado.Rows[0]["BinRange"]);
					if (!dtResultado.Rows[0].IsNull("IdUpdFormaPago")) IdUpdFormaPago = Convert.ToInt32(dtResultado.Rows[0]["IdUpdFormaPago"]);
					if (!dtResultado.Rows[0].IsNull("UpdFormaPagModificadoPor")) UpdFormaPagModificadoPor = dtResultado.Rows[0]["UpdFormaPagModificadoPor"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaUpdaFormaPag")) FechaUpdaFormaPag = Convert.ToDateTime(dtResultado.Rows[0]["FechaUpdaFormaPag"]);
					if (!dtResultado.Rows[0].IsNull("DepartmentCode")) DepartmentCode = dtResultado.Rows[0]["DepartmentCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("DepartmentName")) DepartmentName = dtResultado.Rows[0]["DepartmentName"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrganizationType")) OrganizationType = Convert.ToInt16(dtResultado.Rows[0]["OrganizationType"]);
					if (!dtResultado.Rows[0].IsNull("OrganizationName")) OrganizationName = dtResultado.Rows[0]["OrganizationName"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsFacturableGlobal")) EsFacturableGlobal = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturableGlobal"]);
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

		#region Recuperar RecuperarPagosCabBookingid
		public List<ENTPagosCab> RecuperarPagosCabBookingid(long bookingid)
		{
			List<ENTPagosCab> result = new List<ENTPagosCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param0.Value = bookingid;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetPagosCab_POR_BookingID";
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
				    ENTPagosCab item = new ENTPagosCab();
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					if (!dr.IsNull("FechaPagoUTC")) item.FechaPagoUTC = Convert.ToDateTime(dr["FechaPagoUTC"]);
					if (!dr.IsNull("IdFormaPago")) item.IdFormaPago = Convert.ToInt32(dr["IdFormaPago"]);
					if (!dr.IsNull("PaymentMethodCode")) item.PaymentMethodCode = dr["PaymentMethodCode"].ToString();
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("PaymentAmount")) item.PaymentAmount = Convert.ToDecimal(dr["PaymentAmount"]);
					if (!dr.IsNull("MontoPorAplicar")) item.MontoPorAplicar = Convert.ToDecimal(dr["MontoPorAplicar"]);
					if (!dr.IsNull("CollectedCurrencyCode")) item.CollectedCurrencyCode = dr["CollectedCurrencyCode"].ToString();
					if (!dr.IsNull("CollectedAmount")) item.CollectedAmount = Convert.ToDecimal(dr["CollectedAmount"]);
					if (!dr.IsNull("TipoCambio")) item.TipoCambio = Convert.ToDecimal(dr["TipoCambio"]);
					if (!dr.IsNull("Estatus")) item.Estatus = Convert.ToInt16(dr["Estatus"]);
					if (!dr.IsNull("ParentPaymentID")) item.ParentPaymentID = Convert.ToInt64(dr["ParentPaymentID"]);
					if (!dr.IsNull("EsPagoDividido")) item.EsPagoDividido = Convert.ToBoolean(dr["EsPagoDividido"]);
					if (!dr.IsNull("EsPagoPadre")) item.EsPagoPadre = Convert.ToBoolean(dr["EsPagoPadre"]);
					if (!dr.IsNull("EsPagoHijo")) item.EsPagoHijo = Convert.ToBoolean(dr["EsPagoHijo"]);
					if (!dr.IsNull("EsFacturable")) item.EsFacturable = Convert.ToBoolean(dr["EsFacturable"]);
					if (!dr.IsNull("EsParaAplicar")) item.EsParaAplicar = Convert.ToBoolean(dr["EsParaAplicar"]);
					if (!dr.IsNull("EsFacturado")) item.EsFacturado = Convert.ToBoolean(dr["EsFacturado"]);
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaGlobal")) item.IdFacturaGlobal = Convert.ToInt64(dr["IdFacturaGlobal"]);
					if (!dr.IsNull("FolioPrefactura")) item.FolioPrefactura = Convert.ToInt64(dr["FolioPrefactura"]);
					if (!dr.IsNull("FolioFactura")) item.FolioFactura = Convert.ToInt64(dr["FolioFactura"]);
					if (!dr.IsNull("FechaFactura")) item.FechaFactura = Convert.ToDateTime(dr["FechaFactura"]);
					if (!dr.IsNull("FechaFacturaUTC")) item.FechaFacturaUTC = Convert.ToDateTime(dr["FechaFacturaUTC"]);
					if (!dr.IsNull("VersionFacturacion")) item.VersionFacturacion = dr["VersionFacturacion"].ToString();
					if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt64(dr["IdAgente"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("FechaUltimaActualizacion")) item.FechaUltimaActualizacion = Convert.ToDateTime(dr["FechaUltimaActualizacion"]);
					if (!dr.IsNull("IdNotaCreditoCab")) item.IdNotaCreditoCab = Convert.ToInt64(dr["IdNotaCreditoCab"]);
					if (!dr.IsNull("LocationCode")) item.LocationCode = dr["LocationCode"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("OrganizationCode")) item.OrganizationCode = dr["OrganizationCode"].ToString();
					if (!dr.IsNull("BinRange")) item.BinRange = Convert.ToInt32(dr["BinRange"]);
					if (!dr.IsNull("IdUpdFormaPago")) item.IdUpdFormaPago = Convert.ToInt32(dr["IdUpdFormaPago"]);
					if (!dr.IsNull("UpdFormaPagModificadoPor")) item.UpdFormaPagModificadoPor = dr["UpdFormaPagModificadoPor"].ToString();
					if (!dr.IsNull("FechaUpdaFormaPag")) item.FechaUpdaFormaPag = Convert.ToDateTime(dr["FechaUpdaFormaPag"]);
					if (!dr.IsNull("DepartmentCode")) item.DepartmentCode = dr["DepartmentCode"].ToString();
					if (!dr.IsNull("DepartmentName")) item.DepartmentName = dr["DepartmentName"].ToString();
					if (!dr.IsNull("OrganizationType")) item.OrganizationType = Convert.ToInt16(dr["OrganizationType"]);
					if (!dr.IsNull("OrganizationName")) item.OrganizationName = dr["OrganizationName"].ToString();
					if (!dr.IsNull("EsFacturableGlobal")) item.EsFacturableGlobal = Convert.ToBoolean(dr["EsFacturableGlobal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("PaymentID")) PaymentID = Convert.ToInt64(dtResultado.Rows[0]["PaymentID"]);
					if (!dtResultado.Rows[0].IsNull("FechaPago")) FechaPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaPago"]);
					if (!dtResultado.Rows[0].IsNull("FechaPagoUTC")) FechaPagoUTC = Convert.ToDateTime(dtResultado.Rows[0]["FechaPagoUTC"]);
					if (!dtResultado.Rows[0].IsNull("IdFormaPago")) IdFormaPago = Convert.ToInt32(dtResultado.Rows[0]["IdFormaPago"]);
					if (!dtResultado.Rows[0].IsNull("PaymentMethodCode")) PaymentMethodCode = dtResultado.Rows[0]["PaymentMethodCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("PaymentAmount")) PaymentAmount = Convert.ToDecimal(dtResultado.Rows[0]["PaymentAmount"]);
					if (!dtResultado.Rows[0].IsNull("MontoPorAplicar")) MontoPorAplicar = Convert.ToDecimal(dtResultado.Rows[0]["MontoPorAplicar"]);
					if (!dtResultado.Rows[0].IsNull("CollectedCurrencyCode")) CollectedCurrencyCode = dtResultado.Rows[0]["CollectedCurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CollectedAmount")) CollectedAmount = Convert.ToDecimal(dtResultado.Rows[0]["CollectedAmount"]);
					if (!dtResultado.Rows[0].IsNull("TipoCambio")) TipoCambio = Convert.ToDecimal(dtResultado.Rows[0]["TipoCambio"]);
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = Convert.ToInt16(dtResultado.Rows[0]["Estatus"]);
					if (!dtResultado.Rows[0].IsNull("ParentPaymentID")) ParentPaymentID = Convert.ToInt64(dtResultado.Rows[0]["ParentPaymentID"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoDividido")) EsPagoDividido = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoDividido"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoPadre")) EsPagoPadre = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoPadre"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoHijo")) EsPagoHijo = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoHijo"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturable")) EsFacturable = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturable"]);
					if (!dtResultado.Rows[0].IsNull("EsParaAplicar")) EsParaAplicar = Convert.ToBoolean(dtResultado.Rows[0]["EsParaAplicar"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturado")) EsFacturado = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturado"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaGlobal")) IdFacturaGlobal = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaGlobal"]);
					if (!dtResultado.Rows[0].IsNull("FolioPrefactura")) FolioPrefactura = Convert.ToInt64(dtResultado.Rows[0]["FolioPrefactura"]);
					if (!dtResultado.Rows[0].IsNull("FolioFactura")) FolioFactura = Convert.ToInt64(dtResultado.Rows[0]["FolioFactura"]);
					if (!dtResultado.Rows[0].IsNull("FechaFactura")) FechaFactura = Convert.ToDateTime(dtResultado.Rows[0]["FechaFactura"]);
					if (!dtResultado.Rows[0].IsNull("FechaFacturaUTC")) FechaFacturaUTC = Convert.ToDateTime(dtResultado.Rows[0]["FechaFacturaUTC"]);
					if (!dtResultado.Rows[0].IsNull("VersionFacturacion")) VersionFacturacion = dtResultado.Rows[0]["VersionFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("MontoTarifa")) MontoTarifa = Convert.ToDecimal(dtResultado.Rows[0]["MontoTarifa"]);
					if (!dtResultado.Rows[0].IsNull("MontoServAdic")) MontoServAdic = Convert.ToDecimal(dtResultado.Rows[0]["MontoServAdic"]);
					if (!dtResultado.Rows[0].IsNull("MontoTUA")) MontoTUA = Convert.ToDecimal(dtResultado.Rows[0]["MontoTUA"]);
					if (!dtResultado.Rows[0].IsNull("MontoOtrosCargos")) MontoOtrosCargos = Convert.ToDecimal(dtResultado.Rows[0]["MontoOtrosCargos"]);
					if (!dtResultado.Rows[0].IsNull("MontoIVA")) MontoIVA = Convert.ToDecimal(dtResultado.Rows[0]["MontoIVA"]);
					if (!dtResultado.Rows[0].IsNull("MontoTotal")) MontoTotal = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotal"]);
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt64(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaUltimaActualizacion")) FechaUltimaActualizacion = Convert.ToDateTime(dtResultado.Rows[0]["FechaUltimaActualizacion"]);
					if (!dtResultado.Rows[0].IsNull("IdNotaCreditoCab")) IdNotaCreditoCab = Convert.ToInt64(dtResultado.Rows[0]["IdNotaCreditoCab"]);
					if (!dtResultado.Rows[0].IsNull("LocationCode")) LocationCode = dtResultado.Rows[0]["LocationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrganizationCode")) OrganizationCode = dtResultado.Rows[0]["OrganizationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("BinRange")) BinRange = Convert.ToInt32(dtResultado.Rows[0]["BinRange"]);
					if (!dtResultado.Rows[0].IsNull("IdUpdFormaPago")) IdUpdFormaPago = Convert.ToInt32(dtResultado.Rows[0]["IdUpdFormaPago"]);
					if (!dtResultado.Rows[0].IsNull("UpdFormaPagModificadoPor")) UpdFormaPagModificadoPor = dtResultado.Rows[0]["UpdFormaPagModificadoPor"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaUpdaFormaPag")) FechaUpdaFormaPag = Convert.ToDateTime(dtResultado.Rows[0]["FechaUpdaFormaPag"]);
					if (!dtResultado.Rows[0].IsNull("DepartmentCode")) DepartmentCode = dtResultado.Rows[0]["DepartmentCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("DepartmentName")) DepartmentName = dtResultado.Rows[0]["DepartmentName"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrganizationType")) OrganizationType = Convert.ToInt16(dtResultado.Rows[0]["OrganizationType"]);
					if (!dtResultado.Rows[0].IsNull("OrganizationName")) OrganizationName = dtResultado.Rows[0]["OrganizationName"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsFacturableGlobal")) EsFacturableGlobal = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturableGlobal"]);
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

		#region Recuperar RecuperarPagosCabPaymentid
		public List<ENTPagosCab> RecuperarPagosCabPaymentid(long paymentid)
		{
			List<ENTPagosCab> result = new List<ENTPagosCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@PaymentID", SqlDbType.BigInt);
			param0.Value = paymentid;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetPagosCab_POR_PaymentID";
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
				    ENTPagosCab item = new ENTPagosCab();
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					if (!dr.IsNull("FechaPagoUTC")) item.FechaPagoUTC = Convert.ToDateTime(dr["FechaPagoUTC"]);
					if (!dr.IsNull("IdFormaPago")) item.IdFormaPago = Convert.ToInt32(dr["IdFormaPago"]);
					if (!dr.IsNull("PaymentMethodCode")) item.PaymentMethodCode = dr["PaymentMethodCode"].ToString();
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("PaymentAmount")) item.PaymentAmount = Convert.ToDecimal(dr["PaymentAmount"]);
					if (!dr.IsNull("MontoPorAplicar")) item.MontoPorAplicar = Convert.ToDecimal(dr["MontoPorAplicar"]);
					if (!dr.IsNull("CollectedCurrencyCode")) item.CollectedCurrencyCode = dr["CollectedCurrencyCode"].ToString();
					if (!dr.IsNull("CollectedAmount")) item.CollectedAmount = Convert.ToDecimal(dr["CollectedAmount"]);
					if (!dr.IsNull("TipoCambio")) item.TipoCambio = Convert.ToDecimal(dr["TipoCambio"]);
					if (!dr.IsNull("Estatus")) item.Estatus = Convert.ToInt16(dr["Estatus"]);
					if (!dr.IsNull("ParentPaymentID")) item.ParentPaymentID = Convert.ToInt64(dr["ParentPaymentID"]);
					if (!dr.IsNull("EsPagoDividido")) item.EsPagoDividido = Convert.ToBoolean(dr["EsPagoDividido"]);
					if (!dr.IsNull("EsPagoPadre")) item.EsPagoPadre = Convert.ToBoolean(dr["EsPagoPadre"]);
					if (!dr.IsNull("EsPagoHijo")) item.EsPagoHijo = Convert.ToBoolean(dr["EsPagoHijo"]);
					if (!dr.IsNull("EsFacturable")) item.EsFacturable = Convert.ToBoolean(dr["EsFacturable"]);
					if (!dr.IsNull("EsParaAplicar")) item.EsParaAplicar = Convert.ToBoolean(dr["EsParaAplicar"]);
					if (!dr.IsNull("EsFacturado")) item.EsFacturado = Convert.ToBoolean(dr["EsFacturado"]);
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaGlobal")) item.IdFacturaGlobal = Convert.ToInt64(dr["IdFacturaGlobal"]);
					if (!dr.IsNull("FolioPrefactura")) item.FolioPrefactura = Convert.ToInt64(dr["FolioPrefactura"]);
					if (!dr.IsNull("FolioFactura")) item.FolioFactura = Convert.ToInt64(dr["FolioFactura"]);
					if (!dr.IsNull("FechaFactura")) item.FechaFactura = Convert.ToDateTime(dr["FechaFactura"]);
					if (!dr.IsNull("FechaFacturaUTC")) item.FechaFacturaUTC = Convert.ToDateTime(dr["FechaFacturaUTC"]);
					if (!dr.IsNull("VersionFacturacion")) item.VersionFacturacion = dr["VersionFacturacion"].ToString();
					if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt64(dr["IdAgente"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("FechaUltimaActualizacion")) item.FechaUltimaActualizacion = Convert.ToDateTime(dr["FechaUltimaActualizacion"]);
					if (!dr.IsNull("IdNotaCreditoCab")) item.IdNotaCreditoCab = Convert.ToInt64(dr["IdNotaCreditoCab"]);
					if (!dr.IsNull("LocationCode")) item.LocationCode = dr["LocationCode"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("OrganizationCode")) item.OrganizationCode = dr["OrganizationCode"].ToString();
					if (!dr.IsNull("BinRange")) item.BinRange = Convert.ToInt32(dr["BinRange"]);
					if (!dr.IsNull("IdUpdFormaPago")) item.IdUpdFormaPago = Convert.ToInt32(dr["IdUpdFormaPago"]);
					if (!dr.IsNull("UpdFormaPagModificadoPor")) item.UpdFormaPagModificadoPor = dr["UpdFormaPagModificadoPor"].ToString();
					if (!dr.IsNull("FechaUpdaFormaPag")) item.FechaUpdaFormaPag = Convert.ToDateTime(dr["FechaUpdaFormaPag"]);
					if (!dr.IsNull("DepartmentCode")) item.DepartmentCode = dr["DepartmentCode"].ToString();
					if (!dr.IsNull("DepartmentName")) item.DepartmentName = dr["DepartmentName"].ToString();
					if (!dr.IsNull("OrganizationType")) item.OrganizationType = Convert.ToInt16(dr["OrganizationType"]);
					if (!dr.IsNull("OrganizationName")) item.OrganizationName = dr["OrganizationName"].ToString();
					if (!dr.IsNull("EsFacturableGlobal")) item.EsFacturableGlobal = Convert.ToBoolean(dr["EsFacturableGlobal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("PaymentID")) PaymentID = Convert.ToInt64(dtResultado.Rows[0]["PaymentID"]);
					if (!dtResultado.Rows[0].IsNull("FechaPago")) FechaPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaPago"]);
					if (!dtResultado.Rows[0].IsNull("FechaPagoUTC")) FechaPagoUTC = Convert.ToDateTime(dtResultado.Rows[0]["FechaPagoUTC"]);
					if (!dtResultado.Rows[0].IsNull("IdFormaPago")) IdFormaPago = Convert.ToInt32(dtResultado.Rows[0]["IdFormaPago"]);
					if (!dtResultado.Rows[0].IsNull("PaymentMethodCode")) PaymentMethodCode = dtResultado.Rows[0]["PaymentMethodCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("PaymentAmount")) PaymentAmount = Convert.ToDecimal(dtResultado.Rows[0]["PaymentAmount"]);
					if (!dtResultado.Rows[0].IsNull("MontoPorAplicar")) MontoPorAplicar = Convert.ToDecimal(dtResultado.Rows[0]["MontoPorAplicar"]);
					if (!dtResultado.Rows[0].IsNull("CollectedCurrencyCode")) CollectedCurrencyCode = dtResultado.Rows[0]["CollectedCurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CollectedAmount")) CollectedAmount = Convert.ToDecimal(dtResultado.Rows[0]["CollectedAmount"]);
					if (!dtResultado.Rows[0].IsNull("TipoCambio")) TipoCambio = Convert.ToDecimal(dtResultado.Rows[0]["TipoCambio"]);
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = Convert.ToInt16(dtResultado.Rows[0]["Estatus"]);
					if (!dtResultado.Rows[0].IsNull("ParentPaymentID")) ParentPaymentID = Convert.ToInt64(dtResultado.Rows[0]["ParentPaymentID"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoDividido")) EsPagoDividido = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoDividido"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoPadre")) EsPagoPadre = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoPadre"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoHijo")) EsPagoHijo = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoHijo"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturable")) EsFacturable = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturable"]);
					if (!dtResultado.Rows[0].IsNull("EsParaAplicar")) EsParaAplicar = Convert.ToBoolean(dtResultado.Rows[0]["EsParaAplicar"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturado")) EsFacturado = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturado"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaGlobal")) IdFacturaGlobal = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaGlobal"]);
					if (!dtResultado.Rows[0].IsNull("FolioPrefactura")) FolioPrefactura = Convert.ToInt64(dtResultado.Rows[0]["FolioPrefactura"]);
					if (!dtResultado.Rows[0].IsNull("FolioFactura")) FolioFactura = Convert.ToInt64(dtResultado.Rows[0]["FolioFactura"]);
					if (!dtResultado.Rows[0].IsNull("FechaFactura")) FechaFactura = Convert.ToDateTime(dtResultado.Rows[0]["FechaFactura"]);
					if (!dtResultado.Rows[0].IsNull("FechaFacturaUTC")) FechaFacturaUTC = Convert.ToDateTime(dtResultado.Rows[0]["FechaFacturaUTC"]);
					if (!dtResultado.Rows[0].IsNull("VersionFacturacion")) VersionFacturacion = dtResultado.Rows[0]["VersionFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("MontoTarifa")) MontoTarifa = Convert.ToDecimal(dtResultado.Rows[0]["MontoTarifa"]);
					if (!dtResultado.Rows[0].IsNull("MontoServAdic")) MontoServAdic = Convert.ToDecimal(dtResultado.Rows[0]["MontoServAdic"]);
					if (!dtResultado.Rows[0].IsNull("MontoTUA")) MontoTUA = Convert.ToDecimal(dtResultado.Rows[0]["MontoTUA"]);
					if (!dtResultado.Rows[0].IsNull("MontoOtrosCargos")) MontoOtrosCargos = Convert.ToDecimal(dtResultado.Rows[0]["MontoOtrosCargos"]);
					if (!dtResultado.Rows[0].IsNull("MontoIVA")) MontoIVA = Convert.ToDecimal(dtResultado.Rows[0]["MontoIVA"]);
					if (!dtResultado.Rows[0].IsNull("MontoTotal")) MontoTotal = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotal"]);
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt64(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaUltimaActualizacion")) FechaUltimaActualizacion = Convert.ToDateTime(dtResultado.Rows[0]["FechaUltimaActualizacion"]);
					if (!dtResultado.Rows[0].IsNull("IdNotaCreditoCab")) IdNotaCreditoCab = Convert.ToInt64(dtResultado.Rows[0]["IdNotaCreditoCab"]);
					if (!dtResultado.Rows[0].IsNull("LocationCode")) LocationCode = dtResultado.Rows[0]["LocationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrganizationCode")) OrganizationCode = dtResultado.Rows[0]["OrganizationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("BinRange")) BinRange = Convert.ToInt32(dtResultado.Rows[0]["BinRange"]);
					if (!dtResultado.Rows[0].IsNull("IdUpdFormaPago")) IdUpdFormaPago = Convert.ToInt32(dtResultado.Rows[0]["IdUpdFormaPago"]);
					if (!dtResultado.Rows[0].IsNull("UpdFormaPagModificadoPor")) UpdFormaPagModificadoPor = dtResultado.Rows[0]["UpdFormaPagModificadoPor"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaUpdaFormaPag")) FechaUpdaFormaPag = Convert.ToDateTime(dtResultado.Rows[0]["FechaUpdaFormaPag"]);
					if (!dtResultado.Rows[0].IsNull("DepartmentCode")) DepartmentCode = dtResultado.Rows[0]["DepartmentCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("DepartmentName")) DepartmentName = dtResultado.Rows[0]["DepartmentName"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrganizationType")) OrganizationType = Convert.ToInt16(dtResultado.Rows[0]["OrganizationType"]);
					if (!dtResultado.Rows[0].IsNull("OrganizationName")) OrganizationName = dtResultado.Rows[0]["OrganizationName"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsFacturableGlobal")) EsFacturableGlobal = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturableGlobal"]);
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

		#region Recuperar RecuperarPagosCabFolioprefactura
		public List<ENTPagosCab> RecuperarPagosCabFolioprefactura(long folioprefactura)
		{
			List<ENTPagosCab> result = new List<ENTPagosCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@FolioPrefactura", SqlDbType.BigInt);
			param0.Value = folioprefactura;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetPagosCab_POR_FolioPrefactura";
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
				    ENTPagosCab item = new ENTPagosCab();
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					if (!dr.IsNull("FechaPagoUTC")) item.FechaPagoUTC = Convert.ToDateTime(dr["FechaPagoUTC"]);
					if (!dr.IsNull("IdFormaPago")) item.IdFormaPago = Convert.ToInt32(dr["IdFormaPago"]);
					if (!dr.IsNull("PaymentMethodCode")) item.PaymentMethodCode = dr["PaymentMethodCode"].ToString();
					if (!dr.IsNull("CurrencyCode")) item.CurrencyCode = dr["CurrencyCode"].ToString();
					if (!dr.IsNull("PaymentAmount")) item.PaymentAmount = Convert.ToDecimal(dr["PaymentAmount"]);
					if (!dr.IsNull("MontoPorAplicar")) item.MontoPorAplicar = Convert.ToDecimal(dr["MontoPorAplicar"]);
					if (!dr.IsNull("CollectedCurrencyCode")) item.CollectedCurrencyCode = dr["CollectedCurrencyCode"].ToString();
					if (!dr.IsNull("CollectedAmount")) item.CollectedAmount = Convert.ToDecimal(dr["CollectedAmount"]);
					if (!dr.IsNull("TipoCambio")) item.TipoCambio = Convert.ToDecimal(dr["TipoCambio"]);
					if (!dr.IsNull("Estatus")) item.Estatus = Convert.ToInt16(dr["Estatus"]);
					if (!dr.IsNull("ParentPaymentID")) item.ParentPaymentID = Convert.ToInt64(dr["ParentPaymentID"]);
					if (!dr.IsNull("EsPagoDividido")) item.EsPagoDividido = Convert.ToBoolean(dr["EsPagoDividido"]);
					if (!dr.IsNull("EsPagoPadre")) item.EsPagoPadre = Convert.ToBoolean(dr["EsPagoPadre"]);
					if (!dr.IsNull("EsPagoHijo")) item.EsPagoHijo = Convert.ToBoolean(dr["EsPagoHijo"]);
					if (!dr.IsNull("EsFacturable")) item.EsFacturable = Convert.ToBoolean(dr["EsFacturable"]);
					if (!dr.IsNull("EsParaAplicar")) item.EsParaAplicar = Convert.ToBoolean(dr["EsParaAplicar"]);
					if (!dr.IsNull("EsFacturado")) item.EsFacturado = Convert.ToBoolean(dr["EsFacturado"]);
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaGlobal")) item.IdFacturaGlobal = Convert.ToInt64(dr["IdFacturaGlobal"]);
					if (!dr.IsNull("FolioPrefactura")) item.FolioPrefactura = Convert.ToInt64(dr["FolioPrefactura"]);
					if (!dr.IsNull("FolioFactura")) item.FolioFactura = Convert.ToInt64(dr["FolioFactura"]);
					if (!dr.IsNull("FechaFactura")) item.FechaFactura = Convert.ToDateTime(dr["FechaFactura"]);
					if (!dr.IsNull("FechaFacturaUTC")) item.FechaFacturaUTC = Convert.ToDateTime(dr["FechaFacturaUTC"]);
					if (!dr.IsNull("VersionFacturacion")) item.VersionFacturacion = dr["VersionFacturacion"].ToString();
					if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					if (!dr.IsNull("MontoTotal")) item.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt64(dr["IdAgente"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("FechaUltimaActualizacion")) item.FechaUltimaActualizacion = Convert.ToDateTime(dr["FechaUltimaActualizacion"]);
					if (!dr.IsNull("IdNotaCreditoCab")) item.IdNotaCreditoCab = Convert.ToInt64(dr["IdNotaCreditoCab"]);
					if (!dr.IsNull("LocationCode")) item.LocationCode = dr["LocationCode"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("OrganizationCode")) item.OrganizationCode = dr["OrganizationCode"].ToString();
					if (!dr.IsNull("BinRange")) item.BinRange = Convert.ToInt32(dr["BinRange"]);
					if (!dr.IsNull("IdUpdFormaPago")) item.IdUpdFormaPago = Convert.ToInt32(dr["IdUpdFormaPago"]);
					if (!dr.IsNull("UpdFormaPagModificadoPor")) item.UpdFormaPagModificadoPor = dr["UpdFormaPagModificadoPor"].ToString();
					if (!dr.IsNull("FechaUpdaFormaPag")) item.FechaUpdaFormaPag = Convert.ToDateTime(dr["FechaUpdaFormaPag"]);
					if (!dr.IsNull("DepartmentCode")) item.DepartmentCode = dr["DepartmentCode"].ToString();
					if (!dr.IsNull("DepartmentName")) item.DepartmentName = dr["DepartmentName"].ToString();
					if (!dr.IsNull("OrganizationType")) item.OrganizationType = Convert.ToInt16(dr["OrganizationType"]);
					if (!dr.IsNull("OrganizationName")) item.OrganizationName = dr["OrganizationName"].ToString();
					if (!dr.IsNull("EsFacturableGlobal")) item.EsFacturableGlobal = Convert.ToBoolean(dr["EsFacturableGlobal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("PaymentID")) PaymentID = Convert.ToInt64(dtResultado.Rows[0]["PaymentID"]);
					if (!dtResultado.Rows[0].IsNull("FechaPago")) FechaPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaPago"]);
					if (!dtResultado.Rows[0].IsNull("FechaPagoUTC")) FechaPagoUTC = Convert.ToDateTime(dtResultado.Rows[0]["FechaPagoUTC"]);
					if (!dtResultado.Rows[0].IsNull("IdFormaPago")) IdFormaPago = Convert.ToInt32(dtResultado.Rows[0]["IdFormaPago"]);
					if (!dtResultado.Rows[0].IsNull("PaymentMethodCode")) PaymentMethodCode = dtResultado.Rows[0]["PaymentMethodCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CurrencyCode")) CurrencyCode = dtResultado.Rows[0]["CurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("PaymentAmount")) PaymentAmount = Convert.ToDecimal(dtResultado.Rows[0]["PaymentAmount"]);
					if (!dtResultado.Rows[0].IsNull("MontoPorAplicar")) MontoPorAplicar = Convert.ToDecimal(dtResultado.Rows[0]["MontoPorAplicar"]);
					if (!dtResultado.Rows[0].IsNull("CollectedCurrencyCode")) CollectedCurrencyCode = dtResultado.Rows[0]["CollectedCurrencyCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("CollectedAmount")) CollectedAmount = Convert.ToDecimal(dtResultado.Rows[0]["CollectedAmount"]);
					if (!dtResultado.Rows[0].IsNull("TipoCambio")) TipoCambio = Convert.ToDecimal(dtResultado.Rows[0]["TipoCambio"]);
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = Convert.ToInt16(dtResultado.Rows[0]["Estatus"]);
					if (!dtResultado.Rows[0].IsNull("ParentPaymentID")) ParentPaymentID = Convert.ToInt64(dtResultado.Rows[0]["ParentPaymentID"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoDividido")) EsPagoDividido = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoDividido"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoPadre")) EsPagoPadre = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoPadre"]);
					if (!dtResultado.Rows[0].IsNull("EsPagoHijo")) EsPagoHijo = Convert.ToBoolean(dtResultado.Rows[0]["EsPagoHijo"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturable")) EsFacturable = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturable"]);
					if (!dtResultado.Rows[0].IsNull("EsParaAplicar")) EsParaAplicar = Convert.ToBoolean(dtResultado.Rows[0]["EsParaAplicar"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturado")) EsFacturado = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturado"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaGlobal")) IdFacturaGlobal = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaGlobal"]);
					if (!dtResultado.Rows[0].IsNull("FolioPrefactura")) FolioPrefactura = Convert.ToInt64(dtResultado.Rows[0]["FolioPrefactura"]);
					if (!dtResultado.Rows[0].IsNull("FolioFactura")) FolioFactura = Convert.ToInt64(dtResultado.Rows[0]["FolioFactura"]);
					if (!dtResultado.Rows[0].IsNull("FechaFactura")) FechaFactura = Convert.ToDateTime(dtResultado.Rows[0]["FechaFactura"]);
					if (!dtResultado.Rows[0].IsNull("FechaFacturaUTC")) FechaFacturaUTC = Convert.ToDateTime(dtResultado.Rows[0]["FechaFacturaUTC"]);
					if (!dtResultado.Rows[0].IsNull("VersionFacturacion")) VersionFacturacion = dtResultado.Rows[0]["VersionFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("MontoTarifa")) MontoTarifa = Convert.ToDecimal(dtResultado.Rows[0]["MontoTarifa"]);
					if (!dtResultado.Rows[0].IsNull("MontoServAdic")) MontoServAdic = Convert.ToDecimal(dtResultado.Rows[0]["MontoServAdic"]);
					if (!dtResultado.Rows[0].IsNull("MontoTUA")) MontoTUA = Convert.ToDecimal(dtResultado.Rows[0]["MontoTUA"]);
					if (!dtResultado.Rows[0].IsNull("MontoOtrosCargos")) MontoOtrosCargos = Convert.ToDecimal(dtResultado.Rows[0]["MontoOtrosCargos"]);
					if (!dtResultado.Rows[0].IsNull("MontoIVA")) MontoIVA = Convert.ToDecimal(dtResultado.Rows[0]["MontoIVA"]);
					if (!dtResultado.Rows[0].IsNull("MontoTotal")) MontoTotal = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotal"]);
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt64(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaUltimaActualizacion")) FechaUltimaActualizacion = Convert.ToDateTime(dtResultado.Rows[0]["FechaUltimaActualizacion"]);
					if (!dtResultado.Rows[0].IsNull("IdNotaCreditoCab")) IdNotaCreditoCab = Convert.ToInt64(dtResultado.Rows[0]["IdNotaCreditoCab"]);
					if (!dtResultado.Rows[0].IsNull("LocationCode")) LocationCode = dtResultado.Rows[0]["LocationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrganizationCode")) OrganizationCode = dtResultado.Rows[0]["OrganizationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("BinRange")) BinRange = Convert.ToInt32(dtResultado.Rows[0]["BinRange"]);
					if (!dtResultado.Rows[0].IsNull("IdUpdFormaPago")) IdUpdFormaPago = Convert.ToInt32(dtResultado.Rows[0]["IdUpdFormaPago"]);
					if (!dtResultado.Rows[0].IsNull("UpdFormaPagModificadoPor")) UpdFormaPagModificadoPor = dtResultado.Rows[0]["UpdFormaPagModificadoPor"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaUpdaFormaPag")) FechaUpdaFormaPag = Convert.ToDateTime(dtResultado.Rows[0]["FechaUpdaFormaPag"]);
					if (!dtResultado.Rows[0].IsNull("DepartmentCode")) DepartmentCode = dtResultado.Rows[0]["DepartmentCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("DepartmentName")) DepartmentName = dtResultado.Rows[0]["DepartmentName"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrganizationType")) OrganizationType = Convert.ToInt16(dtResultado.Rows[0]["OrganizationType"]);
					if (!dtResultado.Rows[0].IsNull("OrganizationName")) OrganizationName = dtResultado.Rows[0]["OrganizationName"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsFacturableGlobal")) EsFacturableGlobal = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturableGlobal"]);
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
			_copia = new DALPagosCab(_conexion);
			Type tipo = typeof(ENTPagosCab);
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

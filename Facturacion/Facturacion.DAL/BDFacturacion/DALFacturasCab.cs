using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALFacturasCab: ENTFacturasCab
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALFacturasCab _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALFacturasCab(SqlConnection conexion)
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
			IdEmpresa = 0;
			BookingID = 0;
			FechaHoraExpedicion = new DateTime();
			TipoFacturacion = String.Empty;
			Version = String.Empty;
			Serie = String.Empty;
			FolioFactura = 0;
			UUID = String.Empty;
			TransactionID = String.Empty;
			IdPeticionPAC = 0;
			Estatus = String.Empty;
			RfcEmisor = String.Empty;
			RazonSocialEmisor = String.Empty;
			NoCertificado = String.Empty;
			IdRegimenFiscal = String.Empty;
			RfcReceptor = String.Empty;
			RazonSocialReceptor = String.Empty;
			EmailReceptor = String.Empty;
			EsExtranjero = false;
			IdPaisResidenciaFiscal = String.Empty;
			NumRegIdTrib = String.Empty;
			UsoCFDI = String.Empty;
			FormaPago = String.Empty;
			MetodoPago = String.Empty;
			TipoComprobante = String.Empty;
			LugarExpedicion = String.Empty;
			CondicionesPago = String.Empty;
			Moneda = String.Empty;
			TipoCambio = 0M;
			SubTotal = 0M;
			Descuento = 0M;
			Total = 0M;
			MontoTarifa = 0M;
			MontoServAdic = 0M;
			MontoTUA = 0M;
			MontoOtrosCargos = 0M;
			MontoIVA = 0M;
			IdAgente = 0;
			IdUsuario = 0;
			FechaHoraLocal = new DateTime();
			IdUsuarioCancelo = 0;
			FechaHoraCancelLocal = new DateTime();
			FechaHoraPago = new DateTime();
		}
		#endregion

		#region Agregar VBFac_Facturas_Cab
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = IdFacturaCab;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdEmpresa", SqlDbType.TinyInt);
			param1.Value = IdEmpresa;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param2.Value = BookingID;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@FechaHoraExpedicion", SqlDbType.VarChar);
			param3.Value = FechaHoraExpedicion.Year > 1900 ? FechaHoraExpedicion.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@TipoFacturacion", SqlDbType.VarChar);
			param4.Value = TipoFacturacion;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Version", SqlDbType.VarChar);
			param5.Value = Version;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Serie", SqlDbType.VarChar);
			param6.Value = Serie;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@FolioFactura", SqlDbType.BigInt);
			param7.Value = FolioFactura;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@UUID", SqlDbType.VarChar);
			param8.Value = UUID;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@TransactionID", SqlDbType.VarChar);
			param9.Value = TransactionID;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@IdPeticionPAC", SqlDbType.BigInt);
			param10.Value = IdPeticionPAC;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@Estatus", SqlDbType.VarChar);
			param11.Value = Estatus;
			commandParameters.Add(param11);
			SqlParameter param12 = new SqlParameter("@RfcEmisor", SqlDbType.VarChar);
			param12.Value = RfcEmisor;
			commandParameters.Add(param12);
			SqlParameter param13 = new SqlParameter("@RazonSocialEmisor", SqlDbType.VarChar);
			param13.Value = RazonSocialEmisor;
			commandParameters.Add(param13);
			SqlParameter param14 = new SqlParameter("@NoCertificado", SqlDbType.VarChar);
			param14.Value = NoCertificado;
			commandParameters.Add(param14);
			SqlParameter param15 = new SqlParameter("@IdRegimenFiscal", SqlDbType.VarChar);
			param15.Value = IdRegimenFiscal;
			commandParameters.Add(param15);
			SqlParameter param16 = new SqlParameter("@RfcReceptor", SqlDbType.VarChar);
			param16.Value = RfcReceptor;
			commandParameters.Add(param16);
			SqlParameter param17 = new SqlParameter("@RazonSocialReceptor", SqlDbType.VarChar);
			param17.Value = RazonSocialReceptor;
			commandParameters.Add(param17);
			SqlParameter param18 = new SqlParameter("@EmailReceptor", SqlDbType.VarChar);
			param18.Value = EmailReceptor;
			commandParameters.Add(param18);
			SqlParameter param19 = new SqlParameter("@EsExtranjero", SqlDbType.Bit);
			param19.Value = EsExtranjero;
			commandParameters.Add(param19);
			SqlParameter param20 = new SqlParameter("@IdPaisResidenciaFiscal", SqlDbType.VarChar);
			param20.Value = IdPaisResidenciaFiscal;
			commandParameters.Add(param20);
			SqlParameter param21 = new SqlParameter("@NumRegIdTrib", SqlDbType.VarChar);
			param21.Value = NumRegIdTrib;
			commandParameters.Add(param21);
			SqlParameter param22 = new SqlParameter("@UsoCFDI", SqlDbType.VarChar);
			param22.Value = UsoCFDI;
			commandParameters.Add(param22);
			SqlParameter param23 = new SqlParameter("@FormaPago", SqlDbType.VarChar);
			param23.Value = FormaPago;
			commandParameters.Add(param23);
			SqlParameter param24 = new SqlParameter("@MetodoPago", SqlDbType.VarChar);
			param24.Value = MetodoPago;
			commandParameters.Add(param24);
			SqlParameter param25 = new SqlParameter("@TipoComprobante", SqlDbType.VarChar);
			param25.Value = TipoComprobante;
			commandParameters.Add(param25);
			SqlParameter param26 = new SqlParameter("@LugarExpedicion", SqlDbType.VarChar);
			param26.Value = LugarExpedicion;
			commandParameters.Add(param26);
			SqlParameter param27 = new SqlParameter("@CondicionesPago", SqlDbType.VarChar);
			param27.Value = CondicionesPago;
			commandParameters.Add(param27);
			SqlParameter param28 = new SqlParameter("@Moneda", SqlDbType.VarChar);
			param28.Value = Moneda;
			commandParameters.Add(param28);
			SqlParameter param29 = new SqlParameter("@TipoCambio", SqlDbType.Money);
			param29.Value = TipoCambio;
			commandParameters.Add(param29);
			SqlParameter param30 = new SqlParameter("@SubTotal", SqlDbType.Money);
			param30.Value = SubTotal;
			commandParameters.Add(param30);
			SqlParameter param31 = new SqlParameter("@Descuento", SqlDbType.Money);
			param31.Value = Descuento;
			commandParameters.Add(param31);
			SqlParameter param32 = new SqlParameter("@Total", SqlDbType.Money);
			param32.Value = Total;
			commandParameters.Add(param32);
			SqlParameter param33 = new SqlParameter("@MontoTarifa", SqlDbType.Money);
			param33.Value = MontoTarifa;
			commandParameters.Add(param33);
			SqlParameter param34 = new SqlParameter("@MontoServAdic", SqlDbType.Money);
			param34.Value = MontoServAdic;
			commandParameters.Add(param34);
			SqlParameter param35 = new SqlParameter("@MontoTUA", SqlDbType.Money);
			param35.Value = MontoTUA;
			commandParameters.Add(param35);
			SqlParameter param36 = new SqlParameter("@MontoOtrosCargos", SqlDbType.Money);
			param36.Value = MontoOtrosCargos;
			commandParameters.Add(param36);
			SqlParameter param37 = new SqlParameter("@MontoIVA", SqlDbType.Money);
			param37.Value = MontoIVA;
			commandParameters.Add(param37);
			SqlParameter param38 = new SqlParameter("@IdAgente", SqlDbType.Int);
			param38.Value = IdAgente;
			commandParameters.Add(param38);
			SqlParameter param39 = new SqlParameter("@IdUsuario", SqlDbType.Int);
			param39.Value = IdUsuario;
			commandParameters.Add(param39);
			SqlParameter param40 = new SqlParameter("@IdUsuarioCancelo", SqlDbType.Int);
			param40.Value = IdUsuarioCancelo;
			commandParameters.Add(param40);
			SqlParameter param41 = new SqlParameter("@FechaHoraCancelLocal", SqlDbType.VarChar);
			param41.Value = FechaHoraCancelLocal.Year > 1900 ? FechaHoraCancelLocal.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param41);
			SqlParameter param42 = new SqlParameter("@FechaHoraPago", SqlDbType.VarChar);
			param42.Value = FechaHoraPago.Year > 1900 ? FechaHoraPago.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param42);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsFacturasCab";
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
						IdFacturaCab = Convert.ToInt64(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_Facturas_Cab
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = IdFacturaCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdEmpresa", SqlDbType.TinyInt);
			param1.Value = IdEmpresa;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param2.Value = BookingID;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@FechaHoraExpedicion", SqlDbType.VarChar);
			param3.Value = FechaHoraExpedicion.Year > 1900 ? FechaHoraExpedicion.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@TipoFacturacion", SqlDbType.VarChar);
			param4.Value = TipoFacturacion;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Version", SqlDbType.VarChar);
			param5.Value = Version;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Serie", SqlDbType.VarChar);
			param6.Value = Serie;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@FolioFactura", SqlDbType.BigInt);
			param7.Value = FolioFactura;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@UUID", SqlDbType.VarChar);
			param8.Value = UUID;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@TransactionID", SqlDbType.VarChar);
			param9.Value = TransactionID;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@IdPeticionPAC", SqlDbType.BigInt);
			param10.Value = IdPeticionPAC;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@Estatus", SqlDbType.VarChar);
			param11.Value = Estatus;
			commandParameters.Add(param11);
			SqlParameter param12 = new SqlParameter("@RfcEmisor", SqlDbType.VarChar);
			param12.Value = RfcEmisor;
			commandParameters.Add(param12);
			SqlParameter param13 = new SqlParameter("@RazonSocialEmisor", SqlDbType.VarChar);
			param13.Value = RazonSocialEmisor;
			commandParameters.Add(param13);
			SqlParameter param14 = new SqlParameter("@NoCertificado", SqlDbType.VarChar);
			param14.Value = NoCertificado;
			commandParameters.Add(param14);
			SqlParameter param15 = new SqlParameter("@IdRegimenFiscal", SqlDbType.VarChar);
			param15.Value = IdRegimenFiscal;
			commandParameters.Add(param15);
			SqlParameter param16 = new SqlParameter("@RfcReceptor", SqlDbType.VarChar);
			param16.Value = RfcReceptor;
			commandParameters.Add(param16);
			SqlParameter param17 = new SqlParameter("@RazonSocialReceptor", SqlDbType.VarChar);
			param17.Value = RazonSocialReceptor;
			commandParameters.Add(param17);
			SqlParameter param18 = new SqlParameter("@EmailReceptor", SqlDbType.VarChar);
			param18.Value = EmailReceptor;
			commandParameters.Add(param18);
			SqlParameter param19 = new SqlParameter("@EsExtranjero", SqlDbType.Bit);
			param19.Value = EsExtranjero;
			commandParameters.Add(param19);
			SqlParameter param20 = new SqlParameter("@IdPaisResidenciaFiscal", SqlDbType.VarChar);
			param20.Value = IdPaisResidenciaFiscal;
			commandParameters.Add(param20);
			SqlParameter param21 = new SqlParameter("@NumRegIdTrib", SqlDbType.VarChar);
			param21.Value = NumRegIdTrib;
			commandParameters.Add(param21);
			SqlParameter param22 = new SqlParameter("@UsoCFDI", SqlDbType.VarChar);
			param22.Value = UsoCFDI;
			commandParameters.Add(param22);
			SqlParameter param23 = new SqlParameter("@FormaPago", SqlDbType.VarChar);
			param23.Value = FormaPago;
			commandParameters.Add(param23);
			SqlParameter param24 = new SqlParameter("@MetodoPago", SqlDbType.VarChar);
			param24.Value = MetodoPago;
			commandParameters.Add(param24);
			SqlParameter param25 = new SqlParameter("@TipoComprobante", SqlDbType.VarChar);
			param25.Value = TipoComprobante;
			commandParameters.Add(param25);
			SqlParameter param26 = new SqlParameter("@LugarExpedicion", SqlDbType.VarChar);
			param26.Value = LugarExpedicion;
			commandParameters.Add(param26);
			SqlParameter param27 = new SqlParameter("@CondicionesPago", SqlDbType.VarChar);
			param27.Value = CondicionesPago;
			commandParameters.Add(param27);
			SqlParameter param28 = new SqlParameter("@Moneda", SqlDbType.VarChar);
			param28.Value = Moneda;
			commandParameters.Add(param28);
			SqlParameter param29 = new SqlParameter("@TipoCambio", SqlDbType.Money);
			param29.Value = TipoCambio;
			commandParameters.Add(param29);
			SqlParameter param30 = new SqlParameter("@SubTotal", SqlDbType.Money);
			param30.Value = SubTotal;
			commandParameters.Add(param30);
			SqlParameter param31 = new SqlParameter("@Descuento", SqlDbType.Money);
			param31.Value = Descuento;
			commandParameters.Add(param31);
			SqlParameter param32 = new SqlParameter("@Total", SqlDbType.Money);
			param32.Value = Total;
			commandParameters.Add(param32);
			SqlParameter param33 = new SqlParameter("@MontoTarifa", SqlDbType.Money);
			param33.Value = MontoTarifa;
			commandParameters.Add(param33);
			SqlParameter param34 = new SqlParameter("@MontoServAdic", SqlDbType.Money);
			param34.Value = MontoServAdic;
			commandParameters.Add(param34);
			SqlParameter param35 = new SqlParameter("@MontoTUA", SqlDbType.Money);
			param35.Value = MontoTUA;
			commandParameters.Add(param35);
			SqlParameter param36 = new SqlParameter("@MontoOtrosCargos", SqlDbType.Money);
			param36.Value = MontoOtrosCargos;
			commandParameters.Add(param36);
			SqlParameter param37 = new SqlParameter("@MontoIVA", SqlDbType.Money);
			param37.Value = MontoIVA;
			commandParameters.Add(param37);
			SqlParameter param38 = new SqlParameter("@IdAgente", SqlDbType.Int);
			param38.Value = IdAgente;
			commandParameters.Add(param38);
			SqlParameter param39 = new SqlParameter("@IdUsuario", SqlDbType.Int);
			param39.Value = IdUsuario;
			commandParameters.Add(param39);
			SqlParameter param40 = new SqlParameter("@IdUsuarioCancelo", SqlDbType.Int);
			param40.Value = IdUsuarioCancelo;
			commandParameters.Add(param40);
			SqlParameter param41 = new SqlParameter("@FechaHoraCancelLocal", SqlDbType.VarChar);
			param41.Value = FechaHoraCancelLocal.Year > 1900 ? FechaHoraCancelLocal.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param41);
			SqlParameter param42 = new SqlParameter("@FechaHoraPago", SqlDbType.VarChar);
			param42.Value = FechaHoraPago.Year > 1900 ? FechaHoraPago.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param42);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdFacturasCab";
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

		#region Eliminar VBFac_Facturas_Cab
		public void Eliminar(long idfacturacab)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = idfacturacab;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelFacturasCab";
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

		#region Deshacer VBFac_Facturas_Cab
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdFacturaCab);
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
		public List<ENTFacturasCab> RecuperarTodo()
		{
			List<ENTFacturasCab> result = new List<ENTFacturasCab>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasCab_TODO";
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
				    ENTFacturasCab item = new ENTFacturasCab();
					 if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					 if (!dr.IsNull("IdEmpresa")) item.IdEmpresa = Convert.ToByte(dr["IdEmpresa"]);
					 if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					 if (!dr.IsNull("FechaHoraExpedicion")) item.FechaHoraExpedicion = Convert.ToDateTime(dr["FechaHoraExpedicion"]);
					 if (!dr.IsNull("TipoFacturacion")) item.TipoFacturacion = dr["TipoFacturacion"].ToString();
					 if (!dr.IsNull("Version")) item.Version = dr["Version"].ToString();
					 if (!dr.IsNull("Serie")) item.Serie = dr["Serie"].ToString();
					 if (!dr.IsNull("FolioFactura")) item.FolioFactura = Convert.ToInt64(dr["FolioFactura"]);
					 if (!dr.IsNull("UUID")) item.UUID = dr["UUID"].ToString();
					 if (!dr.IsNull("TransactionID")) item.TransactionID = dr["TransactionID"].ToString();
					 if (!dr.IsNull("IdPeticionPAC")) item.IdPeticionPAC = Convert.ToInt64(dr["IdPeticionPAC"]);
					 if (!dr.IsNull("Estatus")) item.Estatus = dr["Estatus"].ToString();
					 if (!dr.IsNull("RfcEmisor")) item.RfcEmisor = dr["RfcEmisor"].ToString();
					 if (!dr.IsNull("RazonSocialEmisor")) item.RazonSocialEmisor = dr["RazonSocialEmisor"].ToString();
					 if (!dr.IsNull("NoCertificado")) item.NoCertificado = dr["NoCertificado"].ToString();
					 if (!dr.IsNull("IdRegimenFiscal")) item.IdRegimenFiscal = dr["IdRegimenFiscal"].ToString();
					 if (!dr.IsNull("RfcReceptor")) item.RfcReceptor = dr["RfcReceptor"].ToString();
					 if (!dr.IsNull("RazonSocialReceptor")) item.RazonSocialReceptor = dr["RazonSocialReceptor"].ToString();
					 if (!dr.IsNull("EmailReceptor")) item.EmailReceptor = dr["EmailReceptor"].ToString();
					 if (!dr.IsNull("EsExtranjero")) item.EsExtranjero = Convert.ToBoolean(dr["EsExtranjero"]);
					 if (!dr.IsNull("IdPaisResidenciaFiscal")) item.IdPaisResidenciaFiscal = dr["IdPaisResidenciaFiscal"].ToString();
					 if (!dr.IsNull("NumRegIdTrib")) item.NumRegIdTrib = dr["NumRegIdTrib"].ToString();
					 if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					 if (!dr.IsNull("FormaPago")) item.FormaPago = dr["FormaPago"].ToString();
					 if (!dr.IsNull("MetodoPago")) item.MetodoPago = dr["MetodoPago"].ToString();
					 if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
					 if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					 if (!dr.IsNull("CondicionesPago")) item.CondicionesPago = dr["CondicionesPago"].ToString();
					 if (!dr.IsNull("Moneda")) item.Moneda = dr["Moneda"].ToString();
					 if (!dr.IsNull("TipoCambio")) item.TipoCambio = Convert.ToDecimal(dr["TipoCambio"]);
					 if (!dr.IsNull("SubTotal")) item.SubTotal = Convert.ToDecimal(dr["SubTotal"]);
					 if (!dr.IsNull("Descuento")) item.Descuento = Convert.ToDecimal(dr["Descuento"]);
					 if (!dr.IsNull("Total")) item.Total = Convert.ToDecimal(dr["Total"]);
					 if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					 if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					 if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					 if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					 if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					 if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt32(dr["IdAgente"]);
					 if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
					 if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					 if (!dr.IsNull("IdUsuarioCancelo")) item.IdUsuarioCancelo = Convert.ToInt32(dr["IdUsuarioCancelo"]);
					 if (!dr.IsNull("FechaHoraCancelLocal")) item.FechaHoraCancelLocal = Convert.ToDateTime(dr["FechaHoraCancelLocal"]);
					 if (!dr.IsNull("FechaHoraPago")) item.FechaHoraPago = Convert.ToDateTime(dr["FechaHoraPago"]);
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

		#region Recuperar RecuperarFacturasCabPorLlavePrimaria
		public List<ENTFacturasCab> RecuperarFacturasCabPorLlavePrimaria(long idfacturacab)
		{
			List<ENTFacturasCab> result = new List<ENTFacturasCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = idfacturacab;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasCab_POR_PK";
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
				    ENTFacturasCab item = new ENTFacturasCab();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdEmpresa")) item.IdEmpresa = Convert.ToByte(dr["IdEmpresa"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("FechaHoraExpedicion")) item.FechaHoraExpedicion = Convert.ToDateTime(dr["FechaHoraExpedicion"]);
					if (!dr.IsNull("TipoFacturacion")) item.TipoFacturacion = dr["TipoFacturacion"].ToString();
					if (!dr.IsNull("Version")) item.Version = dr["Version"].ToString();
					if (!dr.IsNull("Serie")) item.Serie = dr["Serie"].ToString();
					if (!dr.IsNull("FolioFactura")) item.FolioFactura = Convert.ToInt64(dr["FolioFactura"]);
					if (!dr.IsNull("UUID")) item.UUID = dr["UUID"].ToString();
					if (!dr.IsNull("TransactionID")) item.TransactionID = dr["TransactionID"].ToString();
					if (!dr.IsNull("IdPeticionPAC")) item.IdPeticionPAC = Convert.ToInt64(dr["IdPeticionPAC"]);
					if (!dr.IsNull("Estatus")) item.Estatus = dr["Estatus"].ToString();
					if (!dr.IsNull("RfcEmisor")) item.RfcEmisor = dr["RfcEmisor"].ToString();
					if (!dr.IsNull("RazonSocialEmisor")) item.RazonSocialEmisor = dr["RazonSocialEmisor"].ToString();
					if (!dr.IsNull("NoCertificado")) item.NoCertificado = dr["NoCertificado"].ToString();
					if (!dr.IsNull("IdRegimenFiscal")) item.IdRegimenFiscal = dr["IdRegimenFiscal"].ToString();
					if (!dr.IsNull("RfcReceptor")) item.RfcReceptor = dr["RfcReceptor"].ToString();
					if (!dr.IsNull("RazonSocialReceptor")) item.RazonSocialReceptor = dr["RazonSocialReceptor"].ToString();
					if (!dr.IsNull("EmailReceptor")) item.EmailReceptor = dr["EmailReceptor"].ToString();
					if (!dr.IsNull("EsExtranjero")) item.EsExtranjero = Convert.ToBoolean(dr["EsExtranjero"]);
					if (!dr.IsNull("IdPaisResidenciaFiscal")) item.IdPaisResidenciaFiscal = dr["IdPaisResidenciaFiscal"].ToString();
					if (!dr.IsNull("NumRegIdTrib")) item.NumRegIdTrib = dr["NumRegIdTrib"].ToString();
					if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					if (!dr.IsNull("FormaPago")) item.FormaPago = dr["FormaPago"].ToString();
					if (!dr.IsNull("MetodoPago")) item.MetodoPago = dr["MetodoPago"].ToString();
					if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("CondicionesPago")) item.CondicionesPago = dr["CondicionesPago"].ToString();
					if (!dr.IsNull("Moneda")) item.Moneda = dr["Moneda"].ToString();
					if (!dr.IsNull("TipoCambio")) item.TipoCambio = Convert.ToDecimal(dr["TipoCambio"]);
					if (!dr.IsNull("SubTotal")) item.SubTotal = Convert.ToDecimal(dr["SubTotal"]);
					if (!dr.IsNull("Descuento")) item.Descuento = Convert.ToDecimal(dr["Descuento"]);
					if (!dr.IsNull("Total")) item.Total = Convert.ToDecimal(dr["Total"]);
					if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt32(dr["IdAgente"]);
					if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("IdUsuarioCancelo")) item.IdUsuarioCancelo = Convert.ToInt32(dr["IdUsuarioCancelo"]);
					if (!dr.IsNull("FechaHoraCancelLocal")) item.FechaHoraCancelLocal = Convert.ToDateTime(dr["FechaHoraCancelLocal"]);
					if (!dr.IsNull("FechaHoraPago")) item.FechaHoraPago = Convert.ToDateTime(dr["FechaHoraPago"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdEmpresa")) IdEmpresa = Convert.ToByte(dtResultado.Rows[0]["IdEmpresa"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraExpedicion")) FechaHoraExpedicion = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraExpedicion"]);
					if (!dtResultado.Rows[0].IsNull("TipoFacturacion")) TipoFacturacion = dtResultado.Rows[0]["TipoFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("Version")) Version = dtResultado.Rows[0]["Version"].ToString();
					if (!dtResultado.Rows[0].IsNull("Serie")) Serie = dtResultado.Rows[0]["Serie"].ToString();
					if (!dtResultado.Rows[0].IsNull("FolioFactura")) FolioFactura = Convert.ToInt64(dtResultado.Rows[0]["FolioFactura"]);
					if (!dtResultado.Rows[0].IsNull("UUID")) UUID = dtResultado.Rows[0]["UUID"].ToString();
					if (!dtResultado.Rows[0].IsNull("TransactionID")) TransactionID = dtResultado.Rows[0]["TransactionID"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdPeticionPAC")) IdPeticionPAC = Convert.ToInt64(dtResultado.Rows[0]["IdPeticionPAC"]);
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = dtResultado.Rows[0]["Estatus"].ToString();
					if (!dtResultado.Rows[0].IsNull("RfcEmisor")) RfcEmisor = dtResultado.Rows[0]["RfcEmisor"].ToString();
					if (!dtResultado.Rows[0].IsNull("RazonSocialEmisor")) RazonSocialEmisor = dtResultado.Rows[0]["RazonSocialEmisor"].ToString();
					if (!dtResultado.Rows[0].IsNull("NoCertificado")) NoCertificado = dtResultado.Rows[0]["NoCertificado"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdRegimenFiscal")) IdRegimenFiscal = dtResultado.Rows[0]["IdRegimenFiscal"].ToString();
					if (!dtResultado.Rows[0].IsNull("RfcReceptor")) RfcReceptor = dtResultado.Rows[0]["RfcReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("RazonSocialReceptor")) RazonSocialReceptor = dtResultado.Rows[0]["RazonSocialReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("EmailReceptor")) EmailReceptor = dtResultado.Rows[0]["EmailReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsExtranjero")) EsExtranjero = Convert.ToBoolean(dtResultado.Rows[0]["EsExtranjero"]);
					if (!dtResultado.Rows[0].IsNull("IdPaisResidenciaFiscal")) IdPaisResidenciaFiscal = dtResultado.Rows[0]["IdPaisResidenciaFiscal"].ToString();
					if (!dtResultado.Rows[0].IsNull("NumRegIdTrib")) NumRegIdTrib = dtResultado.Rows[0]["NumRegIdTrib"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsoCFDI")) UsoCFDI = dtResultado.Rows[0]["UsoCFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("FormaPago")) FormaPago = dtResultado.Rows[0]["FormaPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("MetodoPago")) MetodoPago = dtResultado.Rows[0]["MetodoPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("TipoComprobante")) TipoComprobante = dtResultado.Rows[0]["TipoComprobante"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
					if (!dtResultado.Rows[0].IsNull("CondicionesPago")) CondicionesPago = dtResultado.Rows[0]["CondicionesPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("Moneda")) Moneda = dtResultado.Rows[0]["Moneda"].ToString();
					if (!dtResultado.Rows[0].IsNull("TipoCambio")) TipoCambio = Convert.ToDecimal(dtResultado.Rows[0]["TipoCambio"]);
					if (!dtResultado.Rows[0].IsNull("SubTotal")) SubTotal = Convert.ToDecimal(dtResultado.Rows[0]["SubTotal"]);
					if (!dtResultado.Rows[0].IsNull("Descuento")) Descuento = Convert.ToDecimal(dtResultado.Rows[0]["Descuento"]);
					if (!dtResultado.Rows[0].IsNull("Total")) Total = Convert.ToDecimal(dtResultado.Rows[0]["Total"]);
					if (!dtResultado.Rows[0].IsNull("MontoTarifa")) MontoTarifa = Convert.ToDecimal(dtResultado.Rows[0]["MontoTarifa"]);
					if (!dtResultado.Rows[0].IsNull("MontoServAdic")) MontoServAdic = Convert.ToDecimal(dtResultado.Rows[0]["MontoServAdic"]);
					if (!dtResultado.Rows[0].IsNull("MontoTUA")) MontoTUA = Convert.ToDecimal(dtResultado.Rows[0]["MontoTUA"]);
					if (!dtResultado.Rows[0].IsNull("MontoOtrosCargos")) MontoOtrosCargos = Convert.ToDecimal(dtResultado.Rows[0]["MontoOtrosCargos"]);
					if (!dtResultado.Rows[0].IsNull("MontoIVA")) MontoIVA = Convert.ToDecimal(dtResultado.Rows[0]["MontoIVA"]);
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt32(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("IdUsuario")) IdUsuario = Convert.ToInt32(dtResultado.Rows[0]["IdUsuario"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("IdUsuarioCancelo")) IdUsuarioCancelo = Convert.ToInt32(dtResultado.Rows[0]["IdUsuarioCancelo"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraCancelLocal")) FechaHoraCancelLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraCancelLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraPago")) FechaHoraPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraPago"]);
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

		#region Recuperar RecuperarFacturasCabIdpeticionpac
		public List<ENTFacturasCab> RecuperarFacturasCabIdpeticionpac(long idpeticionpac)
		{
			List<ENTFacturasCab> result = new List<ENTFacturasCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdPeticionPAC", SqlDbType.BigInt);
			param0.Value = idpeticionpac;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasCab_POR_IdPeticionPAC";
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
				    ENTFacturasCab item = new ENTFacturasCab();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdEmpresa")) item.IdEmpresa = Convert.ToByte(dr["IdEmpresa"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("FechaHoraExpedicion")) item.FechaHoraExpedicion = Convert.ToDateTime(dr["FechaHoraExpedicion"]);
					if (!dr.IsNull("TipoFacturacion")) item.TipoFacturacion = dr["TipoFacturacion"].ToString();
					if (!dr.IsNull("Version")) item.Version = dr["Version"].ToString();
					if (!dr.IsNull("Serie")) item.Serie = dr["Serie"].ToString();
					if (!dr.IsNull("FolioFactura")) item.FolioFactura = Convert.ToInt64(dr["FolioFactura"]);
					if (!dr.IsNull("UUID")) item.UUID = dr["UUID"].ToString();
					if (!dr.IsNull("TransactionID")) item.TransactionID = dr["TransactionID"].ToString();
					if (!dr.IsNull("IdPeticionPAC")) item.IdPeticionPAC = Convert.ToInt64(dr["IdPeticionPAC"]);
					if (!dr.IsNull("Estatus")) item.Estatus = dr["Estatus"].ToString();
					if (!dr.IsNull("RfcEmisor")) item.RfcEmisor = dr["RfcEmisor"].ToString();
					if (!dr.IsNull("RazonSocialEmisor")) item.RazonSocialEmisor = dr["RazonSocialEmisor"].ToString();
					if (!dr.IsNull("NoCertificado")) item.NoCertificado = dr["NoCertificado"].ToString();
					if (!dr.IsNull("IdRegimenFiscal")) item.IdRegimenFiscal = dr["IdRegimenFiscal"].ToString();
					if (!dr.IsNull("RfcReceptor")) item.RfcReceptor = dr["RfcReceptor"].ToString();
					if (!dr.IsNull("RazonSocialReceptor")) item.RazonSocialReceptor = dr["RazonSocialReceptor"].ToString();
					if (!dr.IsNull("EmailReceptor")) item.EmailReceptor = dr["EmailReceptor"].ToString();
					if (!dr.IsNull("EsExtranjero")) item.EsExtranjero = Convert.ToBoolean(dr["EsExtranjero"]);
					if (!dr.IsNull("IdPaisResidenciaFiscal")) item.IdPaisResidenciaFiscal = dr["IdPaisResidenciaFiscal"].ToString();
					if (!dr.IsNull("NumRegIdTrib")) item.NumRegIdTrib = dr["NumRegIdTrib"].ToString();
					if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					if (!dr.IsNull("FormaPago")) item.FormaPago = dr["FormaPago"].ToString();
					if (!dr.IsNull("MetodoPago")) item.MetodoPago = dr["MetodoPago"].ToString();
					if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("CondicionesPago")) item.CondicionesPago = dr["CondicionesPago"].ToString();
					if (!dr.IsNull("Moneda")) item.Moneda = dr["Moneda"].ToString();
					if (!dr.IsNull("TipoCambio")) item.TipoCambio = Convert.ToDecimal(dr["TipoCambio"]);
					if (!dr.IsNull("SubTotal")) item.SubTotal = Convert.ToDecimal(dr["SubTotal"]);
					if (!dr.IsNull("Descuento")) item.Descuento = Convert.ToDecimal(dr["Descuento"]);
					if (!dr.IsNull("Total")) item.Total = Convert.ToDecimal(dr["Total"]);
					if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt32(dr["IdAgente"]);
					if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("IdUsuarioCancelo")) item.IdUsuarioCancelo = Convert.ToInt32(dr["IdUsuarioCancelo"]);
					if (!dr.IsNull("FechaHoraCancelLocal")) item.FechaHoraCancelLocal = Convert.ToDateTime(dr["FechaHoraCancelLocal"]);
					if (!dr.IsNull("FechaHoraPago")) item.FechaHoraPago = Convert.ToDateTime(dr["FechaHoraPago"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdEmpresa")) IdEmpresa = Convert.ToByte(dtResultado.Rows[0]["IdEmpresa"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraExpedicion")) FechaHoraExpedicion = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraExpedicion"]);
					if (!dtResultado.Rows[0].IsNull("TipoFacturacion")) TipoFacturacion = dtResultado.Rows[0]["TipoFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("Version")) Version = dtResultado.Rows[0]["Version"].ToString();
					if (!dtResultado.Rows[0].IsNull("Serie")) Serie = dtResultado.Rows[0]["Serie"].ToString();
					if (!dtResultado.Rows[0].IsNull("FolioFactura")) FolioFactura = Convert.ToInt64(dtResultado.Rows[0]["FolioFactura"]);
					if (!dtResultado.Rows[0].IsNull("UUID")) UUID = dtResultado.Rows[0]["UUID"].ToString();
					if (!dtResultado.Rows[0].IsNull("TransactionID")) TransactionID = dtResultado.Rows[0]["TransactionID"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdPeticionPAC")) IdPeticionPAC = Convert.ToInt64(dtResultado.Rows[0]["IdPeticionPAC"]);
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = dtResultado.Rows[0]["Estatus"].ToString();
					if (!dtResultado.Rows[0].IsNull("RfcEmisor")) RfcEmisor = dtResultado.Rows[0]["RfcEmisor"].ToString();
					if (!dtResultado.Rows[0].IsNull("RazonSocialEmisor")) RazonSocialEmisor = dtResultado.Rows[0]["RazonSocialEmisor"].ToString();
					if (!dtResultado.Rows[0].IsNull("NoCertificado")) NoCertificado = dtResultado.Rows[0]["NoCertificado"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdRegimenFiscal")) IdRegimenFiscal = dtResultado.Rows[0]["IdRegimenFiscal"].ToString();
					if (!dtResultado.Rows[0].IsNull("RfcReceptor")) RfcReceptor = dtResultado.Rows[0]["RfcReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("RazonSocialReceptor")) RazonSocialReceptor = dtResultado.Rows[0]["RazonSocialReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("EmailReceptor")) EmailReceptor = dtResultado.Rows[0]["EmailReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsExtranjero")) EsExtranjero = Convert.ToBoolean(dtResultado.Rows[0]["EsExtranjero"]);
					if (!dtResultado.Rows[0].IsNull("IdPaisResidenciaFiscal")) IdPaisResidenciaFiscal = dtResultado.Rows[0]["IdPaisResidenciaFiscal"].ToString();
					if (!dtResultado.Rows[0].IsNull("NumRegIdTrib")) NumRegIdTrib = dtResultado.Rows[0]["NumRegIdTrib"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsoCFDI")) UsoCFDI = dtResultado.Rows[0]["UsoCFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("FormaPago")) FormaPago = dtResultado.Rows[0]["FormaPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("MetodoPago")) MetodoPago = dtResultado.Rows[0]["MetodoPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("TipoComprobante")) TipoComprobante = dtResultado.Rows[0]["TipoComprobante"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
					if (!dtResultado.Rows[0].IsNull("CondicionesPago")) CondicionesPago = dtResultado.Rows[0]["CondicionesPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("Moneda")) Moneda = dtResultado.Rows[0]["Moneda"].ToString();
					if (!dtResultado.Rows[0].IsNull("TipoCambio")) TipoCambio = Convert.ToDecimal(dtResultado.Rows[0]["TipoCambio"]);
					if (!dtResultado.Rows[0].IsNull("SubTotal")) SubTotal = Convert.ToDecimal(dtResultado.Rows[0]["SubTotal"]);
					if (!dtResultado.Rows[0].IsNull("Descuento")) Descuento = Convert.ToDecimal(dtResultado.Rows[0]["Descuento"]);
					if (!dtResultado.Rows[0].IsNull("Total")) Total = Convert.ToDecimal(dtResultado.Rows[0]["Total"]);
					if (!dtResultado.Rows[0].IsNull("MontoTarifa")) MontoTarifa = Convert.ToDecimal(dtResultado.Rows[0]["MontoTarifa"]);
					if (!dtResultado.Rows[0].IsNull("MontoServAdic")) MontoServAdic = Convert.ToDecimal(dtResultado.Rows[0]["MontoServAdic"]);
					if (!dtResultado.Rows[0].IsNull("MontoTUA")) MontoTUA = Convert.ToDecimal(dtResultado.Rows[0]["MontoTUA"]);
					if (!dtResultado.Rows[0].IsNull("MontoOtrosCargos")) MontoOtrosCargos = Convert.ToDecimal(dtResultado.Rows[0]["MontoOtrosCargos"]);
					if (!dtResultado.Rows[0].IsNull("MontoIVA")) MontoIVA = Convert.ToDecimal(dtResultado.Rows[0]["MontoIVA"]);
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt32(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("IdUsuario")) IdUsuario = Convert.ToInt32(dtResultado.Rows[0]["IdUsuario"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("IdUsuarioCancelo")) IdUsuarioCancelo = Convert.ToInt32(dtResultado.Rows[0]["IdUsuarioCancelo"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraCancelLocal")) FechaHoraCancelLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraCancelLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraPago")) FechaHoraPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraPago"]);
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

		#region Recuperar RecuperarFacturasCabBookingid
		public List<ENTFacturasCab> RecuperarFacturasCabBookingid(long bookingid)
		{
			List<ENTFacturasCab> result = new List<ENTFacturasCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param0.Value = bookingid;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasCab_POR_BookingID";
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
				    ENTFacturasCab item = new ENTFacturasCab();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdEmpresa")) item.IdEmpresa = Convert.ToByte(dr["IdEmpresa"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("FechaHoraExpedicion")) item.FechaHoraExpedicion = Convert.ToDateTime(dr["FechaHoraExpedicion"]);
					if (!dr.IsNull("TipoFacturacion")) item.TipoFacturacion = dr["TipoFacturacion"].ToString();
					if (!dr.IsNull("Version")) item.Version = dr["Version"].ToString();
					if (!dr.IsNull("Serie")) item.Serie = dr["Serie"].ToString();
					if (!dr.IsNull("FolioFactura")) item.FolioFactura = Convert.ToInt64(dr["FolioFactura"]);
					if (!dr.IsNull("UUID")) item.UUID = dr["UUID"].ToString();
					if (!dr.IsNull("TransactionID")) item.TransactionID = dr["TransactionID"].ToString();
					if (!dr.IsNull("IdPeticionPAC")) item.IdPeticionPAC = Convert.ToInt64(dr["IdPeticionPAC"]);
					if (!dr.IsNull("Estatus")) item.Estatus = dr["Estatus"].ToString();
					if (!dr.IsNull("RfcEmisor")) item.RfcEmisor = dr["RfcEmisor"].ToString();
					if (!dr.IsNull("RazonSocialEmisor")) item.RazonSocialEmisor = dr["RazonSocialEmisor"].ToString();
					if (!dr.IsNull("NoCertificado")) item.NoCertificado = dr["NoCertificado"].ToString();
					if (!dr.IsNull("IdRegimenFiscal")) item.IdRegimenFiscal = dr["IdRegimenFiscal"].ToString();
					if (!dr.IsNull("RfcReceptor")) item.RfcReceptor = dr["RfcReceptor"].ToString();
					if (!dr.IsNull("RazonSocialReceptor")) item.RazonSocialReceptor = dr["RazonSocialReceptor"].ToString();
					if (!dr.IsNull("EmailReceptor")) item.EmailReceptor = dr["EmailReceptor"].ToString();
					if (!dr.IsNull("EsExtranjero")) item.EsExtranjero = Convert.ToBoolean(dr["EsExtranjero"]);
					if (!dr.IsNull("IdPaisResidenciaFiscal")) item.IdPaisResidenciaFiscal = dr["IdPaisResidenciaFiscal"].ToString();
					if (!dr.IsNull("NumRegIdTrib")) item.NumRegIdTrib = dr["NumRegIdTrib"].ToString();
					if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					if (!dr.IsNull("FormaPago")) item.FormaPago = dr["FormaPago"].ToString();
					if (!dr.IsNull("MetodoPago")) item.MetodoPago = dr["MetodoPago"].ToString();
					if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("CondicionesPago")) item.CondicionesPago = dr["CondicionesPago"].ToString();
					if (!dr.IsNull("Moneda")) item.Moneda = dr["Moneda"].ToString();
					if (!dr.IsNull("TipoCambio")) item.TipoCambio = Convert.ToDecimal(dr["TipoCambio"]);
					if (!dr.IsNull("SubTotal")) item.SubTotal = Convert.ToDecimal(dr["SubTotal"]);
					if (!dr.IsNull("Descuento")) item.Descuento = Convert.ToDecimal(dr["Descuento"]);
					if (!dr.IsNull("Total")) item.Total = Convert.ToDecimal(dr["Total"]);
					if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt32(dr["IdAgente"]);
					if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("IdUsuarioCancelo")) item.IdUsuarioCancelo = Convert.ToInt32(dr["IdUsuarioCancelo"]);
					if (!dr.IsNull("FechaHoraCancelLocal")) item.FechaHoraCancelLocal = Convert.ToDateTime(dr["FechaHoraCancelLocal"]);
					if (!dr.IsNull("FechaHoraPago")) item.FechaHoraPago = Convert.ToDateTime(dr["FechaHoraPago"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdEmpresa")) IdEmpresa = Convert.ToByte(dtResultado.Rows[0]["IdEmpresa"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraExpedicion")) FechaHoraExpedicion = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraExpedicion"]);
					if (!dtResultado.Rows[0].IsNull("TipoFacturacion")) TipoFacturacion = dtResultado.Rows[0]["TipoFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("Version")) Version = dtResultado.Rows[0]["Version"].ToString();
					if (!dtResultado.Rows[0].IsNull("Serie")) Serie = dtResultado.Rows[0]["Serie"].ToString();
					if (!dtResultado.Rows[0].IsNull("FolioFactura")) FolioFactura = Convert.ToInt64(dtResultado.Rows[0]["FolioFactura"]);
					if (!dtResultado.Rows[0].IsNull("UUID")) UUID = dtResultado.Rows[0]["UUID"].ToString();
					if (!dtResultado.Rows[0].IsNull("TransactionID")) TransactionID = dtResultado.Rows[0]["TransactionID"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdPeticionPAC")) IdPeticionPAC = Convert.ToInt64(dtResultado.Rows[0]["IdPeticionPAC"]);
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = dtResultado.Rows[0]["Estatus"].ToString();
					if (!dtResultado.Rows[0].IsNull("RfcEmisor")) RfcEmisor = dtResultado.Rows[0]["RfcEmisor"].ToString();
					if (!dtResultado.Rows[0].IsNull("RazonSocialEmisor")) RazonSocialEmisor = dtResultado.Rows[0]["RazonSocialEmisor"].ToString();
					if (!dtResultado.Rows[0].IsNull("NoCertificado")) NoCertificado = dtResultado.Rows[0]["NoCertificado"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdRegimenFiscal")) IdRegimenFiscal = dtResultado.Rows[0]["IdRegimenFiscal"].ToString();
					if (!dtResultado.Rows[0].IsNull("RfcReceptor")) RfcReceptor = dtResultado.Rows[0]["RfcReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("RazonSocialReceptor")) RazonSocialReceptor = dtResultado.Rows[0]["RazonSocialReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("EmailReceptor")) EmailReceptor = dtResultado.Rows[0]["EmailReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsExtranjero")) EsExtranjero = Convert.ToBoolean(dtResultado.Rows[0]["EsExtranjero"]);
					if (!dtResultado.Rows[0].IsNull("IdPaisResidenciaFiscal")) IdPaisResidenciaFiscal = dtResultado.Rows[0]["IdPaisResidenciaFiscal"].ToString();
					if (!dtResultado.Rows[0].IsNull("NumRegIdTrib")) NumRegIdTrib = dtResultado.Rows[0]["NumRegIdTrib"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsoCFDI")) UsoCFDI = dtResultado.Rows[0]["UsoCFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("FormaPago")) FormaPago = dtResultado.Rows[0]["FormaPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("MetodoPago")) MetodoPago = dtResultado.Rows[0]["MetodoPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("TipoComprobante")) TipoComprobante = dtResultado.Rows[0]["TipoComprobante"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
					if (!dtResultado.Rows[0].IsNull("CondicionesPago")) CondicionesPago = dtResultado.Rows[0]["CondicionesPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("Moneda")) Moneda = dtResultado.Rows[0]["Moneda"].ToString();
					if (!dtResultado.Rows[0].IsNull("TipoCambio")) TipoCambio = Convert.ToDecimal(dtResultado.Rows[0]["TipoCambio"]);
					if (!dtResultado.Rows[0].IsNull("SubTotal")) SubTotal = Convert.ToDecimal(dtResultado.Rows[0]["SubTotal"]);
					if (!dtResultado.Rows[0].IsNull("Descuento")) Descuento = Convert.ToDecimal(dtResultado.Rows[0]["Descuento"]);
					if (!dtResultado.Rows[0].IsNull("Total")) Total = Convert.ToDecimal(dtResultado.Rows[0]["Total"]);
					if (!dtResultado.Rows[0].IsNull("MontoTarifa")) MontoTarifa = Convert.ToDecimal(dtResultado.Rows[0]["MontoTarifa"]);
					if (!dtResultado.Rows[0].IsNull("MontoServAdic")) MontoServAdic = Convert.ToDecimal(dtResultado.Rows[0]["MontoServAdic"]);
					if (!dtResultado.Rows[0].IsNull("MontoTUA")) MontoTUA = Convert.ToDecimal(dtResultado.Rows[0]["MontoTUA"]);
					if (!dtResultado.Rows[0].IsNull("MontoOtrosCargos")) MontoOtrosCargos = Convert.ToDecimal(dtResultado.Rows[0]["MontoOtrosCargos"]);
					if (!dtResultado.Rows[0].IsNull("MontoIVA")) MontoIVA = Convert.ToDecimal(dtResultado.Rows[0]["MontoIVA"]);
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt32(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("IdUsuario")) IdUsuario = Convert.ToInt32(dtResultado.Rows[0]["IdUsuario"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("IdUsuarioCancelo")) IdUsuarioCancelo = Convert.ToInt32(dtResultado.Rows[0]["IdUsuarioCancelo"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraCancelLocal")) FechaHoraCancelLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraCancelLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraPago")) FechaHoraPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraPago"]);
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

		#region Recuperar RecuperarFacturasCabFoliofactura
		public List<ENTFacturasCab> RecuperarFacturasCabFoliofactura(long foliofactura)
		{
			List<ENTFacturasCab> result = new List<ENTFacturasCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@FolioFactura", SqlDbType.BigInt);
			param0.Value = foliofactura;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasCab_POR_FolioFactura";
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
				    ENTFacturasCab item = new ENTFacturasCab();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdEmpresa")) item.IdEmpresa = Convert.ToByte(dr["IdEmpresa"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("FechaHoraExpedicion")) item.FechaHoraExpedicion = Convert.ToDateTime(dr["FechaHoraExpedicion"]);
					if (!dr.IsNull("TipoFacturacion")) item.TipoFacturacion = dr["TipoFacturacion"].ToString();
					if (!dr.IsNull("Version")) item.Version = dr["Version"].ToString();
					if (!dr.IsNull("Serie")) item.Serie = dr["Serie"].ToString();
					if (!dr.IsNull("FolioFactura")) item.FolioFactura = Convert.ToInt64(dr["FolioFactura"]);
					if (!dr.IsNull("UUID")) item.UUID = dr["UUID"].ToString();
					if (!dr.IsNull("TransactionID")) item.TransactionID = dr["TransactionID"].ToString();
					if (!dr.IsNull("IdPeticionPAC")) item.IdPeticionPAC = Convert.ToInt64(dr["IdPeticionPAC"]);
					if (!dr.IsNull("Estatus")) item.Estatus = dr["Estatus"].ToString();
					if (!dr.IsNull("RfcEmisor")) item.RfcEmisor = dr["RfcEmisor"].ToString();
					if (!dr.IsNull("RazonSocialEmisor")) item.RazonSocialEmisor = dr["RazonSocialEmisor"].ToString();
					if (!dr.IsNull("NoCertificado")) item.NoCertificado = dr["NoCertificado"].ToString();
					if (!dr.IsNull("IdRegimenFiscal")) item.IdRegimenFiscal = dr["IdRegimenFiscal"].ToString();
					if (!dr.IsNull("RfcReceptor")) item.RfcReceptor = dr["RfcReceptor"].ToString();
					if (!dr.IsNull("RazonSocialReceptor")) item.RazonSocialReceptor = dr["RazonSocialReceptor"].ToString();
					if (!dr.IsNull("EmailReceptor")) item.EmailReceptor = dr["EmailReceptor"].ToString();
					if (!dr.IsNull("EsExtranjero")) item.EsExtranjero = Convert.ToBoolean(dr["EsExtranjero"]);
					if (!dr.IsNull("IdPaisResidenciaFiscal")) item.IdPaisResidenciaFiscal = dr["IdPaisResidenciaFiscal"].ToString();
					if (!dr.IsNull("NumRegIdTrib")) item.NumRegIdTrib = dr["NumRegIdTrib"].ToString();
					if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					if (!dr.IsNull("FormaPago")) item.FormaPago = dr["FormaPago"].ToString();
					if (!dr.IsNull("MetodoPago")) item.MetodoPago = dr["MetodoPago"].ToString();
					if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("CondicionesPago")) item.CondicionesPago = dr["CondicionesPago"].ToString();
					if (!dr.IsNull("Moneda")) item.Moneda = dr["Moneda"].ToString();
					if (!dr.IsNull("TipoCambio")) item.TipoCambio = Convert.ToDecimal(dr["TipoCambio"]);
					if (!dr.IsNull("SubTotal")) item.SubTotal = Convert.ToDecimal(dr["SubTotal"]);
					if (!dr.IsNull("Descuento")) item.Descuento = Convert.ToDecimal(dr["Descuento"]);
					if (!dr.IsNull("Total")) item.Total = Convert.ToDecimal(dr["Total"]);
					if (!dr.IsNull("MontoTarifa")) item.MontoTarifa = Convert.ToDecimal(dr["MontoTarifa"]);
					if (!dr.IsNull("MontoServAdic")) item.MontoServAdic = Convert.ToDecimal(dr["MontoServAdic"]);
					if (!dr.IsNull("MontoTUA")) item.MontoTUA = Convert.ToDecimal(dr["MontoTUA"]);
					if (!dr.IsNull("MontoOtrosCargos")) item.MontoOtrosCargos = Convert.ToDecimal(dr["MontoOtrosCargos"]);
					if (!dr.IsNull("MontoIVA")) item.MontoIVA = Convert.ToDecimal(dr["MontoIVA"]);
					if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt32(dr["IdAgente"]);
					if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("IdUsuarioCancelo")) item.IdUsuarioCancelo = Convert.ToInt32(dr["IdUsuarioCancelo"]);
					if (!dr.IsNull("FechaHoraCancelLocal")) item.FechaHoraCancelLocal = Convert.ToDateTime(dr["FechaHoraCancelLocal"]);
					if (!dr.IsNull("FechaHoraPago")) item.FechaHoraPago = Convert.ToDateTime(dr["FechaHoraPago"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdEmpresa")) IdEmpresa = Convert.ToByte(dtResultado.Rows[0]["IdEmpresa"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraExpedicion")) FechaHoraExpedicion = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraExpedicion"]);
					if (!dtResultado.Rows[0].IsNull("TipoFacturacion")) TipoFacturacion = dtResultado.Rows[0]["TipoFacturacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("Version")) Version = dtResultado.Rows[0]["Version"].ToString();
					if (!dtResultado.Rows[0].IsNull("Serie")) Serie = dtResultado.Rows[0]["Serie"].ToString();
					if (!dtResultado.Rows[0].IsNull("FolioFactura")) FolioFactura = Convert.ToInt64(dtResultado.Rows[0]["FolioFactura"]);
					if (!dtResultado.Rows[0].IsNull("UUID")) UUID = dtResultado.Rows[0]["UUID"].ToString();
					if (!dtResultado.Rows[0].IsNull("TransactionID")) TransactionID = dtResultado.Rows[0]["TransactionID"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdPeticionPAC")) IdPeticionPAC = Convert.ToInt64(dtResultado.Rows[0]["IdPeticionPAC"]);
					if (!dtResultado.Rows[0].IsNull("Estatus")) Estatus = dtResultado.Rows[0]["Estatus"].ToString();
					if (!dtResultado.Rows[0].IsNull("RfcEmisor")) RfcEmisor = dtResultado.Rows[0]["RfcEmisor"].ToString();
					if (!dtResultado.Rows[0].IsNull("RazonSocialEmisor")) RazonSocialEmisor = dtResultado.Rows[0]["RazonSocialEmisor"].ToString();
					if (!dtResultado.Rows[0].IsNull("NoCertificado")) NoCertificado = dtResultado.Rows[0]["NoCertificado"].ToString();
					if (!dtResultado.Rows[0].IsNull("IdRegimenFiscal")) IdRegimenFiscal = dtResultado.Rows[0]["IdRegimenFiscal"].ToString();
					if (!dtResultado.Rows[0].IsNull("RfcReceptor")) RfcReceptor = dtResultado.Rows[0]["RfcReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("RazonSocialReceptor")) RazonSocialReceptor = dtResultado.Rows[0]["RazonSocialReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("EmailReceptor")) EmailReceptor = dtResultado.Rows[0]["EmailReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsExtranjero")) EsExtranjero = Convert.ToBoolean(dtResultado.Rows[0]["EsExtranjero"]);
					if (!dtResultado.Rows[0].IsNull("IdPaisResidenciaFiscal")) IdPaisResidenciaFiscal = dtResultado.Rows[0]["IdPaisResidenciaFiscal"].ToString();
					if (!dtResultado.Rows[0].IsNull("NumRegIdTrib")) NumRegIdTrib = dtResultado.Rows[0]["NumRegIdTrib"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsoCFDI")) UsoCFDI = dtResultado.Rows[0]["UsoCFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("FormaPago")) FormaPago = dtResultado.Rows[0]["FormaPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("MetodoPago")) MetodoPago = dtResultado.Rows[0]["MetodoPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("TipoComprobante")) TipoComprobante = dtResultado.Rows[0]["TipoComprobante"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
					if (!dtResultado.Rows[0].IsNull("CondicionesPago")) CondicionesPago = dtResultado.Rows[0]["CondicionesPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("Moneda")) Moneda = dtResultado.Rows[0]["Moneda"].ToString();
					if (!dtResultado.Rows[0].IsNull("TipoCambio")) TipoCambio = Convert.ToDecimal(dtResultado.Rows[0]["TipoCambio"]);
					if (!dtResultado.Rows[0].IsNull("SubTotal")) SubTotal = Convert.ToDecimal(dtResultado.Rows[0]["SubTotal"]);
					if (!dtResultado.Rows[0].IsNull("Descuento")) Descuento = Convert.ToDecimal(dtResultado.Rows[0]["Descuento"]);
					if (!dtResultado.Rows[0].IsNull("Total")) Total = Convert.ToDecimal(dtResultado.Rows[0]["Total"]);
					if (!dtResultado.Rows[0].IsNull("MontoTarifa")) MontoTarifa = Convert.ToDecimal(dtResultado.Rows[0]["MontoTarifa"]);
					if (!dtResultado.Rows[0].IsNull("MontoServAdic")) MontoServAdic = Convert.ToDecimal(dtResultado.Rows[0]["MontoServAdic"]);
					if (!dtResultado.Rows[0].IsNull("MontoTUA")) MontoTUA = Convert.ToDecimal(dtResultado.Rows[0]["MontoTUA"]);
					if (!dtResultado.Rows[0].IsNull("MontoOtrosCargos")) MontoOtrosCargos = Convert.ToDecimal(dtResultado.Rows[0]["MontoOtrosCargos"]);
					if (!dtResultado.Rows[0].IsNull("MontoIVA")) MontoIVA = Convert.ToDecimal(dtResultado.Rows[0]["MontoIVA"]);
					if (!dtResultado.Rows[0].IsNull("IdAgente")) IdAgente = Convert.ToInt32(dtResultado.Rows[0]["IdAgente"]);
					if (!dtResultado.Rows[0].IsNull("IdUsuario")) IdUsuario = Convert.ToInt32(dtResultado.Rows[0]["IdUsuario"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("IdUsuarioCancelo")) IdUsuarioCancelo = Convert.ToInt32(dtResultado.Rows[0]["IdUsuarioCancelo"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraCancelLocal")) FechaHoraCancelLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraCancelLocal"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraPago")) FechaHoraPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraPago"]);
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
			_copia = new DALFacturasCab(_conexion);
			Type tipo = typeof(ENTFacturasCab);
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

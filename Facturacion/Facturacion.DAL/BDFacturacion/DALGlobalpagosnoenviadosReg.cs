using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALGlobalpagosnoenviadosReg: ENTGlobalpagosnoenviadosReg
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALGlobalpagosnoenviadosReg _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALGlobalpagosnoenviadosReg(SqlConnection conexion)
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
			IdPagoOmitidoFG = 0;
			BookingID = 0;
			PaymentID = 0;
			RecordLocator = String.Empty;
			FechaPago = new DateTime();
			CodigoMoneda = String.Empty;
			LugarExpedicion = String.Empty;
			FechaEnvioFG = new DateTime();
			MotivoOmision = String.Empty;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_GlobalPagosNoEnviados_Reg
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdPagoOmitidoFG", SqlDbType.Int);
			param0.Value = IdPagoOmitidoFG;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param1.Value = BookingID;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@PaymentID", SqlDbType.BigInt);
			param2.Value = PaymentID;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@RecordLocator", SqlDbType.VarChar);
			param3.Value = RecordLocator;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@FechaPago", SqlDbType.VarChar);
			param4.Value = FechaPago.Year > 1900 ? FechaPago.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@CodigoMoneda", SqlDbType.VarChar);
			param5.Value = CodigoMoneda;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@LugarExpedicion", SqlDbType.VarChar);
			param6.Value = LugarExpedicion;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@FechaEnvioFG", SqlDbType.VarChar);
			param7.Value = FechaEnvioFG.Year > 1900 ? FechaEnvioFG.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@MotivoOmision", SqlDbType.VarChar);
			param8.Value = MotivoOmision;
			commandParameters.Add(param8);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsGlobalPagosNoEnviadosReg";
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
						IdPagoOmitidoFG = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_GlobalPagosNoEnviados_Reg
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdPagoOmitidoFG", SqlDbType.Int);
			param0.Value = IdPagoOmitidoFG;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@BookingID", SqlDbType.BigInt);
			param1.Value = BookingID;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@PaymentID", SqlDbType.BigInt);
			param2.Value = PaymentID;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@RecordLocator", SqlDbType.VarChar);
			param3.Value = RecordLocator;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@FechaPago", SqlDbType.VarChar);
			param4.Value = FechaPago.Year > 1900 ? FechaPago.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@CodigoMoneda", SqlDbType.VarChar);
			param5.Value = CodigoMoneda;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@LugarExpedicion", SqlDbType.VarChar);
			param6.Value = LugarExpedicion;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@FechaEnvioFG", SqlDbType.VarChar);
			param7.Value = FechaEnvioFG.Year > 1900 ? FechaEnvioFG.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@MotivoOmision", SqlDbType.VarChar);
			param8.Value = MotivoOmision;
			commandParameters.Add(param8);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdGlobalPagosNoEnviadosReg";
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

		#region Eliminar VBFac_GlobalPagosNoEnviados_Reg
		public void Eliminar(int idpagoomitidofg)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdPagoOmitidoFG", SqlDbType.Int);
			param0.Value = idpagoomitidofg;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelGlobalPagosNoEnviadosReg";
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

		#region Deshacer VBFac_GlobalPagosNoEnviados_Reg
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdPagoOmitidoFG);
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
		public List<ENTGlobalpagosnoenviadosReg> RecuperarTodo()
		{
			List<ENTGlobalpagosnoenviadosReg> result = new List<ENTGlobalpagosnoenviadosReg>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetGlobalPagosNoEnviadosReg_TODO";
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
				    ENTGlobalpagosnoenviadosReg item = new ENTGlobalpagosnoenviadosReg();
					 if (!dr.IsNull("IdPagoOmitidoFG")) item.IdPagoOmitidoFG = Convert.ToInt32(dr["IdPagoOmitidoFG"]);
					 if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					 if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					 if (!dr.IsNull("RecordLocator")) item.RecordLocator = dr["RecordLocator"].ToString();
					 if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					 if (!dr.IsNull("CodigoMoneda")) item.CodigoMoneda = dr["CodigoMoneda"].ToString();
					 if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					 if (!dr.IsNull("FechaEnvioFG")) item.FechaEnvioFG = Convert.ToDateTime(dr["FechaEnvioFG"]);
					 if (!dr.IsNull("MotivoOmision")) item.MotivoOmision = dr["MotivoOmision"].ToString();
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

		#region Recuperar RecuperarGlobalpagosnoenviadosRegPorLlavePrimaria
		public List<ENTGlobalpagosnoenviadosReg> RecuperarGlobalpagosnoenviadosRegPorLlavePrimaria(int idpagoomitidofg)
		{
			List<ENTGlobalpagosnoenviadosReg> result = new List<ENTGlobalpagosnoenviadosReg>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdPagoOmitidoFG", SqlDbType.Int);
			param0.Value = idpagoomitidofg;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetGlobalPagosNoEnviadosReg_POR_PK";
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
				    ENTGlobalpagosnoenviadosReg item = new ENTGlobalpagosnoenviadosReg();
					if (!dr.IsNull("IdPagoOmitidoFG")) item.IdPagoOmitidoFG = Convert.ToInt32(dr["IdPagoOmitidoFG"]);
					if (!dr.IsNull("BookingID")) item.BookingID = Convert.ToInt64(dr["BookingID"]);
					if (!dr.IsNull("PaymentID")) item.PaymentID = Convert.ToInt64(dr["PaymentID"]);
					if (!dr.IsNull("RecordLocator")) item.RecordLocator = dr["RecordLocator"].ToString();
					if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					if (!dr.IsNull("CodigoMoneda")) item.CodigoMoneda = dr["CodigoMoneda"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("FechaEnvioFG")) item.FechaEnvioFG = Convert.ToDateTime(dr["FechaEnvioFG"]);
					if (!dr.IsNull("MotivoOmision")) item.MotivoOmision = dr["MotivoOmision"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdPagoOmitidoFG")) IdPagoOmitidoFG = Convert.ToInt32(dtResultado.Rows[0]["IdPagoOmitidoFG"]);
					if (!dtResultado.Rows[0].IsNull("BookingID")) BookingID = Convert.ToInt64(dtResultado.Rows[0]["BookingID"]);
					if (!dtResultado.Rows[0].IsNull("PaymentID")) PaymentID = Convert.ToInt64(dtResultado.Rows[0]["PaymentID"]);
					if (!dtResultado.Rows[0].IsNull("RecordLocator")) RecordLocator = dtResultado.Rows[0]["RecordLocator"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaPago")) FechaPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaPago"]);
					if (!dtResultado.Rows[0].IsNull("CodigoMoneda")) CodigoMoneda = dtResultado.Rows[0]["CodigoMoneda"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaEnvioFG")) FechaEnvioFG = Convert.ToDateTime(dtResultado.Rows[0]["FechaEnvioFG"]);
					if (!dtResultado.Rows[0].IsNull("MotivoOmision")) MotivoOmision = dtResultado.Rows[0]["MotivoOmision"].ToString();
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
			_copia = new DALGlobalpagosnoenviadosReg(_conexion);
			Type tipo = typeof(ENTGlobalpagosnoenviadosReg);
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

using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALFacturascomplpagoReg: ENTFacturascomplpagoReg
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALFacturascomplpagoReg _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALFacturascomplpagoReg(SqlConnection conexion)
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
			IdFacturaComplPago = 0;
			IdPagosCab = 0;
			IdFacturaCab = 0;
			FechaPago = new DateTime();
			FormaDePagoP = String.Empty;
			MonedaP = String.Empty;
			Monto = 0M;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_FacturasComplPago_Reg
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaComplPago", SqlDbType.BigInt);
			param0.Value = IdFacturaComplPago;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param1.Value = IdPagosCab;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param2.Value = IdFacturaCab;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@FechaPago", SqlDbType.VarChar);
			param3.Value = FechaPago.Year > 1900 ? FechaPago.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@FormaDePagoP", SqlDbType.VarChar);
			param4.Value = FormaDePagoP;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@MonedaP", SqlDbType.VarChar);
			param5.Value = MonedaP;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Monto", SqlDbType.Money);
			param6.Value = Monto;
			commandParameters.Add(param6);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsFacturasComplPagoReg";
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
						IdFacturaComplPago = Convert.ToInt64(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_FacturasComplPago_Reg
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaComplPago", SqlDbType.BigInt);
			param0.Value = IdFacturaComplPago;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param1.Value = IdPagosCab;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param2.Value = IdFacturaCab;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@FechaPago", SqlDbType.VarChar);
			param3.Value = FechaPago.Year > 1900 ? FechaPago.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@FormaDePagoP", SqlDbType.VarChar);
			param4.Value = FormaDePagoP;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@MonedaP", SqlDbType.VarChar);
			param5.Value = MonedaP;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Monto", SqlDbType.Money);
			param6.Value = Monto;
			commandParameters.Add(param6);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdFacturasComplPagoReg";
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

		#region Eliminar VBFac_FacturasComplPago_Reg
		public void Eliminar(long idfacturacomplpago)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaComplPago", SqlDbType.BigInt);
			param0.Value = idfacturacomplpago;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelFacturasComplPagoReg";
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

		#region Deshacer VBFac_FacturasComplPago_Reg
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdFacturaComplPago);
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
		public List<ENTFacturascomplpagoReg> RecuperarTodo()
		{
			List<ENTFacturascomplpagoReg> result = new List<ENTFacturascomplpagoReg>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasComplPagoReg_TODO";
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
				    ENTFacturascomplpagoReg item = new ENTFacturascomplpagoReg();
					 if (!dr.IsNull("IdFacturaComplPago")) item.IdFacturaComplPago = Convert.ToInt64(dr["IdFacturaComplPago"]);
					 if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					 if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					 if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					 if (!dr.IsNull("FormaDePagoP")) item.FormaDePagoP = dr["FormaDePagoP"].ToString();
					 if (!dr.IsNull("MonedaP")) item.MonedaP = dr["MonedaP"].ToString();
					 if (!dr.IsNull("Monto")) item.Monto = Convert.ToDecimal(dr["Monto"]);
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

		#region Recuperar RecuperarFacturascomplpagoRegPorLlavePrimaria
		public List<ENTFacturascomplpagoReg> RecuperarFacturascomplpagoRegPorLlavePrimaria(long idfacturacomplpago)
		{
			List<ENTFacturascomplpagoReg> result = new List<ENTFacturascomplpagoReg>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFacturaComplPago", SqlDbType.BigInt);
			param0.Value = idfacturacomplpago;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasComplPagoReg_POR_PK";
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
				    ENTFacturascomplpagoReg item = new ENTFacturascomplpagoReg();
					if (!dr.IsNull("IdFacturaComplPago")) item.IdFacturaComplPago = Convert.ToInt64(dr["IdFacturaComplPago"]);
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					if (!dr.IsNull("FormaDePagoP")) item.FormaDePagoP = dr["FormaDePagoP"].ToString();
					if (!dr.IsNull("MonedaP")) item.MonedaP = dr["MonedaP"].ToString();
					if (!dr.IsNull("Monto")) item.Monto = Convert.ToDecimal(dr["Monto"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaComplPago")) IdFacturaComplPago = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaComplPago"]);
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("FechaPago")) FechaPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaPago"]);
					if (!dtResultado.Rows[0].IsNull("FormaDePagoP")) FormaDePagoP = dtResultado.Rows[0]["FormaDePagoP"].ToString();
					if (!dtResultado.Rows[0].IsNull("MonedaP")) MonedaP = dtResultado.Rows[0]["MonedaP"].ToString();
					if (!dtResultado.Rows[0].IsNull("Monto")) Monto = Convert.ToDecimal(dtResultado.Rows[0]["Monto"]);
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

		#region Recuperar RecuperarFacturascomplpagoRegIdpagoscab
		public List<ENTFacturascomplpagoReg> RecuperarFacturascomplpagoRegIdpagoscab(long idpagoscab)
		{
			List<ENTFacturascomplpagoReg> result = new List<ENTFacturascomplpagoReg>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = idpagoscab;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasComplPagoReg_POR_IdPagosCab";
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
				    ENTFacturascomplpagoReg item = new ENTFacturascomplpagoReg();
					if (!dr.IsNull("IdFacturaComplPago")) item.IdFacturaComplPago = Convert.ToInt64(dr["IdFacturaComplPago"]);
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					if (!dr.IsNull("FormaDePagoP")) item.FormaDePagoP = dr["FormaDePagoP"].ToString();
					if (!dr.IsNull("MonedaP")) item.MonedaP = dr["MonedaP"].ToString();
					if (!dr.IsNull("Monto")) item.Monto = Convert.ToDecimal(dr["Monto"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaComplPago")) IdFacturaComplPago = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaComplPago"]);
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("FechaPago")) FechaPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaPago"]);
					if (!dtResultado.Rows[0].IsNull("FormaDePagoP")) FormaDePagoP = dtResultado.Rows[0]["FormaDePagoP"].ToString();
					if (!dtResultado.Rows[0].IsNull("MonedaP")) MonedaP = dtResultado.Rows[0]["MonedaP"].ToString();
					if (!dtResultado.Rows[0].IsNull("Monto")) Monto = Convert.ToDecimal(dtResultado.Rows[0]["Monto"]);
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

		#region Recuperar RecuperarFacturascomplpagoRegIdfacturacab
		public List<ENTFacturascomplpagoReg> RecuperarFacturascomplpagoRegIdfacturacab(long idfacturacab)
		{
			List<ENTFacturascomplpagoReg> result = new List<ENTFacturascomplpagoReg>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = idfacturacab;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasComplPagoReg_POR_IdFacturaCab";
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
				    ENTFacturascomplpagoReg item = new ENTFacturascomplpagoReg();
					if (!dr.IsNull("IdFacturaComplPago")) item.IdFacturaComplPago = Convert.ToInt64(dr["IdFacturaComplPago"]);
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("FechaPago")) item.FechaPago = Convert.ToDateTime(dr["FechaPago"]);
					if (!dr.IsNull("FormaDePagoP")) item.FormaDePagoP = dr["FormaDePagoP"].ToString();
					if (!dr.IsNull("MonedaP")) item.MonedaP = dr["MonedaP"].ToString();
					if (!dr.IsNull("Monto")) item.Monto = Convert.ToDecimal(dr["Monto"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaComplPago")) IdFacturaComplPago = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaComplPago"]);
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("FechaPago")) FechaPago = Convert.ToDateTime(dtResultado.Rows[0]["FechaPago"]);
					if (!dtResultado.Rows[0].IsNull("FormaDePagoP")) FormaDePagoP = dtResultado.Rows[0]["FormaDePagoP"].ToString();
					if (!dtResultado.Rows[0].IsNull("MonedaP")) MonedaP = dtResultado.Rows[0]["MonedaP"].ToString();
					if (!dtResultado.Rows[0].IsNull("Monto")) Monto = Convert.ToDecimal(dtResultado.Rows[0]["Monto"]);
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
			_copia = new DALFacturascomplpagoReg(_conexion);
			Type tipo = typeof(ENTFacturascomplpagoReg);
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

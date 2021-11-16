using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALFacturasivaDet: ENTFacturasivaDet
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALFacturasivaDet _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALFacturasivaDet(SqlConnection conexion)
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
			TipoFactor = String.Empty;
			TasaOCuota = 0M;
			Base = 0M;
			Impuesto = String.Empty;
			Importe = 0M;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_FacturasIVA_Det
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
			SqlParameter param2 = new SqlParameter("@TipoFactor", SqlDbType.VarChar);
			param2.Value = TipoFactor;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@TasaOCuota", SqlDbType.Decimal);
			param3.Value = TasaOCuota;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Base", SqlDbType.Money);
			param4.Value = Base;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Impuesto", SqlDbType.VarChar);
			param5.Value = Impuesto;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Importe", SqlDbType.Money);
			param6.Value = Importe;
			commandParameters.Add(param6);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsFacturasIVADet";
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

		#region Actualizar VBFac_FacturasIVA_Det
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
			SqlParameter param2 = new SqlParameter("@TipoFactor", SqlDbType.VarChar);
			param2.Value = TipoFactor;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@TasaOCuota", SqlDbType.Decimal);
			param3.Value = TasaOCuota;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Base", SqlDbType.Money);
			param4.Value = Base;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Impuesto", SqlDbType.VarChar);
			param5.Value = Impuesto;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Importe", SqlDbType.Money);
			param6.Value = Importe;
			commandParameters.Add(param6);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdFacturasIVADet";
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

		#region Eliminar VBFac_FacturasIVA_Det
		public void Eliminar(long idfacturacab,int idfacturadet,decimal tasaocuota,string tipofactor)
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
			SqlParameter param2 = new SqlParameter("@TasaOCuota", SqlDbType.Decimal);
			param2.Value = tasaocuota;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@TipoFactor", SqlDbType.VarChar);
			param3.Value = tipofactor;
			commandParameters.Add(param3);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelFacturasIVADet";
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

		#region Deshacer VBFac_FacturasIVA_Det
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdFacturaCab,IdFacturaDet,TasaOCuota,TipoFactor);
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
		public List<ENTFacturasivaDet> RecuperarTodo()
		{
			List<ENTFacturasivaDet> result = new List<ENTFacturasivaDet>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasIVADet_TODO";
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
				    ENTFacturasivaDet item = new ENTFacturasivaDet();
					 if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					 if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					 if (!dr.IsNull("TipoFactor")) item.TipoFactor = dr["TipoFactor"].ToString();
					 if (!dr.IsNull("TasaOCuota")) item.TasaOCuota = Convert.ToDecimal(dr["TasaOCuota"]);
					 if (!dr.IsNull("Base")) item.Base = Convert.ToDecimal(dr["Base"]);
					 if (!dr.IsNull("Impuesto")) item.Impuesto = dr["Impuesto"].ToString();
					 if (!dr.IsNull("Importe")) item.Importe = Convert.ToDecimal(dr["Importe"]);
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

		#region Recuperar RecuperarFacturasivaDetPorLlavePrimaria
		public List<ENTFacturasivaDet> RecuperarFacturasivaDetPorLlavePrimaria(long idfacturacab,int idfacturadet,string tipofactor,decimal tasaocuota)
		{
			List<ENTFacturasivaDet> result = new List<ENTFacturasivaDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = idfacturacab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdFacturaDet", SqlDbType.Int);
			param1.Value = idfacturadet;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@TipoFactor", SqlDbType.VarChar);
			param2.Value = tipofactor;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@TasaOCuota", SqlDbType.Decimal);
			param3.Value = tasaocuota;
			commandParameters.Add(param3);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasIVADet_POR_PK";
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
				    ENTFacturasivaDet item = new ENTFacturasivaDet();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					if (!dr.IsNull("TipoFactor")) item.TipoFactor = dr["TipoFactor"].ToString();
					if (!dr.IsNull("TasaOCuota")) item.TasaOCuota = Convert.ToDecimal(dr["TasaOCuota"]);
					if (!dr.IsNull("Base")) item.Base = Convert.ToDecimal(dr["Base"]);
					if (!dr.IsNull("Impuesto")) item.Impuesto = dr["Impuesto"].ToString();
					if (!dr.IsNull("Importe")) item.Importe = Convert.ToDecimal(dr["Importe"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaDet")) IdFacturaDet = Convert.ToInt32(dtResultado.Rows[0]["IdFacturaDet"]);
					if (!dtResultado.Rows[0].IsNull("TipoFactor")) TipoFactor = dtResultado.Rows[0]["TipoFactor"].ToString();
					if (!dtResultado.Rows[0].IsNull("TasaOCuota")) TasaOCuota = Convert.ToDecimal(dtResultado.Rows[0]["TasaOCuota"]);
					if (!dtResultado.Rows[0].IsNull("Base")) Base = Convert.ToDecimal(dtResultado.Rows[0]["Base"]);
					if (!dtResultado.Rows[0].IsNull("Impuesto")) Impuesto = dtResultado.Rows[0]["Impuesto"].ToString();
					if (!dtResultado.Rows[0].IsNull("Importe")) Importe = Convert.ToDecimal(dtResultado.Rows[0]["Importe"]);
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

		#region Recuperar RecuperarFacturasivaDetIdfacturacabIdfacturadet
		public List<ENTFacturasivaDet> RecuperarFacturasivaDetIdfacturacabIdfacturadet(long idfacturacab,int idfacturadet)
		{
			List<ENTFacturasivaDet> result = new List<ENTFacturasivaDet>();
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
			cmm.CommandText = "uspFac_GetFacturasIVADet_POR_IdFacturaCab_IdFacturaDet";
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
				    ENTFacturasivaDet item = new ENTFacturasivaDet();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					if (!dr.IsNull("TipoFactor")) item.TipoFactor = dr["TipoFactor"].ToString();
					if (!dr.IsNull("TasaOCuota")) item.TasaOCuota = Convert.ToDecimal(dr["TasaOCuota"]);
					if (!dr.IsNull("Base")) item.Base = Convert.ToDecimal(dr["Base"]);
					if (!dr.IsNull("Impuesto")) item.Impuesto = dr["Impuesto"].ToString();
					if (!dr.IsNull("Importe")) item.Importe = Convert.ToDecimal(dr["Importe"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaDet")) IdFacturaDet = Convert.ToInt32(dtResultado.Rows[0]["IdFacturaDet"]);
					if (!dtResultado.Rows[0].IsNull("TipoFactor")) TipoFactor = dtResultado.Rows[0]["TipoFactor"].ToString();
					if (!dtResultado.Rows[0].IsNull("TasaOCuota")) TasaOCuota = Convert.ToDecimal(dtResultado.Rows[0]["TasaOCuota"]);
					if (!dtResultado.Rows[0].IsNull("Base")) Base = Convert.ToDecimal(dtResultado.Rows[0]["Base"]);
					if (!dtResultado.Rows[0].IsNull("Impuesto")) Impuesto = dtResultado.Rows[0]["Impuesto"].ToString();
					if (!dtResultado.Rows[0].IsNull("Importe")) Importe = Convert.ToDecimal(dtResultado.Rows[0]["Importe"]);
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
			_copia = new DALFacturasivaDet(_conexion);
			Type tipo = typeof(ENTFacturasivaDet);
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

using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALNotascreditoivaDet: ENTNotascreditoivaDet
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALNotascreditoivaDet _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALNotascreditoivaDet(SqlConnection conexion)
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
			IdNotaCreditoCab = 0;
			IdNotaCreditoDet = 0;
			TipoFactor = String.Empty;
			Base = 0M;
			Impuesto = String.Empty;
			TasaOCuota = 0M;
			Importe = 0M;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_NotasCreditoIVA_Det
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdNotaCreditoCab", SqlDbType.BigInt);
			param0.Value = IdNotaCreditoCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdNotaCreditoDet", SqlDbType.Int);
			param1.Value = IdNotaCreditoDet;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@TipoFactor", SqlDbType.VarChar);
			param2.Value = TipoFactor;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Base", SqlDbType.Money);
			param3.Value = Base;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Impuesto", SqlDbType.VarChar);
			param4.Value = Impuesto;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@TasaOCuota", SqlDbType.Decimal);
			param5.Value = TasaOCuota;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Importe", SqlDbType.Money);
			param6.Value = Importe;
			commandParameters.Add(param6);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsNotasCreditoIVADet";
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

		#region Actualizar VBFac_NotasCreditoIVA_Det
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdNotaCreditoCab", SqlDbType.BigInt);
			param0.Value = IdNotaCreditoCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdNotaCreditoDet", SqlDbType.Int);
			param1.Value = IdNotaCreditoDet;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@TipoFactor", SqlDbType.VarChar);
			param2.Value = TipoFactor;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Base", SqlDbType.Money);
			param3.Value = Base;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Impuesto", SqlDbType.VarChar);
			param4.Value = Impuesto;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@TasaOCuota", SqlDbType.Decimal);
			param5.Value = TasaOCuota;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Importe", SqlDbType.Money);
			param6.Value = Importe;
			commandParameters.Add(param6);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdNotasCreditoIVADet";
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

		#region Eliminar VBFac_NotasCreditoIVA_Det
		public void Eliminar(long idnotacreditocab,int idnotacreditodet)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdNotaCreditoCab", SqlDbType.BigInt);
			param0.Value = idnotacreditocab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdNotaCreditoDet", SqlDbType.Int);
			param1.Value = idnotacreditodet;
			commandParameters.Add(param1);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelNotasCreditoIVADet";
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

		#region Deshacer VBFac_NotasCreditoIVA_Det
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdNotaCreditoCab,IdNotaCreditoDet);
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
		public List<ENTNotascreditoivaDet> RecuperarTodo()
		{
			List<ENTNotascreditoivaDet> result = new List<ENTNotascreditoivaDet>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetNotasCreditoIVADet_TODO";
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
				    ENTNotascreditoivaDet item = new ENTNotascreditoivaDet();
					 if (!dr.IsNull("IdNotaCreditoCab")) item.IdNotaCreditoCab = Convert.ToInt64(dr["IdNotaCreditoCab"]);
					 if (!dr.IsNull("IdNotaCreditoDet")) item.IdNotaCreditoDet = Convert.ToInt32(dr["IdNotaCreditoDet"]);
					 if (!dr.IsNull("TipoFactor")) item.TipoFactor = dr["TipoFactor"].ToString();
					 if (!dr.IsNull("Base")) item.Base = Convert.ToDecimal(dr["Base"]);
					 if (!dr.IsNull("Impuesto")) item.Impuesto = dr["Impuesto"].ToString();
					 if (!dr.IsNull("TasaOCuota")) item.TasaOCuota = Convert.ToDecimal(dr["TasaOCuota"]);
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

		#region Recuperar RecuperarNotascreditoivaDetPorLlavePrimaria
		public List<ENTNotascreditoivaDet> RecuperarNotascreditoivaDetPorLlavePrimaria(long idnotacreditocab,int idnotacreditodet)
		{
			List<ENTNotascreditoivaDet> result = new List<ENTNotascreditoivaDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdNotaCreditoCab", SqlDbType.BigInt);
			param0.Value = idnotacreditocab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdNotaCreditoDet", SqlDbType.Int);
			param1.Value = idnotacreditodet;
			commandParameters.Add(param1);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetNotasCreditoIVADet_POR_PK";
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
				    ENTNotascreditoivaDet item = new ENTNotascreditoivaDet();
					if (!dr.IsNull("IdNotaCreditoCab")) item.IdNotaCreditoCab = Convert.ToInt64(dr["IdNotaCreditoCab"]);
					if (!dr.IsNull("IdNotaCreditoDet")) item.IdNotaCreditoDet = Convert.ToInt32(dr["IdNotaCreditoDet"]);
					if (!dr.IsNull("TipoFactor")) item.TipoFactor = dr["TipoFactor"].ToString();
					if (!dr.IsNull("Base")) item.Base = Convert.ToDecimal(dr["Base"]);
					if (!dr.IsNull("Impuesto")) item.Impuesto = dr["Impuesto"].ToString();
					if (!dr.IsNull("TasaOCuota")) item.TasaOCuota = Convert.ToDecimal(dr["TasaOCuota"]);
					if (!dr.IsNull("Importe")) item.Importe = Convert.ToDecimal(dr["Importe"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdNotaCreditoCab")) IdNotaCreditoCab = Convert.ToInt64(dtResultado.Rows[0]["IdNotaCreditoCab"]);
					if (!dtResultado.Rows[0].IsNull("IdNotaCreditoDet")) IdNotaCreditoDet = Convert.ToInt32(dtResultado.Rows[0]["IdNotaCreditoDet"]);
					if (!dtResultado.Rows[0].IsNull("TipoFactor")) TipoFactor = dtResultado.Rows[0]["TipoFactor"].ToString();
					if (!dtResultado.Rows[0].IsNull("Base")) Base = Convert.ToDecimal(dtResultado.Rows[0]["Base"]);
					if (!dtResultado.Rows[0].IsNull("Impuesto")) Impuesto = dtResultado.Rows[0]["Impuesto"].ToString();
					if (!dtResultado.Rows[0].IsNull("TasaOCuota")) TasaOCuota = Convert.ToDecimal(dtResultado.Rows[0]["TasaOCuota"]);
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

		#region Recuperar RecuperarNotascreditoivaDetIdnotacreditocabIdnotacreditodet
		public List<ENTNotascreditoivaDet> RecuperarNotascreditoivaDetIdnotacreditocabIdnotacreditodet(long idnotacreditocab,int idnotacreditodet)
		{
			List<ENTNotascreditoivaDet> result = new List<ENTNotascreditoivaDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdNotaCreditoCab", SqlDbType.BigInt);
			param0.Value = idnotacreditocab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdNotaCreditoDet", SqlDbType.Int);
			param1.Value = idnotacreditodet;
			commandParameters.Add(param1);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetNotasCreditoIVADet_POR_IdNotaCreditoCab_IdNotaCreditoDet";
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
				    ENTNotascreditoivaDet item = new ENTNotascreditoivaDet();
					if (!dr.IsNull("IdNotaCreditoCab")) item.IdNotaCreditoCab = Convert.ToInt64(dr["IdNotaCreditoCab"]);
					if (!dr.IsNull("IdNotaCreditoDet")) item.IdNotaCreditoDet = Convert.ToInt32(dr["IdNotaCreditoDet"]);
					if (!dr.IsNull("TipoFactor")) item.TipoFactor = dr["TipoFactor"].ToString();
					if (!dr.IsNull("Base")) item.Base = Convert.ToDecimal(dr["Base"]);
					if (!dr.IsNull("Impuesto")) item.Impuesto = dr["Impuesto"].ToString();
					if (!dr.IsNull("TasaOCuota")) item.TasaOCuota = Convert.ToDecimal(dr["TasaOCuota"]);
					if (!dr.IsNull("Importe")) item.Importe = Convert.ToDecimal(dr["Importe"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdNotaCreditoCab")) IdNotaCreditoCab = Convert.ToInt64(dtResultado.Rows[0]["IdNotaCreditoCab"]);
					if (!dtResultado.Rows[0].IsNull("IdNotaCreditoDet")) IdNotaCreditoDet = Convert.ToInt32(dtResultado.Rows[0]["IdNotaCreditoDet"]);
					if (!dtResultado.Rows[0].IsNull("TipoFactor")) TipoFactor = dtResultado.Rows[0]["TipoFactor"].ToString();
					if (!dtResultado.Rows[0].IsNull("Base")) Base = Convert.ToDecimal(dtResultado.Rows[0]["Base"]);
					if (!dtResultado.Rows[0].IsNull("Impuesto")) Impuesto = dtResultado.Rows[0]["Impuesto"].ToString();
					if (!dtResultado.Rows[0].IsNull("TasaOCuota")) TasaOCuota = Convert.ToDecimal(dtResultado.Rows[0]["TasaOCuota"]);
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
			_copia = new DALNotascreditoivaDet(_conexion);
			Type tipo = typeof(ENTNotascreditoivaDet);
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

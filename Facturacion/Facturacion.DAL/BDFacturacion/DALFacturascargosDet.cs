using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALFacturascargosDet: ENTFacturascargosDet
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALFacturascargosDet _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALFacturascargosDet(SqlConnection conexion)
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
			CodigoCargo = String.Empty;
			Importe = 0M;
			EsTua = false;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_FacturasCargos_Det
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
			SqlParameter param2 = new SqlParameter("@CodigoCargo", SqlDbType.VarChar);
			param2.Value = CodigoCargo;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Importe", SqlDbType.Money);
			param3.Value = Importe;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@EsTua", SqlDbType.Bit);
			param4.Value = EsTua;
			commandParameters.Add(param4);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsFacturasCargosDet";
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

		#region Actualizar VBFac_FacturasCargos_Det
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
			SqlParameter param2 = new SqlParameter("@CodigoCargo", SqlDbType.VarChar);
			param2.Value = CodigoCargo;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Importe", SqlDbType.Money);
			param3.Value = Importe;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@EsTua", SqlDbType.Bit);
			param4.Value = EsTua;
			commandParameters.Add(param4);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdFacturasCargosDet";
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

		#region Eliminar VBFac_FacturasCargos_Det
		public void Eliminar(string codigocargo,long idfacturacab,int idfacturadet)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@CodigoCargo", SqlDbType.VarChar);
			param0.Value = codigocargo;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param1.Value = idfacturacab;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@IdFacturaDet", SqlDbType.Int);
			param2.Value = idfacturadet;
			commandParameters.Add(param2);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelFacturasCargosDet";
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

		#region Deshacer VBFac_FacturasCargos_Det
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(CodigoCargo,IdFacturaCab,IdFacturaDet);
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
		public List<ENTFacturascargosDet> RecuperarTodo()
		{
			List<ENTFacturascargosDet> result = new List<ENTFacturascargosDet>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasCargosDet_TODO";
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
				    ENTFacturascargosDet item = new ENTFacturascargosDet();
					 if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					 if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					 if (!dr.IsNull("CodigoCargo")) item.CodigoCargo = dr["CodigoCargo"].ToString();
					 if (!dr.IsNull("Importe")) item.Importe = Convert.ToDecimal(dr["Importe"]);
					 if (!dr.IsNull("EsTua")) item.EsTua = Convert.ToBoolean(dr["EsTua"]);
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

		#region Recuperar RecuperarFacturascargosDetPorLlavePrimaria
		public List<ENTFacturascargosDet> RecuperarFacturascargosDetPorLlavePrimaria(long idfacturacab,int idfacturadet,string codigocargo)
		{
			List<ENTFacturascargosDet> result = new List<ENTFacturascargosDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = idfacturacab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdFacturaDet", SqlDbType.Int);
			param1.Value = idfacturadet;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@CodigoCargo", SqlDbType.VarChar);
			param2.Value = codigocargo;
			commandParameters.Add(param2);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasCargosDet_POR_PK";
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
				    ENTFacturascargosDet item = new ENTFacturascargosDet();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					if (!dr.IsNull("CodigoCargo")) item.CodigoCargo = dr["CodigoCargo"].ToString();
					if (!dr.IsNull("Importe")) item.Importe = Convert.ToDecimal(dr["Importe"]);
					if (!dr.IsNull("EsTua")) item.EsTua = Convert.ToBoolean(dr["EsTua"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaDet")) IdFacturaDet = Convert.ToInt32(dtResultado.Rows[0]["IdFacturaDet"]);
					if (!dtResultado.Rows[0].IsNull("CodigoCargo")) CodigoCargo = dtResultado.Rows[0]["CodigoCargo"].ToString();
					if (!dtResultado.Rows[0].IsNull("Importe")) Importe = Convert.ToDecimal(dtResultado.Rows[0]["Importe"]);
					if (!dtResultado.Rows[0].IsNull("EsTua")) EsTua = Convert.ToBoolean(dtResultado.Rows[0]["EsTua"]);
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

		#region Recuperar RecuperarFacturascargosDetIdfacturacabIdfacturadet
		public List<ENTFacturascargosDet> RecuperarFacturascargosDetIdfacturacabIdfacturadet(long idfacturacab,int idfacturadet)
		{
			List<ENTFacturascargosDet> result = new List<ENTFacturascargosDet>();
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
			cmm.CommandText = "uspFac_GetFacturasCargosDet_POR_IdFacturaCab_IdFacturaDet";
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
				    ENTFacturascargosDet item = new ENTFacturascargosDet();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					if (!dr.IsNull("CodigoCargo")) item.CodigoCargo = dr["CodigoCargo"].ToString();
					if (!dr.IsNull("Importe")) item.Importe = Convert.ToDecimal(dr["Importe"]);
					if (!dr.IsNull("EsTua")) item.EsTua = Convert.ToBoolean(dr["EsTua"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaDet")) IdFacturaDet = Convert.ToInt32(dtResultado.Rows[0]["IdFacturaDet"]);
					if (!dtResultado.Rows[0].IsNull("CodigoCargo")) CodigoCargo = dtResultado.Rows[0]["CodigoCargo"].ToString();
					if (!dtResultado.Rows[0].IsNull("Importe")) Importe = Convert.ToDecimal(dtResultado.Rows[0]["Importe"]);
					if (!dtResultado.Rows[0].IsNull("EsTua")) EsTua = Convert.ToBoolean(dtResultado.Rows[0]["EsTua"]);
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
			_copia = new DALFacturascargosDet(_conexion);
			Type tipo = typeof(ENTFacturascargosDet);
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

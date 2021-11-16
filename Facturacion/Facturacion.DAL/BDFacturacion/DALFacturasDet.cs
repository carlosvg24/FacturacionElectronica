using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALFacturasDet: ENTFacturasDet
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALFacturasDet _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALFacturasDet(SqlConnection conexion)
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
			ClaveProdServ = String.Empty;
			NoIdentificacion = String.Empty;
			Cantidad = 0;
			ClaveUnidad = String.Empty;
			Unidad = String.Empty;
			Descripcion = String.Empty;
			ValorUnitario = 0M;
			Importe = 0M;
			Descuento = 0M;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_Facturas_Det
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
			SqlParameter param2 = new SqlParameter("@ClaveProdServ", SqlDbType.VarChar);
			param2.Value = ClaveProdServ;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@NoIdentificacion", SqlDbType.VarChar);
			param3.Value = NoIdentificacion;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Cantidad", SqlDbType.Int);
			param4.Value = Cantidad;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@ClaveUnidad", SqlDbType.VarChar);
			param5.Value = ClaveUnidad;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Unidad", SqlDbType.VarChar);
			param6.Value = Unidad;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param7.Value = Descripcion;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@ValorUnitario", SqlDbType.Money);
			param8.Value = ValorUnitario;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@Importe", SqlDbType.Money);
			param9.Value = Importe;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@Descuento", SqlDbType.Money);
			param10.Value = Descuento;
			commandParameters.Add(param10);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsFacturasDet";
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

		#region Actualizar VBFac_Facturas_Det
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
			SqlParameter param2 = new SqlParameter("@ClaveProdServ", SqlDbType.VarChar);
			param2.Value = ClaveProdServ;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@NoIdentificacion", SqlDbType.VarChar);
			param3.Value = NoIdentificacion;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Cantidad", SqlDbType.Int);
			param4.Value = Cantidad;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@ClaveUnidad", SqlDbType.VarChar);
			param5.Value = ClaveUnidad;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Unidad", SqlDbType.VarChar);
			param6.Value = Unidad;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param7.Value = Descripcion;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@ValorUnitario", SqlDbType.Money);
			param8.Value = ValorUnitario;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@Importe", SqlDbType.Money);
			param9.Value = Importe;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@Descuento", SqlDbType.Money);
			param10.Value = Descuento;
			commandParameters.Add(param10);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdFacturasDet";
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

		#region Eliminar VBFac_Facturas_Det
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
			cmm.CommandText = "uspFac_DelFacturasDet";
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

		#region Deshacer VBFac_Facturas_Det
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
		public List<ENTFacturasDet> RecuperarTodo()
		{
			List<ENTFacturasDet> result = new List<ENTFacturasDet>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasDet_TODO";
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
				    ENTFacturasDet item = new ENTFacturasDet();
					 if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					 if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					 if (!dr.IsNull("ClaveProdServ")) item.ClaveProdServ = dr["ClaveProdServ"].ToString();
					 if (!dr.IsNull("NoIdentificacion")) item.NoIdentificacion = dr["NoIdentificacion"].ToString();
					 if (!dr.IsNull("Cantidad")) item.Cantidad = Convert.ToInt32(dr["Cantidad"]);
					 if (!dr.IsNull("ClaveUnidad")) item.ClaveUnidad = dr["ClaveUnidad"].ToString();
					 if (!dr.IsNull("Unidad")) item.Unidad = dr["Unidad"].ToString();
					 if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					 if (!dr.IsNull("ValorUnitario")) item.ValorUnitario = Convert.ToDecimal(dr["ValorUnitario"]);
					 if (!dr.IsNull("Importe")) item.Importe = Convert.ToDecimal(dr["Importe"]);
					 if (!dr.IsNull("Descuento")) item.Descuento = Convert.ToDecimal(dr["Descuento"]);
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

		#region Recuperar RecuperarFacturasDetPorLlavePrimaria
		public List<ENTFacturasDet> RecuperarFacturasDetPorLlavePrimaria(long idfacturacab,int idfacturadet)
		{
			List<ENTFacturasDet> result = new List<ENTFacturasDet>();
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
			cmm.CommandText = "uspFac_GetFacturasDet_POR_PK";
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
				    ENTFacturasDet item = new ENTFacturasDet();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					if (!dr.IsNull("ClaveProdServ")) item.ClaveProdServ = dr["ClaveProdServ"].ToString();
					if (!dr.IsNull("NoIdentificacion")) item.NoIdentificacion = dr["NoIdentificacion"].ToString();
					if (!dr.IsNull("Cantidad")) item.Cantidad = Convert.ToInt32(dr["Cantidad"]);
					if (!dr.IsNull("ClaveUnidad")) item.ClaveUnidad = dr["ClaveUnidad"].ToString();
					if (!dr.IsNull("Unidad")) item.Unidad = dr["Unidad"].ToString();
					if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					if (!dr.IsNull("ValorUnitario")) item.ValorUnitario = Convert.ToDecimal(dr["ValorUnitario"]);
					if (!dr.IsNull("Importe")) item.Importe = Convert.ToDecimal(dr["Importe"]);
					if (!dr.IsNull("Descuento")) item.Descuento = Convert.ToDecimal(dr["Descuento"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaDet")) IdFacturaDet = Convert.ToInt32(dtResultado.Rows[0]["IdFacturaDet"]);
					if (!dtResultado.Rows[0].IsNull("ClaveProdServ")) ClaveProdServ = dtResultado.Rows[0]["ClaveProdServ"].ToString();
					if (!dtResultado.Rows[0].IsNull("NoIdentificacion")) NoIdentificacion = dtResultado.Rows[0]["NoIdentificacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("Cantidad")) Cantidad = Convert.ToInt32(dtResultado.Rows[0]["Cantidad"]);
					if (!dtResultado.Rows[0].IsNull("ClaveUnidad")) ClaveUnidad = dtResultado.Rows[0]["ClaveUnidad"].ToString();
					if (!dtResultado.Rows[0].IsNull("Unidad")) Unidad = dtResultado.Rows[0]["Unidad"].ToString();
					if (!dtResultado.Rows[0].IsNull("Descripcion")) Descripcion = dtResultado.Rows[0]["Descripcion"].ToString();
					if (!dtResultado.Rows[0].IsNull("ValorUnitario")) ValorUnitario = Convert.ToDecimal(dtResultado.Rows[0]["ValorUnitario"]);
					if (!dtResultado.Rows[0].IsNull("Importe")) Importe = Convert.ToDecimal(dtResultado.Rows[0]["Importe"]);
					if (!dtResultado.Rows[0].IsNull("Descuento")) Descuento = Convert.ToDecimal(dtResultado.Rows[0]["Descuento"]);
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

		#region Recuperar RecuperarFacturasDetIdfacturacab
		public List<ENTFacturasDet> RecuperarFacturasDetIdfacturacab(long idfacturacab)
		{
			List<ENTFacturasDet> result = new List<ENTFacturasDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = idfacturacab;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasDet_POR_IdFacturaCab";
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
				    ENTFacturasDet item = new ENTFacturasDet();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					if (!dr.IsNull("ClaveProdServ")) item.ClaveProdServ = dr["ClaveProdServ"].ToString();
					if (!dr.IsNull("NoIdentificacion")) item.NoIdentificacion = dr["NoIdentificacion"].ToString();
					if (!dr.IsNull("Cantidad")) item.Cantidad = Convert.ToInt32(dr["Cantidad"]);
					if (!dr.IsNull("ClaveUnidad")) item.ClaveUnidad = dr["ClaveUnidad"].ToString();
					if (!dr.IsNull("Unidad")) item.Unidad = dr["Unidad"].ToString();
					if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					if (!dr.IsNull("ValorUnitario")) item.ValorUnitario = Convert.ToDecimal(dr["ValorUnitario"]);
					if (!dr.IsNull("Importe")) item.Importe = Convert.ToDecimal(dr["Importe"]);
					if (!dr.IsNull("Descuento")) item.Descuento = Convert.ToDecimal(dr["Descuento"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaDet")) IdFacturaDet = Convert.ToInt32(dtResultado.Rows[0]["IdFacturaDet"]);
					if (!dtResultado.Rows[0].IsNull("ClaveProdServ")) ClaveProdServ = dtResultado.Rows[0]["ClaveProdServ"].ToString();
					if (!dtResultado.Rows[0].IsNull("NoIdentificacion")) NoIdentificacion = dtResultado.Rows[0]["NoIdentificacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("Cantidad")) Cantidad = Convert.ToInt32(dtResultado.Rows[0]["Cantidad"]);
					if (!dtResultado.Rows[0].IsNull("ClaveUnidad")) ClaveUnidad = dtResultado.Rows[0]["ClaveUnidad"].ToString();
					if (!dtResultado.Rows[0].IsNull("Unidad")) Unidad = dtResultado.Rows[0]["Unidad"].ToString();
					if (!dtResultado.Rows[0].IsNull("Descripcion")) Descripcion = dtResultado.Rows[0]["Descripcion"].ToString();
					if (!dtResultado.Rows[0].IsNull("ValorUnitario")) ValorUnitario = Convert.ToDecimal(dtResultado.Rows[0]["ValorUnitario"]);
					if (!dtResultado.Rows[0].IsNull("Importe")) Importe = Convert.ToDecimal(dtResultado.Rows[0]["Importe"]);
					if (!dtResultado.Rows[0].IsNull("Descuento")) Descuento = Convert.ToDecimal(dtResultado.Rows[0]["Descuento"]);
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

		#region Recuperar RecuperarFacturasDetIdfacturacabIdfacturadet
		public List<ENTFacturasDet> RecuperarFacturasDetIdfacturacabIdfacturadet(long idfacturacab,int idfacturadet)
		{
			List<ENTFacturasDet> result = new List<ENTFacturasDet>();
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
			cmm.CommandText = "uspFac_GetFacturasDet_POR_IdFacturaCab_IdFacturaDet";
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
				    ENTFacturasDet item = new ENTFacturasDet();
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdFacturaDet")) item.IdFacturaDet = Convert.ToInt32(dr["IdFacturaDet"]);
					if (!dr.IsNull("ClaveProdServ")) item.ClaveProdServ = dr["ClaveProdServ"].ToString();
					if (!dr.IsNull("NoIdentificacion")) item.NoIdentificacion = dr["NoIdentificacion"].ToString();
					if (!dr.IsNull("Cantidad")) item.Cantidad = Convert.ToInt32(dr["Cantidad"]);
					if (!dr.IsNull("ClaveUnidad")) item.ClaveUnidad = dr["ClaveUnidad"].ToString();
					if (!dr.IsNull("Unidad")) item.Unidad = dr["Unidad"].ToString();
					if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					if (!dr.IsNull("ValorUnitario")) item.ValorUnitario = Convert.ToDecimal(dr["ValorUnitario"]);
					if (!dr.IsNull("Importe")) item.Importe = Convert.ToDecimal(dr["Importe"]);
					if (!dr.IsNull("Descuento")) item.Descuento = Convert.ToDecimal(dr["Descuento"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaDet")) IdFacturaDet = Convert.ToInt32(dtResultado.Rows[0]["IdFacturaDet"]);
					if (!dtResultado.Rows[0].IsNull("ClaveProdServ")) ClaveProdServ = dtResultado.Rows[0]["ClaveProdServ"].ToString();
					if (!dtResultado.Rows[0].IsNull("NoIdentificacion")) NoIdentificacion = dtResultado.Rows[0]["NoIdentificacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("Cantidad")) Cantidad = Convert.ToInt32(dtResultado.Rows[0]["Cantidad"]);
					if (!dtResultado.Rows[0].IsNull("ClaveUnidad")) ClaveUnidad = dtResultado.Rows[0]["ClaveUnidad"].ToString();
					if (!dtResultado.Rows[0].IsNull("Unidad")) Unidad = dtResultado.Rows[0]["Unidad"].ToString();
					if (!dtResultado.Rows[0].IsNull("Descripcion")) Descripcion = dtResultado.Rows[0]["Descripcion"].ToString();
					if (!dtResultado.Rows[0].IsNull("ValorUnitario")) ValorUnitario = Convert.ToDecimal(dtResultado.Rows[0]["ValorUnitario"]);
					if (!dtResultado.Rows[0].IsNull("Importe")) Importe = Convert.ToDecimal(dtResultado.Rows[0]["Importe"]);
					if (!dtResultado.Rows[0].IsNull("Descuento")) Descuento = Convert.ToDecimal(dtResultado.Rows[0]["Descuento"]);
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
			_copia = new DALFacturasDet(_conexion);
			Type tipo = typeof(ENTFacturasDet);
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

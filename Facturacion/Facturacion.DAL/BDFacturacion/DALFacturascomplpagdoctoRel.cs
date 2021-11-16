using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALFacturascomplpagdoctoRel: ENTFacturascomplpagdoctoRel
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALFacturascomplpagdoctoRel _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALFacturascomplpagdoctoRel(SqlConnection conexion)
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
			IdFacturaPPD = 0;
			IdDocumento = String.Empty;
			Serie = String.Empty;
			Folio = 0;
			MonedaDR = String.Empty;
			MetodoDePagoDR = String.Empty;
			NumParcialidad = 0;
			ImpSaldoAnt = 0M;
			ImpPagado = 0M;
			ImpSaldoInsoluto = 0M;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_FacturasComplPagDocto_Rel
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaComplPago", SqlDbType.BigInt);
			param0.Value = IdFacturaComplPago;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param1.Value = IdPagosCab;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@IdFacturaPPD", SqlDbType.BigInt);
			param2.Value = IdFacturaPPD;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@IdDocumento", SqlDbType.VarChar);
			param3.Value = IdDocumento;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Serie", SqlDbType.VarChar);
			param4.Value = Serie;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Folio", SqlDbType.BigInt);
			param5.Value = Folio;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@MonedaDR", SqlDbType.VarChar);
			param6.Value = MonedaDR;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@MetodoDePagoDR", SqlDbType.VarChar);
			param7.Value = MetodoDePagoDR;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@NumParcialidad", SqlDbType.TinyInt);
			param8.Value = NumParcialidad;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@ImpSaldoAnt", SqlDbType.Money);
			param9.Value = ImpSaldoAnt;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@ImpPagado", SqlDbType.Money);
			param10.Value = ImpPagado;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@ImpSaldoInsoluto", SqlDbType.Money);
			param11.Value = ImpSaldoInsoluto;
			commandParameters.Add(param11);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsFacturasComplPagDoctoRel";
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

		#region Actualizar VBFac_FacturasComplPagDocto_Rel
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
			SqlParameter param2 = new SqlParameter("@IdFacturaPPD", SqlDbType.BigInt);
			param2.Value = IdFacturaPPD;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@IdDocumento", SqlDbType.VarChar);
			param3.Value = IdDocumento;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Serie", SqlDbType.VarChar);
			param4.Value = Serie;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Folio", SqlDbType.BigInt);
			param5.Value = Folio;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@MonedaDR", SqlDbType.VarChar);
			param6.Value = MonedaDR;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@MetodoDePagoDR", SqlDbType.VarChar);
			param7.Value = MetodoDePagoDR;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@NumParcialidad", SqlDbType.TinyInt);
			param8.Value = NumParcialidad;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@ImpSaldoAnt", SqlDbType.Money);
			param9.Value = ImpSaldoAnt;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@ImpPagado", SqlDbType.Money);
			param10.Value = ImpPagado;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@ImpSaldoInsoluto", SqlDbType.Money);
			param11.Value = ImpSaldoInsoluto;
			commandParameters.Add(param11);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdFacturasComplPagDoctoRel";
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

		#region Eliminar VBFac_FacturasComplPagDocto_Rel
		public void Eliminar(long idfacturacomplpago,long idfacturappd,long idpagoscab)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaComplPago", SqlDbType.BigInt);
			param0.Value = idfacturacomplpago;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdFacturaPPD", SqlDbType.BigInt);
			param1.Value = idfacturappd;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param2.Value = idpagoscab;
			commandParameters.Add(param2);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelFacturasComplPagDoctoRel";
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

		#region Deshacer VBFac_FacturasComplPagDocto_Rel
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdFacturaComplPago,IdFacturaPPD,IdPagosCab);
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
		public List<ENTFacturascomplpagdoctoRel> RecuperarTodo()
		{
			List<ENTFacturascomplpagdoctoRel> result = new List<ENTFacturascomplpagdoctoRel>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasComplPagDoctoRel_TODO";
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
				    ENTFacturascomplpagdoctoRel item = new ENTFacturascomplpagdoctoRel();
					 if (!dr.IsNull("IdFacturaComplPago")) item.IdFacturaComplPago = Convert.ToInt64(dr["IdFacturaComplPago"]);
					 if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					 if (!dr.IsNull("IdFacturaPPD")) item.IdFacturaPPD = Convert.ToInt64(dr["IdFacturaPPD"]);
					 if (!dr.IsNull("IdDocumento")) item.IdDocumento = dr["IdDocumento"].ToString();
					 if (!dr.IsNull("Serie")) item.Serie = dr["Serie"].ToString();
					 if (!dr.IsNull("Folio")) item.Folio = Convert.ToInt64(dr["Folio"]);
					 if (!dr.IsNull("MonedaDR")) item.MonedaDR = dr["MonedaDR"].ToString();
					 if (!dr.IsNull("MetodoDePagoDR")) item.MetodoDePagoDR = dr["MetodoDePagoDR"].ToString();
					 if (!dr.IsNull("NumParcialidad")) item.NumParcialidad = Convert.ToByte(dr["NumParcialidad"]);
					 if (!dr.IsNull("ImpSaldoAnt")) item.ImpSaldoAnt = Convert.ToDecimal(dr["ImpSaldoAnt"]);
					 if (!dr.IsNull("ImpPagado")) item.ImpPagado = Convert.ToDecimal(dr["ImpPagado"]);
					 if (!dr.IsNull("ImpSaldoInsoluto")) item.ImpSaldoInsoluto = Convert.ToDecimal(dr["ImpSaldoInsoluto"]);
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

		#region Recuperar RecuperarFacturascomplpagdoctoRelPorLlavePrimaria
		public List<ENTFacturascomplpagdoctoRel> RecuperarFacturascomplpagdoctoRelPorLlavePrimaria(long idfacturacomplpago,long idpagoscab,long idfacturappd)
		{
			List<ENTFacturascomplpagdoctoRel> result = new List<ENTFacturascomplpagdoctoRel>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFacturaComplPago", SqlDbType.BigInt);
			param0.Value = idfacturacomplpago;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param1.Value = idpagoscab;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@IdFacturaPPD", SqlDbType.BigInt);
			param2.Value = idfacturappd;
			commandParameters.Add(param2);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasComplPagDoctoRel_POR_PK";
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
				    ENTFacturascomplpagdoctoRel item = new ENTFacturascomplpagdoctoRel();
					if (!dr.IsNull("IdFacturaComplPago")) item.IdFacturaComplPago = Convert.ToInt64(dr["IdFacturaComplPago"]);
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("IdFacturaPPD")) item.IdFacturaPPD = Convert.ToInt64(dr["IdFacturaPPD"]);
					if (!dr.IsNull("IdDocumento")) item.IdDocumento = dr["IdDocumento"].ToString();
					if (!dr.IsNull("Serie")) item.Serie = dr["Serie"].ToString();
					if (!dr.IsNull("Folio")) item.Folio = Convert.ToInt64(dr["Folio"]);
					if (!dr.IsNull("MonedaDR")) item.MonedaDR = dr["MonedaDR"].ToString();
					if (!dr.IsNull("MetodoDePagoDR")) item.MetodoDePagoDR = dr["MetodoDePagoDR"].ToString();
					if (!dr.IsNull("NumParcialidad")) item.NumParcialidad = Convert.ToByte(dr["NumParcialidad"]);
					if (!dr.IsNull("ImpSaldoAnt")) item.ImpSaldoAnt = Convert.ToDecimal(dr["ImpSaldoAnt"]);
					if (!dr.IsNull("ImpPagado")) item.ImpPagado = Convert.ToDecimal(dr["ImpPagado"]);
					if (!dr.IsNull("ImpSaldoInsoluto")) item.ImpSaldoInsoluto = Convert.ToDecimal(dr["ImpSaldoInsoluto"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaComplPago")) IdFacturaComplPago = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaComplPago"]);
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaPPD")) IdFacturaPPD = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaPPD"]);
					if (!dtResultado.Rows[0].IsNull("IdDocumento")) IdDocumento = dtResultado.Rows[0]["IdDocumento"].ToString();
					if (!dtResultado.Rows[0].IsNull("Serie")) Serie = dtResultado.Rows[0]["Serie"].ToString();
					if (!dtResultado.Rows[0].IsNull("Folio")) Folio = Convert.ToInt64(dtResultado.Rows[0]["Folio"]);
					if (!dtResultado.Rows[0].IsNull("MonedaDR")) MonedaDR = dtResultado.Rows[0]["MonedaDR"].ToString();
					if (!dtResultado.Rows[0].IsNull("MetodoDePagoDR")) MetodoDePagoDR = dtResultado.Rows[0]["MetodoDePagoDR"].ToString();
					if (!dtResultado.Rows[0].IsNull("NumParcialidad")) NumParcialidad = Convert.ToByte(dtResultado.Rows[0]["NumParcialidad"]);
					if (!dtResultado.Rows[0].IsNull("ImpSaldoAnt")) ImpSaldoAnt = Convert.ToDecimal(dtResultado.Rows[0]["ImpSaldoAnt"]);
					if (!dtResultado.Rows[0].IsNull("ImpPagado")) ImpPagado = Convert.ToDecimal(dtResultado.Rows[0]["ImpPagado"]);
					if (!dtResultado.Rows[0].IsNull("ImpSaldoInsoluto")) ImpSaldoInsoluto = Convert.ToDecimal(dtResultado.Rows[0]["ImpSaldoInsoluto"]);
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

		#region Recuperar RecuperarFacturascomplpagdoctoRelIdpagoscab
		public List<ENTFacturascomplpagdoctoRel> RecuperarFacturascomplpagdoctoRelIdpagoscab(long idpagoscab)
		{
			List<ENTFacturascomplpagdoctoRel> result = new List<ENTFacturascomplpagdoctoRel>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdPagosCab", SqlDbType.BigInt);
			param0.Value = idpagoscab;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasComplPagDoctoRel_POR_IdPagosCab";
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
				    ENTFacturascomplpagdoctoRel item = new ENTFacturascomplpagdoctoRel();
					if (!dr.IsNull("IdFacturaComplPago")) item.IdFacturaComplPago = Convert.ToInt64(dr["IdFacturaComplPago"]);
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("IdFacturaPPD")) item.IdFacturaPPD = Convert.ToInt64(dr["IdFacturaPPD"]);
					if (!dr.IsNull("IdDocumento")) item.IdDocumento = dr["IdDocumento"].ToString();
					if (!dr.IsNull("Serie")) item.Serie = dr["Serie"].ToString();
					if (!dr.IsNull("Folio")) item.Folio = Convert.ToInt64(dr["Folio"]);
					if (!dr.IsNull("MonedaDR")) item.MonedaDR = dr["MonedaDR"].ToString();
					if (!dr.IsNull("MetodoDePagoDR")) item.MetodoDePagoDR = dr["MetodoDePagoDR"].ToString();
					if (!dr.IsNull("NumParcialidad")) item.NumParcialidad = Convert.ToByte(dr["NumParcialidad"]);
					if (!dr.IsNull("ImpSaldoAnt")) item.ImpSaldoAnt = Convert.ToDecimal(dr["ImpSaldoAnt"]);
					if (!dr.IsNull("ImpPagado")) item.ImpPagado = Convert.ToDecimal(dr["ImpPagado"]);
					if (!dr.IsNull("ImpSaldoInsoluto")) item.ImpSaldoInsoluto = Convert.ToDecimal(dr["ImpSaldoInsoluto"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaComplPago")) IdFacturaComplPago = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaComplPago"]);
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaPPD")) IdFacturaPPD = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaPPD"]);
					if (!dtResultado.Rows[0].IsNull("IdDocumento")) IdDocumento = dtResultado.Rows[0]["IdDocumento"].ToString();
					if (!dtResultado.Rows[0].IsNull("Serie")) Serie = dtResultado.Rows[0]["Serie"].ToString();
					if (!dtResultado.Rows[0].IsNull("Folio")) Folio = Convert.ToInt64(dtResultado.Rows[0]["Folio"]);
					if (!dtResultado.Rows[0].IsNull("MonedaDR")) MonedaDR = dtResultado.Rows[0]["MonedaDR"].ToString();
					if (!dtResultado.Rows[0].IsNull("MetodoDePagoDR")) MetodoDePagoDR = dtResultado.Rows[0]["MetodoDePagoDR"].ToString();
					if (!dtResultado.Rows[0].IsNull("NumParcialidad")) NumParcialidad = Convert.ToByte(dtResultado.Rows[0]["NumParcialidad"]);
					if (!dtResultado.Rows[0].IsNull("ImpSaldoAnt")) ImpSaldoAnt = Convert.ToDecimal(dtResultado.Rows[0]["ImpSaldoAnt"]);
					if (!dtResultado.Rows[0].IsNull("ImpPagado")) ImpPagado = Convert.ToDecimal(dtResultado.Rows[0]["ImpPagado"]);
					if (!dtResultado.Rows[0].IsNull("ImpSaldoInsoluto")) ImpSaldoInsoluto = Convert.ToDecimal(dtResultado.Rows[0]["ImpSaldoInsoluto"]);
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

		#region Recuperar RecuperarFacturascomplpagdoctoRelIdfacturappd
		public List<ENTFacturascomplpagdoctoRel> RecuperarFacturascomplpagdoctoRelIdfacturappd(long idfacturappd)
		{
			List<ENTFacturascomplpagdoctoRel> result = new List<ENTFacturascomplpagdoctoRel>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFacturaPPD", SqlDbType.BigInt);
			param0.Value = idfacturappd;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasComplPagDoctoRel_POR_IdFacturaPPD";
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
				    ENTFacturascomplpagdoctoRel item = new ENTFacturascomplpagdoctoRel();
					if (!dr.IsNull("IdFacturaComplPago")) item.IdFacturaComplPago = Convert.ToInt64(dr["IdFacturaComplPago"]);
					if (!dr.IsNull("IdPagosCab")) item.IdPagosCab = Convert.ToInt64(dr["IdPagosCab"]);
					if (!dr.IsNull("IdFacturaPPD")) item.IdFacturaPPD = Convert.ToInt64(dr["IdFacturaPPD"]);
					if (!dr.IsNull("IdDocumento")) item.IdDocumento = dr["IdDocumento"].ToString();
					if (!dr.IsNull("Serie")) item.Serie = dr["Serie"].ToString();
					if (!dr.IsNull("Folio")) item.Folio = Convert.ToInt64(dr["Folio"]);
					if (!dr.IsNull("MonedaDR")) item.MonedaDR = dr["MonedaDR"].ToString();
					if (!dr.IsNull("MetodoDePagoDR")) item.MetodoDePagoDR = dr["MetodoDePagoDR"].ToString();
					if (!dr.IsNull("NumParcialidad")) item.NumParcialidad = Convert.ToByte(dr["NumParcialidad"]);
					if (!dr.IsNull("ImpSaldoAnt")) item.ImpSaldoAnt = Convert.ToDecimal(dr["ImpSaldoAnt"]);
					if (!dr.IsNull("ImpPagado")) item.ImpPagado = Convert.ToDecimal(dr["ImpPagado"]);
					if (!dr.IsNull("ImpSaldoInsoluto")) item.ImpSaldoInsoluto = Convert.ToDecimal(dr["ImpSaldoInsoluto"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaComplPago")) IdFacturaComplPago = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaComplPago"]);
					if (!dtResultado.Rows[0].IsNull("IdPagosCab")) IdPagosCab = Convert.ToInt64(dtResultado.Rows[0]["IdPagosCab"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaPPD")) IdFacturaPPD = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaPPD"]);
					if (!dtResultado.Rows[0].IsNull("IdDocumento")) IdDocumento = dtResultado.Rows[0]["IdDocumento"].ToString();
					if (!dtResultado.Rows[0].IsNull("Serie")) Serie = dtResultado.Rows[0]["Serie"].ToString();
					if (!dtResultado.Rows[0].IsNull("Folio")) Folio = Convert.ToInt64(dtResultado.Rows[0]["Folio"]);
					if (!dtResultado.Rows[0].IsNull("MonedaDR")) MonedaDR = dtResultado.Rows[0]["MonedaDR"].ToString();
					if (!dtResultado.Rows[0].IsNull("MetodoDePagoDR")) MetodoDePagoDR = dtResultado.Rows[0]["MetodoDePagoDR"].ToString();
					if (!dtResultado.Rows[0].IsNull("NumParcialidad")) NumParcialidad = Convert.ToByte(dtResultado.Rows[0]["NumParcialidad"]);
					if (!dtResultado.Rows[0].IsNull("ImpSaldoAnt")) ImpSaldoAnt = Convert.ToDecimal(dtResultado.Rows[0]["ImpSaldoAnt"]);
					if (!dtResultado.Rows[0].IsNull("ImpPagado")) ImpPagado = Convert.ToDecimal(dtResultado.Rows[0]["ImpPagado"]);
					if (!dtResultado.Rows[0].IsNull("ImpSaldoInsoluto")) ImpSaldoInsoluto = Convert.ToDecimal(dtResultado.Rows[0]["ImpSaldoInsoluto"]);
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
			_copia = new DALFacturascomplpagdoctoRel(_conexion);
			Type tipo = typeof(ENTFacturascomplpagdoctoRel);
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

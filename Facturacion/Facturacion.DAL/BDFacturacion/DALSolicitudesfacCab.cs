using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALSolicitudesfacCab: ENTSolicitudesfacCab
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALSolicitudesfacCab _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALSolicitudesfacCab(SqlConnection conexion)
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
			IdSolicitudesFac = 0;
			PNR = String.Empty;
			NombrePasajero = String.Empty;
			ApellidosPasajero = String.Empty;
			UsoCFDI = String.Empty;
			EsExtranjero = false;
			RFCReceptor = String.Empty;
			ResidenciaFiscal = String.Empty;
			NumRegIdTrib_TAXID = String.Empty;
			emailReceptor = String.Empty;
			EsRobot = false;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_SolicitudesFac_Cab
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdSolicitudesFac", SqlDbType.BigInt);
			param0.Value = IdSolicitudesFac;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@PNR", SqlDbType.VarChar);
			param1.Value = PNR;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@NombrePasajero", SqlDbType.VarChar);
			param2.Value = NombrePasajero;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@ApellidosPasajero", SqlDbType.VarChar);
			param3.Value = ApellidosPasajero;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@UsoCFDI", SqlDbType.VarChar);
			param4.Value = UsoCFDI;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@EsExtranjero", SqlDbType.Bit);
			param5.Value = EsExtranjero;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@RFCReceptor", SqlDbType.VarChar);
			param6.Value = RFCReceptor;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@ResidenciaFiscal", SqlDbType.VarChar);
			param7.Value = ResidenciaFiscal;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@NumRegIdTrib_TAXID", SqlDbType.VarChar);
			param8.Value = NumRegIdTrib_TAXID;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@emailReceptor", SqlDbType.VarChar);
			param9.Value = emailReceptor;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@EsRobot", SqlDbType.Bit);
			param10.Value = EsRobot;
			commandParameters.Add(param10);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsSolicitudesFacCab";
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
						IdSolicitudesFac = Convert.ToInt64(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_SolicitudesFac_Cab
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdSolicitudesFac", SqlDbType.BigInt);
			param0.Value = IdSolicitudesFac;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@PNR", SqlDbType.VarChar);
			param1.Value = PNR;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@NombrePasajero", SqlDbType.VarChar);
			param2.Value = NombrePasajero;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@ApellidosPasajero", SqlDbType.VarChar);
			param3.Value = ApellidosPasajero;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@UsoCFDI", SqlDbType.VarChar);
			param4.Value = UsoCFDI;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@EsExtranjero", SqlDbType.Bit);
			param5.Value = EsExtranjero;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@RFCReceptor", SqlDbType.VarChar);
			param6.Value = RFCReceptor;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@ResidenciaFiscal", SqlDbType.VarChar);
			param7.Value = ResidenciaFiscal;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@NumRegIdTrib_TAXID", SqlDbType.VarChar);
			param8.Value = NumRegIdTrib_TAXID;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@emailReceptor", SqlDbType.VarChar);
			param9.Value = emailReceptor;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@EsRobot", SqlDbType.Bit);
			param10.Value = EsRobot;
			commandParameters.Add(param10);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdSolicitudesFacCab";
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

		#region Eliminar VBFac_SolicitudesFac_Cab
		public void Eliminar(long idsolicitudesfac)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdSolicitudesFac", SqlDbType.BigInt);
			param0.Value = idsolicitudesfac;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelSolicitudesFacCab";
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

		#region Deshacer VBFac_SolicitudesFac_Cab
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdSolicitudesFac);
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
		public List<ENTSolicitudesfacCab> RecuperarTodo()
		{
			List<ENTSolicitudesfacCab> result = new List<ENTSolicitudesfacCab>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetSolicitudesFacCab_TODO";
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
				    ENTSolicitudesfacCab item = new ENTSolicitudesfacCab();
					 if (!dr.IsNull("IdSolicitudesFac")) item.IdSolicitudesFac = Convert.ToInt64(dr["IdSolicitudesFac"]);
					 if (!dr.IsNull("PNR")) item.PNR = dr["PNR"].ToString();
					 if (!dr.IsNull("NombrePasajero")) item.NombrePasajero = dr["NombrePasajero"].ToString();
					 if (!dr.IsNull("ApellidosPasajero")) item.ApellidosPasajero = dr["ApellidosPasajero"].ToString();
					 if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					 if (!dr.IsNull("EsExtranjero")) item.EsExtranjero = Convert.ToBoolean(dr["EsExtranjero"]);
					 if (!dr.IsNull("RFCReceptor")) item.RFCReceptor = dr["RFCReceptor"].ToString();
					 if (!dr.IsNull("ResidenciaFiscal")) item.ResidenciaFiscal = dr["ResidenciaFiscal"].ToString();
					 if (!dr.IsNull("NumRegIdTrib_TAXID")) item.NumRegIdTrib_TAXID = dr["NumRegIdTrib_TAXID"].ToString();
					 if (!dr.IsNull("emailReceptor")) item.emailReceptor = dr["emailReceptor"].ToString();
					 if (!dr.IsNull("EsRobot")) item.EsRobot = Convert.ToBoolean(dr["EsRobot"]);
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

		#region Recuperar RecuperarSolicitudesfacCabPorLlavePrimaria
		public List<ENTSolicitudesfacCab> RecuperarSolicitudesfacCabPorLlavePrimaria(long idsolicitudesfac)
		{
			List<ENTSolicitudesfacCab> result = new List<ENTSolicitudesfacCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdSolicitudesFac", SqlDbType.BigInt);
			param0.Value = idsolicitudesfac;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetSolicitudesFacCab_POR_PK";
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
				    ENTSolicitudesfacCab item = new ENTSolicitudesfacCab();
					if (!dr.IsNull("IdSolicitudesFac")) item.IdSolicitudesFac = Convert.ToInt64(dr["IdSolicitudesFac"]);
					if (!dr.IsNull("PNR")) item.PNR = dr["PNR"].ToString();
					if (!dr.IsNull("NombrePasajero")) item.NombrePasajero = dr["NombrePasajero"].ToString();
					if (!dr.IsNull("ApellidosPasajero")) item.ApellidosPasajero = dr["ApellidosPasajero"].ToString();
					if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					if (!dr.IsNull("EsExtranjero")) item.EsExtranjero = Convert.ToBoolean(dr["EsExtranjero"]);
					if (!dr.IsNull("RFCReceptor")) item.RFCReceptor = dr["RFCReceptor"].ToString();
					if (!dr.IsNull("ResidenciaFiscal")) item.ResidenciaFiscal = dr["ResidenciaFiscal"].ToString();
					if (!dr.IsNull("NumRegIdTrib_TAXID")) item.NumRegIdTrib_TAXID = dr["NumRegIdTrib_TAXID"].ToString();
					if (!dr.IsNull("emailReceptor")) item.emailReceptor = dr["emailReceptor"].ToString();
					if (!dr.IsNull("EsRobot")) item.EsRobot = Convert.ToBoolean(dr["EsRobot"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdSolicitudesFac")) IdSolicitudesFac = Convert.ToInt64(dtResultado.Rows[0]["IdSolicitudesFac"]);
					if (!dtResultado.Rows[0].IsNull("PNR")) PNR = dtResultado.Rows[0]["PNR"].ToString();
					if (!dtResultado.Rows[0].IsNull("NombrePasajero")) NombrePasajero = dtResultado.Rows[0]["NombrePasajero"].ToString();
					if (!dtResultado.Rows[0].IsNull("ApellidosPasajero")) ApellidosPasajero = dtResultado.Rows[0]["ApellidosPasajero"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsoCFDI")) UsoCFDI = dtResultado.Rows[0]["UsoCFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsExtranjero")) EsExtranjero = Convert.ToBoolean(dtResultado.Rows[0]["EsExtranjero"]);
					if (!dtResultado.Rows[0].IsNull("RFCReceptor")) RFCReceptor = dtResultado.Rows[0]["RFCReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("ResidenciaFiscal")) ResidenciaFiscal = dtResultado.Rows[0]["ResidenciaFiscal"].ToString();
					if (!dtResultado.Rows[0].IsNull("NumRegIdTrib_TAXID")) NumRegIdTrib_TAXID = dtResultado.Rows[0]["NumRegIdTrib_TAXID"].ToString();
					if (!dtResultado.Rows[0].IsNull("emailReceptor")) emailReceptor = dtResultado.Rows[0]["emailReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsRobot")) EsRobot = Convert.ToBoolean(dtResultado.Rows[0]["EsRobot"]);
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

		#region Recuperar RecuperarSolicitudesfacCabPnr
		public List<ENTSolicitudesfacCab> RecuperarSolicitudesfacCabPnr(string pnr)
		{
			List<ENTSolicitudesfacCab> result = new List<ENTSolicitudesfacCab>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@PNR", SqlDbType.VarChar);
			param0.Value = pnr;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetSolicitudesFacCab_POR_PNR";
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
				    ENTSolicitudesfacCab item = new ENTSolicitudesfacCab();
					if (!dr.IsNull("IdSolicitudesFac")) item.IdSolicitudesFac = Convert.ToInt64(dr["IdSolicitudesFac"]);
					if (!dr.IsNull("PNR")) item.PNR = dr["PNR"].ToString();
					if (!dr.IsNull("NombrePasajero")) item.NombrePasajero = dr["NombrePasajero"].ToString();
					if (!dr.IsNull("ApellidosPasajero")) item.ApellidosPasajero = dr["ApellidosPasajero"].ToString();
					if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					if (!dr.IsNull("EsExtranjero")) item.EsExtranjero = Convert.ToBoolean(dr["EsExtranjero"]);
					if (!dr.IsNull("RFCReceptor")) item.RFCReceptor = dr["RFCReceptor"].ToString();
					if (!dr.IsNull("ResidenciaFiscal")) item.ResidenciaFiscal = dr["ResidenciaFiscal"].ToString();
					if (!dr.IsNull("NumRegIdTrib_TAXID")) item.NumRegIdTrib_TAXID = dr["NumRegIdTrib_TAXID"].ToString();
					if (!dr.IsNull("emailReceptor")) item.emailReceptor = dr["emailReceptor"].ToString();
					if (!dr.IsNull("EsRobot")) item.EsRobot = Convert.ToBoolean(dr["EsRobot"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdSolicitudesFac")) IdSolicitudesFac = Convert.ToInt64(dtResultado.Rows[0]["IdSolicitudesFac"]);
					if (!dtResultado.Rows[0].IsNull("PNR")) PNR = dtResultado.Rows[0]["PNR"].ToString();
					if (!dtResultado.Rows[0].IsNull("NombrePasajero")) NombrePasajero = dtResultado.Rows[0]["NombrePasajero"].ToString();
					if (!dtResultado.Rows[0].IsNull("ApellidosPasajero")) ApellidosPasajero = dtResultado.Rows[0]["ApellidosPasajero"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsoCFDI")) UsoCFDI = dtResultado.Rows[0]["UsoCFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsExtranjero")) EsExtranjero = Convert.ToBoolean(dtResultado.Rows[0]["EsExtranjero"]);
					if (!dtResultado.Rows[0].IsNull("RFCReceptor")) RFCReceptor = dtResultado.Rows[0]["RFCReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("ResidenciaFiscal")) ResidenciaFiscal = dtResultado.Rows[0]["ResidenciaFiscal"].ToString();
					if (!dtResultado.Rows[0].IsNull("NumRegIdTrib_TAXID")) NumRegIdTrib_TAXID = dtResultado.Rows[0]["NumRegIdTrib_TAXID"].ToString();
					if (!dtResultado.Rows[0].IsNull("emailReceptor")) emailReceptor = dtResultado.Rows[0]["emailReceptor"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsRobot")) EsRobot = Convert.ToBoolean(dtResultado.Rows[0]["EsRobot"]);
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
			_copia = new DALSolicitudesfacCab(_conexion);
			Type tipo = typeof(ENTSolicitudesfacCab);
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

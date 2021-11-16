using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALXmlFtpReg: ENTXmlFtpReg
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALXmlFtpReg _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALXmlFtpReg(SqlConnection conexion)
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
			IdXMLFTP = 0;
			IdPeticionPAC = 0;
			FechaTimbrado = new DateTime();
			RutaCFDI = String.Empty;
			RutaPDF = String.Empty;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_XML_FTP_Reg
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdXMLFTP", SqlDbType.BigInt);
			param0.Value = IdXMLFTP;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdPeticionPAC", SqlDbType.BigInt);
			param1.Value = IdPeticionPAC;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@FechaTimbrado", SqlDbType.VarChar);
			param2.Value = FechaTimbrado.Year > 1900 ? FechaTimbrado.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@RutaCFDI", SqlDbType.VarChar);
			param3.Value = RutaCFDI;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@RutaPDF", SqlDbType.VarChar);
			param4.Value = RutaPDF;
			commandParameters.Add(param4);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsXMLFTPReg";
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
						IdXMLFTP = Convert.ToInt64(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_XML_FTP_Reg
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdXMLFTP", SqlDbType.BigInt);
			param0.Value = IdXMLFTP;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdPeticionPAC", SqlDbType.BigInt);
			param1.Value = IdPeticionPAC;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@FechaTimbrado", SqlDbType.VarChar);
			param2.Value = FechaTimbrado.Year > 1900 ? FechaTimbrado.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@RutaCFDI", SqlDbType.VarChar);
			param3.Value = RutaCFDI;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@RutaPDF", SqlDbType.VarChar);
			param4.Value = RutaPDF;
			commandParameters.Add(param4);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdXMLFTPReg";
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

		#region Eliminar VBFac_XML_FTP_Reg
		public void Eliminar(long idxmlftp)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdXMLFTP", SqlDbType.BigInt);
			param0.Value = idxmlftp;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelXMLFTPReg";
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

		#region Deshacer VBFac_XML_FTP_Reg
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdXMLFTP);
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
		public List<ENTXmlFtpReg> RecuperarTodo()
		{
			List<ENTXmlFtpReg> result = new List<ENTXmlFtpReg>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetXMLFTPReg_TODO";
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
				    ENTXmlFtpReg item = new ENTXmlFtpReg();
					 if (!dr.IsNull("IdXMLFTP")) item.IdXMLFTP = Convert.ToInt64(dr["IdXMLFTP"]);
					 if (!dr.IsNull("IdPeticionPAC")) item.IdPeticionPAC = Convert.ToInt64(dr["IdPeticionPAC"]);
					 if (!dr.IsNull("FechaTimbrado")) item.FechaTimbrado = Convert.ToDateTime(dr["FechaTimbrado"]);
					 if (!dr.IsNull("RutaCFDI")) item.RutaCFDI = dr["RutaCFDI"].ToString();
					 if (!dr.IsNull("RutaPDF")) item.RutaPDF = dr["RutaPDF"].ToString();
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

		#region Recuperar RecuperarXmlFtpRegPorLlavePrimaria
		public List<ENTXmlFtpReg> RecuperarXmlFtpRegPorLlavePrimaria(long idxmlftp)
		{
			List<ENTXmlFtpReg> result = new List<ENTXmlFtpReg>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdXMLFTP", SqlDbType.BigInt);
			param0.Value = idxmlftp;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetXMLFTPReg_POR_PK";
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
				    ENTXmlFtpReg item = new ENTXmlFtpReg();
					if (!dr.IsNull("IdXMLFTP")) item.IdXMLFTP = Convert.ToInt64(dr["IdXMLFTP"]);
					if (!dr.IsNull("IdPeticionPAC")) item.IdPeticionPAC = Convert.ToInt64(dr["IdPeticionPAC"]);
					if (!dr.IsNull("FechaTimbrado")) item.FechaTimbrado = Convert.ToDateTime(dr["FechaTimbrado"]);
					if (!dr.IsNull("RutaCFDI")) item.RutaCFDI = dr["RutaCFDI"].ToString();
					if (!dr.IsNull("RutaPDF")) item.RutaPDF = dr["RutaPDF"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdXMLFTP")) IdXMLFTP = Convert.ToInt64(dtResultado.Rows[0]["IdXMLFTP"]);
					if (!dtResultado.Rows[0].IsNull("IdPeticionPAC")) IdPeticionPAC = Convert.ToInt64(dtResultado.Rows[0]["IdPeticionPAC"]);
					if (!dtResultado.Rows[0].IsNull("FechaTimbrado")) FechaTimbrado = Convert.ToDateTime(dtResultado.Rows[0]["FechaTimbrado"]);
					if (!dtResultado.Rows[0].IsNull("RutaCFDI")) RutaCFDI = dtResultado.Rows[0]["RutaCFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("RutaPDF")) RutaPDF = dtResultado.Rows[0]["RutaPDF"].ToString();
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

		#region Recuperar RecuperarXmlFtpRegIdpeticionpac
		public List<ENTXmlFtpReg> RecuperarXmlFtpRegIdpeticionpac(long idpeticionpac)
		{
			List<ENTXmlFtpReg> result = new List<ENTXmlFtpReg>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdPeticionPAC", SqlDbType.BigInt);
			param0.Value = idpeticionpac;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetXMLFTPReg_POR_IdPeticionPAC";
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
				    ENTXmlFtpReg item = new ENTXmlFtpReg();
					if (!dr.IsNull("IdXMLFTP")) item.IdXMLFTP = Convert.ToInt64(dr["IdXMLFTP"]);
					if (!dr.IsNull("IdPeticionPAC")) item.IdPeticionPAC = Convert.ToInt64(dr["IdPeticionPAC"]);
					if (!dr.IsNull("FechaTimbrado")) item.FechaTimbrado = Convert.ToDateTime(dr["FechaTimbrado"]);
					if (!dr.IsNull("RutaCFDI")) item.RutaCFDI = dr["RutaCFDI"].ToString();
					if (!dr.IsNull("RutaPDF")) item.RutaPDF = dr["RutaPDF"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdXMLFTP")) IdXMLFTP = Convert.ToInt64(dtResultado.Rows[0]["IdXMLFTP"]);
					if (!dtResultado.Rows[0].IsNull("IdPeticionPAC")) IdPeticionPAC = Convert.ToInt64(dtResultado.Rows[0]["IdPeticionPAC"]);
					if (!dtResultado.Rows[0].IsNull("FechaTimbrado")) FechaTimbrado = Convert.ToDateTime(dtResultado.Rows[0]["FechaTimbrado"]);
					if (!dtResultado.Rows[0].IsNull("RutaCFDI")) RutaCFDI = dtResultado.Rows[0]["RutaCFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("RutaPDF")) RutaPDF = dtResultado.Rows[0]["RutaPDF"].ToString();
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
			_copia = new DALXmlFtpReg(_conexion);
			Type tipo = typeof(ENTXmlFtpReg);
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

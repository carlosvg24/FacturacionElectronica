using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALFoliosfiscalesCnf: ENTFoliosfiscalesCnf
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALFoliosfiscalesCnf _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALFoliosfiscalesCnf(SqlConnection conexion)
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
			IdFolioFiscal = 0;
			TipoComprobante = String.Empty;
			ClavePegaso = String.Empty;
			NombrePegaso = String.Empty;
			Serie = String.Empty;
			FolioInicial = 0;
			FolioFinal = 0;
			FolioActual = 0;
			FechaFinVigencia = new DateTime();
			Activo = true;
			UsuarioRegistro = 0;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_FoliosFiscales_Cnf
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFolioFiscal", SqlDbType.Int);
			param0.Value = IdFolioFiscal;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@TipoComprobante", SqlDbType.VarChar);
			param1.Value = TipoComprobante;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@ClavePegaso", SqlDbType.VarChar);
			param2.Value = ClavePegaso;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@NombrePegaso", SqlDbType.VarChar);
			param3.Value = NombrePegaso;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Serie", SqlDbType.VarChar);
			param4.Value = Serie;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@FolioInicial", SqlDbType.BigInt);
			param5.Value = FolioInicial;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@FolioFinal", SqlDbType.BigInt);
			param6.Value = FolioFinal;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@FolioActual", SqlDbType.BigInt);
			param7.Value = FolioActual;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@FechaFinVigencia", SqlDbType.VarChar);
			param8.Value = FechaFinVigencia.Year > 1900 ? FechaFinVigencia.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@Activo", SqlDbType.Bit);
			param9.Value = Activo;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@UsuarioRegistro", SqlDbType.Int);
			param10.Value = UsuarioRegistro;
			commandParameters.Add(param10);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsFoliosFiscalesCnf";
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
						IdFolioFiscal = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_FoliosFiscales_Cnf
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFolioFiscal", SqlDbType.Int);
			param0.Value = IdFolioFiscal;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@TipoComprobante", SqlDbType.VarChar);
			param1.Value = TipoComprobante;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@ClavePegaso", SqlDbType.VarChar);
			param2.Value = ClavePegaso;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@NombrePegaso", SqlDbType.VarChar);
			param3.Value = NombrePegaso;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Serie", SqlDbType.VarChar);
			param4.Value = Serie;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@FolioInicial", SqlDbType.BigInt);
			param5.Value = FolioInicial;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@FolioFinal", SqlDbType.BigInt);
			param6.Value = FolioFinal;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@FolioActual", SqlDbType.BigInt);
			param7.Value = FolioActual;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@FechaFinVigencia", SqlDbType.VarChar);
			param8.Value = FechaFinVigencia.Year > 1900 ? FechaFinVigencia.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@Activo", SqlDbType.Bit);
			param9.Value = Activo;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@UsuarioRegistro", SqlDbType.Int);
			param10.Value = UsuarioRegistro;
			commandParameters.Add(param10);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdFoliosFiscalesCnf";
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

		#region Eliminar VBFac_FoliosFiscales_Cnf
		public void Eliminar(int idfoliofiscal)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFolioFiscal", SqlDbType.Int);
			param0.Value = idfoliofiscal;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelFoliosFiscalesCnf";
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

		#region Deshacer VBFac_FoliosFiscales_Cnf
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdFolioFiscal);
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
		public List<ENTFoliosfiscalesCnf> RecuperarTodo()
		{
			List<ENTFoliosfiscalesCnf> result = new List<ENTFoliosfiscalesCnf>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFoliosFiscalesCnf_TODO";
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
				    ENTFoliosfiscalesCnf item = new ENTFoliosfiscalesCnf();
					 if (!dr.IsNull("IdFolioFiscal")) item.IdFolioFiscal = Convert.ToInt32(dr["IdFolioFiscal"]);
					 if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
					 if (!dr.IsNull("ClavePegaso")) item.ClavePegaso = dr["ClavePegaso"].ToString();
					 if (!dr.IsNull("NombrePegaso")) item.NombrePegaso = dr["NombrePegaso"].ToString();
					 if (!dr.IsNull("Serie")) item.Serie = dr["Serie"].ToString();
					 if (!dr.IsNull("FolioInicial")) item.FolioInicial = Convert.ToInt64(dr["FolioInicial"]);
					 if (!dr.IsNull("FolioFinal")) item.FolioFinal = Convert.ToInt64(dr["FolioFinal"]);
					 if (!dr.IsNull("FolioActual")) item.FolioActual = Convert.ToInt64(dr["FolioActual"]);
					 if (!dr.IsNull("FechaFinVigencia")) item.FechaFinVigencia = Convert.ToDateTime(dr["FechaFinVigencia"]);
					 if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					 if (!dr.IsNull("UsuarioRegistro")) item.UsuarioRegistro = Convert.ToInt32(dr["UsuarioRegistro"]);
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

		#region Recuperar RecuperarFoliosfiscalesCnfPorLlavePrimaria
		public List<ENTFoliosfiscalesCnf> RecuperarFoliosfiscalesCnfPorLlavePrimaria(int idfoliofiscal)
		{
			List<ENTFoliosfiscalesCnf> result = new List<ENTFoliosfiscalesCnf>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFolioFiscal", SqlDbType.Int);
			param0.Value = idfoliofiscal;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFoliosFiscalesCnf_POR_PK";
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
				    ENTFoliosfiscalesCnf item = new ENTFoliosfiscalesCnf();
					if (!dr.IsNull("IdFolioFiscal")) item.IdFolioFiscal = Convert.ToInt32(dr["IdFolioFiscal"]);
					if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
					if (!dr.IsNull("ClavePegaso")) item.ClavePegaso = dr["ClavePegaso"].ToString();
					if (!dr.IsNull("NombrePegaso")) item.NombrePegaso = dr["NombrePegaso"].ToString();
					if (!dr.IsNull("Serie")) item.Serie = dr["Serie"].ToString();
					if (!dr.IsNull("FolioInicial")) item.FolioInicial = Convert.ToInt64(dr["FolioInicial"]);
					if (!dr.IsNull("FolioFinal")) item.FolioFinal = Convert.ToInt64(dr["FolioFinal"]);
					if (!dr.IsNull("FolioActual")) item.FolioActual = Convert.ToInt64(dr["FolioActual"]);
					if (!dr.IsNull("FechaFinVigencia")) item.FechaFinVigencia = Convert.ToDateTime(dr["FechaFinVigencia"]);
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("UsuarioRegistro")) item.UsuarioRegistro = Convert.ToInt32(dr["UsuarioRegistro"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFolioFiscal")) IdFolioFiscal = Convert.ToInt32(dtResultado.Rows[0]["IdFolioFiscal"]);
					if (!dtResultado.Rows[0].IsNull("TipoComprobante")) TipoComprobante = dtResultado.Rows[0]["TipoComprobante"].ToString();
					if (!dtResultado.Rows[0].IsNull("ClavePegaso")) ClavePegaso = dtResultado.Rows[0]["ClavePegaso"].ToString();
					if (!dtResultado.Rows[0].IsNull("NombrePegaso")) NombrePegaso = dtResultado.Rows[0]["NombrePegaso"].ToString();
					if (!dtResultado.Rows[0].IsNull("Serie")) Serie = dtResultado.Rows[0]["Serie"].ToString();
					if (!dtResultado.Rows[0].IsNull("FolioInicial")) FolioInicial = Convert.ToInt64(dtResultado.Rows[0]["FolioInicial"]);
					if (!dtResultado.Rows[0].IsNull("FolioFinal")) FolioFinal = Convert.ToInt64(dtResultado.Rows[0]["FolioFinal"]);
					if (!dtResultado.Rows[0].IsNull("FolioActual")) FolioActual = Convert.ToInt64(dtResultado.Rows[0]["FolioActual"]);
					if (!dtResultado.Rows[0].IsNull("FechaFinVigencia")) FechaFinVigencia = Convert.ToDateTime(dtResultado.Rows[0]["FechaFinVigencia"]);
					if (!dtResultado.Rows[0].IsNull("Activo")) Activo = Convert.ToBoolean(dtResultado.Rows[0]["Activo"]);
					if (!dtResultado.Rows[0].IsNull("UsuarioRegistro")) UsuarioRegistro = Convert.ToInt32(dtResultado.Rows[0]["UsuarioRegistro"]);
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
			_copia = new DALFoliosfiscalesCnf(_conexion);
			Type tipo = typeof(ENTFoliosfiscalesCnf);
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

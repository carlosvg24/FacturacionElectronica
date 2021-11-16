using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALConceptosCat: ENTConceptosCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALConceptosCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALConceptosCat(SqlConnection conexion)
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
			IdConcepto = 0;
			TipoComprobante = String.Empty;
			ClaveProdServ = String.Empty;
			NoIdentificacion = String.Empty;
			ClaveUnidad = String.Empty;
			Unidad = String.Empty;
			Descripcion = String.Empty;
			OrdenConcepto = 0;
			ClasFact = 0;
			Activo = true;
			UsuarioRegistro = 0;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_Conceptos_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdConcepto", SqlDbType.SmallInt);
			param0.Value = IdConcepto;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@TipoComprobante", SqlDbType.VarChar);
			param1.Value = TipoComprobante;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@ClaveProdServ", SqlDbType.VarChar);
			param2.Value = ClaveProdServ;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@NoIdentificacion", SqlDbType.VarChar);
			param3.Value = NoIdentificacion;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@ClaveUnidad", SqlDbType.VarChar);
			param4.Value = ClaveUnidad;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Unidad", SqlDbType.VarChar);
			param5.Value = Unidad;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param6.Value = Descripcion;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@OrdenConcepto", SqlDbType.Int);
			param7.Value = OrdenConcepto;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@ClasFact", SqlDbType.TinyInt);
			param8.Value = ClasFact;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@Activo", SqlDbType.Bit);
			param9.Value = Activo;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@UsuarioRegistro", SqlDbType.Int);
			param10.Value = UsuarioRegistro;
			commandParameters.Add(param10);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsConceptosCat";
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
						IdConcepto = Convert.ToInt16(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_Conceptos_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdConcepto", SqlDbType.SmallInt);
			param0.Value = IdConcepto;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@TipoComprobante", SqlDbType.VarChar);
			param1.Value = TipoComprobante;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@ClaveProdServ", SqlDbType.VarChar);
			param2.Value = ClaveProdServ;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@NoIdentificacion", SqlDbType.VarChar);
			param3.Value = NoIdentificacion;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@ClaveUnidad", SqlDbType.VarChar);
			param4.Value = ClaveUnidad;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Unidad", SqlDbType.VarChar);
			param5.Value = Unidad;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param6.Value = Descripcion;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@OrdenConcepto", SqlDbType.Int);
			param7.Value = OrdenConcepto;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@ClasFact", SqlDbType.TinyInt);
			param8.Value = ClasFact;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@Activo", SqlDbType.Bit);
			param9.Value = Activo;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@UsuarioRegistro", SqlDbType.Int);
			param10.Value = UsuarioRegistro;
			commandParameters.Add(param10);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdConceptosCat";
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

		#region Eliminar VBFac_Conceptos_Cat
		public void Eliminar(int idconcepto)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdConcepto", SqlDbType.SmallInt);
			param0.Value = idconcepto;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelConceptosCat";
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

		#region Deshacer VBFac_Conceptos_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdConcepto);
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
		public List<ENTConceptosCat> RecuperarTodo()
		{
			List<ENTConceptosCat> result = new List<ENTConceptosCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetConceptosCat_TODO";
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
				    ENTConceptosCat item = new ENTConceptosCat();
					 if (!dr.IsNull("IdConcepto")) item.IdConcepto = Convert.ToInt16(dr["IdConcepto"]);
					 if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
					 if (!dr.IsNull("ClaveProdServ")) item.ClaveProdServ = dr["ClaveProdServ"].ToString();
					 if (!dr.IsNull("NoIdentificacion")) item.NoIdentificacion = dr["NoIdentificacion"].ToString();
					 if (!dr.IsNull("ClaveUnidad")) item.ClaveUnidad = dr["ClaveUnidad"].ToString();
					 if (!dr.IsNull("Unidad")) item.Unidad = dr["Unidad"].ToString();
					 if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					 if (!dr.IsNull("OrdenConcepto")) item.OrdenConcepto = Convert.ToInt32(dr["OrdenConcepto"]);
					 if (!dr.IsNull("ClasFact")) item.ClasFact = Convert.ToByte(dr["ClasFact"]);
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

		#region Recuperar RecuperarConceptosCatPorLlavePrimaria
		public List<ENTConceptosCat> RecuperarConceptosCatPorLlavePrimaria(int idconcepto)
		{
			List<ENTConceptosCat> result = new List<ENTConceptosCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdConcepto", SqlDbType.SmallInt);
			param0.Value = idconcepto;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetConceptosCat_POR_PK";
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
				    ENTConceptosCat item = new ENTConceptosCat();
					if (!dr.IsNull("IdConcepto")) item.IdConcepto = Convert.ToInt16(dr["IdConcepto"]);
					if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
					if (!dr.IsNull("ClaveProdServ")) item.ClaveProdServ = dr["ClaveProdServ"].ToString();
					if (!dr.IsNull("NoIdentificacion")) item.NoIdentificacion = dr["NoIdentificacion"].ToString();
					if (!dr.IsNull("ClaveUnidad")) item.ClaveUnidad = dr["ClaveUnidad"].ToString();
					if (!dr.IsNull("Unidad")) item.Unidad = dr["Unidad"].ToString();
					if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					if (!dr.IsNull("OrdenConcepto")) item.OrdenConcepto = Convert.ToInt32(dr["OrdenConcepto"]);
					if (!dr.IsNull("ClasFact")) item.ClasFact = Convert.ToByte(dr["ClasFact"]);
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("UsuarioRegistro")) item.UsuarioRegistro = Convert.ToInt32(dr["UsuarioRegistro"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdConcepto")) IdConcepto = Convert.ToInt16(dtResultado.Rows[0]["IdConcepto"]);
					if (!dtResultado.Rows[0].IsNull("TipoComprobante")) TipoComprobante = dtResultado.Rows[0]["TipoComprobante"].ToString();
					if (!dtResultado.Rows[0].IsNull("ClaveProdServ")) ClaveProdServ = dtResultado.Rows[0]["ClaveProdServ"].ToString();
					if (!dtResultado.Rows[0].IsNull("NoIdentificacion")) NoIdentificacion = dtResultado.Rows[0]["NoIdentificacion"].ToString();
					if (!dtResultado.Rows[0].IsNull("ClaveUnidad")) ClaveUnidad = dtResultado.Rows[0]["ClaveUnidad"].ToString();
					if (!dtResultado.Rows[0].IsNull("Unidad")) Unidad = dtResultado.Rows[0]["Unidad"].ToString();
					if (!dtResultado.Rows[0].IsNull("Descripcion")) Descripcion = dtResultado.Rows[0]["Descripcion"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrdenConcepto")) OrdenConcepto = Convert.ToInt32(dtResultado.Rows[0]["OrdenConcepto"]);
					if (!dtResultado.Rows[0].IsNull("ClasFact")) ClasFact = Convert.ToByte(dtResultado.Rows[0]["ClasFact"]);
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
			_copia = new DALConceptosCat(_conexion);
			Type tipo = typeof(ENTConceptosCat);
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

using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALFeeCat: ENTFeeCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALFeeCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALFeeCat(SqlConnection conexion)
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
			IdFee = 0;
			FeeCode = String.Empty;
			DisplayCode = String.Empty;
			Name = String.Empty;
			Description = String.Empty;
			ClasFact = 0;
			Activo = true;
			UsuarioRegistro = 0;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_Fee_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFee", SqlDbType.Int);
			param0.Value = IdFee;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@FeeCode", SqlDbType.VarChar);
			param1.Value = FeeCode;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@DisplayCode", SqlDbType.VarChar);
			param2.Value = DisplayCode;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Name", SqlDbType.VarChar);
			param3.Value = Name;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Description", SqlDbType.VarChar);
			param4.Value = Description;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@ClasFact", SqlDbType.TinyInt);
			param5.Value = ClasFact;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Activo", SqlDbType.Bit);
			param6.Value = Activo;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@UsuarioRegistro", SqlDbType.Int);
			param7.Value = UsuarioRegistro;
			commandParameters.Add(param7);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsFeeCat";
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
						IdFee = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_Fee_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFee", SqlDbType.Int);
			param0.Value = IdFee;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@FeeCode", SqlDbType.VarChar);
			param1.Value = FeeCode;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@DisplayCode", SqlDbType.VarChar);
			param2.Value = DisplayCode;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@Name", SqlDbType.VarChar);
			param3.Value = Name;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Description", SqlDbType.VarChar);
			param4.Value = Description;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@ClasFact", SqlDbType.TinyInt);
			param5.Value = ClasFact;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@Activo", SqlDbType.Bit);
			param6.Value = Activo;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@UsuarioRegistro", SqlDbType.Int);
			param7.Value = UsuarioRegistro;
			commandParameters.Add(param7);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdFeeCat";
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

		#region Eliminar VBFac_Fee_Cat
		public void Eliminar(int idfee)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFee", SqlDbType.Int);
			param0.Value = idfee;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelFeeCat";
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

		#region Deshacer VBFac_Fee_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdFee);
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
		public List<ENTFeeCat> RecuperarTodo()
		{
			List<ENTFeeCat> result = new List<ENTFeeCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFeeCat_TODO";
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
				    ENTFeeCat item = new ENTFeeCat();
					 if (!dr.IsNull("IdFee")) item.IdFee = Convert.ToInt32(dr["IdFee"]);
					 if (!dr.IsNull("FeeCode")) item.FeeCode = dr["FeeCode"].ToString();
					 if (!dr.IsNull("DisplayCode")) item.DisplayCode = dr["DisplayCode"].ToString();
					 if (!dr.IsNull("Name")) item.Name = dr["Name"].ToString();
					 if (!dr.IsNull("Description")) item.Description = dr["Description"].ToString();
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

		#region Recuperar RecuperarFeeCatPorLlavePrimaria
		public List<ENTFeeCat> RecuperarFeeCatPorLlavePrimaria(int idfee)
		{
			List<ENTFeeCat> result = new List<ENTFeeCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFee", SqlDbType.Int);
			param0.Value = idfee;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFeeCat_POR_PK";
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
				    ENTFeeCat item = new ENTFeeCat();
					if (!dr.IsNull("IdFee")) item.IdFee = Convert.ToInt32(dr["IdFee"]);
					if (!dr.IsNull("FeeCode")) item.FeeCode = dr["FeeCode"].ToString();
					if (!dr.IsNull("DisplayCode")) item.DisplayCode = dr["DisplayCode"].ToString();
					if (!dr.IsNull("Name")) item.Name = dr["Name"].ToString();
					if (!dr.IsNull("Description")) item.Description = dr["Description"].ToString();
					if (!dr.IsNull("ClasFact")) item.ClasFact = Convert.ToByte(dr["ClasFact"]);
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("UsuarioRegistro")) item.UsuarioRegistro = Convert.ToInt32(dr["UsuarioRegistro"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFee")) IdFee = Convert.ToInt32(dtResultado.Rows[0]["IdFee"]);
					if (!dtResultado.Rows[0].IsNull("FeeCode")) FeeCode = dtResultado.Rows[0]["FeeCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("DisplayCode")) DisplayCode = dtResultado.Rows[0]["DisplayCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("Name")) Name = dtResultado.Rows[0]["Name"].ToString();
					if (!dtResultado.Rows[0].IsNull("Description")) Description = dtResultado.Rows[0]["Description"].ToString();
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
			_copia = new DALFeeCat(_conexion);
			Type tipo = typeof(ENTFeeCat);
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

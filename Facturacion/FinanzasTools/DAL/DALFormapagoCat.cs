using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace FinanzasTools
{
	public class DALFormapagoCat: ENTFormapagoCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALFormapagoCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALFormapagoCat(SqlConnection conexion)
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
			IdFormaPago = 0;
			PaymentMethodCode = String.Empty;
			Descripcion = String.Empty;
			CveFormaPagoSAT = String.Empty;
			EsFacturable = false;
			Activo = true;
			UsuarioRegistro = 0;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_FormaPago_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFormaPago", SqlDbType.Int);
			param0.Value = IdFormaPago;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@PaymentMethodCode", SqlDbType.VarChar);
			param1.Value = PaymentMethodCode;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param2.Value = Descripcion;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@CveFormaPagoSAT", SqlDbType.VarChar);
			param3.Value = CveFormaPagoSAT;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@EsFacturable", SqlDbType.Bit);
			param4.Value = EsFacturable;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Activo", SqlDbType.Bit);
			param5.Value = Activo;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@UsuarioRegistro", SqlDbType.Int);
			param6.Value = UsuarioRegistro;
			commandParameters.Add(param6);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsFormaPagoCat";
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
						IdFormaPago = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_FormaPago_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFormaPago", SqlDbType.Int);
			param0.Value = IdFormaPago;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@PaymentMethodCode", SqlDbType.VarChar);
			param1.Value = PaymentMethodCode;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Descripcion", SqlDbType.VarChar);
			param2.Value = Descripcion;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@CveFormaPagoSAT", SqlDbType.VarChar);
			param3.Value = CveFormaPagoSAT;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@EsFacturable", SqlDbType.Bit);
			param4.Value = EsFacturable;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Activo", SqlDbType.Bit);
			param5.Value = Activo;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@UsuarioRegistro", SqlDbType.Int);
			param6.Value = UsuarioRegistro;
			commandParameters.Add(param6);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdFormaPagoCat";
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

		#region Eliminar VBFac_FormaPago_Cat
		public void Eliminar(int idformapago)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFormaPago", SqlDbType.Int);
			param0.Value = idformapago;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelFormaPagoCat";
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

		#region Deshacer VBFac_FormaPago_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdFormaPago);
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
		public List<ENTFormapagoCat> RecuperarTodo()
		{
			List<ENTFormapagoCat> result = new List<ENTFormapagoCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFormaPagoCat_TODO";
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
				    ENTFormapagoCat item = new ENTFormapagoCat();
					 if (!dr.IsNull("IdFormaPago")) item.IdFormaPago = Convert.ToInt32(dr["IdFormaPago"]);
					 if (!dr.IsNull("PaymentMethodCode")) item.PaymentMethodCode = dr["PaymentMethodCode"].ToString();
					 if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					 if (!dr.IsNull("CveFormaPagoSAT")) item.CveFormaPagoSAT = dr["CveFormaPagoSAT"].ToString();
					 if (!dr.IsNull("EsFacturable")) item.EsFacturable = Convert.ToBoolean(dr["EsFacturable"]);
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

		#region Recuperar RecuperarFormapagoCatPorLlavePrimaria
		public List<ENTFormapagoCat> RecuperarFormapagoCatPorLlavePrimaria(int idformapago)
		{
			List<ENTFormapagoCat> result = new List<ENTFormapagoCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFormaPago", SqlDbType.Int);
			param0.Value = idformapago;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFormaPagoCat_POR_PK";
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
				    ENTFormapagoCat item = new ENTFormapagoCat();
					if (!dr.IsNull("IdFormaPago")) item.IdFormaPago = Convert.ToInt32(dr["IdFormaPago"]);
					if (!dr.IsNull("PaymentMethodCode")) item.PaymentMethodCode = dr["PaymentMethodCode"].ToString();
					if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
					if (!dr.IsNull("CveFormaPagoSAT")) item.CveFormaPagoSAT = dr["CveFormaPagoSAT"].ToString();
					if (!dr.IsNull("EsFacturable")) item.EsFacturable = Convert.ToBoolean(dr["EsFacturable"]);
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("UsuarioRegistro")) item.UsuarioRegistro = Convert.ToInt32(dr["UsuarioRegistro"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFormaPago")) IdFormaPago = Convert.ToInt32(dtResultado.Rows[0]["IdFormaPago"]);
					if (!dtResultado.Rows[0].IsNull("PaymentMethodCode")) PaymentMethodCode = dtResultado.Rows[0]["PaymentMethodCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("Descripcion")) Descripcion = dtResultado.Rows[0]["Descripcion"].ToString();
					if (!dtResultado.Rows[0].IsNull("CveFormaPagoSAT")) CveFormaPagoSAT = dtResultado.Rows[0]["CveFormaPagoSAT"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsFacturable")) EsFacturable = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturable"]);
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
			_copia = new DALFormapagoCat(_conexion);
			Type tipo = typeof(ENTFormapagoCat);
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

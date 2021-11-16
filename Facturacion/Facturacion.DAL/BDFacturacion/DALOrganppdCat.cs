using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALOrganppdCat: ENTOrganppdCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALOrganppdCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALOrganppdCat(SqlConnection conexion)
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
			IdOrganPPD = 0;
			OrganizationCode = String.Empty;
			OrganizationName = String.Empty;
			EsFacturaPPD = false;
			EsFacturaPagos = false;
			Activo = true;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_OrganPPD_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdOrganPPD", SqlDbType.Int);
			param0.Value = IdOrganPPD;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@OrganizationCode", SqlDbType.VarChar);
			param1.Value = OrganizationCode;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@OrganizationName", SqlDbType.VarChar);
			param2.Value = OrganizationName;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@EsFacturaPPD", SqlDbType.Bit);
			param3.Value = EsFacturaPPD;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@EsFacturaPagos", SqlDbType.Bit);
			param4.Value = EsFacturaPagos;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Activo", SqlDbType.Bit);
			param5.Value = Activo;
			commandParameters.Add(param5);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsOrganPPDCat";
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
						IdOrganPPD = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_OrganPPD_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdOrganPPD", SqlDbType.Int);
			param0.Value = IdOrganPPD;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@OrganizationCode", SqlDbType.VarChar);
			param1.Value = OrganizationCode;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@OrganizationName", SqlDbType.VarChar);
			param2.Value = OrganizationName;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@EsFacturaPPD", SqlDbType.Bit);
			param3.Value = EsFacturaPPD;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@EsFacturaPagos", SqlDbType.Bit);
			param4.Value = EsFacturaPagos;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Activo", SqlDbType.Bit);
			param5.Value = Activo;
			commandParameters.Add(param5);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdOrganPPDCat";
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

		#region Eliminar VBFac_OrganPPD_Cat
		public void Eliminar(int idorganppd)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdOrganPPD", SqlDbType.Int);
			param0.Value = idorganppd;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelOrganPPDCat";
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

		#region Deshacer VBFac_OrganPPD_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdOrganPPD);
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
		public List<ENTOrganppdCat> RecuperarTodo()
		{
			List<ENTOrganppdCat> result = new List<ENTOrganppdCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetOrganPPDCat_TODO";
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
				    ENTOrganppdCat item = new ENTOrganppdCat();
					 if (!dr.IsNull("IdOrganPPD")) item.IdOrganPPD = Convert.ToInt32(dr["IdOrganPPD"]);
					 if (!dr.IsNull("OrganizationCode")) item.OrganizationCode = dr["OrganizationCode"].ToString();
					 if (!dr.IsNull("OrganizationName")) item.OrganizationName = dr["OrganizationName"].ToString();
					 if (!dr.IsNull("EsFacturaPPD")) item.EsFacturaPPD = Convert.ToBoolean(dr["EsFacturaPPD"]);
					 if (!dr.IsNull("EsFacturaPagos")) item.EsFacturaPagos = Convert.ToBoolean(dr["EsFacturaPagos"]);
					 if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
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

		#region Recuperar RecuperarOrganppdCatPorLlavePrimaria
		public List<ENTOrganppdCat> RecuperarOrganppdCatPorLlavePrimaria(int idorganppd)
		{
			List<ENTOrganppdCat> result = new List<ENTOrganppdCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdOrganPPD", SqlDbType.Int);
			param0.Value = idorganppd;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetOrganPPDCat_POR_PK";
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
				    ENTOrganppdCat item = new ENTOrganppdCat();
					if (!dr.IsNull("IdOrganPPD")) item.IdOrganPPD = Convert.ToInt32(dr["IdOrganPPD"]);
					if (!dr.IsNull("OrganizationCode")) item.OrganizationCode = dr["OrganizationCode"].ToString();
					if (!dr.IsNull("OrganizationName")) item.OrganizationName = dr["OrganizationName"].ToString();
					if (!dr.IsNull("EsFacturaPPD")) item.EsFacturaPPD = Convert.ToBoolean(dr["EsFacturaPPD"]);
					if (!dr.IsNull("EsFacturaPagos")) item.EsFacturaPagos = Convert.ToBoolean(dr["EsFacturaPagos"]);
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdOrganPPD")) IdOrganPPD = Convert.ToInt32(dtResultado.Rows[0]["IdOrganPPD"]);
					if (!dtResultado.Rows[0].IsNull("OrganizationCode")) OrganizationCode = dtResultado.Rows[0]["OrganizationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("OrganizationName")) OrganizationName = dtResultado.Rows[0]["OrganizationName"].ToString();
					if (!dtResultado.Rows[0].IsNull("EsFacturaPPD")) EsFacturaPPD = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturaPPD"]);
					if (!dtResultado.Rows[0].IsNull("EsFacturaPagos")) EsFacturaPagos = Convert.ToBoolean(dtResultado.Rows[0]["EsFacturaPagos"]);
					if (!dtResultado.Rows[0].IsNull("Activo")) Activo = Convert.ToBoolean(dtResultado.Rows[0]["Activo"]);
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
			_copia = new DALOrganppdCat(_conexion);
			Type tipo = typeof(ENTOrganppdCat);
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

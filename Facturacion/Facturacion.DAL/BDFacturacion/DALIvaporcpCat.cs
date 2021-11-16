using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALIvaporcpCat: ENTIvaporcpCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALIvaporcpCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALIvaporcpCat(SqlConnection conexion)
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
			IdIvaPorCP = 0;
			CodigoPostal = String.Empty;
			PorcIVA = 0;
			EsFrontera = false;
			Activo = true;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_IVAPorCP_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdIvaPorCP", SqlDbType.Int);
			param0.Value = IdIvaPorCP;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@CodigoPostal", SqlDbType.VarChar);
			param1.Value = CodigoPostal;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@PorcIVA", SqlDbType.Int);
			param2.Value = PorcIVA;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@EsFrontera", SqlDbType.Bit);
			param3.Value = EsFrontera;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Activo", SqlDbType.Bit);
			param4.Value = Activo;
			commandParameters.Add(param4);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsIVAPorCPCat";
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
						IdIvaPorCP = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_IVAPorCP_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdIvaPorCP", SqlDbType.Int);
			param0.Value = IdIvaPorCP;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@CodigoPostal", SqlDbType.VarChar);
			param1.Value = CodigoPostal;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@PorcIVA", SqlDbType.Int);
			param2.Value = PorcIVA;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@EsFrontera", SqlDbType.Bit);
			param3.Value = EsFrontera;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Activo", SqlDbType.Bit);
			param4.Value = Activo;
			commandParameters.Add(param4);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdIVAPorCPCat";
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

		#region Eliminar VBFac_IVAPorCP_Cat
		public void Eliminar(int idivaporcp)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdIvaPorCP", SqlDbType.Int);
			param0.Value = idivaporcp;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelIVAPorCPCat";
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

		#region Deshacer VBFac_IVAPorCP_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdIvaPorCP);
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
		public List<ENTIvaporcpCat> RecuperarTodo()
		{
			List<ENTIvaporcpCat> result = new List<ENTIvaporcpCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetIVAPorCPCat_TODO";
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
				    ENTIvaporcpCat item = new ENTIvaporcpCat();
					 if (!dr.IsNull("IdIvaPorCP")) item.IdIvaPorCP = Convert.ToInt32(dr["IdIvaPorCP"]);
					 if (!dr.IsNull("CodigoPostal")) item.CodigoPostal = dr["CodigoPostal"].ToString();
					 if (!dr.IsNull("PorcIVA")) item.PorcIVA = Convert.ToInt32(dr["PorcIVA"]);
					 if (!dr.IsNull("EsFrontera")) item.EsFrontera = Convert.ToBoolean(dr["EsFrontera"]);
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

		#region Recuperar RecuperarIvaporcpCatPorLlavePrimaria
		public List<ENTIvaporcpCat> RecuperarIvaporcpCatPorLlavePrimaria(int idivaporcp)
		{
			List<ENTIvaporcpCat> result = new List<ENTIvaporcpCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdIvaPorCP", SqlDbType.Int);
			param0.Value = idivaporcp;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetIVAPorCPCat_POR_PK";
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
				    ENTIvaporcpCat item = new ENTIvaporcpCat();
					if (!dr.IsNull("IdIvaPorCP")) item.IdIvaPorCP = Convert.ToInt32(dr["IdIvaPorCP"]);
					if (!dr.IsNull("CodigoPostal")) item.CodigoPostal = dr["CodigoPostal"].ToString();
					if (!dr.IsNull("PorcIVA")) item.PorcIVA = Convert.ToInt32(dr["PorcIVA"]);
					if (!dr.IsNull("EsFrontera")) item.EsFrontera = Convert.ToBoolean(dr["EsFrontera"]);
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdIvaPorCP")) IdIvaPorCP = Convert.ToInt32(dtResultado.Rows[0]["IdIvaPorCP"]);
					if (!dtResultado.Rows[0].IsNull("CodigoPostal")) CodigoPostal = dtResultado.Rows[0]["CodigoPostal"].ToString();
					if (!dtResultado.Rows[0].IsNull("PorcIVA")) PorcIVA = Convert.ToInt32(dtResultado.Rows[0]["PorcIVA"]);
					if (!dtResultado.Rows[0].IsNull("EsFrontera")) EsFrontera = Convert.ToBoolean(dtResultado.Rows[0]["EsFrontera"]);
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
			_copia = new DALIvaporcpCat(_conexion);
			Type tipo = typeof(ENTIvaporcpCat);
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

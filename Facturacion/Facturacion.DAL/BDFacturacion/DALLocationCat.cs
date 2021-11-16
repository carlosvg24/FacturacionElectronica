using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALLocationCat: ENTLocationCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALLocationCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALLocationCat(SqlConnection conexion)
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
			IdLocation = 0;
			LocationCode = String.Empty;
			Name = String.Empty;
			LugarExpedicion = String.Empty;
			Activo = true;
			FechaHoraLocal = new DateTime();
			EsFranjaFronteriza = false;
			LugarExpPublicoGeneral = String.Empty;
		}
		#endregion

		#region Agregar VBFac_Location_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdLocation", SqlDbType.Int);
			param0.Value = IdLocation;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@LocationCode", SqlDbType.VarChar);
			param1.Value = LocationCode;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Name", SqlDbType.VarChar);
			param2.Value = Name;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@LugarExpedicion", SqlDbType.VarChar);
			param3.Value = LugarExpedicion;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Activo", SqlDbType.Bit);
			param4.Value = Activo;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@EsFranjaFronteriza", SqlDbType.Bit);
			param5.Value = EsFranjaFronteriza;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@LugarExpPublicoGeneral", SqlDbType.VarChar);
			param6.Value = LugarExpPublicoGeneral;
			commandParameters.Add(param6);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsLocationCat";
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
						IdLocation = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_Location_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdLocation", SqlDbType.Int);
			param0.Value = IdLocation;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@LocationCode", SqlDbType.VarChar);
			param1.Value = LocationCode;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Name", SqlDbType.VarChar);
			param2.Value = Name;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@LugarExpedicion", SqlDbType.VarChar);
			param3.Value = LugarExpedicion;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@Activo", SqlDbType.Bit);
			param4.Value = Activo;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@EsFranjaFronteriza", SqlDbType.Bit);
			param5.Value = EsFranjaFronteriza;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@LugarExpPublicoGeneral", SqlDbType.VarChar);
			param6.Value = LugarExpPublicoGeneral;
			commandParameters.Add(param6);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdLocationCat";
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

		#region Eliminar VBFac_Location_Cat
		public void Eliminar(int idlocation)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdLocation", SqlDbType.Int);
			param0.Value = idlocation;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelLocationCat";
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

		#region Deshacer VBFac_Location_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdLocation);
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
		public List<ENTLocationCat> RecuperarTodo()
		{
			List<ENTLocationCat> result = new List<ENTLocationCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetLocationCat_TODO";
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
				    ENTLocationCat item = new ENTLocationCat();
					 if (!dr.IsNull("IdLocation")) item.IdLocation = Convert.ToInt32(dr["IdLocation"]);
					 if (!dr.IsNull("LocationCode")) item.LocationCode = dr["LocationCode"].ToString();
					 if (!dr.IsNull("Name")) item.Name = dr["Name"].ToString();
					 if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					 if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					 if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					 if (!dr.IsNull("EsFranjaFronteriza")) item.EsFranjaFronteriza = Convert.ToBoolean(dr["EsFranjaFronteriza"]);
					 if (!dr.IsNull("LugarExpPublicoGeneral")) item.LugarExpPublicoGeneral = dr["LugarExpPublicoGeneral"].ToString();
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

		#region Recuperar RecuperarLocationCatPorLlavePrimaria
		public List<ENTLocationCat> RecuperarLocationCatPorLlavePrimaria(int idlocation)
		{
			List<ENTLocationCat> result = new List<ENTLocationCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdLocation", SqlDbType.Int);
			param0.Value = idlocation;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetLocationCat_POR_PK";
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
				    ENTLocationCat item = new ENTLocationCat();
					if (!dr.IsNull("IdLocation")) item.IdLocation = Convert.ToInt32(dr["IdLocation"]);
					if (!dr.IsNull("LocationCode")) item.LocationCode = dr["LocationCode"].ToString();
					if (!dr.IsNull("Name")) item.Name = dr["Name"].ToString();
					if (!dr.IsNull("LugarExpedicion")) item.LugarExpedicion = dr["LugarExpedicion"].ToString();
					if (!dr.IsNull("Activo")) item.Activo = Convert.ToBoolean(dr["Activo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
					if (!dr.IsNull("EsFranjaFronteriza")) item.EsFranjaFronteriza = Convert.ToBoolean(dr["EsFranjaFronteriza"]);
					if (!dr.IsNull("LugarExpPublicoGeneral")) item.LugarExpPublicoGeneral = dr["LugarExpPublicoGeneral"].ToString();
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdLocation")) IdLocation = Convert.ToInt32(dtResultado.Rows[0]["IdLocation"]);
					if (!dtResultado.Rows[0].IsNull("LocationCode")) LocationCode = dtResultado.Rows[0]["LocationCode"].ToString();
					if (!dtResultado.Rows[0].IsNull("Name")) Name = dtResultado.Rows[0]["Name"].ToString();
					if (!dtResultado.Rows[0].IsNull("LugarExpedicion")) LugarExpedicion = dtResultado.Rows[0]["LugarExpedicion"].ToString();
					if (!dtResultado.Rows[0].IsNull("Activo")) Activo = Convert.ToBoolean(dtResultado.Rows[0]["Activo"]);
					if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
					if (!dtResultado.Rows[0].IsNull("EsFranjaFronteriza")) EsFranjaFronteriza = Convert.ToBoolean(dtResultado.Rows[0]["EsFranjaFronteriza"]);
					if (!dtResultado.Rows[0].IsNull("LugarExpPublicoGeneral")) LugarExpPublicoGeneral = dtResultado.Rows[0]["LugarExpPublicoGeneral"].ToString();
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
			_copia = new DALLocationCat(_conexion);
			Type tipo = typeof(ENTLocationCat);
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

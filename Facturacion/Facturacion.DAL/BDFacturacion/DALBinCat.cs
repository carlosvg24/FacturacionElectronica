using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;


namespace Facturacion.DAL
{
	public class DALBinCat: ENTBinCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALBinCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALBinCat(SqlConnection conexion)
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
            BIN = 0D;
            MARCA = String.Empty;
			BANCO = String.Empty;
			TIPO = String.Empty;
			BANCO1 = String.Empty;
			COUNTRY = String.Empty;
			COUNTRY1 = String.Empty;
			CONUTRY = String.Empty;
		}
		#endregion

		#region Agregar VBFac_Bin_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@BIN", SqlDbType.Float);
			param0.Value = BIN;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@MARCA", SqlDbType.NVarChar);
			param1.Value = MARCA;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@BANCO", SqlDbType.NVarChar);
			param2.Value = BANCO;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@TIPO", SqlDbType.NVarChar);
			param3.Value = TIPO;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@BANCO1", SqlDbType.NVarChar);
			param4.Value = BANCO1;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@COUNTRY", SqlDbType.NVarChar);
			param5.Value = COUNTRY;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@COUNTRY1", SqlDbType.NVarChar);
			param6.Value = COUNTRY1;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@CONUTRY", SqlDbType.NVarChar);
			param7.Value = CONUTRY;
			commandParameters.Add(param7);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsBinCat";
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

		#region Actualizar VBFac_Bin_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@BIN", SqlDbType.Float);
			param0.Value = BIN;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@MARCA", SqlDbType.NVarChar);
			param1.Value = MARCA;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@BANCO", SqlDbType.NVarChar);
			param2.Value = BANCO;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@TIPO", SqlDbType.NVarChar);
			param3.Value = TIPO;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@BANCO1", SqlDbType.NVarChar);
			param4.Value = BANCO1;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@COUNTRY", SqlDbType.NVarChar);
			param5.Value = COUNTRY;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@COUNTRY1", SqlDbType.NVarChar);
			param6.Value = COUNTRY1;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@CONUTRY", SqlDbType.NVarChar);
			param7.Value = CONUTRY;
			commandParameters.Add(param7);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdBinCat";
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

		#region Eliminar VBFac_Bin_Cat
		public void Eliminar(double bin)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@BIN", SqlDbType.Float);
			param0.Value = bin;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelBinCat";
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

		#region Deshacer VBFac_Bin_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(BIN);
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
		public List<ENTBinCat> RecuperarTodo()
		{
			List<ENTBinCat> result = new List<ENTBinCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetBinCat_TODO";
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
				    ENTBinCat item = new ENTBinCat();
					 if (!dr.IsNull("BIN")) item.BIN = Convert.ToDouble(dr["BIN"]);
					 if (!dr.IsNull("MARCA")) item.MARCA = dr["MARCA"].ToString();
					 if (!dr.IsNull("BANCO")) item.BANCO = dr["BANCO"].ToString();
					 if (!dr.IsNull("TIPO")) item.TIPO = dr["TIPO"].ToString();
					 if (!dr.IsNull("BANCO1")) item.BANCO1 = dr["BANCO1"].ToString();
					 if (!dr.IsNull("COUNTRY")) item.COUNTRY = dr["COUNTRY"].ToString();
					 if (!dr.IsNull("COUNTRY1")) item.COUNTRY1 = dr["COUNTRY1"].ToString();
					 if (!dr.IsNull("CONUTRY")) item.CONUTRY = dr["CONUTRY"].ToString();
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

		#region Recuperar RecuperarBinCatPorLlavePrimaria
		public List<ENTBinCat> RecuperarBinCatPorLlavePrimaria(double bin)
		{
			List<ENTBinCat> result = new List<ENTBinCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@BIN", SqlDbType.Float);
			param0.Value = bin;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetBinCat_POR_PK";
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
				    ENTBinCat item = new ENTBinCat();
					if (!dr.IsNull("BIN")) item.BIN = Convert.ToDouble(dr["BIN"]);
					if (!dr.IsNull("MARCA")) item.MARCA = dr["MARCA"].ToString();
					if (!dr.IsNull("BANCO")) item.BANCO = dr["BANCO"].ToString();
					if (!dr.IsNull("TIPO")) item.TIPO = dr["TIPO"].ToString();
					if (!dr.IsNull("BANCO1")) item.BANCO1 = dr["BANCO1"].ToString();
					if (!dr.IsNull("COUNTRY")) item.COUNTRY = dr["COUNTRY"].ToString();
					if (!dr.IsNull("COUNTRY1")) item.COUNTRY1 = dr["COUNTRY1"].ToString();
					if (!dr.IsNull("CONUTRY")) item.CONUTRY = dr["CONUTRY"].ToString();
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("BIN")) BIN = Convert.ToDouble(dtResultado.Rows[0]["BIN"]);
					if (!dtResultado.Rows[0].IsNull("MARCA")) MARCA = dtResultado.Rows[0]["MARCA"].ToString();
					if (!dtResultado.Rows[0].IsNull("BANCO")) BANCO = dtResultado.Rows[0]["BANCO"].ToString();
					if (!dtResultado.Rows[0].IsNull("TIPO")) TIPO = dtResultado.Rows[0]["TIPO"].ToString();
					if (!dtResultado.Rows[0].IsNull("BANCO1")) BANCO1 = dtResultado.Rows[0]["BANCO1"].ToString();
					if (!dtResultado.Rows[0].IsNull("COUNTRY")) COUNTRY = dtResultado.Rows[0]["COUNTRY"].ToString();
					if (!dtResultado.Rows[0].IsNull("COUNTRY1")) COUNTRY1 = dtResultado.Rows[0]["COUNTRY1"].ToString();
					if (!dtResultado.Rows[0].IsNull("CONUTRY")) CONUTRY = dtResultado.Rows[0]["CONUTRY"].ToString();
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
			_copia = new DALBinCat(_conexion);
			Type tipo = typeof(ENTBinCat);
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

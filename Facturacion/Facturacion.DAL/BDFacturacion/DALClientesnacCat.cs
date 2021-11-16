using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALClientesnacCat: ENTClientesnacCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALClientesnacCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALClientesnacCat(SqlConnection conexion)
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
			IdCliente = 0;
			RFC = String.Empty;
			Nombre = String.Empty;
			UsoCFDI = String.Empty;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_ClientesNac_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdCliente", SqlDbType.Int);
			param0.Value = IdCliente;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@RFC", SqlDbType.VarChar);
			param1.Value = RFC;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Nombre", SqlDbType.VarChar);
			param2.Value = Nombre;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@UsoCFDI", SqlDbType.VarChar);
			param3.Value = UsoCFDI;
			commandParameters.Add(param3);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsClientesNacCat";
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
						IdCliente = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_ClientesNac_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdCliente", SqlDbType.Int);
			param0.Value = IdCliente;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@RFC", SqlDbType.VarChar);
			param1.Value = RFC;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Nombre", SqlDbType.VarChar);
			param2.Value = Nombre;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@UsoCFDI", SqlDbType.VarChar);
			param3.Value = UsoCFDI;
			commandParameters.Add(param3);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdClientesNacCat";
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

		#region Eliminar VBFac_ClientesNac_Cat
		public void Eliminar(int idcliente)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdCliente", SqlDbType.Int);
			param0.Value = idcliente;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelClientesNacCat";
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

		#region Deshacer VBFac_ClientesNac_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdCliente);
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
		public List<ENTClientesnacCat> RecuperarTodo()
		{
			List<ENTClientesnacCat> result = new List<ENTClientesnacCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetClientesNacCat_TODO";
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
				    ENTClientesnacCat item = new ENTClientesnacCat();
					 if (!dr.IsNull("IdCliente")) item.IdCliente = Convert.ToInt32(dr["IdCliente"]);
					 if (!dr.IsNull("RFC")) item.RFC = dr["RFC"].ToString();
					 if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					 if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
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

		#region Recuperar RecuperarClientesnacCatPorLlavePrimaria
		public List<ENTClientesnacCat> RecuperarClientesnacCatPorLlavePrimaria(int idcliente)
		{
			List<ENTClientesnacCat> result = new List<ENTClientesnacCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdCliente", SqlDbType.Int);
			param0.Value = idcliente;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetClientesNacCat_POR_PK";
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
				    ENTClientesnacCat item = new ENTClientesnacCat();
					if (!dr.IsNull("IdCliente")) item.IdCliente = Convert.ToInt32(dr["IdCliente"]);
					if (!dr.IsNull("RFC")) item.RFC = dr["RFC"].ToString();
					if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdCliente")) IdCliente = Convert.ToInt32(dtResultado.Rows[0]["IdCliente"]);
					if (!dtResultado.Rows[0].IsNull("RFC")) RFC = dtResultado.Rows[0]["RFC"].ToString();
					if (!dtResultado.Rows[0].IsNull("Nombre")) Nombre = dtResultado.Rows[0]["Nombre"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsoCFDI")) UsoCFDI = dtResultado.Rows[0]["UsoCFDI"].ToString();
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

		#region Recuperar RecuperarClientesnacCatRfc
		public List<ENTClientesnacCat> RecuperarClientesnacCatRfc(string rfc)
		{
			List<ENTClientesnacCat> result = new List<ENTClientesnacCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@RFC", SqlDbType.VarChar);
			param0.Value = rfc;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetClientesNacCat_POR_RFC";
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
				    ENTClientesnacCat item = new ENTClientesnacCat();
					if (!dr.IsNull("IdCliente")) item.IdCliente = Convert.ToInt32(dr["IdCliente"]);
					if (!dr.IsNull("RFC")) item.RFC = dr["RFC"].ToString();
					if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdCliente")) IdCliente = Convert.ToInt32(dtResultado.Rows[0]["IdCliente"]);
					if (!dtResultado.Rows[0].IsNull("RFC")) RFC = dtResultado.Rows[0]["RFC"].ToString();
					if (!dtResultado.Rows[0].IsNull("Nombre")) Nombre = dtResultado.Rows[0]["Nombre"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsoCFDI")) UsoCFDI = dtResultado.Rows[0]["UsoCFDI"].ToString();
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
			_copia = new DALClientesnacCat(_conexion);
			Type tipo = typeof(ENTClientesnacCat);
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

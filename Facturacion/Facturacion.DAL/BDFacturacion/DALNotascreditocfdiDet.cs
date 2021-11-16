using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALNotascreditocfdiDet: ENTNotascreditocfdiDet
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALNotascreditocfdiDet _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALNotascreditocfdiDet(SqlConnection conexion)
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
			IdNotaCreditoCab = 0;
			TransaccionID = String.Empty;
			CFDI = String.Empty;
			CadenaOriginal = String.Empty;
			FechaTimbrado = new DateTime();
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_NotasCreditoCFDI_Det
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdNotaCreditoCab", SqlDbType.BigInt);
			param0.Value = IdNotaCreditoCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@TransaccionID", SqlDbType.VarChar);
			param1.Value = TransaccionID;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@CFDI", SqlDbType.VarChar);
			param2.Value = CFDI;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@CadenaOriginal", SqlDbType.VarChar);
			param3.Value = CadenaOriginal;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@FechaTimbrado", SqlDbType.VarChar);
			param4.Value = FechaTimbrado.Year > 1900 ? FechaTimbrado.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param4);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsNotasCreditoCFDIDet";
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

		#region Actualizar VBFac_NotasCreditoCFDI_Det
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdNotaCreditoCab", SqlDbType.BigInt);
			param0.Value = IdNotaCreditoCab;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@TransaccionID", SqlDbType.VarChar);
			param1.Value = TransaccionID;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@CFDI", SqlDbType.VarChar);
			param2.Value = CFDI;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@CadenaOriginal", SqlDbType.VarChar);
			param3.Value = CadenaOriginal;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@FechaTimbrado", SqlDbType.VarChar);
			param4.Value = FechaTimbrado.Year > 1900 ? FechaTimbrado.ToString("yyyy-MM-dd HH:mm:ss.fff") : ""; 
			commandParameters.Add(param4);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdNotasCreditoCFDIDet";
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

		#region Eliminar VBFac_NotasCreditoCFDI_Det
		public void Eliminar(long idnotacreditocab)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdNotaCreditoCab", SqlDbType.BigInt);
			param0.Value = idnotacreditocab;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelNotasCreditoCFDIDet";
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

		#region Deshacer VBFac_NotasCreditoCFDI_Det
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdNotaCreditoCab);
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
		public List<ENTNotascreditocfdiDet> RecuperarTodo()
		{
			List<ENTNotascreditocfdiDet> result = new List<ENTNotascreditocfdiDet>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetNotasCreditoCFDIDet_TODO";
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
				    ENTNotascreditocfdiDet item = new ENTNotascreditocfdiDet();
					 if (!dr.IsNull("IdNotaCreditoCab")) item.IdNotaCreditoCab = Convert.ToInt64(dr["IdNotaCreditoCab"]);
					 if (!dr.IsNull("TransaccionID")) item.TransaccionID = dr["TransaccionID"].ToString();
					 if (!dr.IsNull("CFDI")) item.CFDI = dr["CFDI"].ToString();
					 if (!dr.IsNull("CadenaOriginal")) item.CadenaOriginal = dr["CadenaOriginal"].ToString();
					 if (!dr.IsNull("FechaTimbrado")) item.FechaTimbrado = Convert.ToDateTime(dr["FechaTimbrado"]);
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

		#region Recuperar RecuperarNotascreditocfdiDetPorLlavePrimaria
		public List<ENTNotascreditocfdiDet> RecuperarNotascreditocfdiDetPorLlavePrimaria(long idnotacreditocab)
		{
			List<ENTNotascreditocfdiDet> result = new List<ENTNotascreditocfdiDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdNotaCreditoCab", SqlDbType.BigInt);
			param0.Value = idnotacreditocab;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetNotasCreditoCFDIDet_POR_PK";
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
				    ENTNotascreditocfdiDet item = new ENTNotascreditocfdiDet();
					if (!dr.IsNull("IdNotaCreditoCab")) item.IdNotaCreditoCab = Convert.ToInt64(dr["IdNotaCreditoCab"]);
					if (!dr.IsNull("TransaccionID")) item.TransaccionID = dr["TransaccionID"].ToString();
					if (!dr.IsNull("CFDI")) item.CFDI = dr["CFDI"].ToString();
					if (!dr.IsNull("CadenaOriginal")) item.CadenaOriginal = dr["CadenaOriginal"].ToString();
					if (!dr.IsNull("FechaTimbrado")) item.FechaTimbrado = Convert.ToDateTime(dr["FechaTimbrado"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdNotaCreditoCab")) IdNotaCreditoCab = Convert.ToInt64(dtResultado.Rows[0]["IdNotaCreditoCab"]);
					if (!dtResultado.Rows[0].IsNull("TransaccionID")) TransaccionID = dtResultado.Rows[0]["TransaccionID"].ToString();
					if (!dtResultado.Rows[0].IsNull("CFDI")) CFDI = dtResultado.Rows[0]["CFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("CadenaOriginal")) CadenaOriginal = dtResultado.Rows[0]["CadenaOriginal"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaTimbrado")) FechaTimbrado = Convert.ToDateTime(dtResultado.Rows[0]["FechaTimbrado"]);
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

		#region Recuperar RecuperarNotascreditocfdiDetIdnotacreditocab
		public List<ENTNotascreditocfdiDet> RecuperarNotascreditocfdiDetIdnotacreditocab(long idnotacreditocab)
		{
			List<ENTNotascreditocfdiDet> result = new List<ENTNotascreditocfdiDet>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdNotaCreditoCab", SqlDbType.BigInt);
			param0.Value = idnotacreditocab;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetNotasCreditoCFDIDet_POR_IdNotaCreditoCab";
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
				    ENTNotascreditocfdiDet item = new ENTNotascreditocfdiDet();
					if (!dr.IsNull("IdNotaCreditoCab")) item.IdNotaCreditoCab = Convert.ToInt64(dr["IdNotaCreditoCab"]);
					if (!dr.IsNull("TransaccionID")) item.TransaccionID = dr["TransaccionID"].ToString();
					if (!dr.IsNull("CFDI")) item.CFDI = dr["CFDI"].ToString();
					if (!dr.IsNull("CadenaOriginal")) item.CadenaOriginal = dr["CadenaOriginal"].ToString();
					if (!dr.IsNull("FechaTimbrado")) item.FechaTimbrado = Convert.ToDateTime(dr["FechaTimbrado"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdNotaCreditoCab")) IdNotaCreditoCab = Convert.ToInt64(dtResultado.Rows[0]["IdNotaCreditoCab"]);
					if (!dtResultado.Rows[0].IsNull("TransaccionID")) TransaccionID = dtResultado.Rows[0]["TransaccionID"].ToString();
					if (!dtResultado.Rows[0].IsNull("CFDI")) CFDI = dtResultado.Rows[0]["CFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("CadenaOriginal")) CadenaOriginal = dtResultado.Rows[0]["CadenaOriginal"].ToString();
					if (!dtResultado.Rows[0].IsNull("FechaTimbrado")) FechaTimbrado = Convert.ToDateTime(dtResultado.Rows[0]["FechaTimbrado"]);
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
			_copia = new DALNotascreditocfdiDet(_conexion);
			Type tipo = typeof(ENTNotascreditocfdiDet);
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

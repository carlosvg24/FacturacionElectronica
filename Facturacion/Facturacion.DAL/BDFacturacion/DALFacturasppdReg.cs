using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALFacturasppdReg: ENTFacturasppdReg
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALFacturasppdReg _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALFacturasppdReg(SqlConnection conexion)
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
			IdFacturaPPD = 0;
			IdFacturaCab = 0;
			IdReservaCab = 0;
			BookingId = 0;
			MontoTotalFactura = 0M;
			MontoPagado = 0M;
			MontoSaldo = 0M;
			MontoNC = 0M;
			NumParcialidad = 0;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_FacturasPPD_Reg
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaPPD", SqlDbType.BigInt);
			param0.Value = IdFacturaPPD;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param1.Value = IdFacturaCab;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@IdReservaCab", SqlDbType.BigInt);
			param2.Value = IdReservaCab;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@BookingId", SqlDbType.BigInt);
			param3.Value = BookingId;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@MontoTotalFactura", SqlDbType.Money);
			param4.Value = MontoTotalFactura;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@MontoPagado", SqlDbType.Money);
			param5.Value = MontoPagado;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@MontoSaldo", SqlDbType.Money);
			param6.Value = MontoSaldo;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@MontoNC", SqlDbType.Money);
			param7.Value = MontoNC;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@NumParcialidad", SqlDbType.TinyInt);
			param8.Value = NumParcialidad;
			commandParameters.Add(param8);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsFacturasPPDReg";
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
						IdFacturaPPD = Convert.ToInt64(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_FacturasPPD_Reg
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaPPD", SqlDbType.BigInt);
			param0.Value = IdFacturaPPD;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param1.Value = IdFacturaCab;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@IdReservaCab", SqlDbType.BigInt);
			param2.Value = IdReservaCab;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@BookingId", SqlDbType.BigInt);
			param3.Value = BookingId;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@MontoTotalFactura", SqlDbType.Money);
			param4.Value = MontoTotalFactura;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@MontoPagado", SqlDbType.Money);
			param5.Value = MontoPagado;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@MontoSaldo", SqlDbType.Money);
			param6.Value = MontoSaldo;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@MontoNC", SqlDbType.Money);
			param7.Value = MontoNC;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@NumParcialidad", SqlDbType.TinyInt);
			param8.Value = NumParcialidad;
			commandParameters.Add(param8);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdFacturasPPDReg";
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

		#region Eliminar VBFac_FacturasPPD_Reg
		public void Eliminar(long idfacturappd)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@IdFacturaPPD", SqlDbType.BigInt);
			param0.Value = idfacturappd;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelFacturasPPDReg";
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

		#region Deshacer VBFac_FacturasPPD_Reg
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(IdFacturaPPD);
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
		public List<ENTFacturasppdReg> RecuperarTodo()
		{
			List<ENTFacturasppdReg> result = new List<ENTFacturasppdReg>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasPPDReg_TODO";
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
				    ENTFacturasppdReg item = new ENTFacturasppdReg();
					 if (!dr.IsNull("IdFacturaPPD")) item.IdFacturaPPD = Convert.ToInt64(dr["IdFacturaPPD"]);
					 if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					 if (!dr.IsNull("IdReservaCab")) item.IdReservaCab = Convert.ToInt64(dr["IdReservaCab"]);
					 if (!dr.IsNull("BookingId")) item.BookingId = Convert.ToInt64(dr["BookingId"]);
					 if (!dr.IsNull("MontoTotalFactura")) item.MontoTotalFactura = Convert.ToDecimal(dr["MontoTotalFactura"]);
					 if (!dr.IsNull("MontoPagado")) item.MontoPagado = Convert.ToDecimal(dr["MontoPagado"]);
					 if (!dr.IsNull("MontoSaldo")) item.MontoSaldo = Convert.ToDecimal(dr["MontoSaldo"]);
					 if (!dr.IsNull("MontoNC")) item.MontoNC = Convert.ToDecimal(dr["MontoNC"]);
					 if (!dr.IsNull("NumParcialidad")) item.NumParcialidad = Convert.ToByte(dr["NumParcialidad"]);
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

		#region Recuperar RecuperarFacturasppdRegPorLlavePrimaria
		public List<ENTFacturasppdReg> RecuperarFacturasppdRegPorLlavePrimaria(long idfacturappd)
		{
			List<ENTFacturasppdReg> result = new List<ENTFacturasppdReg>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFacturaPPD", SqlDbType.BigInt);
			param0.Value = idfacturappd;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasPPDReg_POR_PK";
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
				    ENTFacturasppdReg item = new ENTFacturasppdReg();
					if (!dr.IsNull("IdFacturaPPD")) item.IdFacturaPPD = Convert.ToInt64(dr["IdFacturaPPD"]);
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdReservaCab")) item.IdReservaCab = Convert.ToInt64(dr["IdReservaCab"]);
					if (!dr.IsNull("BookingId")) item.BookingId = Convert.ToInt64(dr["BookingId"]);
					if (!dr.IsNull("MontoTotalFactura")) item.MontoTotalFactura = Convert.ToDecimal(dr["MontoTotalFactura"]);
					if (!dr.IsNull("MontoPagado")) item.MontoPagado = Convert.ToDecimal(dr["MontoPagado"]);
					if (!dr.IsNull("MontoSaldo")) item.MontoSaldo = Convert.ToDecimal(dr["MontoSaldo"]);
					if (!dr.IsNull("MontoNC")) item.MontoNC = Convert.ToDecimal(dr["MontoNC"]);
					if (!dr.IsNull("NumParcialidad")) item.NumParcialidad = Convert.ToByte(dr["NumParcialidad"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaPPD")) IdFacturaPPD = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaPPD"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdReservaCab")) IdReservaCab = Convert.ToInt64(dtResultado.Rows[0]["IdReservaCab"]);
					if (!dtResultado.Rows[0].IsNull("BookingId")) BookingId = Convert.ToInt64(dtResultado.Rows[0]["BookingId"]);
					if (!dtResultado.Rows[0].IsNull("MontoTotalFactura")) MontoTotalFactura = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotalFactura"]);
					if (!dtResultado.Rows[0].IsNull("MontoPagado")) MontoPagado = Convert.ToDecimal(dtResultado.Rows[0]["MontoPagado"]);
					if (!dtResultado.Rows[0].IsNull("MontoSaldo")) MontoSaldo = Convert.ToDecimal(dtResultado.Rows[0]["MontoSaldo"]);
					if (!dtResultado.Rows[0].IsNull("MontoNC")) MontoNC = Convert.ToDecimal(dtResultado.Rows[0]["MontoNC"]);
					if (!dtResultado.Rows[0].IsNull("NumParcialidad")) NumParcialidad = Convert.ToByte(dtResultado.Rows[0]["NumParcialidad"]);
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

		#region Recuperar RecuperarFacturasppdRegIdfacturacab
		public List<ENTFacturasppdReg> RecuperarFacturasppdRegIdfacturacab(long idfacturacab)
		{
			List<ENTFacturasppdReg> result = new List<ENTFacturasppdReg>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdFacturaCab", SqlDbType.BigInt);
			param0.Value = idfacturacab;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasPPDReg_POR_IdFacturaCab";
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
				    ENTFacturasppdReg item = new ENTFacturasppdReg();
					if (!dr.IsNull("IdFacturaPPD")) item.IdFacturaPPD = Convert.ToInt64(dr["IdFacturaPPD"]);
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdReservaCab")) item.IdReservaCab = Convert.ToInt64(dr["IdReservaCab"]);
					if (!dr.IsNull("BookingId")) item.BookingId = Convert.ToInt64(dr["BookingId"]);
					if (!dr.IsNull("MontoTotalFactura")) item.MontoTotalFactura = Convert.ToDecimal(dr["MontoTotalFactura"]);
					if (!dr.IsNull("MontoPagado")) item.MontoPagado = Convert.ToDecimal(dr["MontoPagado"]);
					if (!dr.IsNull("MontoSaldo")) item.MontoSaldo = Convert.ToDecimal(dr["MontoSaldo"]);
					if (!dr.IsNull("MontoNC")) item.MontoNC = Convert.ToDecimal(dr["MontoNC"]);
					if (!dr.IsNull("NumParcialidad")) item.NumParcialidad = Convert.ToByte(dr["NumParcialidad"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaPPD")) IdFacturaPPD = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaPPD"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdReservaCab")) IdReservaCab = Convert.ToInt64(dtResultado.Rows[0]["IdReservaCab"]);
					if (!dtResultado.Rows[0].IsNull("BookingId")) BookingId = Convert.ToInt64(dtResultado.Rows[0]["BookingId"]);
					if (!dtResultado.Rows[0].IsNull("MontoTotalFactura")) MontoTotalFactura = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotalFactura"]);
					if (!dtResultado.Rows[0].IsNull("MontoPagado")) MontoPagado = Convert.ToDecimal(dtResultado.Rows[0]["MontoPagado"]);
					if (!dtResultado.Rows[0].IsNull("MontoSaldo")) MontoSaldo = Convert.ToDecimal(dtResultado.Rows[0]["MontoSaldo"]);
					if (!dtResultado.Rows[0].IsNull("MontoNC")) MontoNC = Convert.ToDecimal(dtResultado.Rows[0]["MontoNC"]);
					if (!dtResultado.Rows[0].IsNull("NumParcialidad")) NumParcialidad = Convert.ToByte(dtResultado.Rows[0]["NumParcialidad"]);
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

		#region Recuperar RecuperarFacturasppdRegIdreservacab
		public List<ENTFacturasppdReg> RecuperarFacturasppdRegIdreservacab(long idreservacab)
		{
			List<ENTFacturasppdReg> result = new List<ENTFacturasppdReg>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@IdReservaCab", SqlDbType.BigInt);
			param0.Value = idreservacab;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetFacturasPPDReg_POR_IdReservaCab";
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
				    ENTFacturasppdReg item = new ENTFacturasppdReg();
					if (!dr.IsNull("IdFacturaPPD")) item.IdFacturaPPD = Convert.ToInt64(dr["IdFacturaPPD"]);
					if (!dr.IsNull("IdFacturaCab")) item.IdFacturaCab = Convert.ToInt64(dr["IdFacturaCab"]);
					if (!dr.IsNull("IdReservaCab")) item.IdReservaCab = Convert.ToInt64(dr["IdReservaCab"]);
					if (!dr.IsNull("BookingId")) item.BookingId = Convert.ToInt64(dr["BookingId"]);
					if (!dr.IsNull("MontoTotalFactura")) item.MontoTotalFactura = Convert.ToDecimal(dr["MontoTotalFactura"]);
					if (!dr.IsNull("MontoPagado")) item.MontoPagado = Convert.ToDecimal(dr["MontoPagado"]);
					if (!dr.IsNull("MontoSaldo")) item.MontoSaldo = Convert.ToDecimal(dr["MontoSaldo"]);
					if (!dr.IsNull("MontoNC")) item.MontoNC = Convert.ToDecimal(dr["MontoNC"]);
					if (!dr.IsNull("NumParcialidad")) item.NumParcialidad = Convert.ToByte(dr["NumParcialidad"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("IdFacturaPPD")) IdFacturaPPD = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaPPD"]);
					if (!dtResultado.Rows[0].IsNull("IdFacturaCab")) IdFacturaCab = Convert.ToInt64(dtResultado.Rows[0]["IdFacturaCab"]);
					if (!dtResultado.Rows[0].IsNull("IdReservaCab")) IdReservaCab = Convert.ToInt64(dtResultado.Rows[0]["IdReservaCab"]);
					if (!dtResultado.Rows[0].IsNull("BookingId")) BookingId = Convert.ToInt64(dtResultado.Rows[0]["BookingId"]);
					if (!dtResultado.Rows[0].IsNull("MontoTotalFactura")) MontoTotalFactura = Convert.ToDecimal(dtResultado.Rows[0]["MontoTotalFactura"]);
					if (!dtResultado.Rows[0].IsNull("MontoPagado")) MontoPagado = Convert.ToDecimal(dtResultado.Rows[0]["MontoPagado"]);
					if (!dtResultado.Rows[0].IsNull("MontoSaldo")) MontoSaldo = Convert.ToDecimal(dtResultado.Rows[0]["MontoSaldo"]);
					if (!dtResultado.Rows[0].IsNull("MontoNC")) MontoNC = Convert.ToDecimal(dtResultado.Rows[0]["MontoNC"]);
					if (!dtResultado.Rows[0].IsNull("NumParcialidad")) NumParcialidad = Convert.ToByte(dtResultado.Rows[0]["NumParcialidad"]);
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
			_copia = new DALFacturasppdReg(_conexion);
			Type tipo = typeof(ENTFacturasppdReg);
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

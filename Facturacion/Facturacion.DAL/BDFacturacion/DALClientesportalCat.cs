using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Facturacion.ENT;

namespace Facturacion.DAL
{
	public class DALClientesportalCat: ENTClientesportalCat
	{

		#region Propiedades Privadas
		protected SqlConnection _conexion;
		protected bool _nuevo = false;
		protected string _idUsuario;
		protected string _proceso;
		protected DALClientesportalCat _copia;
		#endregion Propiedades Privadas

		#region Constructores
		public DALClientesportalCat(SqlConnection conexion)
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
			Id = 0;
			Email = String.Empty;
			Contrasenia = String.Empty;
			RFC = String.Empty;
			TAXID = String.Empty;
			Nombre = String.Empty;
			UsoCFDI = String.Empty;
			FormaPago = String.Empty;
			Pais = String.Empty;
			ClienteTipoId = 0;
			CodigoVerificacion = new Guid();
			UsuarioVerificado = false;
			UsuarioActivo = false;
			FechaHoraLocal = new DateTime();
		}
		#endregion

		#region Agregar VBFac_ClientesPortal_Cat
		public void Agregar()
		{
			_nuevo = true;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@Id", SqlDbType.Int);
			param0.Value = Id;
			param0.Direction = ParameterDirection.InputOutput;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@Email", SqlDbType.VarChar);
			param1.Value = Email;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Contrasenia", SqlDbType.VarChar);
			param2.Value = Contrasenia;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@RFC", SqlDbType.VarChar);
			param3.Value = RFC;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@TAXID", SqlDbType.VarChar);
			param4.Value = TAXID;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Nombre", SqlDbType.VarChar);
			param5.Value = Nombre;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@UsoCFDI", SqlDbType.VarChar);
			param6.Value = UsoCFDI;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@FormaPago", SqlDbType.VarChar);
			param7.Value = FormaPago;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@Pais", SqlDbType.VarChar);
			param8.Value = Pais;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@ClienteTipoId", SqlDbType.Int);
			param9.Value = ClienteTipoId;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@CodigoVerificacion", SqlDbType.UniqueIdentifier);
			param10.Value = CodigoVerificacion;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@UsuarioVerificado", SqlDbType.Bit);
			param11.Value = UsuarioVerificado;
			commandParameters.Add(param11);
			SqlParameter param12 = new SqlParameter("@UsuarioActivo", SqlDbType.Bit);
			param12.Value = UsuarioActivo;
			commandParameters.Add(param12);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_InsClientesPortalCat";
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
						Id = Convert.ToInt32(cmm.Parameters[0].Value);

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

		#region Actualizar VBFac_ClientesPortal_Cat
		public void Actualizar()
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@Id", SqlDbType.Int);
			param0.Value = Id;
			commandParameters.Add(param0);
			SqlParameter param1 = new SqlParameter("@Email", SqlDbType.VarChar);
			param1.Value = Email;
			commandParameters.Add(param1);
			SqlParameter param2 = new SqlParameter("@Contrasenia", SqlDbType.VarChar);
			param2.Value = Contrasenia;
			commandParameters.Add(param2);
			SqlParameter param3 = new SqlParameter("@RFC", SqlDbType.VarChar);
			param3.Value = RFC;
			commandParameters.Add(param3);
			SqlParameter param4 = new SqlParameter("@TAXID", SqlDbType.VarChar);
			param4.Value = TAXID;
			commandParameters.Add(param4);
			SqlParameter param5 = new SqlParameter("@Nombre", SqlDbType.VarChar);
			param5.Value = Nombre;
			commandParameters.Add(param5);
			SqlParameter param6 = new SqlParameter("@UsoCFDI", SqlDbType.VarChar);
			param6.Value = UsoCFDI;
			commandParameters.Add(param6);
			SqlParameter param7 = new SqlParameter("@FormaPago", SqlDbType.VarChar);
			param7.Value = FormaPago;
			commandParameters.Add(param7);
			SqlParameter param8 = new SqlParameter("@Pais", SqlDbType.VarChar);
			param8.Value = Pais;
			commandParameters.Add(param8);
			SqlParameter param9 = new SqlParameter("@ClienteTipoId", SqlDbType.Int);
			param9.Value = ClienteTipoId;
			commandParameters.Add(param9);
			SqlParameter param10 = new SqlParameter("@CodigoVerificacion", SqlDbType.UniqueIdentifier);
			param10.Value = CodigoVerificacion;
			commandParameters.Add(param10);
			SqlParameter param11 = new SqlParameter("@UsuarioVerificado", SqlDbType.Bit);
			param11.Value = UsuarioVerificado;
			commandParameters.Add(param11);
			SqlParameter param12 = new SqlParameter("@UsuarioActivo", SqlDbType.Bit);
			param12.Value = UsuarioActivo;
			commandParameters.Add(param12);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_UpdClientesPortalCat";
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

        public bool ActivarUsuario()
        {
            bool seDaAlta = false;
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlCommand cmm = new SqlCommand();

            SqlParameter param0 = new SqlParameter("@CodigoVerificacion", SqlDbType.UniqueIdentifier);
            param0.Value = CodigoVerificacion;
            commandParameters.Add(param0);
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_UpdClienteActivacion";
            cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
            cmm.Connection = _conexion;
            bool cerrarConexion = false;
            foreach (SqlParameter p in commandParameters)
            {
                cmm.Parameters.Add(p);
            }

            try
            {
                if (_conexion.State.Equals(ConnectionState.Closed))
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

                    seDaAlta = true;
                }                
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmm.Dispose();
                if (cerrarConexion)
                {
                    _conexion.Close();
                }
            }

            return seDaAlta;
        }
        #endregion

        #region Eliminar VBFac_ClientesPortal_Cat
        public void Eliminar(int id)
		{
			_nuevo = false;
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlCommand cmm = new SqlCommand();
			SqlParameter param0 = new SqlParameter("@Id", SqlDbType.Int);
			param0.Value = id;
			commandParameters.Add(param0);
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_DelClientesPortalCat";
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

		#region Deshacer VBFac_ClientesPortal_Cat
		public void Deshacer()
		{
			if (_nuevo)
			{
				this.Eliminar(Id);
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
		public List<ENTClientesportalCat> RecuperarTodo()
		{
			List<ENTClientesportalCat> result = new List<ENTClientesportalCat>();
			DataTable dtResultado = new DataTable();
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetClientesPortalCat_TODO";
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
				    ENTClientesportalCat item = new ENTClientesportalCat();
					 if (!dr.IsNull("Id")) item.Id = Convert.ToInt32(dr["Id"]);
					 if (!dr.IsNull("Email")) item.Email = dr["Email"].ToString();
					 if (!dr.IsNull("Contrasenia")) item.Contrasenia = dr["Contrasenia"].ToString();
					 if (!dr.IsNull("RFC")) item.RFC = dr["RFC"].ToString();
					 if (!dr.IsNull("TAXID")) item.TAXID = dr["TAXID"].ToString();
					 if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					 if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					 if (!dr.IsNull("FormaPago")) item.FormaPago = dr["FormaPago"].ToString();
					 if (!dr.IsNull("Pais")) item.Pais = dr["Pais"].ToString();
					 if (!dr.IsNull("ClienteTipoId")) item.ClienteTipoId = Convert.ToInt32(dr["ClienteTipoId"]);
					 if (!dr.IsNull("CodigoVerificacion")) item.CodigoVerificacion = Guid.Parse((dr["CodigoVerificacion"]).ToString());
					 if (!dr.IsNull("UsuarioVerificado")) item.UsuarioVerificado = Convert.ToBoolean(dr["UsuarioVerificado"]);
					 if (!dr.IsNull("UsuarioActivo")) item.UsuarioActivo = Convert.ToBoolean(dr["UsuarioActivo"]);
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

		#region Recuperar RecuperarClientesportalCatPorLlavePrimaria
		public List<ENTClientesportalCat> RecuperarClientesportalCatPorLlavePrimaria(int id)
		{
			List<ENTClientesportalCat> result = new List<ENTClientesportalCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@Id", SqlDbType.Int);
			param0.Value = id;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetClientesPortalCat_POR_PK";
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
				    ENTClientesportalCat item = new ENTClientesportalCat();
					if (!dr.IsNull("Id")) item.Id = Convert.ToInt32(dr["Id"]);
					if (!dr.IsNull("Email")) item.Email = dr["Email"].ToString();
					if (!dr.IsNull("Contrasenia")) item.Contrasenia = dr["Contrasenia"].ToString();
					if (!dr.IsNull("RFC")) item.RFC = dr["RFC"].ToString();
					if (!dr.IsNull("TAXID")) item.TAXID = dr["TAXID"].ToString();
					if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					if (!dr.IsNull("FormaPago")) item.FormaPago = dr["FormaPago"].ToString();
					if (!dr.IsNull("Pais")) item.Pais = dr["Pais"].ToString();
					if (!dr.IsNull("ClienteTipoId")) item.ClienteTipoId = Convert.ToInt32(dr["ClienteTipoId"]);
					if (!dr.IsNull("CodigoVerificacion")) item.CodigoVerificacion = Guid.Parse(dr["CodigoVerificacion"].ToString());
                    if (!dr.IsNull("UsuarioVerificado")) item.UsuarioVerificado = Convert.ToBoolean(dr["UsuarioVerificado"]);
					if (!dr.IsNull("UsuarioActivo")) item.UsuarioActivo = Convert.ToBoolean(dr["UsuarioActivo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("Id")) Id = Convert.ToInt32(dtResultado.Rows[0]["Id"]);
					if (!dtResultado.Rows[0].IsNull("Email")) Email = dtResultado.Rows[0]["Email"].ToString();
					if (!dtResultado.Rows[0].IsNull("Contrasenia")) Contrasenia = dtResultado.Rows[0]["Contrasenia"].ToString();
					if (!dtResultado.Rows[0].IsNull("RFC")) RFC = dtResultado.Rows[0]["RFC"].ToString();
					if (!dtResultado.Rows[0].IsNull("TAXID")) TAXID = dtResultado.Rows[0]["TAXID"].ToString();
					if (!dtResultado.Rows[0].IsNull("Nombre")) Nombre = dtResultado.Rows[0]["Nombre"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsoCFDI")) UsoCFDI = dtResultado.Rows[0]["UsoCFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("FormaPago")) FormaPago = dtResultado.Rows[0]["FormaPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("Pais")) Pais = dtResultado.Rows[0]["Pais"].ToString();
					if (!dtResultado.Rows[0].IsNull("ClienteTipoId")) ClienteTipoId = Convert.ToInt32(dtResultado.Rows[0]["ClienteTipoId"]);
					if (!dtResultado.Rows[0].IsNull("CodigoVerificacion")) CodigoVerificacion = Guid.Parse(dtResultado.Rows[0]["CodigoVerificacion"].ToString());
					if (!dtResultado.Rows[0].IsNull("UsuarioVerificado")) UsuarioVerificado = Convert.ToBoolean(dtResultado.Rows[0]["UsuarioVerificado"]);
					if (!dtResultado.Rows[0].IsNull("UsuarioActivo")) UsuarioActivo = Convert.ToBoolean(dtResultado.Rows[0]["UsuarioActivo"]);
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

		#region Recuperar RecuperarClientesportalCatEmail
		public List<ENTClientesportalCat> RecuperarClientesportalCatEmail(string email)
		{
			List<ENTClientesportalCat> result = new List<ENTClientesportalCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@Email", SqlDbType.VarChar);
			param0.Value = email;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetClientesPortalCat_POR_Email";
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
				    ENTClientesportalCat item = new ENTClientesportalCat();
					if (!dr.IsNull("Id")) item.Id = Convert.ToInt32(dr["Id"]);
					if (!dr.IsNull("Email")) item.Email = dr["Email"].ToString();
					if (!dr.IsNull("Contrasenia")) item.Contrasenia = dr["Contrasenia"].ToString();
					if (!dr.IsNull("RFC")) item.RFC = dr["RFC"].ToString();
					if (!dr.IsNull("TAXID")) item.TAXID = dr["TAXID"].ToString();
					if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					if (!dr.IsNull("FormaPago")) item.FormaPago = dr["FormaPago"].ToString();
					if (!dr.IsNull("Pais")) item.Pais = dr["Pais"].ToString();
					if (!dr.IsNull("ClienteTipoId")) item.ClienteTipoId = Convert.ToInt32(dr["ClienteTipoId"]);
					if (!dr.IsNull("CodigoVerificacion")) item.CodigoVerificacion = Guid.Parse(dr["CodigoVerificacion"].ToString());
					if (!dr.IsNull("UsuarioVerificado")) item.UsuarioVerificado = Convert.ToBoolean(dr["UsuarioVerificado"]);
					if (!dr.IsNull("UsuarioActivo")) item.UsuarioActivo = Convert.ToBoolean(dr["UsuarioActivo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("Id")) Id = Convert.ToInt32(dtResultado.Rows[0]["Id"]);
					if (!dtResultado.Rows[0].IsNull("Email")) Email = dtResultado.Rows[0]["Email"].ToString();
					if (!dtResultado.Rows[0].IsNull("Contrasenia")) Contrasenia = dtResultado.Rows[0]["Contrasenia"].ToString();
					if (!dtResultado.Rows[0].IsNull("RFC")) RFC = dtResultado.Rows[0]["RFC"].ToString();
					if (!dtResultado.Rows[0].IsNull("TAXID")) TAXID = dtResultado.Rows[0]["TAXID"].ToString();
					if (!dtResultado.Rows[0].IsNull("Nombre")) Nombre = dtResultado.Rows[0]["Nombre"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsoCFDI")) UsoCFDI = dtResultado.Rows[0]["UsoCFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("FormaPago")) FormaPago = dtResultado.Rows[0]["FormaPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("Pais")) Pais = dtResultado.Rows[0]["Pais"].ToString();
					if (!dtResultado.Rows[0].IsNull("ClienteTipoId")) ClienteTipoId = Convert.ToInt32(dtResultado.Rows[0]["ClienteTipoId"]);
					if (!dtResultado.Rows[0].IsNull("CodigoVerificacion")) CodigoVerificacion = Guid.Parse(dtResultado.Rows[0]["CodigoVerificacion"].ToString());
					if (!dtResultado.Rows[0].IsNull("UsuarioVerificado")) UsuarioVerificado = Convert.ToBoolean(dtResultado.Rows[0]["UsuarioVerificado"]);
					if (!dtResultado.Rows[0].IsNull("UsuarioActivo")) UsuarioActivo = Convert.ToBoolean(dtResultado.Rows[0]["UsuarioActivo"]);
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

		#region Recuperar RecuperarClientesportalCatCodigoverificacion
		public List<ENTClientesportalCat> RecuperarClientesportalCatCodigoverificacion(Guid codigoverificacion)
		{
			List<ENTClientesportalCat> result = new List<ENTClientesportalCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@CodigoVerificacion", SqlDbType.UniqueIdentifier);
			param0.Value = codigoverificacion;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetClientesPortalCat_POR_CodigoVerificacion";
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
				    ENTClientesportalCat item = new ENTClientesportalCat();
					if (!dr.IsNull("Id")) item.Id = Convert.ToInt32(dr["Id"]);
					if (!dr.IsNull("Email")) item.Email = dr["Email"].ToString();
					if (!dr.IsNull("Contrasenia")) item.Contrasenia = dr["Contrasenia"].ToString();
					if (!dr.IsNull("RFC")) item.RFC = dr["RFC"].ToString();
					if (!dr.IsNull("TAXID")) item.TAXID = dr["TAXID"].ToString();
					if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					if (!dr.IsNull("FormaPago")) item.FormaPago = dr["FormaPago"].ToString();
					if (!dr.IsNull("Pais")) item.Pais = dr["Pais"].ToString();
					if (!dr.IsNull("ClienteTipoId")) item.ClienteTipoId = Convert.ToInt32(dr["ClienteTipoId"]);
					if (!dr.IsNull("CodigoVerificacion")) item.CodigoVerificacion = Guid.Parse(dr["CodigoVerificacion"].ToString());
					if (!dr.IsNull("UsuarioVerificado")) item.UsuarioVerificado = Convert.ToBoolean(dr["UsuarioVerificado"]);
					if (!dr.IsNull("UsuarioActivo")) item.UsuarioActivo = Convert.ToBoolean(dr["UsuarioActivo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("Id")) Id = Convert.ToInt32(dtResultado.Rows[0]["Id"]);
					if (!dtResultado.Rows[0].IsNull("Email")) Email = dtResultado.Rows[0]["Email"].ToString();
					if (!dtResultado.Rows[0].IsNull("Contrasenia")) Contrasenia = dtResultado.Rows[0]["Contrasenia"].ToString();
					if (!dtResultado.Rows[0].IsNull("RFC")) RFC = dtResultado.Rows[0]["RFC"].ToString();
					if (!dtResultado.Rows[0].IsNull("TAXID")) TAXID = dtResultado.Rows[0]["TAXID"].ToString();
					if (!dtResultado.Rows[0].IsNull("Nombre")) Nombre = dtResultado.Rows[0]["Nombre"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsoCFDI")) UsoCFDI = dtResultado.Rows[0]["UsoCFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("FormaPago")) FormaPago = dtResultado.Rows[0]["FormaPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("Pais")) Pais = dtResultado.Rows[0]["Pais"].ToString();
					if (!dtResultado.Rows[0].IsNull("ClienteTipoId")) ClienteTipoId = Convert.ToInt32(dtResultado.Rows[0]["ClienteTipoId"]);
					if (!dtResultado.Rows[0].IsNull("CodigoVerificacion")) CodigoVerificacion = Guid.Parse(dtResultado.Rows[0]["CodigoVerificacion"].ToString());
					if (!dtResultado.Rows[0].IsNull("UsuarioVerificado")) UsuarioVerificado = Convert.ToBoolean(dtResultado.Rows[0]["UsuarioVerificado"]);
					if (!dtResultado.Rows[0].IsNull("UsuarioActivo")) UsuarioActivo = Convert.ToBoolean(dtResultado.Rows[0]["UsuarioActivo"]);
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

		#region Recuperar RecuperarClientesportalCatRfc
		public List<ENTClientesportalCat> RecuperarClientesportalCatRfc(string rfc)
		{
			List<ENTClientesportalCat> result = new List<ENTClientesportalCat>();
			DataTable dtResultado = new DataTable();
			List<SqlParameter> commandParameters = new List<SqlParameter>();
			SqlParameter param0 = new SqlParameter("@RFC", SqlDbType.VarChar);
			param0.Value = rfc;
			commandParameters.Add(param0);
			SqlCommand cmm = new SqlCommand();
			cmm.CommandType = CommandType.StoredProcedure;
			cmm.CommandText = "uspFac_GetClientesPortalCat_POR_RFC";
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
				    ENTClientesportalCat item = new ENTClientesportalCat();
					if (!dr.IsNull("Id")) item.Id = Convert.ToInt32(dr["Id"]);
					if (!dr.IsNull("Email")) item.Email = dr["Email"].ToString();
					if (!dr.IsNull("Contrasenia")) item.Contrasenia = dr["Contrasenia"].ToString();
					if (!dr.IsNull("RFC")) item.RFC = dr["RFC"].ToString();
					if (!dr.IsNull("TAXID")) item.TAXID = dr["TAXID"].ToString();
					if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
					if (!dr.IsNull("UsoCFDI")) item.UsoCFDI = dr["UsoCFDI"].ToString();
					if (!dr.IsNull("FormaPago")) item.FormaPago = dr["FormaPago"].ToString();
					if (!dr.IsNull("Pais")) item.Pais = dr["Pais"].ToString();
					if (!dr.IsNull("ClienteTipoId")) item.ClienteTipoId = Convert.ToInt32(dr["ClienteTipoId"]);
					if (!dr.IsNull("CodigoVerificacion")) item.CodigoVerificacion = Guid.Parse(dr["CodigoVerificacion"].ToString());
					if (!dr.IsNull("UsuarioVerificado")) item.UsuarioVerificado = Convert.ToBoolean(dr["UsuarioVerificado"]);
					if (!dr.IsNull("UsuarioActivo")) item.UsuarioActivo = Convert.ToBoolean(dr["UsuarioActivo"]);
					if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
				    result.Add(item);
				}
				if (dtResultado.Rows.Count.Equals(1))
				{
					if (!dtResultado.Rows[0].IsNull("Id")) Id = Convert.ToInt32(dtResultado.Rows[0]["Id"]);
					if (!dtResultado.Rows[0].IsNull("Email")) Email = dtResultado.Rows[0]["Email"].ToString();
					if (!dtResultado.Rows[0].IsNull("Contrasenia")) Contrasenia = dtResultado.Rows[0]["Contrasenia"].ToString();
					if (!dtResultado.Rows[0].IsNull("RFC")) RFC = dtResultado.Rows[0]["RFC"].ToString();
					if (!dtResultado.Rows[0].IsNull("TAXID")) TAXID = dtResultado.Rows[0]["TAXID"].ToString();
					if (!dtResultado.Rows[0].IsNull("Nombre")) Nombre = dtResultado.Rows[0]["Nombre"].ToString();
					if (!dtResultado.Rows[0].IsNull("UsoCFDI")) UsoCFDI = dtResultado.Rows[0]["UsoCFDI"].ToString();
					if (!dtResultado.Rows[0].IsNull("FormaPago")) FormaPago = dtResultado.Rows[0]["FormaPago"].ToString();
					if (!dtResultado.Rows[0].IsNull("Pais")) Pais = dtResultado.Rows[0]["Pais"].ToString();
					if (!dtResultado.Rows[0].IsNull("ClienteTipoId")) ClienteTipoId = Convert.ToInt32(dtResultado.Rows[0]["ClienteTipoId"]);
					if (!dtResultado.Rows[0].IsNull("CodigoVerificacion")) CodigoVerificacion = Guid.Parse(dtResultado.Rows[0]["CodigoVerificacion"].ToString());
					if (!dtResultado.Rows[0].IsNull("UsuarioVerificado")) UsuarioVerificado = Convert.ToBoolean(dtResultado.Rows[0]["UsuarioVerificado"]);
					if (!dtResultado.Rows[0].IsNull("UsuarioActivo")) UsuarioActivo = Convert.ToBoolean(dtResultado.Rows[0]["UsuarioActivo"]);
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
			_copia = new DALClientesportalCat(_conexion);
			Type tipo = typeof(ENTClientesportalCat);
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

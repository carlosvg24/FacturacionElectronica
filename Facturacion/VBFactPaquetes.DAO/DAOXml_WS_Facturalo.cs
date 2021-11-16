using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VBFactPaquetes.Model;

namespace VBFactPaquetes.DAO
{
    public class DAOXml_WS_Facturalo : Xml_WS_Facturalo
    {

        #region Propiedades Privadas
        protected SqlConnection _conexion;
        protected bool _nuevo = false;
        protected string _idUsuario;
        protected string _proceso;
        protected DAOXml_WS_Facturalo _copia;
        #endregion Propiedades Privadas

        #region Constructores
        public DAOXml_WS_Facturalo(SqlConnection conexion)
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
            IdPeticionPAC = 0;
            FechaPeticion = new DateTime();
            FolioCFDI = 0;
            TipoComprobante = String.Empty;
            XMLRequest = String.Empty;
            XMLResponse = String.Empty;
            MensajeRespuesta = String.Empty;
            EsCorrecto = false;
            FechaTimbrado = new DateTime();
            Transaccion_Id = String.Empty;
            Transaccion_Tipo = String.Empty;
            Transaccion_Estatus = String.Empty;
            CFD_CadenaOriginal = String.Empty;
            CFD_NoCertificado = String.Empty;
            CFD_ComprobanteStr = String.Empty;
            CFD_Serie = String.Empty;
            CFD_CodigoDeBarras = String.Empty;
            CFD_Fecha = new DateTime();
            CFD_Folio = 0;
            CFD_Sello = String.Empty;
            TFD_UUID = String.Empty;
            TFD_FechaTimbrado = new DateTime();
            TFD_NoCertificadoSAT = String.Empty;
            TFD_SelloSAT = String.Empty;
            FechaHoraLocal = new DateTime();
            PAC = String.Empty;
        }
        #endregion

        #region Agregar VBFac_XML_Pegaso
        public void Agregar()
        {
            _nuevo = true;
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlCommand cmm = new SqlCommand();
            SqlParameter param0 = new SqlParameter("@IdPeticionPAC", SqlDbType.BigInt);
            param0.Value = IdPeticionPAC;
            param0.Direction = ParameterDirection.InputOutput;
            commandParameters.Add(param0);
            SqlParameter param1 = new SqlParameter("@FechaPeticion", SqlDbType.VarChar);
            param1.Value = FechaPeticion.Year > 1900 ? FechaPeticion.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
            commandParameters.Add(param1);
            SqlParameter param2 = new SqlParameter("@FolioCFDI", SqlDbType.BigInt);
            param2.Value = FolioCFDI;
            commandParameters.Add(param2);
            SqlParameter param3 = new SqlParameter("@TipoComprobante", SqlDbType.VarChar);
            param3.Value = TipoComprobante;
            commandParameters.Add(param3);
            SqlParameter param4 = new SqlParameter("@XMLRequest", SqlDbType.VarChar);
            param4.Value = XMLRequest;
            commandParameters.Add(param4);
            SqlParameter param5 = new SqlParameter("@XMLResponse", SqlDbType.VarChar);
            param5.Value = XMLResponse;
            commandParameters.Add(param5);
            SqlParameter param6 = new SqlParameter("@MensajeRespuesta", SqlDbType.VarChar);
            param6.Value = MensajeRespuesta;
            commandParameters.Add(param6);
            SqlParameter param7 = new SqlParameter("@EsCorrecto", SqlDbType.Bit);
            param7.Value = EsCorrecto;
            commandParameters.Add(param7);
            SqlParameter param8 = new SqlParameter("@FechaTimbrado", SqlDbType.VarChar);
            param8.Value = FechaTimbrado.Year > 1900 ? FechaTimbrado.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
            commandParameters.Add(param8);
            SqlParameter param9 = new SqlParameter("@Transaccion_Id", SqlDbType.VarChar);
            param9.Value = Transaccion_Id;
            commandParameters.Add(param9);
            SqlParameter param10 = new SqlParameter("@Transaccion_Tipo", SqlDbType.VarChar);
            param10.Value = Transaccion_Tipo;
            commandParameters.Add(param10);
            SqlParameter param11 = new SqlParameter("@Transaccion_Estatus", SqlDbType.VarChar);
            param11.Value = Transaccion_Estatus;
            commandParameters.Add(param11);
            SqlParameter param12 = new SqlParameter("@CFD_CadenaOriginal", SqlDbType.VarChar);
            param12.Value = CFD_CadenaOriginal;
            commandParameters.Add(param12);
            SqlParameter param13 = new SqlParameter("@CFD_NoCertificado", SqlDbType.VarChar);
            param13.Value = CFD_NoCertificado;
            commandParameters.Add(param13);
            SqlParameter param14 = new SqlParameter("@CFD_ComprobanteStr", SqlDbType.VarChar);
            param14.Value = CFD_ComprobanteStr;
            commandParameters.Add(param14);
            SqlParameter param15 = new SqlParameter("@CFD_Serie", SqlDbType.VarChar);
            param15.Value = CFD_Serie;
            commandParameters.Add(param15);
            SqlParameter param16 = new SqlParameter("@CFD_CodigoDeBarras", SqlDbType.VarChar);
            param16.Value = CFD_CodigoDeBarras;
            commandParameters.Add(param16);
            SqlParameter param17 = new SqlParameter("@CFD_Fecha", SqlDbType.VarChar);
            param17.Value = CFD_Fecha.Year > 1900 ? CFD_Fecha.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
            commandParameters.Add(param17);
            SqlParameter param18 = new SqlParameter("@CFD_Folio", SqlDbType.BigInt);
            param18.Value = CFD_Folio;
            commandParameters.Add(param18);
            SqlParameter param19 = new SqlParameter("@CFD_Sello", SqlDbType.VarChar);
            param19.Value = CFD_Sello;
            commandParameters.Add(param19);
            SqlParameter param20 = new SqlParameter("@TFD_UUID", SqlDbType.VarChar);
            param20.Value = TFD_UUID;
            commandParameters.Add(param20);
            SqlParameter param21 = new SqlParameter("@TFD_FechaTimbrado", SqlDbType.VarChar);
            param21.Value = TFD_FechaTimbrado.Year > 1900 ? TFD_FechaTimbrado.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
            commandParameters.Add(param21);
            SqlParameter param22 = new SqlParameter("@TFD_NoCertificadoSAT", SqlDbType.VarChar);
            param22.Value = TFD_NoCertificadoSAT;
            commandParameters.Add(param22);
            SqlParameter param23 = new SqlParameter("@TFD_SelloSAT", SqlDbType.VarChar);
            param23.Value = TFD_SelloSAT;
            commandParameters.Add(param23);
            SqlParameter param24 = new SqlParameter("@PAC", SqlDbType.VarChar);
            param24.Value = PAC;
            commandParameters.Add(param24);
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_InsXMLPegaso";
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
                    else
                    {
                        IdPeticionPAC = Convert.ToInt64(cmm.Parameters[0].Value);

                    }
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
        }
        #endregion

        #region Actualizar VBFac_XML_Pegaso
        public void Actualizar()
        {
            _nuevo = false;
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlCommand cmm = new SqlCommand();
            SqlParameter param0 = new SqlParameter("@IdPeticionPAC", SqlDbType.BigInt);
            param0.Value = IdPeticionPAC;
            commandParameters.Add(param0);
            SqlParameter param1 = new SqlParameter("@FechaPeticion", SqlDbType.VarChar);
            param1.Value = FechaPeticion.Year > 1900 ? FechaPeticion.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
            commandParameters.Add(param1);
            SqlParameter param2 = new SqlParameter("@FolioCFDI", SqlDbType.BigInt);
            param2.Value = FolioCFDI;
            commandParameters.Add(param2);
            SqlParameter param3 = new SqlParameter("@TipoComprobante", SqlDbType.VarChar);
            param3.Value = TipoComprobante;
            commandParameters.Add(param3);
            SqlParameter param4 = new SqlParameter("@XMLRequest", SqlDbType.VarChar);
            param4.Value = XMLRequest;
            commandParameters.Add(param4);
            SqlParameter param5 = new SqlParameter("@XMLResponse", SqlDbType.VarChar);
            param5.Value = XMLResponse;
            commandParameters.Add(param5);
            SqlParameter param6 = new SqlParameter("@MensajeRespuesta", SqlDbType.VarChar);
            param6.Value = MensajeRespuesta;
            commandParameters.Add(param6);
            SqlParameter param7 = new SqlParameter("@EsCorrecto", SqlDbType.Bit);
            param7.Value = EsCorrecto;
            commandParameters.Add(param7);
            SqlParameter param8 = new SqlParameter("@FechaTimbrado", SqlDbType.VarChar);
            param8.Value = FechaTimbrado.Year > 1900 ? FechaTimbrado.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
            commandParameters.Add(param8);
            SqlParameter param9 = new SqlParameter("@Transaccion_Id", SqlDbType.VarChar);
            param9.Value = Transaccion_Id;
            commandParameters.Add(param9);
            SqlParameter param10 = new SqlParameter("@Transaccion_Tipo", SqlDbType.VarChar);
            param10.Value = Transaccion_Tipo;
            commandParameters.Add(param10);
            SqlParameter param11 = new SqlParameter("@Transaccion_Estatus", SqlDbType.VarChar);
            param11.Value = Transaccion_Estatus;
            commandParameters.Add(param11);
            SqlParameter param12 = new SqlParameter("@CFD_CadenaOriginal", SqlDbType.VarChar);
            param12.Value = CFD_CadenaOriginal;
            commandParameters.Add(param12);
            SqlParameter param13 = new SqlParameter("@CFD_NoCertificado", SqlDbType.VarChar);
            param13.Value = CFD_NoCertificado;
            commandParameters.Add(param13);
            SqlParameter param14 = new SqlParameter("@CFD_ComprobanteStr", SqlDbType.VarChar);
            param14.Value = CFD_ComprobanteStr;
            commandParameters.Add(param14);
            SqlParameter param15 = new SqlParameter("@CFD_Serie", SqlDbType.VarChar);
            param15.Value = CFD_Serie;
            commandParameters.Add(param15);
            SqlParameter param16 = new SqlParameter("@CFD_CodigoDeBarras", SqlDbType.VarChar);
            param16.Value = CFD_CodigoDeBarras;
            commandParameters.Add(param16);
            SqlParameter param17 = new SqlParameter("@CFD_Fecha", SqlDbType.VarChar);
            param17.Value = CFD_Fecha.Year > 1900 ? CFD_Fecha.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
            commandParameters.Add(param17);
            SqlParameter param18 = new SqlParameter("@CFD_Folio", SqlDbType.BigInt);
            param18.Value = CFD_Folio;
            commandParameters.Add(param18);
            SqlParameter param19 = new SqlParameter("@CFD_Sello", SqlDbType.VarChar);
            param19.Value = CFD_Sello;
            commandParameters.Add(param19);
            SqlParameter param20 = new SqlParameter("@TFD_UUID", SqlDbType.VarChar);
            param20.Value = TFD_UUID;
            commandParameters.Add(param20);
            SqlParameter param21 = new SqlParameter("@TFD_FechaTimbrado", SqlDbType.VarChar);
            param21.Value = TFD_FechaTimbrado.Year > 1900 ? TFD_FechaTimbrado.ToString("yyyy-MM-dd HH:mm:ss.fff") : "";
            commandParameters.Add(param21);
            SqlParameter param22 = new SqlParameter("@TFD_NoCertificadoSAT", SqlDbType.VarChar);
            param22.Value = TFD_NoCertificadoSAT;
            commandParameters.Add(param22);
            SqlParameter param23 = new SqlParameter("@TFD_SelloSAT", SqlDbType.VarChar);
            param23.Value = TFD_SelloSAT;
            commandParameters.Add(param23);
            SqlParameter param24 = new SqlParameter("@PAC", SqlDbType.VarChar);
            param24.Value = PAC;
            commandParameters.Add(param24);
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_UpdXMLPegaso";
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
        }
        #endregion

        #region Eliminar VBFac_XML_Pegaso
        public void Eliminar(long idpeticionpac)
        {
            _nuevo = false;
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlCommand cmm = new SqlCommand();
            SqlParameter param0 = new SqlParameter("@IdPeticionPAC", SqlDbType.BigInt);
            param0.Value = idpeticionpac;
            commandParameters.Add(param0);
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_DelXMLPegaso";
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
        }
        #endregion

        #region Deshacer VBFac_XML_Pegaso
        public void Deshacer()
        {
            if (_nuevo)
            {
                this.Eliminar(IdPeticionPAC);
            }
            else
            {
                if (_copia != null)
                {
                    _copia.Actualizar();
                }
            }
        }
        #endregion

        #region Metodos Recuperar
        #region Recuperar toda la tabla
        public List<Xml_WS_Facturalo> RecuperarTodo()
        {
            List<Xml_WS_Facturalo> result = new List<Xml_WS_Facturalo>();
            DataTable dtResultado = new DataTable();
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_GetXMLPegaso_TODO";
            cmm.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
            cmm.Connection = _conexion;
            bool cerrarConexion = false;

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
                dtResultado = dsResult.Tables["dtResultado"];
                foreach (DataRow dr in dtResultado.Rows)
                {
                    Xml_WS_Facturalo item = new Xml_WS_Facturalo();
                    if (!dr.IsNull("IdPeticionPAC")) item.IdPeticionPAC = Convert.ToInt64(dr["IdPeticionPAC"]);
                    if (!dr.IsNull("FechaPeticion")) item.FechaPeticion = Convert.ToDateTime(dr["FechaPeticion"]);
                    if (!dr.IsNull("FolioCFDI")) item.FolioCFDI = Convert.ToInt64(dr["FolioCFDI"]);
                    if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
                    if (!dr.IsNull("XMLRequest")) item.XMLRequest = dr["XMLRequest"].ToString();
                    if (!dr.IsNull("XMLResponse")) item.XMLResponse = dr["XMLResponse"].ToString();
                    if (!dr.IsNull("MensajeRespuesta")) item.MensajeRespuesta = dr["MensajeRespuesta"].ToString();
                    if (!dr.IsNull("EsCorrecto")) item.EsCorrecto = Convert.ToBoolean(dr["EsCorrecto"]);
                    if (!dr.IsNull("FechaTimbrado")) item.FechaTimbrado = Convert.ToDateTime(dr["FechaTimbrado"]);
                    if (!dr.IsNull("Transaccion_Id")) item.Transaccion_Id = dr["Transaccion_Id"].ToString();
                    if (!dr.IsNull("Transaccion_Tipo")) item.Transaccion_Tipo = dr["Transaccion_Tipo"].ToString();
                    if (!dr.IsNull("Transaccion_Estatus")) item.Transaccion_Estatus = dr["Transaccion_Estatus"].ToString();
                    if (!dr.IsNull("CFD_CadenaOriginal")) item.CFD_CadenaOriginal = dr["CFD_CadenaOriginal"].ToString();
                    if (!dr.IsNull("CFD_NoCertificado")) item.CFD_NoCertificado = dr["CFD_NoCertificado"].ToString();
                    if (!dr.IsNull("CFD_ComprobanteStr")) item.CFD_ComprobanteStr = dr["CFD_ComprobanteStr"].ToString();
                    if (!dr.IsNull("CFD_Serie")) item.CFD_Serie = dr["CFD_Serie"].ToString();
                    if (!dr.IsNull("CFD_CodigoDeBarras")) item.CFD_CodigoDeBarras = dr["CFD_CodigoDeBarras"].ToString();
                    if (!dr.IsNull("CFD_Fecha")) item.CFD_Fecha = Convert.ToDateTime(dr["CFD_Fecha"]);
                    if (!dr.IsNull("CFD_Folio")) item.CFD_Folio = Convert.ToInt64(dr["CFD_Folio"]);
                    if (!dr.IsNull("CFD_Sello")) item.CFD_Sello = dr["CFD_Sello"].ToString();
                    if (!dr.IsNull("TFD_UUID")) item.TFD_UUID = dr["TFD_UUID"].ToString();
                    if (!dr.IsNull("TFD_FechaTimbrado")) item.TFD_FechaTimbrado = Convert.ToDateTime(dr["TFD_FechaTimbrado"]);
                    if (!dr.IsNull("TFD_NoCertificadoSAT")) item.TFD_NoCertificadoSAT = dr["TFD_NoCertificadoSAT"].ToString();
                    if (!dr.IsNull("TFD_SelloSAT")) item.TFD_SelloSAT = dr["TFD_SelloSAT"].ToString();
                    if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
                    if (!dr.IsNull("PAC")) item.PAC = dr["PAC"].ToString();
                    result.Add(item);
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
            return result;
        }
        #endregion

        #region Recuperar RecuperarXmlPegasoPorLlavePrimaria
        public List<Xml_WS_Facturalo> RecuperarXmlPegasoPorLlavePrimaria(long idpeticionpac)
        {
            List<Xml_WS_Facturalo> result = new List<Xml_WS_Facturalo>();
            DataTable dtResultado = new DataTable();
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@IdPeticionPAC", SqlDbType.BigInt);
            param0.Value = idpeticionpac;
            commandParameters.Add(param0);
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_GetXMLPegaso_POR_PK";
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
                dtResultado = dsResult.Tables["dtResultado"];
                foreach (DataRow dr in dtResultado.Rows)
                {
                    Xml_WS_Facturalo item = new Xml_WS_Facturalo();
                    if (!dr.IsNull("IdPeticionPAC")) item.IdPeticionPAC = Convert.ToInt64(dr["IdPeticionPAC"]);
                    if (!dr.IsNull("FechaPeticion")) item.FechaPeticion = Convert.ToDateTime(dr["FechaPeticion"]);
                    if (!dr.IsNull("FolioCFDI")) item.FolioCFDI = Convert.ToInt64(dr["FolioCFDI"]);
                    if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
                    if (!dr.IsNull("XMLRequest")) item.XMLRequest = dr["XMLRequest"].ToString();
                    if (!dr.IsNull("XMLResponse")) item.XMLResponse = dr["XMLResponse"].ToString();
                    if (!dr.IsNull("MensajeRespuesta")) item.MensajeRespuesta = dr["MensajeRespuesta"].ToString();
                    if (!dr.IsNull("EsCorrecto")) item.EsCorrecto = Convert.ToBoolean(dr["EsCorrecto"]);
                    if (!dr.IsNull("FechaTimbrado")) item.FechaTimbrado = Convert.ToDateTime(dr["FechaTimbrado"]);
                    if (!dr.IsNull("Transaccion_Id")) item.Transaccion_Id = dr["Transaccion_Id"].ToString();
                    if (!dr.IsNull("Transaccion_Tipo")) item.Transaccion_Tipo = dr["Transaccion_Tipo"].ToString();
                    if (!dr.IsNull("Transaccion_Estatus")) item.Transaccion_Estatus = dr["Transaccion_Estatus"].ToString();
                    if (!dr.IsNull("CFD_CadenaOriginal")) item.CFD_CadenaOriginal = dr["CFD_CadenaOriginal"].ToString();
                    if (!dr.IsNull("CFD_NoCertificado")) item.CFD_NoCertificado = dr["CFD_NoCertificado"].ToString();
                    if (!dr.IsNull("CFD_ComprobanteStr")) item.CFD_ComprobanteStr = dr["CFD_ComprobanteStr"].ToString();
                    if (!dr.IsNull("CFD_Serie")) item.CFD_Serie = dr["CFD_Serie"].ToString();
                    if (!dr.IsNull("CFD_CodigoDeBarras")) item.CFD_CodigoDeBarras = dr["CFD_CodigoDeBarras"].ToString();
                    if (!dr.IsNull("CFD_Fecha")) item.CFD_Fecha = Convert.ToDateTime(dr["CFD_Fecha"]);
                    if (!dr.IsNull("CFD_Folio")) item.CFD_Folio = Convert.ToInt64(dr["CFD_Folio"]);
                    if (!dr.IsNull("CFD_Sello")) item.CFD_Sello = dr["CFD_Sello"].ToString();
                    if (!dr.IsNull("TFD_UUID")) item.TFD_UUID = dr["TFD_UUID"].ToString();
                    if (!dr.IsNull("TFD_FechaTimbrado")) item.TFD_FechaTimbrado = Convert.ToDateTime(dr["TFD_FechaTimbrado"]);
                    if (!dr.IsNull("TFD_NoCertificadoSAT")) item.TFD_NoCertificadoSAT = dr["TFD_NoCertificadoSAT"].ToString();
                    if (!dr.IsNull("TFD_SelloSAT")) item.TFD_SelloSAT = dr["TFD_SelloSAT"].ToString();
                    if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
                    if (!dr.IsNull("PAC")) item.PAC = dr["PAC"].ToString();
                    result.Add(item);
                }
                if (dtResultado.Rows.Count.Equals(1))
                {
                    if (!dtResultado.Rows[0].IsNull("IdPeticionPAC")) IdPeticionPAC = Convert.ToInt64(dtResultado.Rows[0]["IdPeticionPAC"]);
                    if (!dtResultado.Rows[0].IsNull("FechaPeticion")) FechaPeticion = Convert.ToDateTime(dtResultado.Rows[0]["FechaPeticion"]);
                    if (!dtResultado.Rows[0].IsNull("FolioCFDI")) FolioCFDI = Convert.ToInt64(dtResultado.Rows[0]["FolioCFDI"]);
                    if (!dtResultado.Rows[0].IsNull("TipoComprobante")) TipoComprobante = dtResultado.Rows[0]["TipoComprobante"].ToString();
                    if (!dtResultado.Rows[0].IsNull("XMLRequest")) XMLRequest = dtResultado.Rows[0]["XMLRequest"].ToString();
                    if (!dtResultado.Rows[0].IsNull("XMLResponse")) XMLResponse = dtResultado.Rows[0]["XMLResponse"].ToString();
                    if (!dtResultado.Rows[0].IsNull("MensajeRespuesta")) MensajeRespuesta = dtResultado.Rows[0]["MensajeRespuesta"].ToString();
                    if (!dtResultado.Rows[0].IsNull("EsCorrecto")) EsCorrecto = Convert.ToBoolean(dtResultado.Rows[0]["EsCorrecto"]);
                    if (!dtResultado.Rows[0].IsNull("FechaTimbrado")) FechaTimbrado = Convert.ToDateTime(dtResultado.Rows[0]["FechaTimbrado"]);
                    if (!dtResultado.Rows[0].IsNull("Transaccion_Id")) Transaccion_Id = dtResultado.Rows[0]["Transaccion_Id"].ToString();
                    if (!dtResultado.Rows[0].IsNull("Transaccion_Tipo")) Transaccion_Tipo = dtResultado.Rows[0]["Transaccion_Tipo"].ToString();
                    if (!dtResultado.Rows[0].IsNull("Transaccion_Estatus")) Transaccion_Estatus = dtResultado.Rows[0]["Transaccion_Estatus"].ToString();
                    if (!dtResultado.Rows[0].IsNull("CFD_CadenaOriginal")) CFD_CadenaOriginal = dtResultado.Rows[0]["CFD_CadenaOriginal"].ToString();
                    if (!dtResultado.Rows[0].IsNull("CFD_NoCertificado")) CFD_NoCertificado = dtResultado.Rows[0]["CFD_NoCertificado"].ToString();
                    if (!dtResultado.Rows[0].IsNull("CFD_ComprobanteStr")) CFD_ComprobanteStr = dtResultado.Rows[0]["CFD_ComprobanteStr"].ToString();
                    if (!dtResultado.Rows[0].IsNull("CFD_Serie")) CFD_Serie = dtResultado.Rows[0]["CFD_Serie"].ToString();
                    if (!dtResultado.Rows[0].IsNull("CFD_CodigoDeBarras")) CFD_CodigoDeBarras = dtResultado.Rows[0]["CFD_CodigoDeBarras"].ToString();
                    if (!dtResultado.Rows[0].IsNull("CFD_Fecha")) CFD_Fecha = Convert.ToDateTime(dtResultado.Rows[0]["CFD_Fecha"]);
                    if (!dtResultado.Rows[0].IsNull("CFD_Folio")) CFD_Folio = Convert.ToInt64(dtResultado.Rows[0]["CFD_Folio"]);
                    if (!dtResultado.Rows[0].IsNull("CFD_Sello")) CFD_Sello = dtResultado.Rows[0]["CFD_Sello"].ToString();
                    if (!dtResultado.Rows[0].IsNull("TFD_UUID")) TFD_UUID = dtResultado.Rows[0]["TFD_UUID"].ToString();
                    if (!dtResultado.Rows[0].IsNull("TFD_FechaTimbrado")) TFD_FechaTimbrado = Convert.ToDateTime(dtResultado.Rows[0]["TFD_FechaTimbrado"]);
                    if (!dtResultado.Rows[0].IsNull("TFD_NoCertificadoSAT")) TFD_NoCertificadoSAT = dtResultado.Rows[0]["TFD_NoCertificadoSAT"].ToString();
                    if (!dtResultado.Rows[0].IsNull("TFD_SelloSAT")) TFD_SelloSAT = dtResultado.Rows[0]["TFD_SelloSAT"].ToString();
                    if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
                    if (!dtResultado.Rows[0].IsNull("PAC")) PAC = dtResultado.Rows[0]["PAC"].ToString();
                    this.Clone();
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
            return result;
        }
        #endregion

        #region Recuperar RecuperarXmlPegasoFoliocfdi
        public List<Xml_WS_Facturalo> RecuperarXmlPegasoFoliocfdi(long foliocfdi)
        {
            List<Xml_WS_Facturalo> result = new List<Xml_WS_Facturalo>();
            DataTable dtResultado = new DataTable();
            List<SqlParameter> commandParameters = new List<SqlParameter>();
            SqlParameter param0 = new SqlParameter("@FolioCFDI", SqlDbType.BigInt);
            param0.Value = foliocfdi;
            commandParameters.Add(param0);
            SqlCommand cmm = new SqlCommand();
            cmm.CommandType = CommandType.StoredProcedure;
            cmm.CommandText = "uspFac_GetXMLPegaso_POR_FolioCFDI";
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
                dtResultado = dsResult.Tables["dtResultado"];
                foreach (DataRow dr in dtResultado.Rows)
                {
                    Xml_WS_Facturalo item = new Xml_WS_Facturalo();
                    if (!dr.IsNull("IdPeticionPAC")) item.IdPeticionPAC = Convert.ToInt64(dr["IdPeticionPAC"]);
                    if (!dr.IsNull("FechaPeticion")) item.FechaPeticion = Convert.ToDateTime(dr["FechaPeticion"]);
                    if (!dr.IsNull("FolioCFDI")) item.FolioCFDI = Convert.ToInt64(dr["FolioCFDI"]);
                    if (!dr.IsNull("TipoComprobante")) item.TipoComprobante = dr["TipoComprobante"].ToString();
                    if (!dr.IsNull("XMLRequest")) item.XMLRequest = dr["XMLRequest"].ToString();
                    if (!dr.IsNull("XMLResponse")) item.XMLResponse = dr["XMLResponse"].ToString();
                    if (!dr.IsNull("MensajeRespuesta")) item.MensajeRespuesta = dr["MensajeRespuesta"].ToString();
                    if (!dr.IsNull("EsCorrecto")) item.EsCorrecto = Convert.ToBoolean(dr["EsCorrecto"]);
                    if (!dr.IsNull("FechaTimbrado")) item.FechaTimbrado = Convert.ToDateTime(dr["FechaTimbrado"]);
                    if (!dr.IsNull("Transaccion_Id")) item.Transaccion_Id = dr["Transaccion_Id"].ToString();
                    if (!dr.IsNull("Transaccion_Tipo")) item.Transaccion_Tipo = dr["Transaccion_Tipo"].ToString();
                    if (!dr.IsNull("Transaccion_Estatus")) item.Transaccion_Estatus = dr["Transaccion_Estatus"].ToString();
                    if (!dr.IsNull("CFD_CadenaOriginal")) item.CFD_CadenaOriginal = dr["CFD_CadenaOriginal"].ToString();
                    if (!dr.IsNull("CFD_NoCertificado")) item.CFD_NoCertificado = dr["CFD_NoCertificado"].ToString();
                    if (!dr.IsNull("CFD_ComprobanteStr")) item.CFD_ComprobanteStr = dr["CFD_ComprobanteStr"].ToString();
                    if (!dr.IsNull("CFD_Serie")) item.CFD_Serie = dr["CFD_Serie"].ToString();
                    if (!dr.IsNull("CFD_CodigoDeBarras")) item.CFD_CodigoDeBarras = dr["CFD_CodigoDeBarras"].ToString();
                    if (!dr.IsNull("CFD_Fecha")) item.CFD_Fecha = Convert.ToDateTime(dr["CFD_Fecha"]);
                    if (!dr.IsNull("CFD_Folio")) item.CFD_Folio = Convert.ToInt64(dr["CFD_Folio"]);
                    if (!dr.IsNull("CFD_Sello")) item.CFD_Sello = dr["CFD_Sello"].ToString();
                    if (!dr.IsNull("TFD_UUID")) item.TFD_UUID = dr["TFD_UUID"].ToString();
                    if (!dr.IsNull("TFD_FechaTimbrado")) item.TFD_FechaTimbrado = Convert.ToDateTime(dr["TFD_FechaTimbrado"]);
                    if (!dr.IsNull("TFD_NoCertificadoSAT")) item.TFD_NoCertificadoSAT = dr["TFD_NoCertificadoSAT"].ToString();
                    if (!dr.IsNull("TFD_SelloSAT")) item.TFD_SelloSAT = dr["TFD_SelloSAT"].ToString();
                    if (!dr.IsNull("FechaHoraLocal")) item.FechaHoraLocal = Convert.ToDateTime(dr["FechaHoraLocal"]);
                    if (!dr.IsNull("PAC")) item.PAC = dr["PAC"].ToString();
                    result.Add(item);
                }
                if (dtResultado.Rows.Count.Equals(1))
                {
                    if (!dtResultado.Rows[0].IsNull("IdPeticionPAC")) IdPeticionPAC = Convert.ToInt64(dtResultado.Rows[0]["IdPeticionPAC"]);
                    if (!dtResultado.Rows[0].IsNull("FechaPeticion")) FechaPeticion = Convert.ToDateTime(dtResultado.Rows[0]["FechaPeticion"]);
                    if (!dtResultado.Rows[0].IsNull("FolioCFDI")) FolioCFDI = Convert.ToInt64(dtResultado.Rows[0]["FolioCFDI"]);
                    if (!dtResultado.Rows[0].IsNull("TipoComprobante")) TipoComprobante = dtResultado.Rows[0]["TipoComprobante"].ToString();
                    if (!dtResultado.Rows[0].IsNull("XMLRequest")) XMLRequest = dtResultado.Rows[0]["XMLRequest"].ToString();
                    if (!dtResultado.Rows[0].IsNull("XMLResponse")) XMLResponse = dtResultado.Rows[0]["XMLResponse"].ToString();
                    if (!dtResultado.Rows[0].IsNull("MensajeRespuesta")) MensajeRespuesta = dtResultado.Rows[0]["MensajeRespuesta"].ToString();
                    if (!dtResultado.Rows[0].IsNull("EsCorrecto")) EsCorrecto = Convert.ToBoolean(dtResultado.Rows[0]["EsCorrecto"]);
                    if (!dtResultado.Rows[0].IsNull("FechaTimbrado")) FechaTimbrado = Convert.ToDateTime(dtResultado.Rows[0]["FechaTimbrado"]);
                    if (!dtResultado.Rows[0].IsNull("Transaccion_Id")) Transaccion_Id = dtResultado.Rows[0]["Transaccion_Id"].ToString();
                    if (!dtResultado.Rows[0].IsNull("Transaccion_Tipo")) Transaccion_Tipo = dtResultado.Rows[0]["Transaccion_Tipo"].ToString();
                    if (!dtResultado.Rows[0].IsNull("Transaccion_Estatus")) Transaccion_Estatus = dtResultado.Rows[0]["Transaccion_Estatus"].ToString();
                    if (!dtResultado.Rows[0].IsNull("CFD_CadenaOriginal")) CFD_CadenaOriginal = dtResultado.Rows[0]["CFD_CadenaOriginal"].ToString();
                    if (!dtResultado.Rows[0].IsNull("CFD_NoCertificado")) CFD_NoCertificado = dtResultado.Rows[0]["CFD_NoCertificado"].ToString();
                    if (!dtResultado.Rows[0].IsNull("CFD_ComprobanteStr")) CFD_ComprobanteStr = dtResultado.Rows[0]["CFD_ComprobanteStr"].ToString();
                    if (!dtResultado.Rows[0].IsNull("CFD_Serie")) CFD_Serie = dtResultado.Rows[0]["CFD_Serie"].ToString();
                    if (!dtResultado.Rows[0].IsNull("CFD_CodigoDeBarras")) CFD_CodigoDeBarras = dtResultado.Rows[0]["CFD_CodigoDeBarras"].ToString();
                    if (!dtResultado.Rows[0].IsNull("CFD_Fecha")) CFD_Fecha = Convert.ToDateTime(dtResultado.Rows[0]["CFD_Fecha"]);
                    if (!dtResultado.Rows[0].IsNull("CFD_Folio")) CFD_Folio = Convert.ToInt64(dtResultado.Rows[0]["CFD_Folio"]);
                    if (!dtResultado.Rows[0].IsNull("CFD_Sello")) CFD_Sello = dtResultado.Rows[0]["CFD_Sello"].ToString();
                    if (!dtResultado.Rows[0].IsNull("TFD_UUID")) TFD_UUID = dtResultado.Rows[0]["TFD_UUID"].ToString();
                    if (!dtResultado.Rows[0].IsNull("TFD_FechaTimbrado")) TFD_FechaTimbrado = Convert.ToDateTime(dtResultado.Rows[0]["TFD_FechaTimbrado"]);
                    if (!dtResultado.Rows[0].IsNull("TFD_NoCertificadoSAT")) TFD_NoCertificadoSAT = dtResultado.Rows[0]["TFD_NoCertificadoSAT"].ToString();
                    if (!dtResultado.Rows[0].IsNull("TFD_SelloSAT")) TFD_SelloSAT = dtResultado.Rows[0]["TFD_SelloSAT"].ToString();
                    if (!dtResultado.Rows[0].IsNull("FechaHoraLocal")) FechaHoraLocal = Convert.ToDateTime(dtResultado.Rows[0]["FechaHoraLocal"]);
                    if (!dtResultado.Rows[0].IsNull("PAC")) PAC = dtResultado.Rows[0]["PAC"].ToString();
                    this.Clone();
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
            return result;
        }
        #endregion

        #endregion

        #region Miembros de ICloneable
        public object Clone()
        {
            _copia = new DAOXml_WS_Facturalo(_conexion);
            Type tipo = typeof(Xml_WS_Facturalo);
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

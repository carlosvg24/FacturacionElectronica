using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VBFactPaquetes.Comun;
using VBFactPaquetes.Comun.Log;
using VBFactPaquetes.Model.Facturacion;
using VBFactPaquetes.Model.PantallaFacturacion;

namespace VBFactPaquetes.DAO.Facturacion
{

    /// <summary>
    /// Acceso a los objetos de la Base de Datos 
    /// </summary>
    public class DAOFacturacion
    {
        private DataSet dsResultado;
        private DataTable dtResultado;
        private SqlConnection con = new SqlConnection();
        private SqlCommand cmd = new SqlCommand();
        private ConexionBD connection = new ConexionBD();
        private SqlDataAdapter adapter = new SqlDataAdapter();


        public DataSet ConsultarPagosVivaPaquetes(Pago pago)
        {
            dsResultado = new DataSet();

            try
            {
                con = connection.Conexion();

                cmd = new SqlCommand("[Facturacion].[SPVBFactPaqConsPagosParaFacturar]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PNR", SqlDbType.VarChar, 8).Value = pago.PNR;
                adapter.SelectCommand = cmd;
                connection.openSQLConnection(con);
                adapter.Fill(dsResultado);

            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pago", pago);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }
            finally
            {
                connection.closeSQLConnection(con);
                cmd.Dispose();
            }

            return dsResultado;
        }

        public DataTable PagosFactura(Pago pago, String accion)
        {
            try
            {
                con = new SqlConnection();
                adapter = new SqlDataAdapter();
                connection = new ConexionBD();
                dtResultado = new DataTable();
                con = connection.Conexion();
                cmd = new SqlCommand("[Facturacion].[SPVBFactPaqPagosFactura]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Accion", SqlDbType.VarChar, 5).Value = accion;
                cmd.Parameters.Add("@IdFactPaqPagosFactura", SqlDbType.BigInt).Value = pago.IdFactPagosFactura;
                cmd.Parameters.Add("@IdFactPaqReserva", SqlDbType.BigInt).Value = pago.IdFactPaqReserva;
                cmd.Parameters.Add("@IdFactPaqPagos", SqlDbType.BigInt).Value = pago.IdFactPaqPagos;
                cmd.Parameters.Add("@VersionCFDI", SqlDbType.VarChar, 5).Value = pago.LstDatosGralDTO == null ? "" : pago.LstDatosGralDTO[0].VersionCFDI;
                cmd.Parameters.Add("@NoCertificadoSAT", SqlDbType.VarChar, 100).Value = pago.NoCertificadoSAT;
                cmd.Parameters.Add("@NoCertificadoEmisor", SqlDbType.VarChar, 100).Value = pago.NoCertificado;
                cmd.Parameters.Add("@CertificadoComprobante", SqlDbType.VarChar, 5000).Value = pago.CertificadoComprobante;
                cmd.Parameters.Add("@UUID", SqlDbType.VarChar, 100).Value = pago.UUID;
                cmd.Parameters.Add("@RFCProveedorCertifica", SqlDbType.VarChar, 13).Value = pago.RFCProveedorCertifica;
                cmd.Parameters.Add("@SelloSAT", SqlDbType.VarChar, 1000).Value = pago.SelloSAT;
                cmd.Parameters.Add("@SelloDigital", SqlDbType.VarChar, 1000).Value = pago.SelloDigital;
                cmd.Parameters.Add("@CadenaOriginal", SqlDbType.VarChar, 1000).Value = pago.CadenaOriginal;
                cmd.Parameters.Add("@FechaEmision", SqlDbType.DateTime).Value = pago.FechaEmision;
                cmd.Parameters.Add("@FechaTimbrado", SqlDbType.DateTime).Value = pago.FechaTimbrado;
                cmd.Parameters.Add("@IdEstatusPagoFact", SqlDbType.Int).Value = null;
                cmd.Parameters.Add("@RFCCliente", SqlDbType.VarChar, 14).Value = pago.LstDatosFiscales == null ? "" : pago.LstDatosFiscales[0].RFC;
                cmd.Parameters.Add("@RazonSocialCliente", SqlDbType.VarChar, 300).Value = pago.LstDatosFiscales == null ? "" : String.IsNullOrEmpty(pago.LstDatosFiscales[0].RazonSocial) ? String.Empty : pago.LstDatosFiscales[0].RazonSocial;
                cmd.Parameters.Add("@CodigoCFDICliente", SqlDbType.VarChar, 4).Value = pago.LstDatosFiscales == null ? "" : pago.LstDatosFiscales[0].CodigoUsoCFDI;
                cmd.Parameters.Add("@Domicilio", SqlDbType.VarChar, 500).Value = pago.LstDatosFiscales == null ? "" : pago.LstDatosFiscales[0].DireccionFiscal;
                cmd.Parameters.Add("@Pais", SqlDbType.VarChar, 500).Value = pago.LstDatosFiscales == null ? "" : pago.LstDatosFiscales[0].Pais;
                cmd.Parameters.Add("@Estado", SqlDbType.VarChar, 500).Value = pago.LstDatosFiscales == null ? "" : pago.LstDatosFiscales[0].Estado;
                cmd.Parameters.Add("@Municipio", SqlDbType.VarChar, 500).Value = pago.LstDatosFiscales == null ? "" : pago.LstDatosFiscales[0].Municipio;
                cmd.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 100).Value = pago.LstDatosFiscales == null ? "" : pago.LstDatosFiscales[0].CodigoPostal.ToString();
                cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 20).Value = pago.LstDatosFiscales == null ? "" : pago.LstDatosFiscales[0].Telefono;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = pago.LstDatosFiscales == null ? "" : String.IsNullOrEmpty(pago.LstDatosFiscales[0].Email) ? String.Empty : pago.LstDatosFiscales == null ? "" : pago.LstDatosFiscales[0].Email;
                cmd.Parameters.Add("@FolioFactura", SqlDbType.VarChar, 50).Value = pago.NoFolio;
                cmd.Parameters.Add("@XMLResponse", SqlDbType.VarChar).Value = pago.XMLResponse;
                cmd.Parameters.Add("@ArchivoPDF", SqlDbType.VarBinary).Value = pago.ArchivoPDF;

                adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                connection.openSQLConnection(con);
                adapter.Fill(dtResultado);
            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pago", pago);
                parametros.Add("accion", accion);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }
            finally
            {
                connection.closeSQLConnection(con);
                cmd.Dispose();
            }

            return dtResultado;
        }



        /*Metodo Detalle Paquete*/
        public DataSet DetalleSeccion(Pago pago)
        {

            dsResultado = new DataSet();

            try
            {
                con = connection.Conexion();

                cmd = new SqlCommand("[Facturacion].[SPVBFactPaqConsPagosParaFacturar]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PNR", SqlDbType.VarChar, 8).Value = pago.PNR;

                adapter.SelectCommand = cmd;
                connection.openSQLConnection(con);

                adapter.Fill(dsResultado);

               
            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pago", pago);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }
            finally
            {
                connection.closeSQLConnection(con);
                cmd.Dispose();
            }

            return dsResultado;
        }

        public DataSet CRUDFacturaBoleto(Pago pago, String accion)
        {
            dsResultado = new DataSet();
            byte[] pdf = null;

            try
            {
                pdf = pago.LstConcepto.Find(x => x.PNRBoleto.Length > 0).PDF;
                con = new SqlConnection();
                adapter = new SqlDataAdapter();
                connection = new ConexionBD();
                dtResultado = new DataTable();
                con = connection.Conexion();
                cmd = new SqlCommand("[Facturacion].[SPVBFactPaqFacturaBoleto]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Accion", SqlDbType.VarChar, 5).Value = accion;
                cmd.Parameters.Add("@IdFactPaqReserva", SqlDbType.BigInt).Value = pago.IdFactPaqReserva;
                cmd.Parameters.Add("@IdFactPaqPagos", SqlDbType.BigInt).Value = pago.IdFactPaqPagos;
                cmd.Parameters.Add("@IdFactPaqPagosConceptos", SqlDbType.BigInt).Value = pago.LstConcepto.Find(x => x.PNRBoleto.Length > 0).IdFactPaqPagosConceptos;
                cmd.Parameters.Add("@PNR", SqlDbType.VarChar).Value = pago.LstConcepto.Find(x => x.PNRBoleto.Length > 0).PNRBoleto;

                cmd.Parameters.Add("@XML", SqlDbType.VarChar).Value = pago.LstConcepto.Find(x => x.PNRBoleto.Length > 0).XML;
                cmd.Parameters.Add("@ArchivoPDF", SqlDbType.VarBinary).Value = pdf == null ? new byte[0] : pdf;

                adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                connection.openSQLConnection(con);
                adapter.Fill(dsResultado);

            }
            catch(Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pago", pago);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }
            finally
            {
                connection.closeSQLConnection(con);
                cmd.Dispose();
            }

            return dsResultado;

        }

        public DataTable ObtenerFacturasComprobante(Pago pago)
        {
            dtResultado = new DataTable();

            try
            {
                con = new SqlConnection();
                adapter = new SqlDataAdapter();
                connection = new ConexionBD();
                dtResultado = new DataTable();
                con = connection.Conexion();
                cmd = new SqlCommand("[Facturacion].[SPVBFactPaqDatosCFDI]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IdFactPaqReserva", SqlDbType.BigInt).Value = pago.IdFactPaqReserva;
                cmd.Parameters.Add("@IdFactPaqPagos", SqlDbType.BigInt).Value = pago.IdFactPaqPagos;

                adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                connection.openSQLConnection(con);
                adapter.Fill(dtResultado);
            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pago", pago);
                throw new Excepciones(MethodBase.GetCurrentMethod().DeclaringType.Name,
                                      MethodBase.GetCurrentMethod().Name,
                                      parametros,
                                      ex, Excepciones.TipoPortal.VivaPaquetes);
            }
            finally
            {
                connection.closeSQLConnection(con);
                cmd.Dispose();
            }

            return dtResultado;
        }

    }
}

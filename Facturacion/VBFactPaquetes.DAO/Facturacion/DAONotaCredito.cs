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

namespace VBFactPaquetes.DAO.Facturacion
{
    public class DAONotaCredito
    {
        private DataSet dtResultado;
        //DataTable dtResultado;
        DataSet dsResNav;
        DataSet dsRes;
        DataTable dtRes;
        private SqlConnection con = new SqlConnection();
        private SqlCommand cmd = new SqlCommand();
        private ConexionBD connection = new ConexionBD();
        private SqlDataAdapter adapter = new SqlDataAdapter();




        public DataTable CRUDNotasCredito(Pago pagosDTO, String accion)
        {
            try
            {
                con = new SqlConnection();
                adapter = new SqlDataAdapter();
                connection = new ConexionBD();
                dtRes = new DataTable();

                con = connection.Conexion();

                cmd = new SqlCommand("[Facturacion].[SPVOIFactYavNotasCredito]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Accion", SqlDbType.VarChar, 5).Value = accion;
                cmd.Parameters.Add("@IdFactYavReserva", SqlDbType.BigInt).Value = pagosDTO.IdFactPaqReserva;
                cmd.Parameters.Add("@IdFactYavPagos", SqlDbType.BigInt).Value = pagosDTO.IdFactPaqPagos;
                cmd.Parameters.Add("@IdFactYavNotaCredito", SqlDbType.BigInt).Value = pagosDTO.IdFactPaqNotaCredito;
                cmd.Parameters.Add("@VersionCFDI", SqlDbType.VarChar, 5).Value = pagosDTO.LstDatosGralDTO[0].VersionCFDI;
                cmd.Parameters.Add("@NoCertificadoSAT", SqlDbType.VarChar, 100).Value = pagosDTO.NoCertificadoSAT;
                cmd.Parameters.Add("@NoCertificadoEmisor", SqlDbType.VarChar, 100).Value = pagosDTO.NoCertificado;
                cmd.Parameters.Add("@CertificadoComprobante", SqlDbType.VarChar, 5000).Value = pagosDTO.CertificadoComprobante;
                cmd.Parameters.Add("@UUID", SqlDbType.VarChar, 100).Value = pagosDTO.UUID;
                cmd.Parameters.Add("@RFCProveedorCertifica", SqlDbType.VarChar, 13).Value = pagosDTO.RFCProveedorCertifica;
                cmd.Parameters.Add("@SelloSAT", SqlDbType.VarChar, 1000).Value = pagosDTO.SelloSAT;
                cmd.Parameters.Add("@SelloDigital", SqlDbType.VarChar, 1000).Value = pagosDTO.SelloDigital;
                cmd.Parameters.Add("@CadenaOriginal", SqlDbType.VarChar, 1000).Value = pagosDTO.CadenaOriginal;
                cmd.Parameters.Add("@FechaEmision", SqlDbType.DateTime).Value = pagosDTO.FechaEmision;
                cmd.Parameters.Add("@FechaTimbrado", SqlDbType.DateTime).Value = pagosDTO.FechaTimbrado;
                cmd.Parameters.Add("@IdEstatusPagoFact", SqlDbType.Int).Value = null;
                cmd.Parameters.Add("@RFCCliente", SqlDbType.VarChar, 14).Value = pagosDTO.LstDatosFiscales[0].RFC;
                cmd.Parameters.Add("@RazonSocialCliente", SqlDbType.VarChar, 300).Value = String.IsNullOrEmpty(pagosDTO.LstDatosFiscales[0].RazonSocial) ? String.Empty : pagosDTO.LstDatosFiscales[0].RazonSocial;
                cmd.Parameters.Add("@CodigoCFDICliente", SqlDbType.VarChar, 4).Value = pagosDTO.LstDatosFiscales[0].CodigoUsoCFDI;
                cmd.Parameters.Add("@Domicilio", SqlDbType.VarChar, 500).Value = pagosDTO.LstDatosFiscales[0].DireccionFiscal;
                cmd.Parameters.Add("@Pais", SqlDbType.VarChar, 500).Value = pagosDTO.LstDatosFiscales[0].Pais;
                cmd.Parameters.Add("@Estado", SqlDbType.VarChar, 500).Value = pagosDTO.LstDatosFiscales[0].Estado;
                cmd.Parameters.Add("@Municipio", SqlDbType.VarChar, 500).Value = pagosDTO.LstDatosFiscales[0].Municipio;
                cmd.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 100).Value = pagosDTO.LstDatosFiscales[0].CodigoPostal;
                cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 20).Value = pagosDTO.LstDatosFiscales[0].Telefono;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = String.IsNullOrEmpty(pagosDTO.LstDatosFiscales[0].Email) ? String.Empty : pagosDTO.LstDatosFiscales[0].Email;
                cmd.Parameters.Add("@XMLResponse", SqlDbType.VarChar).Value = pagosDTO.XMLResponse;
                cmd.Parameters.Add("@ArchivoPDF", SqlDbType.VarBinary).Value = pagosDTO.ArchivoPDF;

                adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                connection.openSQLConnection(con);
                adapter.Fill(dtRes);

                return dtRes;
            }
            catch (Excepciones ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Dictionary<string, object> parametros = new Dictionary<string, object>();
                parametros.Add("pagosDTO", pagosDTO);
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
        }
    }
}

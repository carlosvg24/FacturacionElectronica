using Comun.Security;
using Facturacion.ENT;
using Facturacion.ENT.Comun;
using Facturacion.ENT.NotaCredito;
using MetodosComunes;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.DAL.NotaCredito
{    
    public class DALNotaCredito
    {
        #region Propiedades Publicas
        public Database SqlDatabase { get; set; }

        public DbConnection Cn { get; set; }

        public DbTransaction Tx { get; set; }

        #endregion

        #region Constructor

        public DALNotaCredito(string cadenaConexion)
        {
            this.SqlDatabase = new SqlDatabase(cadenaConexion);
        }

        #endregion

        #region Metodos Publicos

        public Int64 InsertUtNotasCreditoCab(NotasCreditoCab.UtNotasCreditoCab notasCreditoCab)
        {
            SqlParameter param = new SqlParameter("@lista", SqlDbType.Structured);

            DbCommand sqlCommand = this.SqlDatabase.GetStoredProcCommand("uspFac_InsertaNotasCreditoCab");

            param.Value = notasCreditoCab;
            sqlCommand.Parameters.Add(param);
            return (Int64)this.SqlDatabase.ExecuteScalar(sqlCommand, this.Tx);
            
        }

        public void InsertUtNotasCreditoDet(NotasCreditoDet.UtNotasCreditoDet notasCreditoDet)
        {
            SqlParameter param = new SqlParameter("@lista", SqlDbType.Structured);

            DbCommand sqlCommand = this.SqlDatabase.GetStoredProcCommand("uspFac_InsertNotasCredito_Det");

            param.Value = notasCreditoDet;
            sqlCommand.Parameters.Add(param);
            this.SqlDatabase.ExecuteNonQuery(sqlCommand, this.Tx);
        }

        public void InsertUtNotasCreditoIVADet(NotaCreditoIVADet.UtNotasCreditoivaDet utNotasCreditoivaDet)
        {
            SqlParameter param = new SqlParameter("@lista", SqlDbType.Structured);

            DbCommand sqlCommand = this.SqlDatabase.GetStoredProcCommand("uspFac_InsertNotasCreditoIVA_Det");

            param.Value = utNotasCreditoivaDet;
            sqlCommand.Parameters.Add(param);
            this.SqlDatabase.ExecuteNonQuery(sqlCommand, this.Tx);
        }


        public void InsertNotaCreditoCFDIDet(Int64 idNotaCreditoCab, ENTXmlPegaso responsePegaso)
        {
            DbCommand sqlCommand = this.SqlDatabase.GetStoredProcCommand("uspFac_InsertaNotaCreditoCFDIDet");

            this.SqlDatabase.AddInParameter(sqlCommand, "@IdNotaCreditoCab", DbType.Int64, idNotaCreditoCab);
            this.SqlDatabase.AddInParameter(sqlCommand, "@TransaccionID", DbType.String, responsePegaso.Transaccion_Id);
            this.SqlDatabase.AddInParameter(sqlCommand, "@CFDI", DbType.String, responsePegaso.CFD_ComprobanteStr);
            this.SqlDatabase.AddInParameter(sqlCommand, "@CadenaOriginal", DbType.String, responsePegaso.CFD_CadenaOriginal);
            this.SqlDatabase.AddInParameter(sqlCommand, "@FechaTimbrado", DbType.String,string.Format("{0:yyyy-MM-dd} {0:HH:mm:ss}", responsePegaso.FechaTimbrado));

            this.SqlDatabase.ExecuteNonQuery(sqlCommand, this.Tx);
        }

        public void InsertUpdateUtENTGlobalticketsDet(GlobalTickets_Det.UtGlobalticketsDet globalTicketsDet)
        {
            SqlParameter param = new SqlParameter("@lista", SqlDbType.Structured);

            DbCommand sqlCommand = this.SqlDatabase.GetStoredProcCommand("uspFac_InsertUpdateGlobalticketsDet");

            param.Value = globalTicketsDet;
            sqlCommand.Parameters.Add(param);
            this.SqlDatabase.ExecuteNonQuery(sqlCommand, this.Tx);
        }

        public Int64 InsertCFDIRelacionadosDet(ENTCfdirelacionadosDet cfdiRelacionado)
        {
            DbCommand sqlCommand = this.SqlDatabase.GetStoredProcCommand("uspFac_InsCFDIRelacionadosDet");

            this.SqlDatabase.AddOutParameter(sqlCommand, "@IdCfdiRel", DbType.Int64,8);
            this.SqlDatabase.AddInParameter(sqlCommand, "@IdCFDI", DbType.Int64,cfdiRelacionado.IdCFDI);
            this.SqlDatabase.AddInParameter(sqlCommand, "@IdCFDIVinculado", DbType.Int64, cfdiRelacionado.IdCFDIVinculado);
            this.SqlDatabase.AddInParameter(sqlCommand, "@UUIDVinculado", DbType.String, cfdiRelacionado.UUIDVinculado);
            this.SqlDatabase.AddInParameter(sqlCommand, "@TipoComprobante", DbType.String, cfdiRelacionado.TipoComprobante);
            this.SqlDatabase.AddInParameter(sqlCommand, "@TipoRelacion", DbType.String, cfdiRelacionado.TipoRelacion);

            this.SqlDatabase.ExecuteNonQuery(sqlCommand, this.Tx);

            return (Int64)this.SqlDatabase.GetParameterValue(sqlCommand, "@IdCfdiRel");
        }

        public void InsertUtNotasCreditoCargo(NotasCreditoCargo.UtNotasCreditoCargo notasCreditoCargo)
        {
            SqlParameter param = new SqlParameter("@lista", SqlDbType.Structured);

            DbCommand sqlCommand = this.SqlDatabase.GetStoredProcCommand("uspFac_InsertaNotasCreditoCargos_Det");

            param.Value = notasCreditoCargo;
            sqlCommand.Parameters.Add(param);
            this.SqlDatabase.ExecuteNonQuery(sqlCommand, this.Tx);
        }


        #region Transaction Comands
        public void BeginTran()
        {
            this.Cn = this.SqlDatabase.CreateConnection();
            this.Cn.Open();
            this.Tx = this.Cn.BeginTransaction();
        }

        public void CommitTran()
        {
            this.Tx.Commit();
            this.Cn.Close();

            this.Tx = null;
        }

        public void RollBackTran()
        {
            this.Tx.Rollback();
            this.Cn.Close();

            this.Tx = null;
        }

        #endregion

        #region Metodos Estaticos

        public static List<StoredProcedureValidation> DALValidarExistenciaSp(StoredProcedureValidation.UtStoredProcedureValidation ut)
        {
            Encrypt encrypt = new Encrypt();
            Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));
            SqlParameter param = new SqlParameter("@lista", SqlDbType.Structured);

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_ExistStoredProcedure");

            param.Value = ut;
            sqlCommand.Parameters.Add(param);
            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<StoredProcedureValidation>.ConvertDataSetToList(data);
        }

        public static ResponseGeneric DALTestConection(string cadenaConexionKey)
        {
            Encrypt encrypt = new Encrypt();
            SqlConnection sqlConexion = new SqlConnection(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings[cadenaConexionKey].ToString()));

            try
            {
                //abrimos conexion
                sqlConexion.Open();
                return new ResponseGeneric() { Succes = true };
            }
            catch (SqlException e)
            {
                return new ResponseGeneric() { Succes = false, Message = string.Format("Fallo conexion a {0}: {1} ", cadenaConexionKey, e.Message) };
            }
            finally
            {
                if (sqlConexion != null && sqlConexion.State == ConnectionState.Open)
                {
                    sqlConexion.Close();
                }
            }
        }

        //public static List<PagosFacturadosDiferenteDia> DALGetPagosFacturadosDiferentedia(DateTime fecha)
        //{
        //    Encrypt encrypt = new Encrypt();
        //    Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

        //    DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_GetPagosFacturadosDiferenteDia");

        //    sqlDataBase.AddInParameter(sqlCommand, "@Fecha", DbType.Date, fecha);

        //    IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

        //    return FnGenericas<PagosFacturadosDiferenteDia>.ConvertDataSetToList(data);
        //}


        /// <summary>
        /// Metodo que Obtiene los pagos que se va a realizar la nota de credito
        /// </summary>
        /// <param name="fechaInicial">Para el rengo de obtencion de los pagos</param>
        /// <param name="fechaFinal">Para el rengo de obtencion de los pagos</param>
        /// <param name="storedProcedure">StoredProcedure para obtener diferente tipo de pagossegun se necesite </param>
        /// <returns></returns>
        /// <example>Ejemplos de stored uspFac_GetPagosFacturadosDiferenteDia,uspFac_GetPagosHijosFacturadosDiferenteDiaPadre </example>
        public static List<PagosParaNotaCredito> DALGetPagosParaNotaCredito(DateTime fechaInicial, DateTime fechaFinal,string storedProcedure)
        {
            Encrypt encrypt = new Encrypt();
            Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand(storedProcedure);
            sqlCommand.CommandTimeout = 0;

            sqlDataBase.AddInParameter(sqlCommand, "@FechaIni", DbType.Date, fechaInicial);
            sqlDataBase.AddInParameter(sqlCommand, "@FechaFin", DbType.Date, fechaFinal);
            
            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<PagosParaNotaCredito>.ConvertDataSetToList(data);
        }        

        public static List<Concepto> DALGetFacturaDetByPaymentID(string cadenaPaymentID)
        {
            Encrypt encrypt = new Encrypt();
            Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_GetFacturasDetByPaymentId");

            sqlDataBase.AddInParameter(sqlCommand, "@CadenaPaymentID", DbType.String, cadenaPaymentID);

            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<Concepto>.ConvertDataSetToList(data);
        }


        public static List<Traslado> DALGetFacturaIVADetByPaymentID(string cadenaPaymentID)
        {
            Encrypt encrypt = new Encrypt();
            Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_GetFacturasIVADetByPaymentId");

            sqlDataBase.AddInParameter(sqlCommand, "@CadenaPaymentID", DbType.String, cadenaPaymentID);

            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<Traslado>.ConvertDataSetToList(data);
        }


        public static Comprobante DALGetFacturaCab(Int64 idFacturaCab)
        {
            Encrypt encrypt = new Encrypt();
            Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_GetFacturasCab_POR_PK");

            sqlDataBase.AddInParameter(sqlCommand, "@IdFacturaCab", DbType.Int64, idFacturaCab);

            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<Comprobante>.ConvertDataSetObject(data);
        }

        public static List<ENTFacturasDet> DALGetFacturasDet(Int64 idFacturaCab, string cadenaIdFacturaDet)
        {
            Encrypt encrypt = new Encrypt();
            Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_GetFacturasDet_ByLote");

            sqlDataBase.AddInParameter(sqlCommand, "@IdFacturaCab", DbType.Int64, idFacturaCab);
            sqlDataBase.AddInParameter(sqlCommand, "@CadenaIdFacturaDet", DbType.String, cadenaIdFacturaDet);

            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<ENTFacturasDet>.ConvertDataSetToList(data);
        }

        public static List<GlobalTickets_Det> DALGetGlobalTicketsDet(string cadenaIdFacturaDet,Int64? idFacturaCab = null)
        {
            Encrypt encrypt = new Encrypt();
            Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_GetGlobalTicketsByIdFacturaCabStringPayment");

            sqlDataBase.AddInParameter(sqlCommand, "@IdFacturaCab", DbType.Int64, idFacturaCab);
            sqlDataBase.AddInParameter(sqlCommand, "@CadenaPaymentId", DbType.String, cadenaIdFacturaDet);

            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<GlobalTickets_Det>.ConvertDataSetToList(data);
        }


        public static List<PagosParaNotaCredito> DalGetPagosFacturasClienteParaNotaCredito(DateTime fechaInicial, DateTime fechaFinal)
        {
            Encrypt encrypt = new Encrypt();
            Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_GetPagosClienteParaNotaCredito");

            sqlDataBase.AddInParameter(sqlCommand, "@FechaIni", DbType.DateTime, fechaInicial);
            sqlDataBase.AddInParameter(sqlCommand, "@FechaFin", DbType.DateTime, fechaFinal);

            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<PagosParaNotaCredito>.ConvertDataSetToList(data);
        }

        public static string DALGetCadenaConexion(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ToString();
        }

        #endregion

        #endregion

    }
}

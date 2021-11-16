using Comun.Security;
using Facturacion.ENT;
using Facturacion.ENT.Comun;
using Facturacion.ENT.FacturaGlobal;
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

namespace Facturacion.DAL.FacturaGlobal
{
    public class DALFacturaGlobal
    {
        #region Constructor

        public DALFacturaGlobal(string cadenaConexion)
        {
            this.SqlDatabase = new SqlDatabase(cadenaConexion);
        }

        #endregion

        #region Propiedades Publicas
        public Database SqlDatabase { get; set; }

        public DbConnection Cn { get; set; }

        public DbTransaction Tx { get; set; }

        #endregion

        #region Metodos Publicos

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

        public decimal InsertUtFacturasCab(FacturasCab.UtFacturasCab facturasCab)
        {
            decimal idFacturaCab = 0;

            SqlParameter param = new SqlParameter("@lista", SqlDbType.Structured);

            DbCommand sqlCommand = this.SqlDatabase.GetStoredProcCommand("uspFac_InsertaFacturasCab");

            param.Value = facturasCab;
            sqlCommand.Parameters.Add(param);
            idFacturaCab = (decimal)this.SqlDatabase.ExecuteScalar(sqlCommand, this.Tx);

            return idFacturaCab;
        }

        public void InsertUtENTFacturasDet(FacturasDet.UtFacturasDet facturasDet)
        {
            SqlParameter param = new SqlParameter("@lista", SqlDbType.Structured);

            DbCommand sqlCommand = this.SqlDatabase.GetStoredProcCommand("uspFac_InsertaFacturasDet");

            param.Value = facturasDet;
            sqlCommand.Parameters.Add(param);
            this.SqlDatabase.ExecuteNonQuery(sqlCommand, this.Tx);
        }

        public void InsertUtENTFacturasivaDet(FacturaIVADet.UtFacturasivaDet facturasIVADet)
        {
            SqlParameter param = new SqlParameter("@lista", SqlDbType.Structured);

            DbCommand sqlCommand = this.SqlDatabase.GetStoredProcCommand("uspFac_InsertarFacturasIVADet");

            param.Value = facturasIVADet;
            sqlCommand.Parameters.Add(param);
            this.SqlDatabase.ExecuteNonQuery(sqlCommand, this.Tx);
        }

        public void InsertUtENTGlobalticketsDet(GlobalticketsDet.UtGlobalticketsDet globalTicketsDet)
        {
            SqlParameter param = new SqlParameter("@lista", SqlDbType.Structured);

            DbCommand sqlCommand = this.SqlDatabase.GetStoredProcCommand("uspFac_InsertUpdateGlobalticketsDet");

            param.Value = globalTicketsDet;
            sqlCommand.Parameters.Add(param);
            this.SqlDatabase.ExecuteNonQuery(sqlCommand, this.Tx);
        }

        public void InsertUtENTFacturasCargos_Det(FacturasCargoDet.UtFacturasCargoDet facturasCargoDet)
        {
            SqlParameter param = new SqlParameter("@lista", SqlDbType.Structured);

            DbCommand sqlCommand = this.SqlDatabase.GetStoredProcCommand("uspFac_InsertaFacturasCargos_Det");

            param.Value = facturasCargoDet;
            sqlCommand.Parameters.Add(param);
            this.SqlDatabase.ExecuteNonQuery(sqlCommand, this.Tx);
        }


        public void InsertFacturasCFDIDet(Int64 idFacturaCab,ENTXmlPegaso responsePegaso)
        {            
            DbCommand sqlCommand = this.SqlDatabase.GetStoredProcCommand("uspFac_InsertarFacturasCFDIDet");

            this.SqlDatabase.AddInParameter(sqlCommand, "@IdFacturaCab", DbType.Int64, idFacturaCab);
            this.SqlDatabase.AddInParameter(sqlCommand, "@TransaccionID", DbType.String, responsePegaso.Transaccion_Id);
            this.SqlDatabase.AddInParameter(sqlCommand, "@CFDI", DbType.String, responsePegaso.CFD_ComprobanteStr);
            this.SqlDatabase.AddInParameter(sqlCommand, "@CadenaOriginal", DbType.String, responsePegaso.CFD_CadenaOriginal);
            this.SqlDatabase.AddInParameter(sqlCommand, "@FechaTimbrado", DbType.String,  responsePegaso.FechaTimbrado.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            this.SqlDatabase.ExecuteNonQuery(sqlCommand, this.Tx);
        }

        #endregion

        #region Metodos Publicos Estaticos

        public static List<PagoTraslados> DALGetPagos(decimal IVA, bool divididos, DateTime fechaInicial,DateTime fechaFinal,string moneda=null,Int64? folioPrefacatura=null)
        {           
                /*encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["sqlViva"].ToString())*/
                Encrypt encrypt = new Encrypt();
                Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

                DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_GetPagosFactGlobal");
                sqlCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BigSqlCommandTimeOut"]);

                sqlDataBase.AddInParameter(sqlCommand, "@FechaIni", DbType.Date, fechaInicial);
                sqlDataBase.AddInParameter(sqlCommand, "@FechaFin", DbType.Date, fechaFinal);
                sqlDataBase.AddInParameter(sqlCommand, "@Moneda", DbType.String, moneda);
                sqlDataBase.AddInParameter(sqlCommand, "@IVA", DbType.Decimal, IVA);
                sqlDataBase.AddInParameter(sqlCommand, "@FolioPrefactura", DbType.Int64, folioPrefacatura);
                sqlDataBase.AddInParameter(sqlCommand, "@Divididos", DbType.Boolean,divididos );

            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

                return FnGenericas<PagoTraslados>.ConvertDataSetToList(data);         
        }

        public static List<ENTPagosCab> DALGetPagosHijosPorProcesoAtrasado(DateTime fechaProceso,string cadenaParentPaymentId)
        {
            Encrypt encrypt = new Encrypt();
            Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_GetPagosHijosPorProcesoAtrasado");
            sqlCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LittleSqlCommandTimeOut"]);

            sqlDataBase.AddInParameter(sqlCommand, "@FechaPagoProceso", DbType.Date, fechaProceso);
            sqlDataBase.AddInParameter(sqlCommand, "@CadenaPaymentIdPadre", DbType.String, cadenaParentPaymentId);            

            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<ENTPagosCab>.ConvertDataSetToList(data);
        }

        public static List<T> DALGetCargosComplemento<T>(string  cadenaIdPagos,string moneda)
        {

            /*encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["sqlViva"].ToString())*/
            Encrypt encrypt = new Encrypt();
            Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_GetComplementoByIdPagosCab");
            sqlCommand.CommandTimeout = 180;

            sqlDataBase.AddInParameter(sqlCommand, "@CadenaIdPagosCab", DbType.String, cadenaIdPagos);
            sqlDataBase.AddInParameter(sqlCommand, "@Moneda", DbType.String, moneda);

            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<T>.ConvertDataSetToList(data);
        }

        public static List<MonedaPagos> DALGetPagosByCurrencyCode(DateTime fechaInicial,DateTime fechaFinal)
        {
            /*encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["sqlViva"].ToString())*/

            Encrypt encrypt = new Encrypt();
            Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_GetTipoMonedaPagos");
            sqlCommand.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BigSqlCommandTimeOut"]);
            sqlDataBase.AddInParameter(sqlCommand, "@FechaIni", DbType.Date, fechaInicial);
            sqlDataBase.AddInParameter(sqlCommand, "@FechaFin", DbType.Date, fechaFinal);

            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<MonedaPagos>.ConvertDataSetToList(data);
        }

        public static List<ReservaDetalleTraslado> DALGetTraslados(DateTime fecha,string moneda,decimal IVA,Int64? folioPreFactura=null)
        {
            Encrypt encrypt = new Encrypt();
            Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_GetReservaDetalleFactGlobal");

            sqlDataBase.AddInParameter(sqlCommand, "@Fecha", DbType.Date, fecha);
            sqlDataBase.AddInParameter(sqlCommand, "@Moneda", DbType.String, moneda);
            sqlDataBase.AddInParameter(sqlCommand, "@IVA", DbType.Decimal, IVA);
            sqlDataBase.AddInParameter(sqlCommand, "@FolioPrefactura", DbType.Int64, folioPreFactura);

            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<ReservaDetalleTraslado>.ConvertDataSetToList(data);
        }

        public static string DALGetCadenaConexion(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ToString();
        }

        public static ResponseGeneric DALTestConection(string cadenaConexionKey)
        {
            Encrypt encrypt = new Encrypt();
            SqlConnection sqlConexion = new SqlConnection(encrypt.DecryptKey( ConfigurationManager.ConnectionStrings[cadenaConexionKey].ToString()));

            try
            {
                //abrimos conexion
                sqlConexion.Open();
                return new ResponseGeneric() { Succes = true };
            }
            catch (SqlException e)
            {
                return new ResponseGeneric() { Succes = false, Message = string.Format("Fallo conexion a {0}:{1}",cadenaConexionKey, e.Message) } ;
            }
            finally
            {
                if (sqlConexion != null && sqlConexion.State== ConnectionState.Open)
                {
                    sqlConexion.Close();
                }
            }
        }


        public static List<StoredProcedureValidation> DALValidarExistenciaSp(StoredProcedureValidation.UtStoredProcedureValidation ut)
        {
            Encrypt encrypt = new Encrypt();
            Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));
            SqlParameter param = new SqlParameter("@lista", SqlDbType.Structured);

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_ExistStoredProcedure");

            param.Value = ut;
            sqlCommand.Parameters.Add(param);
            IDataReader data =sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<StoredProcedureValidation>.ConvertDataSetToList(data);
        }


        #endregion
    }
}

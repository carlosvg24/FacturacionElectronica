using System;
using System.Configuration;
using System.Data.SqlClient;
using Comun.Security;

namespace Facturacion.InterfaceRestViva
{
    public static class BLLConfiguracion
    {

     
        #region Propiedades Públicas
        public static SqlConnection Conexion
        {
            get
            {
                Encrypt encrypt = new Encrypt();
                SqlConnection _conexion = new SqlConnection();
                String conString = "";
                conString = encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString());
                _conexion.ConnectionString = conString;
                return _conexion;
            }
        }

        public static SqlConnection ConexionNavitaireWB
        {
            get
            {
                Encrypt encrypt = new Encrypt();
                SqlConnection _conexion = new SqlConnection();
                String conString = "";
                conString = encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBNavitaire"].ToString());
                _conexion.ConnectionString = conString;
                return _conexion;
            }
        }

        #endregion Propiedades Públicas
    }
}

using System;
using System.Web;
using System.IO;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using Comun.Security;

namespace Facturacion.BLL
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

using Comun.Security;
using Facturacion.ENT.NotaCredito;
using Facturacion.ENT.Portal.AuthenticationController;
using MetodosComunes;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.DAL.Portal.AuthenticationController
{
    public static class AuthenticationControllerDAL
    {
        public static SqlDatabase GetSqlDatabaseFacturacion()
        {
            Encrypt encrypt = new Encrypt();
            return new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));
        }

        /// <summary>
        /// Se obtiene la relacion de Los menus, roles y opciones por Usuario
        /// </summary>
        /// <typeparam name="T">Tipo de dato que se regresara</typeparam>
        /// <param name="idUser">IdUsuario </param>
        /// <param name="idRol">Id de rol</param>
        /// <param name="idMenuOpcion">Id del menu</param>
        /// <returns></returns>
        public static List<T> GetRolPermisoMenuByUser<T>(int idUser,int? idRol,int? idMenuOpcion )
        {
            Database sqlDataBase = GetSqlDatabaseFacturacion();

            DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_GetRolesPermisoMenu");

            sqlDataBase.AddInParameter(sqlCommand, "@IdUsuario", DbType.Int32, idUser);
            sqlDataBase.AddInParameter(sqlCommand, "@IdRol", DbType.Int32,idRol);
            sqlDataBase.AddInParameter(sqlCommand, "@IdMenuOpcion", DbType.Int32, idMenuOpcion); 
            IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

            return FnGenericas<T>.ConvertDataSetToList(data);

        }
    }
}

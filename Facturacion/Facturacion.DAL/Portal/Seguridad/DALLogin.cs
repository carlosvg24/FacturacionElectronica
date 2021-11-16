using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.ENT;
using System.Data;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Comun.Security;
using System.Configuration;
using MetodosComunes;

namespace Facturacion.DAL
{
    public class DALLogin
    {
        public DALLogin(string cadenaConexion)
        {
            
        }
        
        #region Metodos Staticos Publicos

        //public static ENTUsuariosCat RecuperarUsuariosLogin()
        //{
        //    Encrypt encrypt = new Encrypt();
        //    Database sqlDataBase = new SqlDatabase(encrypt.DecryptKey(ConfigurationManager.ConnectionStrings["DBFacturacion"].ToString()));

        //    List<SqlParameter> parameters = new List<SqlParameter>()
        //    {
        //        new SqlParameter("@Usuario",SqlDbType.VarChar,256),
        //        new SqlParameter("@Password",SqlDbType.VarChar,500)
        //    };

        //    DbCommand sqlCommand = sqlDataBase.GetStoredProcCommand("uspFac_spS_UsuarioLogin");
        //    sqlCommand.Parameters.AddRange(parameters.ToArray());

        //    IDataReader data = sqlDataBase.ExecuteReader(sqlCommand);

        //    return FnGenericas<ENTUsuariosCat>.ConvertDataSetObject(data);
        //}

        //public List<ENTMenu> RecuperarMenuUsuarios()
        //{
        //    List<ENTMenu> result = new List<ENTMenu>();
        //    DataTable dtResultado = new DataTable();
        //    List<SqlParameter> commandParameters = new List<SqlParameter>();

        //    SqlParameter param0 = new SqlParameter("@Usuario", SqlDbType.VarChar);
        //    param0.Value = Usuario;
        //    commandParameters.Add(param0);
        //    SqlParameter param1 = new SqlParameter("@Password", SqlDbType.VarChar);
        //    param1.Value = Password;
        //    commandParameters.Add(param1);
        //    SqlParameter param2 = new SqlParameter("@IdRol", SqlDbType.VarChar);
        //    param2.Value = IdRol;
        //    commandParameters.Add(param2);
        //    SqlParameter param3 = new SqlParameter("@Bandera", SqlDbType.VarChar);
        //    param3.Value = "GetMenu";
        //    commandParameters.Add(param3);


        //    SqlCommand cmm = new SqlCommand();
        //    cmm.CommandType = CommandType.StoredProcedure;
        //    cmm.CommandText = "usp_FacGetUsuariosPermisos";
        //    cmm.Connection = _conexion;
        //    bool cerrarConexion = false;
        //    foreach (SqlParameter p in commandParameters)
        //    {
        //        cmm.Parameters.Add(p);
        //    }

        //    try
        //    {
        //        if (_conexion.State.Equals(ConnectionState.Closed))
        //        {
        //            _conexion.Open();
        //            cerrarConexion = true;
        //        }
        //        SqlDataAdapter da = new SqlDataAdapter(cmm);
        //        DataSet dsResult = new DataSet();
        //        da.Fill(dsResult, "dtResultado");
        //        dtResultado = dsResult.Tables["dtResultado"];

        //        foreach (DataRow dr in dtResultado.Rows)
        //        {
        //            ENTMenu item = new ENTMenu();
        //            if (!dr.IsNull("IdMenuOpcion")) item.IdMenuOpcion = Convert.ToInt32(dr["IdMenuOpcion"]);
        //            if (!dr.IsNull("Nombre_Menu")) item.Nombre_Menu = dr["Nombre_Menu"].ToString();
        //            if (!dr.IsNull("UrlImagen")) item.UrlImagen = dr["UrlImagen"].ToString();
        //            if (!dr.IsNull("UrlMenu")) item.UrlMenu = dr["UrlMenu"].ToString();
        //            if (!dr.IsNull("Orden")) item.Orden = Convert.ToInt32(dr["Orden"].ToString());
        //            if (!dr.IsNull("Descripcion")) item.Descripcion = dr["Descripcion"].ToString();
        //            if (!dr.IsNull("IdMenuPadre")) item.IdMenuPadre = Convert.ToInt32(dr["IdMenuPadre"].ToString());
        //            if (!dr.IsNull("PermisoAgregar")) item.PermisoAgregar = Convert.ToBoolean(dr["PermisoAgregar"].ToString());
        //            if (!dr.IsNull("PermisoConsultar")) item.PermisoConsultar = Convert.ToBoolean(dr["PermisoConsultar"]);
        //            if (!dr.IsNull("PermisoConsultar")) item.PermisoEditar = Convert.ToBoolean(dr["PermisoEditar"]);
        //            if (!dr.IsNull("PermisoConsultar")) item.PermisoEliminar = Convert.ToBoolean(dr["PermisoEliminar"]);
        //            if (!dr.IsNull("OrdenMostrar")) item.OrdenMostrar = Convert.ToDecimal(dr["OrdenMostrar"]);
        //            if (!dr.IsNull("EsParent")) item.EsParent = Convert.ToBoolean(dr["EsParent"]);
        //            result.Add(item);
        //        }
        //        //if (dtResultado.Rows.Count.Equals(1))
        //        //{
        //        //    IdMenuOpcion = Convert.ToInt32(dtResultado.Rows[0]["IdMenuOpcion"]);
        //        //    Nombre_Menu = dtResultado.Rows[0]["Nombre_Menu"].ToString();
        //        //    UrlImagen = dtResultado.Rows[0]["UrlImagen"].ToString();
        //        //    UrlMenu = dtResultado.Rows[0]["UrlMenu"].ToString();
        //        //    Orden = Convert.ToInt32(dtResultado.Rows[0]["Orden"].ToString());
        //        //    Descripcion = dtResultado.Rows[0]["Descripcion"].ToString();
        //        //    IdMenuPadre = Convert.ToInt32(dtResultado.Rows[0]["IdMenuPadre"].ToString());
        //        //    PermisoAgregar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoAgregar"].ToString());
        //        //    PermisoConsultar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoConsultar"]);
        //        //    PermisoEditar = Convert.ToBoolean(dtResultado.Rows[0]["PermisoEditar"]);
        //        //    OrdenMostrar = Convert.ToDecimal(dtResultado.Rows[0]["OrdenMostrar"]);
        //        //    EsParent = Convert.ToBoolean(dtResultado.Rows[0]["EsParent"]);
        //        //}
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        cmm.Dispose();
        //        if (cerrarConexion)
        //        {
        //            _conexion.Close();
        //        }
        //    }
        //    return result;
        //}

        #endregion

    }
}

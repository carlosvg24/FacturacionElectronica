using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.ENT;
using System.Data;
using System.Reflection;

namespace Facturacion.DAL
{
    public class DALSeguridad : ENTSeguridad
    {
        #region Propiedades Privadas
        protected SqlConnection _conexion;
        protected bool _nuevo = false;
        protected string _idUsuario;
        protected string _proceso;
        protected DALSeguridad _copia;
        #endregion Propiedades Privadas

        #region Constructores
        public DALSeguridad(SqlConnection conexion)
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
            IdRolActual = 0;
            MenuPrincipalPorRol = new List<ENTMenusViva>();
            ListaComboRoles = new Dictionary<int, string>();
            ListaRoles = new List<ENTMenusPorRol>();
        }
        #endregion

        #region Metodos Recuperar
        #region Recuperar RecuperarUsuariosLogin
        //public List<ENTSeguridad> RecuperarUsuariosLogin()
        //{
        //    List<ENTSeguridad> result = new List<ENTSeguridad>();
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
        //    param3.Value = "Login";
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
        //            ENTSeguridad item = new ENTSeguridad();
        //            if (!dr.IsNull("IdUsuario")) item.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
        //            if (!dr.IsNull("Nombre_Usr")) item.Nombre_Usr = dr["Nombre_Usr"].ToString();
        //            if (!dr.IsNull("Apellidos")) item.Apellidos = dr["Apellidos"].ToString();
        //            if (!dr.IsNull("IdAgente")) item.IdAgente = Convert.ToInt32(dr["IdAgente"].ToString());
        //            if (!dr.IsNull("CodigoSAP")) item.CodigoSAP = dr["CodigoSAP"].ToString();
        //            if (!dr.IsNull("CodigoAgente")) item.CodigoAgente = dr["CodigoAgente"].ToString();
        //            if (!dr.IsNull("Nombre")) item.Nombre = dr["Nombre"].ToString();
        //            if (!dr.IsNull("DescripcionRol")) item.DescripcionRol = dr["DescripcionRol"].ToString();
        //            if (!dr.IsNull("IdRol")) item.IdRol = Convert.ToInt32(dr["IdRol"]);
        //            result.Add(item);
        //        }
        //        if (dtResultado.Rows.Count.Equals(1))
        //        {
        //            IdUsuario = Convert.ToInt32(dtResultado.Rows[0]["IdUsuario"]);
        //            Nombre_Usr = dtResultado.Rows[0]["Nombre_Usr"].ToString();
        //            Apellidos = dtResultado.Rows[0]["Apellidos"].ToString();
        //            IdAgente = Convert.ToInt32(dtResultado.Rows[0]["IdAgente"]);
        //            CodigoSAP = dtResultado.Rows[0]["CodigoSAP"].ToString();
        //            CodigoAgente = dtResultado.Rows[0]["CodigoAgente"].ToString();
        //            Nombre = dtResultado.Rows[0]["Nombre"].ToString();
        //            DescripcionRol = dtResultado.Rows[0]["DescripcionRol"].ToString();
        //            IdRol = Convert.ToInt32(dtResultado.Rows[0]["IdRol"]);
        //            this.Clone();
        //        }
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
        #endregion

        #region Miembros de ICloneable
        public object Clone()
        {
            _copia = new DALSeguridad(_conexion);
            Type tipo = typeof(ENTSeguridad);
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

using Facturacion.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.ENT;

namespace Facturacion.BLL.Portal.Seguridad
{
    public class BLLSeguridad : DALSeguridad
    {
        public BLLSeguridad()
        : base(BLLConfiguracion.Conexion)
        {
            BllLogErrores = new BLLBitacoraErrores();
        }




        #region Propiedades Privadas
        private BLLBitacoraErrores BllLogErrores { get; set; }
        #endregion Propiedades Privadas

        #region Métodos Privados
        #endregion Métodos Privados

        #region Propiedades Públicas
        #endregion Propiedades Públicas

        #region Métodos Públicos
        public ENTSeguridad ValidarUsuario(string usuario, string password)
        {
            ENTSeguridad result = new ENTSeguridad();
            BLLUsuariosCat bllUsuario = new BLL.BLLUsuariosCat();
            List<ENTUsuariosCat> listUsuarios = new List<ENTUsuariosCat>();
            result.ListaComboRoles = new Dictionary<int, string>();
            result.ListaRoles = new List<ENTMenusPorRol>();
            result.MenuPrincipalPorRol = new List<ENTMenusViva>();

            //Se recupera la informacion del usuario solicitado
            listUsuarios = bllUsuario.RecuperarUsuariosCatUsuario(usuario);

            try
            {

                if (listUsuarios.Count == 0)
                {
                    //En este caso no se encuentra registrado el usuario solicitado
                    throw new ExceptionViva("Usuario no se encuentra registrado en sistema...");
                }
                else
                {
                    //El usuario se encuentra registrado en la BD

                    ENTUsuariosCat entUsuario = new ENTUsuariosCat();
                    entUsuario = listUsuarios.FirstOrDefault();

                    //Se compara la contraseña registrada en la BD y la capturada por el usuario
                    if (entUsuario.Password == password)
                    {
                        //Se recupera la informacion de la BD
                        result.IdUsuario = entUsuario.IdUsuario;
                        result.Nombre = entUsuario.Nombre;
                        result.Apellidos = entUsuario.Apellidos;
                        result.IdAgente = entUsuario.IdAgente;
                        result.Usuario = entUsuario.Usuario;
                        result.EsValido = true;

                        //Buscar la informacion del Agente
                        BLLAgentesCat bllAgente = new BLLAgentesCat();
                        List<ENTAgentesCat> listaAgentes = new List<ENTAgentesCat>();
                        listaAgentes = bllAgente.RecuperarAgentesCatAgentid(result.IdAgente);

                        //En caso de que exista informacion del Agente se obtiene su informacion
                        if (listaAgentes.Count > 0)
                        {
                            ENTAgentesCat entAgente = listaAgentes.FirstOrDefault();
                            result.CodigoSAP = entAgente.CodigoSAP;
                            result.CodigoAgente = entAgente.CodigoAgente;
                        }

                        //Se recuperan los roles que tiene asignado el usuario
                        BLLUsuariosrolesCnf bllUsuRol = new BLLUsuariosrolesCnf();
                        List<ENTUsuariosrolesCnf> listaRoles = new List<ENTUsuariosrolesCnf>();
                        listaRoles = bllUsuRol.RecuperarUsuariosrolesCnfIdusuario(entUsuario.IdUsuario);

                        if (listaRoles.Count == 0)
                        {
                            throw new ExceptionViva("El usuario no tiene asignado un Rol de Sistema, verifique...");
                        }
                        else
                        {
                            //En caso de que exista al menos un rol se recuperara el catalogo de Roles
                            BLLRolesCat bllRoles = new BLL.BLLRolesCat();
                            List<ENTRolesCat> listaCatRoles = new List<ENTRolesCat>();
                            listaCatRoles = bllRoles.RecuperarTodo();
                            bool esPrimerRol = true;
                            //Se recupera la informacion de todos los roles vinculados al usuario
                            foreach (ENTUsuariosrolesCnf entUsuRol in listaRoles)
                            {
                                ENTMenusPorRol cnfRol = new ENTMenusPorRol();
                                cnfRol = RecuperaConfiguracionRol(entUsuRol, listaCatRoles);
                                //Verifica si se logro obtener la configuracion del rol, si el IdRol = 0 significa que no se configuro
                                if (cnfRol.IdRol > 0)
                                {
                                    result.ListaRoles.Add(cnfRol);
                                    result.ListaComboRoles.Add(cnfRol.IdRol, cnfRol.DescripcionRol);
                                    if (esPrimerRol)
                                    {
                                        result.IdRolActual = cnfRol.IdRol;
                                        result.DescripcionRol = cnfRol.DescripcionRol;
                                        result.MenuPrincipalPorRol = cnfRol.MenuPrincipalPorRol;
                                        esPrimerRol = false;
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        throw new ExceptionViva("Contraseña incorrecta, verifique...");
                    }
                }
            }
            catch (ExceptionViva ex)
            {
                result.EsValido = false;
                result.Error = ex.Message;
            }
            catch (Exception)
            {
                result.EsValido = false;
                result.Error = "Error al validar el usuario, verifique...";
            }
            return result;
        }


        public ENTMenusPorRol RecuperaConfiguracionRol(ENTUsuariosrolesCnf rol, List<ENTRolesCat> catRoles)
        {
            ENTMenusPorRol menuRol = new ENTMenusPorRol();

            if (catRoles.Where(x => x.IdRol == rol.IdRol).Count() > 0)
            {
                ENTRolesCat rolCat = new ENTRolesCat();
                rolCat = catRoles.Where(x => x.IdRol == rol.IdRol).FirstOrDefault();
                if (rolCat.Activo == true)
                {
                    menuRol.IdRol = rolCat.IdRol;
                    menuRol.DescripcionRol = rolCat.Nombre;
                    menuRol.MenuPrincipalPorRol = new List<ENTMenusViva>();

                    //Se recuperan las opciones de Menu asignadas al Rol
                    BLLRolesopcionesCnf bllRolOpcion = new BLL.BLLRolesopcionesCnf();
                    List<ENTRolesopcionesCnf> listaOpciones = new List<ENTRolesopcionesCnf>();

                    listaOpciones = bllRolOpcion.RecuperarRolesopcionesCnfIdrol(rol.IdRol);



                    //Se recupera la configuracion de las opciones del Menu
                    List<ENTOpcionesmenuCat> listaMenus = new List<ENTOpcionesmenuCat>();
                    BLL.BLLOpcionesmenuCat bllOpc = new BLL.BLLOpcionesmenuCat();
                    listaMenus = bllOpc.RecuperarTodo();

                    var listaMnuMod = listaMenus.Where(x => x.IdMenuPadre == 0).OrderBy(x => x.Orden);

                    foreach (ENTOpcionesmenuCat mnuOpc in listaMnuMod)
                    {
                        ENTMenusViva mnuModuloPrin = new ENTMenusViva();
                        mnuModuloPrin = GeneraMenuviva(mnuOpc, listaMenus, listaOpciones);
                        menuRol.MenuPrincipalPorRol.Add(mnuModuloPrin);
                    }

                    //ENTMenusViva menuSalir = new ENTMenusViva();
                    //menuSalir.IdMenuOpcion = 1;
                    //menuSalir.Nombre_Menu = "Salir";
                    //menuSalir.UrlMenu = "Login.aspx";
                    //menuSalir.ListaSubMenus = new List<ENTElementoMenu>();
                    //menuRol.MenuPrincipalPorRol.Add(menuSalir);

                }
            }


            return menuRol;

        }


        private ENTMenusViva GeneraMenuviva(ENTOpcionesmenuCat mnuOpc, List<ENTOpcionesmenuCat> listaMenus, List<ENTRolesopcionesCnf> listaOpciones)
        {
            ENTMenusViva result = new ENTMenusViva();

            //Verifica si la opcion del menu se encuentra dentro de las opciones asignadas al Rol
            ENTRolesopcionesCnf cnfMenuRol = new ENTRolesopcionesCnf();
            cnfMenuRol = listaOpciones.Where(x => x.IdMenuOpcion == mnuOpc.IdMenuOpcion).FirstOrDefault();

            if (cnfMenuRol != null)
            {
                //Verifica si la opcion tiene Submenus y en su caso los recupera
                bool tieneSubMenus = false;
                List<ENTOpcionesmenuCat> listaSubmenus = new List<ENTOpcionesmenuCat>();
                listaSubmenus = listaMenus.Where(x => x.IdMenuPadre == mnuOpc.IdMenuOpcion).OrderBy(x => x.Orden).ToList();
                tieneSubMenus = (listaSubmenus.Count() > 0);

                //Asigna los valores principales del menu
                result.IdMenuOpcion = mnuOpc.IdMenuOpcion;
                result.Nombre_Menu = mnuOpc.Nombre;

                if (!tieneSubMenus)
                {
                    result.UrlMenu = mnuOpc.UrlMenu;
                    result.UrlImagen = mnuOpc.UrlImagen;
                    result.PermisoAgregar = cnfMenuRol.PermisoAgregar;
                    result.PermisoConsultar = cnfMenuRol.PermisoConsultar;
                    result.PermisoEditar = cnfMenuRol.PermisoEditar;
                    result.PermisoEliminar = cnfMenuRol.PermisoEliminar;

                    result.ListaSubMenus = new List<ENTElementoMenu>();
                }
                else
                {
                    result.UrlMenu = "#";
                    result.Descripcion = mnuOpc.Nombre;
                    result.ListaSubMenus = new List<ENTElementoMenu>();

                    foreach (ENTOpcionesmenuCat subMenu in listaSubmenus)
                    {
                        ENTMenusViva entSubMenu = new ENTMenusViva();
                        entSubMenu = GeneraMenuviva(subMenu, listaMenus, listaOpciones);
                        result.ListaSubMenus.Add(entSubMenu);
                    }

                }
            }

            return result;

        }

        public List<ENTMenusViva> RecuperarPermisosUsuario(ENTSeguridad usuario)
        {
            int idRol = usuario.ListaRoles.First().IdRol;

            List<ENTMenusViva> menuPrincipal = new List<ENTMenusViva>();

            ENTMenusViva menu = new ENTMenusViva();
            menu.IdMenuOpcion = 1;
            menu.Nombre_Menu = "Nuevos Aviones";
            menu.UrlMenu = "/mx/nueva-viva";
            menu.ListaSubMenus = new List<ENTElementoMenu>();

            menuPrincipal.Add(menu);

            ENTMenusViva menu2 = new ENTMenusViva();
            menu2.IdMenuOpcion = 2;
            menu2.Nombre_Menu = "Destinos";
            menu2.UrlMenu = "#";
            menu2.ListaSubMenus = new List<ENTElementoMenu>();

            ENTMenusViva submenu1 = new ENTMenusViva();
            submenu1.IdMenuOpcion = 3;
            submenu1.Nombre_Menu = "Todos nuestros destinos";
            submenu1.UrlMenu = "/mx/destinos/nuestros-destinos";
            submenu1.Descripcion = "Conoce todos nuestros destinos";

            ENTMenusViva submenu2 = new ENTMenusViva();
            submenu2.IdMenuOpcion = 4;
            submenu2.Nombre_Menu = "Nuevas rutas para ti";
            submenu2.UrlMenu = "/mx/destinos/nuevas-rutas-para-ti";
            submenu2.Descripcion = "DENTRO DEL TERRITORIO NACIONAL";
            menu2.ListaSubMenus.Add(submenu1);
            menu2.ListaSubMenus.Add(submenu2);

            menuPrincipal.Add(menu2);


            ENTMenusViva menu3 = new ENTMenusViva();
            menu3.IdMenuOpcion = 5;
            menu3.Nombre_Menu = "ABANDONAR SESION";
            menu3.UrlMenu = "#";
            menu3.ListaSubMenus = new List<ENTElementoMenu>();

            menuPrincipal.Add(menu3);


            ENTMenusViva menuSalir = new ENTMenusViva();
            menuSalir.IdMenuOpcion = 1;
            menuSalir.Nombre_Menu = "Salir";
            menuSalir.UrlMenu = "6";
            menuSalir.ListaSubMenus = new List<ENTElementoMenu>();

            menuPrincipal.Add(menuSalir);

            return menuPrincipal;

        }

        #endregion Métodos Públicos
    }


}


using Facturacion.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.ENT;
using Comun.Security;

namespace Facturacion.BLL.Portal.Seguridad
{
    public class BLLLogin
    {
        #region Métodos Privados
        #endregion Métodos Privados

        #region Propiedades Públicas
        #endregion Propiedades Públicas

        #region Métodos Públicos

        public List<ENTMenusViva> RecuperarPermisosUsuario()
        {
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
            menu3.Nombre_Menu = "";
            menu3.UrlMenu = "#";
            menu3.ListaSubMenus = new List<ENTElementoMenu>();

            menuPrincipal.Add(menu3);

            return menuPrincipal;

        }

        #endregion Métodos Públicos
    }


}


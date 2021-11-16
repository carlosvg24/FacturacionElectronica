using Facturacion.BLL.Portal.Seguridad;
using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Facturacion33.PortalAdmin.Master
{
    public partial class MasterViva : System.Web.UI.MasterPage
    {

        public ENTSeguridad Usuario
        {
            get
            {
                if (Session["Usuario"] == null)
                {
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                    return new ENTSeguridad();
                }
                else
                {
                    return (ENTSeguridad)Session["Usuario"];
                }
            }
            set
            {
                Session["Usuario"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Usuario.ListaRoles != null)
            {
                lblUsuario.Text = string.Format("{0} {1}", Usuario.Nombre, Usuario.Apellidos);

                ddlRol.DataSource = Usuario.ListaRoles;
                ddlRol.DataBind();
            }

            HtmlGenericControl tabs = new HtmlGenericControl("ul");
            tabs.ID = "menuPrincipal";
            tabs.Attributes.Add("class", "nav navbar-nav");

            if (Usuario.MenuPrincipalPorRol != null)
            {


                List<ENTMenusViva> menuPrincipal = Usuario.MenuPrincipalPorRol;

                foreach (ENTMenusViva eMenu in menuPrincipal)
                {
                    if (eMenu.ListaSubMenus.Count() == 0)
                    {
                        tabs.Controls.Add(MenuSinNiveles(eMenu));
                    }
                    else
                    {
                        tabs.Controls.Add(MenuConNiveles(eMenu));
                    }
                }

                ControlContainer.Controls.Add(tabs);
            }
            else
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("Login.aspx");

            }


        }


        private HtmlGenericControl MenuSinNiveles(ENTMenusViva opcionMenu)
        {
            HtmlGenericControl li = new HtmlGenericControl("li");
            HtmlGenericControl ianchor = new HtmlGenericControl("a");
            ianchor.Attributes.Add("href", opcionMenu.UrlMenu);
            //ianchor.Attributes.Add("class", "active");
            ianchor.InnerText = opcionMenu.Nombre_Menu;
            li.Controls.Add(ianchor);
            return li;

        }

        private HtmlGenericControl MenuConNiveles(ENTMenusViva opcionMenu)
        {
            //Nombre del MenuPrincipal
            HtmlGenericControl li2 = new HtmlGenericControl("li");
            li2.Attributes.Add("class", "dropdown");

            //Descripcion que se muestra en el menuprincipal
            HtmlGenericControl ianchor = new HtmlGenericControl("a");
            ianchor.Attributes.Add("href", "#");
            ianchor.Attributes.Add("tabindex", "0");
            ianchor.Attributes.Add("class", "dropdown-toggle needsclick");
            ianchor.Attributes.Add("data-toggle", "dropdown");
            ianchor.Attributes.Add("role", "button");
            ianchor.Attributes.Add("aria-haspopup", "true");
            ianchor.Attributes.Add("aria-expanded", "false");

            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes.Add("class", "title");
            span.InnerText = opcionMenu.Nombre_Menu;
            HtmlGenericControl span2 = new HtmlGenericControl("span");
            span2.Attributes.Add("class", "caret");
            ianchor.Controls.Add(span);
            ianchor.Controls.Add(span2);
            li2.Controls.Add(ianchor);

            //SubMenus
            HtmlGenericControl luSubMenu = new HtmlGenericControl("ul");
            luSubMenu.Attributes.Add("class", "dropdown-menu");
            foreach (ENTMenusViva subMenu in opcionMenu.ListaSubMenus)
            {
                HtmlGenericControl liDestino = new HtmlGenericControl("li");
                HtmlGenericControl ianchorDest = new HtmlGenericControl("a");

                ianchorDest.Attributes.Add("href", subMenu.UrlMenu);

                HtmlGenericControl divSubItems = new HtmlGenericControl("div");
                divSubItems.Attributes.Add("class", "submenuitem");

                HtmlGenericControl spansub1 = new HtmlGenericControl("span");
                spansub1.Attributes.Add("class", "icon hidden-sm hidden-xs");
                spansub1.Attributes.Add("style", subMenu.UrlImagen);

                HtmlGenericControl spansub2 = new HtmlGenericControl("span");
                spansub2.Attributes.Add("class", "title");
                spansub2.InnerText = subMenu.Nombre_Menu;


                HtmlGenericControl spansub3 = new HtmlGenericControl("span");
                spansub3.Attributes.Add("class", "text");
                spansub3.InnerText = subMenu.Descripcion;

                divSubItems.Controls.Add(spansub1);
                divSubItems.Controls.Add(spansub2);
                divSubItems.Controls.Add(spansub3);

                ianchorDest.Controls.Add(divSubItems);
                liDestino.Controls.Add(ianchorDest);
                luSubMenu.Controls.Add(liDestino);

            }
            li2.Controls.Add(luSubMenu);
            return li2;

        }

    }

}
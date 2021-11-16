using Facturacion.BLL.Portal.Seguridad;
using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Facturacion33.PortalAdmin
{
    public partial class Login : System.Web.UI.Page
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
            //if (!IsPostBack)
            //{
            //    Session.Abandon();
            //    FormsAuthentication.SignOut();
            //}
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = txtUserName.Value;
                string password = txtPassword.Value;

                //Se recupera la informacion del usuario registrado
                BLLSeguridad bllSeguridad = new BLLSeguridad();
                ENTSeguridad entSeguridad = new ENTSeguridad();
                entSeguridad = bllSeguridad.ValidarUsuario(usuario, password);

                //Se verifica si el usuario es valido
                if (entSeguridad.EsValido == true)
                {
                    
                    Usuario = entSeguridad;
                    FormsAuthentication.RedirectFromLoginPage
                       (usuario, false);
                }
                else
                {
                    //En caso de que el usuario no sea valido se define el mensaje de error
                    lblMensaje.Text = entSeguridad.Error;
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al momento de validar usuario...";
            }

        }
    }
}
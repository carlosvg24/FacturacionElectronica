using Facturacion.BLL;
using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FacturacionOnLine.AdminUsuariosPortal
{
    public partial class LoginMobile : System.Web.UI.Page
    {
        protected static string ReCaptcha_Key = "6Lcd3zIUAAAAAIf53dpsBtDYLzxKZtdOerXljq8p";
        protected static string ReCaptcha_Secret = "6Lcd3zIUAAAAAGxrXc1p7hkpVizTdci48K0Ex4K2";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            BLLClientesportalCat usuarios = new BLLClientesportalCat();
            List<ENTClientesportalCat> listUsuarios = new List<ENTClientesportalCat>();
            ENTClientesportalCat usuario = new ENTClientesportalCat();
            Comun.Security.Encrypt enc = new Comun.Security.Encrypt();
            string email = String.Empty;
            string pass = String.Empty;

            try
            {
                email = txtUsuario.Text;
                usuarios.IniciarPropiedades();
                usuarios.Email = email;
                listUsuarios = usuarios.RecuperarClientesportalCatEmail(email);

                if (listUsuarios.Count > 0)
                {
                    usuario = listUsuarios.FirstOrDefault();
                    pass = enc.DecryptKey(usuario.Contrasenia);
                    if (pass.Equals(txtContrasenia.Text))
                    {
                        //txt_error.InnerText = "Usuario logueó";
                        Session["Login"] = email;
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), Guid.NewGuid().ToString(), "window.parent.location.href = '../default.aspx';", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= txtCaptcha]').val(''); renderRecaptcha();", true);
                        txt_error.InnerText = "Contraseña no válida";
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= txtCaptcha]').val(''); renderRecaptcha();", true);
                    txt_error.InnerText = "Usuario no válido";
                }


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= txtCaptcha]').val(''); renderRecaptcha();", true);
                txt_error.InnerText = ex.Message;
            }
        }


    }
}
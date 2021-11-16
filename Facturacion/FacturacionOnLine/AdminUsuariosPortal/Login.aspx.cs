using Facturacion.BLL;
using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FacturacionOnLine
{
    public partial class Login : System.Web.UI.Page
    {
        protected static string ReCaptcha_Key = "6Lcd3zIUAAAAAIf53dpsBtDYLzxKZtdOerXljq8p";
        protected static string ReCaptcha_Secret = "6Lcd3zIUAAAAAGxrXc1p7hkpVizTdci48K0Ex4K2";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Session.Contents.RemoveAll();
            //Session.Abandon();
        }

        protected void btnIniciarSesion_Click(object sender, EventArgs e)
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
                        //txt_dialogo.InnerText = "Usuario logueó";
                        Session["Login"] = email;
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), Guid.NewGuid().ToString(), "window.parent.location.href = window.parent.location;", true);
                    } else
                    {
                        txt_dialogo.InnerText = "Contraseña no válida";
                    }
                }
                else
                {
                    txt_dialogo.InnerText = "Usuario no válido";
                }


            } catch(Exception ex)
            {
                //Response.Write(ex.Message);
                txt_dialogo.InnerText = ex.Message;
            }
            
        }
    }
}
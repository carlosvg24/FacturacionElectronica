using Facturacion.BLL;
using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FacturacionOnLine.Master
{
    public partial class VIVA : System.Web.UI.MasterPage
    {
        protected static string ReCaptcha_Key = "6Lcd3zIUAAAAAIf53dpsBtDYLzxKZtdOerXljq8p";
        protected static string ReCaptcha_Secret = "6Lcd3zIUAAAAAGxrXc1p7hkpVizTdci48K0Ex4K2";

        protected void Page_Load(object sender, EventArgs e)
        {
            String email = String.Empty;
 
            if (Session.Count > 0)
            {
                if (Session["Login"] != null && Session["Login"].ToString().Length > 0)
                {
                    email = Session["Login"].ToString();
                    //aBienvenido.InnerText = "Bienvenido: " + email;
                    ulLogin.Visible = false;
                    ulUserProfile.Visible = true;
                    btnCerrarSesion.Visible = true;
                    lbIniSes.Visible = false;
                    lbRegistrarse.Visible = false;
                }
                else
                {
                    btnCerrarSesion.Visible = false;
                    lbIniSes.Visible = true;
                    lbRegistrarse.Visible = true;
                }
            } else
            {
                btnCerrarSesion.Visible = false;
                lbIniSes.Visible = true;
                lbRegistrarse.Visible = true;
            }
                

        }

        private void CerrarSesion()
        {
            Session["Login"] = String.Empty;
            ulLogin.Visible = true;
            ulUserProfile.Visible = false;
            btnCerrarSesion.Visible = false;
            lbIniSes.Visible = true;
            lbRegistrarse.Visible = true;

            ScriptManager.RegisterClientScriptBlock(Page, GetType(), Guid.NewGuid().ToString(), "window.parent.location.href = window.parent.location;", true);
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
                        msjError.InnerText = "";
                        ScriptManager.RegisterClientScriptBlock(Page, GetType(), Guid.NewGuid().ToString(), "$('#myModal').modal('hide'); window.parent.location.href = window.parent.location;", true);
                    }
                    else
                    {
                        msjError.InnerText = "Contraseña no válida";
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= txtLoginCaptcha]').val(''); renderLoginRecaptcha(); console.log('con')", true);
                    }
                }
                else
                {
                    msjError.InnerText = "Usuario no válido";
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= txtLoginCaptcha]').val(''); renderLoginRecaptcha(); console.log('usr')", true);
                }

            
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
                msjError.InnerText = ex.Message;
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "$('[id *= txtLoginCaptcha]').val(''); renderLoginRecaptcha(); console.log('ex')", true);
            }
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            CerrarSesion();
        }

        protected void btnGuiaFacturacion_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.ResolveUrl("~/Contents/GuiasPDF/Guía%20para%20proceso%20de%20facturación.pdf"));
        }
        
    }
}
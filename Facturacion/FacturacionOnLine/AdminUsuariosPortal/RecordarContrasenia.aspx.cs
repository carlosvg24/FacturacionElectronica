using Facturacion.BLL;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FacturacionOnLine.AdminUsuariosPortal
{
    public partial class RecordarContrasenia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label mpLabel = (Label)Page.Master.FindControl("lblTitulo");

            if (mpLabel != null)
            {
                mpLabel.Text = "Recuperar Contraseña";
            }
        }

        #region "F U N C I O N E S"
        protected string Mensaje_Texto(string cadena)
        {
            return cadena.Replace("'", @"\'");
        }

        private void MostrarDialogo(String tipo, String mensaje, String hdn_value = null)
        {
            try
            {
                // tipo: informacion, pregunta, alerta, error
                // hdn_value: es opcional (se ocupa para definir la sucesion de funciones en el boton aceptar del mensaje)
                //-- ejemplo: mostrar_dialogo("informacion", "datos guardados correctamente")

                // guarda valor en el control hidden
                //hdn_confirmacion.Value = hdn_value;

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), Guid.NewGuid().ToString(), "Mostrar_Dialogo('" + tipo + "','" + Mensaje_Texto(mensaje) + "');", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        
        protected void btnRecuperar_Click(object sender, EventArgs e)
        {
            Comun.Security.Encrypt dec = new Comun.Security.Encrypt();
            BLLClientesportalCat clientes = new BLLClientesportalCat();
            List<ENTClientesportalCat> listUsuarios = new List<ENTClientesportalCat>();
            ENTClientesportalCat usuario = new ENTClientesportalCat();
            BLLFacturacion bllFac = new BLLFacturacion();
            String pass = String.Empty;
            String email = String.Empty;
            String result = String.Empty;

            try
            {
                email = txtEmail.Text;
                clientes.IniciarPropiedades();
                listUsuarios = clientes.RecuperarClientesportalCatEmail(email);

                if (listUsuarios.Count > 0)
                {
                    usuario = listUsuarios.FirstOrDefault();
                    pass = dec.DecryptKey(usuario.Contrasenia);


                    result = bllFac.EnviarCorreoRecordarPassword(email, pass);
                    if (result.ToLower().Equals("ok"))
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(), "$('#txt_dialogo').removeClass('validationMsj');", true);
                        MostrarDialogo("informacion", "Tu contraseña se a enviado<br><br>Revisa tu e-mail: <b>" + email + "</b>");
                    }
                }
                else
                {
                    txtErrores.InnerText = "Usuario no válido";
                }

            }
            catch (ExceptionViva ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(), "$('#txtErrores').addClass('validationMsj'); $get('txtErrores').innerHTML = '" + ex.Message + "';", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(), "$('#txtErrores').addClass('validationMsj'); $get('txtErrores').innerHTML = '" + ex.Message + "';", true);
            }
        }
    }
}
using Facturacion.BLL;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.ENT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FacturacionOnLine
{
    public partial class NuevoUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label mpLabel = (Label)Page.Master.FindControl("lblTitulo");

            if (mpLabel != null)
            {
                mpLabel.Text = "Crear usuario de acceso";
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


        #region  "E V E N T O S   P O S T - B A C K "
        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            bool datosValidos = true;
            string mensaje = String.Empty;

            try
            {
                if (!String.IsNullOrEmpty(txtEmailAlta.Text) && !String.IsNullOrEmpty(txtConfirmarEmailAlta.Text))
                {
                    object email1 = new StringBuilder(txtEmailAlta.Text).ToString();
                    object email2 = new StringBuilder(txtConfirmarEmailAlta.Text).ToString();

                    if (!email1.Equals(email2))
                    {
                        mensaje = "El e-mail de confirmación no coincide";
                        datosValidos = false;
                    }
                }
                else
                {
                    mensaje = "El e-mail de confirmación no coincide";
                    datosValidos = false;
                }

                if (!String.IsNullOrEmpty(txtBxContrasenia.Text) && !String.IsNullOrEmpty(txtBxConfirmarContrasenia.Text))
                {
                    object pass1 = new StringBuilder(txtBxContrasenia.Text).ToString();
                    object pass2 = new StringBuilder(txtBxConfirmarContrasenia.Text).ToString();

                    if (!pass1.Equals(pass2))
                    {
                        mensaje += (mensaje.Length > 0 ? "<br>" : "") + "La contraseña no coincide";
                        datosValidos = false;
                    }
                }
                else
                {
                    mensaje += (mensaje.Length > 0 ? "<br>" : "") + "La contraseña no coincide";
                    datosValidos = false;
                }

                if (mensaje.Length > 0)
                    ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(),
                        "$('#txt_dialogo').addClass('validationMsj'); $get('txt_dialogo').innerHTML = '" + mensaje + "';", true);

                if (datosValidos)
                {
                    Comun.Security.Encrypt enc = new Comun.Security.Encrypt();
                    Guid codigoVerificacion = Guid.NewGuid();
                    String email = txtConfirmarEmailAlta.Text;


                    BLLClientesportalCat clientes = new BLLClientesportalCat();
                    BLLClientestipoportalCat tipoCliente = new BLLClientestipoportalCat();
                    List<ENTClientestipoportalCat> listaTiposCliente = new List<ENTClientestipoportalCat>();

                    clientes.UsuarioActivo = true;
                    clientes.UsuarioVerificado = false;
                    clientes.CodigoVerificacion = codigoVerificacion;
                    clientes.Contrasenia = enc.EncryptKey(txtBxConfirmarContrasenia.Text);
                    clientes.Email = email;
                    clientes.Nombre = String.Empty;
                    clientes.Pais = "MXN";
                    clientes.RFC = String.Empty;
                    clientes.TAXID = String.Empty;
                    clientes.UsoCFDI = String.Empty;
                    listaTiposCliente = tipoCliente.RecuperarClientestipoportalCatNombre(Comun.Utils.Tipo.ClientePortal.Cliente);
                    clientes.ClienteTipoId = listaTiposCliente.FirstOrDefault().Id;
                    clientes.Agregar();

                    BLLFacturacion bllFac = new BLLFacturacion();
                    bllFac.EnviarCorreoConfirmaAltaUsuario(email, codigoVerificacion);

                    ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(), "$('#txt_dialogo').removeClass('validationMsj');", true);
                    MostrarDialogo("informacion", "Usuario Creado exitosamente<br><br>Revisa tu e-mail: <b>" + email + "</b> y sigue las instrucciones de activación de usuario");
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(),
                        "", true);
            }
            catch (ExceptionViva ex)
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(), "$('#txt_dialogo').addClass('validationMsj'); $get('txt_dialogo').innerHTML = '" + ex.Message + "';", true);
            }
            catch (Exception ex)
            {
                String mensajeError = String.Empty;
                if (ex.Message.ToLower().Contains("insert duplicate key"))
                    mensajeError = "Email existente";
                else
                    mensajeError = ex.Message;
                ScriptManager.RegisterClientScriptBlock(this.Page, Page.GetType(), Guid.NewGuid().ToString(), "$('#txt_dialogo').addClass('validationMsj'); $get('txt_dialogo').innerHTML = '" + mensajeError + "';", true);
                //string mensajeUsuario = MensajeErrorUsuario;
                //BllLogErrores.RegistrarError(PNR, mensajeUsuario, ex, "Portal", "AgregarClientePortal");
                //ShowFalla("", mensajeUsuario);
            }
        }

        protected void btnCancelarDialogo_Click(object sender, EventArgs e)
        {
            //Response.Redirect(prevPage);
        }
        #endregion
    }
}
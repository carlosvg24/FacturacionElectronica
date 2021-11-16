using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Facturacion.RESTServices.Controllers
{
    public class UsuariosPortalController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(Guid))]
        public HttpResponseMessage Get(Guid? id)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            bool seDaAlta = false;
            string paginaRedirect = String.Empty;

            try
            {
                result.StatusCode = HttpStatusCode.OK;

                // actualiza a UsuarioVerificado = 1
                BLL.BLLClientesportalCat usuario = new BLL.BLLClientesportalCat();
                usuario.CodigoVerificacion = Guid.Parse(id.ToString());
                seDaAlta = usuario.ActivarUsuario();

                Uri baseUri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, String.Empty));

                if (seDaAlta)
                {
                    paginaRedirect = "~/ConfirmarAlta.html";
                }
                else
                {
                    paginaRedirect = "~/ErrorConfirmacion.html";
                }
                
                Uri resourceFullPath = new Uri(baseUri, VirtualPathUtility.ToAbsolute(paginaRedirect));
                var response = Request.CreateResponse(HttpStatusCode.Moved);
                response.Headers.Location = resourceFullPath; //new Uri("http://www.google.com.mx");
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}

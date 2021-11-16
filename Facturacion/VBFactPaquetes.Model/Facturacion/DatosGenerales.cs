using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model.Facturacion
{
    public class DatosGenerales
    {
        public String CarpetaArchivosCFDI { get; set; }
        public String CarpetaXML { get; set; }
        public String CarpetaPDF { get; set; }
        public String CarpetaNoProcesados { get; set; }
        public String HostSMTP { get; set; }
        public String UsuarioSMTP { get; set; }
        public String ContraseñaSMTP { get; set; }
        public Int32 PuertoSMTP { get; set; }
        public String CategoriaSMTP { get; set; }
        public String MailEmpresaSMTP { get; set; }
        public String NombreResponsableSMTP { get; set; }
        public String MailSoporteSMTP { get; set; }
        public String ApikeyPAC { get; set; }
        public String VersionCFDI { get; set; }
    }
}

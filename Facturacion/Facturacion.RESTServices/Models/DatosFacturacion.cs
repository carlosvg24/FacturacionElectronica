using Facturacion.ENT.ProcesoFacturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REST.Services.Models
{
    public class DatosParaFacturar: ENTDatosFacturacion
    {
        public String Password { get; set; }
      
    }
}
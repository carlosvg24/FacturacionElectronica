using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model.Facturacion
{
    public class Impuestos
    {
        public Int64 IdFactPagosImpuestos { get; set; }
        public Int64 IdFactPagosConceptos { get; set; }
        public String DescripcionTipoImpuesto { get; set; }
        public String CodigoImpuesto { get; set; }
        public String DescripcionTipoFactor { get; set; }
        public Decimal TasaOCuota { get; set; }
        public Decimal Importe { get; set; }
    }
}

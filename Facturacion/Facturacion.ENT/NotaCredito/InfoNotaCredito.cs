using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.NotaCredito
{
    public class InfoNotaCredito
    {
        public Comprobante Comprobante { get; set; }

        public List<Traslado> Traslados { get; set; }

        public List<Concepto> Conceptos { get; set; }

        public List<ENTFacturasDet> FacturaDet { get; set; }
    }
}

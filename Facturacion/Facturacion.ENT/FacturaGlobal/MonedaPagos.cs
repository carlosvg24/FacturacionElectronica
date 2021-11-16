using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.FacturaGlobal
{
    public class MonedaPagos
    {
        /// <summary>
        /// Currency Code
        /// </summary>
        public string Moneda { get; set; }


        /// <summary>
        /// Cantidad de pagos con dicha moneda
        /// </summary>
        public int Pagos { get; set; }
    }
}

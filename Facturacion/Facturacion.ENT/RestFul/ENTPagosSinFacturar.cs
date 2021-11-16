using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT
{
    public class ENTPagosSinFacturar
    {

        public long IdPagosCab { get; set; }

        /// <summary>
        /// Identificador de la reservacion en Navitaire
        /// </summary>
        public long BookingID { get; set; }

        /// <summary>
        /// Identificador del Pago en Navitaire
        /// </summary>
        public long PaymentID { get; set; }

        public decimal MontoTotal { get; set; }
        /// <summary>
		/// Folio de la prefactura, en caso de multipagos tendra el PaymentId del pago mas representativo
		/// </summary>
		public long FolioPrefactura { get; set; }
        public bool EnVigenciaParaFacturacion { get; set; }

        public bool EstaMarcadoParaFacturacion { get; set; }




    }
}

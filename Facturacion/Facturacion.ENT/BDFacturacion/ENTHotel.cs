using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT
{
    public class ENTHotel
    {
        /// <summary>
		/// Identificador de la reserva registrada en BD
		/// </summary>
		public int idReservaCab { get; set; }

        /// <summary>
        /// Identificador de la Reserva del Vuelo
        /// </summary>
        public string pnr { get; set; }

        /// <summary>
        /// Identificador del Paquete
        /// </summary>
        public string superPNR { get; set; }

        /// <summary>
        /// Identificador de la reservacion Hotel
        /// </summary>
        public string referenceID { get; set; }

        /// <summary>
        /// Identifica el Tipo de producto del paquete
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Identificador de la moneda del Paquete
        /// </summary>
        public string currencyCode { get; set; }

        /// <summary>
        /// Indica el monto del Paquete
        /// </summary>
        public double chargeAmount { get; set; }

        /// <summary>
        /// Indica el monto del Paquete
        /// </summary>
        /// 
        public DateTime createdDate { get; set; }
        public string tipoProceso { get; set; }

        #region Nuevas Propiedades para Terceros
        /// <summary>
        /// RFC del proveedor de Hotel
        /// </summary>
        public string RFC { get; set; }
        /// <summary>
        /// Razon Social del proveedor de Hotel
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Calle - Domicilio fiscal del proveedor de Hotel
        /// </summary>
        public string Calle { get; set; }
        /// <summary>
        /// No, - Domicilio fiscal del proveedor de Hotel
        /// </summary>
        public string NumExt { get; set; }
        /// <summary>
        /// Municipio - Domicilio fiscal del proveedor de Hotel
        /// </summary>
        public string Municipio { get; set; }
        /// <summary>
        /// Estado - Domicilio fiscal del proveedor de Hotel
        /// </summary>
        public string Estado { get; set; }
        /// <summary>
        /// País - Domicilio fiscal del proveedor de Hotel
        /// </summary>
        public string Pais { get; set; }
        /// <summary>
        /// CP - Domicilio fiscal del proveedor de Hotel
        /// </summary>
        public string CodigoPostal { get; set; }
        /// <summary>
        /// Folio que se asignara exclusivamente a la FG de reservas con paquete
        /// </summary>
        public int FolioPG { get; set; }
        #endregion
    }
}

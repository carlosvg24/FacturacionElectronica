using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT
{
    public class ENTPagosFacturadosREST
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
        /// Sello Digital del SAT
        /// </summary>
        public DateTime FechaTimbrado { get; set; }       
        /// <summary>
        /// Serie utilizada para la factura
        /// </summary>
        public string Serie { get; set; }        
        /// <summary>
        /// Folio de la factura, para Factura al cliente seria el PaymentId, para la Global seria uno de los foliosFiscales
        /// </summary>
        public long FolioFactura { get; set; }

        /// <summary>
        /// Identificador SAT de la factura generada
        /// </summary>
        public string UUID { get; set; }
        /// <summary>
		/// CFDI emitido por Pegaso despues de timbrar la factura
		/// </summary>
		public string CFDI { get; set; }

        public string CadenaOriginal { get; set; }
        /// <summary>
        /// Identificador de la peticion realizada a Pegaso
        /// </summary>
        public long IdPeticionPAC { get; set; }

        public string RutaCFDI { get; set; }
        public string RutaPDF { get; set; }
        public string Mensaje { get; set; }

        /// <summary>
		/// Atributo requerido para identificar la clave de la moneda utilizada para expresar los montos, cuando se usa moneda nacional se registra MXN. Catalogo SAT(c_Moneda)
		/// </summary>
		public string Moneda { get; set; }

        /// <summary>
        /// Atributo condicional para representar el tipo de cambio conforme con la moneda usada. Es requerido cuando la clave de moneda es distinta de MXN y de XXX. El valor debe reflejar el número de pesos mexicanos que equivalen a una unidad de la divisa señalada en el atributo moneda. Si el valor está fuera del porcentaje aplicable a la moneda tomado del catálogo c_Moneda, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion.
        /// </summary>
        public decimal TipoCambio { get; set; }

        /// <summary>
        /// Requerido para representar la suma de los importes de los conceptos antes de descuentos e impuesto. No se permiten valores negativos.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Condicional para representar el importe total de los descuentos aplicables antes de impuestos. No se permiten valores negativos. Se debe registrar cuando existan conceptos con descuento
        /// </summary>
        public decimal Descuento { get; set; }

        /// <summary>
        /// Monto total facturado por concepto de IVA
        /// </summary>
        public decimal MontoIVA { get; set; }

        /// <summary>
        /// Atributo requerido para representar la suma del subtotal, menos los descuentos aplicables, más las contribuciones recibidas (impuestos trasladados - federales o locales, derechos, productos, aprovechamientos, aportaciones de seguridad social, contribuciones de mejoras) menos los impuestos retenidos. Si el valor es superior al límite que establezca el SAT en la Resolución Miscelánea Fiscal vigente, el emisor debe obtener del PAC que vaya a timbrar el CFDI, de manera no automática, una clave de confirmación para ratificar que el valor es correcto e integrar dicha clave en el atributo Confirmacion. No se permiten valores negativos.
        /// </summary>
        public decimal Total { get; set; }

    }
}

using MetodosComunes.ClasesAbstractas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Facturacion.ENT.FacturaGlobal
{
    public class Comprobante:XmlElementTag
    {
        /// <summary>
        /// Este dato lo integra el sistema que utiliza el contribuyente para la emisión del comprobante fiscal.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Es el número de serie que utiliza el contribuyente para control interno de su información.Este campo acepta de 1 hasta 25 caracteres alfanuméricos.
        /// </summary>
        public string Serie { get; set; }

        /// <summary>
        /// Es el folio de control interno que asigna el contribuyente al comprobante, puede conformarse desde 1 hasta 40 caracteres alfanuméricos.
        /// </summary>
        public Int64 Folio { get; set; }

        /// <summary>
        /// Es la fecha y hora de expedición del comprobante fiscal. Se expresa en la forma AAAA-MM-DDThh:mm:ss y debe corresponder con la hora local donde se expide el comprobante.
        /// </summary>
        public string Fecha { get; set; }

        /// <summary>
        /// Se debe registrar la clave de forma de pago con la que se liquidó el  comprobante simplificado de mayor monto de entre los contenidos en el CFDI global
        /// </summary>
        public string FormaPago { get; set; }

        /// <summary>
        /// Es el número que identifica al certificado de sello digital del emisor, el cual lo incluye en el comprobante fiscal el sistema que utiliza el contribuyente para la emisión.
        /// </summary>
        public string NoCertificado { get; set; }

        /// <summary>
        /// Es la suma de los importes de los conceptos antes de descuentos e impuestos.No se permiten valores negativos.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Se debe registrar la clave de la moneda utilizada para expresar los montos conforme con la especificación ISO 4217 
        /// </summary>
        public string Moneda { get; set; }

        /// <summary>
        /// Se puede registrar el tipo de cambio conforme a la moneda registrada en el comprobante.
        /// </summary>
        public decimal TipoCambio { get; set; }

        /// <summary>
        /// Es la suma del subtotal, menos los descuentos aplicables, más las contribuciones recibidas(impuestos). No se permiten  valores negativos.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Se debe registrar la clave "I" de conformidad de catalogo
        /// </summary>
        public string TipoDeComprobante { get; set; }

        /// <summary>
        ///  Se debe registrar la clave "PUE" (pago una sola exhibicion) de conformidad con el catalogo
        /// </summary>
        public string MetodoPago { get; set; }

        /// <summary>
        /// Se debe registrar el código postal del lugar de expedición del comprobante(domicilio de la matriz o de la sucursal), debe corresponder con una clave de código postal incluida en el catálogo.
        /// </summary>
        public string LugarExpedicion { get; set; }

        /// <summary>
        /// Pedir confirmacion al SAT
        /// </summary>
        public string permitirConfirmacion { get; set; }

        /// <summary>
        /// Obtiene el XML dnde la clase s la etiqueta del elemente y las propiedades los atributos del elemento
        /// </summary>
        /// <returns></returns>
        public XElement GetXml()
        {
            return this.GetXmlElement(typeof(Comprobante),this);
        }
        
    }
}

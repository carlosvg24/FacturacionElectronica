using MetodosComunes.ClasesAbstractas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Facturacion.ENT.FacturaGlobal
{
    public class Concepto: XmlElementTag
    {
        /// <summary>
        /// Se debe registrar ClaveProdServ= 01010101 
        /// </summary>
        public string ClaveProdServ { get; set; }

        /// <summary>
        /// Número de folio o de operación de los comprobantes de operación con el público en general.
        /// </summary>
        public string NoIdentificacion { get; set; }

        /// <summary>
        /// Se debe registrar 1
        /// </summary>
        public int Cantidad { get; set; }

        /// <summary>
        /// Se debe registrar ClaveUnidad= ACT 
        /// </summary>
        public string ClaveUnidad { get; set; }

        /// <summary>
        /// Se debe registrar el valor "Venta"
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Subtotal del comprobante de operaciones con el público en general, el cual puede contener de cero hasta seis decimales.
        /// </summary>
        public decimal ValorUnitario { get; set; }

        /// <summary>
        /// Equivalente al resultado de multiplicar la Cantidad por el ValorUnitario expresado en el concepto, el cual será calculado por el sistema que genera el comprobante y considerará los redondeos que tenga registrado este campo. No se permiten valores negativos.
        /// </summary>
        public decimal Importe { get; set; }

        /// <summary>
        /// Obtiene el XML dnde la clase s la etiqueta del elemente y las propiedades los atributos del elemento
        /// </summary>
        /// <returns></returns>
        public XElement GetXml()
        {
            return this.GetXmlElement(typeof(Concepto), this);
        }

    }
}

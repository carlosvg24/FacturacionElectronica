using MetodosComunes.ClasesAbstractas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Facturacion.ENT.FacturaGlobal
{
    public class Impuestos:XmlElementTag
    {
        /// <summary>
        /// Es el total de los impuestos trasladados que se desprenden de los conceptos contenidos en el comprobante fiscal, el cual debe ser igual a la suma de los importes registrados en la sección Traslados, no se permiten valores negativos y es requerido cuando en los conceptos se registren impuestos trasladados.
        /// </summary>
        public string TotalImpuestosTrasladados { get; set; }

        /// <summary>
        /// Obtiene el XML dnde la clase s la etiqueta del elemente y las propiedades los atributos del elemento
        /// </summary>
        /// <returns></returns>
        public XElement GetXml()
        {
            return this.GetXmlElement(typeof(Comprobante), this);
        }
    }
}

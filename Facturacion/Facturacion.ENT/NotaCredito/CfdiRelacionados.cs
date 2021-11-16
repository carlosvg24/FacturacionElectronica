using MetodosComunes.ClasesAbstractas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Facturacion.ENT.NotaCredito
{
    /// <summary>
    /// En este nodo se puede expresar la información de los comprobantes fiscales relacionados.
    /// </summary>
    public class CfdiRelacionados: XmlElementTag
    {
        /// <summary>
        /// Se debe registrar la clave de la relación que existe entre éste comprobante que se está generando y el o los CFDI previos.
        /// </summary>
        public string TipoRelacion { get; set; }

        /// <summary>
        /// Obtiene el XML dnde la clase s la etiqueta del elemente y las propiedades los atributos del elemento
        /// </summary>
        /// <returns></returns>
        public XElement GetXml()
        {
            return this.GetXmlElement(typeof(CfdiRelacionados), this);
        }
    }
}

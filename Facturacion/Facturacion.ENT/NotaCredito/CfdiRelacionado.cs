using MetodosComunes.ClasesAbstractas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Facturacion.ENT.NotaCredito
{
    public class CfdiRelacionado : XmlElementTag
    {
        /// <summary>
        /// Se debe registrar el folio fiscal (UUID) de un comprobante fiscal relacionado con el presente comprobante.
        /// </summary>
        public string UUID { get; set; }

        /// <summary>
        /// Obtiene el XML dnde la clase s la etiqueta del elemente y las propiedades los atributos del elemento
        /// </summary>
        /// <returns></returns>
        public XElement GetXml()
        {
            return this.GetXmlElement(typeof(CfdiRelacionado), this);
        }
    }
}

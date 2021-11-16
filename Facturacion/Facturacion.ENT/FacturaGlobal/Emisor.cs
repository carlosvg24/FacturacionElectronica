using MetodosComunes.ClasesAbstractas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Facturacion.ENT.FacturaGlobal
{
    public class Emisor : XmlElementTag
    {
        /// <summary>
        /// Clave en el Registro Federal de Contribuyentes del emisor del comprobante.
        /// </summary>
        public string Rfc { get; set; }

        /// <summary>
        ///  N, denominación o razón social del emisor del comprobante.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Régimen fiscal del contribuyente emisor bajo el cual se está emitiendo el comprobante.
        /// </summary>
        public string RegimenFiscal { get; set; }

        /// <summary>
        /// Obtiene el XML dnde la clase s la etiqueta del elemente y las propiedades los atributos del elemento
        /// </summary>
        /// <returns></returns>
        public XElement GetXml()
        {
            return this.GetXmlElement(typeof(Emisor), this);
        }
    }
}

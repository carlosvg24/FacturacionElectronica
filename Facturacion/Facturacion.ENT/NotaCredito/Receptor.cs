using MetodosComunes.ClasesAbstractas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Facturacion.ENT.NotaCredito
{
    public class Receptor : XmlElementTag
    {
        /// <summary>
        /// Clave en el Registro Federal de Contribuyentes del emisor del comprobante.
        /// </summary>
        public string Rfc { get; set; }

        /// <summary>
        ///Se debe registrar XAXX010101000  
        /// </summary>
        public string UsoCFDI { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string emailReceptor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string codigoReceptor { get; set; }

        /// <summary>
        /// Obtiene el XML dnde la clase s la etiqueta del elemente y las propiedades los atributos del elemento
        /// </summary>
        /// <returns></returns>
        public XElement GetXml()
        {
            return this.GetXmlElement(typeof(Receptor), this);
        }
    }
}

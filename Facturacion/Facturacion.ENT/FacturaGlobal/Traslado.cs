using MetodosComunes.ClasesAbstractas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Facturacion.ENT.FacturaGlobal
{
    public class Traslado:XmlElementTag
    {
        /// <summary>
        /// Se debe registrar el valor para el cálculo del impuesto que se traslada, puede contener de cero hasta seis decimales.
        /// </summary>
        public string Base { get; set; }

        /// <summary>
        ///Clave del tipo de impuesto trasladado aplicable a cada comprobante de operaciones con el público en general, las cuales se encuentran incluidas en el catálogo c_Impuesto publicado en el Portal del SAT.
        /// </summary>
        public string Impuesto { get; set; }

        /// <summary>
        /// Se debe registrar el tipo de factor que se aplica a la base del impuesto, el cual se encuentra incluido en el catálogo c_TipoFactor publicado en el Portal del SAT.
        /// </summary>
        public string TipoFactor { get; set; }

        /// <summary>
        /// Se puede registrar el valor de la tasa o cuota del impuesto que se traslada para cada comprobante de operaciones con el público en general.Es requerido cuando el campo TipoFactor corresponda a Tasa o Cuota.
        /// </summary>
        public decimal TasaOCuota { get; set; }

        /// <summary>
        /// Se puede registrar el importe del impuesto trasladado que aplica a cada concepto.No se permiten valores negativos. Este campo es requerido cuando en el campo TipoFactor se haya registrado como Tasa o Cuota.
        /// </summary>
        public decimal Importe { get; set; }


        /// <summary>
        /// Obtiene el XML dnde la clase s la etiqueta del elemente y las propiedades los atributos del elemento
        /// </summary>
        /// <returns></returns>
        public XElement GetXml()
        {
            return this.GetXmlElement(typeof(Traslado), this);
        }
    }
}

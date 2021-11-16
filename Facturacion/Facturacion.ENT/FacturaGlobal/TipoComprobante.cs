﻿using MetodosComunes.ClasesAbstractas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Facturacion.ENT.FacturaGlobal
{
    public class TipoComprobante:XmlElementTag
    {
        /// <summary>
        /// 
        /// </summary>
        public string Clave { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Nombre { get; set; }

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

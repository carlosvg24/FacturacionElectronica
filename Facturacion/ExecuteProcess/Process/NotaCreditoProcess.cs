using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Facturacion.ENT.NotaCredito;
using Facturacion.BLL.NotaCredito;
using Facturacion.ENT;
using ExecuteProcess.ServiceEnt;
using ExecuteProcess.Interfaces;
using Facturacion.BLL.ProcesoFacturacion;

namespace ExecuteProcess.Process
{
    public class NotaCreditoProcess : IProcess
    {
        public bool OnDemand { get; set; }

        /// <summary>
        /// Bandera de activacion Modo debug
        /// </summary>
        public bool ModeDebug { get; set; }

        public Dictionary<string, Dictionary<string, string>> Parametros { get; set; }
        
        public ResponseTask ValidationsBeforeExecution()
        {
            return new ResponseTask() { Succes = true };
        }

        public ResponseTask MainProcess()
        {
            try
            {
                DateTime fecha = DateTime.Today.AddDays(
                    -int.Parse(
                        (OnDemand) ? (Parametros["OnDemand"]["CANTIDADDIASOBTENERPAGOSFACT"]) : (Parametros["Tarea"]["CANTDIAS_GETFACTURAS_DIFERENTEDIA"])
                        ));

                List<PagosFacturadosDiferenteDia> facturasDiferenteDia = BLLNotaCredito.GetFacturasDiferenteDia(fecha);

                ///Se obtiene un enumerable con solo IdfacturaCab Global
                var groupedGlobalIdFacturaCab = facturasDiferenteDia.OrderBy(x => x.GlobalIdFacturaCab)
                   .GroupBy(x => x.GlobalIdFacturaCab).ToList();

                foreach (var idFacturaCab in groupedGlobalIdFacturaCab)
                {
                    List<PagosFacturadosDiferenteDia> lote = facturasDiferenteDia.FindAll(f => f.GlobalIdFacturaCab == idFacturaCab.Key);                    

                    InfoNotaCredito info = BLLNotaCredito.GetInfoNotaCredito(idFacturaCab.Key, lote.Select(o => o.PaymentId).ToArray());

                    XElement xml = BuildXML(lote, idFacturaCab, info);
                    var responsePegaso = TimbrarFacturaGlobal(xml);

                    if (responsePegaso.Transaccion_Estatus.ToUpper() == "EXITO")
                    {
                        BLLNotaCredito.GuardarNotaCredito(responsePegaso, xml, this.Parametros, info, lote, idFacturaCab.Key);
                    }
                    else
                    {

                    }
                }                

            }
            catch(Exception ex)
            {

            }

            return new ResponseTask();
        }

        private XElement UpdateTagComprobante(Comprobante tagComprobante, decimal total,decimal subTotal)
        {
            //tagComprobante.Version = Parametros["Comprobante"]["VERSION"];
            tagComprobante.Serie = Parametros["Comprobante"]["SERIE"];
            tagComprobante.Folio = BLLNotaCredito.GetFolio();
            tagComprobante.Fecha = string.Format("{0:yyyy-MM-dd}T{0:hh:mm:ss}", DateTime.Now);
            //tagComprobante.FormaPago = pagoMax.IdFormaPago;
            //tagComprobante.NoCertificado = empresa.NoCertificado;
            tagComprobante.Total = total;
            tagComprobante.SubTotal = subTotal;
            //tagComprobante.Moneda = moneda;
            tagComprobante.TipoDeComprobante = Parametros["Comprobante"]["TIPOCOMPROBANTE"];
            //tagComprobante.MetodoPago = Parametros["Comprobante"]["METODOPAGO"];
            //tagComprobante.LugarExpedicion = Parametros["Comprobante"]["LUGAREXPEDICION"];
            tagComprobante.permitirConfirmacion = "false";
            //tagComprobante.TipoCambio = decimal.Parse((moneda == "MXN") ? ("1") : ("19"));

            return tagComprobante.GetXml();
        }


        #region Construccion XML

        private XElement BuildXML(List<PagosFacturadosDiferenteDia> facturasDiferenteDia, IGrouping<long, PagosFacturadosDiferenteDia> idFacturaCab, InfoNotaCredito info)
        {
            XElement tagMain = new XElement("RequestCFD", new XAttribute("version", Parametros["Comprobante"]["VERSION"]));            

            decimal sumConceptos = info.Conceptos.Sum(s => s.ValorUnitario), sumImporte = info.Traslados.Sum(s => s.Importe);

            XElement tagComprobante = UpdateTagComprobante(info.Comprobante, sumConceptos + sumImporte, sumConceptos);

            tagComprobante.SetAttributeValue("Version", Parametros["Comprobante"]["VERSION"]);

            tagComprobante.Add(
                GetCfdiRelacionados(facturasDiferenteDia.Find(f => f.GlobalIdFacturaCab == idFacturaCab.Key).GlobalUUID)
                );

            tagComprobante.Add(
          GetTagEmisor(BLLNotaCredito.GetAllEmpresas().Find(f => f.Rfc == Parametros["Emisor"]["RFC"]))
              );

            tagComprobante.Add(
                GetTagReceptor()
                );

            XElement tagConceptos = new XElement("Conceptos");

            foreach (Concepto concepto in info.Conceptos)
            {
                List<Traslado> trasladosByConcepto = info.Traslados.FindAll(f => f.IdFacturaDet == concepto.IdFacturaDet).ToList();

                concepto.ClaveProdServ = Parametros["Concepto"]["CLAVEPRODSERV"];
                //concepto.NoIdentificacion = pago.PaymentId.ToString();
                //concepto.Cantidad = int.Parse(Parametros["Concepto"]["CANTIDAD"]);
                //concepto.ClaveUnidad = Parametros["Concepto"]["CLAVEUNIDAD"];
                //concepto.Descripcion = Parametros["Concepto"]["DESCRIPCION"];
                concepto.ValorUnitario = info.FacturaDet.Find(f => f.IdFacturaDet == concepto.IdFacturaDet).Importe;//trasladosByConcepto.Sum(s=> Decimal.Parse(s.Base));
                concepto.Importe = (concepto.ValorUnitario * concepto.Cantidad);

                XElement tagConcepto = concepto.GetXml();
                XElement tagImpuestos = new XElement("Impuestos");

                XElement tagTraslados = new XElement("Traslados");
                tagTraslados.Add(trasladosByConcepto[0].GetXml());
                tagTraslados.Add(trasladosByConcepto[1].GetXml());

                tagImpuestos.Add(tagTraslados);

                tagConcepto.Add(tagImpuestos);

                tagConceptos.Add(tagConcepto);
            }

            tagComprobante.Add(tagConceptos);

            tagComprobante.Add(GetTagImpuestos(tagConceptos));

            tagMain.Add(tagComprobante);

            tagMain.Add(GetTagTrasaccion(tagComprobante.Attribute("Folio").Value));

            tagMain.Add(GetTagTipoComprobante());

           tagMain.Add(GetTagSucursal());

            return tagMain;
        }

        private XElement GetTagEmisor(ENTEmpresaCat empresa)
        {
            Emisor tagEmisor = new Emisor();

            tagEmisor.Nombre = empresa.RazonSocial;
            tagEmisor.RegimenFiscal = empresa.IdRegimenFiscal;
            tagEmisor.Rfc = empresa.Rfc;

            return tagEmisor.GetXml();
        }

        private XElement GetTagReceptor()
        {
            Receptor tagReceptor = new Receptor();

            tagReceptor.Rfc = Parametros["Receptor"]["RFC"];
            tagReceptor.UsoCFDI = Parametros["Receptor"]["USOCFDI"];
            tagReceptor.emailReceptor = string.Empty;
            tagReceptor.codigoReceptor = string.Empty;

            return tagReceptor.GetXml();
        }

        private XElement GetTagImpuestos(XElement tagConceptos)
        {
            List<XElement> traslados = new List<XElement>();
            XElement impuestosTag = new XElement("Impuestos");
            XElement trasladosTag = new XElement("Traslados");

            foreach (XElement concepto in tagConceptos.Elements("Concepto").ToList())
            {
                traslados.AddRange(concepto.Element("Impuestos").Elements("Traslados").ToList());
            }

            var trasladosElementsList = traslados.Elements("Traslado").ToList();
            var traslado16 = trasladosElementsList.FindAll(f => float.Parse(f.Attribute("TasaOCuota").Value) == 0.16f).ToList();

            decimal importe16 = traslado16.Sum(s => decimal.Parse(s.Attribute("Importe").Value));
            //decimal base16 = traslado16.Sum(s => decimal.Parse(s.Attribute("Base").Value));

            Traslado traslado0Tag = new Traslado();
            traslado0Tag.Importe = 0.0M;
            traslado0Tag.Impuesto = Parametros["Traslado"]["IMPUESTO"];
            traslado0Tag.TasaOCuota = 0.0M;
            traslado0Tag.TipoFactor = Parametros["Traslado"]["TIPOFACTOR"];
            traslado0Tag.Base = string.Empty;

            Traslado traslado16Tag = new Traslado();
            traslado16Tag.Importe = importe16;
            traslado16Tag.Impuesto = Parametros["Traslado"]["IMPUESTO"];
            traslado16Tag.TasaOCuota = Decimal.Parse(Parametros["Traslado"]["IVA"]);
            traslado16Tag.TipoFactor = Parametros["Traslado"]["TIPOFACTOR"];
            traslado16Tag.Base = string.Empty;

            trasladosTag.Add(traslado0Tag.GetXml());
            trasladosTag.Add(traslado16Tag.GetXml());

            impuestosTag.Add(trasladosTag);
            impuestosTag.Add(new XAttribute("TotalImpuestosTrasladados", importe16));

            return impuestosTag;
        }

        private XElement GetTagTipoComprobante()
        {
            XElement tipoComprobanteTag = new XElement("TipoComprobante");
            XAttribute clave = new XAttribute("clave", Parametros["TipoComprobante"]["CLAVE"]);
            XAttribute nombre = new XAttribute("nombre", Parametros["TipoComprobante"]["NOMBRE"]);

            tipoComprobanteTag.Add(clave, nombre);

            return tipoComprobanteTag;
        }

        private XElement GetTagSucursal()
        {
            XElement sucursalTag = new XElement("Sucursal");
            XAttribute nombre = new XAttribute("nombre", Parametros["Sucursal"]["NOMBRE"]);

            sucursalTag.Add(nombre);

            return sucursalTag;
        }

        private XElement GetTagTrasaccion(string folio)
        {
            string id = string.Empty;

            id = string.Format("ANA4000902{0:yyMMdd}CREDIT-{1}", DateTime.Now,folio.PadLeft(8,'0'));

            XElement transaccionTag = new XElement("Transaccion");
            transaccionTag.Add(new XAttribute("id", id));

            return transaccionTag;
        }

        private XElement GetCfdiRelacionados(string uuidFacturaGlobal)
        {
            XElement cfdiRelacionados = new XElement("CfdiRelacionados");
            cfdiRelacionados.Add(new XAttribute("TipoRelacion",Parametros["CfdiRelacionados"]["TIPORELACION"]));

            XElement cfdiRelacionado = new XElement("CfdiRelacionado");
            cfdiRelacionado.Add(new XAttribute("UUID", uuidFacturaGlobal));

            cfdiRelacionados.Add(cfdiRelacionado);

            return cfdiRelacionados;
        }

        #endregion

        /// <summary>
        /// Genera la factura
        /// </summary>
        /// <param name="pagos">Pagos a facturar</param>
        /// <param name="moneda">moneda de a factura</param>
        /// <param name="fechaPagos">fecha de los pagos</param>
        /// <returns>IdPeticionPAC que regresa serv de pegaso</returns>
        private ENTXmlPegaso TimbrarFacturaGlobal(XElement xmlFacturaGloba)
        {
            BLLPegaso pegaso = new BLLPegaso();
            ENTXmlPegaso xmlPegaso = pegaso.EnviaTimbradoPegaso(xmlFacturaGloba.ToString(), long.Parse(xmlFacturaGloba.Element("Comprobante").Attribute("Folio").Value), "NG", "");
            //ENTXmlPegaso xmlPegaso = pegaso.EnviaTimbradoPegasoGlobal(nodo.ToString(), long.Parse(nodo.Element("Comprobante").Attribute("Folio").Value), "FG");            

            return xmlPegaso;
        }
    }
}

using ExecuteProcess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExecuteProcess.ServiceEnt;
using System.Xml.Linq;
using System.Xml;
using Facturacion.BLL.ProcesoFacturacion;
using Facturacion.ENT;
//using Facturacion.BLL.ComplementoPagos;
//using Facturacion.ENT.ComplementosPagos;

namespace ExecuteProcess.Process
{
    public class ComplementosPagos : IProcess
    {
        public bool ModeDebug  {get;set;}

        public bool OnDemand { get; set; }

        public Dictionary<string, Dictionary<string, string>> Parametros { get; set; }

        public ComplementosPagos()
        {

        }

        public event ShowPorcentProgress OnShowPorcentProgress;

        public ResponseTask ValidationsBeforeExecution()
        {
            ResponseTask response = new ResponseTask();
            response.Succes = true;

            return response;
        }
        public ResponseTask MainProcess()
        {
            ResponseTask response = new ResponseTask();

            try
            {
                System.Xml.XmlDocument xmltest = new System.Xml.XmlDocument();
                xmltest.Load(@"C:\Logs\Complemento1.xml");

                XElement nodoPrincipal = XElement.Load(new XmlNodeReader(xmltest));
                //FD=FacturaDiferida
                //
                BLLPegaso pegaso = new BLLPegaso();
                ENTXmlPegaso xmlPegaso = pegaso.EnviaTimbrado(nodoPrincipal.ToString(), /*BLLComplementoPagos.GetFolio()*/1, "PD", "BYVNWN", false);
                //PagoInfo pago = GetPago();
                //var t = GetXML(pago);

            }
            catch(Exception ex)
            {

            }

            response.Succes = true;

            return response;
        }

        //private PagoInfo GetPago()
        //{
        //    PagoInfo pago = new PagoInfo();
        //    pago.Fecha = DateTime.Now;
        //    pago.UUIDFacturaBase = "E785EEFA-7DAD-4A6B-AC0C-71F7D248B126";
        //    pago.ClienteRFC = "XAXX010101000";

        //    return pago;
        //}

        //private XElement GetXML(PagoInfo pago)
        //{
        //    ENTEmpresaCat empresa = BLLComplementoPagos.GetAllEmpresas().Find(f => f.Rfc == Parametros["EMISOR"]["RFC"]);

        //    XElement tagMain = new XElement("RequestCFD", new XAttribute("version", Parametros["COMPROBANTE"]["Version"]));

        //    XElement tagComprobante = GetTagComprobante(empresa, pago);

        //    tagMain.Add(tagComprobante);
        //    tagMain.Add(GetCfdiRelacionados(pago.UUIDFacturaBase));
        //    tagMain.Add(GetTagEmisor(empresa));
        //    tagMain.Add(GetTagReceptor(pago.ClienteRFC));
        //    tagMain.Add(GetTagConceptos());
        //    tagMain.Add(GetTagPagos());
        //    tagMain.Add(GetTagTrasaccion("1"));
        //    tagMain.Add(GetTagTipoComprobante());
        //    tagMain.Add(GetTagSucursal());


        //    return tagMain;
        //}


        //private XElement GetTagComprobante(ENTEmpresaCat empresa, PagoInfo pago)
        //{
        //    Comprobante tagComprobante = new Comprobante();

        //    DateTime fechaGlobal = new DateTime(pago.Fecha.Year, pago.Fecha.Month, pago.Fecha.Day).AddDays(1).AddSeconds(-1);

        //    tagComprobante.Version = Parametros["COMPROBANTE"]["Version"];
        //    tagComprobante.Serie = Parametros["COMPROBANTE"]["Serie"];
        //    tagComprobante.Folio = BLLComplementoPagos.GetFolio();                   ////Validar que la fecha del pago aun esta dentro de las 72 hrs de limite
        //    tagComprobante.Fecha = string.Format("{0:yyyy-MM-dd}T{0:HH:mm:ss}", (fechaGlobal.AddHours(72) > DateTime.Now) ? fechaGlobal : DateTime.Now);
        //    //tagComprobante.FormaPago = pagoMax.IdFormaPago;
        //    tagComprobante.NoCertificado = empresa.NoCertificado;
        //    tagComprobante.Total = 0;
        //    tagComprobante.SubTotal = 0;
        //    tagComprobante.Moneda = "XXX";
        //    tagComprobante.TipoDeComprobante = Parametros["COMPROBANTE"]["TipoComprobante"];
        //    //tagComprobante.MetodoPago = Parametros["COMPROBANTE"]["MetodoPago"];
        //    tagComprobante.LugarExpedicion = Parametros["COMPROBANTE"]["LugarExpedicion"];
        //    tagComprobante.permitirConfirmacion = /*"false"*/string.Empty;
        //    //tagComprobante.TipoCambio = (moneda == "MXN") ? (1) : (BLLFacturaGlobal.GetDolarEnPesos(this.FechaInicial));

        //    //decimal.Parse((moneda == "MXN") ? ("1") : ("19"));

        //    return tagComprobante.GetXml();
        //}

        //private XElement GetCfdiRelacionados(string uuidFacturaBase)
        //{
        //    XElement cfdiRelacionados = new XElement("CfdiRelacionados");
        //    cfdiRelacionados.Add(new XAttribute("TipoRelacion", Parametros["CFDIRELACIONADOS"]["TipoRelacion"]));

        //    XElement cfdiRelacionado = new XElement("CfdiRelacionado");
        //    cfdiRelacionado.Add(new XAttribute("UUID", uuidFacturaBase));

        //    cfdiRelacionados.Add(cfdiRelacionado);

        //    return cfdiRelacionados;
        //}

        //private XElement GetTagEmisor(ENTEmpresaCat empresa)
        //{
        //    Emisor tagEmisor = new Emisor();

        //    tagEmisor.Nombre = empresa.RazonSocial;
        //    tagEmisor.RegimenFiscal = empresa.IdRegimenFiscal;
        //    tagEmisor.Rfc = empresa.Rfc;

        //    return tagEmisor.GetXml();
        //}

        //private XElement GetTagReceptor(string rfcCliente)
        //{
        //    Receptor tagReceptor = new Receptor();

        //    tagReceptor.Rfc = rfcCliente;//Parametros["RECEPTOR"]["RFC"];
        //    tagReceptor.UsoCFDI = Parametros["RECEPTOR"]["UsoCFDI"];
        //    tagReceptor.emailReceptor = string.Empty;
        //    tagReceptor.codigoReceptor = string.Empty;

        //    return tagReceptor.GetXml();
        //}

        //private XElement GetTagConceptos()
        //{
        //    XElement tagConceptos = new XElement("Conceptos");            

        //    Concepto tagConcepto = new Concepto();

        //    tagConcepto.ClaveProdServ = Parametros["CONCEPTO"]["ClaveProdServ"];
        //    //tagConcepto.NoIdentificacion = pago.PaymentId.ToString();
        //    tagConcepto.Cantidad = int.Parse(Parametros["CONCEPTO"]["Cantidad"]);
        //    tagConcepto.ClaveUnidad = Parametros["CONCEPTO"]["ClaveUnidad"];
        //    tagConcepto.Descripcion = Parametros["CONCEPTO"]["Descripcion"];
        //    tagConcepto.ValorUnitario = 0;
        //    tagConcepto.Importe = 0;

        //    XElement xmlConcepto = tagConcepto.GetXml();
        //    tagConceptos.Add(xmlConcepto);

        //    return tagConceptos;
        //}

        //private XElement GetTagPagos()
        //{
        //    XElement pagosTag = new XElement("Pagos");
        //    XAttribute versionAtt= new XAttribute("Version", Parametros["PAGOS"]["Version"]);
        //    pagosTag.Add(versionAtt);            

        //    Pago pago = new Pago();
        //    pago.FechaPago = string.Format("{0:yyyy-MM-dd}T{0:HH:mm:ss}", DateTime.Now);
        //    pago.FormaDePagoP = "01";
        //    pago.MonedaP = "MXN";
        //    pago.Monto = 100.00M;
        //    pago.TipoCambioP = 1;

        //    XElement pagoTag = pago.GetXml();
        //    pagoTag.Add(GetDocumentoRelcionado());

        //    pagosTag.Add(pagoTag);

        //    return pagosTag;
        //}

        //private XElement GetDocumentoRelcionado()
        //{
        //    DoctoRelacionado doc = new DoctoRelacionado();
        //    doc.Folio = 1;
        //    doc.IdDocumento = "E785EEFA-7DAD-4A6B-AC0C-71F7D248B126";
        //    doc.ImpPagado = 100.00M;//No es obligatorio
        //    doc.ImpSaldoAnt = 2075.66M;//Saldo antes de la parcialidad a facturar
        //    doc.ImpSaldoInsoluto = 1975.66M;//Saldo que se debe ya con laparcialidad sumada
        //    doc.MetodoDePagoDR = "PPD";
        //    doc.MonedaDR = "MXN";
        //    doc.NumParcialidad = 1;
        //    doc.Serie = "1";
        //    doc.TipoCambioDR = 1;

        //    return doc.GetXml();
        //}

        //private XElement GetTagTrasaccion(string folio)
        //{
        //    string id = string.Empty;

        //    id = string.Format("ANA4000902{0:yyMMdd}Complemento-{1}", DateTime.Now, folio.PadLeft(8, '0'));

        //    XElement transaccionTag = new XElement("Transaccion");
        //    transaccionTag.Add(new XAttribute("id", id));

        //    return transaccionTag;
        //}

        //private XElement GetTagTipoComprobante()
        //{
        //    XElement tipoComprobanteTag = new XElement("TipoComprobante");
        //    XAttribute clave = new XAttribute("clave", Parametros["TIPOCOMPROBANTE"]["Clave"]);
        //    XAttribute nombre = new XAttribute("nombre", Parametros["TIPOCOMPROBANTE"]["Nombre"]);

        //    tipoComprobanteTag.Add(clave, nombre);

        //    return tipoComprobanteTag;
        //}

        //private XElement GetTagSucursal()
        //{
        //    XElement sucursalTag = new XElement("Sucursal");
        //    XAttribute nombre = new XAttribute("nombre", Parametros["SUCURSAL"]["Nombre"]);

        //    sucursalTag.Add(nombre);

        //    return sucursalTag;
        //}

    }
}

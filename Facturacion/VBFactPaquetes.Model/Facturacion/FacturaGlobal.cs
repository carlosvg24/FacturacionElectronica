using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model.Facturacion
{
    public class FacturaGlobal
    {
        public Int64 IdFactPaqGlobal { get; set; }
        public Decimal TotalFactura { get; set; }
        public Decimal TotalIva { get; set; }
        public Decimal SubTotal { get; set; }
        public Decimal TotalDescuento { get; set; }
        public String Folio { get; set; }
        public String Serie { get; set; }
        public String CodigoTC { get; set; }
        public String DescTC { get; set; }
        public String TipoCambio { get; set; }
        public String CodigoMoneda { get; set; }
        public String DescMoneda { get; set; }
        public String CodigoMP { get; set; }
        public String DescMP { get; set; }
        public String CodigoFP { get; set; }
        public String DescFP { get; set; }
        public String LugarExp { get; set; }
        public String RFCEmisor { get; set; }
        public String RazonSocialEmisor { get; set; }
        public String CodigoRF { get; set; }
        public String DescRF { get; set; }
        public String RFCReceptor { get; set; }
        public String CodigoUsoCFDI { get; set; }
        public String DescUsoCFDI { get; set; }
        public String FechaEmision { get; set; }

        /*DATOS DE LA FACTURA*/
        public String CertificadoComprobante { get; set; }
        public String SelloComprobante { get; set; }
        public String FechaTimbrado { get; set; }
        public String UUID { get; set; }
        public String NoCertificadoSAT { get; set; }
        public String SelloSAT { get; set; }
        public String NoCertificado { get; set; }
        public String XMLResponse { get; set; }
        public Byte[] ArchivoPDF { get; set; }

        /*SE AGREGA LA INFORMACION DE DATOS PARA EL TIMBRADO*/
        public Int64 IdFactPaqPagosFactura { get; set; }
        public String CodigoPostal { get; set; }
        public String RFCProveedorCertifica { get; set; }
        public String SelloDigital { get; set; }
        public String CadenaOriginal { get; set; }

        //public String XMLRequest { get; set; }
        public String RutaArchivosCFDI { get; set; }
        public String RutaArchivoXML { get; set; }
        public String RutaArchivoPDF { get; set; }

        public List<ConceptosDTO> LstConcepto { get; set; }
        //Impuestos
        public List<ImpuestosDTO> LstImpuestos { get; set; }
        public List<DatosGeneralesDTO> LstDatosGralDTO { get; set; }
    }

    public class ConceptosDTO
    {
        public Int64 IdConceptos { get; set; }
        public String BookingID { get; set; }
        public String PNR { get; set; }
        public Int64 IdFactPaqPagos { get; set; }
        public Int16 Cantidad { get; set; }
        public String ClaveUnidad { get; set; }
        public String Unidad { get; set; }
        public String NoIdentificacion { get; set; }
        public String DescProdSer { get; set; }
        public String DescripcionConcepto { get; set; }
        public Decimal PrecioUnitario { get; set; }
        public Decimal Descuento { get; set; }
        public Decimal ImporteBase { get; set; }
        public Decimal ImporteIva { get; set; }
        public String Impuesto { get; set; }
        public String DescImpuesto { get; set; }
        public String Factor { get; set; }
        public Decimal TasaOCuota { get; set; }
        public String DescTipoImpuesto { get; set; }
        public Int16 TipoConcepto { get; set; }
        /*DATOS DE LA FACTURA*/
        public String CertificadoComprobante { get; set; }
        public String SelloComprobante { get; set; }
        public String FechaTimbrado { get; set; }
        public String UUID { get; set; }
        public String NoCertificadoSAT { get; set; }
        public String SelloSAT { get; set; }
        public String NoCertificado { get; set; }
        public String XMLResponse { get; set; }
        public Byte[] ArchivoPDF { get; set; }
    }

    //Impuestos
    public class ImpuestosDTO
    {
        public Int64 IdFactCOIPagosImpuestos { get; set; }
        public Int64 IdFactCOIPagosConceptos { get; set; }
        public String DescripcionTipoImpuesto { get; set; }
        public String CodigoImpuesto { get; set; }
        public String DescripcionTipoFactor { get; set; }
        public String DescripcionImpuesto { get; set; }
        public Decimal TasaOCuota { get; set; }
        public Decimal Importe { get; set; }

    }

    public class DatosGeneralesDTO
    {
        public String CarpetaArchivosCFDI { get; set; }
        public String CarpetaXML { get; set; }
        public String CarpetaPDF { get; set; }
        public String CarpetaNoProcesados { get; set; }
        public String HostSMTP { get; set; }
        public String UsuarioSMTP { get; set; }
        public String ContraseñaSMTP { get; set; }
        public Int32 PuertoSMTP { get; set; }
        public String CategoriaSMTP { get; set; }
        public String MailEmpresaSMTP { get; set; }
        public String NombreResponsableSMTP { get; set; }
        public String MailSoporteSMTP { get; set; }
        public String ApikeyPAC { get; set; }
        public String VersionCFDI { get; set; }

    }
}

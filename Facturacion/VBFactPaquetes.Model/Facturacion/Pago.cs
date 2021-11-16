using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model.Facturacion
{
    public class Pago
    {
        public Int64 IdFactPaqReserva { get; set; }
        public Int32 IdFactPaqPagos { get; set; }
        
        public Int32 IdFactPaqNotaCredito { get; set; }

        public Int64 IdFactReserva { get; set; }
        public Int32 IdFactPagos { get; set; }
        public Int32 IdFactNotaCredito { get; set; }

        public Int64 IdFactPaqGlobal { get; set; }

        [Display(Name = "PNR")]
        [Required(ErrorMessage = "Debe de Ingresar un {0}")]

        public String PNR { get; set; }
        public List<String> PNRs { get; set; }
        public Decimal TotalPago { get; set; }
        public DateTime FechaPago { get; set; }
        public Boolean PagoFacturado { get; set; }
        public Boolean PNRFacturado { get; set; }
        public Boolean PNREncontrado { get; set; } = true;
        public Boolean PNRDuplicado { get; set; } = false;
        public Boolean PNRError { get; set; } = false;
        public Boolean PagoFacturar { get; set; } = false;

        List<Pago> lstPagos { get; set; }
        // public List<Conceptos> LstConcepto { get; set; }
        //public List<Conceptos> LstConcepto { get; set; }

        public Pago()
        {
            this.IdFactPaqReserva = 0;
            this.IdFactPaqPagos = 0;

            this.IdFactReserva = 0;
            this.IdFactPagos = 0;
            this.IdFactNotaCredito = 0;
            
            //this.IdPagos = 0;
            this.PNR = String.Empty;
            this.PNRs = new List<string>();
            this.TotalPago = 0;
            this.FechaPago = new DateTime();
            //this.BookingID = 0;
            this.PNREncontrado = false;
            this.PNRFacturado = false;
            this.PNRDuplicado = false;
            this.PagoFacturar = false;
            this.lstPagos = new List<Pago>();
        }


        public int TotalListaPagos { get; set; }
        public Int64 NoFolio { get; set; }
        public String Serie { get; set; }
        public Decimal SubTotalPago { get; set; }
        public Decimal DescuentoPago { get; set; }
        public String CodigoFP { get; set; }
        public String DescFP { get; set; }
        public String CodigoMP { get; set; }
        public String DescMP { get; set; }
        public String CodigoM { get; set; }
        public String DescM { get; set; }
        public String CodigoTC { get; set; }
        public String DescTC { get; set; }
        public Decimal TipoCambio { get; set; }
        public String LugarExpedicion { get; set; }
        ///
        public String FechaEmision { get; set; }
        public String CondicionesDePago { get; set; }

        //EMISOR
        public String RFCEmisor { get; set; }
        public Int64 CodigoRFEmisor { get; set; }
        public String DescRFEmisor { get; set; }
        public String RazonSocialEmisor { get; set; }

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
        public String XMLResponseTercero { get; set; }
        public Byte[] ArchivoPDFTercero { get; set; }

        /*SE AGREGA LA INFORMACION DE DATOS PARA EL TIMBRADO*/
        public Int64 IdFactPagosFactura { get; set; }
        public String CodigoPostal { get; set; }
        public String RFCProveedorCertifica { get; set; }
        public String SelloDigital { get; set; }
        public String CadenaOriginal { get; set; }

        //public String XMLRequest { get; set; }
        public String RutaArchivosCFDI { get; set; }
        public String RutaArchivoXML { get; set; }
        public String RutaArchivoPDF { get; set; }

        /*SE AGREGA INFORMACIÓN PARA FORMULARIO CUANDO SE PRESENTA UN ERROR*/
        public String ErrorPNR { get; set; }
        public String ErrorFechaCompra { get; set; }
        public Decimal ErrorImportePagado { get; set; }
        public String ErrorRFCCliente { get; set; }
        public String ErrorMailCliente { get; set; }
        public String ErrorComentarios { get; set; }

        //CONCEPTO
        public List<Conceptos> LstConcepto { get; set; }

        //Impuestos
        public List<Impuestos> LstImpuestos { get; set; }

        //CARGOS
        public List<Cargos> LstCargos { get; set; }

        //DatosYavas
        public Int64 bookingid { get; set; }
        public String bookingcode { get; set; }
        public String status { get; set; }
        public String servicecurrency { get; set; }
        public String bookingdate { get; set; }
        public String lastmodifieddate { get; set; }
        public Decimal totalpaid { get; set; }
        public String producttype { get; set; }
        public Int64 idbookline { get; set; }
        public String linedate { get; set; }
        public String externalreference { get; set; }
        public String producttypename { get; set; }
        public Decimal sellingprice { get; set; }
        public Decimal iva_sellingprice { get; set; }
        public String sellcurrency { get; set; }
        public Boolean PNRNoEncontrado { get; set; }

        public List<String> prodycctype { get; set; }

        /*Se crea la lista LstDatosMail*/
        public List<DatosGenerales> LstDatosGralDTO { get; set; }

        public List<DatosFiscales> LstDatosFiscales { get; set; }
        public List<PagosEgresos> LstPagosEgresos { get; set; }
        public List<PagosEgresosGlobal> LstPagosEgresosGlobal { get; set; }

        public class PagosEgresos
        {
            public String PNR { get; set; }
            public String RutaArchivo { get; set; }
            public String UUID { get; set; }
        }
        public class PagosEgresosGlobal
        {
            public String PNR { get; set; }
            public String RutaArchivo { get; set; }
            public String UUID { get; set; }
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
}

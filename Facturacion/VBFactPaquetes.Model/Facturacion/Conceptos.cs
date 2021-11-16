using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model.Facturacion
{
    public class Conceptos
    {
        //CONCEPTO
        public Int64 IdFactPaqReserva { get; set; }
        public Int32 IdFactPaqPagos { get; set; }
        
        public Int32 IdFactPaqPagosConceptos { get; set; }
        public String CodigoProdSer { get; set; }

        public String CodigoTC { get; set; }
        public String DescProdSer { get; set; }

        public Int32 Cantidad { get; set; }
        public String ClaveUnidad { get; set; }
        public String Unidad { get; set; }
        public Decimal PrecioUnitario { get; set; }
        public Decimal Importe { get; set; }
        public Decimal ImporteBase { get; set; }

        //TRASLADOS
        public Decimal ImporteTotal { get; set; }
        public String Impuesto { get; set; }
        public String DescImpuesto { get; set; }
        public String Factor { get; set; }
        public Decimal TasaOCuota { get; set; }
        public String DescTipoImp { get; set; }
        public String PNRBoleto { get; set; }

        public String XML { get; set; }
        public byte[] PDF { get; set; }
        public String RFCTercero { get; set; }
        public String RazonSocialTercero { get; set; }



        public int TipoConcepto { get; set; }
        public decimal BaseTraslado { get; set; }



        public Conceptos()
        {
            //
            this.TipoConcepto = 0;
            this.BaseTraslado = 0;

            //CONCEPTO
            this.IdFactPaqPagos = 0;
            this.IdFactPaqReserva = 0;
            this.CodigoProdSer = String.Empty;
            this.DescProdSer = String.Empty;
            this.Cantidad = 0;
            this.ClaveUnidad = String.Empty;
            this.Unidad = String.Empty;
            this.PrecioUnitario = 0;
            this.Importe = 0;
            this.ImporteBase = 0;
            //TRASLADOS
            this.ImporteTotal = 0;
            this.Impuesto = String.Empty;
            this.DescImpuesto = String.Empty;
            this.Factor = String.Empty;
            this.TasaOCuota = 0;
            this.DescTipoImp = String.Empty;
            this.PNRBoleto = String.Empty;
            this.XML = String.Empty;
            this.PDF = null;
            this.RFCTercero = String.Empty;
            this.RazonSocialTercero = String.Empty;
        }
    }
}

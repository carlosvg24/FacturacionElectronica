using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VBFactPaquetes.Model.PantallaFacturacion
{
    public class ReservaPorFacturar
    {

        public Int64 IdFactReserva { get; set; }

        //[Display(Name = "PNR")]
        [Required(ErrorMessage = "Debe de Ingresar un {0}")]
        public String PNR { get; set; }



        /*Objetos para llenar la tabla de pagos Facturados y Detalle*/
        public Int64 IdFactPaqPagos { get; set; }

        public String CodigoM { get; set; }



        public DateTime FechaFacturacion { get; set; }

        public String NoFolio { get; set; }

        public Decimal Total { get; set; }


        /*Objetos para llenar la tabla Detalle*/
        public String DescProdSer { get; set; }

        public String ClaveUnidad { get; set; }

        //Moneda

        public String Importe { get; set; }

        public List<PagoPorFacturar> listPagos { get; set; }

        public String Email { get; set; }

        public String EmailConfirmacion { get; set; }

        public String RFC { get; set; }

        public int IdUsoCFDI { get; set; }

        public String DescUsoCFDI { get; set; }

        public int? EsFacturado { get; set; }

        /*Drop Down List Descripción Uso CFDI Fijo*/
        public IEnumerable<System.Web.Mvc.SelectListItem> listDescUsoCFDI { get; set; }

        public bool SoyExtranjero { get; set; }

        public int IdPais { get; set;  }

        public String CodigoPais { get; set;  }

        /*Drop Down List Codigo Pais fijo*/
        public IEnumerable<System.Web.Mvc.SelectListItem> listCodigoPais { get; set; }

        public String TAXId { get; set; }

        public String AccionBoton { get; set; }

        //public String listBloqueos { get; set; } /*Esta es */


        public ReservaPorFacturar()
        {
            this.IdFactReserva = 0;
            this.PNR = String.Empty;
            this.listPagos = new List<PagoPorFacturar>(); ;


            /* Propiedades para llenar la tabla de pagos Facturados*/
            this.IdFactPaqPagos = 0;
            this.NoFolio = String.Empty;
            this.FechaFacturacion = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            this.CodigoM = String.Empty;
            this.Total = 0;
            this.EsFacturado = 0;


            /*Propiedades para llenar la tabla Detalle*/
            this.DescProdSer = String.Empty;
            this.ClaveUnidad = String.Empty;
            this.Importe = String.Empty;

            /*Propiedades para llenar los datos de facturación*/
            this.Email = String.Empty;
            this.EmailConfirmacion = String.Empty;
            this.RFC = String.Empty;
            this.IdUsoCFDI = 0;
            this.DescUsoCFDI = String.Empty;
            this.SoyExtranjero = false;
            this.IdPais = 0;
            this.CodigoPais = String.Empty;
           
            //this.AccionBoton = String.Empty;


            /*Drop Down List Descripción Uso CFDI Fijo*/
            this.listDescUsoCFDI = null;

            /*Drop Down List Codigo Pais fijo*/
            this.listCodigoPais = null; 
        }
    }
}

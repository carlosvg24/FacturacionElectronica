using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model.Facturacion
{
    public class DatosFiscales
    {
        public Int64 IdFactPagos { get; set; }
        public String PNR { get; set; }
        public Int64 BookingID { get; set; }
        public String Pais { get; set; }
        public String RFC { get; set; }
        // public String RFCExtranjero { get; set; }
        public String Email { get; set; }
        public String UsoCFDI { get; set; }
        public String CodigoUsoCFDI { get; set; }
        public String DescUsoCFDI { get; set; }

        public String RazonSocial { get; set; }
        public String DireccionFiscal { get; set; }
        public Int64 NoExterior { get; set; }
        public Int64 NoInterior { get; set; }
        public String Colonia { get; set; }
        public String Municipio { get; set; }
        public String Estado { get; set; }
        public Int64 CodigoPostal { get; set; }
        public Boolean MostrarDatos { get; set; }
        public int TotalListaPagos { get; set; }
        public String Telefono { get; set; }
        public String RFCExtranjero { get; set; }
        public String CFDIExtranjero { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model.PantallaFacturacion
{
    public class PagoPorFacturar
    {

        public Int64 IdFactReserva { get; set; }
        public Int64 IdFactPagos { get; set; }
        public Int64 IdFactNotaCredito { get; set; }

        public Int64 IdFactYavGlobal { get; set; }

        public bool SeMandaraFacturar { get; set; }

        public DateTime FechaPago { get; set; }
        public DateTime FechaFacturacion { get; set; }

        public String NoFolio { get; set; }
        public String Serie { get; set; }

        public Int32 IdMoneda { get; set; }

        public string Moneda { get; set; }

        public decimal Total { get; set; }

        public int? EsFacturado { get; set; }

        public String FormaPago { get; set; }
        public String LugarExpedicion { get; set; }

        public List<ItemPagoPorFacturar> listItems { get; set; }
    }
}

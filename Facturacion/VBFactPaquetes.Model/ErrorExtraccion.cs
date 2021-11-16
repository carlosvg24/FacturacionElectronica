using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model
{
    public class ErrorExtraccion
    {
        public String PNR { get; set; }
        public String BookingId { get; set; }
        public String PaymentId { get; set; }
        public String Lines { get; set; }
        public String MontoTotal { get; set; }
        public String TieneCancelacion { get; set; }
        public String FechaRegistroJun { get; set; }
        public String FechaExtraccion { get; set; }
        public String Error { get; set; }

        public ErrorExtraccion()
        {
            this.PNR = String.Empty;
            this.BookingId = String.Empty;
            this.PaymentId = String.Empty;
            this.Lines = String.Empty;
            this.MontoTotal = String.Empty;
            this.TieneCancelacion = String.Empty;
            this.FechaRegistroJun = String.Empty;
            this.FechaExtraccion = String.Empty;
            this.Error = String.Empty;
        }
    }
}

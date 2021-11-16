using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model
{

    /*
    "order": {
      "bookingId": "VKGF84T",
      "amount": 2000,
      "status": 2,
      "currencyCode": "MXN",
      "conversionFactor": 1,
      "createdUTC": "2021-02-22T10:32:45Z",
      "modifiedUTC": "2021-02-23T08:40:22Z"
    } 
    */
    public class Order
    {
        public string bookingId { get; set; }
        public double amount { get; set; }
        public int status { get; set; }
        public string currencyCode { get; set; }
        public double conversionFactor { get; set; }
        public DateTime createdUTC { get; set; }
        public DateTime modifiedUTC { get; set; }
        public List<string> productTypes { get; set; }

        //public Order()
        //{
        //    this.BookingID = String.Empty;
        //    this.Amount = 0;
        //    this.Status = 0;
        //    this.CurrencyCode = String.Empty;
        //    this.ConversionFactor = 0;
        //    this.CreatedUTC = new DateTime();
        //    this.ModifiedUTC = new DateTime();
        //}
    }
}

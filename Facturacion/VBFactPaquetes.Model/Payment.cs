using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model
{
    /*
     {
        "paymentId": 1235,
        "lastFourDigits": "1111",
        "holderName": "Nobody",
        "transactionId": "OK",
        "amount": 500,
        "currency": "MXN",
        "updated": "2021-02-25T13:40:32Z ",
        "state": 2
      } 
    */
    public class Payment
    {
        public string paymentId { get; set; }
        public string lastFourDigits { get; set; }
        public string holderName { get; set; }
        public string transactionId { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public DateTime updated { get; set; }
        public int state { get; set; }

        //public Payment()
        //{
        //    this.PaymentId = 0;
        //    this.LastFourDigits = String.Empty;
        //    this.HolderName = String.Empty;
        //    this.TransactionId = string.Empty;
        //    this.Amount = 0;
        //    this.Currency = String.Empty;
        //    this.Updated = new DateTime();
        //    this.State = 0;
        //}
    }
}

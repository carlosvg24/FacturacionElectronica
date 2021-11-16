using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model
{
    /*
    "productType": {
          "ProductTypeId": 2,
          "ProductType": "Hotel",
          "referenceId": "71-2083728"
        } 
    */
    public class ProductType
    {
        public int productTypeId { get; set; }
        public string productType { get; set; }
        public string referenceId { get; set; }

        //public ProductType()
        //{
        //    this.ProductTypeId = 0;
        //    this.ProductTypeName = String.Empty;
        //    this.ReferenceId = String.Empty;
        //}
    }
}

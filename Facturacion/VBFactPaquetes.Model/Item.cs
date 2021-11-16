using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model
{
    public class Item
    {
        /*
         {
        "itemId": "33eb74f3b9e06dc2a662f018980cc830",
        "status": 2,
        "productType": {
          "ProductTypeId": 2,
          "ProductType": "Hotel",
          "referenceId": "71-2083728"
        }
         "sellingPrice": 1200,
        "sellingCurrency": "MXN",
        "cost": 0,
        "costCurrency": "MXN",
        "costConversionFactor": 1,
        "tax": 300,
        "cityTaxes": null
         */

        public string itemId { get; set; }
        public int status { get; set; }
        public ProductType productType { get; set; }
        public double sellingPrice { get; set; }
        public string sellingCurrency { get; set; }
        public double cost { get; set; }
        public string costCurrency { get; set; }
        public double costConversionFactor { get; set; }
        public double tax { get; set; }
        public CityTaxes cityTaxes { get; set; }
        public Location location { get; set; }
        //public Item()
        //{
        //    this.ItemId = String.Empty;
        //    this.Status = 0;
        //    this.SellingPrice = 0;
        //    this.SellingCurrency = String.Empty;
        //    this.Cost = 0;
        //    this.CostCurrency = String.Empty;
        //    this.CostConversionFactor = 0;
        //    this.Tax = 0;
        //    this.CityTaxes = new CityTaxes();
        //    this.ProductType = new ProductType();
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.ENT.ProcesoFacturacion
{

    public class Number
    {
        public int length { get; set; }
        public bool luhn { get; set; }
    }

    public class Country
    {
        public string numeric { get; set; }
        public string alpha2 { get; set; }
        public string name { get; set; }
        public string emoji { get; set; }
        public string currency { get; set; }
        public int latitude { get; set; }
        public int longitude { get; set; }
    }

    public class Bank
    {
        public string name { get; set; }
        public string phone { get; set; }
    }


    public class ResponseBINRest
    {
        public Number number { get; set; }
        public string scheme { get; set; }
        public string type { get; set; }
        public string brand { get; set; }
        public Country country { get; set; }
        public Bank bank { get; set; }
    }

}

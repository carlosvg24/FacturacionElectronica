using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBFactPaquetes.Model
{
    public class Paquete
    {
        public Order order { get; set; }
        public List<Item> items { get; set; }
        public List<Payment> payments { get; set; }
    }
}

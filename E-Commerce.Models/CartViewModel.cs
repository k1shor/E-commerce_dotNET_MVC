using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class CartViewModel
    {
        public IEnumerable<ShoppingCart> shoppingCarts{ get; set; }
        //public double total { get; set; }
        public OrderHeader orderHeader { get; set; }
    }
}

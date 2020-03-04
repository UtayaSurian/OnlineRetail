using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRetail.Core.Models
{
    public class CartItem : BaseEntity
    {
        public string BasketId { get; set; }
        public string ProductId { get; set; }     //To dynamically change the price when manager change the actual product price
        public int Quantity { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRetail.Core.Models
{
    public class Cart : BaseEntity
    {   
        //lazy loading to load itens from basket model
        public virtual ICollection<CartItem> CartItems { get; set; }
        
        public Cart()
        {
            this.CartItems = new List<CartItem>();

        }
    }
}

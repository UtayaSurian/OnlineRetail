using OnlineRetail.Core.Contracts;
using OnlineRetail.Core.Models;
using OnlineRetail.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRetail.Services         
{                                                               //For order service  handling                  
    public class OrderService : IOrderService
    {
        IRepository<Order> orderContext;
        public OrderService(IRepository<Order> OrderContext)
        {
            this.orderContext = OrderContext;   
        }

        public void CreateOrder(Order baseOrder, List<CartItemViewModel> cartItems)
        {
            foreach(var item in cartItems)
            {
                //each cart items will be added into this underline baseOrder:
                baseOrder.OrderItems.Add(new OrderItem()
                {
                    ProductId = item.id,
                    Image = item.Image,
                    Price = item.Price,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                });
            }
            orderContext.Insert(baseOrder);
            orderContext.Commit();
        }
    }
}

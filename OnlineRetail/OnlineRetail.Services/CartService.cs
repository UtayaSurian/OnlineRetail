using OnlineRetail.Core.Contracts;
using OnlineRetail.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OnlineRetail.Services
{
    public class CartService
    {
        IRepository<Product> productContext;
        IRepository<Cart> cartContext;

        //Cookie 
        //Fixed
        public const string CartSessionName = "Mobile&WebMultimedia";

        public CartService(IRepository<Product> ProductContext, IRepository<Cart> CartContext)
        {
            this.productContext = ProductContext;
            this.cartContext = CartContext;
        }
        //Assining cookies in newly created cart
        private Cart CreateNewCart(HttpContextBase httpContext)
        {
            Cart cart = new Cart();
            cartContext.Insert(cart);
            cartContext.Commit();

            HttpCookie cookie = new HttpCookie(CartSessionName);
            cookie.Value = cart.Id;
            cookie.Expires = DateTime.Now.AddDays(2); //How many days for the cookie to be active
            httpContext.Response.Cookies.Add(cookie);

            return cart;
        }


        private Cart GetCart(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(CartSessionName);

            Cart cart = new Cart();
            if (cookie != null){
                string cartId = cookie.Value;
                if (!string.IsNullOrEmpty(cartId))  //If not empty
                {
                    cart = cartContext.Find(cartId);
                }
                else
                {
                    if (createIfNull)
                    {
                        cart = CreateNewCart(httpContext);
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    cart = CreateNewCart(httpContext);
                }
            }

            return cart;
        }

        // add items in backet
        public void AddToCart(HttpContextBase httpContext, string productId)
        {
            Cart cart = GetCart(httpContext, true); //load the cart from the database
            CartItem item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

            //If the item exist in the basket, do the following:
            if (item == null)
            {
                item = new CartItem()
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                };

              //  cart = CartItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;     //Add item to currect existing cart
            }

            cartContext.Commit();
        }
    }
}

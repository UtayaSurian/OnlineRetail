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
        public const string CartSessionName = "MobileWebMultimedia";

        public CartService(IRepository<Product> ProductContext, IRepository<Cart> CartContext)
        {
            this.productContext = ProductContext;
            this.cartContext = CartContext;
        }

        //Load the cart via cart id
        private Cart GetCart(HttpContextBase httpContext, bool createIfNull)
        {
            //Look for available cookie
            HttpCookie cookie = httpContext.Request.Cookies.Get(CartSessionName);

            Cart cart = new Cart();
            if(cookie != null)
            {
                string cartId = cookie.Value;
                if (!string.IsNullOrEmpty(cartId))
                {   //Load the cart
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

        private Cart CreateNewCart(HttpContextBase httpContext)
        {
            Cart cart = new Cart();
            cartContext.Insert(cart);
            cartContext.Commit();

            HttpCookie cookie = new HttpCookie(CartSessionName);
            cookie.Value = cart.Id;
            cookie.Expires = DateTime.Now.AddMonths(1); //Cookie expiration
            httpContext.Response.Cookies.Add(cookie);

            return cart;
        }

        public void AddToCart(HttpContextBase httpContext, string productId)
        {
            Cart cart = GetCart(httpContext, true);  //Load from the database
            CartItem item = cart.CartItems.FirstOrDefault(i=>i.ProductId == productId);


            if (item == null)
            {
                item = new CartItem()
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                cart.CartItems.Add(item);


            }
            else
            {   //Implement the property
                item.Quantity = item.Quantity + 1; //update the value 
            }

            cartContext.Commit();
        }

        public void RemoveFromCart(HttpContextBase httpContext, string itemId)
        {
            Cart cart = GetCart(httpContext, true);
            CartItem item = cart.CartItems.FirstOrDefault(i => i.Id == itemId);

            if(item != null)
            {
                cart.CartItems.Remove(item);
                cartContext.Commit();
            }

        }
    }
}

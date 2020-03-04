using OnlineRetail.Core.Contracts;
using OnlineRetail.Core.Models;
using OnlineRetail.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OnlineRetail.Services
{                                                                        //Cart database
    public class CartService : ICartService           
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
    
        public List<CartItemViewModel> GetCartItems(HttpContextBase httpContext)
        {   //Get the cart from the database
            Cart cart = GetCart(httpContext, false);    //return empty if not basket is available

            if (cart != null)
            {                   //retunr the data to cart
                var results = (from c in cart.CartItems
                              join p in productContext.Collection() on c.ProductId equals p.Id
                              select new CartItemViewModel()
                              {

                                  id = c.Id,
                                  Quantity = c.Quantity,
                                  ProductName = p.Name,
                                  Image = p.Image,
                                  Price = p.Price

                              }).ToList();

                return results;
            }
            else
            {
                return new List<CartItemViewModel>(); 
            }
        }


        public CartSummaryViewModel GetCartSummary(HttpContextBase httpContext)
        {
            Cart cart = GetCart(httpContext, false); //no need to create if it's empty
            CartSummaryViewModel model = new CartSummaryViewModel(0,0); //show zeros in home page
            if (cart != null)
            {   //count the total quantity from the cart table
                //? means return null if no ietms
                int? cartCount = (from item in cart.CartItems
                                  select item.Quantity).Sum();

                decimal? cartTotal = (from item in cart.CartItems
                                      join p in productContext.Collection() on item.ProductId equals p.Id
                                      select item.Quantity * p.Price).Sum();

                model.CartCount = cartCount ?? 0;       //retunr 0 if not value in cartCount
                model.CartTotal = cartTotal ?? decimal.Zero;


                return model;
            }
            else
            {
                return model;
            }
        }
    }
}

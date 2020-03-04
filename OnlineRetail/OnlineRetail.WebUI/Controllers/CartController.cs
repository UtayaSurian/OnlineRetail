using OnlineRetail.Core.Contracts;
using OnlineRetail.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineRetail.WebUI.Controllers
{
    public class CartController : Controller
    {
        IRepository<Customer> customers;
        ICartService cartService;
        IOrderService orderService;
        public CartController(ICartService CartService, IOrderService OrderService, IRepository<Customer> Customers)
        {
            this.cartService = CartService;
            this.orderService = OrderService;
            this.customers = Customers;
        }
        // GET: Cart
        public ActionResult Index()
        {
            var model = cartService.GetCartItems(this.HttpContext);

            return View(model);
        }

        public ActionResult AddToCart(string Id)
        {
            cartService.AddToCart(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(string Id)
        {
            cartService.RemoveFromCart(this.HttpContext, Id);

            return RedirectToAction("Index");
        }

        public PartialViewResult CartSummary()
        {
            var cartSummary = cartService.GetCartSummary(this.HttpContext);

            return PartialView(cartSummary);
        }
        
        [Authorize] //To force user to log in to checkout
        public ActionResult Checkout()
        {
            //To get/check the customer name based on logged in mail
            Customer customer = customers.Collection().FirstOrDefault(c => c.Email == User.Identity.Name);
            if (customer!=null){
                Order order = new Order()
                {
                    Email = customer.Email,
                    City = customer.City,
                    State = customer.State,
                    Street = customer.Street,
                    FirstName = customer.FirstName,
                    Surname = customer.LastName,
                    Country = customer.Country,

                };

                return View();
            }
            else
            {
                return RedirectToAction("Error! Please try again.");
            }
            
        }

        [HttpPost]
        [Authorize]
        public ActionResult Checkout(Order order)
        {
            var cartItems = cartService.GetCartItems(this.HttpContext);
            order.OrderStatus = "Order Created!";

            order.Email = User.Identity.Name;   //To avoid user from bypass

            //process for payment
            order.OrderStatus = "Payment Processed!";
            orderService.CreateOrder(order, cartItems);
            cartService.ClearCart(this.HttpContext);                    //After checkout

            return RedirectToAction("ThankYou", new { OrderId = order.Id });

        }

        //Thank you page
        public ActionResult ThankYou(string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }
    }
}
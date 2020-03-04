using OnlineRetail.Core.Contracts;
using OnlineRetail.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineRetail.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> context; //Object to call all methods delcared in InMemory
        IRepository<ProductCategory> productCategories; //To load all categories from the database for drop-down list
        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoriesContext)
        {
            //Initialize the object automatically
            context = productContext;
            productCategories = productCategoriesContext;
        }
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }
        public ActionResult Details(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
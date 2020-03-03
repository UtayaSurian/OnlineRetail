using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineRetail.Core.Models;
using OnlineRetail.DataAccess.InMemory;

namespace OnlineRetail.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        ProductRepository context; //Object to call all methods delcared in InMemory
        public ProductManagerController()
        {
            //Initialize the object automatically
            context = new ProductRepository();
        }
        // GET: ProductManager
        public ActionResult Index()
        {   //To extract from existing cache's list and display
            List<Product> products = context.Collection().ToList(); 
            return View(products);
        }

        //This method is just to display the page
        //To fill up product details
        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid) //Check for validation that in product model
            {
                return View(product);   //Return back the page if user failed to meet the validation
            }
            else
            {
                context.Insert(product);    //call the method from that declared in InMemory file
                context.Commit();       //Save

                return RedirectToAction("Index");
            }

        }

        //This method is to load the data from existing database
        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if(product == null)
            {
                return HttpNotFound();
            }
            else{
                return View(product);
            }
        }
        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToEdit = context.Find(Id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Image = product.Image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

                context.Commit();
                return RedirectToAction("Index"); //Redirect to product home page
                
            }
        }


        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete  == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}
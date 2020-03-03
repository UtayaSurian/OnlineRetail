using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineRetail.Core.Models;
using OnlineRetail.DataAccess.InMemory;

namespace OnlineRetail.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        InMemoryRepository<ProductCategory> context; //Object to call all methods delcared in InMemory
        public ProductCategoryManagerController()
        {
            //Initialize the object automatically
            context = new InMemoryRepository<ProductCategory>();
        }
        // GET: ProductManager
        public ActionResult Index()
        {   //To extract from existing cache's list and display
            List<ProductCategory> productCategories = context.Collection().ToList();
            return View(productCategories);
        }

        //This method is just to display the page
        //To fill up product details
        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid) //Check for validation that in product model
            {
                return View(productCategory);   //Return back the page if user failed to meet the validation
            }
            else
            {
                context.Insert(productCategory);    //call the method from that declared in InMemory file
                context.Commit();       //Save

                return RedirectToAction("Index");
            }

        }

        //This method is to load the data from existing database
        public ActionResult Edit(string Id)
        {
            ProductCategory productCategory = context.Find(Id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategory);
            }
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory product, string Id)
        {
            ProductCategory productCategoryToEdit = context.Find(Id);
            if (productCategoryToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                productCategoryToEdit.Category = product.Category;
              
                context.Commit();
                return RedirectToAction("Index"); //Redirect to product home page

            }
        }

        
        public ActionResult Delete(string Id)
        {
            ProductCategory productCategoryToDelete = context.Find(Id);
            if (productCategoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCategoryToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCategory productCategoryToDelete = context.Find(Id);
            if (productCategoryToDelete == null)
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
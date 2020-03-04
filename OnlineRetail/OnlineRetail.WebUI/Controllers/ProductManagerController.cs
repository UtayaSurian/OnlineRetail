using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineRetail.Core.Contracts;
using OnlineRetail.Core.Models;
using OnlineRetail.Core.ViewModels;
using OnlineRetail.DataAccess.InMemory;

namespace OnlineRetail.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductManagerController : Controller
    {
        IRepository<Product> context; //Object to call all methods delcared in InMemory
        IRepository<ProductCategory> productCategories; //To load all categories from the database for drop-down list
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoriesContext)
        {
            //Initialize the object automatically
            context = productContext;
            productCategories = productCategoriesContext;
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
            ProductManagerViewModel viewModel = new ProductManagerViewModel();
            viewModel.Product = new Product(); //To associate with product model
            viewModel.ProductCategories = productCategories.Collection(); //To extract all categories from the object of viewModel
     
            return View(viewModel); //Return all category and product of models to view page
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {  
            if (!ModelState.IsValid) //Check for validation that in product model
            {
                return View(product);   //Return back the page if user failed to meet the validation
            }
            else
            {
                if(file != null)
                {   //to save image id along with its extension which can be retrieve from a local path of the product images
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }
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
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = productCategories.Collection();

                return View(viewModel);
            }
        }
        [HttpPost]
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
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
                if (file != null)
                {
                    productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                }
                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using OnlineRetail.Core.Models;

namespace OnlineRetail.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {

        ObjectCache cache = MemoryCache.Default;  //Object for cache
        List<ProductCategory> productCategories;

        //constructor
        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>; //To check for cache
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>(); //then install a new cache if not exist
            }

        }
        //Save products in cache
        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }

        //Add products in into cache's list called products
        public void Insert(ProductCategory p)
        {
            productCategories.Add(p);
        }

        public void Update(ProductCategory productCategory)
        {
            ProductCategory productCategoryToUpdate = productCategories.Find(p => p.Id == productCategory.Id);
            if (productCategoryToUpdate != null)
            {
                productCategoryToUpdate = productCategory;
            }
            else
            {
                throw new Exception("Product Category not found");
            }
        }

        //Search
        public ProductCategory Find(string Id)
        {
            ProductCategory productCategory = productCategories.Find(p => p.Id == Id);
            if (productCategory != null)
            {
                return productCategory;
            }
            else
            {
                throw new Exception("Product not found!");
            }
        }

        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string Id)
        {
            ProductCategory productCategoryToDelete = productCategories.Find(p => p.Id == Id);
            if (productCategoryToDelete != null)
            {
                productCategories.Remove(productCategoryToDelete);
            }
            else
            {
                throw new Exception("Product not found!");
            }
        }
    }
}

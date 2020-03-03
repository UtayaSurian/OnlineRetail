using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using OnlineRetail.Core;
using OnlineRetail.Core.Models;
                                        
                                         //__To data from shoping cart temporarily__\\ 

namespace OnlineRetail.DataAccess.InMemory
{
    
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;  //Object for cache
        List<Product> products;

        //constructor
        public ProductRepository()
        {
            products = cache["products"] as List<Product>; //To check for cache
            if(products == null)
            {
                products = new List<Product>(); //then install a new cache if not exist
            }
     
        }
        //Save products in cache
        public void Commit()
        {
            cache["products"] = products;
        }

        //Add products in into cache's list called products
        public void Insert(Product p)
        {
            products.Add(p);
        }

        public void Update(Product product)
        {
            Product productToUpdate = products.Find(p => p.Id == product.Id);
            if (productToUpdate != null)
            {
                productToUpdate = product;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        //Search
        public Product Find(string Id)
        {
            Product product = products.Find(p => p.Id == Id);
            if (product != null)
            {
                return product;
            }
            else
            {
                throw new Exception("Product not found!");
            }
        }

        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }

        public void Delete(string Id)
        {
            Product product = products.Find(p => p.Id == Id);
            if(product != null)
            {
                products.Remove(product);
            }
            else
            {
                throw new Exception("Product not found!");
            }
        }
    }
}

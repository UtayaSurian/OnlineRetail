using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using OnlineRetail.Core.Contracts;

namespace OnlineRetail.DataAccess.InMemory
{                                        //The generic class use for cache, create,edit and delete
                                        //To avoid deduplication of codes
                                        //Reference: https://www.youtube.com/watch?v=vCw_LhhSFU4
                                        //Reference: https://teamtreehouse.com/library/creating-a-generic-base-repository-class
    public class InMemoryRepository<G> : IRepository<G> where G : Core.Models.BaseEntity //Inherit from base entity abstract class
    {
        ObjectCache cache = MemoryCache.Default;
        List<G> items; //placeholder
        string className;

        //constructor
        
        public InMemoryRepository(){
            //To to the object "<G>"
            className = typeof(G).Name;  //Name to get the actual name of any mention class such as Product's and Product Category's model class name
            items = cache[className] as List<G>; //Product or Product Category
            if(items == null)
            {
                items = new List<G>();
            }
           
        }

        public void Commit()
        {
            cache[className] = items;       //Store the items in cache
        }

        //Add products in into cache's list called products
        public void Insert(G g)
        {
            items.Add(g);
        }

        public void Update(G g)
        {
            G gToUpdate = items.Find(i => i.Id == g.Id);

            if (gToUpdate != null)
            {
                gToUpdate = g;
            }
            else
            {
                throw new Exception(className + " not found!");
            }
        }

        public G Find(string Id)
        {
            G g = items.Find(i => i.Id == Id);
            if (g != null)
            {
                return g;
            }
            else
            {
                throw new Exception(className + " not found!");
            }
        }

        public IQueryable<G> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {

            G gToDelete = items.Find(i => i.Id == Id);
            if (gToDelete != null)
            {
               items.Remove(gToDelete);
            }
            else
            {
                throw new Exception(className+" not found!");
            }
        }
    }
}

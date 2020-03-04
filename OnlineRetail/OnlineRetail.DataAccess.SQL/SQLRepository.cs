using OnlineRetail.Core.Contracts;
using OnlineRetail.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRetail.DataAccess.SQL
{
    public class SQLRepository<G> : IRepository<G> where G : BaseEntity
    {
        internal DataContext context;   //calling the DataContext class
        internal DbSet<G> dbSet;            //calling the underline table using entity DbSet<>


        public SQLRepository(DataContext context)
        {
            this.context = context;     //passing the context
            this.dbSet = context.Set<G>();      //Set product or product categories into its own table
        }


        public IQueryable<G> Collection()   
        {
            return dbSet;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var g = Find(Id);
            if (context.Entry(g).State== EntityState.Detached)
            {
                dbSet.Attach(g);
            }

            dbSet.Remove(g);
        }

        public G Find(string Id)
        {
            return dbSet.Find(Id);
        }

        public void Insert(G g)
        {
            dbSet.Add(g);       
        }

        public void Update(G g)
        {
            dbSet.Attach(g); //Passing the object and attach to the table
            context.Entry(g).State = EntityState.Modified;  //Update the data
        }
    }
}

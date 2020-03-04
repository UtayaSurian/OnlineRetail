using OnlineRetail.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRetail.DataAccess.SQL
{                                       //Will be used to inherit from data entity framework class
                                        //Data Migration to SQL database
                                        //Refrence: https://www.youtube.com/watch?v=hV5QO_aJTi8 for migration
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection") //To install the connection strings for SQL Database
        {

        }
        //Models initilizations
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}

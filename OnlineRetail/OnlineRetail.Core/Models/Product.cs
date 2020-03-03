using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnlineRetail.Core.Models
{
    public class Product
    {
        public string Id { get; set; }

        [StringLength(20)]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, 1000)]
        public decimal Price { get; set; }
        public string Category { get; set; }    //For Grouping/Filter Purpose
        public string Image { get; set; }       //Using URL to store image such as path

        //Contructor to automate ID
        public Product()
        {
            //To generate random characters for Id automatically in database
            this.Id = Guid.NewGuid().ToString();
        }

    }
}

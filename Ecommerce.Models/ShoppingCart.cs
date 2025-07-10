using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; } // Foreign key to the Product
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        [Range(1,1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; } // Number of items in the cart
        public string ApplicationUserId { get; set; } // Foreign key to the User
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        public double Price { get; set; }// This property is not mapped to the database, used for calculations
    }
}

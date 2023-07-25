using E_commerce.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class ShoppingCart
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int ProductID{ get; set; }
        
        [ValidateNever]
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }

        [Required]
        public String ApplicationUserID { get; set; }
        [ForeignKey("ApplicationUserID")]
        [Required]
        public ApplicationUser ApplicationUser { get; set;}

    }
}

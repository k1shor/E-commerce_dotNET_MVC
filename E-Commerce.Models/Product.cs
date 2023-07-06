using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    public class Product
    {
        [Key]
        public int ID{ get; set; }

        [DisplayName("Product Name")]
        [Required(ErrorMessage ="Product Name is required")]
        [MinLength(3, ErrorMessage ="Product must be at least 3 characters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000000, ErrorMessage ="Price must be between 1 and 1000000")]
        public int Price { get; set; }

        [Required]
        [MinLength(30, ErrorMessage ="Description must be at least 30 characters")]
        public String Description { get; set; } = string.Empty;
        [Required]
        [Display(Name ="Count In Stock")]
        public int Count_In_Stock { get; set; }

        [Required]
        [Range(1,5)]
        public int Rating { get; set; }

        [DisplayName("Created At")]
        public string? createdAt { get; set; }

        [Display(Name ="Updated At")]
        public string? updatedAt { get; set; }


        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models
{
    public class Category
    {
        [Key]
        public int ID{ get; set; }
        [DisplayName("Category Name")]
        [Required(ErrorMessage ="Category Name is required")]
        [MinLength(3, ErrorMessage ="Category must be at least 3 characters")]
        public string Name { get; set; } = string.Empty;
        [DisplayName("Created At")]
        public string? createdAt { get; set; }
        public string? updatedAt { get; set; }

    }
}

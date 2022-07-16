using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Abby.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter value for the category name ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter value for display order ")]
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display order must be 1-100")]
        public string DisplayOrder { get; set; }

    }
}
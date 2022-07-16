using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abby.Model
{
    public class MenuItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter value for Menu item name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter value for Menu item description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please upload image for the Menu item")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Please enter price for the Menu item")]
        [Range(1, 1000, ErrorMessage = "Price should between $1 to $1000")]
        public double Price { get; set; }

        [DisplayName("Food Type")]
        public int FoodTypeId { get; set; }
        public FoodType FoodType { get; set; }

        [DisplayName("Category Id")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

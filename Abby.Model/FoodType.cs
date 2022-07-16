using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abby.Model
{
   public class FoodType
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please add value for name field")]
        public string Name { get; set; }
    }
}

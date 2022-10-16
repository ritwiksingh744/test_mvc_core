using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace FoodShopApp.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please enter Category Name.")]
        public string CategoryName { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime? UpdatedOn { get; set; } = null;
        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;

        public ICollection<Product> Products { get; set; }
    }
}

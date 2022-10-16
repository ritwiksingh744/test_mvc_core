using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace FoodShopApp.Models
{

    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public string Type { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; } = DateTime.Now.Date;
        [DataType(DataType.Date)]
        public DateTime? UpdatedOn { get; set; } = null;
        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}

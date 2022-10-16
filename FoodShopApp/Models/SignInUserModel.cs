using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FoodShopApp.Models
{
    public class SignInUserModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Rememer me")]
        public bool RememberMe { get; set; }
    }
}

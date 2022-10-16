using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FoodShopApp.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Please enter registered email."), EmailAddress, Display(Name = "Registered email")]
        public string Email { get; set; }
        public bool EmailSent { get; set; }
    }
}

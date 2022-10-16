using FoodShopApp.Models;
using System.Threading.Tasks;

namespace FoodShopApp.Service
{
    public interface IEmailService
    {
        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
    }
}

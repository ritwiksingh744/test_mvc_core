using FoodShopApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FoodShopApp.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel);
        Task<SignInResult> PasswordSignInAsync(SignInUserModel signInModel);
        Task SignOutAsync();
        Task GenerateForgotPasswordTokenAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model);
    }
}
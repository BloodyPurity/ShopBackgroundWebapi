using Microsoft.IdentityModel.Tokens;
using ShopBackgroundSystem.Models;
using ShopBackgroundSystem.VM;

namespace ShopBackgroundSystem.Services.Interfaces
{
    public interface IUserService
    {
        UserLoginResponseVM Authenticate(User? user);
    }
}

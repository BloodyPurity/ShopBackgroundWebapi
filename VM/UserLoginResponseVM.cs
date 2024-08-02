using ShopBackgroundSystem.Models;

namespace ShopBackgroundSystem.VM
{
    public class UserLoginResponseVM
    {
        public UserLoginResponseVM(User user,string token)
        {
            Username = user.Uaccount;
            Token = token;
            Role = user.Utype;
        }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }

    }
}

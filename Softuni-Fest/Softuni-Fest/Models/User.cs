using Microsoft.AspNetCore.Identity;

namespace Softuni_Fest
{
    public class User : IdentityUser
    {
        public User()
        {
        }

        public string? NamePersonal { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;

namespace Sispos.Services.AuthApi.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
         
    }
}

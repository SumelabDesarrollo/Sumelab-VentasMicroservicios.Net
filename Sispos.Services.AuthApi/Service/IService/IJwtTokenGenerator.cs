using Microsoft.Identity.Client;
using Sispos.Services.AuthApi.Models;

namespace Sispos.Services.AuthApi.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}

using Sispos.Services.AuthApi.Models.Dto;

namespace Sispos.Services.AuthApi.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegisterationRequestDTO registrationRequestDto);

        Task<LoginResponseDto> Login(LoginRequestDTO loginRequestDTO);

        Task<bool> AssignRole(string email, string roleName);
    }
}

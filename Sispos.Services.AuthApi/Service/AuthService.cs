﻿using Microsoft.AspNetCore.Identity;
using Sispos.Services.AuthApi.Data;
using Sispos.Services.AuthApi.Models;
using Sispos.Services.AuthApi.Models.Dto;
using Sispos.Services.AuthApi.Service.IService;

namespace Sispos.Services.AuthApi.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db,IJwtTokenGenerator jwtTokenGenerator,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _jwtTokenGenerator=jwtTokenGenerator;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null) {

                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult()) { 
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();     
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDTO loginRequestDTO) {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (user==null||isValid==false)
            {
                return new LoginResponseDto() {User= null,Token=""};
            }

            var token = _jwtTokenGenerator.GenerateToken(user);
            UserDTO userDTO = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber

            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDTO,
                Token = token
            };

            return loginResponseDto;

        }

        public async Task<string> Register(RegisterationRequestDTO registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber

            };
            try
            {
                var result =await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);

                    UserDTO userDTO = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber

                    };
                    return "";

                }
                else {
                    return result.Errors.FirstOrDefault().Description;
                       
                }
            }
            catch (Exception ex) { 
            
            
            
            }
            return "error encountered";
        }

    }
}

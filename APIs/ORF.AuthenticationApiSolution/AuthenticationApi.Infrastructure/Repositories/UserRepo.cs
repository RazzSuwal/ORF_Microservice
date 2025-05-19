using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories.Interfaces;
using Microservice.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationApi.Infrastructure.Repositories
{
    public class UserRepo(AuthenticationDbContext context, IConfiguration config) : IUser
    {
        private async Task<AppUser> GetUserByEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user is null ? null! : user;
        }
        public async Task<GetUserDTO> GetUser(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            return user is not null ? new GetUserDTO(user.Id,
                user.FirstName!,
                user.LastName!,
                user.MobileNumber!,
                user.Address!,
                user.Email!,
                user.Role!) : null!;
        }

        public async Task<Response> Login(LoginDTO loginDTO)
        {
            var getUser = await GetUserByEmail(loginDTO.Email);
            if (getUser is null)
                return new Response(false, "Invalid credentials");

            bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);
            if (!verifyPassword)
                return new Response(false, "Invalid credentials");

            string token = GenerateToken(getUser);
            return new Response(true, token);
        }

        private string GenerateToken(AppUser user)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.FirstName!),
                new(ClaimTypes.Email, user.Email!)
            };
            if (!string.IsNullOrEmpty(user.Role) || !Equals("string", user.Role))
                claims.Add(new(ClaimTypes.Role, user.Role!));

            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience: config["AuthenticationApi:Audience"],
                claims: claims,
                expires: null,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Response> Register(AppUserDTO appUserDTO)
        {
            var getUser = await GetUserByEmail(appUserDTO.Email);
            if (getUser is null)
                return new Response(false, $"you cannont user this email for registration");

            var result = context.Users.Add(new AppUser()
            {
                FirstName = appUserDTO.FirstName,
                LastName = appUserDTO.LastName,
                MobileNumber = appUserDTO.MobileNumber,
                Address = appUserDTO.Address,
                Email = appUserDTO.Email,
                Role = appUserDTO.Role,
                Password = BCrypt.Net.BCrypt.HashPassword(appUserDTO.Password)
            });

            await context.SaveChangesAsync();
            return result.Entity.Id > 0 ? new Response(true, "User registered successfully") :
            new Response(false, "Invalid data provided");
        }
    }
}

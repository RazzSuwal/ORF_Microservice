using AuthenticationApi.Application.DTOs;
using Microservice.SharedLibrary.Response;

namespace AuthenticationApi.Infrastructure.Repositories.Interfaces
{
    public interface IUser
    {
        Task<Response> Register(AppUserDTO appUserDTO);
        Task<Response> Login(LoginDTO loginDTO);
        Task<GetUserDTO> GetUser(int userId);
    }
}

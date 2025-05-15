using Microservice.SharedLibrary.Response;
using PropertyApi.Domain.Entities;

namespace PropertyApi.Infrastructure.Repositories.Interfaces
{
    public interface IPropertyRepo
    {
        Task<Response> CreateAsync(Property entity);
        Task<Response> CreateAsync(int id);
        Task<IEnumerable<Property>> GetAllAsync();
        Task<Response> UpdateAsync(Property entity);
    }
}

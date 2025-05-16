using Microservice.SharedLibrary.Response;
using PropertyApi.Domain.Entities;

namespace PropertyApi.Infrastructure.Repositories.Interfaces
{
    public interface IPropertyRepo
    {
        Task<Response> CreateAsync(Property entity);
        Task<Property> GetByIdAsync(int id);
        Task<IEnumerable<Property>> GetAllAsync();
        Task<Response> UpdateAsync(Property entity);
        Task<Response> DeleteAsync(int id);
    }
}

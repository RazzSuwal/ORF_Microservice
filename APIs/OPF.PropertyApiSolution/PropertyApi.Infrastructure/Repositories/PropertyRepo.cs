using PropertyApi.Domain.Entities;
using PropertyApi.Infrastructure.Repositories.Interfaces;
using Microservice.SharedLibrary.Response;
using Microservice.SharedLibrary.CURDHelper.Interfaces;


namespace PropertyApi.Infrastructure.Repositories
{
    public class PropertyRepo : IPropertyRepo
    {
        private readonly ICURDHelper _helper;

        public PropertyRepo(ICURDHelper helper)
        {
            _helper = helper;
        }

        public Task<Response> CreateAsync(Property entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> CreateAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Property>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Property entity)
        {
            throw new NotImplementedException();
        }
    }
}
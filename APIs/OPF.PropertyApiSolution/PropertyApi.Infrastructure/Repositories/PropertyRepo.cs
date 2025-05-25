using Microservice.SharedLibrary.CURDHelper.Interfaces;
using Microservice.SharedLibrary.Logs;
using Microservice.SharedLibrary.Response;
using PropertyApi.Domain.Entities;
using PropertyApi.Infrastructure.Repositories.Interfaces;


namespace PropertyApi.Infrastructure.Repositories
{
    public class PropertyRepo(ICURDHelper helper) : IPropertyRepo
    {
        private readonly ICURDHelper _helper = helper;

        public async Task<Response> CreateAsync(Property entity)
        {
            try
            {
                var currentEntity = await _helper.InsertAsync(entity);
                if (currentEntity == 1)
                    return new Response(true, $"{entity.Name} property added successfully");
                else
                    return new Response(false, $"Error occured while adding property {entity.Name}");
            }
            catch (Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                // display scary-free message in the client
                return new Response(false, "Error occurred adding new property");
            }
        }

        public async Task<Property> GetByIdAsync(int id)
        {
            try
            {
                var property = await _helper.GetAsync<Property>(id);
                return property is not null ? property : null!;
            }
            catch (Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                // display scary-free message in the client
                throw new Exception("Error occurred while retrieving property");
            }
        }

        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            try
            {
                var property = await _helper.GetListAsync<Property>(null!);
                return property is not null ? property : null!;
            }
            catch (Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                // display scary-free message in the client
                throw new Exception("Error occurred while retrieving property");
            }
        }

        public async Task<Response> UpdateAsync(Property entity)
        {
            try
            {
                var property = await _helper.GetAsync<Property>(entity.Id);
                if (property is null)
                    return new Response(false, $"{entity.Name} property notfound");
                var currentEntity = await _helper.UpdateAsync(entity);
                if (currentEntity == 1)
                    return new Response(true, $"{entity.Name} property update successfully");
                else
                    return new Response(false, $"Error occured while updating property {entity.Name}");
            }
            catch (Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                // display scary-free message in the client
                return new Response(false, "Error occurred updating property");
            }
        }

        public async Task<Response> DeleteAsync(int id)
        {
            try
            {
                var property = await _helper.GetAsync<Property>(id);
                if (property is null)
                    return new Response(false, "Property notfound");
                var currentEntity = await _helper.DeleteAsync<Property>(id);
                if (currentEntity == 1)
                    return new Response(true, "Property delete successfully");
                else
                    return new Response(false, "Error occured while deleting property");
            }
            catch (Exception ex)
            {
                //log the original exception
                LogException.LogExceptions(ex);

                // display scary-free message in the client
                return new Response(false, "Error occurred deleting property");
            }
        }
    }
}
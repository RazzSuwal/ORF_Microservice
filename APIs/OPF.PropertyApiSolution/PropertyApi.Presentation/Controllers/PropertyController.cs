using Microservice.SharedLibrary.Response;
using Microsoft.AspNetCore.Mvc;
using PropertyApi.Application.DTOs;
using PropertyApi.Application.DTOs.Conversions;
using PropertyApi.Infrastructure.Repositories.Interfaces;

namespace PropertyApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController(IPropertyRepo propertyRepo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetProperties()
        {
            var properties = await propertyRepo.GetAllAsync();

            if (!properties.Any())
                return NotFound("No properties detected in database");

            var (_, list) = PropertyConversion.FromEntity(null!, properties);

            return list!.Any() ? Ok(list) : NotFound("No property found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PropertyDTO>> GetPropertyById(int id)
        {
            var property = await propertyRepo.GetByIdAsync(id);

            if (property is null)
                return NotFound("Property requested not found");

            var (_property, _) = PropertyConversion.FromEntity(property, null!);

            return _property is not null ? Ok(_property) : NotFound("No property found");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProperty(PropertyDTO property)
        {
            //check model state is all data annotations are passed
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //convert to entity
            var getEntity = PropertyConversion.ToEntity(property);
            var response = await propertyRepo.CreateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateProperty(PropertyDTO property)
        {
            //check model state is all data annotations are passed
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //convert to entity
            var getEntity = PropertyConversion.ToEntity(property);
            var response = await propertyRepo.UpdateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Response>> DeleteProperty(int id)
        {
            var property = await propertyRepo.GetByIdAsync(id);

            if (property is null)
                return NotFound("Property not found");
            var response = await propertyRepo.DeleteAsync(id);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}

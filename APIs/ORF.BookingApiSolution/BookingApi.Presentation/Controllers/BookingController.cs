using BookingApi.Application.DTOs;
using BookingApi.Application.DTOs.Conversion;
using BookingApi.Infrastructure.Respositories.Interfaces;
using Microservice.SharedLibrary.Response;
using Microsoft.AspNetCore.Mvc;
using static Dapper.SqlMapper;

namespace BookingApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController(IBookingRepo bookingRepo) : ControllerBase
    {

        [HttpPost("CreateUpdateBooking")]
        public async Task<ActionResult<Response>> CreateUpdateBooking(BookingDTO booking)
        {
            //check model state is all data annotations are passed
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //convert to entity
            var getEntity = BookingConversion.ToEntity(booking);
            Response response;
            if (booking.Id is null or 0)
            {
                response = await bookingRepo.CreateAsync(getEntity);
            }
            else
            {
                response = await bookingRepo.UpdateAsync(getEntity);
            }
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}

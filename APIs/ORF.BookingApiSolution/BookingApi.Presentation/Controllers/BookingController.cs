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

        [HttpPost("CreateBooking")]
        public async Task<ActionResult<Response>> CreateBooking(BookingDTO booking)
        {
            //check model state is all data annotations are passed
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //convert to entity
            var getEntity = BookingConversion.ToEntity(booking);
            var response = await bookingRepo.CreateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpPut("UpdateBooking")]
        public async Task<ActionResult<Response>> UpdateBooking(BookingDTO booking)
        {
            //check model state is all data annotations are passed
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //convert to entity
            var getEntity = BookingConversion.ToEntity(booking);
            var response = await bookingRepo.UpdateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }
}

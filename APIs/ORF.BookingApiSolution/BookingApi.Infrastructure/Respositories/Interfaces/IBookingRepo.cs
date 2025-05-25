using BookingApi.Domain.Entities;
using Microservice.SharedLibrary.Response;

namespace BookingApi.Infrastructure.Respositories.Interfaces
{
    public interface IBookingRepo
    {
        Task<Response> CreateAsync(Booking entity);
        Task<Response> UpdateAsync(Booking entity);
    }
}

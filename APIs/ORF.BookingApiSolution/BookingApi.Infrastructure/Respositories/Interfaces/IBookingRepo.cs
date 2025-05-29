using BookingApi.Application.DTOs;
using BookingApi.Domain.Entities;
using Microservice.SharedLibrary.Response;

namespace BookingApi.Infrastructure.Respositories.Interfaces
{
    public interface IBookingRepo
    {
        Task<Response> CreateAsync(Booking entity);
        Task<Response> UpdateAsync(Booking entity);
        Task<IEnumerable<BookingDTO>> GetBookingByClientId(int bookBy);
        Task<BookingDTO> GetBookingDetails(int bookingId);
        Task<IEnumerable<BookingDetailsDTO>> GetBookingByPropertyId(int propertyId);
    }
}

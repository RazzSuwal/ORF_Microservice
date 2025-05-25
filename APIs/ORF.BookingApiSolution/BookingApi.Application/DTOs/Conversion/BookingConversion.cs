using BookingApi.Domain.Entities;

namespace BookingApi.Application.DTOs.Conversion
{
    public static class BookingConversion
    {
        public static Booking ToEntity(BookingDTO booking) => new()
        {
            Id = booking.Id,
            Description = booking.Description,
            PropertyId = booking.PropertyId,
            BookBy = booking.BookBy
        };
    }
}

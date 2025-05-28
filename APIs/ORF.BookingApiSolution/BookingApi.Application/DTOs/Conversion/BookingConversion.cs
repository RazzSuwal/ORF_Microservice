using BookingApi.Domain.Entities;

namespace BookingApi.Application.DTOs.Conversion
{
    public static class BookingConversion
    {
        public static Booking ToEntity(BookingDTO booking) => new()
        {
            Id = booking.Id,
            PropertyId = booking.PropertyId,
            Description = booking.Description,
            BookBy = booking.BookBy
        };

        public static (BookingDTO?, IEnumerable<BookingDTO>?) FromEntity(Booking booking, IEnumerable<Booking>? bookings)
        {
            //return single
            if (booking is not null || bookings is null)
            {
                var singlePropety = new BookingDTO
                    (
                    booking!.Id,
                    booking.PropertyId,
                    booking.Description!,
                    booking.BookBy,
                    booking.BookOn
                    );
                return (singlePropety, null);
            }

            //return list
            if (bookings is not null || booking is null)
            {
                var _bookings = bookings!.Select(b =>
                    new BookingDTO(b.Id, b.PropertyId!, b.Description!, b.BookBy, b.BookOn)).ToList();

                return (null, _bookings);
            }

            return (null, null);
        }
    }
}

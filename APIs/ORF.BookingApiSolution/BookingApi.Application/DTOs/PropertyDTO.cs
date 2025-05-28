using System.ComponentModel.DataAnnotations;

namespace BookingApi.Application.DTOs
{
    public record PropertyDTO
    (
        int Id,
        [Required] string Name,
        [Required] string Description,
        [Required, Range(0.1, float.MaxValue)] decimal Area,
        [Required] string Location,
        [Required, Range(1, int.MaxValue)] int NumberOfRooms,
        [Required, DataType(DataType.Currency)] decimal Price
    );
}

using System.ComponentModel.DataAnnotations;

namespace PropertyApi.Application.DTOs
{
    public record PropertyDTO
    (
        int Id,
        [Required] string Name,
        [Required] string Description,
        [Required, Range(1, int.MaxValue)] int Area,
        [Required] string Location,
        [Required, Range(1, int.MaxValue)] int NumberOfRooms,
        [Required, DataType(DataType.Currency)] decimal Price
    );
}

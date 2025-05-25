using System.ComponentModel.DataAnnotations;

namespace BookingApi.Application.DTOs
{
    public record BookingDTO
    (
        int? Id,
        [Required] int PropertyId,
        string? Description,
        [Required] int BookBy
    );
}

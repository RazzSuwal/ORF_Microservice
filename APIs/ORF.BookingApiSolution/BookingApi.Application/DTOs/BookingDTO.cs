using System.ComponentModel.DataAnnotations;

namespace BookingApi.Application.DTOs
{
    public record BookingDTO
    (
        int Id,
        [Required, Range(1, int.MaxValue)] int PropertyId,
        string? Description,
        [Required, Range(1, int.MaxValue)] int BookBy,
        DateTime BookOn
    );
}

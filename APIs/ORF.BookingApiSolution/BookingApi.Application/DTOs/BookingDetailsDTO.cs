using System.ComponentModel.DataAnnotations;

namespace BookingApi.Application.DTOs
{
    public record BookingDetailsDTO
    (
         int? Id,
        string? Description,
        [Required] DateTime BookOn,
        [Required, Range(1, int.MaxValue)] int PropertyId,
        [Required] string Name,
        [Required] string PropertyDescription,
        [Required, Range(0.1, float.MaxValue)] decimal Area,
        [Required] string Location,
        [Required, Range(1, int.MaxValue)] int NumberOfRooms,
        [Required, DataType(DataType.Currency)] decimal Price,
        [Required, Range(1, int.MaxValue)] int BookBy,
        [Required] string FirstName,
        [Required] string LastName,
        [Required] string MobileNumber,
        [Required] string Address,
        [Required, EmailAddress] string Email
    );
}

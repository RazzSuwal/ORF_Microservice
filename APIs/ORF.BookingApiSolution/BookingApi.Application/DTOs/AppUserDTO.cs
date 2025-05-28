using System.ComponentModel.DataAnnotations;

namespace BookingApi.Application.DTOs
{
    public record AppUserDTO
    (
         int Id,
        [Required] string FirstName,
        [Required] string LastName,
        [Required] string MobileNumber,
        [Required] string Address,
        [Required, EmailAddress] string Email,
        [Required] string Role
    );
}

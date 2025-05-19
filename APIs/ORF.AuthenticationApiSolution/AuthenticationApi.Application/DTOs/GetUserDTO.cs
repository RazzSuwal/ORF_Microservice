using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Application.DTOs
{
    public record GetUserDTO
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

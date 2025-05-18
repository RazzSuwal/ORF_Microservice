using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Application.DTOs
{
    public record AppUserDTO
    (
        int Id,
        [Required] string UserName,
        [Required] string FirstName,
        [Required] string LastName,
        [Required] string MobileNumber,
        [Required] string Address,
        [Required, EmailAddress] string Email,
        [Required] string Password,
        [Required] string Role
    );
}

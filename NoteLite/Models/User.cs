using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NoteLite.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "First Name Is Required")]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required(ErrorMessage = "Last Name Is Required")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        public string? Password { get; set; }
    }
}

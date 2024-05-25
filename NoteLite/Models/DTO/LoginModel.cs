using System.ComponentModel.DataAnnotations;

namespace NoteLite.Models.DTO
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email Is Required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password Must Not be Required")]
        public string? Password { get; set; }
    }
}

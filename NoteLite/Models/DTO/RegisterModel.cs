using System.ComponentModel.DataAnnotations;

namespace NoteLite.Models.DTO
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "First Name Is Required")]
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required(ErrorMessage = "Last Name Is Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        [MinLength(8, ErrorMessage = "password Must be at Least 8 Character long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",ErrorMessage = "The password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "phone Number Is Required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "The phone number must contain exactly 10 digits.")]
        public string PhoneNumber { get; set; }
        [Required]
        [Compare("Password", ErrorMessage ="Password and Confirm Password Should be Matched!")]
        public string ConfirmPassword { get; set; }
    }
}

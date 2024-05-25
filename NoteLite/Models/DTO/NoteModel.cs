using System.ComponentModel.DataAnnotations;

namespace NoteLite.Models.DTO
{
    public class NoteModel
    {
        [Required(ErrorMessage = "Note Title Is Required")]
        [StringLength(20, ErrorMessage ="Title of Note can be set between 5 to 20 Characters only."),MinLength(5, ErrorMessage = "Title of Note can be set between 5 to 20 Characters only.")]
        public string NoteTitle { get; set; } = null!;
        [Required(ErrorMessage = "Description Is Required")]
        [StringLength(500, ErrorMessage = "Description Must be Greater than 20 Characters"), MinLength(5)]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "Category Is Required")]
        public int Category_Id { get; set; }
        public string? User_Id { get; set; }
    }
}

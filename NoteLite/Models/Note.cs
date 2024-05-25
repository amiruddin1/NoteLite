using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteLite.Models
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }
        [Required(ErrorMessage = "Note Title Is Required")]
        public string NoteTitle { get; set; } = null!;
        [Required(ErrorMessage = "Description Is Required")]
        public string Description { get; set; } = null!;
        [ForeignKey("CategoryId")]
        [Required(ErrorMessage = "Category Is Required")]
        public int CategoryId { get; set; }
        public string? UserId { get; set; }
        public DateTime NoteAdded { get; set; }
        public virtual Category? Category { get; set; } = null!;
        public ICollection<Tag>? Tags { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace NoteLite.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? TimeStamp { get; set; }
        public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}

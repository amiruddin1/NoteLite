using System.ComponentModel.DataAnnotations.Schema;

namespace NoteLite.Models
{
    public class Tag
    {
        public int TagID { get; set; }
        public int NoteId { get; set; }
        public string UserId { get; set; }
        public DateTime TagTimeSpan { get; set; }
        public Note Note { get; set; }

    }
}

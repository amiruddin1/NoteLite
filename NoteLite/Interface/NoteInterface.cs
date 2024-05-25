using NoteLite.Models;
using NoteLite.Models.DTO;

namespace NoteLite.Interface
{
    public interface NoteInterface
    {
        Task<List<Note>> GetAll();
        Task<List<Note>> GetAllByCategory(int id);
        Task<List<Note>> GetAllByCategoryUser(int id, string uid);
        Task<List<Note>> GetAllByUser(string id);
        Task<List<Note>> GetAllBySearch(string search,string id);
        Task<Note> GetById(int id);
        Task<String> AddNewNote(NoteModel note);
        Task<String> EditNote(Note note);
        Task<String> RemoveNewNote(int id);
        Task<bool> TagPerson(Tag tag);
        Task<List<Tag>> GetTaggedPost(string userId);
        Task<List<Tag>> GetAllTaggedData();
        Task<bool> DeleteTaggedPosts(int postId);
    }
}

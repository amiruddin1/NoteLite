using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteLite.Interface;
using NoteLite.Models;
using NoteLite.Models.DTO;
using System.Security.Claims;

namespace NoteLite.Repository
{
    public class NoteRepository : NoteInterface
    {
        private readonly NoteDBContext _Context;

        public NoteRepository(NoteDBContext noteLiteContext)
        {
            _Context = noteLiteContext;
        }
        public async Task<string> AddNewNote(NoteModel note)
        {
            if(note == null)
            {
                return "Empty Data Passed";
            }
            else
            {
                Note newNote = new Note()
                {
                    CategoryId = note.Category_Id,
                    Description = note.Description,
                    NoteAdded = DateTime.Now,
                    NoteTitle = note.NoteTitle,
                    UserId = note.User_Id
                };

                await _Context.Notes.AddAsync(newNote);
                var result = await _Context.SaveChangesAsync();
                if (result > 0)
                {
                    return "Success";
                }
                return "Failed";
            }
        }

        public async Task<bool> DeleteTaggedPosts(int postId)
        {
            if (postId != 0)
            {
                var data = _Context.Tags.Where(a => a.NoteId == postId).FirstOrDefault();
                if (data != null)
                {
                    _Context.Tags.Remove(data);
                    var result = await _Context.SaveChangesAsync();
                    if(result> 0)
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<string> EditNote(Note note)
        {
            if (note == null)
            {
                return "Empty Data Passed";
            }
            else
            {
                Note noteData = _Context.Notes.Find(note.NoteId);
                noteData.NoteTitle = note.NoteTitle;
                noteData.Description = note.Description;
                noteData.CategoryId = note.CategoryId;
                noteData.NoteAdded = DateTime.Now;
                var result = await _Context.SaveChangesAsync();
                if (result > 0)
                {
                    return "Success";
                }
                return "Failed";
            }
        }

        public async Task<List<Note>> GetAll()
        {
            return await _Context.Notes.ToListAsync();
        }

        public async Task<List<Note>> GetAllByCategory(int id)
        {
            if (id == 0)
            {
                return null;
            }
            return await _Context.Notes.Where(a=>a.CategoryId==id).ToListAsync();
        }
        public async Task<List<Note>> GetAllByCategoryUser(int id, string uid)
        {
            if (id == 0)
            {
                return null;
            }
            return await _Context.Notes.Where(a => a.CategoryId == id && a.UserId == uid).ToListAsync();
        }

        public async Task<List<Note>> GetAllBySearch(string search, string id)
        {
            if (id == null)
                return null;
            return await _Context.Notes.Where(a => a.UserId == id && (a.Description.Contains(search) || a.NoteTitle.Contains(search))).ToListAsync();
        }

        public async Task<List<Note>> GetAllByUser(string id)
        {
            if (id == null)
                return null;
            return await _Context.Notes.Where(a => a.UserId == id).ToListAsync();
        }

        public Task<List<Tag>> GetAllTaggedData()
        {
            return _Context.Tags.ToListAsync();
        }

        public async Task<Note> GetById(int id)
        {
            if(id == 0)
            {
                return null;
            }
            return await _Context.Notes
                        .Include(a => a.Category)
                        .Where(a=>a.NoteId==id).FirstOrDefaultAsync();
        }

        public async Task<List<Tag>> GetTaggedPost(string userId)
        {
            if (userId != null)
            {
                return await _Context.Tags.Where(a=>a.UserId==userId).ToListAsync();
            }
            return null;
        }

        public async Task<string> RemoveNewNote(int id)
        {
            if (id == 0)
            {
                return null;
            }
            Note noteData = _Context.Notes.Find(id);
            if (noteData != null)
            {
                _Context.Remove(noteData);
                var result = await _Context.SaveChangesAsync();
                if (result > 0)
                {
                    return "Success";
                }
                return "Failed";
            }
            return "Failed";
        }

        public async Task<bool> TagPerson(Tag tag)
        {
            if (tag == null)
            {
                return false;
            }
            _Context.Tags.Add(tag);
            var result = await _Context.SaveChangesAsync();
            if (result > 0)
            { 
                return true;
            }
            return false;
        }
    }
}

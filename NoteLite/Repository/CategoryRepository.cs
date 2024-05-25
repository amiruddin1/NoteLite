using Microsoft.EntityFrameworkCore;
using NoteLite.Interface;
using NoteLite.Models;

namespace NoteLite.Repository
{
    public class CategoryRepository : CategoryInterface
    {
        private readonly NoteDBContext _Context;

        public CategoryRepository(NoteDBContext noteLiteContext)
        {
            _Context = noteLiteContext;
        }
        public async Task<string> AddNewCategory(Category category)
        {
            if (category == null)
            {
                return "Empty Data Passed";
            }
            else
            {
                _Context.Categories.Add(category);
                var result = await _Context.SaveChangesAsync();
                if (result > 0)
                {
                    return "Success";
                }
                return "Failed";
            }
        }

        public async Task<string> EditCategory(Category category)
        {
            if (category == null)
            {
                return "Empty Data Passed";
            }
            else
            {
                Category existing = _Context.Categories.Find(category.CategoryId);
                existing.CategoryName = category.CategoryName;
                existing.TimeStamp = DateTime.Now.ToString();
                var result = await _Context.SaveChangesAsync();
                if (result > 0)
                {
                    return "Success";
                }
                return "Failed";
            }
        }

        public async Task<List<Category>> GetAll()
        {
            return await _Context.Categories
                       .ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            if (id == 0)
            {
                return null;
            }
            return await _Context.Categories
                        .Where(a => a.CategoryId == id).FirstOrDefaultAsync();
        }

        public async Task<string> Removecategory(int id)
        {
            if (id == 0)
            {
                return null;
            }
            Category catData = _Context.Categories.Find(id);
            if (catData != null)
            {
                _Context.Remove(catData);
                var result = await _Context.SaveChangesAsync();
                if (result > 0)
                {
                    return "Success";
                }
                return "Failed";
            }
            return "Failed";
        }
    }
}

using NoteLite.Models;

namespace NoteLite.Interface
{
    public interface CategoryInterface
    {
        Task<List<Category>> GetAll();
        Task<Category> GetById(int id);
        Task<String> AddNewCategory(Category category);
        Task<String> EditCategory(Category category);
        Task<String> Removecategory(int id);
    }
}

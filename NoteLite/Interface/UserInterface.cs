using NoteLite.Models;
using NoteLite.Models.DTO;

namespace NoteLite.Interface
{
    public interface UserInterface
    {
        Task<string> Login(LoginModel loginModel);
        Task<bool> Logout();
        Task<string> Register(RegisterModel registerModel);
        Task<User> GetByID(string id);
        Task<List<User>> GetAll();
        Task<string> RemoveUserByID(string id);
    }
}

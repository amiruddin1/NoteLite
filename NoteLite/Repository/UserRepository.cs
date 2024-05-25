using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteLite.Interface;
using NoteLite.Models;
using NoteLite.Models.DTO;

namespace NoteLite.Repository
{
    public class UserRepository : UserInterface
    {
        private readonly NoteDBContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(NoteDBContext dbContext, UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<List<User>> GetAll()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetByID(string id)
        {
            if (id == null)
            {
                return null;
            }
            return await _dbContext.Users.Where(a=>a.Id==id).FirstOrDefaultAsync();
        }

        public async Task<string> Login(LoginModel loginModel)
        {
            if (loginModel == null)
            {
                return "Null Data Passed";
            }

            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginModel.Email);

                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("User"))
                {
                    return "User Logged in Successfully";
                }
                else if (roles.Contains("Admin"))
                {
                    return "Admin Logged in Successfully";
                }
                else
                {
                    return "No Identity Found!";
                }
                
            }
            else
            {
                return "Enter Valid Credentials";
            }
        }

        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<string> Register(RegisterModel userDTO)
        {
            if (userDTO == null)
            {
                return "Null Data Passed";
            }
            var user = new User
            {
                FirstName = userDTO.FirstName,
                MiddleName = userDTO.MiddleName,
                LastName = userDTO.LastName,
                PhoneNumber = userDTO.PhoneNumber,
                Email = userDTO.Email,
                UserName = userDTO.Email
            };
            var result = await _userManager.CreateAsync(user, userDTO.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                var response = await _userManager.AddToRoleAsync(user, "User");
                if (response.Succeeded)
                {
                    return "User Added Successfully";
                }
                return "Failed to Add user";
            }
            else
            {
                return "Failed to Add user";
            }
        }

        public async Task<string> RemoveUserByID(string id)
        {
            if (id == null)
            {
                return "Null Data Passed";
            }
            else
            {
                var user = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return "Success";
                }
                return "Failed";
            }
        }
    }
}

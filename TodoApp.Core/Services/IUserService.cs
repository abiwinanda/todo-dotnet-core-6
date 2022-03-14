using System;
using TodoApp.Data.Entities;

namespace TodoApp.Core.Services
{
    public interface IUserService
    {
        Task<User> RegisterUser(string username, string password);
        string CreateUserToken(User user);
        string? Login(string username, string password);
        Task<bool> IsUserExists(int userId);
    }
}


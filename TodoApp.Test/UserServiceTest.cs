using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoApp.Core.Repositories;
using TodoApp.Core.Services;
using TodoApp.Data.DbContexts;
using TodoApp.Data.Entities;
using Xunit;
using System.IO;
using System.Text;

namespace TodoApp.Test;

public class UserServiceTest
{
    private readonly TodoDbContext _context;
    private readonly IRepository<User> _userRepository;
    private readonly IUserService _userService;

    public UserServiceTest()
    {
        DbContextOptionsBuilder<TodoDbContext> dbOptions = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(
                Guid.NewGuid().ToString()
            );

        _context = new TodoDbContext(dbOptions.Options);
        _userRepository = new Repository<User>(_context);

        var appSettings = @"{""AppSettings"":{
            ""Token"" : ""super secret token""
        }}";

        var builder = new ConfigurationBuilder();
        builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));
        var configuration = builder.Build();

        _userService = new UserService(_userRepository, configuration);
    }

    [Fact]
    public async void RegisterUser_Should_Create_New_User()
    {
        User user = await _userService.RegisterUser("username", "password");
        Assert.NotNull(user);
    }

    [Fact]
    public async void CreateUserToken_Should_Return_Jwt_Token()
    {
        User user = await _userService.RegisterUser("username", "password");
        string token = _userService.CreateUserToken(user);

        Assert.NotNull(token);
    }

    [Fact]
    public async void Login_Should_Failed_If_Wrong_Password()
    {
        User user = await _userService.RegisterUser("username", "password");
        string? token = _userService.Login("username", "wrongpassword");
        Assert.Null(token);
    }

    [Fact]
    public async void Login_Should_Success_If_Correct_Password()
    {
        User user = await _userService.RegisterUser("username", "password");
        string? token = _userService.Login("username", "password");
        Assert.NotNull(token);
    }
}
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Repositories;
using TodoApp.Core.Services;
using TodoApp.Data.DbContexts;
using TodoApp.Data.Entities;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;

namespace TodoApp.Test;

public class TodoServiceTest
{
    private readonly TodoDbContext _context;
    private readonly IRepository<TodoTag> _todoTagRepository;
    private readonly IRepository<TodoList> _todoListRepository;
    private readonly IRepository<TodoItem> _todoItemRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IUserService _userService;
    private readonly ITodoService _todoService;

    public TodoServiceTest()
    {
        DbContextOptionsBuilder<TodoDbContext> dbOptions = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(
                Guid.NewGuid().ToString()
            );

        _context = new TodoDbContext(dbOptions.Options);
        _todoTagRepository = new Repository<TodoTag>(_context);
        _todoListRepository = new Repository<TodoList>(_context);
        _todoItemRepository = new Repository<TodoItem>(_context);
        _userRepository = new Repository<User>(_context);

        var appSettings = @"{""AppSettings"":{
            ""Token"" : ""super secret token""
        }}";

        var builder = new ConfigurationBuilder();
        builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));
        var configuration = builder.Build();

        _userService = new UserService(_userRepository, configuration);
        _todoService = new TodoService(_todoTagRepository, _todoListRepository, _todoItemRepository);
    }

    [Fact]
    public async void GetAllTodoTags_Should_Return_All_Tags()
    {
        await _todoTagRepository.Insert(new TodoTag { Id = 1, Name = "Personal" });
        await _todoTagRepository.Insert(new TodoTag { Id = 2, Name = "Work" });
        await _todoTagRepository.Insert(new TodoTag { Id = 3, Name = "Recreation" });
        await _todoTagRepository.Save();

        var todoTags = _todoService.GetAllTodoTags();

        Assert.Equal(3, todoTags.Count());
    }

    [Fact]
    public async void GetTodoTagById_Should_Return_A_Todo_Tag_If_Exists()
    {
        await _todoTagRepository.Insert(new TodoTag { Id = 1, Name = "Personal" });
        await _todoTagRepository.Save();

        var todoTag = await _todoService.GetTodoTagById(1);

        Assert.Equal("Personal", todoTag.Name);
    }

    [Fact]
    public async void GetTodoTagById_Should_Return_Null_If_Does_Not_Exists()
    {
        await _todoTagRepository.Insert(new TodoTag { Id = 1, Name = "Personal" });
        await _todoTagRepository.Save();

        var todoTags = await _todoService.GetTodoTagById(2);

        Assert.Null(todoTags);
    }

    [Fact]
    public async void GetListOfTodoTagByListOfId_Should_Return_List_Of_TodoTag()
    {
        await _todoTagRepository.Insert(new TodoTag { Id = 1, Name = "Personal" });
        await _todoTagRepository.Insert(new TodoTag { Id = 2, Name = "Work" });
        await _todoTagRepository.Insert(new TodoTag { Id = 3, Name = "Recreation" });
        await _todoTagRepository.Save();

        var todoTags = _todoService.GetListOfTodoTagByListOfId(new List<int> { 1, 3 });

        Assert.Equal(2, todoTags.Count());
        Assert.Equal("Personal", todoTags[0].Name);
        Assert.Equal("Recreation", todoTags[1].Name);
    }

    [Fact]
    public async void IsTodoTagExists_Should_Return_True_If_TodoTag_Exists()
    {
        await _todoTagRepository.Insert(new TodoTag { Id = 1, Name = "Personal" });
        await _todoTagRepository.Save();

        var IsExists = await _todoService.IsTodoTagExists(1);

        Assert.True(IsExists);
    }

    [Fact]
    public async void IsTodoTagExists_Should_Return_False_If_TodoTag_Doet_Not_Exists()
    {
        await _todoTagRepository.Insert(new TodoTag { Id = 1, Name = "Personal" });
        await _todoTagRepository.Save();

        var IsExists = await _todoService.IsTodoTagExists(2);

        Assert.False(IsExists);
    }

    [Fact]
    public async void GetAllUserTodoList_Should_Return_All_User_Todo()
    {
        User user1 = await _userService.RegisterUser("username1", "password");
        User user2 = await _userService.RegisterUser("username2", "password");

        await _todoListRepository.Insert(new TodoList() { Name = "List 1", UserId = user1.Id });
        await _todoListRepository.Insert(new TodoList() { Name = "List 2", UserId = user1.Id });
        await _todoListRepository.Insert(new TodoList() { Name = "List 3", UserId = user2.Id });
        await _todoListRepository.Save();

        var user1TodoLists = _todoService.GetAllUserTodoList(user1.Id);

        Assert.Equal(2, user1TodoLists.Count());
    }

    [Fact]
    public async void GetUserTodoListById_Should_Return_TodoList_If_TodoList_Exists()
    {
        User user1 = await _userService.RegisterUser("username1", "password");

        await _todoListRepository.Insert(new TodoList() { Id = 1, Name = "List 1", UserId = user1.Id });
        await _todoListRepository.Save();

        var todoList = _todoService.GetUserTodoListById(user1.Id, 1);

        Assert.NotNull(todoList);
    }

    [Fact]
    public async void GetUserTodoListById_Should_Return_Null_If_TodoList_Doest_Not_Exists()
    {
        User user1 = await _userService.RegisterUser("username1", "password");
        User user2 = await _userService.RegisterUser("username2", "password");

        await _todoListRepository.Insert(new TodoList() { Id = 1, Name = "List 1", UserId = user1.Id });
        await _todoListRepository.Save();

        var todoList = _todoService.GetUserTodoListById(user2.Id, 1);

        Assert.Null(todoList);
    }

    [Fact]
    public async void CreateTodoList_Should_Create_New_TodoList()
    {
        User user1 = await _userService.RegisterUser("username1", "password");

        TodoList todoList = await _todoService.CreateTodoList(user1.Id, "ListName");

        Assert.Equal("ListName", todoList.Name);
    }

    [Fact]
    public async void IsUserTodoListExists_Should_Return_True_If_Exists()
    {
        User user1 = await _userService.RegisterUser("username1", "password");
        TodoList todoList = await _todoService.CreateTodoList(user1.Id, "ListName");

        var isExists = _todoService.IsUserTodoListExists(user1.Id, todoList.Id);

        Assert.True(isExists);
    }

    [Fact]
    public async void IsUserTodoListExists_Should_Return_False_If_Does_Not_Exists()
    {
        User user1 = await _userService.RegisterUser("username1", "password");
        User user2 = await _userService.RegisterUser("username2", "password");

        TodoList todoList = await _todoService.CreateTodoList(user1.Id, "ListName");

        var isExists = _todoService.IsUserTodoListExists(user2.Id, todoList.Id);

        Assert.False(isExists);
    }

    [Fact]
    public async void IsTodoItemExists_Should_Return_True_If_Exists()
    {
        User user1 = await _userService.RegisterUser("username1", "password");
        await _todoItemRepository.Insert(new TodoItem() { Id = 1, Name = "Name", UserId = user1.Id });
        await _todoItemRepository.Save();

        var isExists = await _todoService.IsTodoItemExists(1);

        Assert.True(isExists);
    }

    [Fact]
    public async void IsTodoItemExists_Should_Return_False_If_Does_Not_Exists()
    {
        User user1 = await _userService.RegisterUser("username1", "password");
        await _todoItemRepository.Insert(new TodoItem() { Id = 1, Name = "Name", UserId = user1.Id});
        await _todoItemRepository.Save();

        var isExists = await _todoService.IsTodoItemExists(2);

        Assert.False(isExists);
    }

    [Fact]
    public async void DeleteTodoItem_Should_Delete_TodoItem()
    {
        User user1 = await _userService.RegisterUser("username1", "password");
        await _todoItemRepository.Insert(new TodoItem() { Id = 1, Name = "Name", UserId = user1.Id });
        await _todoItemRepository.Save();

        var isExists = await _todoService.IsTodoItemExists(1);

        Assert.True(isExists);

        await _todoService.DeleteTodoItem(user1.Id, 1);

        isExists = await _todoService.IsTodoItemExists(1);

        Assert.False(isExists);
    }

}
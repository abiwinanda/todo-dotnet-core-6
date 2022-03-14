using System;
using TodoApp.Data.Entities;

namespace TodoApp.Core.Services
{
    public interface ITodoService
    {
        // TodoTag
        IEnumerable<TodoTag> GetAllTodoTags();
        Task<TodoTag?> GetTodoTagById(int id);
        List<TodoTag> GetListOfTodoTagByListOfId(List<int> todoTagIds);
        Task<bool> IsTodoTagExists(int id);

        // TodoList
        IEnumerable<TodoList> GetAllUserTodoList(int userId);
        TodoList? GetUserTodoListById(int userId, int id);
        Task<TodoList> CreateTodoList(int userId, string name);
        bool IsUserTodoListExists(int userId, int id);

        // TodoItem
        IEnumerable<TodoItem> GetAllUserTodoItems(int userId);
        TodoItem? GetUserTodoItemById(int userId, int id);
        Task<TodoItem> CreateTodoItem(TodoItem todoItem);
        Task DeleteTodoItem(int userId, int id);
        Task<bool> IsTodoItemExists(int id);
        Task UpdateTodoItem(TodoItem todoItem);
    }
}


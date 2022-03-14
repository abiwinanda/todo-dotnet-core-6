using System;
namespace TodoApp.Data.Entities
{
    public class TodoList
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public List<TodoItem> TodoItems { get; set; } = new List<TodoItem>();

        public User User { get; set; }
        public int UserId { get; set; }
    }
}


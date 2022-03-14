using System;
namespace TodoApp.Data.Entities
{
    public class TodoItem
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;

        public User User { get; set; }
        public int UserId { get; set; }

        public TodoList TodoList { get; set; }
        public int TodoListId { get; set; }

        public List<TodoTag> TodoTags { get; set; } = new List<TodoTag>();
    }
}


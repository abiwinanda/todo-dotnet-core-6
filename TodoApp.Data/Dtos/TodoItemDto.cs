using System;
using TodoApp.Data.Entities;

namespace TodoApp.Data.Dtos
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public string TodoList { get; set; }

        public ICollection<string> TodoTags { get; set; }

        public TodoItemDto(TodoItem todoItem)
        {
            Id = todoItem.Id;
            Name = todoItem.Name;
            Description = todoItem.Description;
            IsCompleted = todoItem.IsCompleted;
            TodoList = todoItem.TodoList.Name;
            TodoTags = todoItem.TodoTags.Select(tt => tt.Name).ToList();
        }
    }
}


using System;
using TodoApp.Data.Entities;

namespace TodoApp.Data.Dtos
{
    public class TodoListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TodoItemDto> TodoItems { get; set; }

        public TodoListDto(TodoList todoList)
        {
            Id = todoList.Id;
            Name = todoList.Name;
            TodoItems = todoList.TodoItems.Select(ti => new TodoItemDto(ti)).AsEnumerable();
        }
    }
}


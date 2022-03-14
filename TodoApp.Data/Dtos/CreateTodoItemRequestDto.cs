using System;
using System.ComponentModel.DataAnnotations;
using TodoApp.Data.Entities;

namespace TodoApp.Data.Dtos
{
    public class CreateTodoItemRequestDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int TodoListId { get; set; }
        public List<int> TodoTagIds { get; set; } = new List<int>();
    }
}


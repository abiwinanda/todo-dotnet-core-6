using System;
using System.ComponentModel.DataAnnotations;
using TodoApp.Data.Entities;

namespace TodoApp.Data.Dtos
{
    public class UpdateTodoItemRequestDto
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TodoListId { get; set; }
        public bool? IsCompleted { get; set; }
        public List<int> TodoTagIds { get; set; } = new List<int>();
    }
}


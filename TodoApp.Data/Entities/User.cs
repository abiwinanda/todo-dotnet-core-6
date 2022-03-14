using System;
namespace TodoApp.Data.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;

		public List<TodoList> TodoLists { get; set; }
		public List<TodoItem> TodoItems { get; set; }
	}
}


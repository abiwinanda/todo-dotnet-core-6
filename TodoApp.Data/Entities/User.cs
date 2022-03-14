using System;
namespace TodoApp.Data.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public byte[] PasswordHash { get; set; }
		public byte[] PasswordSalt { get; set; }

		public List<TodoList> TodoLists { get; set; }
		public List<TodoItem> TodoItems { get; set; }
	}
}


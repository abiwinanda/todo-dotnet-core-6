using System;
namespace TodoApp.Data.Entities
{
	public class TodoTag
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<TodoItem> TodoItems { get; set; }
	}
}


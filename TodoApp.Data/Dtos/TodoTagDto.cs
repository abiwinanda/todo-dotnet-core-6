using System;
using TodoApp.Data.Entities;

namespace TodoApp.Data.Dtos
{
	public class TodoTagDto
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public TodoTagDto(TodoTag todoTag)
		{
			Id = todoTag.Id;
			Name = todoTag.Name;
		}
	}
}


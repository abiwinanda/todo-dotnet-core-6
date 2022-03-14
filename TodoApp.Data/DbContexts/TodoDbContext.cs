using System;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Entities;

namespace TodoApp.Data.DbContexts
{
	public class TodoDbContext : DbContext
	{
		public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// User (1) => TodoList (M)
			modelBuilder.Entity<TodoList>()
				.HasOne(tl => tl.User)
				.WithMany(u => u.TodoLists)
				.HasForeignKey(tl => tl.UserId);

			// User (1) => TodoItem (M)
			modelBuilder.Entity<TodoItem>()
				.HasOne(ti => ti.User)
				.WithMany(u => u.TodoItems)
				.HasForeignKey(ti => ti.UserId);

			// TodoList (1) => TodoItem (M)
			modelBuilder.Entity<TodoItem>()
				.HasOne(ti => ti.TodoList)
				.WithMany(tl => tl.TodoItems)
				.HasForeignKey(ti => ti.TodoListId);

			// TodoItem (M) => TodoTag (M)
			modelBuilder.Entity<TodoItem>()
				.HasMany(ti => ti.TodoTags)
				.WithMany(tt => tt.TodoItems)
				.UsingEntity(j => j.ToTable("TodoItemTags"));

			// Seeding TodoTag
			modelBuilder.Entity<TodoTag>().HasData(
				new TodoTag { Id = 1, Name = "Personal" },
				new TodoTag { Id = 2, Name = "Work" },
				new TodoTag { Id = 3, Name = "Recreation" });
		}

		public DbSet<User> Users { get; set; }
		public DbSet<TodoList> TodoLists { get; set; }
		public DbSet<TodoItem> TodoItems { get; set; }
		public DbSet<TodoTag> TodoTags { get; set; }
	}
}


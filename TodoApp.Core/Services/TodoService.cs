using System;
using TodoApp.Core.Repositories;
using TodoApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Core.Services
{
    public class TodoService : ITodoService
    {
        private readonly IRepository<TodoTag> _todoTagRepository;
        private readonly IRepository<TodoList> _todoListRepository;
        private readonly IRepository<TodoItem> _todoItemRepository;

        public TodoService(
            IRepository<TodoTag> todoTagRepository,
            IRepository<TodoList> todoListRepository,
            IRepository<TodoItem> todoItemRepository
        )
        {
            _todoTagRepository = todoTagRepository;
            _todoListRepository = todoListRepository;
            _todoItemRepository = todoItemRepository;
        }

        public async Task<TodoList> CreateTodoList(int userId, string name)
        {
            TodoList newTodoList = new TodoList()
            {
                Name = name,
                UserId = userId
            };

            await _todoListRepository.Insert(newTodoList);
            await _todoListRepository.Save();

            return newTodoList;
        }

        public IEnumerable<TodoList> GetAllUserTodoList(int userId)
        {
            return _todoListRepository.GetAll()
                .Where(tl => tl.UserId == userId)
                .Include(tl => tl.TodoItems)
                    .ThenInclude(ti => ti.TodoTags)
                .AsEnumerable();
        }

        public IEnumerable<TodoTag> GetAllTodoTags()
        {
            return _todoTagRepository.GetAll().AsEnumerable();
        }

        public TodoList? GetUserTodoListById(int userId, int id)
        {
            return _todoListRepository.GetAll()
                .Where(tl => tl.UserId == userId && tl.Id == id)
                .Include(tl => tl.TodoItems)
                    .ThenInclude(ti => ti.TodoTags)
                .SingleOrDefault();
        }

        public bool IsUserTodoListExists(int userId, int id)
        {
            return _todoListRepository.GetAll().Where(tl => tl.UserId == userId && tl.Id == id).SingleOrDefault() != null;
        }

        public async Task<TodoTag?> GetTodoTagById(int id)
        {
            TodoTag? todoTag = await _todoTagRepository.GetById(id);
            return todoTag;
        }

        public async Task<bool> IsTodoTagExists(int id)
        {
            return (await _todoTagRepository.GetById(id)) != null;
        }

        public IEnumerable<TodoItem> GetAllUserTodoItems(int userId)
        {
            return _todoItemRepository.GetAll()
                .Where(tl => tl.UserId == userId)
                .Include(ti => ti.TodoTags)
                .Include(ti => ti.TodoList)
                .AsEnumerable();
        }

        public TodoItem? GetUserTodoItemById(int userId, int id)
        {
            return _todoItemRepository.GetAll()
                .Where(ti => ti.UserId == userId && ti.Id == id)
                .Include(ti => ti.TodoTags)
                .Include(ti => ti.TodoList)
                .SingleOrDefault();
        }

        public async Task<TodoItem> CreateTodoItem(TodoItem todoItem)
        {
            await _todoItemRepository.Insert(todoItem);
            await _todoItemRepository.Save();

            return todoItem;
        }

        public async Task DeleteTodoItem(int userId, int id)
        {
            TodoItem todoItem;

            try
            {
                todoItem = _todoItemRepository.GetAll()
                    .Where(ti => ti.UserId == userId && ti.Id == id)
                    .Single();
            }
            catch
            {
                return;
            }

            _todoItemRepository.Delete(todoItem);
            await _todoListRepository.Save();
        }

        public List<TodoTag> GetListOfTodoTagByListOfId(List<int> todoTagIds)
        {
            return _todoTagRepository.GetAll()
                .Where(tt => todoTagIds.Contains(tt.Id))
                .ToList();
        }

        public async Task<bool> IsTodoItemExists(int id)
        {
            return (await _todoItemRepository.GetById(id)) != null;
        }

        public async Task UpdateTodoItem(TodoItem todoItem)
        {
            _todoItemRepository.Update(todoItem);
            await _todoItemRepository.Save();
        }
    }
}


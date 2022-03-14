using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.Services;
using TodoApp.Data.Dtos;
using TodoApp.Data.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApp.Api.Controllers
{
    [Route("api/[controller]")]
    public class ItemController : Controller
    {
        private readonly ITodoService _todoService;
        private readonly IUserService _userService;

        public ItemController(ITodoService todoService, IUserService userService)
        {
            _todoService = todoService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListDto>>> Get()
        {
            try
            {
                if (String.IsNullOrEmpty(Request.Headers["X-User-Id"]))
                {
                    return BadRequest("X-User-Id headers must be provided");
                }

                int userId = int.Parse(Request.Headers["X-User-Id"]);
                if (!(await _userService.IsUserExists(userId)))
                {
                    return Unauthorized("Invalid user id");
                }

                return Ok(_todoService
                    .GetAllUserTodoItems(userId)
                    .Select(ti => new TodoItemDto(ti))
                    .ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught", e);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoListDto>> Get(int id)
        {
            try
            {
                if (String.IsNullOrEmpty(Request.Headers["X-User-Id"]))
                {
                    return BadRequest("X-User-Id headers must be provided");
                }

                int userId = int.Parse(Request.Headers["X-User-Id"]);
                if (!(await _userService.IsUserExists(userId)))
                {
                    return Unauthorized("Invalid user id");
                }

                TodoItem? todoItem = _todoService.GetUserTodoItemById(userId, id);
                if (todoItem == null) return NotFound("Item not found");
                return Ok(new TodoItemDto(todoItem));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught", e);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> Create([FromBody] CreateTodoItemRequestDto request)
        {
            try
            {
                if (String.IsNullOrEmpty(Request.Headers["X-User-Id"]))
                {
                    return BadRequest("X-User-Id headers must be provided");
                }

                int userId = int.Parse(Request.Headers["X-User-Id"]);
                if (!(await _userService.IsUserExists(userId)))
                {
                    return Unauthorized("Invalid user id");
                }

                if (!_todoService.IsUserTodoListExists(userId, request.TodoListId))
                {
                    return BadRequest("Invalid list Id");
                }

                foreach (int todoTagId in request.TodoTagIds)
                {
                    if (!(await _todoService.IsTodoTagExists(todoTagId))) return BadRequest("Invalid todo tag Id");
                }

                TodoItem newTodoItem = await _todoService.CreateTodoItem(new TodoItem()
                {
                    Name = request.Name,
                    Description = request.Description,
                    UserId = userId,
                    TodoListId = request.TodoListId,
                    TodoTags = _todoService.GetListOfTodoTagByListOfId(request.TodoTagIds)
                });

                return Ok(new TodoItemDto(newTodoItem));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught", e);
                return StatusCode(500);
            }
        }

        [HttpPut]
        public async Task<ActionResult<TodoItemDto>> Update([FromBody] UpdateTodoItemRequestDto request)
        {
            try
            {
                if (String.IsNullOrEmpty(Request.Headers["X-User-Id"]))
                {
                    return BadRequest("X-User-Id headers must be provided");
                }

                int userId = int.Parse(Request.Headers["X-User-Id"]);
                if (!(await _userService.IsUserExists(userId)))
                {
                    return Unauthorized("Invalid user id");
                }

                if (request.TodoListId.HasValue && !_todoService.IsUserTodoListExists(userId, (int)request.TodoListId))
                {
                    return BadRequest("Invalid list Id");
                }

                foreach (int todoTagId in request.TodoTagIds)
                {
                    if (!(await _todoService.IsTodoTagExists(todoTagId))) return BadRequest("Invalid todo tag Id");
                }

                var todoItem = _todoService.GetUserTodoItemById(userId, request.Id);
                if (todoItem == null)
                {
                    return BadRequest("Item not found");
                }

                todoItem.Name = (!String.IsNullOrEmpty(request.Name)) ? request.Name : todoItem.Name;
                todoItem.Description = (!String.IsNullOrEmpty(request.Description)) ? request.Description : todoItem.Description;
                todoItem.TodoListId = (request.TodoListId.HasValue) ? (int)request.TodoListId : (int)todoItem.TodoListId;
                todoItem.IsCompleted = (request.IsCompleted.HasValue) ? (bool)request.IsCompleted : (bool)todoItem.IsCompleted;
                todoItem.TodoTags = (request.TodoTagIds.Count() > 0) ? _todoService.GetListOfTodoTagByListOfId(request.TodoTagIds) : todoItem.TodoTags;

                await _todoService.UpdateTodoItem(todoItem);

                return Ok(new TodoItemDto(todoItem));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught", e);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItemDto>> Delete(int id)
        {
            try
            {
                if (String.IsNullOrEmpty(Request.Headers["X-User-Id"]))
                {
                    return BadRequest("X-User-Id headers must be provided");
                }

                int userId = int.Parse(Request.Headers["X-User-Id"]);
                if (!(await _userService.IsUserExists(userId)))
                {
                    return Unauthorized("Invalid user id");
                }

                if (!(await _todoService.IsTodoItemExists(id)))
                {
                    return BadRequest("Item not found");
                }

                await _todoService.DeleteTodoItem(userId, id);

                return Ok(_todoService
                    .GetAllUserTodoItems(userId)
                    .Select(ti => new TodoItemDto(ti))
                    .ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught", e);
                return StatusCode(500);
            }
        }
    }
}


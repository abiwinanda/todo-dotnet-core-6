using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.Services;
using TodoApp.Data.Dtos;
using TodoApp.Data.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApp.Api.Controllers
{
    [Route("api/[controller]")]
    public class ListController : Controller
    {
        private readonly ITodoService _todoService;
        private readonly IUserService _userService;

        public ListController(ITodoService todoService, IUserService userService)
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
                    return Unauthorized("X-User-Id headers must be provided");
                }

                int userId = int.Parse(Request.Headers["X-User-Id"]);
                if (!(await _userService.IsUserExists(userId)))
                {
                    return Unauthorized("Invalid user id");
                }

                return Ok(_todoService
                    .GetAllUserTodoList(userId)
                    .Select(tl => new TodoListDto(tl))
                    .AsEnumerable());
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
                    return Unauthorized("X-User-Id headers must be provided");
                }

                int userId = int.Parse(Request.Headers["X-User-Id"]);
                if (!(await _userService.IsUserExists(userId)))
                {
                    return Unauthorized("Invalid user id");
                }

                TodoList? todoList = _todoService.GetUserTodoListById(userId, id);
                if (todoList == null) return NotFound("List not found");
                return Ok(new TodoListDto(todoList));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught", e);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TodoListDto>> Create([FromBody] CreateTodoListRequestDto request)
        {
            try
            {
                if (String.IsNullOrEmpty(Request.Headers["X-User-Id"]))
                {
                    return Unauthorized("X-User-Id headers must be provided");
                }

                int userId = int.Parse(Request.Headers["X-User-Id"]);
                if (!(await _userService.IsUserExists(userId)))
                {
                    return Unauthorized("Invalid user id");
                }

                TodoList newTodoList = await _todoService.CreateTodoList(userId, request.Name);
                return Ok(new TodoListDto(newTodoList));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught", e);
                return StatusCode(500);
            }
        }
    }
}


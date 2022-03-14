using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.Services;
using TodoApp.Data.Dtos;
using TodoApp.Data.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApp.Api.Controllers
{
    [Route("api/[controller]")]
    public class TagController : Controller
    {
        private readonly ITodoService _todoService;

        public TagController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoTagDto>> Get()
        {
            try
            {
                IEnumerable<TodoTag> todoTags = _todoService.GetAllTodoTags();
                var response = todoTags.Select(t => new TodoTagDto(t)).ToList();

                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught", e);
                return StatusCode(500);
            }
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult<TodoTagDto>> Get(int id)
        {
            try
            {
                TodoTag? todoTag = await _todoService.GetTodoTagById(id);

                if (todoTag == null) return NotFound("Tag not found");

                return Ok(new TodoTagDto(todoTag));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught", e);
                return StatusCode(500);
            }
        }
    }
}


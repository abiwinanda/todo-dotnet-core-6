using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.Services;
using TodoApp.Data.Dtos;
using TodoApp.Data.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApp.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequestDto request)
        {
            try
            {
                User newUser = await _userService.RegisterUser(request.Username, request.Password);
                string token = _userService.CreateUserToken(newUser);

                return Ok(new RegisterResponseDto(token));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught", e);
                return StatusCode(500);
            }
        }

        [HttpPost("login")]
        public ActionResult<LoginResponseDto> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                string? token = _userService.Login(request.Username, request.Password);

                if (token == null) return BadRequest("Username or password is incorrect");

                return Ok(new LoginResponseDto(token));
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught", e);
                return StatusCode(500);
            }
        }
    }
}


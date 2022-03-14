using System;
namespace TodoApp.Data.Dtos
{
    public class RegisterResponseDto
    {
        public string Token { get; set; }

        public RegisterResponseDto(string token)
        {
            Token = token;
        }
    }
}


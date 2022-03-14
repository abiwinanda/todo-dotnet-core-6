using System;
namespace TodoApp.Data.Dtos
{
	public class LoginResponseDto
	{
		public string Token { get; set; }

		public LoginResponseDto(string token)
		{
			Token = token;
		}
	}
}


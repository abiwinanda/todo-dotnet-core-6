using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Core.Repositories;
using TodoApp.Data.Entities;

namespace TodoApp.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IRepository<User> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<User> RegisterUser(string username, string password)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            await _userRepository.Insert(new User()
            {
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            });

            await _userRepository.Save();

            User user = _userRepository.GetAll()
                .Where(u => u.Username == username)
                .Single();

            return user;
        }

        public string CreateUserToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public string? Login(string username, string password)
        {
            User user = _userRepository.GetAll()
                .Where(u => u.Username == username)
                .Single();

            if (user == null) return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return CreateUserToken(user);
        }

        public async Task<bool> IsUserExists(int userId)
        {
            return (await _userRepository.GetById(userId)) != null;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}


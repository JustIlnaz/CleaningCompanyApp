using System;
using System.Threading.Tasks;
using CleaningApi.DatabaseContext;
using CleaningApi.Interfaces;
using CleaningApi.Models;
using CleaningApi.Requests;
using CleaningApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CleaningApi.Services
{
    public class UserServices : IUserServices
    {
        private readonly ContextDb _contextDb;
        private readonly IConfiguration _configuration;

        public UserServices(ContextDb contextDb, IConfiguration configuration)
        {
            _contextDb = contextDb;
            _configuration = configuration;
        }

        public async Task<IActionResult> Auth(Auth auth)
        {
            var user = await _contextDb.Users.FirstOrDefaultAsync(x => x.Email == auth.Email && x.Password == auth.Password);

            if (user == null)
            {
                return new NotFoundObjectResult(new AuthResponse { Status = false, Error = "Неверный email или пароль" });
            }

            var token = GenerateJwtToken(user);
            var session = new Session
            {
                Token = token,
                Expires = DateTime.UtcNow.AddHours(24),
                UserId = user.Id_User
            };

            await _contextDb.Sessions.AddAsync(session);
            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new AuthResponse { Status = true, Token = token, User = user });
        }

        public async Task<IActionResult> Register(CreateUser newUser)
        {
            var existingUser = await _contextDb.Users.FirstOrDefaultAsync(x => x.Email == newUser.Email);
            if (existingUser != null)
            {
                return new BadRequestObjectResult(new RegisterResponse { Status = false, Error = "Пользователь уже существует" });
            }

            var user = new User
            {
                Name = newUser.Name,
                Email = newUser.Email,
                Password = newUser.Password,
                Phone = newUser.Phone,
                Role_Id = newUser.Role_Id
            };

            await _contextDb.Users.AddAsync(user);
            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new RegisterResponse { Status = true, User = user });
        }

        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _contextDb.Users.Include(x => x.Role).ToListAsync();

            if (users == null || users.Count == 0)
            {
                return new NotFoundObjectResult(new UsersResponse { Status = false, Error = "Пользователи не найдены" });
            }

            return new OkObjectResult(new UsersResponse { Status = true, Users = users });
        }

        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _contextDb.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id_User == id);

            if (user == null)
            {
                return new NotFoundObjectResult(new UserResponse { Status = false, Error = "Пользователь не найден" });
            }

            return new OkObjectResult(new UserResponse { Status = true, User = user });
        }

        public async Task<IActionResult> UpdateUser(UpdateUser updateUser)
        {
            var user = await _contextDb.Users.FirstOrDefaultAsync(x => x.Id_User == updateUser.Id_User);

            if (user == null)
            {
                return new NotFoundObjectResult(new UserResponse { Status = false, Error = "Пользователь не найден" });
            }

            if (!string.IsNullOrEmpty(updateUser.Name)) user.Name = updateUser.Name;
            if (!string.IsNullOrEmpty(updateUser.Email)) user.Email = updateUser.Email;
            if (!string.IsNullOrEmpty(updateUser.Password)) user.Password = updateUser.Password;
            if (!string.IsNullOrEmpty(updateUser.Phone)) user.Phone = updateUser.Phone;
            if (updateUser.Role_Id.HasValue) user.Role_Id = updateUser.Role_Id.Value;

            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new UserResponse { Status = true, User = user });
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _contextDb.Users.FirstOrDefaultAsync(x => x.Id_User == id);

            if (user == null)
            {
                return new NotFoundObjectResult(new UserResponse { Status = false, Error = "Пользователь не найден" });
            }

            _contextDb.Users.Remove(user);
            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new UserResponse { Status = true, User = user });
        }

        public async Task<IActionResult> GetProfile(string token)
        {
            var session = await _contextDb.Sessions.Include(x => x.User).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.Token == token);

            if (session == null)
            {
                return new NotFoundObjectResult(new ProfileResponse { Status = false, Error = "Сессия не найдена" });
            }

            return new OkObjectResult(new ProfileResponse { Status = true, User = session.User });
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id_User.ToString()),
                new Claim(ClaimTypes.Role, user.Role_Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

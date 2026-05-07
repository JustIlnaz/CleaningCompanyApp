using System;
using System.Linq;
using System.Threading.Tasks;
using CleaningApi.CustomAttributes;
using CleaningApi.Interfaces;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CleaningApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost]
        [Route("Auth")]
        public async Task<IActionResult> Auth([FromBody] Auth auth)
        {
            try
            {
                return await _userServices.Auth(auth);
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, error = ex.Message });
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] CreateUser newUser)
        {
            try
            {
                return await _userServices.Register(newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, error = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        [RoleAuthorize([1, 2])]
        public async Task<IActionResult> GetAllUsers()
        {
            return await _userServices.GetAllUsers();
        }

        [HttpGet]
        [Route("GetUserById")]
        [RoleAuthorize([1, 2])]
        public async Task<IActionResult> GetUserById(int id)
        {
            return await _userServices.GetUserById(id);
        }

        [HttpPut]
        [Route("UpdateUser")]
        [RoleAuthorize([1])]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUser user)
        {
            return await _userServices.UpdateUser(user);
        }

        [HttpDelete]
        [Route("DeleteUser")]
        [RoleAuthorize([1])]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return await _userServices.DeleteUser(id);
        }

        [HttpGet]
        [Route("GetProfile")]
        [RoleAuthorize([1, 2, 3, 4])]
        public async Task<IActionResult> GetProfile()
        {
            string? token = Request.Headers["Authorization"].FirstOrDefault();
            return await _userServices.GetProfile(token);
        }
    }
}

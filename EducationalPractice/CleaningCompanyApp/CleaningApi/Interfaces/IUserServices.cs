using System.Threading.Tasks;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CleaningApi.Interfaces
{
    public interface IUserServices
    {
        Task<IActionResult> Auth(Auth auth);
        Task<IActionResult> Register(CreateUser user);
        Task<IActionResult> GetAllUsers();
        Task<IActionResult> GetUserById(int id);
        Task<IActionResult> UpdateUser(UpdateUser user);
        Task<IActionResult> DeleteUser(int id);
        Task<IActionResult> GetProfile(string token);
    }
}

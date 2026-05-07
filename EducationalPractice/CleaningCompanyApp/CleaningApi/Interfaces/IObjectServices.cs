using System.Threading.Tasks;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CleaningApi.Interfaces
{
    public interface IObjectServices
    {
        Task<IActionResult> GetAllObjects();
        Task<IActionResult> CreateObject(CreateObject newObject);
    }
}

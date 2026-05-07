using System.Threading.Tasks;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CleaningApi.Interfaces
{
    public interface IBrigadeServices
    {
        Task<IActionResult> GetAllBrigades();
        Task<IActionResult> GetBrigadeById(int id);
        Task<IActionResult> CreateBrigade(CreateBrigade brigade);
        Task<IActionResult> UpdateBrigade(UpdateBrigade brigade);
        Task<IActionResult> DeleteBrigade(int id);
        Task<IActionResult> GetBrigadeLoad(int id);
    }
}

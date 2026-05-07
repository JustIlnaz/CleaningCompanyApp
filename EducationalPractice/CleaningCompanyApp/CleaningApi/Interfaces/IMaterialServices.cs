using System.Threading.Tasks;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CleaningApi.Interfaces
{
    public interface IMaterialServices
    {
        Task<IActionResult> GetAllMaterials();
        Task<IActionResult> GetMaterialById(int id);
        Task<IActionResult> CreateMaterial(CreateMaterial material);
        Task<IActionResult> UpdateMaterial(UpdateMaterial material);
        Task<IActionResult> DeleteMaterial(int id);
        Task<IActionResult> GetMaterialsByBrigade(int brigadeId);
    }
}

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
    public class MaterialController : Controller
    {
        private readonly IMaterialServices _materialServices;

        public MaterialController(IMaterialServices materialServices)
        {
            _materialServices = materialServices;
        }

        [HttpGet]
        [Route("GetAllMaterials")]
        [RoleAuthorize([1, 2, 3])]
        public async Task<IActionResult> GetAllMaterials()
        {
            return await _materialServices.GetAllMaterials();
        }

        [HttpGet]
        [Route("GetMaterialById")]
        [RoleAuthorize([1, 2, 3])]
        public async Task<IActionResult> GetMaterialById(int id)
        {
            return await _materialServices.GetMaterialById(id);
        }

        [HttpPost]
        [Route("CreateMaterial")]
        [RoleAuthorize([1, 2])]
        public async Task<IActionResult> CreateMaterial([FromBody] CreateMaterial material)
        {
            return await _materialServices.CreateMaterial(material);
        }

        [HttpPut]
        [Route("UpdateMaterial")]
        [RoleAuthorize([1, 2])]
        public async Task<IActionResult> UpdateMaterial([FromBody] UpdateMaterial material)
        {
            return await _materialServices.UpdateMaterial(material);
        }

        [HttpDelete]
        [Route("DeleteMaterial")]
        [RoleAuthorize([1])]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            return await _materialServices.DeleteMaterial(id);
        }

        [HttpGet]
        [Route("GetMaterialsByBrigade")]
        [RoleAuthorize([1, 2, 3])]
        public async Task<IActionResult> GetMaterialsByBrigade(int brigadeId)
        {
            return await _materialServices.GetMaterialsByBrigade(brigadeId);
        }

        [HttpPost]
        [Route("Request")]
        [RoleAuthorize([3])]
        public async Task<IActionResult> RequestMaterial([FromBody] RequestMaterial request)
        {
            string? token = Request.Headers["Authorization"].FirstOrDefault();
            return await _materialServices.RequestMaterial(request, token);
        }
    }
}

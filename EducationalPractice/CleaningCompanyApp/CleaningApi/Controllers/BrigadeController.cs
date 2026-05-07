using System.Threading.Tasks;
using CleaningApi.CustomAttributes;
using CleaningApi.Interfaces;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CleaningApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrigadeController : Controller
    {
        private readonly IBrigadeServices _brigadeServices;

        public BrigadeController(IBrigadeServices brigadeServices)
        {
            _brigadeServices = brigadeServices;
        }

        [HttpGet]
        [Route("GetAllBrigades")]
        [RoleAuthorize([1, 2])]
        public async Task<IActionResult> GetAllBrigades()
        {
            return await _brigadeServices.GetAllBrigades();
        }

        [HttpGet]
        [Route("GetBrigadeById")]
        [RoleAuthorize([1, 2, 3])]
        public async Task<IActionResult> GetBrigadeById(int id)
        {
            return await _brigadeServices.GetBrigadeById(id);
        }

        [HttpPost]
        [Route("CreateBrigade")]
        [RoleAuthorize([1, 2])]
        public async Task<IActionResult> CreateBrigade([FromBody] CreateBrigade brigade)
        {
            return await _brigadeServices.CreateBrigade(brigade);
        }

        [HttpPut]
        [Route("UpdateBrigade")]
        [RoleAuthorize([1, 2])]
        public async Task<IActionResult> UpdateBrigade([FromBody] UpdateBrigade brigade)
        {
            return await _brigadeServices.UpdateBrigade(brigade);
        }

        [HttpDelete]
        [Route("DeleteBrigade")]
        [RoleAuthorize([1])]
        public async Task<IActionResult> DeleteBrigade(int id)
        {
            return await _brigadeServices.DeleteBrigade(id);
        }

        [HttpGet]
        [Route("GetBrigadeLoad")]
        [RoleAuthorize([1, 2, 3])]
        public async Task<IActionResult> GetBrigadeLoad(int id)
        {
            return await _brigadeServices.GetBrigadeLoad(id);
        }
    }
}

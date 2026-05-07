using System.Threading.Tasks;
using CleaningApi.CustomAttributes;
using CleaningApi.Interfaces;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CleaningApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObjectController : Controller
    {
        private readonly IObjectServices _objectServices;

        public ObjectController(IObjectServices objectServices)
        {
            _objectServices = objectServices;
        }

        [HttpGet]
        [Route("GetAllObjects")]
        [RoleAuthorize([1, 2])]
        public async Task<IActionResult> GetAllObjects()
        {
            return await _objectServices.GetAllObjects();
        }

        [HttpPost]
        [Route("CreateObject")]
        [RoleAuthorize([1, 2, 4])]
        public async Task<IActionResult> CreateObject([FromBody] CreateObject newObject)
        {
            return await _objectServices.CreateObject(newObject);
        }
    }
}

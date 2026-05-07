using System.Threading.Tasks;
using CleaningApi.DatabaseContext;
using CleaningApi.Interfaces;
using CleaningApi.Models;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleaningApi.Services
{
    public class ObjectServices : IObjectServices
    {
        private readonly ContextDb _contextDb;

        public ObjectServices(ContextDb contextDb)
        {
            _contextDb = contextDb;
        }

        public async Task<IActionResult> GetAllObjects()
        {
            var objects = await _contextDb.CleaningObjects.ToListAsync();
            return new OkObjectResult(new { status = true, objects });
        }

        public async Task<IActionResult> CreateObject(CreateObject newObject)
        {
            var cleaningObject = new CleaningObject
            {
                Address = newObject.Address,
                Type = newObject.Type,
                Area = newObject.Area,
                ClientName = newObject.ClientName ?? string.Empty,
                ClientPhone = newObject.ClientPhone ?? string.Empty
            };

            await _contextDb.CleaningObjects.AddAsync(cleaningObject);
            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, cleaningObject });
        }
    }
}

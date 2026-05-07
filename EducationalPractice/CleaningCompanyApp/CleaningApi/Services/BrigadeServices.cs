using System.Linq;
using System.Threading.Tasks;
using CleaningApi.DatabaseContext;
using CleaningApi.Interfaces;
using CleaningApi.Models;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleaningApi.Services
{
    public class BrigadeServices : IBrigadeServices
    {
        private readonly ContextDb _contextDb;

        public BrigadeServices(ContextDb contextDb)
        {
            _contextDb = contextDb;
        }

        public async Task<IActionResult> GetAllBrigades()
        {
            var brigades = await _contextDb.Brigades.Include(x => x.Brigadier).ToListAsync();

            if (brigades == null || brigades.Count == 0)
            {
                return new NotFoundObjectResult(new { error = "Бригады не найдены" });
            }

            return new OkObjectResult(new { status = true, brigades });
        }

        public async Task<IActionResult> GetBrigadeById(int id)
        {
            var brigade = await _contextDb.Brigades.Include(x => x.Brigadier).FirstOrDefaultAsync(x => x.Id_Brigade == id);

            if (brigade == null)
            {
                return new NotFoundObjectResult(new { error = "Бригада не найдена" });
            }

            return new OkObjectResult(new { status = true, brigade });
        }

        public async Task<IActionResult> CreateBrigade(CreateBrigade newBrigade)
        {
            var brigade = new Brigade
            {
                Name = newBrigade.Name,
                Rating = newBrigade.Rating,
                BrigadierId = newBrigade.BrigadierId
            };

            await _contextDb.Brigades.AddAsync(brigade);
            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, brigade });
        }

        public async Task<IActionResult> UpdateBrigade(UpdateBrigade updateBrigade)
        {
            var brigade = await _contextDb.Brigades.FirstOrDefaultAsync(x => x.Id_Brigade == updateBrigade.Id_Brigade);

            if (brigade == null)
            {
                return new NotFoundObjectResult(new { error = "Бригада не найдена" });
            }

            if (!string.IsNullOrEmpty(updateBrigade.Name)) brigade.Name = updateBrigade.Name;
            if (updateBrigade.Rating.HasValue) brigade.Rating = updateBrigade.Rating.Value;
            if (updateBrigade.BrigadierId.HasValue) brigade.BrigadierId = updateBrigade.BrigadierId;

            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, brigade });
        }

        public async Task<IActionResult> DeleteBrigade(int id)
        {
            var brigade = await _contextDb.Brigades.FirstOrDefaultAsync(x => x.Id_Brigade == id);

            if (brigade == null)
            {
                return new NotFoundObjectResult(new { error = "Бригада не найдена" });
            }

            _contextDb.Brigades.Remove(brigade);
            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, brigade });
        }

        public async Task<IActionResult> GetBrigadeLoad(int id)
        {
            var activeOrders = await _contextDb.Orders
                .Where(x => x.BrigadeId == id && x.Status != "Завершен")
                .CountAsync();

            return new OkObjectResult(new { status = true, brigadeId = id, activeOrders });
        }
    }
}

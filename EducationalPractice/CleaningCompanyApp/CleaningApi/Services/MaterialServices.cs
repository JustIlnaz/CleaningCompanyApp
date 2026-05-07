using System.Linq;
using System.Threading.Tasks;
using CleaningApi.DatabaseContext;
using CleaningApi.Interfaces;
using CleaningApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleaningApi.Services
{
    public class MaterialServices : IMaterialServices
    {
        private readonly ContextDb _contextDb;

        public MaterialServices(ContextDb contextDb)
        {
            _contextDb = contextDb;
        }

        public async Task<IActionResult> GetAllMaterials()
        {
            var materials = await _contextDb.Materials.Include(x => x.Brigade).ToListAsync();

            if (materials == null || materials.Count == 0)
            {
                return new NotFoundObjectResult(new { error = "Материалы не найдены" });
            }

            return new OkObjectResult(new { status = true, materials });
        }

        public async Task<IActionResult> GetMaterialById(int id)
        {
            var material = await _contextDb.Materials.Include(x => x.Brigade).FirstOrDefaultAsync(x => x.Id_Material == id);

            if (material == null)
            {
                return new NotFoundObjectResult(new { error = "Материал не найден" });
            }

            return new OkObjectResult(new { status = true, material });
        }

        public async Task<IActionResult> CreateMaterial(CreateMaterial newMaterial)
        {
            var material = new Models.Material
            {
                Name = newMaterial.Name,
                Unit = newMaterial.Unit,
                Quantity = newMaterial.Quantity,
                MinQuantity = newMaterial.MinQuantity,
                BrigadeId = newMaterial.BrigadeId
            };

            await _contextDb.Materials.AddAsync(material);
            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, material });
        }

        public async Task<IActionResult> UpdateMaterial(UpdateMaterial updateMaterial)
        {
            var material = await _contextDb.Materials.FirstOrDefaultAsync(x => x.Id_Material == updateMaterial.Id_Material);

            if (material == null)
            {
                return new NotFoundObjectResult(new { error = "Материал не найден" });
            }

            if (!string.IsNullOrEmpty(updateMaterial.Name)) material.Name = updateMaterial.Name;
            if (!string.IsNullOrEmpty(updateMaterial.Unit)) material.Unit = updateMaterial.Unit;
            if (updateMaterial.Quantity.HasValue) material.Quantity = updateMaterial.Quantity.Value;
            if (updateMaterial.MinQuantity.HasValue) material.MinQuantity = updateMaterial.MinQuantity.Value;
            if (updateMaterial.BrigadeId.HasValue) material.BrigadeId = updateMaterial.BrigadeId;

            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, material });
        }

        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _contextDb.Materials.FirstOrDefaultAsync(x => x.Id_Material == id);

            if (material == null)
            {
                return new NotFoundObjectResult(new { error = "Материал не найден" });
            }

            _contextDb.Materials.Remove(material);
            await _contextDb.SaveChangesAsync();

            return new OkObjectResult(new { status = true, material });
        }

        public async Task<IActionResult> GetMaterialsByBrigade(int brigadeId)
        {
            var materials = await _contextDb.Materials
                .Where(x => x.BrigadeId == brigadeId || x.BrigadeId == null)
                .ToListAsync();

            return new OkObjectResult(new { status = true, materials });
        }
    }
}

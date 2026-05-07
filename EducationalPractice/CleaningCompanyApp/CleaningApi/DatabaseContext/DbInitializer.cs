using CleaningApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CleaningApi.DatabaseContext
{
    public static class DbInitializer
    {
        public static void Initialize(ContextDb context)
        {
            context.Database.EnsureCreated();

            if (!context.Roles.Any())
            {
                var roles = new Role[]
                {
                    new Role { Id_Role = 1, Name = "Administrator" },
                    new Role { Id_Role = 2, Name = "Dispatcher" },
                    new Role { Id_Role = 3, Name = "Brigadier" },
                    new Role { Id_Role = 4, Name = "Client" }
                };

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            if (!context.Brigades.Any())
            {
                var brigade = new Brigade
                {
                    Name = "Бригада №1",
                    Rating = 5,
                    BrigadierId = null
                };
                context.Brigades.Add(brigade);
                context.SaveChanges();
            }

            if (!context.Materials.Any())
            {
                var brigade = context.Brigades.FirstOrDefault();
                var materials = new Material[]
                {
                    new Material { Name = "Универсальное чистящее средство", Unit = "л", Quantity = 50, MinQuantity = 5, BrigadeId = brigade?.Id_Brigade },
                    new Material { Name = "Средство для стёкол", Unit = "л", Quantity = 30, MinQuantity = 5, BrigadeId = brigade?.Id_Brigade },
                    new Material { Name = "Перчатки одноразовые", Unit = "пар", Quantity = 200, MinQuantity = 20, BrigadeId = null },
                    new Material { Name = "Микрофибра", Unit = "шт", Quantity = 100, MinQuantity = 10, BrigadeId = null }
                };
                context.Materials.AddRange(materials);
                context.SaveChanges();
            }
        }
    }
}

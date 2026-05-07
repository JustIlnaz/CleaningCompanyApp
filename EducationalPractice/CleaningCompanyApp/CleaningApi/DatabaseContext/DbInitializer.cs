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

            if (context.Roles.Any())
            {
                return;
            }

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
    }
}

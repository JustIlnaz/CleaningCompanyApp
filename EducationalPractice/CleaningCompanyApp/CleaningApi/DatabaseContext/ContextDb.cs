using CleaningApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CleaningApi.DatabaseContext
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Brigade> Brigades { get; set; }
        public DbSet<CleaningObject> CleaningObjects { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<Act> Acts { get; set; }
        public DbSet<Session> Sessions { get; set; }
    }
}

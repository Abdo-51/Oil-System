using Microsoft.EntityFrameworkCore;
using Oil_System.Models;

namespace Oil_System.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }


        public virtual DbSet<Product> OilBottles { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }
    }
}

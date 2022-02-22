using ASPNETLIVE.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNETLIVE.Data
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; } = null!;
        public DbSet<Product> Product { get; set; } = null!;
    }

}

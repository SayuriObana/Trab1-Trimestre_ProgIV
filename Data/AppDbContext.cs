using Microsoft.EntityFrameworkCore;
using NotasApi.Models;

namespace NotasApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Nota> Notas => Set<Nota>();
    }
}

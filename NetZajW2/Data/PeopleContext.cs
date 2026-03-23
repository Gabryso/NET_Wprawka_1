using Microsoft.EntityFrameworkCore;
using NetZajW2.Models;

namespace NetZajW2.Data
{
    public class HeroesContext : DbContext
    {
        public HeroesContext(DbContextOptions options) : base(options) { }
        public DbSet<Hero> Hero { get; set; }
    }
}

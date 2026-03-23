using Microsoft.EntityFrameworkCore;
using WA1.Models;


namespace WA1.Data
{
    public class HeroesContext : DbContext
    {
        public HeroesContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Hero> Hero { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Movie> Movie { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hero>()
            .HasMany(e => e.Movies)
            .WithMany(e => e.Heros);
        }
    }
}
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
        public DbSet<Category> Category { get; set; }
        public DbSet<Mission> Mission { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hero>()
            .HasMany(e => e.Movies)
            .WithMany(e => e.Heros);
            modelBuilder.Entity<Hero>()
            .HasMany(e => e.Missions)
            .WithMany(e => e.Heroes);
        }
    }
}
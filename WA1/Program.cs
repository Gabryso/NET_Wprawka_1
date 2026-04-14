using Microsoft.EntityFrameworkCore;
using WA1.Data;

namespace WA1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<HeroesContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SuperHeroesDB")));
            builder.Services.AddMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            //Halo halo londyn?
            // Seedowanie przyk³adowych danych przy starcie
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<WA1.Data.HeroesContext>();

                // Zespo³y (10)
                var teams = new[]
                {
        "Avengers", "Justice League", "X-Men", "Fantastic Four", "Guardians of the Galaxy",
        "Suicide Squad", "Teen Titans", "The Seven", "The Boys", "Legion of Super-Heroes"
    };
                foreach (var name in teams)
                    if (!context.Team.Any(t => t.Name == name))
                        context.Team.Add(new WA1.Models.Team { Name = name });

                // Kategorie (3)
                var categories = new[] { "Human", "Alien", "Mutant" };
                foreach (var name in categories)
                    if (!context.Category.Any(c => c.Name == name))
                        context.Category.Add(new WA1.Models.Category { Name = name });

                // Filmy (5)
                var movies = new[] { "Endgame", "Infinity War", "Civil War", "Age of Ultron", "Dark Phoenix" };
                foreach (var title in movies)
                    if (!context.Movie.Any(m => m.Title == title))
                        context.Movie.Add(new WA1.Models.Movie { Title = title });

                // Misje (5)
                var missions = new[] { "Save the world", "Defeat Thanos", "Rescue the president", "Stop the bomb", "Find the crystal" };
                foreach (var name in missions)
                    if (!context.Mission.Any(m => m.Name == name))
                        context.Mission.Add(new WA1.Models.Mission { Name = name, StartDate = DateTime.Now, Location = "Unknown" });

                context.SaveChanges(); // zapisz wszystkie zmiany
            }
            //Tu londyn!

            app.Run();
        }
    }
}

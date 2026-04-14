using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WA1.Data;
using WA1.Models;

namespace WA1.Controllers
{
    public class HeroesController : Controller
    {
        private readonly HeroesContext _context;

        public HeroesController(HeroesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchName, int? teamId, int? categoryId)
        {
            var heroes = _context.Hero
                .Include(h => h.Team)
                .Include(h => h.Category)
                .Include(h => h.Movies)
                .Include(h => h.Missions)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
                heroes = heroes.Where(h => h.Name.Contains(searchName));
            if (teamId.HasValue)
                heroes = heroes.Where(h => h.TeamId == teamId);
            if (categoryId.HasValue)
                heroes = heroes.Where(h => h.CategoryId == categoryId);

            ViewBag.Teams = new SelectList(await _context.Team.ToListAsync(), "Id", "Name");
            ViewBag.Categories = new SelectList(await _context.Category.ToListAsync(), "Id", "Name");

            return View(await heroes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var hero = await _context.Hero
                .Include(h => h.Team)
                .Include(h => h.Category)
                .Include(h => h.Movies)
                .Include(h => h.Missions)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (hero == null) return NotFound();
            return View(hero);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.TeamId = new SelectList(await _context.Team.ToListAsync(), "Id", "Name");
            ViewBag.CategoryId = new SelectList(await _context.Category.ToListAsync(), "Id", "Name");
            ViewBag.MovieIds = new MultiSelectList(await _context.Movie.ToListAsync(), "Id", "Title");
            ViewBag.MissionIds = new MultiSelectList(await _context.Mission.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Bio,FirstName,LastName,TeamId,CategoryId")] Hero hero, int[] selectedMovies, int[] selectedMissions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hero);
                await _context.SaveChangesAsync();

                if (selectedMovies != null)
                {
                    foreach (var movieId in selectedMovies)
                    {
                        var movie = await _context.Movie.FindAsync(movieId);
                        if (movie != null) hero.Movies.Add(movie);
                    }
                }
                if (selectedMissions != null)
                {
                    foreach (var missionId in selectedMissions)
                    {
                        var mission = await _context.Mission.FindAsync(missionId);
                        if (mission != null) hero.Missions.Add(mission);
                    }
                }
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("Data", JsonConvert.SerializeObject(hero));

                return RedirectToAction(nameof(SavedInSession));
            }

            ViewBag.TeamId = new SelectList(await _context.Team.ToListAsync(), "Id", "Name", hero.TeamId);
            ViewBag.CategoryId = new SelectList(await _context.Category.ToListAsync(), "Id", "Name", hero.CategoryId);
            ViewBag.MovieIds = new MultiSelectList(await _context.Movie.ToListAsync(), "Id", "Title", selectedMovies);
            ViewBag.MissionIds = new MultiSelectList(await _context.Mission.ToListAsync(), "Id", "Name", selectedMissions);
            return View(hero);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var hero = await _context.Hero
                .Include(h => h.Movies)
                .Include(h => h.Missions)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (hero == null) return NotFound();

            ViewBag.TeamId = new SelectList(await _context.Team.ToListAsync(), "Id", "Name", hero.TeamId);
            ViewBag.CategoryId = new SelectList(await _context.Category.ToListAsync(), "Id", "Name", hero.CategoryId);
            ViewBag.MovieIds = new MultiSelectList(await _context.Movie.ToListAsync(), "Id", "Title", hero.Movies.Select(m => m.Id));
            ViewBag.MissionIds = new MultiSelectList(await _context.Mission.ToListAsync(), "Id", "Name", hero.Missions.Select(m => m.Id));
            return View(hero);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Bio,FirstName,LastName,TeamId,CategoryId")] Hero hero, int[] selectedMovies, int[] selectedMissions)
        {
            if (id != hero.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var heroToUpdate = await _context.Hero
                        .Include(h => h.Movies)
                        .Include(h => h.Missions)
                        .FirstOrDefaultAsync(h => h.Id == id);

                    if (heroToUpdate == null) return NotFound();

                    heroToUpdate.Name = hero.Name;
                    heroToUpdate.Bio = hero.Bio;
                    heroToUpdate.FirstName = hero.FirstName;
                    heroToUpdate.LastName = hero.LastName;
                    heroToUpdate.TeamId = hero.TeamId;
                    heroToUpdate.CategoryId = hero.CategoryId;

                    heroToUpdate.Movies.Clear();
                    if (selectedMovies != null)
                    {
                        foreach (var movieId in selectedMovies)
                        {
                            var movie = await _context.Movie.FindAsync(movieId);
                            if (movie != null) heroToUpdate.Movies.Add(movie);
                        }
                    }

                    heroToUpdate.Missions.Clear();
                    if (selectedMissions != null)
                    {
                        foreach (var missionId in selectedMissions)
                        {
                            var mission = await _context.Mission.FindAsync(missionId);
                            if (mission != null) heroToUpdate.Missions.Add(mission);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeroExists(hero.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TeamId = new SelectList(await _context.Team.ToListAsync(), "Id", "Name", hero.TeamId);
            ViewBag.CategoryId = new SelectList(await _context.Category.ToListAsync(), "Id", "Name", hero.CategoryId);
            ViewBag.MovieIds = new MultiSelectList(await _context.Movie.ToListAsync(), "Id", "Title", selectedMovies);
            ViewBag.MissionIds = new MultiSelectList(await _context.Mission.ToListAsync(), "Id", "Name", selectedMissions);
            return View(hero);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var hero = await _context.Hero
                .Include(h => h.Team)
                .Include(h => h.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hero == null) return NotFound();

            return View(hero);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hero = await _context.Hero.FindAsync(id);
            if (hero != null)
            {
                _context.Hero.Remove(hero);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult SavedInSession()
        {
            var data = HttpContext.Session.GetString("Data");
            if (data != null)
                ViewBag.Hero = JsonConvert.DeserializeObject<Hero>(data);
            return View();
        }

        private bool HeroExists(int id)
        {
            return _context.Hero.Any(e => e.Id == id);
        }
    }
}

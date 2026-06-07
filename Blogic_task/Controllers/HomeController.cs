using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blogic_task.Data;
using Blogic_task.Models;
using System.Diagnostics;

namespace Blogic_task.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var smlouvy = _context.Smlouvy
                .Include(s => s.Klient)
                .Include(s => s.Spravce)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                smlouvy = smlouvy.Where(s => s.EvidencniCislo.Contains(searchString)
                                          || s.Klient.Prijmeni.Contains(searchString));
            }

            return View(await smlouvy.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
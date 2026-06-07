using Blogic_task.Data;
using Blogic_task.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogic_task.Controllers
{
    public class PoradciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PoradciController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var poradci = _context.Poradci.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                poradci = poradci.Where(p => p.Prijmeni.Contains(searchString)
                                          || p.Email.Contains(searchString));
            }

            return View(await poradci.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var poradce = await _context.Poradci
                .Include(p => p.SpravovaneSmlouvy)
                .Include(p => p.DalsiSmlouvy)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (poradce == null) return NotFound();

            return View(poradce);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create([Bind("Jmeno,Prijmeni,Email,Telefon,RodneCislo,DatumNarozeni")] Poradce poradce)
        {
            if (ModelState.IsValid)
            {
                _context.Add(poradce);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(poradce);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var poradce = await _context.Poradci
                .FirstOrDefaultAsync(m => m.Id == id);

            if (poradce == null) return NotFound();

            return View(poradce);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var poradce = await _context.Poradci.FindAsync(id);
            if (poradce != null)
            {
                _context.Poradci.Remove(poradce);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var klient = await _context.Klienti.FindAsync(id);
            if (klient == null) return NotFound();

            return View(klient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Jmeno,Prijmeni,Email,Telefon,RodneCislo,DatumNarozeni")] Poradce poradce)
        {
            if (id != poradce.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(poradce);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Klienti.Any(e => e.Id == poradce.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(poradce);
        }


        public async Task<IActionResult> ExportToCsv()
        {
            var poradci = await _context.Poradci.ToListAsync();
            var builder = new System.Text.StringBuilder();

            builder.AppendLine("Id;Jmeno;Prijmeni;Email;Telefon;DatumNarozeni");

            foreach (var p in poradci)
            {
                builder.AppendLine($"{p.Id};{p.Jmeno};{p.Prijmeni};{p.Email};{p.Telefon};{p.DatumNarozeni.ToShortDateString()}");
            }

            byte[] preamble = System.Text.Encoding.UTF8.GetPreamble();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
            byte[] fileBytes = preamble.Concat(data).ToArray();

            return File(fileBytes, "text/csv", "Poradci_Export.csv");
        }
    }
}
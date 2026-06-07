using Blogic_task.Data;
using Blogic_task.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogic_task.Controllers
{
    public class KlientiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KlientiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var klienti = _context.Klienti.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                klienti = klienti.Where(k => k.Prijmeni.Contains(searchString)
                                          || k.Email.Contains(searchString));
            }

            return View(await klienti.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Jmeno,Prijmeni,Email,Telefon,RodneCislo,DatumNarozeni")] Klient klient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(klient);
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index)); 
            }
            return View(klient); 
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var klient = await _context.Klienti
                .FirstOrDefaultAsync(m => m.Id == id);

            if (klient == null) return NotFound();

            return View(klient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var klient = await _context.Klienti.FindAsync(id);
            if (klient != null)
            {
                _context.Klienti.Remove(klient);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Jmeno,Prijmeni,Email,Telefon,RodneCislo,DatumNarozeni")] Klient klient)
        {
            if (id != klient.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(klient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Klienti.Any(e => e.Id == klient.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(klient);
        }

        public async Task<IActionResult> ExportToCsv()
        {
            var klienti = await _context.Klienti.ToListAsync();
            var builder = new System.Text.StringBuilder();

            builder.AppendLine("Id;Jmeno;Prijmeni;Email;Telefon;RodneCislo;DatumNarozeni");

            foreach (var k in klienti)
            {
                builder.AppendLine($"{k.Id};{k.Jmeno};{k.Prijmeni};{k.Email};{k.Telefon};{k.RodneCislo};{k.DatumNarozeni.ToShortDateString()}");
            }

            byte[] preamble = System.Text.Encoding.UTF8.GetPreamble();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
            byte[] fileBytes = preamble.Concat(data).ToArray();

            return File(fileBytes, "text/csv", "Klienti_Export.csv");
        }
    }
}
using Blogic_task.Data;
using Blogic_task.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Blogic_task.Controllers
{
    public class SmlouvyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SmlouvyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var smlouvy = await _context.Smlouvy
                .Include(s => s.Klient)
                .Include(s => s.Spravce)
                .ToListAsync();
            return View(smlouvy);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var smlouva = await _context.Smlouvy
                .Include(s => s.Klient)
                .Include(s => s.Spravce)
                .Include(s => s.DalsiPoradci)
                .FirstOrDefaultAsync(m => m.EvidencniCislo == id);

            if (smlouva == null) return NotFound();

            return View(smlouva);
        }


        public IActionResult Create()
        {
            ViewData["KlientId"] = new SelectList(_context.Klienti, "Id", "Prijmeni");
            ViewData["SpravceId"] = new SelectList(_context.Poradci, "Id", "Prijmeni");

            ViewData["DalsiPoradciId"] = new SelectList(_context.Poradci, "Id", "Prijmeni");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EvidencniCislo,Instituce,DatumUzavreni,DatumPlatnosti,DatumUkonceni,KlientId,SpravceId")] Smlouva smlouva, int[] vybraniDalsiPoradci)
        {
            ModelState.Remove("Klient");
            ModelState.Remove("Spravce");
            ModelState.Remove("DalsiPoradci"); 

            if (ModelState.IsValid)
            {
                if (vybraniDalsiPoradci != null && vybraniDalsiPoradci.Length > 0)
                {
                    smlouva.DalsiPoradci = await _context.Poradci
                        .Where(p => vybraniDalsiPoradci.Contains(p.Id))
                        .ToListAsync();
                }

                _context.Add(smlouva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["KlientId"] = new SelectList(_context.Klienti, "Id", "Prijmeni", smlouva.KlientId);
            ViewData["SpravceId"] = new SelectList(_context.Poradci, "Id", "Prijmeni", smlouva.SpravceId);
            ViewData["DalsiPoradciId"] = new SelectList(_context.Poradci, "Id", "Prijmeni");
            return View(smlouva);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var smlouva = await _context.Smlouvy
                .Include(s => s.Klient)
                .Include(s => s.Spravce)
                .FirstOrDefaultAsync(m => m.EvidencniCislo == id);

            if (smlouva == null) return NotFound();

            return View(smlouva);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var smlouva = await _context.Smlouvy.FindAsync(id);
            if (smlouva != null)
            {
                _context.Smlouvy.Remove(smlouva);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var smlouva = await _context.Smlouvy.FindAsync(id);
            if (smlouva == null) return NotFound();

            ViewData["KlientId"] = new SelectList(_context.Klienti, "Id", "Prijmeni", smlouva.KlientId);
            ViewData["SpravceId"] = new SelectList(_context.Poradci, "Id", "Prijmeni", smlouva.SpravceId);
            return View(smlouva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EvidencniCislo,Instituce,DatumUzavreni,DatumPlatnosti,DatumUkonceni,KlientId,SpravceId")] Smlouva smlouva)
        {
            if (id != smlouva.EvidencniCislo) return NotFound();

            ModelState.Remove("Klient");
            ModelState.Remove("Spravce");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(smlouva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Smlouvy.Any(e => e.EvidencniCislo == smlouva.EvidencniCislo))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["KlientId"] = new SelectList(_context.Klienti, "Id", "Prijmeni", smlouva.KlientId);
            ViewData["SpravceId"] = new SelectList(_context.Poradci, "Id", "Prijmeni", smlouva.SpravceId);
            return View(smlouva);
        }

        public async Task<IActionResult> ExportToCsv()
        {
            var smlouvy = await _context.Smlouvy
                .Include(s => s.Klient)
                .Include(s => s.Spravce)
                .ToListAsync();

            var builder = new System.Text.StringBuilder();
            builder.AppendLine("EvidencniCislo;Instituce;Zakaznik;Spravce;DatumUzavreni;DatumPlatnosti;DatumUkonceni");

            foreach (var s in smlouvy)
            {
                var datumUkonceni = s.DatumUkonceni?.ToShortDateString() ?? "";
                builder.AppendLine($"{s.EvidencniCislo};{s.Instituce};{s.Klient.Prijmeni};{s.Spravce.Prijmeni};{s.DatumUzavreni.ToShortDateString()};{s.DatumPlatnosti.ToShortDateString()};{datumUkonceni}");
            }

            byte[] preamble = System.Text.Encoding.UTF8.GetPreamble();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
            byte[] fileBytes = preamble.Concat(data).ToArray();

            return File(fileBytes, "text/csv", "Smlouvy_Export.csv");
        }
    }
}
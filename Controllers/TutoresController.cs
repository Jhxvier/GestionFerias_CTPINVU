using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionFerias_CTPINVU.Data;
using GestionFerias_CTPINVU.Models;

namespace GestionFerias_CTPINVU.Controllers
{
    public class TutoresController : Controller
    {
        private readonly AppDbContext _context;

        public TutoresController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Tutores.Include(t => t.Tutor);
            return View(await appDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            return RedirectToAction("Perfil", "Usuarios");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tutore tutore)
        {
            return RedirectToAction("Perfil", "Usuarios");
        }

        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return RedirectToAction("Perfil", "Usuarios");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, Tutore tutore)
        {
            return RedirectToAction("Perfil", "Usuarios");
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutore = await _context.Tutores
                .Include(t => t.Tutor)
                .FirstOrDefaultAsync(m => m.TutorId == id);

            if (tutore == null)
            {
                return NotFound();
            }

            return View(tutore);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutore = await _context.Tutores
                .Include(t => t.Tutor)
                .FirstOrDefaultAsync(m => m.TutorId == id);

            if (tutore == null)
            {
                return NotFound();
            }

            return View(tutore);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var tutore = await _context.Tutores.FindAsync(id);
            if (tutore != null)
            {
                _context.Tutores.Remove(tutore);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutoreExists(long id)
        {
            return _context.Tutores.Any(e => e.TutorId == id);
        }
    }
}
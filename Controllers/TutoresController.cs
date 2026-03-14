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

        // GET: Tutores/Create
        public IActionResult Create()
        {
            return RedirectToAction("Perfil", "Usuarios", new { modo = "create", rol = "tutor" });
        }

        // POST: Tutores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("TutorId,Especialidad,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Tutore tutore)
        {
            return RedirectToAction("Perfil", "Usuarios", new { modo = "create", rol = "tutor" });
        }

        // GET: Tutores/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return RedirectToAction("Perfil", "Usuarios", new { id, modo = "edit", rol = "tutor" });
        }

        // POST: Tutores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("TutorId,Especialidad,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Tutore tutore)
        {
            return RedirectToAction("Perfil", "Usuarios", new { id, modo = "edit", rol = "tutor" });
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
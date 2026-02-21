using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // GET: Tutores
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Tutores.Include(t => t.Usuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Tutores/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutore = await _context.Tutores
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (tutore == null)
            {
                return NotFound();
            }

            return View(tutore);
        }

        // GET: Tutores/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            return View();
        }

        // POST: Tutores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,Especialidad,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Tutore tutore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tutore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", tutore.UsuarioId);
            return View(tutore);
        }

        // GET: Tutores/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutore = await _context.Tutores.FindAsync(id);
            if (tutore == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", tutore.UsuarioId);
            return View(tutore);
        }

        // POST: Tutores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("UsuarioId,Especialidad,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Tutore tutore)
        {
            if (id != tutore.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tutore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutoreExists(tutore.UsuarioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", tutore.UsuarioId);
            return View(tutore);
        }

        // GET: Tutores/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutore = await _context.Tutores
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (tutore == null)
            {
                return NotFound();
            }

            return View(tutore);
        }

        // POST: Tutores/Delete/5
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
            return _context.Tutores.Any(e => e.UsuarioId == id);
        }
    }
}

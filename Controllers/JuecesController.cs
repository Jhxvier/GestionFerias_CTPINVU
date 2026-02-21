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
    public class JuecesController : Controller
    {
        private readonly AppDbContext _context;

        public JuecesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Jueces
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Jueces.Include(j => j.Usuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Jueces/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var juece = await _context.Jueces
                .Include(j => j.Usuario)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (juece == null)
            {
                return NotFound();
            }

            return View(juece);
        }

        // GET: Jueces/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            return View();
        }

        // POST: Jueces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Juece juece)
        {
            if (ModelState.IsValid)
            {
                _context.Add(juece);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", juece.UsuarioId);
            return View(juece);
        }

        // GET: Jueces/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var juece = await _context.Jueces.FindAsync(id);
            if (juece == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", juece.UsuarioId);
            return View(juece);
        }

        // POST: Jueces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("UsuarioId,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Juece juece)
        {
            if (id != juece.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(juece);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JueceExists(juece.UsuarioId))
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", juece.UsuarioId);
            return View(juece);
        }

        // GET: Jueces/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var juece = await _context.Jueces
                .Include(j => j.Usuario)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (juece == null)
            {
                return NotFound();
            }

            return View(juece);
        }

        // POST: Jueces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var juece = await _context.Jueces.FindAsync(id);
            if (juece != null)
            {
                _context.Jueces.Remove(juece);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JueceExists(long id)
        {
            return _context.Jueces.Any(e => e.UsuarioId == id);
        }
    }
}

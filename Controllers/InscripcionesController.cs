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
    public class InscripcionesController : Controller
    {
        private readonly AppDbContext _context;

        public InscripcionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Inscripciones
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Inscripciones.Include(i => i.Evento).Include(i => i.LiderUsuario).Include(i => i.Subcategoria).Include(i => i.TutorUsuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Inscripciones/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcione = await _context.Inscripciones
                .Include(i => i.Evento)
                .Include(i => i.LiderUsuario)
                .Include(i => i.Subcategoria)
                .Include(i => i.TutorUsuario)
                .FirstOrDefaultAsync(m => m.InscripcionId == id);
            if (inscripcione == null)
            {
                return NotFound();
            }

            return View(inscripcione);
        }

        // GET: Inscripciones/Create
        public IActionResult Create()
        {
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId");
            ViewData["LiderUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            ViewData["SubcategoriaId"] = new SelectList(_context.Subcategorias, "SubcategoriaId", "SubcategoriaId");
            ViewData["TutorUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            return View();
        }

        // POST: Inscripciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InscripcionId,EventoId,LiderUsuarioId,SubcategoriaId,TituloProyecto,DescripcionProyecto,TutorUsuarioId,EstadoInscripcion,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Inscripcione inscripcione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inscripcione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId", inscripcione.EventoId);
            ViewData["LiderUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", inscripcione.LiderUsuarioId);
            ViewData["SubcategoriaId"] = new SelectList(_context.Subcategorias, "SubcategoriaId", "SubcategoriaId", inscripcione.SubcategoriaId);
            ViewData["TutorUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", inscripcione.TutorUsuarioId);
            return View(inscripcione);
        }

        // GET: Inscripciones/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcione = await _context.Inscripciones.FindAsync(id);
            if (inscripcione == null)
            {
                return NotFound();
            }
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId", inscripcione.EventoId);
            ViewData["LiderUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", inscripcione.LiderUsuarioId);
            ViewData["SubcategoriaId"] = new SelectList(_context.Subcategorias, "SubcategoriaId", "SubcategoriaId", inscripcione.SubcategoriaId);
            ViewData["TutorUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", inscripcione.TutorUsuarioId);
            return View(inscripcione);
        }

        // POST: Inscripciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("InscripcionId,EventoId,LiderUsuarioId,SubcategoriaId,TituloProyecto,DescripcionProyecto,TutorUsuarioId,EstadoInscripcion,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Inscripcione inscripcione)
        {
            if (id != inscripcione.InscripcionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inscripcione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InscripcioneExists(inscripcione.InscripcionId))
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
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId", inscripcione.EventoId);
            ViewData["LiderUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", inscripcione.LiderUsuarioId);
            ViewData["SubcategoriaId"] = new SelectList(_context.Subcategorias, "SubcategoriaId", "SubcategoriaId", inscripcione.SubcategoriaId);
            ViewData["TutorUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", inscripcione.TutorUsuarioId);
            return View(inscripcione);
        }

        // GET: Inscripciones/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcione = await _context.Inscripciones
                .Include(i => i.Evento)
                .Include(i => i.LiderUsuario)
                .Include(i => i.Subcategoria)
                .Include(i => i.TutorUsuario)
                .FirstOrDefaultAsync(m => m.InscripcionId == id);
            if (inscripcione == null)
            {
                return NotFound();
            }

            return View(inscripcione);
        }

        // POST: Inscripciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var inscripcione = await _context.Inscripciones.FindAsync(id);
            if (inscripcione != null)
            {
                _context.Inscripciones.Remove(inscripcione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InscripcioneExists(long id)
        {
            return _context.Inscripciones.Any(e => e.InscripcionId == id);
        }
    }
}

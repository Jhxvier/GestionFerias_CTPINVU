using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionFerias_CTPINVU.Data;
using GestionFerias_CTPINVU.Models;

namespace GestionFerias_CTPINVU.Controllers
{
    public class SubcategoriasController : Controller
    {
        private readonly AppDbContext _context;

        public SubcategoriasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var subcategorias = await _context.Subcategorias.Where(x => x.EsActivo)
                .Include(s => s.Categoria)
                .ToListAsync();
            return View(subcategorias);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var subcategoria = await _context.Subcategorias
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(s => s.SubcategoriaId == id);
            if (subcategoria == null) return NotFound();

            return View(subcategoria);
        }

        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias.Where(x => x.EsActivo), "CategoriaId", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,CategoriaId")] Subcategoria subcategoria)
        {
            ModelState.Remove("Categoria");
            ModelState.Remove("Inscripciones");
            if (ModelState.IsValid)
            {
                var usuarioId = long.TryParse(HttpContext.Session.GetString("UsuarioId"), out var uid) ? uid : (long?)null;
                subcategoria.UsuarioCreacion = usuarioId;
                subcategoria.UsuarioModificacion = usuarioId;
                _context.Add(subcategoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias.Where(x => x.EsActivo), "CategoriaId", "Nombre", subcategoria.CategoriaId);
            return View(subcategoria);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var subcategoria = await _context.Subcategorias.FindAsync(id);
            if (subcategoria == null) return NotFound();

            ViewData["CategoriaId"] = new SelectList(_context.Categorias.Where(x => x.EsActivo), "CategoriaId", "Nombre", subcategoria.CategoriaId);
            return View(subcategoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubcategoriaId,Nombre,CategoriaId")] Subcategoria subcategoria)
        {
            if (id != subcategoria.SubcategoriaId) return NotFound();

            ModelState.Remove("Categoria");
            ModelState.Remove("Inscripciones");
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioId = long.TryParse(HttpContext.Session.GetString("UsuarioId"), out var uid) ? uid : (long?)null;
                    subcategoria.UsuarioModificacion = usuarioId;
                    _context.Update(subcategoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Subcategorias.Any(s => s.SubcategoriaId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias.Where(x => x.EsActivo), "CategoriaId", "Nombre", subcategoria.CategoriaId);
            return View(subcategoria);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var subcategoria = await _context.Subcategorias
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(s => s.SubcategoriaId == id);
            if (subcategoria == null) return NotFound();

            return View(subcategoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subcategoria = await _context.Subcategorias.FindAsync(id);
            if (subcategoria != null)
            {
                subcategoria.EsActivo = false;
                _context.Subcategorias.Update(subcategoria);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

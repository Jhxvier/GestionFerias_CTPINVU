using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionFerias_CTPINVU.Data;
using GestionFerias_CTPINVU.Models;

namespace GestionFerias_CTPINVU.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int pagina = 1)
        {
            const int pageSize = 20;
            var query = _context.Categorias.Where(x => x.EsActivo)
                .Include(c => c.Subcategoria)
                .OrderBy(c => c.Nombre);
            var resultado = await PaginatedList<Categoria>.CreateAsync(query, pagina, pageSize);
            return View(resultado);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias
                .Include(c => c.Subcategoria)
                .FirstOrDefaultAsync(c => c.CategoriaId == id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,TipoFeria")] Categoria categoria)
        {
            ModelState.Remove("Subcategoria");
            if (ModelState.IsValid)
            {
                var usuarioId = long.TryParse(HttpContext.Session.GetString("UsuarioId"), out var uid) ? uid : (long?)null;
                categoria.UsuarioCreacion = usuarioId;
                categoria.UsuarioModificacion = usuarioId;
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoriaId,Nombre,TipoFeria")] Categoria categoria)
        {
            if (id != categoria.CategoriaId) return NotFound();

            ModelState.Remove("Subcategoria");
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioId = long.TryParse(HttpContext.Session.GetString("UsuarioId"), out var uid) ? uid : (long?)null;
                    categoria.UsuarioModificacion = usuarioId;
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Categorias.Any(c => c.CategoriaId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias
                .Include(c => c.Subcategoria)
                .FirstOrDefaultAsync(c => c.CategoriaId == id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                categoria.EsActivo = false;
                _context.Categorias.Update(categoria);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

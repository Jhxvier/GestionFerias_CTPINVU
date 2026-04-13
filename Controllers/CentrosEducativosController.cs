using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionFerias_CTPINVU.Data;
using GestionFerias_CTPINVU.Models;

namespace GestionFerias_CTPINVU.Controllers
{
    public class CentrosEducativosController : Controller
    {
        private readonly AppDbContext _context;

        public CentrosEducativosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var centros = await _context.CentrosEducativos.ToListAsync();
            return View(centros);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var centro = await _context.CentrosEducativos
                .FirstOrDefaultAsync(c => c.CentroId == id);
            if (centro == null) return NotFound();

            return View(centro);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreCentro,NombreDirector,CircuitoEducativo,DireccionRegional,Direccion")] CentrosEducativo centro)
        {
            if (ModelState.IsValid)
            {
                var usuarioId = long.TryParse(HttpContext.Session.GetString("UsuarioId"), out var uid) ? uid : (long?)null;
                centro.UsuarioCreacion = usuarioId;
                centro.UsuarioModificacion = usuarioId;
                _context.Add(centro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(centro);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var centro = await _context.CentrosEducativos.FindAsync(id);
            if (centro == null) return NotFound();

            return View(centro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CentroId,NombreCentro,NombreDirector,CircuitoEducativo,DireccionRegional,Direccion")] CentrosEducativo centro)
        {
            if (id != centro.CentroId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioId = long.TryParse(HttpContext.Session.GetString("UsuarioId"), out var uid) ? uid : (long?)null;
                    centro.UsuarioModificacion = usuarioId;
                    _context.Update(centro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CentroExists(centro.CentroId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(centro);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var centro = await _context.CentrosEducativos
                .FirstOrDefaultAsync(c => c.CentroId == id);
            if (centro == null) return NotFound();

            return View(centro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var centro = await _context.CentrosEducativos.FindAsync(id);
            if (centro != null)
            {
                _context.CentrosEducativos.Remove(centro);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CentroExists(long id)
        {
            return _context.CentrosEducativos.Any(c => c.CentroId == id);
        }
    }
}

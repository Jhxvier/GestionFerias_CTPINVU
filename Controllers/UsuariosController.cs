using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionFerias_CTPINVU.Data;
using GestionFerias_CTPINVU.Models;

namespace GestionFerias_CTPINVU.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _context.Usuarios
                .Include(u => u.UsuarioCreacionNavigation)
                .Include(u => u.UsuarioModificacionNavigation)
                .ToListAsync();

            return View(lista);
        }

        public async Task<IActionResult> Perfil()
        {
            var appDbContext = _context.Usuarios.Include(u => u.UsuarioCreacionNavigation).Include(u => u.UsuarioModificacionNavigation);
            return View(await appDbContext.ToListAsync());
        }

        public IActionResult Create(string rol = "")
        {
            return RedirectToAction("Perfil", new { modo = "create", rol });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Usuario usuario)
        {
            return RedirectToAction("Perfil", new { modo = "create" });
        }

        public IActionResult Edit(long? id, string rol = "")
        {
            if (id == null)
            {
                return NotFound();
            }

            return RedirectToAction("Perfil", new { id, modo = "edit", rol });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, Usuario usuario)
        {
            return RedirectToAction("Perfil", new { id, modo = "edit" });
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioCreacionNavigation)
                .Include(u => u.UsuarioModificacionNavigation)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioCreacionNavigation)
                .Include(u => u.UsuarioModificacionNavigation)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(long id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionFerias_CTPINVU.Data;
using GestionFerias_CTPINVU.Models;

namespace GestionFerias_CTPINVU.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly Data.AppDbContext _context;

        public EstudiantesController(Data.AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Estudiantes
                .Include(e => e.EstudianteNavigation)
                    .ThenInclude(u => u.Persona);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Estudiantes/Create
        public IActionResult Create()
        {
            return RedirectToAction("Perfil", "Usuarios", new { modo = "create", rol = "estudiante" });
        }

        // POST: Estudiantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("EstudianteId,Grado,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Estudiante estudiante)
        {
            return RedirectToAction("Perfil", "Usuarios", new { modo = "create", rol = "estudiante" });
        }

        // GET: Estudiantes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return RedirectToAction("Perfil", "Usuarios", new { id, modo = "edit", rol = "estudiante" });
        }

        // POST: Estudiantes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("EstudianteId,Grado,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Estudiante estudiante)
        {
            return RedirectToAction("Perfil", "Usuarios", new { id, modo = "edit", rol = "estudiante" });
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .Include(e => e.EstudianteNavigation)
                .FirstOrDefaultAsync(m => m.EstudianteId == id);

            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .Include(e => e.EstudianteNavigation)
                .FirstOrDefaultAsync(m => m.EstudianteId == id);

            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante != null)
            {
                _context.Estudiantes.Remove(estudiante);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteExists(long id)
        {
            return _context.Estudiantes.Any(e => e.EstudianteId == id);
        }
    }
}
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

        private bool EsAdminOCoord()
        {
            var rol = HttpContext.Session.GetString("Rol") ?? "";
            return rol.Contains("Administrador", StringComparison.OrdinalIgnoreCase) ||
                   rol.Contains("Coordinador", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<IActionResult> Index(string? textoBuscar, string? filtroGrado)
        {
            if (!EsAdminOCoord()) return StatusCode(403);

            var query = _context.Estudiantes
                .Include(e => e.EstudianteNavigation)
                    .ThenInclude(u => u.Persona)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                var lowerBuscar = textoBuscar.ToLower();
                query = query.Where(e => (e.EstudianteNavigation != null && e.EstudianteNavigation.Correo.ToLower().Contains(lowerBuscar)) ||
                                         (e.EstudianteNavigation != null && e.EstudianteNavigation.Persona != null && 
                                            (e.EstudianteNavigation.Persona.Nombres.ToLower().Contains(lowerBuscar) ||
                                             e.EstudianteNavigation.Persona.Apellidos.ToLower().Contains(lowerBuscar) ||
                                             e.EstudianteNavigation.Persona.Documento.ToLower().Contains(lowerBuscar))));
            }

            if (!string.IsNullOrWhiteSpace(filtroGrado))
            {
                query = query.Where(e => e.Grado == filtroGrado);
            }

            ViewData["CurrentBuscar"] = textoBuscar;
            ViewData["CurrentGrado"] = filtroGrado;

            return View(await query.ToListAsync());
        }

        // GET: Estudiantes/Create
        public IActionResult Create()
        {
            if (!EsAdminOCoord()) return StatusCode(403);
            return RedirectToAction("Perfil", "Usuarios", new { modo = "create", rol = "estudiante" });
        }

        // POST: Estudiantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("EstudianteId,Grado,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Estudiante estudiante)
        {
            if (!EsAdminOCoord()) return StatusCode(403);
            return RedirectToAction("Perfil", "Usuarios", new { modo = "create", rol = "estudiante" });
        }

        // GET: Estudiantes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (!EsAdminOCoord()) return StatusCode(403);

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
            if (!EsAdminOCoord()) return StatusCode(403);
            return RedirectToAction("Perfil", "Usuarios", new { id, modo = "edit", rol = "estudiante" });
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (!EsAdminOCoord()) return StatusCode(403);
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
            if (!EsAdminOCoord()) return StatusCode(403);
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .Include(e => e.EstudianteNavigation)
                    .ThenInclude(u => u.Persona)
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
            if (!EsAdminOCoord()) return StatusCode(403);
            var estudiante = await _context.Estudiantes
                .Include(e => e.EstudianteNavigation)
                .FirstOrDefaultAsync(e => e.EstudianteId == id);
            if (estudiante != null)
            {
                // cambiar estado a inactivo antes de eliminar para mantener integridad referencial
                if (estudiante.EstudianteNavigation != null)
                {
                    estudiante.EstudianteNavigation.Estado = "Inactivo";
                }
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
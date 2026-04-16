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

        private bool EsAdminOCoord()
        {
            var rol = HttpContext.Session.GetString("Rol") ?? "";
            return rol.Contains("Administrador", StringComparison.OrdinalIgnoreCase) ||
                   rol.Contains("Coordinador", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<IActionResult> Index(string? textoBuscar, string? filtroEspecialidad)
        {
            if (!EsAdminOCoord()) return StatusCode(403);

            var query = _context.Tutores
                .Include(t => t.Tutor)
                    .ThenInclude(u => u.Persona)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                var lowerBuscar = textoBuscar.ToLower();
                query = query.Where(t => (t.Tutor != null && t.Tutor.Correo.ToLower().Contains(lowerBuscar)) ||
                                         (t.Tutor != null && t.Tutor.Persona != null && 
                                            (t.Tutor.Persona.Nombres.ToLower().Contains(lowerBuscar) ||
                                             t.Tutor.Persona.Apellidos.ToLower().Contains(lowerBuscar) ||
                                             t.Tutor.Persona.Documento.ToLower().Contains(lowerBuscar))));
            }

            if (!string.IsNullOrWhiteSpace(filtroEspecialidad))
            {
                query = query.Where(t => t.Especialidad == filtroEspecialidad);
            }

            ViewData["CurrentBuscar"] = textoBuscar;
            ViewData["CurrentEspecialidad"] = filtroEspecialidad;

            return View(await query.ToListAsync());
        }

        // GET: Tutores/Create
        public IActionResult Create()
        {
            if (!EsAdminOCoord()) return StatusCode(403);
            return RedirectToAction("Perfil", "Usuarios", new { modo = "create", rol = "tutor" });
        }

        // POST: Tutores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("TutorId,Especialidad,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Tutore tutore)
        {
            if (!EsAdminOCoord()) return StatusCode(403);
            return RedirectToAction("Perfil", "Usuarios", new { modo = "create", rol = "tutor" });
        }

        // GET: Tutores/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (!EsAdminOCoord()) return StatusCode(403);

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
            if (!EsAdminOCoord()) return StatusCode(403);
            return RedirectToAction("Perfil", "Usuarios", new { id, modo = "edit", rol = "tutor" });
        }

        public IActionResult Details(long? id)
        {
            if (!EsAdminOCoord()) return StatusCode(403);
            if (id == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Usuarios", new { id });
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (!EsAdminOCoord()) return StatusCode(403);
            if (id == null)
            {
                return NotFound();
            }

            var tutore = await _context.Tutores
                .Include(t => t.Tutor)
                    .ThenInclude(u => u.Persona)
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
            if (!EsAdminOCoord()) return StatusCode(403);
            var tutore = await _context.Tutores
                .Include(t => t.Tutor)
                .FirstOrDefaultAsync(t => t.TutorId == id);
            if (tutore != null)
            {
                // En lugar de eliminar el usuario, se marca como inactivo para mantener la integridad referencial
                if (tutore.Tutor != null)
                {
                    tutore.Tutor.Estado = "Inactivo";
                }
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
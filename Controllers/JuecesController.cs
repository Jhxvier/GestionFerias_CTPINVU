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
    public class JuecesController : Controller
    {
        private readonly AppDbContext _context;

        public JuecesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? textoBuscar, string? filtroEstado)
        {
            var query = _context.Jueces
                .Include(j => j.Juez)
                    .ThenInclude(u => u.Persona)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                var lowerBuscar = textoBuscar.ToLower();
                query = query.Where(j => (j.Juez != null && j.Juez.Correo.ToLower().Contains(lowerBuscar)) ||
                                         (j.Juez != null && j.Juez.Persona != null && 
                                            (j.Juez.Persona.Nombres.ToLower().Contains(lowerBuscar) ||
                                             j.Juez.Persona.Apellidos.ToLower().Contains(lowerBuscar) ||
                                             j.Juez.Persona.Documento.ToLower().Contains(lowerBuscar))));
            }

            if (!string.IsNullOrWhiteSpace(filtroEstado))
            {
                query = query.Where(j => j.Juez != null && j.Juez.Estado == filtroEstado);
            }

            ViewData["CurrentBuscar"] = textoBuscar;
            ViewData["CurrentEstado"] = filtroEstado;

            return View(await query.ToListAsync());
        }

        // GET: Jueces/Create
        public IActionResult Create()
        {
            return RedirectToAction("Perfil", "Usuarios", new { modo = "create", rol = "juez" });
        }

        // POST: Jueces/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("JuezId,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Juece juece)
        {
            return RedirectToAction("Perfil", "Usuarios", new { modo = "create", rol = "juez" });
        }

        // GET: Jueces/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return RedirectToAction("Perfil", "Usuarios", new { id, modo = "edit", rol = "juez" });
        }

        // POST: Jueces/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("JuezId,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] Juece juece)
        {
            return RedirectToAction("Perfil", "Usuarios", new { id, modo = "edit", rol = "juez" });
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var juece = await _context.Jueces
                .Include(j => j.Juez)
                .FirstOrDefaultAsync(m => m.JuezId == id);

            if (juece == null)
            {
                return NotFound();
            }

            return View(juece);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var juece = await _context.Jueces
                .Include(j => j.Juez)
                    .ThenInclude(u => u.Persona)
                .FirstOrDefaultAsync(m => m.JuezId == id);

            if (juece == null)
            {
                return NotFound();
            }

            return View(juece);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var juece = await _context.Jueces
                .Include(j => j.Juez)
                .FirstOrDefaultAsync(j => j.JuezId == id);
            if (juece != null)
            {
                //cambiar estado del usuario a inactivo antes de eliminar el juez
                if (juece.Juez != null)
                {
                    juece.Juez.Estado = "Inactivo";
                }
                _context.Jueces.Remove(juece);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JueceExists(long id)
        {
            return _context.Jueces.Any(e => e.JuezId == id);
        }
    }
}
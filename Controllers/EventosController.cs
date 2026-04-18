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

    public class EventosController : Controller
    {
        private readonly AppDbContext _context;

        public EventosController(AppDbContext context)
        {
            _context = context;
        }

        private bool EsAdminOCoord()
        {
            var rol = HttpContext.Session.GetString("Rol") ?? "";
            return rol.Contains("Administrador", StringComparison.OrdinalIgnoreCase) ||
                   rol.Contains("Coordinador", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<IActionResult> Index()
        {
            if (!EsAdminOCoord()) return StatusCode(403);

            var eventos = await _context.Eventos.Include(e => e.Centro).Where(e => e.EsActivo).ToListAsync();
            
            var today = DateOnly.FromDateTime(DateTime.Now);
            bool updated = false;
            foreach (var evt in eventos)
            {
                if (evt.EstadoEvento != "Cancelado" && evt.EstadoEvento != "Finalizado" && today > evt.FechaFin)
                {
                    evt.EstadoEvento = "Finalizado";
                    updated = true;
                }
            }
            if (updated) await _context.SaveChangesAsync();

            return View(eventos);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (!EsAdminOCoord()) return StatusCode(403);
            if (id == null) return NotFound();

            var evento = await _context.Eventos
                .Include(e => e.Centro)
                .FirstOrDefaultAsync(m => m.EventoId == id);
            if (evento == null) return NotFound();

            return View(evento);
        }

        public IActionResult Create()
        {
            if (!EsAdminOCoord()) return StatusCode(403);

            ViewData["CentroId"] = new SelectList(_context.CentrosEducativos.Where(x => x.EsActivo), "CentroId", "NombreCentro");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventoId,CodigoEvento,CentroId,NombreEvento,TipoFeria,Descripcion,FechaInicio,FechaFin,EstadoEvento")] Evento evento)
        {
            if (!EsAdminOCoord()) return StatusCode(403);

            ModelState.Remove("Centro");
            ModelState.Remove("CodigoEvento");
            ModelState.Remove("EstadoEvento");
            
            evento.EstadoEvento = "En proceso";

            var hoy = DateOnly.FromDateTime(DateTime.Now);

            if (evento.FechaInicio > evento.FechaFin)
            {
                ModelState.AddModelError("FechaFin", "La Fecha de Fin no puede ser anterior a la Fecha de Inicio.");
            }
            else if (evento.FechaFin < hoy)
            {
                ModelState.AddModelError("FechaFin", "La Fecha de Fin no puede ser una fecha pasada.");
            }

            if (ModelState.IsValid)
            {
                var maxId = await _context.Eventos.MaxAsync(e => (long?)e.EventoId) ?? 0;
                evento.CodigoEvento = $"EVT-{(maxId + 1):D3}";
                
                var usuarioId = long.TryParse(HttpContext.Session.GetString("UsuarioId"), out var uid) ? uid : (long?)null;
                evento.UsuarioCreacion = usuarioId;
                evento.UsuarioModificacion = usuarioId;
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CentroId"] = new SelectList(_context.CentrosEducativos, "CentroId", "NombreCentro", evento.CentroId);
            return View(evento);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (!EsAdminOCoord()) return StatusCode(403);
            if (id == null) return NotFound();

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null) return NotFound();

            ViewData["CentroId"] = new SelectList(_context.CentrosEducativos, "CentroId", "NombreCentro", evento.CentroId);
            return View(evento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("EventoId,CodigoEvento,CentroId,NombreEvento,TipoFeria,Descripcion,FechaInicio,FechaFin,EstadoEvento")] Evento evento)
        {
            if (!EsAdminOCoord()) return StatusCode(403);
            if (id != evento.EventoId) return NotFound();

            ModelState.Remove("Centro");
            ModelState.Remove("CodigoEvento");
            if (evento.FechaInicio > evento.FechaFin)
            {
                ModelState.AddModelError("FechaFin", "La Fecha de Fin no puede ser anterior a la Fecha de Inicio.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioId = long.TryParse(HttpContext.Session.GetString("UsuarioId"), out var uid) ? uid : (long?)null;
                    evento.UsuarioModificacion = usuarioId;
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.EventoId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CentroId"] = new SelectList(_context.CentrosEducativos, "CentroId", "NombreCentro", evento.CentroId);
            return View(evento);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (!EsAdminOCoord()) return StatusCode(403);
            if (id == null) return NotFound();

            var evento = await _context.Eventos
                .Include(e => e.Centro)
                .FirstOrDefaultAsync(m => m.EventoId == id);
            if (evento == null) return NotFound();

            // Verificar si tiene inscripciones activas asociadas
            var tieneInscripciones = await _context.Inscripciones
                .AnyAsync(i => i.EventoId == id && i.EsActivo);

            ViewData["TieneInscripciones"] = tieneInscripciones;

            return View(evento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (!EsAdminOCoord()) return StatusCode(403);

            // Bloquear si el evento tiene inscripciones activas
            var tieneInscripciones = await _context.Inscripciones
                .AnyAsync(i => i.EventoId == id && i.EsActivo);

            if (tieneInscripciones)
            {
                TempData["ErrorEliminar"] = "No se puede desactivar este evento porque tiene inscripciones registradas. Primero gestione o desactive las inscripciones asociadas.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                evento.EsActivo = false;
                _context.Eventos.Update(evento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EventoExists(long id)
        {
            return _context.Eventos.Any(e => e.EventoId == id);
        }
    }
}

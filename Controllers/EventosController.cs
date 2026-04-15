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
            if (!EsAdminOCoord()) return Unauthorized();

            var appDbContext = _context.Eventos.Include(e => e.Centro);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (!EsAdminOCoord()) return Unauthorized();
            if (id == null) return NotFound();

            var evento = await _context.Eventos
                .Include(e => e.Centro)
                .FirstOrDefaultAsync(m => m.EventoId == id);
            if (evento == null) return NotFound();

            return View(evento);
        }

        public IActionResult Create()
        {
            if (!EsAdminOCoord()) return Unauthorized();

            ViewData["CentroId"] = new SelectList(_context.CentrosEducativos, "CentroId", "NombreCentro");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventoId,CodigoEvento,CentroId,NombreEvento,TipoFeria,Descripcion,FechaInicio,FechaFin,EstadoEvento")] Evento evento)
        {
            if (!EsAdminOCoord()) return Unauthorized();

            ModelState.Remove("Centro");
            ModelState.Remove("CodigoEvento");
            if (ModelState.IsValid)
            {
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
            if (!EsAdminOCoord()) return Unauthorized();
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
            if (!EsAdminOCoord()) return Unauthorized();
            if (id != evento.EventoId) return NotFound();

            ModelState.Remove("Centro");
            ModelState.Remove("CodigoEvento");
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
            if (!EsAdminOCoord()) return Unauthorized();
            if (id == null) return NotFound();

            var evento = await _context.Eventos
                .Include(e => e.Centro)
                .FirstOrDefaultAsync(m => m.EventoId == id);
            if (evento == null) return NotFound();

            return View(evento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (!EsAdminOCoord()) return Unauthorized();

            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoExists(long id)
        {
            return _context.Eventos.Any(e => e.EventoId == id);
        }
    }
}

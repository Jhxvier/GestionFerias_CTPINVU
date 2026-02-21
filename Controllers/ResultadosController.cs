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

    public class ResultadosController : Controller
    {
        private readonly AppDbContext _context;

        public ResultadosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ResultadosEventos
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ResultadosEventos.Include(r => r.Evento).Include(r => r.JuezResponsableUsuario);
            return View(await appDbContext.ToListAsync());
        }

        // GET: ResultadosEventos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resultadosEvento = await _context.ResultadosEventos
                .Include(r => r.Evento)
                .Include(r => r.JuezResponsableUsuario)
                .FirstOrDefaultAsync(m => m.ResultadoEventoId == id);
            if (resultadosEvento == null)
            {
                return NotFound();
            }

            return View(resultadosEvento);
        }

        // GET: ResultadosEventos/Create
        public IActionResult Create()
        {
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId");
            ViewData["JuezResponsableUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId");
            return View();
        }

        // POST: ResultadosEventos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResultadoEventoId,EventoId,EstadoResultados,JuezResponsableUsuarioId,ResolucionFinal,FechaPublicacion,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] ResultadosEvento resultadosEvento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resultadosEvento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId", resultadosEvento.EventoId);
            ViewData["JuezResponsableUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", resultadosEvento.JuezResponsableUsuarioId);
            return View(resultadosEvento);
        }

        // GET: ResultadosEventos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resultadosEvento = await _context.ResultadosEventos.FindAsync(id);
            if (resultadosEvento == null)
            {
                return NotFound();
            }
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId", resultadosEvento.EventoId);
            ViewData["JuezResponsableUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", resultadosEvento.JuezResponsableUsuarioId);
            return View(resultadosEvento);
        }

        // POST: ResultadosEventos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ResultadoEventoId,EventoId,EstadoResultados,JuezResponsableUsuarioId,ResolucionFinal,FechaPublicacion,UsuarioCreacion,FechaCreacion,UsuarioModificacion,FechaModificacion")] ResultadosEvento resultadosEvento)
        {
            if (id != resultadosEvento.ResultadoEventoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resultadosEvento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResultadosEventoExists(resultadosEvento.ResultadoEventoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "EventoId", resultadosEvento.EventoId);
            ViewData["JuezResponsableUsuarioId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", resultadosEvento.JuezResponsableUsuarioId);
            return View(resultadosEvento);
        }

        // GET: ResultadosEventos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resultadosEvento = await _context.ResultadosEventos
                .Include(r => r.Evento)
                .Include(r => r.JuezResponsableUsuario)
                .FirstOrDefaultAsync(m => m.ResultadoEventoId == id);
            if (resultadosEvento == null)
            {
                return NotFound();
            }

            return View(resultadosEvento);
        }

        // POST: ResultadosEventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var resultadosEvento = await _context.ResultadosEventos.FindAsync(id);
            if (resultadosEvento != null)
            {
                _context.ResultadosEventos.Remove(resultadosEvento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResultadosEventoExists(long id)
        {
            return _context.ResultadosEventos.Any(e => e.ResultadoEventoId == id);
        }
    }
}

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

        private long? GetUsuarioId()
        {
            return long.TryParse(HttpContext.Session.GetString("UsuarioId"), out var uid) ? uid : null;
        }

        private string GetRol()
        {
            return HttpContext.Session.GetString("Rol") ?? "";
        }

        private bool EsAdminOCoord()
        {
            var rol = GetRol();
            return rol.Contains("Administrador", StringComparison.OrdinalIgnoreCase) ||
                   rol.Contains("Coordinador", StringComparison.OrdinalIgnoreCase) ||
                   rol.Contains("Juez", StringComparison.OrdinalIgnoreCase);
        }

        // GET: Resultados
        public async Task<IActionResult> Index(string? textoBuscar, string? filtroEvento, string? filtroEstado)
        {
            var query = _context.ResultadosEventos
                .Include(r => r.Evento)
                .Include(r => r.ResultadosGanadores).ThenInclude(g => g.Inscripcion).ThenInclude(i => i.LiderUsuario).ThenInclude(u => u.Persona)
                .Include(r => r.JuezResponsableUsuario).ThenInclude(u => u.Juez).ThenInclude(u => u.Persona)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(textoBuscar))
            {
                var lowerBuscar = textoBuscar.ToLower();
                query = query.Where(r => (r.Evento != null && r.Evento.NombreEvento.ToLower().Contains(lowerBuscar)) ||
                                         (r.ResolucionFinal != null && r.ResolucionFinal.ToLower().Contains(lowerBuscar)));
            }

            if (!string.IsNullOrWhiteSpace(filtroEvento) && long.TryParse(filtroEvento, out long eventoId))
            {
                query = query.Where(r => r.EventoId == eventoId);
            }

            if (!string.IsNullOrWhiteSpace(filtroEstado))
            {
                query = query.Where(r => r.EstadoResultados == filtroEstado);
            }

            ViewData["EventosList"] = new SelectList(await _context.Eventos.ToListAsync(), "EventoId", "NombreEvento", filtroEvento);
            ViewData["CurrentBuscar"] = textoBuscar;
            ViewData["CurrentEvento"] = filtroEvento;
            ViewData["CurrentEstado"] = filtroEstado;
            ViewData["EsAdminOCoord"] = EsAdminOCoord();

            return View(await query.ToListAsync());
        }

        // GET: Resultados/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var resultadosEvento = await _context.ResultadosEventos
                .Include(r => r.Evento)
                .Include(r => r.ResultadosGanadores).ThenInclude(g => g.Inscripcion).ThenInclude(i => i.LiderUsuario).ThenInclude(u => u.Persona)
                .Include(r => r.ResultadosGanadores).ThenInclude(g => g.Inscripcion).ThenInclude(i => i.Evento)
                .Include(r => r.JuezResponsableUsuario).ThenInclude(u => u.Juez).ThenInclude(u => u.Persona)
                .FirstOrDefaultAsync(m => m.ResultadoEventoId == id);

            if (resultadosEvento == null) return NotFound();

            ViewData["EsAdminOCoord"] = EsAdminOCoord();
            return View(resultadosEvento);
        }

        // GET: Resultados/Create
        public IActionResult Create()
        {
            if (!EsAdminOCoord()) return Unauthorized();

            var uid = GetUsuarioId();
            var juezActual = _context.Usuarios.Include(u => u.Persona).FirstOrDefault(u => u.UsuarioId == uid);
            ViewData["JuezNombre"] = juezActual?.Persona != null ? $"{juezActual.Persona.Nombres} {juezActual.Persona.Apellidos}" : "Juez Actual";
            
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "NombreEvento");
            return View(new ResultadoEventoViewModel());
        }

        // POST: Resultados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResultadoEventoViewModel viewModel)
        {
            if (!EsAdminOCoord()) return Unauthorized();

            // Validar que no se repitan las inscripciones
            if (viewModel.Inscripcion1erLugarId == viewModel.Inscripcion2doLugarId ||
                viewModel.Inscripcion1erLugarId == viewModel.Inscripcion3erLugarId ||
                viewModel.Inscripcion2doLugarId == viewModel.Inscripcion3erLugarId)
            {
                ModelState.AddModelError("", "No se puede asignar el mismo proyecto a múltipes lugares.");
            }

            if (ModelState.IsValid)
            {
                var usuarioId = GetUsuarioId();
                var nuevoResultado = new ResultadosEvento
                {
                    EventoId = viewModel.EventoId,
                    EstadoResultados = "Publicado", // Se publica directamente al crear en la nueva vista
                    ResolucionFinal = viewModel.ResolucionFinal,
                    JuezResponsableUsuarioId = usuarioId,
                    UsuarioCreacion = usuarioId,
                    UsuarioModificacion = usuarioId,
                    FechaPublicacion = DateTime.Now
                };

                _context.ResultadosEventos.Add(nuevoResultado);
                await _context.SaveChangesAsync(); // Para obtener el ID

                var ganadores = new List<ResultadosGanadore>
                {
                    new ResultadosGanadore { ResultadoEventoId = nuevoResultado.ResultadoEventoId, Posicion = 1, InscripcionId = viewModel.Inscripcion1erLugarId, Nota = viewModel.Nota1erLugar, UsuarioCreacion = usuarioId },
                    new ResultadosGanadore { ResultadoEventoId = nuevoResultado.ResultadoEventoId, Posicion = 2, InscripcionId = viewModel.Inscripcion2doLugarId, Nota = viewModel.Nota2doLugar, UsuarioCreacion = usuarioId },
                    new ResultadosGanadore { ResultadoEventoId = nuevoResultado.ResultadoEventoId, Posicion = 3, InscripcionId = viewModel.Inscripcion3erLugarId, Nota = viewModel.Nota3erLugar, UsuarioCreacion = usuarioId }
                };

                _context.ResultadosGanadores.AddRange(ganadores);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var uid = GetUsuarioId();
            var juezActual = _context.Usuarios.Include(u => u.Persona).FirstOrDefault(u => u.UsuarioId == uid);
            ViewData["JuezNombre"] = juezActual?.Persona != null ? $"{juezActual.Persona.Nombres} {juezActual.Persona.Apellidos}" : "Juez Actual";
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "NombreEvento", viewModel.EventoId);
            return View(viewModel);
        }

        // GET: Resultados/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || !EsAdminOCoord()) return NotFound();

            var resultadosEvento = await _context.ResultadosEventos
                .Include(r => r.ResultadosGanadores)
                .FirstOrDefaultAsync(r => r.ResultadoEventoId == id);

            if (resultadosEvento == null) return NotFound();

            var vm = new ResultadoEventoViewModel
            {
                ResultadoEventoId = resultadosEvento.ResultadoEventoId,
                EventoId = resultadosEvento.EventoId,
                ResolucionFinal = resultadosEvento.ResolucionFinal,
                EstadoResultados = resultadosEvento.EstadoResultados
            };

            var primerLugar = resultadosEvento.ResultadosGanadores.FirstOrDefault(g => g.Posicion == 1);
            if (primerLugar != null) { vm.Inscripcion1erLugarId = primerLugar.InscripcionId; vm.Nota1erLugar = primerLugar.Nota; }

            var segundoLugar = resultadosEvento.ResultadosGanadores.FirstOrDefault(g => g.Posicion == 2);
            if (segundoLugar != null) { vm.Inscripcion2doLugarId = segundoLugar.InscripcionId; vm.Nota2doLugar = segundoLugar.Nota; }

            var tercerLugar = resultadosEvento.ResultadosGanadores.FirstOrDefault(g => g.Posicion == 3);
            if (tercerLugar != null) { vm.Inscripcion3erLugarId = tercerLugar.InscripcionId; vm.Nota3erLugar = tercerLugar.Nota; }

            var uid = resultadosEvento.JuezResponsableUsuarioId;
            var juezOriginal = _context.Usuarios.Include(u => u.Persona).FirstOrDefault(u => u.UsuarioId == uid);
            ViewData["JuezNombre"] = juezOriginal?.Persona != null ? $"{juezOriginal.Persona.Nombres} {juezOriginal.Persona.Apellidos}" : "Juez Actual";
            
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "NombreEvento", vm.EventoId);
            return View(vm);
        }

        // POST: Resultados/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, ResultadoEventoViewModel viewModel)
        {
            if (id != viewModel.ResultadoEventoId || !EsAdminOCoord()) return NotFound();

            if (viewModel.Inscripcion1erLugarId == viewModel.Inscripcion2doLugarId ||
                viewModel.Inscripcion1erLugarId == viewModel.Inscripcion3erLugarId ||
                viewModel.Inscripcion2doLugarId == viewModel.Inscripcion3erLugarId)
            {
                ModelState.AddModelError("", "No se puede asignar el mismo proyecto a múltipes lugares.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioId = GetUsuarioId();
                    var resultadoDB = await _context.ResultadosEventos
                        .Include(r => r.ResultadosGanadores)
                        .FirstOrDefaultAsync(r => r.ResultadoEventoId == id);

                    if (resultadoDB == null) return NotFound();

                    resultadoDB.EventoId = viewModel.EventoId;
                    resultadoDB.ResolucionFinal = viewModel.ResolucionFinal;
                    resultadoDB.UsuarioModificacion = usuarioId;

                    // Remover ganadores anteriores y asignar nuevos
                    _context.ResultadosGanadores.RemoveRange(resultadoDB.ResultadosGanadores);
                    
                    resultadoDB.ResultadosGanadores.Add(new ResultadosGanadore { ResultadoEventoId = id, Posicion = 1, InscripcionId = viewModel.Inscripcion1erLugarId, Nota = viewModel.Nota1erLugar, UsuarioCreacion = usuarioId });
                    resultadoDB.ResultadosGanadores.Add(new ResultadosGanadore { ResultadoEventoId = id, Posicion = 2, InscripcionId = viewModel.Inscripcion2doLugarId, Nota = viewModel.Nota2doLugar, UsuarioCreacion = usuarioId });
                    resultadoDB.ResultadosGanadores.Add(new ResultadosGanadore { ResultadoEventoId = id, Posicion = 3, InscripcionId = viewModel.Inscripcion3erLugarId, Nota = viewModel.Nota3erLugar, UsuarioCreacion = usuarioId });

                    _context.Update(resultadoDB);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ResultadosEventos.Any(e => e.ResultadoEventoId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var uid = GetUsuarioId();
            var juezActual = _context.Usuarios.Include(u => u.Persona).FirstOrDefault(u => u.UsuarioId == uid);
            ViewData["JuezNombre"] = juezActual?.Persona != null ? $"{juezActual.Persona.Nombres} {juezActual.Persona.Apellidos}" : "Juez Actual";
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "NombreEvento", viewModel.EventoId);
            return View(viewModel);
        }

        // GET: Resultados/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || !EsAdminOCoord()) return NotFound();

            var resultadosEvento = await _context.ResultadosEventos
                .Include(r => r.Evento)
                .Include(r => r.JuezResponsableUsuario).ThenInclude(u => u.Juez).ThenInclude(u => u.Persona)
                .FirstOrDefaultAsync(m => m.ResultadoEventoId == id);

            if (resultadosEvento == null) return NotFound();

            return View(resultadosEvento);
        }

        // POST: Resultados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (!EsAdminOCoord()) return Unauthorized();

            var resultadosEvento = await _context.ResultadosEventos
                .Include(r => r.ResultadosGanadores)
                .FirstOrDefaultAsync(m => m.ResultadoEventoId == id);

            if (resultadosEvento != null)
            {
                _context.ResultadosGanadores.RemoveRange(resultadosEvento.ResultadosGanadores);
                _context.ResultadosEventos.Remove(resultadosEvento);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> GetInscripcionesPorEvento(long eventoId)
        {
            var inscripciones = await _context.Inscripciones
                .Include(i => i.LiderUsuario).ThenInclude(u => u.Persona)
                .Where(i => i.EventoId == eventoId && i.EstadoInscripcion == "Aprobado")
                .Select(i => new
                {
                    id = i.InscripcionId,
                    titulo = i.TituloProyecto ?? "Sin título",
                    lider = i.LiderUsuario.Persona != null ? 
                            i.LiderUsuario.Persona.Nombres + " " + i.LiderUsuario.Persona.Apellidos : 
                            "ID: " + i.LiderUsuarioId
                })
                .ToListAsync();

            return Json(inscripciones);
        }
    }
}

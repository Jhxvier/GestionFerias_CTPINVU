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
            var query = _context.ResultadosEventos.Where(r => r.EsActivo)
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

            // Si no es Admin/Coord/Juez (es decir, es Estudiante o Tutor), solo mostrar resultados publicados
            if (!EsAdminOCoord())
            {
                query = query.Where(r => r.EstadoResultados == "Publicado");
            }

            ViewData["EventosList"] = new SelectList(await _context.Eventos.Where(e => e.EsActivo).ToListAsync(), "EventoId", "NombreEvento", filtroEvento);
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
            if (!EsAdminOCoord()) return StatusCode(403);

            var uid = GetUsuarioId();
            var juezActual = _context.Usuarios.Include(u => u.Persona).FirstOrDefault(u => u.UsuarioId == uid);
            ViewData["JuezNombre"] = juezActual?.Persona != null ? $"{juezActual.Persona.Nombres} {juezActual.Persona.Apellidos}" : "Juez Actual";

            // Solo mostrar eventos que NO tienen ya un resultado publicado activo
            var eventosConResultado = _context.ResultadosEventos
                .Where(r => r.EsActivo)
                .Select(r => r.EventoId)
                .Distinct();

            var eventosSinResultado = _context.Eventos
                .Where(e => e.EsActivo && !eventosConResultado.Contains(e.EventoId));

            ViewData["EventoId"] = new SelectList(eventosSinResultado, "EventoId", "NombreEvento");
            return View(new ResultadoEventoViewModel());
        }

        // POST: Resultados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResultadoEventoViewModel viewModel)
        {
            if (!EsAdminOCoord()) return StatusCode(403);

            // Verificar que no exista ya un resultado activo para este evento (solo 1 por evento)
            bool yaExisteResultado = await _context.ResultadosEventos
                .AnyAsync(r => r.EventoId == viewModel.EventoId && r.EsActivo);

            if (yaExisteResultado)
            {
                TempData["ToastError"] = "Ya existe un resultado registrado para este evento. Solo se permite un resultado por evento.";
                ModelState.AddModelError("EventoId", "Ya existe un resultado publicado para este evento.");
            }

            // Validar que no se repitan las inscripciones (solo comparar los que tienen valor)
            if (viewModel.Inscripcion2doLugarId.HasValue &&
                viewModel.Inscripcion1erLugarId == viewModel.Inscripcion2doLugarId)
            {
                ModelState.AddModelError("", "El 2do lugar no puede ser el mismo proyecto que el 1er lugar.");
            }
            if (viewModel.Inscripcion3erLugarId.HasValue &&
                viewModel.Inscripcion1erLugarId == viewModel.Inscripcion3erLugarId)
            {
                ModelState.AddModelError("", "El 3er lugar no puede ser el mismo proyecto que el 1er lugar.");
            }
            if (viewModel.Inscripcion2doLugarId.HasValue && viewModel.Inscripcion3erLugarId.HasValue &&
                viewModel.Inscripcion2doLugarId == viewModel.Inscripcion3erLugarId)
            {
                ModelState.AddModelError("", "El 2do y 3er lugar no pueden ser el mismo proyecto.");
            }

            // Si seleccionó inscripción para 2do lugar, la nota es obligatoria
            if (viewModel.Inscripcion2doLugarId.HasValue && !viewModel.Nota2doLugar.HasValue)
            {
                ModelState.AddModelError("Nota2doLugar", "Debe ingresar una nota para el 2do lugar.");
            }
            // Si seleccionó inscripción para 3er lugar, la nota es obligatoria
            if (viewModel.Inscripcion3erLugarId.HasValue && !viewModel.Nota3erLugar.HasValue)
            {
                ModelState.AddModelError("Nota3erLugar", "Debe ingresar una nota para el 3er lugar.");
            }
            // No puede haber 3er lugar sin 2do lugar
            if (viewModel.Inscripcion3erLugarId.HasValue && !viewModel.Inscripcion2doLugarId.HasValue)
            {
                ModelState.AddModelError("", "No puede registrar un 3er lugar sin haber registrado un 2do lugar.");
            }

            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var usuarioId = GetUsuarioId();

                    // Buscar el registro de juez para el usuario actual (puede ser null si es Admin/Coordinador)
                    var juezRecord = await _context.Jueces.FirstOrDefaultAsync(j => j.JuezId == usuarioId);

                    var nuevoResultado = new ResultadosEvento
                    {
                        EventoId = viewModel.EventoId,
                        EstadoResultados = "Publicado",
                        ResolucionFinal = viewModel.ResolucionFinal,
                        JuezResponsableUsuarioId = juezRecord?.JuezId, // null si el usuario no es Juez
                        UsuarioCreacion = usuarioId,
                        UsuarioModificacion = usuarioId,
                        FechaPublicacion = DateTime.Now
                    };

                    _context.ResultadosEventos.Add(nuevoResultado);
                    await _context.SaveChangesAsync();

                    // Solo agregar los lugares que tienen inscripción asignada
                    var ganadores = new List<ResultadosGanadore>();
                    ganadores.Add(new ResultadosGanadore { ResultadoEventoId = nuevoResultado.ResultadoEventoId, Posicion = 1, InscripcionId = viewModel.Inscripcion1erLugarId, Nota = viewModel.Nota1erLugar, UsuarioCreacion = usuarioId });

                    if (viewModel.Inscripcion2doLugarId.HasValue)
                        ganadores.Add(new ResultadosGanadore { ResultadoEventoId = nuevoResultado.ResultadoEventoId, Posicion = 2, InscripcionId = viewModel.Inscripcion2doLugarId.Value, Nota = viewModel.Nota2doLugar!.Value, UsuarioCreacion = usuarioId });

                    if (viewModel.Inscripcion3erLugarId.HasValue)
                        ganadores.Add(new ResultadosGanadore { ResultadoEventoId = nuevoResultado.ResultadoEventoId, Posicion = 3, InscripcionId = viewModel.Inscripcion3erLugarId.Value, Nota = viewModel.Nota3erLugar!.Value, UsuarioCreacion = usuarioId });

                    _context.ResultadosGanadores.AddRange(ganadores);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Ocurrió un error al guardar los resultados. Intente nuevamente.");
                }
            }

            var uid = GetUsuarioId();
            var juezActual = _context.Usuarios.Include(u => u.Persona).FirstOrDefault(u => u.UsuarioId == uid);
            ViewData["JuezNombre"] = juezActual?.Persona != null ? $"{juezActual.Persona.Nombres} {juezActual.Persona.Apellidos}" : "Juez Actual";

            // Mantener el mismo filtro: eventos sin resultado activo
            var eventosConResultadoR = _context.ResultadosEventos.Where(r => r.EsActivo).Select(r => r.EventoId).Distinct();
            var eventosSinResultadoR = _context.Eventos.Where(e => e.EsActivo && !eventosConResultadoR.Contains(e.EventoId));
            ViewData["EventoId"] = new SelectList(eventosSinResultadoR, "EventoId", "NombreEvento", viewModel.EventoId);
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

            // Validar duplicados (solo entre los que tienen valor)
            if (viewModel.Inscripcion2doLugarId.HasValue &&
                viewModel.Inscripcion1erLugarId == viewModel.Inscripcion2doLugarId)
            {
                ModelState.AddModelError("", "El 2do lugar no puede ser el mismo proyecto que el 1er lugar.");
            }
            if (viewModel.Inscripcion3erLugarId.HasValue &&
                viewModel.Inscripcion1erLugarId == viewModel.Inscripcion3erLugarId)
            {
                ModelState.AddModelError("", "El 3er lugar no puede ser el mismo proyecto que el 1er lugar.");
            }
            if (viewModel.Inscripcion2doLugarId.HasValue && viewModel.Inscripcion3erLugarId.HasValue &&
                viewModel.Inscripcion2doLugarId == viewModel.Inscripcion3erLugarId)
            {
                ModelState.AddModelError("", "El 2do y 3er lugar no pueden ser el mismo proyecto.");
            }

            if (viewModel.Inscripcion2doLugarId.HasValue && !viewModel.Nota2doLugar.HasValue)
                ModelState.AddModelError("Nota2doLugar", "Debe ingresar una nota para el 2do lugar.");

            if (viewModel.Inscripcion3erLugarId.HasValue && !viewModel.Nota3erLugar.HasValue)
                ModelState.AddModelError("Nota3erLugar", "Debe ingresar una nota para el 3er lugar.");

            if (viewModel.Inscripcion3erLugarId.HasValue && !viewModel.Inscripcion2doLugarId.HasValue)
                ModelState.AddModelError("", "No puede registrar un 3er lugar sin haber registrado un 2do lugar.");

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

                    // Remover ganadores anteriores y reasignar solo los que tienen valor
                    _context.ResultadosGanadores.RemoveRange(resultadoDB.ResultadosGanadores);

                    resultadoDB.ResultadosGanadores.Clear();
                    resultadoDB.ResultadosGanadores.Add(new ResultadosGanadore { ResultadoEventoId = id, Posicion = 1, InscripcionId = viewModel.Inscripcion1erLugarId, Nota = viewModel.Nota1erLugar, UsuarioCreacion = usuarioId });

                    if (viewModel.Inscripcion2doLugarId.HasValue)
                        resultadoDB.ResultadosGanadores.Add(new ResultadosGanadore { ResultadoEventoId = id, Posicion = 2, InscripcionId = viewModel.Inscripcion2doLugarId.Value, Nota = viewModel.Nota2doLugar!.Value, UsuarioCreacion = usuarioId });

                    if (viewModel.Inscripcion3erLugarId.HasValue)
                        resultadoDB.ResultadosGanadores.Add(new ResultadosGanadore { ResultadoEventoId = id, Posicion = 3, InscripcionId = viewModel.Inscripcion3erLugarId.Value, Nota = viewModel.Nota3erLugar!.Value, UsuarioCreacion = usuarioId });

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
            ViewData["EventoId"] = new SelectList(_context.Eventos.Where(e => e.EsActivo), "EventoId", "NombreEvento", viewModel.EventoId);
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
            if (!EsAdminOCoord()) return StatusCode(403);

            var resultadosEvento = await _context.ResultadosEventos
                .Include(r => r.ResultadosGanadores)
                .FirstOrDefaultAsync(m => m.ResultadoEventoId == id);

            if (resultadosEvento != null)
            {
                // Borrado lógico: se mantiene el historial de resultados en la BD
                resultadosEvento.EsActivo = false;
                _context.ResultadosEventos.Update(resultadosEvento);
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

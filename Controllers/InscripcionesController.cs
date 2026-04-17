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
    public class InscripcionesController : Controller
    {
        private readonly AppDbContext _context;

        public InscripcionesController(AppDbContext context)
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
            return rol.Contains("Administrador", StringComparison.OrdinalIgnoreCase)
                || rol.Contains("Coordinador", StringComparison.OrdinalIgnoreCase);
        }

        private bool EsEstudiante()
        {
            return GetRol().Contains("Estudiante", StringComparison.OrdinalIgnoreCase);
        }

        private bool EsTutor()
        {
            return GetRol().Contains("Tutor", StringComparison.OrdinalIgnoreCase);
        }

        private bool EsJuez()
        {
            return GetRol().Contains("Juez", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = GetUsuarioId();
            var esAdminOCoord = EsAdminOCoord();

            IQueryable<Inscripcione> query = _context.Inscripciones
                .Where(i => i.EsActivo)
                .Include(i => i.Evento)
                .Include(i => i.LiderUsuario).ThenInclude(u => u.Persona)
                .Include(i => i.Subcategoria).ThenInclude(s => s.Categoria)
                .Include(i => i.TutorUsuario).ThenInclude(t => t.Tutor).ThenInclude(u => u.Persona)
                .Include(i => i.InscripcionIntegrantes).ThenInclude(ii => ii.EstudianteUsuario).ThenInclude(e => e.EstudianteNavigation).ThenInclude(u => u.Persona);

            // Admin, Coord y Juez ven TODAS las inscripciones. Estudiantes y Tutores solo ven las suyas.
            if (!esAdminOCoord && !EsJuez())
            {
                if (EsTutor())
                {
                    query = query.Where(i => i.TutorUsuarioId == usuarioId);
                }
                else
                {
                    // Comportamiento para estudiantes: lider + integrantes
                    query = query.Where(i => i.LiderUsuarioId == usuarioId
                        || i.InscripcionIntegrantes.Any(ii => ii.EstudianteUsuarioId == usuarioId));
                }
            }

            var inscripciones = await query.ToListAsync();

            ViewData["EsAdminOCoord"] = esAdminOCoord;
            ViewData["EsEstudiante"] = EsEstudiante();
            ViewData["EsJuez"] = EsJuez();
            ViewData["UsuarioId"] = usuarioId;

            return View(inscripciones);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var inscripcion = await _context.Inscripciones
                .Include(i => i.Evento)
                .Include(i => i.LiderUsuario).ThenInclude(u => u.Persona)
                .Include(i => i.Subcategoria).ThenInclude(s => s.Categoria)
                .Include(i => i.TutorUsuario).ThenInclude(t => t.Tutor).ThenInclude(u => u.Persona)
                .Include(i => i.InscripcionIntegrantes).ThenInclude(ii => ii.EstudianteUsuario).ThenInclude(e => e.EstudianteNavigation).ThenInclude(u => u.Persona)
                .FirstOrDefaultAsync(i => i.InscripcionId == id);
            if (inscripcion == null) return NotFound();

            return View(inscripcion);
        }

        public IActionResult Create()
        {
            // Solo Estudiantes y Admin/Coord pueden crear inscripciones. Jueces no.
            if (EsJuez()) return StatusCode(403);
            var usuarioId = GetUsuarioId();
            var lider = _context.Usuarios.Include(u => u.Persona).FirstOrDefault(u => u.UsuarioId == usuarioId);

            ViewData["LiderNombre"] = lider?.Persona != null ? $"{lider.Persona.Nombres} {lider.Persona.Apellidos}" : "Usuario actual";
            ViewData["EventoId"] = new SelectList(_context.Eventos.Where(e => e.EsActivo), "EventoId", "NombreEvento");
            ViewData["CategoriaId"] = new SelectList(_context.Categorias.Where(c => c.EsActivo), "CategoriaId", "Nombre");
            ViewData["SubcategoriaId"] = new SelectList(_context.Subcategorias.Where(s => s.EsActivo), "SubcategoriaId", "Nombre");
            CargarTutores();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("EventoId,SubcategoriaId,TituloProyecto,DescripcionProyecto")] Inscripcione inscripcion,
            long? Integrante1Id, long? Integrante2Id)
        {
            ModelState.Remove("Evento");
            ModelState.Remove("LiderUsuario");
            ModelState.Remove("Subcategoria");
            ModelState.Remove("TutorUsuario");
            ModelState.Remove("InscripcionIntegrantes");
            ModelState.Remove("ResultadosGanadores");
            ModelState.Remove("EstadoInscripcion");

            var usuarioId = GetUsuarioId() ?? 0;

            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(inscripcion.DescripcionProyecto))
            {
                ModelState.AddModelError("DescripcionProyecto", "La descripción del proyecto es obligatoria.");
            }

            // Validar que el evento no esté finalizado
            var evento = await _context.Eventos.FindAsync(inscripcion.EventoId);
            if (evento != null && evento.EstadoEvento == "Finalizado")
            {
                ModelState.AddModelError("EventoId", "No se puede inscribir a un evento que ya está finalizado.");
            }

            // Validar que los integrantes no sean el mismo usuario (el líder)
            if (Integrante1Id.HasValue && Integrante1Id.Value == usuarioId)
            {
                ModelState.AddModelError("", "No puede agregarse a usted mismo como integrante 1.");
            }
            if (Integrante2Id.HasValue && Integrante2Id.Value == usuarioId)
            {
                ModelState.AddModelError("", "No puede agregarse a usted mismo como integrante 2.");
            }

            // Validar de no permitir duplicados: El estudiante ya tiene un proyecto en este evento
            bool existeInscripcion = await _context.Inscripciones
                .AnyAsync(i => i.EventoId == inscripcion.EventoId && i.LiderUsuarioId == usuarioId);
            
            if (existeInscripcion)
            {
                ModelState.AddModelError("", "Usted ya tiene un proyecto inscrito en este evento.");
            }

            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    inscripcion.LiderUsuarioId = usuarioId;
                    inscripcion.EstadoInscripcion = "Pendiente";
                    inscripcion.UsuarioCreacion = usuarioId;
                    inscripcion.UsuarioModificacion = usuarioId;

                    _context.Add(inscripcion);
                    await _context.SaveChangesAsync();

                    if (Integrante1Id.HasValue)
                    {
                        _context.InscripcionIntegrantes.Add(new InscripcionIntegrante
                        {
                            InscripcionId = inscripcion.InscripcionId,
                            EstudianteUsuarioId = Integrante1Id.Value,
                            UsuarioCreacion = usuarioId
                        });
                    }
                    if (Integrante2Id.HasValue)
                    {
                        _context.InscripcionIntegrantes.Add(new InscripcionIntegrante
                        {
                            InscripcionId = inscripcion.InscripcionId,
                            EstudianteUsuarioId = Integrante2Id.Value,
                            UsuarioCreacion = usuarioId
                        });
                    }
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Ocurrió un error al guardar la inscripción. Inténtelo nuevamente.");
                }
            }

            var uid = GetUsuarioId();
            var lider = _context.Usuarios.Include(u => u.Persona).FirstOrDefault(u => u.UsuarioId == uid);
            ViewData["LiderNombre"] = lider?.Persona != null ? $"{lider.Persona.Nombres} {lider.Persona.Apellidos}" : "Usuario actual";
            ViewData["EventoId"] = new SelectList(_context.Eventos.Where(e => e.EsActivo), "EventoId", "NombreEvento", inscripcion.EventoId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias.Where(c => c.EsActivo), "CategoriaId", "Nombre");
            ViewData["SubcategoriaId"] = new SelectList(_context.Subcategorias.Where(s => s.EsActivo), "SubcategoriaId", "Nombre", inscripcion.SubcategoriaId);
            CargarTutores();
            return View(inscripcion);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            // Solo Admin/Coord puede editar (asignar tutor, aprobar). Juez y Estudiante no.
            if (!EsAdminOCoord()) return StatusCode(403);
            if (id == null) return NotFound();

            var inscripcion = await _context.Inscripciones
                .Include(i => i.LiderUsuario).ThenInclude(u => u.Persona)
                .Include(i => i.Subcategoria)
                .Include(i => i.InscripcionIntegrantes).ThenInclude(ii => ii.EstudianteUsuario).ThenInclude(e => e.EstudianteNavigation).ThenInclude(u => u.Persona)
                .FirstOrDefaultAsync(i => i.InscripcionId == id);
            if (inscripcion == null) return NotFound();

            ViewData["LiderNombre"] = inscripcion.LiderUsuario?.Persona != null
                ? $"{inscripcion.LiderUsuario.Persona.Nombres} {inscripcion.LiderUsuario.Persona.Apellidos}" : "—";
            ViewData["EsAdminOCoord"] = EsAdminOCoord();
            ViewData["EventoId"] = new SelectList(_context.Eventos.Where(e => e.EsActivo), "EventoId", "NombreEvento", inscripcion.EventoId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre",
                inscripcion.Subcategoria?.CategoriaId);
            ViewData["SubcategoriaId"] = new SelectList(
                _context.Subcategorias.Where(s => s.EsActivo && s.CategoriaId == (inscripcion.Subcategoria != null ? inscripcion.Subcategoria.CategoriaId : 0)),
                "SubcategoriaId", "Nombre", inscripcion.SubcategoriaId);
            CargarTutores(inscripcion.TutorUsuarioId);

            return View(inscripcion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id,
            [Bind("InscripcionId,EventoId,SubcategoriaId,TituloProyecto,DescripcionProyecto,TutorUsuarioId,EstadoInscripcion,LiderUsuarioId,Justificacion")] Inscripcione inscripcion)
        {
            if (id != inscripcion.InscripcionId) return NotFound();

            ModelState.Remove("Evento");
            ModelState.Remove("LiderUsuario");
            ModelState.Remove("Subcategoria");
            ModelState.Remove("TutorUsuario");
            ModelState.Remove("InscripcionIntegrantes");
            ModelState.Remove("ResultadosGanadores");

            // Validar que no se puede aprobar sin tutor
            if (inscripcion.EstadoInscripcion == "Aprobado" && !inscripcion.TutorUsuarioId.HasValue)
            {
                ModelState.AddModelError("TutorUsuarioId", "No se puede aprobar la inscripción sin asignar un tutor.");
            }

            // Validar justificación obligatoria al aprobar o rechazar
            if ((inscripcion.EstadoInscripcion == "Aprobado" || inscripcion.EstadoInscripcion == "Rechazado")
                && string.IsNullOrWhiteSpace(inscripcion.Justificacion))
            {
                ModelState.AddModelError("Justificacion", "La justificación es obligatoria al aprobar o rechazar una inscripción.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioId = GetUsuarioId();
                    inscripcion.UsuarioModificacion = usuarioId;

                    _context.Update(inscripcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Inscripciones.Any(i => i.InscripcionId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["EsAdminOCoord"] = EsAdminOCoord();
            ViewData["EventoId"] = new SelectList(_context.Eventos.Where(e => e.EsActivo), "EventoId", "NombreEvento", inscripcion.EventoId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias.Where(c => c.EsActivo), "CategoriaId", "Nombre");
            ViewData["SubcategoriaId"] = new SelectList(_context.Subcategorias.Where(s => s.EsActivo), "SubcategoriaId", "Nombre", inscripcion.SubcategoriaId);
            CargarTutores(inscripcion.TutorUsuarioId);
            return View(inscripcion);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (!EsAdminOCoord()) return StatusCode(403);
            if (id == null) return NotFound();

            var inscripcion = await _context.Inscripciones
                .Include(i => i.Evento)
                .Include(i => i.LiderUsuario).ThenInclude(u => u.Persona)
                .Include(i => i.Subcategoria).ThenInclude(s => s.Categoria)
                .FirstOrDefaultAsync(i => i.InscripcionId == id);
            if (inscripcion == null) return NotFound();

            return View(inscripcion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (!EsAdminOCoord()) return StatusCode(403);
            var inscripcion = await _context.Inscripciones
                .Include(i => i.InscripcionIntegrantes)
                .FirstOrDefaultAsync(i => i.InscripcionId == id);
            if (inscripcion != null)
            {
                // Borrado lógico: se mantiene el historial en la BD
                inscripcion.EsActivo = false;
                _context.Inscripciones.Update(inscripcion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public JsonResult GetSubcategorias(int categoriaId)
        {
            var subcategorias = _context.Subcategorias
                .Where(s => s.CategoriaId == categoriaId && s.EsActivo)
                .Select(s => new { s.SubcategoriaId, s.Nombre })
                .ToList();
            return Json(subcategorias);
        }

        [HttpGet]
        public JsonResult BuscarEstudiantes(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return Json(new List<object>());

            var usuarioId = GetUsuarioId();

            var estudiantes = _context.Estudiantes
                .Include(e => e.EstudianteNavigation).ThenInclude(u => u.Persona)
                .Where(e => e.EstudianteNavigation.Persona != null &&
                    e.EstudianteId != usuarioId &&
                    (e.EstudianteNavigation.Persona.Nombres.Contains(term) ||
                     e.EstudianteNavigation.Persona.Apellidos.Contains(term) ||
                     e.EstudianteNavigation.Persona.Documento.Contains(term)))
                .Take(10)
                .Select(e => new
                {
                    id = e.EstudianteId,
                    nombre = e.EstudianteNavigation.Persona!.Nombres + " " + e.EstudianteNavigation.Persona.Apellidos,
                    documento = e.EstudianteNavigation.Persona.Documento
                })
                .ToList();

            return Json(estudiantes);
        }

        private void CargarTutores(long? selectedId = null)
        {
            var tutores = _context.Tutores
                .Include(t => t.Tutor).ThenInclude(u => u.Persona)
                .Where(t => t.Tutor.Persona != null)
                .Select(t => new SelectListItem
                {
                    Value = t.TutorId.ToString(),
                    Text = t.Tutor.Persona!.Nombres + " " + t.Tutor.Persona.Apellidos
                })
                .ToList();

            ViewData["TutorUsuarioId"] = new SelectList(tutores, "Value", "Text", selectedId?.ToString());
        }
    }
}

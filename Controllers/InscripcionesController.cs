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

        public async Task<IActionResult> Index()
        {
            var usuarioId = GetUsuarioId();
            var esAdminOCoord = EsAdminOCoord();

            IQueryable<Inscripcione> query = _context.Inscripciones
                .Include(i => i.Evento)
                .Include(i => i.LiderUsuario).ThenInclude(u => u.Persona)
                .Include(i => i.Subcategoria).ThenInclude(s => s.Categoria)
                .Include(i => i.TutorUsuario).ThenInclude(t => t.Tutor).ThenInclude(u => u.Persona)
                .Include(i => i.InscripcionIntegrantes).ThenInclude(ii => ii.EstudianteUsuario).ThenInclude(e => e.EstudianteNavigation).ThenInclude(u => u.Persona);

            if (!esAdminOCoord)
            {
                query = query.Where(i => i.LiderUsuarioId == usuarioId
                    || i.InscripcionIntegrantes.Any(ii => ii.EstudianteUsuarioId == usuarioId));
            }

            var inscripciones = await query.ToListAsync();

            ViewData["EsAdminOCoord"] = esAdminOCoord;
            ViewData["EsEstudiante"] = EsEstudiante();
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
            var usuarioId = GetUsuarioId();
            var lider = _context.Usuarios.Include(u => u.Persona).FirstOrDefault(u => u.UsuarioId == usuarioId);

            ViewData["LiderNombre"] = lider?.Persona != null ? $"{lider.Persona.Nombres} {lider.Persona.Apellidos}" : "Usuario actual";
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "NombreEvento");
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre");
            ViewData["SubcategoriaId"] = new SelectList(_context.Subcategorias, "SubcategoriaId", "Nombre");
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

            if (ModelState.IsValid)
            {
                var usuarioId = GetUsuarioId();
                inscripcion.LiderUsuarioId = usuarioId ?? 0;
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

                return RedirectToAction(nameof(Index));
            }

            var uid = GetUsuarioId();
            var lider = _context.Usuarios.Include(u => u.Persona).FirstOrDefault(u => u.UsuarioId == uid);
            ViewData["LiderNombre"] = lider?.Persona != null ? $"{lider.Persona.Nombres} {lider.Persona.Apellidos}" : "Usuario actual";
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "NombreEvento", inscripcion.EventoId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre");
            ViewData["SubcategoriaId"] = new SelectList(_context.Subcategorias, "SubcategoriaId", "Nombre", inscripcion.SubcategoriaId);
            CargarTutores();
            return View(inscripcion);
        }

        public async Task<IActionResult> Edit(long? id)
        {
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
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "NombreEvento", inscripcion.EventoId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre",
                inscripcion.Subcategoria?.CategoriaId);
            ViewData["SubcategoriaId"] = new SelectList(
                _context.Subcategorias.Where(s => s.CategoriaId == (inscripcion.Subcategoria != null ? inscripcion.Subcategoria.CategoriaId : 0)),
                "SubcategoriaId", "Nombre", inscripcion.SubcategoriaId);
            CargarTutores(inscripcion.TutorUsuarioId);

            return View(inscripcion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id,
            [Bind("InscripcionId,EventoId,SubcategoriaId,TituloProyecto,DescripcionProyecto,TutorUsuarioId,EstadoInscripcion,LiderUsuarioId")] Inscripcione inscripcion)
        {
            if (id != inscripcion.InscripcionId) return NotFound();

            ModelState.Remove("Evento");
            ModelState.Remove("LiderUsuario");
            ModelState.Remove("Subcategoria");
            ModelState.Remove("TutorUsuario");
            ModelState.Remove("InscripcionIntegrantes");
            ModelState.Remove("ResultadosGanadores");

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioId = GetUsuarioId();
                    inscripcion.UsuarioModificacion = usuarioId;

                    if (inscripcion.TutorUsuarioId.HasValue && inscripcion.EstadoInscripcion == "Pendiente")
                    {
                        inscripcion.EstadoInscripcion = "Aprobado";
                    }

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
            ViewData["EventoId"] = new SelectList(_context.Eventos, "EventoId", "NombreEvento", inscripcion.EventoId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre");
            ViewData["SubcategoriaId"] = new SelectList(_context.Subcategorias, "SubcategoriaId", "Nombre", inscripcion.SubcategoriaId);
            CargarTutores(inscripcion.TutorUsuarioId);
            return View(inscripcion);
        }

        public async Task<IActionResult> Delete(long? id)
        {
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
            var inscripcion = await _context.Inscripciones
                .Include(i => i.InscripcionIntegrantes)
                .FirstOrDefaultAsync(i => i.InscripcionId == id);
            if (inscripcion != null)
            {
                _context.InscripcionIntegrantes.RemoveRange(inscripcion.InscripcionIntegrantes);
                _context.Inscripciones.Remove(inscripcion);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public JsonResult GetSubcategorias(int categoriaId)
        {
            var subcategorias = _context.Subcategorias
                .Where(s => s.CategoriaId == categoriaId)
                .Select(s => new { s.SubcategoriaId, s.Nombre })
                .ToList();
            return Json(subcategorias);
        }

        [HttpGet]
        public JsonResult BuscarEstudiantes(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return Json(new List<object>());

            var estudiantes = _context.Estudiantes
                .Include(e => e.EstudianteNavigation).ThenInclude(u => u.Persona)
                .Where(e => e.EstudianteNavigation.Persona != null &&
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

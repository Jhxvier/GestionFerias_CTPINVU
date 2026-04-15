using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionFerias_CTPINVU.Data;
using ClosedXML.Excel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace GestionFerias_CTPINVU.Controllers
{
    public class ReportesController : Controller
    {
        private readonly AppDbContext _context;

        public ReportesController(AppDbContext context)
        {
            _context = context;
        }

        private bool EsAdminOCoord()
        {
            var rol = HttpContext.Session.GetString("Rol") ?? "";
            return rol.Contains("Administrador", StringComparison.OrdinalIgnoreCase) ||
                   rol.Contains("Coordinador", StringComparison.OrdinalIgnoreCase);
        }

        public IActionResult Index()
        {
            if (!EsAdminOCoord())
                return Unauthorized();

            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetEventosAJAX()
        {
            var eventos = await _context.Eventos
                .OrderByDescending(e => e.FechaInicio)
                .Select(e => new { id = e.EventoId, texto = $"EVT-{e.EventoId} - {e.NombreEvento} ({e.FechaInicio.Year})" })
                .ToListAsync();
            return Json(eventos);
        }

        [HttpGet]
        public async Task<JsonResult> GetEstudiantesAJAX()
        {
            var estudiantes = await _context.Usuarios
                .Include(u => u.Persona)
                .Where(u => u.Estudiante != null)
                .Select(u => new { id = u.UsuarioId, texto = $"{u.Persona.Documento} - {u.Persona.Nombres} {u.Persona.Apellidos}" })
                .ToListAsync();
            return Json(estudiantes);
        }

        // --- 1. Participantes por Evento ---
        private IQueryable<GestionFerias_CTPINVU.Models.Inscripcione> QueryParticipantes(long eventoId)
        {
            return _context.Inscripciones
                .Include(i => i.Evento)
                .Include(i => i.Subcategoria).ThenInclude(s => s.Categoria)
                .Include(i => i.LiderUsuario).ThenInclude(u => u.Persona)
                .Where(i => i.EventoId == eventoId && i.EstadoInscripcion == "Aprobado");
        }

        public async Task<IActionResult> ParticipantesPdf(long eventoId)
        {
            if (!EsAdminOCoord()) return Unauthorized();
            var participantes = await QueryParticipantes(eventoId).ToListAsync();
            var evento = await _context.Eventos.FindAsync(eventoId);
            ViewBag.ReporteTitulo = $"Participantes Aprobados - {evento?.NombreEvento}";
            return View("PrintParticipantes", participantes);
        }

        public async Task<IActionResult> ParticipantesExcel(long eventoId)
        {
            if (!EsAdminOCoord()) return Unauthorized();
            var participantes = await QueryParticipantes(eventoId).ToListAsync();
            var evento = await _context.Eventos.FindAsync(eventoId);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Participantes");
            worksheet.Cell(1, 1).Value = $"Lista de Participantes - {evento?.NombreEvento}";
            worksheet.Range("A1:D1").Merge().Style.Font.SetBold().Font.FontSize = 14;

            worksheet.Cell(3, 1).Value = "Proyecto";
            worksheet.Cell(3, 2).Value = "Líder";
            worksheet.Cell(3, 3).Value = "Categoría";
            worksheet.Cell(3, 4).Value = "Subcategoría";
            worksheet.Range("A3:D3").Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.LightGray);

            int row = 4;
            foreach (var p in participantes)
            {
                worksheet.Cell(row, 1).Value = p.TituloProyecto;
                worksheet.Cell(row, 2).Value = p.LiderUsuario?.Persona != null ? $"{p.LiderUsuario.Persona.Nombres} {p.LiderUsuario.Persona.Apellidos}" : "";
                worksheet.Cell(row, 3).Value = p.Subcategoria?.Categoria?.Nombre;
                worksheet.Cell(row, 4).Value = p.Subcategoria?.Nombre;
                row++;
            }
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Participantes_EVT{eventoId}.xlsx");
        }


        // --- 2. Resultados por Evento ---
        private IQueryable<GestionFerias_CTPINVU.Models.ResultadosGanadore> QueryResultados(long eventoId)
        {
            return _context.ResultadosGanadores
                .Include(g => g.ResultadoEvento).ThenInclude(r => r.Evento)
                .Include(g => g.Inscripcion).ThenInclude(i => i.LiderUsuario).ThenInclude(u => u.Persona)
                .Include(g => g.Inscripcion).ThenInclude(i => i.Subcategoria).ThenInclude(s => s.Categoria)
                .Where(g => g.ResultadoEvento.EventoId == eventoId)
                .OrderBy(g => g.Posicion);
        }

        public async Task<IActionResult> ResultadosPdf(long eventoId)
        {
            if (!EsAdminOCoord()) return Unauthorized();
            var resultados = await QueryResultados(eventoId).ToListAsync();
            var evento = await _context.Eventos.FindAsync(eventoId);
            ViewBag.ReporteTitulo = $"Resultados Oficiales - {evento?.NombreEvento}";
            return View("PrintResultados", resultados);
        }

        public async Task<IActionResult> ResultadosExcel(long eventoId)
        {
            if (!EsAdminOCoord()) return Unauthorized();
            var resultados = await QueryResultados(eventoId).ToListAsync();
            var evento = await _context.Eventos.FindAsync(eventoId);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Resultados");
            worksheet.Cell(1, 1).Value = $"Resultados Oficiales - {evento?.NombreEvento}";
            worksheet.Range("A1:D1").Merge().Style.Font.SetBold().Font.FontSize = 14;

            worksheet.Cell(3, 1).Value = "Lugar";
            worksheet.Cell(3, 2).Value = "Nota";
            worksheet.Cell(3, 3).Value = "Proyecto";
            worksheet.Cell(3, 4).Value = "Líder";
            worksheet.Range("A3:D3").Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.LightGray);

            int row = 4;
            foreach (var r in resultados)
            {
                worksheet.Cell(row, 1).Value = $"{r.Posicion}° Lugar";
                worksheet.Cell(row, 2).Value = r.Nota;
                worksheet.Cell(row, 3).Value = r.Inscripcion?.TituloProyecto;
                worksheet.Cell(row, 4).Value = r.Inscripcion?.LiderUsuario?.Persona != null ? $"{r.Inscripcion.LiderUsuario.Persona.Nombres} {r.Inscripcion.LiderUsuario.Persona.Apellidos}" : "";
                row++;
            }
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Resultados_EVT{eventoId}.xlsx");
        }


        // --- 3. Historial de eventos por año ---
        private IQueryable<GestionFerias_CTPINVU.Models.Evento> QueryHistorialAnio(int anio)
        {
            return _context.Eventos
                .Where(e => e.FechaInicio.Year == anio)
                .OrderBy(e => e.FechaInicio);
        }

        public async Task<IActionResult> HistorialAnioPdf(int anio)
        {
            if (!EsAdminOCoord()) return Unauthorized();
            var eventos = await QueryHistorialAnio(anio).ToListAsync();
            ViewBag.ReporteTitulo = $"Historial de Eventos - Año {anio}";
            return View("PrintHistorialAnio", eventos);
        }

        public async Task<IActionResult> HistorialAnioExcel(int anio)
        {
            if (!EsAdminOCoord()) return Unauthorized();
            var eventos = await QueryHistorialAnio(anio).ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add($"Eventos {anio}");
            worksheet.Cell(1, 1).Value = $"Historial de Eventos - Año {anio}";
            worksheet.Range("A1:D1").Merge().Style.Font.SetBold().Font.FontSize = 14;

            worksheet.Cell(3, 1).Value = "ID Evento";
            worksheet.Cell(3, 2).Value = "Nombre del Evento";
            worksheet.Cell(3, 3).Value = "Fecha Inicio";
            worksheet.Cell(3, 4).Value = "Estado";
            worksheet.Range("A3:D3").Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.LightGray);

            int row = 4;
            foreach (var e in eventos)
            {
                worksheet.Cell(row, 1).Value = $"EVT-{e.EventoId}";
                worksheet.Cell(row, 2).Value = e.NombreEvento;
                worksheet.Cell(row, 3).Value = e.FechaInicio.ToString("dd/MM/yyyy");
                worksheet.Cell(row, 4).Value = e.EstadoEvento;
                row++;
            }
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Eventos_Anio_{anio}.xlsx");
        }


        // --- 4. Historial por estudiante ---
        private IQueryable<GestionFerias_CTPINVU.Models.Inscripcione> QueryHistorialEstudiante(long estudianteId)
        {
            // Busca donde es líder o integrante
            return _context.Inscripciones
                .Include(i => i.Evento)
                .Include(i => i.ResultadosGanadores)
                .Where(i => i.LiderUsuarioId == estudianteId || i.InscripcionIntegrantes.Any(integ => integ.EstudianteUsuarioId == estudianteId))
                .OrderByDescending(i => i.FechaCreacion);
        }

        public async Task<IActionResult> HistorialEstudiantePdf(long estudianteId)
        {
            if (!EsAdminOCoord()) return Unauthorized();
            var inscripciones = await QueryHistorialEstudiante(estudianteId).ToListAsync();
            var user = await _context.Usuarios.Include(u => u.Persona).FirstOrDefaultAsync(u => u.UsuarioId == estudianteId);
            ViewBag.ReporteTitulo = $"Historial de Participación - {(user?.Persona != null ? user.Persona.Nombres + " " + user.Persona.Apellidos : "ID " + estudianteId)}";
            return View("PrintHistorialEstudiante", inscripciones);
        }

        public async Task<IActionResult> HistorialEstudianteExcel(long estudianteId)
        {
            if (!EsAdminOCoord()) return Unauthorized();
            var inscripciones = await QueryHistorialEstudiante(estudianteId).ToListAsync();
            var user = await _context.Usuarios.Include(u => u.Persona).FirstOrDefaultAsync(u => u.UsuarioId == estudianteId);
            string studentName = user?.Persona != null ? $"{user.Persona.Nombres} {user.Persona.Apellidos}" : $"ID {estudianteId}";

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Participaciones");
            worksheet.Cell(1, 1).Value = $"Historial de Estudiante: {studentName}";
            worksheet.Range("A1:D1").Merge().Style.Font.SetBold().Font.FontSize = 14;

            worksheet.Cell(3, 1).Value = "Evento";
            worksheet.Cell(3, 2).Value = "Proyecto";
            worksheet.Cell(3, 3).Value = "Estado Inscripción";
            worksheet.Cell(3, 4).Value = "Premiaciones";
            worksheet.Range("A3:D3").Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.LightGray);

            int row = 4;
            foreach (var i in inscripciones)
            {
                var premiacion = i.ResultadosGanadores.FirstOrDefault();
                string lugarStr = premiacion != null ? $"{premiacion.Posicion}° Lugar" : "Participante";

                worksheet.Cell(row, 1).Value = i.Evento?.NombreEvento;
                worksheet.Cell(row, 2).Value = i.TituloProyecto;
                worksheet.Cell(row, 3).Value = i.EstadoInscripcion;
                worksheet.Cell(row, 4).Value = lugarStr;
                row++;
            }
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Historial_{(user?.Persona?.Documento ?? estudianteId.ToString())}.xlsx");
        }

    }
}

using System.ComponentModel.DataAnnotations;
using GestionFerias_CTPINVU.Models;

namespace GestionFerias_CTPINVU.Models
{
    public class ResultadoEventoViewModel
    {
        public long ResultadoEventoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un evento.")]
        public long EventoId { get; set; }

        public string EstadoResultados { get; set; } = "Pendiente";

        public long? JuezResponsableUsuarioId { get; set; }

        [Display(Name = "Resolución Final")]
        [Required(ErrorMessage = "La resolución final es obligatoria.")]
        public string ResolucionFinal { get; set; } = null!;

        // --- 1er Lugar (OBLIGATORIO) ---
        [Required(ErrorMessage = "Debe seleccionar una inscripción para el 1er lugar.")]
        public long Inscripcion1erLugarId { get; set; }

        [Required(ErrorMessage = "Debe ingresar una nota para el 1er lugar.")]
        [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100.")]
        public decimal Nota1erLugar { get; set; }

        // --- 2do Lugar (OPCIONAL) ---
        public long? Inscripcion2doLugarId { get; set; }

        [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100.")]
        public decimal? Nota2doLugar { get; set; }

        // --- 3er Lugar (OPCIONAL) ---
        public long? Inscripcion3erLugarId { get; set; }

        [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100.")]
        public decimal? Nota3erLugar { get; set; }
    }
}

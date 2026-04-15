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
        public string? ResolucionFinal { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una inscripción para el 1er lugar.")]
        public long Inscripcion1erLugarId { get; set; }

        [Required(ErrorMessage = "Debe ingresar una nota para el 1er lugar.")]
        [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100.")]
        public decimal Nota1erLugar { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una inscripción para el 2do lugar.")]
        public long Inscripcion2doLugarId { get; set; }

        [Required(ErrorMessage = "Debe ingresar una nota para el 2do lugar.")]
        [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100.")]
        public decimal Nota2doLugar { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una inscripción para el 3er lugar.")]
        public long Inscripcion3erLugarId { get; set; }

        [Required(ErrorMessage = "Debe ingresar una nota para el 3er lugar.")]
        [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100.")]
        public decimal Nota3erLugar { get; set; }
    }
}

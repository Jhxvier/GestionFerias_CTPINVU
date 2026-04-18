using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GestionFerias_CTPINVU.ViewModels
{
    public class ConfiguracionInicioViewModel
    {
        [Required(ErrorMessage = "El año es requerido.")]
        [Display(Name = "Año Escolar")]
        public string Anio { get; set; } = "2026";

        [Display(Name = "Nueva Imagen (Formato válido: Jpg, Png, Webp)")]
        public IFormFile? ImagenNueva { get; set; }

        public string? ImagenActual { get; set; }
    }
}

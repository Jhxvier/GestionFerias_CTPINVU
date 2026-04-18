using System;
using System.ComponentModel.DataAnnotations;

namespace GestionFerias_CTPINVU.ViewModels
{
    public class MiPerfilViewModel
    {
        public long UsuarioId { get; set; }
        public string? Documento { get; set; }
        
        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        public string Apellidos { get; set; }

        public string? Correo { get; set; }
        public string? Telefono { get; set; }

        // Para cambio de contraseña:
        public string? ClaveActual { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "La contraseña debe tener mínimo 8 caracteres, mayúsculas, minúsculas, al menos 1 número y 1 carácter especial.")]
        public string? NuevaClave { get; set; }
        public string? ConfirmarNuevaClave { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace GestionFerias_CTPINVU.ViewModels
{
    public class PerfilViewModel
    {
        // Identity
        public long? UsuarioId { get; set; }
        public long? PersonaId { get; set; }
        public string Modo { get; set; } = "create"; // create / edit

        // Role Control
        [Required(ErrorMessage = "Debe asignar un rol en el sistema.")]
        public string RolSeleccionado { get; set; } // estudiante, tutor, juez, coord

        // Base Persona Data
        [Required(ErrorMessage = "El documento es obligatorio.")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        public string Apellidos { get; set; }

        public string Telefono { get; set; }

        public int? Edad { get; set; }

        public string Sexo { get; set; } // Masculino, Femenino, Otro, Prefiero no decir
        
        public string Nacionalidad { get; set; }

        // Application Data
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato de correo no es válido.")]
        public string Correo { get; set; }

        public string Clave { get; set; } // Only required to reset/create passwords

        public string EstadoUsuario { get; set; } = "Activo";

        // Role-Specific Data (Estudiante)
        public int? Grado { get; set; }

        // Role-Specific Data (Tutor)
        public string Especialidad { get; set; }
    }
}

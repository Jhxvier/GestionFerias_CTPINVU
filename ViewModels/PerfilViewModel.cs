using System;
using System.ComponentModel.DataAnnotations;

namespace GestionFerias_CTPINVU.ViewModels
{
    public class PerfilViewModel
    {
        // identificadores
        public long? UsuarioId { get; set; }
        public long? PersonaId { get; set; }
        public string Modo { get; set; } = "create"; //crear o editar

        //control de acceso
        [Required(ErrorMessage = "Debe asignar un rol en el sistema.")]
        public string RolSeleccionado { get; set; } // estudiante, tutor, juez, coord

        // Base Persona Data
        [Required(ErrorMessage = "El documento es obligatorio.")]
        public string Documento { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [RegularExpression(@"^(\+506\s?)?([24678]\d{3}[\s-]?\d{4})$", ErrorMessage = "Formato inválido. Debe ser un número de Costa Rica (ej. 8888-8888).")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateOnly? FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El sexo es obligatorio.")]
        public string Sexo { get; set; } // Masculino, Femenino, Otro, Prefiero no decir
        
        [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
        public string Nacionalidad { get; set; }

        // Application Data
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato de correo no es válido.")]
        public string Correo { get; set; }

        public string? Clave { get; set; } // solo para creación, no se muestra en edición

        public string? EstadoUsuario { get; set; } = "Activo";

        // rol específico (Estudiante)
        public int? Grado { get; set; }

        // rol específico (Tutor)
        public string? Especialidad { get; set; }
    }
}

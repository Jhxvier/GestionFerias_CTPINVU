using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class Persona
{
    public long PersonaId { get; set; }

    public long UsuarioId { get; set; }

    public string Documento { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Telefono { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string? Sexo { get; set; }

    public string? Nacionalidad { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}

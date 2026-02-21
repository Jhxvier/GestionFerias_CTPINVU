using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class Estudiante
{
    public long UsuarioId { get; set; }

    public string? Grado { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}

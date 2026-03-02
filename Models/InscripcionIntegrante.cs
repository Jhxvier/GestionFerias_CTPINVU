using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class InscripcionIntegrante
{
    public long InscripcionId { get; set; }

    public long EstudianteUsuarioId { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Estudiante EstudianteUsuario { get; set; } = null!;

    public virtual Inscripcione Inscripcion { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class Tutore
{
    public long TutorId { get; set; }

    public string Especialidad { get; set; } = null!;

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Inscripcione> Inscripciones { get; set; } = new List<Inscripcione>();

    public virtual Usuario Tutor { get; set; } = null!;
}

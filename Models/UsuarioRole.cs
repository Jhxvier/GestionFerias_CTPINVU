using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class UsuarioRole
{
    public long UsuarioId { get; set; }

    public int RolId { get; set; }

    public DateTime? FechaAsignacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Role Rol { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}

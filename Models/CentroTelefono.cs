using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class CentroTelefono
{
    public long CentroTelefonoId { get; set; }

    public long CentroId { get; set; }

    public string Telefono { get; set; } = null!;

    public string? Tipo { get; set; }

    public bool EsPrincipal { get; set; }

    public virtual CentrosEducativo Centro { get; set; } = null!;
}

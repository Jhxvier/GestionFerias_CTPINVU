using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class CentrosEducativo
{
    public long CentroId { get; set; }

    public string NombreCentro { get; set; } = null!;

    public string? NombreDirector { get; set; }

    public string? CircuitoEducativo { get; set; }

    public string? DireccionRegional { get; set; }

    public string? Direccion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<CentroTelefono> CentroTelefonos { get; set; } = new List<CentroTelefono>();

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}

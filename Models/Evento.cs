using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class Evento
{
    public long EventoId { get; set; }

    public string CodigoEvento { get; set; } = null!;

    public long CentroId { get; set; }

    public string NombreEvento { get; set; } = null!;

    public string? TipoEvento { get; set; }

    public string? Descripcion { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public string EstadoEvento { get; set; } = null!;

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual CentrosEducativo Centro { get; set; } = null!;

    public virtual ICollection<Inscripcione> Inscripciones { get; set; } = new List<Inscripcione>();

    public virtual ResultadosEvento? ResultadosEvento { get; set; }
}

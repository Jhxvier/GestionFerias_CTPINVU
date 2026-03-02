using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class ResultadosEvento
{
    public long ResultadoEventoId { get; set; }

    public long EventoId { get; set; }

    public string EstadoResultados { get; set; } = null!;

    public long? JuezResponsableUsuarioId { get; set; }

    public string? ResolucionFinal { get; set; }

    public DateTime? FechaPublicacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Evento Evento { get; set; } = null!;

    public virtual Juece? JuezResponsableUsuario { get; set; }

    public virtual ICollection<ResultadosGanadore> ResultadosGanadores { get; set; } = new List<ResultadosGanadore>();
}

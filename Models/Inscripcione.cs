using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class Inscripcione
{
    public long InscripcionId { get; set; }

    public long EventoId { get; set; }

    public long LiderUsuarioId { get; set; }

    public int SubcategoriaId { get; set; }

    public string TituloProyecto { get; set; } = null!;

    public string? DescripcionProyecto { get; set; }

    public long? TutorUsuarioId { get; set; }

    public string EstadoInscripcion { get; set; } = null!;

    public string? Justificacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public bool EsActivo { get; set; } = true;

    public virtual Evento Evento { get; set; } = null!;

    public virtual ICollection<InscripcionIntegrante> InscripcionIntegrantes { get; set; } = new List<InscripcionIntegrante>();

    public virtual Usuario LiderUsuario { get; set; } = null!;

    public virtual ICollection<ResultadosGanadore> ResultadosGanadores { get; set; } = new List<ResultadosGanadore>();

    public virtual Subcategoria Subcategoria { get; set; } = null!;

    public virtual Tutore? TutorUsuario { get; set; }
}

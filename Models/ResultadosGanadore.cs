using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class ResultadosGanadore
{
    public long ResultadoEventoId { get; set; }

    public sbyte Posicion { get; set; }

    public long InscripcionId { get; set; }

    public decimal Nota { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Inscripcione Inscripcion { get; set; } = null!;

    public virtual ResultadosEvento ResultadoEvento { get; set; } = null!;
}

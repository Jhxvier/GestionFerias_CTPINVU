using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class Subcategoria
{
    public int SubcategoriaId { get; set; }

    public int CategoriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual ICollection<Inscripcione> Inscripciones { get; set; } = new List<Inscripcione>();
}

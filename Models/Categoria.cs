using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class Categoria
{
    public int CategoriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string TipoFeria { get; set; } = null!;

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Subcategoria> Subcategoria { get; set; } = new List<Subcategoria>();
}

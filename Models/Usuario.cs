using System;
using System.Collections.Generic;

namespace GestionFerias_CTPINVU.Models;

public partial class Usuario
{
    public long UsuarioId { get; set; }

    public string Correo { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public DateTime? UltimoAcceso { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Estudiante? Estudiante { get; set; }

    public virtual ICollection<InscripcionIntegrante> InscripcionIntegrantes { get; set; } = new List<InscripcionIntegrante>();

    public virtual ICollection<Inscripcione> InscripcioneLiderUsuarios { get; set; } = new List<Inscripcione>();

    public virtual ICollection<Inscripcione> InscripcioneTutorUsuarios { get; set; } = new List<Inscripcione>();

    public virtual ICollection<Usuario> InverseUsuarioCreacionNavigation { get; set; } = new List<Usuario>();

    public virtual ICollection<Usuario> InverseUsuarioModificacionNavigation { get; set; } = new List<Usuario>();

    public virtual Juece? Juece { get; set; }

    public virtual Persona? Persona { get; set; }

    public virtual ICollection<ResultadosEvento> ResultadosEventos { get; set; } = new List<ResultadosEvento>();

    public virtual Tutore? Tutore { get; set; }

    public virtual Usuario? UsuarioCreacionNavigation { get; set; }

    public virtual Usuario? UsuarioModificacionNavigation { get; set; }

    public virtual ICollection<UsuarioRole> UsuarioRoles { get; set; } = new List<UsuarioRole>();
}

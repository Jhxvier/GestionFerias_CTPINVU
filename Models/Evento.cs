using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionFerias_CTPINVU.Models;

public partial class Evento
{
    public long EventoId { get; set; }

    public string CodigoEvento { get; set; } = null!;

    [Required(ErrorMessage = "Debe de seleccionar un Centro Educativo")]
    public long CentroId { get; set; }

    [Required(ErrorMessage = "Debe de ingresar el Nombre del Evento")]
    public string NombreEvento { get; set; } = null!;

    [Required(ErrorMessage = "Debe de especificar el Tipo de Feria")]
    public string TipoFeria { get; set; } = null!;

    [Required(ErrorMessage = "Debe de ingresar una descripción para el evento")]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "Debe de seleccionar la Fecha de Inicio")]
    public DateOnly FechaInicio { get; set; }

    [Required(ErrorMessage = "Debe de seleccionar la Fecha de Fin")]
    public DateOnly FechaFin { get; set; }

    [Required(ErrorMessage = "El Estado del Evento es obligatorio")]
    public string EstadoEvento { get; set; } = null!;

    public long? UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public bool EsActivo { get; set; } = true;

    public virtual CentrosEducativo Centro { get; set; } = null!;

    public virtual ICollection<Inscripcione> Inscripciones { get; set; } = new List<Inscripcione>();

    public virtual ICollection<ResultadosEvento> ResultadosEventos { get; set; } = new List<ResultadosEvento>();
}

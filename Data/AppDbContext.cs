using System;
using System.Collections.Generic;
using GestionFerias_CTPINVU.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace GestionFerias_CTPINVU.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<CentroTelefono> CentroTelefonos { get; set; }

    public virtual DbSet<CentrosEducativo> CentrosEducativos { get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    public virtual DbSet<Evento> Eventos { get; set; }

    public virtual DbSet<InscripcionIntegrante> InscripcionIntegrantes { get; set; }

    public virtual DbSet<Inscripcione> Inscripciones { get; set; }

    public virtual DbSet<Juece> Jueces { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<ResultadosEvento> ResultadosEventos { get; set; }

    public virtual DbSet<ResultadosGanadore> ResultadosGanadores { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subcategoria> Subcategorias { get; set; }

    public virtual DbSet<Tutore> Tutores { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioRole> UsuarioRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PRIMARY");

            entity.ToTable("categorias");

            entity.HasIndex(e => e.TipoFeria, "idx_cat_tipo_feria");

            entity.HasIndex(e => new { e.Nombre, e.TipoFeria }, "uq_categoria_tipo").IsUnique();

            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .HasColumnName("nombre");
            entity.Property(e => e.TipoFeria)
                .HasMaxLength(60)
                .HasColumnName("tipo_feria");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");
        });

        modelBuilder.Entity<CentroTelefono>(entity =>
        {
            entity.HasKey(e => e.CentroTelefonoId).HasName("PRIMARY");

            entity.ToTable("centro_telefonos");

            entity.HasIndex(e => new { e.CentroId, e.Telefono }, "uq_centro_tel").IsUnique();

            entity.Property(e => e.CentroTelefonoId).HasColumnName("centro_telefono_id");
            entity.Property(e => e.CentroId).HasColumnName("centro_id");
            entity.Property(e => e.EsPrincipal).HasColumnName("es_principal");
            entity.Property(e => e.Telefono)
                .HasMaxLength(30)
                .HasColumnName("telefono");
            entity.Property(e => e.Tipo)
                .HasMaxLength(30)
                .HasColumnName("tipo");

            entity.HasOne(d => d.Centro).WithMany(p => p.CentroTelefonos)
                .HasForeignKey(d => d.CentroId)
                .HasConstraintName("fk_centro_tel_centro");
        });

        modelBuilder.Entity<CentrosEducativo>(entity =>
        {
            entity.HasKey(e => e.CentroId).HasName("PRIMARY");

            entity.ToTable("centros_educativos");

            entity.HasIndex(e => e.CircuitoEducativo, "idx_centros_circuito");

            entity.HasIndex(e => e.NombreCentro, "idx_centros_nombre");

            entity.Property(e => e.CentroId).HasColumnName("centro_id");
            entity.Property(e => e.CircuitoEducativo)
                .HasMaxLength(50)
                .HasColumnName("circuito_educativo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .HasColumnName("direccion");
            entity.Property(e => e.DireccionRegional)
                .HasMaxLength(120)
                .HasColumnName("direccion_regional");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.NombreCentro)
                .HasMaxLength(160)
                .HasColumnName("nombre_centro");
            entity.Property(e => e.NombreDirector)
                .HasMaxLength(160)
                .HasColumnName("nombre_director");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.EstudianteId).HasName("PRIMARY");

            entity.ToTable("estudiantes");

            entity.HasIndex(e => e.Grado, "idx_estudiantes_grado");

            entity.Property(e => e.EstudianteId)
                .ValueGeneratedNever()
                .HasColumnName("estudiante_id");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.Grado)
                .HasColumnType("enum('7','8','9','10','11')")
                .HasColumnName("grado");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");

            entity.HasOne(d => d.EstudianteNavigation).WithOne(p => p.Estudiante)
                .HasForeignKey<Estudiante>(d => d.EstudianteId)
                .HasConstraintName("fk_estudiantes_usuario");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.EventoId).HasName("PRIMARY");

            entity.ToTable("eventos");

            entity.HasIndex(e => e.CodigoEvento, "codigo_evento").IsUnique();

            entity.HasIndex(e => e.CentroId, "fk_eventos_centro");

            entity.HasIndex(e => new { e.FechaInicio, e.FechaFin }, "idx_eventos_fechas");

            entity.HasIndex(e => e.NombreEvento, "idx_eventos_nombre");

            entity.HasIndex(e => e.TipoFeria, "idx_eventos_tipo_feria");

            entity.Property(e => e.EventoId).HasColumnName("evento_id");
            entity.Property(e => e.CentroId).HasColumnName("centro_id");
            entity.Property(e => e.CodigoEvento)
                .HasMaxLength(20)
                .HasColumnName("codigo_evento");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("descripcion");
            entity.Property(e => e.EstadoEvento)
                .HasDefaultValueSql("'En proceso'")
                .HasColumnType("enum('En proceso','Finalizado','Cancelado')")
                .HasColumnName("estado_evento");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.NombreEvento)
                .HasMaxLength(160)
                .HasColumnName("nombre_evento");
            entity.Property(e => e.TipoFeria)
                .HasMaxLength(60)
                .HasColumnName("tipo_feria");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");

            entity.HasOne(d => d.Centro).WithMany(p => p.Eventos)
                .HasForeignKey(d => d.CentroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_eventos_centro");
        });

        modelBuilder.Entity<InscripcionIntegrante>(entity =>
        {
            entity.HasKey(e => new { e.InscripcionId, e.EstudianteUsuarioId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("inscripcion_integrantes");

            entity.HasIndex(e => e.EstudianteUsuarioId, "fk_integ_estudiante");

            entity.Property(e => e.InscripcionId).HasColumnName("inscripcion_id");
            entity.Property(e => e.EstudianteUsuarioId).HasColumnName("estudiante_usuario_id");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");

            entity.HasOne(d => d.EstudianteUsuario).WithMany(p => p.InscripcionIntegrantes)
                .HasForeignKey(d => d.EstudianteUsuarioId)
                .HasConstraintName("fk_integ_estudiante");

            entity.HasOne(d => d.Inscripcion).WithMany(p => p.InscripcionIntegrantes)
                .HasForeignKey(d => d.InscripcionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_integ_inscripcion");
        });

        modelBuilder.Entity<Inscripcione>(entity =>
        {
            entity.HasKey(e => e.InscripcionId).HasName("PRIMARY");

            entity.ToTable("inscripciones");

            entity.HasIndex(e => e.EventoId, "fk_insc_evento");

            entity.HasIndex(e => e.LiderUsuarioId, "fk_insc_lider");

            entity.HasIndex(e => e.SubcategoriaId, "fk_insc_subcat");

            entity.HasIndex(e => e.TutorUsuarioId, "fk_insc_tutor");

            entity.HasIndex(e => e.TituloProyecto, "idx_insc_titulo");

            entity.Property(e => e.InscripcionId).HasColumnName("inscripcion_id");
            entity.Property(e => e.DescripcionProyecto)
                .HasColumnType("text")
                .HasColumnName("descripcion_proyecto");
            entity.Property(e => e.EstadoInscripcion)
                .HasDefaultValueSql("'Pendiente'")
                .HasColumnType("enum('Pendiente','Aprobado','Rechazado')")
                .HasColumnName("estado_inscripcion");
            entity.Property(e => e.Justificacion)
                .HasColumnType("text")
                .HasColumnName("justificacion");
            entity.Property(e => e.EventoId).HasColumnName("evento_id");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.LiderUsuarioId).HasColumnName("lider_usuario_id");
            entity.Property(e => e.SubcategoriaId).HasColumnName("subcategoria_id");
            entity.Property(e => e.TituloProyecto)
                .HasMaxLength(160)
                .HasColumnName("titulo_proyecto");
            entity.Property(e => e.TutorUsuarioId).HasColumnName("tutor_usuario_id");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");

            entity.HasOne(d => d.Evento).WithMany(p => p.Inscripciones)
                .HasForeignKey(d => d.EventoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_insc_evento");

            entity.HasOne(d => d.LiderUsuario).WithMany(p => p.Inscripciones)
                .HasForeignKey(d => d.LiderUsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_insc_lider");

            entity.HasOne(d => d.Subcategoria).WithMany(p => p.Inscripciones)
                .HasForeignKey(d => d.SubcategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_insc_subcat");

            entity.HasOne(d => d.TutorUsuario).WithMany(p => p.Inscripciones)
                .HasForeignKey(d => d.TutorUsuarioId)
                .HasConstraintName("fk_insc_tutor");
        });

        modelBuilder.Entity<Juece>(entity =>
        {
            entity.HasKey(e => e.JuezId).HasName("PRIMARY");

            entity.ToTable("jueces");

            entity.Property(e => e.JuezId)
                .ValueGeneratedNever()
                .HasColumnName("juez_id");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");

            entity.HasOne(d => d.Juez).WithOne(p => p.Juece)
                .HasForeignKey<Juece>(d => d.JuezId)
                .HasConstraintName("fk_jueces_usuario");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.PersonaId).HasName("PRIMARY");

            entity.ToTable("personas");

            entity.HasIndex(e => e.Documento, "documento").IsUnique();

            entity.Property(e => e.PersonaId).HasColumnName("persona_id");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(80)
                .HasColumnName("apellidos");
            entity.Property(e => e.Documento)
                .HasMaxLength(30)
                .HasColumnName("documento");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Nacionalidad)
                .HasMaxLength(60)
                .HasColumnName("nacionalidad");
            entity.Property(e => e.Nombres)
                .HasMaxLength(80)
                .HasColumnName("nombres");
            entity.Property(e => e.Sexo)
                .HasColumnType("enum('Masculino','Femenino','Otro','Prefiero no decir')")
                .HasColumnName("sexo");
            entity.Property(e => e.Telefono)
                .HasMaxLength(30)
                .HasColumnName("telefono");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");
        });

        modelBuilder.Entity<ResultadosEvento>(entity =>
        {
            entity.HasKey(e => e.ResultadoEventoId).HasName("PRIMARY");

            entity.ToTable("resultados_eventos");

            entity.HasIndex(e => e.EventoId, "evento_id").IsUnique();

            entity.HasIndex(e => e.JuezResponsableUsuarioId, "fk_res_juez");

            entity.HasIndex(e => e.FechaPublicacion, "idx_res_fecha");

            entity.Property(e => e.ResultadoEventoId).HasColumnName("resultado_evento_id");
            entity.Property(e => e.EstadoResultados)
                .HasDefaultValueSql("'Pendiente'")
                .HasColumnType("enum('Pendiente','Publicado')")
                .HasColumnName("estado_resultados");
            entity.Property(e => e.EventoId).HasColumnName("evento_id");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaPublicacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_publicacion");
            entity.Property(e => e.JuezResponsableUsuarioId).HasColumnName("juez_responsable_usuario_id");
            entity.Property(e => e.ResolucionFinal)
                .HasColumnType("text")
                .HasColumnName("resolucion_final");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");

            entity.HasOne(d => d.Evento).WithOne(p => p.ResultadosEvento)
                .HasForeignKey<ResultadosEvento>(d => d.EventoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_res_evento");

            entity.HasOne(d => d.JuezResponsableUsuario).WithMany(p => p.ResultadosEventos)
                .HasForeignKey(d => d.JuezResponsableUsuarioId)
                .HasConstraintName("fk_res_juez");
        });

        modelBuilder.Entity<ResultadosGanadore>(entity =>
        {
            entity.HasKey(e => new { e.ResultadoEventoId, e.Posicion })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("resultados_ganadores");

            entity.HasIndex(e => e.InscripcionId, "fk_ganadores_inscripcion");

            entity.HasIndex(e => new { e.ResultadoEventoId, e.InscripcionId }, "uq_no_repetir_grupo").IsUnique();

            entity.Property(e => e.ResultadoEventoId).HasColumnName("resultado_evento_id");
            entity.Property(e => e.Posicion).HasColumnName("posicion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.InscripcionId).HasColumnName("inscripcion_id");
            entity.Property(e => e.Nota)
                .HasPrecision(5, 2)
                .HasColumnName("nota");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");

            entity.HasOne(d => d.Inscripcion).WithMany(p => p.ResultadosGanadores)
                .HasForeignKey(d => d.InscripcionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ganadores_inscripcion");

            entity.HasOne(d => d.ResultadoEvento).WithMany(p => p.ResultadosGanadores)
                .HasForeignKey(d => d.ResultadoEventoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ganadores_resultado");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.HasIndex(e => e.NombreRol, "nombre_rol").IsUnique();

            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .HasColumnName("nombre_rol");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");
        });

        modelBuilder.Entity<Subcategoria>(entity =>
        {
            entity.HasKey(e => e.SubcategoriaId).HasName("PRIMARY");

            entity.ToTable("subcategorias");

            entity.HasIndex(e => new { e.CategoriaId, e.Nombre }, "uq_cat_subcat").IsUnique();

            entity.Property(e => e.SubcategoriaId).HasColumnName("subcategoria_id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .HasColumnName("nombre");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Subcategoria)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_subcat_categoria");
        });

        modelBuilder.Entity<Tutore>(entity =>
        {
            entity.HasKey(e => e.TutorId).HasName("PRIMARY");

            entity.ToTable("tutores");

            entity.HasIndex(e => e.Especialidad, "idx_tutores_especialidad");

            entity.Property(e => e.TutorId)
                .ValueGeneratedNever()
                .HasColumnName("tutor_id");
            entity.Property(e => e.Especialidad)
                .HasMaxLength(80)
                .HasColumnName("especialidad");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");

            entity.HasOne(d => d.Tutor).WithOne(p => p.Tutore)
                .HasForeignKey<Tutore>(d => d.TutorId)
                .HasConstraintName("fk_tutores_usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "correo").IsUnique();

            entity.HasIndex(e => e.UsuarioCreacion, "fk_usu_creacion");

            entity.HasIndex(e => e.UsuarioModificacion, "fk_usu_modificacion");

            entity.HasIndex(e => e.Estado, "idx_usuarios_estado");

            entity.HasIndex(e => e.PersonaId, "persona_id").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.Correo)
                .HasMaxLength(120)
                .HasColumnName("correo");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'Activo'")
                .HasColumnType("enum('Activo','Inactivo')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.RequiereCambioClave)
                .HasColumnName("requiere_cambio_clave")
                .HasDefaultValue(false);
            entity.Property(e => e.PersonaId).HasColumnName("persona_id");
            entity.Property(e => e.UltimoAcceso)
                .HasColumnType("datetime")
                .HasColumnName("ultimo_acceso");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");

            entity.HasOne(d => d.Persona).WithOne(p => p.Usuario)
                .HasForeignKey<Usuario>(d => d.PersonaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuarios_persona");

            entity.HasOne(d => d.UsuarioCreacionNavigation).WithMany(p => p.InverseUsuarioCreacionNavigation)
                .HasForeignKey(d => d.UsuarioCreacion)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_usu_creacion");

            entity.HasOne(d => d.UsuarioModificacionNavigation).WithMany(p => p.InverseUsuarioModificacionNavigation)
                .HasForeignKey(d => d.UsuarioModificacion)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_usu_modificacion");
        });

        modelBuilder.Entity<UsuarioRole>(entity =>
        {
            entity.HasKey(e => new { e.UsuarioId, e.RolId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("usuario_roles");

            entity.HasIndex(e => e.RolId, "fk_uroles_rol");

            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.FechaAsignacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_asignacion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaModificacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.UsuarioCreacion).HasColumnName("usuario_creacion");
            entity.Property(e => e.UsuarioModificacion).HasColumnName("usuario_modificacion");

            entity.HasOne(d => d.Rol).WithMany(p => p.UsuarioRoles)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_uroles_rol");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioRoles)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_uroles_usuario");
        });

        // alimentación inicial de datos para roles y un usuario administrador
        modelBuilder.Entity<Role>().HasData(
            new Role { RolId = 1, NombreRol = "Administrador", Descripcion = "Administrador del sistema", FechaCreacion = DateTime.Now }
        );

        modelBuilder.Entity<Persona>().HasData(
            new Persona { PersonaId = 1, Documento = "000000000", Nombres = "Administrador", Apellidos = "Sistema", FechaCreacion = DateTime.Now }
        );

        modelBuilder.Entity<Usuario>().HasData(
            new Usuario 
            { 
                UsuarioId = 1, 
                PersonaId = 1, 
                Correo = "admin@invu.cr",
                // usa el hash de "1234" para el password
                PasswordHash = "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4", 
                Estado = "Activo", 
                FechaCreacion = DateTime.Now 
            }
        );

        modelBuilder.Entity<UsuarioRole>().HasData(
            new UsuarioRole { UsuarioId = 1, RolId = 1, FechaAsignacion = DateTime.Now, FechaCreacion = DateTime.Now }
        );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

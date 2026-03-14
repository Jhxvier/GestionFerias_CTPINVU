using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionFerias_CTPINVU.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    categoria_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_feria = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.categoria_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "centros_educativos",
                columns: table => new
                {
                    centro_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre_centro = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombre_director = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    circuito_educativo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    direccion_regional = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    direccion = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.centro_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "personas",
                columns: table => new
                {
                    persona_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    documento = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombres = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    apellidos = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefono = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_nacimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    sexo = table.Column<string>(type: "enum('Masculino','Femenino','Otro','Prefiero no decir')", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nacionalidad = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.persona_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    rol_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre_rol = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.rol_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "subcategorias",
                columns: table => new
                {
                    subcategoria_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    categoria_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.subcategoria_id);
                    table.ForeignKey(
                        name: "fk_subcat_categoria",
                        column: x => x.categoria_id,
                        principalTable: "categorias",
                        principalColumn: "categoria_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "centro_telefonos",
                columns: table => new
                {
                    centro_telefono_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    centro_id = table.Column<long>(type: "bigint", nullable: false),
                    telefono = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    es_principal = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.centro_telefono_id);
                    table.ForeignKey(
                        name: "fk_centro_tel_centro",
                        column: x => x.centro_id,
                        principalTable: "centros_educativos",
                        principalColumn: "centro_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "eventos",
                columns: table => new
                {
                    evento_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    codigo_evento = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    centro_id = table.Column<long>(type: "bigint", nullable: false),
                    nombre_evento = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_feria = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: false),
                    estado_evento = table.Column<string>(type: "enum('En proceso','Finalizado','Cancelado')", nullable: false, defaultValueSql: "'En proceso'", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.evento_id);
                    table.ForeignKey(
                        name: "fk_eventos_centro",
                        column: x => x.centro_id,
                        principalTable: "centros_educativos",
                        principalColumn: "centro_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    usuario_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    persona_id = table.Column<long>(type: "bigint", nullable: false),
                    correo = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado = table.Column<string>(type: "enum('Activo','Inactivo')", nullable: false, defaultValueSql: "'Activo'", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ultimo_acceso = table.Column<DateTime>(type: "datetime", nullable: true),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.usuario_id);
                    table.ForeignKey(
                        name: "fk_usu_creacion",
                        column: x => x.usuario_creacion,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_usu_modificacion",
                        column: x => x.usuario_modificacion,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_usuarios_persona",
                        column: x => x.persona_id,
                        principalTable: "personas",
                        principalColumn: "persona_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "estudiantes",
                columns: table => new
                {
                    estudiante_id = table.Column<long>(type: "bigint", nullable: false),
                    grado = table.Column<string>(type: "enum('7','8','9','10','11')", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.estudiante_id);
                    table.ForeignKey(
                        name: "fk_estudiantes_usuario",
                        column: x => x.estudiante_id,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "jueces",
                columns: table => new
                {
                    juez_id = table.Column<long>(type: "bigint", nullable: false),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.juez_id);
                    table.ForeignKey(
                        name: "fk_jueces_usuario",
                        column: x => x.juez_id,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "tutores",
                columns: table => new
                {
                    tutor_id = table.Column<long>(type: "bigint", nullable: false),
                    especialidad = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.tutor_id);
                    table.ForeignKey(
                        name: "fk_tutores_usuario",
                        column: x => x.tutor_id,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "usuario_roles",
                columns: table => new
                {
                    usuario_id = table.Column<long>(type: "bigint", nullable: false),
                    rol_id = table.Column<int>(type: "int", nullable: false),
                    fecha_asignacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.usuario_id, x.rol_id })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_uroles_rol",
                        column: x => x.rol_id,
                        principalTable: "roles",
                        principalColumn: "rol_id");
                    table.ForeignKey(
                        name: "fk_uroles_usuario",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "resultados_eventos",
                columns: table => new
                {
                    resultado_evento_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    estado_resultados = table.Column<string>(type: "enum('Pendiente','Publicado')", nullable: false, defaultValueSql: "'Pendiente'", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    juez_responsable_usuario_id = table.Column<long>(type: "bigint", nullable: true),
                    resolucion_final = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_publicacion = table.Column<DateTime>(type: "datetime", nullable: true),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.resultado_evento_id);
                    table.ForeignKey(
                        name: "fk_res_evento",
                        column: x => x.evento_id,
                        principalTable: "eventos",
                        principalColumn: "evento_id");
                    table.ForeignKey(
                        name: "fk_res_juez",
                        column: x => x.juez_responsable_usuario_id,
                        principalTable: "jueces",
                        principalColumn: "juez_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "inscripciones",
                columns: table => new
                {
                    inscripcion_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    evento_id = table.Column<long>(type: "bigint", nullable: false),
                    lider_usuario_id = table.Column<long>(type: "bigint", nullable: false),
                    subcategoria_id = table.Column<int>(type: "int", nullable: false),
                    titulo_proyecto = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion_proyecto = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tutor_usuario_id = table.Column<long>(type: "bigint", nullable: true),
                    estado_inscripcion = table.Column<string>(type: "enum('Pendiente','Aprobado')", nullable: false, defaultValueSql: "'Pendiente'", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.inscripcion_id);
                    table.ForeignKey(
                        name: "fk_insc_evento",
                        column: x => x.evento_id,
                        principalTable: "eventos",
                        principalColumn: "evento_id");
                    table.ForeignKey(
                        name: "fk_insc_lider",
                        column: x => x.lider_usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id");
                    table.ForeignKey(
                        name: "fk_insc_subcat",
                        column: x => x.subcategoria_id,
                        principalTable: "subcategorias",
                        principalColumn: "subcategoria_id");
                    table.ForeignKey(
                        name: "fk_insc_tutor",
                        column: x => x.tutor_usuario_id,
                        principalTable: "tutores",
                        principalColumn: "tutor_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "inscripcion_integrantes",
                columns: table => new
                {
                    inscripcion_id = table.Column<long>(type: "bigint", nullable: false),
                    estudiante_usuario_id = table.Column<long>(type: "bigint", nullable: false),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.inscripcion_id, x.estudiante_usuario_id })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_integ_estudiante",
                        column: x => x.estudiante_usuario_id,
                        principalTable: "estudiantes",
                        principalColumn: "estudiante_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_integ_inscripcion",
                        column: x => x.inscripcion_id,
                        principalTable: "inscripciones",
                        principalColumn: "inscripcion_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "resultados_ganadores",
                columns: table => new
                {
                    resultado_evento_id = table.Column<long>(type: "bigint", nullable: false),
                    posicion = table.Column<sbyte>(type: "tinyint", nullable: false),
                    inscripcion_id = table.Column<long>(type: "bigint", nullable: false),
                    nota = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    usuario_creacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    usuario_modificacion = table.Column<long>(type: "bigint", nullable: true),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime", nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.resultado_evento_id, x.posicion })
                        .Annotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                    table.ForeignKey(
                        name: "fk_ganadores_inscripcion",
                        column: x => x.inscripcion_id,
                        principalTable: "inscripciones",
                        principalColumn: "inscripcion_id");
                    table.ForeignKey(
                        name: "fk_ganadores_resultado",
                        column: x => x.resultado_evento_id,
                        principalTable: "resultados_eventos",
                        principalColumn: "resultado_evento_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.InsertData(
                table: "personas",
                columns: new[] { "persona_id", "apellidos", "documento", "fecha_creacion", "fecha_nacimiento", "nacionalidad", "nombres", "sexo", "telefono", "usuario_creacion", "usuario_modificacion" },
                values: new object[] { 1L, "Sistema", "000000000", new DateTime(2026, 3, 14, 0, 50, 6, 142, DateTimeKind.Local).AddTicks(3530), null, null, "Administrador", null, null, null, null });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "rol_id", "descripcion", "fecha_creacion", "nombre_rol", "usuario_creacion", "usuario_modificacion" },
                values: new object[] { 1, "Administrador del sistema", new DateTime(2026, 3, 14, 0, 50, 6, 142, DateTimeKind.Local).AddTicks(3290), "Administrador", null, null });

            migrationBuilder.InsertData(
                table: "usuarios",
                columns: new[] { "usuario_id", "correo", "estado", "fecha_creacion", "password_hash", "persona_id", "ultimo_acceso", "usuario_creacion", "usuario_modificacion" },
                values: new object[] { 1L, "admin@invu.cr", "Activo", new DateTime(2026, 3, 14, 0, 50, 6, 142, DateTimeKind.Local).AddTicks(3560), "03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4", 1L, null, null, null });

            migrationBuilder.InsertData(
                table: "usuario_roles",
                columns: new[] { "rol_id", "usuario_id", "fecha_asignacion", "fecha_creacion", "usuario_creacion", "usuario_modificacion" },
                values: new object[] { 1, 1L, new DateTime(2026, 3, 14, 0, 50, 6, 142, DateTimeKind.Local).AddTicks(3580), new DateTime(2026, 3, 14, 0, 50, 6, 142, DateTimeKind.Local).AddTicks(3590), null, null });

            migrationBuilder.CreateIndex(
                name: "idx_cat_tipo_feria",
                table: "categorias",
                column: "tipo_feria");

            migrationBuilder.CreateIndex(
                name: "uq_categoria_tipo",
                table: "categorias",
                columns: new[] { "nombre", "tipo_feria" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_centro_tel",
                table: "centro_telefonos",
                columns: new[] { "centro_id", "telefono" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_centros_circuito",
                table: "centros_educativos",
                column: "circuito_educativo");

            migrationBuilder.CreateIndex(
                name: "idx_centros_nombre",
                table: "centros_educativos",
                column: "nombre_centro");

            migrationBuilder.CreateIndex(
                name: "idx_estudiantes_grado",
                table: "estudiantes",
                column: "grado");

            migrationBuilder.CreateIndex(
                name: "codigo_evento",
                table: "eventos",
                column: "codigo_evento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_eventos_centro",
                table: "eventos",
                column: "centro_id");

            migrationBuilder.CreateIndex(
                name: "idx_eventos_fechas",
                table: "eventos",
                columns: new[] { "fecha_inicio", "fecha_fin" });

            migrationBuilder.CreateIndex(
                name: "idx_eventos_nombre",
                table: "eventos",
                column: "nombre_evento");

            migrationBuilder.CreateIndex(
                name: "idx_eventos_tipo_feria",
                table: "eventos",
                column: "tipo_feria");

            migrationBuilder.CreateIndex(
                name: "fk_integ_estudiante",
                table: "inscripcion_integrantes",
                column: "estudiante_usuario_id");

            migrationBuilder.CreateIndex(
                name: "fk_insc_evento",
                table: "inscripciones",
                column: "evento_id");

            migrationBuilder.CreateIndex(
                name: "fk_insc_lider",
                table: "inscripciones",
                column: "lider_usuario_id");

            migrationBuilder.CreateIndex(
                name: "fk_insc_subcat",
                table: "inscripciones",
                column: "subcategoria_id");

            migrationBuilder.CreateIndex(
                name: "fk_insc_tutor",
                table: "inscripciones",
                column: "tutor_usuario_id");

            migrationBuilder.CreateIndex(
                name: "idx_insc_titulo",
                table: "inscripciones",
                column: "titulo_proyecto");

            migrationBuilder.CreateIndex(
                name: "documento",
                table: "personas",
                column: "documento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "evento_id",
                table: "resultados_eventos",
                column: "evento_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_res_juez",
                table: "resultados_eventos",
                column: "juez_responsable_usuario_id");

            migrationBuilder.CreateIndex(
                name: "idx_res_fecha",
                table: "resultados_eventos",
                column: "fecha_publicacion");

            migrationBuilder.CreateIndex(
                name: "fk_ganadores_inscripcion",
                table: "resultados_ganadores",
                column: "inscripcion_id");

            migrationBuilder.CreateIndex(
                name: "uq_no_repetir_grupo",
                table: "resultados_ganadores",
                columns: new[] { "resultado_evento_id", "inscripcion_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "nombre_rol",
                table: "roles",
                column: "nombre_rol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_cat_subcat",
                table: "subcategorias",
                columns: new[] { "categoria_id", "nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_tutores_especialidad",
                table: "tutores",
                column: "especialidad");

            migrationBuilder.CreateIndex(
                name: "fk_uroles_rol",
                table: "usuario_roles",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "correo",
                table: "usuarios",
                column: "correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fk_usu_creacion",
                table: "usuarios",
                column: "usuario_creacion");

            migrationBuilder.CreateIndex(
                name: "fk_usu_modificacion",
                table: "usuarios",
                column: "usuario_modificacion");

            migrationBuilder.CreateIndex(
                name: "idx_usuarios_estado",
                table: "usuarios",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "persona_id",
                table: "usuarios",
                column: "persona_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "centro_telefonos");

            migrationBuilder.DropTable(
                name: "inscripcion_integrantes");

            migrationBuilder.DropTable(
                name: "resultados_ganadores");

            migrationBuilder.DropTable(
                name: "usuario_roles");

            migrationBuilder.DropTable(
                name: "estudiantes");

            migrationBuilder.DropTable(
                name: "inscripciones");

            migrationBuilder.DropTable(
                name: "resultados_eventos");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "subcategorias");

            migrationBuilder.DropTable(
                name: "tutores");

            migrationBuilder.DropTable(
                name: "eventos");

            migrationBuilder.DropTable(
                name: "jueces");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "centros_educativos");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "personas");
        }
    }
}

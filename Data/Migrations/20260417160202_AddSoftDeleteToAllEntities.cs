using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionFerias_CTPINVU.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToAllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "es_activo",
                table: "subcategorias",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "es_activo",
                table: "resultados_eventos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "es_activo",
                table: "inscripciones",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "es_activo",
                table: "eventos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "es_activo",
                table: "centros_educativos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "es_activo",
                table: "categorias",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "personas",
                keyColumn: "persona_id",
                keyValue: 1L,
                column: "fecha_creacion",
                value: new DateTime(2026, 4, 17, 10, 2, 1, 910, DateTimeKind.Local).AddTicks(4330));

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "rol_id",
                keyValue: 1,
                column: "fecha_creacion",
                value: new DateTime(2026, 4, 17, 10, 2, 1, 910, DateTimeKind.Local).AddTicks(4180));

            migrationBuilder.UpdateData(
                table: "usuario_roles",
                keyColumns: new[] { "rol_id", "usuario_id" },
                keyValues: new object[] { 1, 1L },
                columns: new[] { "fecha_asignacion", "fecha_creacion" },
                values: new object[] { new DateTime(2026, 4, 17, 10, 2, 1, 910, DateTimeKind.Local).AddTicks(4370), new DateTime(2026, 4, 17, 10, 2, 1, 910, DateTimeKind.Local).AddTicks(4380) });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "usuario_id",
                keyValue: 1L,
                column: "fecha_creacion",
                value: new DateTime(2026, 4, 17, 10, 2, 1, 910, DateTimeKind.Local).AddTicks(4350));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "es_activo",
                table: "subcategorias");

            migrationBuilder.DropColumn(
                name: "es_activo",
                table: "resultados_eventos");

            migrationBuilder.DropColumn(
                name: "es_activo",
                table: "inscripciones");

            migrationBuilder.DropColumn(
                name: "es_activo",
                table: "eventos");

            migrationBuilder.DropColumn(
                name: "es_activo",
                table: "centros_educativos");

            migrationBuilder.DropColumn(
                name: "es_activo",
                table: "categorias");

            migrationBuilder.UpdateData(
                table: "personas",
                keyColumn: "persona_id",
                keyValue: 1L,
                column: "fecha_creacion",
                value: new DateTime(2026, 4, 17, 9, 55, 23, 972, DateTimeKind.Local).AddTicks(7050));

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "rol_id",
                keyValue: 1,
                column: "fecha_creacion",
                value: new DateTime(2026, 4, 17, 9, 55, 23, 972, DateTimeKind.Local).AddTicks(6910));

            migrationBuilder.UpdateData(
                table: "usuario_roles",
                keyColumns: new[] { "rol_id", "usuario_id" },
                keyValues: new object[] { 1, 1L },
                columns: new[] { "fecha_asignacion", "fecha_creacion" },
                values: new object[] { new DateTime(2026, 4, 17, 9, 55, 23, 972, DateTimeKind.Local).AddTicks(7090), new DateTime(2026, 4, 17, 9, 55, 23, 972, DateTimeKind.Local).AddTicks(7090) });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "usuario_id",
                keyValue: 1L,
                column: "fecha_creacion",
                value: new DateTime(2026, 4, 17, 9, 55, 23, 972, DateTimeKind.Local).AddTicks(7070));
        }
    }
}

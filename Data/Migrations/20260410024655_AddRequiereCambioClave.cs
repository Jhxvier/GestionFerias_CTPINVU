using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionFerias_CTPINVU.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRequiereCambioClave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "requiere_cambio_clave",
                table: "usuarios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "personas",
                keyColumn: "persona_id",
                keyValue: 1L,
                column: "fecha_creacion",
                value: new DateTime(2026, 4, 9, 20, 46, 55, 173, DateTimeKind.Local).AddTicks(640));

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "rol_id",
                keyValue: 1,
                column: "fecha_creacion",
                value: new DateTime(2026, 4, 9, 20, 46, 55, 173, DateTimeKind.Local).AddTicks(440));

            migrationBuilder.UpdateData(
                table: "usuario_roles",
                keyColumns: new[] { "rol_id", "usuario_id" },
                keyValues: new object[] { 1, 1L },
                columns: new[] { "fecha_asignacion", "fecha_creacion" },
                values: new object[] { new DateTime(2026, 4, 9, 20, 46, 55, 173, DateTimeKind.Local).AddTicks(680), new DateTime(2026, 4, 9, 20, 46, 55, 173, DateTimeKind.Local).AddTicks(690) });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "usuario_id",
                keyValue: 1L,
                column: "fecha_creacion",
                value: new DateTime(2026, 4, 9, 20, 46, 55, 173, DateTimeKind.Local).AddTicks(670));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "requiere_cambio_clave",
                table: "usuarios");

            migrationBuilder.UpdateData(
                table: "personas",
                keyColumn: "persona_id",
                keyValue: 1L,
                column: "fecha_creacion",
                value: new DateTime(2026, 3, 14, 0, 50, 6, 142, DateTimeKind.Local).AddTicks(3530));

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "rol_id",
                keyValue: 1,
                column: "fecha_creacion",
                value: new DateTime(2026, 3, 14, 0, 50, 6, 142, DateTimeKind.Local).AddTicks(3290));

            migrationBuilder.UpdateData(
                table: "usuario_roles",
                keyColumns: new[] { "rol_id", "usuario_id" },
                keyValues: new object[] { 1, 1L },
                columns: new[] { "fecha_asignacion", "fecha_creacion" },
                values: new object[] { new DateTime(2026, 3, 14, 0, 50, 6, 142, DateTimeKind.Local).AddTicks(3580), new DateTime(2026, 3, 14, 0, 50, 6, 142, DateTimeKind.Local).AddTicks(3590) });

            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "usuario_id",
                keyValue: 1L,
                column: "fecha_creacion",
                value: new DateTime(2026, 3, 14, 0, 50, 6, 142, DateTimeKind.Local).AddTicks(3560));
        }
    }
}

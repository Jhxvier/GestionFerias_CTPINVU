using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionFerias_CTPINVU.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddJustificacionAndRechazadoEstado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "estado_inscripcion",
                table: "inscripciones",
                type: "enum('Pendiente','Aprobado','Rechazado')",
                nullable: false,
                defaultValueSql: "'Pendiente'",
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "enum('Pendiente','Aprobado')",
                oldDefaultValueSql: "'Pendiente'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AddColumn<string>(
                name: "justificacion",
                table: "inscripciones",
                type: "text",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "justificacion",
                table: "inscripciones");

            migrationBuilder.AlterColumn<string>(
                name: "estado_inscripcion",
                table: "inscripciones",
                type: "enum('Pendiente','Aprobado')",
                nullable: false,
                defaultValueSql: "'Pendiente'",
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "enum('Pendiente','Aprobado','Rechazado')",
                oldDefaultValueSql: "'Pendiente'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

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
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionFerias_CTPINVU.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixResultadosEventoUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the unique index on evento_id so that soft-deleted results
            // don't block creating a new result for the same event.
            migrationBuilder.DropIndex(
                name: "evento_id",
                table: "resultados_eventos");

            migrationBuilder.CreateIndex(
                name: "evento_id",
                table: "resultados_eventos",
                column: "evento_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "evento_id",
                table: "resultados_eventos");

            migrationBuilder.CreateIndex(
                name: "evento_id",
                table: "resultados_eventos",
                column: "evento_id",
                unique: true);
        }
    }
}

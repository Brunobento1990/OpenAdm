using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class PesoAtivoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Pesos",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pesos_Ativo",
                table: "Pesos",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_Pesos_Descricao_Ativo",
                table: "Pesos",
                columns: new[] { "Descricao", "Ativo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pesos_Ativo",
                table: "Pesos");

            migrationBuilder.DropIndex(
                name: "IX_Pesos_Descricao_Ativo",
                table: "Pesos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Pesos");
        }
    }
}

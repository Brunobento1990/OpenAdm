using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class TamanhoAtivoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Tamanhos",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tamanhos_Ativo",
                table: "Tamanhos",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_Tamanhos_Descricao_Ativo",
                table: "Tamanhos",
                columns: new[] { "Descricao", "Ativo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tamanhos_Ativo",
                table: "Tamanhos");

            migrationBuilder.DropIndex(
                name: "IX_Tamanhos_Descricao_Ativo",
                table: "Tamanhos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Tamanhos");
        }
    }
}

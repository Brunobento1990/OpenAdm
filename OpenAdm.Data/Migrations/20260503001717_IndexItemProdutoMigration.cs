using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class IndexItemProdutoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItensPedidos_ProdutoId",
                table: "ItensPedidos");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedidos_ProdutoId_TamanhoId_PesoId",
                table: "ItensPedidos",
                columns: new[] { "ProdutoId", "TamanhoId", "PesoId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItensPedidos_ProdutoId_TamanhoId_PesoId",
                table: "ItensPedidos");

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedidos_ProdutoId",
                table: "ItensPedidos",
                column: "ProdutoId");
        }
    }
}

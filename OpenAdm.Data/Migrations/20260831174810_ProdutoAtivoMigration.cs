using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProdutoAtivoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Produtos_CategoriaId",
                table: "Produtos");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Produtos",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Ativo",
                table: "Produtos",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Ativo_Descricao",
                table: "Produtos",
                columns: new[] { "Ativo", "Descricao" });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Ativo_InativoEcommerce_Numero",
                table: "Produtos",
                columns: new[] { "Ativo", "InativoEcommerce", "Numero" });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Ativo_InativoEcommerce_Referencia",
                table: "Produtos",
                columns: new[] { "Ativo", "InativoEcommerce", "Referencia" });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CategoriaId_Ativo_InativoEcommerce_Numero",
                table: "Produtos",
                columns: new[] { "CategoriaId", "Ativo", "InativoEcommerce", "Numero" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Produtos_Ativo",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_Ativo_Descricao",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_Ativo_InativoEcommerce_Numero",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_Ativo_InativoEcommerce_Referencia",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_CategoriaId_Ativo_InativoEcommerce_Numero",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Produtos");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CategoriaId",
                table: "Produtos",
                column: "CategoriaId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovidoInativoEcommerceProdutoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""UPDATE "Produtos" SET "Ativo" = FALSE WHERE "InativoEcommerce" = TRUE;""");
            
            migrationBuilder.DropIndex(
                name: "IX_Produtos_Ativo_InativoEcommerce_Numero",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_Ativo_InativoEcommerce_Referencia",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_CategoriaId_Ativo_InativoEcommerce_Numero",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_InativoEcommerce",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "InativoEcommerce",
                table: "Produtos");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Ativo_Numero",
                table: "Produtos",
                columns: new[] { "Ativo", "Numero" });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_Ativo_Referencia",
                table: "Produtos",
                columns: new[] { "Ativo", "Referencia" });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CategoriaId_Ativo_Numero",
                table: "Produtos",
                columns: new[] { "CategoriaId", "Ativo", "Numero" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Produtos_Ativo_Numero",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_Ativo_Referencia",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_CategoriaId_Ativo_Numero",
                table: "Produtos");

            migrationBuilder.AddColumn<bool>(
                name: "InativoEcommerce",
                table: "Produtos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_InativoEcommerce",
                table: "Produtos",
                column: "InativoEcommerce");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class InativoEcommerceIndexMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Produtos_InativoEcommerce",
                table: "Produtos",
                column: "InativoEcommerce");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_InativoEcommerce",
                table: "Categorias",
                column: "InativoEcommerce");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Produtos_InativoEcommerce",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_InativoEcommerce",
                table: "Categorias");
        }
    }
}

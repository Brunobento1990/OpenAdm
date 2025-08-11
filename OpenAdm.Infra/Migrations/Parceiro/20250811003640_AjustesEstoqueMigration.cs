using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class AjustesEstoqueMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Estoques_PesoId",
                table: "Estoques",
                column: "PesoId");

            migrationBuilder.CreateIndex(
                name: "IX_Estoques_TamanhoId",
                table: "Estoques",
                column: "TamanhoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_Pesos_PesoId",
                table: "Estoques",
                column: "PesoId",
                principalTable: "Pesos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_Produtos_ProdutoId",
                table: "Estoques",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_Tamanhos_TamanhoId",
                table: "Estoques",
                column: "TamanhoId",
                principalTable: "Tamanhos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_Pesos_PesoId",
                table: "Estoques");

            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_Produtos_ProdutoId",
                table: "Estoques");

            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_Tamanhos_TamanhoId",
                table: "Estoques");

            migrationBuilder.DropIndex(
                name: "IX_Estoques_PesoId",
                table: "Estoques");

            migrationBuilder.DropIndex(
                name: "IX_Estoques_TamanhoId",
                table: "Estoques");
        }
    }
}

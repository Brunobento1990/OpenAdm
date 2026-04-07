using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class AjusteTabelaMovimentacaoProdutoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesDeProdutos_PesoId",
                table: "MovimentacoesDeProdutos",
                column: "PesoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesDeProdutos_TamanhoId",
                table: "MovimentacoesDeProdutos",
                column: "TamanhoId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesDeProdutos_Pesos_PesoId",
                table: "MovimentacoesDeProdutos",
                column: "PesoId",
                principalTable: "Pesos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesDeProdutos_Produtos_ProdutoId",
                table: "MovimentacoesDeProdutos",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacoesDeProdutos_Tamanhos_TamanhoId",
                table: "MovimentacoesDeProdutos",
                column: "TamanhoId",
                principalTable: "Tamanhos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesDeProdutos_Pesos_PesoId",
                table: "MovimentacoesDeProdutos");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesDeProdutos_Produtos_ProdutoId",
                table: "MovimentacoesDeProdutos");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacoesDeProdutos_Tamanhos_TamanhoId",
                table: "MovimentacoesDeProdutos");

            migrationBuilder.DropIndex(
                name: "IX_MovimentacoesDeProdutos_PesoId",
                table: "MovimentacoesDeProdutos");

            migrationBuilder.DropIndex(
                name: "IX_MovimentacoesDeProdutos_TamanhoId",
                table: "MovimentacoesDeProdutos");
        }
    }
}

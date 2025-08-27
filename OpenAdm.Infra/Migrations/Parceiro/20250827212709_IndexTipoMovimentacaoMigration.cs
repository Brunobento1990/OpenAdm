using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class IndexTipoMovimentacaoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesDeProdutos_TipoMovimentacaoDeProduto",
                table: "MovimentacoesDeProdutos",
                column: "TipoMovimentacaoDeProduto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MovimentacoesDeProdutos_TipoMovimentacaoDeProduto",
                table: "MovimentacoesDeProdutos");
        }
    }
}

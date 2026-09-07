using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjusteParceiroiCOntextMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ItensTabelaDePreco_PesoId",
                table: "ItensTabelaDePreco",
                column: "PesoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensTabelaDePreco_TamanhoId",
                table: "ItensTabelaDePreco",
                column: "TamanhoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensTabelaDePreco_Pesos_PesoId",
                table: "ItensTabelaDePreco",
                column: "PesoId",
                principalTable: "Pesos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensTabelaDePreco_Tamanhos_TamanhoId",
                table: "ItensTabelaDePreco",
                column: "TamanhoId",
                principalTable: "Tamanhos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensTabelaDePreco_Pesos_PesoId",
                table: "ItensTabelaDePreco");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensTabelaDePreco_Tamanhos_TamanhoId",
                table: "ItensTabelaDePreco");

            migrationBuilder.DropIndex(
                name: "IX_ItensTabelaDePreco_PesoId",
                table: "ItensTabelaDePreco");

            migrationBuilder.DropIndex(
                name: "IX_ItensTabelaDePreco_TamanhoId",
                table: "ItensTabelaDePreco");
        }
    }
}

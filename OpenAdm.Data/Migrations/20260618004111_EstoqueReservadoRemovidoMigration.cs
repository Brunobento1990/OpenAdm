using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class EstoqueReservadoRemovidoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeReservada",
                table: "Estoques");

            migrationBuilder.CreateIndex(
                name: "IX_Estoques_ProdutoId_PesoId_TamanhoId",
                table: "Estoques",
                columns: new[] { "ProdutoId", "PesoId", "TamanhoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Estoques_ProdutoId_PesoId_TamanhoId",
                table: "Estoques");

            migrationBuilder.AddColumn<decimal>(
                name: "QuantidadeReservada",
                table: "Estoques",
                type: "numeric(12,2)",
                precision: 12,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}

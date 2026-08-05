using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class IndexConfiguracaoDePedidoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDePedidos_Ativo_ParceiroId",
                table: "ConfiguracoesDePedidos",
                columns: new[] { "Ativo", "ParceiroId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConfiguracoesDePedidos_Ativo_ParceiroId",
                table: "ConfiguracoesDePedidos");
        }
    }
}

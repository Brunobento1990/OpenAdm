using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class PedidoExcluidoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Excluido",
                table: "Pedidos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_Excluido_StatusPedido",
                table: "Pedidos",
                columns: new[] { "Excluido", "StatusPedido" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pedidos_Excluido_StatusPedido",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Excluido",
                table: "Pedidos");
        }
    }
}

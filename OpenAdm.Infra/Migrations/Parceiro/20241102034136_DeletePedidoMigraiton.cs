using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class DeletePedidoMigraiton : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faturas_Pedidos_PedidoId",
                table: "Faturas");

            migrationBuilder.DropIndex(
                name: "IX_Faturas_PedidoId",
                table: "Faturas");

            migrationBuilder.CreateIndex(
                name: "IX_Faturas_PedidoId",
                table: "Faturas",
                column: "PedidoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Faturas_Pedidos_PedidoId",
                table: "Faturas",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faturas_Pedidos_PedidoId",
                table: "Faturas");

            migrationBuilder.DropIndex(
                name: "IX_Faturas_PedidoId",
                table: "Faturas");

            migrationBuilder.CreateIndex(
                name: "IX_Faturas_PedidoId",
                table: "Faturas",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faturas_Pedidos_PedidoId",
                table: "Faturas",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id");
        }
    }
}

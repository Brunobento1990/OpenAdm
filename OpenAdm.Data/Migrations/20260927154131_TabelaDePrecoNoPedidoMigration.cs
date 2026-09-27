using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class TabelaDePrecoNoPedidoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TabelaDePrecoId",
                table: "Pedidos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_TabelaDePrecoId",
                table: "Pedidos",
                column: "TabelaDePrecoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_TabelaDePreco_TabelaDePrecoId",
                table: "Pedidos",
                column: "TabelaDePrecoId",
                principalTable: "TabelaDePreco",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_TabelaDePreco_TabelaDePrecoId",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_TabelaDePrecoId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "TabelaDePrecoId",
                table: "Pedidos");
        }
    }
}

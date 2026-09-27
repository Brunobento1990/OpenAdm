using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class RepresentantePedidoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RepresentanteId",
                table: "Pedidos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_RepresentanteId",
                table: "Pedidos",
                column: "RepresentanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Representantes_RepresentanteId",
                table: "Pedidos",
                column: "RepresentanteId",
                principalTable: "Representantes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Representantes_RepresentanteId",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_RepresentanteId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "RepresentanteId",
                table: "Pedidos");
        }
    }
}

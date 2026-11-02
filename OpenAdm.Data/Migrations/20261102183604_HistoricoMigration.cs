using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class HistoricoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "ItensPedidos",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "PedidosHistoricos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PedidoId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosHistoricos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidosHistoricos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItensPedidos_Ativo",
                table: "ItensPedidos",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosHistoricos_PedidoId",
                table: "PedidosHistoricos",
                column: "PedidoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidosHistoricos");

            migrationBuilder.DropIndex(
                name: "IX_ItensPedidos_Ativo",
                table: "ItensPedidos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "ItensPedidos");
        }
    }
}

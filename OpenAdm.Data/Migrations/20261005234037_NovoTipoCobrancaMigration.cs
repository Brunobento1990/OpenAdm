using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class NovoTipoCobrancaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CobrancasPedidosEcommerce",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PedidoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CobrancasPedidosEcommerce", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CobrancasPedidosEcommerce_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CobrancasPedidosEcommerce_Ativo",
                table: "CobrancasPedidosEcommerce",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_CobrancasPedidosEcommerce_PedidoId",
                table: "CobrancasPedidosEcommerce",
                column: "PedidoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CobrancasPedidosEcommerce_Status",
                table: "CobrancasPedidosEcommerce",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CobrancasPedidosEcommerce_Status_Ativo",
                table: "CobrancasPedidosEcommerce",
                columns: new[] { "Status", "Ativo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CobrancasPedidosEcommerce");
        }
    }
}

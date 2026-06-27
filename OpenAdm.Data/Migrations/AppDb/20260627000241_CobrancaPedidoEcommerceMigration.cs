using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class CobrancaPedidoEcommerceMigration : Migration
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
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CobrancasPedidosEcommerce", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CobrancasPedidosEcommerce_Ativo",
                table: "CobrancasPedidosEcommerce",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_CobrancasPedidosEcommerce_Numero",
                table: "CobrancasPedidosEcommerce",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_CobrancasPedidosEcommerce_ParceiroId",
                table: "CobrancasPedidosEcommerce",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_CobrancasPedidosEcommerce_ParceiroId_Status_Ativo",
                table: "CobrancasPedidosEcommerce",
                columns: new[] { "ParceiroId", "Status", "Ativo" });

            migrationBuilder.CreateIndex(
                name: "IX_CobrancasPedidosEcommerce_PedidoId",
                table: "CobrancasPedidosEcommerce",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_CobrancasPedidosEcommerce_Status",
                table: "CobrancasPedidosEcommerce",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CobrancasPedidosEcommerce");
        }
    }
}

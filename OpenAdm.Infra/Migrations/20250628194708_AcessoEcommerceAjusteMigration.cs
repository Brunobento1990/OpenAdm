using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AcessoEcommerceAjusteMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcessosEcommerce",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Quantidade = table.Column<long>(type: "bigint", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcessosEcommerce", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcessosEcommerce_DataDeCriacao",
                table: "AcessosEcommerce",
                column: "DataDeCriacao");

            migrationBuilder.CreateIndex(
                name: "IX_AcessosEcommerce_ParceiroId",
                table: "AcessosEcommerce",
                column: "ParceiroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcessosEcommerce");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguracaoDeFreteMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracoesDeFrete",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CepOrigem = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CobrarCnpj = table.Column<bool>(type: "boolean", nullable: false),
                    CobrarCpf = table.Column<bool>(type: "boolean", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracoesDeFrete", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDeFrete_Numero",
                table: "ConfiguracoesDeFrete",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDeFrete_ParceiroId",
                table: "ConfiguracoesDeFrete",
                column: "ParceiroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracoesDeFrete");
        }
    }
}

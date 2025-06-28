using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations
{
    /// <inheritdoc />
    public partial class LojasParceirasAjusteMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LojasParceiras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeFoto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Foto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Instagram = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Facebook = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Endereco = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Contato = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LojasParceiras", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_ParceiroId",
                table: "Funcionarios",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDePedidos_ParceiroId",
                table: "ConfiguracoesDePedidos",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDePagamento_ParceiroId",
                table: "ConfiguracoesDePagamento",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_ParceiroId",
                table: "Banners",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_LojasParceiras_Numero",
                table: "LojasParceiras",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_LojasParceiras_ParceiroId",
                table: "LojasParceiras",
                column: "ParceiroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LojasParceiras");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_ParceiroId",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_ConfiguracoesDePedidos_ParceiroId",
                table: "ConfiguracoesDePedidos");

            migrationBuilder.DropIndex(
                name: "IX_ConfiguracoesDePagamento_ParceiroId",
                table: "ConfiguracoesDePagamento");

            migrationBuilder.DropIndex(
                name: "IX_Banners_ParceiroId",
                table: "Banners");
        }
    }
}

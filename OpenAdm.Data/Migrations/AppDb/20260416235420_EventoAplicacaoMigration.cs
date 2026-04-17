using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class EventoAplicacaoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventosAplicacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Dados = table.Column<JsonDocument>(type: "jsonb", nullable: false),
                    TipoEventoAplicacao = table.Column<int>(type: "integer", nullable: false),
                    Finalizado = table.Column<bool>(type: "boolean", nullable: false),
                    Mensagem = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    QuantidadeDeTentativa = table.Column<short>(type: "smallint", nullable: false),
                    EmpresaOpenAdmId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosAplicacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventosAplicacao_Empresas_EmpresaOpenAdmId",
                        column: x => x.EmpresaOpenAdmId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventosAplicacao_EmpresaOpenAdmId",
                table: "EventosAplicacao",
                column: "EmpresaOpenAdmId");

            migrationBuilder.CreateIndex(
                name: "IX_EventosAplicacao_QuantidadeDeTentativa_Finalizado",
                table: "EventosAplicacao",
                columns: new[] { "QuantidadeDeTentativa", "Finalizado" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventosAplicacao");
        }
    }
}

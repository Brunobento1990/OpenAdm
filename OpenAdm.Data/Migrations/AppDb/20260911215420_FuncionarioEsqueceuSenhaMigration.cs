using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class FuncionarioEsqueceuSenhaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuncionariosEsqueceramSenha",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<Guid>(type: "uuid", nullable: false),
                    DataHoraExpiracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Resetado = table.Column<bool>(type: "boolean", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuncionariosEsqueceramSenha", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuncionariosEsqueceramSenha_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuncionariosEsqueceramSenha_FuncionarioId",
                table: "FuncionariosEsqueceramSenha",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_FuncionariosEsqueceramSenha_Numero",
                table: "FuncionariosEsqueceramSenha",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_FuncionariosEsqueceramSenha_ParceiroId",
                table: "FuncionariosEsqueceramSenha",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_FuncionariosEsqueceramSenha_Token",
                table: "FuncionariosEsqueceramSenha",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuncionariosEsqueceramSenha");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class SessaoUsuarioMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessoesUsuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false),
                    EhFuncionario = table.Column<bool>(type: "boolean", nullable: false),
                    UltimaAtividadeEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ExpiraEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RevogadoEm = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EnderecoIp = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SistemaOperacional = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Navegador = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Dispositivo = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessoesUsuarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessoesUsuarios_UsuarioId_ParceiroId_EhFuncionario_Revogado~",
                table: "SessoesUsuarios",
                columns: new[] { "UsuarioId", "ParceiroId", "EhFuncionario", "RevogadoEm", "ExpiraEm" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessoesUsuarios");
        }
    }
}

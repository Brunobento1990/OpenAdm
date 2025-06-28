using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class TopUsuariosAjusteMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracoesDeFrete");

            migrationBuilder.DropTable(
                name: "TopUsuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracoesDeFrete",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AlturaEmbalagem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CepOrigem = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    ChaveApi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CobrarCnpj = table.Column<bool>(type: "boolean", nullable: true),
                    CobrarCpf = table.Column<bool>(type: "boolean", nullable: true),
                    ComprimentoEmbalagem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Inativo = table.Column<bool>(type: "boolean", nullable: true),
                    LarguraEmbalagem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Peso = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracoesDeFrete", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TopUsuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    TotalCompra = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPedidos = table.Column<int>(type: "integer", nullable: false),
                    Usuario = table.Column<string>(type: "text", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopUsuarios", x => x.Id);
                });
        }
    }
}

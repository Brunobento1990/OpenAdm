using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class FreteMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PesoReal",
                table: "Tamanhos",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PesoReal",
                table: "Pesos",
                type: "numeric",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConfiguracoesDeFrete",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CepOrigem = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    AlturaEmbalagem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LarguraEmbalagem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ComprimentoEmbalagem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ChaveApi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Peso = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracoesDeFrete", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracoesDeFrete");

            migrationBuilder.DropColumn(
                name: "PesoReal",
                table: "Tamanhos");

            migrationBuilder.DropColumn(
                name: "PesoReal",
                table: "Pesos");
        }
    }
}

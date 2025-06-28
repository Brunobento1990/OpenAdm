using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class LojasParceirasAjusteMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LojasParceiras");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LojasParceiras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Contato = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Endereco = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Facebook = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Foto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Instagram = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    NomeFoto = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LojasParceiras", x => x.Id);
                });
        }
    }
}

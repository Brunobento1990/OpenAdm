using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenAdm.Infra.Migrations
{
    /// <inheritdoc />
    public partial class V1Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    UrlEcommerce = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false),
                    UrlAdmin = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false),
                    ConnectionString = table.Column<string>(type: "text", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Ativo",
                table: "Empresas",
                column: "Ativo");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_UrlAdmin",
                table: "Empresas",
                column: "UrlAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_UrlAdmin_UrlEcommerce",
                table: "Empresas",
                columns: new[] { "UrlAdmin", "UrlEcommerce" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_UrlAdmin_UrlEcommerce_Ativo",
                table: "Empresas",
                columns: new[] { "UrlAdmin", "UrlEcommerce", "Ativo" });

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_UrlEcommerce",
                table: "Empresas",
                column: "UrlEcommerce");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}

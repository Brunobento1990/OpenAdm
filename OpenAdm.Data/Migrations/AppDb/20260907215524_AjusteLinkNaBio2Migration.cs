using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenAdm.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class AjusteLinkNaBio2Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LinkBioConfiguracoes_Slug",
                table: "LinkBioConfiguracoes");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_UrlAdmin",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_UrlAdmin_UrlEcommerce",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_UrlAdmin_UrlEcommerce_Ativo",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_UrlEcommerce",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "LinkBioConfiguracoes");

            migrationBuilder.DropColumn(
                name: "UrlAdmin",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "UrlEcommerce",
                table: "Empresas");

            migrationBuilder.CreateTable(
                name: "LinksEmpresas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinksEmpresas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinksEmpresas_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinksEmpresas_EmpresaId",
                table: "LinksEmpresas",
                column: "EmpresaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LinksEmpresas_Url",
                table: "LinksEmpresas",
                column: "Url",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinksEmpresas");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "LinkBioConfiguracoes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlAdmin",
                table: "Empresas",
                type: "character varying(350)",
                maxLength: 350,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlEcommerce",
                table: "Empresas",
                type: "character varying(350)",
                maxLength: 350,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LinkBioConfiguracoes_Slug",
                table: "LinkBioConfiguracoes",
                column: "Slug",
                unique: true);

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
    }
}

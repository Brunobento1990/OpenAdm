using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenAdm.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class LinkNaBioMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LinkBioConfiguracoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    CorDeFundo = table.Column<string>(type: "text", nullable: true),
                    CorPrincipal = table.Column<string>(type: "text", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkBioConfiguracoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkBioConfiguracoes_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinkBioItens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LinkBioConfiguracaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Ordem = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkBioItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkBioItens_LinkBioConfiguracoes_LinkBioConfiguracaoId",
                        column: x => x.LinkBioConfiguracaoId,
                        principalTable: "LinkBioConfiguracoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinkBioEventos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uuid", nullable: false),
                    LinkBioConfiguracaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    LinkBioItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkBioEventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkBioEventos_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkBioEventos_LinkBioConfiguracoes_LinkBioConfiguracaoId",
                        column: x => x.LinkBioConfiguracaoId,
                        principalTable: "LinkBioConfiguracoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkBioEventos_LinkBioItens_LinkBioItemId",
                        column: x => x.LinkBioItemId,
                        principalTable: "LinkBioItens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinkBioConfiguracoes_EmpresaId",
                table: "LinkBioConfiguracoes",
                column: "EmpresaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LinkBioConfiguracoes_Slug",
                table: "LinkBioConfiguracoes",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LinkBioEventos_EmpresaId_DataDeCriacao",
                table: "LinkBioEventos",
                columns: new[] { "EmpresaId", "DataDeCriacao" });

            migrationBuilder.CreateIndex(
                name: "IX_LinkBioEventos_LinkBioConfiguracaoId_Tipo",
                table: "LinkBioEventos",
                columns: new[] { "LinkBioConfiguracaoId", "Tipo" });

            migrationBuilder.CreateIndex(
                name: "IX_LinkBioEventos_LinkBioItemId",
                table: "LinkBioEventos",
                column: "LinkBioItemId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkBioItens_LinkBioConfiguracaoId_Ordem",
                table: "LinkBioItens",
                columns: new[] { "LinkBioConfiguracaoId", "Ordem" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkBioEventos");

            migrationBuilder.DropTable(
                name: "LinkBioItens");

            migrationBuilder.DropTable(
                name: "LinkBioConfiguracoes");
        }
    }
}

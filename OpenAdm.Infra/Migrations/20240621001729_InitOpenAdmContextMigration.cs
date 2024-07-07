using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenAdm.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitOpenAdmContextMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Origem = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Latitude = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Longitude = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Host = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Ip = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Path = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Erro = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    StatusCode = table.Column<int>(type: "integer", nullable: false),
                    LogLevel = table.Column<int>(type: "integer", nullable: false),
                    XApi = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parceiros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RazaoSocial = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    NomeFantasia = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parceiros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracoesParceiro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConexaoDb = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DominioSiteAdm = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DominioSiteEcommerce = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ClienteMercadoPago = table.Column<string>(type: "text", nullable: true),
                    XApi = table.Column<Guid>(type: "uuid", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracoesParceiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracoesParceiro_Parceiros_ParceiroId",
                        column: x => x.ParceiroId,
                        principalTable: "Parceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesParceiro_DominioSiteAdm",
                table: "ConfiguracoesParceiro",
                column: "DominioSiteAdm",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesParceiro_DominioSiteEcommerce",
                table: "ConfiguracoesParceiro",
                column: "DominioSiteEcommerce",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesParceiro_ParceiroId",
                table: "ConfiguracoesParceiro",
                column: "ParceiroId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesParceiro_XApi",
                table: "ConfiguracoesParceiro",
                column: "XApi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_LogLevel",
                table: "Logs",
                column: "LogLevel");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_StatusCode",
                table: "Logs",
                column: "StatusCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracoesParceiro");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Parceiros");
        }
    }
}

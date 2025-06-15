using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenAdm.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ParceirosMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parceiros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmpresaOpenAdmId = table.Column<Guid>(type: "uuid", nullable: false),
                    RazaoSocial = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Logo = table.Column<byte[]>(type: "bytea", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_Parceiros_Empresas_EmpresaOpenAdmId",
                        column: x => x.EmpresaOpenAdmId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RedesSociais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RedeSocialEnum = table.Column<int>(type: "integer", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedesSociais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RedesSociais_Parceiros_ParceiroId",
                        column: x => x.ParceiroId,
                        principalTable: "Parceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TelefonesParceiro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false),
                    Telefone = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelefonesParceiro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelefonesParceiro_Parceiros_ParceiroId",
                        column: x => x.ParceiroId,
                        principalTable: "Parceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parceiros_EmpresaOpenAdmId",
                table: "Parceiros",
                column: "EmpresaOpenAdmId");

            migrationBuilder.CreateIndex(
                name: "IX_RedesSociais_ParceiroId",
                table: "RedesSociais",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_TelefonesParceiro_ParceiroId",
                table: "TelefonesParceiro",
                column: "ParceiroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RedesSociais");

            migrationBuilder.DropTable(
                name: "TelefonesParceiro");

            migrationBuilder.DropTable(
                name: "Parceiros");
        }
    }
}

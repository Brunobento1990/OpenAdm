using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenegociacaoFaturaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Parcelas",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "FaturaHistorico",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FaturaId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaturaHistorico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaturaHistorico_Faturas_FaturaId",
                        column: x => x.FaturaId,
                        principalTable: "Faturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FaturaHistorico_FaturaId",
                table: "FaturaHistorico",
                column: "FaturaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaturaHistorico");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Parcelas");
        }
    }
}

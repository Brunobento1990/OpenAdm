using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class LimpezaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopUsuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TopUsuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalCompra = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalPedidos = table.Column<int>(type: "integer", nullable: false),
                    Usuario = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopUsuarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopUsuarios_Numero",
                table: "TopUsuarios",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_TopUsuarios_ParceiroId",
                table: "TopUsuarios",
                column: "ParceiroId");
        }
    }
}

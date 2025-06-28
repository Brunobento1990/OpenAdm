using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OpenAdm.Infra.Migrations
{
    /// <inheritdoc />
    public partial class BannersAjusteMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_ParceiroId",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_ConfiguracoesDePedidos_ParceiroId",
                table: "ConfiguracoesDePedidos");

            migrationBuilder.DropIndex(
                name: "IX_ConfiguracoesDePagamento_ParceiroId",
                table: "ConfiguracoesDePagamento");

            migrationBuilder.AlterColumn<long>(
                name: "Numero",
                table: "Funcionarios",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Numero",
                table: "ConfiguracoesDePedidos",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Numero",
                table: "ConfiguracoesDePagamento",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Foto = table.Column<string>(type: "text", nullable: false),
                    NomeFoto = table.Column<string>(type: "text", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataDeCriacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    DataDeAtualizacao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    Numero = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParceiroId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_Numero",
                table: "Funcionarios",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDePedidos_Numero",
                table: "ConfiguracoesDePedidos",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDePagamento_Numero",
                table: "ConfiguracoesDePagamento",
                column: "Numero");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_Numero",
                table: "Banners",
                column: "Numero");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_Numero",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_ConfiguracoesDePedidos_Numero",
                table: "ConfiguracoesDePedidos");

            migrationBuilder.DropIndex(
                name: "IX_ConfiguracoesDePagamento_Numero",
                table: "ConfiguracoesDePagamento");

            migrationBuilder.AlterColumn<long>(
                name: "Numero",
                table: "Funcionarios",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Numero",
                table: "ConfiguracoesDePedidos",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Numero",
                table: "ConfiguracoesDePagamento",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_ParceiroId",
                table: "Funcionarios",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDePedidos_ParceiroId",
                table: "ConfiguracoesDePedidos",
                column: "ParceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracoesDePagamento_ParceiroId",
                table: "ConfiguracoesDePagamento",
                column: "ParceiroId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Infra.Migrations.Parceiro
{
    /// <inheritdoc />
    public partial class CobraCpfFreteMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CobrarCnpj",
                table: "ConfiguracoesDeFrete",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CobrarCpf",
                table: "ConfiguracoesDeFrete",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CobrarCnpj",
                table: "ConfiguracoesDeFrete");

            migrationBuilder.DropColumn(
                name: "CobrarCpf",
                table: "ConfiguracoesDeFrete");
        }
    }
}

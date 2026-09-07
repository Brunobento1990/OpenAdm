using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenAdm.Data.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class LojaParceiraAtivoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "LojasParceiras",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_LojasParceiras_ParceiroId_Ativo",
                table: "LojasParceiras",
                columns: new[] { "ParceiroId", "Ativo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LojasParceiras_ParceiroId_Ativo",
                table: "LojasParceiras");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "LojasParceiras");
        }
    }
}
